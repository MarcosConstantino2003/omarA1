using System;
using System.Drawing;
using System.Windows.Forms;

public class Game
{
    private Frame frame;
    private Omar omar;
    private bool isInGame;
    private bool isGameOver;
    private System.Windows.Forms.Timer gameTimer; // El temporizador para actualizar la pantalla
    private HashSet<Keys> pressedKeys; // Usaremos un HashSet para almacenar las teclas presionadas
    private Map map;
    private bool isFullScreen;
    private const int GameWidth = 800;  // Ancho real del juego
    private const int GameHeight = 600; // Alto real del juego
    public Game()
    {
        frame = new Frame();
        omar = new Omar(100, 100, 40); // Inicializa a Omar con tamaño 40
        map = new Map(); // Inicializamos el mapa
        isInGame = false; // Inicialmente en el menú
        pressedKeys = new HashSet<Keys>(); // Inicializar el HashSet
        isFullScreen = false; // Inicialmente no está en pantalla completa


        // Suscribimos los eventos de teclas presionadas y liberadas
        frame.KeyDown += new KeyEventHandler(OnKeyDown);
        frame.KeyUp += new KeyEventHandler(OnKeyUp);
        
        // Permitir el enfoque del formulario para detectar las teclas
        frame.KeyPreview = true;
        frame.Paint += new PaintEventHandler(FramePaint);

          // Inicializamos el temporizador
        gameTimer = new System.Windows.Forms.Timer(); // Especificamos el namespace completo
        gameTimer.Interval = 16; // Aproximadamente 60 FPS
        gameTimer.Tick += GameTimer_Tick;   
    }

    // Método para cambiar al estado de juego
    public void StartGame()
    {
        isInGame = true;
        frame.BackColor = Color.Gray; // Cambiar el fondo a gris
        gameTimer.Start(); // Iniciar el temporizador
        frame.Invalidate(); // Forzar un repintado de la ventana
    }

      // Reiniciar el juego
    private void RestartGame()
    {
        omar = new Omar(100, 100, 40); // Reinicia a Omar
        map = new Map(); // Reinicia el mapa
        isInGame = true; // Comienza el juego
        isGameOver = false; // Reinicia el estado de Game Over
        frame.BackColor = Color.Gray; // Fondo gris
        gameTimer.Start(); // Inicia el temporizador
        frame.Invalidate(); // Forzar repintado
    }


    // Método para cambiar al estado de menú
    public void ShowMenu()
    {
        isInGame = false;
        frame.BackColor = Color.White; // Fondo blanco en el menú
        gameTimer.Stop(); // Detener el temporizador
        frame.Invalidate(); // Forzar un repintado de la ventana
    }
     private void ToggleFullScreen()
    {
        if (isFullScreen)
        {
            // Regresar a la ventana normal
            frame.FormBorderStyle = FormBorderStyle.Sizable;
            frame.WindowState = FormWindowState.Normal;
            frame.Size = new Size(800, 600); // Resolución original
            isFullScreen = false;
        }
        else
        {
               // Cambiar a pantalla completa
            frame.FormBorderStyle = FormBorderStyle.None;
            frame.WindowState = FormWindowState.Maximized;
            isFullScreen = true;
        }
    }

  // Método que se llama cada vez que el temporizador "tickea" (cada 16 ms)
    private void GameTimer_Tick(object? sender, EventArgs e)
    {
        if (isInGame)
        {
            // Actualizar la posición de Omar suavemente en cada "tick"
            omar.UpdatePosition();
            // Revisar las colisiones
            map.CheckCollisions(omar);
            // Redibujar la pantalla constantemente
            frame.Invalidate();
              if (omar.HP <= 0)
            {
                isInGame = false;
                isGameOver = true;  // El juego ha terminado
            }
        }
    }
    // Detecta la tecla presionada y cambia el estado del juego
    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F)
        {
            // Cambiar a pantalla completa o volver a ventana normal al presionar F
            ToggleFullScreen();
        }

        if (!pressedKeys.Contains(e.KeyCode))
        {
            pressedKeys.Add(e.KeyCode); // Agregar la tecla al HashSet cuando se presiona
        }

        if (isInGame)
        {
            int dx = 0;
            int dy = 0;

            // Usamos las teclas presionadas para mover a Omar
            if (pressedKeys.Contains(Keys.W)) dy = -1; // Mover hacia arriba
            if (pressedKeys.Contains(Keys.S)) dy = 1;  // Mover hacia abajo
            if (pressedKeys.Contains(Keys.A)) dx = -1; // Mover hacia la izquierda
            if (pressedKeys.Contains(Keys.D)) dx = 1;  // Mover hacia la derecha
            omar.MoveSmooth(dx, dy);

            if (e.KeyCode == Keys.Escape)
            {
                ShowMenu();
            }
              else if (e.KeyCode == Keys.R) // Reiniciar el juego
            {
                RestartGame(); // Reinicia el juego
            }
        } else if (isGameOver){
            if (e.KeyCode == Keys.Escape)
            {
                isGameOver = false;
                RestartGame(); // Reinicia el juego
                ShowMenu();

            }
        }
        else
        {
            // Si no está en el juego, solo manejamos las teclas de Enter y Escape
            if (e.KeyCode == Keys.Enter)
            {
                StartGame();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }
        }
    }

    private void OnKeyUp(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.W || e.KeyCode == Keys.S)
        {
            omar.VelocityY = 0; // Detener el movimiento en Y
        }

        if (e.KeyCode == Keys.A || e.KeyCode == Keys.D)
        {
            omar.VelocityX = 0; // Detener el movimiento en X
        }
        pressedKeys.Remove(e.KeyCode);
    }


    private void FramePaint(object? sender, PaintEventArgs e)
    {   
        if (isInGame)
        {
            // Dibuja el mapa
            map.Draw(e.Graphics);
            // Dibuja a Omar solo si estamos en el juego
            omar.Draw(e.Graphics);
            // Mover a los enemigos hacia Omar
            map.UpdateEnemies(omar);
            // Dibuja las estadísticas (en este caso, la velocidad)
            frame.DrawStatistics(e.Graphics, omar);
           
        }
        else
        {
             // Mostrar el mensaje de menú o derrota
            if (isGameOver)
            {
                frame.DrawGameOverMessage(e.Graphics); // Mostrar mensaje de derrota
            } else {
            // Mostrar el mensaje de menú
            frame.ShowMenuMessage(e.Graphics);
        }
        }
    }

    

    public static void Main(string[] args)
    {
        Game game = new Game();
        game.ShowMenu(); // Empieza en el menú
        Application.Run(game.frame);
    }
}
