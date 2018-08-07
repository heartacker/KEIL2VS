using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Table2Code
{
    class CodeGenerator
    {

        CodeTemplate template;
        StringBuilder CodeSb = new StringBuilder();
        string rn = "\r\n";
        public enum ERROR
        {
            NO_ERROR,
            THE_ANGLE_ERROR,
            INPUT_NAME_IS_NULL,
            DEFAULT_VALUE_IS_NULL,
            BIT_WIDTH_NOT_MATCH,
            BLOCK_NAME_NOT_MATCH,
            VAL_DOMAIN_NOT_MATCH,
            INPUT_IS_NOT_BIN_VALUE,
            // "请确认全角半角"
        }
        public enum VOL_domain_en
        {
            LVCC,
            DVDD,
            VREG_PLL0,
            VREG_PLL1
        }
        public enum BLOCK_name_en
        {
            Null_BN,
            SPARE_PINS,
            ATB_DTB_CONTROL,
            POWER_DOWN,
            BIAS_MASTER,
            REFCLK_CTRL,
            MISC_COMP,
            MISC_ADC,
            MISC_TSENSOR,
            MISC_VGSA,
            MISC_RPOLY,
            PLL0_VREG,
            PLL0_PFD,
            PLL0_VREFGEN,
            PLL0_CP,
            PLL0_DLF,
            PLL0_VCO,
            PLL0_KADIV,
            PLL0_FBPATH,
            PLL0_FRACN,
            PLL0_HSCLK,
            PLL1_VREG,
            PLL1_PFD,
            PLL1_VREFGEN,
            PLL1_CP,
            PLL1_DLF,
            PLL1_VCO,
            PLL1_KADIV,
            PLL1_FBPATH,
            PLL1_FRACN,
            PLL1_HSCLK,
            D2A_Clock,
            A2D_Signal
        }


        public struct Module
        {
            public string name;
            public string[] FixPara;
            public string[] FixPara_declare;
            public string[] argu;
            public string end;
        }

        public struct Analog
        {
            public string name;
            public string Init_begin;
            public string Init_Fix;
            public string Init_end;
            public string Time_begin;
            public string end;
            public Analog(string _name, string _Init_begin, string _Init_Fix, string _Init_end, string _Time_begin, string _end)
            {
                name = _name;
                Init_begin = _Init_begin;
                Init_Fix = _Init_Fix;
                Init_end = _Init_end;
                Time_begin = _Time_begin;
                end = _end;
            }
        }


        public struct Declare_meta
        {
            public string key;
            public string name;
            public string bit_format;
            public short bit;
            public Declare_meta(string k, string n)
            {
                key = k;
                name = n;
                bit_format = "";
                bit = 0;
            }
            public Declare_meta(string k, string n, string f, short b)
            {
                key = k;
                name = n;
                bit_format = f;
                bit = b;
            }
            public string Printf()
            {
                if (key == "electrical")
                {
                    return key + " " + bit_format + " " + name + ";";
                }

                return key + "\t\t" + bit_format + " " + name + ";";

            }
        }

        #region Inputpara
        public struct In_p_declare
        {
            public Declare_meta input;
            public Declare_meta eLectrical;
            public In_p_declare(string name)
            {
                input = new Declare_meta("input", name);
                eLectrical = new Declare_meta("electrical", name);
            }
            public In_p_declare(string name, short b)
            {
                string fm = "[" + (b - 1) + ":" + "0]";
                input = new Declare_meta("input", name, fm, b);
                eLectrical = new Declare_meta("electrical", name, fm, b);
            }
        }

        public struct In_a2d_para_ST
        {
            public String block_name;
            public string flexpara;
            public VOL_domain_en volDomain;
            public ulong default_value;
            public short bit_width;
            public In_p_declare inPutPara;
            public In_a2d_para_ST(string bn, string psName, VOL_domain_en vdomain, ulong defV, short bw)
            {
                block_name = bn;
                flexpara = psName;
                volDomain = vdomain;
                default_value = defV;
                bit_width = bw;
                if (bw > 1)
                {
                    inPutPara = new In_p_declare(psName, bw);
                }
                else
                {
                    inPutPara = new In_p_declare(psName);
                }
            }
        }
        #endregion




        #region Out_para
        public struct Out_p_declare
        {
            public Declare_meta outPut;
            //{
            //    get { return (string)outPut.key + "\t"};
            //}
            public Declare_meta eLectrical;
            public Declare_meta[] inTeger;
            public Out_p_declare(string name, short b)
            {
                string fm = "[" + (b - 1) + ":" + "0]";
                outPut = new Declare_meta("output", name, fm, b);
                eLectrical = new Declare_meta("electrical", name, fm, b);
                inTeger = new Declare_meta[b];
                for (int i = 0; i < b; i++)
                {
                    inTeger[i] = new Declare_meta("integer", "val_" + name + "_bit" + i);
                }
            }
            public Out_p_declare(string name)
            {
                outPut = new Declare_meta("output", name);
                eLectrical = new Declare_meta("electrical", name);
                inTeger = new Declare_meta[1]
                {
                    new Declare_meta("integer", "val_"+name)
                };
            }
        }
        public struct Ou_d2a_para_ST
        {

            public string block_name;
            public string para;//canshu de mingzi
            public VOL_domain_en volDomain;
            public ulong default_value;
            public short bit_width;
            public Out_p_declare outPutPara;

            public Ou_d2a_para_ST(string bn, string psName, VOL_domain_en vdomain, ulong defV, short bw)
            {
                block_name = bn;
                para = psName;
                volDomain = vdomain;
                default_value = defV;

                bit_width = bw;
                if (bw > 1)
                {
                    outPutPara = new Out_p_declare(psName, bw);

                }
                else
                {
                    outPutPara = new Out_p_declare(psName);
                }
            }
            /// <summary>
            /// printf             The assignment operation 
            /// </summary>
            /// <returns></returns>
            public string AssOper(sbyte tab_num)
            {
                if (tab_num > 10)
                {
                    tab_num = 5;
                }
                string tabStr = "";
                for (int j = 0; j < tab_num; j++)
                {
                    tabStr += "\t";
                }
                StringBuilder sb = new StringBuilder();
                if (outPutPara.inTeger.Length == 1)
                {
                    sb.Append(string.Format(tabStr + "V({0}) <+ {1} * val_{2};", para, outPutPara.inTeger[0].name, volDomain.ToString().ToLower()) + "\r\n");

                }
                else
                {
                    for (int i = outPutPara.inTeger.Length - 1; i >= 0; i--)
                    {
                        sb.Append(string.Format(tabStr + "V({0}[{1}]) <+ {2} * val_{3};", para, i, outPutPara.inTeger[i].name, volDomain.ToString().ToLower()) + "\r\n");
                    }
                }
                //byte i = 0;
                //foreach (Declare_meta itg in outPutPara.inTeger)
                //{
                //    sb.Append(string.Format(tabStr + "V({0}[{1}]) <+ {2}*val_{3};", this.para, i, itg.name, volDomain.ToString().ToLower()) + "\r\n");
                //}
                return sb.ToString();
            }
        }
        #endregion


        public struct CodeTemplate
        {
            public string firstLine;
            public string include;

            public List<Ou_d2a_para_ST> ou_para_List;
            public List<In_a2d_para_ST> in_para_List;

            public Module module;
            public Analog analog;

        }
        private short StringBintoShort(string bin)
        {
            int bit_width = bin.Length;
            string one_bit_str = "";
            short One_bit = 0;
            short ret = 0;
            for (int i = 0; i < bit_width; i++)
            {
                one_bit_str = bin.Substring(bit_width - i - 1, 1);
                One_bit = short.Parse(one_bit_str);
                if (1 == One_bit)
                {
                    ret += (short)Math.Pow(2, i);
                }

            }
            return ret;
        }
        private string[] Strtobin(string str)
        {
            char[] cha = str.ToCharArray();
            string[] bin = new string[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                double stob = Convert.ToDouble(Convert.ToInt32(cha[i]));
                string temp = "";
                while (stob > 0)
                {
                    if (stob == 1)
                    {
                        temp += "1";
                        stob = 0;
                    }
                    else
                    {
                        temp += (stob % 2).ToString();
                        stob = Convert.ToInt32(Math.Floor(stob / 2));
                    }
                }
                for (int k = temp.Length - 1; k >= 0; k--)
                {
                    bin[i] += temp.Substring(k, 1);
                }
            }
            return bin;
        }

        public ulong ConvertPingNameToDefaultVaule(ref string pin_signal_name, ref string[] psname)
        {
            ulong bit_width;
            if (pin_signal_name.Replace(" ", "").EndsWith("]"))
            {
                psname = pin_signal_name.Split('[');
                bit_width = Convert.ToUInt64(psname[1].Substring(0, 2).Replace(":", "")) + 1;

            }
            else if (pin_signal_name.Replace(" ", "").EndsWith(">"))
            {
                psname = pin_signal_name.Split('<');
                bit_width = Convert.ToUInt64(psname[1].Substring(0, 2).Replace(":", "")) + 1;

            }
            else
            {
                psname = new string[1] { pin_signal_name.Replace(" ", "") };
                bit_width = 1;
            }
            return bit_width;
        }
        public ERROR ConvertStringToDefaultValue(string def_str, ref string defValue_bin, out ulong dev, out short bw, ref bool isNA)
        {
            bool isBin = false;
            dev = 0;
            if (def_str == @"n/a")
            {
                dev = 0;
                bw = 0;
                isNA = true;
                return ERROR.NO_ERROR;
            }
            if (def_str.Contains("b"))
            {
                isBin = true;
                //this is bin number
            }

            string[] bit_Value = def_str.Split('\'');
            if (bit_Value.Length != 2)
            {
                Debug.WriteLine("there have a error" + def_str + "请确认全角半角");
                bit_Value = def_str.Replace("’", "'").Split('\'');
                if (bit_Value.Length != 2)
                {
                    Debug.WriteLine("there have a error" + def_str + "请确认全角半角");
                    bit_Value = def_str.Replace("‘", "'").Split('\'');
                    if (bit_Value.Length != 2)
                    {
                        Debug.WriteLine("there have a error" + def_str + "请确认全角半角");
                        dev = 0;
                        bw = 0;
                        return ERROR.NO_ERROR;
                    }
                }
            }
            bw = Convert.ToByte(bit_Value[0]);
            bit_Value[1] = bit_Value[1].Remove(0, 1).Replace("_", "");
            if (isBin)
            {
                //dev = StringBintoShort(bit_Value[1]);
                defValue_bin = bit_Value[1];
                try
                {
                    dev = Convert.ToUInt64(bit_Value[1], 2);
                }
                catch
                {

                    return ERROR.INPUT_IS_NOT_BIN_VALUE;
                }


            }
            else
            {
                defValue_bin = bit_Value[1];
                try
                {
                    dev = Convert.ToUInt64(bit_Value[1], 16);
                }
                catch
                {
                    return ERROR.INPUT_IS_NOT_BIN_VALUE;
                }

            }
            return ERROR.NO_ERROR;

        }
        public ERROR Add_Arguments(string block_name, string pin_signal_name, string volDomain, string defaultValue)
        {
            string blkname = null;
            string[] psname = null;
            string defValue_bin = null;
            short bit_width = 0;
            bool isNA = false;
            VOL_domain_en vd_en;
            blkname = block_name.Replace(@" ", "_").Replace("&", "_");
            try
            {

                vd_en = (VOL_domain_en)Enum.Parse(typeof(VOL_domain_en), volDomain);
            }
            catch
            {

                return ERROR.VAL_DOMAIN_NOT_MATCH;
            }
            if (pin_signal_name == null)
            {
                return ERROR.INPUT_NAME_IS_NULL;
            }
            //get bit wide
            bit_width = (short)ConvertPingNameToDefaultVaule(ref pin_signal_name, ref psname);
            var error = ConvertStringToDefaultValue(defaultValue, ref defValue_bin, out ulong defV, out short bit_width_validation, ref isNA);
            if (error != ERROR.NO_ERROR)
            {
                return error;
            }

            //if bit =0.error
            if ((bit_width == 0) && pin_signal_name.StartsWith("d2a"))
            {
                //MessageBox.Show("PLEASE Check the angle,please!", "tired", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ERROR.THE_ANGLE_ERROR;
            }
            if ((bit_width_validation != bit_width) && pin_signal_name.StartsWith("d2a"))
            {
                return ERROR.BIT_WIDTH_NOT_MATCH;
            }

            if ((!isNA) && defValue_bin != null && defValue_bin.Length != bit_width)
            {
                return ERROR.BIT_WIDTH_NOT_MATCH;
            }





            if (pin_signal_name.StartsWith("a2d"))
            {
                template.in_para_List.Add(new In_a2d_para_ST(block_name, psname[0], vd_en, defV, bit_width));
                //Out_d2a_para_ST(BLOCK_name_en bn, string psName, VOL_domain_en vdomain, short defV, short bw)
                //this is input signae
            }
            if (pin_signal_name.StartsWith("d2a"))
            {
                template.ou_para_List.Add(new Ou_d2a_para_ST(block_name, psname[0], vd_en, defV, bit_width));
                //this is argument;
            }
            return ERROR.NO_ERROR;
        }
        public StringBuilder Generate()
        {
            CodeTemplate t = template;
            CodeSb.Append(@"//" + t.firstLine + rn);
            CodeSb.Append(t.include + rn);
            CodeSb.Append(t.module.name);
            foreach (var item in t.module.FixPara)
            {
                CodeSb.Append(item + ',');
            }
            /********************************
             * 
             * add input argument
             * 
             * 
             ***************************************** */
            // output
            foreach (Ou_d2a_para_ST ou_para in t.ou_para_List)
            {
                CodeSb.Append(ou_para.para + ",");
            }
            //input
            foreach (In_a2d_para_ST in_para in t.in_para_List)
            {
                CodeSb.Append(in_para.flexpara + ",");
            }

            CodeSb.Remove(CodeSb.Length - 1, 1);//去除逗号
            CodeSb.Append(");" + rn);
            CodeSb.Append(rn);
            foreach (string str in t.module.FixPara_declare)
            {
                CodeSb.Append(str + rn);
            }
            CodeSb.Append(rn);
            /**************************************    
             *              * add input declare
             **************************************/
            foreach (Ou_d2a_para_ST ou_para in t.ou_para_List)
            {
                CodeSb.Append("\t" + ou_para.outPutPara.outPut.Printf() + rn);
                CodeSb.Append("\t" + ou_para.outPutPara.eLectrical.Printf() + rn);
            }
            foreach (In_a2d_para_ST in_para in t.in_para_List)
            {
                CodeSb.Append("\t" + in_para.inPutPara.input.Printf() + rn);
                CodeSb.Append("\t" + in_para.inPutPara.eLectrical.Printf() + rn);
            }
            CodeSb.Append(rn);

            foreach (Ou_d2a_para_ST ou_para in t.ou_para_List)
            {
                for (int i = ou_para.outPutPara.inTeger.Length - 1; i >= 0; i--)
                {
                    CodeSb.Append("\t" + ou_para.outPutPara.inTeger[i].Printf() + rn);
                }
                //foreach (Declare_meta inter in ou_para.outPutPara.inTerger)
                //{
                //    CodeSb.Append("\t" + inter.Printf() + rn);
                //}
            }
            CodeSb.Append(rn);
            CodeSb.Append(t.analog.name).Append(t.analog.Init_begin);
            CodeSb.Append(t.analog.Init_Fix + rn);
            /***********************************************
             临时变量操作 todo
             * *******************************************/
            string BLOCK_NAME = "";
            foreach (Ou_d2a_para_ST ou in t.ou_para_List)
            {
                if (ou.block_name != BLOCK_NAME)
                {
                    CodeSb.Append(rn);
                    CodeSb.Append("\t\t\t//" + ou.block_name + rn);
                }
                BLOCK_NAME = ou.block_name;
                for (int i = ou.outPutPara.inTeger.Length - 1; i >= 0; i--)
                {
                    CodeSb.Append("\t\t\t" + ou.outPutPara.inTeger[i].name + " = " + (ou.default_value >> i & 1) + ";\r\n");
                }
            }
            CodeSb.Append(rn);
            CodeSb.Append(t.analog.Init_end + rn);
            CodeSb.Append(rn);
            CodeSb.Append(t.analog.Time_begin + rn);
            CodeSb.Append(rn);

            /***********************
             赋值操作
                         The assignment operation
             */
            BLOCK_NAME = "";
            foreach (Ou_d2a_para_ST ou in t.ou_para_List)
            {
                BLOCK_NAME = ou.block_name;
                if (ou.block_name != BLOCK_NAME)
                {
                    CodeSb.Append(rn);
                    CodeSb.Append("\t\t\t\t//" + ou.block_name + rn);
                }
                CodeSb.Append(ou.AssOper(2));
            }
            CodeSb.Append(rn);

            CodeSb.Append(t.analog.end + rn);
            CodeSb.Append(t.module.end);

            return CodeSb;
        }
        public void Init(string F_name)
        {
            CodeSb.Clear();
            template = new CodeTemplate
            {
                firstLine = "VerilogA for h60_t7_CSTESTBENCH,h60t7cs_clockslice-dig_sim_temp2,veriloga",
                include = "`include \"constants.vams\" \r\n`include \"disciplines.vams\"",
                module = new Module(),
                analog = new Analog(),
                in_para_List = new List<In_a2d_para_ST>(),
                ou_para_List = new List<Ou_d2a_para_ST>()
            };
            template.module.name = @"module " + F_name + "(";
            template.module.FixPara = new[] {
                "dvdd",
                "dvddce",
                "dvss",
                "lvcc",
                "vreg_pll0",
                "vreg_pll1"
            };
            /*****************************
             * 
             * 
             * para 参数操作
             * 
             * 
             * 
             * */
            template.module.FixPara_declare = new string[3];
            template.module.FixPara_declare[0] = "\t" + "input" + "\t\t";
            template.module.FixPara_declare[1] = "\t" + "electrical" + "\t";
            template.module.FixPara_declare[2] = "\t" + "real" + "\t\t";

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < template.module.FixPara.Length; j++)
                {
                    if (i == 2)
                    {
                        template.module.FixPara_declare[i] += "val_" + template.module.FixPara[j] + ((j < template.module.FixPara.Length - 1) ? ',' : ';');
                    }
                    else
                    {

                        template.module.FixPara_declare[i] += template.module.FixPara[j] + ((j < template.module.FixPara.Length - 1) ? ',' : ';');
                    }
                }
            }
            /********************
             *              * 
             * 声明操作             * 
             * 
             * 
             * ******************/

            template.analog.name = "\tanalog begin\r\n";
            template.analog.Init_begin = "\t\t" + @"@(initial_step) begin" + rn;
            string tempstr = null;
            for (VOL_domain_en i = VOL_domain_en.LVCC; i <= VOL_domain_en.VREG_PLL1; i++)
            {
                tempstr += "\t\t\tval_" + i.ToString().ToLower() + " = V(" + i.ToString().ToLower() + ");" + rn;

            }
            template.analog.Init_Fix = tempstr;
            /**********************
             临时变量操作 todo
            ***********************/
            template.analog.Init_end = "\t\tend" + "  //initial_step";
            template.analog.Time_begin = "\t\t" + @"@(timer(1n, 1n)) begin" + rn + tempstr + "\t\tend";

            /**********************
             赋值操作 todo
            ***********************/

            template.analog.end = "\tend" + "  //analog" + "\r\n";
            template.module.end = "endmodule" + "\r\n";

        }
    }
}
