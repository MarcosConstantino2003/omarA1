public class BasicEnemy : Enemy
{
    public BasicEnemy(PointF position) : base(position, 30, Color.Brown)
    {
        HP = 5;
        Speed = 1.0f;
        Defense = 0;
    }

    protected override Color GetEnemyColor()
    {
        return Color.Brown; // Marrón
    }
}