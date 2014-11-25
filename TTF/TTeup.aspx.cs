using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.DataVisualization;
using System.Data;
namespace TTF
{
    public partial class TTeup : System.Web.UI.Page
    {
        TTF.App_Code.bpHelber h;
        TTF.App_Code.Util u;
        TTF.App_Code.TT_WP_Reports r;
        int cCustEventTypeId = 0;
        int cFormTypeId = 0;

        #region load
        protected void Page_PreInit(object sender, EventArgs e)
        {
            cCustEventTypeId = int.Parse(Request.QueryString["ET"]);
            cFormTypeId = int.Parse(Request.QueryString["FT"]);
            u = new TTF.App_Code.Util();
            h = new TTF.App_Code.bpHelber(Page, cFormTypeId, cCustEventTypeId);
            h.lvMP = lvMP;
            u = new TTF.App_Code.Util();
            r = new TTF.App_Code.TT_WP_Reports();
            h.hdnCurrentEventId = hdnCurrentEventId;
        }

        public TTeup()
        {
            //u = new TTF.App_Code.Util();
            //h = new TTF.App_Code.bpHelber(Page, cFormTypeId, cCustEventTypeId);
            //h.lvMP = lvMP;
            //u = new TTF.App_Code.Util();
            //r = new TTF.App_Code.TT_WP_Reports();
            //h.hdnCurrentEventId = hdnCurrentEventId;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                h.initForm();
                DataTable dt = h.p5Details(127);
                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    DateTime d = (DateTime)r["CustEventDate"];
                    string s = string.Format("{0:dd/MM/yyyy}", d);
                    if (s != "01/01/1900") hdnDate127.Value = s;
                    hdnForm127.Value = r["CustRelateId"].ToString();
                    dt.Dispose();
                }

                h.selectAction();

