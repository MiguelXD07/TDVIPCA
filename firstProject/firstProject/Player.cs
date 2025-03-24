using firstProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static firstProject.Game1;

namespace firstProject
{
    class Player
    {
        // Current player position in the matrix (multiply by tileSize prior to drawing)

        private Point position; //Point = Vector2, mas são inteiros
        public Point Position => position; //auto função (equivalente a ter só get sem put) - AUTOPROPERTY

        private Game1 game; //reference from Game1 to Player


        private bool keysReleased = true;
        public int tileSize = 64; //potencias de 2 (operações binárias)

        private Texture2D dot, box, wall; //Load images Texture 
        private Texture2D[][] sprites;
        private Direction direction = Direction.Down;





        enum Direction
        {
            Up, Down, Left, Right // 0, 1, 2, 3
        }

        public Player(Game1 game1, int x, int y) //constructor que dada a as posições guarda a sua posição
        {
            position = new Point(x, y);
            game = game1;
        }

        public void loadContents()
        {
            sprites = new Texture2D[4][];

            sprites[(int)Direction.Up] = new[] 
            {
            game.Content.Load<Texture2D>("Character7"),
            game.Content.Load<Texture2D>("Character8"),
            game.Content.Load<Texture2D>("Character9") 
            };


            sprites[(int)Direction.Down] = new[] 
            {
            game.Content.Load<Texture2D>("Character4"),
            game.Content.Load<Texture2D>("Character5"),
            game.Content.Load<Texture2D>("Character6") 
            };


            sprites[(int)Direction.Left] = new[] 
            {
            game.Content.Load<Texture2D>("Character1"),
            game.Content.Load<Texture2D>("Character10") 
            };


            sprites[(int)Direction.Right] = new[] 
            {
            game.Content.Load<Texture2D>("Character2"),
            game.Content.Load<Texture2D>("Character3") 
            };

        }


        public void Draw(SpriteBatch sb)
        {
            Rectangle rect = new Rectangle(game.tileSize * position.X,
                                           game.tileSize * position.Y,
                                           game.tileSize, game.tileSize);
            sb.Draw(sprites[(int)direction][0], rect, Color.White); //desenha o Player
        }



        public void Update(GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState();
            if (keysReleased)
            {
                Point lastPosition = position;
                keysReleased = false;
                if ((kState.IsKeyDown(Keys.A)) || (kState.IsKeyDown(Keys.Left)))
                {
                    position.X--;
                    //game.direction = Direction.Left;
                    direction = Direction.Left;
                }
                else if ((kState.IsKeyDown(Keys.W)) || (kState.IsKeyDown(Keys.Up)))
                {
                    position.Y--;
                    //game.direction = Direction.Up;
                    direction = Direction.Up;
                }
                else if ((kState.IsKeyDown(Keys.S)) || (kState.IsKeyDown(Keys.Down)))
                {
                    position.Y++;
                    //game.direction = Direction.Down;
                    direction = Direction.Down;
                }
                else if ((kState.IsKeyDown(Keys.D)) || (kState.IsKeyDown(Keys.Right)))
                {
                    position.X++;
                    //game.direction = Direction.Right;
                    direction = Direction.Right;
                }

                else keysReleased = true;

                // destino é caixa?
                if (game.HasBox(position.X, position.Y))
                {
                    int deltaX = position.X - lastPosition.X;
                    int deltaY = position.Y - lastPosition.Y;
                    Point boxTarget = new Point(deltaX + position.X, deltaY + position.Y);
                    // se sim, caixa pode mover-se?
                    if (game.FreeTile(boxTarget.X, boxTarget.Y))
                    {
                        for (int i = 0; i < game.boxes.Count; i++)
                        {
                            if (game.boxes[i].X == position.X && game.boxes[i].Y == position.Y)
                            {
                                game.boxes[i] = boxTarget;
                            }
                        }
                    }
                    else
                    {
                        position = lastPosition;
                    }
                }
                else
                {
                    // se não é caixa, se não está livre, parado!
                    if (!game.FreeTile(position.X, position.Y))
                        position = lastPosition;
                }
            }


            else
            {
                if (kState.IsKeyUp(Keys.A) && kState.IsKeyUp(Keys.W) &&
                    kState.IsKeyUp(Keys.S) && kState.IsKeyUp(Keys.D) && 
                    kState.IsKeyUp(Keys.Left) && kState.IsKeyUp(Keys.Up) &&
                    kState.IsKeyUp(Keys.Down) && kState.IsKeyUp(Keys.Right))
                {
                    keysReleased = true;
                }
            
            }
        }


    }


}