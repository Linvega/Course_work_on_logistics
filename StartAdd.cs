using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logistic
{
    public partial class StartAdd : Form
    {
        public List<Rocket> rockets = new List<Rocket> { };
        public int a = 0;
        public int id = 0;
        public string name;
        public string date;
        public StartAdd()
        {

            Rocket Zenit = new Rocket("Зенит (3SL)", 13, 70);
            Zenit.description = "Трёхступенчетая ракета разработанная в СССР/России";
            rockets.Add(Zenit);

            Rocket Saturn = new Rocket("Saturn 1(B)", 17, 350);
            Saturn.description = "Двухступенчатая ракета для доставки тяжёлых грузов разработанная в США";
            rockets.Add(Saturn);

            Rocket Falcon = new Rocket("Falcon 9", 22, 62 );
            Falcon.description = "Ракета с многоразовой первой ступенью разработанная компанией SpaceX";
            rockets.Add(Falcon);

            Rocket Delta = new Rocket("Delta IV", 14, 160);
            Delta.description = "Двуступенчатая ракета с пятью разными модивикациями разработанная компанией Boeing";
            rockets.Add(Delta);

            Rocket SLS = new Rocket("Space Launch System", 95, 500);
            SLS.description = "Сверхтяжёлая ракета для миссий за пределами земной орбиты, разработана Boeing";
            rockets.Add(SLS);

            InitializeComponent();
            for (int i = 0; i < rockets.Count; i++)
            {
                comboBox1.Items.Add(rockets[i].name);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox1.Text += "Грузоподъёмность: " + rockets[comboBox1.SelectedIndex].cargo_mass + " т.";
            richTextBox1.Text += "\n" + "Стоимость запуска: " + rockets[comboBox1.SelectedIndex].cost + " млн. долл.";
            richTextBox1.Text += "\n" + rockets[comboBox1.SelectedIndex].description;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null || dateTimePicker1.Text == null)
            {
                MessageBox.Show(
                "Одно из ключевых полей не заполнено или заполнено неверно",
                "Ошибка",
                MessageBoxButtons.OK);
            }
            else
            {
                name = comboBox1.Text;
                date = dateTimePicker1.Text;
                id = comboBox1.SelectedIndex;
                a = 1;
                this.Close();
            }
        }
    }
}
