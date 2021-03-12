using System;
using System.Collections.Generic;
using System.Linq;


namespace ChessLogic
{
    class ChessAI
    {
        ChessGame game;
        PieceColor colorAI;
        public int compLvl;

        public ChessAI(ChessGame game, PieceColor color, int lvl)
        {
            this.game = game;
            colorAI = color;
            compLvl = lvl;
        }

        private List<int> piecesAI;

        public ChessMove MakeMove()
        {
            int bestMoveScore = -999999;
            int bestIndex = 99;
            int bestIndex2 = 99;
            var computerMove = new ChessMove(bestIndex, bestIndex2);
            piecesAI = game.blackPieces;

            foreach (var index in piecesAI.OrderBy(x => Guid.NewGuid()).ToArray()) 
            {
                foreach (var index2 in game.PossibleMoves(index).OrderBy(x => Guid.NewGuid()).ToArray()) 
                {
                    game.MakeMove(index, index2);
                    if (game.IsDoingCheck(PieceColor.White)) {
                        game.MoveBack();
                        continue;
                    }
                    var tempScore = BestMove(PieceColor.White, bestMoveScore, compLvl);
                    if (tempScore > bestMoveScore) {
                        bestMoveScore = tempScore;
                        bestIndex = index;
                        bestIndex2 = index2;
                    }
                    game.MoveBack();
                }
            }
            if (bestIndex == 99) return computerMove;
            computerMove.first = bestIndex;
            computerMove.last = bestIndex2;
            return computerMove;
        }


        // calculate board score
        private int BoardScore()
        {
            int score = 0;
            foreach (var index in game.blackPieces.ToArray()) 
            {
                switch (game.board1d[index].pieceType)
                {
                    case PieceType.Pawn:
                        score += 100;
                        break;
                    case PieceType.Knight:
                        score += 350;
                        break;
                    case PieceType.Bishop:
                        score += 350;
                        break;
                    case PieceType.Rock:
                    case PieceType.Rock0:
                        score += 525;
                        break;
                    case PieceType.Queen:
                        score += 1000;
                        break;
                    case PieceType.King:
                    case PieceType.King0:
                        score += 10000;
                        break;
                }
            }
            foreach (var index in game.whitePieces.ToArray()) {
            switch(game.board1d[index].pieceType)
            {
                    case PieceType.Pawn:
                        score -= 100;
                        break;
                    case PieceType.Knight:
                        score -= 350;
                        break;
                    case PieceType.Bishop:
                        score -= 350;
                        break;
                    case PieceType.Rock:
                    case PieceType.Rock0:
                        score -= 525;
                        break;
                    case PieceType.Queen:
                        score -= 1000;
                        break;
                    case PieceType.King:
                    case PieceType.King0:
                        score -= 10000;
                        break;
                }
            }
            return score;
        }

        private int BestMove(PieceColor color, int score, int depth)
        {
            var oppositeColor = (color == PieceColor.White) ? PieceColor.Black : PieceColor.White;
            var bestMoveScore = (color == PieceColor.White) ? 99999 : -99999;
            var pieces = (color == PieceColor.White) ? game.whitePieces.ToArray() : game.blackPieces.ToArray();

            foreach (var index in pieces) 
            {
                foreach (var index2 in game.PossibleMoves(index)) 
                {
                    game.MakeMove(index, index2);
                    if (game.IsDoingCheck(oppositeColor)) 
                    {
                        game.MoveBack();
                        continue;
                    }
                    var tempScore = (depth > 1) ? BestMove(oppositeColor, bestMoveScore, depth - 1) : BoardScore();
                    if (color == PieceColor.White) 
                    {
                        if (tempScore <= score) 
                        {
                            game.MoveBack();
                            return score;
                        }
                        if (tempScore < bestMoveScore)
                        {
                            bestMoveScore = tempScore;
                        }
                    } 
                    else
                    {
                        if (tempScore >= score)
                        {
                            game.MoveBack();
                            return score;
                        }
                        if (tempScore > bestMoveScore)
                        {
                            bestMoveScore = tempScore;
                        }
                    }
                    game.MoveBack();
                }
            }
            return bestMoveScore;
        }

    }
}


