using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTF.App_Code
{
    public class TT_WP_Week
    {
        public int FormId { get; set; }
        public int EventId { get; set; }
        public Int64 CustomerId { get; set; }
        public int Frm_CatId { get; set; }
        public int DetailsId { get; set; }
        public char DayId { get; set; }
        public int Status { get; set; }
        public int PeriodKey { get { return System.DateTime.Today.Year * 100 + System.DateTime.Today.Month; } }
        public DateTime LoadTime { get; set; }
        public int UserId { get; set; }

        Util u;

        public TT_WP_Week()
        {
            u = new Util();
            LoadTime = System.DateTime.Now;
        }
 
        internal bool updateRecord()
        {
            string sql = string.Format("DECLARE @EventId int " +
                                       "DECLARE @FormId int " +
                                       "SELECT @FormId = FormId,@EventId = EventId FROM TT_WP_Details WHERE Id = {3} " +
                                       "INSERT INTO Book10.dbo.TT_WP_Week(FormID,EventID,CustomerId,Frm_CatId,DayID,DetailsId,Status,PeriodKey,LoadTime,UserID) " +
                                                    "VALUES(@FormId,@EventId,{0},{1},'{2}',{3},0,{4},'{5:yyyy-MM-dd HH:mm:ss}',{6})", CustomerId, Frm_CatId, DayId, DetailsId, PeriodKey, LoadTime, UserId);
            Exception ex = u.executeSql(sql);
            return ex == null;
        }


        internal void deleteRecord(long Id)
        {
            Exception ex = u.executeSql(string.Format("UPDATE TT_WP_Week SET Status = 1 WHERE Id={0}", Id));
        }
    }
}