using System;
using System.Collections.Generic;
using System.Linq;


namespace ChessLogic
{
    class ChessGame
    {
        public ChessSquare[] board1d = new ChessSquare[64];
        ChessSquare[,] board2d = new ChessSquare[8, 8];

        public List<ChessSquare[]> moveHistory = new List<ChessSquare[]>();
        public int moveHistoryPointer = 0;
        public PieceColor turnColor = PieceColor.White;
        public List<int> whitePieces = new List<int>();
        public List<int> blackPieces = new List<int>();

        public ChessGame()
        {
            NewBoard();
            turnColor = PieceColor.White;
        }

        public void NewBoard()
        {
            for (int i = 0; i < 64; i++)
            {
                board1d[i] = new ChessSquare();
                board1d[i].enPassant = false;
                board1d[i].pieceColor = PieceColor.Non;
                board1d[i].pieceType = PieceType.Non;
            }

            for (int i = 0; i < 16; i++)
            {
                board1d[i].pieceColor = PieceColor.Black;
            }

            for (int i = 48; i < 64; i++)
            {
                board1d[i].pieceColor = PieceColor.White;
            }

            for (int i = 8; i < 16; i++)
            {
                board1d[i].pieceType = PieceType.Pawn;
            }

            for (int i = 48; i < 56; i++)
            {
                board1d[i].pieceType = PieceType.Pawn;
            }

            board1d[0].pieceType = PieceType.Rock0;
            board1d[7].pieceType = PieceType.Rock0;
            board1d[56].pieceType = PieceType.Rock0;
            board1d[63].pieceType = PieceType.Rock0;

            board1d[1].pieceType = PieceType.Knight;
            board1d[6].pieceType = PieceType.Knight;
            board1d[57].pieceType = PieceType.Knight;
            board1d[62].pieceType = PieceType.Knight;

            board1d[2].pieceType = PieceType.Bishop;
            board1d[5].pieceType = PieceType.Bishop;
            board1d[58].pieceType = PieceType.Bishop;
            board1d[61].pieceType = PieceType.Bishop;

            board1d[3].pieceType = PieceType.Queen;
            board1d[59].pieceType = PieceType.Queen;

            board1d[4].pieceType = PieceType.King0;
            board1d[60].pieceType = PieceType.King0;

            moveHistory.Clear();

            AddMoveToHistory();
            Board1dToBoard2d();
            FillPiecesArrays();
            turnColor = PieceColor.White;
        }

        private void AddMoveToHistory()
        {
            var tempBoard = new ChessSquare[64];
            for (int i = 0; i < 64; i++)
            {
                tempBoard[i] = new ChessSquare();
                tempBoard[i].enPassant = board1d[i].enPassant;
                tempBoard[i].pieceColor = board1d[i].pieceColor;
                tempBoard[i].pieceType = board1d[i].pieceType;
            }
            moveHistory.Add(tempBoard);
            moveHistoryPointer = moveHistory.Count - 1;
        }


        private void FillPiecesArrays()
        {
            whitePieces.Clear();
            blackPieces.Clear();
            for (int i = 0; i < 64; i++)
            {
                if (board1d[i].pieceColor == PieceColor.Non)
                {
                    continue;
                }
                else if (board1d[i].pieceColor == PieceColor.White)
                {
                    whitePieces.Add(i);
                }
                else
                {
                    blackPieces.Add(i);
                }
            }
        }

