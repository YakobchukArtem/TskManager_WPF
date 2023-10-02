using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TskManager_WPF
{
    public class Statistics
    {
        public Statistics() { statistics_read(); }
        private int amount_completed_tasks_day { get; set; }
        private int amount_tasks_day { get;set;}
        private int amount_completed_tasks_week { get; set; }
        private int amount_tasks_month { get; set; }
        private int amount_completed_tasks_month { get; set; }
        private int amount_tasks_week { get; set; }
        List<TaskItem> taskItems = new List<TaskItem>();
        public void statistics_read() { 

            DB db = new DB("tasks");
            taskItems=db.get_all_tasks();
            DateTime startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek+1); // Початок тижня (Понеділок)
            DateTime endOfWeek = startOfWeek.AddDays(6); // Кінець тижня (неділя)
            foreach (var taskItem in taskItems)
            {
                if (taskItem.dateTime.Month == DateTime.Today.Month && taskItem.dateTime.Year == DateTime.Today.Year)
                {
                    amount_tasks_month++;
                    if (taskItem.IsDone == true) amount_completed_tasks_month++;
                }
                if (taskItem.dateTime.Date == DateTime.Today)
                {
                    amount_tasks_day++;
                    if (taskItem.IsDone == true) amount_completed_tasks_day++;
                }
                else if (taskItem.dateTime.Date >= startOfWeek.Date && taskItem.dateTime.Date <= endOfWeek.Date)
                {
                    amount_tasks_week++;
                    if (taskItem.IsDone == true) amount_completed_tasks_week++;
                }
                

            }
            amount_tasks_week += amount_tasks_day;
            amount_completed_tasks_week += amount_completed_tasks_day;
        }
        public int statistics_day()
        {
            return (100*amount_completed_tasks_day )/ amount_tasks_day;
        }
        public int statistics_week()
        {
            return (amount_completed_tasks_week * 100 ) / amount_tasks_week;
        }
        public int statistics_month()
        {
            return (amount_completed_tasks_month * 100 ) / amount_tasks_month;
        }

    }
}
