using System;
using System.Collections.Generic;
using StudyMate.Models;

namespace StudyMate.Services
{
    public class TaskManager
    {
        public List<Subject> Subjects { get; set; } = new List<Subject>();
        public List<StudyTask> Tasks { get; set; } = new List<StudyTask>();
        public List<Note> Notes { get; set; } = new List<Note>();

        // Event declaration using TaskEventArgs
        public event EventHandler<TaskEventArgs> TaskCompleted;

        // Subject Methods
        public void AddSubject(string name)
        {
            int nextId = Subjects.Count + 1;
            Subjects.Add(new Subject(nextId, name));
        }

        public void RemoveSubject(int subjectId)
        {
            Subjects.RemoveAll(s => s.Id == subjectId);
        }

        // Task Methods
        public void AddTask(string name, Subject subject, DateTime deadline, string priority)
        {
            int nextId = Tasks.Count + 1;
            Tasks.Add(new StudyTask(nextId, name, subject, deadline, priority));
        }

        public void DeleteTask(int taskId)
        {
            Tasks.RemoveAll(t => t.Id == taskId);
        }

        public void CompleteTask(int taskId)
        {
            var task = Tasks.Find(t => t.Id == taskId);
            if (task != null)
            {
                task.IsCompleted = true;
                // Trigger event for subscribers (like DashboardForm)
                TaskCompleted?.Invoke(this, new TaskEventArgs(task));
            }
        }
    }
}