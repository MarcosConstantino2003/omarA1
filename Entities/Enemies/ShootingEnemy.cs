public class ShootingEnemy : Enemy
{
    private float shootCooldown;
    private float shootCooldownMax = 12.0f;
    public List<Bullet> bullets;

    public ShootingEnemy(PointF position) : base(position, 30, Color.DarkBlue)
    {
        HP = 4;
        Speed = 0.6f;
        Defense = 0.2f;
        Damage = 2;
        bullets = new List<Bullet>();
        shootCooldown = 0; 
    }

    // Método para disparar balas hacia Omar
    public void Shoot(List<Bullet> bullets, Omar omar)
    {
        float dx = omar.X - Position.X;
        float dy = omar.Y - Position.Y;
        float distance = (float)Math.Sqrt(dx * dx + dy * dy);

        float directionX = dx / distance;
        float directionY = dy / distance;

        float bulletStartX = Position.X + directionX * 15;
        float bulletStartY = Position.Y + directionY * 15;

        bullets.Add(new EnemyBullet(new PointF(bulletStartX, bulletStartY), directionX, directionY, (int)Damage));
    }

    protected override Color GetEnemyColor()
    {
        return Color.Blue; 
    }


    public override void act(List<Bullet> bullets, Omar omar)
    {
        base.MoveTowardsOmar(omar); // Mover al enemigo hacia Omar
        shootCooldown += 0.1f; 

        if (shootCooldown >= shootCooldownMax)
        {
            Shoot(bullets, omar);
            shootCooldown = 0;
        }
    }
}
