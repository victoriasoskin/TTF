using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTF
{
    public partial class delweek : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            App_Code.Util u = new App_Code.Util();
            Exception ex = u.executeSql(string.Format("UPDATE TT_WP_Week SET Status = 1 WHERE Id={0}", Request.QueryString["id"]));
        }
    }
}