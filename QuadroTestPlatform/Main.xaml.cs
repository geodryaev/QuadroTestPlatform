using System;
using System.Data.SqlClient;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Windows;
using System.Windows.Media.Animation;
using System.Xml.Serialization;

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
                AllPerson person = new AllPerson(strSQLConnection());
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
                    comm.CommandText = "INSERT INTO t_tems (Name, tree, four, five, CountQMax, time) VALUES (\'" + TextBoxNameTems.Text.ToString().Trim() + "\', 3, 2, 1, 20, 3)";
                    comm.Connection = conn;
                    comm.ExecuteNonQuery();
                    conn.Close();
                    refresh();
                }
            }
            TextBoxNameTems.Text = ""; 
        }

        private void b_deleteTems_Click(object sender, RoutedEventArgs e)
        {
            TemsTree.Items.Remove(TemsTree.SelectedItem);
            using (SqlConnection conn= new SqlConnection (strSQLConnection()))
            {
                conn.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = "DELETE t_tems WHERE Name = '" + TemsTree.SelectedItem.ToString()+"'";
                com.Connection = conn;
                com.ExecuteNonQuery();
                conn.Close();
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
                    com.CommandText = "CREATE TABLE t_tems (Id INT PRIMARY KEY IDENTITY, Name NVARCHAR(1000), tree  NVARCHAR (1000), four  NVARCHAR (1000), five  NVARCHAR (1000), CountQMax NVARCHAR (1000), time NVARCHAR (1000))";
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
                    com.CommandText = "CREATE TABLE t_question (Id INT PRIMARY KEY IDENTITY, tKey INT NOT NULL, aKey NVARCHAR (1000), Question NVARCHAR (1000))";
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
                    com.CommandText = "CREATE TABLE t_answer (Id INT PRIMARY KEY IDENTITY, qKey INT, answer NVARCHAR (1000))";
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
                set();
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


        class Personality
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
    }
}