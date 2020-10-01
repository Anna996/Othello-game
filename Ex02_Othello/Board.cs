namespace Ex05_GraphicOthello
{
    public class Board
    {
        public const char k_WhitePlayer = 'O';
        public const char k_BlackPlayer = 'X';
        public const char k_Empty = ' ';
        private const bool v_LegalMove = true;
        public char[,] m_Board;
        private readonly int r_Size;
        private int m_CountEmptyCells;
        private int m_UpdateCount;

        public event NotifocationDelegate<int, char> SquareChoosedNotifier;

        public Board(int i_Size)
        {
            r_Size = i_Size;
            m_Board = new char[r_Size, r_Size];
            m_CountEmptyCells = (r_Size * r_Size) - 4;
            initializeBoard();
        }

        public char this[int i_Row, int i_Col]
        {
            get
            {
                return m_Board[i_Row, i_Col];
            }

            set
            {
                m_Board[i_Row, i_Col] = value;
            }
        }

        public int Size
        {
            get
            {
                return r_Size;
            }
        }

        public int EmptyCellsCounter
        {
            get
            {
                return m_CountEmptyCells;
            }

            set
            {
                m_CountEmptyCells = value;
            }
        }

        private void initializeBoard()
        {
            for (int i = 0; i < r_Size; i++)
            {
                for (int j = 0; j < r_Size; j++)
                {
                    m_Board[i, j] = k_Empty;
                }
            }
        }

        public void InitializeInitialSquares()
        {
            int row, col;

            row = (r_Size / 2) - 1;
            col = (r_Size / 2) - 1;
            m_Board[row, col] = k_WhitePlayer;
            NotifiySquareChanged(row, col, k_WhitePlayer);
            row = (r_Size / 2) - 1;
            col = r_Size / 2;
            m_Board[row, col] = k_BlackPlayer;
            NotifiySquareChanged(row, col, k_BlackPlayer);
            row = r_Size / 2;
            col = (r_Size / 2) - 1;
            m_Board[row, col] = k_BlackPlayer;
            NotifiySquareChanged(row, col, k_BlackPlayer);
            row = r_Size / 2;
            col = r_Size / 2;
            m_Board[row, col] = k_WhitePlayer;
            NotifiySquareChanged(row, col, k_WhitePlayer);
        }

        public void NotifiySquareChanged(int i_Row, int i_Col, char i_Color)
        {
            if (SquareChoosedNotifier != null)
            {
                SquareChoosedNotifier.Invoke(i_Row, i_Col, i_Color);
            }
        }

        public bool IsLeftEmptyLegalSquaresForPlayer(Player i_Player)
        {
            int countEmptyLegalSquares = 0;

            if (m_CountEmptyCells != 0)
            {
                for (int i = 0; i < r_Size; i++)
                {
                    for (int j = 0; j < r_Size; j++)
                    {
                        if (IsSquareLegal(i, j, i_Player.Color))
                        {
                            countEmptyLegalSquares++;
                        }
                    }
                }
            }

            i_Player.CountOfLeftEmptyLegalSquares = countEmptyLegalSquares;

            return (countEmptyLegalSquares > 0) ? true : false;
        }

        public static bool IsSyntaxLegal(int i_Row, int i_Col, int i_Size)
        {
            bool o_Legal = v_LegalMove;

            if (i_Row < 0 || i_Row > i_Size - 1 || i_Col < 0 || i_Col > i_Size - 1)
            {
                o_Legal = !v_LegalMove;
            }

            return o_Legal;
        }

        public bool IsSquareLegal(int i_Row, int i_Col, char i_Color)
        {
            int num = 0;
            bool o_Legal = !v_LegalMove;

            if (IsEmpty(i_Row, i_Col) &&
                isOponnentColorInNeighbor(i_Row, i_Col, i_Color) &&
               (isValidNorthRow(i_Row, i_Col, i_Color, ref num)
                || isValidSouthRow(i_Row, i_Col, i_Color, ref num)
                || isValidEastCol(i_Row, i_Col, i_Color, ref num)
                || isValidWestCol(i_Row, i_Col, i_Color, ref num)
                || isValidNorthEastDiagonal(i_Row, i_Col, i_Color, ref num)
                || isValidNorthWestDiagonal(i_Row, i_Col, i_Color, ref num)
                || isValidSouthEastDiagonal(i_Row, i_Col, i_Color, ref num)
                || isValidSouthWestDiagonal(i_Row, i_Col, i_Color, ref num)))
            {
                o_Legal = v_LegalMove;
            }

            return o_Legal;
        }

        public void UpdateArrayOfLegalSquares(ref int[] io_ArrayOfLegalSquares, char i_Color)
        {
            int k = 0;

            for (int i = 0; i < r_Size; i++)
            {
                for (int j = 0; j < r_Size; j++)
                {
                    if (IsSquareLegal(i, j, i_Color))
                    {
                        io_ArrayOfLegalSquares[k++] = (i * 10) + (j + 100);
                    }
                }
            }
        }

        public bool IsEmpty(int i_Row, int i_Col)
        {
            return this.m_Board[i_Row, i_Col] == k_Empty;
        }

        /// #################### GAME STATE UPDATE ##################################
        public void UpdateBoard(int i_Row, int i_Col, Player i_CurrentPlayer, Player i_Oponnent)
        {
            int scoreToAdd = 0;

            m_UpdateCount = 0;
            m_CountEmptyCells -= 1;
            if (isValidEastCol(i_Row, i_Col, i_CurrentPlayer.Color, ref scoreToAdd))
            {
                updateEastCol(i_Row, i_Col, i_CurrentPlayer.Color, ref scoreToAdd);
                updateScore(i_CurrentPlayer, i_Oponnent, ref scoreToAdd);
                updateCount();
            }

            if (isValidWestCol(i_Row, i_Col, i_CurrentPlayer.Color, ref scoreToAdd))
            {
                updateWestCol(i_Row, i_Col, i_CurrentPlayer.Color, ref scoreToAdd);
                updateScore(i_CurrentPlayer, i_Oponnent, ref scoreToAdd);
                updateCount();
            }

            if (isValidNorthRow(i_Row, i_Col, i_CurrentPlayer.Color, ref scoreToAdd))
            {
                updateNorthRow(i_Row, i_Col, i_CurrentPlayer.Color, ref scoreToAdd);
                updateScore(i_CurrentPlayer, i_Oponnent, ref scoreToAdd);
                updateCount();
            }

            if (isValidSouthRow(i_Row, i_Col, i_CurrentPlayer.Color, ref scoreToAdd))
            {
                updateSouthRow(i_Row, i_Col, i_CurrentPlayer.Color, ref scoreToAdd);
                updateScore(i_CurrentPlayer, i_Oponnent, ref scoreToAdd);
                updateCount();
            }

            if (isValidNorthEastDiagonal(i_Row, i_Col, i_CurrentPlayer.Color, ref scoreToAdd))
            {
                updateNorthEastDiagonal(i_Row, i_Col, i_CurrentPlayer.Color, ref scoreToAdd);
                updateScore(i_CurrentPlayer, i_Oponnent, ref scoreToAdd);
                updateCount();
            }

            if (isValidNorthWestDiagonal(i_Row, i_Col, i_CurrentPlayer.Color, ref scoreToAdd))
            {
                updateNorthWestDiagonal(i_Row, i_Col, i_CurrentPlayer.Color, ref scoreToAdd);
                updateScore(i_CurrentPlayer, i_Oponnent, ref scoreToAdd);
                updateCount();
            }

            if (isValidSouthEastDiagonal(i_Row, i_Col, i_CurrentPlayer.Color, ref scoreToAdd))
            {
                updateSouthEastDiagonal(i_Row, i_Col, i_CurrentPlayer.Color, ref scoreToAdd);
                updateScore(i_CurrentPlayer, i_Oponnent, ref scoreToAdd);
                updateCount();
            }

            if (isValidSouthWestDiagonal(i_Row, i_Col, i_CurrentPlayer.Color, ref scoreToAdd))
            {
                updateSouthWestDiagonal(i_Row, i_Col, i_CurrentPlayer.Color, ref scoreToAdd);
                updateScore(i_CurrentPlayer, i_Oponnent, ref scoreToAdd);
                updateCount();
            }
        }

        //////////// Methods that help to update the board ///////////////////

        private void updateCount()
        {
            m_UpdateCount++;
        }

        private void updateScore(Player i_CurrentPlayer, Player i_Oponnent, ref int io_ScoreToAdd)
        {
            i_CurrentPlayer.Score += io_ScoreToAdd;
            i_Oponnent.Score -= io_ScoreToAdd;
            if (m_UpdateCount == 0)
            {
                i_Oponnent.Score++;
            }

            io_ScoreToAdd = 0;
        }

        private void updateNorthRow(int i_Row, int i_Col, char i_Color, ref int io_ScoreToAdd)
        {
            int countSquaresToChange = 0;

            if (m_UpdateCount > 0)
            {
                i_Row--;
            }

            for (int i = i_Row; this.m_Board[i, i_Col] != i_Color; i--)
            {
                this.m_Board[i, i_Col] = i_Color;
                NotifiySquareChanged(i, i_Col, i_Color);
                countSquaresToChange++;
            }

            io_ScoreToAdd = countSquaresToChange;
        }

        private void updateSouthRow(int i_Row, int i_Col, char i_Color, ref int io_ScoreToAdd)
        {
            int countSquaresToChange = 0;

            if (m_UpdateCount > 0)
            {
                i_Row++;
            }

            for (int i = i_Row; this.m_Board[i, i_Col] != i_Color; i++)
            {
                this.m_Board[i, i_Col] = i_Color;
                NotifiySquareChanged(i, i_Col, i_Color);
                countSquaresToChange++;
            }

            io_ScoreToAdd = countSquaresToChange;
        }

        private void updateEastCol(int i_Row, int i_Col, char i_Color, ref int io_ScoreToAdd)
        {
            int countSquaresToChange = 0;

            if (m_UpdateCount > 0)
            {
                i_Col++;
            }

            for (int j = i_Col; this.m_Board[i_Row, j] != i_Color; j++)
            {
                this.m_Board[i_Row, j] = i_Color;
                NotifiySquareChanged(i_Row, j, i_Color);
                countSquaresToChange++;
            }

            io_ScoreToAdd = countSquaresToChange;
        }

        private void updateWestCol(int i_Row, int i_Col, char i_Color, ref int io_ScoreToAdd)
        {
            int countSquaresToChange = 0;

            if (m_UpdateCount > 0)
            {
                i_Col--;
            }

            for (int j = i_Col; this.m_Board[i_Row, j] != i_Color; j--)
            {
                this.m_Board[i_Row, j] = i_Color;
                NotifiySquareChanged(i_Row, j, i_Color);
                countSquaresToChange++;
            }

            io_ScoreToAdd = countSquaresToChange;
        }

        private void updateNorthEastDiagonal(int i_Row, int i_Col, char i_Color, ref int io_ScoreToAdd)
        {
            int countSquaresToChange = 0;

            if (m_UpdateCount > 0)
            {
                i_Row--;
                i_Col++;
            }

            for (int i = i_Row, j = i_Col; this.m_Board[i, j] != i_Color; i--, j++)
            {
                this.m_Board[i, j] = i_Color;
                NotifiySquareChanged(i, j, i_Color);
                countSquaresToChange++;
            }

            io_ScoreToAdd = countSquaresToChange;
        }

        private void updateNorthWestDiagonal(int i_Row, int i_Col, char i_Color, ref int io_ScoreToAdd)
        {
            int countSquaresToChange = 0;

            if (m_UpdateCount > 0)
            {
                i_Row--;
                i_Col--;
            }

            for (int i = i_Row, j = i_Col; this.m_Board[i, j] != i_Color; i--, j--)
            {
                this.m_Board[i, j] = i_Color;
                NotifiySquareChanged(i, j, i_Color);
                countSquaresToChange++;
            }

            io_ScoreToAdd = countSquaresToChange;
        }

        private void updateSouthEastDiagonal(int i_Row, int i_Col, char i_Color, ref int io_ScoreToAdd)
        {
            int countSquaresToChange = 0;

            if (m_UpdateCount > 0)
            {
                i_Row++;
                i_Col++;
            }

            for (int i = i_Row, j = i_Col; this.m_Board[i, j] != i_Color; i++, j++)
            {
                this.m_Board[i, j] = i_Color;
                NotifiySquareChanged(i, j, i_Color);
                countSquaresToChange++;
            }

            io_ScoreToAdd = countSquaresToChange;
        }

        private void updateSouthWestDiagonal(int i_Row, int i_Col, char i_Color, ref int io_ScoreToAdd)
        {
            int countSquaresToChange = 0;

            if (m_UpdateCount > 0)
            {
                i_Row++;
                i_Col--;
            }

            for (int i = i_Row, j = i_Col; this.m_Board[i, j] != i_Color; i++, j--)
            {
                this.m_Board[i, j] = i_Color;
                NotifiySquareChanged(i, j, i_Color);
                countSquaresToChange++;
            }

            io_ScoreToAdd = countSquaresToChange;
        }

        /// ##########################LEGAL MOVES#############################//
        /// ## Methods that help to check if a given square is legal ##//
        private bool checkIfIsFlipable(int i_Row, int i_Col, char i_Color, ref bool io_Oponnent, ref bool io_Stop)
        {
            io_Stop = !io_Oponnent && m_Board[i_Row, i_Col] == i_Color || io_Oponnent && IsEmpty(i_Row, i_Col);
            if (this.m_Board[i_Row, i_Col] != i_Color)
            {
                io_Oponnent = v_LegalMove;
            }

            return io_Oponnent && this.m_Board[i_Row, i_Col] == i_Color;
        }

        private void oponnentColor(ref bool io_Oponnent)
        {
            io_Oponnent = true;
        }

        private bool isOponnentColorInNeighbor(int i_Row, int i_Col, char i_Color)
        {
            bool oponnent = false;

            if (Board.IsSyntaxLegal(i_Row, i_Col, this.r_Size))
            {
                if (i_Row == 0 && i_Col == 0)
                {
                    if (isOponnent(i_Row, i_Col + 1, i_Color) || isOponnent(i_Row + 1, i_Col, i_Color)
                        || isOponnent(i_Row + 1, i_Col + 1, i_Color))
                    {
                        oponnentColor(ref oponnent);
                    }
                }
                else if (i_Row == this.r_Size - 1 && i_Col == this.r_Size - 1)
                {
                    if (isOponnent(i_Row, i_Col - 1, i_Color) || isOponnent(i_Row - 1, i_Col, i_Color)
                        || isOponnent(i_Row - 1, i_Col - 1, i_Color))
                    {
                        oponnentColor(ref oponnent);
                    }
                }
                else if (i_Row == 0 && i_Col == this.r_Size - 1)
                {
                    if (isOponnent(i_Row, i_Col - 1, i_Color) || isOponnent(i_Row + 1, i_Col - 1, i_Color)
                        || isOponnent(i_Row + 1, i_Col, i_Color))
                    {
                        oponnentColor(ref oponnent);
                    }
                }
                else if (i_Row == this.r_Size - 1 && i_Col == 0)
                {
                    if (isOponnent(i_Row - 1, i_Col, i_Color) || isOponnent(i_Row - 1, i_Col + 1, i_Color)
                        || isOponnent(i_Row, i_Col + 1, i_Color))
                    {
                        oponnentColor(ref oponnent);
                    }
                }
                else if (i_Row == 0 && i_Col != 0 && i_Col != this.r_Size - 1)
                {
                    if (isOponnent(i_Row, i_Col - 1, i_Color) || isOponnent(i_Row, i_Col + 1, i_Color)
                        || isOponnent(i_Row + 1, i_Col - 1, i_Color) || isOponnent(i_Row + 1, i_Col, i_Color)
                        || isOponnent(i_Row + 1, i_Col + 1, i_Color))
                    {
                        oponnentColor(ref oponnent);
                    }
                }
                else if (i_Row == this.r_Size - 1 && i_Col != 0 && i_Col != this.r_Size - 1)
                {
                    if (isOponnent(i_Row, i_Col - 1, i_Color) || isOponnent(i_Row, i_Col + 1, i_Color)
                        || isOponnent(i_Row - 1, i_Col - 1, i_Color) || isOponnent(i_Row - 1, i_Col, i_Color)
                        || isOponnent(i_Row - 1, i_Col + 1, i_Color))
                    {
                        oponnentColor(ref oponnent);
                    }
                }
                else if (i_Col == 0 && i_Row != 0 && i_Row != this.r_Size - 1)
                {
                    if (isOponnent(i_Row - 1, i_Col, i_Color) || isOponnent(i_Row + 1, i_Col, i_Color)
                        || isOponnent(i_Row - 1, i_Col + 1, i_Color) || isOponnent(i_Row, i_Col + 1, i_Color)
                        || isOponnent(i_Row + 1, i_Col + 1, i_Color))
                    {
                        oponnentColor(ref oponnent);
                    }
                }
                else if (i_Col == this.r_Size - 1 && i_Row != 0 && i_Row != this.r_Size - 1)
                {
                    if (isOponnent(i_Row - 1, i_Col, i_Color) || isOponnent(i_Row + 1, i_Col, i_Color)
                        || isOponnent(i_Row - 1, i_Col - 1, i_Color) || isOponnent(i_Row, i_Col - 1, i_Color)
                        || isOponnent(i_Row + 1, i_Col - 1, i_Color))
                    {
                        oponnentColor(ref oponnent);
                    }
                }
                else if (i_Row != 0 && i_Row != this.r_Size - 1 && i_Col != 0 && i_Col != this.r_Size - 1)
                {
                    if (isOponnent(i_Row - 1, i_Col - 1, i_Color) || isOponnent(i_Row - 1, i_Col, i_Color)
                        || isOponnent(i_Row - 1, i_Col + 1, i_Color) || isOponnent(i_Row, i_Col - 1, i_Color)
                        || isOponnent(i_Row, i_Col + 1, i_Color) || isOponnent(i_Row + 1, i_Col - 1, i_Color)
                        || isOponnent(i_Row + 1, i_Col, i_Color) || isOponnent(i_Row + 1, i_Col + 1, i_Color))
                    {
                        oponnentColor(ref oponnent);
                    }
                }
            }

            return oponnent;
        }

        private bool isOponnent(int i_Row, int i_Col, char i_Color)
        {
            bool oponnent = false;

            if (this.m_Board[i_Row, i_Col] != i_Color && this.m_Board[i_Row, i_Col] != k_Empty)
            {
                oponnentColor(ref oponnent);
            }

            return oponnent;
        }

        private void updataeCurrentPossibleScore(ref int io_PossibleScore)
        {
            io_PossibleScore += 1;
        }

        private void checkIfLegalSquare(
            int i_Row,
            int i_Col,
            char i_Color,
            ref bool io_Oponnent,
            ref bool io_Stop,
            ref int io_PossibleScore,
            ref bool o_Valid)
        {
            if (IsEmpty(i_Row, i_Col))
            {
                io_Stop = true;
                io_PossibleScore = 0;
            }
            else
            {
                if (checkIfIsFlipable(i_Row, i_Col, i_Color, ref io_Oponnent, ref io_Stop))
                {
                    o_Valid = v_LegalMove;
                }

                updataeCurrentPossibleScore(ref io_PossibleScore);
            }
        }

        private bool isValidEastCol(int i_Row, int i_Col, char i_Color, ref int io_PosibleScore)
        {
            bool oponnent = false, o_Valid = !v_LegalMove, stopChecking = false;

            for (int j = i_Col + 1; j < r_Size && !stopChecking; j++)
            {
                checkIfLegalSquare(i_Row, j, i_Color, ref oponnent, ref stopChecking, ref io_PosibleScore, ref o_Valid);
            }

            return o_Valid;
        }

        private bool isValidWestCol(int i_Row, int i_Col, char i_Color, ref int io_PosibleScore)
        {
            bool oponnent = false, o_Valid = !v_LegalMove, stopChecking = false;

            for (int j = i_Col - 1; j >= 0 && !stopChecking; j--)
            {
                checkIfLegalSquare(i_Row, j, i_Color, ref oponnent, ref stopChecking, ref io_PosibleScore, ref o_Valid);
            }

            return o_Valid;
        }

        private bool isValidSouthRow(int i_Row, int i_Col, char i_Color, ref int io_PosibleScore)
        {
            bool oponnent = false, o_Valid = !v_LegalMove, stopChecking = false;

            for (int i = i_Row + 1; i < r_Size && !stopChecking; i++)
            {
                checkIfLegalSquare(i, i_Col, i_Color, ref oponnent, ref stopChecking, ref io_PosibleScore, ref o_Valid);
            }

            return o_Valid;
        }

        private bool isValidNorthRow(int i_Row, int i_Col, char i_Color, ref int io_PosibleScore)
        {
            bool oponnent = false, o_Valid = !v_LegalMove, stopChecking = false;

            for (int i = i_Row - 1; i >= 0 && !stopChecking; i--)
            {
                checkIfLegalSquare(i, i_Col, i_Color, ref oponnent, ref stopChecking, ref io_PosibleScore, ref o_Valid);
            }

            return o_Valid;
        }

        private bool isValidSouthEastDiagonal(int i_Row, int i_Col, char i_Color, ref int io_PosibleScore)
        {
            bool oponnent = false, o_Valid = !v_LegalMove, stopChecking = false;

            for (int i = i_Row + 1, j = i_Col + 1; i < r_Size && j < r_Size && !stopChecking; i++, j++)
            {
                checkIfLegalSquare(i, j, i_Color, ref oponnent, ref stopChecking, ref io_PosibleScore, ref o_Valid);
            }

            return o_Valid;
        }

        private bool isValidSouthWestDiagonal(int i_Row, int i_Col, char i_Color, ref int io_PosibleScore)
        {
            bool oponnent = false, o_Valid = !v_LegalMove, stopChecking = false;

            for (int i = i_Row + 1, j = i_Col - 1; i < r_Size && j >= 0 && !stopChecking; i++, j--)
            {
                checkIfLegalSquare(i, j, i_Color, ref oponnent, ref stopChecking, ref io_PosibleScore, ref o_Valid);
            }

            return o_Valid;
        }

        private bool isValidNorthEastDiagonal(int i_Row, int i_Col, char i_Color, ref int io_PosibleScore)
        {
            bool oponnent = false, o_Valid = !v_LegalMove, stopChecking = false;

            for (int i = i_Row - 1, j = i_Col + 1; i >= 0 && j < r_Size && !stopChecking; i--, j++)
            {
                checkIfLegalSquare(i, j, i_Color, ref oponnent, ref stopChecking, ref io_PosibleScore, ref o_Valid);
            }

            return o_Valid;
        }

        private bool isValidNorthWestDiagonal(int i_Row, int i_Col, char i_Color, ref int io_PosibleScore)
        {
            bool oponnent = false, o_Valid = !v_LegalMove, stopChecking = false;

            for (int i = i_Row - 1, j = i_Col - 1; i >= 0 && j >= 0 && !stopChecking; i--, j--)
            {
                checkIfLegalSquare(i, j, i_Color, ref oponnent, ref stopChecking, ref io_PosibleScore, ref o_Valid);
            }

            return o_Valid;
        }
    }
}
