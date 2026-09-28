using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyMate.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsEncrypted { get; set; }

        public Note(int id, string title, string content, bool isEncrypted = false)
        {
            Id = id;
            Title = title;
            Content = content;
            IsEncrypted = isEncrypted;
        }

        public override string ToString()
        {
            return IsEncrypted ? $"[Encrypted] {Title}" : Title;
        }
    }
}
