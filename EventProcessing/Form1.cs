using EventProcessing.Objects;

namespace EventProcessing
{
    public partial class Form1 : Form
    {
        private Random random = new Random();
        private List<BaseObject> objects = new List<BaseObject>(); // ������ ��������
        private Player player; // ���� �����
        private Marker marker; // ���� ���������
        private RedCircle redCicle; // ���� ������� ����

        private int maxCircle = 5; // ���������� ������� ������� �������
        private int amountCircleNow = 0; // ���������� ������� ������� ������

        

        
        public Form1()
        {
            InitializeComponent();

            
            player = new Player(pbMain.Width, pbMain.Height, 0); // ���� ������

            redCicle = new RedCircle(pbMain.Width, pbMain.Height, 0); // �������� �������� �����

            objects.Add(redCicle);
            
            // �������� ������� �� �����������
            player.OnOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] ����� ��������� � {obj}\n" + txtLog.Text;
            };
            // ������� ������� �� ����������� � ��������
            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };
            // ������� �� ��������� ������ � ���������
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

            // �������� ������
            updatePlayer();
            

            // ������ �� ��������
            if (amountCircleNow < maxCircle)
            {
                objects.Add(new GreenCircle(random.Next() % pbMain.Width, random.Next() % pbMain.Height, 0));
                amountCircleNow++;
            }
            



            // ������������� �����������
            foreach (var obj in objects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj);


                    obj.Overlap(player); // �������� ��� �������� �����
                }
            }

            // �������� �������
            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }
            // ������� ����
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

                // �� ���� �� ������ ���������� ������ dx, dy
                // ��� ������ ���������, ������ ���� ������ ����������
                // ������� ����������� ������ � �������
                // 0.5 ������ ����������� ������� �������� �� ����
                // � ������� ���� ������������ �������� ��������
                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;

                // ����������� ���� �������� ������ 
                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }

            // ���������� ������,
            // ����� �����, ����� ����� ��������� ������� ��������� ����������� ����������
            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            // �������� ������� ������ � ������� ������� ��������

            player.X += player.vX;
            player.Y += player.vY;      
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            
            pbMain.Invalidate();
        }


        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            // ��� ������� �������� ������� �� ����� ���� �� ��� �� ������
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker); // � ������� �� ������ ��������� � objects
            }

            marker.X = e.X;
            marker.Y = e.Y;
        }
    }
}
