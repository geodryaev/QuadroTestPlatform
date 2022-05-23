using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace QuadroTestPlatform
{
    public partial class CRQuestion : Window
    {
        answerCH[] myArray;
        bool errorNameQ, isReplace;
        int keyTem;
        string keyQuestion, nameTems;
        string[] idAnswer = new string[6];
        public CRQuestion(int key_tems, string nameT)
        {
            isReplace = false;
            keyTem = key_tems;
            InitializeComponent();
            myArray = new answerCH[6];
            myArray[0].textBox = tb_answer1;
            myArray[0].checkBox = cb_true1;
            myArray[1].textBox = tb_answer2;
            myArray[1].checkBox = cb_true2;
            myArray[2].textBox = tb_answer3;
            myArray[2].checkBox = cb_true3;
            myArray[3].textBox = tb_answer4;
            myArray[3].checkBox = cb_true4;
            myArray[4].textBox = tb_answer5;
            myArray[4].checkBox = cb_true5;
            myArray[5].textBox = tb_answer6;
            myArray[5].checkBox = cb_true6;
            nameTems = nameT;
        }

        public CRQuestion(string nameQustion, string nameT)
        {
            nameTems = nameT;

            isReplace = true;
            InitializeComponent();
            myArray = new answerCH[6];
            myArray[0].textBox  = tb_answer1;
            myArray[0].checkBox = cb_true1;
            myArray[1].textBox  = tb_answer2;
            myArray[1].checkBox = cb_true2;
            myArray[2].textBox  = tb_answer3;
            myArray[2].checkBox = cb_true3;
            myArray[3].textBox  = tb_answer4;
            myArray[3].checkBox = cb_true4;
            myArray[4].textBox  = tb_answer5;
            myArray[4].checkBox = cb_true5;
            myArray[5].textBox  = tb_answer6;
            myArray[5].checkBox = cb_true6;
            
            string[] answerTrue = null;
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
                    if (read.GetValue(3).ToString() == nameQustion)
                    {
                        tb_nameQuestion.Text = read.GetString(3);
                        keyQuestion = read.GetValue(0).ToString();
                        answerTrue = GetArrayTrueAnswer(read.GetValue(2).ToString());
                    }
                }
                command.CommandText = "SELECT * FROM t_answer";
                read.Close();
                read = command.ExecuteReader();
                TextBox[] t = new TextBox[6];
                t[0] = tb_answer1;
                t[1] = tb_answer2;
                t[2] = tb_answer3;
                t[3] = tb_answer4;
                t[4] = tb_answer5;
                t[5] = tb_answer6;
                int countTB = 0;
                while (read.Read())
                {
                    if (keyQuestion == read.GetValue(1).ToString())
                    {
                        t[countTB].Text = read.GetValue(2).ToString();
                        countTB++;
                    }

                }
                SetCheak(answerTrue);
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
                        if (read.GetValue(3).ToString() == tb_nameQuestion.Text)
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
                                string buf1, aKey = "";
                                connect.Open();
                                SqlCommand command = new SqlCommand();
                                command.Connection = connect;
                                command.CommandText = "DELETE t_answer WHERE qKey='" + keyQuestion + "'";
                                SqlDataReader read = command.ExecuteReader();
                                read.Close();
                                command.CommandText = "UPDATE t_question SET Question = '"+tb_nameQuestion.Text.Trim()+ "' WHERE Id = " + keyQuestion;
                                command.ExecuteNonQuery();
                                SetAnswer(tb_answer1.Text.Trim(), tb_answer2.Text.Trim(), tb_answer3.Text.Trim(), tb_answer4.Text.Trim(), tb_answer5.Text.Trim(), tb_answer6.Text.Trim(), keyQuestion);
                                for (int i = 0; i < 6; i++)
                                {
                                    buf1 = GetStringAnswer();
                                    if (buf1 != "QTP error 12")
                                    {
                                        command.CommandText = "SELECT * FROM t_answer";
                                        read = command.ExecuteReader();
                                        while (read.Read())
                                        {
                                            if (read.GetValue(2).ToString() == buf1 && keyQuestion == read.GetValue(1).ToString())
                                                aKey += read.GetValue(0) + "|";
                                        }
                                        read.Close();
                                    }
                                }
                                command.CommandText = "UPDATE t_question SET aKey = '" + aKey + "' WHERE Id = " + keyQuestion;
                                command.ExecuteNonQuery();
                                connect.Close();
                                Tems t = new Tems(nameTems);
                                t.Show();
                                Close();
                            }
                        }
                        else
                        {
                            if (!errorNameQ)
                            {

                                using (SqlConnection connect = new SqlConnection(strSQLConnection()))
                                {
                                    string buf1, aKey = "";

                                    connect.Open();
                                    SqlCommand command = new SqlCommand();
                                    command.Connection = connect;
                                    command.CommandText = "INSERT INTO t_question (tKey, Question) VALUES (" + keyTem.ToString() + ", \'" + tb_nameQuestion.Text.ToString().Trim() + "\')";
                                    command.ExecuteNonQuery();
                                    command.CommandText = "SELECT * FROM t_question";
                                    SqlDataReader read = command.ExecuteReader();
                                    while (read.Read())
                                    {
                                        if (read.GetValue(3).ToString() == tb_nameQuestion.Text.ToString().Trim())
                                            keyQuestion = read.GetValue(0).ToString();
                                    }
                                    read.Close();
                                    SetAnswer(tb_answer1.Text.Trim(), tb_answer2.Text.Trim(), tb_answer3.Text.Trim(), tb_answer4.Text.Trim(), tb_answer5.Text.Trim(), tb_answer6.Text.Trim(), keyQuestion);

                                    for (int i = 0; i < 6; i++)
                                    {
                                        buf1 = GetStringAnswer();
                                        if (buf1 != "QTP error 12")
                                        {
                                            command.CommandText = "SELECT * FROM t_answer";
                                            read = command.ExecuteReader();
                                            while (read.Read())
                                            {
                                                if (read.GetValue(2).ToString() == buf1 && keyQuestion == read.GetValue(1).ToString())
                                                    aKey += read.GetValue(0) + "|";
                                            }
                                            read.Close();
                                        }
                                    }
                                    command.CommandText = "UPDATE t_question SET aKey = '" + aKey + "' WHERE Id = " + keyQuestion;
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
        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                b_SetQuestion.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
        public void SetCheak(string[] str)
        {
            if (str.Length == 0)
                return;


            for (int i = 0; i < str.Length; i++)
            {
                using (SqlConnection con = new SqlConnection(strSQLConnection()))
                {
                    con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "SELECT * FROM t_answer;";
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.GetValue(0).ToString() == str[i])
                        {
                            SetCheckOne(reader.GetValue(2).ToString());
                        }
                    }
                }
            }
            return;
        }
        public void SetAnswer(string str1, string str2, string str3, string str4, string str5, string str6, string qKey)
        {
            using (SqlConnection con = new SqlConnection(strSQLConnection()))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                if (str1 != null && str1.Length != 0)
                {
                    com.CommandText = "INSERT INTO t_answer (qKey, answer) VALUES ('" + qKey + "', '" + str1 + "')";
                    com.ExecuteNonQuery();
                }
                if (str2 != null && str2.Length != 0)
                {
                    com.CommandText = "INSERT INTO t_answer (qKey, answer) VALUES ('" + qKey + "', '" + str2 + "')";
                    com.ExecuteNonQuery();
                }
                if (str3 != null && str3.Length != 0)
                {
                    com.CommandText = "INSERT INTO t_answer (qKey, answer) VALUES ('" + qKey + "', '" + str3 + "')";
                    com.ExecuteNonQuery();
                }
                if (str4 != null && str4.Length != 0)
                {
                    com.CommandText = "INSERT INTO t_answer (qKey, answer) VALUES ('" + qKey + "', '" + str4 + "')";
                    com.ExecuteNonQuery();
                }
                if (str5 != null && str5.Length != 0)
                {
                    com.CommandText = "INSERT INTO t_answer (qKey, answer) VALUES ('" + qKey + "', '" + str5 + "')";
                    com.ExecuteNonQuery();
                }
                if (str6 != null && str6.Length != 0)
                {
                    com.CommandText = "INSERT INTO t_answer (qKey, answer) VALUES ('" + qKey + "', '" + str6 + "')";
                    com.ExecuteNonQuery();
                }
                con.Close();
            }
        }
        public string GetStringAnswer()
        {
            string outp = "QTP error 12";
            if (cb_true1.IsChecked == true)
            {
                cb_true1.IsChecked = false;
                return tb_answer1.Text.Trim();
            }
            if (cb_true2.IsChecked == true)
            {
                cb_true2.IsChecked = false;
                return tb_answer2.Text.Trim();
            }
            if (cb_true3.IsChecked == true)
            {
                cb_true3.IsChecked = false;
                return tb_answer3.Text.Trim();
            }
            if (cb_true4.IsChecked == true)
            {
                cb_true4.IsChecked = false;
                return tb_answer4.Text.Trim();
            }
            if (cb_true4.IsChecked == true)
            {
                cb_true4.IsChecked = false;
                return tb_answer5.Text.Trim();
            }
            if (cb_true5.IsChecked == true)
            {
                cb_true5.IsChecked = false;
                return tb_answer6.Text.Trim();
            }
            if (cb_true6.IsChecked == true)
            {
                cb_true6.IsChecked = false;
                return tb_answer6.Text.Trim();
            }


            return outp;
        }
        public string[] GetArrayTrueAnswer(string str)
        {
            int count = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '|')
                    count++;
            }
            string[] arrayStr = new string[count];
            count = 0;
            string buffer = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != '|')
                {
                    while (str[i]!= '|' && i < str.Length)
                    {
                        buffer += str[i];
                        i++;
                    }
                    arrayStr[count] = buffer;
                    buffer = "";
                    count++;
                }
            }

            return arrayStr;
        }
        public void SetCheckOne(string str)
        {
            for (int i = 0; i < 6; i++)
            {
                if (myArray[i].textBox.Text == str)
                {
                    myArray[i].checkBox.IsChecked = true;
                }
            }
        }
        public struct answerCH
        {
            public CheckBox checkBox;
            public TextBox textBox;
        }

    }
}
