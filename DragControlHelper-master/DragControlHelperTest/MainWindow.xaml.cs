using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UICommon.Controls;

namespace DragControlHelperTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum shapeName { Empty, Canvas, Rectangle, Ellipse, Line, Polyline, TextBox };/*图形类型：无、矩形、圆、直线、箭头、输入框*/
        public enum operatorType { Empty, Add, Move };
        private operatorType mOperatorType = operatorType.Empty;             /*操作类型*/
        private shapeName mShapeName = shapeName.Empty; /*形状名*/
        private Object mSelectShape;                    /*当前形状*/
        private Point nullPoint = new Point(-1, -1);    /*无效坐标,用于统一标准*/
        private Point mMouseDownPoint, mMouseTempPoint;   /*鼠标点下，当前坐标*/
        private PointCollection mPointCollection = new PointCollection();    /*用于记录图形最初坐标*/

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        #region 初始化
        private void Init()
        {
            toolbar1.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new RoutedEventHandler(shape_click));/*添加对工具栏图形选择的统一处理*/
            mMouseDownPoint = nullPoint;
            mMouseTempPoint = nullPoint;
        }
        #endregion

        #region 工具栏点击事件
        private void shape_click(object sender, RoutedEventArgs e)
        {
            string mName = (e.OriginalSource as System.Windows.Controls.RadioButton).Name;
            switch (mName)
            {
                case "rbRectangle":
                    AddRectangle(canvas1,null);
                    mShapeName = shapeName.Rectangle;
                    break;
                case "rbCircle":
                    AddEllipse(canvas1);
                    mShapeName = shapeName.Ellipse;
                    break;
                case "rbLine":
                    mShapeName = shapeName.Line;
                    break;
                case "rbArrow":
                    mShapeName = shapeName.Polyline;
                    break;
                case "rbInput":
                    mShapeName = shapeName.TextBox;
                    break;
                case "rbTC1":
                    AddRectangle(canvas1, @"image\tc1.png");
                    mShapeName = shapeName.Rectangle;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region 画布-左键事件
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mMouseDownPoint = e.GetPosition(canvas1);
            mPointCollection.Clear();
            /*只记录选中的绘图元素*/
            switch ((Int32)Enum.Parse(typeof(shapeName), e.Source.GetType().Name))
            {
                case (Int32)shapeName.Ellipse:
                    mSelectShape = e.Source;
                    break;
                case (Int32)shapeName.Rectangle:
                    mSelectShape = e.Source;
                    break;
                case (Int32)shapeName.Line:
                    Line mLine = e.Source as Line;
                    mPointCollection.Add(new Point(mLine.X1, mLine.Y1));
                    mPointCollection.Add(new Point(mLine.X2, mLine.Y2));
                    mSelectShape = e.Source;
                    mOperatorType = operatorType.Move;
                    break;
                case (Int32)shapeName.Polyline:
                    mPointCollection = (e.Source as Polyline).Points.Clone();
                    mSelectShape = e.Source;
                    mOperatorType = operatorType.Move;
                    break;
                case (Int32)shapeName.TextBox:
                    /*输入框额外处理*/
                    break;
                default:
                    mSelectShape = null;
                    if (InitOther() > 0) mOperatorType = operatorType.Add;
                    break;
            }
        }

        private void OnMouseLeftButtonMove(object sender, MouseEventArgs e)
        {
            mMouseTempPoint = e.GetPosition(canvas1);            
            switch (mOperatorType)
            {
                case operatorType.Add:
                    AlterOther();
                    break;
                case operatorType.Move:                   
                    Move(new Point(mMouseTempPoint.X - mMouseDownPoint.X, mMouseTempPoint.Y - mMouseDownPoint.Y));                    
                    break;
                default:
                    break;
            }
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (mOperatorType == operatorType.Add)
            {
                FinishOther();
            }
            else if (mOperatorType == operatorType.Move)
            {
                canvas1.UpdateLayout();
            }
            mShapeName = shapeName.Empty;
            mOperatorType = operatorType.Empty;
        }
        #endregion

        #region （直线、箭头、文本框）初始化、修改、完成
        private int InitOther()
        {
            int num = 0;
            switch (mShapeName)
            {
                case shapeName.Line:
                    mSelectShape = InitLine(canvas1, null, mMouseDownPoint, mMouseDownPoint);
                    num = 1;
                    break;
                case shapeName.Polyline:
                    mSelectShape = InitArrow(canvas1, null, mMouseDownPoint, mMouseDownPoint);
                    num = 1;
                    break;
                case shapeName.TextBox:
                    mSelectShape = AddInput(canvas1, mMouseDownPoint);
                    num = 1;
                    break;
                default:
                    break;
            }
            return num;
        }
        private void AlterOther()
        {
            switch (mShapeName)
            {
                case shapeName.Line:
                    mSelectShape = InitLine(null, mSelectShape as Line, mMouseDownPoint, mMouseTempPoint);
                    break;
                case shapeName.Polyline:
                    mSelectShape = InitArrow(null, mSelectShape as Polyline, mMouseDownPoint, mMouseTempPoint);
                    break;
                default:
                    break;
            }
        }
        private int FinishOther()
        {
            int num = 0;
            switch (mShapeName)
            {
                case shapeName.Line:
                case shapeName.Polyline:
                    (mSelectShape as Shape).Cursor = Cursors.SizeAll;
                    num = 1;
                    break;
                case shapeName.TextBox:
                    num = 1;
                    break;
                default:
                    num = 0;
                    break;
            }
            return num;
        }
        #endregion

        #region 删除
        private void Window_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && mSelectShape != null)
            {
                this.canvas1.Children.Remove(mSelectShape as UIElement);
            }
        }
        #endregion

        #region 添加
        /*圆*/
        private Ellipse AddEllipse(Canvas parentCanvas)
        {
            Ellipse mEllipse = new Ellipse()
            {
                Width = 30,
                Height = 30,
                Fill = new SolidColorBrush(Colors.Red)
            };
            mEllipse.SetValue(DragControlHelper.IsSelectableProperty, true);
            mEllipse.SetValue(DragControlHelper.IsEditableProperty, true);
            parentCanvas.Children.Add(mEllipse);
            return mEllipse;
        }
        /*矩形*/
        private Rectangle AddRectangle(Canvas parentCanvas,string bgImgUri)
        {
            Rectangle mRectangle = new Rectangle()
            {
                Width = 30,
                Height = 30,
                Fill = string.IsNullOrEmpty(bgImgUri) ? new SolidColorBrush(Colors.Red) as Brush : new ImageBrush() { ImageSource = new BitmapImage(new Uri(bgImgUri,UriKind.Relative)) } as Brush,
            };
            mRectangle.SetValue(DragControlHelper.IsSelectableProperty, true);
            mRectangle.SetValue(DragControlHelper.IsEditableProperty, true);
            parentCanvas.Children.Add(mRectangle);
            return mRectangle;
        }
        /*线*/
        private Line InitLine(Canvas parentCanvas, Line mLine, Point mPoint1, Point mPoint2)
        {
            if (mLine == null)
            {
                mLine = new Line()
                {
                    X1 = mPoint1.X,
                    Y1 = mPoint1.Y,
                    X2 = mPoint2.X,
                    Y2 = mPoint2.Y,
                    Stroke = new SolidColorBrush(Colors.LightPink),
                    StrokeThickness = 2
                };
            }
            else
            {
                mLine.X1 = mPoint1.X;
                mLine.Y1 = mPoint1.Y;
                mLine.X2 = mPoint2.X;
                mLine.Y2 = mPoint2.Y;
            }
            if (parentCanvas != null)
            {
                parentCanvas.Children.Add(mLine);
            }
            return mLine;
        }
        /*箭头*/
        private Polyline InitArrow(Canvas parentCanvas, Polyline mPolyline, Point mPoint1, Point mPoint2)
        {
            PointCollection mPointCollection = new PointCollection();
            mPointCollection.Add(mPoint1);
            mPointCollection.Add(mPoint2);

            if (mPolyline == null)
            {
                mPolyline = new Polyline()
               {
                   StrokeLineJoin = PenLineJoin.Round,
                   StrokeThickness = 2,
                   Points = mPointCollection,
                   Stroke = new SolidColorBrush(Colors.LightPink)
               };
            }
            else
            {
                List<Point> mPointList = calcucatePoint(mPoint1, mPoint2, 50, 15);
                mPointCollection.Add(mPointList[0]);
                mPointCollection.Add(mPoint2);
                mPointCollection.Add(mPointList[1]);
                mPolyline.Points = mPointCollection;
            }
            if (parentCanvas != null)
            {
                parentCanvas.Children.Add(mPolyline);
            }
            return mPolyline;
        }
        /*计算坐标*/
        private List<Point> calcucatePoint(Point startPoint, Point endPoint, double angle, double distance)
        {
            double temX, temY;
            Point point1 = new Point(), point2 = new Point();

            if (endPoint.X - startPoint.X == 0)
            { //斜率不存在（即Y轴上）
                temX = endPoint.X;
                if (endPoint.Y > startPoint.Y)
                {
                    temY = endPoint.Y - distance;
                }
                else
                {
                    temY = endPoint.Y + distance;
                }
                //已知直角三角形两个点坐标及其中一个角，求另外一个点坐标算法  
                point1.X = temX - distance * Math.Tan(angle);
                point2.X = temX + distance * Math.Tan(angle);
                point1.Y = point2.Y = temY;
            }
            else  //斜率存在时  
            {
                var delta = (endPoint.Y - startPoint.Y) / (endPoint.X - startPoint.X);
                var param = Math.Sqrt(delta * delta + 1);

                if ((endPoint.X - startPoint.X) < 0) //第二、三象限  
                {
                    temX = endPoint.X + distance / param;
                    temY = endPoint.Y + delta * distance / param;
                }
                else//第一、四象限  
                {
                    temX = endPoint.X - distance / param;
                    temY = endPoint.Y - delta * distance / param;
                }
                //已知直角三角形两个点坐标及其中一个角，求另外一个点坐标算法 
                point1 = new Point(temX + Math.Tan(angle) * distance * delta / param, temY - Math.Tan(angle) * distance / param);
                point2 = new Point(temX - Math.Tan(angle) * distance * delta / param, temY + Math.Tan(angle) * distance / param);
            }
            return new List<Point> { point1, point2 };
        }
        /*输入框*/
        private ContentControl AddInput(Canvas parentCanvas, Point mPoint1)
        {
            ContentControl mContentControl = new ContentControl()
            {
                Template =Application.Current.Resources["TxtThumbTemplate"] as ControlTemplate
            };
            mContentControl.Content = InitTextBox();
            Canvas.SetLeft(mContentControl, mPoint1.X);
            Canvas.SetTop(mContentControl, mPoint1.Y);
            parentCanvas.Children.Add(mContentControl);
            
            return mContentControl;
        }
        /*初始化一个TextBox*/
        private TextBox InitTextBox()
        {
            TextBox mTextBox = new TextBox()
            {
                MinWidth = 20,
                MinHeight = 25,
                AcceptsReturn = true,
                AcceptsTab = true,
                Background = new SolidColorBrush(Colors.Transparent),
                FontSize = 28
            };
            mTextBox.PreviewMouseLeftButtonDown += mTextBox_PreviewMouseLeftButtonDown;
            return mTextBox;
        }

        private void mTextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mSelectShape = e.Source;
            TextBox mTextBox = e.Source as TextBox;
            //mMouseDownPoint = new Point(Canvas.GetLeft(mTextBox), Canvas.GetTop(mTextBox));
            //mMouseDownPoint = e.MouseDevice.NonRelativePosition;
            mPointCollection.Add(mMouseDownPoint);

            //mMouseDownPoint=e.MouseDevice.GetPosition(sender As TextBox );

            //mTextBox.FontSize = 12;
            //mTextBox.FontStyle = FontStyles.Italic;
            //mTextBox.Foreground = new SolidColorBrush(Colors.Red);

            mOperatorType = operatorType.Empty;
        }
        #endregion

        #region 移动
        private void Move(Point mDiffPoint)
        {
            if (mSelectShape == null) return;
            switch ((Int32)Enum.Parse(typeof(shapeName), mSelectShape.GetType().Name))
            {
                case (Int32)shapeName.Line:
                    Line mLine = mSelectShape as Line;
                    mLine.X1 = mPointCollection[0].X + mDiffPoint.X;
                    mLine.Y1 = mPointCollection[0].Y + mDiffPoint.Y;
                    mLine.X2 = mPointCollection[1].X + mDiffPoint.X;
                    mLine.Y2 = mPointCollection[1].Y + mDiffPoint.Y;
                    break;
                case (Int32)shapeName.Polyline:
                    Polyline mPolyline = mSelectShape as Polyline;

                    string mPointStr = string.Empty;
                    for (int i = mPointCollection.Count - 1; i > -1; i--)
                    {
                        mPointStr += "(" + Convert.ToInt32(mPointCollection[i].X).ToString() + "," + Convert.ToInt32(mPointCollection[i].Y).ToString()+")、";
                        mPolyline.Points[i] = new Point(mPointCollection[i].X + mDiffPoint.X, mPointCollection[i].Y + mDiffPoint.Y);
                    }
                    break;
                case (Int32)shapeName.TextBox:
                    TextBox mTextBox = mSelectShape as TextBox;
                    Canvas.SetLeft(mTextBox, mPointCollection[0].X + mDiffPoint.X);
                    Canvas.SetTop(mTextBox, mPointCollection[0].Y + mDiffPoint.Y);
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
