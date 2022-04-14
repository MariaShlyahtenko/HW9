using Avalonia.Threading;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HW9.Models
{
    public class FileTree : ReactiveObject
    {
        private string ppat;
        private string nm;
        private FileSystemWatcher? wth;
        private ObservableCollection<FileTree>? kid;
        private bool exp;
        public bool IsChecked { get; set; }
        public FileTree(string path,
            bool isRoot = false)
        {
            ppat = path;
            nm = isRoot ? path : System.IO.Path.GetFileName(Path);
            exp = isRoot;
        }
        public string Path
        {
            get => ppat;
            private set => this.RaiseAndSetIfChanged(ref ppat, value);
        }
        public string Name
        {
            get => nm;
            private set => this.RaiseAndSetIfChanged(ref nm, value);
        }
        public bool IsExpanded
        {
            get => exp;
            set => this.RaiseAndSetIfChanged(ref exp, value);
        }
        public IReadOnlyList<FileTree> Children => kid ??= LoadChildren();

        private ObservableCollection<FileTree> LoadChildren()
        {
            var options = new EnumerationOptions { IgnoreInaccessible = true };
            var result = new ObservableCollection<FileTree>();

            foreach (var d in Directory.EnumerateDirectories(Path, "*", options))
            {
                result.Add(new FileTree(d, true));
            }

            foreach (var f in Directory.EnumerateFiles(Path, "*.jpg", options))
            {
                result.Add(new FileTree(f, false));
            }

            wth = new FileSystemWatcher
            {
                Path = Path,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size | NotifyFilters.LastWrite,
            };
           
            wth.EnableRaisingEvents = true;

            return result;
        }

    }
}