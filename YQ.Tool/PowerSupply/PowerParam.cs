using System;
using System.Collections.Generic;

namespace YQ.Tool
{
    [Serializable]
    public class PowerParam
    {

        /// <summary>
        /// <para>盛迪、新跃 - 相线：0：单相, 1：三相四线有功, 2：三相三线有功, 3：90 度无功, 4：60 度无功, 5：四线正弦无功, 6：三线正弦无功, 7：单相无功</para>
        /// <para>国网 - 接线方式：0：PT4 三相四线有功, 1：QT4 三相四线无功, 2：P32 三相三线有功, 3：Q32 三相三线无功。单相表接线方式选择 0。</para>
        /// </summary>
        public int Phase { set; get; }

        /// <summary>
        ///电压电流输出控制：1Byte 的数值；0-2Bit 分别表示 A、B、C 相电压，3-5Bit 分别表示 A、B、C 相电流；1：表示输出，0：表示不输出。
        /// </summary>
        public byte PowerFlag { set; get; }

        /// <summary>
        /// 被校表额定频率,范围 45~65
        /// </summary>
        public double Rated_Freq { set; get; }
        /// <summary>
        /// A相电压(V)
        /// </summary>
        public double Ua { get; set; }

        /// <summary>
        /// B相电压(V)
        /// </summary>
        public double Ub { get; set; }

        /// <summary>
        /// C相电压(V)
        /// </summary>
        public double Uc { get; set; }

        /// <summary>
        /// A相电压相角(°)
        /// </summary>
        public double Ua_Phase { get; set; }

        /// <summary>
        /// B相电压相角(°)
        /// </summary>
        public double Ub_Phase { get; set; }

        /// <summary>
        /// C相电压相角(°)
        /// </summary>
        public double Uc_Phase { get; set; }


        /// <summary>
        /// A相电流(A)
        /// </summary>
        public double Ia { get; set; }

        /// <summary>
        ///  A相电流(A)
        /// </summary>
        public double Ib { get; set; }

        /// <summary>
        ///  A相电流(A)
        /// </summary>
        public double Ic { get; set; }

        /// <summary>
        /// A相电流相角(°)
        /// </summary>
        public double Ia_Phase { get; set; }

        /// <summary>
        /// B相电流相角(°)
        /// </summary>
        public double Ib_Phase { get; set; }

        /// <summary>
        /// C相电流相角(°)
        /// </summary>
        public double Ic_Phase { get; set; }

        /// <summary>
        /// 谐波开关：1Byte 的数值；0-2Bit 分别表示 A、B、C 相电压，3-5Bit 分别表示 A、B、C 相电流；1：表示含谐波，0：表示不含谐波。]
        /// </summary>
        public byte HarmonicFlag { set; get; }

        /// <summary>
        /// 总有功功率(kW)
        /// </summary>
        public double P { get; set; }

        /// <summary>
        /// A相有功功率(kW)
        /// </summary>
        public double Pa { get; set; }

        /// <summary>
        /// B相有功功率(kW)
        /// </summary>
        public double Pb { get; set; }

        /// <summary>
        /// C相有功功率(kW)
        /// </summary>
        public double Pc { get; set; }

        /// <summary>
        /// 总无功功率(kW)
        /// </summary>
        public double Q { get; set; }

        /// <summary>
        /// A相无功功率(kW)
        /// </summary>
        public double Qa { get; set; }

        /// <summary>
        /// B相无功功率(kW)
        /// </summary>
        public double Qb { get; set; }

        /// <summary>
        /// C相无功功率(kW)
        /// </summary>
        public double Qc { get; set; }

        /// <summary>
        /// 总视在功率(kW)
        /// </summary>
        public double S { get; set; }

        /// <summary>
        /// A相视在功率(kW)
        /// </summary>
        public double Sa { get; set; }

        /// <summary>
        /// B相视在功率(kW)
        /// </summary>
        public double Sb { get; set; }

