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
        int Speed;
        Dictionary<Label, double> Directions = new Dictionary<Label, double>();
        List<Label> Labels = new List<Label>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Random rnd = new Random();
            // Ищем все Labels и заносим в список
            
            foreach (var l in Labels)
            {
                l.Text = "T";
                l.TextAlign = ContentAlignment.MiddleCenter;

                // Проверяем коллизию с другими элементами


                // Создаём направления для всех
                Directions.Add(l, rnd.NextDouble() * Math.PI * 2);
            }

            timer1.Interval = 1000 / Speed;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (var l in Labels)
            {
                // Перемещаем l

                //l.Top += Math.Cos(Directions[l]);

                // Проверяем коллизии
                
                // Меняем направление
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            Speed = trackBar1.Value;
            timer1.Interval = 1000 / Speed;
        }
    }
}
