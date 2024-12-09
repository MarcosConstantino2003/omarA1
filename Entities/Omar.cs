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
    public float Armor { get; private set; }

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
    private int playAreaRight = 1270;
    private int playAreaTop = 25;
    private int playAreaBottom = 825;

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
        range = 180;
        Armor = 0;
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

        X = Math.Clamp(X, playAreaLeft + Size / 2, playAreaRight - Size / 2);
        Y = Math.Clamp(Y, playAreaTop + Size / 2, playAreaBottom - Size / 2);

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
        // Desplazamientos para ajustar la hitbox del diamante
        float offsetX = diamond.Size * 0.5f; // Ajustar hacia la derecha (10% del tamaño del diamante)
        float offsetY = diamond.Size * 0.5f; // Ajustar hacia abajo (10% del tamaño del diamante)

        // Coordenadas ajustadas del diamante
        float adjustedDiamondX = diamond.Position.X + offsetX;
        float adjustedDiamondY = diamond.Position.Y + offsetY;

        // Calcular la distancia entre Omar y el centro ajustado del diamante
        float dx = X - adjustedDiamondX;
        float dy = Y - adjustedDiamondY;
        float distance = (float)Math.Sqrt(dx * dx + dy * dy);

        // Verificar colisión usando las hitboxes ajustadas
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
        float offsetX = heart.Size;
        float offsetY = heart.Size;

        float adjustedHeartX = heart.Position.X + offsetX;
        float adjustedHeartY = heart.Position.Y + offsetY;

        return (X < adjustedHeartX + heart.Size &&
                X + Size > adjustedHeartX &&
                Y < adjustedHeartY + heart.Size &&
                Y + Size > adjustedHeartY);
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

    public void heal(){
        HP = MaxHP;
    }

    public int takeDamage(float amount)
    {
        if (!isInvulnerable)
        {
            //aplicar armor
            float reductionFactor = 1 - (Armor * 0.06f);
            if (reductionFactor < 0) reductionFactor = 0;
            float finalDamage = amount * reductionFactor;
            int roundedDamage = (int)Math.Round(finalDamage);
            //reducir hp
            HP -= roundedDamage;
            if (HP < 0) HP = 0;
            isTakingDamage = true;
            damageStartTime = DateTime.Now;
            isInvulnerable = true;
            invulnerabilityTimer.Start();
            return roundedDamage;
        }
        return 0;
    }


    public void changeSpeed(float amount)
    {
        Speed += amount;
        if (Speed > MaxSpeed)
        {
            Speed = MaxSpeed;
        }
    }

    public void changeShotSpeed(int amount)
    {
        shotSpeed += amount;
    }

    public void changeHPRegen(int amount)
    {
        HPRegen += amount;
    }
    public void changeDamage(int amount)
    {
        damage += amount;
    }

    public void changeMaxHP(int amount)
    {
        MaxHP += amount;
    }

    public void ChangeArmor(float amount)
    {
        Armor += amount;
        if (Armor < 0) Armor = 0;
    }

    public bool getisInvulnerable()
    {
        return isInvulnerable;
    }

    public void ResetPosition()
    {
        X = 770;
        Y = 400;
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
        float bulletStartX = X + directionX * 30;
        float bulletStartY = Y + directionY * 30 + 20;

        bullets.Add(new OmarBullet(new PointF(bulletStartX, bulletStartY), directionX, directionY, damage));
    }

    public void Draw(Graphics g)
    {
        // Cargar la imagen de Omar
        Image omarImage = Image.FromFile("img\\omar.webp");

        // Verificar si Omar está tomando daño y decidir el color
        bool drawRedOverlay = isTakingDamage && isRed;

        // Dibujar la imagen centrada en las coordenadas de Omar
        float imageX = X - Size / 2;
        float imageY = Y - Size / 2;
        g.DrawImage(omarImage, imageX, imageY, Size, Size);

        // Si está en rojo, dibujar un rectángulo semi-transparente encima de la imagen
        if (drawRedOverlay)
            if (drawRedOverlay)
            {
                using (Brush redBrush = new SolidBrush(Color.FromArgb(120, Color.Red))) // Semitransparente
                {
                    g.FillEllipse(redBrush, imageX, imageY, Size - 2, Size - 2);
                }
            }

        // Dibujar el rango de disparo
        Pen rangePen = new Pen(Color.LightBlue, 1);
        g.DrawEllipse(rangePen, X - range, Y - range, range * 2, range * 2);
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

}
