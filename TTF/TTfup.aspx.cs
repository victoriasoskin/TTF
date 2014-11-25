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
    public partial class TTfup : System.Web.UI.Page
    {
        TTF.App_Code.bpHelber h;
        TTF.App_Code.Util u;
        TTF.App_Code.TT_WP_Reports r;
        int cCustEventTypeId = 0;
        int cFormTypeId = 0;
        #region load

        public TTfup()
        {
           // u = new TTF.App_Code.Util();
            //h = new TTF.App_Code.bpHelber(Page, 4, 132);
           // h.lvMP = lvMP;
           // u = new TTF.App_Code.Util();
            //r = new TTF.App_Code.TT_WP_Reports();
            //h.hdnCurrentEventId = hdnCurrentEventId;
            //h.lvWP = lvWP;
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            cCustEventTypeId = int.Parse(Request.QueryString["ET"]);
            cFormTypeId = int.Parse(Request.QueryString["FT"]);

            u = new TTF.App_Code.Util();

            h = new TTF.App_Code.bpHelber(Page, cFormTypeId, cCustEventTypeId);
            h.lvMP = lvMP;
            h.lvWP = lvWP;
            r = new TTF.App_Code.TT_WP_Reports();
           // h.LVWeeklyPlan = LVWeeklyPlan;
            h.hdnCurrentEventId = hdnCurrentEventId;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            u = new App_Code.Util();
            Session["UserId"] = getUserId();
            h.lvWP = lvWP;
          //  DSWP.SelectParameters["FormTypeId"].DefaultValue = u.selectDBScalar(string.Format("select MasterFormTypeId from book10_21.dbo.TT_FormTypes where id = {0}",cFormTypeId)).ToString();
            if (!IsPostBack)
            {
                h.initForm();
                DataTable dt = h.p5Details(cCustEventTypeId);
                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    DateTime d = (DateTime)r["CustEventDate"];
                    string s = string.Format("{0:dd/MM/yyyy}", d);
                    if (s != "01/01/1900") hdnDate132.Value = s;
                    hdnForm132.Value = r["CustRelateId"].ToString();
                    dt.Dispose();
                }

                h.selectAction();

                lvHdr.DataBind();
                Chart1.DataBind();
            }
        }

        public int getUserId()
        {

            int i = (HttpContext.Current.Request.QueryString["E"] == null ? 0 * 62 : int.Parse(HttpContext.Current.Request.QueryString["E"].ToString()));
            // if (i == 0) HttpContext.Current.Response.Redirect("~/CustEventReport.Aspx");
            i = i / 62;
            return i;
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

            //ddl = (DropDownList)lvi.FindControl("lblIndexName");
            //if (ddl != null)
            //{
            //    if (ddl.SelectedValue != string.Empty)
            //    {
            //        TargetId = int.Parse(ddl.SelectedValue);
            //    }
            //}
            //else
            //{
            //    hdn = (HiddenField)lvi.FindControl("hdnIndexId");
            //    if (hdn != null)
            //    {
            //        if (hdn.Value != string.Empty)
            //        {
            //            TargetId = int.Parse(hdn.Value);
            //        }
            //    }
            //}



            ////////////if (indexId == (int)u.selectDBScalar(string.Format("SELECT IndexId FROM B10Sec.dbo.TT_Class_AdditionalColumns WHERE ClassId={0}", TargetId)))
            ////////////{
            ////////////    lbl.BorderWidth = 2;
            ////////////    lbl.BorderColor = System.Drawing.Color.Aqua;
            ////////////}
            ////////////else
            ////////////{
            ////////////    lbl.BorderWidth = 0;
            ////////////    lbl.BorderColor = System.Drawing.Color.Transparent;
            ////////////}
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
                h = new TTF.App_Code.bpHelber(Page, 4, 132);
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

        protected void lvIdexes_PreRender(object sender, System.EventArgs e)
        {
            ListView lv = (ListView)sender;
            ListViewItem lvi = (ListViewItem)lv.NamingContainer;
            HiddenField hdn = (HiddenField)lvi.FindControl("hdnWPId");
            int id = (hdn.Value == string.Empty ? 0 : int.Parse(hdn.Value));
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
            System.Web.UI.DataVisualization.Charting.Chart chrt1;

        }
        protected void lblStart_Prerender(object sender, System.EventArgs e)
        {

            Label lbl = (Label)lvHdr.Items[0].FindControl("lblStart");
            string s = lbl.Text;
            lbl = (Label)sender;
            lbl.Text = s.Substring(s.Length - 4);

        }

        protected void Graph_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
           
           // e.Command.Parameters[0].Value = 9394; 
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
        /// <summary>
        /// בפונקציה זו חוזרים מהגרף המפורט לגרף הממוצעים
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void backTomainChart_Click(object sender, System.EventArgs e)
        {
            Button lnkb = (Button)sender;
            System.Web.UI.DataVisualization.Charting.Chart chrt = (System.Web.UI.DataVisualization.Charting.Chart)lnkb.FindControl("Chart1");
            chrt.DataSourceID = null;
          
            chrt.DataSource = DSGRAPH;
            chrt.Series[0].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
            chrt.DataBind();
            lnkb.Visible = false;
        }

        protected void chrt_Drill(object sender, ImageMapEventArgs e)
        {
            System.Web.UI.DataVisualization.Charting.Chart chrt = (System.Web.UI.DataVisualization.Charting.Chart)sender;
            System.Web.UI.DataVisualization.Charting.Chart chrt1;
            chrt1 = (System.Web.UI.DataVisualization.Charting.Chart)chrt.FindControl("Chart1");
            Button lnkb = (Button)chrt.FindControl("lnkbBacktoMainChart");
            string s = e.PostBackValue;
            chrt1.Series[0].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
            chrt1.Series[0].XValueMember = "m";
            chrt1.Series[0].YValueMembers = "av";
            string sql = string.Format("select wpr.Val av , wpr.LoadTime m from [Book10].[dbo].[TT_WP_Reports] wpr " +
            "inner join (select * from book10.dbo.TT_Forms where formtypeId = {2}) w on w.Id= wpr.FormId " +
            "where (wpr.CustomerId = {0}) and  book10.dbo.Month_Number_To_Word(month(wpr.LoadTime)) = N'{1}' and status =0 order by wpr.LoadTime ", h.CustomerId, s, cFormTypeId);
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
                if (i == 132)
                {
                    lbl.Text = hdnDate132.Value;
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
            string s = prog + string.Format("?RId={0}&CID={1}&E={2}&F={3}&Id={4}&ET={5}&FT={6}", Eval(idField), h.CustomerId, h.UserId * 62, h.Frm_CatId, h.EventId,cCustEventTypeId,cFormTypeId);
            return s;
        }

        protected void hlprint_PreRender(object sender, EventArgs e)
        {
            HyperLink hl = (HyperLink)sender;
            int i = h.UserId * 62;
            string s = string.Format("TTfup_p.aspx?CID={0}&F={1}&E={2}&ID={3}&ET={4}&FT={5}", h.CustomerId, h.Frm_CatId, i, h.EventId,cCustEventTypeId,cFormTypeId); // + (Request.QueryString["ID"] != null ? string.Empty : "id+" + Request.QueryString["ID"].ToString());
            hl.NavigateUrl = s;
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
        /* הפונקציה גורמת לכך שנוכל לבחור רק רדיו באטן אחד 
         בגלל שרשימת כפתורי הרדיו נמצאת בתיך ליסט ויו , כל רדיו מקבל שם אחד  ונוצר מצב שאפשר לבחור כמה  . 
         הפונקציה הזו מבטלת את זה, נותנת לכל הרדיוים שם אחיד */
        protected void rb_PreRender(object sender, EventArgs e)
        {
            //RadioButton rb = (RadioButton)sender;
            //ListViewItem lvi = (ListViewItem)rb.NamingContainer;
            //int p = lvi.DisplayIndex;
            //string s = rb.ClientID;
            //try
            //{
            //    int s1 = int.Parse(s.Substring(s.Length - 4,1));
            //    s = s.Substring(0, (s.Length - 1));
            //    string temp = "";
            //    for (int i = 0; i < 5; i++)
            //    {
            //        if (i != s1)
            //        {
            //            temp += string.Format("$('#{0}{1}').prop('checked', false);", s, i);
            //        }
            //        rb.Attributes.Add("onChange", temp);
            //    }

            //}
            //catch (Exception ex) { }
        }
        // שמירה של מעקב התמיכות. בנוסף לעדכון בסיס הנתונים גם מרפרש את התת גרף ומוסיף לתת הגרף את הנתונים החדשים ששמרנו עתה.
        protected void lnkbSaveRep_Click(object sender, EventArgs e)
        {
            LinkButton lnkb = (LinkButton)sender;
            ListViewItem lvi = (ListViewItem)lnkb.NamingContainer;
            ListView lv = (ListView)lvi.NamingContainer;
            int res = 0;
            h.WpId = h.getWpId(lvi.DisplayIndex);
            TextBox temp = (TextBox)lvi.FindControl("TextBXDetReport");
            string str = temp.Text;

            ListView lv_rb = (ListView)lvi.FindControl("lvIndexes");
            foreach (ListViewItem lvk in lv_rb.Items)
            {
                RadioButton r = (RadioButton)lvk.FindControl("rb");
                bool status = r.Checked;
                if (status)
                {
                    Label result = (Label)lvk.FindControl("lblIndexName");
                    res = int.Parse(result.Text);
                    break;
                }
            }
            h.InsertToDB(h.WpId, lvi, res, str);
            RefreshGraph(h.WpId, h.EventId, sender);

        }
        //פרה-רנדר לתתי הגרפים של המעקבים
        protected void MiniGraph_PreRender(object sender, EventArgs e)
        {
            System.Web.UI.DataVisualization.Charting.Chart chrt = (System.Web.UI.DataVisualization.Charting.Chart)sender;
            System.Web.UI.DataVisualization.Charting.Chart chrt1;
            ListViewItem lvi = (ListViewItem)chrt.NamingContainer;
            System.Web.UI.DataVisualization.Charting.Series srs = chrt.Series[0];
            chrt1 = (System.Web.UI.DataVisualization.Charting.Chart)lvi.FindControl("Chart2");
            string cmnd = DSMiniGraph.SelectCommand;
            int e1 = h.EventId;
            h.WpId = h.getWpId(lvi.DisplayIndex);
            cmnd = cmnd.Replace("@WpId", h.WpId.ToString()).Replace("@EventId", e1.ToString());
            DataTable dt = u.selectDBQuesry(cmnd);
            chrt1.DataSource = dt;
            chrt1.DataBind();
            string x = chrt1.Series[0].XValueMember;
            string y = chrt1.Series[0].YValueMembers;
            //chrt1.DataBind();
        }
        //מרפרש את הגרף של מעקב התמיכות כאשר לוחצים על  שמירה של מעקב ומעדכן אותו עם הנתונים החדשים
        protected void RefreshGraph(int WpId1, int EventId1, object sender)
        {

            LinkButton lnkb = (LinkButton)sender;
            ListViewItem lvi = (ListViewItem)lnkb.NamingContainer;
            System.Web.UI.DataVisualization.Charting.Chart chrt1 = (System.Web.UI.DataVisualization.Charting.Chart)lvi.FindControl("Chart2");
            System.Web.UI.DataVisualization.Charting.Series srs = chrt1.Series[0];
            string cmnd = DSMiniGraph.SelectCommand;
            h.WpId = h.getWpId(lvi.DisplayIndex);

            cmnd = cmnd.Replace("@WpId", WpId1.ToString()).Replace("@EventId", EventId1.ToString());
            DataTable dt = u.selectDBQuesry(cmnd);
            chrt1.DataSource = dt;
            chrt1.DataBind();
            string x = chrt1.Series[0].XValueMember;
            string y = chrt1.Series[0].YValueMembers;
        }

        protected void DSGRAPH_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            string s = e.Command.CommandText;
            e.Command.Parameters[0].Value = h.CustomerId;
            e.Command.Parameters["@FormTypeId"].Value = cFormTypeId;
        }

        protected void DS_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
                       e.Command.Parameters["@EventId"].Value = h.getMasterEventId();
            
        }

        protected void hlDetRep_Click(object sender, EventArgs e)
        {
            //Button btn = (Button)sender;
            //ListViewItem lvi = (ListViewItem)btn.NamingContainer;

            //int id = h.getWpId(lvi.DisplayIndex);
            //string res = string.Format("window.open('TTdfupList.aspx?Rid={0}&ET={1}&FT={2}&E={3}','_blank')", id, cCustEventTypeId, cFormTypeId,getUserId());
            
            //Response.Redirect(res);
        }
        

        protected void DSWP_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@EventId"].Value = h.getMasterEventId();
            e.Command.Parameters["@FormTypeId"].Value = u.selectDBScalar(string.Format("select MasterFormTypeId from book10_21.dbo.TT_FormTypes where CustEventTypeId = {0}", cCustEventTypeId)).ToString();

        }

        protected void hlDetRep_PreRender(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            ListViewItem lvi = (ListViewItem)btn.NamingContainer;

            int id = h.getWpId(lvi.DisplayIndex);
       //     string res = string.Format("window.open('TTdfupList.aspx?Rid={0}&ET={1}&FT={2}&E={3}','_blank')", id, cCustEventTypeId, cFormTypeId, getUserId());
            string res = string.Format("window.open('TTdfupList.aspx?Rid={0}&ET={1}&FT={2}&E={3}&CID={4}&F={3}&Id={4}','_blank')", id, cCustEventTypeId, cFormTypeId, 51026, h.CustomerId, h.Frm_CatId, h.EventId);

            btn.Attributes.Add("onclick", res);
        }


    }
}


