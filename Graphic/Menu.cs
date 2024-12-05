using System.Drawing;
using System.Drawing.Text;

public class Menu
{
    private int selectedIndex; // Índice de la opción seleccionada
    private string[] options; // Opciones del menú
    private Font font;
    private Brush normalBrush;
    private Brush selectedBrush;
    private Image logoImage; // Para cargar la imagen del logo

    private float logoMaxWidth = 600f;
    private float logoMaxHeight = 200f; 
    private PrivateFontCollection fontCollection;
    public Menu()
    {
        options = new string[] { "Jugar", "Opciones", "Salir" };
        selectedIndex = 0; 
        normalBrush = Brushes.Black; 
        selectedBrush = Brushes.Red; 
        logoImage = Image.FromFile("img/mainlogo.png");
        fontCollection = new PrivateFontCollection();
        fontCollection.AddFontFile("font\\chewypro.otf"); 
        font = new Font(fontCollection.Families[0], 32, FontStyle.Bold);
    }

    public void Draw(Graphics g, Size clientSize)
    {
        float ratio = Math.Min(logoMaxWidth / logoImage.Width, logoMaxHeight / logoImage.Height);
        int newWidth = (int)(logoImage.Width * ratio);
        int newHeight = (int)(logoImage.Height * ratio);

        float logoX = (clientSize.Width - newWidth) / 2; // Centrado horizontal
        float logoY = clientSize.Height / 5; // Distancia desde el borde superior

        g.DrawImage(logoImage, logoX, logoY, newWidth, newHeight);

        // Dibujar las opciones del menú
        for (int i = 0; i < options.Length; i++)
        {
            string text = options[i];
            Brush brush = (i == selectedIndex) ? selectedBrush : normalBrush;

            // Calcular la posición centrada para las opciones
            SizeF textSize = g.MeasureString(text, font);
            float x = (clientSize.Width - textSize.Width) / 2;
            float y = (clientSize.Height - clientSize.Height/3 ) + i * (textSize.Height + 10);

            // Dibujar cada opción
            g.DrawString(text, font, brush, x, y);
        }
    }

    public void MoveUp()
    {
        selectedIndex = (selectedIndex > 0) ? selectedIndex - 1 : options.Length - 1;
    }

    public void MoveDown()
    {
        selectedIndex = (selectedIndex < options.Length - 1) ? selectedIndex + 1 : 0;
    }

    public string GetSelectedOption()
    {
        return options[selectedIndex];
    }
}
