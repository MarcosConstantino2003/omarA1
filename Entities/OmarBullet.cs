public class OmarBullet : Bullet
{
    public OmarBullet(PointF startPosition, float directionX, float directionY, int damage)
        : base(startPosition, directionX, directionY, damage, 16f)  // Velocidad diferente para las balas de Omar
    {
    }

    public override void Draw(Graphics g)
    {
        g.FillEllipse(Brushes.Yellow, Position.X - Size / 2, Position.Y - Size / 2, Size, Size);
    }
}
