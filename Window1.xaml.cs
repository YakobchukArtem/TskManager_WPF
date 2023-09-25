using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TskManager_WPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1
    {
        public Window1(MainWindow parentwindow )
        {
            InitializeComponent();
            this.parentwindow = parentwindow;
            if (!MainWindow.is_new) { checkbox1.Visibility = Visibility.Visible; addnewtask_textblock.Text = "Change Task"; }
        }
        MainWindow parentwindow;
        
        public string name { get { return name_textbox.Text; } }
        public string description { get { return description_textbox.Text; } }
        public DateTime date { get { return date_picker.SelectedDate ?? DateTime.Now; } }
        public bool is_completed { get { if (checkbox1.IsChecked == true) return true; else return false; } }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DB_uncompleted dB_uncompleted = new DB_uncompleted();
            if (MainWindow.is_new)
            {
                dB_uncompleted.newtask(name, description, date, is_completed);
                parentwindow.PopulateListView();
                Close();
            }
            else
            {
                dB_uncompleted.change_task(MainWindow.current_task_id, name, description, date, is_completed);
                Close();
            }
        }
    }
}
