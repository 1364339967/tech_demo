using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Aspose.Slides;
using Newtonsoft.Json.Linq;
using WriteToPPT.Info;

namespace WriteToPPT
{
    public class WritePPT
    {
        Presentation mPresentationTemplate, mPresentation = new Presentation();
        string mOrderID = string.Empty;

        /*初始化实例对象（PPT模板、分工单ID）*/
        public WritePPT(string fileName, string mID)
        {
            #region 初始化
            if (!File.Exists(fileName))
            {
                return;
            }
            Crack();
            mPresentationTemplate = new Presentation(fileName);
            mOrderID = mID;
            #endregion
            #region 创建目录
            #region 目录树
            JArray mCatalogJArray = new JArray();
            string[] mutilPage = { "安全风险", "风险源", "业务学习", "危险源" };
            foreach (KeyValuePair<string, string> mPair in Common.PageType)
            {
                if (!string.IsNullOrEmpty(mPair.Value))
                {
                    if (mutilPage.Contains(mPair.Key))
                    {
                        mCatalogJArray.Add(new JObject(new JProperty("page-type", mPair.Value), new JProperty("data-id", "991_1")));
                        mCatalogJArray.Add(new JObject(new JProperty("page-type", mPair.Value), new JProperty("data-id", "991_2")));
                    }
                    else
                    {
                        mCatalogJArray.Add(new JObject(new JProperty("page-type", mPair.Value), new JProperty("data-id", "991")));
                    }
                }
                else
                {
                    foreach (KeyValuePair<string, string> mGroup in Common.GroupPageType)
                    {
                        mCatalogJArray.Add(new JObject(new JProperty("page-type", mGroup.Value), new JProperty("data-id", "991_" + new Random().Next(200))));
                    }
                }
            }
            #endregion
            for (int i = mPresentation.Slides.Count - 1; i > -1; i--)
            {
                mPresentation.Slides.RemoveAt(i);
            }
            mPresentation.SlideSize.Size = mPresentationTemplate.SlideSize.Size;
            SetPPTSlide(mPresentationTemplate, mPresentation, mCatalogJArray);
            #endregion
            #region 填充数据
            JObject mPageDataJObject = new JObject();
            #region 作业方案
            mPageDataJObject.Add(
                new JProperty("作业方案",
                    new JObject(
                        new JProperty("ticketNum", "07-13-01"),
                        new JProperty("workShop", "广州供电段"),
                        new JProperty("workArea", "广州南高铁工区广州南高铁工区"),
                        new JProperty("meetingDate", "2017年07月15日"))
                ));
            #endregion
            #region 点名及检查着装
            mPageDataJObject.Add(
                new JProperty("点名及检查着装",
                    new JObject(
                        new JProperty("workLeader", "广州南工作领导人"),
                        new JProperty("mettingPerson",
                            new JObject(
                                new JProperty("坐台人员", "阿三(一)、阿胖是谁(无)、一号坐台人员(五)"),
                                new JProperty("高空作业人员", "荣庞观(一)、乔荣峰(五)、荣庞观荣(一)、乔荣峰(五)、荣庞观(一)、乔荣峰(五)、荣庞观荣(一)、乔荣峰(五)、荣庞观(一)、乔荣峰(五)、荣庞观荣(一)、乔荣峰(五)、荣庞观(一)、乔荣峰(五)、荣庞观荣(一)、乔荣峰(五)、荣庞观(一)、乔荣峰(五)、荣庞观荣(一)、乔荣峰(五)、荣庞观(一)、乔荣峰(五)、荣庞观荣(一)、乔荣峰(五)"),
                                new JProperty("司机", "小花、廖观亮、阿阿啊啊、老司机、新手上路、马路杀手、小花、廖观亮、阿阿啊啊、老司机、新手上路、马路杀手、小花、廖观亮、阿阿啊啊、老司机、新手上路、马路杀手"))))
                ));
            #endregion
            #region 核对计划
            mPageDataJObject.Add(
                new JProperty("核对计划",
                    new JArray(
                            new JObject(
                                new JProperty("SkylightDate", "2017071301"),
                                new JProperty("PlanNum", "071301"),
                                new JProperty("Line", "广珠城际"),
                                new JProperty("UpDownLink", "上行,下行"),
                                new JProperty("Mileage", "88km446m-89km663m"),
                                new JProperty("RegistrationStation", "上下行登记站"),
                                new JProperty("Project", "接触网设备小修、网开关调试、电缆头检修"),
                                new JProperty("ConstructionUnit", "广州供电段"),
                                new JProperty("OperationTime", "00:00-01:50(110分钟)"),
                                new JProperty("SkylightType", "垂直夜间"),
                                new JProperty("RepairType", "供电"),
                                new JProperty("EffectAndDemand", "1、作业范围：南朗-翠亨上、下行线K88+446-K89+663；2、人工车梯作业；3、停电范围：新乐昌变电所至桂头分区所上、下行供电臂，桂头分区所至新韶关变电所上、下行供电臂，停电单元：武广52、53、54、55； 4、作业工区：中山高铁接触网工区；5、跟班干部：王文雄（13570221401）副工长，6、联络员：中山高铁接触网工区靳圣楠（14985731401）。"),
                                new JProperty("TrafficLimitCard", "武广52,武广53,武广54,武广55"),
                                new JProperty("Principal", "周夏明（接触网工）")
                            ),
                            new JObject(
                                new JProperty("SkylightDate", "2017071301"),
                                new JProperty("PlanNum", "071301"),
                                new JProperty("Line", "广珠城际"),
                                new JProperty("UpDownLink", "上行,下行"),
                                new JProperty("Mileage", "88km446m-89km663m"),
                                new JProperty("RegistrationStation", "上下行登记站"),
                                new JProperty("Project", "接触网设备小修、网开关调试、电缆头检修"),
                                new JProperty("ConstructionUnit", "广州供电段"),
                                new JProperty("OperationTime", "00:00-01:50(110分钟)"),
                                new JProperty("SkylightType", "垂直夜间"),
                                new JProperty("RepairType", "供电"),
                                new JProperty("EffectAndDemand", "1008、作业范围：南朗-翠亨上、下行线K88+446-K89+663；2、人工车梯作业；3、停电范围：新乐昌变电所至桂头分区所上、下行供电臂，桂头分区所至新韶关变电所上、下行供电臂，停电单元：武广52、53、54、55； 4、作业工区：中山高铁接触网工区；5、跟班干部：王文雄（13570221401）副工长，6、联络员：中山高铁接触网工区靳圣楠（14985731401）。"),
                                new JProperty("TrafficLimitCard", "武广52,武广53,武广54,武广55"),
                                new JProperty("Principal", "周夏明（接触网工）")
                            )
                            )));
            #endregion
            #region 施工方案
            mPageDataJObject.Add(
                new JProperty("施工方案",
                    new JObject(
                        new JProperty("base", new JObject(
                            new JProperty("workLeader", "顾明10086(3级)"),
                            new JProperty("personNum", "8"),
                            new JProperty("workPlace", "广州南-碧江(含Ⅰ、Ⅱ道及GZ3、GZ4、GZ5、GZ2号道岔)-北滘(含Ⅰ、Ⅱ道)"),
                            new JProperty("SkylightDate", "2017年04月20日"),
                            new JProperty("Content", "接触网设备小修、网开关调试、电缆头检修"),
                            new JProperty("timeArrange", "20日 00：00开作业方案会，20日 24：00点名分工"))),
                        new JProperty("equipment", new JArray(
                            new JObject(
                                new JProperty("category", "地面柱"),
                                new JProperty("equipmentName", "189、185"),
                                new JProperty("count", "3")
                                ),
                            new JObject(
                                new JProperty("category", "支柱"),
                                new JProperty("equipmentName", "101、103"),
                                new JProperty("count", "2")
                                )
                            )),
                        new JProperty("cararrange", new JArray(
                            new JObject(
                                new JProperty("car", "无牌照汽车"),
                                new JProperty("driver", "SJ1"),
                                new JProperty("takepersons", "2"),
                                new JProperty("route", "从大门到小门"),
                                new JProperty("safetysupervisor", "小安"),
                                new JProperty("remark", "上下道地点待定")
                                ),
                            new JObject(
                                new JProperty("car", "雪佛兰999"),
                                new JProperty("driver", "老司机"),
                                new JProperty("takepersons", "工作领导人"),
                                new JProperty("route", "从工区头巡视到工区尾"),
                                new JProperty("safetysupervisor", "1号安全监督员"),
                                new JProperty("remark", "上下道地点待定")
                                )
                            ))
                        )));
            #endregion
            #region 作业区防护措施
            mPageDataJObject.Add(
                new JProperty("作业区防护措施",
                        new JObject(
                            new JProperty("width", 157),
                            new JProperty("height", 157),
                            new JProperty("path", @"D:\源代码\工具材料\EMMS_JCW\EMMS_JCW\ImageLibrary\智能头盔_07.png"))
                ));
            #endregion
            #region 业务学习
            mPageDataJObject.Add(
                new JProperty("业务学习",
                    new JArray(
                        new JObject(
                            new JProperty("width", 157),
                            new JProperty("height", 157),
                            new JProperty("path", @"D:\源代码\工具材料\EMMS_JCW\EMMS_JCW\ImageLibrary\智能头盔_07.png"),
                            new JProperty("txt", "这是一段文本，非常长的文本")),
                        new JObject(
                            new JProperty("width", 180),
                            new JProperty("height", 180),
                            new JProperty("path", @"D:\源代码\工具材料\EMMS_JCW\EMMS_JCW\ImageLibrary\单机版系统.png"),
                            new JProperty("txt", "这是一段文本，非常长的文本"))
                            )
                ));
            #endregion
            #region 工作小组
            mPageDataJObject.Add(
                new JProperty("工作组",
                    new JArray(
                        new JObject(
                            new JProperty("GroupName", "车梯1组"),
                            new JProperty("workPlace", "广州供电段"),
                            new JProperty("workArea", "广州南高铁工区广州南高铁工区"),
                            new JProperty("ticketNum", "07-03-01"),
                            new JProperty("Licence", "金杯小面包"),
                            new JProperty("person", new JObject(new JProperty("Principle", "小组负责人：乔峰(一)"), new JProperty("AerialMan", "高空人员：乔峰(二)"), new JProperty("Assistant", "辅助人员：乔峰(三)"))),
                            new JProperty("WorkContent", "检修设备：一号战场1号股道短柱子扳手、扳手、扳手；肇庆站场1肇庆一股道CC001、避雷器；肇庆站场3肇庆三股道中型大刀大刀；赣韶普速梅关-珠玑巷单线关节式069；赣韶普速珠玑巷-南雄无氧化锌G10-；广茂线广州西站（广茂）I道MDGJ666；"),
                            new JProperty("Category", "铁开口滑轮1T 4把"),
                            new JProperty("alterCategory", ""),
                            new JProperty("Attention", "1.梯车:接地前须先进行验电操作；\n2.梯车:接地前先对钢轨除锈；\n3.梯车:行走在线路时，看清路况，注意不要滑到摔伤；")
                            ),
                        new JObject(
                            new JProperty("GroupName", "车梯2组"),
                            new JProperty("workPlace", "广州供电段"),
                            new JProperty("workArea", "广州南高铁工区广州南高铁工区"),
                            new JProperty("ticketNum", "07-03-01"),
                            new JProperty("Licence", "金杯小面包"),
                            new JProperty("person", new JObject(new JProperty("Principle", "小组负责人：乔峰(一)"), new JProperty("AerialMan", "高空人员：乔峰(二)"), new JProperty("Assistant", "辅助人员：乔峰(三)"))),
                            new JProperty("WorkContent", "检修设备：一号战场1号股道短柱子扳手、扳手、扳手；肇庆站场1肇庆一股道CC001、避雷器；肇庆站场3肇庆三股道中型大刀大刀；赣韶普速梅关-珠玑巷单线关节式069；赣韶普速珠玑巷-南雄无氧化锌G10-；广茂线广州西站（广茂）I道MDGJ666；"),
                            new JProperty("Category", "铁开口滑轮1T 4把"),
                            new JProperty("alterCategory", ""),
                            new JProperty("Attention", "1.梯车:接地前须先进行验电操作；\n2.梯车:接地前先对钢轨除锈；\n3.梯车:行走在线路时，看清路况，注意不要滑到摔伤；")
                            ),
                        new JObject(
                            new JProperty("GroupName", "测量1组"),
                            new JProperty("workPlace", "广州供电段"),
                            new JProperty("workArea", "广州南高铁工区广州南高铁工区"),
                            new JProperty("ticketNum", "07-03-01"),
                            new JProperty("Licence", "金杯小面包"),
                            new JProperty("person", new JObject(new JProperty("Principle", "小组负责人：乔峰(一)"), new JProperty("MeasureMan", "测量人员：乔峰(二)"), new JProperty("Recorder", "记录人员：乔峰(三)"))),
                            new JProperty("WorkContent", "检修设备：一号战场1号股道短柱子扳手、扳手、扳手；肇庆站场1肇庆一股道CC001、避雷器；肇庆站场3肇庆三股道中型大刀大刀；赣韶普速梅关-珠玑巷单线关节式069；赣韶普速珠玑巷-南雄无氧化锌G10-；广茂线广州西站（广茂）I道MDGJ666；"),
                            new JProperty("Category", "铁开口滑轮1T 4把"),
                            new JProperty("alterCategory", ""),
                            new JProperty("Attention", "1.梯车:接地前须先进行验电操作；\n2.梯车:接地前先对钢轨除锈；\n3.梯车:行走在线路时，看清路况，注意不要滑到摔伤；")
                            ),
                        new JObject(
                           new JProperty("GroupName", "地线1组"),
                           new JProperty("workPlace", "广州供电段"),
                           new JProperty("workArea", "广州南高铁工区广州南高铁工区"),
                           new JProperty("ticketNum", "07-03-01"),
                           new JProperty("Licence", "金杯小面包"),
                           new JProperty("person", new JObject(new JProperty("GuardianAndProtectionMan", "接地监护人及行车防护：乔峰(一)"), new JProperty("Operator", "操作人员：乔峰(二)"))),
                           new JProperty("WorkContent", "检修设备：一号战场1号股道短柱子扳手、扳手、扳手；肇庆站场1肇庆一股道CC001、避雷器；肇庆站场3肇庆三股道中型大刀大刀；赣韶普速梅关-珠玑巷单线关节式069；赣韶普速珠玑巷-南雄无氧化锌G10-；广茂线广州西站（广茂）I道MDGJ666；"),
                           new JProperty("Category", "铁开口滑轮1T 4把"),
                           new JProperty("alterCategory", ""),
                           new JProperty("Attention", "1.梯车:接地前须先进行验电操作；\n2.梯车:接地前先对钢轨除锈；\n3.梯车:行走在线路时，看清路况，注意不要滑到摔伤；")
                           ),
                        new JObject(
                            new JProperty("GroupName", "巡视1组"),
                            new JProperty("workPlace", "广州供电段"),
                            new JProperty("workArea", "广州南高铁工区广州南高铁工区"),
                            new JProperty("ticketNum", "07-03-01"),
                            new JProperty("Licence", "金杯小面包"),
                            new JProperty("person", new JObject(new JProperty("Principle", "小组负责人：乔峰(一)"), new JProperty("Guardian", "监护人员：乔峰(二)"), new JProperty("AerialMan", "巡视人员：乔峰(三)"), new JProperty("Assistant", "移动防护人员：乔峰(三)"))),
                            new JProperty("WorkContent", "检修设备：一号战场1号股道短柱子扳手、扳手、扳手；肇庆站场1肇庆一股道CC001、避雷器；肇庆站场3肇庆三股道中型大刀大刀；赣韶普速梅关-珠玑巷单线关节式069；赣韶普速珠玑巷-南雄无氧化锌G10-；广茂线广州西站（广茂）I道MDGJ666；"),
                            new JProperty("Category", "铁开口滑轮1T 4把"),
                            new JProperty("alterCategory", ""),
                            new JProperty("Attention", "1.梯车:接地前须先进行验电操作；\n2.梯车:接地前先对钢轨除锈；\n3.梯车:行走在线路时，看清路况，注意不要滑到摔伤；")
                            )
                    )
                ));
            #endregion
            #region 人身安全方案
            mPageDataJObject.Add(
                new JProperty("人身安全方案",
                        new JObject(
                            new JProperty("safatestep",
                                new JObject(
                                    new JProperty("材料准备", "1.人身安全--材料准备\n2.人身安全--材料准备#\n3.人身安全---道路安全#"),
                                    new JProperty("出工组织", "1.人身安全--材料准备\n2.人身安全--材料准备#\n3.人身安全---道路安全#"),
                                    new JProperty("人身安全", "1.人身安全注意事项#"),
                                    new JProperty("夜间作业", "1.人身安全--夜间作业##")
                                    )))
                ));
            #endregion
            #region 施工行车方案
            mPageDataJObject.Add(
                new JProperty("施工行车方案",
                        new JObject(
                            new JProperty("safatestep",
                                new JObject(
                                    new JProperty("交通安全", "1.人身安全--材料准备\n2.人身安全--材料准备#\n3.人身安全---道路安全#"),
                                    new JProperty("行车安全", "1.人身安全--材料准备\n2.人身安全--材料准备#\n3.人身安全---道路安全#")
                                    )))
                ));
            #endregion
            #region 安全风险
            mPageDataJObject.Add(
               new JProperty("安全风险", "1.作业人员上下道工具材料时应注意台阶，不要拥挤、注意防滑，确保人身安全；出车前汽车司机负责检查车辆，确保机油、水、刹车等状态良好；行走不良路段时应小心慢行，确保人车安全。\n2.作业人员上下道工具材料时应注意台阶，不要拥挤、注意防滑，确保人身安全；出车前汽车司机负责检查车辆，确保机油、水、刹车等状态良好；行走不良路段时应小心慢行，确保人车安全。\n3.严格执行上下道物料卡死制度。每次作业前后，均要进行上下道物料的登记，禁止遗留任何物品现场，一经发现，比照集团“红线”管理办法进行分析考核处理。\n4.严格执行上下道物料卡死制度。每次作业前后，均要进行上下道物料的登记，禁止遗留任何物品现场，一经发现，比照集团“红线”管理办法进行分析考核处理。"
               ));
            #endregion
            #region 安全源
            mPageDataJObject.Add(
               new JProperty("风险源", "1.风险名称：联络\n2.风险名称：联络员与工作领导人联系中断无法及时掌控情况和行车防护信息（QGZ01-H009）防范措施：作业中驻站联络员与行车防护员要向工作领导人确保联系畅通\n3.风险名称：作业工具、件遗留在道上（QGZ3301-A023）防范措施：认真检查道上无遗留工具、材料才能结束作业。\n4.风险名称：作业工具、件遗留在道上（QGZ3301-A023）防范措施：认真检查道上无遗留工具、材料才能结束作业。\n5.风险名称：作业工具、件遗留在道上（QGZ3301-A023）防范措施：认真检查道上无遗留工具、材料才能结束作业。"
               ));
            #endregion
            #region 危险源
            mPageDataJObject.Add(
                new JProperty("危险源",
                    new JArray(
                        new JObject(
                            new JProperty("width", 157),
                            new JProperty("height", 157),
                            new JProperty("path", @"D:\源代码\工具材料\EMMS_JCW\EMMS_JCW\ImageLibrary\智能头盔_07.png"),
                            new JProperty("txt", "这是一段文本，非常长的文本")),
                        new JObject(
                            new JProperty("width", 180),
                            new JProperty("height", 180),
                            new JProperty("path", @"D:\源代码\工具材料\EMMS_JCW\EMMS_JCW\ImageLibrary\单机版系统.png"),
                            new JProperty("txt", "这是一段文本，非常长的文本"))
                            )
                ));
            #endregion
            FillSlideData(mPresentation.Slides, mPageDataJObject);
            #endregion
            #region 保存
            SavePPT(@"File\PPTX\123456.pptx");
            #endregion
        }


