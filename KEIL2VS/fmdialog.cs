﻿using System;
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
    public partial class Fmdialog : Form
    {
        public Fmdialog(string tipsMessage = "成功地将Keil Project 转为Visual Studio Project！\n请重载（如已经打开）或者打开当前Project ，享受visual Studi" +
   "o 带来的高效和便利吧")
        {
            Fmdialog.TipMessage = tipsMessage;
            InitializeComponent();
        }
        static NextAction nextAction = NextAction.None;

        enum NextAction
        {
            None,
            OPenAndTrack,
            OpenOnly,
            OPenFolder,
        }

        private void BtnAction(object sender, EventArgs e)
        {
            if ((Button)sender == btnCancel)
            {

            }

        }
    }
}
