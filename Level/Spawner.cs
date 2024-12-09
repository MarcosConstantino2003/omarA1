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
    private int playAreaLeft = 270;
    private int playAreaRight = 1230;
    private int playAreaTop = 45;
    private int playAreaBottom = 805;
    private int diamondRate = 6000;
    private int enemyRate = 4000;
    private int heartRate = 10000;


    public Spawner(Omar omar, List<Diamond> diamonds, List<Enemy> enemies, List<Heart> hearts)
    {
        this.omar = omar;
        this.diamonds = diamonds;
        this.enemies = enemies;
        this.hearts = hearts;
        spawnMarkers = new List<PointF>();
        random = new Random();

        diamondTimer = new System.Windows.Forms.Timer();
        diamondTimer.Interval = diamondRate;
        diamondTimer.Tick += SpawnDiamonds;
        diamondTimer.Start();

        enemyTimer = new System.Windows.Forms.Timer();
        enemyTimer.Interval = enemyRate;
        enemyTimer.Tick += SpawnEnemies;
        enemyTimer.Start();

        heartTimer = new System.Windows.Forms.Timer();
        heartTimer.Interval = heartRate;
        heartTimer.Tick += SpawnHearts;
        heartTimer.Start();
    }

    private void SpawnDiamonds(object? sender, EventArgs e)
    {
        float x = random.Next(playAreaLeft, playAreaRight);
        float y = random.Next(playAreaTop, playAreaBottom);


        Color diamondColor;
        int randomChoice = random.Next(0, 5);

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
            case 4:
                diamondColor = Color.Orange;
                break;
            default:
                diamondColor = Color.Green;
                break;
        }

        diamonds.Add(new Diamond(new PointF(x, y), diamondColor, 20));
    }

    private async void SpawnEnemies(object? sender, EventArgs e)
    {
        int numberOfEnemies = random.Next(3, 7);
        PointF centerPoint = GetValidSpawnPoint();

        spawnMarkers.Add(centerPoint);
        await Task.Delay(400);

        for (int i = 0; i < numberOfEnemies; i++)
        {
            PointF spawnPoint = GetRandomSpawnPoint(centerPoint);
            Enemy newEnemy = GetRandomEnemyType(spawnPoint);

            enemies.Add(newEnemy);

            spawnMarkers.Remove(centerPoint);
            await Task.Delay(200);
        }
    }

    // Método privado para obtener un punto de spawn válido
    private PointF GetValidSpawnPoint()
    {
        PointF centerPoint;
        do
        {
            float centerX = random.Next(playAreaLeft, playAreaRight);
            float centerY = random.Next(playAreaTop, playAreaBottom);
            centerPoint = new PointF(centerX, centerY);
        }
        while (IsTooCloseToOmar(centerPoint));
        return centerPoint;
    }

    // Método privado para obtener un punto de spawn aleatorio alrededor del centro
    private PointF GetRandomSpawnPoint(PointF centerPoint)
    {
        float offsetX = random.Next(-30, 31);
        float offsetY = random.Next(-30, 31);

        float x = Math.Clamp(centerPoint.X + offsetX, playAreaLeft, playAreaRight);
        float y = Math.Clamp(centerPoint.Y + offsetY, playAreaTop, playAreaBottom);

        return new PointF(x, y);
    }

    // Método privado para obtener un tipo de enemigo aleatorio
    private Enemy GetRandomEnemyType(PointF spawnPoint)
    {
        int enemyTypeChance = random.Next(1, 101);

        if (enemyTypeChance <= 10)
        {
            return new FastEnemy(spawnPoint);
        }
        else if (enemyTypeChance <= 25)
        {
            return new SlowEnemy(spawnPoint);
        }
        else
        {
            return new BasicEnemy(spawnPoint);
        }
    }

    private bool IsTooCloseToOmar(PointF point)
    {
        const float minimumDistance = 100;
        float dx = point.X - omar.X;
        float dy = point.Y - omar.Y;
        float distance = (float)Math.Sqrt(dx * dx + dy * dy);
        return distance < minimumDistance;
    }

    private void SpawnHearts(object? sender, EventArgs e)
    {
        float x = random.Next(playAreaLeft, playAreaRight);
        float y = random.Next(playAreaTop, playAreaBottom);
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

    public void ResetTimers()
    {
        diamondTimer.Stop();
        enemyTimer.Stop();
        heartTimer.Stop();

        diamondTimer.Interval = diamondRate;
        enemyTimer.Interval = enemyRate;
        heartTimer.Interval = heartRate;

        diamondTimer.Start();
        enemyTimer.Start();
        heartTimer.Start();
    }
    public void StopSpawning()
    {
        diamondTimer.Stop();
        enemyTimer.Stop();
        heartTimer.Stop();
    }
}
