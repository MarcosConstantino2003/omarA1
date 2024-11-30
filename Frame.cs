using System;
using System.Drawing;
using System.Windows.Forms;

public class Frame : Form
{
    public Frame()
    {
        this.Text = "Juego en C#";
        this.ClientSize = new Size(800, 600);
        this.DoubleBuffered = true; // Para evitar parpadeos
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;

        // El dibujo de objetos será manejado por el método FramePaint en Game
    }

    public void DrawStatistics(Graphics g, Omar omar) {
    // Definir el mensaje con el valor de speed
    string speedText = "Speed: " + omar.Speed;

    // Fuente y pincel
    Font font = new Font("Arial", 16, FontStyle.Bold);
    Brush brush = Brushes.White;
    Pen pen = new Pen(Color.Black, 2);

    // Medir el tamaño del texto
    SizeF textSize = g.MeasureString(speedText, font);

    // Calcular la posición en la esquina superior derecha
    float x = ClientSize.Width - textSize.Width - 10; // Un margen de 10 píxeles desde el borde derecho
    float y = 10; // Margen superior

    // Dibujar el borde
    g.DrawRectangle(pen, x - 2, y - 2, textSize.Width + 4, textSize.Height + 4);
    
    // Dibujar el texto
    g.DrawString(speedText, font, brush, x, y);
}
}
