﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace KEIL2VS
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private void Keil2VS_Load(object sender, EventArgs e)
        {
            //this.elementHost.Child = this.TargetListBOX;
            //this.TargetListBOX.SelectionChanged += new EventHandler(this.TargetListBOX_SelectionChanged);
            this.GroupListBox.SelectedIndexChanged += this.GroupListBox_SelectedIndexChanged;
            this.FileBox.DoubleClick += this.FileBox_DoubleClick;
            this.tbKeil_path.DoubleClick += this.Tbkeil_path_DoubleClick;
            this.tbKeil_path.DragDrop += this.Tbkeil_pathBOX_DragDrop;
            this.tbKeil_path.DragEnter += this.KEIL2VSDragEnter;
            this.MinimumSize = new Size(this.Width, this.Height);

            base.AutoScaleMode = AutoScaleMode.None;
            this.ProjectIno.NMakeCleanCommandLine = "";
            this.CreateButton.Enabled = false;
            string[] array = new string[2];
            array[0] = this.PreStr.ApplicationStartpath;
            array[1] = this.PreStr.configfilename;
            this.Config.DocName = Path.Combine(array);
            this.tbKeil_path.Text = this.PreStr.noKeil;
            this.tb_predef.Text = this.PreStr.predefine;
            this.Config.PreDefine = this.tb_predef.Text;
        }


        private void Keil2VS_Shown(object sender, EventArgs e)
        {
            if (!TryGetSoftwarePath(ref this.Config.UV4Path))
            {
                this.ReadConfig(this.Config.DocName);
                this.ProjectIno.UV4_Path = this.Config.UV4Path + " ";
            }
            else
            {
                try
                {
                    File.Delete(this.Config.DocName);
                }
                catch { }
                this.Config.UV4Path = this.Config.UV4Path.Replace("C51", @"UV4\UV4.exe");
                this.Config.UV4IncPath = this.Config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\INC");
                this.Config.UV4LibPath = this.Config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\LIB");
                this.tbKeil_path.BackColor = Color.LawnGreen;
                this.tbKeil_path.Text = this.Config.UV4Path;
                this.tbKeil_path.AllowDrop = false;
            }
            UpDateCurfolderUproj(sender, e);

        }


        private void UpDateCurfolderUproj(object sender, EventArgs e)
        {
            if (!ScanCurrentFolderHas_uProj(this.PreStr.ApplicationStartpath, ref this.fileInfos))
            {
                return;
            }
            else
            {
                this.uprojInfo = new _uprojInfo[this.fileInfos.GetLength(0)];
                for (int i = 0; i < this.fileInfos.GetLength(0); i++)
                {
                    this.uprojInfo[i].fileName = fileInfos[i].Name;
                    this.uprojInfo[i].fileFullname = fileInfos[i].FullName;
                }
                SourcePathComobox_add(this.uprojInfo);
                this.sourcepathTip.Show(this.PreStr.FindUprojInThisFolder, SourcePathCBOX, this.SourcePathCBOX.Location.X + 200, this.SourcePathCBOX.Top - 100, 1000 * 10);
            }
            this.TryDispuProjinfo(this.uprojInfo[0].fileFullname);
        }


        private bool ScanCurrentFolderHas_uProj(string exeDir, ref FileInfo[] fileInfo)
        {
            int cnt_1 = 0, cnt_2 = 0, cnt_2_1 = 0, ficount = 0;
            bool isfind = false;
            DirectoryInfo TheFolder = new DirectoryInfo(exeDir);
            cnt_1 = TheFolder.GetFiles("*.uvproj").Length;
            cnt_2 = TheFolder.GetDirectories().Length;
            FileInfo[] fileInfo1 = null, fileInfo2 = null;
            if (0 != cnt_1)
            {
                fileInfo1 = new FileInfo[cnt_1];
                cnt_1 = 0;
                foreach (FileInfo NextFile in TheFolder.GetFiles("*.uvproj"))
                {
                    fileInfo1[cnt_1++] = NextFile;
                }
                isfind = true;
            }
            if (cnt_2 != 0)
            {
                foreach (DirectoryInfo drinfo in TheFolder.GetDirectories())
                {
                    cnt_2_1 += drinfo.GetFiles("*.uvproj").Length;
                }
            }
            if (cnt_2_1 != 0)
            {
                fileInfo2 = new FileInfo[cnt_2_1];
                cnt_2_1 = 0;
                foreach (DirectoryInfo drinfo in TheFolder.GetDirectories())
                {
                    foreach (FileInfo NextFile in drinfo.GetFiles("*.uvproj"))
                    {
                        fileInfo2[cnt_2_1++] = NextFile;
                    }
                }
                isfind = true;
            }
            if (isfind)
            {
                fileInfo = new FileInfo[cnt_1 + cnt_2_1];
                if (cnt_1 != 0)
                {
                    foreach (FileInfo f1 in fileInfo1)
                    {
                        fileInfo[ficount++] = f1;
                    }
                }

                if (cnt_2_1 != 0)
                {
                    foreach (FileInfo f2 in fileInfo2)
                    {
                        fileInfo[ficount++] = f2;
                    }
                }
            }
            return isfind;
        }


        private void SourcePathComobox_add(_uprojInfo[] it)
        {
            this.SourcePathCBOX.Items.Clear();
            for (int i = 0; i < it.GetLength(0); i++)
            {
                this.SourcePathCBOX.Items.Add(it[i].fileFullname);
            }
            //this.SourcePathCBOX.Items.Add(this.PreStr.selectnewfolder);
            this.SourcePathCBOX.SelectedIndex = 0;
        }

        //private void elementHost_Add(string[] Items)
        //{
        //    this.elementHost.Items.Clear();
        //    foreach (string newItem in Items)
        //    {
        //        this.elementHost.Items.Add(newItem);
        //    }
        //}

        private void ElementHost_Add(string[] Items)
        {
            this.elementHost.Items.Clear();
            foreach (string newItem in Items)
            {
                this.elementHost.Items.Add(newItem);
            }
            this.elementHost.SelectedIndex = 0;
        }



        private void GroupListBox_Add(string[] Items)
        {
            this.GroupListBox.Items.Clear();
            foreach (string item in Items)
            {
                this.GroupListBox.Items.Add(item);
            }
        }

        private void SrcFileBox_Add(string[] Items)
        {
            this.FileBox.Items.Clear();
            foreach (string item in Items)
            {
                this.FileBox.Items.Add(item);
            }
        }

        private void TargetStatusBox_Add(string Str)
        {
            this.TargetStatus.Text = Str;
        }
        private void GroupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ProjectIno.CuruProjectFileDir != "")
            {
                string[] array = this.MDK_TargetRead(this.ProjectIno.CuruProjectFileDir);
                string[] array2 = this.MDK_GroupRead(this.ProjectIno.CuruProjectFileDir, array[this.elementHost.SelectedIndex]);
                string[] items = this.MDK_SrcRead(this.ProjectIno.CuruProjectFileDir, array[this.elementHost.SelectedIndex], array2[this.GroupListBox.SelectedIndex]);
                this.SrcFileBox_Add(items);
            }
        }


        private void FileBox_DoubleClick(object sander, EventArgs e)
        {
            if (this.FileBox.SelectedIndex < 0)
            {
                return;
            }
            string text = this.FileBox.SelectedItem.ToString();
            text = this.GetFullPath(this.ProjectIno.MDK_Project_Path, text);
            try
            {
                new Process
                {
                    StartInfo =
                {
                    FileName = "notepad++.exe",
                    Arguments = text
                }
                }.Start();
            }
            catch
            {

                new Process
                {
                    StartInfo =
                {
                    FileName = "notepad.exe",
                    Arguments = text
                }
                }.Start();
            }

        }
        private void BtnOpenFolder_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                DefaultExt = "uvproj",
                Filter = "MDK Project File (*.uvproj)|*.uvproj|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string fileName = openFileDialog.FileName;
            if (fileName == "")
            {
                return;
            }
            if (!MDK_CheckProject(fileName))
            {
                return;
            }
            TryDispuProjinfo(fileName);
            this.uprojInfo = new _uprojInfo[1];
            this.uprojInfo[0].fileName = this.ProjectIno.ProjectName;
            this.uprojInfo[0].fileFullname = this.ProjectIno.MDK_Project_File;

            this.SourcePathCBOX.Text = this.uprojInfo[0].fileFullname;
            SourcePathComobox_add(this.uprojInfo);

        }

        private void SourcePathCBOX_DragDrop(object sender, DragEventArgs e)
        {
            this.SourcePathCBOX.ForeColor = Color.Black;
            string Orig = this.SourcePathCBOX.Text;
            string dropFilename = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            if (!MDK_CheckProject(dropFilename))
            {
                return;
            }
            else
            {
                TryDispuProjinfo(dropFilename);
            }
            this.uprojInfo = new _uprojInfo[1];
            this.uprojInfo[0].fileName = this.ProjectIno.ProjectName;
            this.uprojInfo[0].fileFullname = this.ProjectIno.MDK_Project_File;
            this.SourcePathCBOX.Text = this.uprojInfo[0].fileFullname;
            SourcePathComobox_add(this.uprojInfo);
        }
        private void TryDispuProjinfo(string fileFullname)
        {
            if (this.MDK_Display_Info(fileFullname))
            {
                this.ProjectIno.CuruProjectFileDir = fileFullname;
                this.ProjectIno.MDK_Project_File = fileFullname;
                this.ProjectIno.MDK_Project_Path = Path.GetDirectoryName(fileFullname) + "\\";
                this.ProjectIno.ProjectName = Path.GetFileNameWithoutExtension(fileFullname);
                this.ProjectIno.VcxprojName = this.ProjectIno.ProjectName + ".vcxproj";
                this.ProjectIno.VC_Filters_Name = this.ProjectIno.VcxprojName + ".filters";
                this.ProjectIno.VC_UserFileName = this.ProjectIno.VcxprojName + ".user";
            }
        }

        private void KEIL2VSDragEnter(object sander, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
                return;
            }
            e.Effect = DragDropEffects.None;
        }
        private void Tbkeil_path_DoubleClick(object sender, EventArgs e)
        {
            string tempPath = tbKeil_path.Text;
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                DefaultExt = "exe",
                Filter = "Keil Exe File (UV4.exe)|UV4.exe",
                Title = "Select Keil Vision4 Exe Path"
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                tbKeil_path.Text = tempPath;
                return;
            }
            string fileName = openFileDialog.FileName;
            if (fileName != "")
            {

                this.Config.UV4Path = openFileDialog.FileName;
                this.Config.UV4IncPath = this.Config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\INC");
                this.Config.UV4LibPath = this.Config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\LIB");
                this.Creat_Config(this.Config.DocName);
                tbKeil_path.Text = this.Config.UV4Path;
                tbKeil_path.BackColor = Color.LightGreen;
            }
            else
            {
                this.tbKeil_path.Text = tempPath;
                this.tbKeil_path.ForeColor = Color.Gray;
            }
        }

        private bool MDK_check_keil_UV4(string text)
        {
            if (text.EndsWith("UV4.exe", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else return false;
        }
        private void Tbkeil_pathBOX_DragDrop(object sender, DragEventArgs e)
        {
            string path = tbKeil_path.Text;
            this.tbKeil_path.ForeColor = Color.Black;
            this.tbKeil_path.Text = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            string text = this.tbKeil_path.Text;

            if (this.MDK_check_keil_UV4(text))
            {
                this.Config.UV4Path = text;
                this.Config.UV4IncPath = this.Config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\INC");
                this.Config.UV4LibPath = this.Config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\LIB");
                this.Creat_Config(this.Config.DocName);
                return;
            }
            MessageBox.Show(this.PreStr.notkeilexe + "\nthis is" + this.tbKeil_path.Text);
            this.tbKeil_path.Text = path;
            this.tbKeil_path.ForeColor = Color.Gray;
        }
        // Token: 0x0600000D RID: 13 RVA: 0x00002740 File Offset: 0x00000940
        private void Creat_Config(string DocName)
        {
            if (DocName == "")
            {
                return;
            }
            this.Config.ToolName = "Keil2VisualStudio";
            this.Config.ToolsVersion = this.PreStr.ToolsVersion;
            XNamespace ns = this.PreStr.nameSp;
            XElement xelement = new XElement(ns + "Tool", new object[]
            {
                new XAttribute("Name", this.Config.ToolName),
                new XAttribute("Author", this.PreStr.author),
                new XAttribute("ToolsVersion", this.Config.ToolsVersion),
                new XElement(ns + "UV4Path", this.Config.UV4Path),
                new XElement(ns + "UV4IncPath", this.Config.UV4IncPath),
                new XElement(ns + "UV4LibPath", this.Config.UV4LibPath),

        });
            xelement.Save(DocName);
        }

        // Token: 0x0600000E RID: 14 RVA: 0x00002818 File Offset: 0x00000A18
        private void ReadConfig(string DocName)
        {
            bool temp = File.Exists(DocName);
            Color c = this.tbKeil_path.BackColor;
            string t = this.tbKeil_path.Text;

            if (!temp)
            {
                this.tbKeil_path.BackColor = Color.LightPink;
                MessageBox.Show(this.PreStr.fristUse);
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    DefaultExt = "exe",
                    Filter = "Keil Exe File (UV4.exe)|UV4.exe",
                    Title = "Select Keil Vision4 Exe Path"
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.Config.UV4Path = openFileDialog.FileName;
                    this.Config.UV4IncPath = this.Config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\INC");
                    this.Config.UV4LibPath = this.Config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\LIB");
                    this.Creat_Config(this.Config.DocName);
                    this.tbKeil_path.BackColor = Color.LightGreen;
                    this.tbKeil_path.Text = this.Config.UV4Path;
                    return;
                }
                else
                {
                    this.tbKeil_path.Text = t;

                }
            }
            else
            {
                XNamespace ns = this.PreStr.nameSp;
                XDocument xdocument = XDocument.Load(DocName);

                this.Config.ToolsVersion = xdocument.Root.Attribute("ToolsVersion").Value;
                this.Config.UV4Path = xdocument.Root.Element(ns + "UV4Path").Value;
                if (this.Config.ToolsVersion.Equals("0.1"))
                {
                    try
                    {
                        File.Delete(this.Config.DocName);
                    }
                    catch
                    {
                    }

                    MessageBox.Show("配置版本更新,请重新设置keil 软件的安装路径");
                    OpenFileDialog openFileDialog = new OpenFileDialog
                    {
                        InitialDirectory = this.Config.UV4Path,
                        DefaultExt = "exe",
                        Filter = "Keil Exe File (UV4.exe)|UV4.exe",
                        Title = "Select Keil Vision4 Exe Path"
                    };
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        this.Config.UV4Path = openFileDialog.FileName;
                        this.Config.UV4IncPath = openFileDialog.FileName.Replace(@"UV4\UV4.exe", @"C51\INC");
                        this.Config.UV4LibPath = openFileDialog.FileName.Replace(@"UV4\UV4.exe", @"C51\LIB");
                        this.Creat_Config(this.Config.DocName);
                        this.tbKeil_path.BackColor = Color.LightGreen;
                        this.tbKeil_path.Text = this.Config.UV4Path;
                        return;
                    }
                }
                this.Config.UV4IncPath = xdocument.Root.Element(ns + "UV4IncPath").Value;
                this.Config.UV4LibPath = xdocument.Root.Element(ns + "UV4LibPath").Value;
                this.tbKeil_path.BackColor = Color.LightGreen;
                this.tbKeil_path.Text = this.Config.UV4Path;
            }
        }

        // Token: 0x0600000F RID: 15 RVA: 0x000028CC File Offset: 0x00000ACC
        public string GetRelativePath(string basePath, string targetPath)
        {
            Uri uri = new Uri(basePath);
            Uri uri2 = new Uri(targetPath);
            return uri.MakeRelativeUri(uri2).ToString().Replace("/", "\\");
        }

        // Token: 0x06000010 RID: 16 RVA: 0x00002904 File Offset: 0x00000B04
        private string GetFullPath(string basePath, string targetPat)
        {
            Uri baseUri = new Uri(basePath);
            Uri uri = new Uri(baseUri, targetPat);
            return uri.ToString().Replace("/", "\\").Replace("file:\\\\\\", "");
        }


        private string[] MDK_TargetRead(string Doc)
        {
            if (Doc == "")
            {
                return null;
            }
            this.xmlDoc.Load(Doc);
            XmlNodeList xmlNodeList = this.xmlDoc.SelectNodes(".//Targets/*");
            string[] array = new string[xmlNodeList.Count];
            int num = 0;
            foreach (object obj in xmlNodeList)
            {
                XmlNode xmlNode = (XmlNode)obj;
                array[num] = xmlNode.SelectSingleNode("./TargetName").InnerText;
                num++;
            }
            return array;
        }

        private string MDK_TargetStatusRead(string Doc, string TargetName)
        {
            if (Doc == "")
            {
                return "";
            }
            if (TargetName == "")
            {
                return "";
            }
            this.xmlDoc.Load(Doc);
            XmlNodeList xmlNodeList = this.xmlDoc.SelectNodes(".//Targets/Target");
            foreach (object obj in xmlNodeList)
            {
                XmlNode xmlNode = (XmlNode)obj;
                if (xmlNode.SelectSingleNode("./TargetName").InnerText == TargetName)
                {
                    string str = "";
                    str = str + "Device: " + xmlNode.SelectSingleNode(".//Device").InnerText + "\r\n";
                    str = str + "Error: " + xmlNode.SelectSingleNode(".//TargetStatus/Error").InnerText + "\r\n";
                    str = str + "ListingPath: " + xmlNode.SelectSingleNode(".//TargetCommonOption/ListingPath").InnerText + "\r\n";
                    str = str + "OutputDirectory: " + xmlNode.SelectSingleNode(".//TargetCommonOption/OutputDirectory").InnerText + "\r\n";
                    str = str + "Define: " + xmlNode.SelectSingleNode(".//VariousControls/Define").InnerText + "\r\n";
                    str += "IncludePath: \r\n";
                    string text = xmlNode.SelectSingleNode(".//VariousControls/IncludePath").InnerText;
                    text = text.Replace("..\\", "**\\");
                    text = text.Replace(".\\", "..\\");
                    text = text.Replace("**\\", "..\\");
                    text = text.Replace(";", ";\r\n");
                    return str + text;
                }
            }
            return null;
        }

        // Token: 0x06000013 RID: 19 RVA: 0x00002BD0 File Offset: 0x00000DD0
        private string MDK_DefineRead(string Doc, string TargetName)
        {
            string defineString = "";
            if (Doc == "")
            {
                return "";
            }
            if (TargetName == "")
            {
                return "";
            }
            this.xmlDoc.Load(Doc);
            XmlNodeList xmlNodeList = this.xmlDoc.SelectNodes(".//Targets/Target");
            foreach (object obj in xmlNodeList)
            {
                XmlNode xmlNode = (XmlNode)obj;
                if (xmlNode.SelectSingleNode("./TargetName").InnerText == TargetName)
                {
                    defineString = xmlNode.SelectSingleNode(".//VariousControls/Define").InnerText;
                    if (!defineString.EndsWith(","))
                    {
                        defineString += ";";
                    }
                    defineString = defineString.Replace(',', ';');
                    return defineString;
                }
            }
            return null;
        }

        // Token: 0x06000014 RID: 20 RVA: 0x00002C94 File Offset: 0x00000E94
        private string MDK_IncludePathRead(string Doc, string TargetName)
        {
            if (Doc == "")
            {
                return "";
            }
            if (TargetName == "")
            {
                return "";
            }
            this.xmlDoc.Load(Doc);
            XmlNodeList xmlNodeList = this.xmlDoc.SelectNodes(".//Targets/Target");
            foreach (object obj in xmlNodeList)
            {
                XmlNode xmlNode = (XmlNode)obj;
                if (xmlNode.SelectSingleNode("./TargetName").InnerText == TargetName)
                {
                    return xmlNode.SelectSingleNode(".//VariousControls/IncludePath").InnerText;
                }
            }
            return null;
        }

        // Token: 0x06000015 RID: 21 RVA: 0x00002D58 File Offset: 0x00000F58
        private string[] MDK_GroupRead(string Doc, string TargetName)
        {
            if (Doc == "")
            {
                return null;
            }
            if (TargetName == "")
            {
                return null;
            }
            this.xmlDoc.Load(Doc);
            XmlNodeList xmlNodeList = this.xmlDoc.SelectNodes(".//Targets/Target");
            foreach (object obj in xmlNodeList)
            {
                XmlNode xmlNode = (XmlNode)obj;
                if (xmlNode.SelectSingleNode("./TargetName").InnerText == TargetName)
                {
                    XmlNodeList xmlNodeList2 = xmlNode.SelectNodes(".//Groups/*");
                    string[] array = new string[xmlNodeList2.Count];
                    int num = 0;
                    foreach (object obj2 in xmlNodeList2)
                    {
                        XmlNode xmlNode2 = (XmlNode)obj2;
                        array[num] = xmlNode2.SelectSingleNode("./GroupName").InnerText;
                        num++;
                    }
                    return array;
                }
            }
            return null;
        }

        // Token: 0x06000016 RID: 22 RVA: 0x00002E88 File Offset: 0x00001088
        private string[] MDK_SrcRead(string Doc, string TargetName, string Group)
        {
            if (Doc == "")
            {
                return null;
            }
            if (Group == "")
            {
                return null;
            }
            this.xmlDoc.Load(Doc);
            XmlNodeList xmlNodeList = this.xmlDoc.SelectNodes(".//Targets/Target");
            foreach (object obj in xmlNodeList)
            {
                XmlNode xmlNode = (XmlNode)obj;
                if (xmlNode.SelectSingleNode("./TargetName").InnerText == TargetName)
                {
                    XmlNodeList xmlNodeList2 = xmlNode.SelectNodes(".//Groups/Group");
                    foreach (object obj2 in xmlNodeList2)
                    {
                        XmlNode xmlNode2 = (XmlNode)obj2;
                        if (xmlNode2.SelectSingleNode("./GroupName").InnerText == Group)
                        {
                            XmlNodeList xmlNodeList3 = xmlNode2.SelectNodes("./Files/File");
                            string[] array = new string[xmlNodeList3.Count];
                            int num = 0;
                            foreach (object obj3 in xmlNodeList3)
                            {
                                XmlNode xmlNode3 = (XmlNode)obj3;
                                string innerText = xmlNode3.SelectSingleNode("./FilePath").InnerText;
                                array[num] = innerText;
                                num++;
                            }
                            return array;
                        }
                    }
                }
            }
            return null;
        }

        // Token: 0x06000017 RID: 23 RVA: 0x00003058 File Offset: 0x00001258
        private bool MDK_CheckProject(string DocName)
        {
            if (DocName == "")
            {
                return false;
            }
            bool ISUPROJ;
            try
            {
                this.xmlDoc.Load(DocName);
                XmlNode xmlNode = this.xmlDoc.SelectSingleNode(".//Header");
                if (xmlNode == null)
                {
                    ISUPROJ = false;
                }
                else if (xmlNode.InnerText == "### uVision Project, (C) Keil Software")
                {
                    ISUPROJ = true;
                }
                else
                {
                    ISUPROJ = false;
                }
            }
            catch
            {
                MessageBox.Show(DocName + this.PreStr.notUvProj, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                ISUPROJ = false;
            }
            return ISUPROJ;
        }
        private bool MDK_Display_Info(string DocName)
        {
            bool result;
            try
            {
                string[] TargetArray = this.MDK_TargetRead(DocName);
                this.ElementHost_Add(TargetArray);
                this.ProjectIno.MDK_Target = TargetArray[0];
                this.ProjectIno.IncludePath = this.MDK_IncludePathRead(DocName, TargetArray[0]);
                this.ProjectIno.IncludePath += this.Config.UV4IncPath;
                this.ProjectIno.NMakePreprocessorDefinitions = this.MDK_DefineRead(DocName, TargetArray[0]);
                string[] GroupArray = this.MDK_GroupRead(DocName, this.ProjectIno.MDK_Target);
                this.GroupListBox_Add(GroupArray);
                string[] items = this.MDK_SrcRead(DocName, TargetArray[0], GroupArray[0]);
                this.SrcFileBox_Add(items);
                string str = this.MDK_TargetStatusRead(DocName, TargetArray[0]);
                this.TargetStatusBox_Add(str);
                this.CreateButton.Enabled = true;
                result = true;
            }
            catch
            {
                MessageBox.Show(this.PreStr.notUvProj, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                result = false;
            }
            return result;
        }
        private void VC_vcxproj_Create(string DocName, string[] Targets)
        {
            if (DocName == "")
            {
                return;
            }
            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
            XElement xEl_Project = new XElement(ns + "Project", new object[]
            {
                new XAttribute("DefaultTargets", "Build"),
                new XAttribute("ToolsVersion", "15.0")
            });
            XElement xEl_ItemGroup = new XElement(ns + "ItemGroup", new XAttribute("Label", "ProjectConfigurations"));
            foreach (string targetName in Targets)
            {
                xEl_ItemGroup.Add(new XElement(ns + "ProjectConfiguration", new object[]
                {
                    new XAttribute("Include", "Target|Win32".Replace("Target", targetName)),
                    new XElement(ns + "Configuration", targetName),
                    new XElement(ns + "Platform", "Win32")
                }));
            }
            xEl_Project.Add(xEl_ItemGroup);
            XElement xEl_PropertyGroup = new XElement(ns + "PropertyGroup", new object[]
            {
                new XAttribute("Label", "Globals"),
                new XElement(ns + "ProjectGuid", Guid.NewGuid().ToString("B")),
                new XElement(ns + "Keyword", "MakeFileProj"),
                new XElement(ns + "WindowsTargetPlatformVersion", "10.0.17134.0"),
            });
            xEl_Project.Add(xEl_PropertyGroup);
            xEl_Project.Add(new XElement(ns + "Import", new XAttribute("Project", "$(VCTargetsPath)\\Microsoft.Cpp.Default.props")));
            foreach (string newValue in Targets)
            {
                xEl_Project.Add(new XElement(ns + "PropertyGroup", new object[]
                {
                    new XAttribute("Condition", "'$(Configuration)|$(Platform)'=='Target|Win32'".Replace("Target", newValue)),
                    new XAttribute("Label", "Configuration"),
                    new XElement(ns + "ConfigurationType", "Makefile"),
                    new XElement(ns + "UseDebugLibraries", "true"),
                    new XElement(ns + "PlatformToolset", "v141"),
                }));
            }
            xEl_Project.Add(new object[]
            {
                new XElement(ns + "Import", new XAttribute("Project", "$(VCTargetsPath)\\Microsoft.Cpp.props")),
                new XElement(ns + "ImportGroup", new XAttribute("Label", "ExtensionSettings"))
            });
            foreach (string newValue2 in Targets)
            {
                xEl_Project.Add(new XElement(ns + "ImportGroup", new object[]
                {
                    new XAttribute("Condition", "'$(Configuration)|$(Platform)'=='Target|Win32'".Replace("Target", newValue2)),
                    new XAttribute("Label", "PropertySheets"),
                    new XElement(ns + "Import", new object[]
                    {
                        new XAttribute("Project", "$(UserRootDir)\\Microsoft.Cpp.$(Platform).user.props"),
                        new XAttribute("Condition", "exists('$(UserRootDir)\\Microsoft.Cpp.$(Platform).user.props')"),
                        new XAttribute("Label", "LocalAppDataPlatform")
                    })
                }));
            }
            xEl_Project.Add(new XElement(ns + "PropertyGroup", new XAttribute("Label", "UserMacros")));
            foreach (string oneTarget in Targets)
            {
                string includepathTemp = this.MDK_IncludePathRead(this.ProjectIno.MDK_Project_File, oneTarget);
                string[] array = includepathTemp.Split(new char[]
                {
                    ';'
                });
                string keilIncludePath = null;
                for (int m = 0; m < array.Length; m++)
                {
                    string fullPath = this.GetFullPath(this.ProjectIno.MDK_Project_Path, array[m]);
                    keilIncludePath = keilIncludePath + this.GetRelativePath(this.ProjectIno.VCProject_Path, fullPath) + ";";
                }
                keilIncludePath += this.Config.UV4IncPath + ";";

                xEl_Project.Add(new XElement(ns + "PropertyGroup", new object[]
                {
                    new XAttribute("Condition", "'$(Configuration)|$(Platform)'=='Target|Win32'".Replace("Target", oneTarget)),
                    new XElement(ns + "NMakeOutput", "Template.bin".Replace("Template", this.ProjectIno.ProjectName)),


                    new XElement(ns + "NMakePreprocessorDefinitions",this.PreStr.predefineKeil+this.Config.PreDefine+ this.MDK_DefineRead(this.ProjectIno.MDK_Project_File, oneTarget)),

                    new XElement(ns + "IncludePath", keilIncludePath),
                    new XElement(ns + "NMakeBuildCommandLine", this.ProjectIno.NMakeBuildCommandLine.Replace("Target", oneTarget)),
                    new XElement(ns + "LibraryPath",@"$(VC_LibraryPath_x86);$(WindowsSDK_LibraryPath_x86);$(NETFXKitsDir)Lib\um\x86;"+this.Config.UV4LibPath+";")

                }));
            }
            xEl_Project.Add(new XElement(ns + "ItemDefinitionGroup", ""));
            string[] array2 = this.MDK_GroupRead(this.ProjectIno.MDK_Project_File, Targets[0]);
            XElement xEl_none = new XElement(ns + "ItemGroup", "");
            XElement xEl_ClCompile = new XElement(ns + "ItemGroup", "");
            foreach (string group in array2)
            {
                string[] array4 = this.MDK_SrcRead(this.ProjectIno.MDK_Project_File, Targets[0], group);
                foreach (string targetPat in array4)
                {
                    string text5 = this.GetFullPath(this.ProjectIno.MDK_Project_Path, targetPat);
                    text5 = this.GetRelativePath(this.ProjectIno.VCProject_Path, text5);
                    if (text5.EndsWith(".c"))
                    {
                        xEl_ClCompile.Add(new XElement(ns + "ClCompile", new XAttribute("Include", text5)));
                    }
                    else
                    {
                        xEl_none.Add(new XElement(ns + "None", new XAttribute("Include", text5)));
                    }
                }
            }
            xEl_none.Add(new XElement(ns + "None", new XAttribute("Include", "Readme.txt")));
            xEl_Project.Add(xEl_none);
            xEl_Project.Add(xEl_ClCompile);
            xEl_Project.Add(new XElement(ns + "Import", new XAttribute("Project", "$(VCTargetsPath)\\Microsoft.Cpp.targets")));
            xEl_Project.Add(new XElement(ns + "ImportGroup", new object[]
            {
                new XAttribute("Label", "ExtensionTargets"),
                ""
            }));
            xEl_Project.Save(DocName);
        }

        private void VC_Filters_Create(string DocName, string[] Targets)
        {
            if (DocName == "")
            {
                return;
            }
            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
            XElement xelement = new XElement(ns + "Project", new object[]
            {
                new XAttribute("DefaultTargets", "Build"),
                new XAttribute("ToolsVersion", "4.0")
            });
            XElement xEl_ItemGroup = new XElement(ns + "ItemGroup", "");
            string sourceFiles = "资源文件";
            xEl_ItemGroup.Add(new object[]
            {
                new XElement(ns + "Filter", new object[]
                {
                    new XAttribute("Include", sourceFiles),
                    new XElement(ns + "UniqueIdentifier", Guid.NewGuid().ToString("B")),
                    new XElement(ns + "Extensions", "cpp;c;cc;cxx;def;odl;idl;hpj;bat;asm;asmx")
                }),
                new XElement(ns + "Filter", new object[]
                {
                    new XAttribute("Include", "include"),
                    new XElement(ns + "UniqueIdentifier", Guid.NewGuid().ToString("B")),
                    new XElement(ns + "Extensions", "h;hpp;hxx;hm;inl;inc;xsd")
                }),
                new XElement(ns + "Filter", new object[]
                {
                    new XAttribute("Include", "项目说明"),
                    new XElement(ns + "UniqueIdentifier", Guid.NewGuid().ToString("B")),
                    new XElement(ns + "Extensions", "txt")
                })
            });
            string[] groups = this.MDK_GroupRead(this.ProjectIno.MDK_Project_File, this.ProjectIno.MDK_Target);
            foreach (string str in groups)
            {
                xEl_ItemGroup.Add(new XElement(ns + "Filter", new object[]
                {
                    new XAttribute("Include", sourceFiles + "\\" + str),
                    new XElement(ns + "UniqueIdentifier", Guid.NewGuid().ToString("B"))
                }));
            }
            xelement.Add(xEl_ItemGroup);
            xEl_ItemGroup = new XElement(ns + "ItemGroup", "");
            XElement xEl_itemGroup = new XElement(ns + "ItemGroup", "");
            foreach (string group in groups)
            {
                string[] src_inThisGroup = this.MDK_SrcRead(this.ProjectIno.MDK_Project_File, Targets[0], group);
                foreach (string src_path in src_inThisGroup)
                {
                    string sr_Full_path = this.GetFullPath(this.ProjectIno.MDK_Project_Path, src_path);
                    sr_Full_path = this.GetRelativePath(this.ProjectIno.VCProject_Path, sr_Full_path);
                    if (sr_Full_path.EndsWith(".c"))
                    {
                        xEl_itemGroup.Add(new XElement(ns + "ClCompile", new object[]
                        {
                            new XAttribute("Include", sr_Full_path),
                            new XElement(ns + "Filter", sourceFiles +"\\" + group)
                        }));
                    }
                    else
                    {
                        xEl_ItemGroup.Add(new XElement(ns + "None", new object[]
                        {
                            new XAttribute("Include", sr_Full_path),
                            new XElement(ns + "Filter", sourceFiles+"\\" + group)
                        }));
                    }
                }
            }
            xEl_ItemGroup.Add(new XElement(ns + "None", new object[]
            {
                new XAttribute("Include", "Readme.txt"),
                new XElement(ns + "Filter", sourceFiles)
            }));
            xelement.Add(xEl_itemGroup);
            xelement.Add(xEl_ItemGroup);
            xelement.Save(DocName);
        }
        private void VC_Create_UserFile(string DocName, string Debugcmd, string WorkingDirectory, string[] Targets)
        {
            if (DocName == "")
            {
                return;
            }
            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
            XElement xEl_proj = new XElement(ns + "Project", new XAttribute("ToolsVersion", "4.0"));
            foreach (string newValue in Targets)
            {
                xEl_proj.Add(new XElement(ns + "PropertyGroup", new object[]
                {
                    new XAttribute("Condition", "'$(Configuration)|$(Platform)'=='Target|Win32'".Replace("Target", newValue)),
                    new XElement(ns + "LocalDebuggerCommand", this.ProjectIno.UV4_Path),
                    new XElement(ns + "LocalDebuggerCommandArguments", Debugcmd.Replace("Target", newValue)),
                    new XElement(ns + "LocalDebuggerWorkingDirectory", WorkingDirectory),
                    new XElement(ns + "DebuggerFlavor", "WindowsLocalDebugger")
                }));
            }
            xEl_proj.Save(DocName);
        }

        private void VC_Creat_readme(string DocName, string ProjectName)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("========================================================================\r\n");
            stringBuilder.Append("    生成文件项目：Template 项目概述\r\n");
            stringBuilder.Append("========================================================================\r\n");
            stringBuilder.Append("\r\n");
            stringBuilder.Append("本文件概要介绍组成 Template 项目的每个文件的内容。\r\n");
            stringBuilder.Append("\r\n");
            stringBuilder.Append("Template.sln\r\n");
            stringBuilder.Append("    这是Template项目的解决方案文件\r\n");
            stringBuilder.Append("\r\n");
            stringBuilder.Append("Template.vcxproj\r\n");
            stringBuilder.Append("    这是Template项目的主项目文件\r\n");
            stringBuilder.Append("    其中包含了这个项目中的各个Target，\r\n");
            stringBuilder.Append("    以及Include Path、所有源文件的路径、编译命令。\r\n");
            stringBuilder.Append("\r\n");
            stringBuilder.Append("Template.vcxproj.filters\r\n");
            stringBuilder.Append("    这是Template项目的项目筛选器文件。\r\n");
            stringBuilder.Append("    它包含了这个项目中的所有源文件分组及源文件的路径。\r\n");
            stringBuilder.Append("\r\n");
            stringBuilder.Append("Template.vcxproj.user\r\n");
            stringBuilder.Append("    这是Template项目的 用户文件，\r\n");
            stringBuilder.Append("    它包含了这个项目中的各个Target的 Debug命令。\r\n");
            stringBuilder.Append("\r\n");
            stringBuilder.Append("以上文件由MDK Project  To Visual Studio Project 工具读取 Keil uVision4\r\n");
            stringBuilder.Append("的项目文件：Template.uvproj 中的设定，按照Visual Studio 2010\r\n");
            stringBuilder.Append("中VC++ “生成文件项目” 的模板文件来生成的，如有疑问，请看MSDN~\r\n");
            stringBuilder.Append("\r\n");
            DateTime dt = DateTime.Now;
            stringBuilder.Append(dt.ToString() + "\r\n");
            stringBuilder.Append(this.PreStr.author + "\r\n");
            stringBuilder.Append("/////////////////////////////////////////////////////////////////////////////\r\n");
            stringBuilder = stringBuilder.Replace("Template", ProjectName);
            FileStream fileStream = File.OpenWrite(DocName);
            byte[] bytes = new UTF8Encoding(true).GetBytes(stringBuilder.ToString());
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
        }

        private void VC_Creat_Sln(string DocName, string ProjectName, string[] Targets)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Microsoft Visual Studio Solution File, Format Version 12.00\r\n");
            stringBuilder.Append("# Visual Studio 15\r\n");
            stringBuilder.Append("VisualStudioVersion = 15.0.27703.2018\r\n");
            stringBuilder.Append("MinimumVisualStudioVersion = 10.0.40219.1\r\n");
            stringBuilder.Append("Project(\"{" + Guid.NewGuid().ToString("B") + "}\") = \"Template\", \"Template.vcxproj\", \"{" + Guid.NewGuid().ToString("B") + "}\"");
            stringBuilder.Append("\r\nEndProject\r\n");
            stringBuilder.Append("Global\r\n");
            stringBuilder.Append("\tGlobalSection(SolutionConfigurationPlatforms) = preSolution\r\n");
            foreach (string newValue in Targets)
            {
                string text = "\t\tDebug|Win32 = Debug|Win32\r\n";
                text = text.Replace("Debug", newValue);
                stringBuilder.Append(text);
            }
            stringBuilder.Append("\tEndGlobalSection\r\n");
            stringBuilder.Append("\tGlobalSection(ProjectConfigurationPlatforms) = postSolution\r\n");
            string newValue2 = Guid.NewGuid().ToString("B");
            foreach (string newValue3 in Targets)
            {
                string text2 = "\t\tGUID.Debug|Win32.ActiveCfg = Debug|Win32\r\n";
                string text3 = "\t\tGUID.Debug|Win32.Build.0 = Debug|Win32\r\n";
                text2 = text2.Replace("GUID", newValue2);
                text2 = text2.Replace("Debug", newValue3);
                text3 = text3.Replace("GUID", newValue2);
                text3 = text3.Replace("Debug", newValue3);
                stringBuilder.Append(text2);
                stringBuilder.Append(text3);
            }
            stringBuilder.Append("\tEndGlobalSection\r\n");
            stringBuilder.Append("\t\tGlobalSection(SolutionProperties) = preSolution\r\n");
            stringBuilder.Append("\t\tHideSolutionNode = FALSE\r\n");
            stringBuilder.Append("\tEndGlobalSection\r\n");
            stringBuilder.Append("\tGlobalSection(ExtensibilityGlobals) = postSolution\r\n");
            stringBuilder.Append("\t\tSolutionGuid = {" + Guid.NewGuid().ToString("B") + "}\r\n");
            stringBuilder.Append("\tEndGlobalSection\r\n");
            stringBuilder.Append("EndGlobal\r\n");
            stringBuilder = stringBuilder.Replace("Template", ProjectName);
            FileStream fileStream = File.OpenWrite(DocName);
            byte[] bytes = new UTF8Encoding(true).GetBytes(stringBuilder.ToString());
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
            stringBuilder.Clear();
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
            {
                //RootFolder = Environment.SpecialFolder.MyComputer,
                Description = "Please select the Visual Studio Project Path",
                SelectedPath = this.ProjectIno.MDK_Project_Path
            };
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.ProjectIno.VCProject_Path = Path.Combine(new string[]
                {
                    folderBrowserDialog.SelectedPath,
                    "Visual_Studio_Project"
                }) + "\\";
                if (!Directory.Exists(this.ProjectIno.VCProject_Path))
                {
                    Directory.CreateDirectory(this.ProjectIno.VCProject_Path);
                }
                string[] targets = this.MDK_TargetRead(this.ProjectIno.MDK_Project_File);
                string relativePath = this.GetRelativePath(this.ProjectIno.VCProject_Path, this.ProjectIno.MDK_Project_File);
                this.ProjectIno.NMakeBuildCommandLine = string.Concat(new string[]
                {
                    "\"",
                    this.ProjectIno.UV4_Path,
                    "\" -b ",
                    relativePath,
                    " -t \"Target\" -j0 -o Build.log"
                });
                this.ProjectIno.LocalDebuggerCommandArguments = "-d " + this.ProjectIno.ProjectName + ".uvproj -t \"Target\"";

                string docName = this.ProjectIno.VCProject_Path + this.ProjectIno.ProjectName + ".sln";
                this.VC_Creat_Sln(docName, this.ProjectIno.ProjectName, targets);

                docName = this.ProjectIno.VCProject_Path + this.ProjectIno.VC_Filters_Name;
                this.VC_Filters_Create(docName, targets);

                docName = this.ProjectIno.VCProject_Path + this.ProjectIno.VcxprojName;
                this.VC_vcxproj_Create(docName, targets);

                this.ProjectIno.LocalDebuggerWorkingDirectory = this.GetRelativePath(this.ProjectIno.VCProject_Path, this.ProjectIno.MDK_Project_Path);

                docName = this.ProjectIno.VCProject_Path + this.ProjectIno.VC_UserFileName;
                this.VC_Create_UserFile(docName, this.ProjectIno.LocalDebuggerCommandArguments, this.ProjectIno.LocalDebuggerWorkingDirectory, targets);

                docName = this.ProjectIno.VCProject_Path + "readme.txt";
                this.VC_Creat_readme(docName, this.ProjectIno.ProjectName);


                docName = this.ProjectIno.VCProject_Path + this.ProjectIno.ProjectName + ".sln";

                DialogResult dr = MessageBox.Show(
                    "The Visual Studio Project Creat Complete!\n打开VS工程(是)\n打开文件夹(否)\n退出(取消)", "Enjoy VS Coding!",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                Uri baseUri = new Uri(docName);
                docName = GetFullPath(docName, "");
                switch (dr)
                {
                    case DialogResult.None:
                        break;
                    case DialogResult.OK:
                        break;
                    case DialogResult.Cancel:
                        break;
                    case DialogResult.Abort:
                        break;
                    case DialogResult.Retry:
                        break;
                    case DialogResult.Ignore:
                        break;
                    case DialogResult.Yes:
                        try
                        {
                            Process.Start(docName);
                        }
                        catch
                        {

                            throw;
                        }

                        break;
                    case DialogResult.No:
                        Process.Start(this.ProjectIno.VCProject_Path);
                        break;
                    default:
                        break;
                }

                return;
            }
        }
        private bool TryGetSoftwarePath(ref string path)
        {
            string strPathResult = string.Empty;
            //string strKeyName = "";     //"(Default)" key, which contains the intalled path 
            object objResult = null;

            Microsoft.Win32.RegistryKey regKey = null;
            Microsoft.Win32.RegistryKey regSubKey = null;

            try
            {
                //Read the key 
                regKey = Microsoft.Win32.Registry.LocalMachine;
                regSubKey = regKey.OpenSubKey(this.PreStr.regPath, false);

                //Read the path 
                try
                {
                    objResult = regSubKey.GetValue("Path");
                }
                catch
                {
                }

                //regValueKind = regSubKey.GetValueKind("Path");

                ////Set the path 
                //if (regValueKind == Microsoft.Win32.RegistryValueKind.String)
                //{
                //    strPathResult = objResult.ToString();
                //}
            }
            catch (System.Security.SecurityException ex)
            {
                throw new System.Security.SecurityException("You have no right to read the registry!", ex);
            }
            finally
            {

                if (regKey != null)
                {
                    regKey.Close();
                    regKey = null;
                }

                if (regSubKey != null)
                {
                    regSubKey.Close();
                    regSubKey = null;
                }
            }
            if (objResult == null)
            {
                return false;
            }

            if (objResult.ToString() != string.Empty)
            {
                //Found 
                path = objResult.ToString();
                return true;
            }
            else
            {
                //Not found 
                path = null;
                return false;
            }
        }
        private void Tb_predef_TextChanged(object sender, EventArgs e)
        {
            if (!tb_predef.Text.EndsWith(";"))
            {
                tb_predef.Text += ";";
            }
            tb_predef.Text = tb_predef.Text.Replace(",", ";").Replace("，", ";").Replace("；", ";").Replace(".", ";").Replace(";;;", ";").Replace(";;", ";").Replace(@" ", "");
            if (tb_predef.Text == ";")
                tb_predef.Text = "";
            this.Config.PreDefine = tb_predef.Text;
        }

        private void Tb_predef_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Tb_predef_TextChanged(sender, e);
            }
        }

        private void Lbppd_Click(object sender, EventArgs e)
        {
            tb_predef.Text = this.PreStr.predefine;
        }
        private XDocument document = new XDocument();
        private XmlDocument xmlDoc = new XmlDocument();
        private FileInfo[] fileInfos;
        private _ProjectInfo ProjectIno;
        private _Config Config;
        private _uprojInfo[] uprojInfo;

        private _PreStr PreStr = new _PreStr
        {
            ToolsVersion = "0.3",
            ApplicationStartpath = Path.GetDirectoryName(Application.ExecutablePath),
            configfilename = "Config.xml",
            fristUse = "Select the Keil Vision4 installation directory when you first times use the tool",
            predefineKeil = @"sbit=;sfr=;data=;xdata=;code=;idata=;pdata=;_at_=#;",
            predefine = @"VS;",
            regPath = @"SOFTWARE\WOW6432Node\Keil\Products\C51\",
            notUvProj = "\nRead File Error! Maybe it is't MDK Project File",
            notkeilexe = "This is NOT KEIL PROGRAM!!\n",
            Sourepretext = "Open Or drop The MDK the Project File to here!",
            nameSp = "MAILTO:heartacker@outlook.com",
            author = "heartacker",
            noKeil = "Can not detect keil Program!",
            FindUprojInThisFolder = "Oops!,Find uproject in this folder!\n Plz select one to convert!",
            selectnewfolder = "Select new Folder",
        };
        struct _uprojInfo
        {
            public string fileName;
            public string fileFullname;
        }

        private struct _ProjectInfo
        {
            public string UV4_Path;
            public string MDK_Project_Path;
            public string MDK_Project_File;
            public string MDK_Target;
            public string ProjectName;
            public string IncludePath;
            public string VCProject_Path;
            public string VcxprojName;
            public string VC_Filters_Name;
            public string VC_UserFileName;
            public string NMakePreprocessorDefinitions;
            public string NMakeBuildCommandLine;
            public string NMakeCleanCommandLine;
            public string LocalDebuggerCommandArguments;
            public string LocalDebuggerWorkingDirectory;
            public string CuruProjectFileDir;
        }
        private struct _Config
        {
            public string ToolName;
            public string ToolsVersion;
            public string UV4Path;
            public string DocName;
            public string UV4IncPath;
            public string UV4LibPath;
            public string PreDefine;
        }

        private struct _PreStr
        {
            public string ApplicationStartpath;
            public string configfilename;
            public string fristUse;
            public string ToolsVersion;
            public string predefineKeil;
            public string predefine;
            public string noKeil;
            public string regPath;
            public string Sourepretext;
            public string notUvProj;
            public string notkeilexe;
            public string author;
            public string nameSp;
            public string FindUprojInThisFolder;
            public string selectnewfolder;

        }

        private void SourcePathCBOX_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.SourcePathCBOX.SelectedIndex < 0)
            {
                return;
            }
            if (this.SourcePathCBOX.SelectedIndex >= this.SourcePathCBOX.Items.Count)
            {
                return;
            }
            else
            {
                this.TryDispuProjinfo(this.uprojInfo[this.SourcePathCBOX.SelectedIndex].fileFullname);
            }
        }

        private void ElementHost_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.ProjectIno.CuruProjectFileDir != "")
            {
                string[] array = this.MDK_TargetRead(this.ProjectIno.CuruProjectFileDir);
                string[] array2 = this.MDK_GroupRead(this.ProjectIno.CuruProjectFileDir, array[this.elementHost.SelectedIndex]);
                this.GroupListBox_Add(array2);
                string[] items = this.MDK_SrcRead(this.ProjectIno.CuruProjectFileDir, array[this.elementHost.SelectedIndex], array2[0]);
                this.SrcFileBox_Add(items);
                string str = this.MDK_TargetStatusRead(this.ProjectIno.CuruProjectFileDir, array[this.elementHost.SelectedIndex]);
                this.TargetStatusBox_Add(str);
            }

        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                UpDateCurfolderUproj(sender, e);
            }
        }
        //private void Keil2VS_MouseEnter(object sender, EventArgs e)
        //{
        //    if (e.Data.GetDataPresent(DataFormats.FileDrop))
        //        e.Effect = DragDropEffects.Move;
        //    else e.Effect = DragDropEffects.None;
        //}

        //private void Keil2VS_MouseDown(object sender, MouseEventArgs e)
        //{
        //    string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
        //    extension = System.IO.Path.GetExtension(path);//扩展名
        //    if (extension == ".encrypted")
        //    {
        //        btnEncy.Enabled = false;
        //        btnDecy.Enabled = true;
        //    }
        //    else
        //    {
        //        btnEncy.Enabled = true;
        //        btnDecy.Enabled = false;
        //    }
        //    txtInFile.Text = path;
        //}
        //}

    }
}