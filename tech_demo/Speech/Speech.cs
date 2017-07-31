using System;
using System.Collections.Generic;
using DotNetSpeech;

namespace tech_demo.Speech
{
    /// <summary>
    /// 语音类
    /// </summary>
    public class SpeechClass
    {
        SpVoiceClass mSpVoiceClass = new SpVoiceClass();

        #region 属性值
        /// <summary>
        /// 播放内容
        /// </summary>
        private string _txt = string.Empty;
        public string txt
        {
            get { return _txt; }
            set { _txt = value; }
        }

        /// <summary>
        /// 语言类型列表
        /// </summary>
        private List<VoiceInfo> _voiceList;
        public List<VoiceInfo> VoiceList
        {
            get
            {
                if (_voiceList == null)
                {
                    _voiceList = new List<VoiceInfo>();
                    foreach (SpObjectToken Token in mSpVoiceClass.GetVoices(string.Empty, string.Empty))  //查找系统语音实体
                    {
                        _voiceList.Add(new VoiceInfo() { voiceName = Token.GetDescription(), voiceId = Token.Id, SpObjectToken = Token });
                    }
                }
                return _voiceList;
            }
        }

        /// <summary>
        /// 语音引擎
        /// </summary>
        public string Voice
        {
            get { return mSpVoiceClass.Voice.Id; }
            set
            {
                foreach (VoiceInfo mVoiceInfo in VoiceList)
                {
                    if (mVoiceInfo.voiceId == value)
                    {
                        mSpVoiceClass.SetVoice(mVoiceInfo.SpObjectToken as ISpObjectToken);
                    }
                }
            }
        }

        /// <summary>
        /// 音量
        /// </summary>
        public int Volume
        {
            get { return mSpVoiceClass.Volume; }
            set { mSpVoiceClass.SetVolume((ushort)value); }
        }

        /// <summary>
        /// 速率
        /// </summary>
        public int Rate
        {
            get { return mSpVoiceClass.Rate; }
            set { mSpVoiceClass.SetRate(value); }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 讲话
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="mSpeakFlags"></param>
        public void Speak(SpeechVoiceSpeakFlags mSpeakFlags, string mTxt = "")
        {
            if (string.IsNullOrEmpty(mTxt))
            {
                mSpVoiceClass.Speak(txt, mSpeakFlags);
            }
            else
            {
                mSpVoiceClass.Speak(mTxt, mSpeakFlags);
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            mSpVoiceClass.Pause();
        }

        /// <summary>
        /// 继续
        /// </summary>
        public void Resume()
        {
            mSpVoiceClass.Resume();
        }


        /// <summary>
        /// 生成音频文件
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="mSpeakFlags"></param>
        public string ExportFile(SpeechVoiceSpeakFlags mSpeakFlags)
        {
            SpFileStream SpFileStream=null;
            try
            {
                SpFileStream = new SpFileStream();
                string fileName = DateTime.Now.Ticks.ToString();
                SpFileStream.Open(AppDomain.CurrentDomain.BaseDirectory + @"\wav\" + fileName + ".wav", SpeechStreamFileMode.SSFMCreateForWrite, false);
                mSpVoiceClass.AudioOutputStream = SpFileStream;
                this.Speak(mSpeakFlags);
                mSpVoiceClass.WaitUntilDone(-1);
                mSpVoiceClass.AudioOutputStream = null;   //必需置空，否则后面无法发音
                return fileName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (SpFileStream != null)
                {
                    SpFileStream.Close();
                }
            }
        }
        #endregion
    }


    //语音引擎类
    public class VoiceInfo
    {
        private string _voiceName;
        private string _voiceId = string.Empty;
        private SpObjectToken _SpObjectToken;

        /// <summary>
        /// 语音引擎ID
        /// </summary>
        public string voiceId
        {
            get { return _voiceId; }
            set { _voiceId = value; }
        }

        /// <summary>
        /// 语音引擎名称
        /// </summary>
        public string voiceName
        {
            get { return _voiceName; }
            set { _voiceName = value; }
        }

        public SpObjectToken SpObjectToken
        {
            get { return _SpObjectToken; }
            set { _SpObjectToken = value; }
        }
    }
}
