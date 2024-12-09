public abstract class Enemy : Entity
{
    public float HP { get; protected set; }
    public float Speed { get; protected set; }
    public float Defense { get; protected set; }
    public Color EnemyColor { get; protected set; }
     public float Damage { get; protected set; }
    public Enemy(PointF position, float size, Color color) : base(position, size)
    {
        EnemyColor = color; 
    }

    public virtual void MoveTowardsOmar(Omar omar)
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

    public virtual void TakeDamage(int damage)
    {
        float damageReductionFactor = 1 - (Defense * 0.05f); 
        if (damageReductionFactor < 0) damageReductionFactor = 0; 

        float reducedDamage = (damage * damageReductionFactor);

        HP = Math.Max(0, HP - reducedDamage);
    }

    public override void Draw(Graphics g)
    {
        // Cada enemigo puede definir su color, pero este es el código común
        Brush brush = new SolidBrush(EnemyColor); 
        Pen blackPen = new Pen(Color.Black, 2);
        g.FillRectangle(brush, Position.X, Position.Y, Size, Size);
        g.DrawRectangle(blackPen, Position.X, Position.Y, Size, Size);

        string hpText = $"HP: {HP:F1}";
        using (Font font = new Font("Arial", 10, FontStyle.Bold))
        {
            SizeF textSize = g.MeasureString(hpText, font);
            float textX = Position.X + (Size - textSize.Width) / 2;
            float textY = Position.Y - textSize.Height - 5;
            g.DrawString(hpText, font, Brushes.White, textX, textY);
        }
    }

    protected abstract Color GetEnemyColor();
}
