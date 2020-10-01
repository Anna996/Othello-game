using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ex05_GraphicOthello;

namespace GraphicUI
{
    public partial class OthelloGameForm : Form
    {
        public static bool s_RunGame = true;
        private const char k_White = 'O';
        private const char k_Black = 'X';
        private NewGame m_NewGame;
        private int m_BoardSize;
        private PictureBox[,] m_PictureBoxes;

        public OthelloGameForm(int i_BoardSize, int i_NumOfPlayers)
        {
            InitializeComponent();
            m_BoardSize = i_BoardSize;
            initializeBoard();
            m_NewGame = new NewGame(i_BoardSize, "Yellow", "Red", i_NumOfPlayers);
            m_NewGame.Game.Board.SquareChoosedNotifier += changeDesignForBoardSquares;
            m_NewGame.SetTitleNotifier += setTitle;
            m_NewGame.ShowPossibleMovesNotifier += possibleMoves;
            m_NewGame.EndGameNotifier += endGame;
            m_NewGame.Game.Board.InitializeInitialSquares();
            m_NewGame.startGame();
        }

        public NewGame TheGame
        {
            get
            {
                return this.m_NewGame;
            }
        }

        private void changeDesignForBoardSquares(int i_Row, int i_Col, char i_Color)
        {
            designPictureBox(m_PictureBoxes[i_Row, i_Col], i_Color);
        }

        private void initializeBoard()
        {
            int toAdd = 70, widthAdd = 0;

            makePictureBoxes();
            m_PictureBoxes[0, 0].Location = new Point(31, 17);
            for (int i = 1; i < m_BoardSize; i++)
            {
                m_PictureBoxes[i, 0].Location = new Point(m_PictureBoxes[0, 0].Location.X, m_PictureBoxes[i - 1, 0].Location.Y + toAdd);
                m_PictureBoxes[0, i].Location = new Point(m_PictureBoxes[0, i - 1].Location.X + toAdd, m_PictureBoxes[0, 0].Location.Y);
            }

            for (int i = 1; i < m_BoardSize; i++)
            {
                for (int j = 1; j < m_BoardSize; j++)
                {
                    m_PictureBoxes[i, j].Location = new Point(m_PictureBoxes[i, j - 1].Location.X + toAdd, m_PictureBoxes[i - 1, j].Location.Y + toAdd);
                }
            }

            toAdd *= 2;
            widthAdd = m_BoardSize == 12 ? 30 : 0;

            this.Size = new Size(m_BoardSize * m_PictureBoxes[0, 0].Width + toAdd + widthAdd, m_BoardSize * m_PictureBoxes[0, 0].Height + toAdd);
        }

        private void makePictureBoxes()
        {
            m_PictureBoxes = new PictureBox[m_BoardSize, m_BoardSize];
            for (int i = 0; i < m_BoardSize; i++)
            {
                for (int j = 0; j < m_BoardSize; j++)
                {
                    m_PictureBoxes[i, j] = new PictureBox();
                    m_PictureBoxes[i, j].Size = new Size(60, 60);
                    m_PictureBoxes[i, j].Enabled = false;
                    m_PictureBoxes[i, j].Click += new EventHandler(pictureBoxes_Click);
                    this.Controls.Add(m_PictureBoxes[i, j]);
                }
            }
        }

        private void designPictureBox(PictureBox i_PictureBox, char i_Color)
        {
            i_PictureBox.BackColor = System.Drawing.SystemColors.ControlDark;
            if (i_Color == 'O')
            {
                i_PictureBox.Image = GraphicUI.Properties.Resources.CoinYellow;
            }
            else
            {
                i_PictureBox.Image = GraphicUI.Properties.Resources.CoinRed;
            }
            i_PictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
        }

        private void resetEnablePictureBoxes()
        {
            PictureBox pictureBox;

            for (int i = 0; i < m_NewGame.Game.Board.Size; i++)
            {
                for (int j = 0; j < m_NewGame.Game.Board.Size; j++)
                {
                    pictureBox = m_PictureBoxes[i, j];
                    if (pictureBox != null)
                    {
                        if (m_NewGame.Game.Board.m_Board[i, j] == Board.k_Empty)
                        {
                            pictureBox.Enabled = false;
                            pictureBox.BackColor = System.Drawing.SystemColors.ControlDark;
                        }
                    }
                }
            }
        }

        private void possibleMoves(Player i_PlayerPlayingNow)
        {
            PictureBox pictureBox;

            resetEnablePictureBoxes();
            if (m_NewGame.Game.Board.IsLeftEmptyLegalSquaresForPlayer(i_PlayerPlayingNow))
            {
                for (int i = 0; i < m_NewGame.Game.Board.Size; i++)
                {
                    for (int j = 0; j < m_NewGame.Game.Board.Size; j++)
                    {
                        pictureBox = m_PictureBoxes[i, j];
                        if (pictureBox != null)
                        {
                            if (m_NewGame.Game.Board.IsSquareLegal(i, j, i_PlayerPlayingNow.Color))
                            {
                                pictureBox.Enabled = true;
                                pictureBox.BackColor = Color.Green;
                            }
                        }
                    }
                }
            }
        }

        private void pictureBoxes_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;

            for (int i = 0; i < m_BoardSize; i++)
            {
                for (int j = 0; j < m_BoardSize; j++)
                {
                    if (pictureBox != null)
                    {
                        if (pictureBox == m_PictureBoxes[i, j])
                        {
                            if (m_NewGame.Game.Board.IsEmpty(i, j))
                            {
                                m_NewGame.AfterChoosingLegalSquare(i, j);
                            }

                            break;
                        }
                    }
                }
            }
        }

        private void setTitle(Player i_PlayerPlayingNow)
        {
            this.Text = string.Format("Othello - {0}'s Turn", i_PlayerPlayingNow.Name);
        }

        private void endGame()
        {
            this.Close();
        }

        private void OthelloGameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialog;
            string result = string.Empty;

            if (m_NewGame.Game != null)
            {
                if (m_NewGame.Game.IsTie())
                {
                    result = "It's a Tie !!!";
                }
                else
                {
                    result = string.Format("{0} Won !!!", m_NewGame.Game.Winner().Name);
                }

                dialog = MessageBox.Show(string.Format(
    @"{0} ({1}/{2}) ({3}/{4})
Would you like another round?",
    result,
    m_NewGame.Game.Winner().Score,
    m_NewGame.Game.Loser().Score,
    NewGame.s_NumberOfWinsPlayerWhite,
    NewGame.s_NumberOfWinsPlayerBlack),
    "Othello", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                s_RunGame = dialog == DialogResult.No ? false : true;
            }
        }
    }
}
