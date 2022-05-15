using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace QuadroTestPlatform
{
    public partial class Tems : Window
    {
        int numCol = 0;
        int keyTems;
        public string nameTems;
        public Tems(string nameT)
        {
            InitializeComponent();
            nameTems = nameT;
            tb_nameTems.Text = nameT;
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
                        tb_countQMax.Text = read.GetValue(5).ToString();
                        keyTems = Convert.ToInt32(read.GetValue(0));
                        tb_tree.Text = read.GetValue(2).ToString();
                        tb_four.Text = read.GetValue(3).ToString();
                        tb_five.Text = read.GetValue(4).ToString();
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
                        numCol++;
                    }

                }
                dg_question.ItemsSource = q;
                dg_question.ColumnWidth = 900;
                dg_question.Columns[0].IsReadOnly = false;
                dg_question.CanUserAddRows = false;
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
        }

        private void dg_question_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void b_replace_Click(object sender, RoutedEventArgs e)
        {
            if (dg_question.SelectedItems.Count == 0)
                return;

            cQuestion nq = (cQuestion)dg_question.SelectedItems[0];
            CRQuestion form = new CRQuestion(nq.Question);
            form.Show();
            Close();
        }

        private void b_deleteQ_Click(object sender, RoutedEventArgs e)
        {
            if (dg_question.SelectedItems.Count == 0)
                return;

            cQuestion nq = (cQuestion)dg_question.SelectedItems[0];
            using (SqlConnection connect = new SqlConnection(strSQLConnection()))
            {
                string keyDelete = "";
                connect.OpenAsync();
                SqlCommand command = new SqlCommand();
                command.Connection = connect;
                command.CommandText = "SELECT * FROM t_question";
                SqlDataReader read = command.ExecuteReader();
                while (read.Read())
                {
                    if (read.GetString(2) == nq.Question)
                    {
                        keyDelete = read.GetValue(0).ToString();
                    }
                }
                read.Close();
                command.CommandText = "DELETE t_question WHERE id = " + keyDelete;
                read = command.ExecuteReader();
                read.Close();
                command.CommandText = "DELETE t_answer WHERE qKey = " + keyDelete;
                read = command.ExecuteReader();
                read.Close();
                connect.Close();
                Tems form = new Tems(nameTems);
                form.Show();
                Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connect = new SqlConnection(strSQLConnection()))
            {
                connect.Open();
                SqlCommand commnad = new SqlCommand();
                commnad.Connection = connect;
                commnad.CommandText = "UPDATE t_tems SET CountQMax = '" + tb_countQMax.Text.Trim() + "', tree = '" + tb_tree.Text.Trim() + "', four = '" + tb_four.Text.Trim() + "', five = '" + tb_five.Text.Trim() + "' WHERE Id = " + keyTems.ToString();
                SqlDataReader read = commnad.ExecuteReader();
                read.Close();
                connect.Close();
                Close();
            }
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.F1)
                b_createQuestion.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
    }
}
