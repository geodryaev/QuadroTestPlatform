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
    public partial class CreateNewTems : Window
    {
        public CreateNewTems()
        {
            InitializeComponent();
            time.Items.Add("0,5");
            time.Items.Add("1");
            time.Items.Add("1,5");
            time.Items.Add("2");
            time.Items.Add("2,5");
            time.Items.Add("3");
            time.SelectedItem = time.Items[1];
            setNumberItem(five);
            setNumberItem(three);
            setNumberItem(four);
            setNumberItem(tb_numberQusetion);
        }

        private void setNumberItem(ComboBox item)
        {
            for (int i = 0;i <100;i++)
            {
                item.Items.Add(i);
            }
        }
        
        private void submit_Click(object sender, RoutedEventArgs e)
        {
            if(tb_numberQusetion.Text == "" ||tb_nameTems.Text == "" || five.Text == "" || four.Text == "" || three.Text == "")
            {
                MessageBox.Show("Заполните все поля","Внимание",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }

            if (isExist(tb_nameTems.Text.Trim()))
            {
                MessageBox.Show("Данная дисциплина уже существует", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                int a = Convert.ToInt32(five.Text.ToString().Trim());
                a = Convert.ToInt32(four.Text.ToString().Trim());
                a = Convert.ToInt32(three.Text.ToString().Trim());
                a = Convert.ToInt32(tb_numberQusetion.Text.ToString().Trim());
            }
            catch
            {
                MessageBox.Show("Введите корректно критерии оценивания", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!(Convert.ToInt32(five.Text.ToString().Trim()) < Convert.ToInt32(four.Text.ToString().Trim()) || Convert.ToInt32(four.Text.ToString().Trim()) < Convert.ToInt32(three.Text.ToString().Trim())))
            {
                MessageBox.Show("Внивание, вы ввели неверно критери оценивани (не может критерий за 5 быть больше или равен за 4).", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            using (SqlConnection conn = new SqlConnection(strSQLConnection()))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand();
                comm.CommandText = "INSERT INTO t_tems (Name, tree, four, five, CountQMax, time, tr) VALUES (\'" + tb_nameTems.Text.Trim() + "\', " + three.Text.Trim() + ", " + four.Text.Trim() + ", " + five.Text.Trim() + ", " + tb_numberQusetion.Text.Trim() + ", \'" + time.Text.Trim() + "\', 1)";
                comm.Connection = conn;
                comm.ExecuteNonQuery();
                conn.Close();
                Close();
            }
        }

        public bool isExist (string str)
        {
            using (SqlConnection c = new SqlConnection(strSQLConnection()))
            {
                c.Open();
                SqlCommand com = new SqlCommand("SELECT * FROM t_tems");
                com.Connection = c;
                SqlDataReader read = com.ExecuteReader();
                while (read.Read())
                {
                    if (read.GetValue(1).ToString().ToLower() == str.ToLower().Trim())
                        return true;
                }
                read.Close();
                c.Close();
            }

            return false;
        }
        public string strSQLConnection()
        {
            return "Server=" + Properties.Settings.Default.pathSQL + ";Initial Catalog =QTPDB; User ID = sa; Password = qwerty12";
        }
    }
}
