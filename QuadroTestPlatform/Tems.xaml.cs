using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace QuadroTestPlatform
{

    public partial class Tems : Window
    {
        int keyTems;
        public string nameTems;
        public Tems(string nameT)
        {
            InitializeComponent();
            tb_nameTems.Text = nameT;
            nameTems = nameT;
            using (SqlConnection connect = new SqlConnection(strSQLConnection()))
            {
                connect.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connect;
                command.CommandText = "SELECT * FROM t_tems";
                SqlDataReader read = command.ExecuteReader();
                while (read.Read())
                {
                    if (read.GetValue(1).ToString() == nameTems)
                    {
                        tb_countQMax.Text = read.GetValue(2).ToString();
                        keyTems = Convert.ToInt32(read.GetValue(0));
                    }

                }
                read.Close();
                command.CommandText = "SELECT * FROM t_question";
                read = command.ExecuteReader();
                List<cQuestion> q = new List<cQuestion>
                {

                };
                while (read.Read())
                {
                    if (read.GetValue(1).ToString() == keyTems.ToString().Trim())
                    {
                        cQuestion a = new cQuestion();
                        a.Question = read.GetValue(2).ToString().Trim();
                        q.Add(a);
                    }

                }
                dg_question.ItemsSource = q;
                dg_question.ColumnWidth = 900;
                
                connect.Close();
            }
        }
        public string strSQLConnection()
        {
            return "Server=" + Properties.Settings.Default.pathSQL + ";Initial Catalog =QTPDB; User ID = sa; Password = qwerty12";
        }

        class cQuestion
        {
            public string Question { get; set; }
        }

        private void b_createQuestion_Click(object sender, RoutedEventArgs e)
        {
            CRQuestion cR = new CRQuestion(keyTems, nameTems);
            cR.Show();
            Close();
            //refreshDataGrid();
        }
        public void refreshDataGrid()
        {
            using(SqlConnection connect = new SqlConnection(strSQLConnection()))
            {
                connect.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = "SELECT * FROM t_question";
                SqlDataReader read = command.ExecuteReader();
                List<cQuestion> q = new List<cQuestion>
                {

                };
                while (read.Read())
                {
                    if (read.GetValue(1).ToString() == keyTems.ToString().Trim())
                    {
                        cQuestion a = new cQuestion();
                        a.Question = read.GetValue(2).ToString().Trim();
                        q.Add(a);
                    }

                }
                dg_question.ItemsSource = q;
                connect.Close();
            }
        }
    }
}
