using System.Drawing;

public class Omar
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Size { get; set; }

    public Omar(int x, int y, int size)
    {
        X = x;
        Y = y;
        Size = size;
    }

    // Método para dibujar a Omar
    public void Draw(Graphics g)
    {
        g.FillRectangle(Brushes.White, X, Y, Size, Size);
    }
}
