using System.Collections.Generic;

namespace Assets.Scripts.ChessBoardElement
{
    /// <summary>
    /// Цвет фигуры
    /// </summary>
    public enum PlayerColor
    {
        Black,
        White
    }

    /// <summary>
    /// Тип перемещения фигур
    /// </summary>
    public enum PieceMovementType
    {
        Descrete,
        Continuos
    }

    /// <summary>
    /// Структура для позиции фигуры
    /// </summary>
    public struct BoardCoord: IEqualityComparer<BoardCoord>
    {
        public sbyte row;
        public sbyte column;

        public BoardCoord(sbyte row, sbyte column)
        {
            this.row = row;
            this.column = column;
        }

        public BoardCoord Inverted()
        {
            return new BoardCoord((sbyte)(-row), column);
        }

        public bool Equals(BoardCoord x, BoardCoord y)
        {
            return x.row == y.row && x.column == y.column;
        }

        public int GetHashCode(BoardCoord obj)
        {
            return row ^ column;
        }

        public static BoardCoord operator +(BoardCoord b1, BoardCoord b2)
        {
            sbyte row = (sbyte)(b1.row + b2.row);
            sbyte column = (sbyte)(b1.column + b2.column);
            return new BoardCoord(row, column);
        }
        public static BoardCoord operator -(BoardCoord b1, BoardCoord b2)
        {
            sbyte row = (sbyte)(b1.row - b2.row);
            sbyte column = (sbyte)(b1.column - b2.column);
            return new BoardCoord(row, column);
        }

        public static bool operator ==(BoardCoord b1, BoardCoord b2)
        {
            return b1.Equals(b2);
        }

        public static bool operator !=(BoardCoord b1, BoardCoord b2)
        {
            return !b1.Equals(b2);
        }

        public static int Length(BoardCoord b1, BoardCoord b2)
        {
            return System.Math.Max(System.Math.Abs(b1.row - b2.row), System.Math.Abs(b1.column - b2.column));
        }
    }

    /// <summary>
    /// Класс доски
    /// </summary>
    public class ChessBoard
    {
        /// <summary>
        /// Размер доски
        /// </summary>
        public const int BOARD_SIZE = 8;

        /// <summary>
        /// Хранилище фигур и из позиций
        /// </summary>
        private ChessPiece[,] board = new ChessPiece[BOARD_SIZE, BOARD_SIZE];

        public ChessBoard() {}

        /// <summary>
        /// Проверка позиции на доске
        /// </summary>
        /// <param name="pos">позиция</param>
        /// <returns></returns>
        public bool IsValidSpace(BoardCoord pos)
        {
            return pos.row >= 0 && pos.row < BOARD_SIZE && pos.column >= 0 && pos.column < BOARD_SIZE;
        }

        /// <summary>
        /// Получить фигуру на доске
        /// </summary>
        /// <param name="position">позиция</param>
        /// <param name="piece">фигура</param>
        /// <returns></returns>
        public bool TryGetPiece(BoardCoord position, out ChessPiece piece)
        {
            piece = null;
            if (!IsValidSpace(position))
                return false;
            piece = board[position.row, position.column];
            return piece != null;
        }

        public ChessPiece GetPiece(BoardCoord position)
        {
            if (!IsValidSpace(position))
                return null;
            return board[position.row, position.column];
        }

        /// <summary>
        /// Поставить фигуру на доску lol
        /// </summary>
        /// <param name="piece">фигура</param>
        /// <param name="position">позиция</param>
        public void PutPiece(ChessPiece piece, BoardCoord position)
        {
            if (!IsValidSpace(position))
                return;
            board[position.row, position.column] = piece;
        }

        /// <summary>
        /// Поставить фигуру на стол
        /// </summary>
        /// <param name="piece">фигура</param>
        /// <param name="row">строка</param>
        /// <param name="column">колонка</param>
        public void PutPiece(ChessPiece piece, sbyte row, sbyte column)
        {
            PutPiece(piece, new BoardCoord(row, column));
        }

        public void MovePiece(BoardCoord from, BoardCoord to)
        {
            ChessPiece p = board[from.row, from.column];
            if(p != null)
            {
                PutPiece(p, to);
                RemovePiece(from);
            }
        }

        /// <summary>
        /// Убрать фигуру с доски
        /// </summary>
        /// <param name="position">позиция</param>
        public void RemovePiece(BoardCoord position)
        {
            if (!IsValidSpace(position))
                return;
            board[position.row, position.column] = null;
        }

        /// <summary>
        /// Получить фигуры определнного типа
        /// </summary>
        /// <param name="pieceType">тип фигуры</param>
        /// <param name="color">цвет фигуры</param>
        /// <returns></returns>
        public Dictionary<BoardCoord, ChessPiece> GetPiecesOfType(ChessPieceType pieceType, PlayerColor color)
        {
            Dictionary<BoardCoord, ChessPiece> result = new Dictionary<BoardCoord, ChessPiece>();
            for (int row = 0; row < BOARD_SIZE; row++)
                for (int column = 0; column < BOARD_SIZE; column++)
                {
                    ChessPiece piece = board[row, column];
                    if (piece != null && piece.PieceType == pieceType && piece.Color == color)
                    {
                        result.Add(new BoardCoord((sbyte)row, (sbyte)column), piece);
                    }
                }
            return result;
        }

        /// <summary>
        /// Получить все фигуры одного цвета
        /// </summary>
        /// <param name="color">цвет</param>
        /// <returns></returns>
        public Dictionary<BoardCoord, ChessPiece> GetAllChessPieces(PlayerColor color)
        {
            Dictionary<BoardCoord, ChessPiece> result = new Dictionary<BoardCoord, ChessPiece>();
            for (int row = 0; row < BOARD_SIZE; row++)
                for (int column = 0; column < BOARD_SIZE; column++)
                {
                    ChessPiece piece = board[row, column];
                    if (piece != null && piece.Color == color)
                        result.Add(new BoardCoord((sbyte)row, (sbyte)column), piece);
                }
            return result;
        }

        /// <summary>
        /// Получить все фигуры и их позиции
        /// </summary>
        public Dictionary<BoardCoord, ChessPiece> GetAllChessPieces()
        {
            Dictionary<BoardCoord, ChessPiece> result = new Dictionary<BoardCoord, ChessPiece>();
            for (int row = 0; row < BOARD_SIZE; row++)
                for (int column = 0; column < BOARD_SIZE; column++)
                {
                    ChessPiece piece = board[row, column];
                    if (piece != null)
                        result.Add(new BoardCoord((sbyte)row, (sbyte)column), piece);
                }
            return result;
        }
    }
}
