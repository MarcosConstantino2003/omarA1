public abstract class RangedWeapon : Weapon
{
    public RangedWeapon(Omar owner) : base(owner) {
        CalculateStats();
    }

    public void CalculateStats()
    {
        // Calculamos las estadísticas específicas de las armas de rango
        damage = baseDamage + owner.rangedDamage; // Si es un arma de rango, calculamos daño de manera combinada
        shotSpeed = baseShotSpeed + owner.shotSpeed; // Velocidad de disparo basada en stats de Omar
        range = baseRange + owner.range; // Rango del arma
    }

    // Disparo: la lógica de disparo es la misma para todas las armas de rango
    public override void Shoot(List<Bullet> bullets, Enemy closestEnemy)
    {
        Console.WriteLine("base:" + baseDamage);
        Console.WriteLine("damage:" + damage);
        CalculateStats();
        if (closestEnemy == null) return; // Si no hay enemigos cercanos, no hacer nada

        // Calcular la dirección hacia el enemigo más cercano
        float triangleBaseX = owner.X + 30;
        float triangleBaseY = owner.Y - 30;
        float dx = closestEnemy.Position.X - triangleBaseX;
        float dy = closestEnemy.Position.Y - triangleBaseY;
        float distance = (float)Math.Sqrt(dx * dx + dy * dy);

        if (distance > range) return; // Si el enemigo está fuera del rango, no disparar

        // Calcular el ángulo hacia el enemigo
        float angle = (float)Math.Atan2(dy, dx); // Ángulo de la línea entre el enemigo y la punta del arma
        float adjustedAngle = angle - 0.05f; // Ajuste del ángulo (por ejemplo, 5 grados hacia la izquierda)

        // Ajustar la posición inicial de la bala en la punta del triángulo
        float bulletStartX = triangleBaseX + (float)Math.Cos(adjustedAngle) * 10;
        float bulletStartY = triangleBaseY + (float)Math.Sin(adjustedAngle) * 10;

        // Calcular la dirección normalizada con el ángulo ajustado
        float directionX = (float)Math.Cos(adjustedAngle);
        float directionY = (float)Math.Sin(adjustedAngle);

        // Crear la bala con la posición de inicio ajustada y la dirección calculada
        bullets.Add(new OmarBullet(new PointF(bulletStartX, bulletStartY), directionX, directionY, damage));
    }

    public override void Draw(Graphics g)
    {

    }
}
