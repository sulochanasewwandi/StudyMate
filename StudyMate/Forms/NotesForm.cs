using System;
using System.Drawing;
using System.Windows.Forms;
using StudyMate.Models;
using StudyMate.Services;

namespace StudyMate.Forms
{
    public partial class NotesForm : Form
    {
        private readonly TaskManager _taskManager;
        private readonly EncryptionService _encryptionService;

        private TextBox _txtTitle;
        private RichTextBox _rtbContent;
        private ListBox _lstNotes;
        private Button _btnEncryptSave;
        private Button _btnDecrypt;
        private Label _lblStatus;

        public NotesForm(TaskManager taskManager)
        {
            _taskManager = taskManager;
            _encryptionService = new EncryptionService();

            InitializeProgrammaticUI();
            LoadNotesList();
        }

        private void InitializeProgrammaticUI()
        {
            this.Text = "Private Notes (AES Cryptography)";
            this.Size = new Size(540, 420);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            Label lblTitle = new Label { Text = "Title:", Location = new Point(20, 15), AutoSize = true };
            _txtTitle = new TextBox { Location = new Point(20, 35), Width = 300 };

            Label lblContent = new Label { Text = "Note Content:", Location = new Point(20, 70), AutoSize = true };
            _rtbContent = new RichTextBox { Location = new Point(20, 90), Size = new Size(300, 200) };

            _btnEncryptSave = new Button
            {
                Text = "Encrypt & Save 🔐",
                Location = new Point(20, 300),
                Size = new Size(140, 35),
                BackColor = Color.FromArgb(31, 41, 55),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _btnEncryptSave.Click += BtnEncryptSave_Click;

            _btnDecrypt = new Button
            {
                Text = "Decrypt Note 🔓",
                Location = new Point(180, 300),
                Size = new Size(140, 35),
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _btnDecrypt.Click += BtnDecrypt_Click;

            Label lblSaved = new Label { Text = "Saved Notes:", Location = new Point(340, 15), AutoSize = true };
            _lstNotes = new ListBox { Location = new Point(340, 35), Size = new Size(160, 255) };

            _lblStatus = new Label
            {
                Text = "Status: Ready",
                Location = new Point(20, 345),
                AutoSize = true,
                ForeColor = Color.Gray
            };

            this.Controls.AddRange(new Control[] {
                lblTitle, _txtTitle, lblContent, _rtbContent, _btnEncryptSave, _btnDecrypt, lblSaved, _lstNotes, _lblStatus
            });
        }

        private void LoadNotesList()
        {
            _lstNotes.Items.Clear();
            foreach (var note in _taskManager.Notes)
            {
                _lstNotes.Items.Add(note);
            }
        }

        private void BtnEncryptSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtTitle.Text) || string.IsNullOrWhiteSpace(_rtbContent.Text))
            {
                MessageBox.Show("Please enter both title and content.", "Warning");
                return;
            }

            // Using EncryptionService (implementing IEncryptable)
            string encryptedText = _encryptionService.Encrypt(_rtbContent.Text);

            Note note = new Note(_taskManager.Notes.Count + 1, _txtTitle.Text.Trim(), encryptedText, isEncrypted: true);
            _taskManager.Notes.Add(note);

            _txtTitle.Clear();
            _rtbContent.Clear();
            LoadNotesList();

            _lblStatus.Text = "Status: Note encrypted and saved successfully!";
            _lblStatus.ForeColor = Color.Green;
        }

        private void BtnDecrypt_Click(object sender, EventArgs e)
        {
            if (_lstNotes.SelectedItem is Note selectedNote)
            {
                if (selectedNote.IsEncrypted)
                {
                    _txtTitle.Text = selectedNote.Title;
                    _rtbContent.Text = _encryptionService.Decrypt(selectedNote.Content);
                    _lblStatus.Text = "Status: Note decrypted for viewing!";
                    _lblStatus.ForeColor = Color.Blue;
                }
            }
            else
            {
                MessageBox.Show("Select a note from the saved list to decrypt.", "Information");
            }
        }
    }
}