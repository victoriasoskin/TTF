using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TTF.App_Code
{
    public class TT_WP_Indexes
    {
        public int Id { get; set; }
        public int FormId { get; set; }
        public int EventId { get; set; }
        public int IndexId { get; set; }
        public int WPId { get; set; }
        public string text { get; set; }
        public int val { get; set; }
        public int Status { get; set; }
        public Int64 CustomerId { get; set; }
        public int Frm_CatId { get; set; }
        public int PeriodKey { get { return System.DateTime.Today.Year * 100 + System.DateTime.Today.Month; } }
        public DateTime LoadTime { get; set; }
        public int UserId { get; set; }

        Util u;

        public TT_WP_Indexes()
        {
            u = new Util();
            LoadTime = System.DateTime.Now;
        }

        public DataTable getIndexes(int Id, int root)
        {
            string sql;
            SqlConnection cn = u.sqlConn;
            sql = string.Format("SELECT i.Id ,ISNULL(i.IndexId,c.Id) IndexId,c.Name ,WPId ,i.Text ,c.Val " +
                "FROM B10Sec.dbo.TT_Classes c " +
                "LEFT OUTER JOIN (SELECT * FROM TT_WP_Indexes WHERE WPId={0}) i ON c.Val=i.Val " +
                "WHERE Parent={1}", Id, root);
            SqlCommand cD = new SqlCommand(sql, cn);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cD;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        internal bool UpdateRecord()
        {
            string sql = string.Format("IF EXISTS(SELECT * FROM Book10.dbo.TT_WP_Indexes WHERE WpId={0} AND IndexId={1})", WPId, IndexId);

            sql += "UPDATE Book10.dbo.TT_WP_Indexes SET ";
            if (text != null && text != string.Empty) sql += "Text='" + text.Replace("'", "''") + "',";
            sql += "PeriodKey=" + PeriodKey.ToString()+ ",";
            sql += "LoadTime='" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", LoadTime) + "',";
            sql += "UserId=" + UserId.ToString() + " ";
            sql += string.Format("WHERE WpId={0} AND IndexId={1} ", WPId, IndexId);
 
            sql += "ELSE ";
  
            sql += "INSERT INTO Book10.dbo.TT_WP_Indexes(FormID,EventID,IndexId,WPId,Text,Val,Status,CustomerId,Frm_CatId,PeriodKey,LoadTime,UserId) VALUES(";
            sql += FormId.ToString() + ",";
            sql += EventId.ToString() + ",";
            sql += IndexId.ToString() + ",";
            sql += WPId.ToString() + ",";
            sql += (text == string.Empty ? "NULL," : "'" + text.Replace("'", "''") + "',");
            sql += val.ToString() + ",";
            sql += Status.ToString() + ",";
            sql += CustomerId.ToString() + ",";
            sql += Frm_CatId.ToString() + ",";
            sql += PeriodKey.ToString() + ",";
            sql += "'" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", LoadTime) + "',";
            sql += UserId.ToString() + ")";

            Exception ex = u.executeSql(sql);
            return ex == null;
        }
        /// <summary>
        /// this function gets the indexes parameters  from tohniot tmiha  and insert thm into data base.
        /// in case the indexes are already there , we delete the exesting indexes and insert a new ones.
        /// we get to this function from "שמירה והוספת פירוט(1)" or from "שמירה וסגירה(2)"
        /// in case 1 there are not supposed to be any indexes so there has nothing to delete
        /// only to nsert
        /// on the second case we check if this eventid is already on the table TT_WP_Indexes and if  os we delete
        /// them and insert aain
        /// </summary>
        /// <param name="WpId"></param>
        /// <param name="sp2"></param>
        /// <param name="sp1"></param>
        /// <param name="s0"></param>
        /// <param name="sm1"></param>
        /// <param name="sm2"></param>
        public void insertFirstTimeIndexes(int WpId, string sp2, string sp1, string s0, string sm1, string sm2)
        {
            string str = string.Format("select eventid from book10.dbo.TT_WP_Indexes where Wpid={0}", WpId);
           
            object res = u.selectDBScalar(str);
            if (res!=null)
            {
                string deleteWpId=string.Format("delete from book10.dbo.TT_WP_Indexes where wpid={0}", WpId);
                u.executeSql(deleteWpId);
            }
            string sql = string.Format("insert into [Book10].[dbo].[TT_WP_Indexes]  (FormId ,EventId,wpid, text ,Val,Status,CustomerId,Frm_CatId,LoadTime,PeriodKey, UserId) values  ( '{0}' , '{1}' ,'{2}','{3}', '{4}' , '{5}' ,'{6}','{7}','{8}','{9}','{10}' ) \n"
                        , FormId, EventId, WpId, sp2.Replace("'","''"), -2, 0, CustomerId, Frm_CatId, string.Format("{0:yyyy-MM-dd HH:mm:ss}", LoadTime), PeriodKey, UserId)
             + string.Format("insert into [Book10].[dbo].[TT_WP_Indexes]  (FormId ,EventId,wpid, text ,Val,Status,CustomerId,Frm_CatId, LoadTime,PeriodKey,UserId) values  ( '{0}' , '{1}' ,'{2}','{3}', '{4}' , '{5}' ,'{6}','{7}','{8}' ,'{9}','{10}') \n"
                        , FormId, EventId, WpId, sp1.Replace("'", "''"), -1, 0, CustomerId, Frm_CatId, string.Format("{0:yyyy-MM-dd HH:mm:ss}", LoadTime), PeriodKey, UserId)
            + string.Format("insert into [Book10].[dbo].[TT_WP_Indexes]   (FormId ,EventId,wpid, text ,Val,Status,CustomerId,Frm_CatId,LoadTime,PeriodKey, UserId) values  ( '{0}' , '{1}' ,'{2}','{3}', '{4}' , '{5}' ,'{6}','{7}' ,'{8}','{9}','{10}') \n"
                         , FormId, EventId, WpId, s0.Replace("'", "''"), 0, 0, CustomerId, Frm_CatId, string.Format("{0:yyyy-MM-dd HH:mm:ss}", LoadTime), PeriodKey, UserId)
            + string.Format("insert into [Book10].[dbo].[TT_WP_Indexes]  (FormId ,EventId,wpid, text ,Val,Status,CustomerId,Frm_CatId, LoadTime,PeriodKey,UserId) values  ( '{0}' , '{1}' ,'{2}','{3}', '{4}' , '{5}' ,'{6}','{7}','{8}','{9}','{10}' ) \n"
                         , FormId, EventId, WpId, sm1.Replace("'", "''"), +1, 0, CustomerId, Frm_CatId, string.Format("{0:yyyy-MM-dd HH:mm:ss}", LoadTime), PeriodKey, UserId)
            + string.Format("insert into [Book10].[dbo].[TT_WP_Indexes]  (FormId ,EventId,wpid, text ,Val,Status,CustomerId,Frm_CatId,LoadTime,PeriodKey, UserId) values  ( '{0}' , '{1}' ,'{2}','{3}', '{4}' , '{5}' ,'{6}','{7}' ,'{8}','{9}','{10}') "
                        , FormId, EventId, WpId, sm2.Replace("'", "''"), +2, 0, CustomerId, Frm_CatId, string.Format("{0:yyyy-MM-dd HH:mm:ss}", LoadTime), PeriodKey, UserId);
           // var eventId = EventId;

            u.executeSql(sql);
            //u.executeSql(sqlp1);
            //u.executeSql(sql0);
            //u.executeSql(sqlm1);
            //u.executeSql(sqlm2);

        }
    }
}
