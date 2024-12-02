using System;
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
    private Frame frame;
    public Omar omar;
    private GameState currentState; // Estado actual del juego
    private System.Windows.Forms.Timer gameTimer;
    private HashSet<Keys> pressedKeys;
    private Map map;
    private bool isFullScreen;
    private const int GameWidth = 800;
    private const int GameHeight = 600;
    private Menu menu;
    private PauseScreen pauseScreen;
    private GameOverScreen gameOverScreen;
    private LobbyScreen lobbyScreen;
    private DateTime gameStartTime;
    private TimeSpan gameDuration = TimeSpan.FromSeconds(10); // Duración del juego
    private TimeSpan pausedDuration = TimeSpan.Zero; // Acumula el tiempo en pausa
    private DateTime pauseStartTime; // Marca cuándo empezó la pausa
    

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
        gameStartTime = DateTime.Now;
        gameTimer.Start();
        frame.Invalidate();
        UpdateStartTime();
    }

    private void RestartGame()
    {
        omar = new Omar(100, 100, 40);
        map = new Map(omar);
        currentState = GameState.InGame;
        frame.BackColor = Color.Gray;
        gameTimer.Start();
        frame.Invalidate();
        UpdateStartTime();
    }

    private void ResetGameForLobby()
    {
        UpdateStartTime();
        map.ClearObjects();
        omar.ResetPosition();
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

    public void ResumeGame()
    {
        currentState = GameState.InGame;
        frame.BackColor = Color.Gray;
        pausedDuration += DateTime.Now - pauseStartTime; // Suma el tiempo pausado
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
                int timeLeft = GetTimeLeft();
                if (timeLeft == 0)
                {
                    // Cambiar al estado Lobby cuando el tiempo termine
                    currentState = GameState.Lobby;
                    ResetGameForLobby();
                    frame.Invalidate();
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
    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F)
        {
            ToggleFullScreen();
        }

        if (!pressedKeys.Contains(e.KeyCode))
        {
            pressedKeys.Add(e.KeyCode); 
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
            case GameState.Lobby:
                HandleLobbyControls(e);
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
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
        {
            menu.MoveUp();
            frame.Invalidate();
        }
        else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
        {
            menu.MoveDown();
            frame.Invalidate();
        }
        else if (e.KeyCode == Keys.Enter)
        {
            string selectedOption = menu.GetSelectedOption();
            if (selectedOption == "Jugar")
            {
                StartGame();
            }
            else if (selectedOption == "Opciones")
            {
                // Aquí puedes implementar el menú de opciones
            }
            else if (selectedOption == "Salir")
            {
                Application.Exit();
            }
        }
        else if (e.KeyCode == Keys.Escape)
        {
          Application.Exit();  
        }
    }

   private void HandleGameOverControls(KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
        {
            gameOverScreen.MoveUp();
            frame.Invalidate();
        }
        else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
        {
            gameOverScreen.MoveDown();
            frame.Invalidate();
        }
        else if (e.KeyCode == Keys.Enter)
        {
            string selectedOption = gameOverScreen.GetSelectedOption();
            if (selectedOption == "Reintentar")
            {
                RestartGame();
            }
            else if (selectedOption == "Menu Principal")
            {
                RestartGame();
                ShowMenu();
            }
        }
    }

    private void HandlePausedControls(KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
        {
            pauseScreen.MoveUp();
            frame.Invalidate();
        }
        else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
        {
            pauseScreen.MoveDown();
            frame.Invalidate();
        }
        else if (e.KeyCode == Keys.Enter)
        {
            string selectedOption = pauseScreen.GetSelectedOption();
            if (selectedOption == "Continuar")
            {
                ResumeGame();
            }
            else if (selectedOption == "Menu Principal")
            {
                RestartGame();
                ShowMenu();
            }
        }
    }

    private void HandleLobbyControls(KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
        {
            currentState = GameState.InGame;
            frame.BackColor = Color.Gray; // Cambiar el fondo a gris
            frame.Invalidate();
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
