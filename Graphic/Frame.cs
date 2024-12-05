using System;
using System.Drawing;
using System.Windows.Forms;

public class Frame : Form
{
    public Frame()
    {
        this.Text = "Omar's Brotato";
        this.ClientSize = new Size(800, 600);
        this.DoubleBuffered = true; // Para evitar parpadeos
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;

        // El dibujo de objetos será manejado por el método FramePaint en Game
    }

    public void DrawTimer(Graphics g, int timeLeft, int waveCount)
    {
        string timerText = $"Tiempo: {timeLeft}s";
        string waveText = $"Oleada: {waveCount}";

        Font font = new Font("Arial", 24, FontStyle.Bold);
        Brush brush = Brushes.Black;

        // Medir el tamaño del texto para el temporizador
        SizeF timerTextSize = g.MeasureString(timerText, font);
        float x = (ClientSize.Width - timerTextSize.Width) / 2; // Centrado horizontalmente
        float y = 10; // Posición cerca del borde superior

        // Dibujar el temporizador
        g.DrawString(timerText, font, brush, x, y);

        // Cambiar el tamaño de la fuente para las oleadas
        Font waveFont = new Font("Arial", 18, FontStyle.Bold); // Fuente más pequeña
        SizeF waveTextSize = g.MeasureString(waveText, waveFont);
        float waveTextY = y + timerTextSize.Height + 5; // Un poco debajo del temporizador

        // Dibujar el contador de oleadas centrado
        g.DrawString(waveText, waveFont, brush, (ClientSize.Width - waveTextSize.Width) / 2, waveTextY);
    }

    public void DrawStatistics(Graphics g, Omar omar)
    {
        // Dibujar estadísticas en la parte superior derecha
        Font font = new Font("Arial", 14, FontStyle.Bold);
        Brush whiteBrush = Brushes.White;
        Brush blackBrush = Brushes.Black;
        Pen blackPen = new Pen(Color.Black, 2);

        // Espaciado entre líneas
        int lineHeight = 20;

        // Dibujar HP
        string hpText = $"HP: {omar.HP}/{omar.MaxHP}";
        SizeF hpSize = g.MeasureString(hpText, font);
        Brush hpBrush = (omar.HP == omar.MaxHP) ? Brushes.DarkRed : whiteBrush;
        g.DrawString(hpText, font, blackBrush, this.ClientSize.Width - hpSize.Width - 10, 10);
        g.DrawString(hpText, font, hpBrush, this.ClientSize.Width - hpSize.Width - 9, 9);

        // Dibujar HP Regen
        string regenText = $"HP Regen: {omar.HPRegen}"; // Nueva estadística
        SizeF regenSize = g.MeasureString(regenText, font);
        g.DrawString(regenText, font, blackBrush, this.ClientSize.Width - regenSize.Width - 10, 10 + lineHeight);
        g.DrawString(regenText, font, whiteBrush, this.ClientSize.Width - regenSize.Width - 9, 9 + lineHeight);

        // Dibujar daño
        string dmgText = $"Damage: {omar.damage}";
        SizeF dmgSize = g.MeasureString(dmgText, font);
        g.DrawString(dmgText, font, blackBrush, this.ClientSize.Width - dmgSize.Width - 10, 10 + lineHeight * 2);
        g.DrawString(dmgText, font, whiteBrush, this.ClientSize.Width - dmgSize.Width - 9, 9 + lineHeight * 2);

        // Dibujar velocidad de disparo
        string shotSpeedText = $"Shot Speed: {omar.shotSpeed}";
        SizeF shotSpeedSize = g.MeasureString(shotSpeedText, font);
        g.DrawString(shotSpeedText, font, blackBrush, this.ClientSize.Width - shotSpeedSize.Width - 10, 10 + lineHeight * 3);
        g.DrawString(shotSpeedText, font, whiteBrush, this.ClientSize.Width - shotSpeedSize.Width - 9, 9 + lineHeight * 3);

        // Dibujar velocidad
        string speedText = $"Speed: {omar.Speed}";
        SizeF speedSize = g.MeasureString(speedText, font);
        Brush speedBrush = (omar.Speed == omar.MaxSpeed) ? Brushes.DarkRed : whiteBrush;
        g.DrawString(speedText, font, blackBrush, this.ClientSize.Width - speedSize.Width - 10, 10 + lineHeight * 4);
        g.DrawString(speedText, font, speedBrush, this.ClientSize.Width - speedSize.Width - 9, 9 + lineHeight * 4);

        // Dibujar rango
        string rangeText = $"Range: {omar.range}";
        SizeF rangeSize = g.MeasureString(rangeText, font);
        g.DrawString(rangeText, font, blackBrush, this.ClientSize.Width - rangeSize.Width - 10, 10 + lineHeight * 5);
        g.DrawString(rangeText, font, whiteBrush, this.ClientSize.Width - rangeSize.Width - 9, 9 + lineHeight * 5);

    }


}
