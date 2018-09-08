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

using KEIL2VS.ProjGen;

namespace KEIL2VS
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        Keil2vsGen VsGen = new Keil2vsGen();
        private FileInfo[] fileInfos;
        private Infos.ProjectInfo projectIno;
        private Infos.Config config;
        private Infos.UprojInfo[] uprojInfo;

        Fmdialog fmdia;

        private Infos.PreStr preStr = new Infos.PreStr
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
            projectIno.NMakeCleanCommandLine = "";
            CreateButton.Enabled = false;
            string[] array = {
                preStr.ApplicationStartpath,
                preStr.Configfilename
            };

            config.DocName = Path.Combine(array);
            tbKeil_path.Text = preStr.NoKeil;
            tb_predef.Text = preStr.Predefine;
            config.PreDefine = tb_predef.Text;
        }


        private void Keil2VS_Shown(object sender, EventArgs e)
        {
            if (!TryGetUV4Path(ref config.UV4Path))
            {
                ReadConfig(config.DocName);
            }
            else
            {
                try
                {
                    File.Delete(config.DocName);
                }
                catch
                {
                    // ignored
                }

                config.UV4Path = config.UV4Path.Replace("C51", @"UV4\UV4.exe");
                config.UV4IncPath = config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\INC");
                config.UV4LibPath = config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\LIB");
                tbKeil_path.BackColor = Color.LawnGreen;
                tbKeil_path.Text = config.UV4Path;
                tbKeil_path.AllowDrop = false;
            }
            #region BUILD PART 0
            projectIno.UV4Path = config.UV4Path + " ";
            //_projectIno.UV4Path = "UV4";
            #endregion
            UpDateCurfolderUproj(sender, e);

        }


        private void UpDateCurfolderUproj(object sender, EventArgs e)
        {
            string scanpath = preStr.ApplicationStartpath;
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

            if (!ScanCurrentFolderHas_uProj(scanpath, ref fileInfos))
                return;

            uprojInfo = new Infos.UprojInfo[fileInfos.GetLength(0)];
            for (var i = 0; i < fileInfos.GetLength(0); i++)
            {
                uprojInfo[i].FileName = fileInfos[i].Name;
                uprojInfo[i].FileFullname = fileInfos[i].FullName;
            }
            SourcePathComobox_add(uprojInfo);
            //sourcepathTip.Show(_preStr.FindUprojInThisFolder, SourcePathCBOX, SourcePathCBOX.Location.X + 200, SourcePathCBOX.Top - 100, 1000 * 10);
            TryDispuProjinfo(uprojInfo[0].FileFullname);
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


        private void SourcePathComobox_add(Infos.UprojInfo[] it)
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
            if (projectIno.CuruProjectFileDir == "") return;
            string[] array = VsGen.MDK_TargetRead(projectIno.CuruProjectFileDir);
            string[] array2 = VsGen.MDK_GroupRead(projectIno.CuruProjectFileDir, array[elementHost.SelectedIndex]);
            string[] items = VsGen.MDK_SrcRead(projectIno.CuruProjectFileDir, array[elementHost.SelectedIndex], array2[GroupListBox.SelectedIndex]);
            SrcFileBox_Add(items);
        }


        private void FileBox_DoubleClick(object sander, EventArgs e)
        {
            if (FileBox.SelectedIndex < 0)
            {
                return;
            }
            var text = FileBox.SelectedItem.ToString();
            text = VsGen.GetFullPath(projectIno.MdkProjectPath, text);
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

            if (!VsGen.MDK_CheckProject(fileName, ref preStr)) return;

            TryDispuProjinfo(fileName);
            uprojInfo = new Infos.UprojInfo[1];
            uprojInfo[0].FileName = projectIno.ProjectName;
            uprojInfo[0].FileFullname = projectIno.MdkProjectFile;
            SourcePathCBOX.Text = uprojInfo[0].FileFullname;
            SourcePathComobox_add(uprojInfo);

        }

        private void SourcePathCBOX_DragDrop(object sender, DragEventArgs e)
        {
            SourcePathCBOX.ForeColor = Color.Black;
            var dropFilename = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            if (!VsGen.MDK_CheckProject(dropFilename, ref preStr))
            {
                return;
            }

            TryDispuProjinfo(dropFilename);
            uprojInfo = new Infos.UprojInfo[1];
            uprojInfo[0].FileName = projectIno.ProjectName;
            uprojInfo[0].FileFullname = projectIno.MdkProjectFile;
            SourcePathCBOX.Text = uprojInfo[0].FileFullname;
            SourcePathComobox_add(uprojInfo);
        }

        public bool MDK_Display_Info(string docName)
        {
            bool result;
            try
            {
                string[] targetArray = VsGen.MDK_TargetRead(docName);
                ElementHost_Add(targetArray);
                projectIno.MdkTarget = targetArray[0];
                projectIno.IncludePath = VsGen.MDK_IncludePathRead(docName, targetArray[0]);
                projectIno.IncludePath += config.UV4IncPath;
                projectIno.NMakePreprocessorDefinitions = VsGen.MDK_DefineRead(docName, targetArray[0]);
                string[] groupArray = VsGen.MDK_GroupRead(docName, projectIno.MdkTarget);
                GroupListBox_Add(groupArray);
                string[] items = VsGen.MDK_SrcRead(docName, targetArray[0], groupArray[0]);
                SrcFileBox_Add(items);
                var str = VsGen.MDK_TargetStatusRead(docName, targetArray[0]);
                TargetStatusBox_Add(str);
                CreateButton.Enabled = true;
                result = true;
            }
            catch
            {
                MessageBox.Show(preStr.NotUvProj, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                result = false;
            }
            return result;
        }

        private void TryDispuProjinfo(string fileFullname)
        {
            if (!MDK_Display_Info(fileFullname)) return;
            UpdataLinkOfProject(fileFullname);
            projectIno.CuruProjectFileDir = fileFullname;
            projectIno.MdkProjectFile = fileFullname;
            projectIno.MdkProjectPath = Path.GetDirectoryName(fileFullname) + "\\";
            projectIno.ProjectName = Path.GetFileNameWithoutExtension(fileFullname);
            projectIno.VcxprojName = projectIno.ProjectName + ".vcxproj";
            projectIno.VcFiltersName = projectIno.VcxprojName + ".filters";
            projectIno.VcUserFileName = projectIno.VcxprojName + ".user";
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

                config.UV4Path = openFileDialog.FileName;
                config.UV4IncPath = config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\INC");
                config.UV4LibPath = config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\LIB");
                Creat_Config(config.DocName);
                tbKeil_path.Text = config.UV4Path;
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
                config.UV4Path = text;
                config.UV4IncPath = config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\INC");
                config.UV4LibPath = config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\LIB");
                Creat_Config(config.DocName);
                return;
            }
            MessageBox.Show(preStr.Notkeilexe + "\nthis is" + tbKeil_path.Text);
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
            config.ToolName = "Keil2VisualStudio";
            config.ToolsVersion = preStr.ToolsVersion;
            XNamespace ns = preStr.NameSp;
            var xelement = new XElement(ns + "Tool",
                new XAttribute("Name", config.ToolName),
                new XAttribute("Author", preStr.Author),
                new XAttribute("ToolsVersion", config.ToolsVersion),
                new XElement(ns + "UV4Path", config.UV4Path),
                new XElement(ns + "UV4IncPath", config.UV4IncPath),
                new XElement(ns + "UV4LibPath", config.UV4LibPath)
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
                MessageBox.Show(preStr.FristUse);
                var openFileDialog = new OpenFileDialog
                {
                    DefaultExt = "exe",
                    Filter = @"Keil Exe File (UV4.exe)|UV4.exe",
                    Title = Resources.SELUV4PATH
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    config.UV4Path = openFileDialog.FileName;
                    config.UV4IncPath = config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\INC");
                    config.UV4LibPath = config.UV4Path.Replace(@"UV4\UV4.exe", @"C51\LIB");
                    Creat_Config(config.DocName);
                    tbKeil_path.BackColor = Color.LightGreen;
                    tbKeil_path.Text = config.UV4Path;
                }
                else
                {
                    tbKeil_path.Text = t;
                }
            }
            else
            {
                XNamespace ns = preStr.NameSp;
                XDocument xdocument = XDocument.Load(docName);
                if (xdocument.Root != null)
                {
                    config.ToolsVersion = xdocument.Root.Attribute("ToolsVersion")?.Value;
                    config.UV4Path = xdocument.Root.Element(ns + "UV4Path")?.Value;
                    config.UV4IncPath = xdocument.Root.Element(ns + "UV4IncPath").Value;
                    config.UV4LibPath = xdocument.Root.Element(ns + "UV4LibPath").Value;
                }

                tbKeil_path.BackColor = Color.LightGreen;
                tbKeil_path.Text = config.UV4Path;
            }
        }

        bool fixVSPath = true;

        private void CreateButton_Click(object sender, EventArgs e)
        {
            cboxtrack.Checked = false;
            if (cboxbatch.Visible && cboxbatch.Checked)
            {

            }
            if (!fixVSPath)
            {
                var folderBrowserDialog = new FolderBrowserDialog
                {
                    //RootFolder = Environment.SpecialFolder.MyComputer,
                    Description = Resources.vsp_path,
                    SelectedPath = projectIno.MdkProjectPath
                };
                if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;
                projectIno.VcProjectPath = Path.Combine(new[]
                {
                folderBrowserDialog.SelectedPath,
                "VS"
                }) + "\\";

            }
            else
            {
                projectIno.VcProjectPath = projectIno.MdkProjectPath + "VS\\";
            }

            if (!Directory.Exists(projectIno.VcProjectPath))
            {
                Directory.CreateDirectory(projectIno.VcProjectPath);
            }
            string[] targets = VsGen.MDK_TargetRead(projectIno.MdkProjectFile);
            var relativePath = VsGen.GetRelativePath(projectIno.VcProjectPath, projectIno.MdkProjectFile);
            #region BUILD PART
            projectIno.NMakeBuildCommandLine = string.Concat(projectIno.UV4Path, " -b ", projectIno.MdkProjectFile);
            //_projectIno.NMakeBuildCommandLine = string.Concat(_projectIno.UV4Path, " -b ", _projectIno.MdkProjectFile, " -t \"Target\" -j0 -o Build.log");
            //_projectIno.NMakeBuildCommandLine = string.Concat(_projectIno.UV4Path, " -b ", relativePath, " -t \"Target\" -j0 -o Build.log");
            projectIno.LocalDebuggerCommandArguments = "-d " + projectIno.ProjectName + ".uvproj -t \"Target\"";

            #endregion


            var docName = projectIno.VcProjectPath + projectIno.ProjectName + ".sln";
            VsGen.VC_Creat_Sln(docName, projectIno.ProjectName, targets);

            docName = projectIno.VcProjectPath + projectIno.VcFiltersName;
            VsGen.VC_Filters_Create(docName, targets, ref projectIno);

            docName = projectIno.VcProjectPath + projectIno.VcxprojName;
            VsGen.VC_vcxproj_Create(docName, targets, ref projectIno, ref preStr, ref config);

            projectIno.LocalDebuggerWorkingDirectory = VsGen.GetRelativePath(projectIno.VcProjectPath, projectIno.MdkProjectPath);

            docName = projectIno.VcProjectPath + projectIno.VcUserFileName;
            VsGen.VC_Create_UserFile(docName, projectIno.LocalDebuggerCommandArguments, projectIno.LocalDebuggerWorkingDirectory, targets, ref projectIno);

            docName = projectIno.VcProjectPath + "readme.txt";
            VsGen.VC_Creat_readme(docName, projectIno.ProjectName, ref preStr);


            docName = projectIno.VcProjectPath + projectIno.ProjectName + ".sln";

            var baseUri = new Uri(docName);
            docName = VsGen.GetFullPath(docName, "");
            fmdia = new Fmdialog(

                Resources.sueecssTip1 + "\r\n" +
                Resources.enjoy_vs

            );
            Fmdialog.NextAction nextAct = Fmdialog.NextAction.None;
            fmdia.ShowDialog();
            nextAct = fmdia.nextAction;
            switch (nextAct)
            {
                case Fmdialog.NextAction.None:
                    break;
                case Fmdialog.NextAction.OPenAndTrack:
                    cboxtrack.Checked = true;
                    try
                    {
                        Process.Start(docName);
                    }
                    catch
                    {
                        MessageBox.Show("无法打开当前VS项目！\n请确任安装好Visual Studio 2017+!", "很遗憾!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    break;
                case Fmdialog.NextAction.OpenOnly:
                    try
                    {
                        Process.Start(docName);
                    }
                    catch
                    {
                        MessageBox.Show("无法打开当前VS项目！\n请确任安装好Visual Studio 2017+!", "很遗憾!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case Fmdialog.NextAction.OPenFolder:
                    Process.Start(projectIno.VcProjectPath);
                    break;
                default:
                    break;
            }
        }
        private bool TryGetUV4Path(ref string path)
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
                regSubKey = regKey.OpenSubKey(preStr.RegPath, false);

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
            config.PreDefine = tb_predef.Text;
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
            tb_predef.Text = preStr.Predefine;
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

            TryDispuProjinfo(uprojInfo[SourcePathCBOX.SelectedIndex].FileFullname);
        }

        private void ElementHost_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (projectIno.CuruProjectFileDir == "") return;
            string[] array = VsGen.MDK_TargetRead(projectIno.CuruProjectFileDir);
            string[] array2 = VsGen.MDK_GroupRead(projectIno.CuruProjectFileDir, array[elementHost.SelectedIndex]);
            GroupListBox_Add(array2);
            string[] items = VsGen.MDK_SrcRead(projectIno.CuruProjectFileDir, array[elementHost.SelectedIndex], array2[0]);
            SrcFileBox_Add(items);
            var str = VsGen.MDK_TargetStatusRead(projectIno.CuruProjectFileDir, array[elementHost.SelectedIndex]);
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