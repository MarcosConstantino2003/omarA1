public class EnemyBullet : Bullet
{
    public EnemyBullet(PointF startPosition, float directionX, float directionY, int damage)
        : base(startPosition, directionX, directionY, damage, 4f) 
    {
    }

    public override void Draw(Graphics g)
    {
        g.FillEllipse(Brushes.White, Position.X - Size / 2, Position.Y - Size / 2, Size, Size);
        g.DrawEllipse(Pens.Red, Position.X - Size / 2, Position.Y - Size / 2, Size, Size);
   
    }

}
