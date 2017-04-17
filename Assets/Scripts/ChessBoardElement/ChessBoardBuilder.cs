
namespace Assets.Scripts.ChessBoardElement
{
    /// <summary>
    /// Класс сборки доски
    /// </summary>
    public static class ChessBoardBuilder
    {
        /// <summary>
        /// Сосздать доску с начальной расстановкой фигур
        /// </summary>
        /// <param name="startPlayerColor">цвет главного игрока</param>
        /// <returns></returns>
        public static ChessBoard CreateStartChessBoard(PlayerColor startPlayerColor)
        {
            ChessBoard board = new ChessBoard();
            PlayerColor otherPlayerColor = startPlayerColor == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;

            /// start player pieces
            board.PutPiece(new ChessPiece(ChessPieceType.Rook, startPlayerColor), 0, 0);
            board.PutPiece(new ChessPiece(ChessPieceType.Knight, startPlayerColor), 0, 1);
            board.PutPiece(new ChessPiece(ChessPieceType.Bishop, startPlayerColor), 0, 2);
            board.PutPiece(new ChessPiece(ChessPieceType.King, startPlayerColor), 0, 3);
            board.PutPiece(new ChessPiece(ChessPieceType.Queen, startPlayerColor), 0, 4);
            board.PutPiece(new ChessPiece(ChessPieceType.Bishop, startPlayerColor), 0, 5);
            board.PutPiece(new ChessPiece(ChessPieceType.Knight, startPlayerColor), 0, 6);
            board.PutPiece(new ChessPiece(ChessPieceType.Rook, startPlayerColor), 0, 7);
            board.PutPiece(new ChessPiece(ChessPieceType.Pawn, startPlayerColor), 1, 0);
            board.PutPiece(new ChessPiece(ChessPieceType.Pawn, startPlayerColor), 1, 1);
            board.PutPiece(new ChessPiece(ChessPieceType.Pawn, startPlayerColor), 1, 2);
            board.PutPiece(new ChessPiece(ChessPieceType.Pawn, startPlayerColor), 1, 3);
            board.PutPiece(new ChessPiece(ChessPieceType.Pawn, startPlayerColor), 1, 4);
            board.PutPiece(new ChessPiece(ChessPieceType.Pawn, startPlayerColor), 1, 5);
            board.PutPiece(new ChessPiece(ChessPieceType.Pawn, startPlayerColor), 1, 6);
            board.PutPiece(new ChessPiece(ChessPieceType.Pawn, startPlayerColor), 1, 7);

            /// other player pieces
            board.PutPiece(new ChessPiece(ChessPieceType.Rook, otherPlayerColor), 7, 0);
            board.PutPiece(new ChessPiece(ChessPieceType.Knight, otherPlayerColor), 7, 1);
            board.PutPiece(new ChessPiece(ChessPieceType.Bishop, otherPlayerColor), 7, 2);
            board.PutPiece(new ChessPiece(ChessPieceType.King, otherPlayerColor), 7, 3);
            board.PutPiece(new ChessPiece(ChessPieceType.Queen, otherPlayerColor), 7, 4);
            board.PutPiece(new ChessPiece(ChessPieceType.Bishop, otherPlayerColor), 7, 5);
            board.PutPiece(new ChessPiece(ChessPieceType.Knight, otherPlayerColor), 7, 6);
            board.PutPiece(new ChessPiece(ChessPieceType.Rook, otherPlayerColor), 7, 7);
            board.PutPiece(new ChessPiece(ChessPieceType.Pawn, otherPlayerColor), 6, 0);
            board.PutPiece(new ChessPiece(ChessPieceType.Pawn, otherPlayerColor), 6, 1);
            board.PutPiece(new ChessPiece(ChessPieceType.Pawn, otherPlayerColor), 6, 2);
            board.PutPiece(new ChessPiece(ChessPieceType.Pawn, otherPlayerColor), 6, 3);
            board.PutPiece(new ChessPiece(ChessPieceType.Pawn, otherPlayerColor), 6, 4);
            board.PutPiece(new ChessPiece(ChessPieceType.Pawn, otherPlayerColor), 6, 5);
            board.PutPiece(new ChessPiece(ChessPieceType.Pawn, otherPlayerColor), 6, 6);
            board.PutPiece(new ChessPiece(ChessPieceType.Pawn, otherPlayerColor), 6, 7);

            return board;
        }
    }
}
