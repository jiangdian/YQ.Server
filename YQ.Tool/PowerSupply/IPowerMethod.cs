using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YQ.Tool
{
    public interface IPowerMethod
    {
        bool OpenComm(int Port);

        bool Power_On(PowerParam power);


        /// <summary>
        ///校表脉冲采集通道切换
        /// </summary>
        /// <param name="Meter_No">表位号</param>
        /// <param name="Channel_Flag">脉冲采样通道：0-有功正向/反向；1-有功反向；2-无功反向/正向；3-无功反向； 4-时钟；5-投切；6-需量 
        /// <param name="Dev_Port">端口</param>
        /// <returns></returns>       
        bool Set_Pulse_Channel(int Meter_No, int Channel_Flag, int Dev_Port);

        /// <summary>
        /// 开始测试误差
        /// </summary>
        /// <param name="Meter_No"></param>
        /// <param name="Constant"></param>
        /// <param name="Pulse"></param>
        /// <param name="Dev_Port"></param>
        /// <returns></returns>
        bool Error_Start(int Meter_No, double Constant, int Pulse, int Dev_Port);
                     
        /// <summary>
        /// 读取误差，次数+","+误差  如2,-0.024 表示误差为-0.024,是第二次误差,第0次误差表示误差还没有，需要丢弃。
        /// </summary>
        /// <param name="MError"></param>
        /// <param name="Meter_No"></param>
        /// <param name="Dev_Port"></param>
        /// <returns></returns>
        bool Error_Read(ref string MError, int Meter_No, int circle, int Dev_Port);

        /// <summary>
        /// 指示仪表读取
        /// </summary>
        /// <param name="SData">结果Ua,Ub,Uc,Ia,Ib,Ic,φa,φb,φc,Pa,Pb,Pc,Qa,Qb,Qc,Sa,Sb,Sc,有功功率(总),无功功率(总),视在功率(总),频率,{电压档位},电流档位</param>
        /// <param name="SModel">装置配置的标准表型号</param>
        /// <param name="Dev_Port">通讯口号。如：1=COM1</param>
        /// <returns></returns>
        PowerParam StdMeter_Read();

        /// <summary>
        /// 误差仪总清
        /// </summary>
        /// <param name="Dev_Port"></param>
        /// <returns></returns>
        bool Error_Clear(int Dev_Port);

        /// <summary>
        /// 停止潜动
        /// </summary>
        /// <param name="Dev_Port"></param>
        /// <returns></returns>
        bool Stop_Test(int Dev_Port);

        
        /// <summary>
        /// 降电压电流
        /// </summary>
        /// <param name="Dev_Port"></param>
        /// <returns></returns>
        bool Power_Off(int Dev_Port);

        /// <summary>
        /// 标准时钟仪切换
        /// </summary>
        /// <param name="SetFlag">0-切开 ；1-切上</param>
        /// <param name="Dev_Port"></param>
        /// <returns></returns>
        bool SetRefClock(byte SetFlag, int Dev_Port);
        /// <summary>
        /// 开始测试时钟误差
        /// </summary>
        /// <param name="Meter_No"></param>
        /// <param name="TheoryFreq">输出频率(Hz)</param>
        /// <param name="TestTime">测试时间(s)</param>
        /// <param name="Dev_Port"></param>
        /// <returns></returns>
        bool Clock_Error_Start(int Meter_No, double TheoryFreq, int TestTime, int Dev_Port);
        /// <summary>
        /// 读取时钟误差
        /// </summary>
        /// <param name="MError"></param>
        /// <param name="TheoryFreq"></param>
        /// <param name="ErrorType">0-频率；1-日计时误差；2-相对误差</param>
        /// <param name="Meter_No"></param>
        /// <param name="Dev_Port"></param>
        /// <returns></returns>
        bool Clock_Error_Read(ref string MError, double TheoryFreq, byte ErrorType, int Meter_No, int Dev_Port);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="StdFreq"></param>
        /// <returns></returns>
        bool SetStdFreq(double StdFreq);
        /// <summary>
        /// 设置谐波参数
        /// <para>说明：所有参数为三维数组，在升源命令前调用</para>
        /// <para>举例：Set_Harmonic_Data([0,0,0], [7,3,5], [0,0,0], [10,15,30])</para>
        /// </summary>
        /// <param name="HAng">谐波相位 三维数组</param>
        /// <param name="HNum">谐波次数 (3、5、7 次可任意叠加，传送时高次在前，其它次数只能发送单次，且放在第一位)</param>
        /// <param name="HVolt">电压谐波含量(总含量不能超40%)</param>
        /// <param name="HCur">电流谐波含量(总含量不能超40%)</param>
        /// <returns></returns>
        bool Set_Harmonic_Data(double[] HAng, int[] HNum, double[] HVolt, double[] HCur);

        /// <summary>
        /// 自动复位
        /// </summary>
        /// <param name="Meter_No">1开始</param>
        /// <param name="Dev_Port"></param>
        /// <returns></returns>    
        bool AutoSub_Reset(int Meter_No, byte Dev_Port);


        /// <summary>
        /// 自动短接
        /// </summary>
        /// <param name="Meter_No">1开始</param>
        /// <param name="Dev_Port"></param>
        /// <returns></returns>
        bool AutoSub(int Meter_No, byte Dev_Port);

        /// <summary>
        /// 所有表位复位，用于新跃表位初始化
        /// </summary>
        /// <param name="Meter_No">1开始</param>
        /// <param name="Dev_Port"></param>
        /// <returns></returns>    
        bool AutoAll_ResetSY();

        /// <summary>
        /// 发送命令 有返回值
        /// </summary>
        /// <param name="Dev_Port">通讯串口号 ,整形</param>
        /// <param name="ErrAddr"> 误差仪地址 41H+表位号，如2表位43H,H代表16进制 ; $FF为广播地址 ;$41为0#误差仪(功能切换总控板)</param>
        /// <param name="ErrCmd">命令字 ,整形18H - 读取走字脉冲数33H - 指示灯控制41H - 485极性反转</param>
        /// <param name="CmdData">命令参数 ,字符串 在C中为指针型        如果要传16进制格式，就加前缀#。如’#13’，代表传进去为十进制19的字符串。</param>
        /// <param name="BackData">命令返回数据帧,字符串 在C中为指针型        返回：01H+地址(41H+表位号) +长度+电表脉冲数(6位)+ 标准表脉冲数10位(每个分机一样)+校验位+ 结束(17H)</param>
        /// <returns></returns>
        bool ErrorComm(int Dev_Port, int ErrAddr, byte ErrCmd, byte[] CmdData, ref StringBuilder BackData);

        /// <summary>
        /// 发送命令 无返回值
        /// </summary>
        /// <param name="Dev_Port">通讯串口号 ,整形</param>
        /// <param name="ErrAddr"> 误差仪地址 41H+表位号，如2表位43H,H代表16进制 ; $FF为广播地址 ;$41为0#误差仪(功能切换总控板)</param>
        /// <param name="ErrCmd">命令字 ,整形18H - 读取走字脉冲数33H - 指示灯控制41H - 485极性反转</param>
        /// <param name="CmdData">命令参数 ,字符串 在C中为指针型        如果要传16进制格式，就加前缀#。如’#13’，代表传进去为十进制19的字符串。</param>
        /// <returns></returns>
        bool ErrorComm1(int Dev_Port, int ErrAddr, byte ErrCmd, byte[] CmdData);

        /// <summary>
        /// 误差仪累计脉冲读取 读取脉冲数
        /// </summary>
        /// <param name="MPulse">表累计脉冲数</param>
        /// <param name="Meter_No"></param>
        /// <param name="Dev_Port"></param>
        /// <returns></returns>  
        bool Count_Read(ref int MPulse, int Meter_No, byte Dev_Port);
        /// <summary>
        /// 误差仪计数开始
        /// </summary>
        /// <param name="Meter_No">表位号[1-N]</param>
        /// <param name="Active">true=有功，false=无功</param>
        /// <param name="Pulse">脉冲个数</param>
        /// <param name="TotalTime">总时长</param>
        /// <returns></returns>
        bool Count_Start(int Meter_No, bool Active, int Pulse,int TotalTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Pulse">脉冲数</param>
        /// <param name="StdKWH">标准表走字量</param>
        /// <param name="Constant">被校表常数  1200 1600 etc</param>
        /// <param name="Meter_No"></param>
        /// <param name="Dev_Port"></param>
        /// <returns></returns>
        bool ConstPulse_Read(ref int Pulse, ref double StdKWH, double Constant, int Meter_No, int Dev_Port);

    }
}
