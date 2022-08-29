using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuadroTestPlatform
{
    public partial class MainWindow : Window
    {
        public bool _auntification;
        public MainWindow()
        {
            InitializeComponent();
            //изменить при релизе
            string ans = "1,5";
            double a = Convert.ToDouble(ans);
            _auntification = false;
            tb_pathSQL.Text = Properties.Settings.Default.pathSQL;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Password.Password.ToString() == "qwe"
            if (true)
            {
                Main _collection = new Main();
                _collection.Show();
                Close();
            }
            else
            {
                
            }
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
                    com.CommandText = "CREATE TABLE t_tems (Id INT PRIMARY KEY IDENTITY, Name NVARCHAR(1000), tree  NVARCHAR (1000), four  NVARCHAR (1000), five  NVARCHAR (1000), CountQMax NVARCHAR (1000), time NVARCHAR (1000), tr NVARCHAR (1000))";
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
                    com.CommandText = "CREATE TABLE t_question (Id INT PRIMARY KEY IDENTITY, tKey INT NOT NULL, aKey NVARCHAR (1000), Question NVARCHAR (1000), tr NVARCHAR (1000))";
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
                    com.CommandText = "CREATE TABLE t_answer (Id INT PRIMARY KEY IDENTITY, qKey INT, answer NVARCHAR (1000), tr NVARCHAR (1000))";
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
                    com.CommandText = "INSERT t_zvezda VALUES ('Страший мичман')";
                    com.ExecuteNonQuery();
                    com.CommandText = "INSERT t_zvezda VALUES ('Старший прапорщик')";
                    com.ExecuteNonQuery();
                    com.CommandText = "INSERT t_zvezda VALUES ('Младший лейтенант')";
                    com.ExecuteNonQuery();
                    com.CommandText = "INSERT t_zvezda VALUES ('Лейтенант')";
                    com.ExecuteNonQuery();
                    com.CommandText = "INSERT t_zvezda VALUES ('Старший лейтенант')";
                    com.ExecuteNonQuery();
                    com.CommandText = "INSERT t_zvezda VALUES ('Капитан-лейтинант')";
                    com.ExecuteNonQuery();
                    com.CommandText = "INSERT t_zvezda VALUES ('Капитан')";
                    com.ExecuteNonQuery();
                    com.CommandText = "INSERT t_zvezda VALUES ('Капитан 3-го ранга')";
                    com.ExecuteNonQuery();
                    com.CommandText = "INSERT t_zvezda VALUES ('Майор')";
                    com.ExecuteNonQuery();
                    com.CommandText = "INSERT t_zvezda VALUES ('Капитан 2-го ранга')";
                    com.ExecuteNonQuery();
                    com.CommandText = "INSERT t_zvezda VALUES ('Подполковник')";
                    com.ExecuteNonQuery();
                    com.CommandText = "INSERT t_zvezda VALUES ('Капитан 1-го ранга')";
                    com.ExecuteNonQuery();
                    com.CommandText = "INSERT t_zvezda VALUES ('Полковник')";
                    com.ExecuteNonQuery();
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
                try
                {
                    connect.Open();
                    SqlCommand com = new SqlCommand();
                    com.CommandText = "SELECT * FROM t_result";
                    com.Connection = connect;
                    com.ExecuteNonQuery();
                    connect.Close();

                }
                catch
                {
                    SqlCommand com = new SqlCommand();
                    com.CommandText = "CREATE TABLE t_result (Id INT PRIMARY KEY IDENTITY, Unit NVARCHAR (1000), numberUnit NVARCHAR (1000), zvezda NVARCHAR (1000), Name NVARCHAR (1000), data NVARCHAR (1000), g1 NVARCHAR (1000), g2 NVARCHAR (1000))";
                    com.Connection = connect;
                    com.ExecuteNonQuery();
                    connect.Close();
                }
            }
        }

        public string strSQLConnection()
        {
            return "Server=" + Properties.Settings.Default.pathSQL + ";Initial Catalog =QTPDB; User ID = sa; Password = qwerty12";
        }
    }
}
 