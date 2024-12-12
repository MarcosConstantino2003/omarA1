using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Text;

public class Frame : Form
{
    private PrivateFontCollection fontCollection;

    public Frame()
    {
        this.Text = "Omar's Brotato";
        this.WindowState = FormWindowState.Maximized;
        this.FormBorderStyle = FormBorderStyle.None;
        this.DoubleBuffered = true;
        this.BackColor = Color.Black;
        fontCollection = new PrivateFontCollection();
        fontCollection.AddFontFile("font\\chewypro.otf");

    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;
    }

    public void drawUI(Graphics g, int timeLeft, int waveCount, Omar omar)
    {
        drawTimer(g, timeLeft, waveCount);
        drawStatistics(g, omar);
        drawLifeBar(g, omar);
    }

    public void drawTimer(Graphics g, int timeLeft, int waveCount)
    {
        string timerText = $"Tiempo: {timeLeft}s";
        string waveText = $"Oleada: {waveCount}";

        Font font = new Font(fontCollection.Families[0], 24, FontStyle.Bold);
        Brush brush = Brushes.Black;
        Brush borderBrush = Brushes.White;

        // Medir el tamaño del texto para el temporizador
        SizeF timerTextSize = g.MeasureString(timerText, font);
        float x = (ClientSize.Width - timerTextSize.Width) / 2;
        float y = 10;

        // Dibujar el borde blanco primero (con un pequeño desplazamiento)
        g.DrawString(timerText, font, borderBrush, x + 2, y + 2);
        g.DrawString(timerText, font, borderBrush, x - 2, y + 2);
        g.DrawString(timerText, font, borderBrush, x + 2, y - 2);
        g.DrawString(timerText, font, borderBrush, x - 2, y - 2);

        // Dibujar el texto original (negro)
        g.DrawString(timerText, font, brush, x, y);

        // Cambiar el tamaño de la fuente para las oleadas
        Font waveFont = new Font(fontCollection.Families[0], 18, FontStyle.Bold);
        SizeF waveTextSize = g.MeasureString(waveText, waveFont);
        float waveTextY = y + timerTextSize.Height + 5; // Un poco debajo del temporizador

        // Dibujar el borde blanco primero (con un pequeño desplazamiento)
        g.DrawString(waveText, waveFont, borderBrush, (ClientSize.Width - waveTextSize.Width) / 2 + 2, waveTextY + 2);
        g.DrawString(waveText, waveFont, borderBrush, (ClientSize.Width - waveTextSize.Width) / 2 - 2, waveTextY + 2);
        g.DrawString(waveText, waveFont, borderBrush, (ClientSize.Width - waveTextSize.Width) / 2 + 2, waveTextY - 2);
        g.DrawString(waveText, waveFont, borderBrush, (ClientSize.Width - waveTextSize.Width) / 2 - 2, waveTextY - 2);

        // Dibujar el texto original (negro)
        g.DrawString(waveText, waveFont, brush, (ClientSize.Width - waveTextSize.Width) / 2, waveTextY);
    }


