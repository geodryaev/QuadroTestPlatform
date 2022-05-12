using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace QuadroTestPlatform
{
    public partial class CRQuestion : Window
    {
        bool errorNameQ;
        bool isReplace;
        int keyTem;
        string keyQuestion, nameTems;
        public CRQuestion(int key_tems, string nameT)
        {
            isReplace = false;
            keyTem = key_tems;
            InitializeComponent();
            nameTems = nameT;
        }
        public CRQuestion(string nameQustion)
        {
            isReplace = true;
            InitializeComponent();

            b_SetQuestion.Content = "Обновить";
            using (SqlConnection connect = new SqlConnection(strSQLConnection()))
            {
                connect.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connect;
                command.CommandText = "SELECT * FROM t_question";
                SqlDataReader read = command.ExecuteReader();

                while (read.Read())
                {
                    if (read.GetValue(2).ToString() == nameQustion)
                    {
                        tb_nameQuestion.Text = read.GetString(2);
                        keyQuestion = read.GetValue(0).ToString();
                    }
                }
                command.CommandText = "SELECT * FROM t_answer";
                read.Close();
                read = command.ExecuteReader();
                while (read.Read())
                {
                    if (keyQuestion == read.GetValue(1).ToString())
                    {
                        tb_answer1.Text = read.GetValue(2).ToString();
                        tb_answer2.Text = read.GetValue(3).ToString();
                        tb_answer3.Text = read.GetValue(4).ToString();
                        tb_answer4.Text = read.GetValue(5).ToString();
                        tb_answer5.Text = read.GetValue(6).ToString();
                        tb_answer6.Text = read.GetValue(7).ToString();
                        SetCheak(cb_true1, cb_true2, cb_true3, cb_true4, cb_true5, cb_true6, read.GetValue(8).ToString());
                    }

                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            errorNameQ = false;
            {
                using (SqlConnection connect = new SqlConnection(strSQLConnection()))
                {
                    connect.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connect;
                    command.CommandText = "SELECT * FROM t_question";
                    SqlDataReader read = command.ExecuteReader();
                    while (read.Read())
                    {
                        if (read.GetValue(2).ToString() == tb_nameQuestion.Text)
                        {
                            errorNameQ = true;
                        }
                    }
                }
            }

            if (!isVoid(tb_answer1) || !isVoid(tb_answer2) || !isVoid(tb_answer3) || !isVoid(tb_answer4) || !isVoid(tb_answer5) || !isVoid(tb_answer6))
            {
                if (tb_nameQuestion.Text != null && tb_nameQuestion.Text.ToString().Trim() != "")
                {
                    if (getNumberTrueAnswer() != "")
                    {
                        if (isReplace)
                        {
                            using (SqlConnection connect = new SqlConnection(strSQLConnection()))
                            {
                                connect.Open();
                                SqlCommand command = new SqlCommand();
                                command.Connection = connect;
                                command.CommandText = "UPDATE t_answer SET ans1 = '" + tb_answer1.Text.ToString().Trim() + "',ans2 = '" + tb_answer2.Text.ToString().Trim() + "',ans3 = '" + tb_answer3.Text.ToString().Trim() + "',ans4 = '" + tb_answer4.Text.ToString().Trim() + "',ans5 = '" + tb_answer5.Text.ToString().Trim() + "',ans6 = '" + tb_answer6.Text.ToString().Trim() + "', numberTrue = '" + getNumberTrueAnswer() + "' WHERE qKey = " + keyQuestion;
                                MessageBox.Show(command.CommandText);
                                SqlDataReader read =  command.ExecuteReader();
                                connect.Close();
                                Close();
                            }
                        }
                        else
                        {
                            if(!errorNameQ)
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
                                MessageBox.Show("Такой вопрос уже существет");
                            } 
                                
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

        public void SetCheak(CheckBox cb1, CheckBox cb2, CheckBox cb3, CheckBox cb4, CheckBox cb5, CheckBox cb6, string str)
        {
            if (str.Length == 0)
                return;

            for (int i = 0; i < str.Length; i++)
            {
                switch (str[i])
                {
                    case '1':
                        cb1.IsChecked = true;
                        break;
                    case '2':
                        cb2.IsChecked = true;
                        break;
                    case '3':
                        cb3.IsChecked = true;
                        break;
                    case '4':
                        cb4.IsChecked = true;
                        break;
                    case '5':
                        cb5.IsChecked = true;
                        break;
                    case '6':
                        cb6.IsChecked = true;
                        break;

                    default:
                        break;
                }
            }
            return;
        }

    }
}
