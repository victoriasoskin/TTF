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
    public partial class TTewp : System.Web.UI.Page
    {
        TTF.App_Code.bpHelber h;
        TTF.App_Code.Util u;
        int cCustEventTypeId = 0;
        int cFormTypeId = 0;
        int EduNewForm = 0;

        #region load

        protected void Page_PreInit(object sender, EventArgs e)
        {
            cCustEventTypeId = int.Parse(Request.QueryString["ET"]);
            cFormTypeId = int.Parse(Request.QueryString["FT"]);
            u = new TTF.App_Code.Util();
            h = new TTF.App_Code.bpHelber(Page, cFormTypeId, cCustEventTypeId);
            h.lvMP = lvMP;
            h.lvWP = lvWP;
            h.LVWeeklyPlan = LVWeeklyPlan;
            h.hdnCurrentEventId = hdnCurrentEventId;
            EduNewForm = h.GetEduNewForm(cCustEventTypeId);
        }

        public TTewp()
        {
            //u = new TTF.App_Code.Util();
            //h = new TTF.App_Code.bpHelber(Page, cFormTypeId, cCustEventTypeId);
            //h.lvMP = lvMP;
            //h.lvWP = lvWP;
            //h.LVWeeklyPlan = LVWeeklyPlan;
            //h.hdnCurrentEventId = hdnCurrentEventId;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            h.LVWeeklyPlan = LVWeeklyPlan;
            h.divWeeklytimetable = divWeeklytimetable;
            h.lvWP = lvWP;
            
            if (!IsPostBack)
            {
                //h.initForm();
                h.initForm(cFormTypeId);
                DataTable dt = h.p5Details(EduNewForm);
                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    DateTime d = (DateTime)r["CustEventDate"];
                    string s = string.Format("{0:dd/MM/yyyy}", d);
                    if (s != "01/01/1900") hdnDate127.Value = s;
                    hdnForm127.Value = r["CustRelateId"].ToString();
                    dt.Dispose();
                }

                lvHdr.DataBind();
            }
            h.selectAction();

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
            if (lvWP.Items.Count > 0)
            {
                LinkButton lnkb1 = (LinkButton)lvWP.Items[0].FindControl("lnkbEdit");
                lnkb.Visible = lvMP.InsertItemPosition == InsertItemPosition.None && (lnkb1 == null ? true : lnkb1.Visible);
            }
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
                hdnCurrentWpId.Value = hdn.Value;
            }
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
        }

        protected void lnkbMP_Click(object sender, System.EventArgs e)
        {
            LinkButton lnkb = (LinkButton)sender;
            ListViewItem lvi = (ListViewItem)lnkb.NamingContainer;
            ListView lv = (ListView)lvi.NamingContainer;
            h.UpdateMp(lvi);
            lv.DataBind();
            hdnItemHandled.Value = null;
            if (lv.EditIndex >= 0) lv.EditIndex = -1;
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
            int res = 0;
            string sql = string.Format("select eventId FROM [Book10].[dbo].[TT_WP_Details] where WPID = {0}", hdn.Value);
            try
            {
                res = int.Parse(u.selectDBScalar(sql).ToString());
            }
            catch (Exception)
            {

                res = 0;
            }
            
            if (res == 0)
            {
                int id = (hdn.Value == string.Empty ? 0 : int.Parse(hdn.Value));
                if (dl.InsertItem != null)
                    if (id == 0) dl.InsertItemPosition = InsertItemPosition.None;
                    else dl.InsertItemPosition = InsertItemPosition.FirstItem;
                h.getDetails(id, dl);
            }
            else
            {
                int id = (hdn.Value == string.Empty ? 0 : int.Parse(hdn.Value));
                if (dl.InsertItem != null)
                    if (id == 0) dl.InsertItemPosition = InsertItemPosition.None;
                    else dl.InsertItemPosition = InsertItemPosition.None;
                h.getDetails(id, dl);
            }
            
        }
        protected void ddl_DataBinding(object sender, EventArgs e)
        {
            if (h == null)
            {
                h = new TTF.App_Code.bpHelber(Page, cFormTypeId, cCustEventTypeId);
                h.lvWP = lvWP;
            }
            DropDownList ddl = (DropDownList)sender;
            ListViewItem lvi = (ListViewItem)ddl.NamingContainer;
            h.SetSelectList(lvi, ddl);
        }

        /// <summary>
        /// This method checks if we already have details for the workplan. 
        /// if not, it lets the theacher  to insert the details.
        /// if there are already filled details , the the teacher can only edit the existind details. 
        /// The method limits the teacher of inserthing more than one set of details.
        /// Udi asked for this feature  in the meeting 18.6.14
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_PreRender(object sender, System.EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            ListViewItem lvi = (ListViewItem)ddl.NamingContainer;
            if (ddl.SelectedValue == string.Empty)
            {
                if (((ListView)lvi.NamingContainer).EditIndex >= 0)
                {
                    HiddenField hdn = (HiddenField)lvi.FindControl("hdn" + ddl.ID.Substring(3));
                    if (hdn != null)
                    {
                        ListItem li = ddl.Items.FindByValue(hdn.Value);
                        if (li != null)
                        {
                            ddl.ClearSelection();
                            li.Selected = true;
                        }
                    }
                }
            }
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
            //       h = new TTF.App_Code.bpHelber(Page, lvWP, cFormTypeId);
            ListView lv = (ListView)sender;
            ListViewItem lvi = (ListViewItem)lv.NamingContainer;
            //TextBox tb = (TextBox)lv.FindControl("tbDetails");
            //if (tb.Text.Length > 50)
            //{
            //    OpenPopUP("הטקסט ארוך מדי");
            //}

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
            if (lvMP.Items.Count > 0)
            {
                if (lvWP.Items.Count == 0)
                    lvWP.InsertItemPosition = InsertItemPosition.FirstItem;
                else
                    if (hdnItemHandled.Value == string.Empty) lvWP.InsertItemPosition = InsertItemPosition.None;
            }
            else
                lvWP.InsertItemPosition = InsertItemPosition.None;

            if (hdnItemHandled.Value != string.Empty && int.Parse(hdnItemHandled.Value) >= 0) lvWP.EditIndex = 0;
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

            Label lbl = (Label)lvHdr.Items[0].FindControl("lblStart");
            string s = lbl.Text;
            lbl = (Label)sender;
            if (s != string.Empty)
            {
                lbl.Text = s.Substring(s.Length - 4);
            }

        }

        protected void lblDxxx_PreRnder(object sender, System.EventArgs e)
        {
            Label lbl = (Label)sender;
            if (lbl.Text == string.Empty)
            {
                int i = int.Parse(lbl.ID.Substring(4));
                if (i == 173)
                //if (i == 127)
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
            Label lbl = (Label)sender;
            if (lbl.Text == string.Empty)
            {
                lbl.Text = h.CustomerAge();
            }
        }
        protected void lnkbDetailsSave_Click(object sender, System.EventArgs e)
        {

        }
        protected void chrt_Drill(object sender, ImageMapEventArgs e)
        {
            System.Web.UI.DataVisualization.Charting.Chart chrt = (System.Web.UI.DataVisualization.Charting.Chart)sender;
            ListViewItem lvi = (ListViewItem)chrt.NamingContainer;
            LinkButton lnkb = (LinkButton)lvi.FindControl("lnkbBacktoMainChart");

            string s = e.PostBackValue;

            string sql = string.Format("SELECT grp,perc FROM (" +
                                        "SELECT  Ord,txt grp ,a.val perc " +
                                        "FROM (select * from Book10_21.dbo.p5t_FormsQuestions where eventtypeid={2}) q " +
                                        "LEFT OUTER JOIN (select * from Book10_21.dbo.p5t_Answers where FormId={1})  a ON a.QuestionID=q.QuestionID " +
                                        "LEFT OUTER JOIN Book10_21.dbo.p5t_FormQuestionGroups g ON g.gid=q.gid " +
                                        "WHERE ((g.Name = N'{0}') OR (g.ShortName = N'{0}')) AND a.FormID={1} " +
                                        "UNION ALL " +
                                        "SELECT 99999 ord,grp, perc " +
                                        "FROM (SELECT grp,perc " +
                                        "FROM Book10_21.dbo.p5t_Forms f " +
                                        "LEFT OUTER JOIN Book10_21.dbo.p5t_FormResults r ON r.FormID = f.FormID " +
                                        "WHERE f.FormID = {1} AND grp = N'{0}') y) x " +
                                        "Order By Ord", s, hdnForm127.Value, EduNewForm);
            DataTable dt = u.selectDBQuesry(sql);


//            string sql1=string.Format("SELECT grp,perc "+
//"FROM (SELECT  Ord,txt grp ,a.val perc "+
//        "FROM  (select * from Book10_21.dbo.p5t_Answers where FormId=20316)  a   "+
//        "LEFT OUTER JOIN  (select * from Book10_21.dbo.p5t_FormsQuestions where eventtypeid={2}) q ON a.QuestionID=q.QuestionID "+
//        "LEFT OUTER JOIN Book10_21.dbo.p5t_FormQuestionGroups g ON g.gid=q.gid "+
//        "WHERE ((g.Name = N'חברתי') OR (g.ShortName = N'חברתי')) AND a.FormID={1} "+
//        "UNION ALL "+
//        "SELECT 99999 ord,grp, perc "+
//        "FROM (SELECT grp,perc FROM Book10_21.dbo.p5t_Forms f "+
//        "LEFT OUTER JOIN Book10_21.dbo.p5t_FormResults r ON r.FormID = f.FormID WHERE f.FormID = 20316 AND grp = N'חברתי') y) x "+
//"Order By Ord", s, hdnForm127.Value,h.CustEventTypeId);
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

        protected void lvWP_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            ListView lv = (ListView)sender;

            h.DeleteWp(lv.Items[e.ItemIndex]);
            e.Cancel = true;
            lv.DataBind();
            if (lv.Items.Count == 0)
            {
                lv.InsertItemPosition = InsertItemPosition.FirstItem;
            }
        }

        protected void dlDetails_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            ListView lv = (ListView)sender;

            h.DeleteWp(lv.Items[e.ItemIndex]);
            e.Cancel = true;
            lv.DataBind();
            if (lv.Items.Count == 0)
            {
                lv.InsertItemPosition = InsertItemPosition.FirstItem;
            }
        }

        protected void lnkbDelete_Click(object sender, EventArgs e)
        {

            LinkButton lnkb = (LinkButton)sender;
            ListViewItem lvi = (ListViewItem)lnkb.NamingContainer;
            ListView lv = (ListView)lvi.NamingContainer;

            h.DeleteDetails(lvi);
            lv.DataBind();


        }
        protected string WW(int i)
        {
            string s = ((char)160).ToString();

            if (Eval("P" + i.ToString()) == System.DBNull.Value)
            {
                if (Eval("C" + i.ToString()) != System.DBNull.Value)
                {
                    s += Eval("C" + i.ToString());
                }
            }
            else
            {
                {
                    s = Eval("P" + i.ToString()).ToString();

                    if (Eval("C" + i.ToString()) != System.DBNull.Value)
                    {
                        s += "<br />" + Eval("C" + i.ToString());
                    }
                }
            }
            return s;
        }

        protected string OpenPopUP(string s)
        {
            return string.Format("OpenPopUP('{0}'); return false;", s);
        }

        protected void hl_Click(object sender, EventArgs e)
        {

        }

        protected void lnkbSave_PreRender(object sender, EventArgs e)
        {
            LinkButton lnkb = (LinkButton)sender;
            TextBox tb = (TextBox)lnkb.FindControl("tbDetails");
            /*check the lenght of the textboxes*/
            if (tb.Text.Length > 50)
            {
                OpenPopUP("הטקסט ארוך מדי");
            }
            lnkb.Enabled = hdnCurrentWpId.Value != string.Empty;
        }

        protected void dlDetails_DataBinding(object sender, EventArgs e)
        {

        }
        protected string delWeek(string s)
        {
            return "delweek(" + Eval(s).ToString() + ");";
        }
        protected string DB(string s)
        {
            string sX = string.Empty;
            if (Eval(s) != DBNull.Value)
            {
                sX = Eval(s).ToString();
                //       sX = "<input type='button' value='x' class='delbtn' onclick=\"delweek('" + sX + "');\" />";
                sX = "<input type='button' value='x' class='delbtn' onclick=\"delweek('" + sX + "',this);\" />";
            }
            return sX;
        }

        protected void hlprint_PreRender(object sender, EventArgs e)
        {
            HyperLink hl = (HyperLink)sender;
            int i = h.UserId * 62;
            string s = string.Format("TTewp_p.aspx?CID={0}&F={1}&E={2}&ID={3}&FT={4}&ET={5}", h.CustomerId, h.Frm_CatId, i, h.EventId,cFormTypeId,cCustEventTypeId); // + (Request.QueryString["ID"] != null ? string.Empty : "id+" + Request.QueryString["ID"].ToString());
            hl.NavigateUrl = s;
        }

        protected void DSWeeklyPlan_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters[0].Value = h.EventId;
        }

        protected void DS_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@EventId"].Value = h.EventId;
            try
            {
                e.Command.Parameters["@FormTypeId"].Value = h.FormTypeId;
            }
            catch (Exception ex) { }
            try
            {
                e.Command.Parameters["@NewFormTypeId"].Value = EduNewForm;
            }

            catch (Exception ex) { }

        }
        protected bool isUpdatable()
        {
            return h.Isupdatable();
        }

        protected void DSGRAPH_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            string s = DSGRAPH.SelectCommand;
        }      

       

        protected void printReka_PreRender(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int i = h.UserId * 62;
            string s = string.Format("window.open('TTewp_r.aspx?CID={0}&F={1}&E={2}&ID={3}&FT={4}&ET={5}','_blank')", h.CustomerId, h.Frm_CatId, i, h.EventId, cFormTypeId, cCustEventTypeId); // + (Request.QueryString["ID"] != null ? string.Empty : "id+" + Request.QueryString["ID"].ToString());
            btn.Attributes.Add("onclick", s);
        }
    }
}
