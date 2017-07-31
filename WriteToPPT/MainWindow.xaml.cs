using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using Aspose.Slides;
using System.Text.RegularExpressions;

namespace WriteToPPT
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {       
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                //NewPPT();
                WriteToPPT(AppDomain.CurrentDomain.BaseDirectory + @"File\PPTX\20170703作业方案.ppt");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void Crack()
        {
            string[] stModule = new string[8]
            {
                "\u0002\u2006\u2006\u2003",
                "\u0003\u2006\u2006\u2003",
                "\u0005\u2005\u2000\u2003",
                "\u0003\u2000",
                "\u000F",
                "\u0002\u2000",
                "\u0003",
                "\u0002"
            };
            Assembly assembly = Assembly.GetAssembly(typeof(Aspose.Slides.License));


            Type typeLic = null, typeIsTrial = null, typeHelper = null;

            foreach (Type type in assembly.GetTypes())
            {
                if ((typeLic == null) && (type.Name == stModule[0]))
                {
                    typeLic = type;
                }
                else if ((typeIsTrial == null) && (type.Name == stModule[1]))
                {
                    typeIsTrial = type;
                }
                else if ((typeHelper == null) && (type.Name == stModule[2]))
                {
                    typeHelper = type;
                }
            }
            if (typeLic == null || typeIsTrial == null || typeHelper == null)
            {
                throw new Exception();
            }
            object lic = Activator.CreateInstance(typeLic);
            int findCount = 0;

            foreach (FieldInfo field in typeLic.GetFields(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
            {
                if (field.FieldType == typeLic && field.Name == stModule[3])
                {
                    field.SetValue(null, lic);
                    ++findCount;
                }
                else if (field.FieldType == typeof(DateTime) && field.Name == stModule[4])
                {
                    field.SetValue(lic, DateTime.MaxValue);
                    ++findCount;
                }
                else if (field.FieldType == typeIsTrial && field.Name == stModule[5])
                {
                    field.SetValue(lic, 1);
                    ++findCount;
                }

            }
            foreach (FieldInfo field in typeHelper.GetFields(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
            {
                if (field.FieldType == typeof(bool) && field.Name == stModule[6])
                {
                    field.SetValue(null, false);
                    ++findCount;
                }
                if (field.FieldType == typeof(int) && field.Name == stModule[7])
                {
                    field.SetValue(null, 128);
                    ++findCount;
                }
            }
            if (findCount < 5)
            {
                throw new NotSupportedException("无效的版本");
            }
        }

        private void WriteToPPT(string filePath)
        {
            /*打开PPT模板*/
            WritePPT mWritePPT = new WritePPT(filePath, "991");
            /*按分工单生成新的PPT*/
            /*填充PPT页面数据*/

            /*保存PPT*/
            mWritePPT.SavePPT("123456.pptx");
        }

        private void NewPPT()
        {
            Presentation mPresentation = new Presentation(AppDomain.CurrentDomain.BaseDirectory + @"File\PPTX\20170703作业方案.ppt");
            ISlideCollection mSlideCollection = mPresentation.Slides;
            Slide mSlide, mCopySlide;
            IShapeCollection mShapeCollection;
            IShape mShape;
            for (int i = mSlideCollection.Count - 1; i > -1; i--)
            {
                mSlide = mSlideCollection[i] as Slide;
                if (mSlide.Name.Contains("TCZPage"))
                {
                    mCopySlide = mPresentation.Slides.InsertClone(i, mSlide) as Slide;
                    mShapeCollection = mCopySlide.Shapes;
                    for (int j = mShapeCollection.Count - 1; j > -1; j--)
                    {
                        mShape = mShapeCollection[j] as Aspose.Slides.Shape;
                        if (mShape.Name == "GroupName")
                        {
                            if (mShape.GetType() == typeof(AutoShape))
                            {
                                ((AutoShape)mShape).Height = 2 * ((AutoShape)mShape).Height;
                                ((AutoShape)mShape).TextFrame.Text = "克隆小组大家阿斯顿浪费净资产阿斯顿法律金钱啊我饿杰拉德";
                            }
                        }
                    }
                }
            }
            mPresentation.Save(AppDomain.CurrentDomain.BaseDirectory + @"File\PPTX\123456作业方案.ppt", Aspose.Slides.Export.SaveFormat.Pptx);
        }
    }
}
