public class FastEnemy : Enemy
{
    public FastEnemy(PointF position) : base(position, 25, Color.Purple)
    {
        HP = 4;
        Speed = 1.3f;
        Defense = 0;
        Damage = 2;
    }

     protected override Color GetEnemyColor()
    {
        return Color.Purple; // Color morado
    }
}