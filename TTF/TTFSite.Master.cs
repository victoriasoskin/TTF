using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace TTF
{
    public partial class TTFSite : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    string pg = HttpContext.Current.Request.Url.AbsoluteUri;
            //    string cs = System.Configuration.ConfigurationManager.ConnectionStrings["BEBook10"].ConnectionString;
            //    SqlConnection cc = new SqlConnection(cs);
            //    SqlCommand cD = new SqlCommand(string.Format("IF EXISTS(SELECT * FROM p0t_APPClosed WHERE LogActive = 1) \n" +
            //        "INSERT PLog(Page,UserId,SessionId,Logtime)VALUES('{0}',{1},'{2}',GETDATE())",
            //        pg.Replace("'", "''").Substring(0, (pg.Length > 500 ? 500 : pg.Length)),
            //        (HttpContext.Current.Session["Userid"] == null ? 0 : (int)HttpContext.Current.Session["UserID"]),
            //        HttpContext.Current.Session.SessionID.Replace("'", "''"))
            //        , cc);
            //    cc.Open();
            //    cD.ExecuteNonQuery();
            //    cc.Close();
            //}

        }
    }
}