using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TTF.App_Code
{
    public class TT_WP
    {
        public Int64 CustomerId { get; set; }
        public int EventId { get; set; }
        public int FormId { get; set; }
        public int RangeId { get; set; }
        public string Range { get; set; }
        public int SubjectId { get; set; }
        public string Subject { get; set; }
        public int PurposeId { get; set; }
        public string Purpose { get; set; }
        public int TargetId { get; set; }
        public string Target { get; set; }
        public int WeightId { get; set; }
        public string Weight { get; set; }
        public int Ord { get; set; }
        public int Status { get; set; }
        public int Frm_CatID { get; set; }
        public int Frame { get; set; }
        public int PeriodKey { get { return System.DateTime.Today.Year * 100 + System.DateTime.Today.Month; } }
        public DateTime LoadTime { get; set; }
        public int UserId { get; set; }

        Util u;
        TT_WP_Details det;

        public TT_WP()
        {
            u = new Util();
            det = new TT_WP_Details();
            LoadTime = System.DateTime.Now;
        }

        public DataTable getWP(Int64 CustomerId, int root)
        {
            string sql;
            SqlConnection cn = u.sqlConn;
            if (CustomerId == 0)
            {
                sql = string.Format("SELECT Name,Text " +
                    "FROM B10Sec.dbo.TT_Classes " +
                    "WHERE Parent={0}", root);
            }
            else
            {
                sql = string.Format("SELECT *,ROW_NUMBER() OVER(ORDER BY Ord) Ln FROM TT_vWP " +
                    "WHERE CustomerId= {0} AND Frm_CatID = {1} AND PeriodKey <= {2} AND Status = {3} " +
                    "ORDER BY Ord", CustomerId, Frm_CatID, PeriodKey, Status);
            }
            SqlCommand cD = new SqlCommand(sql, cn);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cD;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        internal int deleteRecord(int WpId_E)
        {
            string sql = string.Format("UPDATE TT_WP SET Status = 1 WHERE Id={0}", WpId_E);
            string sql1 = string.Format("UPDATE TT_WP_Reports SET Status = 1 WHERE WPId={0}", WpId_E);
            Exception ex = u.executeSql(sql);
            if (ex != null) { HttpContext.Current.Response.Write(ex.Message); HttpContext.Current.Response.End(); }
            Exception ex1 = u.executeSql(sql1);
            if (ex1 != null) { HttpContext.Current.Response.Write(ex1.Message); HttpContext.Current.Response.End(); }
            return 0;
        }

        internal int UpdateRecord(int WpId_E)
        {
            string sql=string.Empty;
            if (WpId_E == 0)
            {

                sql += "INSERT INTO TT_WP(WpId,CustomerID,EventId,FormId,RangeId,SubjectId,PurposeId,PurposeText,TargetId,WeightId,Ord,Status,Frm_CatID,PeriodKey,LoadTime,UserId) VALUES(";
                sql += "NULL,";
                sql += CustomerId.ToString() + ",";
                sql += EventId.ToString() + ",";
                sql += FormId.ToString() + ",";
                sql += RangeId.ToString() + ",";
                sql += SubjectId.ToString() + ",";
                sql += PurposeId.ToString() + ",";
                sql += (Purpose == string.Empty ? "NULL," : "'" + Purpose.Replace("'", "''") + "',");
                sql += TargetId.ToString() + ",";
                sql += WeightId.ToString() + ",";
                sql += string.Format("ISNULL((SELECT MAX(Ord) + 1 FROM TT_WP WHERE FormID = {0}),1),", FormId);
                sql += Status.ToString() + ",";
                sql += Frm_CatID.ToString() + ",";
                sql += PeriodKey.ToString() + ",";
                sql += "'" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", LoadTime) + "',";
                sql += UserId.ToString() + ")";
            }
            else
            {
                sql += "UPDATE TT_WP SET ";
                sql += "RangeId = " + RangeId.ToString() + ",";
                sql += "SubjectId = " + SubjectId.ToString() + ",";
                sql += "PurposeId = " + PurposeId.ToString() + ",";
                sql += "PurposeText = " + (Purpose == string.Empty ? "NULL," : "'" + Purpose.Replace("'", "''") + "',");
                sql += "TargetId = " + TargetId.ToString() + ",";
                sql += "WeightId = " + WeightId.ToString() + ",";
                sql += "Status = " + Status.ToString() + ",";
                sql += "LoadTime = '" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", LoadTime) + "',";
                sql += "UserId = " + UserId.ToString() + " ";
                sql += "WHERE Id = " + WpId_E.ToString();
            }

            Exception ex = u.executeSql(sql);
            if (ex == null)
            {
                if (WpId_E == 0)
                {
                    WpId_E = (int)u.selectDBScalar(string.Format("SELECT TOP 1 Id FROM TT_WP WHERE LoadTime='{0:yyyy-MM-dd HH:mm:ss}' AND FormId = {1} AND EventID = {2}", LoadTime, FormId, EventId));
                }
            }
            else
            {
                throw ex;
            }
            return WpId_E;
        }

        internal void loadWP(int WpId)
        {
            DataTable dt = u.selectDBQuesry(string.Format("SELECT * from TT_WP WHERE Id={0}", WpId));
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                FormId = (int)r["FormId"];
                EventId = (int)r["EventId"];
                CustomerId = (int)r["CustomerId"];
                Frm_CatID = (int)r["Frm_CatId"];
            }
        }
    }
}
