using ClassMonitor3.Model;
using ClassMonitor3.Util;
using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
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
                operationPanel.BackColor = Color.Gray;
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
                detail.LinkColor = Color.Black;
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
                sound.LinkColor = Color.Black;
                sound.Click += Play_SoundAsync;
                //sound.Enabled = false;
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
                tasks.Add(UpdatePictureBox(p, label, panel, text,i));
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
                    b.BackColor = Color.Gray;
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
            tasks.Insert(taskIndex, UpdatePictureBox((PictureBox)(Helper.GetAll(cb.Parent.Parent, typeof(PictureBox)).First()), (Label)Helper.GetAll(cb.Parent.Parent, typeof(Label)).First(), (Panel)cb.Parent.Parent, cb.Text, taskIndex));
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

        private Task UpdatePictureBox(PictureBox p, Label label, Panel panel, string text, int i)
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
                        string xml = await data.GetImageString(null, null, list[i].ClassroomID, null, null);
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
                        //MessageBox.Show(ex.ToString());
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
        }

        private async void menuCB_SelectedIndexChangedAsync(object sender, EventArgs e)
        {
            int groupID = (int)comboBox.SelectedValue;
            await LoadClassroomList(LoginInfo.sessionID,groupID);
            dataGridView.DataSource = list;
            dataGridView.ClearSelection();

            createPanel(menuCB.SelectedItem.ToString());
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
                        if (status.EngineStatus != "RECORDING A LECTURE" || status.ClassTypeID == null) {
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
            
        }

        private async void btnPushSchedule_ClickAsync(object sender, EventArgs e)
        {
            try
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
            
        }

        private async void button2_ClickAsync(object sender, EventArgs e)
        {
            try
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
            
        }

        private async void button10_ClickAsync(object sender, EventArgs e)
        {
            try
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
            
        }

        private async void button5_ClickAsync(object sender, EventArgs e)
        {
            try
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
            
        }

        private async void button8_ClickAsync(object sender, EventArgs e)
        {
            try
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
            
        }

        private async void button9_ClickAsync(object sender, EventArgs e)
        {
            try
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

    }
}
