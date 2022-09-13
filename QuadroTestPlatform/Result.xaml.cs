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
using System.Windows.Shapes;

namespace QuadroTestPlatform
{

    public partial class Result : Window
    {
        private Main.Disciplines now;

        string[] tems;
        int countNextTems;
        public Result(Main.Disciplines _now, int _countTems, string[] _tems)
        {
            InitializeComponent();
            tems = _tems;
            countNextTems = _countTems;
            this.now = _now;
            int countTrue = getNumberTrueAnswer(now);
            string assessment = "2";
            countTrue = now._arrayQuestion.Length - countTrue;
            if (countTrue <= now._free)
            {
                assessment = "3";
            }
            if (countTrue <= now._four)
            {
                assessment = "4";
            }
            if (countTrue <= now._five)
            {
                assessment = "5";
            }

            countNextTems++;
            if (countNextTems >= tems.Length)
            {
                bt_next.Content = "Закрыть";
            }
            result.Text += "Тема: " + now._nameDisciplines + "\nВаша оцека : " + assessment;
            fullText allResult = new fullText(strSQLConnection(), SP, now);
            //allResult.sqlUpload(unit, numberUnit, zvezda, fullName);
        }
        public int getNumberTrueAnswer(Main.Disciplines dis)
        {
            int answer = 0;
            for (int i = 0; i < dis._arrayQuestion.Length; i++)
            {
                if (isGood(dis._arrayQuestion[i]))
                    answer++;
            }
            return answer;
        }
        public bool isGood(Main.QuestionFF quest)
        {
            if (quest._answerUser == null)
                return false;

            for (int i = 0; i < quest._answerUser.Length; i++)
            {
                if (!oneTrue(quest._kTrueAnswer, quest._answerUser[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private void bt_next_Click(object sender, RoutedEventArgs e)
        {

            if (countNextTems < tems.Length)
            {
                Close();
            }
            else
            {
                Close();
            }
        }

        public bool oneTrue(string[] array, string str)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == str)
                    return true;
            }

            return false;
        }
        public string strSQLConnection()
        {
            return "Server=" + Properties.Settings.Default.pathSQL + ";Initial Catalog =QTPDB; User ID = sa; Password = qwerty12";
        }

        class fullText
        {
            public fullText(string con, StackPanel sp, Main.Disciplines dis)
            {
                _con = new SqlConnection(con);
                _dis = dis;
                _sp = sp;
                _countQ = 1;
                _tb = new TextBlock[0];
                setText();
            }

            public void setText()
            {
                for (int i = 0; i < _dis._arrayQuestion.Length; i++)
                {
                    oneQuestion(_dis._arrayQuestion[i]);
                }
            }

            private void oneQuestion(Main.QuestionFF q)
            {
                newTB();
                _tb[_tb.Length - 1].Text += "\n\n" + Convert.ToString(_countQ) + ". " + q._nameQuestrion + "\n\n";
                _tb[_tb.Length - 1].FontSize = 20;
                _countQ++;
                allAnswer(q);
            }
            private void allAnswer(Main.QuestionFF q)
            {
                for (int i = 0; i < q._kAnswers.Length; i++)
                {
                    newTB();
                    _con.Open();

                    SqlCommand com = new SqlCommand();
                    com.Connection = _con;
                    com.CommandText = "SELECT * FROM t_answer";
                    SqlDataReader read = com.ExecuteReader();
                    while (read.Read())
                    {
                        if (read.GetValue(0).ToString() == q._kAnswers[i])
                        {
                            _tb[_tb.Length - 1].Text += "\n\t " + Convert.ToString(i + 1) + ". " + read.GetValue(2).ToString();
                        }
                    }
                    if (blue(q, q._kAnswers[i]))
                    {
                        _tb[_tb.Length - 1].Foreground = Brushes.Blue;
                    }
                    if (red(q, q._kAnswers[i]))
                    {
                        _tb[_tb.Length - 1].Foreground = Brushes.Red;
                    }
                    if (green(q, q._kAnswers[i]))
                    {
                        _tb[_tb.Length - 1].Foreground = Brushes.Green;
                    }
                    read.Close();
                    _con.Close();
                }


            }
            private bool red(Main.QuestionFF q, string key)
            {
                if (q._answerUser == null)
                    return false;

                for (int i = 0; i < q._answerUser.Length; i++)
                {
                    if (q._answerUser[i] == key)
                        return true;
                }
                return false;
            }

            private bool blue(Main.QuestionFF q, string str)
            {
                if (q._answerUser == null)
                    return false;

                for (int i = 0; i < q._kTrueAnswer.Length; i++) 
                {
                    if (q._kTrueAnswer[i] == str)
                    {
                        return true;
                    }
                }
                return false;
            }
            private bool green(Main.QuestionFF q, string key)
            {
                if (q._answerUser == null)
                    return false;

                for (int i = 0; i < q._answerUser.Length; i++)
                {
                    if (q._answerUser[i] == key)
                    {
                        for (int j = 0; j < q._kTrueAnswer.Length; j++)
                        {
                            if (q._kTrueAnswer[j] == key)
                                return true;
                        }
                    }
                }
                return false;
            }

            private void newTB()
            {
                TextBlock[] n = new TextBlock[_tb.Length + 1];
                for (int i = 0; i < _tb.Length; i++)
                {
                    n[i] = _tb[i];
                }
                _tb = n;
                _tb[_tb.Length - 1] = new TextBlock();
                _tb[_tb.Length - 1].Width = 730;
                _sp.Children.Add(_tb[_tb.Length - 1]);
            }
            public void sqlUpload(string _unit, string _nameUnit, string _zvezda, string _fullName)
            {
                _con.Open();
                SqlCommand com = new SqlCommand("INSERT INTO t_result (Unit, numberUnit, zvezda, Name, data, g1, g2) VALUES (\'" + _unit + "\', \'" + _nameUnit + "\', \'" + _zvezda + "\', \'" + _fullName + "\', \'" + DateTime.Now.ToString() + "\', \'" + getG1() + "\', \'" + getG2() + "\')");
                string str = "INSERT INTO t_result (Unit, numberUnit, zvezda, Name, data, g1, g2) VALUES (\"" + _unit + "\", \"" + _nameUnit + "\", \"" + _fullName + "\", \"" + getG1() + "\", \"" + getG2() + "\")";
                com.Connection = _con;
                com.ExecuteNonQuery();
                _con.Close();
            }
            private string getG1()
            {
                return _dis._nameDisciplines;
            }
            private string getG2()
            {
                string answer = "";

                for (int i = 0; i < _dis._arrayQuestion.Length; i++)
                {
                    answer += getOneQuestionForG2(_dis._arrayQuestion[i]);
                }

                return answer;
            }
            private string getOneQuestionForG2(Main.QuestionFF q)
            {
                string answer = "";
                answer += q._kQuestion + "-";
                if (q._kQuestion == null)
                {
                    return answer;
                }
                if (q._answerUser == null)
                {
                    answer += "&";
                    return answer;
                }
                for (int i = 0; i < q._answerUser.Length; i++)
                {
                    answer += q._answerUser[i] + "|";
                }
                answer += "&";

                return answer;
            }
            private TextBlock[] _tb;
            private StackPanel _sp;
            private int _countQ;
            public Main.Disciplines _dis;
            private SqlConnection _con;
        }
    }
}