        #region 软破解
        /*使用Apose前调用一次软破解即可*/
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
        #endregion

        #region 填充PPT
        /*生成PPT目录，参数结构：[{page-type,data-id},{page-type,data-id},...]*/
        private void SetPPTSlide(Presentation mTemplate, Presentation newPPT, JArray mSlideTree)
        {
            ISlideCollection mSlideCollection = mTemplate.Slides;
            foreach (JObject mJObject in mSlideTree)
            {
                foreach (ISlide mSlide in mTemplate.Slides)
                {
                    if (Regex.Match(mSlide.Name, "^[a-zA-Z|-]+").ToString().Equals(mJObject["page-type"].ToString()))
                    {
                        newPPT.Slides.AddClone(mSlide).Name = mJObject["page-type"].ToString() + "_" + mJObject["data-id"].ToString();
                        break;
                    }
                }
            }
        }

        /*往幻灯片中填充数据*/
        private void FillSlideData(ISlideCollection mSlideCollection, JObject mDataJObject)
        {
            ISlide mSlide;
            IShapeCollection mShapeCollection;
            IShape mShape;
            AutoShape mAutoShape;
            string pageType = string.Empty;
            int shapeIndex = 0;
            for (int slideIndex = 0, slideMaxIndex = mSlideCollection.Count - 1; slideIndex < slideMaxIndex; slideIndex++)
            {
                mSlide = mSlideCollection[slideIndex];
                mShapeCollection = mSlide.Shapes;
                shapeIndex = mShapeCollection.Count - 1;
                pageType = Regex.Match(mSlide.Name, "^[a-zA-Z|-]+").ToString();
                #region 非小组
                if (pageType.Equals(Common.PageType[Common.PageEnum.作业方案.ToString()]))
                {
                    #region 作业方案
                    JObject mJObject = mDataJObject.Property(Common.PageEnum.作业方案.ToString()).Value as JObject;
                    for (; shapeIndex > -1; shapeIndex--)
                    {
                        switch (mShapeCollection[shapeIndex].GetType().Name)
                        {
                            case "AutoShape":
                                SetSingleText(mShapeCollection[shapeIndex] as AutoShape, mJObject.Property(mShapeCollection[shapeIndex].Name));
                                break;
                        }
                    }
                    #endregion
                }
                else if (pageType.Equals(Common.PageType[Common.PageEnum.点名及检查着装.ToString()]))
                {
                    #region 点名及检查着装
                    ITextFrame template_titleTextFrame = null, template_contentTextFrame = null;
                    #region 模板样式
                    for (; shapeIndex > -1; shapeIndex--)
                    {
                        mShape = mShapeCollection[shapeIndex];
                        if (mShape.GetType() == typeof(AutoShape))
                        {
                            mAutoShape = mShape as AutoShape;
                            if (mShape.Name.Contains("content-title"))
                            {
                                template_titleTextFrame = mAutoShape.TextFrame;
                                mShapeCollection.Remove(mShape);
                            }
                            else if (mShape.Name.Contains("content"))
                            {
                                template_contentTextFrame = mAutoShape.TextFrame;
                                mShapeCollection.Remove(mShape);
                            }
                        }
                    }
                    #endregion
                    #region 填充数据
                    JObject mJObject = mDataJObject.Property(Common.PageEnum.点名及检查着装.ToString()).Value as JObject;
                    for (shapeIndex = mShapeCollection.Count - 1; shapeIndex > -1; shapeIndex--)
                    {
                        mShape = mShapeCollection[shapeIndex];
                        mAutoShape = mShape as AutoShape;
                        switch (mShape.Name)
                        {
                            case "workLeader":
                                SetSingleText(mAutoShape, mJObject.Property(mShape.Name));
                                break;
                            case "mettingPerson":
                                JObject mChildJObject = (mJObject.Property(mShape.Name) as JProperty).Value as JObject;
                                int index = 1;
                                mAutoShape.TextFrame.Paragraphs.Clear();
                                if (template_titleTextFrame != null && template_contentTextFrame != null)
                                {
                                    foreach (JProperty mJProperty in mChildJObject.Properties())
                                    {
                                        mAutoShape.TextFrame.Paragraphs.Add(SetParagraph(template_titleTextFrame.Paragraphs[0], index++.ToString() + "、" + mJProperty.Name));
                                        mAutoShape.TextFrame.Paragraphs.Add(SetParagraph(template_contentTextFrame.Paragraphs[0], mJProperty.Value.ToString()));
                                        mAutoShape.TextFrame.Paragraphs.Add(SetParagraph(template_contentTextFrame.Paragraphs[0], " "));
                                    }
                                }
                                break;
                        }
                    }
                    #endregion
                    #endregion
                }
                else if (pageType.Equals(Common.PageType[Common.PageEnum.核对计划.ToString()]))
                {
                    #region 核对计划
                    JArray mJArray = mDataJObject.Property(Common.PageEnum.核对计划.ToString()).Value as JArray;
                    int mCount = mJArray.Count;
                    JObject mChildJObject;

                    for (; shapeIndex > -1; shapeIndex--)/*遍历幻灯片形状元素*/
                    {
                        mShape = mShapeCollection[shapeIndex];
                        switch (mShape.GetType().Name)
                        {
                            case "Table":
                                IRowCollection mRowCollection = (mShape as Table).Rows;
                                IRow mRow;
                                ICell mCell;
                                Boolean isSingleRow = mCount == 1 ? true : false;
                                for (int rowIndex = mRowCollection.Count - 1; rowIndex > -1; rowIndex--)
                                {
                                    /*若是单个计划，则移除第二行,拉高第一行*/
                                    mRow = mRowCollection[rowIndex];
                                    if (isSingleRow)
                                    {
                                        if (rowIndex == 2)
                                        {
                                            mRowCollection.RemoveAt(rowIndex, false);
                                            continue;
                                        }
                                        else if (rowIndex == 1)
                                        {
                                            mRow.MinimalHeight = mRow.MinimalHeight * 2 - 5;
                                        }
                                    }
                                    if (rowIndex == 0)
                                    {
                                        continue;
                                    }
                                    /*通过下标赋值*/
                                    mChildJObject = mJArray[rowIndex - 1] as JObject;
                                    for (int colIndex = mRow.Count - 1; colIndex > -1; colIndex--)
                                    {
                                        mCell = mRow[colIndex];
                                        int mListIndex = 0;
                                        foreach (JProperty mJProperty in mChildJObject.Properties())
                                        {
                                            if (mListIndex == colIndex)
                                            {
                                                mCell.TextFrame.Text = mJProperty.Value.ToString();
                                                break;
                                            }
                                            mListIndex++;
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    #endregion
                }
                else if (pageType.Equals(Common.PageType[Common.PageEnum.施工方案.ToString()]))
                {
                    #region 施工方案
                    JObject mJObject = mDataJObject.Property(Common.PageEnum.施工方案.ToString()).Value as JObject,
                            mChildJObject;
                    JArray mChildJArray;
                    IRowCollection mRowCollection;
                    IRow mRow;
                    int mRowCount = 0,
                        mDataRowCount = 0,
                        mRowIndex = 0;
                    #region 初始化表格，生成必要的行列，如：设备数据行，汽车安排数据行
                    for (; shapeIndex > -1; shapeIndex--)/*遍历幻灯片形状元素*/
                    {
                        mShape = mShapeCollection[shapeIndex];
                        switch (mShape.Name)
                        {
                            case "tableData1":
                                mRowCollection = (mShape as Table).Rows;
                                mRowCount = mRowCollection.Count;
                                mDataRowCount = (mJObject["equipment"] as JArray).Count;
                                for (int rowindex = 0, rowspan = mRowCollection[3][0].RowSpan; rowindex < rowspan - 1; rowindex++)
                                {
                                    mRowCollection[3 + rowindex][0].SplitByRowSpan(1);
                                }
                                InsertCloneRow(ref mRowCollection, mRowCollection[mRowCount - 2], mRowCount - 2, mDataRowCount - mRowCount + 5);
                                (mShape as Table).MergeCells(mRowCollection[3][0], mRowCollection[3 + mDataRowCount][0], true);
                                break;
                            case "CarArrangeTable":
                                mRowCollection = (mShape as Table).Rows;
                                mRowCount = mRowCollection.Count;
                                mDataRowCount = (mJObject["cararrange"] as JArray).Count;
                                InsertCloneRow(ref mRowCollection, mRowCollection[mRowCount - 1], mRowCount - 1, mDataRowCount - mRowCount + 1);
                                break;
                        }
                    }
                    #endregion
                    #region 赋值表格
                    shapeIndex = mShapeCollection.Count - 1;
                    for (; shapeIndex > -1; shapeIndex--)/*遍历幻灯片形状元素*/
                    {
                        mShape = mShapeCollection[shapeIndex];
                        switch (mShape.Name)
                        {
                            case "tableData1":/*第一个表格*/
                                mRowCollection = (mShape as Table).Rows;
                                mRowCount = mRowCollection.Count;
                                mRowIndex = 0;
                                mChildJArray = mJObject["equipment"] as JArray;
                                for (int rowIndex = 0; rowIndex < mRowCount; rowIndex++)
                                {
                                    mRow = mRowCollection[rowIndex];
                                    switch (rowIndex)
                                    {
                                        case 0:/*第一行*/
                                            mChildJObject = mJObject["base"] as JObject;
                                            mRow[2].TextFrame.Text = mChildJObject["workLeader"].ToString();
                                            mRow[4].TextFrame.Text = mChildJObject["personNum"].ToString();
                                            break;
                                        case 1:/*第二行*/
                                            mChildJObject = mJObject["base"] as JObject;
                                            mRow[2].TextFrame.Text = mChildJObject["workPlace"].ToString();
                                            mRow[4].TextFrame.Text = mChildJObject["SkylightDate"].ToString();
                                            break;
                                        case 2: /*第三行*/
                                            mChildJObject = mJObject["base"] as JObject;
                                            mRow[2].TextFrame.Text = mChildJObject["Content"].ToString();
                                            break;
                                        case 3:/*标题行*/
                                            break;
                                        default:
                                            if (rowIndex == mRowCount - 1)/*最后一行*/
                                            {
                                                mChildJObject = mJObject["base"] as JObject;
                                                mRow[2].TextFrame.Text = mChildJObject["timeArrange"].ToString();
                                            }
                                            else/*中间显示设备信息*/
                                            {
                                                mRow[2].TextFrame.Text = mChildJArray[mRowIndex]["category"].ToString();
                                                mRow[3].TextFrame.Text = mChildJArray[mRowIndex]["equipmentName"].ToString();
                                                mRow[5].TextFrame.Text = mChildJArray[mRowIndex]["count"].ToString();
                                                mRow[1].TextFrame.Text = (++mRowIndex).ToString();
                                            }
                                            break;
                                    }
                                }
                                break;
                            case "CarArrangeTable":/*车辆安排表格*/
                                mRowCollection = (mShape as Table).Rows;
                                mRowCount = mRowCollection.Count;
                                mRowIndex = 0;
                                mChildJArray = mJObject["cararrange"] as JArray;
                                for (int rowIndex = mRowCount - 1; rowIndex > -1; rowIndex--)
                                {
                                    mRow = mRowCollection[rowIndex];
                                    if (rowIndex != 0)
                                    {
                                        mRow[0].TextFrame.Text = mChildJArray[mRowIndex]["car"].ToString();
                                        mRow[1].TextFrame.Text = mChildJArray[mRowIndex]["driver"].ToString();
                                        mRow[2].TextFrame.Text = mChildJArray[mRowIndex]["takepersons"].ToString();
                                        mRow[3].TextFrame.Text = mChildJArray[mRowIndex]["route"].ToString();
                                        mRow[4].TextFrame.Text = mChildJArray[mRowIndex]["safetysupervisor"].ToString();
                                        mRow[5].TextFrame.Text = mChildJArray[mRowIndex]["remark"].ToString();
                                        mRowIndex++;
                                    }
                                }
                                break;
                        }
                    }
                    #endregion
                    #endregion
                }
                else if (pageType.Equals(Common.PageType[Common.PageEnum.作业区防护措施.ToString()]))
                {
                    #region 作业区防护措施
                    JObject mJObject = mDataJObject.Property(Common.PageEnum.作业区防护措施.ToString()).Value as JObject;
                    #region 查找图片参数
                    float top = 0, left = 0,
                        width = mSlide.Presentation.SlideSize.Size.Width, height = mSlide.Presentation.SlideSize.Size.Height;
                    for (; shapeIndex > -1; shapeIndex--)
                    {
                        switch (mShapeCollection[shapeIndex].Name)
                        {
                            case "leftDiv":
                                mAutoShape = mShapeCollection[shapeIndex] as AutoShape;
                                left = mAutoShape.Width + 10;
                                top = 0;
                                break;
                            case "map_img":
                                IPictureFrame mPictureFrame = mShapeCollection[shapeIndex] as PictureFrame;
                                mShapeCollection.Remove(mPictureFrame);
                                break;
                        }
                    }
                    #endregion
                    width -= left;
                    #region 设置图片
                    if (mJObject["path"].ToString().Length > 0)
                    {
                        float tempWidth = Convert.ToSingle(mJObject["width"].ToString()),
                              tempHeight = Convert.ToSingle(mJObject["height"].ToString()),
                              tempPercent = Calculate(tempWidth, tempHeight, width, height);
                        InsertImg(mSlide, top, left, tempWidth * tempPercent, tempHeight * tempPercent, mJObject["path"].ToString());
                    }
                    #endregion
                    #endregion
                }
                else if (pageType.Equals(Common.PageType[Common.PageEnum.业务学习.ToString()]))
                {
                    #region 业务学习
                    JArray mJArray = mDataJObject.Property(Common.PageEnum.业务学习.ToString()).Value as JArray;
                    foreach (JObject mJObject in mJArray)
                    {
                        shapeIndex = InitImg_TxtPage(mSlide, mShapeCollection, shapeIndex, mJObject);
                        mJArray.Remove(mJObject);
                        break;
                    }
                    #endregion
                }
                else if (pageType.Equals(Common.PageType[Common.PageEnum.危险源.ToString()]))
                {
                    #region 危险源
                    JArray mJArray = mDataJObject.Property(Common.PageEnum.危险源.ToString()).Value as JArray;
                    foreach (JObject mJObject in mJArray)
                    {
                        shapeIndex = InitImg_TxtPage(mSlide, mShapeCollection, shapeIndex, mJObject);
                        mJArray.Remove(mJObject);
                        break;
                    }
                    #endregion
                }
                else if (pageType.Equals(Common.PageType[Common.PageEnum.人身安全方案.ToString()]))
                {
                    #region 人身安全方案
                    ITextFrame template_titleTextFrame = null, template_contentTextFrame = null;
                    #region 模板样式
                    for (; shapeIndex > -1; shapeIndex--)
                    {
                        mShape = mShapeCollection[shapeIndex];
                        if (mShape.GetType() == typeof(AutoShape))
                        {
                            mAutoShape = mShape as AutoShape;
                            if (mShape.Name.Contains("content_title"))
                            {
                                template_titleTextFrame = mAutoShape.TextFrame;
                                mShapeCollection.Remove(mShape);
                            }
                            else if (mShape.Name.Contains("content"))
                            {
                                template_contentTextFrame = mAutoShape.TextFrame;
                                mShapeCollection.Remove(mShape);
                            }
                        }
                    }
                    #endregion
                    #region 填充数据
                    JObject mJObject = mDataJObject.Property(Common.PageEnum.人身安全方案.ToString()).Value as JObject;
                    for (shapeIndex = mShapeCollection.Count - 1; shapeIndex > -1; shapeIndex--)
                    {
                        mShape = mShapeCollection[shapeIndex];
                        switch (mShape.Name)
                        {
                            case "safatestep":
                                JObject mChildJObject = (mJObject.Property(mShape.Name) as JProperty).Value as JObject;
                                int index = 1;
                                mAutoShape = mShape as AutoShape;
                                mAutoShape.TextFrame.Paragraphs.Clear();
                                if (template_titleTextFrame != null && template_contentTextFrame != null)
                                {
                                    string[] valueStr;
                                    foreach (JProperty mJProperty in mChildJObject.Properties())
                                    {
                                        mAutoShape.TextFrame.Paragraphs.Add(SetParagraph(template_titleTextFrame.Paragraphs[0], mJProperty.Name));
                                        valueStr = mJProperty.Value.ToString().Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (string str in valueStr)
                                        {
                                            mAutoShape.TextFrame.Paragraphs.Add(SetParagraph(template_contentTextFrame.Paragraphs[0], str));
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    #endregion
                    #endregion
                }
                else if (pageType.Equals(Common.PageType[Common.PageEnum.施工行车方案.ToString()]))
                {
                    #region 施工行车方案
                    ITextFrame template_titleTextFrame = null, template_contentTextFrame = null;
                    #region 模板样式
                    for (; shapeIndex > -1; shapeIndex--)
                    {
                        mShape = mShapeCollection[shapeIndex];
                        if (mShape.GetType() == typeof(AutoShape))
                        {
                            mAutoShape = mShape as AutoShape;
                            if (mShape.Name.Contains("content_title"))
                            {
                                template_titleTextFrame = mAutoShape.TextFrame;
                                mShapeCollection.Remove(mShape);
                            }
                            else if (mShape.Name.Contains("content"))
                            {
                                template_contentTextFrame = mAutoShape.TextFrame;
                                mShapeCollection.Remove(mShape);
                            }
                        }
                    }
                    #endregion
                    #region 填充数据
                    JObject mJObject = mDataJObject.Property(Common.PageEnum.施工行车方案.ToString()).Value as JObject;
                    for (shapeIndex = mShapeCollection.Count - 1; shapeIndex > -1; shapeIndex--)
                    {
                        mShape = mShapeCollection[shapeIndex];
                        switch (mShape.Name)
                        {
                            case "safatestep":
                                JObject mChildJObject = (mJObject.Property(mShape.Name) as JProperty).Value as JObject;
                                int index = 1;
                                mAutoShape = mShape as AutoShape;
                                mAutoShape.TextFrame.Paragraphs.Clear();
                                if (template_titleTextFrame != null && template_contentTextFrame != null)
                                {
                                    string[] valueStr;
                                    foreach (JProperty mJProperty in mChildJObject.Properties())
                                    {
                                        mAutoShape.TextFrame.Paragraphs.Add(SetParagraph(template_titleTextFrame.Paragraphs[0], mJProperty.Name));
                                        valueStr = mJProperty.Value.ToString().Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (string str in valueStr)
                                        {
                                            mAutoShape.TextFrame.Paragraphs.Add(SetParagraph(template_contentTextFrame.Paragraphs[0], str));
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    #endregion
                    #endregion
                }
                else if (pageType.Equals(Common.PageType[Common.PageEnum.安全风险.ToString()]))
                {
                    #region 安全风险
                    string[] data_id = mSlide.Name.Split('_');
                    string mSafety = mDataJObject.Property(Common.PageEnum.安全风险.ToString()).Value.ToString();
                    MatchCollection mSafetyStr = Regex.Matches("\n" + mSafety, @"\n\d{1,2}.*");
                    string safetyStr = string.Empty;
                    string title_pre = "安全措施",
                        title_idpre = "pageTitle",
                        content_idpre = "pageTxt";
                    int index = 0;
                    for (; shapeIndex > -1; shapeIndex--)
                    {
                        mShape = mShapeCollection[shapeIndex];
                        if (mShape.Name.Contains(title_idpre))
                        {
                            index = (Convert.ToInt32(data_id[2]) - 1) * 3 + Convert.ToInt32(mShape.Name.Replace(title_idpre, string.Empty));
                            (mShape as AutoShape).TextFrame.Text = title_pre + index.ToString();
                        }
                        else if (mShape.Name.Contains(content_idpre))
                        {
                            index = (Convert.ToInt32(data_id[2]) - 1) * 3 + Convert.ToInt32(mShape.Name.Replace(content_idpre, string.Empty));
                            if (mSafetyStr.Count >= index)
                            {
                                safetyStr = mSafetyStr[index - 1].ToString();
                                (mShape as AutoShape).TextFrame.Text = safetyStr.Replace(safetyStr.IndexOf("\n") == 0 ? "\n" : string.Empty, string.Empty);
                            }
                            else
                            {
                                (mShape as AutoShape).TextFrame.Text = string.Empty;
                            }
                        }
                    }
                    #endregion
                }
                else if (pageType.Equals(Common.PageType[Common.PageEnum.风险源.ToString()]))
                {
                    #region 风险源
                    string[] data_id = mSlide.Name.Split('_');
                    string mSafety = mDataJObject.Property(Common.PageEnum.风险源.ToString()).Value.ToString();
                    MatchCollection mSafetyStr = Regex.Matches("\n" + mSafety, @"\n\d{1,2}.*");
                    string safetyStr = string.Empty;
                    string title_pre = "风险源",
                        title_idpre = "pageTitle",
                        content_idpre = "riskName";
                    int index = 0;
                    for (; shapeIndex > -1; shapeIndex--)
                    {
                        mShape = mShapeCollection[shapeIndex];
                        if (mShape.Name.Contains(title_idpre))
                        {
                            index = (Convert.ToInt32(data_id[2]) - 1) * 3 + Convert.ToInt32(mShape.Name.Replace(title_idpre, string.Empty));
                            (mShape as AutoShape).TextFrame.Text = title_pre + index.ToString();
                        }
                        else if (mShape.Name.Contains(content_idpre))
                        {
                            index = (Convert.ToInt32(data_id[2]) - 1) * 3 + Convert.ToInt32(mShape.Name.Replace(content_idpre, string.Empty));
                            if (mSafetyStr.Count >= index)
                            {
                                safetyStr = mSafetyStr[index - 1].ToString();
                                (mShape as AutoShape).TextFrame.Text = safetyStr.Replace(safetyStr.IndexOf("\n") == 0 ? "\n" : string.Empty, string.Empty);
                            }
                            else
                            {
                                (mShape as AutoShape).TextFrame.Text = string.Empty;
                            }
                        }
                    }
                    #endregion
                }
                else if (pageType.Equals(Common.PageType[Common.PageEnum.结束.ToString()]))
                { }
                #endregion
                #region 工作组
                else
                {
                    IShape mGroupNameShape = null;
                    IRowCollection mRowCollection = null;
                    int mRowCount = 0, mRowIndex = 0;
                    #region 查找页面表格
                    for (; shapeIndex > -1; shapeIndex--)
                    {
                        if (mShapeCollection[shapeIndex].GetType() == typeof(Table))
                        {
                            mRowCollection = (mShapeCollection[shapeIndex] as Table).Rows;
                            mRowCount = mRowCollection.Count;
                        }
                        else if (mShapeCollection[shapeIndex].Name.Equals("GroupName"))
                        {
                            mGroupNameShape = mShapeCollection[shapeIndex];
                        }
                    }
                    #endregion
                    #region 赋值
                    if (mGroupNameShape != null && mRowCollection != null)
                    {
                        JArray mJArray = mDataJObject.Property(Common.PageEnum.工作组.ToString()).Value as JArray;
                        JObject mChildJObject;
                        foreach (JObject mJObject in mJArray)
                        {
                            /*过滤出对应小组数据*/
                            if (!((Regex.IsMatch(pageType, string.Format("^({0})|({1})|({2})|({3})",
                                Common.GroupPageType[Common.PageEnum.车梯.ToString()],
                                Common.GroupPageType[Common.PageEnum.待定.ToString()],
                                Common.GroupPageType[Common.PageEnum.人工.ToString()],
                                Common.GroupPageType[Common.PageEnum.作业车.ToString()]), RegexOptions.None)
                                &
                                Regex.IsMatch(mJObject["GroupName"].ToString(), string.Format("^({0})|({1})|({2})|({3})",
                                Common.PageEnum.车梯.ToString(),
                                Common.PageEnum.待定.ToString(),
                                Common.PageEnum.人工.ToString(),
                                Common.PageEnum.作业车.ToString()), RegexOptions.None))
                                ||
                                (Regex.IsMatch(pageType, string.Format("^({0})", (Common.GroupPageType[Common.PageEnum.测量.ToString()])), RegexOptions.None)
                                &
                                 Regex.IsMatch(mJObject["GroupName"].ToString(), Common.PageEnum.测量.ToString(), RegexOptions.None))
                                 ||
                                (Regex.IsMatch(pageType, string.Format("^({0})", (Common.GroupPageType[Common.PageEnum.地线.ToString()])), RegexOptions.None)
                                &
                                 Regex.IsMatch(mJObject["GroupName"].ToString(), Common.PageEnum.地线.ToString(), RegexOptions.None))
                                 ||
                                (Regex.IsMatch(pageType, string.Format("^({0})", (Common.GroupPageType[Common.PageEnum.巡视.ToString()])), RegexOptions.None)
                                &
                                 Regex.IsMatch(mJObject["GroupName"].ToString(), Common.PageEnum.巡视.ToString(), RegexOptions.None))
                                ||
                                (Regex.IsMatch(pageType, string.Format("^({0})", (Common.GroupPageType[Common.PageEnum.驻站联络.ToString()])), RegexOptions.None)
                                &
                                 Regex.IsMatch(mJObject["GroupName"].ToString(), Common.PageEnum.驻站联络.ToString(), RegexOptions.None))
                                ))
                            {
                                continue;
                            }
                            /*赋值*/
                            mRowIndex = 0;
                            (mGroupNameShape as AutoShape).TextFrame.Text = mJObject["GroupName"].ToString();
                            mRowCollection[mRowIndex++][1].TextFrame.Text = mJObject["workPlace"].ToString();
                            mRowCollection[mRowIndex][1].TextFrame.Text = mJObject["workArea"].ToString();
                            SetCellSpecial(mRowCollection, mRowIndex, 2, mJObject["ticketNum"].ToString());
                            SetCellSpecial(mRowCollection, mRowIndex++, 3, mJObject["Licence"].ToString());
                            mChildJObject = mJObject["person"] as JObject;
                            foreach (JProperty tempJProperty in mChildJObject.Properties())
                            {
                                mRowCollection[mRowIndex++][1].TextFrame.Text = tempJProperty.Value.ToString();
                            }
                            mRowCollection[mRowCount - 4][1].TextFrame.Text = mJObject["WorkContent"].ToString();
                            mRowCollection[mRowCount - 3][1].TextFrame.Text = mJObject["Category"].ToString();
                            mRowCollection[mRowCount - 2][1].TextFrame.Text = mJObject["alterCategory"].ToString();
                            mRowCollection[mRowCount - 1][1].TextFrame.Text = mJObject["Attention"].ToString();

                            mJArray.Remove(mJObject);
                            break;
                        }
                    }
                    #endregion
                }
                #endregion
            }
        }

        /*初始化图文页面*/
        private int InitImg_TxtPage(ISlide mSlide, IShapeCollection mShapeCollection, int shapeIndex, JObject mJObject)
        {
            #region 查找图片参数
            float top = 0, left = 0,
                width = mSlide.Presentation.SlideSize.Size.Width, height = mSlide.Presentation.SlideSize.Size.Height;
            string txt = mJObject["txt"].ToString(),
                path = mJObject["path"].ToString();
            AutoShape txtAutoShape = null;
            IPictureFrame mImgFrame = null;
            for (; shapeIndex > -1; shapeIndex--)
            {
                switch (mShapeCollection[shapeIndex].Name)
                {
                    case "txtContent":
                        txtAutoShape = mShapeCollection[shapeIndex] as AutoShape;
                        break;
                    case "imgContent":
                        IPictureFrame mPictureFrame = mShapeCollection[shapeIndex] as PictureFrame;
                        mShapeCollection.Remove(mPictureFrame);
                        break;
                    case "leftDiv":
                        top = 0;
                        left = (mShapeCollection[shapeIndex] as AutoShape).Width + 8;
                        break;
                }
            }
            #endregion
            width -= left;
            #region 设置图片
            if (path.Length > 0)
            {
                float tempWidth = Convert.ToSingle(mJObject["width"].ToString()),
                      tempHeight = Convert.ToSingle(mJObject["height"].ToString()),
                      tempPercent = Calculate(tempWidth, tempHeight, width, (txt.Length == 0 ? height : height * 0.7f));
                mImgFrame = InsertImg(mSlide, top, left, tempWidth * tempPercent, tempHeight * tempPercent, path);
            }
            if (txt.Length == 0)
            {
                mShapeCollection.Remove(txtAutoShape);
            }
            else
            {
                if (txt.Length == 0)
                {
                    txtAutoShape.Y = 10;
                    txtAutoShape.Height = height;
                }
                else
                {
                    txtAutoShape.Y = mImgFrame.Y + mImgFrame.Height + 5;
                    txtAutoShape.Height = height - txtAutoShape.Y;
                    txtAutoShape.TextFrame.Text = txt;
                }
            }
            #endregion
            return shapeIndex;
        }

        /*插入克隆行（mInsertCount<0时，执行移除操作）*/
        private void InsertCloneRow(ref IRowCollection mRowCollection, IRow mTemplateRow, int insertIndex, int mInsertCount)
        {
            while (mInsertCount > 0)
            {
                mRowCollection.InsertClone(insertIndex++, mTemplateRow, true);
                mInsertCount--;
            }
            while (mInsertCount < 0)
            {
                mRowCollection.RemoveAt(insertIndex--, false);
                mInsertCount++;
            }
        }

        /*插入图片*/
        private IPictureFrame InsertImg(ISlide mSlide, float top, float left, float x, float y, string path)
        {
            IPPImage mImge = mSlide.Presentation.Images.AddImage(System.Drawing.Image.FromFile(path));
            return mSlide.Shapes.AddPictureFrame(ShapeType.Rectangle, left, top, x, y, mImge);
        }

        /*计算图片最好放大倍数*/
        private float Calculate(float imgWidth, float imgHeight, float containWidth, float containHeight)
        {
            float tempPercent = containWidth / imgWidth > containHeight / imgHeight ? containHeight / imgHeight : containWidth / imgWidth;
            if (tempPercent > 2)/*放大两倍后会出现格子*/
            {
                tempPercent = 2;
            }
            return tempPercent;
        }

        /*返回指定名称的幻灯片*/
        public ISlide GetSlide(string slideName)
        {
            if (this.mPresentation == null || this.mPresentation.Slides.Count == 0)
            {
                return null;
            }
            foreach (ISlide mSlide in this.mPresentation.Slides)
            {
                if (Regex.Replace(mSlide.Name, "[1,9]+", "", RegexOptions.IgnoreCase) == slideName)
                {
                    return mSlide;
                }
            }
            return null;
        }

        /*将属性对应的值写入到形状的文本里面*/
        private Boolean SetSingleText(Object mShape, JProperty mJProperty)
        {
            if (mJProperty != null)
            {
                Type mType = mShape.GetType();
                if (mType == typeof(AutoShape))
                {
                    ((AutoShape)mShape).TextFrame.Text = mJProperty.Value.ToString();
                }
                else if (mType == typeof(TextFrame))
                {
                    ((TextFrame)mShape).Text = mJProperty.Value.ToString();
                }
                return true;
            }
            return false;
        }

        /*添加段落(样式与模版样式相同)*/
        private Paragraph SetParagraph(IParagraph templateParagraph, string value)
        {
            Paragraph mParagraph = new Paragraph();
            mParagraph.ParagraphFormat.Alignment = templateParagraph.ParagraphFormat.Alignment;

            IPortion mPort = new Portion(), tmpPort = templateParagraph.Portions[0];
            mPort.PortionFormat.FontHeight = tmpPort.PortionFormat.FontHeight;
            mPort.PortionFormat.EastAsianFont = tmpPort.PortionFormat.EastAsianFont;
            mPort.PortionFormat.FontBold = tmpPort.PortionFormat.FontBold;
            mPort.PortionFormat.FillFormat.FillType = tmpPort.PortionFormat.FillFormat.FillType;
            mPort.PortionFormat.FillFormat.SolidFillColor.Color = tmpPort.PortionFormat.FillFormat.SolidFillColor.Color;
            mPort.Text = value;
            mParagraph.Portions.Add(mPort);

            return mParagraph;
        }

        /*替换单元格中冒号后面的内容*/
        private void SetCellSpecial(IRowCollection mRowCollection, int rowIndex, int colIndex, string text)
        {
            ICell mCell = mRowCollection[rowIndex][colIndex];
            mCell.TextFrame.Text = mCell.TextFrame.Text.Substring(0, mCell.TextFrame.Text.IndexOf("：") + 1) + text;
        }
        #endregion

        /// <summary>
        /// 保存PPT文件
        /// </summary>
        /// <param name="fileName"></param>
        public void SavePPT(string fileName)
        {
            mPresentation.Save(AppDomain.CurrentDomain.BaseDirectory + fileName, Aspose.Slides.Export.SaveFormat.Pptx);
        }
    }
}
