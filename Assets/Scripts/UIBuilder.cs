using Assets.Scripts.ChessBoardElement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    /// <summary>
    /// Класс создания UI элементов
    /// </summary>
    public static class UIBuilder
    {
        /// <summary>
        /// Путь к префабу поля
        /// </summary>
        public const string HIGHLIGHT_PREFAB_PATH = "Prefabs/Highlight";

        /// <summary>
        /// Создать поле возможного хода
        /// </summary>
        /// <param name="parent">контейнер</param>
        /// <param name="position">позиция</param>
        /// <param name="size">размер</param>
        /// <param name="coord">позиция на доске</param>
        /// <param name="onclick">действие по нажатию</param>
        /// <returns></returns>
        public static BoardSpaceScript CreateHighlightSpace(Transform parent, Vector3 position, float size, BoardCoord coord, UnityAction<BaseEventData> onclick)
        {
            Object resource = Resources.Load(HIGHLIGHT_PREFAB_PATH);
            if(resource == null)
                throw new System.Exception("Не найден ресурс " + HIGHLIGHT_PREFAB_PATH);
            BoardSpaceScript highlight = (Object.Instantiate(resource) as GameObject).GetComponent<BoardSpaceScript>();
            highlight.transform.position = position;
            highlight.transform.SetParent(parent);
            highlight.SetSize(size);
            highlight.SetColor(Color.yellow);
            highlight.SetCoordinates(coord);
            highlight.gameObject.name = "Highlight";
            return highlight;
        }

        /// <summary>
        /// Создать поле с фигурой
        /// </summary>
        /// <param name="parent">контейнер</param>
        /// <param name="position">позиция</param>
        /// <param name="size">размер</param>
        /// <param name="coord">позиция на доске</param>
        /// <param name="piece">фигура</param>
        /// <param name="onclick">действие по нажатию</param>
        /// <returns></returns>
        public static ChessPieceScript CreateChessPiece(Transform parent, Vector3 position, float size, BoardCoord coord, ChessPiece piece, UnityAction<BaseEventData> onclick)
        {
            if (piece == null)
                throw new System.Exception("CreateChessPiece: параметр piece = null");

            string path = string.Format("Prefabs/{0}{1}", piece.PieceType.ToString(), piece.Color.ToString());
            Object resource = Resources.Load(path);
            if (resource == null)
                throw new System.Exception("Не найден ресурс " + path);

            ChessPieceScript space = (Object.Instantiate(Resources.Load(path)) as GameObject).GetComponent<ChessPieceScript>();
            space.SetCoordinates(coord);
            space.SetSize(size);
            space.transform.position = position;
            space.transform.SetParent(parent);
            space.Piece = piece;
            space.gameObject.name = string.Format("{0}_{1}", piece.PieceType.ToString(), piece.Color.ToString());
            return space;
        }

        /// <summary>
        /// Добавить обработчик нажатия на объект
        /// </summary>
        /// <param name="go">объект</param>
        /// <param name="onclick">действие по нажатию</param>
        public static void AddClickEventTrigger(this GameObject go, UnityAction<BaseEventData> onclick)
        {
            EventTrigger eventTrigger = go.GetComponent<EventTrigger>();
            if (!eventTrigger)
                eventTrigger = go.AddComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry() { eventID = EventTriggerType.PointerClick };
            entry.callback.AddListener(onclick);
            eventTrigger.triggers.Add(entry);
        }
    }
}
