using System.Drawing;
using System.Drawing.Text;

public class LobbyScreen
{
    private PrivateFontCollection? fontCollection;
     private int selectedOption = 0; 
    private readonly string[] options = { "+3 MAX HP", "+1 Speed", "+1 Damage" };
    public void Draw(Graphics g, Size clientSize)
    {
        // Fondo
        Color semiTransparentBlack = Color.FromArgb(100, 0, 0, 0); // Alpha = 100 for low opacity
        using (Brush backgroundBrush = new SolidBrush(semiTransparentBlack))
        {
            g.FillRectangle(backgroundBrush, 0, 0, clientSize.Width, clientSize.Height);  
        }

         // Configuración de rectángulos
        int rectWidth = clientSize.Width / 4; 
        int rectHeight = clientSize.Height / 2;
        int gap = (clientSize.Width - 3 * rectWidth) / 4;
        int rectY = (clientSize.Height - rectHeight) / 2;

        fontCollection = new PrivateFontCollection();
        fontCollection.AddFontFile("font\\chewypro.otf"); 
        Font font = new Font(fontCollection.Families[0], 32, FontStyle.Bold);

        for (int i = 0; i < 3; i++)
        {
            int rectX = gap + i * (rectWidth + gap);
            Brush rectBrush = (i == selectedOption) ? Brushes.DarkBlue : Brushes.Black;

            g.FillRectangle(rectBrush, rectX, rectY, rectWidth, rectHeight);

            // Dibujar el texto en cada rectángulo
            fontCollection ??= new PrivateFontCollection();
            fontCollection.AddFontFile("font\\chewypro.otf");
            Brush textBrush = Brushes.White;

            string option = options[i];
            SizeF textSize = g.MeasureString(option, font);
            float textX = rectX + (rectWidth - textSize.Width) / 2;
            float textY = rectY + (rectHeight - textSize.Height) / 2;

            g.DrawString(option, font, textBrush, textX, textY);

            // Dibujar botón "Elegir" debajo del texto
            string buttonText = "Elegir";
            SizeF buttonSize = g.MeasureString(buttonText, font);
            float buttonX = rectX + (rectWidth - buttonSize.Width) / 2;
            float buttonY = rectY + rectHeight - buttonSize.Height - 20;

            g.DrawString(buttonText, font, textBrush, buttonX, buttonY);
        }
    }
     public void MoveSelection(int direction)
    {
        selectedOption = (selectedOption + direction + 3) % 3; 
    }

    public int GetSelectedOption()
    {
        return selectedOption;
    }
}

