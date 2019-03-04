using ClassMonitor.Model;
using ClassMonitor.Service;
using ClassMonitor.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassMonitor
{
   

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            ClassroomGroupService service = new ClassroomGroupService();
            List<ClassroomGroup> list = service.GetClassroomGroups();

            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Text", typeof(string));
            foreach (var data in list)
            {
                dt.Rows.Add(data.ClassroomGroupID, data.ClassroomgroupName);
            }
            //Setting Values
            comboBox.ValueMember = "ID";
            comboBox.DisplayMember = "Text";
            comboBox.DataSource = dt;

            if (LoginInfo.user != null)
                lblUser.Text = LoginInfo.user.FullName;
        }

        private void p_MouseHover(object sender, EventArgs e)
        {
            var p = (Panel)sender;
            p.BackColor = Color.Purple;
        }

        private void p_MouseLeave(object sender, EventArgs e)
        {
            var p = (Panel)sender;
            p.BackColor = Color.Teal;
        }

        private void p_DoubleClick(object sender, EventArgs e)
        {
            var p = (Panel)sender;
            p.BackColor = Color.Purple;
            if (p.Width < 400)
            {
                p.Width = p.Width + 200;
                p.Height = p.Height + 200;
            }
            else {
                p.Width = 200;
                p.Height = 200;
            }
        }

        private void createPanel(int num)
        {
            flRightPanel.Controls.Clear();

            for (int i = 0; i < num; i++)
            {
                Panel p = new Panel();
                p.BackColor = Color.Teal;
                p.Width = 200;
                p.Height = 200;
                p.MouseHover += new EventHandler(this.p_MouseHover);
                p.MouseLeave += new EventHandler(this.p_MouseLeave);
                p.DoubleClick += new EventHandler(this.p_DoubleClick);
                flRightPanel.Controls.Add(p);
            }
        }

        private void hideLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //panel2.Hide();

            //tablelayout.ColumnStyles[0].SizeType = SizeType.Percent;
            tablelayout.ColumnStyles[1].Width = 90;
            tablelayout.ColumnStyles[0].Width = 0;

        }

        private void showLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //panel2.Show();
            tablelayout.ColumnStyles[1].Width = 60;
            tablelayout.ColumnStyles[0].Width = 40;
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Object selectedItem = comboBox.SelectedValue;
            int num = int.Parse(selectedItem.ToString());
            if (selectedItem != null) {
                ClassroomService service = new ClassroomService();
                List<ClassroomView> list = service.GetClassroomByGroupID(num);
                dataGridView.DataSource = list;
                dataGridView.DefaultCellStyle.ForeColor = Color.Blue;

                createPanel(list.Count);
            }
        }

        private void dataGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataGridViewRow row = dataGridView.Rows[e.RowIndex];
            if (row.Cells["PPCConnectionStatus"].Value?.ToString() == "True")
            {
                row.DefaultCellStyle.ForeColor = Color.Green;
            }
        }
    }
}
