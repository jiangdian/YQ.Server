using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using YQ.Parsing;
using YQ.Parsing.DoCmdAnalyse;

namespace YQ.FunctionModule.ViewModels
{
    public class CommunicationToolViewModel : BindableBase
    {

        /// <summary>
        /// 实际表地址
        /// </summary>
        private string _actualMeterAddress = "AAAAAAAAAAAA";
        public string ActualMeterAddress
        {
            get { return _actualMeterAddress; }
            set { SetProperty(ref _actualMeterAddress, value); }
        }

        /// <summary>
        /// 客户机地址
        /// </summary>
        private string _logicalAddress = "00";
        public string LogicalAddress
        {
            get { return _logicalAddress; }
            set { SetProperty(ref _logicalAddress, value); }
        }


        /// <summary>
        /// 写入地址
        /// </summary>
        private string _writeAddress = "202405010301";
        public string WriteAddress
        {
            get { return _writeAddress; }
            set { SetProperty(ref _writeAddress, value); }
        }

        /// <summary>
        /// 多路服务器IP
        /// </summary>
        private string _ip = "192.168.1.111";
        public string IP
        {
            get { return _ip; }
            set { SetProperty(ref _ip, value); }
        }
        /// <summary>
        /// 多路服务器IP
        /// </summary>
        private string _port = "10001";
        public string Port
        {
            get { return _port; }
            set { SetProperty(ref _port, value); }
        }

        /// <summary>
        /// 数据
        /// </summary>
        private string _writeData = "060100400102000906202405010301";
        public string WriteData
        {
            get { return _writeData; }
            set { SetProperty(ref _writeData, value); }
        }

        /// <summary>
        /// 通讯方式
        /// </summary>
        private string _com = "1-4851";
        public string Com
        {
            get { return _com; }
            set { SetProperty(ref _com, value); }
        }
        /// <summary>
        /// 通讯方式
        /// </summary>
        /// 1表示485-1，2表示485-2，3. 485-3（逆变器），4. 485-4，5. 485-5（逆变器），6. 485-6，7蓝牙，8. STA，9. CCO
        private List<string> _comList = new List<string>()
        { 
            "1-4851", 
            "2-4852", 
            "3-4853(逆变器)", 
            "4-4854",
            "5-4855(逆变器)",
            "6-4856", 
            "7-蓝牙", 
            "8-STA", 
            "9-CCO"
        };
        public List<string> ComList
        {
            get { return _comList; }
            set { SetProperty(ref _comList, value); }
        }

        /// <summary>
        /// 表位号
        /// </summary>
        private string _currentCeterNo = "1";
        public string CurrentMeterNo
        {
            get { return _currentCeterNo; }
            set { SetProperty(ref _currentCeterNo, value); }
        }
        /// <summary>
        /// 表位号
        /// </summary>
        private List<string> _meterNo = new List<string>() {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"};
        public List<string> MeterNo
        {
            get { return _meterNo; }
            set { SetProperty(ref _meterNo, value); }
        }

        /// <summary>
        /// cmd
        /// </summary>
        private string _cmd = "0201-功率源升源(常用)";
        public string Cmd
        {
            get { return _cmd; }
            set { SetProperty(ref _cmd, value); }
        }
        /// <summary>
        /// cmd列表
        /// </summary>
        private List<string> _cmdList = new List<string>() 
        {
            "0101-连接台体多路服务器",
            "0201-功率源升源(常用)",
            "0202-功率源升源(高级)",
            "0203-功率源关源",
            "0301-读标准表数据",
            "0401-初始化台体表位",
            "0506-读取功耗测试数据",
            "1007-协议转发"
        };
        public List<string> CmdList
        {
            get { return _cmdList; }
            set { SetProperty(ref _cmdList, value); }
        }


        /// <summary>
        /// 在检状态
        /// </summary>
        private string _status = "1";
        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }
        /// <summary>
        /// 在检状态
        /// </summary>
        private List<string> _statusList = new List<string>() { "0", "1"};
        public List<string> StatusList
        {
            get { return _statusList; }
            set { SetProperty(ref _statusList, value); }
        }


