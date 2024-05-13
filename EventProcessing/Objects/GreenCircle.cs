using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventProcessing.Objects
{
    class GreenCircle : BaseObject // наследуем BaseObject
    {

        public GreenCircle(float x, float y, float angle) : base(x, y, angle)
        {
        }

        // переопределяем Render
        public override void Render(Graphics g)
        {

            g.FillEllipse(new SolidBrush(Color.Green), -15, -15, 30, 30); // сам объект
            g.DrawEllipse(new Pen(Color.Black), -15, -15, 30, 30); // окантовка
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            // самый маленький круг в центре маркера
            path.AddEllipse(-15, -15, 30, 30);
            return path;
        }

    }
}
