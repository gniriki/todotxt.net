using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Client
{
    public class FileList
    {
        public FileList(string directoryPath)
        {
            if(!Directory.Exists(directoryPath))
                return;

            Files = Directory.GetFiles(directoryPath,
                    "*.txt",
                    SearchOption.AllDirectories).Select(x => new TodoFile(x, directoryPath))
                .ToList();
        }

        public List<TodoFile> Files { get; set; } = new List<TodoFile>();
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