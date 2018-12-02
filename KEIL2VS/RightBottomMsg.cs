using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KEIL2VS
{
    public partial class RightBottomMsg : Form
    {
        public UPDatePara uPDatePara = UPDatePara.NONE;
        public RightBottomMsg(string MessageTip = "当前Keil工程已经被修改，是否需要更新到VS Project？")
        {
            InitializeComponent();
            this.rtbKeilIsChange.Text = MessageTip;
        }
        public enum UPDatePara
        {
            NONE,
            UPDATA,
            IGNORE,
            CANCLE
        }

        //声明常量：(释义可参见windows API)

        const int WS_EX_NOACTIVATE = 0x08000000;

        //重载Form的CreateParams属性，添加不获取焦点属性值。
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_NOACTIVATE;
                return cp;
            }
        }


        private void BtnUpdata_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn == btnUpdataToVSp)
            {
                uPDatePara = UPDatePara.UPDATA;
            }
            else if (btn == btnIgnore)
            {
                uPDatePara = UPDatePara.IGNORE;
            }
            this.Close();
        }

        private void RightBottomMsg_Load(object sender, EventArgs e)
        {
            this.MaximumSize = new Size(this.Width, this.Height);
        }
    }
}
