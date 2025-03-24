﻿using firstProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;


namespace firstProject;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private int nrLinhas = 0;
    private int nrColunas = 0;
    private SpriteFont font; // Variavel de fonte de texto
    public char[,] level;

    private Texture2D dot, box, wall; //Load images Texture
    public int tileSize = 64;
    private Player firstProject;
    public List<Point> boxes;


    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }


    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        LoadLevel("level1.txt");

        _graphics.PreferredBackBufferHeight = tileSize * level.GetLength(1); //definição da altura
        _graphics.PreferredBackBufferWidth = tileSize * level.GetLength(0); //definição da largura
        _graphics.ApplyChanges(); //aplica a atualização da janela

        firstProject.loadContents();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        font = Content.Load<SpriteFont>("File"); //Use the name of sprite font file ('File')
        dot = Content.Load<Texture2D>("EndPoint_Purple");
        box = Content.Load<Texture2D>("CrateDark_Purple");
        wall = Content.Load<Texture2D>("Wall_Brown");

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (Keyboard.GetState().IsKeyDown(Keys.R)) Initialize();

        if (Victory()) Exit(); // FIXME: Change current level

        firstProject.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        Rectangle position = new Rectangle(0, 0, tileSize, tileSize); //calculo do retangulo a depender do tileSize
        for (int x = 0; x < level.GetLength(0); x++)  //pega a primeira dimensão
        {
            for (int y = 0; y < level.GetLength(1); y++) //pega a segunda dimensão
            {
                position.X = x * tileSize; // define o position
                position.Y = y * tileSize; // define o position

                // Leitura a partir da matriz
                switch (level[x, y])
                {
                    case '.':
                        _spriteBatch.Draw(dot, position, Color.White);
                        break;
                    case 'X':
                        _spriteBatch.Draw(wall, position, Color.White);
                        break;
                }

                // Leitura a partir da Lista boxes
                foreach (Point b in boxes)
                {
                    position.X = b.X * tileSize;
                    position.Y = b.Y * tileSize;
                    _spriteBatch.Draw(box, position, Color.White);
                }

                firstProject.Draw(_spriteBatch);
            }
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    void LoadLevel(string levelFile)
    {
        boxes = new List<Point>();
        string[] linhas = File.ReadAllLines($"Content/{levelFile}");  // "Content/" + level
        nrLinhas = linhas.Length;
        nrColunas = linhas[0].Length;

        level = new char[nrColunas, nrLinhas];

        for (int x = 0; x < nrColunas; x++)
        {
            for (int y = 0; y < nrLinhas; y++)
            {
                if (linhas[y][x] == '#')
                {
                    boxes.Add(new Point(x, y));
                    level[x, y] = ' '; // put a blank instead of the box '#'

                }
                else if (linhas[y][x] == 'Y')
                {
                    firstProject = new Player(this, x, y);
                    level[x, y] = ' '; // put a blank instead of the firstProject 'Y'
                }
                else
                {
                    level[x, y] = linhas[y][x];
                }

            }

        }
    }
    public bool HasBox(int x, int y) // x e y é a posição do Player
    {
        foreach (Point b in boxes)
        {
            if (b.X == x && b.Y == y) return true; // se a caixa tiver a mesma posição do Player
        }
        return false;
    }

    public bool FreeTile(int x, int y)
    {
        if (level[x, y] == 'X') return false;  // se for uma parede está ocupada
        if (HasBox(x, y)) return false; // verifica se é uma caixa
        return true;

        /* The same as:    return level[x,y] != 'X' && !HasBox(x,y);   */
    }

    public bool Victory()
    {
        foreach (Point b in boxes) // pecorrer a lista das caixas
        {
            if (level[b.X, b.Y] != '.') return false; // verifica se há caixas sem pontos
        }
        return true;
    }

}