using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Windows.Threading;

namespace tech_demo.SerialPort
{
    /// <summary>
    /// SerialPort_Demo.xaml 的交互逻辑
    /// </summary>
    public partial class SerialPort_Demo : Window
    {
        SerialPort mSerialPort = new SerialPort();

        public SerialPort_Demo()
        {
            InitializeComponent();
            BindComDataSource();
            BindBaudRateSource();
            this.SerialGrid.DataContext = mSerialPort;
        }

        /// <summary>
        ///  绑定端口数据源
        /// </summary>
        public void BindComDataSource()
        {
            this.comCBB.ItemsSource = System.IO.Ports.SerialPort.GetPortNames();
        }

        /// <summary>
        /// 绑定波特率数据源
        /// </summary>        
        public void BindBaudRateSource()
        {
            List<int> mBaudRateList = new List<int>();
            mBaudRateList.Add(9600);
            mBaudRateList.Add(14400);
            this.BaudRateCBB.ItemsSource = mBaudRateList;
        }

        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button mButton = sender as System.Windows.Controls.Button;
            if (mButton.Content.Equals("打开"))
            {
                mSerialPort.Open();
                mButton.Content = "关闭";
                mSerialPort.setDataReceive(Datareceive);
            }
            else if (mButton.Content.Equals("关闭"))
            {
                mSerialPort.Close();
                mButton.Content = "打开";
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendBTN_Click_1(object sender, RoutedEventArgs e)
        {           
            mSerialPort.SendData(this.sendTxt.Text.ToString());
        }


        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Datareceive(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            int count = ((System.IO.Ports.SerialPort)sender).BytesToRead;
            byte[] bt = new byte[count];
            ((System.IO.Ports.SerialPort)sender).Read(bt, 0, bt.Length);
            string datastring = Encoding.Unicode.GetString(bt);//约定编码格式为Unicode
            this.Dispatcher.BeginInvoke(DispatcherPriority.Send, new System.Windows.Forms.MethodInvoker(delegate()
               {
                   this.receiveTxt.Text = datastring;
               }));
        }
    }


}
