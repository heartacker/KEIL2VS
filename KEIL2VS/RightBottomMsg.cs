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
        static string MessageTip;
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

        private void btnUpdataToVSp_Click(object sender, EventArgs e)
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
    }
}
