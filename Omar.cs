public class Omar
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Size { get; set; }
    public float Speed { get; set; }

    public Omar(float x, float y, float size)
    {
        X = x;
        Y = y;
        Size = size;
        Speed = 2; // Velocidad inicial de Omar
    }

    public void Move(float deltaX, float deltaY)
    {
        X += deltaX * Speed;
        Y += deltaY * Speed;
    }

    // Método para verificar la colisión con un rombo
    public bool IsCollidingWithDiamond(Diamond diamond)
    {
        // Calculamos la distancia entre el centro de Omar y el centro del rombo
        float dx = X - diamond.Position.X;
        float dy = Y - diamond.Position.Y;
        float distance = (float)Math.Sqrt(dx * dx + dy * dy);

        // Si la distancia es menor que la suma de los radios (considerando el tamaño del rombo y de Omar)
        return distance < (Size / 2 + diamond.Size / 2);
    }

    public void Draw(Graphics g)
    {
        Brush brush = Brushes.White;
        Pen blackPen = new Pen(Color.Black, 2);

        // Dibujar Omar como un círculo
        g.FillEllipse(brush, X - Size / 2, Y - Size / 2, Size, Size);
        g.DrawEllipse(blackPen, X - Size / 2, Y - Size / 2, Size, Size);
    }
}
