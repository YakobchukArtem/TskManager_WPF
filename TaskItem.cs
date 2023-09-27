using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TskManager_WPF
{
    public class TaskItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime dateTime { get; set; }
        public bool IsDone { get; set; }
        public bool Isoverdue { get; set; }

    }
}
