public class Omar
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Size { get; set; }
    public float MaxSpeed { get; set; } 
    public float Speed { get; set; }
    public float VelocityX { get; set; } 
    public float VelocityY { get; set; } 
    public float MaxHP { get; set; }
    public float HP { get; set; }
    public int damage {get; set;}
    public int shotSpeed {get; set;}
    private bool isTakingDamage; 
    private DateTime damageStartTime; 
    private bool isRed; 
    private bool isInvulnerable;
    private System.Windows.Forms.Timer invulnerabilityTimer;
    public float range { get; set; }



    public Omar(float x, float y, float size)
    {
        X = x;
        Y = y;
        Size = size;
        //stats iniciales
        Speed = 4;
        MaxSpeed = 9; 
        VelocityX = 0;
        VelocityY = 0;
        MaxHP = 15;
        HP = MaxHP;
        damage = 3;
        shotSpeed = 1;
        range = 120; 

        //
        isInvulnerable = false;
        // Inicializar el temporizador de invulnerabilidad
        invulnerabilityTimer = new System.Windows.Forms.Timer();
        invulnerabilityTimer.Interval = 1000; // 1 segundo
        invulnerabilityTimer.Tick += (sender, e) =>
        {
            isInvulnerable = false; // Desactivar invulnerabilidad
            invulnerabilityTimer.Stop(); // Detener el temporizador
        };
    }

    public void MoveSmooth(float deltaX, float deltaY)
    {
         // Calcular la longitud del vector (es decir, la distancia)
        float magnitude = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            
        // Si no estamos moviéndonos, no normalizamos
        if (magnitude != 0)
        {
            // Normalizamos el vector de dirección para que la velocidad sea constante en todas direcciones
            deltaX = deltaX / magnitude;
            deltaY = deltaY / magnitude;
        }

        // Multiplicamos por la velocidad deseada
        VelocityX = deltaX * Speed;
        VelocityY = deltaY * Speed;
    }

    // Actualizamos la posición basándonos en la velocidad (para movimiento suave)
    public void UpdatePosition()
    {
        X += VelocityX;
        Y += VelocityY;

        if (isTakingDamage)
        {
            double elapsedSeconds = (DateTime.Now - damageStartTime).TotalSeconds;

            // Alternar color cada 100 ms
            if ((int)(elapsedSeconds * 10) % 2 == 0)
            {
                isRed = true;
            }
            else
            {
                isRed = false;
            }

            // Salir del estado de "daño" después de 1 segundo
            if (elapsedSeconds > 0.5)
            {
                isTakingDamage = false;
                isRed = false; // Asegurarse de volver al estado blanco
            }
        }
    }


    // Método para verificar la colisión con un rombo
    public bool IsCollidingWithDiamond(Diamond diamond)
    {
        // Calculamos la distancia entre el centro de Omar y el centro del rombo
        float dx = X - diamond.Position.X;
        float dy = Y - diamond.Position.Y;
        float distance = (float)Math.Sqrt(dx * dx + dy * dy);

        // Si la distancia es menor que la suma de los radios (considerando el tamaño del rombo y de Omar)
        return distance < (Size / 2 + diamond.Size / 2);
    }

     // Método de colisión con el enemigo
  public bool IsCollidingWithEnemy(Enemy enemy)
    {
        // Desplazar ligeramente las coordenadas de Omar hacia la derecha en la colisión
        float offsetX = -Size/2;  // Este valor puede ser ajustado según sea necesario

        return (X + offsetX < enemy.Position.X + enemy.Size &&
                X + Size + offsetX > enemy.Position.X &&
                Y - Size/2 < enemy.Position.Y + enemy.Size &&
                Y + Size/2> enemy.Position.Y);
    }

    // Método para verificar colisiones con los corazones
    public bool IsCollidingWithHeart(Heart heart)
    {
        // Verificar si las posiciones de Omar y el corazón se superponen
        return (X < heart.Position.X + heart.Size &&
                X + Size > heart.Position.X &&
                Y < heart.Position.Y + heart.Size &&
                Y + Size > heart.Position.Y);
    }

    public int GetShootDelay()
    {
        // El delay base es de 500ms y se reduce 30ms por cada unidad de shotSpeed, con un mínimo de 100ms
        int calculatedDelay = 500 - (int)(shotSpeed * 30);
        return Math.Max(calculatedDelay, 100); // Limitar el delay a 100ms como mínimo
    }

    // Método para aumentar los HP de Omar
    public void IncreaseHP(int amount)
    {
        HP += amount;
        if (HP > MaxHP) // Asegurar que no supere el max HP
        {
            HP = MaxHP;
        }
    }

     public void IncreaseSpeed(float amount)
    {
         Speed += amount;
        if (Speed > MaxSpeed) // Límite de velocidad
        {
            Speed = MaxSpeed;
        }
    }

    public void DecreaseSpeed(float amount)
    {
        Speed -= amount;
        if (Speed < 0) // Límite mínimo de velocidad
        {
            Speed = 0;
        }
    }

    public void DecreaseHP(float amount)
    {
        if (!isInvulnerable) // Solo perder HP si no es invulnerable
        {
            HP -= amount;
            if (HP < 0) HP = 0;
            isTakingDamage = true;
            damageStartTime = DateTime.Now;
            isInvulnerable = true;
            invulnerabilityTimer.Start();
        }
    }


    public void IncreaseShotSpeed(int amount)
    {
        shotSpeed += amount; 
    }

    public void IncreaseDamage(int amount)
    {
        damage += amount; // Aumentar el daño
    }

    public void ResetPosition()
    {
        X = 400; 
        Y = 290;
    }

    public void DrawTriangle(Graphics g, Enemy closestEnemy)
    {
        if (closestEnemy == null) return; 

        // Calcular la dirección hacia el enemigo
        float dx = closestEnemy.Position.X - X;
        float dy = closestEnemy.Position.Y - Y;
        float distance = (float)Math.Sqrt(dx * dx + dy * dy);


        // Normalizamos el vector dirección (hacia el enemigo)
        float directionX = dx / distance;
        float directionY = dy / distance;

        // Definir la distancia del triángulo respecto a Omar
        float triangleDistance = 40; 

        // Posición del vértice del triángulo (orbitando cerca de Omar)
        float triangleX = X + directionX * triangleDistance;
        float triangleY = Y + directionY * triangleDistance - 10;  

        // Coordenadas de los tres vértices del triángulo
        PointF p1 = new PointF(triangleX, triangleY);
        PointF p2 = new PointF(triangleX - 10, triangleY + 20); 
        PointF p3 = new PointF(triangleX + 10, triangleY + 20); 

        // Dibujar el triángulo apuntando al enemigo más cercano con color azul oscuro
        g.FillPolygon(Brushes.DarkBlue, new PointF[] { p1, p2, p3 });
    }

    
    public void Shoot(List<Bullet> bullets, Enemy closestEnemy)
    {
        if (closestEnemy == null) return;

        // Calcular la dirección hacia el enemigo más cercano
        float dx = closestEnemy.Position.X - X;
        float dy = closestEnemy.Position.Y - Y;
        float distance = (float)Math.Sqrt(dx * dx + dy * dy);

        // Normalizar la dirección
        float directionX = dx / distance;
        float directionY = dy / distance;

        // Verificar si el enemigo está dentro del rango
        if (distance > range) return;

        // Crear una nueva bala saliendo desde el triángulo
        float bulletStartX = X + directionX * 30; // Coordenada inicial X de la bala
        float bulletStartY = Y + directionY * 30 + 20; // Coordenada inicial Y de la bala

        bullets.Add(new Bullet(new PointF(bulletStartX, bulletStartY), directionX, directionY, damage)); // Daño de 5
    }

    public void Draw(Graphics g)
    {
        Brush brush = isTakingDamage ? (isRed ? Brushes.DarkRed : Brushes.White) : Brushes.White;
        Pen blackPen = new Pen(Color.Black, 2);
        g.FillEllipse(brush, X - Size / 2, Y - Size / 2, Size, Size);
        g.DrawEllipse(blackPen, X - Size / 2, Y - Size / 2, Size, Size);

        // Dibujar el rango de disparo
        Pen rangePen = new Pen(Color.LightBlue, 1);
        g.DrawEllipse(rangePen, X - range, Y - range, range * 2, range * 2);
    }
}
