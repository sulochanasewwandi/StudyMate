using System;

namespace StudyMate.Models
{
    public class StudyTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Subject Subject { get; set; }
        public DateTime Deadline { get; set; }
        public string Priority { get; set; }
        public bool IsCompleted { get; set; }

        public StudyTask(int id, string name, Subject subject, DateTime deadline, string priority)
        {
            Id = id;
            Name = name;
            Subject = subject;
            Deadline = deadline;
            Priority = priority;
            IsCompleted = false;
        }
    }
}