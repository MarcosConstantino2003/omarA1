public abstract class Entity
{
    public PointF Position { get; set; }
    public float Size { get; set; }
    public DateTime CreationTime { get; } // Fecha de creación

    public Entity(PointF position, float size)
    {
        Position = position;
        Size = size;
        CreationTime = DateTime.Now; // Establecer el tiempo de creación
    }

    public bool IsExpired(double seconds)
    {
        return (DateTime.Now - CreationTime).TotalSeconds >= seconds;
    }

    public abstract void Draw(Graphics g);
}
