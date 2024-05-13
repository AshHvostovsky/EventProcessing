using EventProcessing.Objects;

namespace EventProcessing
{
    public partial class Form1 : Form
    {
        private Random random = new Random();
        private List<BaseObject> objects = new List<BaseObject>(); // список объектов
        private Player player; // поле игрок
        private Marker marker; // поле указатель
        private RedCircle redCicle; // поле красный круг

        private int maxCircle = 5; // количество игровых зеленых кружков
        private int amountCircleNow = 0; // количество зеленых кружков сейчас

        

        
        public Form1()
        {
            InitializeComponent();

            
            player = new Player(pbMain.Width, pbMain.Height, 0); // инит игрока

            redCicle = new RedCircle(pbMain.Width, pbMain.Height, 0); // создание красного круга

            objects.Add(redCicle);
            
            // добавл€ю реакцию на пересечение
            player.OnOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] »грок пересекс€ с {obj}\n" + txtLog.Text;
            };
            // добавил реакцию на пересечение с маркером
            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };
            // реакци€ на пересечие игрока с кругл€шом
            player.OnGreenCircleOverlap += (m) =>
            {
                objects.Remove(m);
                amountCircleNow--;
                player.points++;
            };
            redCicle.OnPlayerOverlap += (m) =>
            {
                if (player.points > 0)
                    player.points--;
                redCicle.newGenerate();
                
            };

            objects.Add(player);
            marker = new Marker(pbMain.Width-20, pbMain.Height-20, 0);

            

            objects.Add(marker);

  

   
        }

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            g.Clear(Color.White);

            // ѕересчет игрока
            updatePlayer();
            

            // гл€нем на кругл€ши
            if (amountCircleNow < maxCircle)
            {
                objects.Add(new GreenCircle(random.Next() % pbMain.Width, random.Next() % pbMain.Height, 0));
                amountCircleNow++;
            }
            



            // пересчитываем пересечени€
            foreach (var obj in objects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj);


                    obj.Overlap(player); // работает дл€ красного круга
                }
            }

            // рендерим объекты
            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }
            // »зменим очки
            pointsLabel.Text = "Score: " + player.points;
        }
        private void updatePlayer()
        {
            if (marker != null)
            {
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;
                float length = MathF.Sqrt(dx * dx + dy * dy);
                dx /= length;
                dy /= length;

                // по сути мы теперь используем вектор dx, dy
                // как вектор ускорени€, точнее даже вектор прит€жени€
                // который прит€гивает игрока к маркеру
                // 0.5 просто коэффициент который подобрал на глаз
                // и который дает естественное ощущение движени€
                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;

                // расчитываем угол поворота игрока 
                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }

            // тормоз€щий момент,
            // нужен чтобы, когда игрок достигнет маркера произошло постепенное замедление
            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            // пересчет позици€ игрока с помощью вектора скорости

            player.X += player.vX;
            player.Y += player.vY;      
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            
            pbMain.Invalidate();
        }


        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            // тут добавил создание маркера по клику если он еще не создан
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker); // и главное не забыть пололжить в objects
            }

            marker.X = e.X;
            marker.Y = e.Y;
        }
    }
}
