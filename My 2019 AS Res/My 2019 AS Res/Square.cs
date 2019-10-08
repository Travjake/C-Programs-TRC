using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace My_2019_AS_Res
{
    public class Board
    {
        public int row, col, BSW, BSH;
        public Square[,] grid = new Square[1000, 1000];

        public void Init(int rows, int cols, int BlackSquareWidth, int BlackSquareHeight)
        {
            Color tempcolour = Color.White;
            grid = new Square[rows, cols];
            BSH = BlackSquareHeight;
            BSW = BlackSquareWidth;
           

            for (int ycor = 0; ycor < cols; ycor++)
            {
                for (int xcor = 0; xcor < rows; xcor++)
                {
                    Square temp = new Square();
                    temp.Init(xcor, ycor, 15, tempcolour, BSW, BSH);
                    if (tempcolour == Color.White)
                        tempcolour = Color.Black;
                    else tempcolour = Color.White;
                    grid[xcor, ycor] = temp;
                    
                }
                if (tempcolour == Color.White)
                    tempcolour = Color.Black;
                else tempcolour = Color.White;
            }
        }
        public void Draw(SpriteBatch Space, int BLW, int BLH)
        {
            foreach (Square SQ in grid)
            {
                if (SQ != null)
                    SQ.Draw(Space, BLW, BLH);
               
            }
        }
    }


    public class Square
    {
        public bool active = false;
        public bool PossibleMoveW = false;
        public bool PossibleMoveB = false;
        public int X, Y, Offset;
        public Color SquareColour;
        public Texture2D counter;
        public double IncScale, IncScale2;

        public void Init(int x, int y, int o, Color squarecolour, int BLW, int BLH)
        {
            X = x;
            Y = y;
            Offset = o;
            SquareColour = squarecolour;
            

        }

        public void Draw(SpriteBatch S, int BLW, int BLH)
        {
            IncScale = (((double)(BLW + BLH) / 2) / 100 * 30);
            IncScale2 = (((double)(BLW + BLH) / 2) - IncScale) / 80; 
            

            if (counter != null)
                S.Draw(counter, new Vector2((float)(X * BLW + ((BLW - 70 * IncScale2)/ 2)), (float)(Y * BLH + ((BLH - 70 * IncScale2) / 2))), null,Color.White, 0f, Vector2.Zero, (float)IncScale2, SpriteEffects.None, 0f);
                
        }
    }
}
