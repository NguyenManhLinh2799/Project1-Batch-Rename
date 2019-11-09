using System.Windows;
using System.IO;
using System.ComponentModel;
using Microsoft.WindowsAPICodePack.Dialogs;
using app;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

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

        // path to Preset file
        string path = AppDomain.CurrentDomain.BaseDirectory + @"\preset.JSON";
        PresetList loadFromFile = new PresetList() { List = new List<Preset>()};
        BindingList<Preset> Presets = new BindingList<Preset>();

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

            LoadPresetFromFile();
        }

        private void Start_Clicked(object sender, RoutedEventArgs e)
        {
            // start batching
        }


        // add selected method from comboBox
        private void add_method(object sender, RoutedEventArgs e)
        {
            var method = comboMethod.SelectedItem as StringOperation;
            addedList.Add(method.Clone());
        }

        // open dialog to load folders
        private void AddFolder_Click(object sender, RoutedEventArgs e)
        {

        }

        // open dialog to load files
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


        private void LoadPresetFromFile()
        {
            using (var read = new StreamReader(path))
            {
                string json = read.ReadToEnd();
                loadFromFile = JsonConvert.DeserializeObject<PresetList>(json);
            }

            for (int i = 0; i < loadFromFile.Count; i++)
            {
                Presets.Add(loadFromFile.List[i]);
            }

            PresetCombobox.ItemsSource = Presets;
            PresetCombobox.SelectedIndex = 0;
        }

        // save current list of added method to preset file
        private void SavePreset_Clicked(object sender, RoutedEventArgs e)
        {
            var preset = new Preset();
            preset.PresetName = PresetNameTextBox.Text;
            preset.List = new List<JSONOperation>();

            foreach(var method in addedList)
            {
                preset.AddToList(method);
            }

            loadFromFile.AddToList(preset);
            Presets.Add(preset);
            string JSON = JsonConvert.SerializeObject(loadFromFile, Formatting.Indented);
            using (var write = new StreamWriter(path))
            {
                write.WriteLine(JSON.ToString());
                write.Close();
            }
        }

        // load the selected preset from list to the listBox
        private void LoadPreset_Clicked(object sender, RoutedEventArgs e)
        {
            addedList.Clear();
            var index = PresetCombobox.SelectedIndex;

            for (int i = 0; i < Presets[index].List.Count; i++)
            {
                if (Presets[index].List[i].OperationName == "Replace")
                {
                    var args = Presets[index].List[i].Args;
                    var temp = new ReplaceOperation() { Args = new ReplaceArgs { From = args[0], To = args[1] } };
                    addedList.Add(temp);
                }
                else if (Presets[index].List[i].OperationName == "New Case")
                {
                    var args = Presets[index].List[i].IntArgs;
                    var temp = new NewCaseOperation() { Args = new NewCaseArgs { Mode = args } };
                    addedList.Add(temp);
                }
                else if (Presets[index].List[i].OperationName == "Move")
                {
                    var args = Presets[index].List[i].IntArgs;
                    var temp = new MoveOperation() { Args = new MoveArgs { Mode = args } };
                    addedList.Add(temp);
                }
                else if (Presets[index].List[i].OperationName == "Fullname Normalize")
                {
                    var temp = new NormalizeOperation();
                    addedList.Add(temp);
                }
                else
                {
                    var temp = new UniqueNameOperation();
                    addedList.Add(temp);
                }
            }
        }

        // Delete the selected method from listBox
        private void Delete_Button_Clicked(object sender, RoutedEventArgs e)
        {
            var index = listMethod.Items.IndexOf(listMethod.SelectedItem);
            if (index >= 0 && index < addedList.Count)
            {
                addedList.RemoveAt(index);
            }
        }

        // Move method to the bottom
        private void Bottom_Clicked(object sender, RoutedEventArgs e)
        {
            var index = listMethod.Items.IndexOf(listMethod.SelectedItem);

            if (index >= 0 && index < addedList.Count && addedList.Count != 1)
            {
                var itemToMove = addedList[index];
                addedList.RemoveAt(index);
                addedList.Add(itemToMove);
            }
        }

        // Move method to the top
        private void Top_Clicked(object sender, RoutedEventArgs e)
        {
            var index = listMethod.Items.IndexOf(listMethod.SelectedItem);

            if (index >= 0 && index < addedList.Count && addedList.Count != 1)
            {
                var itemToMove = addedList[index];
                addedList.RemoveAt(index);
                addedList.Insert(0, itemToMove);
            }
        }

        // Move up method
        private void Up_Clicked(object sender, RoutedEventArgs e)
        {
            var index = listMethod.Items.IndexOf(listMethod.SelectedItem);

            if (index > 0 && index < addedList.Count && addedList.Count != 1)
            {
                var itemToMove = addedList[index];
                addedList.RemoveAt(index);
                addedList.Insert(index - 1, itemToMove);
            }
        }

        // Move down method
        private void Down_Clicked(object sender, RoutedEventArgs e)
        {
            var index = listMethod.Items.IndexOf(listMethod.SelectedItem);

            if (index >= 0 && index < addedList.Count - 1 && addedList.Count != 1)
            {
                var itemToMove = addedList[index];
                addedList.RemoveAt(index);
                addedList.Insert(index + 1, itemToMove);
            }
        }
    }
}