        // fill board2d array with elements
        private void Board1dToBoard2d()
        {
            var n = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board2d[i, j] = board1d[n];
                    n++;
                }
            }
        }

        private void SwitchTurnColor()
        {
            turnColor = (turnColor == PieceColor.White)? PieceColor.Black : PieceColor.White;
        }


        public PieceColor OtherColor(PieceColor color)
        {
            return (color == PieceColor.White)? PieceColor.Black: PieceColor.White;
        }


        private void RemoveEnPassant()
        {
            for (int i = 16; i < 24; i++)
            {
                board1d[i].enPassant = false;
            }
            for (int i = 40; i < 48; i++)
            {
                board1d[i].enPassant = false;
            }
        }


        public void MakeMove(int first, int second)
        {
            SwitchTurnColor();
            if (board1d[first].pieceType == PieceType.Pawn && board1d[second].enPassant)
            {
                if (second < first)
                {
                    board1d[second + 8].pieceColor = PieceColor.Non;
                    board1d[second + 8].pieceType = PieceType.Non;
                }
                else
                {
                    board1d[second - 8].pieceColor = PieceColor.Non;
                    board1d[second - 8].pieceType = PieceType.Non;
                }
            }
            RemoveEnPassant();

            board1d[second].pieceType = board1d[first].pieceType;
            board1d[second].pieceColor = board1d[first].pieceColor;

            board1d[first].pieceType = PieceType.Non;
            board1d[first].pieceColor = PieceColor.Non;

            if (board1d[second].pieceType == PieceType.Rock0)
            {
                board1d[second].pieceType = PieceType.Rock;
            }
            else if (board1d[second].pieceType == PieceType.King0)
            {
                board1d[second].pieceType = PieceType.King;
                //Do Castling
                if (second == first + 2)
                {
                    MakeMove(second + 1, second - 1);
                    SwitchTurnColor();
                    return;
                }
                else if (second == first - 2)
                {
                    MakeMove(second - 2, second + 1);
                    SwitchTurnColor();
                    return;
                }
            }
            else if (board1d[second].pieceType == PieceType.Pawn)
            {
                if (second == first + 16)
                {
                    board1d[first + 8].enPassant = true;
                }
                else if (second == first - 16)
                {
                    board1d[first - 8].enPassant = true;
                }
                else if (second >= 0 && second < 8 || second >= 56 && second < 64) {
                    board1d[second].pieceType = PieceType.Queen;
                }
            }
            if (moveHistory.Count > moveHistoryPointer + 1)
            {
                moveHistory = moveHistory.Take(moveHistoryPointer + 1).ToList();
            }
            AddMoveToHistory();
            FillPiecesArrays();
        }


        public void MoveBack()
        {
            if (moveHistory.Count > 0 && moveHistoryPointer > 0)
            {
                moveHistoryPointer--;
                NewBoardPosition();
            }
        }


        public void MoveForward()
        {
            if (moveHistory.Count > moveHistoryPointer + 1)
            {
                moveHistoryPointer++;
                NewBoardPosition();
            }
        }


        // fill new board position according to curent 'moveHistoryPointer'
        void NewBoardPosition()
        {
            SwitchTurnColor();
            for (int i = 0; i < 64; i++)
            {
                board1d[i].pieceColor = moveHistory[moveHistoryPointer][i].pieceColor;
                board1d[i].pieceType = moveHistory[moveHistoryPointer][i].pieceType;
                board1d[i].enPassant = moveHistory[moveHistoryPointer][i].enPassant;
            }
            Board1dToBoard2d();
            FillPiecesArrays();
        }


        // checks if specific square is under attack
        private bool IsThreatened(int index, PieceColor color)
        {
            var tempPieces = (color == PieceColor.Black) ? blackPieces : whitePieces;

            foreach (var i in tempPieces)
            {
                if (IsValidMove(i, index))
                {
                    return true;
                }
            }
            return false;
        }



        public bool IsDoingCheck(PieceColor color)
        {
            var tempPieces = (color == PieceColor.Black) ? whitePieces : blackPieces;

            foreach (int piece in tempPieces)
            {
                if (board1d[piece].pieceType == PieceType.King || board1d[piece].pieceType == PieceType.King0)
                {
                    return IsThreatened(piece, color);
                }
            }
            return false;
        }



        public bool IsDoingCheckmate(PieceColor color)
        {
            var tempHistory = moveHistory.Take(moveHistory.Count).ToList();
            var pieces = (color == PieceColor.Black) ? whitePieces : blackPieces;

            foreach (int piece in pieces.Take(pieces.Count).ToArray())
            {
                foreach (int move in PossibleMoves(piece))
                {
                    MakeMove(piece, move);
                    if (!IsDoingCheck(color))
                    {
                        MoveBack();
                        moveHistory = tempHistory.Take(tempHistory.Count).ToList();
                        return false;
                    }
                    MoveBack();
                }
            }
            moveHistory = tempHistory.Take(tempHistory.Count).ToList();
            return true;
        }


        public bool IsDraw(PieceColor color)
        {
            return IsDoingCheckmate(OtherColor(color));
        }



        //return a list of legal moves for a given piece
        public List<int> PossibleMoves(int index)
        {
            var color = board1d[index].pieceColor;
            var oppositeColor = (color == PieceColor.White) ? PieceColor.Black : PieceColor.White;
            var rowOld = index / 8;
            var columnOld = index % 8;
            var possibleMoves = new List<int>();

            void DiagonalMoveCheck()
            {
                for (int n = 1; n < 8; n++)
                {
                    if (rowOld + n < 8 && columnOld + n < 8)
                    {
                        if (board2d[rowOld + n, columnOld + n].pieceColor == PieceColor.Non)
                        {
                            possibleMoves.Add(index + n * 9);
                        }
                        else if (board2d[rowOld + n, columnOld + n].pieceColor == oppositeColor)
                        {
                            possibleMoves.Add(index + n * 9);
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                for (int n = 1; n < 8; n++)
                {
                    if (rowOld - n > -1 && columnOld + n < 8)
                    {
                        if (board2d[rowOld - n, columnOld + n].pieceColor == PieceColor.Non)
                        {
                            possibleMoves.Add(index - n * 7);
                        }
                        else if (board2d[rowOld - n, columnOld + n].pieceColor == oppositeColor)
                        {
                            possibleMoves.Add(index - n * 7);
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                for (int n = 1; n < 8; n++)
                {
                    if (rowOld + n < 8 && columnOld - n > -1)
                    {
                        if (board2d[rowOld + n, columnOld - n].pieceColor == PieceColor.Non)
                        {
                            possibleMoves.Add(index + n * 7);
                        }
                        else if (board2d[rowOld + n, columnOld - n].pieceColor == oppositeColor)
                        {
                            possibleMoves.Add(index + n * 7);
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                for (int n = 1; n < 8; n++)
                {
                    if (rowOld - n > -1 && columnOld - n > -1)
                    {
                        if (board2d[rowOld - n, columnOld - n].pieceColor == PieceColor.Non)
                        {
                            possibleMoves.Add(index - n * 9);
                        }
                        else if (board2d[rowOld - n, columnOld - n].pieceColor == oppositeColor)
                        {
                            possibleMoves.Add(index - n * 9);
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            void CrossMoveCheck()
            {
                for (int n = 1; n < 8; n++)
                {
                    if (rowOld + n < 8)
                    {
                        if (board2d[rowOld + n, columnOld].pieceColor == PieceColor.Non)
                        {
                            possibleMoves.Add(index + n * 8);
                        }
                        else if (board2d[rowOld + n, columnOld].pieceColor == oppositeColor)
                        {
                            possibleMoves.Add(index + n * 8);
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                for (int n = 1; n < 8; n++)
                {
                    if (rowOld - n > -1)
                    {
                        if (board2d[rowOld - n, columnOld].pieceColor == PieceColor.Non)
                        {
                            possibleMoves.Add(index - n * 8);
                        }
                        else if (board2d[rowOld - n, columnOld].pieceColor == oppositeColor)
                        {
                            possibleMoves.Add(index - n * 8);
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                for (int n = 1; n < 8; n++)
                {
                    if (columnOld + n < 8)
                    {
                        if (board2d[rowOld, columnOld + n].pieceColor == PieceColor.Non)
                        {
                            possibleMoves.Add(index + n);
                        }
                        else if (board2d[rowOld, columnOld + n].pieceColor == oppositeColor)
                        {
                            possibleMoves.Add(index + n);
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                for (int n = 1; n < 8; n++)
                {
                    if (columnOld - n > -1)
                    {
                        if (board2d[rowOld, columnOld - n].pieceColor == PieceColor.Non)
                        {
                            possibleMoves.Add(index - n);
                        }
                        else if (board2d[rowOld, columnOld - n].pieceColor == oppositeColor)
                        {
                            possibleMoves.Add(index - n);
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            //Check Knight posible moves
            if (board1d[index].pieceType == PieceType.Knight)
            {
                try
                {
                    if (board2d[rowOld - 2,columnOld - 1].pieceColor != color)
                    {
                        possibleMoves.Add(index - 17);
                    }
                }
                catch (IndexOutOfRangeException) { }
                try
                {
                    if (board2d[rowOld - 2,columnOld + 1].pieceColor != color)
                    {
                        possibleMoves.Add(index - 15);
                    }
                }
                catch (IndexOutOfRangeException) { }
                try
                {
                    if (board2d[rowOld - 1, columnOld - 2].pieceColor != color)
                    {
                        possibleMoves.Add(index - 10);
                    }
                }
                catch (IndexOutOfRangeException) { }
                try
                {
                    if (board2d[rowOld - 1, columnOld + 2].pieceColor != color)
                    {
                        possibleMoves.Add(index - 6);
                    }
                }
                catch (IndexOutOfRangeException) { }
                try
                {
                    if (board2d[rowOld + 1, columnOld - 2].pieceColor != color)
                    {
                        possibleMoves.Add(index + 6);
                    }
                }
                catch (IndexOutOfRangeException) { }
                try
                {
                    if (board2d[rowOld + 1, columnOld + 2].pieceColor != color)
                    {
                        possibleMoves.Add(index + 10);
                    }
                }
                catch (IndexOutOfRangeException) { }
                try
                {
                    if (board2d[rowOld + 2, columnOld - 1].pieceColor != color)
                    {
                        possibleMoves.Add(index + 15);
                    }
                }
                catch (IndexOutOfRangeException) { }
                try
                {
                    if (board2d[rowOld + 2, columnOld + 1].pieceColor != color)
                    {
                        possibleMoves.Add(index + 17);
                    }
                }
                catch (IndexOutOfRangeException) { }
            }

            //Check King posible moves
            else if (board1d[index].pieceType == PieceType.King || board1d[index].pieceType == PieceType.King0)
            {
                if (rowOld - 1 > -1 && columnOld - 1 > -1)
                {
                    if (board2d[rowOld - 1, columnOld - 1].pieceColor != color)
                    {
                        possibleMoves.Add(index - 9);
                    }
                }
                if (rowOld - 1 > -1 && columnOld + 1 < 8)
                {
                    if (board2d[rowOld - 1, columnOld + 1].pieceColor != color)
                    {
                        possibleMoves.Add(index - 7);
                    }
                }
                if (rowOld + 1 < 8 && columnOld - 1 > -1)
                {
                    if (board2d[rowOld + 1, columnOld - 1].pieceColor != color)
                    {
                        possibleMoves.Add(index + 7);
                    }
                }
                if (rowOld + 1 < 8 && columnOld + 1 < 8)
                {
                    if (board2d[rowOld + 1, columnOld + 1].pieceColor != color)
                    {
                        possibleMoves.Add(index + 9);
                    }
                }
                if (rowOld - 1 > -1)
                {
                    if (board2d[rowOld - 1, columnOld].pieceColor != color)
                    {
                        possibleMoves.Add(index - 8);
                    }
                }
                if (rowOld + 1 < 8)
                {
                    if (board2d[rowOld + 1, columnOld].pieceColor != color)
                    {
                        possibleMoves.Add(index + 8);
                    }
                }
                if (columnOld - 1 > -1)
                {
                    if (board2d[rowOld, columnOld - 1].pieceColor != color)
                    {
                        possibleMoves.Add(index - 1);
                    }
                }
                if (columnOld + 1 < 8)
                {
                    if (board2d[rowOld, columnOld + 1].pieceColor != color)
                    {
                        possibleMoves.Add(index + 1);
                    }
                }
                // check for castling
                if (board1d[index].pieceType == PieceType.King0)
                {
                    if (board1d[index + 3].pieceType == PieceType.Rock0 &&
                        board1d[index + 2].pieceType == PieceType.Non &&
                        board1d[index + 1].pieceType == PieceType.Non)
                    {
                        if (!IsThreatened(index, oppositeColor) &&
                            !IsThreatened(index + 2, oppositeColor) &&
                            !IsThreatened(index + 1, oppositeColor))
                        {
                            if (board1d[index].pieceColor == PieceColor.White)
                            {
                                if (!(board1d[52].pieceColor == PieceColor.Black && board1d[52].pieceType == PieceType.Pawn ||
                                    board1d[54].pieceColor == PieceColor.Black && board1d[54].pieceType == PieceType.Pawn ||
                                    board1d[55].pieceColor == PieceColor.Black && board1d[55].pieceType == PieceType.Pawn))
                                {
                                    possibleMoves.Add(index + 2);
                                }
                            }
                            else
                            {
                                if (!(board1d[12].pieceColor == PieceColor.White && board1d[12].pieceType == PieceType.Pawn ||
                                    board1d[14].pieceColor == PieceColor.White && board1d[14].pieceType == PieceType.Pawn ||
                                    board1d[15].pieceColor == PieceColor.White && board1d[15].pieceType == PieceType.Pawn))
                                {
                                    possibleMoves.Add(index + 2);
                                }
                            }
                        
                        }
                    }
                    if (board1d[index - 4].pieceType == PieceType.Rock0 &&
                        board1d[index - 3].pieceType == PieceType.Non &&
                        board1d[index - 2].pieceType == PieceType.Non &&
                        board1d[index - 1].pieceType == PieceType.Non)
                    {
                        if (!IsThreatened(index, oppositeColor) &&
                            !IsThreatened(index - 2, oppositeColor) &&
                            !IsThreatened(index - 1, oppositeColor))
                        {
                            if (board1d[index].pieceColor == PieceColor.White)
                            {
                                if (!(board1d[49].pieceColor == PieceColor.Black && board1d[49].pieceType == PieceType.Pawn ||
                                    board1d[50].pieceColor == PieceColor.Black && board1d[50].pieceType == PieceType.Pawn ||
                                    board1d[52].pieceColor == PieceColor.Black && board1d[52].pieceType == PieceType.Pawn))
                                {
                                    possibleMoves.Add(index - 2);
                                }
                            }
                            else
                            {
                                if (!(board1d[9].pieceColor == PieceColor.White && board1d[9].pieceType == PieceType.Pawn ||
                                    board1d[10].pieceColor == PieceColor.White && board1d[10].pieceType == PieceType.Pawn ||
                                    board1d[12].pieceColor == PieceColor.White && board1d[12].pieceType == PieceType.Pawn))
                                {
                                    possibleMoves.Add(index - 2);
                                }
                            }
                        }
                    }
                }
            }

            //Calculate Pawn possible moves
            else if (board1d[index].pieceType == PieceType.Pawn)
            {
                var num = board1d[index].pieceColor == PieceColor.Black? 1: -1;
               
                if (((board1d[index].pieceColor == PieceColor.White && (index / 8) == 6) ||
                    (board1d[index].pieceColor == PieceColor.Black && (index / 8) == 1)) &&
                    board2d[rowOld + num * 2,columnOld].pieceColor == PieceColor.Non &&
                    board2d[rowOld + num,columnOld].pieceColor == PieceColor.Non)
                {
                    possibleMoves.Add(index + num * 16);
                }
                try
                {
                    if (board2d[rowOld + num,columnOld].pieceColor == PieceColor.Non)
                    {
                        possibleMoves.Add(index + num * 8);
                    }
                }
                catch (IndexOutOfRangeException) { }
                try
                {
                    if (board2d[rowOld + num,columnOld + 1].pieceColor == oppositeColor ||
                        (board2d[rowOld + num,columnOld + 1].enPassant))

                    {
                        possibleMoves.Add(index + num * 8 + 1);
                    }
                }
                catch (IndexOutOfRangeException) { }
                try
                {
                    if (board2d[rowOld + num,columnOld - 1].pieceColor == oppositeColor ||
                        (board2d[rowOld + num,columnOld - 1].enPassant))
                    {
                        possibleMoves.Add(index + num * 8 - 1);
                    }
                }
                catch (IndexOutOfRangeException) { }
            }

            //Calculate Rock possible moves
            else if (board1d[index].pieceType == PieceType.Rock || board1d[index].pieceType == PieceType.Rock0)
            {
                CrossMoveCheck();
            }

            //Calculate Bishop Possible moves
            else if (board1d[index].pieceType == PieceType.Bishop)
            {
                DiagonalMoveCheck();
            }

            //Calculate Queen Posible moves
            else if (board1d[index].pieceType == PieceType.Queen)
            {
                CrossMoveCheck();
                DiagonalMoveCheck();          
            }

            return possibleMoves;
        }



        public bool IsValidMove(int first, int second)
        {
            if (board1d[first].pieceColor == board1d[second].pieceColor)
            {
                return false;
            }

            var color = board1d[first].pieceColor;
            var oppositeColor = (color == PieceColor.White) ? PieceColor.Black : PieceColor.White;
            var rowOld = first / 8;
            var columnOld = first % 8;
            var rowNew = second / 8;
            var columnNew = second % 8;
            var rowDiff = rowNew - rowOld;
            var columnDiff = columnNew - columnOld;

            bool CrosslMoveCheck()
            {
                if (rowDiff > 1)
                {
                    for (int i = 1; i < rowDiff; i++)
                    {
                        if (board2d[rowOld + i, columnOld].pieceType != PieceType.Non)
                        {
                            return false;
                        }
                    }
                }
                else if (rowDiff < -1)
                {
                    for (int i = 1; i < Math.Abs(rowDiff); i++)
                    {
                        if (board2d[rowOld - i, columnOld].pieceType != PieceType.Non)
                        {
                            return false;
                        }
                    }
                }
                else if (columnDiff < -1)
                {
                    for (int i = 1; i < Math.Abs(columnDiff); i++)
                    {
                        if (board2d[rowOld, columnOld - i].pieceType != PieceType.Non)
                        {
                            return false;
                        }
                    }
                }
                else if (columnDiff > 1)
                {
                    for (int i = 1; i < columnDiff; i++)
                    {
                        if (board2d[rowOld, columnOld + i].pieceType != PieceType.Non)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            bool DiagonalMoveCheck()
            {
                if (rowDiff > 1 && columnDiff > 1)
                {
                    for (int i = 1; i < rowDiff; i++)
                    {
                        if (board2d[rowOld + i, columnOld + i].pieceType != PieceType.Non)
                        {
                            return false;
                        }
                    }
                }
                else if (rowDiff > 1 && columnDiff < -1)
                {
                    for (int i = 1; i < rowDiff; i++)
                    {
                        if (board2d[rowOld + i, columnOld - i].pieceType != PieceType.Non)
                        {
                            return false;
                        }
                    }
                }
                else if (rowDiff < -1 && columnDiff < -1)
                {
                    for (int i = 1; i < Math.Abs(rowDiff); i++)
                    {
                        if (board2d[rowOld - i, columnOld - i].pieceType != PieceType.Non)
                        {
                            return false;
                        }
                    }
                }
                else if (rowDiff < -1 && columnDiff > 1)
                {
                    for (int i = 1; i < Math.Abs(rowDiff); i++)
                    {
                        if (board2d[rowOld - i, columnOld + i].pieceType != PieceType.Non)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            //Knight valid move
            if (board1d[first].pieceType == PieceType.Knight)
            {
                return (Math.Abs(rowDiff) == 2 && Math.Abs(columnDiff) == 1) ||
                        (Math.Abs(rowDiff) == 1 && Math.Abs(columnDiff) == 2);
            }

            //King valid move
            else if (board1d[first].pieceType == PieceType.King || board1d[first].pieceType == PieceType.King0)
            {
                if ((Math.Abs(rowDiff) == 1 && Math.Abs(columnDiff) == 1) ||
                        (Math.Abs(rowDiff) == 0 && Math.Abs(columnDiff) == 1) ||
                        (Math.Abs(rowDiff) == 1 && Math.Abs(columnDiff) == 0))
                {
                    return true;
                }
                // check for castling
                if (board1d[first].pieceType == PieceType.King0 && Math.Abs(first - second) == 2 && !IsThreatened(first, oppositeColor))
                {
                    if (second > first)
                    {
                        if (board1d[first + 3].pieceType == PieceType.Rock0 &&
                            board1d[first + 1].pieceType == PieceType.Non && !IsThreatened(first + 1, oppositeColor) &&
                            board1d[first + 2].pieceType == PieceType.Non && !IsThreatened(first + 2, oppositeColor))
                        {
                            if (color == PieceColor.White)
                            {
                                return !(board1d[52].pieceColor == PieceColor.Black && board1d[52].pieceType == PieceType.Pawn ||
                                        board1d[54].pieceColor == PieceColor.Black && board1d[54].pieceType == PieceType.Pawn ||
                                        board1d[55].pieceColor == PieceColor.Black && board1d[55].pieceType == PieceType.Pawn);
                            }
                            else
                            {
                                return !(board1d[12].pieceColor == PieceColor.White && board1d[12].pieceType == PieceType.Pawn ||
                                        board1d[14].pieceColor == PieceColor.White && board1d[14].pieceType == PieceType.Pawn ||
                                        board1d[15].pieceColor == PieceColor.White && board1d[15].pieceType == PieceType.Pawn);
                            }
                        }
                        return false;
                    }
                    else
                    {
                        if (board1d[first - 4].pieceType == PieceType.Rock0 &&
                            board1d[first - 1].pieceType == PieceType.Non && !IsThreatened(first - 1, oppositeColor) &&
                            board1d[first - 2].pieceType == PieceType.Non && !IsThreatened(first - 2, oppositeColor) &&
                            board1d[first - 3].pieceType == PieceType.Non)
                        {
                            if (color == PieceColor.White)
                            {
                                return !(board1d[49].pieceColor == PieceColor.Black && board1d[49].pieceType == PieceType.Pawn ||
                                        board1d[50].pieceColor == PieceColor.Black && board1d[50].pieceType == PieceType.Pawn ||
                                        board1d[52].pieceColor == PieceColor.Black && board1d[52].pieceType == PieceType.Pawn);
                            }
                            else
                            {
                                return !(board1d[9].pieceColor == PieceColor.White && board1d[9].pieceType == PieceType.Pawn ||
                                        board1d[10].pieceColor == PieceColor.White && board1d[10].pieceType == PieceType.Pawn ||
                                        board1d[12].pieceColor == PieceColor.White && board1d[12].pieceType == PieceType.Pawn);
                            }
                        }
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            //Pawn valid move
            else if (board1d[first].pieceType == PieceType.Pawn)
            {
                if (color == PieceColor.White)
                {
                    if (first == second + 8 && board1d[second].pieceType == PieceType.Non)
                    {
                        return true;
                    }
                    else if (first == second + 16 && board1d[second].pieceType == PieceType.Non &&
                             board1d[first - 8].pieceType == PieceType.Non && rowOld == 6)
                    {
                        return true;
                    }
                    else if ((first == second + 7 || first == second + 9) && Math.Abs(columnDiff) == 1 &&
                            (board1d[second].pieceColor == oppositeColor || board1d[second].enPassant))
                    {
                        return true;
                    }
                }
                else
                {
                    if (first == second - 8 && board1d[second].pieceType == PieceType.Non)
                    {
                        return true;
                    }
                    else if (first == second - 16 && board1d[second].pieceType == PieceType.Non &&
                            board1d[first + 8].pieceType == PieceType.Non && rowOld == 1)
                    {
                        return true;
                    }
                    else if ((first == second - 7 || first == second - 9) && Math.Abs(columnDiff) == 1 &&
                            (board1d[second].pieceColor == oppositeColor || board1d[second].enPassant))
                    {
                        return true;
                    }
                }
                return false;
            }

            //Rock valid moves
            else if (board1d[first].pieceType == PieceType.Rock || board1d[first].pieceType == PieceType.Rock0)
            {
                if (rowDiff != 0 && columnDiff != 0)
                {
                    return false;
                }
                return CrosslMoveCheck();
            }

            //Bishop valid moves
            else if (board1d[first].pieceType == PieceType.Bishop)
            {
                if (Math.Abs(rowDiff) != Math.Abs(columnDiff))
                {
                    return false;
                }
                return DiagonalMoveCheck();
            }

            //Queen valid moves
            else if (board1d[first].pieceType == PieceType.Queen)
            {
                if (Math.Abs(rowDiff) != Math.Abs(columnDiff) && (rowDiff != 0 && columnDiff != 0))
                {
                    return false;
                }
                if (rowDiff == 0 || columnDiff == 0)
                {
                    return CrosslMoveCheck();
                }
                else
                {
                    return DiagonalMoveCheck();
                }
            }
            
            return false;
        }
    }   
}
