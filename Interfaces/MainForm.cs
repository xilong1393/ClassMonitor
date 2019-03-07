using ClassMonitor.Model;
using ClassMonitor.Service;
using ClassMonitor.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading;
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
            if (p.Width < 400)
            {
                p.Width = p.Width + 200;
                p.Height = p.Height + 200;
            }
            else {
                p.Width = 200;
                p.Height = 250;
            }
        }

        private void createPanel(List<ClassroomView> list)
        {
            flRightPanel.Controls.Clear();

            int length = list.Count;
            for (int i= 0;i<length; i++)
            {
                Panel panel = new Panel();
                panel.BackColor = Color.Teal;
                panel.Width = 200;
                panel.Height = 250;

                PictureBox p = new PictureBox();
                p.Name = "p" + i;
                p.SizeMode = PictureBoxSizeMode.CenterImage;
                p.Dock = DockStyle.Fill ;
                p.DoubleClick += Panel_DoubleClick;

                Label label = new Label();
                label.Text = list[i].ClassroomName;
                label.BackColor = Color.Purple;
                label.Dock = DockStyle.Bottom;
                label.ForeColor = Color.White;
                label.TextAlign = ContentAlignment.MiddleCenter;

                panel.Controls.Add(p);
                panel.Controls.Add(label);
                flRightPanel.Controls.Add(panel);
                updatePictureBox(p);
            }
        }

        private void Panel_DoubleClick(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)(sender);
            Panel panel = (Panel)p.Parent;
            p_DoubleClick(panel,e);
        }

        private async void updatePictureBox(PictureBox p)
        {
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    int counter = 0;
                    while (true)
                    {
                        counter %= 4;
                        counter++;
                        if (p.Image != null)
                            p.Image.Dispose();
                        p.Image = Image.FromFile(@"E:\Test\images\" + p.Name + "\\ac" + counter + ".png");
                        Thread.Sleep(1000);
                    }
                });
            }
            catch (Exception ex)
            {
                Label lbl = new Label();
                lbl.AutoSize = true;
                lbl.Text = ex.ToString();
                flowLayoutPanel1.Controls.Add(lbl);
                updatePictureBox(p);
            }
            
        }

        private void P_LoadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Thread.Sleep(1000);
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

                createPanel(list);
            }
        }

        private void dataGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataGridViewRow row = dataGridView.Rows[e.RowIndex];
            if (row.Cells["PPCConnectionStatus"].Value?.ToString() == "True")
            {
                row.DefaultCellStyle.ForeColor = Color.Green;
            }
            if (row.Cells["CameraStatus"].Value?.ToString() == "On")
            {
                row.Cells["CameraStatus"].Style.ForeColor = Color.Blue;
            }
        }
    }
}
