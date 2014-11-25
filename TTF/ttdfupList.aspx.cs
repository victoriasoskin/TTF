using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTF
{
    public partial class ttdfupList : System.Web.UI.Page
    {
        TTF.App_Code.bpHelber h;
        int cCustEventTypeId = 0;
        int cFormTypeId = 0;
        int UpdateIndexValue;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            cCustEventTypeId = int.Parse(Request.QueryString["ET"]);
            cFormTypeId = int.Parse(Request.QueryString["FT"]);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
            h = new App_Code.bpHelber(Page, cFormTypeId, cCustEventTypeId);
            Session["UserId"] = h.getUserId();
            lvHdr.DataBind();

            h.selectAction();


            if (!IsPostBack)
            {
                DataTable dt = h.p5Details(cCustEventTypeId);
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
            //RadioButtonList rbl = (RadioButtonList)lvi.FindControl("rbl1");
            //DSReport.UpdateParameters["val1"].DefaultValue = rbl.SelectedValue.ToString();
            //rbl = (RadioButtonList)lvi.FindControl("rbl2");
            //DSReport.UpdateParameters["val2"].DefaultValue = rbl.SelectedValue.ToString();
            //rbl = (RadioButtonList)lvi.FindControl("rbl3");
            //DSReport.UpdateParameters["val3"].DefaultValue = rbl.SelectedValue.ToString();
        }
        protected void lvHdr_DataBind(object sender, System.EventArgs e)
        {
            h.getHeader(lvHdr);
        }
        protected bool isUpdatable()
        {
            return h.Isupdatable();
        }

        protected void DSReport_Deleting(object sender, SqlDataSourceCommandEventArgs e)
        {
            string s = e.Command.Parameters[0].ToString();
            int i = (int)e.Command.Parameters[0].Value;
        }

        protected void lvIndexes_PreRender(object sender, EventArgs e)
        {
            ListView lv = (ListView)sender;
            ListViewItem lvi = (ListViewItem)lv.NamingContainer;
            int id  = int.Parse(Request.QueryString["RId"]);
            
            //int id = (hdn.Value == string.Empty ? 0 : int.Parse(hdn.Value));

            if (id != 0) h.getSupportIndexes(id, lv);

            string[] onc = { "", "", "", "", "" };
            RadioButton rb;
            for (int i = 0; i < lv.Items.Count; i++)
            {
                lvi = lv.Items[i];
                rb = (RadioButton)lvi.FindControl("rb");
                //      onc[i] = string.Format("$('#{0}').prop('checked', false);", rb.ClientID);
                onc[i] = string.Format("document.getElementById('{0}').checked = false;", rb.ClientID);
            }

            for (int i = 0; i < lv.Items.Count; i++)
            {
                string soc = "";
                lvi = lv.Items[i];
                rb = (RadioButton)lvi.FindControl("rb");
                for (int j = 0; j < lv.Items.Count; j++)
                {
                    if (j != i)
                    {
                        soc += onc[j];
                    }
                }
                rb.Attributes.Add("onclick", soc);
            }
        }

        protected void lblIndexName_PreRender(object sender, EventArgs e)
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
        }

        protected void rb_PreRender(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            //int selectedValue = rb.Checked;
        }

        protected void lnkbEdit_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            ListViewItem lvi = (ListViewItem)lb.NamingContainer;
            HiddenField hdn = (HiddenField)lvi.FindControl("hdnRepId");
            HiddenField hdn1 = (HiddenField)lvi.FindControl("hdnSelectedRowId");
            hdn1 = hdn;
            UpdateIndexValue = int.Parse(hdn.Value);
        }

        protected void lnkbSave_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            ListViewItem lvi = (ListViewItem)lb.NamingContainer;
            ListView lv_rb = (ListView)lvi.FindControl("lvIndexes");
            HiddenField hdn = (HiddenField)lvi.FindControl("hdnSelectedRowId");
            string s = "לא הזינו ערך";
            int selectedValue = 0;
            //string str = "";
            TextBox temp = (TextBox)lvi.FindControl("TBUpdateIndexReport");
            string str = (temp.Text==null?s:temp.Text);
            str = "\'" + str + "\'";
            foreach (ListViewItem lvk in lv_rb.Items)
            {
                RadioButton r = (RadioButton)lvk.FindControl("rb");
                bool status = r.Checked;
                if (status)
                {
                    Label result = (Label)lvk.FindControl("lblIndexName");
                    selectedValue = int.Parse(result.Text);
                    break;
                }
            }
            string updateCommand = "update Book10.dbo.TT_WP_Reports set val = {0} ,text={1} where  id = {2}";
            updateCommand = updateCommand.Replace("{0}", selectedValue.ToString()).Replace("{1}", str).Replace("{2}",hdn.Value.ToString());
            DSReport.UpdateCommand = updateCommand;
            h.UpdateIndexes(h.WpId, h.EventId, selectedValue, str, hdn);
            //RefreshGraph(h.WpId, h.EventId, sender);
        }
    }
}