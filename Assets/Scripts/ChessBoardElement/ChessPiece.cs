using System;

namespace Assets.Scripts.ChessBoardElement
{
    /// <summary>
    /// Тип фигура
    /// </summary>
    public enum ChessPieceType
    {
        Rook,
        Knight,
        Bishop,
        Queen,
        King,
        Pawn
    }

    /// <summary>
    /// Класс фигуры
    /// </summary>
    public class ChessPiece
    {
        /// <summary>
        /// Тип фигуры
        /// </summary>
        public ChessPieceType PieceType { get; private set; }

        /// <summary>
        /// Цвет фигуры
        /// </summary>
        public PlayerColor Color { get; private set; }

        /// <summary>
        /// Совершен первый ход
        /// </summary>
        public bool HasMoved = false;

        public ChessPiece(ChessPieceType pieceType, PlayerColor color)
        {
            PieceType = pieceType;
            Color = color;
        }
    }
}
