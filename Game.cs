using System;
using System.Drawing;
using System.Windows.Forms;

public class Game
{
    private Frame frame;
    private Omar omar;
    private bool isInGame;

    public Game()
    {
        frame = new Frame();
        omar = new Omar(100, 100, 20); // Inicia Omar en (100, 100) con tamaño 20
        isInGame = false; // Inicialmente en el menú

        // Suscribimos el evento de teclas presionadas para detectar Enter y Escape
        frame.KeyDown += new KeyEventHandler(OnKeyDown);
        
        // Permitir el enfoque del formulario para detectar las teclas
        frame.KeyPreview = true;
        
        frame.Paint += new PaintEventHandler(FramePaint);
    }

    // Método para cambiar al estado de juego
    public void StartGame()
    {
        isInGame = true;
        frame.BackColor = Color.Gray; // Cambiar el fondo a gris
        frame.Invalidate(); // Forzar un repintado de la ventana
    }

    // Método para cambiar al estado de menú
    public void ShowMenu()
    {
        isInGame = false;
        frame.BackColor = Color.White; // Fondo blanco en el menú
        frame.Invalidate(); // Forzar un repintado de la ventana
    }

    // Detecta la tecla presionada y cambia el estado del juego
    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            if (!isInGame)
            {
                // Iniciar el juego al presionar Enter
                StartGame();
            }
        }
        else if (e.KeyCode == Keys.Escape)
        {
            if (isInGame)
            {
                // Volver al menú si estamos en el juego
                ShowMenu();
            }
              else
        {
            // Cerrar la aplicación si estamos en el menú
            Application.Exit();
        }
            }
    }

    private void FramePaint(object? sender, PaintEventArgs e)
    {
        if (isInGame)
        {
            // Dibuja a Omar solo si estamos en el juego
            omar.Draw(e.Graphics);
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
