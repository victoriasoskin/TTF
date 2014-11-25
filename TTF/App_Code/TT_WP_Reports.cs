using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TTF.App_Code
{
    public class TT_WP_Reports
    {
        public int FormId { get; set; }
        public int EventId { get; set; }
        public Int64 CustomerId { get; set; }
        public int WPId { get; set; }
        public int DetailsId { get; set; }
        public int val { get; set; }
        public string text { get; set; }
        public int Status { get; set; }
        public int Frm_CatId { get; set; }
        public int PeriodKey { get { return System.DateTime.Today.Year * 100 + System.DateTime.Today.Month; } }
        public DateTime LoadTime { get; set; }
        public int UserId { get; set; }

        Util u = new Util();

        public TT_WP_Reports()
        { LoadTime = System.DateTime.Now; }

        public DataTable getReports(int Id)
        {
            DataTable dt = u.selectDBQuesry(string.Format("SELECT Id ,FormId ,EventId ,CustomerId ,WpId ,val ,Text ,Status ,Frm_CatId ,PeriodKey ,LoadTime ,r.UserId,u.URName " +
                                                        "FROM Book10.dbo.TT_WP_Reports r " +
                                                        "LEFT OUTER JOIN Book10_21.dbo.p0t_NtB u ON u.UserId=r.UserId " +
                                                        "WHERE WpId = {0} " +
                                                        "ORDER BY LoadTime DESC", Id));
            return dt;
        }


        internal int UpdateRecord(string sql)
        {
            //sql += "COMMIT END TRY BEGIN CATCH ROLLBACK END CATCH";
            //string sql = "INSERT INTO Book10.dbo.TT_WP_Reports(FormId,EventId,CustomerId,WpId,val,Text,Status,Frm_CatId,PeriodKey,LoadTime,UserId) VALUES(";
            //sql += FormId.ToString() + ",";
            //sql += EventId.ToString() + ",";
            //sql += CustomerId.ToString() + ",";
            //sql += WPId.ToString() + ",";
            //sql += val.ToString() + ",";
            //sql += (text != null && text != string.Empty ? "'" + text.Replace("'", "''") + "'," : "NULL,");
            //sql += Status.ToString() + ",";
            //sql += Frm_CatId.ToString() + ",";
            //sql += PeriodKey.ToString() + ",";
            //sql += "'" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", LoadTime) + "',";
            //sql += UserId.ToString() + ")";

            Exception ex = u.executeSql(sql);
            
            return 0;
        }

        internal string buildHeader()
        {
            string sql = string.Empty; //"BEGIN TRANSACTION BEGIN TRY ";
            return sql;
        }
        internal string buildReport(int questionId, int val, string text)
        {
            string s = (text != string.Empty && text == "פרט במידת הצורך" ? "NULL" : "'" + text.Replace("'", "''") + "'");
            string sql = string.Format("INSERT INTO Book10.dbo.TT_WP_Reports(FormId,EventId,CustomerId,WpId,DetailsId,QUestionId,val,Text,Status,Frm_CatId,PeriodKey,LoadTime,UserId) VALUES(" +
                 "{0},{1},{2},{3},{4},{5},{6},{7},0,{8},{9},'{10}',{11}) ", FormId, EventId, CustomerId, WPId, DetailsId, questionId, val, s, Frm_CatId, PeriodKey, string.Format("{0:yyyy-MM-dd HH:mm:ss}", LoadTime), UserId);
            return sql;
        }



        internal DataTable getMainReports(int WpId)
        {
            return u.selectDBQuesry(string.Format("SELECT Id, WpId ,DetailsId, val1, Q1, val2 ,Q2,val3 ,Q3 ,LoadTime ,URName " +
                                    "FROM Book10.dbo.TT_VMainReport " +
                                    "WHERE WpId={0} " +
                                    "ORDER BY LoadTime DESC", WpId));
        }

        internal DataTable getDetReports(int detId)
        {
            return u.selectDBQuesry(string.Format("SELECT Id ,FormId ,EventId ,CustomerId ,WpId ,DetailsId ,val4 ,Q4 ,Text ,UserId ,URName ,LoadTime " +
                                                    "FROM Book10.dbo.TT_vDetReports " +
                                                    "WHERE  DetailsId = {0} " +
                                                    "ORDER BY LoadTime DESC",detId));
        }
        

        internal void deleteReport(int Id)
        {
            DataTable dt = u.selectDBQuesry(string.Format("SELECT * FROM TT_WP_Reports WHERE Id={0}", Id));
            DataRow r = dt.Rows[0];
            int usr = (int)r["userid"];
            DateTime d = (DateTime)r["LoadTime"];
            int w = (int)r["WpId"];
            string s = string.Format("UPDATE TT_WP_Reports SET Status = 1 WHERE WpId = {0} AND DetailsId = 0 AND UserId = {1} AND LoadTime = '{2}'", w, usr, string.Format("{0:yyyy-MM-dd HH:mm:ss}", d));
            Exception ex = u.executeSql(s);

        }

        internal void InsertDetailsIntoTT_WP_REPORTS(int WpId, System.Web.UI.WebControls.ListViewItem lvi, int res, string text)
        {
            string sql = string.Format("INSERT INTO Book10.dbo.TT_WP_Reports(FormId,EventId,CustomerId,WpId,val,Text,Status,Frm_CatId,PeriodKey,LoadTime,UserId) VALUES(" +
                                     "'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}') ", FormId, EventId.ToString(), CustomerId, WPId, res,text.Replace("'","''"), 0, Frm_CatId, PeriodKey, string.Format("{0:yyyy-MM-dd HH:mm:ss}", LoadTime), UserId);
            Exception ex = u.executeSql(sql);
           
           }
    }
}