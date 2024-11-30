public class Map
{
    private List<Diamond> greenDiamonds; // Rombos verdes
    private List<Diamond> redDiamonds; // Rombos rojos
    private List<Enemy> enemies;
    private List<PointF> spawnMarkers; 

    private Random random;
    private System.Windows.Forms.Timer greenDiamondTimer;
    private System.Windows.Forms.Timer redDiamondTimer;
    private System.Windows.Forms.Timer enemyTimer;
    private Omar omar;
    public Map(Omar omar)
    {
        this.omar = omar;
        greenDiamonds = new List<Diamond>();
        redDiamonds = new List<Diamond>();
        enemies = new List<Enemy>();
        spawnMarkers = new List<PointF>(); 
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
    private async void SpawnEnemies(object? sender, EventArgs e)
    {
    // Generar un número aleatorio de enemigos entre 4 y 8
    int numberOfEnemies = random.Next(4, 9);

    // Determinar un punto central válido que no esté cerca de Omar
    PointF centerPoint;
    do
    {
        float centerX = random.Next(100, 700); // Evitamos los bordes
        float centerY = random.Next(100, 500);
        centerPoint = new PointF(centerX, centerY);
    }
    while (IsTooCloseToOmar(centerPoint)); // Asegurar que el punto no esté cerca de Omar
    spawnMarkers.Add(centerPoint);
    await Task.Delay(400); // Tiempo entre enemigos
    // Spawnear los enemigos con una leve diferencia de tiempo
    for (int i = 0; i < numberOfEnemies; i++)
    {
        // Generar un desplazamiento pequeño para cada enemigo (cerca del centro)
        float offsetX = random.Next(-30, 31); // Hasta 30px alrededor del centro
        float offsetY = random.Next(-30, 31);

        float x = Math.Clamp(centerPoint.X + offsetX, 50, 750); // Aseguramos que no salgan de los límites
        float y = Math.Clamp(centerPoint.Y + offsetY, 50, 550);

         PointF spawnPoint = new PointF(x, y);

        // Añadir una marca de spawn y esperar 1 segundo

        // Crear y añadir el enemigo
        enemies.Add(new Enemy(spawnPoint, Color.Brown, 30));

        // Quitar la marca de spawn
        spawnMarkers.Remove(centerPoint);
        await Task.Delay(200); // Tiempo entre enemigos
    }
    }

    // Método para verificar si el punto está demasiado cerca de Omar
    private bool IsTooCloseToOmar(PointF point)
    {
        const float minimumDistance = 100; // Distancia mínima permitida
        float dx = point.X - omar.X; // Diferencia en X
        float dy = point.Y - omar.Y; // Diferencia en Y
        float distance = (float)Math.Sqrt(dx * dx + dy * dy);
        return distance < minimumDistance;
    }

    // Método para manejar la lógica de las colisiones con los rombos
    public void CheckCollisions()
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
                omar.DecreaseHP(4);  // Reduce 1 HP por colisión con enemigo
                enemies.Remove(enemy);  // Eliminar al enemigo después de la colisión
                break;
            }
        }
    }

    // Método para mover los enemigos hacia Omar
    public void UpdateEnemies()
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
        foreach (var marker in spawnMarkers)
        {
            DrawSpawnMarker(g, marker);
        }
    }
    private void DrawSpawnMarker(Graphics g, PointF position)
    {
        Pen blackPen = new Pen(Color.Black, 2);

        // Dibujar una "X" centrada en la posición
        g.DrawLine(blackPen, position.X - 10, position.Y - 10, position.X + 10, position.Y + 10);
        g.DrawLine(blackPen, position.X - 10, position.Y + 10, position.X + 10, position.Y - 10);
    }
}
