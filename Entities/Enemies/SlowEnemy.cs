public class SlowEnemy : Enemy
{
    public SlowEnemy(PointF position) : base(position, 30, Color.FromArgb(139, 69, 19))
    {
        HP = 7;
        Speed = 0.75f;
        Defense = 2;
    }

    protected override Color GetEnemyColor()
    {
        return Color.FromArgb(139, 69, 19); // Marrón oscuro
    }
}
