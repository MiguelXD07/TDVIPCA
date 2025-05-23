﻿using firstProject;
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

        private int delta = 0;
        private int speed = 2; // NOTE: must be tileSize divider
        private Vector2 directionVector;



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
            Vector2 pos = position.ToVector2() * game.tileSize;
            int frame = 0;
            if (delta > 0)
            {
                pos -= (game.tileSize - delta) * directionVector;
                float animSpeed = 8f;
                frame = (int)((delta / speed) % ((int)animSpeed * sprites[(int)direction].Length) / animSpeed);
            }
            Rectangle rect = new Rectangle(pos.ToPoint(), new Point(game.tileSize));
            sb.Draw(sprites[(int)direction][frame], rect, Color.White); //desenha o Player
        }


        public void Update(GameTime gameTime)
        {
            if (delta > 0)
            {
                delta = (delta + speed) % game.tileSize;
            }
            else
            {
                KeyboardState kState = Keyboard.GetState();
                if
                (kState.IsKeyDown(Keys.A))
                {
                    position.X--;
                    direction = Direction.Left;
                    delta = speed;
                    // directionVector = new Vector2(-1, 0);
                    directionVector = -Vector2.UnitX;
                }

                else if
                (kState.IsKeyDown(Keys.W))
                {
                    position.Y--;
                    direction = Direction.Up;
                    delta = speed;
                    directionVector = -Vector2.UnitY;
                }

                else if
                (kState.IsKeyDown(Keys.S))
                {
                    position.Y++;
                    direction = Direction.Down;
                    delta = speed;
                    directionVector = Vector2.UnitY;
                }

                else if
                (kState.IsKeyDown(Keys.D))
                {
                    position.X++;
                    direction = Direction.Right;
                    delta = speed;
                    directionVector = Vector2.UnitX;
                }

                Point lastPosition = position;

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
                            if (game.boxes[i].X == position.X && game.boxes[i].Y == position.Y)
                                game.boxes[i] = boxTarget;
                    }
                    else
                    {
                        position = lastPosition;
                        delta = 0;
                    }
                }
                else
                {
                    // se não é caixa, se não está livre, parado!
                    if (!game.FreeTile(position.X, position.Y))
                    {
                        delta = 0;
                        position = lastPosition;
                    }
                }
            }
        }
    }
}