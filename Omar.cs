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
    private bool isTakingDamage; 
    private DateTime damageStartTime; 
    private bool isRed; 

    public Omar(float x, float y, float size)
    {
        X = x;
        Y = y;
        Size = size;
        Speed = 4;
        MaxSpeed = 9; 
        VelocityX = 0;
        VelocityY = 0;
        HP = 15;
        MaxHP = 15;
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
        HP -= amount;
        if (HP < 0) HP = 0;
        isTakingDamage = true;
        damageStartTime = DateTime.Now;
    }


    public void Draw(Graphics g)
    {
        Brush brush = isTakingDamage ? (isRed ? Brushes.DarkRed : Brushes.White) : Brushes.White;
        Pen blackPen = new Pen(Color.Black, 2);

        // Dibujar Omar como un círculo
        g.FillEllipse(brush, X - Size / 2, Y - Size / 2, Size, Size);
        g.DrawEllipse(blackPen, X - Size / 2, Y - Size / 2, Size, Size);
    }
}
