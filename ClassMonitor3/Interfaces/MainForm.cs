using ClassMonitor3.Model;
using ClassMonitor3.Service;
using ClassMonitor3.Util;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassMonitor3.Interfaces
{
    public partial class MainForm : Form
    {
        List<Task> tasks = new List<Task>();
        List<ClassroomView> list = new List<ClassroomView>();
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

        public MainForm()
        {
            InitializeComponent();
            LoadData();
        }
        public async void LoadData()
        {
            try
            {
                List<ClassroomGroup> list = await GetProductAsync();
                if (list != null)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ID", typeof(int));
                    dt.Columns.Add("Text", typeof(string));
                    foreach (var data in list)
                    {
                        dt.Rows.Add(data.ClassroomGroupID, data.ClassroomgroupName);
                    }
                    comboBox.ValueMember = "ID";
                    comboBox.DisplayMember = "Text";
                    comboBox.DataSource = dt;
                }

                if (LoginInfo.user != null)
                    this.Text = "Remote Monitor - " + LoginInfo.user.FullName + ": " + LoginInfo.sessionID;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        static async Task<List<ClassroomGroup>> GetProductAsync()
        {
            string path = @"api/classroom/getclassroomgrouplist";
            List<ClassroomGroup> list = null;
            HttpResponseMessage response = await Helper.CreateClient().GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                list = await response.Content.ReadAsAsync<List<ClassroomGroup>>();
            }
            return list;
        }

        private void createPanel(string text)
        {
            flRightPanel.Controls.Clear();
            foreach (CancellationTokenSource cts in tokenSourceList)
                cts.Cancel();
            tokenSourceList.RemoveRange(0, tokenSourceList.Count);
            tasks.RemoveRange(0, tasks.Count);

            int length = list.Count;
            for (int i = 0; i < length; i++)
            {
                Panel panel = new Panel();
                panel.BackColor = Color.Teal;
                panel.Width = 250;
                panel.Height = 300;

                PictureBox p = new PictureBox();
                p.Name = "p" + i;
                p.SizeMode = PictureBoxSizeMode.CenterImage;
                p.Dock = DockStyle.Fill;
                p.DoubleClick += Panel_DoubleClick;

                FlowLayoutPanel operationPanel = new FlowLayoutPanel();
                operationPanel.BackColor = Color.Gray;
                operationPanel.Dock = DockStyle.Bottom;
                operationPanel.Height = 50;
                operationPanel.Tag = i;

                LinkLabel detail = new LinkLabel();
                detail.Height = 23;
                detail.Width = 40;
                detail.TextAlign = ContentAlignment.MiddleCenter;
                detail.LinkBehavior = LinkBehavior.NeverUnderline;
                //b.AutoSize = true;
                detail.BorderStyle = BorderStyle.FixedSingle;
                detail.Text = "detail";
                detail.LinkColor = Color.Black;
                detail.Click += Panel_Pop;
                detail.Tag = list[i].ClassroomID;
                detail.Name = p.Name;
                operationPanel.Controls.Add(detail);

                LinkLabel sound = new LinkLabel();
                sound.Height = 23;
                sound.Width = 40;
                sound.TextAlign = ContentAlignment.MiddleCenter;
                sound.LinkBehavior = LinkBehavior.NeverUnderline;
                //sound.AutoSize = true;
                sound.BorderStyle = BorderStyle.FixedSingle;
                sound.Text = "play";
                sound.LinkColor = Color.Black;
                sound.Click += Play_Sound;
                operationPanel.Controls.Add(sound);

                ComboBox cb = new ComboBox();
                cb.BackColor = Color.Gray;
                cb.Margin= new Padding(0, 0, 0, 0);
                cb.Width = 92;
                cb.ValueMember = "Value";
                cb.DisplayMember = "Text";
                cb.Items.AddRange(Helper.GetComboItems());
                cb.Text = text;
                cb.SelectedIndexChanged += Slide_Change;
                operationPanel.Controls.Add(cb);

                Label label = new Label();
                label.Text = list[i].ClassroomName;
                label.BackColor = Color.Purple;
                label.Dock = DockStyle.Bottom;
                label.ForeColor = Color.White;
                label.TextAlign = ContentAlignment.MiddleCenter;

                panel.Controls.Add(p);
                panel.Controls.Add(operationPanel);
                panel.Controls.Add(label);
                panel.Tag = TokenSource;
                flRightPanel.Controls.Add(panel);
                tasks.Add(UpdatePictureBox(p, label, panel, text));
            }
        }
        IWavePlayer waveOutDevice = null;
        AudioFileReader audioFileReader = null;

        private void Play_Sound(object sender, EventArgs e)
        {
            LinkLabel b = (LinkLabel)(sender);

            if (audioFileReader != null && waveOutDevice != null)
            {
                waveOutDevice.Stop();
                audioFileReader.Dispose();
                waveOutDevice.Dispose();
                audioFileReader = null;
                waveOutDevice = null;
            }

            if (waveOutDevice == null) waveOutDevice = new WaveOut();
            if (audioFileReader == null) audioFileReader = new AudioFileReader(@"C:\Users\xyu42\Music\Grace.mp3");

            if (b.Text == "play")
            {
                b.Text = "stop";
                waveOutDevice.Init(audioFileReader);
                waveOutDevice.Play();

                Panel p = (Panel)b.Parent.Parent.Parent;
                foreach (LinkLabel l in Helper.GetAll(p, b.GetType()).Where(a=>a.Text!="detail"&&a!=b)) {
                    l.Text = "play";
                }
                try
                {
                    LogService logService = new LogService();
                    UserOperationLog userOperationLog = new UserOperationLog();
                    userOperationLog.SessionID = LoginInfo.sessionID;
                    userOperationLog.Operation = "Play_Sound";
                    userOperationLog.OperationTime = DateTime.Now;
                    logService.LogUserOperation(userOperationLog);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else {
                b.Text = "play";
                waveOutDevice.Stop();
                audioFileReader.Dispose();
                waveOutDevice.Dispose();
                audioFileReader = null;
                waveOutDevice = null;
            }
        }
        private void Slide_Change(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)(sender);
            CancellationTokenSource tokenSource = (CancellationTokenSource)cb.Parent.Parent.Tag;
            int taskIndex = (int)cb.Parent.Tag;
            tokenSource.Cancel();
            tokenSourceList.Remove(tokenSource);
            tasks.RemoveAt(taskIndex);
            ((Panel)cb.Parent.Parent).Tag = TokenSource;
            tasks.Insert(taskIndex, UpdatePictureBox((PictureBox)(Helper.GetAll(cb.Parent.Parent, typeof(PictureBox)).First()), (Label)Helper.GetAll(cb.Parent.Parent, typeof(Label)).First(), (Panel)cb.Parent.Parent, cb.Text));
        }
        private void Panel_Pop(object sender, EventArgs e)
        {
            LinkLabel b = (LinkLabel)(sender);
            CancellationTokenSource tokenSource = (CancellationTokenSource)b.Parent.Parent.Tag;
            tokenSource.Cancel();
            ClassroomDetail classroomDetail = new ClassroomDetail(b, int.Parse(b.Tag.ToString()));
            classroomDetail.Show();
        }

        private void Panel_DoubleClick(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)(sender);
            Panel panel = (Panel)p.Parent;
            p_DoubleClick(panel, e);
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
                p.Width = 250;
                p.Height = 300;
            }
        }

        private Task UpdatePictureBox(PictureBox p, Label label, Panel panel, string text)
        {
            CancellationTokenSource tokenSource = (CancellationTokenSource)panel.Tag;
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
                    catch (Exception ex)
                    {
                        Action action = () => { label.Text = ex.Message; label.BackColor = Color.Red; };
                        label.SafeInvoke(action, true);
                    }
                    finally
                    {
                        p.Image = null;
                    }
                }
            }, tokenSource.Token);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do You Want to Logout?", "Logout", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result.Equals(DialogResult.OK))
            {
                this.Close();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ClassroomInfoForm classroomInfoForm = new ClassroomInfoForm();
            classroomInfoForm.Show();
        }

        private void dataGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataGridViewRow row = dataGridView.Rows[e.RowIndex];
            if (row.Cells["PPCConnectionStatus"].Value?.ToString() == "True")
            {
                row.DefaultCellStyle.ForeColor = Color.Green;
            }
        }

        private async void menuCB_SelectedIndexChangedAsync(object sender, EventArgs e)
        {
            int groupID = (int)comboBox.SelectedValue;
            await LoadClassroomList(LoginInfo.sessionID,groupID);
            dataGridView.DataSource = list;
            createPanel(menuCB.SelectedItem.ToString());
        }
        private async Task LoadClassroomList(string sessionID, int groupID) {
            var client = Helper.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var tuple = new { sessionID= sessionID, groupID= groupID};
            HttpResponseMessage response = await client.PostAsJsonAsync("api/classroom/GetClassroomListByGroupID", tuple);
            if (response.IsSuccessStatusCode)
            {
                list = await response.Content.ReadAsAsync<List<ClassroomView>>();
            }
        }
        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            menuCB.DisplayMember = "Text";
            menuCB.ValueMember = "Value";
            menuCB.DataSource = Helper.GetComboItems();
        }

        private void btnStartCourse_Click(object sender, EventArgs e)
        {
            if (dataGridView.CurrentCell.ColumnIndex != 0)
                MessageBox.Show("Please select a classroom!");
        }
    }
}
