using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTF
{
    public partial class TTeupList : System.Web.UI.Page
    {
        TTF.App_Code.bpHelber h;
        protected void Page_Load(object sender, EventArgs e)
        {
            h = new TTF.App_Code.bpHelber(Page, lvDetup, 7);
            Session["Userid"] = 823;
            lvHdr.DataBind();
        }

        protected void lvDetup_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            ListView lv = (ListView)sender;
            ListViewItem lvi = lv.EditItem;
            RadioButtonList rbl = (RadioButtonList)lvi.FindControl("rblFUDet");
            TextBox TB = (TextBox)lvi.FindControl("tbFuT");
            DSDetReport.UpdateParameters["VAL"].DefaultValue = rbl.SelectedValue.ToString();
            DSDetReport.UpdateParameters["Text"].DefaultValue = TB.Text;
        }
        protected void lvHdr_DataBind(object sender, System.EventArgs e)
        {
            h.getHeader(lvHdr);
        }
    }
}