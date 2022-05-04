using System.Windows;
using System.Windows.Controls;

namespace QuadroTestPlatform
{

    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
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
    }
}