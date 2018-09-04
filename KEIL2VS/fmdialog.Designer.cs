namespace KEIL2VS
{
    partial class fmdialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        const string str = "成功地将Keil Project 转为Visual Studio Project！\n请重载（如已经打开）或者打开当前Project ，享受visual Studi" +
   "o 带来的高效和便利吧";

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent(string TipMessage = str)
        {
            this.btnOpenAndtrack = new System.Windows.Forms.Button();
            this.rtbDialog = new System.Windows.Forms.RichTextBox();
            this.btnOpenOnly = new System.Windows.Forms.Button();
            this.btmOpenFolderOnly = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOpenAndtrack
            // 
            this.btnOpenAndtrack.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOpenAndtrack.Location = new System.Drawing.Point(16, 95);
            this.btnOpenAndtrack.Name = "btnOpenAndtrack";
            this.btnOpenAndtrack.Size = new System.Drawing.Size(87, 44);
            this.btnOpenAndtrack.TabIndex = 0;
            this.btnOpenAndtrack.Text = "打开并监控此工程";
            this.btnOpenAndtrack.UseVisualStyleBackColor = true;
            this.btnOpenAndtrack.Click += new System.EventHandler(this.btnOpenAndtrack_Click);
            // 
            // rtbDialog
            // 
            this.rtbDialog.BackColor = System.Drawing.SystemColors.Control;
            this.rtbDialog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbDialog.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtbDialog.Location = new System.Drawing.Point(16, 12);
            this.rtbDialog.Name = "rtbDialog";
            this.rtbDialog.Size = new System.Drawing.Size(399, 77);
            this.rtbDialog.TabIndex = 1;
            this.rtbDialog.Text = TipMessage;
            // 
            // btnOpenOnly
            // 
            this.btnOpenOnly.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOpenOnly.Location = new System.Drawing.Point(136, 95);
            this.btnOpenOnly.Name = "btnOpenOnly";
            this.btnOpenOnly.Size = new System.Drawing.Size(61, 44);
            this.btnOpenOnly.TabIndex = 2;
            this.btnOpenOnly.Text = "仅打开此工程";
            this.btnOpenOnly.UseVisualStyleBackColor = true;
            // 
            // btmOpenFolderOnly
            // 
            this.btmOpenFolderOnly.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btmOpenFolderOnly.Location = new System.Drawing.Point(230, 95);
            this.btmOpenFolderOnly.Name = "btmOpenFolderOnly";
            this.btmOpenFolderOnly.Size = new System.Drawing.Size(75, 44);
            this.btmOpenFolderOnly.TabIndex = 3;
            this.btmOpenFolderOnly.Text = "打开当前工程文件夹";
            this.btmOpenFolderOnly.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.Location = new System.Drawing.Point(338, 95);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(63, 44);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "什么都不做";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // fmdialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 157);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btmOpenFolderOnly);
            this.Controls.Add(this.btnOpenOnly);
            this.Controls.Add(this.rtbDialog);
            this.Controls.Add(this.btnOpenAndtrack);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "fmdialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "生成成功~~";
            this.TransparencyKey = System.Drawing.Color.Transparent;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOpenAndtrack;
        private System.Windows.Forms.RichTextBox rtbDialog;
        private System.Windows.Forms.Button btnOpenOnly;
        private System.Windows.Forms.Button btmOpenFolderOnly;
        private System.Windows.Forms.Button btnCancel;
    }
}