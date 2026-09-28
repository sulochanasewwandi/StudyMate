using System;
using System.Drawing;
using System.Windows.Forms;
using StudyMate.Services;

namespace StudyMate.Forms
{
    public partial class LoginForm : Form
    {
        private TextBox _txtUsername;
        private TextBox _txtPassword;
        private Button _btnLogin;
        private Label _lblTitle;
        private Label _lblStatus;

        public LoginForm()
        {
            InitializeProgrammaticUI();
        }

        private void InitializeProgrammaticUI()
        {
            this.Text = "StudyMate - Login";
            this.Size = new Size(360, 320);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(243, 244, 246);

            _lblTitle = new Label
            {
                Text = "Welcome to StudyMate",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55),
                Location = new Point(30, 25),
                AutoSize = true
            };

            Label lblUsername = new Label
            {
                Text = "Username:",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Location = new Point(30, 75),
                AutoSize = true
            };

            _txtUsername = new TextBox
            {
                Location = new Point(30, 95),
                Width = 280,
                Font = new Font("Segoe UI", 10),
                Text = "Student" // Default text for easy testing
            };

            Label lblPassword = new Label
            {
                Text = "Password:",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Location = new Point(30, 135),
                AutoSize = true
            };

            _txtPassword = new TextBox
            {
                Location = new Point(30, 155),
                Width = 280,
                Font = new Font("Segoe UI", 10),
                PasswordChar = '●',
                Text = "1234"
            };

            _btnLogin = new Button
            {
                Text = "Login",
                Location = new Point(30, 200),
                Width = 280,
                Height = 38,
                BackColor = Color.FromArgb(31, 41, 55),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            _btnLogin.FlatAppearance.BorderSize = 0;
            _btnLogin.Click += BtnLogin_Click;

            _lblStatus = new Label
            {
                Text = "",
                Location = new Point(30, 245),
                AutoSize = true,
                ForeColor = Color.Red
            };

            this.Controls.Add(_lblTitle);
            this.Controls.Add(lblUsername);
            this.Controls.Add(_txtUsername);
            this.Controls.Add(lblPassword);
            this.Controls.Add(_txtPassword);
            this.Controls.Add(_btnLogin);
            this.Controls.Add(_lblStatus);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = _txtUsername.Text.Trim();
            if (string.IsNullOrWhiteSpace(username))
            {
                _lblStatus.Text = "Please enter a username.";
                return;
            }

            // Hide Login window and open Dashboard
            this.Hide();
            TaskManager taskManager = new TaskManager();
            DashboardForm dashboard = new DashboardForm(taskManager, username);
            dashboard.FormClosed += (s, args) => this.Close(); // Close app when Dashboard closes
            dashboard.Show();
        }
    }
}