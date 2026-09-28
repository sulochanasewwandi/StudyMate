using System;
using System.Drawing;
using System.Windows.Forms;
using StudyMate.Models;
using StudyMate.Services;

namespace StudyMate.Forms
{
    public partial class TasksForm : Form
    {
        private readonly TaskManager _taskManager;

        private TextBox _txtTaskName;
        private ComboBox _cmbSubject;
        private DateTimePicker _dtpDeadline;
        private ComboBox _cmbPriority;
        private DataGridView _dgvTasks;
        private Button _btnAddTask;
        private Button _btnCompleteTask;
        private Button _btnDeleteTask;

        public TasksForm(TaskManager taskManager)
        {
            _taskManager = taskManager;

            InitializeProgrammaticUI();
            LoadSubjectsDropdown();
            LoadTasksGrid();
        }

        private void InitializeProgrammaticUI()
        {
            this.Text = "Task Management";
            this.Size = new Size(720, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Row 1: Inputs
            Label lblName = new Label { Text = "Task Name:", Location = new Point(20, 15), AutoSize = true };
            _txtTaskName = new TextBox { Location = new Point(20, 35), Width = 160 };

            Label lblSubj = new Label { Text = "Subject:", Location = new Point(190, 15), AutoSize = true };
            _cmbSubject = new ComboBox { Location = new Point(190, 35), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblDate = new Label { Text = "Deadline:", Location = new Point(350, 15), AutoSize = true };
            _dtpDeadline = new DateTimePicker { Location = new Point(350, 35), Width = 130, Format = DateTimePickerFormat.Short };

            Label lblPrio = new Label { Text = "Priority:", Location = new Point(490, 15), AutoSize = true };
            _cmbPriority = new ComboBox { Location = new Point(490, 35), Width = 90, DropDownStyle = ComboBoxStyle.DropDownList };
            _cmbPriority.Items.AddRange(new object[] { "High", "Medium", "Low" });
            _cmbPriority.SelectedIndex = 0;

            _btnAddTask = new Button
            {
                Text = "Add Task",
                Location = new Point(590, 33),
                Width = 90,
                Height = 26,
                BackColor = Color.FromArgb(31, 41, 55),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _btnAddTask.Click += BtnAddTask_Click;

            // Grid
            _dgvTasks = new DataGridView
            {
                Location = new Point(20, 80),
                Size = new Size(660, 320),
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false
            };

            // Buttons
            _btnCompleteTask = new Button
            {
                Text = "Mark Complete ✅",
                Location = new Point(20, 415),
                Width = 160,
                Height = 32,
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _btnCompleteTask.Click += BtnCompleteTask_Click;

            _btnDeleteTask = new Button
            {
                Text = "Delete Task 🗑️",
                Location = new Point(190, 415),
                Width = 140,
                Height = 32,
                BackColor = Color.Tomato,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _btnDeleteTask.Click += BtnDeleteTask_Click;

            this.Controls.AddRange(new Control[] {
                lblName, _txtTaskName, lblSubj, _cmbSubject, lblDate, _dtpDeadline,
                lblPrio, _cmbPriority, _btnAddTask, _dgvTasks, _btnCompleteTask, _btnDeleteTask
            });
        }

        private void LoadSubjectsDropdown()
        {
            _cmbSubject.Items.Clear();
            foreach (var subject in _taskManager.Subjects)
            {
                _cmbSubject.Items.Add(subject);
            }
            if (_cmbSubject.Items.Count > 0) _cmbSubject.SelectedIndex = 0;
        }

        private void LoadTasksGrid()
        {
            _dgvTasks.DataSource = null;
            var displayList = _taskManager.Tasks.ConvertAll(t => new
            {
                t.Id,
                Task = t.Name,
                Subject = t.Subject.Name,
                Deadline = t.Deadline.ToShortDateString(),
                t.Priority,
                Status = t.IsCompleted ? "Completed" : "Pending"
            });
            _dgvTasks.DataSource = displayList;
        }

        private void BtnAddTask_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtTaskName.Text) || _cmbSubject.SelectedItem == null)
            {
                MessageBox.Show("Please enter a task name and select a subject.", "Warning");
                return;
            }

            _taskManager.AddTask(_txtTaskName.Text.Trim(), (Subject)_cmbSubject.SelectedItem, _dtpDeadline.Value, _cmbPriority.SelectedItem.ToString());
            _txtTaskName.Clear();
            LoadTasksGrid();
        }

        private void BtnCompleteTask_Click(object sender, EventArgs e)
        {
            if (_dgvTasks.CurrentRow != null)
            {
                int taskId = Convert.ToInt32(_dgvTasks.CurrentRow.Cells["Id"].Value);
                _taskManager.CompleteTask(taskId); // Fires TaskCompleted Custom Event!
                LoadTasksGrid();
            }
        }

        private void BtnDeleteTask_Click(object sender, EventArgs e)
        {
            if (_dgvTasks.CurrentRow != null)
            {
                int taskId = Convert.ToInt32(_dgvTasks.CurrentRow.Cells["Id"].Value);
                _taskManager.DeleteTask(taskId);
                LoadTasksGrid();
            }
        }
    }
}