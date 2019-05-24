using System;
using System.ComponentModel;
using System.Linq;

namespace ProjectTaskManagement.Models.DataAccess.Entity
{
    public class Task
    {
        public int TaskId { get; set; }
        public int ProjectId { get; set; }
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [ReadOnly(true)]
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public Nullable<DateTime> StartingDate { get; set; } = null;
        public Nullable<DateTime> EndingDate { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.Pending;
        public Nullable<int> DemandId { get; set; }

        public Project Project { get; set; }
        public Employee Employee { get; set; }

        public bool IsFirstTask(int taskId)
        {
            var db = new Context.TaskManagerDbContext();
            var task = db.Task.Include("Project").First(t => t.TaskId == taskId);
            var ownProject = db.Project.First(t => t.ProjectId == task.ProjectId);

            if (ownProject != null && ownProject.StartingDate == null)
            {
                return true;
            }
            return false;
        }

        public bool IsLastTaskCompleted(int taskId)
        {
            using (var db = new Context.TaskManagerDbContext())
            {
                var task = db.Task.Include("Project").First(t => t.TaskId == taskId);
                var allTasks = (from t in db.Task
                                where t.ProjectId == task.ProjectId
                                orderby t.EndingDate
                                select t).ToList();
                var counter = 0;

                foreach (var item in allTasks)
                {
                    if (item.EndingDate == null)
                    {
                        if (++counter > 1)
                        {
                            return false;
                        }
                    }
                }

                task.Project.IsActive = false;
                db.Entry(task.Project).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return true;
        }
    }
}