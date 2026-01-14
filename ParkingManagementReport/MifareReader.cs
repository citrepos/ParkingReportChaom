using System;
using System.IO.Ports;
using System.Timers;

namespace ParkingManagementReport
{
    public class MifareReader
    {
        private SerialPort sp1;
        private SerialPort sp2;
        private int[] RTX = new int[32];
        private const int intDelay = 500;
        int n;

        private bool RXComplete = false;
        private bool booChkHead = false;
        private bool booFRM = false;
        private int intRXFinish;
        private bool booOK = false;
        private int intResult;
        private int intByterRX;
        private bool booChar = false;
        //private AES128 aes = new AES128();
        //private long lngTagNo;
        //public int[] intTag = new int[16];
        //private String strTagNo;

        //Timer tmCheckCard;
        //public delegate void DetectCardHandler(string CardID);//object sender, StartEventArgs e
        //public event DetectCardHandler DetectCard;
        //private delegate void myDelegate(string txt);

        public MifareReader(bool booFRM)
        {
            sp1 = new SerialPort();
            sp1.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
            sp2 = new SerialPort();
            sp2.DataReceived += new SerialDataReceivedEventHandler(sp2_DataReceived);
            this.booFRM = booFRM;
        }

        private void sp2_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int intBytes = sp2.BytesToRead;
            byte[] bytes = new byte[intBytes];
            sp2.Read(bytes, 0, intBytes);

            for (int cnt = 0; cnt < intBytes; cnt++)
            {
                RTX[n] = bytes[cnt];
                n++;
                if (booChkHead == true && n > 2)
                {
                    if (RTX[n - 1] == 0 && RTX[n - 2] == 0 && RTX[n - 3] == 2)
                    {
                        booChkHead = false;
                    }
                }
                else if (booChkHead == false)
                {
                    if (!booChar)
                    {
                        if (bytes[cnt] == 0x10)
                            booChar = true;
                    }
                    else
                    {
                        n = n - 2;
                        RTX[n] = bytes[cnt];
                        n++;
                        booChar = false;
                        continue;
                    }
                    if (!booChar && bytes[cnt] == 3)
                    {

                        if (RTX[3] == 16 && RTX[6] == 0)
                            booOK = true;
                        else if (RTX[5] == 0)
                            booOK = true;
                        n = 0;
                        RXComplete = true;
                    }
                }
            }
        }

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

            RTX[25] = calSum();
            sendByte(26);
            waitRXComplete();

