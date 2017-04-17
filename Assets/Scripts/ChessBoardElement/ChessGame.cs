using System;
using System.Linq;
using System.Collections.Generic;

namespace Assets.Scripts.ChessBoardElement
{
    /// <summary>
    /// Класс партии
    /// </summary>
    public class ChessGame
    {
        /// <summary>
        /// Цвет текущего игрока
        /// </summary>
        public PlayerColor CurrentPlayerColor { get; private set; }

        /// <summary>
        /// Правила перемещения фигур
        /// </summary>
        public MovementRules chessRules { get; private set; }

        /// <summary>
        /// Доска
        /// </summary>
        public ChessBoard chessBoard { get; private set; }

        /// <summary>
        ///Шах?
        /// </summary>
        public bool Check { get; private set; }

        /// <summary>
        /// Мат?
        /// </summary>
        public bool Mate { get; private set; }

        /// <summary>
        /// Текущий ход
        /// </summary>
        private Stack<ChessMove> currentMove;

        public ChessGame(PlayerColor mainPlayerColor)
        {
            CurrentPlayerColor = PlayerColor.White;
            chessRules = new MovementRules(mainPlayerColor);
            chessBoard = ChessBoardBuilder.CreateStartChessBoard(chessRules.MainPlayerColor);
            currentMove = new Stack<ChessMove>();
        }

        /// <summary>
        /// Сделать ход
        /// </summary>
        /// <param name="from">начальная позиция</param>
        /// <param name="to">конечная позиция</param>
        /// <returns></returns>
        public Stack<ChessMove> MakeMove(BoardCoord from, BoardCoord to)
        {
            ChessPiece playerPiece, oponentPiece;
            if (!chessBoard.TryGetPiece(from, out playerPiece))
                return null;

            currentMove.Clear();
            if (!CheckForCastling(playerPiece, from, to))
            {
                /// если не рокировка, делаем обычный ход
                ChessMove move = new ChessMove(playerPiece.Color, from, to, playerPiece);
                currentMove.Push(move);
                if (chessBoard.TryGetPiece(to, out oponentPiece))
                    move.DefeatedPiece = oponentPiece;

                CheckForNewQueen(playerPiece, to);
            }

            bool selfCheck = false;
            foreach (ChessMove move in currentMove)
            {
                /// обновляем доску
                chessBoard.RemovePiece(move.To);
                chessBoard.MovePiece(move.From, move.To);

                /// проверка на собственный шах
                if (IsPlayerChecked(move.MovingPiece.Color))
                { selfCheck = true; break; }

                playerPiece.HasMoved = true;

                /// проверка на шах и мат
                bool check;
                Mate = IsMate(move.To, move.MovingPiece.Color, out check);
                Check = check;

                CurrentPlayerColor = CurrentPlayerColor == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;
            }

            /// отменяем ход
            if (selfCheck)
                ReverseMove();

            return currentMove;
        }

        /// <summary>
        /// Отменить ход
        /// </summary>
        private void ReverseMove()
        {
            while(currentMove.Count > 0)
            {
                ChessMove move = currentMove.Pop();
                chessBoard.MovePiece(move.To, move.From);
                chessBoard.PutPiece(move.DefeatedPiece, move.To);
            }
        }

        /// <summary>
        /// Проверка на шах
        /// </summary>
        /// <param name="color">цвет игрока, которому ставят шах</param>
        private bool IsPlayerChecked(PlayerColor color)
        {
            PlayerColor otherPlayerColor = color == PlayerColor.Black ? PlayerColor.White : PlayerColor.Black;
            var king = chessBoard.GetPiecesOfType(ChessPieceType.King, color).FirstOrDefault();
            foreach (var piece in chessBoard.GetAllChessPieces(otherPlayerColor))
            {
                foreach(var pos in chessRules.GetAvailableSpaces(chessBoard, piece.Key))
                {
                    if (pos == king.Key)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Проверка на мат
        /// </summary>
        /// <param name="coord">позиция</param>
        /// <param name="color">цвет игрока</param>
        /// <param name="check">проверка на шах</param>
        /// <returns></returns>
        private bool IsMate(BoardCoord coord, PlayerColor color, out bool check)
        {
            check = false;
            PlayerColor otherPlayerColor = color == PlayerColor.Black ? PlayerColor.White : PlayerColor.Black;

            /// проверка на шах
            List<BoardCoord> spaces = chessRules.GetAvailableSpaces(chessBoard, coord);
            var king = chessBoard.GetPiecesOfType(ChessPieceType.King, otherPlayerColor).FirstOrDefault();
            foreach(var space in spaces)
            {
                if(space == king.Key)
                { check = true; break; }
            }

            /// если не шах, то и продолжать незачем
            if (!check)
                return false;

            /// проверка на мат
            foreach(var piece in chessBoard.GetAllChessPieces(otherPlayerColor))
            {
                foreach(var space in chessRules.GetAvailableSpaces(chessBoard, piece.Key))
                {
                    ChessPiece tmp = chessBoard.GetPiece(space);

                    chessBoard.RemovePiece(space);
                    chessBoard.MovePiece(piece.Key, space);
                    bool oponentChecked = IsPlayerChecked(otherPlayerColor);
                    chessBoard.MovePiece(space, piece.Key);
                    chessBoard.PutPiece(tmp, space);

                    if (!oponentChecked)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Проверка на рокировку
        /// </summary>
        /// <param name="piece">фигура проверки</param>
        /// <param name="from">начальная позиция</param>
        /// <param name="to">конечная позиция</param>
        /// <returns></returns>
        private bool CheckForCastling(ChessPiece piece, BoardCoord from, BoardCoord to)
        {
            if (piece.PieceType != ChessPieceType.King || piece.HasMoved)
                return false;

            int kingMoveLength = from.column - to.column;
            if (from.row != to.row || Math.Abs(kingMoveLength) != 2)
                return false;

            foreach (var rook in chessBoard.GetPiecesOfType(ChessPieceType.Rook, piece.Color))
            {
                if (rook.Value.HasMoved || Math.Abs(rook.Key.column - to.column) > Math.Abs(kingMoveLength))
                    continue;

                int rookColumn = to.column + Math.Sign(kingMoveLength);
                currentMove.Push(new ChessMove(piece.Color, from, to, piece));
                currentMove.Push(new ChessMove(piece.Color, rook.Key, new BoardCoord(rook.Key.row, (sbyte)rookColumn), rook.Value));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Проверка на нового ферзя
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="position"></param>
        private void CheckForNewQueen(ChessPiece piece, BoardCoord position)
        {
            if (piece == null)
                return;

            int lastRow = piece.Color == chessRules.MainPlayerColor ? ChessBoard.BOARD_SIZE - 1 : 0;
            if (piece.PieceType == ChessPieceType.Pawn && position.row == lastRow)
            {
                piece = new ChessPiece(ChessPieceType.Queen, piece.Color);
                chessBoard.PutPiece(piece, position);
            }
        }
    }
}
