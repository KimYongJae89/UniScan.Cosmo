using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StringManager
{
    public partial class MainForm : Form
    {
        public class DictionaryItem
        {
            public string Locale;
            public string Value;
            public DictionaryItem(string locale, string value) { Locale = locale; Value = value; }
        }

        TreeNode rootNode = null;
        public MainForm()
        {
            InitializeComponent();

            rootNode = new TreeNode("Root");
            rootNode.Name = rootNode.Text;

            columnLocale.DataPropertyName = "Key";
            columnValue.DataPropertyName = "Value";
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Xml Files (*.xml)|*.xml";
            dlg.Multiselect = true;
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            foreach (string fileName in dlg.FileNames)
            {
                string name = Path.GetFileNameWithoutExtension(fileName);
                string locale = name.Split('_').Last().Substring(0, 5);

                bool ok = DynMvp.Base.StringManager.Load(fileName);
                if (ok)
                {
                    List<StringTable> stringTableList = DynMvp.Base.StringManager.StringTableList;
                    foreach (StringTable table in stringTableList)
                    {
                        string tableName = table.Name;
                        TreeNode treeNode = rootNode.Nodes.Find(tableName, false).FirstOrDefault();
                        if (treeNode == null)
                        {
                            treeNode = new TreeNode(tableName);
                            treeNode.Name = treeNode.Text;
                            rootNode.Nodes.Add(treeNode);
                        }

                        Dictionary<string, string>.Enumerator dic = table.GetEnumerator();
                        while (dic.MoveNext())
                        {
                            TreeNode subNode = treeNode.Nodes.Find(dic.Current.Key, false).FirstOrDefault();
                            if (subNode == null)
                            {
                                subNode = new TreeNode(dic.Current.Key);
                                subNode.Name = subNode.Text;
                                treeNode.Nodes.Add(subNode);
                            }

                            if (subNode.Tag == null)
                                subNode.Tag = new BindingList<DictionaryItem>();

                            BindingList<DictionaryItem> bindingList = subNode.Tag as BindingList<DictionaryItem>;
                            DictionaryItem item = bindingList.FirstOrDefault(f => f.Locale == locale);
                            if (item == null)
                                bindingList.Add(new DictionaryItem(locale, dic.Current.Value));
                            else
                                item.Value = dic.Current.Value;
                        };
                    }
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Load Fail - ");
                    sb.AppendLine(string.Format("File: {0}", fileName));
                    MessageBox.Show(sb.ToString());
                }
                DynMvp.Base.StringManager.Clear();
            }

            treeView1.Nodes.Clear();
            TreeNode searchNode = Search(this.rootNode, searchBar.Text);
            treeView1.Nodes.Add(searchNode);
            searchNode.Expand();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //if (e.Node.Tag == null)
            //    return;

            BindingList<DictionaryItem> item = e.Node.Tag as BindingList<DictionaryItem>;
            dataGridView1.DataSource = item;
        }

        private void dataGridView1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            BindingList<DictionaryItem> bindingList = dataGridView1.DataSource as BindingList<DictionaryItem>;
            if (bindingList.Count <= e.RowIndex)
                return;

            DictionaryItem item = bindingList[e.RowIndex];

            switch(e.ColumnIndex)
            {
                case 0:
                e.Value = item.Locale;
                    break;
                case 1:
                e.Value = item.Value;
                    break;
                default:
                    break;
            }
        }

        private void dataGridView1_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            BindingList<DictionaryItem> bindingList = dataGridView1.DataSource as BindingList<DictionaryItem>;
            if (bindingList.Count <= e.RowIndex)
                return;

            DictionaryItem item = bindingList[e.RowIndex];

            switch (e.ColumnIndex)
            {
                case 0:
                    item.Locale= e.Value.ToString();
                    break;
                case 1:
                    item.Value = e.Value.ToString();
                    break;
                default:
                    break;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Xml Files (*.xml)|*.xml";
            //if (dlg.ShowDialog() != DialogResult.OK)
            //    return;

            //string filePath = dlg.FileName;
            //string fileName = Path.GetFileNameWithoutExtension(filePath);
            //string locale = fileName.Split('_').Last().Substring(0, 5);

            string savePath = @"d:\temp\Language";
            Directory.CreateDirectory(savePath);

            Dictionary<string, List<StringTable>> stringTableListDic = new Dictionary<string, List<StringTable>>();
            TreeNodeCollection rootNodes = rootNode.Nodes;
            foreach (TreeNode treeNode in rootNodes)
            {
                string tableName = treeNode.Name;

                TreeNodeCollection nodes = treeNode.Nodes;
                foreach (TreeNode node in nodes)
                {
                    BindingList<DictionaryItem> list = node.Tag as BindingList<DictionaryItem>;
                    if (list != null)
                    {
                        foreach (DictionaryItem item in list)
                        {
                            if (stringTableListDic.ContainsKey(item.Locale) == false)
                                stringTableListDic.Add(item.Locale, new List<StringTable>());

                            List<StringTable> stringTableList = stringTableListDic[item.Locale];
                            if (stringTableList.Exists(f => f.Name == tableName) == false)
                                stringTableList.Add(new StringTable(tableName));

                            StringTable stringTable = stringTableList.Find(f => f.Name == tableName);
                            if (stringTable.IsExist(node.Name))
                                stringTable.SetString(node.Name, item.Value);
                            else
                                stringTable.AddString(node.Name, item.Value);
                        }
                    };
                }
            }

            for (int i = 0; i < stringTableListDic.Count; i++)
            {
                DynMvp.Base.StringManager.Load(savePath, stringTableListDic.ElementAt(i).Key);
                DynMvp.Base.StringManager.StringTableList = stringTableListDic.ElementAt(i).Value;
                bool good = DynMvp.Base.StringManager.Save();
                if (good == false)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Save Fail - ");
                    sb.AppendLine(string.Format("Path: {0}", DynMvp.Base.StringManager.ConfigPath));
                    sb.AppendLine(string.Format("Locale: {0}", DynMvp.Base.StringManager.LocaleCode));
                    MessageBox.Show(sb.ToString());
                }
                DynMvp.Base.StringManager.Clear();
            }
        }

        private void searchBar_TextChanged(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            TreeNode treeNode = Search(rootNode, searchBar.Text);
            if (treeNode != null)
            {
                treeView1.Nodes.Add(treeNode);
                treeNode.Expand();
            }
        }

        private TreeNode Search(TreeNode parentTreeNode, string searchText)
        {
            TreeNode treeNode = new TreeNode(parentTreeNode.Name);
            treeNode.Name = treeNode.Text;
            treeNode.Tag = parentTreeNode.Tag;

            //if (parentTreeNode.Nodes.Count > 0)
            //{
                foreach (TreeNode subNode in parentTreeNode.Nodes)
                {
                    TreeNode subTreeNode = Search(subNode, searchText);
                    if (subTreeNode != null)
                    {
                        treeNode.Nodes.Add(subTreeNode);
                        subTreeNode.Tag = subNode.Tag;
                    }
                }
                if (treeNode.Nodes.Count > 0)
                    return treeNode;
            //}
            //else
            //{
                if (treeNode.Name.Contains(searchText))
                    return treeNode;

                BindingList<DictionaryItem> bindingList = treeNode.Tag as BindingList<DictionaryItem>;
                if(bindingList!=null)
                {
                    BindingList<DictionaryItem> newBindingList = new BindingList<DictionaryItem>(bindingList.ToList().FindAll(f => f.Value.Contains(searchText)));
                    if (newBindingList.Count > 0)
                    {
                        treeNode.Tag = newBindingList;
                        return treeNode;
                    }
                }
            //}


            return null;
        }
    }
}
