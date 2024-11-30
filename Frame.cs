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
}
