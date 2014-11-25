using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TTF.App_Code
{
    public class TT_MP
    {
        public int MpId { get; set; }
        public int FormId { get; set; }
        public int EventId { get; set; }
        public Int64 CustomerId { get; set; }
        public string SupportCoordinator { get; set; }
        public string Supporter { get; set; }
        public string Location { get; set; }
        public int FamilyStatusId { get; set; }
        public string FamilyStatus { get; set; }
        public int LayerId { get; set; }
        public int ClassId { get; set; }
        public string Class { get; set; }
        public string Strength { get; set; }
        public string Hobby { get; set; }
        public string PreviousFrame { get; set; }
        public string OtherInfo { get; set; }
        public DateTime LoadTime { get; set; }
        public int PeriodKey { get { return System.DateTime.Today.Year * 100 + System.DateTime.Today.Month; } }
        public int UserId { get; set; }
        public int CustEventTypeId { get; set; }
        Util u;
        bpHelber h ;

        public TT_MP()
        {
            u = new Util();
            LoadTime = System.DateTime.Now;
            
        }

        #region get record

        public DataTable getMP(Int64 CustomerId)
        {
            string sql = string.Format(string.Format("SELECT * FROM Book10.dbo.TT_EduMP({0})", CustomerId));

            DataTable dt = u.selectDBQuesry(sql);
            return dt;
        }

        internal int UpdateRecord(int MpId_E)
        {
            
            string sql = u.getVersionDetails("TT_MP", "MpId", MpId_E);
            sql += string.Format(" IF ISNULL(@OldEvent,0) != {0} ", EventId);
            sql += "INSERT INTO Book10.dbo.TT_MP(MpId, FormId, EventId, CustomerId, SupportCoordinator, Supporter,Location, FamilyStatusId, LayerId,ClassId, Strength, Hobby, PreviousFrame, OtherInfo, LoadTime, PeriodKey, UserId, EventTypeId) VALUES(";
            sql += "@FirstId,";
            sql += FormId.ToString() + ",";
            sql += EventId.ToString() + ",";
            sql += CustomerId.ToString() + ",";
            sql += (SupportCoordinator != null && SupportCoordinator != string.Empty ? "'" + SupportCoordinator.Replace("'", "''") + "'," : "NULL,");
            sql += (Supporter != null && Supporter != string.Empty ? "'" + Supporter.Replace("'", "''") + "'," : "NULL,");
            sql += (Location != null && Location != string.Empty ? "'" + Location.Replace("'", "''") + "'," : "NULL,");
            sql += FamilyStatusId.ToString() + ",";
            sql += LayerId.ToString() + ","; 
            sql += ClassId.ToString() + ",";
            sql += (Strength != null && Strength != string.Empty ? "'" + Strength.Replace("'", "''") + "'," : "NULL,");
            sql += (Hobby != null && Hobby != string.Empty ? "'" + Hobby.Replace("'", "''") + "'," : "NULL,");
            sql += (PreviousFrame != null && PreviousFrame != string.Empty ? "'" + PreviousFrame.Replace("'", "''") + "'," : "NULL,");
            sql += (OtherInfo != null && OtherInfo != string.Empty ? "'" + OtherInfo.Replace("'", "''") + "'," : "NULL,");
            sql += "'" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", LoadTime) + "',";
            sql += PeriodKey.ToString() + ",";
            sql += UserId.ToString() + ",";
            sql += CustEventTypeId.ToString() + ")";
            sql += " else ";

            sql += "UPDATE TT_MP SET ";
            sql += "SupportCoordinator = " + (SupportCoordinator != null && SupportCoordinator != string.Empty ? "'" + SupportCoordinator.Replace("'", "''") + "'," : "NULL,");
            sql += "Supporter = " + (Supporter != null && Supporter != string.Empty ? "'" + Supporter.Replace("'", "''") + "'," : "NULL,");
            sql += "Location = " + (Location != null && Location != string.Empty ? "'" + Location.Replace("'", "''") + "'," : "NULL,");
            sql += "FamilyStatusId = " + FamilyStatusId.ToString() + ",";
            sql += "LayerId = " + LayerId.ToString() + ",";
            sql += "ClassId = " + ClassId.ToString() + ",";
            sql += "Strength = " + (Strength != null && Strength != string.Empty ? "'" + Strength.Replace("'", "''") + "'," : "NULL,");
            sql += "Hobby = " + (Hobby != null && Hobby != string.Empty ? "'" + Hobby.Replace("'", "''") + "'," : "NULL,");
            sql += "PreviousFrame = " + (PreviousFrame != null && PreviousFrame != string.Empty ? "'" + PreviousFrame.Replace("'", "''") + "'," : "NULL,");
            sql += "OtherInfo = " + (OtherInfo != null && OtherInfo != string.Empty ? "'" + OtherInfo.Replace("'", "''") + "'," : "NULL,");
            sql += "LoadTime = '" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", LoadTime) + "',";
            sql += "UserId = " + UserId.ToString() +  " ";
            sql += "WHERE Id = " + MpId_E.ToString();


            Exception ex = u.executeSql(sql);
            if (ex == null)
            {
                if (MpId_E == 0)
                {
                    MpId_E = (int)u.selectDBScalar(string.Format("SELECT TOP 1 Id FROM TT_MP WHERE LoadTime='{0:yyyy-MM-dd HH:mm:ss}' AND FormId = {1} AND EventID = {2}", LoadTime, FormId, EventId));
                }
            }
            else
            {
                throw ex;
            }
            return MpId_E;
        }
        internal void loadMP(int MpId)
        {
            DataTable dt = u.selectDBQuesry(string.Format("SELECT * from TT_MP WHERE Id={0}", MpId));
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                FormId = (int)r["FormId"];
                EventId = (int)r["EventId"];
            }

        }
        #endregion
    }
}