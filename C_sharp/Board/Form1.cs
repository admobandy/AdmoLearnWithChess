using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Board
{
    

    public partial class boardWindow : Form
    {
        boardController boardHandle = new boardController();        
        bool firstClick = new bool();
        int firstClickSquare = new int();
        bool secondClick = new bool();
        int secondClickSquare = new int();
        bool whitesTurn = new bool();
        bool whiteInCheck = new bool();
        bool blackInCheck = new bool();

        public boardWindow()
        {
            InitializeComponent();            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            whitesTurn = true;
            firstClick = false;
            secondClick = false;
            whiteInCheck = false;
            blackInCheck = false;
            firstClickSquare = 0;
            secondClickSquare = 0;            
            drawBoardState();            
        }

        private void hidePieces()
        {
            BQR.Hide();
            BQN.Hide();
            BQB.Hide();
            BQQ.Hide();
            BKK.Hide();
            BKB.Hide();
            BKN.Hide();
            BKR.Hide();
            BQP1.Hide();
            BQP2.Hide();
            BQP3.Hide();
            BQP4.Hide();
            BKP1.Hide();
            BKP2.Hide();
            BKP3.Hide();
            BKP4.Hide();
            WQR.Hide();
            WQN.Hide();
            WQB.Hide();
            WQQ.Hide();
            WKK.Hide();
            WKB.Hide();
            WKN.Hide();
            WKR.Hide();
            WQP1.Hide();
            WQP2.Hide();
            WQP3.Hide();
            WQP4.Hide();
            WKP1.Hide();
            WKP2.Hide();
            WKP3.Hide();
            WKP4.Hide();
        }


        private void drawBoardState()
        {            
            int i = new int();
            i = 0;
            while (i < 64)
            {
                string pieceName = boardHandle.getElement(i);
                int locationTranslationX = new int();
                int locationTranslationY = new int();
                int row = new int();
                int column = new int();
                row = (i / 8) % 8;
                column = (i % 8);
                locationTranslationX = (int)((column * 70.5) + 24);
                locationTranslationY = (int)((row * 64.25) + 24); 

                if(pieceName == "BQR")
                {
                    BQR.Show();
                    BQR.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
                if (pieceName == "BQN")
                {
                    BQN.Show();
                    BQN.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
                if (pieceName == "BQB")
                {
                    BQB.Show();
                    BQB.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
                if (pieceName == "BQQ")
                {
                    BQQ.Show();
                    BQQ.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
                if (pieceName == "BKK")
                {
                    BKK.Show();
                    BKK.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
                if (pieceName == "BKB")
                {
                    BKB.Show();
                    BKB.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
                if (pieceName == "BKN")
                {
                    BKN.Show();
                    BKN.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
                if (pieceName == "BKR")
                {
                    BKR.Show();
                    BKR.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
                if (pieceName == "BQP1")
                {
                    BQP1.Show();
                    BQP1.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;    
                }
                if (pieceName == "BKP1")
                {
                    BKP1.Show();
                    BKP1.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }                              
                if (pieceName == "WQP1")
                {
                    WQP1.Show();
                    WQP1.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;           
                }                
                if (pieceName == "WKP1")
                {
                    WKP1.Show();
                    WKP1.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;    
                }
                if (pieceName == "BQP2")
                {
                    BQP2.Show();
                    BQP2.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
                if (pieceName == "BKP2")
                {
                    BKP2.Show();
                    BKP2.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
                if (pieceName == "WQP2")
                {
                    WQP2.Show();
                    WQP2.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
                if (pieceName == "WKP2")
                {
                    WKP2.Show();
                    WKP2.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
                if (pieceName == "BQP3")
                {
                    BQP3.Show();
                    BQP3.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
                if (pieceName == "BKP3")
                {
                    BKP3.Show();
                    BKP3.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
                if (pieceName == "WQP3")
                {
                    WQP3.Show();
                    WQP3.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
                if (pieceName == "WKP3")
                {
                    WKP3.Show();
                    WKP3.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
                if (pieceName == "BQP4")
                {
                    BQP4.Show();
                    BQP4.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
                if (pieceName == "BKP4")
                {
                    BKP4.Show();
                    BKP4.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
                if (pieceName == "WQP4")
                {
                    WQP4.Show();
                    WQP4.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
                if (pieceName == "WKP4")
                {
                    WKP4.Show();
                    WKP4.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
                if (pieceName == "WQR")
                {
                    WQR.Show();
                    WQR.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
                if (pieceName == "WQN")
                {
                    WQN.Show();
                    WQN.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
                if (pieceName == "WQB")
                {
                    WQB.Show();
                    WQB.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
                if (pieceName == "WQQ")
                {
                    WQQ.Show();
                    WQQ.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
                if (pieceName == "WKK")
                {
                    WKK.Show();
                    WKK.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
                if (pieceName == "WKB")
                {
                    WKB.Show();
                    WKB.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
                if (pieceName == "WKN")
                {
                    WKN.Show();
                    WKN.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
                if (pieceName == "WKR")
                {
                    WKR.Show();
                    WKR.SetBounds(locationTranslationX, locationTranslationY, 64, 64);
                    goto endOfCases;
                }
            endOfCases:
                i = i + 1;
            }
        }

        private void board_Click(object sender, EventArgs e)
        {
            handleClick();           
        }

        private int getMouseX()
        {
            return (int)((boardWindow.MousePosition.X - boardWindow.ActiveForm.Left - 28) / 70.5);
        }

        private int getMouseY()
        {
            return (int)((boardWindow.MousePosition.Y - boardWindow.ActiveForm.Top - 54) / 64.25);
        }

        private int translateMouseCoords()
        {
            return getMouseX() + (getMouseY()*8);
        }

        private void handleClick()
        {
            if (firstClick == false)
            {
                firstClick = true;
                firstClickSquare = translateMouseCoords();
                return;
            }

            if (secondClick == false)
            {
                secondClick = true;
                secondClickSquare = translateMouseCoords();
                pumpGameEvent();
                return;
            } 
        }

        private void pumpGameEvent()
        {
            firstClick = false;
            secondClick = false;
            string destinationPiece = boardHandle.getElement(secondClickSquare);
            string movingPiece = boardHandle.getElement(firstClickSquare);
            string moveColor = movingPiece.Remove(1);
            
            if (whitesTurn == true && moveColor == "B")
            {
                statusLabel.Text = "It's white's turn!";
                return;
            }

            if (whitesTurn == false && moveColor == "W")
            {
                statusLabel.Text = "It's black's turn!";
                return;
            }

            statusLabel.Text = firstClickSquare + "," + secondClickSquare;

            int moveStatus = boardHandle.tryMove(firstClickSquare, secondClickSquare);
            int checkStatus = boardHandle.inCheck();
              
            if (checkStatus == 0)
            {
                whiteInCheck = false;
                blackInCheck = false;
            }

            if(moveStatus == 0)
            {
                statusLabel.Text = "Move not valid, try again";
                return;
            }

            if(moveStatus == -1)
            {
                statusLabel.Text = "A pawn has crossed the board!";
                //figure out a way to clone pieces without creating new method
                //in boardController.cs
            }

            if(checkStatus == -1)
            {
                statusLabel.Text = "White is in check!";
                whiteInCheck = true;
            }

            if(checkStatus == 1)
            {
                statusLabel.Text = "Black is in check!";
                blackInCheck = true;
            }

            if (moveColor == "B" && blackInCheck == true)
            {
                statusLabel.Text = "Move puts or leaves Black in check, try again";
                boardHandle.tryMove(secondClickSquare, firstClickSquare);
                boardHandle.setElement(secondClickSquare, movingPiece);
                return;
            }

            if (moveColor == "W" && whiteInCheck == true)
            {
                statusLabel.Text = "Move puts or leaves White in check, try again";
                boardHandle.tryMove(secondClickSquare, firstClickSquare);
                boardHandle.setElement(secondClickSquare, movingPiece);
                return;
            }

            if (moveStatus == 1)
            {
                statusLabel.Text = "Valid move! " + statusLabel.Text;
                if (whitesTurn == true)
                {
                    whitesTurn = false;
                }
                else
                {
                    whitesTurn = true;
                }
                hidePieces();
                drawBoardState();
                return;
            }
            return;
        }

        private void BQR_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void BQN_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void BQB_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void BQQ_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void BKK_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void BKB_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void BKN_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void BKR_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void BQP1_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void BQP2_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void BQP3_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void BQP4_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void BKP1_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void BKP2_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void BKP3_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void BKP4_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void WQR_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void WQN_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void WQB_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void WQQ_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void WKK_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void WKB_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void WKN_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void WKR_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void WQP1_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void WQP2_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void WQP3_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void WQP4_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void WKP1_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void WKP2_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void WKP3_Click(object sender, EventArgs e)
        {
            handleClick();
        }

        private void WKP4_Click(object sender, EventArgs e)
        {
            handleClick();
        }

    }
}
