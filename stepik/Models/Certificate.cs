using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stepik.Models
{
    public class Certificate
    {
        public string? Title { get; set; }
        public DateTime IssueDate { get; set; }
        public int Grade { get; set; }
    }
}
