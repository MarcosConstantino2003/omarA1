public class Omar
{
     public float X { get; set; }
    public float Y { get; set; }
    public float Size { get; set; }
    public float Speed { get; set; }
    public float VelocityX { get; set; } // Velocidad en X
    public float VelocityY { get; set; } // Velocidad en Y
    public float HP { get; set; }

    public Omar(float x, float y, float size)
    {
        X = x;
        Y = y;
        Size = size;
        Speed = 3; // Velocidad inicial de Omar
        VelocityX = 0;
        VelocityY = 0;
        HP = 10;  // HP inicial de 10
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
        return (X < enemy.Position.X + enemy.Size &&
                X + Size > enemy.Position.X &&
                Y < enemy.Position.Y + enemy.Size &&
                Y + Size > enemy.Position.Y);
    }



     public void IncreaseSpeed(float amount)
    {
        Speed += amount;
        if (Speed > 8) // Límite de velocidad
        {
            Speed = 8;
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
    }


    public void Draw(Graphics g)
    {
        Brush brush = Brushes.White;
        Pen blackPen = new Pen(Color.Black, 2);

        // Dibujar Omar como un círculo
        g.FillEllipse(brush, X - Size / 2, Y - Size / 2, Size, Size);
        g.DrawEllipse(blackPen, X - Size / 2, Y - Size / 2, Size, Size);
    }
}
