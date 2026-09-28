using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace StudyMate.Forms
{
    public partial class TimerForm : Form
    {
        private Label _lblDisplay;
        private Label _lblStatus;
        private Button _btnStart;
        private Button _btnPause;
        private Button _btnReset;

        private Thread _timerThread;
        private int _secondsRemaining = 1500; // 25 minutes
        private bool _isRunning = false;

        public TimerForm()
        {
            InitializeProgrammaticUI();
        }

        private void InitializeProgrammaticUI()
        {
            this.Text = "Study Timer (Multithreading)";
            this.Size = new Size(340, 260);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            _lblDisplay = new Label
            {
                Text = "25:00",
                Font = new Font("Segoe UI", 36, FontStyle.Bold),
                Location = new Point(20, 20),
                Size = new Size(280, 60),
                TextAlign = ContentAlignment.MiddleCenter
            };

            _btnStart = new Button { Text = "Start", Location = new Point(30, 100), Width = 75, Height = 32 };
            _btnPause = new Button { Text = "Pause", Location = new Point(120, 100), Width = 75, Height = 32 };
            _btnReset = new Button { Text = "Reset", Location = new Point(210, 100), Width = 75, Height = 32 };

            _btnStart.Click += BtnStart_Click;
            _btnPause.Click += BtnPause_Click;
            _btnReset.Click += BtnReset_Click;

            _lblStatus = new Label
            {
                Text = "Status: Ready",
                Location = new Point(20, 160),
                Size = new Size(280, 25),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Gray
            };

            this.Controls.AddRange(new Control[] { _lblDisplay, _btnStart, _btnPause, _btnReset, _lblStatus });
            this.FormClosing += TimerForm_FormClosing;
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (_isRunning) return;

            _isRunning = true;
            _lblStatus.Text = "Status: Timer Running...";
            _lblStatus.ForeColor = Color.Green;

            // Multithreading demonstration: Start countdown loop in background thread
            _timerThread = new Thread(RunCountdown) { IsBackground = true };
            _timerThread.Start();
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            _isRunning = false;
            _lblStatus.Text = "Status: Paused";
            _lblStatus.ForeColor = Color.Orange;
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            _isRunning = false;
            _secondsRemaining = 1500;
            UpdateTimerDisplay();
            _lblStatus.Text = "Status: Ready";
            _lblStatus.ForeColor = Color.Gray;
        }

        private void RunCountdown()
        {
            while (_isRunning && _secondsRemaining > 0)
            {
                Thread.Sleep(1000); // Wait 1 second
                _secondsRemaining--;

                // Thread-Safe UI Invocation using Invoke/InvokeRequired
                UpdateTimerDisplay();
            }

            if (_secondsRemaining == 0)
            {
                _isRunning = false;
                SafeUpdateStatus("Status: Time's Up! Take a break.");
            }
        }

        private void UpdateTimerDisplay()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(UpdateTimerDisplay));
                return;
            }

            int mins = _secondsRemaining / 60;
            int secs = _secondsRemaining % 60;
            _lblDisplay.Text = $"{mins:D2}:{secs:D2}";
        }

        private void SafeUpdateStatus(string statusText)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => SafeUpdateStatus(statusText)));
                return;
            }
            _lblStatus.Text = statusText;
            _lblStatus.ForeColor = Color.Red;
        }

        private void TimerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _isRunning = false; // Gracefully stop thread loop on form exit
        }
    }
}