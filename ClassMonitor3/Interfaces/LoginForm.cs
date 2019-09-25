using ClassMonitor3.Model;
using ClassMonitor3.Models;
using ClassMonitor3.Util;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassMonitor3.Interfaces
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnLogin_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                UserLoginForm userLoginForm = new UserLoginForm();
                userLoginForm.UserName = txtName.Text;
                userLoginForm.Password = txtPwd.Text;
                User user = await GetUserAsync(userLoginForm);
                if (user != null)
                {
                    LoginInfo.sessionID = Helper.GenerateSessionID();
                    LoginInfo.user = user;
                    this.Hide();
                    var mainForm = new MainForm();
                    mainForm.ShowDialog();
                    this.Close();
                }
                else {
                    MessageBox.Show("Please provide correct username and password and try again!");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
           

        }
        static async Task<User> GetUserAsync(UserLoginForm userLoginForm)
        {
            //string path = "http://localhost:8080/api/user/GetUserLogin" + "?username=" + txtName.Text + "&password=" + txtPwd.Text;
            var client=Helper.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //HttpResponseMessage response = await client.PostAsJsonAsync("api/user/GetUserLogin", userLoginForm);

            //response.EnsureSuccessStatusCode();
            //User user= await response.Content.ReadAsAsync<User>();
            User user = null;
            var tuple = new { UserName = userLoginForm.UserName, Password = userLoginForm.Password };
            HttpResponseMessage response = await client.PostAsJsonAsync("api/user/GetUserLogin", tuple);
            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadAsAsync<User>();
            }
            return user;
        }

        private void LoginForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            MessageBox.Show(e.KeyChar.ToString());
            if (e.KeyChar == 13)
            {
                MessageBox.Show("Enter key pressed");
            }
        }
    }
}