        /// <summary>
        /// C相视在功率(kW)
        /// </summary>
        public double Sc { get; set; }

        /// <summary>
        /// 总功率因数
        /// </summary>
        public double FP { get; set; }

        /// <summary>
        /// A相功率因数
        /// </summary>
        public double FPa { get; set; }

        /// <summary>
        /// B相功率因数
        /// </summary>
        public double FPb { get; set; }

        /// <summary>
        /// C相功率因数
        /// </summary>
        public double FPc { get; set; }

        /// <summary>
        /// 总功率因数角(°)
        /// </summary>
        public double FP_Angle { get; set; }

        /// <summary>
        /// 频率(Hz)
        /// </summary>
        public double Frequency { get; set; }

        /// <summary>
        /// 报警字 2字节bit0-bit15组成，位为1表示报警，位为0表示正常。
        /// <para>UA(bit0);UB(bit1);UC(bit2);IA(bit3);IB(bit4);IC(bit5);CT(bit6);其他(bit7);均没有报警时为0000H</para>
        /// </summary>
        public string AlarmCode { get; set; } = "0000";

        /// <summary>
        /// 报警信息
        /// </summary>
        public string AlarmMessage { get; set; } = "null";

        /// <summary>
        /// Ua 和 Ub 的夹角
        /// </summary>
        public double Uab { get; set; }

        /// <summary>
        /// Ua 和 Uc 的夹角
        /// </summary>
        public double Uac { get; set; }

        ////相位角
        //float fltPhia = (float.Parse(data[10]) - float.Parse(data[4]) + 360) % 360;
        //float fltPhib = (float.Parse(data[12]) - float.Parse(data[6]) + 360) % 360;
        //float fltPhic = (float.Parse(data[14]) - float.Parse(data[8]) + 360) % 360;

        /// <summary>
        /// A相的电压电流夹角(°)
        /// </summary>
        public double AngA { get; set; }

        /// <summary>
        /// B相的电压电流夹角(°)
        /// </summary>
        public double AngB { get; set; }

        /// <summary>
        /// C相的电压电流夹角(°)
        /// </summary>
        public double AngC { get; set; }

        public bool Result { set; get; } = false;


        public PowerParam()
        {


        }

