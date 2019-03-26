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
        List<Task> tasks = new List<Task>();

        public static CancellationTokenSource storedTokenSource;

        delegate void ImageCallback(PictureBox p, Image image);
        public CancellationTokenSource TokenSource
        {
            get
            {
                if (storedTokenSource == null)
                {
                    storedTokenSource = new CancellationTokenSource();
                    return storedTokenSource;
                }
                else if (storedTokenSource.IsCancellationRequested)
                {
                    storedTokenSource = new CancellationTokenSource();
                    return storedTokenSource;
                }
                else {
                    return storedTokenSource;
                }
            }
            set { this.TokenSource = value; }
        }
        public CancellationToken ct
        {
            get { return TokenSource.Token; }
            set { this.ct = value; }
        }


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
            else
            {
                p.Width = 200;
                p.Height = 250;
            }
        }

        private void createPanel(List<ClassroomView> list)
        {
            flRightPanel.Controls.Clear();
            if (storedTokenSource != null)
                storedTokenSource.Cancel();

            tasks.RemoveRange(0,tasks.Count);
            
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
                tasks.Add(UpdatePictureBox(p, label, panel, ct));
            }
        }

        private void Panel_DoubleClick(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)(sender);
            Panel panel = (Panel)p.Parent;
            p_DoubleClick(panel,e);
        }

        private Task UpdatePictureBox(PictureBox p, Label label, Panel panel, CancellationToken token)
        {
            return Task.Run(() =>
            {
                int counter = 0;
                while (true)
                {
                    if (token.IsCancellationRequested) {
                        break;
                    }
                        
                    counter %= 4;
                    counter++;
                    string filename = @"E:\Test\images\" + p.Name + @"\ac" + counter + ".png";
                    try
                    {
                        using (Image newImg = new Bitmap(filename))
                        {
                            //Action action = () => { p.Image = newImg; };
                            //this.BeginInvoke(action);
                            var d = new ImageCallback(method);
                            this.Invoke(d, new object[] { p, newImg });
                            Thread.Sleep(1000);
                        }
                    }
                    catch (Exception ex)
                    {
                        Action action = () => { label.Text = ex.Message; label.BackColor = Color.Red; };
                        this.BeginInvoke(action);
                    }
                    finally
                    {
                        p.Image = null;
                    }
                }
            }, token);
        }

        private void method(PictureBox p, Image image)
        {
            p.Image = image;
        }

        private void hideLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tablelayout.ColumnStyles[1].Width = 90;
            tablelayout.ColumnStyles[0].Width = 0;
        }

        private void showLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
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

        private void lblUser_Click(object sender, EventArgs e)
        {
            storedTokenSource.Cancel();
            tasks.RemoveRange(0, tasks.Count);
            flRightPanel.Controls.Clear();
        }
    }
}
