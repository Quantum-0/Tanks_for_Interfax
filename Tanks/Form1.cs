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

                // Создаём направления для всех
                Directions.Add(l, rnd.NextDouble() * Math.PI * 2);
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
            foreach (var l in Labels)
            {
                // Перемещаем l
                l.Location = new Point(l.Location.X + (int)Math.Cos(Directions[l]), l.Location.Y + (int)Math.Sin(Directions[l]));

                // Проверяем коллизии
                
                // Меняем направление
            }
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
