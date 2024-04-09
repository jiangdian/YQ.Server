using System;

namespace YQ.Tool
{
    public static class CommCtr
    {
        const int wm_User = 0x0400;
        public const int wm_DevData = wm_User + 0xA0; //装置返回数据的消息类型标识
        public const int wm_DevWarn = wm_User + 0xA1; //装置返回报警信息的消息类型标识
        public const int wm_PosData = wm_User + 0xFB; //表位返回数据的消息类型标识       
        public const int WM_COPYDATA = 0x004A;          //
        public const int WM_Close = 0x0010;             //关闭窗体消息
        private static STComm.IDevComm FDC;
        public static STComm.IDevComm FDevComm
        {
            get
            {
                if (FDC == null)
                    FDC = new STComm.DevComm();
                return FDC;
            }
        }
        private static STComm.IPosComm FPC;
        public static STComm.IPosComm FPosComm
        {
            get
            {
                if (FPC == null)
                    FPC = new STComm.PosComm();
                return FPC;
            }
        }

        /// <summary>
        /// WM_COPYDATA消息，进程间传输信息专用结构
        /// </summary>
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;

            //[MarshalAs(UnmanagedType.LPStr)]
            public IntPtr lpData;

        }
    }
}
