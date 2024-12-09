public class Heart : Entity
{
    public Color Color { get; set; }
    private TimeSpan PausedTime { get; set; } = TimeSpan.Zero;
    private DateTime? PauseStartTime { get; set; } = null;

    public Heart(PointF position, Color color, float size)
        : base(position, size)
    {
        Color = color;
    }

    public bool IsExpired()
    {
        TimeSpan totalPaused = PausedTime;

        // Si actualmente está en pausa, considerar la pausa en curso
        if (PauseStartTime.HasValue)
        {
            totalPaused += DateTime.Now - PauseStartTime.Value;
        }

        // Verificar si ha pasado suficiente tiempo sin incluir las pausas
        return (DateTime.Now - CreationTime - totalPaused).TotalSeconds >= 11;
    }

    public void Pause()
    {
        if (!PauseStartTime.HasValue)
        {
            PauseStartTime = DateTime.Now;
        }
    }

    public void Resume()
    {
        if (PauseStartTime.HasValue)
        {
            PausedTime += DateTime.Now - PauseStartTime.Value;
            PauseStartTime = null;
        }
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
}
