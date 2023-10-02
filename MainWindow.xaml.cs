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
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            
            uncompleted_tasks_listview.SelectionChanged += ListView_SelectionChanged;
            completed_tasks_listview.SelectionChanged += ListView_SelectionChanged;
            PopulateListView(dB_completed);
            PopulateListView(dB_uncompleted);

        }

        private DB_completed dB_completed = new DB_completed();
        private DB_uncompleted dB_uncompleted = new DB_uncompleted();
        TaskItem taskItem=new TaskItem();
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
                    current_task_id = selectedItem.ID;
                    taskItem = selectedItem;
                    string[] parts = selectedItem.Name.Split(':');
                    if (parts.Length > 1)
                    {
                        // Видалити всі символи ":" і пробіли перед останнім ":"
                        taskItem.Name = parts[parts.Length - 1].Trim(':').Trim();
                    }
                    else
                    {
                        taskItem.Name = selectedItem.Name; // Якщо ":" не знайдено, залишити без змін
                    }
                }
                
            }

        } 
        public void PopulateListView<T>(T database) where T : DB
        {
            
            
            List<TaskItem> taskItems = new List<TaskItem>();
            if (database == dB_uncompleted)
            {
                uncompleted_tasks_listview.ItemsSource = null;
                taskItems =dB_uncompleted.taskread();
                uncompleted_tasks_listview.ItemsSource = taskItems;
                amount_today_tasks.Text = "Today tasks = " + dB_uncompleted.count_today_tasks.ToString();
                amount_tasks.Text = "Count tasks = " + taskItems.Count.ToString();
            }
            else
            {
                completed_tasks_listview.ItemsSource = null;
                taskItems = dB_completed.taskread();
                completed_tasks_listview.ItemsSource = taskItems;
            }
          
        }
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false; 
                int taskId = (int)checkBox.Tag;
                dB_uncompleted.transfer_task(taskId);
                PopulateListView(dB_completed);
                PopulateListView(dB_uncompleted);
            }
        }
        
        private void newtaskbutton_Click(object sender, RoutedEventArgs e)
        {
            is_new = true;
            Window1 window1 = new Window1(this, taskItem);
            window1.ShowDialog();
            PopulateListView(dB_uncompleted);

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
                Window1 window1 = new Window1(this, taskItem);
                window1.ShowDialog();
                PopulateListView(dB_completed);
                PopulateListView(dB_uncompleted);
            }

        }

        private void deletetask_Click(object sender, RoutedEventArgs e)
        {
            if (id_task_check())
            {
                dB_uncompleted.deletetask(current_task_id, "tasks");
                PopulateListView(dB_completed);
                PopulateListView(dB_uncompleted);
            }
        }

        private void statistics_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
