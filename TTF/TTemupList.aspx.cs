using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTF
{
    public partial class TTemupList : System.Web.UI.Page
    {
        TTF.App_Code.bpHelber h;
        protected void Page_Load(object sender, EventArgs e)
        {
            h = new App_Code.bpHelber(Page,8,136);

            lvHdr.DataBind();

            h.selectAction();


            if (!IsPostBack)
            {
                DataTable dt = h.p5Details(135);
                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    DateTime d = (DateTime)r["CustEventDate"];
                    string s = string.Format("{0:dd/MM/yyyy}", d);
                    if (s != "01/01/1900") hdnDate127.Value = s;
                    hdnForm127.Value = r["CustRelateId"].ToString();
                    dt.Dispose();
                }
            }
        }

        protected void lvRep_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            ListView lv = (ListView)sender;
            ListViewItem lvi = lv.EditItem;
            RadioButtonList rbl = (RadioButtonList)lvi.FindControl("rbl1");
            DSReport.UpdateParameters["val1"].DefaultValue = rbl.SelectedValue.ToString();
            rbl = (RadioButtonList)lvi.FindControl("rbl2");
            DSReport.UpdateParameters["val2"].DefaultValue = rbl.SelectedValue.ToString();
            rbl = (RadioButtonList)lvi.FindControl("rbl3");
            DSReport.UpdateParameters["val3"].DefaultValue = rbl.SelectedValue.ToString();
        }
        protected void lvHdr_DataBind(object sender, System.EventArgs e)
        {
            h.getHeader(lvHdr);
        }
        protected bool isUpdatable()
        {
            return h.Isupdatable();
        }

        protected void DSDet_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            int t = int.Parse(Request.QueryString["RId"].ToString());
          e.Command.Parameters["@RId"].Value=t ;
        }
    }
}