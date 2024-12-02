public class Bullet : Entity
{
    public float Speed { get; private set; }
    public float DirectionX { get; private set; }
    public float DirectionY { get; private set; }
    public int Damage { get; private set; }

    public Bullet(PointF startPosition, float directionX, float directionY, int damage)
        : base(startPosition, 6)  // Asumimos un tamaño fijo para la bala
    {
        Speed = 10; // Velocidad de la bala
        DirectionX = directionX;
        DirectionY = directionY;
        Damage = damage;
    }

    public void Update()
    {
        Position = new PointF(Position.X + DirectionX * Speed, Position.Y + DirectionY * Speed);
    }

    public bool IsCollidingWithEnemy(Enemy enemy)
    {
        return (Position.X > enemy.Position.X &&
                Position.X < enemy.Position.X + enemy.Size &&
                Position.Y > enemy.Position.Y &&
                Position.Y < enemy.Position.Y + enemy.Size);
    }

    public override void Draw(Graphics g)
    {
        g.FillEllipse(Brushes.Yellow, Position.X - Size / 2, Position.Y - Size / 2, Size, Size);
    }
}
