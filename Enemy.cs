public class Enemy
{
    public PointF Position { get; set; }
    public Color Color { get; set; }
    public float Size { get; set; }
    public float Speed { get; set; } // Velocidad de movimiento del enemigo


    public Enemy(PointF position, Color color, float size)
    {
        Position = position;
        Color = color;
        Size = size;
        Speed = 1.0f;
    }

    public void MoveTowardsOmar(Omar omar)
    {
        // Calcular la diferencia de posición entre el enemigo y Omar
        float deltaX = omar.X - Position.X;
        float deltaY = omar.Y - Position.Y;

        // Calcular la longitud del vector (distancia)
        float magnitude = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

        // Si no estamos en la misma posición, normalizamos y movemos
        if (magnitude != 0)
        {
            // Normalizamos el vector de dirección
            deltaX /= magnitude;
            deltaY /= magnitude;

            // Movemos al enemigo en esa dirección, multiplicado por la velocidad
            Position = new PointF(Position.X + deltaX * Speed, Position.Y + deltaY * Speed);
        }
    }
    public void Draw(Graphics g)
    {
        Brush brush = new SolidBrush(Color);
        Pen blackPen = new Pen(Color.Black, 2);

        // Dibujar el enemigo como un cuadrado
        g.FillRectangle(brush, Position.X, Position.Y, Size, Size);

        // Dibujar borde negro
        g.DrawRectangle(blackPen, Position.X, Position.Y, Size, Size);
    }
}
