public class Bullet
{
    public PointF Position { get; private set; }
    public float Speed { get; private set; }
    public float DirectionX { get; private set; }
    public float DirectionY { get; private set; }
    public int Damage { get; private set; }

    public Bullet(PointF startPosition, float directionX, float directionY, int damage)
    {
        Position = startPosition;
        Speed = 10; // Velocidad de la bala
        DirectionX = directionX;
        DirectionY = directionY;
        Damage = damage;
    }

    public void Update()
    {
        // Mover la bala en la dirección especificada
        Position = new PointF(Position.X + DirectionX * Speed, Position.Y + DirectionY * Speed);
    }

    public void Draw(Graphics g)
    {
        // Dibujar la bala como un pequeño círculo
        g.FillEllipse(Brushes.Yellow, Position.X - 3, Position.Y - 3, 6, 6);
    }
}
