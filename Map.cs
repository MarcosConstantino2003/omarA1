public class Map
{
    private List<Diamond> greenDiamonds; // Rombos verdes
    private List<Diamond> redDiamonds; // Rombos rojos
    private Random random;
    private System.Windows.Forms.Timer greenDiamondTimer;
    private System.Windows.Forms.Timer redDiamondTimer;

    public Map()
    {
        greenDiamonds = new List<Diamond>();
        redDiamonds = new List<Diamond>();
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

    // Método para manejar la lógica de las colisiones con los rombos
    public void CheckCollisions(Omar omar)
    {
        // Revisar las colisiones con los rombos verdes
        foreach (var greenDiamond in greenDiamonds)
        {
            if (omar.IsCollidingWithDiamond(greenDiamond))
            {
                omar.Speed += 1; // Aumentar la velocidad
                greenDiamonds.Remove(greenDiamond); // Eliminar el rombo verde al ser tocado
                break; // Salir del ciclo ya que no es necesario seguir buscando
            }
        }

        // Revisar las colisiones con los rombos rojos
        foreach (var redDiamond in redDiamonds)
        {
            if (omar.IsCollidingWithDiamond(redDiamond))
            {
                omar.Speed -= 0.5f; // Disminuir la velocidad
                redDiamonds.Remove(redDiamond); // Eliminar el rombo rojo al ser tocado
                break; // Salir del ciclo ya que no es necesario seguir buscando
            }
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
    }
}
