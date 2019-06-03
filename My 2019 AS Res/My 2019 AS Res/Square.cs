using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace My_2019_AS_Res
{
    public class Board
    {
        public Square[,] grid = new Square[8, 8];

        public void Init()
        {
            Color tempcolour = Color.White;
            for (int ycor = 0; ycor < 8; ycor++)
            {
                for (int xcor = 0; xcor < 8; xcor++)
                {
                    Square temp = new Square();
                    temp.Init(xcor, ycor, 15, tempcolour);
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
        public void Draw(SpriteBatch Space)
        {
            foreach (Square SQ in grid)
            {
                SQ.Draw(Space);
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

        public void Init(int x, int y, int o, Color squarecolour)
        {
            X = x;
            Y = y;
            Offset = o;
            SquareColour = squarecolour;


        }

        public void Draw(SpriteBatch S)
        {
            if (counter != null)
                S.Draw(counter, new Vector2(X * 100 + Offset, Y * 100 + Offset), Color.White);
        }
    }
}
