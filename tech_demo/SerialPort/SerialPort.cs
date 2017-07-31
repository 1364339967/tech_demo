using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace tech_demo.SerialPort
{
    public class SerialPort
    {
        System.IO.Ports.SerialPort mSerialPort = new System.IO.Ports.SerialPort();

        #region 属性
        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate
        {
            get { return mSerialPort.BaudRate; }
            set { mSerialPort.BaudRate = value; }
        }

        /// <summary>
        /// 端口号
        /// </summary>
        public string Com
        {
            get { return mSerialPort.PortName; }
            set { mSerialPort.PortName = value; }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        private string m_receiveData = "你懂得";
        public string receiveData
        {
            get { return m_receiveData; }
            set { m_receiveData = value; }
        }

        #endregion

        #region 方法
        public void Open()
        {
            if (!mSerialPort.IsOpen)
            {
                mSerialPort.Open();
            }
        }

        public void Close()
        {
            if (mSerialPort.IsOpen)
            {
                mSerialPort.Close();
            }
        }

        /// 字符串转16进制字节数组
        private byte[] StrToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
            {
                hexString += " ";
            }
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return returnBytes;
        }

        /// <summary>
        /// 向串口发送消息
        /// </summary>
        /// <param name="data">消息体（十六进制）</param>
        /// <returns>是否发送成功</returns>
        public bool SendMessage(string code)
        {
            //串口连接方式
            bool result = true;
            if (!mSerialPort.IsOpen)
            {
                result = false;
            }
            else
            {
                byte[] data = StrToToHexByte(code);
                mSerialPort.Write(data, 0, data.Length);
            }

            return result;
        }

        /// <summary>
        /// 设置接收消息方法
        /// </summary>
        /// <param name="DataReceived"></param>
        public void setDataReceive(SerialDataReceivedEventHandler DataReceived)
        {
            mSerialPort.DataReceived += DataReceived;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="dataString"></param>
        public void SendData(string dataString)
        {
            byte[] bt = Encoding.Unicode.GetBytes(dataString);//约定编码格式为Unicode
            mSerialPort.Write(bt, 0, bt.Length);
        }
        #endregion
    }
}
