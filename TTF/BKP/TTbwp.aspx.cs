using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTF
{
    using System.Data;
    using TTF.App_Code;
    public partial class TTbwp : System.Web.UI.Page
    {

        TTF.App_Code.bpHelber h;
        TTF.App_Code.Util u;

        public TTbwp()
        {
           
            u = new Util();
            u = new TTF.App_Code.Util();
            h = new TTF.App_Code.bpHelber(Page, 3, 131);
            h.lvWP = lvWP;
 //           h.LVWeeklyPlan = LVWeeklyPlan;
            h.hdnCurrentEventId = hdnCurrentEventId;

          
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            h = new TTF.App_Code.bpHelber(Page, 3, 131);
            h.lvWP = lvWP;
            h.CustEventTypeId = 131;
            h.FormTypeId = 3;

            lvHdr.DataBind();
            h.initForm();
            h.selectAction();
        }

        protected void ddl_DataBinding(object sender, EventArgs e)
        {
            if (h == null) h = new TTF.App_Code.bpHelber(Page, 3, 131);
            h.lvWP = lvWP;
            DropDownList ddl = (DropDownList)sender;
            ListViewItem lvi = (ListViewItem)ddl.NamingContainer;
            string s = ddl.ID;
            h.SetSelectList(lvi, ddl);
        }

        protected void ddl_PreRender(object sender, System.EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            ListViewItem lvi = (ListViewItem)ddl.NamingContainer;
            h.showTextBoxifOther(ddl, lvi);
        }


        protected void ddl_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // databind cascading ddls

            DropDownList ddl = (DropDownList)sender;
            ListViewItem lvi = (ListViewItem)ddl.NamingContainer;

            h.datasBindCasscadingCtrl(ddl, lvi);

            //foreach (ListItem li in ddl.Items)
            //{
            //    Response.Write(li.Value.ToString() + "=" + li.Text + "<br />");
            //}
            // if "(...)" Show CheckBox

            h.spcialActs(ddl, lvi);
        }
        protected void lnkbShowAdd_Click(object sender, System.EventArgs e)
        {
            LinkButton lnkb = (LinkButton)sender;
            ListView lv = (ListView)lnkb.NamingContainer;
            lv.InsertItemPosition = InsertItemPosition.FirstItem;

            hdnItemHandled.Value = "-1";
        }
        protected void lnkbCancel_Click(object sender, System.EventArgs e)
        {
            LinkButton lnkb = (LinkButton)sender;
            ListView lv = (ListView)lnkb.NamingContainer.NamingContainer;
            lv.InsertItemPosition = InsertItemPosition.None;
            hdnItemHandled.Value = null;
        }
        protected void dlIdexes_PreRender(object sender, System.EventArgs e)
        {
            ListView dl = (ListView)sender;
            ListViewItem lvi = (ListViewItem)dl.NamingContainer;
            HiddenField hdn = (HiddenField)lvi.FindControl("hdnWPId");
            int id = (hdn.Value == string.Empty ? 0 : int.Parse(hdn.Value));
            if (id != 0) h.getSupportIndexes(id, dl);
        }
        protected void dlDetails_PreRender(object sender, System.EventArgs e)
        {
            ListView dl = (ListView)sender;
            ListViewItem lvi = (ListViewItem)dl.NamingContainer;
            HiddenField hdn = (HiddenField)lvi.FindControl("hdnWPId");
            int id = (hdn.Value == string.Empty ? 0 : int.Parse(hdn.Value));
            if (dl.InsertItem != null) // shows or hides the Details listView
                if (id == 0) dl.InsertItemPosition = InsertItemPosition.None; else dl.InsertItemPosition = InsertItemPosition.FirstItem;
            h.getDetails(id, dl);
        }
        protected void lvHdr_DataBind(object sender, System.EventArgs e)
        {
            h.getHeader(lvHdr);
        }

        protected void lnkbWP_Click(object sender, System.EventArgs e)
        {
            //*******************************************************************
            LinkButton lnkb = (LinkButton)sender;
            ListViewItem lvi = (ListViewItem)lnkb.NamingContainer;
            ListView lv = (ListView)lvi.NamingContainer;
            h.WpId = h.UpdateWp(lvi);
            if (lnkb.Text != "שמירה והוספת פירוט")
            {
                ((ListView)lvi.NamingContainer).DataBind();
                hdnItemHandled.Value = null;
                if (lv.EditIndex >= 0) lv.EditIndex = -1;
            }
            else
            {
                hdnCurrentWpId.Value = h.WpId.ToString();
            }
            //*******************************************************************

           // LinkButton lnkb = (LinkButton)sender;
            //ListViewItem lvi = (ListViewItem)lnkb.NamingContainer;
           // h.UpdateWp(lvi);
            ((ListView)lvi.NamingContainer).DataBind();
            hdnItemHandled.Value = null;
        }

        protected void lv_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            ListView lv = (ListView)sender;
            if (lv.ID == "lvWP")
            {

                HiddenField hdn = (HiddenField)lv.Items[e.NewEditIndex].FindControl("hdnWpId");
                hdnItemHandled.Value = hdn.Value;
            }

        }

        protected void lv_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            ListView lv = (ListView)sender;
            lv.EditIndex = -1;
        }
        protected void lnkbShowAdd_PreRender(object sender, System.EventArgs e)
        {
            LinkButton lnkb = (LinkButton)sender;
            lnkb.Visible = lvWP.InsertItemPosition == InsertItemPosition.None;
        }
        protected void lit_Prerender(object sender, System.EventArgs e)
        {
            Literal lit = (Literal)sender;
            ListViewItem lvi = (ListViewItem)lit.NamingContainer;
            ListViewItem outerLvi = (ListViewItem)((ListView)lvi.NamingContainer).NamingContainer;
            string s = string.Format("<a class='btns' href=\"javascript:var s = '{0}|{1}" +
                                                                                h.javaParam(lvi, "hdn", "IndexId") +
                                                                                h.javaParam(lvi, "hdn", "WpId") +
                                                                                h.javaParam(lvi, "hdn", "val") +
                                                                                "' + myValues; __doPostBack('+_+{2}',s);\">{3}</a>", +
                                                                                outerLvi.DisplayIndex, lvi.DisplayIndex, lit.ID.Substring(3), lit.Text);
            lit.Text = s;
        }
        protected void tbIndex_TextChanged(object sender, System.EventArgs e)
        {
        }
        protected void tb_PreRender(object sender, System.EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Attributes.Add("onchange", "myValues = myValues + '|' + 'Text=' + this.value");
        }
        protected void tbOther_PreRender(object sender, System.EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            h.displayIfValue(tb);
        }
        protected void lvWP_PreRender(object sender, System.EventArgs e)
        {
            if (lvWP.Items.Count > 0 && hdnItemHandled.Value == string.Empty) lvWP.InsertItemPosition = InsertItemPosition.None;
            if (hdnItemHandled.Value != string.Empty && int.Parse(hdnItemHandled.Value) >= 0) lvWP.EditIndex = 0;
        }
        protected void Det_Inserting(object sender, ListViewInsertEventArgs e)
        {
            ListView lv = (ListView)sender;
            ListViewItem lvi = (ListViewItem)lv.NamingContainer;
            h.UpdeteDetails(e.Item, lvi.DisplayIndex);
        }
        protected void Det_Updating(object sender, ListViewUpdateEventArgs e)
        {
            ListView lv = (ListView)sender;
            ListViewItem lvi = (ListViewItem)lv.NamingContainer;
            h.UpdeteDetails(lv.EditItem, lvi.DisplayIndex);
            lv.EditIndex = -1;
        }


        protected void lblIndexName_PreRender(object sender, System.EventArgs e)
        {
            //    Label lbl = (Label)sender;
            //    ListViewItem lvi = (ListViewItem)lbl.NamingContainer;
            //    HiddenField hdn = (HiddenField)lvi.FindControl("hdnIndexId");
            //    int indexId = int.Parse(hdn.Value);

            //    int TargetId = 0;
            //    ListView lv = (ListView)lvi.NamingContainer;
            //    lvi = (ListViewItem)lv.NamingContainer;

            //    DropDownList ddl = (DropDownList)lvi.FindControl("ddlTarget");
            //    if (ddl != null)
            //    {
            //        if (ddl.SelectedValue != string.Empty)
            //        {
            //            TargetId = int.Parse(ddl.SelectedValue);
            //        }
            //    }
            //    else
            //    {
            //        hdn = (HiddenField)lvi.FindControl("hdnTargetId");
            //        if (hdn != null)
            //        {
            //            if (hdn.Value != string.Empty)
            //            {
            //                TargetId = int.Parse(hdn.Value);
            //            }
            //        }
            //    }
            //    if (indexId ==(int) u.selectDBScalar(string.Format("SELECT IndexId FROM B10Sec.dbo.TT_Class_AdditionalColumns WHERE ClassId={0}", TargetId)))
            //    {
            //        lbl.BorderWidth = 2;
            //        lbl.BorderColor = System.Drawing.Color.Aqua;
            //    }
            //    else
            //    {
            //        lbl.BorderWidth = 0;
            //        lbl.BorderColor = System.Drawing.Color.Transparent;
            //    }
        }
    }

}