using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventProcessing.Objects
{
    
    internal class RedCircle : BaseObject
    {
        Random random;


        public Action<Player> OnPlayerOverlap;



        int maxRad = 400;
        int radius = 0;

        private int centerX;
        private int centerY;

        private int width;
        private int height;


        // создаем конструктор с тем же набором параметров что и в BaseObject
        // base(x, y, angle) -- вызывает конструктор родительского класса
        public RedCircle(float x, float y, float angle) : base(x, y, angle)
        {
            this.random = new Random();
            this.width = (int) x;
            this.height = (int) y;

            // начальные координаты
            this.X = width / 2;
            this.Y = height / 2;
            
        }

        // переопределяем Render
        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.FromArgb(50, 255, 10, 10)), centerX - radius, centerY - radius, radius * 2, radius * 2); // Сам кружок
            g.DrawEllipse(new Pen(Color.Black), centerX - radius, centerY - radius, radius * 2, radius * 2); // Окантовка
            update();
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(centerX - radius, centerY - radius, radius * 2, radius * 2); // Увеличиваем для самого маленького круга в центре
            return path;
        }
        public override void Overlap(BaseObject obj)
        {
            //base.Overlap(obj);
            if (obj is Player)
            {
                OnPlayerOverlap(obj as Player);
            }

        }

        // Изменяем размер круга
        public void update()
        {
            if (radius < maxRad)
            {
                radius += 1;
            }
            else
            {
                radius = 0;
                newGenerate();
            }
        }

        // генерация новых координат и обнуление радиуса
        public void newGenerate()
        {
            radius = 0;
            X = random.Next() % width;
            Y = random.Next() % height;
        }
    }
}
