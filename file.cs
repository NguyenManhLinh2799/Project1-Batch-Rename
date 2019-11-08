using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app
{
    public class file : INotifyPropertyChanged
    {
        private string name;
        private string newname;
        private string path;
        private string error;
        public event PropertyChangedEventHandler PropertyChanged;
        private void Notify(string name)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(name));
        }
        public string Name
        {
            get => name; set
            {
                name = value;
                Notify("Name");
            }
        }
        public string newName
        {
            get => newname; set
            {
                newname = value;
                Notify("newName");
            }
        }
        public string Path
        {
            get => path; set
            {
                path = value;
                Notify("Path");
            }
        }
        public string Error
        {
            get => error; set
            {
                error = value;
                Notify("Error");
            }
        }

        public file(string name, string newname, string path, string error)
        {
            this.Name = name;
            this.newName = newname;
            this.Path = path;
            this.Error = error;
            Notify("Name");
            Notify("newName");
            Notify("Path");
            Notify("Error");
        }
    }
}