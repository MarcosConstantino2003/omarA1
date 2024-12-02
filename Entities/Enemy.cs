public abstract class Enemy : Entity
{
    public float HP { get; protected set; }
    public float Speed { get; protected set; }
    public float Defense { get; protected set; }

    public Enemy(PointF position, float size) : base(position, size)
    {
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

        int reducedDamage = (int)(damage * damageReductionFactor);

        HP = Math.Max(0, HP - reducedDamage);
    }
public abstract class Entity
{
    public PointF Position { get; protected set; }
    public float Size { get; protected set; }

    public Entity(PointF position, float size)
    {
        Position = position;
        Size = size;
    }

}
}
