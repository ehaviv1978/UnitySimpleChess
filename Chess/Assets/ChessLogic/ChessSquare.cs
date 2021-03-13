
namespace ChessLogic
{
    [System.Serializable]
    class ChessSquare
    {
        public PieceType pieceType = PieceType.Non;
        public PieceColor pieceColor = PieceColor.Non;
        public bool enPassant = false;
    }
}