                lvHdr.DataBind();
            }
        }

        #endregion

        #region header
        protected void lvHdr_DataBind(object sender, System.EventArgs e)
        {
            h.getHeader(lvHdr);
        }
        #endregion

        #region build work plan

        #region Add support

        protected void lnkbShowAdd_PreRender(object sender, System.EventArgs e)
        {
            LinkButton lnkb = (LinkButton)sender;
            lnkb.Visible = lvMP.InsertItemPosition == InsertItemPosition.None;
        }

        protected void lnkbShowAdd_Click(object sender, System.EventArgs e)
        {
            LinkButton lnkb = (LinkButton)sender;
            ListView lv = (ListView)lnkb.NamingContainer;
            lv.InsertItemPosition = InsertItemPosition.FirstItem;

            hdnItemHandled.Value = "-1";
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

        protected void lvRep_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            ListView lv = (ListView)sender;
        }

        protected void lv_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            ListView lv = (ListView)sender;
            lv.EditIndex = -1;
        }

        protected void lnkbWP_Click(object sender, System.EventArgs e)
        {

            LinkButton lnkb = (LinkButton)sender;
            ListViewItem lvi = (ListViewItem)lnkb.NamingContainer;
            h.UpdateWp(lvi);
            ((ListView)lvi.NamingContainer).DataBind();
            hdnItemHandled.Value = null;
        }

        protected void lnkbCancel_Click(object sender, System.EventArgs e)
        {
            LinkButton lnkb = (LinkButton)sender;
            ListView lv = (ListView)lnkb.NamingContainer.NamingContainer;
            lv.InsertItemPosition = InsertItemPosition.None;
            hdnItemHandled.Value = null;
        }
        #endregion

        #endregion

        #region lvW
        protected void lvMP_PreRender(object sender, System.EventArgs e)
        {
            if (lvMP.Items.Count > 0 && hdnItemHandled.Value == string.Empty) lvMP.InsertItemPosition = InsertItemPosition.None;
            if (hdnItemHandled.Value != string.Empty && int.Parse(hdnItemHandled.Value) >= 0) lvMP.EditIndex = 0;
        }

        #endregion
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
        protected void dlDetails_PreRender(object sender, System.EventArgs e)
        {
            ListView dl = (ListView)sender;
            ListViewItem lvi = (ListViewItem)dl.NamingContainer;
            HiddenField hdn = (HiddenField)lvi.FindControl("hdnWPId");
            int id = (hdn.Value == string.Empty ? 0 : int.Parse(hdn.Value));
            h.getWPDetails(id, dl);
        }
        protected void ddl_DataBinding(object sender, EventArgs e)
        {
            if (h == null)
            {
                h = new TTF.App_Code.bpHelber(Page, cFormTypeId, cCustEventTypeId);
               // h = new TTF.App_Code.bpHelber(Page, 8, 136);
                h.lvWP = lvWP;
            }
            DropDownList ddl = (DropDownList)sender;
            ListViewItem lvi = (ListViewItem)ddl.NamingContainer;
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

            // if "(...)" Show CheckBox

            h.spcialActs(ddl, lvi);
        }
        protected void tbOther_PreRender(object sender, System.EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            h.displayIfValue(tb);
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
        protected void lvWP_PreRender(object sender, System.EventArgs e)
        {
            if (lvWP.Items.Count > 0 && hdnItemHandled.Value == string.Empty) lvWP.InsertItemPosition = InsertItemPosition.None;
            if (hdnItemHandled.Value != string.Empty && int.Parse(hdnItemHandled.Value) >= 0) lvWP.EditIndex = 0;
        }
        protected void SaveRep_Click(object sender, System.EventArgs e)
        {
            LinkButton lnkb = (LinkButton)sender;
            lnkb.CausesValidation = false;
            UpdateMainReport(lnkb);
        }
        protected void SaveDRep_Click(object sender, System.EventArgs e)
        {
            LinkButton lnkb = (LinkButton)sender;
            UpdateDetailsReport(lnkb);
        }
        protected void chrt_OnPrePaint(object sender, System.EventArgs e)
        {
            System.Web.UI.DataVisualization.Charting.Chart chrt = (System.Web.UI.DataVisualization.Charting.Chart)sender;
            System.Web.UI.DataVisualization.Charting.Series srs = chrt.Series[0];
            if (srs.Points.Count > 0)
            {
                System.Web.UI.DataVisualization.Charting.DataPoint p = srs.Points[srs.Points.Count - 1];
                p.BackHatchStyle = System.Web.UI.DataVisualization.Charting.ChartHatchStyle.BackwardDiagonal;
                double mx = 0;
                for (int i = 0; i < srs.Points.Count; i++)
                {
                    mx = (mx > srs.Points[i].YValues[0] ? mx : srs.Points[i].YValues[0]);
                }
                for (int i = 0; i < srs.Points.Count; i++)
                {
                    if (srs.Points[i].YValues[0] == mx) srs.Points[i].Color = System.Drawing.Color.Violet;
                }
            }
        }
        protected void lblStart_Prerender(object sender, System.EventArgs e)
        {

            //Label lbl = (Label)lvHdr.Items[0].FindControl("lblStart");
            //string s = lbl.Text;
            //lbl = (Label)sender;
            //lbl.Text = s.Substring(s.Length - 4);

            Label lbl = (Label)lvHdr.Items[0].FindControl("lblStart");
            string s = lbl.Text;
            lbl = (Label)sender;
            if (s != string.Empty)
            {
                lbl.Text = s.Substring(s.Length - 4);
            }

        }

        protected void Graph_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            //  HiddenField hdn = (HiddenField)lvMP.Items[0].FindControl("hdnGraphForm");
            e.Command.Parameters[0].Value = 9394; // u.selectDBScalar("SELECT E127 FROM Book10.dbo.TT_EduMP(50631332,147)");
        }
        protected void ShowFup_Click(object sender, System.EventArgs e)
        {
            LinkButton lnkb = (LinkButton)sender;
            ListViewItem lvi = (ListViewItem)lnkb.NamingContainer;
            ListView lv = (ListView)lvi.FindControl("lvMainReports");
            if (lnkb.Text == "הצג את רשימת המעקבים")
            {
                HiddenField hdn = (HiddenField)lvi.FindControl("hdnWpId");
                DataTable dt = h.getMainReports(int.Parse(hdn.Value));
                lv.DataSource = dt;
                lnkb.Text = "הסתר את רשימת המעקבים";
            }
            else
            {
                lv.DataSource = null;
                lnkb.Text = "הצג את רשימת המעקבים";
            }
            lv.DataBind();
        }

        protected void ShowDetFup_Click(object sender, System.EventArgs e)
        {
            LinkButton lnkb = (LinkButton)sender;
            ListViewItem lvi = (ListViewItem)lnkb.NamingContainer;
            ListView lv = (ListView)lvi.FindControl("lvDetReport");
            if (lnkb.Text == "הצג את רשימת המעקבים")
            {
                HiddenField hdn = (HiddenField)lvi.FindControl("hdnDetId");
                DataTable dt = h.getDetReports(int.Parse(hdn.Value));
                lv.DataSource = dt;
                lnkb.Text = "הסתר את רשימת המעקבים";
            }
            else
            {
                lv.DataSource = null;
                lnkb.Text = "הצג את רשימת המעקבים";
            }
            lv.DataBind();
        }

        protected void backTomainChart_Click(object sender, System.EventArgs e)
        {
            LinkButton lnkb = (LinkButton)sender;
            ListViewItem lvi = (ListViewItem)lnkb.NamingContainer;
            System.Web.UI.DataVisualization.Charting.Chart chrt = (System.Web.UI.DataVisualization.Charting.Chart)lvi.FindControl("Chart1");
            chrt.DataSourceID = null;
            chrt.DataSource = DSGRAPH;
            chrt.DataBind();
            lnkb.Visible = false;
        }

        protected void chrt_Drill(object sender, ImageMapEventArgs e)
        {
            System.Web.UI.DataVisualization.Charting.Chart chrt = (System.Web.UI.DataVisualization.Charting.Chart)sender;
            ListViewItem lvi = (ListViewItem)chrt.NamingContainer;
            LinkButton lnkb = (LinkButton)lvi.FindControl("lnkbBacktoMainChart");

            string s = e.PostBackValue;

            string sql = string.Format("SELECT grp,perc FROM (" +
                                        "SELECT  Ord,txt grp ,a.val perc " +
                                        "FROM Book10_21.dbo.p5t_FormsQuestions q " +
                                        "LEFT OUTER JOIN Book10_21.dbo.p5t_Answers a ON a.QuestionID=q.QuestionID " +
                                        "LEFT OUTER JOIN Book10_21.dbo.p5t_FormQuestionGroups g ON g.gid=q.gid " +
                                        "WHERE ((g.Name = N'{0}') OR (g.ShortName = N'{0}')) AND a.FormID={1} " +
                                        "UNION ALL " +
                                        "SELECT 99999 ord,grp, perc " +
                                        "FROM (SELECT grp,perc " +
                                        "FROM Book10_21.dbo.p5t_Forms f " +
                                        "LEFT OUTER JOIN Book10_21.dbo.p5t_FormResults r ON r.FormID = f.FormID " +
                                        "WHERE f.FormID = {1} AND grp = N'{0}') y) x " +
                                        "Order By Ord", s, 9394);
            DataTable dt = u.selectDBQuesry(sql);
            if (dt.Rows.Count > 1)
            {
                chrt.DataSourceID = null;
                chrt.DataSource = dt;
                System.Web.UI.DataVisualization.Charting.Title ttl = new System.Web.UI.DataVisualization.Charting.Title();
                ttl.Text = s;
                ttl.Font = new System.Drawing.Font("Arial", 16);
                chrt.Titles.Add(ttl);
                lnkb.Visible = true;
            }
            else
            {
                chrt.DataSourceID = null;
                chrt.DataSource = DSGRAPH;
                lnkb.Visible = false;
            }
            chrt.DataBind();
        }
        protected void UpdateMainReport(LinkButton lnkb)
        {
            ListViewItem lvi = (ListViewItem)lnkb.NamingContainer;
            ListView lv = (ListView)lvi.NamingContainer;
            HiddenField hdn = (HiddenField)lvi.FindControl("hdnWpId");
            h.WpId = int.Parse(hdn.Value);
            h.DetailsId = 0;
            string sql = h.BuildReportHeader();

            sql += h.BuildReport(lvi, "rbl1", string.Empty, 1);
            sql += h.BuildReport(lvi, "rbl2", string.Empty, 2);
            sql += h.BuildReport(lvi, "rbl3", string.Empty, 3);

            bool b = h.updateReport(sql);
            lv.DataBind();

        }
        protected void UpdateDetailsReport(LinkButton lnkb)
        {
            ListViewItem lvi = (ListViewItem)lnkb.NamingContainer;
            HiddenField hdn = (HiddenField)lvi.FindControl("hdnDetId");
            h.DetailsId = int.Parse(hdn.Value);

            ListView lv = (ListView)lvi.NamingContainer;
            ListViewItem lviO = (ListViewItem)lv.NamingContainer;
            hdn = (HiddenField)lviO.FindControl("hdnWpId");
            h.WpId = int.Parse(hdn.Value);

            string sql = h.BuildReportHeader();

            sql += h.BuildReport(lvi, "rblFUDet", "tbFuT", 4);

            bool b = h.updateReport(sql);

        }
        protected void lnkbMainSave_Click(object sender, System.EventArgs e)
        {

        }
        protected void lnkbMainDelete_Click(object sender, System.EventArgs e)
        {
            LinkButton lnkb = (LinkButton)sender;
            ListViewItem lvi = (ListViewItem)lnkb.NamingContainer;
            ListView lv = (ListView)lvi.NamingContainer;
            HiddenField hdn = (HiddenField)lvi.FindControl("hdnRepId");
            h.DeleteMainReport(int.Parse(hdn.Value));
            lvi.Visible = false;

        }

        protected void lnkbEdit_Click(object sender, EventArgs e)
        {
            LinkButton lnkb = (LinkButton)sender;
            ListViewItem lvi = (ListViewItem)lnkb.NamingContainer;
            ListView lv = (ListView)lvi.NamingContainer;
            lv.EditIndex = lvi.DisplayIndex;
            lvi = lv.EditItem;
            RadioButtonList rbl = (RadioButtonList)lvi.FindControl("rbl1");
            rbl.SelectedValue = "1";
        }

        protected void lnkbMainCancel_Click(object sender, EventArgs e)
        {
            LinkButton lnkb = (LinkButton)sender;
            ListViewItem lvi = (ListViewItem)lnkb.NamingContainer;
            ListView lv = (ListView)lvi.NamingContainer;
            lv.EditIndex = -1;
        }

        protected void lblDxxx_PreRnder(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lbl = (Label)sender;
                int i = int.Parse(lbl.ID.Substring(4));
                if (i == 127)
                {
                    lbl.Text = hdnDate127.Value;
                }
                else
                {
                    DataTable dt = h.p5Details(i);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow r = dt.Rows[0];
                        DateTime d = (DateTime)r["CustEventDate"];
                        string s = string.Format("{0:dd/MM/yyyy}", d);
                        if (s != "01/01/1900") lbl.Text = s;
                    }
                }
            }
        }

        protected void lblAge_PreRender(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lbl = (Label)sender;
                lbl.Text = h.CustomerAge();
            }
        }
        protected string reDir(string prog, string idField)
        {
            string s = prog + string.Format("?RId={0}&CID={1}&E={2}&F={3}&Id={4}", Eval(idField), h.CustomerId, h.UserId * 62, h.Frm_CatId, h.EventId);
            return s;
        }

        protected void hlprint_PreRender(object sender, EventArgs e)
        {
            HyperLink hl = (HyperLink)sender;
            int i = h.UserId * 62;
            string s = string.Format("TTeup_p.aspx?CID={0}&F={1}&E={2}&ID={3}", h.CustomerId, h.Frm_CatId, i, h.EventId); // + (Request.QueryString["ID"] != null ? string.Empty : "id+" + Request.QueryString["ID"].ToString());
            hl.NavigateUrl = s;
        }
        protected void DS_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            if (h.EventId==0)
            {
                e.Command.Parameters["@EventId"].Value = h.getMPEventId();
            }
            //e.Command.Parameters["@EventId"].Value = h.EventId;
           // e.Command.Parameters["@EventId"].Value = h.MPEventId;
           
        }
        protected bool isUpdatable()
        {
            return h.Isupdatable();
        }
        protected void lblhreport_PreRender(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            string s = string.Empty;
            if (isUpdatable())
            {
                s = "<b>מעקב</b></b><br /><span style='font-size: smaller'>עמידה ביעד</span>";
            }
            lbl.Text = s;

        }

        protected void DSWP_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@EventId"].Value = h.MPEventId;
            e.Command.Parameters["@EventTypeId"].Value = h.CustEventTypeId;
        }
    }
}