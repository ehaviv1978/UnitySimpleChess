using ChessLogic;
using System.Collections.Generic;


namespace Assets.Classes
{
    class SavedChessGame
    {
        public PieceColor turn;
        public List<ChessSquare[]> moveHistory;

        public SavedChessGame(PieceColor turn, List<ChessSquare[]> moveHistory)
        {
            this.turn = turn;
            this.moveHistory = moveHistory;
        }
    }
}
