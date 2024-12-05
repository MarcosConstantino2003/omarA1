using System;
using System.Drawing;
using System.Drawing.Text;
public class GameOverScreen
{
    private PrivateFontCollection? fontCollection;
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
        fontCollection = new PrivateFontCollection();
        fontCollection.AddFontFile("font\\chewypro.otf"); 
        Font titleFont = new Font(fontCollection.Families[0], 36, FontStyle.Bold);
        Font optionsFont = new Font(fontCollection.Families[0], 24, FontStyle.Regular);
        Brush titleBrush = Brushes.Red;
        Brush optionsBrush = Brushes.White;
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