        /// <summary>
        /// RS485接入状态
        /// </summary>
        private string _rs485status = "01";
        public string RS485Status
        {
            get { return _rs485status; }
            set { SetProperty(ref _rs485status, value); }
        }
        /// <summary>
        /// RS485接入状态
        /// </summary>
        private List<string> _rs485statusList = new List<string>() { "00", "01" };
        public List<string> RS485StatusList
        {
            get { return _rs485statusList; }
            set { SetProperty(ref _rs485statusList, value); }
        }

        /// <summary>
        /// 完整数据
        /// </summary>
        private string _completeData = "cmd=0201,data=0;7;220;0;0;50;0";
        public string CompleteData 
        {
            get { return _completeData; }
            set { SetProperty(ref _completeData, value); }
        }


        #region cmd切换事件

        private DelegateCommand _RefreshCommand;
        public DelegateCommand RefreshCommand => _RefreshCommand ?? (_RefreshCommand = new DelegateCommand(Refresh));

        private void Refresh()
        {
            SelectionChanged(Cmd[..4]);
        }


        private DelegateCommand<object> _SelectionChanged;
        public DelegateCommand<object> SelectionChangedCommand =>
            _SelectionChanged ?? (_SelectionChanged = new DelegateCommand<object>(SelectionChanged));
        private void SelectionChanged(object sender)
        {
            if (sender != null)
            {
                switch (sender.ToString().Substring(0, 4))
                {
                    case "0101":
                        CompleteData = $"cmd=0101,data={IP};{Port}";
                        break;
                    case "0201":
                        CompleteData = "cmd=0201,data=0;7;220;0;0;50;0";
                        break;
                    case "0202":
                        CompleteData = "cmd=0202,data=0;7;50;220;0;220;0;220;0;0;0;0;0;0;0;0";
                        break;
                    case "0203":
                        CompleteData = "cmd=0203,data=null";
                        break;
                    case "0301":
                        CompleteData = "cmd=0301,data=null";
                        break;
                    case "0506":
                        CompleteData = $"cmd=0506,data={CurrentMeterNo}";
                        break;
                    case "0401":
                        CompleteData = $"cmd=0401,data={CurrentMeterNo};{Status};{RS485Status}";
                        break;
                    default:
                        CompleteData = "";
                        break;
                }
            }
        }

        #endregion


        private DelegateCommand _SendCommand;
        public DelegateCommand SendCommand => _SendCommand ?? (_SendCommand = new DelegateCommand(SenMessage));

        private DelegateCommand _ReadAddressCommand;
        public DelegateCommand ReadAddressCommand => _ReadAddressCommand ?? (_ReadAddressCommand = new DelegateCommand(ReadMeterAddress));

        private DelegateCommand _WriteAddressCommand;
        public DelegateCommand WriteAddressCommand => _WriteAddressCommand ?? (_WriteAddressCommand = new DelegateCommand(WriteMeterAddress));

        private DelegateCommand _CreateCommand;
        public DelegateCommand CreateCommand => _CreateCommand ?? (_CreateCommand = new DelegateCommand(CreateData));

        
        /// <summary>
        /// 读地址
        /// </summary>
        private void ReadMeterAddress()
        {
            CompleteData = $"cmd=1007,data={CurrentMeterNo};1;6817004345AAAAAAAAAAAA005B4F0501034001020000900F16";

            //TODO:发送数据到台体
        }

