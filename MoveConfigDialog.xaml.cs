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
    /// Interaction logic for MoveConfigDialog.xaml
    /// </summary>
    public partial class MoveConfigDialog : Window
    {
        MoveArgs myArgs;

        public MoveConfigDialog(StringArgs args)
        {
            InitializeComponent();

            myArgs = args as MoveArgs;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (front.IsChecked == true)
            {
                myArgs.Mode = 0;
            }
            else if (back.IsChecked == true)
            {
                myArgs.Mode = 1;
            }
            DialogResult = true;
            Close();
        }
    }
}
