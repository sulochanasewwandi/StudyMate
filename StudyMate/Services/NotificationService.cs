using System.Windows.Forms;

namespace StudyMate.Services
{
    public class NotificationService
    {
        public void SendNotification(string message)
        {
            MessageBox.Show(message, "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}