        /// <summary>
        /// 新跃的
        /// </summary>
        /// <param name="sValue"></param>
        public PowerParam(string[] sValue)
        {
            Ua = YQMath.GetDouble(sValue[0], 0, 2);
            Ub = YQMath.GetDouble(sValue[1], 0, 2);
            Uc = YQMath.GetDouble(sValue[2], 0, 2);
            Ia = YQMath.GetDouble(sValue[3], 0, 4);
            Ib = YQMath.GetDouble(sValue[4], 0, 4);
            Ic = YQMath.GetDouble(sValue[5], 0, 4);

            Ia_Phase = Ua_Phase = YQMath.GetDouble(sValue[6], 0, 2);
            Ib_Phase = Ub_Phase = YQMath.GetDouble(sValue[7], 0, 2);
            Ic_Phase = Uc_Phase = YQMath.GetDouble(sValue[8], 0, 2);
            Pa = YQMath.Div(YQMath.GetDouble(sValue[9]), 1000, 3);
            Pb = YQMath.Div(YQMath.GetDouble(sValue[10]), 1000, 3);
            Pc = YQMath.Div(YQMath.GetDouble(sValue[11]), 1000, 3);
            Qa = YQMath.Div(YQMath.GetDouble(sValue[12]), 1000, 3);
            Qb = YQMath.Div(YQMath.GetDouble(sValue[13]), 1000, 3);
            Qc = YQMath.Div(YQMath.GetDouble(sValue[14]), 1000, 3);
            Frequency = YQMath.GetDouble(sValue[15]);
            Q = Qa + Qb + Qc;
            P = Pa + Pb + Pc;
            S = Math.Round(Math.Sqrt(Math.Pow(P, 2) + Math.Pow(Q, 2)), 3);
            FP = S == 0 ? 0 : Math.Round(P / S, 3);

            Sa = Math.Round(Math.Sqrt(Math.Pow(Pa, 2) + Math.Pow(Qa, 2)), 3);
            Sb = Math.Round(Math.Sqrt(Math.Pow(Pb, 2) + Math.Pow(Qb, 2)), 3);
            Sc = Math.Round(Math.Sqrt(Math.Pow(Pc, 2) + Math.Pow(Qc, 2)), 3);

            FPa = Sa == 0 ? 0 : Math.Round(Pa / Sa, 3);
            FPb = Sb == 0 ? 0 : Math.Round(Pb / Sb, 3);
            FPc = Sc == 0 ? 0 : Math.Round(Pc / Sc, 3);

            SetUI2Zero();//处理感应电压，感应电流
        }
        /// <summary>
        /// 通过盛迪标准表字符串构造功率源参数
        /// </summary>
        /// <param name="shengdiString"></param>
        public PowerParam(string shengdiString, string type, bool Is3Phase)
        {
            if (type == "HS5100") //HS5100
            {
                //219.9939,5.0000,0.01,1099.8558,0.2150,1099.8558,49.9996
                // HS-5100/5102   U,I,φ,有功功率,无功功率,视在功率，频率 
                //  List<string> Data_StdMeter_Read = new List<string> { };
                string[] Data_String = shengdiString.Split(',');
                Ua = YQMath.GetDouble(Data_String[0], 0, 1);
                Ia = YQMath.GetDouble(Data_String[1], 0, 2);
                Ua_Phase = 0;
                Ub_Phase = 120;
                Uc_Phase = 240;
                AngA = YQMath.GetDouble(Data_String[2], 0, 0);
                Pa = YQMath.Div(YQMath.GetDouble(Data_String[3]), 1000, 2);
                Qa = YQMath.Div(YQMath.GetDouble(Data_String[4]), 1000, 2);
                Sa = YQMath.Div(YQMath.GetDouble(Data_String[5]), 1000, 2);
                Frequency = YQMath.GetDouble(Data_String[6]);

                FPa = YQMath.Div(Pa, Sa, 3);// FP = P / S
                Q = Qa;
                P = Pa;
                S = Sa;
                FP = YQMath.Div(P, S, 3);// FP = P / S 
            }
            else
            {
                //    219.9939,0,0,4.99971,0,0,59.99,0,0,
                //0,0,0,0,0,0,0,0,0,
                //550.13050,952.25950,1099.90450,49.9987,,A_0
                //盛迪标准表返回参数
                /* Ua,Ub,Uc,Ia,Ib,Ic,φa,φb,φc,   //φa,φb,φc电压电流的夹角
                 * Pa,Pb,Pc,Qa,Qb,Qc,Sa,Sb,Sc,
                 * 有功功率(总),无功功率(总),视在功率(总),频率,{电压档位},电流档位*/
                List<string> Data_StdMeter_Read = new List<string> { };
                string[] Data_String = shengdiString.Split(',');
                Ua = YQMath.GetDouble(Data_String[0], 0, 1);
                Ub = YQMath.GetDouble(Data_String[1], 0, 1);
                Uc = YQMath.GetDouble(Data_String[2], 0, 1);
                Ia = YQMath.GetDouble(Data_String[3], 0, 3);
                Ib = YQMath.GetDouble(Data_String[4], 0, 3);
                Ic = YQMath.GetDouble(Data_String[5], 0, 3);

                Ua_Phase = 0;
                Ub_Phase = 120;
                Uc_Phase = 240;
                AngA = YQMath.GetDouble(Data_String[6], 0, 0);
                AngB = YQMath.GetDouble(Data_String[7], 0, 0);
                AngC = YQMath.GetDouble(Data_String[8], 0, 0);

                Ia_Phase = (Ua_Phase + Math.Round(AngA / 10.0) * 10) % 360;
                Ib_Phase = (Ub_Phase + Math.Round(AngB / 10.0) * 10) % 360;
                Ic_Phase = (Uc_Phase + Math.Round(AngC / 10.0) * 10) % 360;

                P = YQMath.Div(YQMath.GetDouble(Data_String[18]), 1000, 3);//盛迪功率单位W
                Pa = YQMath.Div(YQMath.GetDouble(Data_String[9]), 1000, 3);
                Pb = YQMath.Div(YQMath.GetDouble(Data_String[10]), 1000, 3);
                Pc = YQMath.Div(YQMath.GetDouble(Data_String[11]), 1000, 3);
                Q = YQMath.Div(YQMath.GetDouble(Data_String[19]), 1000, 3);
                Qa = YQMath.Div(YQMath.GetDouble(Data_String[12]), 1000, 3);
                Qb = YQMath.Div(YQMath.GetDouble(Data_String[13]), 1000, 3);
                Qc = YQMath.Div(YQMath.GetDouble(Data_String[14]), 1000, 3);
                S = YQMath.Div(YQMath.GetDouble(Data_String[20]), 1000, 3);
                Sa = YQMath.Div(YQMath.GetDouble(Data_String[15]), 1000, 3);
                Sb = YQMath.Div(YQMath.GetDouble(Data_String[16]), 1000, 3);
                Sc = YQMath.Div(YQMath.GetDouble(Data_String[17]), 1000, 3);
                Frequency = YQMath.GetDouble(Data_String[21]);
                //AlarmCode = "null";
                //AlarmMessage = "null";
                FP = YQMath.Div(P, S, 5);// FP = P / S
                FPa = YQMath.Div(Pa, Sa, 5);// FPa = Pa / Sa
                FPb = YQMath.Div(Pb, Sb, 5);// FPb = Pb / Sb
                FPc = YQMath.Div(Pc, Sc, 5);// FPc = Pc / Sc
                FP_Angle = YQMath.Div(Math.Acos(FP) * 180, Math.PI, 2);// FP角度
                                                                         //  var config = LocalDB.GetSysConfig(2);
                if (!Is3Phase) //TODO:盛迪单相表没有各相的数值，只有总值，而国网显示A相值
                {
                    Pa = P;
                    FPa = FP;
                    Sa = S;
                    Qa = Q;
                }
            }
            SetUI2Zero();//处理感应电压，感应电流
        }

