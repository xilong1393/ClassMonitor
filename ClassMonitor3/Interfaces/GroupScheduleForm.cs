using ClassMonitor3.Util;
using System;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Forms;

namespace ClassMonitor3.Interfaces
{
    public partial class GroupScheduleForm : Form
    {
        int groupID = 0;
        public GroupScheduleForm(int groupID)
        {
            InitializeComponent();
            this.groupID = groupID;
            //dateTimePicker1.MinDate = DateTime.Today;
            dateTimePicker1.Value=DateTime.Today;
        }

        private async void dateTimePicker1_ValueChangedAsync(object sender, EventArgs e)
        {
            try
            {
                var client = Helper.CreateClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/Operation/GroupSchedule?groupID=" + groupID + "&sessionID=" + LoginInfo.sessionID);
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        DataTable result = await response.Content.ReadAsAsync<DataTable>();
                        dataGridView1.DataSource = result;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("failure!");
                    }
                }
                else
                {
                    MessageBox.Show("failure!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
