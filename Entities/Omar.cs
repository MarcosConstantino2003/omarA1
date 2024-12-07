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
    public int damage { get; set; }
    public int shotSpeed { get; set; }
    private bool isTakingDamage;
    public int HPRegen { get; set; }
    private DateTime damageStartTime;
    private bool isRed;
    private bool isInvulnerable;
    private System.Windows.Forms.Timer invulnerabilityTimer;
    public float range { get; set; }
    private DateTime lastRegenTime;
    private float regenInterval;
    private int playAreaLeft = 270;
    private int playAreaRight = 1230;
    private int playAreaTop = 45;
    private int playAreaBottom = 805;

    public Omar(float x, float y, float size)
    {
        X = x;
        Y = y;
        Size = size;
        VelocityX = 0;
        VelocityY = 0;
        //stats iniciales
        Speed = 4;
        MaxSpeed = 9;
        MaxHP = 15;
        HP = MaxHP;
        damage = 3;
        shotSpeed = 1;
        range = 120;
        //hp regen
        HPRegen = 0;
        lastRegenTime = DateTime.Now;
        regenInterval = 3.3f - 0.3f * HPRegen;
        //iframes
        invulnerabilityTimer = new System.Windows.Forms.Timer();
        invulnerabilityTimer.Interval = 1000; 
        invulnerabilityTimer.Tick += (sender, e) =>
        {
            isInvulnerable = false; 
            invulnerabilityTimer.Stop(); 
        };
    }

    public void MoveSmooth(float deltaX, float deltaY)
    {
        float magnitude = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        // Si no estamos moviéndonos, no normalizamos
        if (magnitude != 0)
        {
            deltaX = deltaX / magnitude;
            deltaY = deltaY / magnitude;
        }

        VelocityX = deltaX * Speed;
        VelocityY = deltaY * Speed;
    }

    public void UpdatePosition()
    {
        X += VelocityX;
        Y += VelocityY;

        X = Math.Clamp(X, playAreaLeft, playAreaRight);
        Y = Math.Clamp(Y, playAreaTop, playAreaBottom);

        //Regeneración de HP
        if (HPRegen > 0)
        {
            regenInterval = Math.Max(1f, 8.8f - 0.8f * HPRegen);
            if ((DateTime.Now - lastRegenTime).TotalSeconds >= regenInterval)
            {
                IncreaseHP(1);
                lastRegenTime = DateTime.Now;
            }
        }
        //iframes
        if (isTakingDamage)
        {
            double elapsedSeconds = (DateTime.Now - damageStartTime).TotalSeconds;
            if ((int)(elapsedSeconds * 10) % 2 == 0)
            {
                isRed = true;
            }
            else
            {
                isRed = false;
            }

            if (elapsedSeconds > 0.5)
            {
                isTakingDamage = false;
                isRed = false;
            }
        }
    }

    public bool IsCollidingWithDiamond(Diamond diamond)
    {
        float dx = X - diamond.Position.X;
        float dy = Y - diamond.Position.Y;
        float distance = (float)Math.Sqrt(dx * dx + dy * dy);

        return distance < (Size / 2 + diamond.Size / 2);
    }

    public bool IsCollidingWithEnemy(Enemy enemy)
    {
        float offsetX = -Size / 2; 

        return (X + offsetX < enemy.Position.X + enemy.Size &&
                X + Size + offsetX > enemy.Position.X &&
                Y - Size / 2 < enemy.Position.Y + enemy.Size &&
                Y + Size / 2 > enemy.Position.Y);
    }

    public bool IsCollidingWithHeart(Heart heart)
    {
        return (X < heart.Position.X + heart.Size &&
                X + Size > heart.Position.X &&
                Y < heart.Position.Y + heart.Size &&
                Y + Size > heart.Position.Y);
    }

    public int GetShootDelay()
    {
        int calculatedDelay = 500 - (int)(shotSpeed * 30);
        return Math.Max(calculatedDelay, 100); // Limitar el delay a 100ms como mínimo
    }

    public void IncreaseHP(int amount)
    {
        HP += amount;
        if (HP > MaxHP) 
        {
            HP = MaxHP;
        }
    }

     public void DecreaseHP(float amount)
    {
        if (!isInvulnerable) 
        {
            HP -= amount;
            if (HP < 0) HP = 0;
            isTakingDamage = true;
            damageStartTime = DateTime.Now;
            isInvulnerable = true;
            invulnerabilityTimer.Start();
        }
    }


    public void IncreaseSpeed(float amount)
    {
        Speed += amount;
        if (Speed > MaxSpeed) 
        {
            Speed = MaxSpeed;
        }
    }

    public void DecreaseSpeed(float amount)
    {
        Speed -= amount;
        if (Speed < 0) 
        {
            Speed = 0;
        }
    }
   

    public void IncreaseShotSpeed(int amount)
    {
        shotSpeed += amount;
    }

    public void IncreaseHPRegen(int amount)
    {
        HPRegen += amount;
    }
    public void IncreaseDamage(int amount)
    {
        damage += amount; 
    }

    public void IncreaseMaxHP(int amount)
    {
        MaxHP += amount;
    }

    public void ResetPosition()
    {
        X = 770;
        Y = 400;
    }

    public void DrawTriangle(Graphics g, Enemy closestEnemy)
    {
        if (closestEnemy == null) return;

        // Calcular la dirección hacia el enemigo
        float dx = closestEnemy.Position.X - X;
        float dy = closestEnemy.Position.Y - Y;
        float distance = (float)Math.Sqrt(dx * dx + dy * dy);

        float directionX = dx / distance;
        float directionY = dy / distance;
        float triangleDistance = 40;


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
