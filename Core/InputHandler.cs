public class InputHandler
{
    private HashSet<Keys> pressedKeys;
    private Omar omar;
    private Menu menu;
    private GameOverScreen gameOverScreen;
    private PauseScreen pauseScreen;
    private Map map;
    private Game game;

    public InputHandler(Game game)
    {
        omar = game.omar;
        menu = game.menu;
        gameOverScreen = game.gameOverScreen;
        pauseScreen = game.pauseScreen;
        map = game.map;
        this.game = game;
        pressedKeys = new HashSet<Keys>();
    }

    public void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F)
        {
            ToggleFullScreen();
        }

        if (!pressedKeys.Contains(e.KeyCode))
        {
            pressedKeys.Add(e.KeyCode);
        }

        switch (game.currentState)
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
            case GameState.Win:
                HandleWinScreenControls(e);
                break;
        }
    }

    public void OnKeyUp(object? sender, KeyEventArgs e)
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
            game.PauseGame();
        }
        else if (e.KeyCode == Keys.R)
        {
            game.RestartGame();
        }
    }

    private void HandleMenuControls(KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
        {
            menu.MoveUp();
            game.frame.Invalidate();
        }
        else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
        {
            menu.MoveDown();
            game.frame.Invalidate();
        }
        else if (e.KeyCode == Keys.Enter)
        {
            string selectedOption = menu.GetSelectedOption();
            if (selectedOption == "Jugar")
            {
                game.StartGame();
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
            game.frame.Invalidate();
        }
        else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
        {
            gameOverScreen.MoveDown();
            game.frame.Invalidate();
        }
        else if (e.KeyCode == Keys.Enter)
        {
            string selectedOption = gameOverScreen.GetSelectedOption();
            if (selectedOption == "Reintentar")
            {
                game.RestartGame();
            }
            else if (selectedOption == "Menu Principal")
            {
                game.RestartGame();
                game.ShowMenu();
            }
        }
    }

    private void HandlePausedControls(KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
        {
            pauseScreen.MoveUp();
            game.frame.Invalidate();
        }
        else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
        {
            pauseScreen.MoveDown();
            game.frame.Invalidate();
        }
        else if (e.KeyCode == Keys.Enter)
        {
            string selectedOption = pauseScreen.GetSelectedOption();
            if (selectedOption == "Continuar")
            {
                game.ResumeGame();
            }
            else if (selectedOption == "Menu Principal")
            {
                game.RestartGame();
                game.ShowMenu();
            }
        }
    }

    private void HandleLobbyControls(KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
        {
            game.ResetGameForLobby();
        }
    }

    private void HandleWinScreenControls(KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
        {
            game.ShowMenu(); 
        }
    }

    private void ToggleFullScreen()
    {
        if (game.isFullScreen)
        {
            game.frame.FormBorderStyle = FormBorderStyle.Sizable;
            game.frame.WindowState = FormWindowState.Normal;
            game.frame.Size = new Size(800, 600); 
            game.isFullScreen = false;
        }
        else
        {
            game.frame.FormBorderStyle = FormBorderStyle.None;
            game.frame.WindowState = FormWindowState.Maximized;
            game.isFullScreen = true;
        }
    }

    public void ResetInputHandler(Omar newOmar)
    {
        omar = newOmar; 
        pressedKeys.Clear(); 
    }
}
