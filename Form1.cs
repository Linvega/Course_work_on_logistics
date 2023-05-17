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
    public partial class Form1 : Form
    {
        public List<Start> start = new List<Start> { };
        public List<Cargo> cargos = new List<Cargo> { };

        public string[] status = { "Обработка","Ожидание","Исполнено", "ВНИМАНИЕ", "Просрочен" };
        public string[] status_start = { "Ожидание", "Завершён", "Провал" };
        public string[] priotity = { "Высший", "Высокий", "Средний", "Низкий" };

        public int number = 0;
        public int number_rock = 0;
        public int id_cell = -1;
        int id_cell_rock;
        int need = 0;

        Random rnd = new Random();

        
        public Form1()
        {
            InitializeComponent();
            dataGridView1.ShowCellToolTips = true;
            label8.BackColor = Color.Gray;
            timer1.Start();
            label15.TextAlign = ContentAlignment.MiddleRight;

            label6.Text = "Используемая дата: " + monthCalendar1.SelectionStart.Date.ToString("d");
        }




        private void button1_Click(object sender, EventArgs e)
        {
            CargoAdd CaAd = new CargoAdd();
            CaAd.FormClosing += (sender1, e1) =>
            {
                if (CaAd.a == 1)
                {
                    Cargo obj = new Cargo(CaAd.name, CaAd.priority, CaAd.mass, Convert.ToDateTime(CaAd.date));
                    obj.description = CaAd.desc;
                    if (obj.mass > 95)
                    {
                        obj.status = 3;
                        obj.status_s = 1;
                        obj.description_alert = "Превышена максимальная вместимость любого из ракетоносителей";
                    }
                    cargos.Add(obj);
                    addCargo();
                    reload();
                }
            };
            CaAd.Show();
        }

       
        public void addCargo()
        {
            id_cell = dataGridView1.RowCount;
            dataGridView1.Rows.Add(number, cargos[number].name, cargos[number].priority, cargos[number].mass+" т.", cargos[number].date.ToString("d"), status[cargos[number].status]);
            if (id_cell == 0)
            {
                dataGridView1.Rows[0].Cells[0].Selected = false;
            }
            for (int i = 0; i < id_cell; i++)
            {
                dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.White;
            }
            dataGridView1.Rows[id_cell].Cells[0].Style.BackColor = Color.Aqua;
            number++;
            resetDrawTableCargo();
            resetTableCargo();
        }
        public void resetDrawTableCargo()
        {
            for (int i = 0; i < number; i++)
            {

                int d_ay = cargos[i].date.Subtract(monthCalendar1.SelectionStart.Date).Days;
                if (d_ay < 7 && d_ay > 0 && cargos[i].status <= 1)
                {
                    dataGridView1.Rows[i].Cells[4].Style.BackColor = Color.Yellow;
                }
                else
                {
                    dataGridView1.Rows[i].Cells[4].Style.BackColor = Color.White;
                }

                if (cargos[i].priority == 0)
                {
                    dataGridView1.Rows[i].Cells[2].Style.BackColor = Color.White;
                }

                if (cargos[i].status == 1)
                {
                    dataGridView1.Rows[i].Cells[5].Style.BackColor = Color.Yellow;
                }
                else if (cargos[i].status == 2)
                {
                    dataGridView1.Rows[i].Cells[5].Style.BackColor = Color.GreenYellow;
                }
                else if (cargos[i].status == 3)
                {
                    dataGridView1.Rows[i].Cells[5].Style.BackColor = Color.Red;
                }
                else if (cargos[i].status == 4)
                {
                    dataGridView1.Rows[i].Cells[5].Style.BackColor = Color.IndianRed;
                    dataGridView1.Rows[i].Cells[4].Style.BackColor = Color.IndianRed;
                }
                else
                {
                    dataGridView1.Rows[i].Cells[5].Style.BackColor = Color.White;
                    if (cargos[i].priority == 0)
                    {
                        dataGridView1.Rows[i].Cells[2].Style.BackColor = Color.Red;
                    }
                }

            }
            
            if (dataGridView1.RowCount != 0)
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (i != id_cell)
                    {
                        dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.White;
                    }
                }

                dataGridView1.Rows[id_cell].Cells[0].Style.BackColor = Color.Aqua;
            }
        }

        public void resetTableCargo()
        {
            for (int i = 0; i < number; i++)
                {
                if (cargos[i].date <= monthCalendar1.SelectionStart.Date && cargos[i].status == 0)
                {
                    cargos[i].status = 4;
                }
                else if (cargos[i].date > monthCalendar1.SelectionStart.Date && cargos[i].status == 4) 
                {
                    cargos[i].status = 0;
                }
                }

            for (int i = 0; i < number; i++)
            {
                dataGridView1.Rows[i].Cells[5].Value = status[cargos[i].status];
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < start.Count; i++)
            {
                if (start[i].status == 0)
                {
                    for (int a = 0; a < cargos.Count; a++)
                    {
                        if (cargos[a].status < 2)
                        {
                            cargos[a].status = 0;
                            cargos[a].aoi = false;
                        }
                        dataGridView1.Rows[a].Cells[1].Style.BackColor = Color.White;

                        start[i].cargo_start.Clear();
                        start[i].result_mass = 0;
                    }
                }
            }
            reload();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (cargos[id_cell].status < 2 || (cargos[id_cell].status == 3 && cargos[id_cell].status_s == 1))
            {
                number -= 1;
                for (int i = 0; i < start.Count; i++)
                {
                    for (int a = 0; a < start[i].cargo_start.Count; a++)
                    {
                        if (start[i].cargo_start[a] == cargos[id_cell])
                        {
                            start[i].cargo_start.RemoveAt(a);
                            start[i].result_mass -= cargos[id_cell].mass;
                        }
                    }
                }
                cargos.RemoveAt(id_cell);
                dataGridView1.Rows.RemoveAt(id_cell);
                id_cell -= 1;
                if (id_cell == -1) { id_cell = 0; }
                resetDrawTableCargo();
                reload();
            }
            else
            {
                MessageBox.Show("Невозможно удалить исполненный или просроченный заказ",
                    "Ошибка",
                    MessageBoxButtons.OK);
                if (cargos[id_cell].status == 2)
                {

                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            StartAdd start_ = new StartAdd();
            start_.FormClosing += (sender1, e1) =>
            {
                if (start_.a == 1)
                {
                    Start obj_ = new Start(start_.rockets[start_.id]);
                    obj_.status = 0;
                    obj_.date = start_.date;
                    start.Add(obj_);
                    dataGridView2.Rows.Add(number_rock, start_.name, start_.rockets[start_.id].cargo_mass + " т.","", start_.date, status_start[obj_.status]);
                    dataGridView2.Rows[id_cell_rock].Cells[0].Selected = false;

                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        if (i != id_cell_rock)
                        {
                            dataGridView2.Rows[i].Cells[0].Style.BackColor = Color.White;
                        }
                        else
                        {
                            dataGridView2.Rows[i].Cells[0].Style.BackColor = Color.Aqua;
                        }
                    }

                    number_rock++;

                    reload();
                }
            };
            start_.Show();
        }


        private void button6_Click(object sender, EventArgs e)
        {
            int v1 = rnd.Next(0, 3);
            double v2 = rnd.Next(1, 21);
            string d = Convert.ToString(rnd.Next(1, 30)) + "." + Convert.ToString(rnd.Next(6, 8)) + ".2022";
            Cargo obj = new Cargo("Заказ №"+need,v1,v2, Convert.ToDateTime(d));
            obj.status = 0;
            obj.description = "Сгенерированный заказ";
            cargos.Add(obj);
            addCargo();
            reload();
            need++;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            richTextBox2.Clear();
            for (int i = 0; i < cargos.Count; i++)
            {
                richTextBox2.Text += cargos[i].aoi + " " + cargos[i].status + "\n";
            }
        }

        public void reload()
        {
            for (int i = 0; i < number; i++)
            {
                if (i != id_cell)
                {
                    dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.White;
                }
            }
            resetTableCargo();
            resetDrawTableCargo();
            if (dataGridView2.Rows.Count != 0)
            {
                //id_cell_rock = dataGridView2.CurrentCell.RowIndex;

                dataGridView2.Rows[id_cell_rock].Cells[0].Selected = false;

                progressBar1.Maximum = Convert.ToInt32(start[id_cell_rock].rocket_start[0].cargo_mass);
                progressBar1.Value = Convert.ToInt32(start[id_cell_rock].result_mass);
                richTextBox1.Text = start[id_cell_rock].rocket_start[0].name;
                richTextBox1.Text += "\nМаксимальная цена за вылет: $" + start[id_cell_rock].rocket_start[0].cost + " тыс.";
                double effect = Math.Round(start[id_cell_rock].result_mass * 100 / start[id_cell_rock].rocket_start[0].cargo_mass);
                if (effect < 80)
                {
                    richTextBox1.Text += String.Format("\nПолучено за вылет: $" + "{0:0.00}" + " тыс.", (start[id_cell_rock].rocket_start[0].cost / (start[id_cell_rock].rocket_start[0].cargo_mass * 0.8) * start[id_cell_rock].result_mass));
                }
                else 
                {
                    richTextBox1.Text += String.Format("\nПолучено за вылет: $" + "{0:0.00}" + " тыс.", start[id_cell_rock].rocket_start[0].cost);
                }
                label5.Text = start[id_cell_rock].result_mass + " из " + dataGridView2.Rows[id_cell_rock].Cells[2].Value;
                label8.Text = Convert.ToString(effect)+"%";
                if (effect >= 80)
                {
                    label8.ForeColor = Color.LightGreen;
                }
                else if (effect < 80 && effect >= 60)
                {
                    label8.ForeColor = Color.LightYellow;
                }
                else if (effect < 60 && effect >= 40)
                {
                    label8.ForeColor = Color.Salmon;
                }
                else if (effect < 40)
                {
                    label8.ForeColor = Color.DarkRed;
                }
                resetCentralTable();
                for (int i = 0; i < start.Count; i++)
                {
                    double effect_ = Math.Round(start[i].result_mass * 100 / start[i].rocket_start[0].cargo_mass);
                    dataGridView2.Rows[i].Cells[3].Value = Convert.ToString(effect_) + "%";
                    if (effect_ >= 80)
                    {
                        dataGridView2.Rows[i].Cells[3].Style.BackColor = Color.LightGreen;
                    }
                    else if (effect_ < 80 && effect_ >= 60)
                    {
                        dataGridView2.Rows[i].Cells[3].Style.BackColor = Color.LightYellow;
                    }
                    else if (effect_ < 60 && effect_ >= 40)
                    {
                        dataGridView2.Rows[i].Cells[3].Style.BackColor = Color.Salmon;
                    }
                    else if (effect_ < 40)
                    {
                        dataGridView2.Rows[i].Cells[3].Style.BackColor = Color.DarkRed;
                    }


                    if (Convert.ToDateTime(start[i].date) <= monthCalendar1.SelectionStart && start[i].status == 0)
                    {
                        start[i].status = rnd.Next(1,101);
                        if (start[i].status >= 30)
                        {
                            start[i].status = 1;
                        }
                        else
                        {
                            start[i].status = 2;
                        }

                        dataGridView2.Rows[i].Cells[5].Value = status_start[start[i].status];
                        if (start[i].status == 1)
                        {
                            dataGridView2.Rows[i].Cells[5].Style.BackColor = Color.GreenYellow;

                            
                        }
                        else if (start[i].status == 2)
                        {
                            dataGridView2.Rows[i].Cells[5].Style.BackColor = Color.Red;
                        }
                        for (int a = 0; a < cargos.Count; a++)
                        {
                            for (int b = 0; b < start[i].cargo_start.Count; b++)
                            {
                                if (cargos[a] == start[i].cargo_start[b])
                                {
                                    cargos[a].status = start[i].status + 1;
                                    if (cargos[a].status == 3)
                                    {
                                        cargos[a].description_alert = "Груз потерян в результате провала запуска";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            distribution();

            resetTableCargo();
            resetDrawTableCargo();
        }

        public void resetCentralTable()
        {
            dataGridView3.Rows.Clear();
            for (int i = 0; i < start[id_cell_rock].cargo_start.Count; i++)
            {
                dataGridView3.Rows.Add(i,start[id_cell_rock].cargo_start[i].name, start[id_cell_rock].cargo_start[i].priority, start[id_cell_rock].cargo_start[i].mass+" т.");
                dataGridView3.Rows[i].Cells[1].Style.BackColor = Color.PeachPuff;
                for (int a = 0; a < cargos.Count; a++)
                {
                    if (cargos[a] == start[id_cell_rock].cargo_start[i])
                    {
                        dataGridView1.Rows[a].Cells[1].Style.BackColor = Color.PeachPuff;
                    }
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < number; i++)
            {
                if (i != id_cell)
                {
                    dataGridView1.Rows[id_cell].Cells[0].Style.BackColor = Color.White;
                }
            }
            id_cell = dataGridView1.CurrentCell.RowIndex;
            resetDrawTableCargo();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (monthCalendar1.Visible == true)
            {
                monthCalendar1.Visible = false;
            }
            else
            {
                monthCalendar1.Visible = true;
            }
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            label6.Text = "Используемая дата: " + monthCalendar1.SelectionStart.Date.ToString("d");
            reload();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            id_cell_rock = dataGridView2.CurrentCell.RowIndex;

            dataGridView2.Rows[id_cell_rock].Cells[0].Selected = false;
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (i != id_cell_rock)
                {
                    dataGridView2.Rows[i].Cells[0].Style.BackColor = Color.White;
                }
                else
                {
                    dataGridView2.Rows[i].Cells[0].Style.BackColor = Color.Aqua;
                }
            }
            progressBar1.Maximum = Convert.ToInt32(start[id_cell_rock].rocket_start[0].cargo_mass);
            progressBar1.Value = Convert.ToInt32(start[id_cell_rock].result_mass);
            richTextBox1.Text = start[id_cell_rock].rocket_start[0].name;
            label13.Text = "$" + String.Format("{0:0.00}" + " тыс.",start[id_cell_rock].rocket_start[0].cost / start[id_cell_rock].rocket_start[0].cargo_mass);
            label14.Text = "$" + String.Format("{0:0.00}" + " тыс.", start[id_cell_rock].rocket_start[0].cost / (start[id_cell_rock].rocket_start[0].cargo_mass * 0.8));
            double effect = Math.Round(start[id_cell_rock].result_mass * 100 / start[id_cell_rock].rocket_start[0].cargo_mass);
            if (effect > 80)
            {
                label15.Text = "$" + String.Format("{0:0.00}" + " тыс.", start[id_cell_rock].rocket_start[0].cost / start[id_cell_rock].result_mass);
            }
            else
            {
                label15.Text = "$" + String.Format("{0:0.00}" + " тыс.", start[id_cell_rock].rocket_start[0].cost / (start[id_cell_rock].rocket_start[0].cargo_mass * 0.8));
            }
            label5.Text = start[id_cell_rock].result_mass + " из " + dataGridView2.Rows[id_cell_rock].Cells[2].Value;
            for (int i = 0; i < cargos.Count; i++)
            {
                dataGridView1.Rows[i].Cells[1].Style.BackColor = Color.White;
            }
            reload();

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string text = cargos[id_cell].description;
            MessageBox.Show("Приоритет: " + priotity[cargos[id_cell].priority] + "\n" + text,
                "Информация о заказе",
                MessageBoxButtons.OK);
        }

        public void distribution()
        {
            for (int r = 0; r < start.Count; r++)
            {
                for (int a = 0; a < 4; a++)
                {
                    for (int i = 0; i < cargos.Count; i++)
                    {
                        if (cargos[i].priority == a && cargos[i].status == 0)
                        {
                            if (cargos[i].mass + start[r].result_mass <= start[r].rocket_start[0].cargo_mass)
                            {
                                if (Convert.ToDateTime(start[r].date) <= cargos[i].date && cargos[i].aoi == false)
                                {
                                    if (start[r].status == 0)
                                    {
                                        start[r].result(cargos[i]);
                                        cargos[i].status = 1;
                                        cargos[i].aoi = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridViewCell cell_p = this.dataGridView1.Rows[e.RowIndex].Cells[2];
            cell_p.ToolTipText = "";

            DataGridViewCell cell = this.dataGridView1.Rows[e.RowIndex].Cells[5];

            if (cargos[dataGridView1.Rows[e.RowIndex].Index].status == 0)
            {
                cell.ToolTipText = "Ожидается очередь";
                if (cargos[dataGridView1.Rows[e.RowIndex].Index].priority == 0)
                {
                    cell_p.ToolTipText = "Отсутствуют рейсы для отправки груза с приоритетом 0";
                }
            }
            else if (cargos[dataGridView1.Rows[e.RowIndex].Index].status == 1)
            {
                cell.ToolTipText = "Ожидается отправка";
            }
            else if (cargos[dataGridView1.Rows[e.RowIndex].Index].status == 2)
            {
                cell.ToolTipText = "Груз отправлен";
            }
            else if (cargos[dataGridView1.Rows[e.RowIndex].Index].status == 3)
            {
                cell.ToolTipText = "ВНИМАНИЕ: "+ cargos[dataGridView1.Rows[e.RowIndex].Index].description_alert;
            }
            else if (cargos[dataGridView1.Rows[e.RowIndex].Index].status == 4)
            {
                cell.ToolTipText = "Истёк срок исполнения";
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (richTextBox2.Visible == true)
            {
                richTextBox2.Visible = false;
            }
            else
            {
                richTextBox2.Visible = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < start.Count; i++)
            {
                if (i == id_cell_rock)
                {
                    for (int a = 0; a < start[i].cargo_start.Count; a++)
                    {
                        for (int b = 0; b < cargos.Count; b++)
                        {
                            if (cargos[b] == start[i].cargo_start[a])
                            {
                                cargos[b].status = 0;
                            }
                        }
                    }
                    start.RemoveAt(i);
                    dataGridView2.Rows.RemoveAt(i);
                    id_cell_rock--;
                    if (id_cell_rock == -1) { id_cell_rock = 0; }
                    if (id_cell_rock != 0)
                    {
                        dataGridView2.Rows[id_cell_rock].Selected = true;
                        id_cell_rock = dataGridView2.CurrentCell.RowIndex;
                        dataGridView2.Rows[id_cell_rock].Selected = false;
                        for (int a = 0; a < dataGridView2.Rows.Count; a++)
                        {
                            if (a != id_cell_rock)
                            {
                                dataGridView2.Rows[a].Cells[0].Style.BackColor = Color.White;
                            }
                            else
                            {
                                dataGridView2.Rows[a].Cells[0].Style.BackColor = Color.Aqua;
                            }
                        }
                    }
                    for (int a = 0; a < cargos.Count; a++)
                    {
                        dataGridView1.Rows[a].Cells[1].Style.BackColor = Color.White;
                    }
                    resetDrawTableCargo();
                    resetCentralTable();
                    reload();
                }
            }

        }
    }
}
