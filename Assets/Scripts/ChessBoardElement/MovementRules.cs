using System.Collections.Generic;

namespace Assets.Scripts.ChessBoardElement
{
    /// <summary>
    /// Клас правил перемещения фигур
    /// </summary>
    public class MovementRules
    {
        public static BoardCoord Left = new BoardCoord(0, -1);
        public static BoardCoord Right = new BoardCoord(0, 1);
        public static BoardCoord Up = new BoardCoord(1, 0);
        public static BoardCoord Down = new BoardCoord(-1, 0);
        public static BoardCoord UpLeft = Up + Left;
        public static BoardCoord UpRight = Up + Right;
        public static BoardCoord DownLeft = Down + Left;
        public static BoardCoord DownRight = Down + Right;

        public MovementRules(PlayerColor mainPlayerColor)
        { MainPlayerColor = mainPlayerColor; }

        public PlayerColor MainPlayerColor { get; private set; }

        /// <summary>
        /// Получить возможные ходы
        /// </summary>
        /// <param name="board">доска</param>
        /// <param name="position">начальная позиция</param>
        /// <returns></returns>
        public List<BoardCoord> GetAvailableSpaces(ChessBoard board, BoardCoord position)
        {
            ChessPiece piece;
            List<BoardCoord> result = new List<BoardCoord>();
            if (board == null || !board.TryGetPiece(position, out piece))
                return result;

            bool invert = MainPlayerColor != piece.Color;
            switch(piece.PieceType)
            {
                case ChessPieceType.Rook:
                    {
                        return GetAvailableSpaces(board, position, PieceMovementType.Continuos, invert,
                            Left, Right, Up, Down);
                    }
                case ChessPieceType.Bishop:
                    {
                        return GetAvailableSpaces(board, position, PieceMovementType.Continuos, invert,
                            UpLeft, UpRight, DownLeft, DownRight);
                    }
                case ChessPieceType.Knight:
                    {
                        return GetAvailableSpaces(board, position, PieceMovementType.Descrete, invert,
                            Up + UpLeft, Up + UpRight, Down + DownLeft, Down + DownRight,
                            Left + UpLeft, Left + DownLeft, Right + UpRight, Right + DownRight);
                    }
                case ChessPieceType.Queen:
                    {
                        return GetAvailableSpaces(board, position, PieceMovementType.Continuos, invert,
                            Left, Right, Up, Down, UpLeft, UpRight, DownLeft, DownRight);
                    }
                case ChessPieceType.King:
                    {
                        return GetKingAvailableSpaces(board, position, piece, invert);
                    }
                case ChessPieceType.Pawn:
                    {
                        return GetPawnAvailableSpaces(board, position, piece, invert);
                    }
            }
            return result;
        }

        /// <summary>
        /// Получить возможные ходы короля
        /// </summary>
        /// <param name="board">доска</param>
        /// <param name="position">начальная позиция</param>
        /// <param name="king">король</param>
        /// <param name="invert">инвертировать ход</param>
        /// <returns></returns>
        private List<BoardCoord> GetKingAvailableSpaces(ChessBoard board, BoardCoord position, ChessPiece king, bool invert)
        {
            /// ходы по-умолчанию
            List<BoardCoord> result = GetAvailableSpaces(board, position, PieceMovementType.Descrete, invert, 
                Up, Down, Left, Right, UpLeft, UpRight, DownLeft, DownRight);

            /// проверка на рокировку
            if (king.HasMoved)
                return result;

            foreach(var rook in board.GetPiecesOfType(ChessPieceType.Rook, king.Color))
            {
                if (rook.Value.HasMoved)
                    continue;

                bool hasOtherPieces = false;
                int kingDir = rook.Key.column - position.column;
                for(int i = 1; i < System.Math.Abs(kingDir); i++)
                {
                    ChessPiece piece;
                    BoardCoord coord = new BoardCoord(rook.Key.row, (sbyte)(rook.Key.column - i * System.Math.Sign(kingDir)));
                    if (board.TryGetPiece(coord, out piece))
                    {
                        hasOtherPieces = true;
                        break;
                    }
                }
                if (!hasOtherPieces)
                    result.Add(new BoardCoord(position.row, (sbyte)(position.column + 2 * System.Math.Sign(kingDir))));
            }

            return result;
        }

        /// <summary>
        /// Получить возможные ходы пешки
        /// </summary>
        /// <param name="board">доска</param>
        /// <param name="position">начальная позиция</param>
        /// <param name="pawn">пешка</param>
        /// <param name="invert">инвертировать ход</param>
        /// <returns></returns>
        private List<BoardCoord> GetPawnAvailableSpaces(ChessBoard board, BoardCoord position, ChessPiece pawn, bool invert)
        {
            List<BoardCoord> result = new List<BoardCoord>();
            if (board == null || pawn.PieceType != ChessPieceType.Pawn)
                return result;

            BoardCoord[] increment = { Up, UpLeft, UpRight };
            if (invert)
            {
                for (int i = 0; i < increment.Length; i++)
                    increment[i] = increment[i].Inverted();
            }

            /// проверка на наличие враждебных фигур
            ChessPiece piece;
            BoardCoord spaceCoord = position + increment[0];
            int moveCount = pawn.HasMoved ? 1 : 2;
            for (int i = 0; i < moveCount; i++)
            {
                if (board.IsValidSpace(spaceCoord) && !board.TryGetPiece(spaceCoord, out piece))
                    result.Add(spaceCoord);
                else break;
                spaceCoord += increment[0];
            }
            for(int i = 1; i < increment.Length; i++)
            {
                spaceCoord = position + increment[i];
                if (board.TryGetPiece(spaceCoord, out piece) && piece.Color != pawn.Color)
                    result.Add(spaceCoord);
            }

            return result;
        }

        /// <summary>
        /// Получить возможные ходы
        /// </summary>
        /// <param name="board">доска</param>
        /// <param name="position">начальная позиция</param>
        /// <param name="moveType">тип перемещения</param>
        /// <param name="invert">инвертировать ход</param>
        /// <param name="increment">направления хода</param>
        /// <returns></returns>
        private List<BoardCoord> GetAvailableSpaces(ChessBoard board, BoardCoord position, PieceMovementType moveType, bool invert, params BoardCoord[] increment)
        {
            ChessPiece piece;
            List<BoardCoord> result = new List<BoardCoord>();
            if (increment == null || increment.Length == 0 || board == null || !board.TryGetPiece(position, out piece))
                return result;

            for(int i = 0; i < increment.Length; i++)
            {
                BoardCoord delta = increment[i];
                if (invert)
                    delta = delta.Inverted();

                ChessPiece pieceAtSpace;
                BoardCoord spaceCoord = position + delta;

                while (board.IsValidSpace(spaceCoord))
                {
                    if (!board.TryGetPiece(spaceCoord, out pieceAtSpace))
                    {
                        result.Add(spaceCoord);
                        spaceCoord += delta;

                        if (moveType == PieceMovementType.Descrete)
                            break;
                    }
                    else
                    {
                        if (pieceAtSpace.Color != piece.Color)
                            result.Add(spaceCoord);
                        break;
                    }
                }
            }
            return result;
        }
    }
}
