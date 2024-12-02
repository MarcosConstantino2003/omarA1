public class Enemy : Entity
{
    public Color Color { get; set; }
    public float Speed { get; set; }
    public int HP { get; private set; }

    public Enemy(PointF position, Color color, float size)
        : base(position, size)
    {
        Color = color;
        Speed = 1.0f;
        HP = 5;
    }

    public void MoveTowardsOmar(Omar omar)
    {
        float deltaX = omar.X - Position.X;
        float deltaY = omar.Y - Position.Y;
        float magnitude = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

        if (magnitude != 0)
        {
            deltaX /= magnitude;
            deltaY /= magnitude;
            Position = new PointF(Position.X + deltaX * Speed, Position.Y + deltaY * Speed);
        }
    }

    public void TakeDamage(int damage)
    {
        HP = Math.Max(0, HP - damage);
    }

    public override void Draw(Graphics g)
    {
        Brush brush = new SolidBrush(Color);
        Pen blackPen = new Pen(Color.Black, 2);
        g.FillRectangle(brush, Position.X, Position.Y, Size, Size);
        g.DrawRectangle(blackPen, Position.X, Position.Y, Size, Size);

        string hpText = $"HP: {HP}";
        using (Font font = new Font("Arial", 10, FontStyle.Bold))
        {
            SizeF textSize = g.MeasureString(hpText, font);
            float textX = Position.X + (Size - textSize.Width) / 2;
            float textY = Position.Y - textSize.Height - 5;
            g.DrawString(hpText, font, Brushes.White, textX, textY);
        }
    }
}
