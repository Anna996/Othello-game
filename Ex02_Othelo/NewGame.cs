using System;

namespace Ex05_GraphicOthello
{
    public class NewGame
    {
        private const char k_Black = 'X';
        private const char k_White = 'O';
        private Game m_Game;
        private Player m_PlayerPlayingNow;
        private Player m_PlayerNotHisTurn;
        public static int s_NumberOfWinsPlayerWhite = 0;
        public static int s_NumberOfWinsPlayerBlack = 0;

        public event NotifocationDelegate<Player> ShowPossibleMovesNotifier;

        public event NotifocationDelegate<Player> SetTitleNotifier;

        public event NotifocationDelegate EndGameNotifier;

        public NewGame(int i_BoardSize,
            string i_PlayerWhiteName,
            string i_PlayerBlackName,
            int i_NumOfPlayers)
        {
            m_Game = new Game(i_BoardSize,
                i_PlayerWhiteName,
                i_PlayerBlackName,
                i_NumOfPlayers);
        }

        public Game Game
        {
            get
            {
                return m_Game;
            }

            set
            {
                m_Game = value;
            }
        }

        public void startGame()
        {
            m_PlayerPlayingNow = m_Game.PlayerWhite;
            m_PlayerNotHisTurn = m_Game.PlayerBlack;
            runGame();
        }

        private void runGame()
        {
            if (!m_Game.IsGameOver())
            {
                if (m_PlayerPlayingNow.HasEmptySquares)
                {
                    if (SetTitleNotifier != null && ShowPossibleMovesNotifier != null)
                    {
                        SetTitleNotifier.Invoke(m_PlayerPlayingNow);
                        ShowPossibleMovesNotifier.Invoke(m_PlayerPlayingNow);
                    }

                    ifComputerPlayingNow();
                }
                else
                {
                    flipPlayersAndRunGame();
                }
            }
            else
            {
                FinishEmptySquares();
                upDateNumberOfWins();
                if (EndGameNotifier != null)
                {
                    EndGameNotifier.Invoke();
                }
            }
        }

        private void ifComputerPlayingNow()
        {
            int rowChoise = 3, colChoise = 'E';

            if (m_Game.NumberOfPlayers == 1 && m_PlayerPlayingNow.Color == k_Black)
            {
                int[] arrayOfLegalSquares = new int[m_PlayerPlayingNow.CountOfLeftEmptyLegalSquares];

                m_Game.Board.UpdateArrayOfLegalSquares(ref arrayOfLegalSquares, k_Black);
                ComputerPlaying(ref colChoise, ref rowChoise, arrayOfLegalSquares);
                AfterChoosingLegalSquare(rowChoise, colChoise);
            }
        }

        public void AfterChoosingLegalSquare(int io_RowChoise, int io_ColChoise)
        {
            m_Game.Board.UpdateBoard(io_RowChoise, io_ColChoise, m_PlayerPlayingNow, m_PlayerNotHisTurn);
            m_PlayerPlayingNow.HasEmptySquares = m_Game.Board.IsLeftEmptyLegalSquaresForPlayer(m_PlayerPlayingNow);
            m_PlayerNotHisTurn.HasEmptySquares = m_Game.Board.IsLeftEmptyLegalSquaresForPlayer(m_PlayerNotHisTurn);
            flipPlayersAndRunGame();
        }

        private void flipPlayersAndRunGame()
        {
            m_PlayerNotHisTurn = m_PlayerPlayingNow;
            m_PlayerPlayingNow = (m_PlayerPlayingNow.Color == k_White) ? m_Game.PlayerBlack : m_Game.PlayerWhite;
            runGame();
        }

        public void ComputerPlaying(ref int io_ColChoise, ref int io_RowChoise, int[] i_ArrayOfLegalSquares)
        {
            int indexChoise, squareChoise;
            Random rndSquare = new Random();

            indexChoise = rndSquare.Next(0, i_ArrayOfLegalSquares.Length - 1);
            squareChoise = i_ArrayOfLegalSquares[indexChoise];
            io_RowChoise = (squareChoise / 10) % 10;
            io_ColChoise = squareChoise % 10;
        }

        private void upDateNumberOfWins()
        {
            Player winningPlayer = m_Game.Winner();

            if (winningPlayer.Color == 'O')
            {
                s_NumberOfWinsPlayerWhite++;
            }
            else
            {
                s_NumberOfWinsPlayerBlack++;
            }
        }

        public void FinishEmptySquares()
        {
            Player winningPlayer;

            if (m_Game.Board.EmptyCellsCounter != 0)
            {
                winningPlayer = m_Game.Winner();
                winningPlayer.Score += m_Game.Board.EmptyCellsCounter;

                for (int i = 0; i < m_Game.Board.Size; i++)
                {
                    for (int j = 0; j < m_Game.Board.Size && m_Game.Board.EmptyCellsCounter > 0; j++)
                    {
                        if (m_Game.Board.IsEmpty(i, j))
                        {
                            m_Game.Board[i, j] = winningPlayer.Color;
                            m_Game.Board.NotifiySquareChanged(i, j, winningPlayer.Color);
                            m_Game.Board.EmptyCellsCounter--;
                        }
                    }
                }
            }
        }
    }
}