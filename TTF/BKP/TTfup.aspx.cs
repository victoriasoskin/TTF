using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTF
{
    public partial class TTfup : System.Web.UI.Page
    {
        TTF.App_Code.bpHelber h;
        TTF.App_Code.Util u = new App_Code.Util();
        TTF.App_Code.TT_WP_Reports r = new App_Code.TT_WP_Reports();

        public TTfup()
        {
            h = new TTF.App_Code.bpHelber(Page,8,136);
            h.lvWP = lvWP;

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Request.QueryString["E"] = 823;
            //Session["UserName"] = "בדיקה";
            //Request.QueryString["F"] = 122;
            //Request.QueryString["CID"] = 50631332;
            h = new TTF.App_Code.bpHelber(Page,4,132);
            h.lvWP = lvWP;
            h.CustEventTypeId = 132;
            h.FormTypeId = 4;
            lvHdr.DataBind();

        }

        protected void hdn_PreRender(object sender, System.EventArgs e)
        {
        }

        protected void lvHdr_DataBind(object sender, System.EventArgs e)
        {
            h.getHeader(lvHdr);
        }

        protected void dlIdexes_PreRender(object sender, System.EventArgs e)
        {
            ListView dl = (ListView)sender;
            ListViewItem lvi = (ListViewItem)dl.NamingContainer;
            HiddenField hdn = (HiddenField)lvi.FindControl("hdnWPId");
            int id = (hdn.Value == string.Empty ? 0 : int.Parse(hdn.Value));
            h.getSupportIndexes(id, dl);
        }
        protected void dlDetails_PreRender(object sender, System.EventArgs e)
        {
            ListView dl = (ListView)sender;
            ListViewItem lvi = (ListViewItem)dl.NamingContainer;
            HiddenField hdn = (HiddenField)lvi.FindControl("hdnWPId");
            int id = (hdn.Value == string.Empty ? 0 : int.Parse(hdn.Value));
            h.getDetails(id, dl);
        }

        protected void ShowFup_Click(object sender, System.EventArgs e)
        {
            LinkButton lnkb = (LinkButton)sender;
            ListViewItem lvi = (ListViewItem)lnkb.NamingContainer;
            ListView lv = (ListView)lvi.FindControl("lvFup");
            HiddenField hdn = (HiddenField)lvi.FindControl("hdnWpId");

            DataTable dt = r.getReports(int.Parse(hdn.Value));
            lv.DataSource = dt;
            lv.DataBind();
        }
        protected void SaveRep_Click(object sender, System.EventArgs e)
        {
            LinkButton lnkb = (LinkButton)sender;
            ListViewItem lvi = (ListViewItem)lnkb.NamingContainer;
          h.updateReport(lvi);
        }
        protected void lblIndexName_PreRender(object sender, System.EventArgs e)
        {
            Label lbl = (Label)sender;
            ListViewItem lvi = (ListViewItem)lbl.NamingContainer;
            HiddenField hdn = (HiddenField)lvi.FindControl("hdnIndexId");
            int indexId = int.Parse(hdn.Value);

            int TargetId = 0;
            ListView lv = (ListView)lvi.NamingContainer;
            lvi = (ListViewItem)lv.NamingContainer;

            DropDownList ddl = (DropDownList)lvi.FindControl("ddlTarget");
            if (ddl != null)
            {
                if (ddl.SelectedValue != string.Empty)
                {
                    TargetId = int.Parse(ddl.SelectedValue);
                }
            }
            else
            {
                hdn = (HiddenField)lvi.FindControl("hdnTargetId");
                if (hdn != null)
                {
                    if (hdn.Value != string.Empty)
                    {
                        TargetId = int.Parse(hdn.Value);
                    }
                }
            }
            if (indexId == (int)u.selectDBScalar(string.Format("SELECT IndexId FROM B10Sec.dbo.TT_Class_AdditionalColumns WHERE ClassId={0}", TargetId)))
            {
                lbl.BorderWidth = 2;
                lbl.BorderColor = System.Drawing.Color.Aqua;
            }
            else
            {
                lbl.BorderWidth = 0;
                lbl.BorderColor = System.Drawing.Color.Transparent;
            }
        }

    }
}