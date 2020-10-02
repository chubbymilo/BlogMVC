using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogMVC.ViewModels
{
    public class TodoItemViewModel
    {
        public int Id { set; get; }
        public string Task { set; get; }
        public string Detail { set; get; }
        public bool IsComplete { set; get; }
        public bool IsWorkingOn { set; get; }
        public string Status { set; get; }
        public DateTime? StartWorking { set; get; }
        public int PlanDuration { set; get; }
        public DateTime? Finished { set; get; }
        public int WorkDuration { set; get; }
        public DateTime? Created { set; get; }
        public int TotalDuration { set; get; }
    }
}
