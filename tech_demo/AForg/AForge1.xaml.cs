using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using AForge;
using AForge.Controls;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Video.FFMPEG;
using System.Windows.Threading;

namespace tech_demo.AForg
{
    /// <summary>
    /// AForge1.xaml 的交互逻辑
    /// </summary>
    public partial class AForge1 : Window
    {
        #region 变量
        private Bitmap m_Printscreen;                              //截图
        private DateTime m_OpenTime = DateTime.Now;               //录像开启时间
        private long m_SaveDuration = 900;                          //录像时长
        private VideoFileWriter videoWriter;
        private int m_frameRate = 25;                             //默认帧率

        private string m_SaveFolder = AppDomain.CurrentDomain.BaseDirectory;
        private string m_VideoPath = @"Media\Video";
        private string m_ImagePath = @"Media\Image";
        #endregion

        #region 属性
        /*视屏保存路径*/
        private string m_VideoAllPath
        {
            get
            {
                try
                {
                    if (!Directory.Exists(m_SaveFolder + m_VideoPath))
                    {
                        Directory.CreateDirectory(m_SaveFolder + m_VideoPath);
                    }
                    return System.IO.Path.Combine(new DirectoryInfo(m_SaveFolder + m_VideoPath).FullName, m_OpenTime.ToString("yyyyMMddHHmmss") + ".avi");
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        /*图片保存路径*/
        private string m_ImageAllPath
        {
            get
            {
                try
                {
                    if (!Directory.Exists(m_SaveFolder + m_ImagePath))
                    {
                        Directory.CreateDirectory(m_SaveFolder + m_ImagePath);
                    }
                    return System.IO.Path.Combine(new DirectoryInfo(m_SaveFolder + m_ImagePath).FullName, m_OpenTime.ToString("yyyyMMddHHmmss") + ".jpg");
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        #endregion

        public AForge1()
        {
            InitializeComponent();
        }

        #region "初始化窗口，关闭窗口"
        //打开摄像头，开始录像
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenCamera();                
            }
            catch (Exception ex)
            { 
           
            }
        }

        //关闭摄像头，结束录像
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                CloseCamera();
                CloseVideFile();
            }
            catch (Exception ex)
            { 
            
            }
        }
        #endregion

        #region "私有方法"
        #region 打开
        //获取摄像头设备
        private FilterInfoCollection GetCamList()
        {
            return new FilterInfoCollection(FilterCategory.VideoInputDevice);
        }

        /*打开摄像头*/
        private Boolean OpenCamera()
        {
            FilterInfoCollection m_videoDevices = this.GetCamList();
            if (m_videoDevices.Count == 0) return false;

            VideoCaptureDevice m_videoSource = new VideoCaptureDevice(m_videoDevices[0].MonikerString);
            m_videoSource.DesiredFrameRate = 1;
            m_videoSource.DesiredFrameSize = new System.Drawing.Size(320, 240);
            this.sourcePlayer.NewFrame += new AForge.Controls.VideoSourcePlayer.NewFrameHandler(sourcePlayer_NewFrame);
            this.sourcePlayer.VideoSource = m_videoSource;
            this.sourcePlayer.Start();
            m_OpenTime = DateTime.Now;

            return true;
        }

        /*保存图片*/
        private Boolean SaveImg(Bitmap mBitMap, string savePath)
        {
            try
            {
                mBitMap.Save(savePath, System.Drawing.Imaging.ImageFormat.Png);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /*录像*/
        private Boolean DoVedio(Bitmap image)
        {
            try
            {
                if (videoWriter == null)
                {
                    InitVideoFile(m_VideoAllPath, image.Width, image.Height, m_frameRate);
                }
                //录像时，图片写入视频流
                videoWriter.WriteVideoFrame(image);                
            }
            catch
            {
                return false;
            }
            return true;
        }

        /*创建视频读写器*/
        private Boolean InitVideoFile(string path, int width, int height, int rate)
        {
            if (videoWriter == null)
            {
                try
                {
                    videoWriter = new VideoFileWriter();
                    videoWriter.Open(path, width, height, rate, VideoCodec.MPEG4);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        }

        //录像
        private void sourcePlayer_NewFrame(object sender, ref Bitmap image)
        {
            /*录像*/
            if (DoVedio(image))
            {
                /*截取图片*/
                if (m_Printscreen == null && (DateTime.Now - m_OpenTime).TotalSeconds > 3)
                {
                    m_Printscreen = (Bitmap)image.Clone();
                }
                /*定时保存*/
                if (videoWriter != null && (DateTime.Now - m_OpenTime).TotalSeconds > m_SaveDuration)
                {
                    CloseVideFile();
                    InitVideoFile(m_VideoAllPath, 400, 300, m_frameRate);
                    m_OpenTime = DateTime.Now;
                    m_Printscreen = null;
                }
            }
        }
        #endregion
        #region 关闭
        /*关闭摄像头*/
        private Boolean CloseCamera()
        {
            if (this.sourcePlayer.IsRunning)
            {
                this.sourcePlayer.Stop();
                this.sourcePlayer.SignalToStop();
            }
            return true;
        }

        //关闭录像文件
        private Boolean CloseVideFile()
        {
            if (videoWriter != null)
            {
                videoWriter.Close();
                videoWriter.Dispose();
                videoWriter = null;
            }

            if (m_Printscreen != null)
            {
                SaveImg(m_Printscreen, m_ImageAllPath);
            }
            return true;
        }
        #endregion
        #endregion
    }
}
