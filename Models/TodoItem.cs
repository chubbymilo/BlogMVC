using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogMVC.Models
{
    public class TodoItem
    {
        public int Id { set; get; }
        public DateTime? Created { set; get; } = DateTime.Now;
        public string Task { set; get; }
        public string Detail { set; get; }
        public bool IsComplete { set; get; }
        public bool IsWorkingOn { set; get; }
        public DateTime? StartWorking { set; get; }
        public DateTime? Finished { set; get; }
    }
}
