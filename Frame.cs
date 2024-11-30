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

    
public void DrawStatistics(Graphics g, Omar omar) 
{
    // Dibujar estadísticas en la parte superior derecha
    Font font = new Font("Arial", 14, FontStyle.Bold);
    Brush whiteBrush = Brushes.White;
    Brush blackBrush = Brushes.Black;
    Pen blackPen = new Pen(Color.Black, 2);

    // Dibujar velocidad
    string speedText = $"Speed: {omar.Speed}";
    SizeF speedSize = g.MeasureString(speedText, font);
    
    // Cambiar el color del texto a rojo oscuro si la velocidad es 8
    Brush speedBrush = (omar.Speed == 8) ? Brushes.DarkRed : whiteBrush;
    
    // Dibujar el texto con borde negro
    g.DrawString(speedText, font, blackBrush, this.ClientSize.Width - speedSize.Width - 10, 10);
    g.DrawString(speedText, font, speedBrush, this.ClientSize.Width - speedSize.Width - 9, 9); // Borde negro

    // Dibujar HP
    string hpText = $"HP: {omar.HP}";
    SizeF hpSize = g.MeasureString(hpText, font);
    g.DrawString(hpText, font, blackBrush, this.ClientSize.Width - hpSize.Width - 10, 30);
    g.DrawString(hpText, font, whiteBrush, this.ClientSize.Width - hpSize.Width - 9, 29); // Borde negro
}
}
