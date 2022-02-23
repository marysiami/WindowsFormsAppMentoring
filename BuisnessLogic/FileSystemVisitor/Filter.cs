using System;
using System.IO;

namespace BusinessLogic.FileSystemVisitor
{
    public class Filter
    {
        public string DirSearchPattern { get; set; } = string.Empty;
        public SearchOption DirSearchOption { get; set; }
        public string FileSearchPattern { get; set; } = string.Empty;

    }
    public struct TreeNode
    {
        public int Directory { get; set; }
        public string FileName { get; set; }
        public DateTime DateTime { get; set; }
    }
}
