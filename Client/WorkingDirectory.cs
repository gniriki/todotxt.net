using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Client
{
    public class WorkingDirectory : INotifyPropertyChanged
    {
        private readonly string _workingDirectory;

        public WorkingDirectory(string directoryPath)
        {
            if(!Directory.Exists(directoryPath))
                return;

            _workingDirectory = directoryPath;

            LoadFiles();
        }

        public void LoadFiles()
        {
            Files = Directory
                .GetFiles(_workingDirectory, "*.txt", SearchOption.AllDirectories)
                .Select(x => new TodoFile(x, _workingDirectory))
                .ToList();
            OnPropertyChanged(nameof(Files));
        }

        public List<TodoFile> Files { get; private set; } = new List<TodoFile>();

        public bool ContainsFile(string fileName)
        {
            return Directory.GetFiles(_workingDirectory, "*.txt", SearchOption.AllDirectories)
                .Contains(fileName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class TodoFile
    {
        public TodoFile(string filePath, string workingDirectory)
        {
            FilePath = filePath;
            Name = Path.GetFileNameWithoutExtension(filePath);

            SetDirectoryGroup(workingDirectory);
        }

        private void SetDirectoryGroup(string workingDirectory)
        {
            if (string.IsNullOrEmpty(FilePath))
                return;

            var directoryName = Path.GetDirectoryName(FilePath);
            if(string.IsNullOrEmpty(directoryName))
                return;
            
            DirectoryGroup = directoryName
                .Replace(workingDirectory, "");

            if (DirectoryGroup.StartsWith("\\"))
                DirectoryGroup = DirectoryGroup.Substring(1);
        }

        public string Name { get; set; }
        public string DirectoryGroup { get; set; }
        public string FilePath { get; set; }
    }
}