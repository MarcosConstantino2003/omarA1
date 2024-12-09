public abstract class Bullet : Entity
{
    public float Speed { get; protected set; }
    public float DirectionX { get; protected set; }
    public float DirectionY { get; protected set; }
    public int Damage { get; protected set; }

    protected Bullet(PointF startPosition, float directionX, float directionY, int damage, float speed)
        : base(startPosition, 6)  // Asumimos un tamaño fijo para la bala
    {
        DirectionX = directionX;
        DirectionY = directionY;
        Damage = damage;
        Speed = speed;
    }

    public void Update()
    {
        Position = new PointF(Position.X + DirectionX * Speed, Position.Y + DirectionY * Speed);
    }


    // Método para comprobar la colisión con un enemigo
    public bool IsCollidingWithEnemy(Enemy enemy)
    {
        return (Position.X > enemy.Position.X &&
                Position.X < enemy.Position.X + enemy.Size &&
                Position.Y > enemy.Position.Y &&
                Position.Y < enemy.Position.Y + enemy.Size);
    }

    public bool IsCollidingWithOmar(Omar omar)
    {
        // Ajusta la hitbox de la bala moviéndola a la izquierda y hacia arriba
        float adjustedX = Position.X + omar.Size/2;
        float adjustedY = Position.Y + omar.Size/2;

        // Realiza la colisión usando la hitbox ajustada
        return (adjustedX + Size > omar.X &&
                adjustedX < omar.X + omar.Size &&
                adjustedY + Size > omar.Y &&
                adjustedY < omar.Y + omar.Size);
    }
}

