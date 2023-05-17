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
    public partial class CargoAdd : Form
    {
        public CargoAdd()
        {
            InitializeComponent();
        }


        public int a = 0;
        public int priority;
        public string name;
        public string desc;
        public double mass;
        public string date;

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null || maskedTextBox1.Text == null || comboBox1.SelectedItem == null || dateTimePicker1.Text == null)
            {
                MessageBox.Show(
                "Одно из ключевых полей не заполнено или заполнено неверно",
                "Ошибка",
                MessageBoxButtons.OK);
            }
            else
            {
                name = textBox1.Text;
                mass = Convert.ToDouble(maskedTextBox1.Text);
                priority = comboBox1.SelectedIndex;
                date = dateTimePicker1.Text;
                if (richTextBox1.Text == "")
                {
                    desc = "Информация отсутствует";
                }
                else
                {
                    desc = richTextBox1.Text;
                }
                a = 1;
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
