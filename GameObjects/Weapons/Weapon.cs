public abstract class Weapon
{
    public Omar owner { get; set; }
    public int baseDamage { get; set; }
    public float baseShotSpeed { get; set; }
    public float baseRange { get; set; }

    public int damage { get; set; }
    public float shotSpeed { get; set; }
    public float range { get; set; }
    public Enemy? closestEnemy { get; set; }

    public Weapon(Omar owner)
    {
        this.owner = owner;
    }

    // Método de disparo: lo implementarán las armas específicas
    public abstract void Shoot(List<Bullet> bullets, Enemy closestEnemy);

    // Método de dibujo: lo implementarán las armas específicas
    public abstract void Draw(Graphics g);
     public  Enemy? GetClosestEnemy(List<Enemy> enemies)
    {
        Enemy? closestEnemy = null;
        float closestDistance = float.MaxValue;

        // Buscar el enemigo más cercano basado en la posición de la arma (o del dueño)
        foreach (var enemy in enemies)
        {
            float dx = enemy.Position.X - owner.X;
            float dy = enemy.Position.Y - owner.Y;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

}
