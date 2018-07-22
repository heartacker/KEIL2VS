using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace KEIL2VS
{
    partial class MainForm : Form
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(MainForm));
            this.CreateButton = new Button();
            this.gbTargetGroup = new GroupBox();
            this.elementHost = new ComboBox();
            this.gbTargetInfo = new GroupBox();
            this.TargetStatus = new TextBox();
            this.gbSourceGroup = new GroupBox();
            this.FileBox = new ListBox();
            this.GroupListBox = new ListBox();
            this.groupBox1 = new GroupBox();
            this.btnFreshFoldor = new Button();
            this.btnOpenFolder = new Button();
            this.SourcePathCBOX = new ComboBox();
            this.gbKeilExePath = new GroupBox();
            this.tbKeil_path = new TextBox();
            this.lbppd = new Label();
            this.tb_predef = new TextBox();
            this.sourcepathTip = new ToolTip(this.components);
            this.gbTargetGroup.SuspendLayout();
            this.gbTargetInfo.SuspendLayout();
            this.gbSourceGroup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbKeilExePath.SuspendLayout();
            this.SuspendLayout();
            // 
            // CreateButton
            // 
            this.CreateButton.Anchor = ((AnchorStyles)(((AnchorStyles.Bottom | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.CreateButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.CreateButton.Font = new Font("Consolas", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.CreateButton.Location = new Point(5, 595);
            this.CreateButton.Margin = new Padding(3, 4, 3, 4);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Padding = new Padding(3, 4, 3, 4);
            this.CreateButton.Size = new Size(629, 42);
            this.CreateButton.TabIndex = 8;
            this.CreateButton.Text = "Create Visual Studio Project";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new EventHandler(this.CreateButton_Click);
            // 
            // gbTargetGroup
            // 
            this.gbTargetGroup.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.gbTargetGroup.Controls.Add(this.elementHost);
            this.gbTargetGroup.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.gbTargetGroup.Location = new Point(5, 147);
            this.gbTargetGroup.Margin = new Padding(3, 4, 3, 4);
            this.gbTargetGroup.Name = "gbTargetGroup";
            this.gbTargetGroup.Padding = new Padding(3, 4, 3, 4);
            this.gbTargetGroup.Size = new Size(629, 48);
            this.gbTargetGroup.TabIndex = 20;
            this.gbTargetGroup.TabStop = false;
            this.gbTargetGroup.Text = "Target Groups";
            // 
            // elementHost
            // 
            this.elementHost.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.elementHost.FormattingEnabled = true;
            this.elementHost.Location = new Point(5, 19);
            this.elementHost.Name = "elementHost";
            this.elementHost.Size = new Size(617, 22);
            this.elementHost.TabIndex = 0;
            this.elementHost.SelectionChangeCommitted += new EventHandler(this.ElementHost_SelectionChangeCommitted);
            // 
            // gbTargetInfo
            // 
            this.gbTargetInfo.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.gbTargetInfo.Controls.Add(this.TargetStatus);
            this.gbTargetInfo.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.gbTargetInfo.Location = new Point(5, 199);
            this.gbTargetInfo.Margin = new Padding(3, 4, 3, 4);
            this.gbTargetInfo.Name = "gbTargetInfo";
            this.gbTargetInfo.Padding = new Padding(3, 4, 3, 4);
            this.gbTargetInfo.Size = new Size(629, 158);
            this.gbTargetInfo.TabIndex = 5;
            this.gbTargetInfo.TabStop = false;
            this.gbTargetInfo.Text = "Target Info";
            // 
            // TargetStatus
            // 
            this.TargetStatus.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) 
            | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.TargetStatus.BackColor = SystemColors.Window;
            this.TargetStatus.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.TargetStatus.Location = new Point(5, 19);
            this.TargetStatus.Margin = new Padding(3, 4, 3, 4);
            this.TargetStatus.Multiline = true;
            this.TargetStatus.Name = "TargetStatus";
            this.TargetStatus.ReadOnly = true;
            this.TargetStatus.ScrollBars = ScrollBars.Both;
            this.TargetStatus.Size = new Size(617, 134);
            this.TargetStatus.TabIndex = 0;
            // 
            // gbSourceGroup
            // 
            this.gbSourceGroup.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) 
            | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.gbSourceGroup.Controls.Add(this.FileBox);
            this.gbSourceGroup.Controls.Add(this.GroupListBox);
            this.gbSourceGroup.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.gbSourceGroup.Location = new Point(5, 363);
            this.gbSourceGroup.Margin = new Padding(3, 4, 3, 4);
            this.gbSourceGroup.Name = "gbSourceGroup";
            this.gbSourceGroup.Padding = new Padding(3, 4, 3, 4);
            this.gbSourceGroup.Size = new Size(629, 224);
            this.gbSourceGroup.TabIndex = 6;
            this.gbSourceGroup.TabStop = false;
            this.gbSourceGroup.Text = "Source Groups";
            // 
            // FileBox
            // 
            this.FileBox.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) 
            | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.FileBox.FormattingEnabled = true;
            this.FileBox.HorizontalScrollbar = true;
            this.FileBox.IntegralHeight = false;
            this.FileBox.ItemHeight = 14;
            this.FileBox.Location = new Point(264, 16);
            this.FileBox.Name = "FileBox";
            this.FileBox.Size = new Size(358, 203);
            this.FileBox.TabIndex = 1;
            // 
            // GroupListBox
            // 
            this.GroupListBox.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Bottom) 
            | AnchorStyles.Left)));
            this.GroupListBox.BackColor = SystemColors.Window;
            this.GroupListBox.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.GroupListBox.FormattingEnabled = true;
            this.GroupListBox.HorizontalScrollbar = true;
            this.GroupListBox.IntegralHeight = false;
            this.GroupListBox.ItemHeight = 14;
            this.GroupListBox.Location = new Point(5, 16);
            this.GroupListBox.Margin = new Padding(3, 4, 3, 4);
            this.GroupListBox.Name = "GroupListBox";
            this.GroupListBox.Size = new Size(255, 203);
            this.GroupListBox.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.groupBox1.BackColor = Color.LightSalmon;
            this.groupBox1.Controls.Add(this.btnFreshFoldor);
            this.groupBox1.Controls.Add(this.btnOpenFolder);
            this.groupBox1.Controls.Add(this.SourcePathCBOX);
            this.groupBox1.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new Point(5, 64);
            this.groupBox1.Margin = new Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new Padding(3, 4, 3, 4);
            this.groupBox1.Size = new Size(629, 47);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Keil Project File Path";
            // 
            // btnFreshFoldor
            // 
            this.btnFreshFoldor.BackColor = Color.LimeGreen;
            this.btnFreshFoldor.Location = new Point(515, 13);
            this.btnFreshFoldor.Name = "btnFreshFoldor";
            this.btnFreshFoldor.Size = new Size(32, 28);
            this.btnFreshFoldor.TabIndex = 2;
            this.btnFreshFoldor.Text = "F5";
            this.btnFreshFoldor.UseVisualStyleBackColor = false;
            this.btnFreshFoldor.Visible = false;
            this.btnFreshFoldor.Click += new EventHandler(this.UpDateCurfolderUproj);
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnOpenFolder.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnOpenFolder.Location = new Point(546, 12);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new Size(76, 30);
            this.btnOpenFolder.TabIndex = 1;
            this.btnOpenFolder.Text = "Open";
            this.btnOpenFolder.UseVisualStyleBackColor = false;
            this.btnOpenFolder.Click += new EventHandler(this.BtnOpenFolder_Click);
            // 
            // SourcePathCBOX
            // 
            this.SourcePathCBOX.AllowDrop = true;
            this.SourcePathCBOX.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.SourcePathCBOX.FormattingEnabled = true;
            this.SourcePathCBOX.Location = new Point(5, 17);
            this.SourcePathCBOX.Name = "SourcePathCBOX";
            this.SourcePathCBOX.Size = new Size(510, 22);
            this.SourcePathCBOX.TabIndex = 0;
            this.SourcePathCBOX.Text = "Open Or drop The Keil the Project File to here";
            this.SourcePathCBOX.SelectionChangeCommitted += new EventHandler(this.SourcePathCBOX_SelectionChangeCommitted);
            this.SourcePathCBOX.DragDrop += new DragEventHandler(this.SourcePathCBOX_DragDrop);
            this.SourcePathCBOX.DragEnter += new DragEventHandler(this.Keil2VsDragEnter);
            // 
            // gbKeilExePath
            // 
            this.gbKeilExePath.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.gbKeilExePath.Controls.Add(this.tbKeil_path);
            this.gbKeilExePath.Location = new Point(5, 12);
            this.gbKeilExePath.Name = "gbKeilExePath";
            this.gbKeilExePath.Size = new Size(629, 45);
            this.gbKeilExePath.TabIndex = 21;
            this.gbKeilExePath.TabStop = false;
            this.gbKeilExePath.Text = "Keil Program Path";
            // 
            // tbKeil_path
            // 
            this.tbKeil_path.AllowDrop = true;
            this.tbKeil_path.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) 
            | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.tbKeil_path.BackColor = SystemColors.Window;
            this.tbKeil_path.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.tbKeil_path.ForeColor = SystemColors.AppWorkspace;
            this.tbKeil_path.Location = new Point(7, 14);
            this.tbKeil_path.Margin = new Padding(3, 4, 3, 4);
            this.tbKeil_path.Name = "tbKeil_path";
            this.tbKeil_path.ReadOnly = true;
            this.tbKeil_path.Size = new Size(615, 22);
            this.tbKeil_path.TabIndex = 2;
            this.tbKeil_path.Text = "Double-click or drag and drop The Keil Program  to here";
            // 
            // lbppd
            // 
            this.lbppd.AutoSize = true;
            this.lbppd.Location = new Point(12, 123);
            this.lbppd.Name = "lbppd";
            this.lbppd.Size = new Size(189, 14);
            this.lbppd.TabIndex = 23;
            this.lbppd.Text = "Add PreCompiled definition";
            this.lbppd.Click += new EventHandler(this.Lbppd_Click);
            // 
            // tb_predef
            // 
            this.tb_predef.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.tb_predef.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.tb_predef.Location = new Point(207, 118);
            this.tb_predef.Name = "tb_predef";
            this.tb_predef.Size = new Size(420, 26);
            this.tb_predef.TabIndex = 24;
            this.tb_predef.KeyDown += new KeyEventHandler(this.Tb_predef_KeyDown);
            this.tb_predef.Leave += new EventHandler(this.Tb_predef_TextChanged);
            // 
            // sourcepathTip
            // 
            this.sourcepathTip.IsBalloon = true;
            this.sourcepathTip.ToolTipTitle = "Pooppoo";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 14F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(640, 644);
            this.Controls.Add(this.tb_predef);
            this.Controls.Add(this.lbppd);
            this.Controls.Add(this.gbKeilExePath);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbSourceGroup);
            this.Controls.Add(this.gbTargetInfo);
            this.Controls.Add(this.gbTargetGroup);
            this.Controls.Add(this.CreateButton);
            this.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Convert Keil Project to Visual Studio Project";
            this.Load += new EventHandler(this.Keil2VS_Load);
            this.Shown += new EventHandler(this.Keil2VS_Shown);
            this.KeyDown += new KeyEventHandler(this.MainForm_KeyDown);
            this.gbTargetGroup.ResumeLayout(false);
            this.gbTargetInfo.ResumeLayout(false);
            this.gbTargetInfo.PerformLayout();
            this.gbSourceGroup.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.gbKeilExePath.ResumeLayout(false);
            this.gbKeilExePath.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //private ComboBox elementHost;
        private Button CreateButton;
        private GroupBox gbTargetGroup;
        private GroupBox gbTargetInfo;
        private TextBox TargetStatus;
        private GroupBox gbSourceGroup;
        private ListBox GroupListBox;
        private GroupBox groupBox1;
        private ListBox FileBox;
        private GroupBox gbKeilExePath;
        private TextBox tbKeil_path;
        private Label lbppd;
        private TextBox tb_predef;
        private ComboBox SourcePathCBOX;
        private Button btnOpenFolder;
        private ToolTip sourcepathTip;
        private ComboBox elementHost;
        private Button btnFreshFoldor;
    }
}


