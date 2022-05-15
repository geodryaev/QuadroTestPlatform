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
                    read.Close();
                    command.CommandText = "SELECT * FROM t_groupe";
                    read = command.ExecuteReader();
                    while (read.Read())
                    {
                        cb_Unit.Items.Add(read.GetValue(1));
                    }
                    read.Close();
                    command.CommandText = "SELECT * FROM t_numberGroupe";
                    read = command.ExecuteReader();
                    while (read.Read())
                    {
                        cb_groupe.Items.Add(read.GetValue(1));
                    }
                    read.Close();
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
                    comm.CommandText =
                    comm.CommandText = "INSERT INTO t_tems (Name, tree, four, five, CountQMax) VALUES (\'" + TextBoxNameTems.Text.ToString().Trim() + "\', 3, 2, 1, 20)";
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
                    com.CommandText = "CREATE TABLE t_tems (Id INT PRIMARY KEY IDENTITY, Name NVARCHAR(1000), tree  NVARCHAR (1000), four  NVARCHAR (1000), five  NVARCHAR (1000), CountQMax NVARCHAR (1000))";
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
                try
                {
                    connect.Open();
                    SqlCommand com = new SqlCommand();
                    com.CommandText = "SELECT * FROM t_zvezda";
                    com.Connection = connect;
                    com.ExecuteNonQuery();
                    connect.Close();

                }
                catch
                {
                    SqlCommand com = new SqlCommand();
                    com.CommandText = "CREATE TABLE t_zvezda (Id INT PRIMARY KEY IDENTITY, zvanie NVARCHAR (1000))";
                    com.Connection = connect;
                    com.ExecuteNonQuery();
                    com.CommandText = "INSERT t_zvezda VALUES ('Мичман')";
                    com.ExecuteNonQuery();
                    com.CommandText = "INSERT t_zvezda VALUES ('Прапорщик')";
                    com.ExecuteNonQuery();
                    com.CommandText = "INSERT t_zvezda VALUES ('Старший прапорщик')";
                    com.ExecuteNonQuery();
                    com.CommandText = "INSERT t_zvezda VALUES ('Младший лейтенант')";
                    com.ExecuteNonQuery();
                    com.CommandText = "INSERT t_zvezda VALUES ('Лейтенант')";
                    com.ExecuteNonQuery();
                    com.CommandText = "INSERT t_zvezda VALUES ('Старший лейтенант')";
                    com.ExecuteNonQuery();
                    com.CommandText = "INSERT t_zvezda VALUES ('Капитан')";
                    com.ExecuteNonQuery();
                    com.CommandText = "INSERT t_zvezda VALUES ('Майор')";
                    com.ExecuteNonQuery();
                    com.CommandText = "INSERT t_zvezda VALUES ('Подполковник')";
                    com.ExecuteNonQuery();
                    com.CommandText = "INSERT t_zvezda VALUES ('Полковник')";
                    com.ExecuteNonQuery();
                    com.CommandText = "INSERT t_zvezda VALUES ('Капитан первого ранга')";
                    connect.Close();
                }
                try
                {
                    connect.Open();
                    SqlCommand com = new SqlCommand();
                    com.CommandText = "SELECT * FROM t_groupe";
                    com.Connection = connect;
                    com.ExecuteNonQuery();
                    connect.Close();

                }
                catch
                {
                    SqlCommand com = new SqlCommand();
                    com.CommandText = "CREATE TABLE t_groupe (Id INT PRIMARY KEY IDENTITY, unit NVARCHAR (1000))";
                    com.Connection = connect;
                    com.ExecuteNonQuery();
                    connect.Close();
                }
                try
                {
                    connect.Open();
                    SqlCommand com = new SqlCommand();
                    com.CommandText = "SELECT * FROM t_numberGroupe";
                    com.Connection = connect;
                    com.ExecuteNonQuery();
                    connect.Close();

                }
                catch
                {
                    SqlCommand com = new SqlCommand();
                    com.CommandText = "CREATE TABLE t_numberGroupe (Id INT PRIMARY KEY IDENTITY, numberGroupe NVARCHAR (1000))";
                    com.Connection = connect;
                    com.ExecuteNonQuery();
                    connect.Close();
                }
            }
        }

        private void b_about_Click(object sender, RoutedEventArgs e)
        {
            if (TemsTree.SelectedItem != null)
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
                    conn.Open();
                    clearTree(ref TemsTree);
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
        public void reafreshUnit()
        {
            cb_groupe.Items.Clear();
            cb_Unit.Items.Clear();
            using (SqlConnection connection = new SqlConnection(strSQLConnection()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                SqlDataReader read;
                command.Connection = connection;
                command.CommandText = "SELECT * FROM t_groupe";
                read = command.ExecuteReader();
                while (read.Read())
                {
                    cb_Unit.Items.Add(read.GetValue(1));
                }
                read.Close();
                command.CommandText = "SELECT * FROM t_numberGroupe";
                read = command.ExecuteReader();
                while (read.Read())
                {
                    cb_groupe.Items.Add(read.GetValue(1));
                }
                read.Close();
                connection.Close();
            }
        }

        private void b_createUnit_Click(object sender, RoutedEventArgs e)
        {
            if (tb_nameUnit.Text != null && tb_nameUnit.Text.ToString() != "")
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(strSQLConnection()))
                    {
                        connection.Open();
                        SqlCommand comm = new SqlCommand();
                        comm.Connection = connection;
                        comm.CommandText = "INSERT t_groupe VALUES ('" + tb_nameUnit.Text.ToString().Trim() + "')";
                        comm.ExecuteNonQuery();
                        connection.Close();
                        tb_nameUnit.Text = "";
                        reafreshUnit();
                    }
                }
                catch
                {

                }
            }
        }

        private void b_deleteUnit_Click(object sender, RoutedEventArgs e)
        {
            if (cb_Unit.SelectedIndex != -1)
            {
                using (SqlConnection connect = new SqlConnection(strSQLConnection()))
                {
                    connect.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connect;
                    command.CommandText = "DELETE t_groupe WHERE Unit = '" + cb_Unit.Text.ToString() + "'";
                    command.ExecuteNonQuery();
                    connect.Close();
                    reafreshUnit();
                }
            }
        }

        private void b_createGroupe_Click(object sender, RoutedEventArgs e)
        {
            if (tb_groupe.Text != null && tb_groupe.Text.ToString() != "")
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(strSQLConnection()))
                    {
                        connection.Open();
                        SqlCommand comm = new SqlCommand();
                        comm.Connection = connection;
                        comm.CommandText = "INSERT t_numberGroupe VALUES ('" + tb_groupe.Text.ToString().Trim() + "')";
                        comm.ExecuteNonQuery();
                        connection.Close();
                        tb_groupe.Text = "";
                        reafreshUnit();
                    }
                }
                catch
                {

                }
            }
        }

        private void b_deleteGroupe_Click(object sender, RoutedEventArgs e)
        {
            if (cb_groupe.SelectedIndex != -1)
            {
                using (SqlConnection connect = new SqlConnection(strSQLConnection()))
                {
                    connect.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connect;
                    command.CommandText = "DELETE t_numberGroupe WHERE numberGroupe = '" + cb_groupe.Text.ToString() + "'";
                    command.ExecuteNonQuery();
                    connect.Close();
                    reafreshUnit();
                }
            }
        }
    }
}