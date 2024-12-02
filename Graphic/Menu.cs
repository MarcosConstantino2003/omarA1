using System.Drawing;

public class Menu
{
    private int selectedIndex; // Índice de la opción seleccionada
    private string[] options; // Opciones del menú
    private Font font;
    private Brush normalBrush;
    private Brush selectedBrush;

    public Menu()
    {
        // Inicializar las opciones del menú
        options = new string[] { "Jugar", "Opciones", "Salir" };
        selectedIndex = 0; // Seleccionar la primera opción por defecto
        font = new Font("Arial", 24, FontStyle.Bold);
        normalBrush = Brushes.Black; // Color normal
        selectedBrush = Brushes.Red; // Color de la opción seleccionada
    }

    public void Draw(Graphics g, Size clientSize)
    {
        for (int i = 0; i < options.Length; i++)
        {
            string text = options[i];
            Brush brush = (i == selectedIndex) ? selectedBrush : normalBrush;

            // Calcular la posición centrada
            SizeF textSize = g.MeasureString(text, font);
            float x = (clientSize.Width - textSize.Width) / 2;
            float y = (clientSize.Height / 2 - (options.Length * textSize.Height) / 2) + i * (textSize.Height + 10);

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
