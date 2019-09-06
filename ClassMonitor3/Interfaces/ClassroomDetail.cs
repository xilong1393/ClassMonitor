using ClassMonitor3.Model;
using ClassMonitor3.Service;
using ClassMonitor3.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassMonitor3.Interfaces
{
    public partial class ClassroomDetail : Form
    {
        public LinkLabel lb;
        List<Task> tasks = new List<Task>();
        List<CancellationTokenSource> tokenSourceList = new List<CancellationTokenSource>();
        public CancellationTokenSource TokenSource
        {
            get
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                tokenSourceList.Add(tokenSource);
                return tokenSource;
            }
            set { this.TokenSource = value; }
        }
        public ClassroomDetail(LinkLabel linkLabel)
        {
            
            this.lb = linkLabel;
            this.lb.Enabled = false;
            InitializeComponent();
            //ClassroomService classroomService = new ClassroomService();
            //ClassroomInfo classroomInfo = classroomService.GetClassroomInfo(Util.LoginInfo.sessionID, classroomID);
            //this.Text = classroomInfo.ClassroomName;
            ////listView1.View = View.Details;
            ////listView1.GridLines = true;
            //listView1.Items.Add(new ListViewItem(new[] { "PPCPublicIP", classroomInfo.PPCPublicIP }));
            //listView1.Items.Add(new ListViewItem(new[] { "IPCPrivateIP", classroomInfo.IPCPrivateIP }));
            //listView1.Items.Add(new ListViewItem(new[] { "PPCPort", classroomInfo.PPCPort+"" }));
            //listView1.Items.Add(new ListViewItem(new[] { "IPCPort", classroomInfo.IPCPort+"" }));
            //listView1.Items.Add(new ListViewItem(new[] { "WBNumber", classroomInfo.WBNumber + "" }));
            //listView1.Items.Add(new ListViewItem(new[] { "Status", classroomInfo.Status == 1 ? "On" : "Off" }));
            Text = ((ClassroomView)lb.Tag).ClassroomName;

            PictureBox p1 = new PictureBox();
            PictureBox p2 = new PictureBox();
            PictureBox p3 = new PictureBox();
            PictureBox p4 = new PictureBox();
            p1.BackColor = Color.Red;
            p2.BackColor = Color.Green;
            p3.BackColor = Color.Yellow;
            p4.BackColor = Color.Violet;
            p1.Dock = DockStyle.Fill;
            p2.Dock = DockStyle.Fill;
            p3.Dock = DockStyle.Fill;
            p4.Dock = DockStyle.Fill;
            p1.Tag = TokenSource;
            p2.Tag = TokenSource;
            p3.Tag = TokenSource;
            p4.Tag = TokenSource;
            p1.SizeMode = PictureBoxSizeMode.CenterImage;
            p2.SizeMode = PictureBoxSizeMode.CenterImage;
            p3.SizeMode = PictureBoxSizeMode.CenterImage;
            p4.SizeMode = PictureBoxSizeMode.CenterImage;
            p1.Name = lb.Name;
            p2.Name = lb.Name;
            p3.Name = lb.Name;
            p4.Name = lb.Name;

            panel1.Controls.Add(p1);
            panel2.Controls.Add(p2);
            panel3.Controls.Add(p3);
            panel4.Controls.Add(p4);

            tasks.Add(UpdatePictureBox(p1, "CameraVideo"));
            tasks.Add(UpdatePictureBox(p2, "whiteboard1"));
            tasks.Add(UpdatePictureBox(p3, "whiteboard2"));
            tasks.Add(UpdatePictureBox(p4, "TeacherScreen"));

        }

        private void ClassroomDetail_FormClosing(object sender, FormClosingEventArgs e)
        {

            foreach (CancellationTokenSource cts in tokenSourceList)
                cts.Cancel();
            tokenSourceList.RemoveRange(0, tokenSourceList.Count);
            tasks.RemoveRange(0, tasks.Count);

            lb.Enabled = true;
            ComboBox cb = (ComboBox)Helper.GetAll((lb.Parent),typeof(ComboBox)).First();
            cb.Items.Clear();
            cb.Items.AddRange(Helper.GetComboItems());
            cb.SelectedIndex = 0;
        }
        private Task UpdatePictureBox(PictureBox p, string text)
        {
            CancellationTokenSource tokenSource = (CancellationTokenSource)p.Tag;
            return Task.Run(() =>
            {
                int counter = 0;
                while (true)
                {
                    if (tokenSource.Token.IsCancellationRequested)
                    {
                        break;
                    }
                    counter %= 4;
                    counter++;
                    string filename = @"E:\Test\images\" + p.Name + @"\" + text + @"\" + text + counter + ".PNG";
                    try
                    {
                        using (FileStream loadStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                        {
                            Action action = () => { p.Image = Image.FromStream(loadStream); };
                            p.SafeInvoke(action, true);
                            Thread.Sleep(1000);
                        }
                    }
                    catch (Exception)
                    {
                        Action action = () => { p.BackColor = Color.Black; };
                        p.SafeInvoke(action, true);
                    }
                    finally
                    {
                        p.Image = null;
                    }
                }
            }, tokenSource.Token);
        }
    }
}
