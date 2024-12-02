public class CollisionHandler
{
    private Omar omar;
    private List<Diamond> diamonds;
    private List<Enemy> enemies;
    private List<Heart> hearts;
    private string floatingText;
    private PointF floatingTextPosition;
    private System.Windows.Forms.Timer floatingTextTimer;

    public CollisionHandler(Omar omar, List<Diamond> diamonds, List<Enemy> enemies, List<Heart> hearts)
    {
        this.omar = omar;
        this.diamonds = diamonds;
        this.enemies = enemies;
        this.hearts = hearts;
        floatingText = "";
        floatingTextPosition = PointF.Empty;
        floatingTextTimer = new System.Windows.Forms.Timer();;
    }

    public void CheckCollisions()
    {
        // Colisiones con diamantes
        foreach (var diamond in diamonds.ToList())
        {
            if (omar.IsCollidingWithDiamond(diamond))
            {
                HandleDiamondCollision(diamond);
                diamonds.Remove(diamond);
                break;
            }
        }

        // Colisiones con enemigos
        foreach (var enemy in enemies.ToList())
        {
            if (omar.IsCollidingWithEnemy(enemy))
            {
                omar.DecreaseHP(3);
                break; 
            }
        }

        // Colisiones con corazones
        foreach (var heart in hearts.ToList())
        {
            if (omar.IsCollidingWithHeart(heart))
            {
                HandleHeartCollision(heart);
                hearts.Remove(heart);
                break; // Solo se aplica la primera colisión
            }
        }
    }

    private void HandleDiamondCollision(Diamond diamond)
    {
        string effectText = "";

        if (diamond.Color == Color.Green)
        {
            omar.IncreaseSpeed(1);
            effectText = "+1 Speed";
        }
        else if (diamond.Color == Color.Cyan)
        {
            omar.IncreaseShotSpeed(2);
            effectText = "+2 Shot Speed";
        }
        else if (diamond.Color == Color.Black)
        {
            omar.IncreaseDamage(1);
            effectText = "+1 Damage";
        }
        else if (diamond.Color == Color.Purple)
        {
            omar.range += 20;
            effectText = "+20 Range";
        }

        // Establecer el texto flotante y su posición
        floatingText = effectText;
        floatingTextPosition = new PointF(omar.X, omar.Y - 20);

        // Temporizador para eliminar el texto flotante
        if (floatingTextTimer != null)
        {
            floatingTextTimer.Stop();
            floatingTextTimer.Dispose();
        }

        floatingTextTimer = new System.Windows.Forms.Timer();
        floatingTextTimer.Interval = 2000; // 2 segundos
        floatingTextTimer.Tick += (sender, e) =>
        {
            floatingText = "";
            floatingTextTimer.Stop();
            floatingTextTimer.Dispose();
        };
        floatingTextTimer.Start();
    }

    private void HandleHeartCollision(Heart heart)
    {
        omar.IncreaseHP(4); // Aumenta 4 HP por corazón recogido
        floatingText = "+4 HP";

        // Temporizador para eliminar el texto flotante
        if (floatingTextTimer != null)
        {
            floatingTextTimer.Stop();
            floatingTextTimer.Dispose();
        }

        floatingTextTimer = new System.Windows.Forms.Timer();
        floatingTextTimer.Interval = 2000; // 2 segundos
        floatingTextTimer.Tick += (sender, e) =>
        {
            floatingText = "";
            floatingTextTimer.Stop();
            floatingTextTimer.Dispose();
        };
        floatingTextTimer.Start();
    }

    public void Update()
    {
        CheckCollisions();
    }

    public string GetFloatingText()
    {
        return floatingText;
    }

    public PointF GetFloatingTextPosition()
    {
        return floatingTextPosition;
    }
}
