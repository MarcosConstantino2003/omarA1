using System;
using System.Drawing;

public class GameOverScreen
{
    private string[] options = { "Reintentar", "Menu Principal" };
    private int selectedOption = 0;

    public void MoveUp()
    {
        selectedOption = (selectedOption - 1 + options.Length) % options.Length;
    }

    public void MoveDown()
    {
        selectedOption = (selectedOption + 1) % options.Length;
    }

    public string GetSelectedOption()
    {
        return options[selectedOption];
    }

    public void Draw(Graphics g, Size clientSize)
    {
        string title = "GAME OVER";
        Font titleFont = new Font("Arial", 36, FontStyle.Bold);
        Font optionsFont = new Font("Arial", 24, FontStyle.Regular);

        Brush titleBrush = Brushes.Red;
        Brush optionsBrush = Brushes.Black;
        Brush selectedBrush = Brushes.DarkRed;

        // Dibujar el título centrado
        SizeF titleSize = g.MeasureString(title, titleFont);
        float titleX = (clientSize.Width - titleSize.Width) / 2;
        float titleY = clientSize.Height / 4;
        g.DrawString(title, titleFont, titleBrush, titleX, titleY);

        // Dibujar las opciones
        float startY = titleY + titleSize.Height + 50;
        for (int i = 0; i < options.Length; i++)
        {
            string option = options[i];
            Brush brush = (i == selectedOption) ? selectedBrush : optionsBrush;

            SizeF optionSize = g.MeasureString(option, optionsFont);
            float optionX = (clientSize.Width - optionSize.Width) / 2;
            float optionY = startY + i * 50;

            g.DrawString(option, optionsFont, brush, optionX, optionY);
        }
    }
}
