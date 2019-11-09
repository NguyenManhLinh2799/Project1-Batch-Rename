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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Winform = System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using Microsoft.WindowsAPICodePack.Dialogs;
using app;

namespace đồ_án_1___interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BindingList<StringOperation> methodList = new BindingList<StringOperation>();
        BindingList<StringOperation> addedList = new BindingList<StringOperation>();
        BindingList<file>ListFile = new BindingList<file>();
        BindingList<file> ListFolder = new BindingList<file>();
        FileInfo[] File=null;
        DirectoryInfo[] Folder = null;
        public MainWindow()
        {
            InitializeComponent();

            methodList.Add(new ReplaceOperation() { Args = new ReplaceArgs() { From = "...", To = "..." } });
            methodList.Add(new NewCaseOperation() { Args = new NewCaseArgs() { Mode = 0 } });
            methodList.Add(new NormalizeOperation());
            methodList.Add(new MoveOperation() { Args = new MoveArgs() { Mode = 0 } });
            methodList.Add(new UniqueNameOperation());
            comboMethod.SelectedIndex = 0;
            comboMethod.ItemsSource = methodList;
            listMethod.ItemsSource = addedList;
        }

        private void add_method(object sender, RoutedEventArgs e)
        {
            var method = comboMethod.SelectedItem as StringOperation;
            addedList.Add(method.Clone());
        }

        private void Delete_Button_Clicked(object sender, RoutedEventArgs e)
        {
            var index = listMethod.Items.IndexOf(listMethod.SelectedItem);
            if (index >= 0 && index < addedList.Count)
            {
                addedList.RemoveAt(index);
            }
        }

        private void Start_Clicked(object sender, RoutedEventArgs e)
        {
            // start batching
        }

        private void AddFile_Click(object sender, RoutedEventArgs e)
        {
            var screen = new CommonOpenFileDialog();
            screen.IsFolderPicker = true;

            if (screen.ShowDialog() == CommonFileDialogResult.Ok)
            {

                //lấy tên file
                DirectoryInfo Filename = new DirectoryInfo(screen.FileName.ToString());

                //lấy tất cả thư mục trong filename

                //cần 2 biến toàn cục để lưu file lại
                var FileNames = Filename.GetFiles();

                foreach (var files in FileNames)
                {
                    file a = new file(files.Name, "", files.Directory.ToString(), "");
                    ListFile.Add(a);
                }
                DSlistFile.ItemsSource = ListFile;

                //var newname = screen.FileName.ToString() + "\\kha";
                //FileNames[1].MoveTo(newname);

                //MessageBox.Show(newname);
                //nếu là file lấy đừng dẫn là tới thư mục gốc của nó là . Directory, đuôi file là .Extention
                //duyệt từng thư mục
                //foreach (var name in FileNames)
                //{
                //    var newName = name.Root + ;

                //}
            }
        }

        private void AddFolder_Click(object sender, RoutedEventArgs e)
        {
            var screen = new CommonOpenFileDialog();
            screen.IsFolderPicker = true;

            if (screen.ShowDialog() == CommonFileDialogResult.Ok)
            {

                //lấy tên file
                DirectoryInfo Filename = new DirectoryInfo(screen.FileName.ToString());

                //lấy tất cả thư mục trong filename
                Folder = Filename.GetDirectories();

                foreach (var files in Folder)
                {
                    file a = new file(files.Name, "", screen.FileName.ToString(), "");
                    //a.newName = files.Name;
                    ListFolder.Add(a);
                }
                DSlistFolder.ItemsSource = ListFolder;
            }
        }

        private void Clear_Clicked(object sender, RoutedEventArgs e)
        {

        }
    }
}
