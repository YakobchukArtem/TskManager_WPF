using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TskManager_WPF
{
    public class TaskItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsDone { get; set; }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            
            uncompleted_tasks_listview.SelectionChanged += ListView_SelectionChanged;
            overdue_uncompleted_tasks_listview.SelectionChanged += ListView_SelectionChanged;
            PopulateListView();

        }

        private DB_completed dB_completed = new DB_completed();
        private DB_uncompleted dB_uncompleted = new DB_uncompleted();
        static public int current_task_id;
        public static bool is_new = true;
        
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            if (listView != null && listView.SelectedIndex >= 0)
            {
                var selectedItem = listView.SelectedItem as TaskItem; // Припустимо, що ваші елементи є типу TaskItem
                if (selectedItem != null)
                {
                    current_task_id = selectedItem.ID; // Отримання ідентифікатора з вибраного елемента
                }
            }

        } 
        public void PopulateListView()
        {
            List<TaskItem> taskItems = new List<TaskItem>();
            List<TaskItem> overduetaskItems = new List<TaskItem>();
            foreach (DataRow row in dB_uncompleted.showtable().Rows)
            {
                int id = Convert.ToInt32(row["ID"]);
                string name = row["Name"].ToString();
                string description = row["Description"].ToString();
                DateTime datetime = Convert.ToDateTime(row["datetime"]);
                bool isDone = Convert.ToBoolean(row["is_done"]);
                string doneStatus = isDone ? "Done" : "Not Done";
                ListViewItem item = new ListViewItem();
                item.Content = $"{datetime}: {name}";
                item.Tag = id; 
                if (datetime > DateTime.Now)
                {
                    taskItems.Add(new TaskItem
                    {
                        ID = id,
                        Name = $"{datetime}: {name}",
                        IsDone = isDone
                    });
                }
                else
                {
                    overduetaskItems.Add(new TaskItem
                    {
                        ID = id,
                        Name = $"{datetime}: {name}",
                        IsDone = isDone
                    });

                }

            }
            uncompleted_tasks_listview.ItemsSource = taskItems;
            overdue_uncompleted_tasks_listview.ItemsSource = overduetaskItems;
        }
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false; 
                int taskId = (int)checkBox.Tag;
                dB_uncompleted.transfer_task(taskId);
                PopulateListView();
            }
        }
        
        private void newtaskbutton_Click(object sender, RoutedEventArgs e)
        {
            is_new = true;
            Window1 window1 = new Window1(this);
            window1.ShowDialog();


        }

        private bool id_task_check()
        {
            if (current_task_id == 0)
            {
                MessageBox.Show("Завдання не вибрано");
                return false;
            }
            else return true;
        }

        private void changetask_Click(object sender, RoutedEventArgs e)
        {
            if (id_task_check())
            {
                is_new = false;
                Window1 window1 = new Window1(this);
                window1.ShowDialog();
                PopulateListView();
            }

        }

        private void deletetask_Click(object sender, RoutedEventArgs e)
        {
            if (id_task_check())
            {
                dB_uncompleted.deletetask(current_task_id, "tasks");
                PopulateListView();
            }
        }

        private void statistics_Click(object sender, RoutedEventArgs e)
        {
            Window2 window2 = new Window2();
            window2.ShowDialog();
        }
    }
}