    public void drawStatistics(Graphics g, Omar omar)
    {
        // Dibujar estadísticas en la parte superior derecha
        Font font = new Font(fontCollection.Families[0], 16, FontStyle.Bold);
        Brush whiteBrush = Brushes.White;
        Brush blackBrush = Brushes.Black;
        int lineHeight = 20;

        // Dibujar HP Regen
        string regenText = $"HP Regen: {omar.hpRegen}"; 
        SizeF regenSize = g.MeasureString(regenText, font);
        g.DrawString(regenText, font, blackBrush, ClientSize.Width - regenSize.Width - 10, 10);
        g.DrawString(regenText, font, whiteBrush, ClientSize.Width - regenSize.Width - 9, 9);

        // Dibujar daño
        string dmgText = $"Damage: {omar.damage}";
        SizeF dmgSize = g.MeasureString(dmgText, font);
        g.DrawString(dmgText, font, blackBrush, ClientSize.Width - dmgSize.Width - 10, 10 + lineHeight);
        g.DrawString(dmgText, font, whiteBrush, ClientSize.Width - dmgSize.Width - 9, 9 + lineHeight );

        // Dibujar melee
        string meleeText = $"Melee Damage: {omar.meleeDamage}";
        SizeF meleeSize = g.MeasureString(meleeText, font);
        g.DrawString(meleeText, font, blackBrush, ClientSize.Width - meleeSize.Width - 10, 10 + lineHeight * 2) ;
        g.DrawString(meleeText, font, whiteBrush, ClientSize.Width - meleeSize.Width - 9, 9 + lineHeight * 2);

        // Dibujar ranged
        string rangedText = $"Ranged Damage: {omar.rangedDamage}";
        SizeF rangedSize = g.MeasureString(rangedText, font);
        g.DrawString(rangedText, font, blackBrush, ClientSize.Width - rangedSize.Width - 10, 10 + lineHeight * 3);
        g.DrawString(rangedText, font, whiteBrush, ClientSize.Width - rangedSize.Width - 9, 9 + lineHeight * 3);

        // Dibujar elemental
        string elemText = $"Elemental Damage: {omar.elementalDamage}";
        SizeF elemSize = g.MeasureString(elemText, font);
        g.DrawString(elemText, font, blackBrush, ClientSize.Width - elemSize.Width - 10, 10 + lineHeight * 4);
        g.DrawString(elemText, font, whiteBrush, ClientSize.Width - elemSize.Width - 9, 9 + lineHeight * 4);

        // Dibujar velocidad de disparo
        string shotSpeedText = $"Shot Speed: {omar.shotSpeed}";
        SizeF shotSpeedSize = g.MeasureString(shotSpeedText, font);
        g.DrawString(shotSpeedText, font, blackBrush, ClientSize.Width - shotSpeedSize.Width - 10, 10 + lineHeight * 5);
        g.DrawString(shotSpeedText, font, whiteBrush, ClientSize.Width - shotSpeedSize.Width - 9, 9 + lineHeight * 5);

        // Dibujar velocidad
        string speedText = $"Speed: {omar.speed}";
        SizeF speedSize = g.MeasureString(speedText, font);
        Brush speedBrush = (omar.speed == omar.maxSpeed) ? Brushes.DarkRed : whiteBrush;
        g.DrawString(speedText, font, blackBrush, ClientSize.Width - speedSize.Width - 10, 10 + lineHeight * 6);
        g.DrawString(speedText, font, speedBrush, ClientSize.Width - speedSize.Width - 9, 9 + lineHeight * 6);

        // Dibujar rango
        string rangeText = $"Range: {omar.range}";
        SizeF rangeSize = g.MeasureString(rangeText, font);
        g.DrawString(rangeText, font, blackBrush, ClientSize.Width - rangeSize.Width - 10, 10 + lineHeight * 7);
        g.DrawString(rangeText, font, whiteBrush, ClientSize.Width - rangeSize.Width - 9, 9 + lineHeight * 7);

        // Dibujar armor
        string armorText = $"Armor: {omar.armor}";
        SizeF armorSize = g.MeasureString(armorText, font);
        g.DrawString(armorText, font, blackBrush, ClientSize.Width - armorSize.Width - 10, 10 + lineHeight * 8);
        g.DrawString(armorText, font, whiteBrush, ClientSize.Width - armorSize.Width - 9, 9 + lineHeight * 8);

        // Dibujar engineering
        string engText = $"Engineering: {omar.engineering}";
        SizeF engSize = g.MeasureString(engText, font);
        g.DrawString(engText, font, blackBrush, ClientSize.Width - engSize.Width - 10, 10 + lineHeight * 9);
        g.DrawString(engText, font, whiteBrush, ClientSize.Width - engSize.Width - 9, 9 + lineHeight * 9);

        // Dibujar luck
        string luckText = $"Luck: {omar.luck}";
        SizeF luckSize = g.MeasureString(luckText, font);
        g.DrawString(luckText, font, blackBrush, ClientSize.Width - luckSize.Width - 10, 10 + lineHeight * 10);
        g.DrawString(luckText, font, whiteBrush, ClientSize.Width - luckSize.Width - 9, 9 + lineHeight * 10);

        // Dibujar harvesting
        string harvestingText = $"Harvesting: {omar.harvesting}";
        SizeF harvestingSize = g.MeasureString(harvestingText, font);
        g.DrawString(harvestingText, font, blackBrush, ClientSize.Width - harvestingSize.Width - 10, 10 + lineHeight * 11);
        g.DrawString(harvestingText, font, whiteBrush, ClientSize.Width - harvestingSize.Width - 9, 9 + lineHeight * 11);
    }
    public void drawLifeBar(Graphics g, Omar omar)
    {
        int barWidth = (int)(ClientSize.Width * 0.15);
        int barHeight = 20;
        int barX = 10;
        int barY = 10;

        // Bordes y colores
        Pen borderPen = Pens.White;
        Brush backgroundBrush = Brushes.Gray;
        Brush fillBrush = Brushes.Red;

        // Cálculo de proporción de vida
        float lifePercentage = omar.hp / omar.maxHP;
        int filledWidth = (int)(barWidth * lifePercentage);

        // Dibujar fondo de la barra (gris)
        g.FillRectangle(backgroundBrush, barX, barY, barWidth, barHeight);

        // Dibujar barra llena (rojo) según el porcentaje de vida
        if (filledWidth > 0)
        {
            g.FillRectangle(fillBrush, barX, barY, filledWidth, barHeight);
        }
        g.DrawRectangle(borderPen, barX, barY, barWidth, barHeight);

        // Texto de vida (centrado)
        string hpText = $"{omar.hp:F0} / {omar.maxHP:F0}";
        using ( Font font = new Font(fontCollection.Families[0], 14, FontStyle.Bold))
        {
            // Medir el tamaño del texto
            SizeF textSize = g.MeasureString(hpText, font);
            float textX = barX + (barWidth - textSize.Width) / 2;
            float textY = barY + (barHeight - textSize.Height) / 2;

            g.DrawString(hpText, font, Brushes.Black, textX + 1, textY + 1);
            g.DrawString(hpText, font, Brushes.White, textX, textY);
        }
    }
}
