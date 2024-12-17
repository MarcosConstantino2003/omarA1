public class MeleeWeapon : Weapon
{
    public MeleeWeapon(Omar owner) : base(owner)
    {
        baseDamage = 8; // Ejemplo de daño base
        baseShotSpeed = 0f; // Las armas cuerpo a cuerpo no tienen disparo
        baseRange = 50; // Ejemplo de rango de alcance cuerpo a cuerpo
    }

    public override void Shoot(List<Bullet> bullets, Enemy closestEnemy)
    {
        // Para las armas cuerpo a cuerpo no disparamos balas, solo se realiza un ataque de daño cercano
        if (closestEnemy != null)
        {
            // Lógica de ataque cuerpo a cuerpo
            closestEnemy.TakeDamage(damage);
        }
    }

    public override void Draw(Graphics g)
    {
        // Implementación vacía para dibujo
    }
}