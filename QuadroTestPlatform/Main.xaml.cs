using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Xml.Serialization;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using OfficeOpenXml.Style;
using Excel = Microsoft.Office.Interop.Excel;
using Window = System.Windows.Window;

namespace QuadroTestPlatform
{

    public partial class Main : Window
    {
        string[] d;
        int state;
        Personality persons;
        AllPerson allPerson;
        myAll all;
        public Main()
        {
            InitializeComponent();
            state = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(strSQLConnection()))
                {
                    clearTree(ref lBox);
                    conn.Open();
                    SqlCommand command = new SqlCommand();
                    command.CommandText = "SELECT * FROM t_tems";
                    command.Connection = conn;
                    SqlDataReader read = command.ExecuteReader();
                    while (read.Read())
                    {
                        string buf = "";
                        if(read.GetValue(7).ToString() == "1")
                        {
                            buf = "Вкл.";
                        }
                        else
                        {
                            buf = "Выкл.";
                        }
                        lb_.Items.Add(read.GetValue(1).ToString() + getVoidSpaceOne(read.GetValue(1).ToString(), 30) + buf);
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

                allPerson = new AllPerson(strSQLConnection());
                if(allPerson._arr != null)
                {
                    for (int i = allPerson._arr.Length - 1; i > -1; i--)
                    {
                        string answer =
                            Convert.ToString(allPerson._arr.Length - i) +
                            getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i), 5) + ///---
                            allPerson._arr[i]._unit +
                            getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i) + getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i), 5) + allPerson._arr[i]._unit, 20) + ///---
                            allPerson._arr[i]._zvezda +
                            getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i) +
                            getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i), 5) + ///---
                            allPerson._arr[i]._unit +
                            getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i) + getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i), 5) + allPerson._arr[i]._unit, 20) + ///---
                            allPerson._arr[i]._zvezda, 50) + ///---
                            allPerson._arr[i]._name +
                            getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i) +
                            getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i), 5) + ///---
                            allPerson._arr[i]._unit +
                            getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i) + getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i), 5) + allPerson._arr[i]._unit, 20) + ///---
                            allPerson._arr[i]._zvezda +
                            getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i) +
                            getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i), 5) + ///---
                            allPerson._arr[i]._unit +
                            getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i) + getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i), 5) + allPerson._arr[i]._unit, 20) + ///---
                            allPerson._arr[i]._zvezda, 50) + ///---
                            allPerson._arr[i]._name, 75) + ///---
                            allPerson._arr[i]._numberUnit;
                        lBox.Items.Add(answer);
                    }
                    string[] buf;
                    int count = 0;
                    using (SqlConnection con = new SqlConnection(strSQLConnection()))
                    {
                        con.Open();
                        SqlCommand comnad = new SqlCommand("SELECT * FROM t_tems");
                        comnad.Connection = con;
                        SqlDataReader read = comnad.ExecuteReader();
                        while (read.Read())
                        {
                            count++;
                        }
                        buf = new string[count];
                        count = 0;
                        read.Close();
                        read = comnad.ExecuteReader();
                        while (read.Read())
                        {
                            buf[count] = read.GetString(1);
                            count++;
                        }
                        read.Close();
                        con.Close();
                    }
                    all = new myAll(buf, strSQLConnection());
                }
                refeshLB();
            }
            catch
            {
                MessageBox.Show("Остуствует подключение к MS SQL Servers, провертье наличие таблиц");
            }
}

        private string CutWord(string word)
        {
            string answer = "";
            if (word.Length > 25)
            {
                for (int i = 0; i < 25; i++)
                {
                    answer += word[i];
                }
            }
            else
            {
                return word;
            }

            return answer+"...";
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreateNewTems form = new CreateNewTems();
            form.ShowDialog();
        }
        private void b_deleteTems_Click(object sender, RoutedEventArgs e)
        {
            if (lb_.SelectedIndex != -1)
            {
                string d = getNameTemsFromStr(lb_.Items[lb_.SelectedIndex].ToString());
                MessageBoxResult b = MessageBox.Show(
                    "Вы действительно хотите перевести состояние темы - " + d,
                    "Подтвердите действие",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Question,MessageBoxResult.Cancel,
                    MessageBoxOptions.DefaultDesktopOnly
                );
                if (b == MessageBoxResult.OK)
                {
                    string a = "";
                    using (SqlConnection conn = new SqlConnection(strSQLConnection()))
                    {
                        conn.Open();
                        SqlCommand com = new SqlCommand();
                        com.CommandText = "SELECT * FROM t_tems";
                        com.Connection = conn;
                        SqlDataReader read = com.ExecuteReader();
                        while (read.Read())
                        {
                            if (d == read.GetValue(1).ToString())
                                a = read.GetValue(7).ToString();
                        }
                        if (a == "1")
                        {
                            a = "0";
                        }
                        else
                        {
                            a = "1";
                        }
                        com.CommandText = "UPDATE t_tems SET tr = \'" + a +"\' WHERE Name = \'"+d+"\'";
                        read.Close();
                        com.ExecuteNonQuery();

                        conn.Close();
                    }
                }
                refeshLB();
            }
        }
        public void clearTree(ref System.Windows.Controls.ListBox a)
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
        public string searhForID(string id, string table, int numberColumn)
        {
            string answer = null;
            using (SqlConnection con = new SqlConnection(strSQLConnection()))
            {
                con.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM "+table);
                command.Connection = con;
                SqlDataReader read = command.ExecuteReader();
                while (read.Read())
                {
                    if (read.GetString(0) == id)
                    {
                        answer = read.GetString(numberColumn);
                    }
                }

            }
            return answer;
        }
        public string timeDay(string time)
        {
            string answewr = "";
            for (int i = 0; time[i] != ' '; i++)
            {
                answewr += time[i];
            }

            return answewr;
        }
        public string timeTime(string time)
        {
            string answewr = "";
            int i = 0;
            for (; time[i] != ' '; i++)
            {

            }
            i++;
            for(;i< time.Length;i++)
            {
                answewr += time[i];
            }
            return answewr;
        }
        public string getVoidSpaceOne(string str, int needSpaace)
        {
            string answer = "";

            for (int i = 0; i < needSpaace - str.Length; i++)
                answer += " ";
            
            return answer;
        }
        class AllPerson
        {
            public AllPerson(string sqlConnection)
            {
                using (SqlConnection con = new SqlConnection(sqlConnection))
                {
                    con.Open();
                    _numberInArray = 0;
                    SqlCommand command = new SqlCommand("SELECT * FROM t_result");
                    command.Connection = con;
                    SqlDataReader read = command.ExecuteReader();
                    _arrAll = new myStruct[0];
                    while(read.Read())
                    {
                        PushToAll(read.GetValue(1).ToString(), read.GetValue(2).ToString(), read.GetValue(3).ToString(), read.GetValue(4).ToString(), read.GetValue(5).ToString(), read.GetValue(6).ToString(), read.GetValue(7).ToString());
                    }
                    read.Close();
                    con.Close();
                }
                if (_arrAll.Length != 0)
                {
                    set();
                }
            }

            private void set()
            {
                for (int i = 0; i < _arrAll.Length; i++)
                {
                    Push(_arrAll[i]._unit, _arrAll[i].numberUnit, _arrAll[i]._zvezda, _arrAll[i]._name, _arrAll[i]._date, _arrAll[i]._dis, _arrAll[i]._answerUser, Exsist(_arrAll[i]._unit, _arrAll[i].numberUnit, _arrAll[i]._zvezda, _arrAll[i]._name));
                }

                _arr = new Personality[_arrStruct.Length];

                for (int i = 0; i < _arrStruct.Length; i++)
                {
                    _arr[i] = new Personality(_arrStruct[i]._unit, _arrStruct[i]._numberUnit, _arrStruct[i]._zvezda, _arrStruct[i]._name, _arrStruct[i]._date, _arrStruct[i]._dis, _arrStruct[i]._answerUser);
                }
            }
            
            private void PushToAll (string unit, string numberUnit, string zvezda, string name, string time, string dis, string answerUser)
            {
                myStruct[] buufer = new myStruct[_arrAll.Length + 1];
                for (int i =0; i < _arrAll.Length;i++)
                {
                    buufer[i] = _arrAll[i];
                }

                buufer[_arrAll.Length]._unit = unit;
                buufer[_arrAll.Length].numberUnit = numberUnit;
                buufer[_arrAll.Length]._zvezda  = zvezda;
                buufer[_arrAll.Length]._name = name;
                buufer[_arrAll.Length]._date = time;
                buufer[_arrAll.Length]._dis = dis;
                buufer[_arrAll.Length]._answerUser = answerUser;
                _arrAll = buufer;
            }

            private void Push(string unit, string numberUnit, string zvezda, string name, string time, string dis, string answerUser, bool noFirst)
            {
                if (noFirst)
                {
                    _arrStruct[_numberInArray].Push(time, dis, answerUser);
                }
                else
                {
                    structRead[] buffer = new structRead[_arrStruct.Length + 1];
                    for (int i = 0; i < _arrStruct.Length; i++)
                    {
                        buffer[i] = _arrStruct[i];
                    }
                    buffer[_arrStruct.Length] = new structRead(unit, numberUnit, zvezda, name);
                    buffer[_arrStruct.Length].Push(time, dis, answerUser);
                    _arrStruct = buffer;
                }
            }
            private bool Exsist(string unit, string numberUnit, string zvezda, string name)
            {
                if (_arrStruct == null)
                {
                    _arrStruct = new  structRead[0];
                }
                for (int i = 0; i < _arrStruct.Length; i++)
                {
                    if (_arrStruct[i]._unit == unit)
                    {
                        if (_arrStruct[i]._numberUnit == numberUnit)
                        {
                            if (_arrStruct[i]._zvezda == zvezda)
                            {
                                if (_arrStruct[i]._name == name)
                                {
                                    _numberInArray = i;
                                    return true;
                                }
                            }
                        }
                    }
                }
                return false;
            }
            private int _numberInArray;
            private structRead[] _arrStruct; 
            private myStruct[] _arrAll;
            public Personality[] _arr;
        }
        public class Personality
        {
            public Personality (string unit, string numberUnit, string zvezda, string name, string[]date, string[] dis, string[] answerUser)
            {
                _unit = unit;
                _zvezda = zvezda;
                _numberUnit = numberUnit;
                _name = name;
                _arr = new DisPer[date.Length];
                for (int i = 0; i < date.Length; i++) 
                {
                    _arr[i] = new DisPer(dis[i], date[i], answerUser[i]);
                }
            }

            public int _ozenka;
            public string _unit;
            public string _numberUnit;
            public string _zvezda;
            public string _name;
            public DisPer[] _arr; 
        }
        public class DisPer
        {
            public DisPer(string nameDis, string time, string answerUser)
            {
                _nameDis = nameDis;
                _time = time;
                int count = 0;
                for (int i = 0; i < answerUser.Length; i++)
                {
                    if (answerUser[i] == '&')
                        count++;
                }
                string buffer = "";
                _arr = new Question[count];
                count = 0;
                for (int i = 0; i < answerUser.Length; i++)
                {
                    if (answerUser[i] != '&')
                    {
                        buffer += answerUser[i];
                    }
                    else
                    {
                        _arr[count] = new Question(buffer);
                        count++;
                        buffer = "";
                    }
                }
            }

            public string _nameDis, _time;
            public Question[] _arr; 
        }
        public class Question
        {
            public Question(string str)
            {
                int i = 0;
                string buffer = "";
                for (; i < str.Length; i++)
                {
                    if (str[i]!= '-')
                    {
                        buffer += str[i];
                    }
                    if (str[i] == '-') 
                        break;

                }
                i++;
                _answer = null;
                _IDQuestion = buffer;
                buffer = "";
                int count = 0;
                for (int j = 0; j < str.Length; j++)
                {
                    if (str[j] == '|')
                        count++;
                }
                _answer = new string[count];
                count = 0;
                for (; i < str.Length; i++)
                {
                    if (str[i] != '|')
                    {
                        buffer += str[i];
                    }
                    if (str[i] == '|')
                    {
                        _answer[count] = buffer;
                        buffer = "";
                        count++;
                    }
                }
            }

            public string _IDQuestion;
            public string[] _answer;
        }
        public struct myStruct
        {
            public string _unit, _name, numberUnit, _zvezda, _date, _dis, _answerUser;
        }
        public class structRead
        {
            public structRead(string unit, string numberUnit, string zvezda, string name)
            { 
                _unit = unit;
                _numberUnit = numberUnit;
                _zvezda = zvezda;
                _name = name;
                _count = 0;
                _date = new string[_count];
                _dis = new string[_count];
                _answerUser = new string[_count];
            }

            public void Push (string date, string dis, string answerUser)
            {
                _count++;
                string[] buffer1, buffer2, buffer3;
                buffer1 = new string[_count];
                buffer2 = new string[_count];
                buffer3 = new string[_count];

                for (int i = 0; i < _dis.Length; i++)
                {
                    buffer1[i] = _date[i];
                    buffer2[i] = _dis[i];
                    buffer3[i] = _answerUser[i];
                }
                buffer1[_count-1] = date;
                buffer2[_count-1] = dis;
                buffer3[_count-1] = answerUser;
                _date = buffer1;
                _dis = buffer2;
                _answerUser = buffer3;
            }

            private int _count;

            public string _unit, _numberUnit, _zvezda, _name;
            public string[] _date, _dis, _answerUser;
        }
        public class myAll
        {
            public myAll(string[] nameDisciplines, string strSQLconnection)
            {
                string n = "0";
                _connectionSQL = strSQLconnection;
                _arrayDisciplines = new Disciplines[nameDisciplines.Length];
                for (int i = 0; i < nameDisciplines.Length; i++)
                {

                    using (SqlConnection con = new SqlConnection(_connectionSQL))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand("SELECT * FROM t_tems");
                        com.Connection = con;
                        SqlDataReader read = com.ExecuteReader();
                        while (read.Read())
                        {
                            if (nameDisciplines[i] == read.GetString(1).ToString())
                            {
                                n = read.GetString(5).ToString();
                            }
                        }
                    }
                    _arrayDisciplines[i] = new Disciplines(nameDisciplines[i], _connectionSQL, Convert.ToInt32(n));
                }
            }
            private string _connectionSQL;
            public Disciplines[] _arrayDisciplines;
            public int _count;
        }
        public class Disciplines
        {
            public Disciplines(string nameDis, string strSQLconnection, int countQuestionTesting)
            {
                _nameDisciplines = nameDis;
                _countQuestionTesting = countQuestionTesting;
                _connection = new SqlConnection(strSQLconnection);
                _connection.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = "SELECT * FROM t_tems";
                command.Connection = _connection;
                SqlDataReader read = command.ExecuteReader();
                r = new Random();

                while (read.Read())
                {
                    if (read.GetValue(1).ToString() == _nameDisciplines)
                    {
                        _tKey = read.GetValue(0).ToString();
                        _free = Convert.ToInt32(read.GetValue(2).ToString());
                        _four = Convert.ToInt32(read.GetValue(3).ToString());
                        _five = Convert.ToInt32(read.GetValue(4).ToString());
                        _time = Convert.ToDouble(read.GetValue(6).ToString());
                    }
                }
                read.Close();
                _connection.Close();
                _arrayQuestion = new QuestionFF[_countQuestionTesting];
                setQuestion();
            }
            public Disciplines(string nameDis, string strSQLconnection, int countQuestionTesting, string[] arrIndex)
            {
                _arrIndex = arrIndex;
                _nameDisciplines = nameDis;
                _countQuestionTesting = countQuestionTesting;
                _connection = new SqlConnection(strSQLconnection);
                _connection.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = "SELECT * FROM t_tems";
                command.Connection = _connection;
                SqlDataReader read = command.ExecuteReader();
                r = new Random();

                while (read.Read())
                {
                    if (read.GetValue(1).ToString() == _nameDisciplines)
                    {
                        _tKey = read.GetValue(0).ToString();
                        _free = Convert.ToInt32(read.GetValue(2).ToString());
                        _four = Convert.ToInt32(read.GetValue(3).ToString());
                        _five = Convert.ToInt32(read.GetValue(4).ToString());
                        _time = Convert.ToDouble(read.GetValue(6).ToString());
                    }
                }
                read.Close();
                _connection.Close();
                _arrayQuestion = new QuestionFF[_countQuestionTesting];
                setQuestionArr();
            }
            private int countQuestion()
            {
                int count = 0;
                _connection.Open();
                SqlCommand com = new SqlCommand("SELECT * FROM t_question");
                com.Connection = _connection;
                SqlDataReader read = com.ExecuteReader();
                while (read.Read())
                {
                    if (read.GetValue(1).ToString() == _tKey)
                    {
                        count++;
                    }
                }
                read.Close();
                _connection.Close();
                return count;
            }
            private void setQuestion()
            {
                int count = 0;
                QuestionFF[] bufferArrayQuestion = new QuestionFF[countQuestion()];
                _connection.Open();
                SqlCommand com = new SqlCommand("SELECT * FROM t_question");
                com.Connection = _connection;
                SqlDataReader read = com.ExecuteReader();
                while (read.Read())
                {
                    if (read.GetValue(1).ToString() == _tKey && read.GetValue(4).ToString() == "0")
                    {
                        bufferArrayQuestion[count]._nameQuestrion = read.GetValue(3).ToString();
                        bufferArrayQuestion[count]._kQuestion = read.GetValue(0).ToString();
                        bufferArrayQuestion[count]._kTrueAnswer = getArrayKTrueAnswer(read.GetValue(2).ToString());
                        count++;
                    }
                }
                read.Close();
                int[] bufNubmerRandom = getArray(bufferArrayQuestion.Length);
                _connection.Close();
                if(bufNubmerRandom.Length !=0)
                {
                    for (int i = 0; i < _arrayQuestion.Length; i++)
                    {
                        _arrayQuestion[i] = bufferArrayQuestion[randomNumber(ref bufNubmerRandom)];
                        _arrayQuestion[i]._kAnswers = getArrayKAnswer(_arrayQuestion[i]._kQuestion);
                    }
                }
                
            }
            private void setQuestionArr()
            {
                int count = 0;
                QuestionFF[] bufferArrayQuestion = new QuestionFF[countQuestion()];
                _connection.Open();
                SqlCommand com = new SqlCommand("SELECT * FROM t_question");
                com.Connection = _connection;
                SqlDataReader read = com.ExecuteReader();
                while (read.Read())
                {
                    if (read.GetValue(1).ToString() == _tKey)
                    {
                        bufferArrayQuestion[count]._nameQuestrion = read.GetValue(3).ToString();
                        bufferArrayQuestion[count]._kQuestion = read.GetValue(0).ToString();
                        bufferArrayQuestion[count]._kTrueAnswer = getArrayKTrueAnswer(read.GetValue(2).ToString());
                        count++;
                    }
                }
                read.Close();
                _connection.Close();
                for (int i = 0; i < _arrIndex.Length; i++)
                {
                    for (int j = 0; j < bufferArrayQuestion.Length;j++)
                    {
                        if (bufferArrayQuestion[j]._kQuestion == _arrIndex[i])
                        {
                            _arrayQuestion[i] = bufferArrayQuestion[j];
                            _arrayQuestion[i]._kAnswers = getArrayKAnswer(_arrayQuestion[i]._kQuestion);

                        }
                    }
                }
            }
            private int randomNumber(ref int[] arrInt)
            {
                int numberRandom = r.Next(0, arrInt.Length), random = arrInt[numberRandom], count = 0;
                int[] newArr = new int[arrInt.Length - 1];
                for (int i = 0; i < arrInt.Length; i++)
                {
                    if (i != numberRandom)
                    {
                        newArr[count] = arrInt[i];
                        count++;
                    }
                }
                arrInt = newArr;
                return random;
            }

            private int[] getArray(int number)
            {
                int[] answer = new int[number];
                for (int i = 0; i < answer.Length; i++)
                {
                    answer[i] = i;
                }

                return answer;
            }
            private string[] getArrayKTrueAnswer(string str)
            {
                if (str.Length > 0)
                {
                    int count = 0, indexArray = 0;
                    string buffer = "";
                    for (int i = 0; i < str.Length; i++)
                    {
                        if (str[i] == '|')
                        {
                            count++;
                        }
                    }
                    string[] answer = new string[count];
                    for (int i = 0; i < str.Length; i++)
                    {
                        buffer = "";
                        while (i < str.Length && str[i] != '|')
                        {
                            buffer += str[i];
                            i++;
                        }
                        if (buffer != "")
                        {
                            answer[indexArray] = buffer;
                            indexArray++;
                        }
                    }
                    return answer;
                }
                return null;
            }
            private string[] getArrayKAnswer(string qKey)
            {
                string[] str;
                int _count = 0;
                _connection.Open();
                SqlCommand com = new SqlCommand("SELECT * FROM t_answer");
                com.Connection = _connection;
                SqlDataReader read = com.ExecuteReader();
                while (read.Read())
                {
                    if (qKey == read.GetValue(1).ToString())
                        _count++;
                }
                str = new string[_count];
                read.Close();
                read = com.ExecuteReader();
                _count = 0;
                while (read.Read())
                {
                    if (qKey == read.GetValue(1).ToString())
                    {
                        str[_count] = read.GetValue(0).ToString();
                        _count++;
                    }
                }
                _connection.Close();

                return str;
            }

            private string[] _arrIndex;
            public Random r;
            private int _countQuestionTesting;
            public int _five, _four, _free;
            public double _time;
            private string _tKey;
            public string _nameDisciplines;
            private SqlConnection _connection;
            public QuestionFF[] _arrayQuestion;
        }
        public struct QuestionFF
        {
            public string _nameQuestrion, _kQuestion;
            public string[] _kAnswers;
            public string[] _kTrueAnswer; //хранит индекс правильного ответа
            public string[] _answerUser;
        }
        public int getIndex (string str)
        {
            string buf = "";
            for (int i = 0; i < str.Length;i++)
            {
                if (str[i] == '.')
                    return Convert.ToInt32(buf);

                buf += str[i];
            }
            
            return Convert.ToInt32(buf);
        }
        private void lBox_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (state == 0)
            {
                persons = allPerson._arr[allPerson._arr.Length - 1 - lBox.SelectedIndex];
                state++;
                lBox.Items.Clear();
                label_name.Content = persons._name;
                label_unit.Content = persons._unit;
                label_zvezda.Content = persons._zvezda;
                d = new string[persons._arr.Length]; 
                for (int i = 0; i < persons._arr.Length;i++)
                {
                    lBox.Items.Add(
                        persons._arr[i]._nameDis + 
                        getVoidSpaceOne(persons._arr[i]._nameDis, 20) +
                        timeDay(persons._arr[i]._time) + 
                        getVoidSpaceOne(persons._arr[i]._nameDis + getVoidSpaceOne(persons._arr[i]._nameDis, 20) + timeDay(persons._arr[i]._time),35) +
                        timeTime(persons._arr[i]._time));
                    d[i] = persons._arr[i]._nameDis;
                }
                return;
            }
            if (state == 1)
            {
                int countI = 0;
                string[] bbdd = new string[1];
                string nameDisIsSelect = d[lBox.SelectedIndex];
                bbdd[0] = nameDisIsSelect;
                for (;countI < persons._arr.Length; countI++)
                {
                    if (persons._arr[countI]._nameDis == nameDisIsSelect)
                        break;
                    
                }
                string[] buffArrayIndex = new string[persons._arr[countI]._arr.Length];
                for (int i = 0; i < buffArrayIndex.Length; i++) 
                {
                    buffArrayIndex[i] = persons._arr[countI]._arr[i]._IDQuestion;
                }
                Disciplines buf = new Disciplines(nameDisIsSelect, strSQLConnection(), persons._arr[countI]._arr.Length, buffArrayIndex);
                for (int i =0; i < buf._arrayQuestion.Length;i++)
                {
                    buf._arrayQuestion[i]._answerUser = persons._arr[countI]._arr[i]._answer;
                }

                Result form = new Result(buf, 1, bbdd);
            }
        }
        private void b_backList_Click(object sender, RoutedEventArgs e)
        {
            switch (state)
            {
                case 0:
                    break;
                case 1:
                    state = 0;
                    lBox.Items.Clear();
                    for (int i = allPerson._arr.Length - 1; i > -1; i--)
                    {
                        string answer =
                            Convert.ToString(allPerson._arr.Length - i) +
                            getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i), 5) + ///---
                            allPerson._arr[i]._unit +
                            getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i) + getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i), 5) + allPerson._arr[i]._unit, 20) + ///---
                            allPerson._arr[i]._zvezda +
                            getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i) +
                            getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i), 5) + ///---
                            allPerson._arr[i]._unit +
                            getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i) + getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i), 5) + allPerson._arr[i]._unit, 20) + ///---
                            allPerson._arr[i]._zvezda, 50) + ///---
                            allPerson._arr[i]._name +
                            getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i) +
                            getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i), 5) + ///---
                            allPerson._arr[i]._unit +
                            getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i) + getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i), 5) + allPerson._arr[i]._unit, 20) + ///---
                            allPerson._arr[i]._zvezda +
                            getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i) +
                            getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i), 5) + ///---
                            allPerson._arr[i]._unit +
                            getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i) + getVoidSpaceOne(Convert.ToString(allPerson._arr.Length - i), 5) + allPerson._arr[i]._unit, 20) + ///---
                            allPerson._arr[i]._zvezda, 50) + ///---
                            allPerson._arr[i]._name, 75) + ///---
                            allPerson._arr[i]._numberUnit;
                        lBox.Items.Add(answer);
                    }
                    label_name.Content = null;
                    label_unit.Content = null;
                    label_zvezda.Content = null;
                    break;
                default:
                    break;
            }

        }
        private void lb__MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lb_.SelectedIndex != -1)
            {
                Tems form = new Tems(getNameTemsFromStr(lb_.Items[lb_.SelectedIndex].ToString()));
                form.ShowDialog();
            }
        }
        public void refeshLB ()
        {
            lb_.Items.Clear();
            using (SqlConnection c = new SqlConnection(strSQLConnection()))
            {
                string OnOrOf;
                int countTemsForLB = 0;
                c.Open();
                SqlCommand com = new SqlCommand("SELECT * FROM t_tems");
                com.Connection = c;
                SqlDataReader read = com.ExecuteReader();
                while (read.Read())
                {
                    if (read.GetValue(7).ToString() == "1")
                    {
                        OnOrOf = "Вкл";
                    }
                    else
                    {
                        OnOrOf = "Выкл";
                    }
                    countTemsForLB++;
                    lb_.Items.Add( Convert.ToString(countTemsForLB) + "." + getVoidSpaceOne(Convert.ToString(countTemsForLB) + ".", 4) +  
                        read.GetValue(1).ToString() + getVoidSpaceOne(Convert.ToString(countTemsForLB) + "." + getVoidSpaceOne(Convert.ToString(countTemsForLB) + ".", 4) + CutWord(read.GetValue(1).ToString()), 45) + OnOrOf);
                }
                read.Close();
                c.Close();
            }
        }
        public string getNameTemsFromStr(string str)
        {
            string ansewr = "";
            int i = 1;
            for (; i < str.Length; i++)
            {
                if (str[i - 1] == '.') 
                    break;
            }
            for (; i < str.Length; i++)
            {
                if (str[i] == 'В')
                    break;
                ansewr += str[i];
                
            }


            return ansewr.Trim();
        }
        private void b_refresh_Click(object sender, RoutedEventArgs e)
        {
            refeshLB();
        }
        private void b_download_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if(saveFileDialog.ShowDialog() == true)
            {
                Excel.Application app;
                Excel.Workbook book;
                Excel.Worksheet sheet;

                app = new Excel.Application();
                book = app.Workbooks.Add();
                sheet = (Excel.Worksheet)book.Worksheets.get_Item(1);
                DownloadExscel(sheet);

                book.SaveAs(saveFileDialog.FileName);
                book.Close(true);
                app.Quit();
            }
        }
        //row,column
        public void DownloadExscel (Excel.Worksheet sheet)
        {
            int indexWrite = 0;
            sheet.Cells[1,1] = "№";
            sheet.Cells[1,1].Font.Bold = true;
            sheet.Cells[1,2] = "Подраздение";
            sheet.Cells[1,2].Font.Bold = true; ;
            sheet.Cells[1,3] = "Воинское звание";
            sheet.Cells[1,3].Font.Bold = true; 
            sheet.Cells[1,4] = "ФИО";
            sheet.Cells[1,4].Font.Bold = true;
            sheet.Cells[1,5] = "Дата";
            sheet.Cells[1,5].Font.Bold = true;
            int row = 1;

            for (int i = 0; i < allPerson._arr.Length; i++) 
            {
                for (int j = 0; j < allPerson._arr[i]._arr.Length; j++) 
                {
                    if (timeDay(allPerson._arr[i]._arr[j]._time) != sheet.Cells[row, 5].Text)
                    {
                        row++;
                        indexWrite++;
                        sheet.Cells[row, 1] = indexWrite;
                        sheet.Cells[row, 2] = allPerson._arr[i]._unit;
                        sheet.Cells[row, 3] = allPerson._arr[i]._zvezda;
                        sheet.Cells[row, 4] = allPerson._arr[i]._name;
                    }

                    if (IsExsistExcel(allPerson._arr[i]._arr[j]._nameDis, sheet) == -1)
                    {

                        int buf = newIndexDisExcel(sheet, allPerson._arr[i]._arr[j]._nameDis);
                        sheet.Cells[row, buf] = getOz(allPerson._arr[i], j, all);

                        switch (sheet.Cells[row, buf].Text)
                        {
                            case "5":
                                sheet.Cells[row, buf].Interior.Color = Excel.XlRgbColor.rgbGreen;
                                break;
                            case "4":
                                sheet.Cells[row, buf].Interior.Color = Excel.XlRgbColor.rgbYellow;
                                break;
                            case "3":
                                sheet.Cells[row, buf].Interior.Color = Excel.XlRgbColor.rgbOrange;
                                break;
                            case "2":
                                sheet.Cells[row, buf].Interior.Color = Excel.XlRgbColor.rgbRed;
                                break;
                            default:
                                break;
                        }
                        sheet.Cells[row, 5] = timeDay(allPerson._arr[i]._arr[j]._time);
                    }
                    else
                    {
                        if (sheet.Cells[row, IndexDisExcel(sheet, allPerson._arr[i]._arr[j]._nameDis)].Text != "")
                        {
                            row++;
                            indexWrite++;
                            sheet.Cells[row, 1] = indexWrite;
                            sheet.Cells[row, 2] = allPerson._arr[i]._unit;
                            sheet.Cells[row, 3] = allPerson._arr[i]._zvezda;
                            sheet.Cells[row, 4] = allPerson._arr[i]._name;
                        }
                        sheet.Cells[row, IndexDisExcel(sheet, allPerson._arr[i]._arr[j]._nameDis)] = getOz(allPerson._arr[i], j, all);
                        switch (sheet.Cells[row, IndexDisExcel(sheet, allPerson._arr[i]._arr[j]._nameDis)].Text)
                        {
                            case "5":
                                sheet.Cells[row, IndexDisExcel(sheet, allPerson._arr[i]._arr[j]._nameDis)].Interior.Color = Excel.XlRgbColor.rgbGreen;
                                break;
                            case "4":
                                sheet.Cells[row, IndexDisExcel(sheet, allPerson._arr[i]._arr[j]._nameDis)].Interior.Color = Excel.XlRgbColor.rgbYellow;
                                break;
                            case "3":
                                sheet.Cells[row, IndexDisExcel(sheet, allPerson._arr[i]._arr[j]._nameDis)].Interior.Color = Excel.XlRgbColor.rgbOrange;
                                break;
                            case "2":
                                sheet.Cells[row, IndexDisExcel(sheet, allPerson._arr[i]._arr[j]._nameDis)].Interior.Color = Excel.XlRgbColor.rgbRed;
                                break;
                            default:
                                break;
                        }
                        if (timeDay(allPerson._arr[i]._arr[j]._time) != sheet.Cells[row, 5].Text)
                        {
                            sheet.Cells[row, 5] = timeDay(allPerson._arr[i]._arr[j]._time);
                        }

                    }
                }
            }



            int allRow = 1, allColumn = 1;

            for (int i = 1; sheet.Cells[i, 1].Text() != ""; i++)
            {
                for (int j = 1; sheet.Cells[1, j].Text() != ""; j++) 
                {
                    sheet.Cells[i, j].Borders.LineStyle = XlLineStyle.xlContinuous;
                    sheet.Cells[i, j].Borders.Weight = XlBorderWeight.xlThin;
                }
                allRow++;
            }
            for(int i=1; sheet.Cells[1,i].Text() != "";i++)
            {
                sheet.Cells[1, i].Borders.LineStyle = XlLineStyle.xlContinuous;
                sheet.Cells[1, i].Borders.Weight = XlBorderWeight.xlThick;
                allColumn++;
            }
            sheet.ListObjects.Add(XlListObjectSourceType.xlSrcRange, sheet.Range[sheet.Cells[1, 1], sheet.Cells[allRow,allColumn]].CurrentRegion, Type.Missing, XlYesNoGuess.xlYes).Name = "Таблица1";
            //sheet.get_Range(sheet.Cells[1, 1], sheet.Cells[allRow, allColumn].InsertTable(dataTable, "Name of table", true);

            sheet.Columns.Font.Size = 14;
            sheet.Columns.Font.Name = "Times New Roman";
            sheet.Columns.AutoFit();
            sheet.Rows.AutoFit();
            sheet.Columns.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
        }
        public int newIndexDisExcel (Excel.Worksheet sheet,string dis)
        {
            int i = 5;
            while (sheet.Cells[1, i].Text != null && sheet.Cells[1, i].Text != "")
            {
                i++;
            }
            sheet.Cells[1, i] = dis;
            sheet.Cells[1, i].Font.Bold = true;
            return i;
        }

        public int IndexDisExcel (Worksheet sheet, string dis)
        {
            int i = 1;
            while (sheet.Cells[1,i].Text != dis)
            {
                i++;
            }
            return i;
        }
        public int IsExsistExcel(string str, Excel.Worksheet sheet)
        {
            int i = 5;
            while (sheet.Cells[1,i].Text != null && sheet.Cells[1, i].Text != "")
            {
                if(str == sheet.Cells[1, i].Text)
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        public int getOz(Personality a, int j ,myAll all)
        {
            using (SqlConnection c = new SqlConnection(strSQLConnection()))
            {
                c.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM t_result");
                command.Connection = c;
                SqlDataReader read = command.ExecuteReader();
                while (read.Read())
                {
                    string ass= read.GetValue(2).ToString();
                    if (read.GetValue(1).ToString() == a._unit && read.GetValue(3).ToString() == a._zvezda && read.GetValue(4).ToString() ==a._name && read.GetValue(6).ToString() == a._arr[j]._nameDis && read.GetValue(5).ToString() == a._arr[j]._time)
                    {
                        return Convert.ToInt32(read.GetValue(8).ToString());
                    }
                }
            }
            return -1;
        }
    }
}