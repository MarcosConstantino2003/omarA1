public enum GameState
{
    Menu,
    InGame,
    GameOver,
    Paused,
    Options,
    Lobby,
    Win
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
    public Menu menu;
    public PauseScreen pauseScreen;
    public GameOverScreen gameOverScreen;
    public LobbyScreen lobbyScreen;
    public WinScreen winScreen;
    private InputHandler inputHandler;
    private Wave currentWave;
    private const int waveTotal = 5;
    private const int tiempoInicialWave = 10;
    private readonly Rectangle playArea = new Rectangle(250, 25, 1000, 800);


    public Game()
    {
        frame = new Frame();
        omar = new Omar(700, 400, 40);
        currentWave = new Wave(1, TimeSpan.FromSeconds(tiempoInicialWave + 1), omar);
        map = new Map(omar, currentWave);
        pressedKeys = new HashSet<Keys>();
        isFullScreen = true;

        menu = new Menu();
        pauseScreen = new PauseScreen();
        gameOverScreen = new GameOverScreen();
        lobbyScreen = new LobbyScreen();
        winScreen = new WinScreen();
        currentState = GameState.Menu;


        inputHandler = new InputHandler(this);

        frame.KeyDown += new KeyEventHandler(inputHandler.OnKeyDown);
        frame.KeyUp += new KeyEventHandler(inputHandler.OnKeyUp);
        frame.KeyPreview = true;
        frame.Paint += new PaintEventHandler(FramePaint);

        gameTimer = new System.Windows.Forms.Timer();
        gameTimer.Interval = 16;
        gameTimer.Tick += GameTimer_Tick;
    }

    public void ShowMenu()
    {
        currentState = GameState.Menu;
        frame.BackColor = Color.White;
        gameTimer.Stop();
        frame.Invalidate();
    }

    public void StartGame()
    {
        omar = new Omar(750, 400, 40);
        currentWave = new Wave(1, TimeSpan.FromSeconds(tiempoInicialWave + 1), omar);
        map = new Map(omar, currentWave);
        frame.BackColor = Color.Black;
        currentState = GameState.InGame;
        gameTimer.Start();
        frame.Invalidate();
        inputHandler.ResetInputHandler(omar);
    }

    public void RestartGame()
    {
        map = new Map(omar, currentWave);
        StartGame();
        inputHandler.ResetInputHandler(omar);
    }

    public void GoToLobby()
    {
        if (currentWave.WaveNumber == waveTotal)
        {
            currentState = GameState.Win;
        }
        else
        {
            currentState = GameState.Lobby;
        }
        gameTimer.Stop();
        frame.Invalidate();
    }

    public void ResetGameForLobby()
    {
        map.ClearObjects();
        omar.ResetPosition();
        currentState = GameState.InGame;
        frame.BackColor = Color.Black;
        currentWave = new Wave(currentWave.WaveNumber + 1, omar);
        gameTimer.Start();
        frame.Invalidate();
    }


    public void PauseGame()
    {
        currentState = GameState.Paused;
        frame.BackColor = Color.White;
        currentWave.Pause();
        gameTimer.Stop();
        frame.Invalidate();
    }

    public void ResumeGame()
    {
        currentState = GameState.InGame;
        currentWave.Resume();
        frame.BackColor = Color.Black;
        gameTimer.Start();
        frame.Invalidate();
    }

    public void ShowWinScreen()
    {
        currentState = GameState.Win;
        frame.BackColor = Color.LightGreen;
        gameTimer.Stop();
        frame.Invalidate();
    }


    private void GameTimer_Tick(object? sender, EventArgs e)
    {
        switch (currentState)
        {
            case GameState.InGame:
                map.update();
                frame.Invalidate();
                int timeLeft = currentWave.GetTimeLeft();
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


    private void FramePaint(object? sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        switch (currentState)
        {
            case GameState.InGame:
                g.FillRectangle(Brushes.Gray, playArea);
                using (Pen borderPen = new Pen(Color.White, 5)) // Grosor de 5 píxeles
                {
                    g.DrawRectangle(borderPen, playArea);
                }
                map.Draw(e.Graphics);
                omar.Draw(e.Graphics);
                frame.DrawStatistics(e.Graphics, omar);
                frame.DrawTimer(e.Graphics, currentWave.GetTimeLeft(), currentWave.WaveNumber);
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
            case GameState.Win:
                winScreen.Draw(e.Graphics, frame.ClientSize);
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
