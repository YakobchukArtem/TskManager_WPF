using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace TskManager_WPF
{
    public class DB
    {
        public DB(string tableName)
        {
            table_name = tableName;
        }
        protected static string connectdata = "Server=localhost; Database=taskmanager;port=3306;User Id=root;pwd=Artem1390;";
        protected MySqlConnection connection = new MySqlConnection(connectdata);
        protected string table_name;
        protected MySqlDataAdapter mysqladapter;
        protected MySqlCommand cmd;
        public int count_today_tasks { get; set; }


        public void newtask(string name, string description, DateTime datetime, bool is_completed)
        {
            try
            {
                connection.Open();

                string script_newtask = $"INSERT INTO {table_name} (name, description, datetime) VALUES (@Name, @Description, @DateTime)";

                cmd = new MySqlCommand(script_newtask, connection);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@DateTime", datetime);
                cmd.ExecuteNonQuery();
                connection.Close();
                sort_db();
            }
            catch
            {
                MessageBox.Show("Failed connection");
            }
        }

        

        public void sort_db()
        {
            connection.Open();
            string script_sort = $"SELECT * FROM {table_name} ORDER BY datetime ASC;";
            cmd = new MySqlCommand(script_sort, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        public DataTable showtable()
        {
            DataTable datatable = new DataTable();
            try
            {
                connection.Open();
                string script_showtask = $"Select * from {table_name} ORDER BY datetime ASC;";
                mysqladapter = new MySqlDataAdapter(script_showtask, connectdata);
                mysqladapter.Fill(datatable);
                connection.Close();

            }
            catch
            {
                MessageBox.Show("Failed");
            }
            return datatable;
        }
        public List<TaskItem> taskread()
        {
            List<TaskItem> taskItems = new List<TaskItem>();
            foreach (DataRow row in showtable().Rows)
            {
                int id = Convert.ToInt32(row["ID"]);
                string name = row["Name"].ToString();
                string description = row["Description"].ToString();
                DateTime datetime = Convert.ToDateTime(row["datetime"]);
                bool isDone = Convert.ToBoolean(row["is_done"]);
                string doneStatus = isDone ? "Done" : "Not Done";
                bool isoverdue;

                if (datetime > DateTime.Now)
                {
                    isoverdue = false;
                }
                else
                {
                    isoverdue = true;
                }
                if (datetime.Date == DateTime.Today) count_today_tasks++;
                taskItems.Add(new TaskItem
                {
                    ID = id,
                    Name = $"{datetime}: {name}",
                    Description = description,
                    dateTime = datetime,
                    IsDone = isDone,
                    Isoverdue = isoverdue
                });

            }
            return taskItems;

        }

            public void deletetask(int current_task_id, string table_Name)
        {
            try
            {
                connection.Open();
                string deleteScript = $"DELETE FROM {table_Name} WHERE id = {current_task_id}";
                cmd = new MySqlCommand(deleteScript, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch
            {
                MessageBox.Show("Failed");
            }

        }

        public void change_task(int current_task_id, string name, string description, DateTime dateTime, bool is_completed)
        {
            try
            {
                connection.Open();
                string updateScript = $"UPDATE {table_name} SET name = @Name, description = @Description, datetime = @DateTime, is_done=@is_completed WHERE id = @TaskID";

                MySqlCommand cmd = new MySqlCommand(updateScript, connection);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@DateTime", dateTime);
                cmd.Parameters.AddWithValue("@TaskID", current_task_id);
                cmd.Parameters.AddWithValue("@is_completed", is_completed);
                cmd.ExecuteNonQuery();
                connection.Close();
                sort_db();
            }
            catch
            {
                MessageBox.Show("Failed");
            }
        }
        public void change_task(int current_task_id, bool is_completed)
        {
            try
            {
                connection.Open();
                string updateScript = $"UPDATE {table_name} SET is_done=@is_completed WHERE id = @TaskID";
                MySqlCommand cmd = new MySqlCommand(updateScript, connection);
                cmd.Parameters.AddWithValue("@TaskID", current_task_id);
                cmd.Parameters.AddWithValue("@is_completed", is_completed);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch
            {
                MessageBox.Show("Failed");
            }
        }
        public void transfer_task(int taskId)
        {
            try
            {
                connection.Open();
                string transfer_Script = $"INSERT INTO tasks_completed (name, description, is_done, datetime) SELECT name, description, is_done, datetime FROM tasks WHERE id = @TaskID";
                MySqlCommand cmd = new MySqlCommand(transfer_Script, connection);
                cmd.Parameters.AddWithValue("@TaskID", taskId);
                cmd.ExecuteNonQuery();
                connection.Close();
                deletetask(taskId, "tasks");
            }
            catch
            {
                MessageBox.Show("Failed");
            }
        }

    }
    public class DB_uncompleted : DB
    {
        public DB_uncompleted() : base("tasks")
        {
        }
        


    }

    public class DB_completed : DB
    {
        public DB_completed() : base("tasks_completed")
        {
        }

    }
}

