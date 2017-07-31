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
    /// AForge.xaml 的交互逻辑
    /// </summary>
    public partial class AForge : Window
    {
         #region "变量"
        private FilterInfoCollection m_videoDevices = null;
        private VideoCaptureDevice m_videoSource = null;
        private Boolean m_OpenFlag = false;        //打开摄像头标志

        private Boolean savefolderFlag = true;
        private Boolean m_SaveVideoIng = false;   //正在录像标志
        private Boolean m_CanOpenVideo = true;     //能否打开录像文件
        private Bitmap m_Printscreen;      //截图
        private DateTime m_StartTime = DateTime.Now;               //摄像开始时间
        private VideoFileWriter videoWriter;
        private int m_frameRate = 25; //默认帧率
        private string m_UseName = string.Empty;
        private int m_SectionID;

        public static string videosavefolder = AppDomain.CurrentDomain.BaseDirectory;
        private string m_VideoPath = @"Media\Video";
        private string m_ImagePath = @"Media\Image";
        #endregion

        #region "属性"
        public Boolean OpenCameraFlag
        {
            get { return m_OpenFlag; }
        }

        //保存路径
        private string m_SaveFolder
        {
            get
            {
                if (string.IsNullOrEmpty(videosavefolder))
                {
                    videosavefolder = AppDomain.CurrentDomain.BaseDirectory;
                }
                return videosavefolder;
            }
        }

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
                    return System.IO.Path.Combine(new DirectoryInfo(m_SaveFolder + m_VideoPath).FullName, m_StartTime.ToString("yyyyMMddHHmmss") + m_UseName + "-" + m_SectionID + ".avi");
                }
                catch
                {
                    savefolderFlag = false;
                    return string.Empty;
                }
            }
        }
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
                    return System.IO.Path.Combine(new DirectoryInfo(m_SaveFolder + m_ImagePath).FullName, m_StartTime.ToString("yyyyMMddHHmmss") + m_UseName + "-" + m_SectionID + ".jpg");
                }
                catch
                {
                    savefolderFlag = false;
                    return string.Empty;
                }
            }
        }
        #endregion

        public AForge()
        {
            InitializeComponent();
            m_UseName = string.Empty;
            FindDevices();
        }

        #region "初始化窗口，关闭窗口"
        //打开摄像头，开始录像
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            m_videoSource = new VideoCaptureDevice(m_videoDevices[0].MonikerString);
            m_videoSource.DesiredFrameRate = 1;
            m_videoSource.DesiredFrameSize = new System.Drawing.Size(320, 240);
            this.sourcePlayer.NewFrame += new AForge.Controls.VideoSourcePlayer.NewFrameHandler(this.sourcePlayer_NewFrame);
            this.sourcePlayer.VideoSource = this.m_videoSource;
            this.sourcePlayer.Start();
            m_OpenFlag = true;
        }

        //关闭摄像头，结束录像
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.sourcePlayer.IsRunning)
            {
                this.sourcePlayer.Stop();
                this.sourcePlayer.SignalToStop();
            }
            this.CloseVideFile();
        }
        #endregion

        #region "私有方法"
        //检查驱动
        public Boolean FindDevices()
        {
            this.m_videoDevices = this.GetCamList();
            if (this.m_videoDevices.Count == 0)
            {
                ShowMessageBox("没检测到摄像头");
                m_OpenFlag = false;

                return false;
            }

            return true;
        }

        //录像
        private void sourcePlayer_NewFrame(object sender, ref Bitmap image)
        {
            if (m_CanOpenVideo) DoVedio(image);
        }

        private void source_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            DoVedio(bitmap);
        }

        //获取摄像头设备
        private FilterInfoCollection GetCamList()
        {
            return new FilterInfoCollection(FilterCategory.VideoInputDevice);
        }

        string drawDate = string.Empty;
        //将图片写进视频
        private void DoVedio(Bitmap image)
        {
            try
            {
                if (m_Printscreen == null)
                {
                    //攫取图片
                    DateTime timeNow = DateTime.Now;
                    TimeSpan timespan = timeNow - m_StartTime;
                    if (timespan.TotalSeconds > 3)
                    {
                        m_Printscreen = (Bitmap)image.Clone();
                    }
                }
                if (!m_SaveVideoIng)
                {
                    //录像前，创建文件
                    m_SaveVideoIng = true;
                    this.CloseVideFile();
                    m_StartTime = DateTime.Now;
                    m_CanOpenVideo = this.OpenVideoFile(m_VideoAllPath, image.Width, image.Height, m_frameRate);
                    if (!m_CanOpenVideo)
                    {
                        this.ShowMessageBox("无法保存录像");
                    }
                }
                if (m_CanOpenVideo)
                {
                    //录像时，图片写入视频流
                    videoWriter.WriteVideoFrame(image);
                }
                if (m_SaveVideoIng)
                {
                    //定时保存
                    DateTime timeNow = DateTime.Now;
                    TimeSpan timespan = timeNow - m_StartTime;
                    if (timespan.TotalSeconds > 900)
                    {
                        m_Printscreen = (Bitmap)image.Clone();
                        m_SaveVideoIng = false;
                    }
                }
            }
            catch (Exception ex)
            {
                m_CanOpenVideo = false;
                this.ShowMessageBox("无法保存录像");
            }
        }

        //打开录像文件
        private Boolean OpenVideoFile(string path, int width, int height, int rate)
        {
            Boolean openFlag = false;
            if (videoWriter == null)
            {
                try
                {
                    videoWriter = new VideoFileWriter();
                    videoWriter.Open(path, width, height, rate, VideoCodec.MPEG4);
                    openFlag = true;
                }
                catch (Exception ex)
                {
                    openFlag = false;
                }
            }
            else
            {
                openFlag = true;
            }
            return openFlag;
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
                //保存图片
                try
                {
                    this.m_Printscreen.Save(m_ImageAllPath, System.Drawing.Imaging.ImageFormat.Png);
                    m_Printscreen = null;
                }
                catch
                { }
            }
            return true;
        }

        private void ShowMessageBox(string msg)
        {
            this.Dispatcher.Invoke(DispatcherPriority.ApplicationIdle, new Action(
            delegate
            {
               
            }));
        }
        #endregion
    }
}
