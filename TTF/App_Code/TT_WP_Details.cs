using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
//using TTF.App_Code;

namespace TTF.App_Code
{
    public class TT_WP_Details
    {
        public int id { get; set; }
        public int EventId { get; set; }
        public int FormId { get; set; }
        public int WPId { get; set; }
        public Int64 CustomerId { get; set; }
        public string text { get; set; }
        public string text1 { get; set; }
        public string text2 { get; set; }
        public int PeriodId { get; set; }
        public string PeriodText { get; set; }
        public string Period { get; set; }
        public int AmountId { get; set; }
        public string Amount { get; set; }
        public int FrequencyId { get; set; }
        public string Frequency { get; set; }
        public int LengthId { get; set; }
        public string length { get; set; }
        public int HelperId { get; set; }
        public string Helper { get; set; }
        public string HelperText { get; set; }
        public string SupporterName { get; set; }
        public int Ord { get; set; }
        public int Status { get; set; }
        public int Frm_CatId { get; set; }
        public int PeriodKey { get { return System.DateTime.Today.Year * 100 + System.DateTime.Today.Month; } }
        public DateTime LoadTime { get; set; }
        public int UserId { get; set; }

        TTF.App_Code.Util u;

        public TT_WP_Details()
        {
            u = new Util();
            LoadTime = System.DateTime.Now;
        }

        public DataTable getDetails(int WpId, int EventId)
        {
            DataTable dt = u.selectDBQuesry(string.Format("SELECT * " +
            "FROM [dbo].[TT_EduDetails]({0},{1}) where status = 0", WpId, EventId));
            return dt;
        }

        internal int deleteRecord(int Id)
        {
            string sql = string.Format("UPDATE TT_WP_Details SET Status = 1,LoadTime='{1}',UserId = {2} WHERE Id={0}", Id, string.Format("{0:yyyy-MM-dd HH:mm:ss}", LoadTime), UserId);
            Exception ex = u.executeSql(sql);
            if (ex != null) { HttpContext.Current.Response.Write(ex.Message); HttpContext.Current.Response.End(); }
            return 0;
        }

        public void UpdateRecord(int Id)
        {
            string sql = string.Empty;
            if (Id == 0)
            {
                sql += "INSERT INTO Book10.dbo.TT_WP_Details(DetailsId,EventId,FormId,WPId,CustomerId,Text,Text1,Text2,PeriodId,PeriodText,AmountId,FrequencyID,LengthId,HelperId,HelperText,SupporterName,Ord,Status,Frm_CatID,PeriodKey,LoadTime,UserId) Values(";
                sql += "NULL,";
                sql += EventId.ToString() + ",";
                sql += FormId.ToString() + ",";
                sql += WPId.ToString() + ",";
                sql += CustomerId.ToString() + ",";
                sql += (text != null && text != string.Empty ? "'" + text.Replace("'", "''") + "'," : "NULL,");
                sql += (text1 != null && text1 != string.Empty ? "'" + text1.Replace("'", "''") + "'," : "NULL,");
                sql += (text2 != null && text2 != string.Empty ? "'" + text2.Replace("'", "''") + "'," : "NULL,");
                sql += PeriodId.ToString() + ",";
                sql += (PeriodText != null && PeriodText != string.Empty ? "'" + PeriodText.Replace("'", "''") + "'," : "NULL,");
                sql += AmountId.ToString() + ",";
                sql += FrequencyId.ToString() + ",";
                sql += LengthId.ToString() + ",";
                sql += HelperId.ToString() + ",";
                sql += (HelperText != null && HelperText != string.Empty ? "'" + HelperText.Replace("'", "''") + "'," : "NULL,");
                sql += (SupporterName != null && SupporterName != string.Empty && SupporterName != "שם הספק" ? "'" + SupporterName.Replace("'", "''") + "'," : "NULL,");
                sql += "0,";
                sql += Status.ToString() + ",";
                sql += Frm_CatId.ToString() + ",";
                sql += PeriodKey.ToString() + ",";
                sql += "'" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", LoadTime) + "',";
                sql += UserId.ToString() + ")";
            }
            else
            {
                sql += "UPDATE Book10.dbo.TT_WP_Details SET ";
                sql += "Text=" + (text != null && text != string.Empty ? "'" + text.Replace("'", "''") + "'," : "NULL,");
                sql += "Text1=" + (text1 != null && text1 != string.Empty ? "'" + text1.Replace("'", "''") + "'," : "NULL,");
                sql += "Text2=" + (text2 != null && text2 != string.Empty ? "'" + text2.Replace("'", "''") + "'," : "NULL,");
                sql += "PeriodId=" + PeriodId.ToString() + ",";
                sql += "PeriodText=" + (PeriodText != null && PeriodText != string.Empty ? "'" + PeriodText.Replace("'", "''") + "'," : "NULL,");
                sql += "AmountId=" + AmountId.ToString() + ",";
                sql += "FrequencyId=" + FrequencyId.ToString() + ",";
                sql += "LengthId=" + LengthId.ToString() + ",";
                sql += "HelperId=" + HelperId.ToString() + ",";
                sql += "HelperText=" + (HelperText != null && HelperText != string.Empty ? "'" + HelperText.Replace("'", "''") + "'," : "NULL,");
                sql += "SupporterName=" + (SupporterName != null && SupporterName != string.Empty && SupporterName != "שם הספק" ? "'" + SupporterName.Replace("'", "''") + "'," : "NULL,");
                sql += "PeriodKey=" + PeriodKey.ToString() + ",";
                sql += "LoadTime='" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", LoadTime) + "',";
                sql += "UserId=" + UserId.ToString() + " ";
                sql += string.Format("WHERE Id={0} ", Id);
            }
            Exception ex = u.executeSql(sql);
        }



