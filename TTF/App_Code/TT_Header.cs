using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TTF.App_Code
{
    public class TT_Header
    {
        public Int64 CustomerId { get; set; }
        public string FormType { get; set; }
        public string Name { get; set; }
        public DateTime CustBirthDate { get; set; }
        public string FrameName { get; set; }
        public DateTime EnterDate { get; set; }
        public string URName { get; set; }
        public DateTime CustEventDate { get; set; }

        public string CustEventId { get; set; }
        public int Frm_CatID { get; set; }

        Util u;

        public TT_Header()
        {

       //     URName =(string) HttpContext.Current.Session["UserName"];
            u = new Util();
        }
       public DataTable getHeader(int EventId,Int64 CustomerId,int FId)
        {

            DataTable dt = u.selectDBQuesry(string.Format("SELECT '" + FormType + "' FormType,*,'" + URName + "' URName FROM book10.dbo.TT_Header({0},{1},{2})", CustomerId, FId, EventId));
            return dt;
        }

    }
}