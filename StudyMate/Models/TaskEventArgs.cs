using System;
using StudyMate.Models;

namespace StudyMate.Services
{
    public class TaskEventArgs : EventArgs
    {
        public StudyTask Task { get; }

        public TaskEventArgs(StudyTask task)
        {
            Task = task;
        }
    }
}