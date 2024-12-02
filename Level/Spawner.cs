public class Spawner
{
    private List<PointF> spawnMarkers;
    private Random random;
    private System.Windows.Forms.Timer diamondTimer;
    private System.Windows.Forms.Timer enemyTimer;
    private System.Windows.Forms.Timer heartTimer;
    private Omar omar;
    private List<Diamond> diamonds;
    private List<Enemy> enemies;
    private List<Heart> hearts;

    public Spawner(Omar omar, List<Diamond> diamonds, List<Enemy> enemies, List<Heart> hearts)
    {
        this.omar = omar;
        this.diamonds = diamonds;
        this.enemies = enemies;
        this.hearts = hearts;
        spawnMarkers = new List<PointF>();
        random = new Random();

        // Temporizador para generar rombos verdes cada 6 segundos
        diamondTimer = new System.Windows.Forms.Timer();
        diamondTimer.Interval = 6000;
        diamondTimer.Tick += SpawnDiamonds;
        diamondTimer.Start();

        // Temporizador para generar enemigos cada 4 segundos
        enemyTimer = new System.Windows.Forms.Timer();
        enemyTimer.Interval = 4000;
        enemyTimer.Tick += SpawnEnemies;
        enemyTimer.Start();

        // Temporizador para generar corazones cada 8 segundos
        heartTimer = new System.Windows.Forms.Timer();
        heartTimer.Interval = 8000; // 8 segundos
        heartTimer.Tick += SpawnHearts;
        heartTimer.Start();
    }

    private void SpawnDiamonds(object? sender, EventArgs e)
    {
        float x = random.Next(50, 750); // Posición aleatoria en X
        float y = random.Next(50, 550); // Posición aleatoria en Y

        // Elegir aleatoriamente el color del rombo
        Color diamondColor;
        int randomChoice = random.Next(0, 4); // 0 para verde, 1 para celeste, 2 para negro, 3 para morado

        switch (randomChoice)
        {
            case 0:
                diamondColor = Color.Green;
                break;
            case 1:
                diamondColor = Color.Cyan;
                break;
            case 2:
                diamondColor = Color.Black;
                break;
            case 3:
                diamondColor = Color.Purple;
                break;
            default:
                diamondColor = Color.Green; // Default verde
                break;
        }

        // Añadir el rombo a la lista común
        diamonds.Add(new Diamond(new PointF(x, y), diamondColor, 20));
    }

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

    public void DrawSpawnMarkers(Graphics g)
    {
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