        /// <summary>
        /// 写地址
        /// </summary>    
        private void WriteMeterAddress()
        {
            string ProtocolHead = $"cmd=1007,data={CurrentMeterNo};{Com[..1]};68";
            string FrameHead = $"1f004345{ReverseEveryTwoCharacters(ActualMeterAddress)}{LogicalAddress}";
            //计算帧头校验
            var FrameHeadHCS = DLT698FCSHelper.CreateFCS16(HexToBytes(FrameHead), 0, HexToBytes(FrameHead).Length);
            byte[] FrameHeadCheckBytes = new byte[] { (byte)(FrameHeadHCS & 0xff), (byte)((FrameHeadHCS >> 8) & 0xff) };
            string FrameData = $"{FrameHead}{BytesToHex(FrameHeadCheckBytes)}060100400102000906{WriteAddress}00";
            //计算帧尾校验
            var FrameTailFCS = DLT698FCSHelper.CreateFCS16(HexToBytes(FrameData), 0, HexToBytes(FrameData).Length);
            byte[] FrameTailCheckBytes = new byte[] { (byte)(FrameTailFCS & 0xff), (byte)((FrameTailFCS >> 8) & 0xff) };
            //完整帧
            CompleteData = $"{ProtocolHead}{FrameData}{BytesToHex(FrameTailCheckBytes)}16";

            //TODO:发送数据到台体
        }

        /// <summary>
        /// 发送指令
        /// </summary>
        private void SenMessage()
        {

            //TODO:发送数据到台体
        }


        /// <summary>
        /// 生成数据
        /// </summary>
        public void CreateData()
        {
            string ProtocolHead = $"cmd={Cmd[..4]},data={CurrentMeterNo};{Com[..1]};68";
            string FrameHead = $"1f004345{ReverseEveryTwoCharacters(ActualMeterAddress)}{LogicalAddress}";
            //计算帧头校验
            var FrameHeadHCS = DLT698FCSHelper.CreateFCS16(HexToBytes(FrameHead), 0, HexToBytes(FrameHead).Length);
            byte[] FrameHeadCheckBytes = new byte[] { (byte)(FrameHeadHCS & 0xff), (byte)((FrameHeadHCS >> 8) & 0xff) };
            string FrameData = $"{FrameHead}{BytesToHex(FrameHeadCheckBytes)}{WriteData}00";
            //计算帧尾校验
            var FrameTailFCS = DLT698FCSHelper.CreateFCS16(HexToBytes(FrameData), 0, HexToBytes(FrameData).Length);
            byte[] FrameTailCheckBytes = new byte[] { (byte)(FrameTailFCS & 0xff), (byte)((FrameTailFCS >> 8) & 0xff) };
            //完整帧
            CompleteData = $"{ProtocolHead}{FrameData}{BytesToHex(FrameTailCheckBytes)}16";
        }


        #region 数据转换
        /// <summary>
        /// Hex无空格 -> Bytes
        /// </summary>
        public static byte[] HexToBytes(string pString)
        {
            byte[] retBytes = { };
            if (!string.IsNullOrEmpty(pString))
            {
                int tmpLength = (pString.Length % 2 == 0) ? (pString.Length / 2) : (pString.Length / 2) + 1;
                pString = pString.PadLeft(2 * tmpLength, '0');

                retBytes = new byte[tmpLength];
                for (int i = 0; i < tmpLength; i++)
                {
                    bool tmpResult = Byte.TryParse(pString.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber, null, out var tmpByte);
                    if (tmpResult)
                    {
                        retBytes[i] = tmpByte;
                    }
                    else
                    {
                        throw new Exception("Hex string error.");
                    }
                }
            }

            return retBytes;
        }

        /// <summary>
        /// 十六进制字符串按字节倒序
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public string ReverseEveryTwoCharacters(string input)
        {
            if (string.IsNullOrEmpty(input) || input.Length % 2 != 0)
                throw new ArgumentException("Input string must have an even length.");

            var result = new StringBuilder();
            for (int i = input.Length - 2; i >= 0; i -= 2)
            {
                result.Append(input.Substring(i, 2));
            }
            return result.ToString();
        }

        /// <summary>
        /// Byte数组转Hex
        /// </summary>
        public static string BytesToHex(byte[] pBytes)
        {
            if (pBytes == null)
                return null;
            return BitConverter.ToString(pBytes).Replace("-", string.Empty);
        }

        #endregion

    }
}
