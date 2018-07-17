using System.Windows.Forms;

namespace KEIL2VSPROJ
{
	// Token: 0x02000002 RID: 2
	public partial class Keil2VS :Form
	{
		// Token: 0x0600001F RID: 31 RVA: 0x00004499 File Offset: 0x00002699
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000044B8 File Offset: 0x000026B8
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Keil2VS));
            this.CreateButton = new Button();
            this.groupBox3 = new GroupBox();
            this.elementHost = new System.Windows.Forms.Integration.ElementHost();
            this.groupBox4 = new GroupBox();
            this.TargetStatus = new TextBox();
            this.groupBox5 = new GroupBox();
            this.FileBox = new ListBox();
            this.GroupListBox = new ListBox();
            this.groupBox1 = new GroupBox();
            this.btnOpenFolder = new Button();
            this.SourcePathCBOX = new ComboBox();
            this.groupBox2 = new GroupBox();
            this.tbKeil_path = new TextBox();
            this.lbppd = new Label();
            this.tb_predef = new TextBox();
            this.sourcepathTip = new ToolTip(this.components);
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // CreateButton
            // 
            this.CreateButton.Anchor = ((AnchorStyles)(((AnchorStyles.Bottom | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.CreateButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.CreateButton.Font = new System.Drawing.Font("Consolas", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreateButton.Location = new System.Drawing.Point(5, 656);
            this.CreateButton.Margin = new Padding(3, 4, 3, 4);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Padding = new Padding(3, 4, 3, 4);
            this.CreateButton.Size = new System.Drawing.Size(779, 42);
            this.CreateButton.TabIndex = 8;
            this.CreateButton.Text = "Create Visual Studio Project";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.elementHost);
            this.groupBox3.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(5, 147);
            this.groupBox3.Margin = new Padding(3, 4, 3, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new Padding(3, 4, 3, 4);
            this.groupBox3.Size = new System.Drawing.Size(779, 48);
            this.groupBox3.TabIndex = 20;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Target Groups";
            // 
            // elementHost
            // 
            this.elementHost.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) 
            | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.elementHost.Location = new System.Drawing.Point(6, 16);
            this.elementHost.Name = "elementHost";
            this.elementHost.Size = new System.Drawing.Size(767, 25);
            this.elementHost.TabIndex = 11;
            this.elementHost.Text = "elementHost1";
            this.elementHost.Child = null;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.TargetStatus);
            this.groupBox4.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(5, 199);
            this.groupBox4.Margin = new Padding(3, 4, 3, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new Padding(3, 4, 3, 4);
            this.groupBox4.Size = new System.Drawing.Size(779, 158);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Target Info";
            // 
            // TargetStatus
            // 
            this.TargetStatus.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) 
            | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.TargetStatus.BackColor = System.Drawing.SystemColors.Window;
            this.TargetStatus.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TargetStatus.Location = new System.Drawing.Point(5, 19);
            this.TargetStatus.Margin = new Padding(3, 4, 3, 4);
            this.TargetStatus.Multiline = true;
            this.TargetStatus.Name = "TargetStatus";
            this.TargetStatus.ReadOnly = true;
            this.TargetStatus.ScrollBars = ScrollBars.Both;
            this.TargetStatus.Size = new System.Drawing.Size(767, 134);
            this.TargetStatus.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) 
            | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.FileBox);
            this.groupBox5.Controls.Add(this.GroupListBox);
            this.groupBox5.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(5, 363);
            this.groupBox5.Margin = new Padding(3, 4, 3, 4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new Padding(3, 4, 3, 4);
            this.groupBox5.Size = new System.Drawing.Size(779, 285);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Source Groups";
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
            this.FileBox.Location = new System.Drawing.Point(264, 16);
            this.FileBox.Name = "FileBox";
            this.FileBox.Size = new System.Drawing.Size(508, 264);
            this.FileBox.TabIndex = 1;
            // 
            // GroupListBox
            // 
            this.GroupListBox.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Bottom) 
            | AnchorStyles.Left)));
            this.GroupListBox.BackColor = System.Drawing.SystemColors.Window;
            this.GroupListBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupListBox.FormattingEnabled = true;
            this.GroupListBox.HorizontalScrollbar = true;
            this.GroupListBox.IntegralHeight = false;
            this.GroupListBox.ItemHeight = 14;
            this.GroupListBox.Location = new System.Drawing.Point(5, 16);
            this.GroupListBox.Margin = new Padding(3, 4, 3, 4);
            this.GroupListBox.Name = "GroupListBox";
            this.GroupListBox.Size = new System.Drawing.Size(255, 264);
            this.GroupListBox.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnOpenFolder);
            this.groupBox1.Controls.Add(this.SourcePathCBOX);
            this.groupBox1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(5, 64);
            this.groupBox1.Margin = new Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(779, 47);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Keil Project File Path";
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnOpenFolder.Location = new System.Drawing.Point(703, 13);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(69, 30);
            this.btnOpenFolder.TabIndex = 1;
            this.btnOpenFolder.Text = "Open";
            this.btnOpenFolder.UseVisualStyleBackColor = false;
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // SourcePathCBOX
            // 
            this.SourcePathCBOX.AllowDrop = true;
            this.SourcePathCBOX.FormattingEnabled = true;
            this.SourcePathCBOX.Items.AddRange(new object[] {
            "Double-click or drag and drop The MDK the Project File to here",
            "NULL"});
            this.SourcePathCBOX.Location = new System.Drawing.Point(5, 17);
            this.SourcePathCBOX.Name = "SourcePathCBOX";
            this.SourcePathCBOX.Size = new System.Drawing.Size(692, 22);
            this.SourcePathCBOX.TabIndex = 0;
            this.SourcePathCBOX.Text = "Open Or drop The MDK the Project File to here";
            this.SourcePathCBOX.DragDrop += new DragEventHandler(this.SourcePathCBOX_DragDrop);
            this.SourcePathCBOX.DragEnter += new DragEventHandler(this.KEIL2VSDragEnter);
            this.SourcePathCBOX.MouseDoubleClick += new MouseEventHandler(this.btnOpenFolder_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbKeil_path);
            this.groupBox2.Location = new System.Drawing.Point(5, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(779, 45);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Keil Program Path";
            // 
            // tbKeil_path
            // 
            this.tbKeil_path.AllowDrop = true;
            this.tbKeil_path.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) 
            | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.tbKeil_path.BackColor = System.Drawing.SystemColors.Window;
            this.tbKeil_path.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbKeil_path.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.tbKeil_path.Location = new System.Drawing.Point(7, 14);
            this.tbKeil_path.Margin = new Padding(3, 4, 3, 4);
            this.tbKeil_path.Name = "tbKeil_path";
            this.tbKeil_path.ReadOnly = true;
            this.tbKeil_path.Size = new System.Drawing.Size(765, 22);
            this.tbKeil_path.TabIndex = 2;
            this.tbKeil_path.Text = "Double-click or drag and drop The Keil Program  to here";
            // 
            // lbppd
            // 
            this.lbppd.AutoSize = true;
            this.lbppd.Location = new System.Drawing.Point(12, 123);
            this.lbppd.Name = "lbppd";
            this.lbppd.Size = new System.Drawing.Size(161, 14);
            this.lbppd.TabIndex = 23;
            this.lbppd.Text = "PreCompiled definition";
            this.lbppd.Click += new System.EventHandler(this.lbppd_Click);
            // 
            // tb_predef
            // 
            this.tb_predef.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_predef.Location = new System.Drawing.Point(184, 118);
            this.tb_predef.Name = "tb_predef";
            this.tb_predef.Size = new System.Drawing.Size(593, 26);
            this.tb_predef.TabIndex = 24;
            this.tb_predef.KeyDown += new KeyEventHandler(this.tb_predef_KeyDown);
            this.tb_predef.Leave += new System.EventHandler(this.tb_predef_TextChanged);
            // 
            // sourcepathTip
            // 
            this.sourcepathTip.IsBalloon = true;
            this.sourcepathTip.ToolTipTitle = "Pooppoo";
            // 
            // Keil2VS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 705);
            this.Controls.Add(this.tb_predef);
            this.Controls.Add(this.lbppd);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.CreateButton);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new Padding(3, 4, 3, 4);
            this.Name = "Keil2VS";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Keil Project Convert To Visual Studio Project";
            this.Load += new System.EventHandler(this.Keil2VS_Load);
            this.Shown += new System.EventHandler(this.Keil2VS_Shown);
            //this.MouseDown += new MouseEventHandler(this.Keil2VS_MouseDown);
            //this.MouseEnter += new System.EventHandler(this.Keil2VS_MouseEnter);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}


		private global::System.ComponentModel.IContainer components;


		private Button CreateButton;


		private GroupBox groupBox3;


		private GroupBox groupBox4;


		private TextBox TargetStatus;


		private GroupBox groupBox5;


		private ListBox GroupListBox;


		private GroupBox groupBox1;

		private ListBox FileBox;

		private System.Windows.Forms.Integration.ElementHost elementHost;
        private GroupBox groupBox2;
        private TextBox tbKeil_path;
        private Label lbppd;
        private TextBox tb_predef;
        private ComboBox SourcePathCBOX;
        private Button btnOpenFolder;
        private ToolTip sourcepathTip;
    }
}
