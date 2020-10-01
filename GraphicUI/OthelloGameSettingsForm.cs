using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GraphicUI
{
    public partial class OthelloGameSettingsForm : Form
    {
        private static int s_NumberOfClicksOnButtonBoardSize = 0;
        public static int[] s_BoardSizes = new int[] { 6, 8, 10, 12 };
        private int m_BoardSize = s_BoardSizes[0];
        private int m_NumberOfPlayers;

        public OthelloGameSettingsForm()
        {
            InitializeComponent();
        }

        private void buttonBoardSize_Click(object sender, EventArgs e)
        {
            int sizeBoardIndex = 0;
            string increaseOrDecrease;

            s_NumberOfClicksOnButtonBoardSize++;
            sizeBoardIndex = s_NumberOfClicksOnButtonBoardSize % s_BoardSizes.Length;
            m_BoardSize = s_BoardSizes[sizeBoardIndex];
            increaseOrDecrease = m_BoardSize == 12 ? "decrease" : "increase";
            buttonBoardSize.Text = string.Format(
                "Board Size : {0}x{1} (click to {2})",
                            m_BoardSize,
                            m_BoardSize,
                            increaseOrDecrease);
        }

        private void buttonsPlay_Click(object sender, EventArgs e)
        {
            OthelloGameForm newGame;

            m_NumberOfPlayers = sender == buttonOnePlayer ? 1 : 2;
            while (OthelloGameForm.s_RunGame)
            {
                newGame = new OthelloGameForm(m_BoardSize, m_NumberOfPlayers);
                newGame.ShowDialog();
            }

            this.Close();
        }
    }
}
