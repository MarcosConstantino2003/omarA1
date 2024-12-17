using System.Data;
using System.Drawing.Drawing2D;

public class Pistol : RangedWeapon
{
    public Pistol(Omar owner) : base(owner)
    {
        // Asignamos valores base de la pistola
        baseDamage = 2;
        baseShotSpeed = 1.5f;
        baseRange = 50;
    }

    // Implementación de la lógica de dibujo para la Pistola (triángulo)
    public override void Draw(Graphics g)
    {
        // Posición fija del triángulo relativa a Omar
        float triangleBaseX = owner.X + 30;
        float triangleBaseY = owner.Y - 30;

        // Calcular el ángulo hacia el enemigo más cercano
        float angle = 0;
        if (closestEnemy != null)
        {
            float dx = closestEnemy.Position.X - triangleBaseX;
            float dy = closestEnemy.Position.Y - triangleBaseY;
            angle = (float)(Math.Atan2(dy, dx) * (180 / Math.PI));
        }

        // Dibujar el rango de disparo (círculo celeste) en la base del triángulo
        Pen rangePen = new Pen(Color.LightBlue, 1);
        g.DrawEllipse(rangePen, triangleBaseX - range, triangleBaseY - range, range * 2, range * 2);

        // Guardar el estado de transformación original
        GraphicsState state = g.Save();

        // Aplicar rotación
        g.TranslateTransform(triangleBaseX, triangleBaseY); // Mover el origen de rotación al centro del triángulo
        g.RotateTransform(angle + 90); // Ajustar 90 grados para que la punta apunte al enemigo

        // Coordenadas relativas del triángulo
        PointF[] trianglePoints = new PointF[]
        {
        new PointF(0, -10),    // Punta del triángulo
        new PointF(-10, 10),   // Esquina izquierda
        new PointF(10, 10)     // Esquina derecha
        };

        // Dibujar el triángulo
        g.FillPolygon(Brushes.DarkBlue, trianglePoints);


        // Restaurar el estado original
        g.Restore(state);
    }


}