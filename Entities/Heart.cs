public class Heart : Entity
{
    public Color Color { get; set; }

    public Heart(PointF position, Color color, float size)
        : base(position, size)
    {
        Color = color;
    }

    public override void Draw(Graphics g)
    {
        Brush brush = new SolidBrush(Color);
        Pen blackPen = new Pen(Color.Black, 2);

        g.FillPolygon(brush, new PointF[] 
        {
            new PointF(Position.X, Position.Y + Size / 3),
            new PointF(Position.X + Size / 2, Position.Y),
            new PointF(Position.X + Size, Position.Y + Size / 3),
            new PointF(Position.X + Size / 2, Position.Y + Size),
            new PointF(Position.X, Position.Y + Size / 3)
        });

        g.DrawPolygon(blackPen, new PointF[] 
        {
            new PointF(Position.X, Position.Y + Size / 3),
            new PointF(Position.X + Size / 2, Position.Y),
            new PointF(Position.X + Size, Position.Y + Size / 3),
            new PointF(Position.X + Size / 2, Position.Y + Size),
            new PointF(Position.X, Position.Y + Size / 3)
        });
    }
    public bool IsExpired()
    {
        return (DateTime.Now - CreationTime).TotalSeconds >= 11;
    }
}