        /// <summary>
        /// 处理感应电压，感应电流
        /// </summary>
        public void SetUI2Zero()
        {

        }
        /// <summary>
        /// 转成字符串
        /// </summary>
        /// <returns></returns>
        //public override string ToString()
        //{
        //    /* A 相电压(V); A 相电压相角(°); B 相电压(V); B 相电压相角(°); C 相电压(V); C 相电压相角(°); 
        //     * A 相电流(A); A 相电流相角(°); B 相电流(A); B 相电流相角(°); C 相电流(A); C 相电流相角(°);
        //     * 总有功功率(kW); A 相有功功率(kW); B 相有功功率(kW); C 相有功功率(kW); 
        //     * 总无功功率(kvar); A 相无功功率(kvar); B 相无功功率(kvar); C 相无功功率(kvar); 
        //     * 总视在功率(kVA); A相视在功率(kVA); B 相视在功率(kVA); C 相视在功率(kVA);
        //     * 总功率因数; A 相功率因数; B 相功率因数; C 相功率因数; 总功率因数角(°); 
        //     * 频率(Hz);报警字;报警信息*/

        //    //return string.Format("0;1;2;3;4;5;6;7;8;9;10;11;12;13;14;15;16;17;18;19;20;21;22;23;24;25;26;27;28;29;30;31");
        //    string str = //string.Format("{0:F4};{1:F2};{2:F4};{3:F2};{4:F4};{5:F2};{6:F5};{7:F2};{8:F5};{9:F2};{10:F5};{11:F2};{12:F5};{13:F5};{14:F5};{15:F5};{16:F5};{17:F5};{18:F5};{19:F5};{20:F5};{21:F5};{22:F5};{23:F5};{24:F5};{25:F5};{26:F5};{27:F5};{28:F5};{29:F2};{30};{31}",
        //        string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19};{20};{21};{22};{23};{24};{25};{26};{27};{28};{29};{30};{31}",
        //        //string str = string.Format("{0:F3};{1:F0};{2:F3};{3:F0};{4:F3};{5:F0};{6:F3};{7:F0};{8:F3};{9:F0};{10:F3};{11:F0};{12:F5};{13:F5};{14:F5};{15:F5};{16:F5};{17:F5};{18:F5};{19:F5};{20:F5};{21:F5};{22:F5};{23:F5};{24:F5};{25:F5};{26:F5};{27:F5};{28:F2};{29};{30};{31}",
        //        Ua, Ua_Phase, Ub, Ub_Phase, Uc, Uc_Phase,
        //        Ia, Ia_Phase, Ib, Ib_Phase, Ic, Ic_Phase,
        //        P, Pa, Pb, Pc,
        //        Q, Qa, Qb, Qc,
        //        S, Sa, Sb, Sc,
        //        FP, FPa, FPb, FPc, FP_Angle,
        //        Frequency, AlarmCode, AlarmMessage);

