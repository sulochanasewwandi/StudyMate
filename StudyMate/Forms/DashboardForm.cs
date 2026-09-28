using System;
using System.Drawing;
using System.Windows.Forms;
using StudyMate.Services;

namespace StudyMate.Forms
{
    public partial class DashboardForm : Form
    {
        private readonly TaskManager _taskManager;
        private readonly string _username;
        private readonly NotificationService _notificationService;

        private Label _lblWelcome;
        private Label _lblSubjectsCount;
        private Label _lblTasksCount;
        private Label _lblCompletedCount;

        private Button _btnNavSubjects;
        private Button _btnNavTasks;
        private Button _btnNavTimer;
        private Button _btnNavNotes;

        public DashboardForm(TaskManager taskManager, string username)
        {
            _taskManager = taskManager;
            _username = username;
            _notificationService = new NotificationService();

            // Subscribe to custom C# Event
            _taskManager.TaskCompleted += TaskManager_TaskCompleted;

            InitializeProgrammaticUI();
            UpdateMetrics();
        }

        private void InitializeProgrammaticUI()
        {
            this.Text = "StudyMate - Dashboard";
            this.Size = new Size(580, 360);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(243, 244, 246);

            _lblWelcome = new Label
            {
                Text = $"Welcome back, {_username}!",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55),
                Location = new Point(25, 20),
                AutoSize = true
            };

            // Status Cards Panel
            GroupBox grpMetrics = new GroupBox
            {
                Text = " Overview Metrics ",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(25, 60),
                Size = new Size(510, 110)
            };

            _lblSubjectsCount = new Label { Text = "Subjects: 0", Location = new Point(20, 35), Font = new Font("Segoe UI", 11), AutoSize = true };
            _lblTasksCount = new Label { Text = "Pending Tasks: 0", Location = new Point(180, 35), Font = new Font("Segoe UI", 11), AutoSize = true };
            _lblCompletedCount = new Label { Text = "Completed: 0", Location = new Point(350, 35), Font = new Font("Segoe UI", 11), AutoSize = true };

            grpMetrics.Controls.Add(_lblSubjectsCount);
            grpMetrics.Controls.Add(_lblTasksCount);
            grpMetrics.Controls.Add(_lblCompletedCount);

            // Navigation Buttons
            _btnNavSubjects = CreateNavButton("📚 Subjects", 25, 200);
            _btnNavTasks = CreateNavButton("✅ Tasks", 155, 200);
            _btnNavTimer = CreateNavButton("⏱️ Timer", 285, 200);
            _btnNavNotes = CreateNavButton("📝 Notes", 415, 200);

            _btnNavSubjects.Click += BtnNavSubjects_Click;
            _btnNavTasks.Click += BtnNavTasks_Click;
            _btnNavTimer.Click += BtnNavTimer_Click;
            _btnNavNotes.Click += BtnNavNotes_Click;

            this.Controls.Add(_lblWelcome);
            this.Controls.Add(grpMetrics);
            this.Controls.Add(_btnNavSubjects);
            this.Controls.Add(_btnNavTasks);
            this.Controls.Add(_btnNavTimer);
            this.Controls.Add(_btnNavNotes);
        }

        private Button CreateNavButton(string text, int x, int y)
        {
            Button btn = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(120, 80),
                BackColor = Color.FromArgb(31, 41, 55),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        public void UpdateMetrics()
        {
            _lblSubjectsCount.Text = $"Subjects: {_taskManager.Subjects.Count}";
            _lblTasksCount.Text = $"Pending: {_taskManager.Tasks.FindAll(t => !t.IsCompleted).Count}";
            _lblCompletedCount.Text = $"Completed: {_taskManager.Tasks.FindAll(t => t.IsCompleted).Count}";
        }

        private void TaskManager_TaskCompleted(object sender, TaskEventArgs e)
        {
            _notificationService.SendNotification($"Task Completed: '{e.Task.Name}'");
            UpdateMetrics();
        }

        private void BtnNavSubjects_Click(object sender, EventArgs e)
        {
            SubjectsForm form = new SubjectsForm(_taskManager, this);
            form.ShowDialog();
        }

        private void BtnNavTasks_Click(object sender, EventArgs e)
        {
            TasksForm form = new TasksForm(_taskManager);
            form.ShowDialog();
            UpdateMetrics();
        }

        private void BtnNavTimer_Click(object sender, EventArgs e)
        {
            TimerForm form = new TimerForm();
            form.ShowDialog();
        }

        private void BtnNavNotes_Click(object sender, EventArgs e)
        {
            NotesForm form = new NotesForm(_taskManager);
            form.ShowDialog();
        }
    }
}