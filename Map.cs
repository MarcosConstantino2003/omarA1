public class Map
{
    private List<Diamond> greenDiamonds; 
    private List<Diamond> purpleDiamonds; // 
    private List<Enemy> enemies;
    private List<PointF> spawnMarkers; 
    private List<Heart> hearts; // Lista de corazones
    private Random random;
    private System.Windows.Forms.Timer greenDiamondTimer;
    private System.Windows.Forms.Timer purpleDiamondTimer;
    private System.Windows.Forms.Timer enemyTimer;
    private System.Windows.Forms.Timer heartTimer;
    private Omar omar;
    public Map(Omar omar)
    {
        this.omar = omar;
        greenDiamonds = new List<Diamond>();
        purpleDiamonds = new List<Diamond>();
        enemies = new List<Enemy>();
        spawnMarkers = new List<PointF>(); 
        hearts = new List<Heart>(); // Inicializar la lista de corazones
        random = new Random();
        // Temporizador para generar rombos verdes cada 6 segundos
        greenDiamondTimer = new System.Windows.Forms.Timer();
        greenDiamondTimer.Interval = 6000; 
        greenDiamondTimer.Tick += SpawnGreenDiamonds;
        greenDiamondTimer.Start();

        // Temporizador para generar rombos rojos cada 12 segundos
        purpleDiamondTimer = new System.Windows.Forms.Timer();
        purpleDiamondTimer.Interval = 12000; 
        purpleDiamondTimer.Tick += SpawnPurpleDiamonds;
        purpleDiamondTimer.Start();

         // Temporizador para generar enemigos cada 4 segundos
        enemyTimer = new System.Windows.Forms.Timer();
        enemyTimer.Interval = 4000; 
        enemyTimer.Tick += SpawnEnemies;
        enemyTimer.Start();

        // Temporizador para generar corazones cada 8 segundos
        heartTimer = new System.Windows.Forms.Timer();
        heartTimer.Interval = 8000;  // 8 segundos
        heartTimer.Tick += SpawnHearts;
        heartTimer.Start();
    }

    private void SpawnGreenDiamonds(object? sender, EventArgs e)
    {
        // Generar rombos verdes en posiciones aleatorias
        float x = random.Next(50, 750); // Posición aleatoria en el rango X
        float y = random.Next(50, 550); // Posición aleatoria en el rango Y

        // Añadir rombos verdes
        greenDiamonds.Add(new Diamond(new PointF(x, y), Color.Green, 20));
    }

    private void SpawnPurpleDiamonds(object? sender, EventArgs e)
    {
        // Generar rombos rojos en posiciones aleatorias
        float x = random.Next(50, 750); // Posición aleatoria en el rango X
        float y = random.Next(50, 550); // Posición aleatoria en el rango Y

        // Añadir rombos rojos
        purpleDiamonds.Add(new Diamond(new PointF(x, y), Color.Purple, 20));
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
        foreach (var redDiamond in purpleDiamonds)
        {
            if (omar.IsCollidingWithDiamond(redDiamond))
            {
                omar.DecreaseSpeed(0.5f);  // Disminuye 0.5 de velocidad por diamante rojo
                purpleDiamonds.Remove(redDiamond); // Eliminar el rombo rojo al ser tocado
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
         // Revisar las colisiones con los corazones
    foreach (var heart in hearts)
    {
        if (omar.IsCollidingWithHeart(heart))
        {
            omar.IncreaseHP(4);  // Aumenta 4 HP por corazón recogido
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
        foreach (var diamond in purpleDiamonds)
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
          // Dibujar los corazones
        foreach (var heart in hearts)
        {
            heart.Draw(g);
        }
        // Obtener el enemigo más cercano a Omar
        Enemy? closestEnemy = GetClosestEnemy();

        // Verificar si el enemigo más cercano es nulo
        if (closestEnemy != null)
        {
            // Dibujar el triángulo apuntando hacia el enemigo más cercano
            omar.DrawTriangle(g, closestEnemy);
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
