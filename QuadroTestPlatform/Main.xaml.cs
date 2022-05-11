using System.Data.SqlClient;
using System.Windows;

namespace QuadroTestPlatform
{

    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
            tb_pathSQL.Text = Properties.Settings.Default.pathSQL;
            try
            {
                using (SqlConnection conn = new SqlConnection(strSQLConnection()))
                {
                    clearTree(ref TemsTree);
                    conn.Open();
                    SqlCommand command = new SqlCommand();
                    command.CommandText = "SELECT * FROM t_tems";
                    command.Connection = conn;
                    SqlDataReader read = command.ExecuteReader();
                    while (read.Read())
                    {
                        TemsTree.Items.Add(read.GetValue(1));
                    }
                    conn.Close();
                }
            }
            catch
            {
                MessageBox.Show("Остуствует подключение к MS SQL Servers, провертье наличие таблиц");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxNameTems.Text.ToString() != null && TextBoxNameTems.Text.ToString() != "")
            {
                using (SqlConnection conn = new SqlConnection(strSQLConnection()))
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand();
                    comm.CommandText = "INSERT INTO t_tems (Name,CountQMax) VALUES (\'" + TextBoxNameTems.Text.ToString().Trim() + "\', 20)";
                    comm.Connection = conn;
                    comm.ExecuteNonQuery();
                    conn.Close();
                    refresh();
                }
            }
        }

        private void b_deleteTems_Click(object sender, RoutedEventArgs e)
        {
            TemsTree.Items.Remove(TemsTree.SelectedItem);
        }

        private void b_replacePathSQL_Click(object sender, RoutedEventArgs e)
        {
            if (tb_pathSQL.Text.ToString() != null && tb_pathSQL.Text.ToString() != "")
            {
                Properties.Settings.Default.pathSQL = tb_pathSQL.Text.ToString();
                Properties.Settings.Default.Save();
                MessageBox.Show("Настройки сохранены");
            }
        }



        private void b_createTableDB_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connect = new SqlConnection(strSQLConnection()))
            {
                try
                {
                    connect.Open();
                    SqlCommand com = new SqlCommand();
                    com.CommandText = "SELECT * FROM t_tems";
                    com.Connection = connect;
                    com.ExecuteNonQuery();
                    connect.Close();

                }
                catch
                {
                    SqlCommand com = new SqlCommand();
                    com.CommandText = "CREATE TABLE t_tems (Id INT PRIMARY KEY IDENTITY, Name NVARCHAR(1000), CountQMax NVARCHAR (1000))";
                    com.Connection = connect;
                    com.ExecuteNonQuery();
                    connect.Close();
                }

                try
                {
                    connect.Open();
                    SqlCommand com = new SqlCommand();
                    com.CommandText = "SELECT * FROM t_question";
                    com.Connection = connect;
                    com.ExecuteNonQuery();
                    connect.Close();

                }
                catch
                {
                    SqlCommand com = new SqlCommand();
                    com.CommandText = "CREATE TABLE t_question (Id INT PRIMARY KEY IDENTITY, tKey INT NOT NULL, Question NVARCHAR (1000))";
                    com.Connection = connect;
                    com.ExecuteNonQuery();
                    connect.Close();
                }
                try
                {
                    connect.Open();
                    SqlCommand com = new SqlCommand();
                    com.CommandText = "SELECT * FROM t_answer";
                    com.Connection = connect;
                    com.ExecuteNonQuery();
                    connect.Close();

                }
                catch
                {
                    SqlCommand com = new SqlCommand();
                    com.CommandText = "CREATE TABLE t_answer (Id INT PRIMARY KEY IDENTITY, qKey INT, ans1 NVARCHAR (1000),ans2 NVARCHAR (1000),ans3 NVARCHAR (1000),ans4 NVARCHAR (1000),ans5 NVARCHAR (1000),ans6 NVARCHAR (1000), numberTrue INT)";
                    com.Connection = connect;
                    com.ExecuteNonQuery();
                    connect.Close();
                }
            }
        }

        private void b_about_Click(object sender, RoutedEventArgs e)
        {
            if  (TemsTree.SelectedItem != null)
            {

                Tems editTems = new Tems(TemsTree.SelectedItem.ToString());
                editTems.Show();
            }

        }

        public void refresh()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(strSQLConnection()))
                {
                    clearTree(ref TemsTree);
                    conn.Open();
                    SqlCommand command = new SqlCommand();
                    command.CommandText = "SELECT * FROM t_tems";
                    command.Connection = conn;
                    SqlDataReader read = command.ExecuteReader();
                    while (read.Read())
                    {
                        TemsTree.Items.Add(read.GetValue(1));
                    }
                    conn.Close();
                }
            }
            catch
            {
                MessageBox.Show("Остуствует подключение к MS SQL Servers, провертье наличие таблиц");
            }
        }

        public void clearTree(ref System.Windows.Controls.TreeView a)
        {
            int count = a.Items.Count;
            for (int i = 0; i < count; i++)
            {
                a.Items.RemoveAt(0);
            }
        }

        public string strSQLConnection()
        {
            return "Server=" + Properties.Settings.Default.pathSQL + ";Initial Catalog =QTPDB; User ID = sa; Password = qwerty12";
        }
    }
}