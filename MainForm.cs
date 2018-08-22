using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OfficeOpenXml;

namespace Table2Code
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            color = this.btnSaveCode2txt.BackColor;
        }
        static FileSystemWatcher watcher = new FileSystemWatcher();

        public delegate void UpDateMainForm();
        public UpDateMainForm udform;
        Color color;

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text += String.Format(" v{0}.{1}.{2}.{3}",
                Assembly.GetExecutingAssembly().GetName().Version.Major.ToString(),
                Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString(),
                Assembly.GetExecutingAssembly().GetName().Version.Build.ToString(),
                Assembly.GetExecutingAssembly().GetName().Version.Revision.ToString("D2"));
            MinimumSize = new Size(Width, Height);
            _applicationPath = Application.StartupPath;
            dataGv.RowHeadersWidth = ColX;
            _ns.defIndex = (int)nudColumns.Value;
            thisDGVC = new DataGridViewTextBoxColumn[ColX];
            for (var i = 0; i < ColX; i++)
            {
                thisDGVC[i] = new DataGridViewTextBoxColumn();
            }
            dataGv.Columns.AddRange(thisDGVC);
            //this.dataGv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            //this.A,
            //this.B,
            //this.C,
            //this.D,
            //this.E});
            dataGv.Select();
        }
        private void btnFileVisual()
        {
            btnFilehasChange.Visible = true;
        }
        private void MainForm_Shown(object sender, EventArgs e)
        {
            //IEnumerable<string> scanfiles = Directory.EnumerateDirectories(ApplicationPath, "xlsm", SearchOption.AllDirectories);
            udform = new UpDateMainForm(btnFileVisual);
            var str = Directory.GetFiles(_applicationPath, "*.xls*", SearchOption.AllDirectories);
            if (str.Length == 0)
            {
                return;
            }
            Cbb_excel_config(str);
            btnCreatCode.Enabled = true;
            ttipluckfind.Show("当前文件夹找到了相关excel", cbbFileArrr, cbbFileArrr.Location.X + 300, cbbFileArrr.Location.Y - 70, 10 * 1000);
            ReadPeekData(_ns.fi);
            FileWatchStart(_ns.fi, true);
        }

        private void FileWatchStart(FileInfo fileInfo, bool isNew)
        {
            watcher.EnableRaisingEvents = false;
            watcher.BeginInit();
            watcher.Path = fileInfo.DirectoryName;
            //watcher.Filter = "*.xlsm|*.xlsx";
            watcher.IncludeSubdirectories = true;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += new FileSystemEventHandler(FileInfoChange);
            //watcher.Created += new FileSystemEventHandler(FileInfoChange);
            //watcher.Deleted += new FileSystemEventHandler(FileInfoChange);
            //watcher.Renamed += new RenamedEventHandler(FileInfoChange_Rename);
            watcher.EndInit();
            watcher.EnableRaisingEvents = true;
        }
        private void FileInfoChange(object ob, FileSystemEventArgs fe)
        {
            if (fe.Name.StartsWith(@"~$") || fe.Name.EndsWith(".txt") || fe.Name.EndsWith(".exe") || fe.Name.EndsWith(".xml"))
            {
                return;
            }
            //if (fe.Name!=_ns.fi.Name)
            //{
            //    return;
            //}
            FileInfo tempfi = new FileInfo(_ns.fi.FullName);
            if ((fe.ChangeType == WatcherChangeTypes.Changed) && (_ns.fi.LastWriteTime != tempfi.LastWriteTime))
            {

                OnChanged(ob, fe);

            }
        }

        private void FileInfoChange_Rename(object ob, RenamedEventArgs fe)   //文件重命名时被调用的处理方法
        {
            if (fe.ChangeType == WatcherChangeTypes.Renamed)
            {
                OnChanged(ob, fe);
            }
        }

        private void OnChanged(object ob, FileSystemEventArgs fe)
        {
            this.BeginInvoke(udform);
            //btnFilehasChange.Visible = true;
            //MessageBox.Show("当前文件已经更改,点击确定会自动刷新。\n不用从新打开程序或者从新打开Excel", "小小提示!", MessageBoxButtons.OK);
        }
        private string _applicationPath;

        private DataGridViewColumn[] thisDGVC;
        private FileInfo[] _listfi;
        private ThisHandlingFile _ns = new ThisHandlingFile
        {
            dTNow = new DataTable(),
            SheetNames = new List<string>()

        };
        struct ThisHandlingFile
        {
            public FileInfo fi;
            public List<string> SheetNames;
            public ExcelPackage EP;// = new ExcelPackage();
            public ExcelWorkbook WB;
            public ExcelWorksheet WS;
            public int maxRow;
            public int maxColum;
            public DataRow dRNow;
            public DataColumn dCnow;
            public DataTable dTNow;
            public String BLKName, pinName, vDomian, deftVal;
            public int defIndex;
            public string CodeFullName;
            public string ERR_OUT()
            {
                return $"block:{BLKName} ,pin_name:{pinName},defVal:{deftVal}";
            }
        }

        private StringBuilder CodeStringbuilder = new StringBuilder();
        private int ColX = 15;
        public string rn = "\r\n";

        private CodeGenerator codeGenLib = new CodeGenerator();


        private void TbExcelPath_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
                return;
            }
            e.Effect = DragDropEffects.None;
        }
        private void ReadPeekData(FileInfo f)
        {
            _ns.EP = new ExcelPackage(f, true);
            _ns.WB = _ns.EP.Workbook;
            _ns.WB.Calculate();
            int sheetcnt = _ns.WB.Worksheets.Count;

            _ns.SheetNames?.Clear();

            for (int i = 1; i < sheetcnt + 1; i++)
            {

                _ns.SheetNames.Add(_ns.WB.Worksheets[i].Name);

            }
            cbbSheets.Items.Clear();
            foreach (string sname in _ns.SheetNames)
            {
                cbbSheets.Items.Add(sname);
            }
            cbbSheets.SelectedIndex = 0;
            lbsheet.Text = "Sheet(x)".Replace("x", (cbbSheets.SelectedIndex + 1).ToString());
            Sheet_Title_SelectionChangeCommitted(null, null);
        }

        private bool CheckisExcelFile(string[] path)
        {
            if (path == null)
            {
                MessageBox.Show("This is Not excel file", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!path.Any(str => !str.EndsWith("xlsx") && !str.EndsWith("xlsm"))) return true;
            MessageBox.Show("This is Not excel file", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;

            //todo check file
        }
        private void Cbb_path_arr_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = { };
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                files = (string[])e.Data.GetData(DataFormats.FileDrop);
            }
            if (!CheckisExcelFile(files))
            {
                return;
            }
            Cbb_excel_config(files);
            ReadPeekData(_ns.fi);
            FileWatchStart(_ns.fi, true);
        }

        private void Cbb_excel_config(String[] path)
        {

            _listfi = new FileInfo[path.GetLength(0)];
            int i = 0;
            foreach (string str in path)
            {
                _listfi[i++] = new FileInfo(str);
            }

            cbbFileArrr.Items.Clear();
            foreach (FileInfo fi in _listfi)
            {
                if (!fi.Name.StartsWith(@"~$"))
                {
                    cbbFileArrr.Items.Add(fi.Name);
                }
            }
            if (cbbFileArrr.Items.Count <= 0)
            {
                return;
            }
            _ns.fi = _listfi[0];
            cbbFileArrr.Font = new Font("Consolas", 11, FontStyle.Bold);
            cbbFileArrr.ForeColor = Color.Black;
            btnCreatCode.Enabled = true;
            cbbFileArrr.SelectedIndex = 0;
        }


        private void btnFolder_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog
            {
                DefaultExt = "xlsm",
                Filter = "Interface File (*.xlsm)|*.xlsm|Interface File (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                Title = "Please Select your Table file！"
            };
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string[] filename = { ofd.FileName };
            if (!CheckisExcelFile(filename))
            {
                return;
            }
            Cbb_excel_config(filename);
            ReadPeekData(_ns.fi);
            FileWatchStart(_ns.fi, true);
        }

        private void cbbFileArrr_SelectedIndexChangedCommitted(object sender, EventArgs e)
        {
            if (cbbFileArrr.SelectedIndex < 0)
            {
                return;
            }
            if (cbbFileArrr.SelectedIndex >= _listfi.Length)
            {
                return;
            }
            rtbCode.Clear();
            _ns.fi = _listfi[cbbFileArrr.SelectedIndex];
            rtbCode.AppendText(_ns.fi.FullName + "\n");
            ReadPeekData(_ns.fi);
            cbbSheets.Enabled = true;
            FileWatchStart(_ns.fi, true);
        }
        private void Sheet_Title_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _ns.WS = _ns.WB.Worksheets[cbbSheets.SelectedIndex + 1];
            lbsheet.Text = "Sheet(x)".Replace("x", (cbbSheets.SelectedIndex + 1).ToString());
            object str = _ns.WS.Cells[(int)nudTitleRow.Value, (int)nudColumns.Value].Value;
            if (str != null)
            {
                tbColValue.Text = str.ToString();
            }
            _ns.maxRow = _ns.WS.Dimension.End.Row;
            _ns.maxColum = _ns.WS.Dimension.End.Column;
            nudStopRow.Value = _ns.maxRow;
            nudStopColum.Value = _ns.maxColum;
            Title_ValueChanged(sender, e);
            WorkSheetNow2dataTableNow(sender, e);
        }
        private void Title_ValueChanged(object sender, EventArgs e)
        {
            _ns.defIndex = (int)nudColumns.Value;
            if (_ns.WB == null || _ns.WS == null)
            {
                return;
            }
            object str = _ns.WS.Cells[(int)nudTitleRow.Value, (int)nudColumns.Value].Value;
            if (str != null)
            {
                tbColValue.Text = str.ToString();
            }
            else
            {
                tbColValue.Text = "";
                if (nudColumns.Value > 1)
                {
                    nudColumns.Value -= 1;
                }
            }
        }
        private bool WorkSheetNow2dataTableNow(object sender, EventArgs e)
        {
            _ns.dTNow.Clear();
            _ns.dTNow.NewRow();
            dataGvClear();
            //for (int i = 0; i < ColX; i++)
            //{
            //    if (workSheetNow.Cells[1, i + 1].Value == null)
            //    {
            //        x++;
            //        if (x >= 2)
            //        {
            //            endOfOneRow = true;
            //            break;
            //        }
            //    }
            //    else
            //    {

            //        thisDGVC[i].HeaderText = workSheetNow.Cells[1, i + 1].Value.ToString();
            //        Application.DoEvents();
            //    }
            //    workSheetNowRowCount = i;
            //}
            for (int i = 1; i <= (_ns.maxColum < ColX ? _ns.maxColum : ColX); i++)
            {
                try
                {

                    thisDGVC[i - 1].HeaderText = _ns.WS.Cells[1, i].Text;

                    //thisDGVC[i - 1].HeaderText = NS.WS.Cells[1, i].Value.ToString();
                }
                catch
                {

                    Debug.WriteLine("NULL");
                }

            }

            dataGv.Refresh();

            tabCtrlView.TabPages[1].Text = "DateTable" + _ns.maxRow + "X" + _ns.maxColum;

            return true;
        }
        private void dataGvClear()
        {
            for (int i = 0; i < ColX; i++)
            {

                thisDGVC[i].HeaderText = "";
            }
        }


        private void BuildCode(object sender, EventArgs e)
        {
            CodeGenerator.ERROR err_msg;
            CodeStringbuilder.Clear();
            rtbCode.Clear();
            Rtb_Error.Clear();
            btnSaveCode2txt.Enabled = true;
            btnSaveCode2txt.BackColor = Color.LawnGreen;
            CodeStringbuilder.Append($"//This code is Generated by {Text}, date:{DateTime.Now},Author:heartacker\r\n");
            codeGenLib.Init(tb_function_name.Text);
            /***************code******************/
            string assignment = _ns.WS.Cells[1, _ns.defIndex].Text;
            var style = codeGenLib.CheckAssignmentStyle(assignment);

            for (int row = 2; row <= (int)nudStopRow.Value; row++)
            //for (int row = 2; row < NS.maxRow; row++)
            {
                if (_ns.WS.Cells[row, 1].Text != "")
                {
                    _ns.BLKName = _ns.WS.Cells[row, 1].Text;
                    continue;
                }
                _ns.pinName = _ns.WS.Cells[row, 2].Text;
                _ns.vDomian = _ns.WS.Cells[row, 3].Text;
                _ns.deftVal = _ns.WS.Cells[row, _ns.defIndex].Text;
                err_msg = codeGenLib.Add_Arguments(_ns.BLKName, _ns.pinName, _ns.vDomian, _ns.deftVal);
                if (CodeGenerator.ERROR.NO_ERROR != err_msg)
                {
                    Rtb_Error.AppendText(err_msg + "\t" + _ns.ERR_OUT() + "\r\n");
                    btnSaveCode2txt.Enabled = false;
                    btnSaveCode2txt.BackColor = color;

                }
                Application.DoEvents();
            }
            CodeStringbuilder.Append(codeGenLib.Generate());
            rtbCode.AppendText(CodeStringbuilder.ToString());
        }

        private void btnSaveCode2txt_Click(object sender, EventArgs e)
        {
            btnSaveCode2txt.BackColor = color;
            btnSaveCode2txt.Enabled = false;
            string dir = _applicationPath + "\\" + "VerilogaCode" + "\\";
            string Text_name = tb_function_name.Text + DateTime.Now;
            Text_name = Text_name + @".vh";

            Text_name = Regex.Replace(Text_name, "[<>/\\|:\"?*]", "_");
            Text_name = Text_name.Replace(" ", "_");
            var fullname = dir + Text_name;

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            FileStream fileStream = File.OpenWrite(fullname);
            _ns.CodeFullName = fullname;
            byte[] bytes = new UTF8Encoding(true).GetBytes(rtbCode.Text);
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
            ntfSuccess.Visible = true;
            ntfSuccess.ShowBalloonTip(1000 * 10, "Congratulations", "Convert successfully,\nCLick this to open Code File!!\nRecommoned use Notepad++", ToolTipIcon.Info);
        }

        private void btnCreatCode_Click(object sender, EventArgs e)
        {
            Enabled = false;
            lock (this)
            {
                if (_ns.fi == null)
                {
                    return;
                }
            }
            BuildCode(sender, e);
            Enabled = true;
        }

        private void ntfSuccess_MouseClick(object sender, EventArgs e)
        {
            try
            {
                Process.Start(_ns.CodeFullName);
            }
            catch
            {
            }

        }

        private void btnFilehasChange_Click(object sender, EventArgs e)
        {
            btnFilehasChange.Visible = false;
            cbbFileArrr_SelectedIndexChangedCommitted(sender, e);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                File.Delete(_applicationPath + @"\EPPlus.dll");
            }
            catch
            {

            }
        }
    }
}


