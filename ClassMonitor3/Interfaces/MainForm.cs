using ClassMonitor3.Model;
using ClassMonitor3.Util;
using NAudio.Wave;
using Newtonsoft.Json;
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
        private const int cGrip = 16;      // Grip size
        private const int cCaption = 32;   // Caption bar height;
        public MainForm()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            LoadData();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rc = new Rectangle(this.ClientSize.Width - cGrip, this.ClientSize.Height - cGrip, cGrip, cGrip);
            ControlPaint.DrawSizeGrip(e.Graphics, this.BackColor, rc);
            rc = new Rectangle(0, 0, this.ClientSize.Width, cCaption);
            e.Graphics.FillRectangle(Brushes.DarkBlue, rc);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            {  // Trap WM_NCHITTEST
                Point pos = new Point(m.LParam.ToInt32());
                pos = this.PointToClient(pos);
                if (pos.Y < cCaption)
                {
                    m.Result = (IntPtr)2;  // HTCAPTION
                    return;
                }
                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip)
                {
                    m.Result = (IntPtr)17; // HTBOTTOMRIGHT
                    return;
                }
            }
            base.WndProc(ref m);
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
                    label1.Text = "Remote Monitor - " + LoginInfo.user.FullName + ": " + LoginInfo.sessionID;

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

        private void createPanel(string value)
        {
            disposeWave();
            flRightPanel.Controls.Clear();
            foreach (CancellationTokenSource cts in tokenSourceList)
                cts.Cancel();
            tokenSourceList.RemoveRange(0, tokenSourceList.Count);
            tasks.RemoveRange(0, tasks.Count);

            int length = list.Count;
            for (int i = 0; i < length; i++)
            {
                Panel panel = new Panel();
                panel.BackColor = Color.LightGray;
                panel.Width = 250;
                panel.Height = 300;
                panel.BorderStyle = BorderStyle.FixedSingle;

                PictureBox p = new PictureBox();
                p.Name = "p" + i;
                p.SizeMode = PictureBoxSizeMode.Zoom;
                p.Dock = DockStyle.Fill;
                p.DoubleClick += Panel_DoubleClick;

                FlowLayoutPanel operationPanel = new FlowLayoutPanel();
                operationPanel.BackColor = Color.Purple;
                operationPanel.Dock = DockStyle.Bottom;
                operationPanel.Height = 25;
                operationPanel.Tag = i;

                LinkLabel detail = new LinkLabel();
                detail.Height = 23;
                detail.Width = 42;
                detail.TextAlign = ContentAlignment.MiddleCenter;
                detail.LinkBehavior = LinkBehavior.NeverUnderline;
                detail.BorderStyle = BorderStyle.FixedSingle;
                detail.Text = "Detail";
                detail.LinkColor = Color.White;
                detail.Click += Panel_Pop;
                detail.Tag = list[i];
                detail.Name = p.Name;
                operationPanel.Controls.Add(detail);

                LinkLabel sound = new LinkLabel();
                sound.Tag = i;
                sound.Height = 23;
                sound.Width = 46;
                sound.TextAlign = ContentAlignment.MiddleCenter;
                sound.LinkBehavior = LinkBehavior.NeverUnderline;
                sound.BorderStyle = BorderStyle.FixedSingle;
                sound.Text = "Sound";
                sound.LinkColor = Color.White;
                sound.Click += Play_SoundAsync;
                operationPanel.Controls.Add(sound);

                ComboBox cb = new ComboBox();
                cb.BackColor = Color.Purple;
                cb.ForeColor = Color.White;
                cb.Margin = new Padding(0, 0, 0, 0);
                cb.Width = 92;
                cb.ValueMember = "Value";
                cb.DisplayMember = "Text";
                cb.Items.AddRange(Helper.GetComboItems(list[i]));
                cb.Text = menuCB.Text;
                cb.SelectedValueChanged += Slide_Change;
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
                tasks.Add(UpdatePictureBox(p, label, panel, value, i));
            }
        }

        private async Task GetWave(int i)
        {
            try
            {
                ClassroomData classroomData = new ClassroomData(list[i].PPCPublicIP, list[i].PPCPort);
                string xml = await classroomData.GetAudioData(null, null, list[i].ClassroomID, null, null);
                byte[] bytes = await classroomData.GetBinary(xml);
                Stream t = new FileStream("video.wav", FileMode.Create);
                BinaryWriter b = new BinaryWriter(t);
                b.Write(bytes);
                t.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
        IWavePlayer waveOutDevice = null;
        AudioFileReader audioFileReader = null;

        private async void Play_SoundAsync(object sender, EventArgs e)
        {
            LinkLabel b = (LinkLabel)(sender);
            try
            {
                if (b.Text == "Sound")
                {
                    b.Text = "Stop";
                    Panel p = (Panel)b.Parent.Parent.Parent;
                    foreach (LinkLabel l in Helper.GetAll(p, b.GetType()).Where(a => a.Text != "Detail" && a != b))
                    {
                        l.Text = "Sound";
                    }
                    disposeWave();
                    await GetWave((int)b.Tag);
                    if (waveOutDevice == null) waveOutDevice = new WaveOut();
                    if (audioFileReader == null) audioFileReader = new AudioFileReader(@"video.wav");
                    waveOutDevice.Init(audioFileReader);
                    waveOutDevice.PlaybackStopped += (send, args) => AutoPlaySoundAsync(send, args, (int)b.Tag);
                    waveOutDevice.Play();
                }
                else
                {
                    b.Text = "Sound";
                    waveOutDevice?.Stop();
                    audioFileReader?.Dispose();
                    waveOutDevice?.Dispose();
                    audioFileReader = null;
                    waveOutDevice = null;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                //throw;
                Action action = () => { b.BackColor = Color.Red; };
                b.SafeInvoke(action, true);
            }
        }

        private async void AutoPlaySoundAsync(object sender, EventArgs e, int i)
        {
            disposeWave();
            await GetWave(i);
            if (waveOutDevice == null) waveOutDevice = new WaveOut();
            waveOutDevice.PlaybackStopped += (send, args) => AutoPlaySoundAsync(send, args, i);
            if (audioFileReader == null) audioFileReader = new AudioFileReader(@"video.wav");
            waveOutDevice.Init(audioFileReader);
            waveOutDevice.Play();
        }
        private void disposeWave()
        {
            if (audioFileReader != null && waveOutDevice != null)
            {
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
            tasks.Insert(taskIndex, UpdatePictureBox((PictureBox)(Helper.GetAll(cb.Parent.Parent, typeof(PictureBox)).First()), (Label)Helper.GetAll(cb.Parent.Parent, typeof(Label)).First(), (Panel)cb.Parent.Parent, cb.SelectedItem.ToString(), taskIndex));
        }
        private void Panel_Pop(object sender, EventArgs e)
        {
            LinkLabel b = (LinkLabel)(sender);
            CancellationTokenSource tokenSource = (CancellationTokenSource)b.Parent.Parent.Tag;
            tokenSource.Cancel();
            ClassroomDetailForm classroomDetail = new ClassroomDetailForm(b);
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

        private Task UpdatePictureBox(PictureBox p, Label label, Panel panel, string value, int i)
        {
            CancellationTokenSource tokenSource = (CancellationTokenSource)panel.Tag;
            return Task.Run(async () =>
            {
                while (true)
                {
                    if (tokenSource.Token.IsCancellationRequested)
                    {
                        break;
                    }
                    try
                    {
                        ClassroomData data = new ClassroomData(list[i].PPCPublicIP, list[i].PPCPort);
                        string xml = await data.GetImageString(null, null, list[i].ClassroomID, null, null, int.Parse(value));
                        byte[] bytes = await data.GetBinary(xml);
                        using (MemoryStream loadStream = new MemoryStream(bytes, 0, bytes.Length))
                        {
                            Action action = () => { p.Image = Image.FromStream(loadStream); };
                            p.SafeInvoke(action, true);
                            Thread.Sleep(2000);
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

        private async void button11_ClickAsync(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
                MessageBox.Show("Please select a classroom");
            else
            {
                int row = dataGridView.SelectedRows[0].Index;
                int classroomID = list[row].ClassroomID;
                var client = Helper.CreateClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //var tuple = new { sessionID = LoginInfo.sessionID, classroomID = classroomID };
                HttpResponseMessage response = await client.GetAsync("api/Operation/ClassroomInfo?classroomID=" + classroomID + "&sessionID=" + LoginInfo.sessionID);

                if (response.IsSuccessStatusCode)
                {
                    ClassroomInfoView classroomInfoView = await response.Content.ReadAsAsync<ClassroomInfoView>();
                    ClassroomInfoForm classroomInfoForm = new ClassroomInfoForm(classroomInfoView);
                    classroomInfoForm.Show();
                }
                else
                {
                    //MessageBox.Show(response.Content.ReadAsStringAsync().Result);
                    MessageBox.Show("failure!");
                }
            }
        }

        private void dataGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataGridViewRow row = dataGridView.Rows[e.RowIndex];
            if (row.Cells["PPCConnectionStatus"].Value?.ToString() == "True")
            {
                row.DefaultCellStyle.ForeColor = Color.Green;
            }
            if (row.Cells["WBNumber"].Value?.ToString()=="1")
            {
                row.Cells["WBNumber"].Value = "1/2";
                row.Cells["WBNumber"].Style.ForeColor = Color.Red;
            }
            if (row.Cells["KaptivoNumber"].Value?.ToString() == "1")
            {
                row.Cells["KaptivoNumber"].Value = "1/2";
                row.Cells["KaptivoNumber"].Style.ForeColor = Color.Red;
            }
        }

        private async void menuCB_SelectedIndexChangedAsync(object sender, EventArgs e)
        {
            try
            {
                int groupID = (int)comboBox.SelectedValue;
                await LoadClassroomList(LoginInfo.sessionID, groupID);
                dataGridView.DataSource = list;
                dataGridView.ClearSelection();
                //dataGridView.BackgroundColor = Color.Gray;

                createPanel(menuCB.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        private async Task LoadClassroomList(string sessionID, int groupID)
        {
            var client = Helper.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var tuple = new { sessionID = sessionID, groupID = groupID };
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

        private void dataGridView_MouseClick(object sender, MouseEventArgs e)
        {
            var ht = dataGridView.HitTest(e.X, e.Y);

            if (ht.Type == DataGridViewHitTestType.None)
            {
                dataGridView.ClearSelection();
            }
        }

        private async void btnStopCourse_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                btnStopCourse.Enabled = false;
                if (dataGridView.SelectedRows.Count == 0)
                    MessageBox.Show("Please select a classroom first");
                else
                {
                    int row = dataGridView.SelectedRows[0].Index;
                    int classroomID = list[row].ClassroomID;

                    var client = Helper.CreateClient();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.GetAsync("api/Operation/GetClassroomStatusbyClassroomID?classroomID=" + classroomID + "&sessionID=" + LoginInfo.sessionID);

                    if (response.IsSuccessStatusCode)
                    {
                        ClassroomEngineAgentStatus status = await response.Content.ReadAsAsync<ClassroomEngineAgentStatus>();
                        string prompt = "";
                        if (status.EngineStatus != "RECORDING A LECTURE" || status.ClassTypeID == null)
                        {
                            MessageBox.Show("There is not recording in this room!");
                            return;
                        }
                        else
                            prompt = GetStopPromptString(status);

                        if (MessageBox.Show(prompt, "",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                        {
                            return;
                        }
                        else
                        {
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            HttpResponseMessage resp = await client.GetAsync("api/Operation/StopRecord?classroomID=" + classroomID + "&sessionID=" + LoginInfo.sessionID);

                            if (resp.IsSuccessStatusCode)
                            {
                                //ExecutionResult result= await response.Content.ReadAsAsync<ExecutionResult>();
                                MessageBox.Show("succeeded!");
                            }
                            else
                            {
                                //MessageBox.Show(response.Content.ReadAsStringAsync().Result);
                                MessageBox.Show("failure!");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("failure!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btnStopCourse.Enabled = true;
            }
        }

        private async void btnPushSchedule_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                btnPushSchedule.Enabled = false;
                if (dataGridView.SelectedRows.Count == 0)
                    MessageBox.Show("Please select a classroom");
                else
                {
                    int row = dataGridView.SelectedRows[0].Index;
                    int classroomID = list[row].ClassroomID;
                    var client = Helper.CreateClient();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //var tuple = new { sessionID = LoginInfo.sessionID, classroomID = classroomID };
                    HttpResponseMessage response = await client.GetAsync("api/Operation/PushSchedule?classroomID=" + classroomID + "&sessionID=" + LoginInfo.sessionID);

                    if (response.IsSuccessStatusCode)
                    {
                        //ExecutionResult result= await response.Content.ReadAsAsync<ExecutionResult>();
                        MessageBox.Show("succeeded!");
                    }
                    else
                    {
                        //MessageBox.Show(response.Content.ReadAsStringAsync().Result);
                        MessageBox.Show("failure!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btnPushSchedule.Enabled = true;
            }
        }

        private async void button2_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                button2.Enabled = false;
                if (dataGridView.SelectedRows.Count == 0)
                    MessageBox.Show("Please select a classroom");
                else
                {
                    int row = dataGridView.SelectedRows[0].Index;
                    int classroomID = list[row].ClassroomID;

                    var client = Helper.CreateClient();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.GetAsync("api/Operation/GetClassroomStatusbyClassroomID?classroomID=" + classroomID + "&sessionID=" + LoginInfo.sessionID);

                    if (response.IsSuccessStatusCode)
                    {
                        ClassroomEngineAgentStatus status = await response.Content.ReadAsAsync<ClassroomEngineAgentStatus>();
                        string prompt = "";
                        string input = "";
                        if (status.EngineStatus != "RECORDING A LECTURE" || status.ClassTypeID == null)
                        {
                            MessageBox.Show("There is not recording in this room!");
                            return;
                        }
                        else if (status.ClassTypeID == 3)
                        {
                            prompt = GetAbortPromptString(status, status.ClassTypeID == 3, ref input);
                            if (MessageBox.Show(prompt, "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                            {
                                return;
                            }
                        }
                        else
                        {
                            prompt = GetAbortPromptString(status, status.ClassTypeID == 3, ref input);

                            string myinput = Microsoft.VisualBasic.Interaction.InputBox(prompt,
                               "confirm", "", -1, -1);
                            MessageBox.Show(myinput + ": " + input);

                            if (myinput != input || myinput == "")
                                return;
                        }

                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        //var tuple = new { sessionID = LoginInfo.sessionID, classroomID = classroomID };
                        HttpResponseMessage resp = await client.GetAsync("api/Operation/AbortRecord?classroomID=" + classroomID + "&sessionID=" + LoginInfo.sessionID);

                        if (resp.IsSuccessStatusCode)
                        {
                            //ExecutionResult result= await response.Content.ReadAsAsync<ExecutionResult>();
                            MessageBox.Show("succeeded!");
                        }
                        else
                        {
                            //MessageBox.Show(response.Content.ReadAsStringAsync().Result);
                            MessageBox.Show("failure!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("failure!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                button2.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
                MessageBox.Show("Please select a classroom");
            else
            {
                int row = dataGridView.SelectedRows[0].Index;
                StartTestCourseForm form = new StartTestCourseForm(list[row]);
                form.Show();
            }
        }

        private async void button6_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                button6.Enabled = false;
                if (dataGridView.SelectedRows.Count == 0)
                    MessageBox.Show("Please select a classroom");
                else
                {
                    int row = dataGridView.SelectedRows[0].Index;
                    int classroomID = list[row].ClassroomID;
                    var client = Helper.CreateClient();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //var tuple = new { sessionID = LoginInfo.sessionID, classroomID = classroomID };
                    HttpResponseMessage response = await client.GetAsync("api/Operation/CheckSchedule?classroomID=" + classroomID + "&sessionID=" + LoginInfo.sessionID);

                    if (response.IsSuccessStatusCode)
                    {
                        ExecutionResult result = await response.Content.ReadAsAsync<ExecutionResult>();
                        DataTable list = JsonConvert.DeserializeObject<DataTable>((string)result.Obj);
                        CheckScheduleForm cs = new CheckScheduleForm(list);
                        cs.Show();
                    }
                    else
                    {
                        //MessageBox.Show(response.Content.ReadAsStringAsync().Result);
                        MessageBox.Show("failure!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                button6.Enabled = true;
            }
        }

        private async void button10_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                button10.Enabled = false;
                if (dataGridView.SelectedRows.Count == 0)
                    MessageBox.Show("Please select a classroom");
                else
                {
                    int row = dataGridView.SelectedRows[0].Index;
                    int classroomID = list[row].ClassroomID;
                    var client = Helper.CreateClient();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //var tuple = new { sessionID = LoginInfo.sessionID, classroomID = classroomID };
                    HttpResponseMessage response = await client.GetAsync("api/Operation/ListLocalData?classroomID=" + classroomID + "&sessionID=" + LoginInfo.sessionID);

                    if (response.IsSuccessStatusCode)
                    {
                        ExecutionResult result = await response.Content.ReadAsAsync<ExecutionResult>();
                        string resultObj = result.Obj.ToString();
                        List<string> list = JsonConvert.DeserializeObject<List<string>>(resultObj);
                        ListLocalDataForm cs = new ListLocalDataForm(list, classroomID);
                        cs.Show();
                    }
                    else
                    {
                        //MessageBox.Show(response.Content.ReadAsStringAsync().Result);
                        MessageBox.Show("failure!");
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                button10.Enabled = true;
            }
        }

        private async void button5_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                button5.Enabled = false;
                if (dataGridView.SelectedRows.Count == 0)
                    MessageBox.Show("Please select a classroom");
                else
                {
                    int row = dataGridView.SelectedRows[0].Index;
                    int classroomID = list[row].ClassroomID;
                    var client = Helper.CreateClient();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //var tuple = new { sessionID = LoginInfo.sessionID, classroomID = classroomID };
                    HttpResponseMessage response = await client.GetAsync("api/Operation/PushPPCConfig?classroomID=" + classroomID + "&sessionID=" + LoginInfo.sessionID);

                    if (response.IsSuccessStatusCode)
                    {
                        //ExecutionResult result= await response.Content.ReadAsAsync<ExecutionResult>();
                        MessageBox.Show("succeeded!");
                    }
                    else
                    {
                        //MessageBox.Show(response.Content.ReadAsStringAsync().Result);
                        MessageBox.Show("failure!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                button5.Enabled = true;
            }
        }

        private async void button8_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                button8.Enabled = false;
                if (dataGridView.SelectedRows.Count == 0)
                    MessageBox.Show("Please select a classroom");
                else
                {
                    int row = dataGridView.SelectedRows[0].Index;
                    int classroomID = list[row].ClassroomID;
                    var client = Helper.CreateClient();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //var tuple = new { sessionID = LoginInfo.sessionID, classroomID = classroomID };
                    HttpResponseMessage response = await client.GetAsync("api/Operation/RebootPPC?classroomID=" + classroomID + "&sessionID=" + LoginInfo.sessionID);

                    if (response.IsSuccessStatusCode)
                    {
                        //ExecutionResult result= await response.Content.ReadAsAsync<ExecutionResult>();
                        MessageBox.Show("succeeded!");
                    }
                    else
                    {
                        //MessageBox.Show(response.Content.ReadAsStringAsync().Result);
                        MessageBox.Show("failure!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                button8.Enabled = true;
            }
        }

        private async void button9_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                button9.Enabled = false;
                if (dataGridView.SelectedRows.Count == 0)
                    MessageBox.Show("Please select a classroom");
                else
                {
                    int row = dataGridView.SelectedRows[0].Index;
                    int classroomID = list[row].ClassroomID;
                    var client = Helper.CreateClient();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //var tuple = new { sessionID = LoginInfo.sessionID, classroomID = classroomID };
                    HttpResponseMessage response = await client.GetAsync("api/Operation/RebootIPC?classroomID=" + classroomID + "&sessionID=" + LoginInfo.sessionID);

                    if (response.IsSuccessStatusCode)
                    {
                        //ExecutionResult result= await response.Content.ReadAsAsync<ExecutionResult>();
                        MessageBox.Show("succeeded!");
                    }
                    else
                    {
                        //MessageBox.Show(response.Content.ReadAsStringAsync().Result);
                        MessageBox.Show("failure!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                button9.Enabled = true;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox.Items.Count == 0)
                    MessageBox.Show("there is no group");
                else
                {
                    int groupID = (int)comboBox.SelectedValue;

                    GroupScheduleForm gs = new GroupScheduleForm(groupID);
                    gs.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected string GetStopPromptString(ClassroomEngineAgentStatus status)
        {
            string coursename = status.CourseName.Replace("\"", "").Replace("'", "");
            if (coursename.Length > 20)
            {
                coursename = coursename.Substring(0, 20);
            }
            string commandstring = "Stop";
            return string.Format("Do you want to {0} course: {1} in classroom {2}", commandstring, coursename, status.ClassroomName);
        }

        protected string GetAbortPromptString(ClassroomEngineAgentStatus status, bool isTestClass,ref string promtstr)
        {
            string coursename = status.CourseName.Replace("\"", "").Replace("'", "");
            if (coursename.Length > 20)
            {
                coursename = coursename.Substring(0, 20);
            }
            promtstr = coursename.Replace(" ", "");
            if (promtstr.Length > 8)
            {
                promtstr = promtstr.Substring(0, 8);
            }
            if (isTestClass == false)
            {
                promtstr = "ABORT" + promtstr;
            }

            string commandstring = "Abort";

            if (isTestClass == true)
            {
                return string.Format("Do you want to {0} course: {1} in classroom {2}", commandstring, coursename, status.ClassroomName);
            }
            else
            {
                return string.Format("If you want to {0} Course: {1} in classroom {2} ? Please input:  {3}", commandstring, coursename, status.ClassroomName, promtstr);
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Normal;
            else
                this.WindowState = FormWindowState.Maximized;
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }
        Point lastPoint;
        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }
    }
}