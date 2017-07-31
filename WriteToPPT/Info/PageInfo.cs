using System;
using System.Collections.Generic;

namespace WriteToPPT.Info
{
    public partial class Common
    {
        public enum PageEnum
        {
            作业方案,
            点名及检查着装,
            核对计划,
            施工方案,
            作业区防护措施,
            工作组,
            业务学习,
            人身安全方案,
            施工行车方案,
            安全风险,
            风险源,
            危险源,
            结束,
            车梯,
            人工,
            作业车,
            测量,
            地线,
            待定,
            驻站联络,
            巡视
        }

        public static Dictionary<string, string> PageType = new Dictionary<string, string>
        {  
            {"作业方案","TitlePage"},
            {"点名及检查着装","SecondPage"},
            {"核对计划","PlanVerifyPage"},
            {"施工方案","OverallArrangPage"},
            {"作业区防护措施","ProtectMapPage"},
            {"工作组",""},
            {"业务学习","WorkArea-TxtImgPage"},
            {"人身安全方案","OrderSafetyStepPage"},            
            {"施工行车方案","SafeDrivePage"},
            {"安全风险","SafetyStepPage"},
            {"风险源","RiskSourcePage"},
            {"危险源","DangerPage"},
            {"结束","ThanksPage"}  
        };

        public static Dictionary<string, string> GroupPageType = new Dictionary<string, string>
        {              
            {"车梯","TCZPage"},
            {"人工","TCZPage"},
            {"作业车","TCZPage"},
            {"测量","CLZPage"},
            {"地线","DXZPage"},
            {"待定","TCZPage"},
            {"驻站联络","StationPage"},
            {"巡视","XSZPage"}
        };
    }
}
