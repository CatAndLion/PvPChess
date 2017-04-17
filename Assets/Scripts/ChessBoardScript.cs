using Assets.Scripts;
using Assets.Scripts.ChessBoardElement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Скрипт доски
/// </summary>
[RequireComponent(typeof(RectTransform), typeof(EventTrigger))]
public class ChessBoardScript : MonoBehaviour {

    public BoardSpaceScript Highlight;

    public RectTransform PieceContainer;

    public RectTransform SpaceContainer;

    public Text InfoText;

    [Range(0.1f, 10f)]
    public float MovementSpeed = 4f;

    private RectTransform rectTransform;

    private ChessGame chessGame;

    private ChessPieceScript currentChessPiece;

    private bool CanSelect = true;

    /// <summary>
    /// Текущий размер ячейки
    /// </summary>
    public float CurrentCellSize
    {
        get { return rectTransform.sizeDelta.x / ChessBoard.BOARD_SIZE; }
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Инициализация
    /// </summary>
    public void Initialize(PlayerColor playerColor)
    {
        ClearSpaceContainer();
        ClearPieceContainer();

        chessGame = new ChessGame(playerColor);
        foreach (var piece in chessGame.chessBoard.GetAllChessPieces())
        {
            Vector3 position = BoardCoordToTransformPosition(piece.Key);
            var c = UIBuilder.CreateChessPiece(PieceContainer, position, CurrentCellSize, piece.Key, piece.Value, OnChessPieceClickHandler);
            c.gameObject.AddClickEventTrigger(OnChessPieceClickHandler);
        }

        Highlight.SetSize(CurrentCellSize);
        Highlight.SetVisibility(false);
    }

    /// <summary>
    /// Найти фигуру
    /// </summary>
    /// <param name="piece">фигура</param>
    /// <returns></returns>
    private ChessPieceScript FindChessPiece(ChessPiece piece)
    {
        if (piece == null)
            return null;
        return PieceContainer.GetComponentsInChildren<ChessPieceScript>().FirstOrDefault(x => x.IsVisible && x.Piece == piece);
    }

    /// <summary>
    /// Преобразовать позицию на доске в позицию на экране
    /// </summary>
    /// <param name="coord">позиция на доске</param>
    /// <returns></returns>
    public Vector3 BoardCoordToTransformPosition(BoardCoord coord)
    {
        float x = rectTransform.position.x + coord.column * CurrentCellSize + CurrentCellSize / 2;
        float y = rectTransform.position.y + coord.row * CurrentCellSize + CurrentCellSize / 2;
        return new Vector3(x, y, 0);
    }

    /// <summary>
    /// Обработка нажатия на фигуру
    /// </summary>
    /// <param name="data">параметр</param>
    public void OnChessPieceClickHandler(BaseEventData data)
    {
        if (!CanSelect || chessGame.Mate)
            return;

        PointerEventData pData = (PointerEventData)data;
        currentChessPiece = pData.pointerPress.GetComponent<ChessPieceScript>();

        if (!currentChessPiece || currentChessPiece.Piece.Color != chessGame.CurrentPlayerColor)
            return;

        Highlight.transform.position = currentChessPiece.transform.position;
        Highlight.SetVisibility(true);

        ClearSpaceContainer();
        CreateAvailableSpaces(currentChessPiece.Coordiantes);
    }

    /// <summary>
    /// Обработка нажатия на место перемещения
    /// </summary>
    /// <param name="data">параметр</param>
    public void OnSpaceClickHandler(BaseEventData data)
    {
        if (!currentChessPiece || !CanSelect || chessGame.Mate)
            return;

        PointerEventData pData = (PointerEventData)data;
        BoardSpaceScript space = pData.pointerPress.GetComponent<BoardSpaceScript>();

        Highlight.SetVisibility(false);

        var move = chessGame.MakeMove(currentChessPiece.Coordiantes, space.Coordiantes);
        UpdateChessPieces(move);

        ClearSpaceContainer();

        InfoText.text = string.Empty;
        if (chessGame.Check)
            InfoText.text = "шах!";
        if (chessGame.Mate)
            InfoText.text = "мат!";
    }

    /// <summary>
    /// Обновить фигуры по их перемещению
    /// </summary>
    /// <param name="move">перемещение фигур</param>
    private void UpdateChessPieces(Stack<ChessMove> move)
    {
        if (move == null || move.Count == 0)
            return;

        ChessMove currentMove = move.Pop();
        if (currentMove == null)
            return;

        ChessPieceScript pieceScript = FindChessPiece(currentMove.MovingPiece);
        ChessPieceScript defeatedScript = FindChessPiece(currentMove.DefeatedPiece);

        /// скрыть захваченную фигуру
        if (defeatedScript)
            defeatedScript.SetVisibility(false);

        /// переместить фигуру
        if(pieceScript.Coordiantes != currentMove.To)
        {
            pieceScript.SetCoordinates(currentMove.To);
            Vector3 from = pieceScript.transform.position;
            Vector3 to = BoardCoordToTransformPosition(currentMove.To);
            StartCoroutine(ChessPieceMoveCoroutine(pieceScript, from, to, new UnityAction(() => { UpdateChessPieces(move); })));
        }
    }

    /// <summary>
    /// Корутина перемещения фигуры
    /// </summary>
    /// <param name="piece">фигура</param>
    /// <param name="from">начальная позиция</param>
    /// <param name="to">конечная позиция</param>
    /// <param name="complete">действие по завершению</param>
    /// <returns></returns>
    private IEnumerator ChessPieceMoveCoroutine(ChessPieceScript piece, Vector3 from, Vector3 to, UnityAction complete)
    {
        CanSelect = false;
        piece.transform.SetAsLastSibling();
        float t = 0;
        while (t <= 1)
        {
            piece.transform.position = Vector3.Lerp(from, to, t);
            t += Time.deltaTime * MovementSpeed;
            yield return null;
        }

        if (complete != null)
            complete.Invoke();

        CanSelect = true;
    }

    /// <summary>
    /// Очистить поля возможного хода
    /// </summary>
    public void ClearSpaceContainer()
    {
        foreach (BoardSpaceScript space in SpaceContainer.GetComponentsInChildren<BoardSpaceScript>())
            Destroy(space.gameObject);
    }

    /// <summary>
    /// Очистить поля с фигурами
    /// </summary>
    public void ClearPieceContainer()
    {
        foreach (ChessPieceScript piece in PieceContainer.GetComponentsInChildren<ChessPieceScript>())
            Destroy(piece.gameObject);
    }

    /// <summary>
    /// Создать поля возможного хода
    /// </summary>
    /// <param name="position">позиция</param>
    private void CreateAvailableSpaces(BoardCoord position)
    {
        foreach(var pos in chessGame.chessRules.GetAvailableSpaces(chessGame.chessBoard, position))
        {
            Vector3 rectPos = BoardCoordToTransformPosition(pos);
            var c = UIBuilder.CreateHighlightSpace(SpaceContainer, rectPos, CurrentCellSize, pos, OnSpaceClickHandler);
            c.gameObject.AddClickEventTrigger(OnSpaceClickHandler);
        }
    }
}
