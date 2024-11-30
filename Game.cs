using System;
using System.Drawing;
using System.Windows.Forms;

public class Game
{
    private Frame frame;
    private Omar omar;
    private bool isInGame;
    private System.Windows.Forms.Timer gameTimer; // El temporizador para actualizar la pantalla
    private HashSet<Keys> pressedKeys; // Usaremos un HashSet para almacenar las teclas presionadas
    private Map map;

    public Game()
    {
        frame = new Frame();
        omar = new Omar(100, 100, 40); // Inicializa a Omar con tamaño 40
        map = new Map(); // Inicializamos el mapa
        isInGame = false; // Inicialmente en el menú
        pressedKeys = new HashSet<Keys>(); // Inicializar el HashSet

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

    // Método para cambiar al estado de menú
    public void ShowMenu()
    {
        isInGame = false;
        frame.BackColor = Color.White; // Fondo blanco en el menú
        gameTimer.Stop(); // Detener el temporizador
        frame.Invalidate(); // Forzar un repintado de la ventana
    }

  // Método que se llama cada vez que el temporizador "tickea" (cada 16 ms)
    private void GameTimer_Tick(object? sender, EventArgs e)
    {
        if (isInGame)
        {
            // Revisar las colisiones
            map.CheckCollisions(omar);
            // Redibujar la pantalla constantemente
            frame.Invalidate();
        }
    }
    // Detecta la tecla presionada y cambia el estado del juego
    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
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

            // Mover a Omar en las 8 direcciones usando el valor de speed
            omar.Move(dx * omar.Speed, dy * omar.Speed);
            
             // Escape: Regresar al menú cuando estamos en el juego
            if (e.KeyCode == Keys.Escape)
            {
                ShowMenu(); // Volver al menú
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
        if (pressedKeys.Contains(e.KeyCode))
        {
            pressedKeys.Remove(e.KeyCode); // Eliminar la tecla del HashSet cuando se deja de presionar
        }
    }


    private void FramePaint(object? sender, PaintEventArgs e)
    {
        if (isInGame)
        {
             // Dibuja el mapa
            map.Draw(e.Graphics);
            // Dibuja a Omar solo si estamos en el juego
            omar.Draw(e.Graphics);
            // Dibuja las estadísticas (en este caso, la velocidad)
            frame.DrawStatistics(e.Graphics, omar);
           
        }
        else
        {
            // Mostrar el mensaje de menú
            ShowMenuMessage(e.Graphics);
        }
    }

    // Método para dibujar el mensaje en el menú
    private void ShowMenuMessage(Graphics g)
    {
       string message1 = "Presiona Enter para empezar";
        string message2 = "Presiona Esc para salir";
        Font font1 = new Font("Arial", 24, FontStyle.Bold);
        Font font2 = new Font("Arial", 18, FontStyle.Regular); // Fuente más pequeña para el segundo mensaje
        Brush brush = Brushes.Black;

        // Calcular la posición centrada para el primer mensaje
        SizeF textSize1 = g.MeasureString(message1, font1);
        float x1 = (frame.ClientSize.Width - textSize1.Width) / 2;
        float y1 = (frame.ClientSize.Height - textSize1.Height) / 2;

        // Dibujar el primer mensaje
        g.DrawString(message1, font1, brush, x1, y1);

        // Calcular la posición para el segundo mensaje (debajo del primero)
        SizeF textSize2 = g.MeasureString(message2, font2);
        float x2 = (frame.ClientSize.Width - textSize2.Width) / 2;
        float y2 = y1 + textSize1.Height + 10; // 10 píxeles de espacio entre los dos mensajes

        // Dibujar el segundo mensaje
        g.DrawString(message2, font2, brush, x2, y2);
    }

    public static void Main(string[] args)
    {
        Game game = new Game();
        game.ShowMenu(); // Empieza en el menú
        Application.Run(game.frame);
    }
}
