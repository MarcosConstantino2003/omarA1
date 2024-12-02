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

    private string floatingText = "";
    private PointF floatingTextPosition = PointF.Empty;
    private System.Windows.Forms.Timer floatingTextTimer;
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
        floatingTextTimer = new System.Windows.Forms.Timer();;

        spawner = new Spawner(omar, diamonds, enemies, hearts);

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


    private void SpawnHearts(object? sender, EventArgs e)
    {
        // Generar corazones en posiciones aleatorias
        float x = random.Next(50, 750); // Posición aleatoria en el rango X
        float y = random.Next(50, 550); // Posición aleatoria en el rango Y

        // Añadir corazón a la lista
        hearts.Add(new Heart(new PointF(x, y), Color.Red, 20));
    }   

    // Método para manejar la lógica de las colisiones con los rombos
    public void CheckCollisions()
    {
        // Revisar las colisiones con los rombos verdes
        foreach (var diamond in diamonds)
        {
            if (omar.IsCollidingWithDiamond(diamond))
            {
                string effectText = ""; // Texto del efecto para mostrar

                if (diamond.Color == Color.Green)
                {
                    omar.IncreaseSpeed(1);  
                    effectText = "+1 Speed";
                }
                else if (diamond.Color == Color.Cyan) 
                {
                    omar.IncreaseShotSpeed(2); 
                    shootTimer.Interval =  omar.GetShootDelay();
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
            // Establecer texto flotante y su posición
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

            diamonds.Remove(diamond);
            break; 
        }
        }

        // Revisar colisiones con los enemigos
        foreach (var enemy in enemies)
        {
            if (omar.IsCollidingWithEnemy(enemy))
            {
                omar.DecreaseHP(3);  
                break;
            }
        }
         
    foreach (var heart in hearts)
    {
       if (omar.IsCollidingWithHeart(heart))
        {
            omar.IncreaseHP(4);  // Aumenta 4 HP por corazón recogido

            // Establecer texto flotante
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

            hearts.Remove(heart); // Eliminar el corazón al ser tocado
            break; // Salir del ciclo ya que no es necesario seguir buscando
        }
    }
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
        CheckCollisions();
        omar.UpdatePosition();
        UpdateEnemies();
        UpdateBullets();
        UpdateDiamonds();
    }
    // Método para mover los enemigos hacia Omar
    public void UpdateEnemies()
    {
        foreach (var enemy in enemies)
        {
            enemy.MoveTowardsOmar(omar);
             // Si el enemigo tiene HP 0, lo agregamos a la lista para eliminarlo
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
        // Actualizar la posición de todas las balas
        foreach (var bullet in bullets)
        {
            bullet.Update();
        
           // Verificar colisiones con enemigos
        foreach (var enemy in enemies)
        {
            if (bullet.IsCollidingWithEnemy(enemy))
            {
                enemy.TakeDamage(bullet.Damage); // Reducir HP del enemigo
                bulletsToRemove.Add(bullet);    // Marcar la bala para eliminarla
                break; // Salir del bucle de enemigos, ya que la bala impactó
            }
        }
    }
    

    // Eliminar las balas que colisionaron
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

    // Método para dibujar los rombos en el gráfico
    public void Draw(Graphics g)
    {
        foreach (var diamond in diamonds)
        {
            diamond.Draw(g);
        }
         foreach (var enemy in enemies)
        {
            enemy.Draw(g); 
        }
        spawner.DrawSpawnMarkers(g); 
        foreach (var heart in hearts)
        {
            heart.Draw(g);
        }
        Enemy? closestEnemy = GetClosestEnemy();
        if (closestEnemy != null)
        {
            omar.DrawTriangle(g, closestEnemy);
        }
        foreach (var bullet in bullets)
        {
            bullet.Draw(g);
        }
        // Dibujar texto flotante si existe
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
