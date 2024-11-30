public class Map
{
    private List<Diamond> greenDiamonds; // Rombos verdes
    private List<Diamond> redDiamonds; // Rombos rojos
    private List<Enemy> enemies;
    private Random random;
    private System.Windows.Forms.Timer greenDiamondTimer;
    private System.Windows.Forms.Timer redDiamondTimer;
    private System.Windows.Forms.Timer enemyTimer;

    public Map()
    {
        greenDiamonds = new List<Diamond>();
        redDiamonds = new List<Diamond>();
        enemies = new List<Enemy>();
        random = new Random();

        // Temporizador para generar rombos verdes cada 3 segundos
        greenDiamondTimer = new System.Windows.Forms.Timer();
        greenDiamondTimer.Interval = 3000; // 3 segundos
        greenDiamondTimer.Tick += SpawnGreenDiamonds;
        greenDiamondTimer.Start();

        // Temporizador para generar rombos rojos cada 5 segundos
        redDiamondTimer = new System.Windows.Forms.Timer();
        redDiamondTimer.Interval = 5000; // 5 segundos
        redDiamondTimer.Tick += SpawnRedDiamonds;
        redDiamondTimer.Start();

         // Temporizador para generar enemigos cada 5 segundos
        enemyTimer = new System.Windows.Forms.Timer();
        enemyTimer.Interval = 6000; // 5 segundos
        enemyTimer.Tick += SpawnEnemies;
        enemyTimer.Start();
    }

    private void SpawnGreenDiamonds(object? sender, EventArgs e)
    {
        // Generar rombos verdes en posiciones aleatorias
        float x = random.Next(50, 750); // Posición aleatoria en el rango X
        float y = random.Next(50, 550); // Posición aleatoria en el rango Y

        // Añadir rombos verdes
        greenDiamonds.Add(new Diamond(new PointF(x, y), Color.Green, 20));
    }

    private void SpawnRedDiamonds(object? sender, EventArgs e)
    {
        // Generar rombos rojos en posiciones aleatorias
        float x = random.Next(50, 750); // Posición aleatoria en el rango X
        float y = random.Next(50, 550); // Posición aleatoria en el rango Y

        // Añadir rombos rojos
        redDiamonds.Add(new Diamond(new PointF(x, y), Color.Red, 20));
    }

     // Generar enemigos
    private void SpawnEnemies(object? sender, EventArgs e)
    {
        float x = random.Next(50, 750); 
        float y = random.Next(50, 550); 

        // Crear un enemigo con color marrón
        enemies.Add(new Enemy(new PointF(x, y), Color.Brown, 30));
    }

    // Método para manejar la lógica de las colisiones con los rombos
    public void CheckCollisions(Omar omar)
    {
        // Revisar las colisiones con los rombos verdes
        foreach (var greenDiamond in greenDiamonds)
        {
            if (omar.IsCollidingWithDiamond(greenDiamond))
            {
                omar.IncreaseSpeed(1);  // Aumenta 1 de velocidad por diamante verde
                greenDiamonds.Remove(greenDiamond); // Eliminar el rombo verde al ser tocado
                break; // Salir del ciclo ya que no es necesario seguir buscando
            }
        }

        // Revisar las colisiones con los rombos rojos
        foreach (var redDiamond in redDiamonds)
        {
            if (omar.IsCollidingWithDiamond(redDiamond))
            {
                omar.DecreaseSpeed(0.5f);  // Disminuye 0.5 de velocidad por diamante rojo
                redDiamonds.Remove(redDiamond); // Eliminar el rombo rojo al ser tocado
                break; // Salir del ciclo ya que no es necesario seguir buscando
            }
        }
        // Revisar colisiones con los enemigos
        foreach (var enemy in enemies)
        {
            if (omar.IsCollidingWithEnemy(enemy))
            {
                omar.DecreaseHP(1);  // Reduce 1 HP por colisión con enemigo
                enemies.Remove(enemy);  // Eliminar al enemigo después de la colisión
                break;
            }
        }
    }

    // Método para mover los enemigos hacia Omar
    public void UpdateEnemies(Omar omar)
    {
        foreach (var enemy in enemies)
        {
            enemy.MoveTowardsOmar(omar);
        }
    }
    // Método para dibujar los rombos en el gráfico
    public void Draw(Graphics g)
    {
        // Dibujar los rombos verdes
        foreach (var diamond in greenDiamonds)
        {
            diamond.Draw(g);
        }

        // Dibujar los rombos rojos
        foreach (var diamond in redDiamonds)
        {
            diamond.Draw(g);
        }
         foreach (var enemy in enemies)
        {
            enemy.Draw(g); // Dibuja cada enemigo
        }
    }
}
