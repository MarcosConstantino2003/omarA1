using System;
using System.Drawing;
using System.Windows.Forms;

public enum GameState
{
    Menu,
    InGame,
    GameOver,
    Paused,
    Options
}

public class Game
{
    private Frame frame;
    public Omar omar;
    private GameState currentState; // Estado actual del juego
    private System.Windows.Forms.Timer gameTimer;
    private HashSet<Keys> pressedKeys;
    private Map map;
    private bool isFullScreen;
    private const int GameWidth = 800;
    private const int GameHeight = 600;

    public Game()
    {
        frame = new Frame();
        omar = new Omar(100, 100, 40); 
        map = new Map(omar); 
        pressedKeys = new HashSet<Keys>(); 
        isFullScreen = false; 

        currentState = GameState.Menu; // Inicialmente en el menú

        frame.KeyDown += new KeyEventHandler(OnKeyDown);
        frame.KeyUp += new KeyEventHandler(OnKeyUp);
        frame.KeyPreview = true;
        frame.Paint += new PaintEventHandler(FramePaint);

        gameTimer = new System.Windows.Forms.Timer();
        gameTimer.Interval = 16; 
        gameTimer.Tick += GameTimer_Tick;   
    }

    
    public void StartGame()
    {
        currentState = GameState.InGame;
        frame.BackColor = Color.Gray;
        gameTimer.Start();
        frame.Invalidate();
    }

    private void RestartGame()
    {
        omar = new Omar(100, 100, 40);
        map = new Map(omar);
        currentState = GameState.InGame;
        frame.BackColor = Color.Gray;
        gameTimer.Start();
        frame.Invalidate();
    }


    public void ShowMenu()
    {
        currentState = GameState.Menu;
        frame.BackColor = Color.White;
        gameTimer.Stop();
        frame.Invalidate();
    }

    public void PauseGame()
    {
        currentState = GameState.Paused;
        frame.BackColor = Color.White;
        gameTimer.Stop();
        frame.Invalidate();
    }

    public void ResumeGame()
    {
        currentState = GameState.InGame;  
        frame.BackColor = Color.Gray;    
        gameTimer.Start();                
        frame.Invalidate();               
    }

    private void ToggleFullScreen()
    {
        if (isFullScreen)
        {
            frame.FormBorderStyle = FormBorderStyle.Sizable;
            frame.WindowState = FormWindowState.Normal;
            frame.Size = new Size(800, 600); 
            isFullScreen = false;
        }
        else
        {
            frame.FormBorderStyle = FormBorderStyle.None;
            frame.WindowState = FormWindowState.Maximized;
            isFullScreen = true;
        }
    }

    private void GameTimer_Tick(object? sender, EventArgs e)
    {
        switch (currentState)
        {
            case GameState.InGame:
                map.update();
                frame.Invalidate();
                if (omar.HP <= 0)
                {
                    currentState = GameState.GameOver;
                }
                break;
            case GameState.Paused:
                break;
        }
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F)
        {
            ToggleFullScreen();
        }

        if (!pressedKeys.Contains(e.KeyCode))
        {
            pressedKeys.Add(e.KeyCode); // Agregar la tecla al HashSet cuando se presiona
        }

        switch (currentState)
        {
            case GameState.InGame:
                HandleInGameControls(e);
                break;
            case GameState.Menu:
                HandleMenuControls(e);
                break;
            case GameState.GameOver:
                HandleGameOverControls(e);
                break;
            case GameState.Paused:
                HandlePausedControls(e);
                break;
        }
    }

    private void HandleInGameControls(KeyEventArgs e)
    {
        int dx = 0;
        int dy = 0;

        if (pressedKeys.Contains(Keys.W)) dy = -1;
        if (pressedKeys.Contains(Keys.S)) dy = 1;
        if (pressedKeys.Contains(Keys.A)) dx = -1;
        if (pressedKeys.Contains(Keys.D)) dx = 1;

        omar.MoveSmooth(dx, dy);

        if (e.KeyCode == Keys.Escape)
        {
            PauseGame();
            currentState = GameState.Paused;
        }
        else if (e.KeyCode == Keys.R)
        {
            RestartGame();
        }
    }

    private void HandleMenuControls(KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            StartGame();
        }
        else if (e.KeyCode == Keys.Escape)
        {
            Application.Exit();
        }
    }

    private void HandleGameOverControls(KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            RestartGame();
            ShowMenu();
        }
    }

    private void HandlePausedControls(KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Enter)
        {
            ResumeGame();
            currentState = GameState.InGame;
        } else if  (e.KeyCode == Keys.M){
            RestartGame();
            ShowMenu();
        }
    }

    private void OnKeyUp(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.W || e.KeyCode == Keys.S)
        {
            omar.VelocityY = 0;
        }

        if (e.KeyCode == Keys.A || e.KeyCode == Keys.D)
        {
            omar.VelocityX = 0;
        }
        pressedKeys.Remove(e.KeyCode);
    }

    private void FramePaint(object? sender, PaintEventArgs e)
    {
        switch (currentState)
        {
            case GameState.InGame:
                map.Draw(e.Graphics);
                omar.Draw(e.Graphics);
                frame.DrawStatistics(e.Graphics, omar);
                break;
            case GameState.GameOver:
                frame.DrawGameOverMessage(e.Graphics);
                break;
            case GameState.Menu:
                frame.ShowMenuMessage(e.Graphics);
                break;
            case GameState.Paused:
                frame.DrawPausedMessage(e.Graphics);
                break;
        }
    }

    public static void Main(string[] args)
    {
        Game game = new Game();
        game.ShowMenu(); 
        Application.Run(game.frame);
    }
}
