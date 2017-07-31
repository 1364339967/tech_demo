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

namespace tech_demo.Speech
{
    /// <summary>
    /// Speech.xaml 的交互逻辑
    /// </summary>
    public partial class Speech : Window
    {
        SpeechClass mSpeech = new SpeechClass();

        public Speech()
        {
            InitializeComponent();
            mainGrid.DataContext = mSpeech;
            BindVoiceList();
        }

        #region "绑定数据源"
        /// <summary>
        /// 绑定语音引擎下拉数据源
        /// </summary>
        private void BindVoiceList()
        {
            this.cbbVoiceList.ItemsSource = mSpeech.VoiceList;
        }
        #endregion

        #region 界面操作
        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void play_Click_1(object sender, RoutedEventArgs e)
        {
            //添加文本到队列中
            mSpeech.Speak(DotNetSpeech.SpeechVoiceSpeakFlags.SVSFlagsAsync);
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pause_Click_1(object sender, RoutedEventArgs e)
        {
            mSpeech.Pause();
        }

        /// <summary>
        /// 继续
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resume_Click_1(object sender, RoutedEventArgs e)
        {
            mSpeech.Resume();
        }

        /// <summary>
        /// 重新播放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void replay_Click_1(object sender, RoutedEventArgs e)
        {
            //清空之前的讲话队列
            mSpeech.Speak(DotNetSpeech.SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak, " ");
            mSpeech.Speak(DotNetSpeech.SpeechVoiceSpeakFlags.SVSFlagsAsync);
        }

        /// <summary>
        /// 导出文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportFile_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                string filename = mSpeech.ExportFile(DotNetSpeech.SpeechVoiceSpeakFlags.SVSFlagsAsync);
                MessageBox.Show(string.Format("成功导出文件:{0}.wav", filename));
            }
            catch
            {
                MessageBox.Show("导出异常");
            }
        }
        #endregion
    }
}
