namespace Ex05_GraphicOthello
{
    public class Player
    {
        private readonly string r_Name;
        private readonly char r_Color;
        private int m_Score;
        private bool m_IsLeftEmptyLegalSquares;
        private int m_CountOfLeftEmptyLegalSquares;

        public Player(string i_Name, char i_Color)
        {
            r_Name = i_Name;
            m_Score = 2;
            r_Color = i_Color;
            m_IsLeftEmptyLegalSquares = true;
        }

        public string Name
        {
            get
            {
                return r_Name;
            }
        }

        public int Score
        {
            get
            {
                return m_Score;
            }

            set
            {
                m_Score = value;
            }
        }

        public char Color
        {
            get
            {
                return r_Color;
            }
        }

        public bool HasEmptySquares
        {
            get
            {
                return m_IsLeftEmptyLegalSquares;
            }

            set
            {
                m_IsLeftEmptyLegalSquares = value;
            }
        }

        public int CountOfLeftEmptyLegalSquares
        {
            get
            {
                return m_CountOfLeftEmptyLegalSquares;
            }

            set
            {
                m_CountOfLeftEmptyLegalSquares = value;
            }
        }
    }
}
