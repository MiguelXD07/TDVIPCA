using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace firstProject;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont font; //Variavel de fonte de texto
    private int nrLinhas = 0;
    private int nrColunas = 0;
    private Texture2D player, dot, box, wall; //Load images Texture
    private Player firstProject;
    int tileSize = 64; //potencias de 2 (operações binárias)

    //Privare char[,] level
    private char[,] level;
    public List<Point> boxes;


    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        LoadLevel("level1.txt");

        _graphics.PreferredBackBufferHeight = tileSize * level.GetLength(1); //definição da altura
        _graphics.PreferredBackBufferWidth = tileSize * level.GetLength(0); //definição da largura
        _graphics.ApplyChanges(); //aplica a atualização da janela


        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        font = Content.Load<SpriteFont>("File"); //Usa o nome do Ficheiro do sprite

        player = Content.Load<Texture2D>("Character4");
        dot = Content.Load<Texture2D>("EndPoint_Purple");
        box = Content.Load<Texture2D>("CrateDark_Purple");
        wall = Content.Load<Texture2D>("Wall_Brown");

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        firstProject.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        //_spriteBatch.DrawString(font, "O texto tem esta fonte", new Vector2(100, 100), Color.Purple);
        //_spriteBatch.DrawString(font, "Este e o texto 2", new Vector2(100, 300), Color.Black);


        Rectangle position = new Rectangle(0, 0, tileSize, tileSize); //calculo do retangulo a depender do tileSize
        for (int x = 0; x < level.GetLength(0); x++) //pega a primeira dimensão
        {
            for (int y = 0; y < level.GetLength(1); y++) //pega a segunda dimensão
            {
                position.X = x * tileSize; // define o position
                position.Y = y * tileSize; // define o position

                switch (level[x, y])
                {
                    //case 'Y':
                    //    _spriteBatch.Draw(player, position, Color.White);
                    //    break;
                    //case '#':
                    //    _spriteBatch.Draw(box, position, Color.White);
                    //    break;
                    case '.':
                        _spriteBatch.Draw(dot, position, Color.White);
                        break;
                    case 'X':
                        _spriteBatch.Draw(wall, position, Color.White);
                        break;
                }
            }
        }
        position.X = firstProject.Position.X * tileSize; //posição do Player
        position.Y = firstProject.Position.Y * tileSize; //posição do Player
        _spriteBatch.Draw(player, position, Color.White); //desenha o Player

        foreach (Point b in boxes)
        {
            position.X = b.X * tileSize;
            position.Y = b.Y * tileSize;
            _spriteBatch.Draw(box, position, Color.White);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
    void LoadLevel(string levelFile)
    {
        boxes = new List<Point>();
        string[] linhas = File.ReadAllLines($"Content/{levelFile}"); // "Content/" + level
        int nrLinhas = linhas.Length;
        int nrColunas = linhas[0].Length;

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
                    level[x, y] = ' '; // put a blank instead of the sokoban 'Y'
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
        if (level[x, y] == 'X') return false; // se for uma parede está ocupada
        if (HasBox(x, y)) return false; // verifica se é uma caixa
        return true;
        /* The same as: return level[x,y] != 'X' && !HasBox(x,y); */
    }


}
