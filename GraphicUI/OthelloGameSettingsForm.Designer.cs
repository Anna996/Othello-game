namespace GraphicUI
{
    public partial class OthelloGameSettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonTwoPlayers = new System.Windows.Forms.Button();
            this.buttonOnePlayer = new System.Windows.Forms.Button();
            this.buttonBoardSize = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonTwoPlayers
            // 
            this.buttonTwoPlayers.Location = new System.Drawing.Point(214, 77);
            this.buttonTwoPlayers.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonTwoPlayers.Name = "buttonTwoPlayers";
            this.buttonTwoPlayers.Size = new System.Drawing.Size(165, 60);
            this.buttonTwoPlayers.TabIndex = 7;
            this.buttonTwoPlayers.Text = "Play against your friend";
            this.buttonTwoPlayers.UseVisualStyleBackColor = true;
            this.buttonTwoPlayers.Click += new System.EventHandler(this.buttonsPlay_Click);
            // 
            // buttonOnePlayer
            // 
            this.buttonOnePlayer.Location = new System.Drawing.Point(25, 77);
            this.buttonOnePlayer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonOnePlayer.Name = "buttonOnePlayer";
            this.buttonOnePlayer.Size = new System.Drawing.Size(165, 60);
            this.buttonOnePlayer.TabIndex = 6;
            this.buttonOnePlayer.Text = "Play against the computer";
            this.buttonOnePlayer.UseVisualStyleBackColor = true;
            this.buttonOnePlayer.Click += new System.EventHandler(this.buttonsPlay_Click);
            // 
            // buttonBoardSize
            // 
            this.buttonBoardSize.BackColor = System.Drawing.SystemColors.Control;
            this.buttonBoardSize.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonBoardSize.Location = new System.Drawing.Point(25, 22);
            this.buttonBoardSize.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonBoardSize.Name = "buttonBoardSize";
            this.buttonBoardSize.Size = new System.Drawing.Size(355, 34);
            this.buttonBoardSize.TabIndex = 5;
            this.buttonBoardSize.Text = "Board Size : 6x6 (click to increase)";
            this.buttonBoardSize.UseVisualStyleBackColor = false;
            this.buttonBoardSize.Click += new System.EventHandler(this.buttonBoardSize_Click);
            // 
            // OthelloGameSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 149);
            this.Controls.Add(this.buttonTwoPlayers);
            this.Controls.Add(this.buttonOnePlayer);
            this.Controls.Add(this.buttonBoardSize);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OthelloGameSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Othello - Game Settings";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonTwoPlayers;
        private System.Windows.Forms.Button buttonOnePlayer;
        private System.Windows.Forms.Button buttonBoardSize;
    }
}