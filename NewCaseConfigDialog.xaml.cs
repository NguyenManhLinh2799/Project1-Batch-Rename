using System;
using System.Collections.Generic;
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

namespace đồ_án_1___interface
{
    /// <summary>
    /// Interaction logic for NewCaseConfigDialog.xaml
    /// </summary>
    public partial class NewCaseConfigDialog : Window
    {
        NewCaseArgs myArgs;

        public NewCaseConfigDialog(StringArgs args)
        {
            InitializeComponent();

            myArgs = args as NewCaseArgs;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (upAll.IsChecked == true)
            {
                myArgs.Mode = 0;
            }
            else if (lowAll.IsChecked == true)
            {
                myArgs.Mode = 1;
            }
            else if (upFirst.IsChecked == true)
            {
                myArgs.Mode = 2;
            }
            DialogResult = true;
            Close();
        }
    }
}
