private void Keil2VS_Load(object sender, EventArgs e)
        {
            this.elementHost.Child = this.TargetListBOX;
            this.TargetListBOX.SelectionChanged += new SelectionChangedEventHandler(this.TargetListBOX_SelectionChanged);
            this.GroupListBox.SelectedIndexChanged += this.GroupListBox_SelectedIndexChanged;
            this.FileBox.DoubleClick += this.FileBox_DoubleClick;
            this.tbKeil_path.DoubleClick += this.Tbkeil_path_DoubleClick;
            this.tbKeil_path.DragDrop += this.Tbkeil_pathBOX_DragDrop;
            this.tbKeil_path.DragEnter += this.KEIL2VSDragEnter;


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
            }
            UpDateCurfolderUproj();
            this.SourcePathCBOX.SelectedIndexChanged += new System.EventHandler(this.SourcePathCBOX_SelectedIndexChanged);

        }


        private void UpDateCurfolderUproj()
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
            int cnt = 0;
            DirectoryInfo TheFolder = new DirectoryInfo(exeDir);
            cnt = TheFolder.GetFiles("*.uvproj").Length;
            if (0 != cnt)
            {
                fileInfo = new FileInfo[cnt];
                cnt = 0;
                foreach (FileInfo NextFile in TheFolder.GetFiles("*.uvproj"))
                {
                    fileInfo[cnt++] = NextFile;
                }
                return true;
            }
            return false;
        }


        private void SourcePathComobox_add(_uprojInfo[] it)
        {
            this.SourcePathCBOX.Items.Clear();
            for (int i = 0; i < it.GetLength(0); i++)
            {
                this.SourcePathCBOX.Items.Add(it[i].fileName);
            }
            //this.SourcePathCBOX.Items.Add(this.PreStr.selectnewfolder);
            this.SourcePathCBOX.SelectedIndex = 0;
        }

        private void TargetListBox_Add(string[] Items)
        {
            this.TargetListBOX.Items.Clear();
            foreach (string newItem in Items)
            {
                this.TargetListBOX.Items.Add(newItem);
            }
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

        private void TargetListBOX_SelectionChanged(object sander, EventArgs e)
        {
            if (this.ProjectIno.CuruProjectFileDir != "")
            {
                string[] array = this.MDK_TargetRead(this.ProjectIno.CuruProjectFileDir);
                string[] array2 = this.MDK_GroupRead(this.ProjectIno.CuruProjectFileDir, array[this.TargetListBOX.SelectedIndex]);
                this.GroupListBox_Add(array2);
                string[] items = this.MDK_SrcRead(this.ProjectIno.CuruProjectFileDir, array[this.TargetListBOX.SelectedIndex], array2[0]);
                this.SrcFileBox_Add(items);
                string str = this.MDK_TargetStatusRead(this.ProjectIno.CuruProjectFileDir, array[this.TargetListBOX.SelectedIndex]);
                this.TargetStatusBox_Add(str);
            }
        }
        private void GroupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ProjectIno.CuruProjectFileDir != "")
            {
                string[] array = this.MDK_TargetRead(this.ProjectIno.CuruProjectFileDir);
                string[] array2 = this.MDK_GroupRead(this.ProjectIno.CuruProjectFileDir, array[this.TargetListBOX.SelectedIndex]);
                string[] items = this.MDK_SrcRead(this.ProjectIno.CuruProjectFileDir, array[this.TargetListBOX.SelectedIndex], array2[this.GroupListBox.SelectedIndex]);
                this.SrcFileBox_Add(items);
            }
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
        private void btnOpenFolder_Click(object sender, EventArgs e)
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
                this.uprojInfo = new _uprojInfo[1];
                this.uprojInfo[0].fileName = this.ProjectIno.ProjectName;
                this.uprojInfo[0].fileFullname = this.SourcePathCBOX.Text;

            }
            this.TargetListBOX.SelectedIndex = 0;
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


