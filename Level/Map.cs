public class Map
{
    private List<Diamond> diamonds;
    private List<Enemy> enemies;
    private List<Enemy> enemiesToRemove;
    private List<Heart> hearts;
    private List<Bullet> bullets;
    private List<Bullet> bulletsToRemove;
    private Random random;

    private Omar omar;
    private Spawner spawner;
    private CollisionHandler collisionHandler;

    private System.Windows.Forms.Timer shootTimer;

    public Map(Omar omar)
    {
        this.omar = omar;
        diamonds = new List<Diamond>();
        enemies = new List<Enemy>();
        enemiesToRemove = new List<Enemy>();
        hearts = new List<Heart>(); 
        bullets = new List<Bullet>();
        bulletsToRemove = new List<Bullet>();
        random = new Random();

        spawner = new Spawner(omar, diamonds, enemies, hearts);
        collisionHandler = new CollisionHandler(omar, diamonds, enemies, hearts);
        shootTimer = new System.Windows.Forms.Timer();
        shootTimer.Interval =  omar.GetShootDelay(); 
        shootTimer.Tick += (sender, e) =>
        {
            var closestEnemy = GetClosestEnemy();
            if (closestEnemy != null)
            {
                omar.Shoot(bullets, closestEnemy); 
            }
        };        
        shootTimer.Start();
    }


    // Método para encontrar el enemigo más cercano a Omar
    public Enemy? GetClosestEnemy()
    {
        Enemy? closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (var enemy in enemies)
        {
            float dx = enemy.Position.X - omar.X;
            float dy = enemy.Position.Y - omar.Y;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    public void update(){
        collisionHandler.Update();
        omar.UpdatePosition();
        UpdateEnemies();
        UpdateBullets();
        UpdateDiamonds();
        UpdateShootSpeed();
    }

    public void UpdateShootSpeed()
    {
        shootTimer.Interval = omar.GetShootDelay(); 
    }

    public void UpdateEnemies()
    {
        foreach (var enemy in enemies)
        {
            enemy.MoveTowardsOmar(omar);
            if (enemy.HP == 0)
            {
                enemiesToRemove.Add(enemy);
            }
        }
        foreach (var enemy in enemiesToRemove)
    {
        enemies.Remove(enemy);
    }
    }
    public void UpdateBullets()
    {
        foreach (var bullet in bullets)
        {
            bullet.Update();
        
        foreach (var enemy in enemies)
        {
            if (bullet.IsCollidingWithEnemy(enemy))
            {
                enemy.TakeDamage(bullet.Damage); 
                bulletsToRemove.Add(bullet);    
                break;
            }
        }
    }
        foreach (var bullet in bulletsToRemove)
        {
            bullets.Remove(bullet);
        }
    }

    public void UpdateDiamonds()
    {
        diamonds.RemoveAll(diamond => diamond.IsExpired());
        hearts.RemoveAll(heart=> heart.IsExpired());
    }

    public void ClearObjects()
    {
        enemies.Clear();
        diamonds.Clear();
        hearts.Clear();
        spawner.ResetTimers();
    }   

    // Método para dibujar los rombos en el gráfico
    public void Draw(Graphics g)
    {
        //diamonds
        foreach (var diamond in diamonds)
        {
            diamond.Draw(g);
        }
        //enemies y spawnmarks
        spawner.DrawSpawnMarkers(g); 
         foreach (var enemy in enemies)
        {
            enemy.Draw(g); 
        }
        //hearts
        foreach (var heart in hearts)
        {
            heart.Draw(g);
        }
        //gun
        Enemy? closestEnemy = GetClosestEnemy();
        if (closestEnemy != null)
        {
            omar.DrawTriangle(g, closestEnemy);
        }
        //balas
        foreach (var bullet in bullets)
        {
            bullet.Draw(g);
        }
        //texto flotante
        string floatingText = collisionHandler.GetFloatingText();
        if (!string.IsNullOrEmpty(floatingText))
        {
            Font font = new Font("Arial", 12, FontStyle.Bold);
            Brush textBrush = Brushes.Yellow;
            Brush shadowBrush = Brushes.Black;

            // Medir el tamaño del texto
            SizeF textSize = g.MeasureString(floatingText, font);

            // Calcular posición del texto centrado sobre Omar
            PointF textPosition = new PointF(
                omar.X  - (textSize.Width / 2), // Centrado en X
                omar.Y - omar.Size // Justo encima en Y
            );

            // Sombra negra para el texto
            g.DrawString(floatingText, font, shadowBrush, textPosition.X + 1, textPosition.Y + 1);
            // Texto principal en amarillo
            g.DrawString(floatingText, font, textBrush, textPosition);
        }
    }
  
}
