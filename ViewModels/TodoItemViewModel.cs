using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name = "Started the task at")]
        public DateTime? StartWorking { set; get; }
        [Display(Name = "Planning Duration")]
        public string PlanDuration { set; get; }
        [Display(Name = "Completed at")]
        public DateTime? Finished { set; get; }
        [Display(Name = "Working Duration")]
        public string WorkDuration { set; get; }
        [Display(Name = "Created at")]
        public DateTime? Created { set; get; }
        [Display(Name = "Total Duration")]
        public string TotalDuration { set; get; }
    }
}