        internal void InsertDetailsInTT_WP_Details(int WpId, string txtbDetails, string period, string frequency_1, string frequency_2, string lasting, string helper, string helperName)
        {
            //PeriodId = int.Parse(u.selectDBScalar(string.Format("select id from [B10Sec].[dbo].[TT_Classes] where Name like '%{0}'", period)).ToString());
           // AmountId = int.Parse(u.selectDBScalar(string.Format("select id from [B10Sec].[dbo].[TT_Classes] where Name = '{0}'", frequency_1)).ToString());
            //FrequencyId = int.Parse(u.selectDBScalar(string.Format("select id from [B10Sec].[dbo].[TT_Classes] where Name = '{0}'", frequency_2)).ToString());
            //LengthId = int.Parse(u.selectDBScalar(string.Format("select id from [B10Sec].[dbo].[TT_Classes] where Name = '{0}'", lasting)).ToString());
            //HelperId = int.Parse(u.selectDBScalar(string.Format("select id from [B10Sec].[dbo].[TT_Classes] where Name = '{0}'", helper)).ToString());
            
            string sql = string.Format("insert into [Book10].[dbo].[TT_WP_Details] "
                + "( EventId"
                +",FormId"
                +", wpid"
                +", CustomerId"
                +", Text" 
                +",periodId,"
                + "AmountId"
                + ",frequencyID"
                + ",lengthId"
                + ",helperID"
                + ",Ord"
                + ",Status"
                + ",Frm_CatID"
                + ", PeriodKey"
                + ",LoadTime"
                + ", UserId"
                + ",SupporterName)"
                + " values  ( '{0}','{1}','{2}','{3}','{4}','{5}' ,'{6}','{7}','{8}','{9}','{10}','{11}','{12}', '{13}','{14}','{15}','{16}' ) \n",
                EventId.ToString(), FormId.ToString(), WpId.ToString(), CustomerId, txtbDetails.Replace("'","''"), period, frequency_1, frequency_2, lasting, helper.Replace("'","''"), 0, 0, Frm_CatId, PeriodKey.ToString(),string.Format("{0:yyyy-MM-dd HH:mm:ss}", LoadTime), UserId, helperName.Replace("'","''"));
                
           u.executeSql(sql);
        }
    }
}