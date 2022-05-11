using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace QuadroTestPlatform
{
    public partial class CRQuestion : Window
    {
        int keyTem;
        string keyQuestion, nameTems;
        public CRQuestion(int key_tems,string nameT)
        {
            keyTem = key_tems;
            InitializeComponent();
            nameTems = nameT;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!isVoid(tb_answer1) || !isVoid(tb_answer2) || !isVoid(tb_answer3) || !isVoid(tb_answer4) || !isVoid(tb_answer5) || !isVoid(tb_answer6))
            {
                if (tb_nameQuestion.Text != null && tb_nameQuestion.Text.ToString().Trim() != "")
                {
                    if (getNumberTrueAnswer() != "")
                    {
                        using (SqlConnection connect = new SqlConnection(strSQLConnection()))
                        {
                            connect.Open();
                            SqlCommand command = new SqlCommand();
                            command.Connection = connect;
                            command.CommandText = "INSERT INTO t_question (tKey,Question) VALUES (" + keyTem.ToString() + ", \'" + tb_nameQuestion.Text.ToString().Trim() + "\')";
                            command.ExecuteNonQuery();

                            command.CommandText = "SELECT * FROM t_question";
                            SqlDataReader read = command.ExecuteReader();
                            while (read.Read())
                            {
                                if (read.GetValue(2).ToString() == tb_nameQuestion.Text.ToString().Trim())
                                    keyQuestion = read.GetValue(0).ToString();
                            }
                            read.Close();
                            command.CommandText = "INSERT INTO t_answer (qkey,ans1, ans2, ans3, ans4, ans5, ans6, numberTrue) VALUES ( '" + keyQuestion + "', '" + tb_answer1.Text.ToString().Trim() + "', '" + tb_answer2.Text.ToString().Trim() + "', '" + tb_answer3.Text.ToString().Trim() + "', '" + tb_answer4.Text.ToString().Trim() + "', '" + tb_answer5.Text.ToString().Trim() + "', '" + tb_answer6.Text.ToString().Trim() + "', '" + getNumberTrueAnswer() + "')";
                            command.ExecuteNonQuery();
                            connect.Close();
                            Tems t = new Tems(nameTems);
                            t.Show();
                            Close();

                        }
                    }
                    else
                    {
                        MessageBox.Show("Не выбран ни один правильный ответ");
                    }
                }
                else
                {
                    MessageBox.Show("Пустое поле формулировки вопроса");
                }
            }
            else
            {
                MessageBox.Show("Незаполнен ни один ответ на вопрос");
            }
            
        }
        public string strSQLConnection()
        {
            return "Server=" + Properties.Settings.Default.pathSQL + ";Initial Catalog =QTPDB; User ID = sa; Password = qwerty12";
        }
        public bool isVoid(TextBox e)
        {
            if (e.Text == null || e.Text.ToString() == "")
                return true;

            return false;
        }
        public string getNumberTrueAnswer()
        {
            string answer = "";
            if (cb_true1.IsChecked == true && !isVoid(tb_answer1))
                answer += "1";
            if (cb_true2.IsChecked == true && !isVoid(tb_answer2))
                answer += "2";
            if (cb_true3.IsChecked == true && !isVoid(tb_answer3))
                answer += "3";
            if (cb_true4.IsChecked == true && !isVoid(tb_answer4))
                answer += "4";
            if (cb_true5.IsChecked == true && !isVoid(tb_answer5))
                answer += "5";
            if (cb_true6.IsChecked == true && !isVoid(tb_answer6))
                answer += "6";
            return answer;
        }

    }
}
