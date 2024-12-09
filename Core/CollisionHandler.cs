public class CollisionHandler
{
    private Omar omar;
    private List<Diamond> diamonds;
    private List<Enemy> enemies;
    private List<Heart> hearts;
    private List<Bullet> bullets;
    private string floatingText;
    private PointF floatingTextPosition;
    private System.Windows.Forms.Timer floatingTextTimer;
    public Brush floatingTextColor { get; set; }

    public CollisionHandler(Omar omar, List<Diamond> diamonds, List<Enemy> enemies, List<Heart> hearts, List<Bullet> bullets)
    {
        this.omar = omar;
        this.diamonds = diamonds;
        this.enemies = enemies;
        this.hearts = hearts;
        this.bullets = bullets;
        floatingText = "";
        floatingTextPosition = PointF.Empty;
        floatingTextTimer = new System.Windows.Forms.Timer(); ;
        floatingTextColor = Brushes.Yellow;
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
                if (omar.getisInvulnerable())
                {
                    return;
                }
                int damageTaken = omar.takeDamage(enemy.Damage);
                floatingText = $"-{damageTaken} HP";
                floatingTextColor = Brushes.Red;
                assignFloatingText();
            }
        }

        // Colisiones con corazones
        foreach (var heart in hearts.ToList())
        {
            if (omar.IsCollidingWithHeart(heart))
            {
                HandleHeartCollision(heart);
                hearts.Remove(heart);
                break;
            }
        }

        CheckBulletCollisions();
    }

     public void CheckBulletCollisions()
    {
        List<Bullet> bulletsToRemove = new List<Bullet>();

        // Revisión de las balas
        foreach (var bullet in bullets)
        {
            bullet.Update();

            if (bullet is EnemyBullet)
            {
                if (bullet.IsCollidingWithOmar(omar))
                {
                    omar.takeDamage(bullet.Damage);
                    floatingText = $"-{bullet.Damage} HP";
                    floatingTextColor = Brushes.Red;
                    assignFloatingText();
                    bulletsToRemove.Add(bullet); // Eliminar bala
                }
            }
            else if (bullet is OmarBullet)
            {
                foreach (var enemy in enemies)
                {
                    if (bullet.IsCollidingWithEnemy(enemy))
                    {
                        enemy.TakeDamage(bullet.Damage);
                        bulletsToRemove.Add(bullet); // Eliminar bala
                        break;
                    }
                }
            }
        }

        // Eliminar las balas que han colisionado
        foreach (var bullet in bulletsToRemove)
        {
            bullets.Remove(bullet);
        }
    }

    private void HandleDiamondCollision(Diamond diamond)
    {
        string effectText = "";

        if (diamond.Color == Color.Green)
        {
            omar.changeSpeed(1);
            effectText = "+1 Speed";
        }
        else if (diamond.Color == Color.Cyan)
        {
            omar.changeShotSpeed(2);
            effectText = "+2 Shot Speed";
        }
        else if (diamond.Color == Color.Black)
        {
            omar.changeDamage(1);
            effectText = "+1 Damage";
        }
        else if (diamond.Color == Color.Purple)
        {
            omar.range += 20;
            effectText = "+20 Range";
        }
        else if (diamond.Color == Color.Orange)
        {
            omar.changeHPRegen(1);
            effectText = "+1 HP Regen";
        }

        floatingText = effectText;
        floatingTextColor = Brushes.Yellow;
        assignFloatingText();
    }

    private void HandleHeartCollision(Heart heart)
    {
        omar.IncreaseHP(4); // Aumenta 4 HP por corazón recogido
        floatingText = "+4 HP";
        floatingTextColor = Brushes.Yellow;
        assignFloatingText();
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

    private void assignFloatingText()
    {
        floatingTextPosition = new PointF(omar.X, omar.Y - 20);
        if (floatingTextTimer != null)
        {
            floatingTextTimer.Stop();
            floatingTextTimer.Dispose();
        }

        floatingTextTimer = new System.Windows.Forms.Timer();
        floatingTextTimer.Interval = 2000;
        floatingTextTimer.Tick += (sender, e) =>
        {
            floatingText = "";
            floatingTextTimer.Stop();
            floatingTextTimer.Dispose();
        };
        floatingTextTimer.Start();
    }
}
