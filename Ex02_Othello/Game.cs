using System;

namespace Ex05_GraphicOthello
{
    public class Game
    {
        private const char k_Black = 'X';
        private const char k_White = 'O';
        private readonly Player r_PlayerBlack;
        private readonly Player r_PlayerWhite;
        private Board m_Board;
        private int m_NumberOfPlayers;

        public Game(int i_BoardSize, string i_PlayerWhiteName, string i_PlayerBlackName, int i_NumberOfPlayers)
        {
            m_Board = new Board(i_BoardSize);
            r_PlayerWhite = new Player(i_PlayerWhiteName, k_White);
            r_PlayerBlack = new Player(i_PlayerBlackName, k_Black);
            NumberOfPlayers = i_NumberOfPlayers;
        }

        public Board Board
        {
            get
            {
                return m_Board;
            }
        }

        public Player PlayerBlack
        {
            get
            {
                return this.r_PlayerBlack;
            }
        }

        public Player PlayerWhite
        {
            get
            {
                return this.r_PlayerWhite;
            }
        }

        public int NumberOfPlayers
        {
            get
            {
                return m_NumberOfPlayers;
            }

            set
            {
                m_NumberOfPlayers = (value == 2) ? value : 1;
            }
        }

        public bool IsGameOver()
        {
            return !m_Board.IsLeftEmptyLegalSquaresForPlayer(r_PlayerBlack) && !m_Board.IsLeftEmptyLegalSquaresForPlayer(r_PlayerWhite);
        }

        public Player Winner()
        {
            return (PlayerBlack.Score > PlayerWhite.Score) ? PlayerBlack : PlayerWhite;
        }

        public Player Loser()
        {
            return (PlayerBlack.Score > PlayerWhite.Score) ? PlayerWhite : PlayerBlack;
        }

        public bool IsTie()
        {
            return PlayerBlack.Score == PlayerWhite.Score;
        }
    }
}
