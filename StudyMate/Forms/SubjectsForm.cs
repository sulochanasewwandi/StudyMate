using System;
using System.Drawing;
using System.Windows.Forms;
using StudyMate.Models;
using StudyMate.Services;

namespace StudyMate.Forms
{
    public partial class SubjectsForm : Form
    {
        private readonly TaskManager _taskManager;
        private readonly DashboardForm _dashboard;

        private TextBox _txtSubjectName;
        private ListBox _lstSubjects;
        private Button _btnAdd;
        private Button _btnDelete;

        public SubjectsForm(TaskManager taskManager, DashboardForm dashboard)
        {
            _taskManager = taskManager;
            _dashboard = dashboard;

            InitializeProgrammaticUI();
            LoadSubjects();
        }

        private void InitializeProgrammaticUI()
        {
            this.Text = "Manage Subjects";
            this.Size = new Size(400, 420);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            Label lblTitle = new Label
            {
                Text = "Subject Name:",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            _txtSubjectName = new TextBox
            {
                Location = new Point(20, 45),
                Width = 240,
                Font = new Font("Segoe UI", 10)
            };

            _btnAdd = new Button
            {
                Text = "Add Subject",
                Location = new Point(270, 44),
                Width = 95,
                Height = 28,
                BackColor = Color.FromArgb(31, 41, 55),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _btnAdd.Click += BtnAdd_Click;

            _lstSubjects = new ListBox
            {
                Location = new Point(20, 90),
                Size = new Size(345, 220),
                Font = new Font("Segoe UI", 10)
            };

            _btnDelete = new Button
            {
                Text = "Delete Selected Subject",
                Location = new Point(20, 325),
                Width = 345,
                Height = 35,
                BackColor = Color.Tomato,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _btnDelete.Click += BtnDelete_Click;

            this.Controls.Add(lblTitle);
            this.Controls.Add(_txtSubjectName);
            this.Controls.Add(_btnAdd);
            this.Controls.Add(_lstSubjects);
            this.Controls.Add(_btnDelete);
        }

        private void LoadSubjects()
        {
            _lstSubjects.Items.Clear();
            foreach (var subject in _taskManager.Subjects)
            {
                _lstSubjects.Items.Add(subject);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string name = _txtSubjectName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter a valid subject name.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _taskManager.AddSubject(name);
            _txtSubjectName.Clear();
            LoadSubjects();
            _dashboard.UpdateMetrics();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (_lstSubjects.SelectedItem is Subject selectedSubject)
            {
                _taskManager.RemoveSubject(selectedSubject.Id);
                LoadSubjects();
                _dashboard.UpdateMetrics();
            }
            else
            {
                MessageBox.Show("Please select a subject to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}