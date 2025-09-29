using System;
using System.IO.Ports;
using ParkingManagementReport.Common;

namespace ParkingManagementReport.Utilities.Hardwares
{
    public class MifareReader
    {
        private SerialPort sp1;
        private SerialPort sp2;
        private int[] RTX = new int[32];
        private const int intDelay = 500;
        int n;

        private bool RXComplete = false;
        private bool boolChkHead = false;
        private bool boolFRM = false;
        private int intRXFinish;
        private bool boolOK = false;
        private int intResult;
        private int intByterRX;
        private bool boolChar = false;

        public MifareReader(bool boolFRM)
        {
            sp1 = new SerialPort();
            sp1.DataReceived += new SerialDataReceivedEventHandler(SP1_OnDataReceived);
            sp2 = new SerialPort();
            sp2.DataReceived += new SerialDataReceivedEventHandler(SP2_OnDataReceived);
            this.boolFRM = boolFRM;
        }

        #region INIT/OPEN/CLOSE
        public string InitializeMfReader()
        {
            string result = "";
            if (Configs.Hardwares.PortMifare != "Not")
            {
                if (Open(Configs.Hardwares.PortMifare))
                {
                    if (Connect())
                    {
                      Configs.UseMifare = true;
                    }
                    else
                    {
                        result += "Can not connect mifare visitor reader\r\n";
                    }
                }
                else
                {
                    result += "Can not open mifare visitor port\r\n";
                }
            }
            return result;
        }

        public bool Open(string strPort)
        {
            bool boolOpen = false;
            sp1.Close();
            sp2.Close();
            if (boolFRM)
            {
                sp2.BaudRate = 38400;
                sp2.PortName = strPort;
                try
                {
                    sp2.Open();
                    boolOpen = true;
                }
                catch (Exception)
                {
                }
                return boolOpen;
            }
            sp1.BaudRate = 19200;
            sp1.PortName = strPort;
            try
            {
                sp1.Open();
                boolOpen = true;
            }
            catch (Exception)
            {
            }
            return boolOpen;
        }

        public void Close()
        {
            sp1.Close();
            sp2.Close();
        }

        public string Init1()
        {
            string strTagNo = "";
            if (boolFRM)
            {
                RTX[3] = 0x04;
                RTX[4] = 0x47;
                RTX[5] = 0x04;
                SendByte2(8);
                WaitRXComplete();
                if (boolOK)
                {
                    for (int i = 6; i < 10; i++)
                    {
                        string strTmp = Convert.ToString(RTX[i], 16).ToUpper();//.PadLeft(2, '0').PadRight(3, ' ')
                        if (strTmp.Length < 2)
                        {
                            strTmp = "0" + strTmp;
                        }
                        strTagNo += strTmp;
                    }
                }
                return strTagNo;
            }
            RTX[2] = 0x06;
            RTX[3] = 0x00;
            RTX[4] = 0x00;
            RTX[5] = 0x00;
            RTX[6] = 0x02;
            RTX[7] = 0x02;
            RTX[8] = 0x04;
            RTX[9] = CalSum();
            SendByte(10);
            WaitRXComplete();
            if (intByterRX >= 10)
            {
                for (int i = 6; i < 10; i++)
                {
                    string strTmp = Convert.ToString(RTX[i], 16).ToUpper();//.PadLeft(2, '0').PadRight(3, ' ')
                    if (strTmp.Length < 2)
                    {
                        strTmp = "0" + strTmp;
                    }
                    strTagNo += strTmp;
                }
            }
            return strTagNo;
        }

        public void Init2()
        {
            RTX[11] = RTX[9];
            RTX[10] = RTX[8];
            RTX[9] = RTX[7];
            RTX[8] = RTX[6];
            RTX[2] = 0x09;
            RTX[3] = 0x00;
            RTX[4] = 0x00;
            RTX[5] = 0x00;
            RTX[6] = 0x03;
            RTX[7] = 0x02;
            RTX[12] = CalSum();
            SendByte(13);
            WaitRXComplete();
        }
        #endregion

