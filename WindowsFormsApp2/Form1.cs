using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
      

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addFive();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = new DataGridViewRow();
            DataGridViewTextBoxCell textboxcell = new DataGridViewTextBoxCell();
            textboxcell.Value = "aaa";
            row.Cells.Add(textboxcell);
        }

        private void addFive()
        {
            for (int i = 0; i <= 4; i++)
            {
                Panel b = new Panel();
                b.Width = 150;
                b.Height = 150;
                b.BackColor = Color.Purple;
                this.flowLayoutPanel1.Controls.Add(b);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (panel3.Visible)
            {
                //button2.Text = "Show Menu";
                this.panel3.Hide();
            }
            else {
                this.panel3.Show();
                //button2.Text = "Hide Menu";
            }
            
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            DataGridViewRow row = new DataGridViewRow();
            DataGridViewTextBoxCell textboxcell = new DataGridViewTextBoxCell();
            DataGridViewTextBoxCell textboxcel2 = new DataGridViewTextBoxCell();
            DataGridViewTextBoxCell textboxcel3 = new DataGridViewTextBoxCell();
            DataGridViewTextBoxCell textboxcel4 = new DataGridViewTextBoxCell();
            DataGridViewTextBoxCell textboxcel5 = new DataGridViewTextBoxCell();
            textboxcell.Value = "aaa";
            textboxcel2.Value = "BBB";
            textboxcel3.Value = "ccc";
            textboxcel4.Value = "DDD";
            textboxcel5.Value = "eee";
            row.Cells.Add(textboxcell);
            row.Cells.Add(textboxcel2);
            row.Cells.Add(textboxcel3);
            row.Cells.Add(textboxcel4);
            row.Cells.Add(textboxcel5);
            dataGridView1.Rows.Add(row);
        }
    }
}
