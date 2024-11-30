using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.Versioning;
[SupportedOSPlatform("windows")]

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

        // Ejemplo de renderizado
        g.FillRectangle(Brushes.CornflowerBlue, 0, 0, this.ClientSize.Width, this.ClientSize.Height);
    }
}
