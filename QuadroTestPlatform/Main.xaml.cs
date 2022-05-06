using System.Windows;
using System.Data.SqlClient;

namespace QuadroTestPlatform
{

    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
            tb_pathSQL.Text = Properties.Settings.Default.pathSQL;
            TemsTree.Items.Add("asd2");
            TemsTree.Items.Add("asd3");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxNameTems.Text.ToString() != null && TextBoxNameTems.Text.ToString() != "")
            {
                TemsTree.Items.Add(TextBoxNameTems.Text.ToString());
            }
        }

        private void b_deleteTems_Click(object sender, RoutedEventArgs e)
        {
            TemsTree.Items.Remove(TemsTree.SelectedItem);
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

        public string strSQLConnection()
        {
            return "Server=" + Properties.Settings.Default.pathSQL + ";Initial Catalog =QTPDB; User ID = sa; Password = qwerty12";
        }

        private void b_createTableDB_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connect = new SqlConnection(strSQLConnection()))
            {
                connect.Open();

                connect.Close();
            }
        }
    }
}