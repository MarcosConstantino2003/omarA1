using System;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Windows.Forms;

public enum GameState
{
    Menu,
    InGame,
    GameOver,
    Paused,
    Options,
    Lobby
}

public class Game
{
    public Frame frame;
    public Omar omar;
    public GameState currentState; 
    private System.Windows.Forms.Timer gameTimer;
    private HashSet<Keys> pressedKeys;
    public Map map;
    public bool isFullScreen;
    private const int GameWidth = 800;
    private const int GameHeight = 600;
    public Menu menu;
    public PauseScreen pauseScreen;
    public GameOverScreen gameOverScreen;
    public LobbyScreen lobbyScreen;
    private DateTime gameStartTime;
    private TimeSpan gameDuration = TimeSpan.FromSeconds(10); // Duración del juego
    private TimeSpan pausedDuration = TimeSpan.Zero; // Acumula el tiempo en pausa
    private DateTime pauseStartTime; // Marca cuándo empezó la pausa
    private InputHandler inputHandler;
    


    public Game()
    {
        frame = new Frame();
        omar = new Omar(100, 100, 40); 
        map = new Map(omar); 
        pressedKeys = new HashSet<Keys>(); 
        isFullScreen = false; 
        menu = new Menu();
        pauseScreen = new PauseScreen();
        gameOverScreen = new GameOverScreen();
        lobbyScreen = new LobbyScreen();
        currentState = GameState.Menu;
        gameStartTime = DateTime.Now;

        inputHandler = new InputHandler(this);
        frame.KeyDown += new KeyEventHandler(inputHandler.OnKeyDown);
        frame.KeyUp += new KeyEventHandler(inputHandler.OnKeyUp);
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
        gameStartTime = DateTime.Now;
        gameTimer.Start();
        frame.Invalidate();
        UpdateStartTime();
    }

    public void RestartGame()
    {
        omar = new Omar(100, 100, 40);
        map = new Map(omar);
        currentState = GameState.InGame;
        frame.BackColor = Color.Gray;
        gameTimer.Start();
        frame.Invalidate();
        UpdateStartTime();
        inputHandler.ResetInputHandler(omar);
    }

    public void ResetGameForLobby()
    {
        UpdateStartTime();
        map.ClearObjects();
        omar.ResetPosition();
        currentState = GameState.InGame;
        frame.BackColor = Color.Gray;
        gameStartTime = DateTime.Now;
        gameTimer.Start();
        frame.Invalidate();
        UpdateStartTime();
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
        pauseStartTime = DateTime.Now; // Marca el inicio de la pausa
        gameTimer.Stop();
        frame.Invalidate();
    }

    public void GoToLobby(){
        currentState = GameState.Lobby;
        gameTimer.Stop();
        frame.Invalidate();
    }

    public void ResumeGame()
    {
        currentState = GameState.InGame;
        frame.BackColor = Color.Gray;
        pausedDuration += DateTime.Now - pauseStartTime; // Suma el tiempo pausado
        gameTimer.Start();
        frame.Invalidate();
    }

    private void GameTimer_Tick(object? sender, EventArgs e)
    {   
        switch (currentState)
        {
            case GameState.InGame:
                map.update();
                frame.Invalidate();
                int timeLeft = GetTimeLeft();
                if (timeLeft == 0)
                {
                    GoToLobby();
                }
                if (omar.HP <= 0)
                {
                    currentState = GameState.GameOver;
                }
                break;
            case GameState.Paused:
                break;
        }
    }
    private int GetTimeLeft()
    {
        TimeSpan elapsed = DateTime.Now - gameStartTime - pausedDuration; // Resta el tiempo en pausa
        int timeLeft = (int)(gameDuration.TotalSeconds - elapsed.TotalSeconds);
        return Math.Max(0, timeLeft);
    }

    private void UpdateStartTime(){
        gameStartTime = DateTime.Now;
    }
    

    private void FramePaint(object? sender, PaintEventArgs e)
    {
        switch (currentState)
        {
            case GameState.InGame:
                map.Draw(e.Graphics);
                omar.Draw(e.Graphics);
                frame.DrawStatistics(e.Graphics, omar);
                frame.DrawTimer(e.Graphics, GetTimeLeft());
                break;
            case GameState.Menu:
                menu.Draw(e.Graphics, frame.ClientSize);
                break;
            case GameState.Paused:
                pauseScreen.Draw(e.Graphics, frame.ClientSize);
                break;
            case GameState.GameOver:
                gameOverScreen.Draw(e.Graphics, frame.ClientSize);
                break;
            case GameState.Lobby:
                lobbyScreen.Draw(e.Graphics, frame.ClientSize);
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
