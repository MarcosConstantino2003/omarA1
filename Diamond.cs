public class Diamond
{
    public PointF Position { get; set; }
    public Color Color { get; set; }
    public float Size { get; set; }

    public Diamond(PointF position, Color color, float size)
    {
        Position = position;
        Color = color;
        Size = size;
    }

    public void Draw(Graphics g)
    {
        Brush brush = new SolidBrush(Color);
        Pen blackPen = new Pen(Color.Black, 2);

        // Dibujar el rombo
        g.FillPolygon(brush, new PointF[]
        {
            new PointF(Position.X + Size / 2, Position.Y),
            new PointF(Position.X + Size, Position.Y + Size / 2),
            new PointF(Position.X + Size / 2, Position.Y + Size),
            new PointF(Position.X, Position.Y + Size / 2)
        });

        // Dibujar borde negro
        g.DrawPolygon(blackPen, new PointF[]
        {
            new PointF(Position.X + Size / 2, Position.Y),
            new PointF(Position.X + Size, Position.Y + Size / 2),
            new PointF(Position.X + Size / 2, Position.Y + Size),
            new PointF(Position.X, Position.Y + Size / 2)
        });
    }
}
