using System;

namespace Assets.Scripts.ChessBoardElement
{
    /// <summary>
    /// Класс хода
    /// </summary>
    public class ChessMove
    {
        /// <summary>
        /// Цвет игрока
        /// </summary>
        public PlayerColor player { get; private set; }

        /// <summary>
        /// Перемещаемая фигура
        /// </summary>
        public ChessPiece MovingPiece;

        /// <summary>
        /// Захваченная фигура
        /// </summary>
        public ChessPiece DefeatedPiece;

        /// <summary>
        /// Начальная позиция
        /// </summary>
        public BoardCoord From { get; private set; }

        /// <summary>
        /// Конечная позиция
        /// </summary>
        public BoardCoord To { get; private set; }

        public ChessMove(PlayerColor player, BoardCoord from, BoardCoord to, ChessPiece piece, ChessPiece defeated = null)
        {
            this.player = player;
            From = from;
            To = to;
            MovingPiece = piece;
            DefeatedPiece = defeated;
        }
    }
}
