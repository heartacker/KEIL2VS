using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
            ProjectIno.NMakeCleanCommandLine = "";
            CreateButton.Enabled = false;
            string[] array = {
                PreStr.ApplicationStartpath,
                PreStr.configfilename
            };

            Config.DocName = Path.Combine(array);
            tbKeil_path.Text = PreStr.noKeil;
            tb_predef.Text = PreStr.predefine;
            Config.PreDefine = tb_predef.Text;
        }


        private void Keil2VS_Shown(object sender, EventArgs e)
        {
            if (!TryGetSoftwarePath(ref Config.UV4Path))
            {
                ReadConfig(Config.DocName);
                ProjectIno.UV4_Path = Config.UV4Path + " ";
            }
            else
            {
                try
                {
                    File.Delete(Config.DocName);
                }
                catch
                {
                    // ignored
                }

                Config.UV4Path = Config.UV4Path.Replace("C51", @"UV4\UV4.exe");
                Config.UV4IncPath = Config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\INC");
                Config.UV4LibPath = Config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\LIB");
                tbKeil_path.BackColor = Color.LawnGreen;
                tbKeil_path.Text = Config.UV4Path;
                tbKeil_path.AllowDrop = false;
            }
            UpDateCurfolderUproj(sender, e);

        }


        private void UpDateCurfolderUproj(object sender, EventArgs e)
        {
            if (!ScanCurrentFolderHas_uProj(PreStr.ApplicationStartpath, ref fileInfos))
                return;

            uprojInfo = new _uprojInfo[fileInfos.GetLength(0)];
            for (var i = 0; i < fileInfos.GetLength(0); i++)
            {
                uprojInfo[i].fileName = fileInfos[i].Name;
                uprojInfo[i].fileFullname = fileInfos[i].FullName;
            }
            SourcePathComobox_add(uprojInfo);
            sourcepathTip.Show(PreStr.FindUprojInThisFolder, SourcePathCBOX, SourcePathCBOX.Location.X + 200, SourcePathCBOX.Top - 100, 1000 * 10);
            TryDispuProjinfo(uprojInfo[0].fileFullname);
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


        private void SourcePathComobox_add(_uprojInfo[] it)
        {
            SourcePathCBOX.Items.Clear();
            for (var i = 0; i < it.GetLength(0); i++)
            {
                SourcePathCBOX.Items.Add(it[i].fileFullname);
            }
            //this.SourcePathCBOX.Items.Add(this.PreStr.selectnewfolder);
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
            if (ProjectIno.CuruProjectFileDir == "") return;
            string[] array = MDK_TargetRead(ProjectIno.CuruProjectFileDir);
            string[] array2 = MDK_GroupRead(ProjectIno.CuruProjectFileDir, array[elementHost.SelectedIndex]);
            string[] items = MDK_SrcRead(ProjectIno.CuruProjectFileDir, array[elementHost.SelectedIndex], array2[GroupListBox.SelectedIndex]);
            SrcFileBox_Add(items);
        }


        private void FileBox_DoubleClick(object sander, EventArgs e)
        {
            if (FileBox.SelectedIndex < 0)
            {
                return;
            }
            var text = FileBox.SelectedItem.ToString();
            text = GetFullPath(ProjectIno.MDK_Project_Path, text);
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
            uprojInfo = new _uprojInfo[1];
            uprojInfo[0].fileName = ProjectIno.ProjectName;
            uprojInfo[0].fileFullname = ProjectIno.MDK_Project_File;

            SourcePathCBOX.Text = uprojInfo[0].fileFullname;
            SourcePathComobox_add(uprojInfo);

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
            uprojInfo = new _uprojInfo[1];
            uprojInfo[0].fileName = ProjectIno.ProjectName;
            uprojInfo[0].fileFullname = ProjectIno.MDK_Project_File;
            SourcePathCBOX.Text = uprojInfo[0].fileFullname;
            SourcePathComobox_add(uprojInfo);
        }
        private void TryDispuProjinfo(string fileFullname)
        {
            if (!MDK_Display_Info(fileFullname)) return;
            ProjectIno.CuruProjectFileDir = fileFullname;
            ProjectIno.MDK_Project_File = fileFullname;
            ProjectIno.MDK_Project_Path = Path.GetDirectoryName(fileFullname) + "\\";
            ProjectIno.ProjectName = Path.GetFileNameWithoutExtension(fileFullname);
            ProjectIno.VcxprojName = ProjectIno.ProjectName + ".vcxproj";
            ProjectIno.VC_Filters_Name = ProjectIno.VcxprojName + ".filters";
            ProjectIno.VC_UserFileName = ProjectIno.VcxprojName + ".user";
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

                Config.UV4Path = openFileDialog.FileName;
                Config.UV4IncPath = Config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\INC");
                Config.UV4LibPath = Config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\LIB");
                Creat_Config(Config.DocName);
                tbKeil_path.Text = Config.UV4Path;
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
                Config.UV4Path = text;
                Config.UV4IncPath = Config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\INC");
                Config.UV4LibPath = Config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\LIB");
                Creat_Config(Config.DocName);
                return;
            }
            MessageBox.Show(PreStr.notkeilexe + "\nthis is" + tbKeil_path.Text);
            tbKeil_path.Text = path;
            tbKeil_path.ForeColor = Color.Gray;
        }
        // Token: 0x0600000D RID: 13 RVA: 0x00002740 File Offset: 0x00000940
        private void Creat_Config(string DocName)
        {
            if (DocName == "")
            {
                return;
            }
            Config.ToolName = "Keil2VisualStudio";
            Config.ToolsVersion = PreStr.ToolsVersion;
            XNamespace ns = PreStr.nameSp;
            var xelement = new XElement(ns + "Tool",
                new XAttribute("Name", Config.ToolName),
                new XAttribute("Author", PreStr.author),
                new XAttribute("ToolsVersion", Config.ToolsVersion),
                new XElement(ns + "UV4Path", Config.UV4Path),
                new XElement(ns + "UV4IncPath", Config.UV4IncPath),
                new XElement(ns + "UV4LibPath", Config.UV4LibPath)
                );
            xelement.Save(DocName);
        }

        // Token: 0x0600000E RID: 14 RVA: 0x00002818 File Offset: 0x00000A18
        private void ReadConfig(string docName)
        {
            var temp = File.Exists(docName);
            Color c = tbKeil_path.BackColor;
            var t = tbKeil_path.Text;

            if (!temp)
            {
                tbKeil_path.BackColor = Color.LightPink;
                MessageBox.Show(PreStr.fristUse);
                var openFileDialog = new OpenFileDialog
                {
                    DefaultExt = "exe",
                    Filter = @"Keil Exe File (UV4.exe)|UV4.exe",
                    Title = Resources.SELUV4PATH
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Config.UV4Path = openFileDialog.FileName;
                    Config.UV4IncPath = Config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\INC");
                    Config.UV4LibPath = Config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\LIB");
                    Creat_Config(Config.DocName);
                    tbKeil_path.BackColor = Color.LightGreen;
                    tbKeil_path.Text = Config.UV4Path;
                }
                else
                {
                    tbKeil_path.Text = t;
                }
            }
            else
            {
                XNamespace ns = PreStr.nameSp;
                XDocument xdocument = XDocument.Load(docName);
                if (xdocument.Root != null)
                {
                    Config.ToolsVersion = xdocument.Root.Attribute("ToolsVersion")?.Value;
                    Config.UV4Path = xdocument.Root.Element(ns + "UV4Path")?.Value;
                    Config.UV4IncPath = xdocument.Root.Element(ns + "UV4IncPath").Value;
                    Config.UV4LibPath = xdocument.Root.Element(ns + "UV4LibPath").Value;
                }

                tbKeil_path.BackColor = Color.LightGreen;
                tbKeil_path.Text = Config.UV4Path;
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
        private string MDK_DefineRead(string Doc, string TargetName)
        {
            if (Doc == "")
            {
                return "";
            }
            if (TargetName == "")
            {
                return "";
            }
            xmlDoc.Load(Doc);
            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(".//Targets/Target");
            foreach (object obj in xmlNodeList)
            {
                var xmlNode = (XmlNode)obj;
                if (xmlNode.SelectSingleNode("./TargetName").InnerText != TargetName) continue;
                var defineString = xmlNode.SelectSingleNode(".//VariousControls/Define").InnerText;
                if (!defineString.EndsWith(","))
                {
                    defineString += ";";
                }
                defineString = "".Replace(',', ';');
                return "";
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
                if (xmlNode.SelectSingleNode("./TargetName").InnerText == targetName)
                {
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
            xmlDoc.Load(Doc);
            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(".//Targets/Target");
            foreach (object obj in xmlNodeList)
            {
                var xmlNode = (XmlNode)obj;
                if (xmlNode.SelectSingleNode("./TargetName").InnerText == TargetName)
                {
                    XmlNodeList xmlNodeList2 = xmlNode.SelectNodes(".//Groups/Group");
                    foreach (object obj2 in xmlNodeList2)
                    {
                        var xmlNode2 = (XmlNode)obj2;
                        if (xmlNode2.SelectSingleNode("./GroupName").InnerText == Group)
                        {
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
            bool isuproj;
            try
            {
                xmlDoc.Load(DocName);
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
                MessageBox.Show(DocName + PreStr.notUvProj, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
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
                ProjectIno.MDK_Target = targetArray[0];
                ProjectIno.IncludePath = MDK_IncludePathRead(docName, targetArray[0]);
                ProjectIno.IncludePath += Config.UV4IncPath;
                ProjectIno.NMakePreprocessorDefinitions = MDK_DefineRead(docName, targetArray[0]);
                string[] GroupArray = MDK_GroupRead(docName, ProjectIno.MDK_Target);
                GroupListBox_Add(GroupArray);
                string[] items = MDK_SrcRead(docName, targetArray[0], GroupArray[0]);
                SrcFileBox_Add(items);
                var str = MDK_TargetStatusRead(docName, targetArray[0]);
                TargetStatusBox_Add(str);
                CreateButton.Enabled = true;
                result = true;
            }
            catch
            {
                MessageBox.Show(PreStr.notUvProj, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                result = false;
            }
            return result;
        }
        private void VC_vcxproj_Create(string docName, string[] targets)
        {
            if (docName == "")
            {
                return;
            }
            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
            var xEl_Project = new XElement(ns + "Project", new XAttribute("DefaultTargets", "Build"), new XAttribute("ToolsVersion", "15.0"));
            var xEl_ItemGroup = new XElement(ns + "ItemGroup", new XAttribute("Label", "ProjectConfigurations"));
            foreach (var targetName in targets)
            {
                xEl_ItemGroup.Add(new XElement(ns + "ProjectConfiguration", new XAttribute("Include", "Target|Win32".Replace("Target", targetName)), new XElement(ns + "Configuration", targetName), new XElement(ns + "Platform", "Win32")));
            }
            xEl_Project.Add(xEl_ItemGroup);
            var xEl_PropertyGroup = new XElement(ns + "PropertyGroup", new XAttribute("Label", "Globals"), new XElement(ns + "ProjectGuid", Guid.NewGuid().ToString("B")), new XElement(ns + "Keyword", "MakeFileProj"), new XElement(ns + "WindowsTargetPlatformVersion", "10.0.17134.0"));
            xEl_Project.Add(xEl_PropertyGroup);
            xEl_Project.Add(new XElement(ns + "Import", new XAttribute("Project", "$(VCTargetsPath)\\Microsoft.Cpp.Default.props")));
            foreach (var newValue in targets)
            {
                xEl_Project.Add(new XElement(ns + "PropertyGroup", new XAttribute("Condition", "'$(Configuration)|$(Platform)'=='Target|Win32'".Replace("Target", newValue)), new XAttribute("Label", "Configuration"), new XElement(ns + "ConfigurationType", "Makefile"), new XElement(ns + "UseDebugLibraries", "true"), new XElement(ns + "PlatformToolset", "v141")));
            }
            xEl_Project.Add(new XElement(ns + "Import", new XAttribute("Project", "$(VCTargetsPath)\\Microsoft.Cpp.props")), new XElement(ns + "ImportGroup", new XAttribute("Label", "ExtensionSettings")));
            foreach (var newValue2 in targets)
            {
                xEl_Project.Add(new XElement(ns + "ImportGroup", new XAttribute("Condition", "'$(Configuration)|$(Platform)'=='Target|Win32'".Replace("Target", newValue2)), new XAttribute("Label", "PropertySheets"), new XElement(ns + "Import", new XAttribute("Project", "$(UserRootDir)\\Microsoft.Cpp.$(Platform).user.props"), new XAttribute("Condition", "exists('$(UserRootDir)\\Microsoft.Cpp.$(Platform).user.props')"), new XAttribute("Label", "LocalAppDataPlatform"))));
            }
            xEl_Project.Add(new XElement(ns + "PropertyGroup", new XAttribute("Label", "UserMacros")));
            foreach (var oneTarget in targets)
            {
                var includepathTemp = MDK_IncludePathRead(ProjectIno.MDK_Project_File, oneTarget);
                string[] array = includepathTemp.Split(';');
                string keilIncludePath = null;
                for (var m = 0; m < array.Length; m++)
                {
                    var fullPath = GetFullPath(ProjectIno.MDK_Project_Path, array[m]);
                    keilIncludePath = keilIncludePath + GetRelativePath(ProjectIno.VCProject_Path, fullPath) + ";";
                }
                keilIncludePath += Config.UV4IncPath + ";";

                xEl_Project.Add(new XElement(ns + "PropertyGroup", new XAttribute("Condition", "'$(Configuration)|$(Platform)'=='Target|Win32'".Replace("Target", oneTarget)), new XElement(ns + "NMakeOutput", "Template.bin".Replace("Template", ProjectIno.ProjectName)), new XElement(ns + "NMakePreprocessorDefinitions",PreStr.predefineKeil+Config.PreDefine+ MDK_DefineRead(ProjectIno.MDK_Project_File, oneTarget)), new XElement(ns + "IncludePath", keilIncludePath), new XElement(ns + "NMakeBuildCommandLine", ProjectIno.NMakeBuildCommandLine.Replace("Target", oneTarget)), new XElement(ns + "LibraryPath",@"$(VC_LibraryPath_x86);$(WindowsSDK_LibraryPath_x86);$(NETFXKitsDir)Lib\um\x86;"+Config.UV4LibPath+";")));
            }
            xEl_Project.Add(new XElement(ns + "ItemDefinitionGroup", ""));
            string[] array2 = MDK_GroupRead(ProjectIno.MDK_Project_File, targets[0]);
            var xElNone = new XElement(ns + "ItemGroup", "");
            var xElClCompile = new XElement(ns + "ItemGroup", "");
            foreach (var group in array2)
            {
                string[] array4 = MDK_SrcRead(ProjectIno.MDK_Project_File, targets[0], group);
                foreach (var targetPat in array4)
                {
                    var text5 = GetFullPath(ProjectIno.MDK_Project_Path, targetPat);
                    text5 = GetRelativePath(ProjectIno.VCProject_Path, text5);
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
            xEl_Project.Add(xElNone);
            xEl_Project.Add(xElClCompile);
            xEl_Project.Add(new XElement(ns + "Import", new XAttribute("Project", "$(VCTargetsPath)\\Microsoft.Cpp.targets")));
            xEl_Project.Add(new XElement(ns + "ImportGroup", new XAttribute("Label", "ExtensionTargets"), ""));
            xEl_Project.Save(docName);
        }

        private void VC_Filters_Create(string docName, string[] targets)
        {
            if (docName == "")
            {
                return;
            }
            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
            var xelement = new XElement(ns + "Project", new XAttribute("DefaultTargets", "Build"), new XAttribute("ToolsVersion", "4.0"));
            var xElItemGroup = new XElement(ns + "ItemGroup", "");
            var sourceFiles = "资源文件";
            xElItemGroup.Add(new XElement(ns + "Filter", new XAttribute("Include", sourceFiles), new XElement(ns + "UniqueIdentifier", Guid.NewGuid().ToString("B")), new XElement(ns + "Extensions", "cpp;c;cc;cxx;def;odl;idl;hpj;bat;asm;asmx")), new XElement(ns + "Filter", new XAttribute("Include", "include"), new XElement(ns + "UniqueIdentifier", Guid.NewGuid().ToString("B")), new XElement(ns + "Extensions", "h;hpp;hxx;hm;inl;inc;xsd")), new XElement(ns + "Filter", new XAttribute("Include", "项目说明"), new XElement(ns + "UniqueIdentifier", Guid.NewGuid().ToString("B")), new XElement(ns + "Extensions", "txt")));
            string[] groups = MDK_GroupRead(ProjectIno.MDK_Project_File, ProjectIno.MDK_Target);
            foreach (var str in groups)
            {
                xElItemGroup.Add(new XElement(ns + "Filter", new XAttribute("Include", sourceFiles + "\\" + str), new XElement(ns + "UniqueIdentifier", Guid.NewGuid().ToString("B"))));
            }
            xelement.Add(xElItemGroup);
            xElItemGroup = new XElement(ns + "ItemGroup", "");
            var xEl_itemGroup = new XElement(ns + "ItemGroup", "");
            foreach (var group in groups)
            {
                string[] src_inThisGroup = MDK_SrcRead(ProjectIno.MDK_Project_File, targets[0], group);
                foreach (var src_path in src_inThisGroup)
                {
                    var sr_Full_path = GetFullPath(ProjectIno.MDK_Project_Path, src_path);
                    sr_Full_path = GetRelativePath(ProjectIno.VCProject_Path, sr_Full_path);
                    if (sr_Full_path.EndsWith(".c"))
                    {
                        xEl_itemGroup.Add(new XElement(ns + "ClCompile", new XAttribute("Include", sr_Full_path), new XElement(ns + "Filter", sourceFiles +"\\" + group)));
                    }
                    else
                    {
                        xElItemGroup.Add(new XElement(ns + "None", new XAttribute("Include", sr_Full_path), new XElement(ns + "Filter", sourceFiles+"\\" + group)));
                    }
                }
            }
            xElItemGroup.Add(new XElement(ns + "None", new XAttribute("Include", "Readme.txt"), new XElement(ns + "Filter", sourceFiles)));
            xelement.Add(xEl_itemGroup);
            xelement.Add(xElItemGroup);
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
                xEl_proj.Add(new XElement(ns + "PropertyGroup", new XAttribute("Condition", "'$(Configuration)|$(Platform)'=='Target|Win32'".Replace("Target", newValue)), new XElement(ns + "LocalDebuggerCommand", ProjectIno.UV4_Path), new XElement(ns + "LocalDebuggerCommandArguments", Debugcmd.Replace("Target", newValue)), new XElement(ns + "LocalDebuggerWorkingDirectory", workingDirectory), new XElement(ns + "DebuggerFlavor", "WindowsLocalDebugger")));
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
            stringBuilder.Append(PreStr.author + "\r\n");
            stringBuilder.Append("/////////////////////////////////////////////////////////////////////////////\r\n");
            stringBuilder = stringBuilder.Replace("Template", projectName);
            FileStream fileStream = File.OpenWrite(docName);
            byte[] bytes = new UTF8Encoding(true).GetBytes(stringBuilder.ToString());
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
        }

        private void VC_Creat_Sln(string DocName, string ProjectName, string[] Targets)
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
            foreach (var newValue in Targets)
            {
                var text = "\t\tDebug|Win32 = Debug|Win32\r\n";
                text = text.Replace("Debug", newValue);
                stringBuilder.Append(text);
            }
            stringBuilder.Append("\tEndGlobalSection\r\n");
            stringBuilder.Append("\tGlobalSection(ProjectConfigurationPlatforms) = postSolution\r\n");
            var newValue2 = Guid.NewGuid().ToString("B");
            foreach (var newValue3 in Targets)
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
            stringBuilder = stringBuilder.Replace("Template", ProjectName);
            FileStream fileStream = File.OpenWrite(DocName);
            byte[] bytes = new UTF8Encoding(true).GetBytes(stringBuilder.ToString());
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
            stringBuilder.Clear();
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog
            {
                //RootFolder = Environment.SpecialFolder.MyComputer,
                Description = "Please select the Visual Studio Project Path",
                SelectedPath = ProjectIno.MDK_Project_Path
            };
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                ProjectIno.VCProject_Path = Path.Combine(new[]
                {
                    folderBrowserDialog.SelectedPath,
                    "VS"
                }) + "\\";
                if (!Directory.Exists(ProjectIno.VCProject_Path))
                {
                    Directory.CreateDirectory(ProjectIno.VCProject_Path);
                }
                string[] targets = MDK_TargetRead(ProjectIno.MDK_Project_File);
                var relativePath = GetRelativePath(ProjectIno.VCProject_Path, ProjectIno.MDK_Project_File);
                ProjectIno.NMakeBuildCommandLine = string.Concat("\"", ProjectIno.UV4_Path, "\" -b ", relativePath, " -t \"Target\" -j0 -o Build.log");
                ProjectIno.LocalDebuggerCommandArguments = "-d " + ProjectIno.ProjectName + ".uvproj -t \"Target\"";

                var docName = ProjectIno.VCProject_Path + ProjectIno.ProjectName + ".sln";
                VC_Creat_Sln(docName, ProjectIno.ProjectName, targets);

                docName = ProjectIno.VCProject_Path + ProjectIno.VC_Filters_Name;
                VC_Filters_Create(docName, targets);

                docName = ProjectIno.VCProject_Path + ProjectIno.VcxprojName;
                VC_vcxproj_Create(docName, targets);

                ProjectIno.LocalDebuggerWorkingDirectory = GetRelativePath(ProjectIno.VCProject_Path, ProjectIno.MDK_Project_Path);

                docName = ProjectIno.VCProject_Path + ProjectIno.VC_UserFileName;
                VC_Create_UserFile(docName, ProjectIno.LocalDebuggerCommandArguments, ProjectIno.LocalDebuggerWorkingDirectory, targets);

                docName = ProjectIno.VCProject_Path + "readme.txt";
                VC_Creat_readme(docName, ProjectIno.ProjectName);


                docName = ProjectIno.VCProject_Path + ProjectIno.ProjectName + ".sln";

                DialogResult dr = MessageBox.Show(
                    Resources.sueecssTip, "Enjoy VS Coding!",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
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
                        }

                        break;
                    case DialogResult.No:
                        Process.Start(ProjectIno.VCProject_Path);
                        break;
                }
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
                regSubKey = regKey.OpenSubKey(PreStr.regPath, false);

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
            Config.PreDefine = tb_predef.Text;
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
            tb_predef.Text = PreStr.predefine;
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
            selectnewfolder = "Select new Folder"
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

            public string ApplicationStartpath { get; set; }
        }

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

            TryDispuProjinfo(uprojInfo[SourcePathCBOX.SelectedIndex].fileFullname);
        }

        private void ElementHost_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (ProjectIno.CuruProjectFileDir == "") return;
            string[] array = MDK_TargetRead(ProjectIno.CuruProjectFileDir);
            string[] array2 = MDK_GroupRead(ProjectIno.CuruProjectFileDir, array[elementHost.SelectedIndex]);
            GroupListBox_Add(array2);
            string[] items = MDK_SrcRead(ProjectIno.CuruProjectFileDir, array[elementHost.SelectedIndex], array2[0]);
            SrcFileBox_Add(items);
            var str = MDK_TargetStatusRead(ProjectIno.CuruProjectFileDir, array[elementHost.SelectedIndex]);
            TargetStatusBox_Add(str);

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