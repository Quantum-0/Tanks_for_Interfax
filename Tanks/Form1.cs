using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tanks
{
    public partial class Form1 : Form
    {
        Dictionary<Label, double> Directions = new Dictionary<Label, double>();
        Dictionary<Label, PointF> Coords = new Dictionary<Label, PointF>();
        List<Label> Labels = new List<Label>();
        List<Button> Buttons = new List<Button>();
        List<Rectangle> Rectangles = new List<Rectangle>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Random rnd = new Random();

            Labels = this.Controls.OfType<Label>().ToList();
            Buttons = this.Controls.OfType<Button>().ToList();
            
            foreach (var l in Labels)
            {
                // Меняем свойства label'ов
                l.Text = "T";
                l.TextAlign = ContentAlignment.MiddleCenter;

                // Добавляем в список прямоугольников рамку label'a
                Rectangles.Add(l.Bounds);

                // Создаём направления и координаты для всех
                Directions.Add(l, rnd.NextDouble() * Math.PI * 2);
                Coords.Add(l, l.Location);
            }

            // Так же записываем рамки всех препятствий
            foreach (var b in Buttons)
            {
                Rectangles.Add(b.Bounds);
            }

            // Проверяем коллизии
            for (int i = 0; i < Rectangles.Count; i++)
            {
                for (int j = i + 1; j < Rectangles.Count; j++)
                {
                    if (Rectangles[i].IntersectsWith(Rectangles[j]))
                    {
                        MessageBox.Show("Некорректное исходное состояние");
                        Close();
                    }
                }

                if (i < Labels.Count)
                    if (Rectangles[i].X < 0 || Rectangles[i].Y < 0 || Rectangles[i].Bottom > this.Height || Rectangles[i].Right > this.Width)
                    {
                        MessageBox.Show("Танк пересекает или находится за пределами формы");
                        Close();
                    }
            }

            timer1.Interval = 200;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < Labels.Count; i++)
            {
                // Перемещаем label
                Coords[Labels[i]] = new PointF(Coords[Labels[i]].X + (float)Math.Cos(Directions[Labels[i]]), Coords[Labels[i]].Y + (float)Math.Sin(Directions[Labels[i]]));
                Labels[i].Location = new Point((int)Math.Round(Coords[Labels[i]].X), (int)Math.Round(Coords[Labels[i]].Y));

                // Обновляем прямоугольники
                Rectangles[i] = Labels[i].Bounds;
            }

            for (int i = 0; i < Labels.Count; i++)
            {
                // Проверяем границы
                if (Rectangles[i].Left < 0 || Rectangles[i].Right > this.ClientRectangle.Width)
                    Directions[Labels[i]] = Math.PI - Directions[Labels[i]];
                if (Rectangles[i].Top < 0 || Rectangles[i].Bottom > this.ClientRectangle.Height)
                    Directions[Labels[i]] = - Directions[Labels[i]];

                if (Directions[Labels[i]] < 0)
                    Directions[Labels[i]] += Math.PI * 2;
                if (Directions[Labels[i]] > Math.PI * 2)
                    Directions[Labels[i]] -= Math.PI * 2;

                // Проверяем коллизии с другими танками
                for (int j = i + 1; j < Labels.Count; j++)
                    if (Rectangles[i].IntersectsWith(Rectangles[j]))
                    {
                        // Меняем направление для labels[i] и labels[j]
                        var Vect = getPointDirection(Labels[j].Location, Labels[i].Location);
                        Directions[Labels[i]] = Vect * 2 - Directions[Labels[i]];
                        Directions[Labels[j]] = Vect * 2 - Directions[Labels[j]];
                    }

                // Проверяем столкновения с препядствиями
                for (int j = Labels.Count; j < Rectangles.Count; j++)
                    if (Rectangles[i].IntersectsWith(Rectangles[j]))
                    {
                        var Vect = getPointDirection(Point.Add(Rectangles[j].Location, new Size(Rectangles[j].Size.Width / 2, Rectangles[j].Size.Height / 2)), Labels[i].Location);
                        Directions[Labels[i]] = Vect * 2 - Directions[Labels[i]];
                    }
                
                if (Directions[Labels[i]] < 0)
                    Directions[Labels[i]] += Math.PI * 2;
                if (Directions[Labels[i]] > Math.PI * 2)
                    Directions[Labels[i]] -= Math.PI * 2;

                    /*
                     * 0 относительно 90 = 180
                     * 45 относительно 90 = 135
                     * 90 относительно 90 = 90
                     * 
                     * (90 * 2) - исходный
                     * 
                     * 0 относительно 0 = 0
                     * 45 относительно 0 = -45
                     * 90 относительно 0 = -90
                     * 
                     * (0 * 2) - исходный
                     * 
                     * 0 относительно 45 = 90
                     * 45 относительно 45 = 45
                     * 90 относительно 45 = 0
                     * 
                     * Значит:
                     * 1) Приводим все векторы к [0; 2*pi]
                     * 2) VectorToTank * 2 - Direction[l]
                     * 3) Приводим Direction[l] к [0; 2*pi]
                     */
            }
        }

        private double getPointDirection(Point p1, Point p2)
        {
            double dx = p2.X - p1.X;
            double dy = p2.Y - p1.Y;

            return Math.Atan2(dy, dy);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (trackBar1.Value != 0)
            {
                timer1.Interval = 1000 / trackBar1.Value;
                timer1.Start();
            }
            else
                timer1.Stop();
        }
    }
}
