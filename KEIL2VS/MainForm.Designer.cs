using System.Windows.Forms;

namespace KEIL2VS
{
    partial class MainForm : Form
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.CreateButton = new System.Windows.Forms.Button();
            this.gbTargetGroup = new System.Windows.Forms.GroupBox();
            this.elementHost = new System.Windows.Forms.ComboBox();
            this.gbTargetInfo = new System.Windows.Forms.GroupBox();
            this.TargetStatus = new System.Windows.Forms.TextBox();
            this.gbSourceGroup = new System.Windows.Forms.GroupBox();
            this.FileBox = new System.Windows.Forms.ListBox();
            this.GroupListBox = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.SourcePathCBOX = new System.Windows.Forms.ComboBox();
            this.gbKeilExePath = new System.Windows.Forms.GroupBox();
            this.tbKeil_path = new System.Windows.Forms.TextBox();
            this.lbppd = new System.Windows.Forms.Label();
            this.tb_predef = new System.Windows.Forms.TextBox();
            this.sourcepathTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbTargetGroup.SuspendLayout();
            this.gbTargetInfo.SuspendLayout();
            this.gbSourceGroup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbKeilExePath.SuspendLayout();
            this.SuspendLayout();
            // 
            // CreateButton
            // 
            this.CreateButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CreateButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CreateButton.Font = new System.Drawing.Font("Consolas", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreateButton.Location = new System.Drawing.Point(5, 639);
            this.CreateButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CreateButton.Size = new System.Drawing.Size(574, 42);
            this.CreateButton.TabIndex = 8;
            this.CreateButton.Text = "Create Visual Studio Project";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // gbTargetGroup
            // 
            this.gbTargetGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbTargetGroup.Controls.Add(this.elementHost);
            this.gbTargetGroup.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbTargetGroup.Location = new System.Drawing.Point(5, 147);
            this.gbTargetGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbTargetGroup.Name = "gbTargetGroup";
            this.gbTargetGroup.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbTargetGroup.Size = new System.Drawing.Size(574, 48);
            this.gbTargetGroup.TabIndex = 20;
            this.gbTargetGroup.TabStop = false;
            this.gbTargetGroup.Text = "Target Groups";
            // 
            // elementHost
            // 
            this.elementHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.elementHost.FormattingEnabled = true;
            this.elementHost.Location = new System.Drawing.Point(0, 19);
            this.elementHost.Name = "elementHost";
            this.elementHost.Size = new System.Drawing.Size(567, 22);
            this.elementHost.TabIndex = 0;
            this.elementHost.SelectionChangeCommitted += new System.EventHandler(this.elementHost_SelectionChangeCommitted);
            // 
            // gbTargetInfo
            // 
            this.gbTargetInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbTargetInfo.Controls.Add(this.TargetStatus);
            this.gbTargetInfo.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbTargetInfo.Location = new System.Drawing.Point(5, 199);
            this.gbTargetInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbTargetInfo.Name = "gbTargetInfo";
            this.gbTargetInfo.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbTargetInfo.Size = new System.Drawing.Size(574, 158);
            this.gbTargetInfo.TabIndex = 5;
            this.gbTargetInfo.TabStop = false;
            this.gbTargetInfo.Text = "Target Info";
            // 
            // TargetStatus
            // 
            this.TargetStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TargetStatus.BackColor = System.Drawing.SystemColors.Window;
            this.TargetStatus.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TargetStatus.Location = new System.Drawing.Point(5, 19);
            this.TargetStatus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TargetStatus.Multiline = true;
            this.TargetStatus.Name = "TargetStatus";
            this.TargetStatus.ReadOnly = true;
            this.TargetStatus.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TargetStatus.Size = new System.Drawing.Size(562, 134);
            this.TargetStatus.TabIndex = 0;
            // 
            // gbSourceGroup
            // 
            this.gbSourceGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSourceGroup.Controls.Add(this.FileBox);
            this.gbSourceGroup.Controls.Add(this.GroupListBox);
            this.gbSourceGroup.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbSourceGroup.Location = new System.Drawing.Point(5, 363);
            this.gbSourceGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbSourceGroup.Name = "gbSourceGroup";
            this.gbSourceGroup.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbSourceGroup.Size = new System.Drawing.Size(574, 268);
            this.gbSourceGroup.TabIndex = 6;
            this.gbSourceGroup.TabStop = false;
            this.gbSourceGroup.Text = "Source Groups";
            // 
            // FileBox
            // 
            this.FileBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FileBox.FormattingEnabled = true;
            this.FileBox.HorizontalScrollbar = true;
            this.FileBox.IntegralHeight = false;
            this.FileBox.ItemHeight = 14;
            this.FileBox.Location = new System.Drawing.Point(264, 16);
            this.FileBox.Name = "FileBox";
            this.FileBox.Size = new System.Drawing.Size(303, 247);
            this.FileBox.TabIndex = 1;
            // 
            // GroupListBox
            // 
            this.GroupListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.GroupListBox.BackColor = System.Drawing.SystemColors.Window;
            this.GroupListBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupListBox.FormattingEnabled = true;
            this.GroupListBox.HorizontalScrollbar = true;
            this.GroupListBox.IntegralHeight = false;
            this.GroupListBox.ItemHeight = 14;
            this.GroupListBox.Location = new System.Drawing.Point(5, 16);
            this.GroupListBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GroupListBox.Name = "GroupListBox";
            this.GroupListBox.Size = new System.Drawing.Size(255, 247);
            this.GroupListBox.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnOpenFolder);
            this.groupBox1.Controls.Add(this.SourcePathCBOX);
            this.groupBox1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(5, 64);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(574, 47);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Keil Project File Path";
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnOpenFolder.Location = new System.Drawing.Point(467, 12);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(107, 30);
            this.btnOpenFolder.TabIndex = 1;
            this.btnOpenFolder.Text = "Open";
            this.btnOpenFolder.UseVisualStyleBackColor = false;
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // SourcePathCBOX
            // 
            this.SourcePathCBOX.AllowDrop = true;
            this.SourcePathCBOX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SourcePathCBOX.FormattingEnabled = true;
            this.SourcePathCBOX.Location = new System.Drawing.Point(5, 17);
            this.SourcePathCBOX.Name = "SourcePathCBOX";
            this.SourcePathCBOX.Size = new System.Drawing.Size(455, 22);
            this.SourcePathCBOX.TabIndex = 0;
            this.SourcePathCBOX.Text = "Open Or drop The MDK the Project File to here";
            this.SourcePathCBOX.SelectionChangeCommitted += new System.EventHandler(this.SourcePathCBOX_SelectionChangeCommitted);
            this.SourcePathCBOX.DragDrop += new System.Windows.Forms.DragEventHandler(this.SourcePathCBOX_DragDrop);
            this.SourcePathCBOX.DragEnter += new System.Windows.Forms.DragEventHandler(this.KEIL2VSDragEnter);
            // 
            // gbKeilExePath
            // 
            this.gbKeilExePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbKeilExePath.Controls.Add(this.tbKeil_path);
            this.gbKeilExePath.Location = new System.Drawing.Point(5, 12);
            this.gbKeilExePath.Name = "gbKeilExePath";
            this.gbKeilExePath.Size = new System.Drawing.Size(574, 45);
            this.gbKeilExePath.TabIndex = 21;
            this.gbKeilExePath.TabStop = false;
            this.gbKeilExePath.Text = "Keil Program Path";
            // 
            // tbKeil_path
            // 
            this.tbKeil_path.AllowDrop = true;
            this.tbKeil_path.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbKeil_path.BackColor = System.Drawing.SystemColors.Window;
            this.tbKeil_path.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbKeil_path.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbKeil_path.Location = new System.Drawing.Point(7, 14);
            this.tbKeil_path.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbKeil_path.Name = "tbKeil_path";
            this.tbKeil_path.ReadOnly = true;
            this.tbKeil_path.Size = new System.Drawing.Size(560, 22);
            this.tbKeil_path.TabIndex = 2;
            this.tbKeil_path.Text = "Double-click or drag and drop The Keil Program  to here";
            // 
            // lbppd
            // 
            this.lbppd.AutoSize = true;
            this.lbppd.Location = new System.Drawing.Point(12, 123);
            this.lbppd.Name = "lbppd";
            this.lbppd.Size = new System.Drawing.Size(189, 14);
            this.lbppd.TabIndex = 23;
            this.lbppd.Text = "Add PreCompiled definition";
            this.lbppd.Click += new System.EventHandler(this.lbppd_Click);
            // 
            // tb_predef
            // 
            this.tb_predef.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_predef.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_predef.Location = new System.Drawing.Point(207, 118);
            this.tb_predef.Name = "tb_predef";
            this.tb_predef.Size = new System.Drawing.Size(365, 26);
            this.tb_predef.TabIndex = 24;
            this.tb_predef.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_predef_KeyDown);
            this.tb_predef.Leave += new System.EventHandler(this.tb_predef_TextChanged);
            // 
            // sourcepathTip
            // 
            this.sourcepathTip.IsBalloon = true;
            this.sourcepathTip.ToolTipTitle = "Pooppoo";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 688);
            this.Controls.Add(this.tb_predef);
            this.Controls.Add(this.lbppd);
            this.Controls.Add(this.gbKeilExePath);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbSourceGroup);
            this.Controls.Add(this.gbTargetInfo);
            this.Controls.Add(this.gbTargetGroup);
            this.Controls.Add(this.CreateButton);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Convert Keil Project to Visual Studio Project";
            this.Load += new System.EventHandler(this.Keil2VS_Load);
            this.Shown += new System.EventHandler(this.Keil2VS_Shown);
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
    }
}


