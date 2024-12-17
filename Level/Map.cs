public class Map
{
    public List<Diamond> diamonds;
    public List<Enemy> enemies;
    private List<Enemy> enemiesToRemove;
    public List<Heart> hearts;
    private List<Bullet> bullets;
    private List<Bullet> bulletsToRemove;
    private Random random;
    private Omar omar;
    private Spawner spawner;
    private CollisionHandler collisionHandler;
    private System.Windows.Forms.Timer shootTimer;

    public Map(Omar omar, Wave currentWave)
    {
        this.omar = omar;
        enemiesToRemove = new List<Enemy>();
        bullets = new List<Bullet>();
        bulletsToRemove = new List<Bullet>();
        random = new Random();
        spawner = currentWave.Spawner;
        diamonds = currentWave.diamonds;
        enemies = currentWave.enemies;
        hearts = currentWave.hearts;
        collisionHandler = new CollisionHandler(omar, diamonds, enemies, hearts, bullets);
        shootTimer = new System.Windows.Forms.Timer();
        shootTimer.Interval = omar.GetShootDelay();
        shootTimer.Tick += (sender, e) =>
        {
             // Disparar con cada arma
            foreach (var weapon in omar.weapons)
            {
                var closestEnemy = weapon.GetClosestEnemy(enemies);
                if (closestEnemy != null)
                {
                    weapon.closestEnemy = closestEnemy;
                    omar.Shoot(bullets, closestEnemy); // Llama al Shoot de Omar, pasando la bala y el enemigo más cercano de la arma
                }
            }
        };
        shootTimer.Start();
    }




    public void update()
    {
        collisionHandler.Update();
        omar.UpdatePosition();
        UpdateEnemies();
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
            enemy.act(bullets, omar);
            if (enemy.HP <= 0)
            {
                enemiesToRemove.Add(enemy);
            }
        }
        foreach (var enemy in enemiesToRemove)
        {
            enemies.Remove(enemy);
        }
    }
  
    public void UpdateDiamonds()
    {
        diamonds.RemoveAll(diamond => diamond.IsExpired());
        hearts.RemoveAll(heart => heart.IsExpired());
    }

    public void ClearObjects()
    {
        enemies.Clear();
        diamonds.Clear();
        hearts.Clear();
        spawner.ResetTimers();
        bullets.Clear();
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
            Brush textBrush = collisionHandler.floatingTextColor;
            Brush shadowBrush = Brushes.Black;

            // Medir el tamaño del texto
            SizeF textSize = g.MeasureString(floatingText, font);

            // Calcular posición del texto centrado sobre Omar
            PointF textPosition = new PointF(
                omar.X - (textSize.Width / 2), // Centrado en X
                omar.Y - omar.size // Justo encima en Y
            );

            // Sombra negra para el texto
            g.DrawString(floatingText, font, shadowBrush, textPosition.X + 1, textPosition.Y + 1);
            // Texto principal en amarillo
            g.DrawString(floatingText, font, textBrush, textPosition);
        }
    }

}
