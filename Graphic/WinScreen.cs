using System.Drawing;

public class WinScreen
{
    public void Draw(Graphics g, Size clientSize)
    {
        // Fondo
        g.Clear(Color.LightGreen);

        // Texto principal
        string message = "¡Victoria!";
        Font font = new Font("Arial", 36, FontStyle.Bold);
        Brush brush = Brushes.Black;

        SizeF messageSize = g.MeasureString(message, font);
        float x = (clientSize.Width - messageSize.Width) / 2;
        float y = (clientSize.Height - messageSize.Height) / 3;

        g.DrawString(message, font, brush, x, y);

        // Botón de ir al menú
        string buttonText = "Ir al menú";
        Font buttonFont = new Font("Arial", 24, FontStyle.Regular);
        Brush buttonBrush = Brushes.White;
        Brush buttonBackground = Brushes.Black;

        SizeF buttonSize = g.MeasureString(buttonText, buttonFont);
        float buttonX = (clientSize.Width - buttonSize.Width) / 2;
        float buttonY = y + messageSize.Height + 50;

        g.FillRectangle(buttonBackground, buttonX - 10, buttonY - 10, buttonSize.Width + 20, buttonSize.Height + 20);
        g.DrawString(buttonText, buttonFont, buttonBrush, buttonX, buttonY);
    }
}
