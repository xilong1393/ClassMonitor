using ClassMonitor3.Model;
using ClassMonitor3.Model.TestCourseModel;
using ClassMonitor3.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassMonitor3.Interfaces
{
    public partial class StartTestCourseForm : Form
    {
        //private TestCourseReturnModel testCourseReturnModel;
        public StartTestCourseForm(int classroomID)
        {
            InitializeComponent();
            this.Text = classroomID + "";
            fillModel(classroomID);

        }
        public async void fillModel(int classroomID) {
            var client = Helper.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var tuple = new { sessionID = LoginInfo.sessionID, classroomID = classroomID };
            HttpResponseMessage response = await client.PostAsJsonAsync("api/Operation/StartTestCourse",tuple);
            if (response.IsSuccessStatusCode)
            {
                TestCourseReturnModel testCourseReturnModel = await response.Content.ReadAsAsync<TestCourseReturnModel>();
                testCourseReturnModel.Qlist.Insert(0, new Quater { QuarterID = 0, QuarterDesc = "--Select Quarter--" });
                comboBox1.DataSource = testCourseReturnModel.Qlist;
                comboBox1.DisplayMember = "QuarterDesc";
                comboBox1.ValueMember = "QuarterID";
                comboBox2.DataSource = testCourseReturnModel.Clist;
                comboBox2.DisplayMember = "ClassTypeName";
                comboBox2.ValueMember = "ClassTypeID";
                comboBox3.DataSource = testCourseReturnModel.Flist;
                comboBox3.DisplayMember = "FileServerName";
                comboBox3.ValueMember = "FileServerID";
                SetDefaultNameforTestCourse(testCourseReturnModel.Classroom);


                comboBox4.DataSource = Util.Helper.TwentyFourArray();
                //comboBox4.SelectedValue = 0;
                comboBox5.DataSource = Util.Helper.SixtyArray();
                comboBox5.SelectedIndex = 15;
                comboBox6.DataSource = Util.Helper.SixtyArray();
                //comboBox6.SelectedValue = 0;

                this.Show();
            }
            else
            {
                MessageBox.Show("failure!");
            }
        }

        private void SetDefaultNameforTestCourse(Classroom classroom)
        {
            string sClassroomName = classroom.ClassroomName;

            if (classroom != null)
            {
                string sTime = " at " + DateTime.Now.ToString("yyyy-MM-dd HH mm ss");
                textBox1.Text = "Test Course in " + sClassroomName + sTime;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0 || comboBox5.SelectedIndex == 0||string.IsNullOrWhiteSpace(textBox2.Text)) {
                MessageBox.Show("illegal argument!");
                return;
            }
            
        }
    }
}
