using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using KEIL2VS.Properties;
using Microsoft.Win32;

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
            GroupListBox.SelectedIndexChanged += GroupListBox_SelectedIndexChanged;
            FileBox.DoubleClick += FileBox_DoubleClick;
            tbKeil_path.DoubleClick += Tbkeil_path_DoubleClick;
            tbKeil_path.DragDrop += Tbkeil_pathBOX_DragDrop;
            tbKeil_path.DragEnter += Keil2VsDragEnter;
            MinimumSize = new Size(Width, Height);

            AutoScaleMode = AutoScaleMode.None;
            _projectIno.NMakeCleanCommandLine = "";
            CreateButton.Enabled = false;
            string[] array = {
                _preStr.ApplicationStartpath,
                _preStr.Configfilename
            };

            _config.DocName = Path.Combine(array);
            tbKeil_path.Text = _preStr.NoKeil;
            tb_predef.Text = _preStr.Predefine;
            _config.PreDefine = tb_predef.Text;
        }


        private void Keil2VS_Shown(object sender, EventArgs e)
        {
            if (!TryGetSoftwarePath(ref _config.UV4Path))
            {
                ReadConfig(_config.DocName);
            }
            else
            {
                try
                {
                    File.Delete(_config.DocName);
                }
                catch
                {
                    // ignored
                }

                _config.UV4Path = _config.UV4Path.Replace("C51", @"UV4\UV4.exe");
                _config.UV4IncPath = _config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\INC");
                _config.UV4LibPath = _config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\LIB");
                tbKeil_path.BackColor = Color.LawnGreen;
                tbKeil_path.Text = _config.UV4Path;
                tbKeil_path.AllowDrop = false;
            }
            #region BUILD PART 0
            _projectIno.UV4Path = _config.UV4Path + " ";
            //_projectIno.UV4Path = "UV4";
            #endregion
            UpDateCurfolderUproj(sender, e);
            fmdia = new Fmdialog("恭喜");
            fmdia.ShowDialog();

        }


        private void UpDateCurfolderUproj(object sender, EventArgs e)
        {
            string scanpath = _preStr.ApplicationStartpath;
            //try
            //{
            //    if ((Button)sender == btnRefresh)
            //    {
            //        scanpath = new FileInfo(_uprojInfo[SourcePathCBOX.SelectedIndex].FileFullname).DirectoryName;
            //    }
            //}
            //catch
            //{


            //}

            if (!ScanCurrentFolderHas_uProj(scanpath, ref _fileInfos))
                return;

            _uprojInfo = new UprojInfo[_fileInfos.GetLength(0)];
            for (var i = 0; i < _fileInfos.GetLength(0); i++)
            {
                _uprojInfo[i].FileName = _fileInfos[i].Name;
                _uprojInfo[i].FileFullname = _fileInfos[i].FullName;
            }
            SourcePathComobox_add(_uprojInfo);
            //sourcepathTip.Show(_preStr.FindUprojInThisFolder, SourcePathCBOX, SourcePathCBOX.Location.X + 200, SourcePathCBOX.Top - 100, 1000 * 10);
            TryDispuProjinfo(_uprojInfo[0].FileFullname);
        }


        private bool ScanCurrentFolderHas_uProj(string exeDir, ref FileInfo[] fileInfo)
        {
            string[] projPath = Directory.GetFiles(exeDir, "*.uvproj", SearchOption.AllDirectories);
            var cnt1 = projPath.Length;

            if (0 == cnt1) return false;
            fileInfo = new FileInfo[cnt1];
            for (var i = 0; i < cnt1; i++)
            {

                fileInfo[i] = new FileInfo(projPath[i]);
            }
            return true;
        }


        private void SourcePathComobox_add(UprojInfo[] it)
        {
            SourcePathCBOX.Items.Clear();
            for (var i = 0; i < it.GetLength(0); i++)
            {
                SourcePathCBOX.Items.Add(it[i].FileName);
            }
            //cboxbatch.Visible = it.Length <= 1 ? false : true;
            //this.SourcePathCBOX.Items.Add(this.preStr.selectnewfolder);
            SourcePathCBOX.SelectedIndex = 0;
        }

        //private void elementHost_Add(string[] Items)
        //{
        //    this.elementHost.Items.Clear();
        //    foreach (string newItem in Items)
        //    {
        //        this.elementHost.Items.Add(newItem);
        //    }
        //}

        private void ElementHost_Add(string[] items)
        {
            elementHost.Items.Clear();
            foreach (var newItem in items)
            {
                elementHost.Items.Add(newItem);
            }
            elementHost.SelectedIndex = 0;
        }



        private void GroupListBox_Add(string[] items)
        {
            GroupListBox.Items.Clear();
            foreach (var item in items)
            {
                GroupListBox.Items.Add(item);
            }
        }

        private void SrcFileBox_Add(string[] items)
        {
            FileBox.Items.Clear();
            foreach (var item in items)
            {
                FileBox.Items.Add(item);
            }
        }

        private void TargetStatusBox_Add(string str) => TargetStatus.Text = str;

        private void GroupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_projectIno.CuruProjectFileDir == "") return;
            string[] array = MDK_TargetRead(_projectIno.CuruProjectFileDir);
            string[] array2 = MDK_GroupRead(_projectIno.CuruProjectFileDir, array[elementHost.SelectedIndex]);
            string[] items = MDK_SrcRead(_projectIno.CuruProjectFileDir, array[elementHost.SelectedIndex], array2[GroupListBox.SelectedIndex]);
            SrcFileBox_Add(items);
        }


        private void FileBox_DoubleClick(object sander, EventArgs e)
        {
            if (FileBox.SelectedIndex < 0)
            {
                return;
            }
            var text = FileBox.SelectedItem.ToString();
            text = GetFullPath(_projectIno.MdkProjectPath, text);
            try
            {
                new Process
                {
                    StartInfo = { FileName = "notepad++.exe", Arguments = text }
                }.Start();
            }
            catch
            {

                Process.Start(text);
            }

        }
        private void BtnOpenFolder_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                DefaultExt = "uvproj",
                Filter = @"MDK Project File (*.uvproj)|*.uvproj|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            var fileName = openFileDialog.FileName;
            if (fileName == "") return;

            if (!MDK_CheckProject(fileName)) return;

            TryDispuProjinfo(fileName);
            _uprojInfo = new UprojInfo[1];
            _uprojInfo[0].FileName = _projectIno.ProjectName;
            _uprojInfo[0].FileFullname = _projectIno.MdkProjectFile;
            SourcePathCBOX.Text = _uprojInfo[0].FileFullname;
            SourcePathComobox_add(_uprojInfo);

        }

        private void SourcePathCBOX_DragDrop(object sender, DragEventArgs e)
        {
            SourcePathCBOX.ForeColor = Color.Black;
            var dropFilename = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            if (!MDK_CheckProject(dropFilename))
            {
                return;
            }

            TryDispuProjinfo(dropFilename);
            _uprojInfo = new UprojInfo[1];
            _uprojInfo[0].FileName = _projectIno.ProjectName;
            _uprojInfo[0].FileFullname = _projectIno.MdkProjectFile;
            SourcePathCBOX.Text = _uprojInfo[0].FileFullname;
            SourcePathComobox_add(_uprojInfo);
        }
        private void TryDispuProjinfo(string fileFullname)
        {
            if (!MDK_Display_Info(fileFullname)) return;
            UpdataLinkOfProject(fileFullname);
            _projectIno.CuruProjectFileDir = fileFullname;
            _projectIno.MdkProjectFile = fileFullname;
            _projectIno.MdkProjectPath = Path.GetDirectoryName(fileFullname) + "\\";
            _projectIno.ProjectName = Path.GetFileNameWithoutExtension(fileFullname);
            _projectIno.VcxprojName = _projectIno.ProjectName + ".vcxproj";
            _projectIno.VcFiltersName = _projectIno.VcxprojName + ".filters";
            _projectIno.VcUserFileName = _projectIno.VcxprojName + ".user";
        }

        private void UpdataLinkOfProject(string fileFullname)
        {
            llbeUprojPath.Links.Clear();
            string padleft = "";
            llbeUprojPath.Text = padleft + fileFullname;
            llbeUprojPath.Links.Add(padleft.Length, fileFullname.Length, fileFullname);
            llbeUprojPath.Links[0].Visited = false;
        }

        private void Keil2VsDragEnter(object sander, DragEventArgs e)
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
            var tempPath = tbKeil_path.Text;
            var openFileDialog = new OpenFileDialog
            {
                DefaultExt = "exe",
                Filter = @"Keil Exe File (UV4.exe)|UV4.exe",
                Title = Resources.SELUV4PATH
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                tbKeil_path.Text = tempPath;
                return;
            }
            var fileName = openFileDialog.FileName;
            if (fileName != "")
            {

                _config.UV4Path = openFileDialog.FileName;
                _config.UV4IncPath = _config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\INC");
                _config.UV4LibPath = _config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\LIB");
                Creat_Config(_config.DocName);
                tbKeil_path.Text = _config.UV4Path;
                tbKeil_path.BackColor = Color.LightGreen;
            }
            else
            {
                tbKeil_path.Text = tempPath;
                tbKeil_path.ForeColor = Color.Gray;
            }
        }

        private bool MDK_check_keil_UV4(string text)
        {
            if (text.EndsWith("UV4.exe", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
        private void Tbkeil_pathBOX_DragDrop(object sender, DragEventArgs e)
        {
            var path = tbKeil_path.Text;
            tbKeil_path.ForeColor = Color.Black;
            tbKeil_path.Text = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            var text = tbKeil_path.Text;

            if (MDK_check_keil_UV4(text))
            {
                _config.UV4Path = text;
                _config.UV4IncPath = _config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\INC");
                _config.UV4LibPath = _config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\LIB");
                Creat_Config(_config.DocName);
                return;
            }
            MessageBox.Show(_preStr.Notkeilexe + "\nthis is" + tbKeil_path.Text);
            tbKeil_path.Text = path;
            tbKeil_path.ForeColor = Color.Gray;
        }
        // Token: 0x0600000D RID: 13 RVA: 0x00002740 File Offset: 0x00000940
        private void Creat_Config(string docName)
        {
            if (docName == "")
            {
                return;
            }
            _config.ToolName = "Keil2VisualStudio";
            _config.ToolsVersion = _preStr.ToolsVersion;
            XNamespace ns = _preStr.NameSp;
            var xelement = new XElement(ns + "Tool",
                new XAttribute("Name", _config.ToolName),
                new XAttribute("Author", _preStr.Author),
                new XAttribute("ToolsVersion", _config.ToolsVersion),
                new XElement(ns + "UV4Path", _config.UV4Path),
                new XElement(ns + "UV4IncPath", _config.UV4IncPath),
                new XElement(ns + "UV4LibPath", _config.UV4LibPath)
                );
            xelement.Save(docName);
        }

        // Token: 0x0600000E RID: 14 RVA: 0x00002818 File Offset: 0x00000A18
        private void ReadConfig(string docName)
        {
            Color c = tbKeil_path.BackColor;
            var t = tbKeil_path.Text;

            if (!File.Exists(docName))
            {
                tbKeil_path.BackColor = Color.LightPink;
                MessageBox.Show(_preStr.FristUse);
                var openFileDialog = new OpenFileDialog
                {
                    DefaultExt = "exe",
                    Filter = @"Keil Exe File (UV4.exe)|UV4.exe",
                    Title = Resources.SELUV4PATH
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _config.UV4Path = openFileDialog.FileName;
                    _config.UV4IncPath = _config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\INC");
                    _config.UV4LibPath = _config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\LIB");
                    Creat_Config(_config.DocName);
                    tbKeil_path.BackColor = Color.LightGreen;
                    tbKeil_path.Text = _config.UV4Path;
                }
                else
                {
                    tbKeil_path.Text = t;
                }
            }
            else
            {
                XNamespace ns = _preStr.NameSp;
                XDocument xdocument = XDocument.Load(docName);
                if (xdocument.Root != null)
                {
                    _config.ToolsVersion = xdocument.Root.Attribute("ToolsVersion")?.Value;
                    _config.UV4Path = xdocument.Root.Element(ns + "UV4Path")?.Value;
                    _config.UV4IncPath = xdocument.Root.Element(ns + "UV4IncPath").Value;
                    _config.UV4LibPath = xdocument.Root.Element(ns + "UV4LibPath").Value;
                }

                tbKeil_path.BackColor = Color.LightGreen;
                tbKeil_path.Text = _config.UV4Path;
            }
        }

        private static string GetRelativePath(string basePath, string targetPath)
        {
            var uri = new Uri(basePath);
            var uri2 = new Uri(targetPath);
            return uri.MakeRelativeUri(uri2).ToString().Replace("/", "\\");
        }

        private static string GetFullPath(string basePath, string targetPat)
        {
            var baseUri = new Uri(basePath);
            var uri = new Uri(baseUri, targetPat);
            return uri.ToString().Replace("/", "\\").Replace("file:\\\\\\", "");
        }


        private string[] MDK_TargetRead(string Doc)
        {
            if (Doc == "")
            {
                return null;
            }
            xmlDoc.Load(Doc);
            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(".//Targets/*");
            string[] array = new string[xmlNodeList.Count];
            var num = 0;
            foreach (object obj in xmlNodeList)
            {
                var xmlNode = (XmlNode)obj;
                array[num] = xmlNode.SelectSingleNode("./TargetName").InnerText;
                num++;
            }
            return array;
        }

        private string MDK_TargetStatusRead(string doc, string targetName)
        {
            if (doc == "")
            {
                return "";
            }
            if (targetName == "")
            {
                return "";
            }
            xmlDoc.Load(doc);
            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(".//Targets/Target");
            foreach (object obj in xmlNodeList)
            {
                var xmlNode = (XmlNode)obj;
                if (xmlNode.SelectSingleNode("./TargetName").InnerText != targetName) continue;
                var str = "";
                str = str + "Device: " + xmlNode.SelectSingleNode(".//Device").InnerText + "\r\n";
                str = str + "Error: " + xmlNode.SelectSingleNode(".//TargetStatus/Error").InnerText + "\r\n";
                str = str + "ListingPath: " + xmlNode.SelectSingleNode(".//TargetCommonOption/ListingPath").InnerText + "\r\n";
                str = str + "OutputDirectory: " + xmlNode.SelectSingleNode(".//TargetCommonOption/OutputDirectory").InnerText + "\r\n";
                str = str + "Define: " + xmlNode.SelectSingleNode(".//VariousControls/Define").InnerText + "\r\n";
                str += "IncludePath: \r\n";
                var text = xmlNode.SelectSingleNode(".//VariousControls/IncludePath").InnerText;
                text = text.Replace("..\\", "**\\");
                text = text.Replace(".\\", "..\\");
                text = text.Replace("**\\", "..\\");
                text = text.Replace(";", ";\r\n");
                return str + text;
            }
            return null;
        }

        // Token: 0x06000013 RID: 19 RVA: 0x00002BD0 File Offset: 0x00000DD0
        private string MDK_DefineRead(string doc, string targetName)
        {
            if (doc == "")
            {
                return "";
            }
            if (targetName == "")
            {
                return "";
            }
            xmlDoc.Load(doc);
            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(".//Targets/Target");
            foreach (object obj in xmlNodeList)
            {
                var xmlNode = (XmlNode)obj;
                if (xmlNode.SelectSingleNode("./TargetName").InnerText != targetName) continue;
                var defineString = xmlNode.SelectSingleNode(".//VariousControls/Define").InnerText;
                if (!defineString.EndsWith(","))
                {
                    defineString += ";";
                }
                defineString = defineString.Replace(',', ';');
                return defineString;
            }
            return null;
        }

        // Token: 0x06000014 RID: 20 RVA: 0x00002C94 File Offset: 0x00000E94
        private string MDK_IncludePathRead(string doc, string targetName)
        {
            if (doc == "")
            {
                return "";
            }
            if (targetName == "")
            {
                return "";
            }
            xmlDoc.Load(doc);
            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(".//Targets/Target");
            foreach (object obj in xmlNodeList)
            {
                var xmlNode = (XmlNode)obj;
                if (xmlNode.SelectSingleNode("./TargetName").InnerText == targetName)
                {
                    return xmlNode.SelectSingleNode(".//VariousControls/IncludePath").InnerText;
                }
            }
            return null;
        }

        // Token: 0x06000015 RID: 21 RVA: 0x00002D58 File Offset: 0x00000F58
        private string[] MDK_GroupRead(string doc, string targetName)
        {
            if (doc == "")
            {
                return null;
            }
            if (targetName == "")
            {
                return null;
            }
            xmlDoc.Load(doc);
            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(".//Targets/Target");
            foreach (object obj in xmlNodeList)
            {
                var xmlNode = (XmlNode)obj;
                if (xmlNode.SelectSingleNode("./TargetName").InnerText != targetName) continue;
                XmlNodeList xmlNodeList2 = xmlNode.SelectNodes(".//Groups/*");
                string[] array = new string[xmlNodeList2.Count];
                var num = 0;
                foreach (object obj2 in xmlNodeList2)
                {
                    var xmlNode2 = (XmlNode)obj2;
                    array[num] = xmlNode2.SelectSingleNode("./GroupName").InnerText;
                    num++;
                }
                return array;
            }
            return null;
        }

        // Token: 0x06000016 RID: 22 RVA: 0x00002E88 File Offset: 0x00001088
        private string[] MDK_SrcRead(string doc, string targetName, string Group)
        {
            if (doc == "")
            {
                return null;
            }
            if (Group == "")
            {
                return null;
            }
            xmlDoc.Load(doc);
            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(".//Targets/Target");
            if (xmlNodeList == null) return null;
            foreach (object obj in xmlNodeList)
            {
                var xmlNode = (XmlNode)obj;
                if (xmlNode.SelectSingleNode("./TargetName")?.InnerText != targetName) continue;
                XmlNodeList xmlNodeList2 = xmlNode.SelectNodes(".//Groups/Group");
                if (xmlNodeList2 == null) continue;
                foreach (object obj2 in xmlNodeList2)
                {
                    var xmlNode2 = (XmlNode)obj2;
                    if (xmlNode2.SelectSingleNode("./GroupName")?.InnerText != Group) continue;
                    XmlNodeList xmlNodeList3 = xmlNode2.SelectNodes("./Files/File");
                    string[] array = new string[xmlNodeList3.Count];
                    var num = 0;
                    foreach (object obj3 in xmlNodeList3)
                    {
                        var xmlNode3 = (XmlNode)obj3;
                        var innerText = xmlNode3.SelectSingleNode("./FilePath").InnerText;
                        array[num] = innerText;
                        num++;
                    }

                    return array;
                }
            }

            return null;
        }

        // Token: 0x06000017 RID: 23 RVA: 0x00003058 File Offset: 0x00001258
        private bool MDK_CheckProject(string docName)
        {
            if (docName == "") return false;
            bool isuproj;
            try
            {
                xmlDoc.Load(docName);
                XmlNode xmlNode = xmlDoc.SelectSingleNode(".//Header");
                if (xmlNode == null)
                {
                    isuproj = false;
                }
                else if (xmlNode.InnerText == "### uVision Project, (C) Keil Software")
                {
                    isuproj = true;
                }
                else
                {
                    isuproj = false;
                }
            }
            catch
            {
                MessageBox.Show(docName + _preStr.NotUvProj, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                isuproj = false;
            }
            return isuproj;
        }
        private bool MDK_Display_Info(string docName)
        {
            bool result;
            try
            {
                string[] targetArray = MDK_TargetRead(docName);
                ElementHost_Add(targetArray);
                _projectIno.MdkTarget = targetArray[0];
                _projectIno.IncludePath = MDK_IncludePathRead(docName, targetArray[0]);
                _projectIno.IncludePath += _config.UV4IncPath;
                _projectIno.NMakePreprocessorDefinitions = MDK_DefineRead(docName, targetArray[0]);
                string[] groupArray = MDK_GroupRead(docName, _projectIno.MdkTarget);
                GroupListBox_Add(groupArray);
                string[] items = MDK_SrcRead(docName, targetArray[0], groupArray[0]);
                SrcFileBox_Add(items);
                var str = MDK_TargetStatusRead(docName, targetArray[0]);
                TargetStatusBox_Add(str);
                CreateButton.Enabled = true;
                result = true;
            }
            catch
            {
                MessageBox.Show(_preStr.NotUvProj, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                result = false;
            }
            return result;
        }
        private void VC_vcxproj_Create(string docName, IReadOnlyList<string> targets)
        {
            if (docName == "")
            {
                return;
            }
            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
            var xElProject = new XElement(ns + "Project", new XAttribute("DefaultTargets", "Build"), new XAttribute("ToolsVersion", "15.0"));
            var xEl_ItemGroup = new XElement(ns + "ItemGroup", new XAttribute("Label", "ProjectConfigurations"));
            foreach (var targetName in targets)
            {
                xEl_ItemGroup.Add(new XElement(ns + "ProjectConfiguration",
                    new XAttribute("Include", "Target|Win32".Replace("Target", targetName)),
                    new XElement(ns + "Configuration", targetName),
                    new XElement(ns + "Platform", "Win32")));
            }
            xElProject.Add(xEl_ItemGroup);
            var xEl_PropertyGroup = new XElement(ns + "PropertyGroup",
                new XAttribute("Label", "Globals"),
                new XElement(ns + "ProjectGuid", Guid.NewGuid().ToString("B")),
                new XElement(ns + "Keyword", "MakeFileProj"),
                new XElement(ns + "WindowsTargetPlatformVersion", "10.0.17134.0"));
            xElProject.Add(xEl_PropertyGroup);
            xElProject.Add(new XElement(ns + "Import",
                new XAttribute("Project", "$(VCTargetsPath)\\Microsoft.Cpp.Default.props")));
            foreach (var newValue in targets)
            {
                xElProject.Add(new XElement(ns + "PropertyGroup",
                    new XAttribute("Condition", "'$(Configuration)|$(Platform)'=='Target|Win32'".Replace("Target", newValue)),
                    new XAttribute("Label", "Configuration"),
                    new XElement(ns + "ConfigurationType", "Makefile"),
                    new XElement(ns + "UseDebugLibraries", "true"),
                    new XElement(ns + "PlatformToolset", "v141")));
            }
            xElProject.Add(new XElement(ns + "Import",
                new XAttribute("Project", "$(VCTargetsPath)\\Microsoft.Cpp.props")),
                new XElement(ns + "ImportGroup",
                new XAttribute("Label", "ExtensionSettings")));
            foreach (var newValue2 in targets)
            {
                xElProject.Add(new XElement(ns + "ImportGroup",
                    new XAttribute("Condition", "'$(Configuration)|$(Platform)'=='Target|Win32'".Replace("Target", newValue2)),
                    new XAttribute("Label", "PropertySheets"),
                    new XElement(ns + "Import",
                    new XAttribute("Project", "$(UserRootDir)\\Microsoft.Cpp.$(Platform).user.props"),
                    new XAttribute("Condition", "exists('$(UserRootDir)\\Microsoft.Cpp.$(Platform).user.props')"),
                    new XAttribute("Label", "LocalAppDataPlatform"))));
            }
            xElProject.Add(new XElement(ns + "PropertyGroup", new XAttribute("Label", "UserMacros")));
            foreach (var oneTarget in targets)
            {
                var includepathTemp = MDK_IncludePathRead(_projectIno.MdkProjectFile, oneTarget);
                string[] array = includepathTemp.Split(';');
                string keilIncludePath = null;
                foreach (var targetPat in array)
                {
                    var fullPath = GetFullPath(_projectIno.MdkProjectPath, targetPat);
                    keilIncludePath = keilIncludePath + GetRelativePath(_projectIno.VcProjectPath, fullPath) + ";";
                }
                //for (var m = 0; m < array.Length; m++)
                //{
                //    var fullPath = GetFullPath(_projectIno.MDK_Project_Path, array[m]);
                //    keilIncludePath = keilIncludePath + GetRelativePath(_projectIno.VCProject_Path, fullPath) + ";";
                //}
                keilIncludePath += _config.UV4IncPath + ";";

                xElProject.Add(new XElement(ns + "PropertyGroup",
                    new XAttribute("Condition", "'$(Configuration)|$(Platform)'=='Target|Win32'".Replace("Target", oneTarget)),
                    new XElement(ns + "NMakeOutput", "Template.bin".Replace("Template", _projectIno.ProjectName)),
                    new XElement(ns + "NMakePreprocessorDefinitions", _preStr.PredefineKeil + _config.PreDefine + MDK_DefineRead(_projectIno.MdkProjectFile, oneTarget)),
                    new XElement(ns + "IncludePath", keilIncludePath),
                    new XElement(ns + "NMakeBuildCommandLine", _projectIno.NMakeBuildCommandLine.Replace("Target", oneTarget)),
                    new XElement(ns + "LibraryPath", @"$(VC_LibraryPath_x86);$(WindowsSDK_LibraryPath_x86);$(NETFXKitsDir)Lib\um\x86;" + _config.UV4LibPath + ";")));
            }
            xElProject.Add(new XElement(ns + "ItemDefinitionGroup", ""));
            string[] array2 = MDK_GroupRead(_projectIno.MdkProjectFile, targets[0]);
            var xElNone = new XElement(ns + "ItemGroup", "");
            var xElClCompile = new XElement(ns + "ItemGroup", "");
            foreach (var group in array2)
            {
                string[] array4 = MDK_SrcRead(_projectIno.MdkProjectFile, targets[0], group);
                foreach (var targetPat in array4)
                {
                    var text5 = GetFullPath(_projectIno.MdkProjectPath, targetPat);
                    text5 = GetRelativePath(_projectIno.VcProjectPath, text5);
                    if (text5.EndsWith(".c"))
                    {
                        xElClCompile.Add(new XElement(ns + "ClCompile", new XAttribute("Include", text5)));
                    }
                    else
                    {
                        xElNone.Add(new XElement(ns + "None", new XAttribute("Include", text5)));
                    }
                }
            }
            xElNone.Add(new XElement(ns + "None", new XAttribute("Include", "Readme.txt")));
            xElProject.Add(xElNone);
            xElProject.Add(xElClCompile);
            xElProject.Add(new XElement(ns + "Import", new XAttribute("Project", "$(VCTargetsPath)\\Microsoft.Cpp.targets")));
            xElProject.Add(new XElement(ns + "ImportGroup", new XAttribute("Label", "ExtensionTargets"), ""));
            xElProject.Save(docName);
        }

        private void VC_Filters_Create(string docName, string[] targets)
        {
            if (docName == "")
            {
                return;
            }
            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
            var xelement = new XElement(ns + "Project", new XAttribute("DefaultTargets", "Build"), new XAttribute("ToolsVersion", "4.0"));
            var xElItemGroup1 = new XElement(ns + "ItemGroup", "");
            const string sourceFiles = "资源文件";
            xElItemGroup1.Add(new XElement(ns + "Filter", new XAttribute("Include", sourceFiles), new XElement(ns + "UniqueIdentifier", Guid.NewGuid().ToString("B")), new XElement(ns + "Extensions", "cpp;c;cc;cxx;def;odl;idl;hpj;bat;asm;asmx")), new XElement(ns + "Filter", new XAttribute("Include", "include"), new XElement(ns + "UniqueIdentifier", Guid.NewGuid().ToString("B")), new XElement(ns + "Extensions", "h;hpp;hxx;hm;inl;inc;xsd")), new XElement(ns + "Filter", new XAttribute("Include", "项目说明"), new XElement(ns + "UniqueIdentifier", Guid.NewGuid().ToString("B")), new XElement(ns + "Extensions", "txt")));
            string[] groups = MDK_GroupRead(_projectIno.MdkProjectFile, _projectIno.MdkTarget);
            foreach (var str in groups)
            {
                xElItemGroup1.Add(new XElement(ns + "Filter", new XAttribute("Include", sourceFiles + "\\" + str), new XElement(ns + "UniqueIdentifier", Guid.NewGuid().ToString("B"))));
            }
            xelement.Add(xElItemGroup1);
            xElItemGroup1 = new XElement(ns + "ItemGroup", "");
            var xElItemGroup2 = new XElement(ns + "ItemGroup", "");
            foreach (var group in groups)
            {
                string[] srcInThisGroup = MDK_SrcRead(_projectIno.MdkProjectFile, targets[0], group);
                foreach (var srcPath in srcInThisGroup)
                {
                    var srFullPath = GetFullPath(_projectIno.MdkProjectPath, srcPath);
                    srFullPath = GetRelativePath(_projectIno.VcProjectPath, srFullPath);
                    if (srFullPath.EndsWith(".c"))
                    {
                        xElItemGroup2.Add(new XElement(ns + "ClCompile",
                            new XAttribute("Include", srFullPath),
                            new XElement(ns + "Filter", sourceFiles + "\\" + group)));
                    }
                    else
                    {
                        xElItemGroup1.Add(new XElement(ns + "None",
                            new XAttribute("Include", srFullPath),
                            new XElement(ns + "Filter", sourceFiles + "\\" + group)));
                    }
                }
            }
            xElItemGroup1.Add(new XElement(ns + "None",
                new XAttribute("Include", "Readme.txt"),
                new XElement(ns + "Filter", sourceFiles)));
            xelement.Add(xElItemGroup1);
            xelement.Add(xElItemGroup2);
            xelement.Save(docName);
        }
        private void VC_Create_UserFile(string docName, string Debugcmd, string workingDirectory, string[] targets)
        {
            if (docName == "")
            {
                return;
            }
            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
            var xEl_proj = new XElement(ns + "Project", new XAttribute("ToolsVersion", "4.0"));
            foreach (var newValue in targets)
            {
                xEl_proj.Add(new XElement(ns + "PropertyGroup", new XAttribute("Condition", "'$(Configuration)|$(Platform)'=='Target|Win32'".Replace("Target", newValue)), new XElement(ns + "LocalDebuggerCommand", _projectIno.UV4Path), new XElement(ns + "LocalDebuggerCommandArguments", Debugcmd.Replace("Target", newValue)), new XElement(ns + "LocalDebuggerWorkingDirectory", workingDirectory), new XElement(ns + "DebuggerFlavor", "WindowsLocalDebugger")));
            }
            xEl_proj.Save(docName);
        }

        private void VC_Creat_readme(string docName, string projectName)
        {
            var stringBuilder = new StringBuilder();
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
            stringBuilder.Append("的项目文件：Template.uvproj 中的设定，按照Visual Studio 2017\r\n");
            stringBuilder.Append("中VC++ “生成文件项目” 的模板文件来生成的，如有疑问，请看MSDN~\r\n");
            stringBuilder.Append("\r\n");
            DateTime dt = DateTime.Now;
            stringBuilder.Append(dt + "\r\n");
            stringBuilder.Append(_preStr.Author + "\r\n");
            stringBuilder.Append("/////////////////////////////////////////////////////////////////////////////\r\n");
            stringBuilder = stringBuilder.Replace("Template", projectName);
            FileStream fileStream = File.OpenWrite(docName);
            byte[] bytes = new UTF8Encoding(true).GetBytes(stringBuilder.ToString());
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
        }

        private static void VC_Creat_Sln(string docName, string projectName, string[] targets)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("Microsoft Visual Studio Solution File, Format Version 12.00\r\n");
            stringBuilder.Append("# Visual Studio 15\r\n");
            stringBuilder.Append("VisualStudioVersion = 15.0.27703.2018\r\n");
            stringBuilder.Append("MinimumVisualStudioVersion = 10.0.40219.1\r\n");
            stringBuilder.Append("Project(\"{" + Guid.NewGuid().ToString("B") + "}\") = \"Template\", \"Template.vcxproj\", \"{" + Guid.NewGuid().ToString("B") + "}\"");
            stringBuilder.Append("\r\nEndProject\r\n");
            stringBuilder.Append("Global\r\n");
            stringBuilder.Append("\tGlobalSection(SolutionConfigurationPlatforms) = preSolution\r\n");
            foreach (var newValue in targets)
            {
                var text = "\t\tDebug|Win32 = Debug|Win32\r\n";
                text = text.Replace("Debug", newValue);
                stringBuilder.Append(text);
            }
            stringBuilder.Append("\tEndGlobalSection\r\n");
            stringBuilder.Append("\tGlobalSection(ProjectConfigurationPlatforms) = postSolution\r\n");
            var newValue2 = Guid.NewGuid().ToString("B");
            foreach (var newValue3 in targets)
            {
                var text2 = "\t\tGUID.Debug|Win32.ActiveCfg = Debug|Win32\r\n";
                var text3 = "\t\tGUID.Debug|Win32.Build.0 = Debug|Win32\r\n";
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
            stringBuilder = stringBuilder.Replace("Template", projectName);
            FileStream fileStream = File.OpenWrite(docName);
            byte[] bytes = new UTF8Encoding(true).GetBytes(stringBuilder.ToString());
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
            stringBuilder.Clear();
        }
        bool fixVSPath = true;
        private void CreateButton_Click(object sender, EventArgs e)
        {
            if (cboxbatch.Visible && cboxbatch.Checked)
            {

            }
            if (!fixVSPath)
            {
                var folderBrowserDialog = new FolderBrowserDialog
                {
                    //RootFolder = Environment.SpecialFolder.MyComputer,
                    Description = Resources.vsp_path,
                    SelectedPath = _projectIno.MdkProjectPath
                };
                if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;
                _projectIno.VcProjectPath = Path.Combine(new[]
                {
                folderBrowserDialog.SelectedPath,
                "VS"
                }) + "\\";

            }
            else
            {
                _projectIno.VcProjectPath = _projectIno.MdkProjectPath + "VS\\";
            }

            if (!Directory.Exists(_projectIno.VcProjectPath))
            {
                Directory.CreateDirectory(_projectIno.VcProjectPath);
            }
            string[] targets = MDK_TargetRead(_projectIno.MdkProjectFile);
            var relativePath = GetRelativePath(_projectIno.VcProjectPath, _projectIno.MdkProjectFile);
            #region BUILD PART
            _projectIno.NMakeBuildCommandLine = string.Concat(_projectIno.UV4Path, " -b ", _projectIno.MdkProjectFile);
            //_projectIno.NMakeBuildCommandLine = string.Concat(_projectIno.UV4Path, " -b ", _projectIno.MdkProjectFile, " -t \"Target\" -j0 -o Build.log");
            //_projectIno.NMakeBuildCommandLine = string.Concat(_projectIno.UV4Path, " -b ", relativePath, " -t \"Target\" -j0 -o Build.log");
            _projectIno.LocalDebuggerCommandArguments = "-d " + _projectIno.ProjectName + ".uvproj -t \"Target\"";

            #endregion


            var docName = _projectIno.VcProjectPath + _projectIno.ProjectName + ".sln";
            VC_Creat_Sln(docName, _projectIno.ProjectName, targets);

            docName = _projectIno.VcProjectPath + _projectIno.VcFiltersName;
            VC_Filters_Create(docName, targets);

            docName = _projectIno.VcProjectPath + _projectIno.VcxprojName;
            VC_vcxproj_Create(docName, targets);

            _projectIno.LocalDebuggerWorkingDirectory = GetRelativePath(_projectIno.VcProjectPath, _projectIno.MdkProjectPath);

            docName = _projectIno.VcProjectPath + _projectIno.VcUserFileName;
            VC_Create_UserFile(docName, _projectIno.LocalDebuggerCommandArguments, _projectIno.LocalDebuggerWorkingDirectory, targets);

            docName = _projectIno.VcProjectPath + "readme.txt";
            VC_Creat_readme(docName, _projectIno.ProjectName);


            docName = _projectIno.VcProjectPath + _projectIno.ProjectName + ".sln";

            DialogResult dr = MessageBox.Show(
                Resources.sueecssTip1 + "\r\n" +
                Resources.sueecssTip2 + "\r\n" +
                Resources.sueecssTip3 + "\r\n" +
                Resources.sueecssTip4,
                Resources.enjoy_vs,
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Information);
            var baseUri = new Uri(docName);
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
                        MessageBox.Show("无法打开当前VS项目！\n请确任安装好Visual Studio 2017+!", "很遗憾!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    break;
                case DialogResult.No:
                    Process.Start(_projectIno.VcProjectPath);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private bool TryGetSoftwarePath(ref string path)
        {
            var strPathResult = string.Empty;
            //string strKeyName = "";     //"(Default)" key, which contains the intalled path 
            object objResult = null;

            RegistryKey regKey = null;
            RegistryKey regSubKey = null;

            try
            {
                //Read the key 
                regKey = Registry.LocalMachine;
                regSubKey = regKey.OpenSubKey(_preStr.RegPath, false);

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
            catch (SecurityException ex)
            {
                throw new SecurityException("You have no right to read the registry!", ex);
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

            //Not found 
            path = null;
            return false;
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
            _config.PreDefine = tb_predef.Text;
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
            tb_predef.Text = _preStr.Predefine;
        }
        private XDocument document = new XDocument();
        private XmlDocument xmlDoc = new XmlDocument();
        private FileInfo[] _fileInfos;
        private ProjectInfo _projectIno;
        private Config _config;
        private UprojInfo[] _uprojInfo;

        private readonly PreStr _preStr = new PreStr
        {
            ToolsVersion = "0.3",
            ApplicationStartpath = Path.GetDirectoryName(Application.ExecutablePath),
            Configfilename = "Config.xml",
            FristUse = "Select the Keil Vision4 installation directory when you first times use the tool",
            PredefineKeil = @"sbit=;sfr=;data=;xdata=;code=;idata=;pdata=;_at_=#;",
            Predefine = @"VS;",
            RegPath = @"SOFTWARE\WOW6432Node\Keil\Products\C51\",
            NotUvProj = "\nRead File Error! Maybe it is't MDK Project File",
            Notkeilexe = "This is NOT KEIL PROGRAM!!\n",
            Sourepretext = "Open Or drop The MDK the Project File to here!",
            NameSp = "MAILTO:heartacker@outlook.com",
            Author = "heartacker",
            NoKeil = "Can not detect keil Program!",
            FindUprojInThisFolder = "Oops!,Find uproject in this folder!\n Plz select one to convert!",
            Selectnewfolder = "Select new Folder"
        };
        struct UprojInfo
        {
            public string FileName;
            public string FileFullname;
        }

        private struct ProjectInfo
        {
            public string UV4Path;
            public string MdkProjectPath;
            public string MdkProjectFile;
            public string MdkTarget;
            public string ProjectName;
            public string IncludePath;
            public string VcProjectPath;
            public string VcxprojName;
            public string VcFiltersName;
            public string VcUserFileName;
            public string NMakePreprocessorDefinitions;
            public string NMakeBuildCommandLine;
            public string NMakeCleanCommandLine;
            public string LocalDebuggerCommandArguments;
            public string LocalDebuggerWorkingDirectory;
            public string CuruProjectFileDir;
        }
        private struct Config
        {
            public string ToolName;
            public string ToolsVersion;
            public string UV4Path;
            public string DocName;
            public string UV4IncPath;
            public string UV4LibPath;
            public string PreDefine;
        }

        private struct PreStr
        {
            public string Configfilename;
            public string FristUse;
            public string ToolsVersion;
            public string PredefineKeil;
            public string Predefine;
            public string NoKeil;
            public string RegPath;
            public string NotUvProj;
            public string Notkeilexe;
            public string Author;
            public string NameSp;
            public string FindUprojInThisFolder;
            public string Selectnewfolder;

            public string ApplicationStartpath { get; set; }
            public string Sourepretext { get; set; }
        }

        Fmdialog fmdia;

        private void SourcePathCBOX_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (SourcePathCBOX.SelectedIndex < 0)
            {
                return;
            }
            if (SourcePathCBOX.SelectedIndex >= SourcePathCBOX.Items.Count)
            {
                return;
            }

            TryDispuProjinfo(_uprojInfo[SourcePathCBOX.SelectedIndex].FileFullname);
        }

        private void ElementHost_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (_projectIno.CuruProjectFileDir == "") return;
            string[] array = MDK_TargetRead(_projectIno.CuruProjectFileDir);
            string[] array2 = MDK_GroupRead(_projectIno.CuruProjectFileDir, array[elementHost.SelectedIndex]);
            GroupListBox_Add(array2);
            string[] items = MDK_SrcRead(_projectIno.CuruProjectFileDir, array[elementHost.SelectedIndex], array2[0]);
            SrcFileBox_Add(items);
            var str = MDK_TargetStatusRead(_projectIno.CuruProjectFileDir, array[elementHost.SelectedIndex]);
            TargetStatusBox_Add(str);

        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                UpDateCurfolderUproj(sender, e);
            }
        }

        private void LinklabelUprojPath_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string targetUrl = e.Link.LinkData as string;
            if (string.IsNullOrEmpty(targetUrl))
            {
                return;
            }
            llbeUprojPath.Links[llbeUprojPath.Links.IndexOf(e.Link)].Visited = true;
            try
            {
                Process.Start(targetUrl);
            }
            catch
            {
                MessageBox.Show("无法打开当前keil项目！\n请确任安装好keil软件!", "很遗憾!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Cboxtrack_CheckedChanged(object sender, EventArgs e)
        {

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