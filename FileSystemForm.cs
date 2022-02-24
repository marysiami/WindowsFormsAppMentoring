﻿using BusinessLogic.FileSystemVisitor;
using System;
using System.Windows.Forms;

namespace WindowsFormsAppMentoring
{
    public partial class FileSystemForm : Form
    {
        private IFileSystemService ReaderService { get; set; }

        public FileSystemForm()
        {
            InitializeComponent();
            ReaderService = (IFileSystemService)Program.ServiceProvider.GetService(typeof(IFileSystemService));
            RegisterEvents();
            resultTree.Nodes.Clear();
        }
        private void RegisterEvents()
        {
            ReaderService.StartedEvent += Visitor_StartedEvent;
            ReaderService.FinishedEvent += Visitor_FinishedEvent;
            ReaderService.FileFoundEvent += ReaderService_FileFoundEvent;
            ReaderService.DirectoryFoundEvent += ReaderService_DirectoryFoundEvent;
            ReaderService.FilteredFileFoundEvent += ReaderService_FilteredFileFoundEvent;
            ReaderService.FilteredDirectoryFoundEvent += ReaderService_FilteredDirectoryFoundEvent;
        }        

        #region events
        private void ReaderService_FilteredDirectoryFoundEvent(object sender, string e)
        {
            resultTree.Nodes.Add(e);
        }

        private void ReaderService_FilteredFileFoundEvent(object sender, BusinessLogic.FileSystemVisitor.TreeNode e)
        {
            resultTree.Nodes[e.DirectoryId].Nodes.Add(e.FileName);
        }

        private void ReaderService_DirectoryFoundEvent(object sender, string e)
        { 
           resultTree.Nodes.Add(e); 
        }

        private void ReaderService_FileFoundEvent(object sender, BusinessLogic.FileSystemVisitor.TreeNode e)
        {
            resultTree.Nodes[e.DirectoryId].Nodes.Add(e.FileName);
        }

        private void Visitor_FinishedEvent(object sender, string e)
        {
            stopLabel.Text =$"Process finished for path: {e}";
            stopLabel.Visible = true;
            resultTree.EndUpdate();
        }

        private void Visitor_StartedEvent(object sender, string e)
        {
            startLabel.Text = $"Process started for path: {e}";
            startLabel.Visible = true;
            resultTree.BeginUpdate();
        }
        #endregion

        private void TreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var node = e.Node;
            if (node != null && e.Clicks > 1)
            {
                resultTree.Nodes.Remove(node);
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (addFiltersLabel.Checked)
            {
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
                label6.Visible = true;
                textBox2.Visible = true;
                textBox3.Visible = true;
                checkBox2.Visible = true;
            }
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            resultTree.Nodes.Clear();
            noPathLabel.Visible = false;

            var path = textBox1.Text;
            if (string.IsNullOrEmpty(path))
            {
                noPathLabel.Visible = true;
                return;
            }
            
            if (addFiltersLabel.Checked)
            {
                Filter filter = new Filter
                {
                    DirSearchPattern = textBox2.Text,
                    FileSearchPattern = textBox3.Text,
                    DirSearchOption = checkBox2.Checked ? System.IO.SearchOption.TopDirectoryOnly : System.IO.SearchOption.AllDirectories
                };

               var visitor = new FileSystemVisitor(ReaderService.GetFilteredFilesTree, path, filter);
            }
            else
            {
               var visitor = new FileSystemVisitor(ReaderService.GetFilesTree, path, null);
            }
            
        }
    }
}
