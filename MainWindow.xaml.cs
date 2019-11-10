using System.Windows;
using System.IO;
using System.ComponentModel;
using Microsoft.WindowsAPICodePack.Dialogs;
using app;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.Linq;

namespace đồ_án_1___interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BindingList<StringOperation> methodList = new BindingList<StringOperation>();
        BindingList<StringOperation> addedList = new BindingList<StringOperation>();
        BindingList<file> ListFile = new BindingList<file>();
        BindingList<file> ListFolder = new BindingList<file>();
        FileInfo[] File = null;
        DirectoryInfo[] Folder = null;
        //List<string> nameFile=new List<string>();
        //List<string> nameFolder = new List<string>();
        //kha 10/11/2019
        int kt=0;

        // path to Preset file
        string path = AppDomain.CurrentDomain.BaseDirectory + @"\preset.JSON";
        PresetList loadFromFile = new PresetList() { List = new List<Preset>() };
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
                    //nameFolder.Add(files.Name);
                    ListFolder.Add(a);
                }
                DSlistFolder.ItemsSource = ListFolder;
            }
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
                File = Filename.GetFiles();

                foreach (var files in File)
                {
                    file a = new file(files.Name, "", files.Directory.ToString(), "");
                    //nameFile.Add(files.Name.ToString());
                    ListFile.Add(a);
                }
                DSlistFile.ItemsSource = ListFile;
            }
        }


        private void Clear_Clicked(object sender, RoutedEventArgs e)
        {
            //xóa list hành động
            addedList.Clear();

            //gán kiểm tra lỗi =0
            kt = 0;

            //xóa list file
            ListFile.Clear();

            //xóa list folder
            ListFolder.Clear();

            //xóa list name
            //nameFile.Clear();
            //nameFolder.Clear();

            //xóa list action
            addedList.Clear();
        }

        private void ConfigMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var item = listMethod.SelectedItem as
                StringOperation;

            item.Config();
        }

        private void LoadPresetFromFile()
        {

            using (var read = new StreamReader(path))
            {
                string json = read.ReadToEnd();
                loadFromFile = JsonConvert.DeserializeObject<PresetList>(json);
            }
            if(loadFromFile!=null)
            {
                for (int i = 0; i < loadFromFile.Count; i++)
                {
                    Presets.Add(loadFromFile.List[i]);
                }

                PresetCombobox.ItemsSource = Presets;
                PresetCombobox.SelectedIndex = 0;
            }
        }

        // save current list of added method to preset file
        private void SavePreset_Clicked(object sender, RoutedEventArgs e)
        {
            var preset = new Preset();
            preset.PresetName = PresetNameTextBox.Text;
            preset.List = new List<JSONOperation>();

            foreach (var method in addedList)
            {
                preset.AddToList(method);
            }

            if (loadFromFile == null)
            {
                PresetList loadFromFile = new PresetList() { List = new List<Preset>() };
            }

            Presets.Add(preset);
            loadFromFile.AddToList(preset);
            
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


        //<kha mới thêm 10/11/2019>
        private void PreviewFile_Click(object sender, RoutedEventArgs e)
        {
            //List<string> NewName = new List<string>();
            //NewName = nameFile;
            for (int i = 0; i < ListFile.Count(); i++)
            {
                string name1 = ListFile[i].Name;
                for(int j=0;j<addedList.Count();j++)
                {
                   
                    ListFile[i].newName = addedList[j].Operate(name1);
                    if(!findPoint(ListFile[i].newName))
                        {
                        ListFile[i].newName += File[i].Extension.ToString();
                        }
                }
            }

            CheckLoopName(ListFile);
            CheckError(ListFile);
        }

        private void ApplyFile_Click(object sender, RoutedEventArgs e)
        {
            if (kt == 0)
            {
                MessageBox.Show("Apply");
                for (int i = 0; i < ListFile.Count(); i++)
                {
                    File[i].MoveTo($"{ListFile[i].Path}\\{ListFile[i].newName}");
                }
            }
            else 
            {
                if (kt == 1)
                    MessageBox.Show("Error: Invalid name");
                if (kt == 2) 
                    MessageBox.Show("Error: Name Existed");
            }
        }

        bool findPoint(string a)
        {
            for(int i=0;i<a.Length;i++)
            {
                if(a[i]=='.')
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// hàm kiểm tra lỗi đặt tên
        /// </summary>
        /// <param name="ListFile"></param>
        void CheckError(BindingList<file> ListFile)
        {
            for (int i = 0; i < ListFile.Count(); i++)
            {
                //string dau = "";
                string temp = ListFile[i].newName;
                for (int j = 0; j < temp.Length; j++)
                {
                    if (temp[j] == '/' || temp[j] == 92 || temp[j] == '>' || temp[j] == '<' || temp[j] == '?' || temp[j] == '*' || temp[j] == '|' || temp[j] == '"')
                    {
                        ListFile[i].Error = "Invalid name";
                        kt = 1;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// hàm kiểm tra lỗi trùng tên file và folder
        /// </summary>
        /// <param name="ListFile"></param>
        void CheckLoopName(BindingList<file> ListFile)
        {
            for (int i = 0; i < ListFile.Count() - 1; i++)
            {
                for (int j = i + 1; j < ListFile.Count(); j++)
                {
                    if (ListFile[i].newName == ListFile[j].newName && ListFile[i].Path == ListFile[j].Path)
                    {
                        ListFile[i].Error = "Existed name";
                        ListFile[j].Error = "Existed name";
                        kt = 2;
                    }
                }
            }
        }

        //chưa fix
        private void PreviewFolder_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < ListFolder.Count(); i++)
            {
                string name1 = ListFolder[i].Name;
                for (int j = 0; j < addedList.Count(); j++)
                {
                    //chỉnh sửa sau
                    //ListFile[i].newName = $"{Guid.NewGuid()}{File[i].Extension}";
                    ListFolder[i].newName = addedList[j].Operate(name1);
                    //add list action
                }
            }

            CheckLoopName(ListFolder);
            CheckError(ListFolder);
        }

        private void ApplyFolder_Click(object sender, RoutedEventArgs e)
        {
            if (kt == 0)
            {
                MessageBox.Show("Apply");
                for (int i = 0; i < File.Count(); i++)
                {
                    File[i].MoveTo($"{ListFile[i].Path}\\{ListFile[i].newName}");
                }
            }
            else
            {
                if (kt == 1)
                    MessageBox.Show("Error: Invalid name");
                if (kt == 2)
                    MessageBox.Show("Error: Name Existed");
            }
        }
        //</kha mới thêm 10/11/2019>
    }
}