            delay(10);
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
            RTX[25] = calSum();
            sendByte(26);
            waitRXComplete();

        }

        private bool waitRXComplete()
        {
            //bool booComplete = true;
            for (int i = 0; i < 100; i++)
            {
                delay(4);
                if (RXComplete)
                {
                    return true;
                }
            }
            return false;
        }

        public void setLED(int b)
        {
            if (booFRM)
            {
                if (b == 0)
                {
                    RTX[3] = 0x04;
                    RTX[4] = 0x6A;
                    RTX[5] = 0;
                    sendByte2(8);
                    waitRXComplete();
                }
                else
                {
                    RTX[3] = 0x04;
                    RTX[4] = 0x6A;
                    RTX[5] = 0x10;
                    RTX[6] = 0x3;
                    sendByte2(9);
                    waitRXComplete();
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
            RTX[9] = calSum();
            sendByte(10);
            waitRXComplete();
        }

        public void setSound(int b)
        {
            if (booFRM)
            {
                setSoundFRM(true);
                delay(b * 10);
                setSoundFRM(false);
                return;
            }
            RTX[2] = 0x06;
            RTX[3] = 0x00;
            RTX[4] = 0x00;
            RTX[5] = 0x00;
            RTX[6] = 0x06;
            RTX[7] = 0x01;
            RTX[8] = b;
            RTX[9] = calSum();
            sendByte(10);
            waitRXComplete();
        }

        private void setSoundFRM(bool booSound)
        {
            if (!booSound)
            {
                RTX[3] = 0x04;
                RTX[4] = 0x6A;
                RTX[5] = 1;
                sendByte2(8);
                waitRXComplete();
            }
            else
            {
                RTX[3] = 0x04;
                RTX[4] = 0x6A;
                RTX[5] = 0x10;
                RTX[6] = 0x2;
                sendByte2(9);
                waitRXComplete();
            }
        }

        public bool chkCard()
        {
            if (booFRM)
            {
                RTX[3] = 0x04;
                RTX[4] = 0x46;
                RTX[5] = 0x52;
                sendByte2(8);
                waitRXComplete();
                return booOK;
            }
            bool booCard = false;
            RTX[2] = 0x06;
            RTX[3] = 0x00;
            RTX[4] = 0x00;
            RTX[5] = 0x00;
            RTX[6] = 0x01;
            RTX[7] = 0x02;
            RTX[8] = 0x52;
            RTX[9] = calSum();
            sendByte(10);
            waitRXComplete();
            if (intResult == 0)
            {
                booCard = true;
            }
            return booCard;
        }

        public string Init1()
        {
            string strTagNo = "";
            if (booFRM)
            {
                RTX[3] = 0x04;
                RTX[4] = 0x47;
                RTX[5] = 0x04;
                sendByte2(8);
                waitRXComplete();
                if (booOK)
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
            RTX[9] = calSum();
            sendByte(10);
            waitRXComplete();
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
            RTX[12] = calSum();
            sendByte(13);
            waitRXComplete();
        }

        public bool Login(int b)
        {
            bool booLogin = false;
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
            RTX[16] = calSum();
            sendByte(17);
            waitRXComplete();
            if (intResult == 0)
            {
                booLogin = true;
            }
            return booLogin;
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
            RTX[9] = calSum();
            sendByte(10);
            waitRXComplete();
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

            RTX[25] = calSum();
            sendByte(26);
            return waitRXComplete();
        }

        public bool Connect()
        {
            bool booConnect = false;
            if (booFRM)
            {
                setLED(1);
                delay(10);
                setSound(10);
                delay(200);
                setLED(0);
                delay(20);
                setLED(1);
                delay(10);
                setSound(10);
                delay(200);
                setLED(0);
                return true;
            }
            setLED(0);
            waitRXComplete();
            if (intResult == 0)
            {
                delay(20);
                setLED(1);
                setSound(10);
                setLED(0);
                delay(200);
                setLED(1);
                setSound(10);
                delay(100);
                setLED(2);
                booConnect = true;
            }
            return booConnect;
        }

        private void sendByte(int nb)
        {
            booChkHead = true;
            RXComplete = false;
            n = 0;
            intResult = 1;
            intByterRX = 0;
            RTX[0] = 0xAA;
            RTX[1] = 0xBB;
            byte[] sendByte = new byte[nb];
            for (int i = 0; i < nb; i++)
            {
                sendByte[i] = (byte)RTX[i];
            }
            sp1.Write(sendByte, 0, nb);
        }

        private void sendByte2(int nb)
        {
            booChkHead = true;
            n = 0;
            booOK = false;
            RXComplete = false;
            RTX[0] = 0x02;
            RTX[1] = 0;
            RTX[2] = 0;
            RTX[nb - 2] = 0;
            RTX[nb - 2] = calSum2();
            RTX[nb - 1] = 3;
            byte[] sendByte = new byte[nb];
            for (int i = 0; i < nb; i++)
            {
                sendByte[i] = (byte)RTX[i];
                RTX[i] = 255;
            }
            sp2.Write(sendByte, 0, nb);
        }

        private int calSum()
        {
            int b = 0;
            b = RTX[3];
            for (int i = 4; i < RTX[2] + 3; i++)
            {
                b = b ^ RTX[i];
            }
            return b;
        }

        private int calSum2()
        {
            byte b1 = 0;
            int b = 0;
            int no = RTX[3] + 3;
            bool booCheckChar = false;
            b = RTX[3];
            if (b == 0x10)
            {
                b = 0;
                no = RTX[4] + 3;
            }
            for (int i = 4; i < no; i++)
            {
                if (!booCheckChar && RTX[i] == 0x10)
                {
                    booCheckChar = true;
                    continue;
                }
                b = b + RTX[i];
                b1 = (byte)b;
                b = (int)b1;
                booCheckChar = false;
            }
            return b1;
        }


        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int intBytes = sp1.BytesToRead;
            byte[] bytes = new byte[intBytes];
            sp1.Read(bytes, 0, intBytes);

            for (int cnt = 0; cnt < intBytes; cnt++)
            {
                RTX[n] = bytes[cnt];
                n++;
                if (booChkHead == true && n > 2)
                {
                    if (RTX[n - 2] == 0xBB && RTX[n - 3] == 0xAA)
                    {
                        intByterRX = bytes[cnt];
                        intRXFinish = bytes[cnt] + 1;
                        n = 0;
                        booChkHead = false;
                    }
                }
                else if (booChkHead == false)
                {
                    if (n >= intRXFinish)
                    {
                        intResult = RTX[n - 2];
                        RXComplete = true;
                        //myDelegate dm = new myDelegate(Form1.SetText1);
                        //dm.Invoke("Test myDelegate");
                    }
                }
            }
        }

        public bool Open(string strPort)
        {
            bool booOpen = false;
            sp1.Close();
            sp2.Close();
            if (booFRM)
            {
                sp2.BaudRate = 38400;
                sp2.PortName = strPort;
                try
                {
                    sp2.Open();
                    booOpen = true;
                }
                catch (Exception)
                {
                }
                return booOpen;
            }
            sp1.BaudRate = 19200;
            sp1.PortName = strPort;
            try
            {
                sp1.Open();
                booOpen = true;
            }
            catch (Exception)
            {
            }
            return booOpen;
        }

        public void Close()
        {
            sp1.Close();
            sp2.Close();
        }

        public void InLift()
        {
            if (booFRM)
            {
                sp2.DtrEnable = true;
                delay(intDelay);
                sp2.DtrEnable = false;
                return;
            }
            sp1.DtrEnable = true;
            delay(intDelay);
            sp1.DtrEnable = false;
        }

        public void OutLift()
        {
            if (booFRM)
            {
                sp2.RtsEnable = true;
                delay(intDelay);
                sp2.RtsEnable = false;
                return;
            }
            sp1.RtsEnable = true;
            delay(intDelay);
            sp1.RtsEnable = false;
        }

        private void delay(int i)
        {
            System.Threading.Thread.Sleep(i);
        }

    }
}
