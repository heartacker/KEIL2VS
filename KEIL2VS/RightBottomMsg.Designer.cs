using System.Windows.Forms;

namespace KEIL2VS
{
    partial class RightBottomMsg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
        private void InitializeComponent()
        {
            this.btnIgnore = new System.Windows.Forms.Button();
            this.btnUpdataToVSp = new System.Windows.Forms.Button();
            this.rtbKeilIsChange = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnIgnore
            // 
            this.btnIgnore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIgnore.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnIgnore.FlatAppearance.BorderSize = 0;
            this.btnIgnore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIgnore.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F);
            this.btnIgnore.Location = new System.Drawing.Point(130, 143);
            this.btnIgnore.Name = "btnIgnore";
            this.btnIgnore.Size = new System.Drawing.Size(106, 42);
            this.btnIgnore.TabIndex = 1;
            this.btnIgnore.Text = "忽略当前更改";
            this.btnIgnore.UseVisualStyleBackColor = false;
            this.btnIgnore.Click += new System.EventHandler(this.BtnUpdata_Click);
            // 
            // btnUpdataToVSp
            // 
            this.btnUpdataToVSp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdataToVSp.AutoEllipsis = true;
            this.btnUpdataToVSp.BackColor = System.Drawing.Color.YellowGreen;
            this.btnUpdataToVSp.FlatAppearance.BorderSize = 0;
            this.btnUpdataToVSp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdataToVSp.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUpdataToVSp.Location = new System.Drawing.Point(251, 143);
            this.btnUpdataToVSp.Name = "btnUpdataToVSp";
            this.btnUpdataToVSp.Size = new System.Drawing.Size(117, 42);
            this.btnUpdataToVSp.TabIndex = 0;
            this.btnUpdataToVSp.Text = "更新到VS工程";
            this.btnUpdataToVSp.UseVisualStyleBackColor = false;
            this.btnUpdataToVSp.Click += new System.EventHandler(this.BtnUpdata_Click);
            // 
            // rtbKeilIsChange
            // 
            this.rtbKeilIsChange.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbKeilIsChange.BackColor = System.Drawing.SystemColors.Info;
            this.rtbKeilIsChange.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbKeilIsChange.Cursor = System.Windows.Forms.Cursors.No;
            this.rtbKeilIsChange.Enabled = false;
            this.rtbKeilIsChange.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtbKeilIsChange.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.rtbKeilIsChange.Location = new System.Drawing.Point(12, 12);
            this.rtbKeilIsChange.Name = "rtbKeilIsChange";
            this.rtbKeilIsChange.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtbKeilIsChange.Size = new System.Drawing.Size(356, 119);
            this.rtbKeilIsChange.TabIndex = 2;
            this.rtbKeilIsChange.Text = "";
            // 
            // RightBottomMsg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 202);
            this.Controls.Add(this.rtbKeilIsChange);
            this.Controls.Add(this.btnUpdataToVSp);
            this.Controls.Add(this.btnIgnore);
            this.Name = "RightBottomMsg";
            this.Text = "RightBottomMsg";
            this.Load += new System.EventHandler(this.RightBottomMsg_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnIgnore;
        private System.Windows.Forms.Button btnUpdataToVSp;
        public System.Windows.Forms.RichTextBox rtbKeilIsChange;
    }
}