        //    return str;
        //}
        public override string ToString()
        {
            /* A 相电压(V); A 相电压相角(°); B 相电压(V); B 相电压相角(°); C 相电压(V); C 相电压相角(°); 
             * A 相电流(A); A 相电流相角(°); B 相电流(A); B 相电流相角(°); C 相电流(A); C 相电流相角(°);
             * 总有功功率(kW); A 相有功功率(kW); B 相有功功率(kW); C 相有功功率(kW); 
             * 总无功功率(kvar); A 相无功功率(kvar); B 相无功功率(kvar); C 相无功功率(kvar); 
             * 总视在功率(kVA); A相视在功率(kVA); B 相视在功率(kVA); C 相视在功率(kVA);
             * 总功率因数; A 相功率因数; B 相功率因数; C 相功率因数; 总功率因数角(°); 
             * 频率(Hz);报警字;报警信息*/

            //return string.Format("0;1;2;3;4;5;6;7;8;9;10;11;12;13;14;15;16;17;18;19;20;21;22;23;24;25;26;27;28;29;30;31");
            string str = //string.Format("{0:F4};{1:F2};{2:F4};{3:F2};{4:F4};{5:F2};{6:F5};{7:F2};{8:F5};{9:F2};{10:F5};{11:F2};{12:F5};{13:F5};{14:F5};{15:F5};{16:F5};{17:F5};{18:F5};{19:F5};{20:F5};{21:F5};{22:F5};{23:F5};{24:F5};{25:F5};{26:F5};{27:F5};{28:F5};{29:F2};{30};{31}",
                string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15}",
                //string str = string.Format("{0:F3};{1:F0};{2:F3};{3:F0};{4:F3};{5:F0};{6:F3};{7:F0};{8:F3};{9:F0};{10:F3};{11:F0};{12:F5};{13:F5};{14:F5};{15:F5};{16:F5};{17:F5};{18:F5};{19:F5};{20:F5};{21:F5};{22:F5};{23:F5};{24:F5};{25:F5};{26:F5};{27:F5};{28:F2};{29};{30};{31}",
                Ua, Ua_Phase, Ub, Ub_Phase, Uc, Uc_Phase,
                Ia, Ia_Phase, Ib, Ib_Phase, Ic, Ic_Phase,
                P, 
                Q,
                FP,
                Frequency);

            return str;
        }
    }

}