        #region EVENT_HANDLER
        private void SP1_OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int intBytes = sp1.BytesToRead;
            byte[] bytes = new byte[intBytes];
            sp1.Read(bytes, 0, intBytes);

            for (int cnt = 0; cnt < intBytes; cnt++)
            {
                RTX[n] = bytes[cnt];
                n++;
                if (boolChkHead == true && n > 2)
                {
                    if (RTX[n - 2] == 0xBB && RTX[n - 3] == 0xAA)
                    {
                        intByterRX = bytes[cnt];
                        intRXFinish = bytes[cnt] + 1;
                        n = 0;
                        boolChkHead = false;
                    }
                }
                else if (boolChkHead == false)
                {
                    if (n >= intRXFinish)
                    {
                        intResult = RTX[n - 2];
                        RXComplete = true;
                    }
                }
            }
        }

        private void SP2_OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int intBytes = sp2.BytesToRead;
            byte[] bytes = new byte[intBytes];
            sp2.Read(bytes, 0, intBytes);

            for (int cnt = 0; cnt < intBytes; cnt++)
            {
                RTX[n] = bytes[cnt];
                n++;
                if (boolChkHead == true && n > 2)
                {
                    if (RTX[n - 1] == 0 && RTX[n - 2] == 0 && RTX[n - 3] == 2)
                    {
                        boolChkHead = false;
                    }
                }
                else if (boolChkHead == false)
                {
                    if (!boolChar)
                    {
                        if (bytes[cnt] == 0x10)
                            boolChar = true;
                    }
                    else
                    {
                        n = n - 2;
                        RTX[n] = bytes[cnt];
                        n++;
                        boolChar = false;
                        continue;
                    }
                    if (!boolChar && bytes[cnt] == 3)
                    {

                        if (RTX[3] == 16 && RTX[6] == 0)
                            boolOK = true;
                        else if (RTX[5] == 0)
                            boolOK = true;
                        n = 0;
                        RXComplete = true;
                    }
                }
            }
        }
        #endregion
        
        public void WritePro(int b, int[] bin)
        {
            int[] intTag = new int[16];
            for (int i = 0; i < 16; i++)
            {
                intTag[i] = 48;
            }
            intTag[0] = 80;
            intTag[1] = 49;
            intTag[2] = b;
            RTX[2] = 0x16;
            RTX[3] = 0x00;
            RTX[4] = 0x00;
            RTX[5] = 0x00;
            RTX[6] = 0x09;
            RTX[7] = 0x02;
            RTX[8] = 5;
            for (int i = 0; i < 16; i++)
            {
                RTX[i + 9] = intTag[i];
            }

            RTX[25] = CalSum();
            SendByte(26);
            WaitRXComplete();

            Delay(10);
            RTX[2] = 0x16;
            RTX[3] = 0x00;
            RTX[4] = 0x00;
            RTX[5] = 0x00;
            RTX[6] = 0x09;
            RTX[7] = 0x02;
            RTX[8] = 6;
            for (int i = 0; i < 16; i++)
            {
                RTX[i + 9] = bin[i];
            }
            RTX[25] = CalSum();
            SendByte(26);
            WaitRXComplete();
        }

        private bool WaitRXComplete()
        {
            for (int i = 0; i < 100; i++)
            {
                Delay(4);
                if (RXComplete)
                {
                    return true;
                }
            }
            return false;
        }

        public void SetLED(int b)
        {
            if (boolFRM)
            {
                if (b == 0)
                {
                    RTX[3] = 0x04;
                    RTX[4] = 0x6A;
                    RTX[5] = 0;
                    SendByte2(8);
                    WaitRXComplete();
                }
                else
                {
                    RTX[3] = 0x04;
                    RTX[4] = 0x6A;
                    RTX[5] = 0x10;
                    RTX[6] = 0x3;
                    SendByte2(9);
                    WaitRXComplete();
                }
                return;
            }
            RTX[2] = 0x06;
            RTX[3] = 0x00;
            RTX[4] = 0x00;
            RTX[5] = 0x00;
            RTX[6] = 0x07;
            RTX[7] = 0x01;
            RTX[8] = b;
            RTX[9] = CalSum();
            SendByte(10);
            WaitRXComplete();
        }

        public void SetSound(int b)
        {
            if (boolFRM)
            {
                SetSoundFRM(true);
                Delay(b * 10);
                SetSoundFRM(false);
                return;
            }
            RTX[2] = 0x06;
            RTX[3] = 0x00;
            RTX[4] = 0x00;
            RTX[5] = 0x00;
            RTX[6] = 0x06;
            RTX[7] = 0x01;
            RTX[8] = b;
            RTX[9] = CalSum();
            SendByte(10);
            WaitRXComplete();
        }

        private void SetSoundFRM(bool boolSound)
        {
            if (!boolSound)
            {
                RTX[3] = 0x04;
                RTX[4] = 0x6A;
                RTX[5] = 1;
                SendByte2(8);
                WaitRXComplete();
            }
            else
            {
                RTX[3] = 0x04;
                RTX[4] = 0x6A;
                RTX[5] = 0x10;
                RTX[6] = 0x2;
                SendByte2(9);
                WaitRXComplete();
            }
        }

        public bool CheckCard()
        {
            if (boolFRM)
            {
                RTX[3] = 0x04;
                RTX[4] = 0x46;
                RTX[5] = 0x52;
                SendByte2(8);
                WaitRXComplete();
                return boolOK;
            }
            bool boolCard = false;
            RTX[2] = 0x06;
            RTX[3] = 0x00;
            RTX[4] = 0x00;
            RTX[5] = 0x00;
            RTX[6] = 0x01;
            RTX[7] = 0x02;
            RTX[8] = 0x52;
            RTX[9] = CalSum();
            SendByte(10);
            WaitRXComplete();
            if (intResult == 0)
            {
                boolCard = true;
            }
            return boolCard;
        }

        public bool Login(int b)
        {
            bool boolLogin = false;
            RTX[2] = 0x0D;
            RTX[3] = 0x00;
            RTX[4] = 0x00;
            RTX[5] = 0x00;
            RTX[6] = 0x07;
            RTX[7] = 0x02;
            RTX[8] = 0x60;
            RTX[9] = b;//Blog
            RTX[10] = 0xFF;
            RTX[11] = 0xFF;
            RTX[12] = 0xFF;
            RTX[13] = 0xFF;
            RTX[14] = 0xFF;
            RTX[15] = 0xFF;
            RTX[16] = CalSum();
            SendByte(17);
            WaitRXComplete();
            if (intResult == 0)
            {
                boolLogin = true;
            }
            return boolLogin;
        }

        public string ReadBlock(int b)
        {
            RTX[2] = 0x06;
            RTX[3] = 0x00;
            RTX[4] = 0x00;
            RTX[5] = 0x00;
            RTX[6] = 0x08;
            RTX[7] = 0x02;
            RTX[8] = b;//BLOG
            RTX[9] = CalSum();
            SendByte(10);
            WaitRXComplete();
            string str = "";
            int[] intTag = new int[16];
            for (int i = 0; i < 16; i++)
            {
                intTag[i] = RTX[i + 6];
                String strTmp = Convert.ToString(intTag[i], 16).ToUpper();//.PadLeft(2, '0').PadRight(3, ' ')
                //String strTmp = Integer.toHexString(intTag[i]).toUpperCase();
                if (strTmp.Length < 2)
                {
                    strTmp = "0" + strTmp;
                }
                str += strTmp;
            }
            return str;
        }

        public bool WriteBlock(int b, String strTag)
        {
            int[] intTag = new int[16];
            int l = strTag.Length;
            if (l < 16)
            {
                for (int i = l; i < 16; i++)
                {
                    strTag += "#";
                }
            }
            for (int i = 0; i < 16; i++)
            {
                intTag[i] = Convert.ToChar(strTag.Substring(i, 1));
            }

            RTX[2] = 0x16;
            RTX[3] = 0x00;
            RTX[4] = 0x00;
            RTX[5] = 0x00;
            RTX[6] = 0x09;
            RTX[7] = 0x02;
            RTX[8] = b;//BLOG
            for (int i = 0; i < 16; i++)
            {
                RTX[i + 9] = intTag[i];
            }

            RTX[25] = CalSum();
            SendByte(26);
            return WaitRXComplete();
        }

        public bool Connect()
        {
            bool BooConnect = false;
            if (boolFRM)
            {
                SetLED(1);
                Delay(10);
                SetSound(10);
                Delay(200);
                SetLED(0);
                Delay(20);
                SetLED(1);
                Delay(10);
                SetSound(10);
                Delay(200);
                SetLED(0);
                return true;
            }
            SetLED(0);
            WaitRXComplete();
            if (intResult == 0)
            {
                Delay(20);
                SetLED(1);
                SetSound(10);
                SetLED(0);
                Delay(200);
                SetLED(1);
                SetSound(10);
                Delay(100);
                SetLED(2);
                BooConnect = true;
            }
            return BooConnect;
        }

        private void SendByte(int nb)
        {
            boolChkHead = true;
            RXComplete = false;
            n = 0;
            intResult = 1;
            intByterRX = 0;
            RTX[0] = 0xAA;
            RTX[1] = 0xBB;
            byte[] SendByte = new byte[nb];
            for (int i = 0; i < nb; i++)
            {
                SendByte[i] = (byte)RTX[i];
            }
            sp1.Write(SendByte, 0, nb);
        }

        private void SendByte2(int nb)
        {
            boolChkHead = true;
            n = 0;
            boolOK = false;
            RXComplete = false;
            RTX[0] = 0x02;
            RTX[1] = 0;
            RTX[2] = 0;
            RTX[nb - 2] = 0;
            RTX[nb - 2] = CalSum2();
            RTX[nb - 1] = 3;
            byte[] SendByte = new byte[nb];
            for (int i = 0; i < nb; i++)
            {
                SendByte[i] = (byte)RTX[i];
                RTX[i] = 255;
            }
            sp2.Write(SendByte, 0, nb);
        }

        private int CalSum()
        {
            int b = 0;
            b = RTX[3];
            for (int i = 4; i < RTX[2] + 3; i++)
            {
                b = b ^ RTX[i];
            }
            return b;
        }

        private int CalSum2()
        {
            byte b1 = 0;
            int b = 0;
            int no = RTX[3] + 3;
            bool boolCheckChar = false;
            b = RTX[3];
            if (b == 0x10)
            {
                b = 0;
                no = RTX[4] + 3;
            }
            for (int i = 4; i < no; i++)
            {
                if (!boolCheckChar && RTX[i] == 0x10)
                {
                    boolCheckChar = true;
                    continue;
                }
                b = b + RTX[i];
                b1 = (byte)b;
                b = (int)b1;
                boolCheckChar = false;
            }
            return b1;
        }

        public void InLift()
        {
            if (boolFRM)
            {
                sp2.DtrEnable = true;
                Delay(intDelay);
                sp2.DtrEnable = false;
                return;
            }
            sp1.DtrEnable = true;
            Delay(intDelay);
            sp1.DtrEnable = false;
        }

        public void OutLift()
        {
            if (boolFRM)
            {
                sp2.RtsEnable = true;
                Delay(intDelay);
                sp2.RtsEnable = false;
                return;
            }
            sp1.RtsEnable = true;
            Delay(intDelay);
            sp1.RtsEnable = false;
        }

        private void Delay(int i)
        {
            System.Threading.Thread.Sleep(i);
        }
    }
}