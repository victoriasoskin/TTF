using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TTF.App_Code;
using System.Web.UI.HtmlControls;

namespace TTF
{
    public partial class TTbwp : System.Web.UI.Page
    {
        TTF.App_Code.bpHelber h;
        TTF.App_Code.Util u;
        //const int cCustEventTypeId = 131;
        //const int cFormTypeId = 3;
        int cCustEventTypeId = 0;
        int cFormTypeId = 0;
        #region load

        public TTbwp()
        {
            //cCustEventTypeId = int.Parse(Request.QueryString["ET"]);
            //cFormTypeId = int.Parse(Request.QueryString["FT"]);

            //u = new TTF.App_Code.Util();

            //h = new TTF.App_Code.bpHelber(Page, cFormTypeId, cCustEventTypeId);
            //h.lvMP = lvMP;
            //h.lvWP = lvWP;
            //h.LVWeeklyPlan = LVWeeklyPlan;
            //h.hdnCurrentEventId = hdnCurrentEventd;
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            cCustEventTypeId = int.Parse(Request.QueryString["ET"]);
            cFormTypeId = int.Parse(Request.QueryString["FT"]);

            u = new TTF.App_Code.Util();  

            h = new TTF.App_Code.bpHelber(Page, cFormTypeId, cCustEventTypeId);
          //  h.lvMP = lvMP;
            h.lvWP = lvWP;
            h.LVWeeklyPlan = LVWeeklyPlan;
            h.hdnCurrentEventId = hdnCurrentEventId;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            Session["UserId"] = getUserId();
            h.LVWeeklyPlan = LVWeeklyPlan;
            h.divWeeklytimetable = divWeeklytimetable;
            h.lvWP = lvWP;
            Session["UserId"] = getUserId();
            if (!IsPostBack)
            {
                if (cFormTypeId==13) //if דיור תפקוד נמוך ובית ספר
                {
                    h.initForm(13);
                }
                else
                    h.initForm();
                DataTable dt = h.p5Details(139);
                //DataTable dt = h.p5Details(cCustEventTypeId);
                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    DateTime d = (DateTime)r["CustEventDate"];
                    string s = string.Format("{0:dd/MM/yyyy}", d);
                    if (s != "01/01/1900") hdnDate139.Value = s;
                    hdnForm139.Value = r["CustRelateId"].ToString();
                    dt.Dispose();
                }

                lvHdr.DataBind();
            }
            h.selectAction();

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
            Button lnkb = (Button)sender;
            if (lvWP.Items.Count > 0)
            {
                Button lnkb1 = (Button)lvWP.Items[0].FindControl("lnkbEdit");
                lnkb.Visible = (lnkb1 == null ? true : lnkb1.Visible);
            }
        }

        protected void lnkbShowAdd_Click(object sender, System.EventArgs e)
        {
            Button lnkb = (Button)sender;
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
            if (lv.ID == "dlIndexes")
            {
                lv.EditIndex = 0;
            }
        }

        protected void lv_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            ListView lv = (ListView)sender;
            lv.EditIndex = -1;
        }

        protected void lnkbWP_Click(object sender, System.EventArgs e)
        {
            Button lnkb = (Button)sender;
            ListViewItem lvi = (ListViewItem)lnkb.NamingContainer;
            ListView lv = (ListView)lvi.NamingContainer;
            h.WpId = h.UpdateWp(lvi);
            //h.InsertFirstTimeIndexes(h.WpId, lvi);  // מוסיף את האינדקסים לבסיס הנתונים

            if (lnkb.Text != "שמירה והוספת פירוט") //שמירה וסגירה
            {
                ((ListView)lvi.NamingContainer).DataBind();

                hdnItemHandled.Value = null;
                if (lv.EditIndex >= 0)
                    lv.EditIndex = -1;
                h.InsertFirstTimeIndexes(h.WpId, lvi);  // מוסיף את האינדקסים לבסיס הנתונים
                //  h.InsertDetails(h.WpId, lvi);
            }
            else //שמירה והוספת פירוט
            {
                hdnCurrentWpId.Value = h.WpId.ToString();
                h.InsertFirstTimeIndexes(h.WpId, lvi);  // מוסיף את האינדקסים לבסיס הנתונים

            }
        }

        protected void InsertDetails(object sender, System.EventArgs e)
        {
            Button lnkb = (Button)sender;
            ListViewItem lvi = (ListViewItem)lnkb.NamingContainer;
            ListView lv = (ListView)lvi.NamingContainer;
            ListViewItem lvi1 = (ListViewItem)lv.NamingContainer;
            h.WpId = h.getWpId(lvi1.DisplayIndex);
            hdnCurrentWpId.Value = h.WpId.ToString();

            h.InsertDetailsInHelper(h.WpId, lvi);
        }

        protected void lnkbMP_Click(object sender, System.EventArgs e)
        {
            Button lnkb = (Button)sender;
            ListViewItem lvi = (ListViewItem)lnkb.NamingContainer;
            ListView lv = (ListView)lvi.NamingContainer;
            h.UpdateMp(lvi);
            lv.DataBind();
            hdnItemHandled.Value = null;
            if (lv.EditIndex >= 0) lv.EditIndex = -1;
        }

        protected void lnkbCancel_Click(object sender, System.EventArgs e)
        {
            Button lnkb = (Button)sender;
            ListViewItem lvi = (ListViewItem)lnkb.NamingContainer;
            ListView lv = (ListView)lnkb.NamingContainer.NamingContainer;
            if (lvi.ItemType == ListViewItemType.InsertItem)
            {
                lv.InsertItemPosition = InsertItemPosition.None;
                hdnItemHandled.Value = null;
            }
        }
        #endregion

        #endregion

        #region lvW
        protected void lvMP_PreRender(object sender, System.EventArgs e)
        {
            //if (lvMP.Items.Count > 0 && hdnItemHandled.Value == string.Empty) lvMP.InsertItemPosition = InsertItemPosition.None;
            //if (hdnItemHandled.Value != string.Empty && int.Parse(hdnItemHandled.Value) >= 0) lvMP.EditIndex = 0;
        }

        #endregion

        protected void lblIndexTxt_PreRender(object sender, System.EventArgs e) // gets the text of indexes in the support
        {
            Label lbl = (Label)sender;
            ListViewItem lvi = (ListViewItem)lbl.NamingContainer;
            int TempEventId = h.EventId;
            string sql = string.Format("select text from [Book10].[dbo].[TT_WP_Indexes] where EventId={0}", TempEventId);
            string s = u.selectDBScalar(sql).ToString();
            lbl.Text = s;
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
            int i = 0;
            object o = u.selectDBScalar(string.Format("SELECT IndexId FROM B10Sec.dbo.TT_Class_AdditionalColumns WHERE ClassId={0}", TargetId));
            if (o != null)
                i = (int)o;
            if (indexId == i)
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
            ListView lv = (ListView)sender;

            ListViewItem lvi = (ListViewItem)lv.NamingContainer;

            HiddenField hdn = (HiddenField)lvi.FindControl("hdnWPId");
            int id = (hdn.Value == string.Empty ? 0 : int.Parse(hdn.Value));
            //if(id==0) id=(hdnCurrentWpId.Value==string.Empty? 0 : int.Parse(hdnCurrentWpId.Value));
            if (lv.InsertItem != null)
                if (id == 0) lv.InsertItemPosition = InsertItemPosition.None; else lv.InsertItemPosition = InsertItemPosition.FirstItem;
            h.getDetails(id, lv);
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

        protected void ddl_PreRender(object sender, System.EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            ListViewItem lvi = (ListViewItem)ddl.NamingContainer;
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
            h.showTextBoxifOther(ddl, lvi);
        }


        protected void ddl_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // databind cascading ddls

            DropDownList ddl = (DropDownList)sender;
            ListViewItem lvi = (ListViewItem)ddl.NamingContainer;
            //**** colouring the main index row by selected value of purpose (שימור או שיפור)
            ListView lv = (ListView)lvi.NamingContainer;
            HtmlTable t = (HtmlTable)lvi.FindControl("indexTbl");

            for (int i = 0; i < 5; i++)
            {
                t.Rows[i].Attributes.Add("style", "background-color:lightgray");
            }
            if (ddl.SelectedValue == "128") //שיפור 
            {
                // HtmlTable t = (HtmlTable)lvi.FindControl("indexTbl");
                for (int i = 0; i < 5; i++)
                {
                    HtmlTableCell cell = (HtmlTableCell)t.Rows[i].FindControl("td_shipur_label");
                    if (cell != null)
                    {
                        cell.Attributes.Add("style", "background-color:Red");
                        cell = (HtmlTableCell)t.Rows[i].FindControl("td_shipur");
                        cell.Attributes.Add("style", "background-color:Red");
                    }
                }
            }
            else if (ddl.SelectedValue == "127")  //שימור
            {
                //HtmlTable t = (HtmlTable)lvi.FindControl("indexTbl");
                //System.Drawing.Color c = System.Drawing.ColorTranslator.FromHtml("#FFCC66");
                for (int i = 0; i < 5; i++)
                {
                    HtmlTableCell cell = (HtmlTableCell)t.Rows[i].FindControl("td_shimur_lable");
                    if (cell != null)
                    {
                        cell.Attributes.Add("style", "background-color:Red");
                        cell = (HtmlTableCell)t.Rows[i].FindControl("td_shimur");
                        cell.Attributes.Add("style", "background-color:Red");
                    }// System.Drawing.ColorTranslator.FromHtml("#FFCC66").ToString();// ;

                }

            }
            ///---------------------------------------------------------------
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

        //insert details into WP listview
        protected void Det_Inserting(object sender, ListViewInsertEventArgs e)
        {
            //       h = new TTF.App_Code.bpHelber(Page, lvWP, 7);
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
          //  lv.InsertItemPosition = InsertItemPosition.LastItem;
        }
        protected void lvWP_PreRender(object sender, System.EventArgs e)
        {
            //if (lvMP.Items.Count > 0 || true)    //++ fake force lvwp to show
            //{
                if (lvWP.Items.Count == 0)
                    lvWP.InsertItemPosition = InsertItemPosition.FirstItem;
                else
                    if (hdnItemHandled.Value == string.Empty) lvWP.InsertItemPosition = InsertItemPosition.None;
            //}
            //else
            //    lvWP.InsertItemPosition = InsertItemPosition.None;

            if (hdnItemHandled.Value != string.Empty && int.Parse(hdnItemHandled.Value) >= 0) lvWP.EditIndex = 0;
        }
        protected void chrt_OnPrePaint(object sender, System.EventArgs e)
        {

            System.Web.UI.DataVisualization.Charting.Chart chrt = (System.Web.UI.DataVisualization.Charting.Chart)sender;
            System.Web.UI.DataVisualization.Charting.Series srs = chrt.Series[0];
            //string sql = string.Format("select l1.CustRelateID from Book10_21.dbo.CustEventList l1 where l1.CustEventID in (" +
            //           "select   MAX(l.CustEventID)from Book10_21.dbo.CustEventList l where l.CustomerID={0} and l.CustEventTypeID=139)", h.CustomerId);
            ////object o = u.selectDBScalar(sql);
            //if (true)
            //{

            //}
            //string SheelonFormId = u.selectDBScalar(sql).ToString();
            //DSGRAPH.SelectCommand = string.Format("SELECT grp, perc, gid FROM Book10_21.dbo.p5t_FormResults  WHERE FormID = {0} ORDER BY gid ", SheelonFormId); 
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
                if (i == 139)
                {
                    lbl.Text = hdnDate139.Value;
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
            //ListViewItem lvi = (ListViewItem)chrt.NamingContainer;
            Button lnkb = (Button)chrt.FindControl("lnkbBacktoMainChart");

            string s = e.PostBackValue;

            string sql = string.Format("SELECT grp,perc FROM (" +
                                        "SELECT  Ord,txt grp ,a.val perc " +
                                        "FROM Book10_21.dbo.p5t_FormsQuestions q " +
                                        "LEFT OUTER JOIN Book10_21.dbo.p5t_Answers a ON a.QuestionID=q.QuestionID " +
                                        "LEFT OUTER JOIN (select * from  Book10_21.dbo.p5t_FormQuestionGroups where eventtypeid =139) g ON g.gid=q.gid " +
                                        "WHERE ((g.Name = N'{0}') OR (g.ShortName = N'{0}')) AND a.FormID={1} " +
                                        "UNION ALL " +
                                        "SELECT 99999 ord,grp, perc " +
                                        "FROM (SELECT grp,perc " +
                                        "FROM Book10_21.dbo.p5t_Forms f " +
                                        "LEFT OUTER JOIN Book10_21.dbo.p5t_FormResults r ON r.FormID = f.FormID " +
                                        "WHERE f.FormID = {1} AND grp = N'{0}') y) x " +
                                        "Order By Ord", s, hdnForm139.Value, cCustEventTypeId);
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
        protected void backTomainChart_Click(object sender, System.EventArgs e)
        {
            Button lnkb = (Button)sender;
            //ListViewItem lvi = (ListViewItem)lnkb.NamingContainer;
            System.Web.UI.DataVisualization.Charting.Chart chrt = (System.Web.UI.DataVisualization.Charting.Chart)lnkb.FindControl("Chart1");
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
            Button lnkb = (Button)sender;
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
            Button lnkb = (Button)sender;
            lnkb.Enabled = hdnCurrentWpId.Value != string.Empty;
        }

        //protected void InsertDetails(object sender, System.EventArgs e)
        //{
        //    LinkButton lnkb = (LinkButton)sender;
        //    ListViewItem lvi = (ListViewItem)lnkb.NamingContainer;
        //    ListView lv = (ListView)lvi.NamingContainer;
        //    //h.WpId = h.UpdateWp(lvi);

        //    hdnCurrentWpId.Value = h.WpId.ToString();

        //    h.InsertDetailsInHelper(h.WpId,lvi);
        //}
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
            Button hl = (Button)sender;
            int i = h.UserId * 62;

            // //string s = string.Format("TTewp_p.aspx?CID={0}&F={1}&E={2}&ID={3}", h.CustomerId, h.Frm_CatId, i, h.EventId); // + (Request.QueryString["ID"] != null ? string.Empty : "id+" + Request.QueryString["ID"].ToString());
            string s = string.Format("TTbwp_p.aspx?CID={0}&F={1}&E={2}&ID={3}", h.CustomerId, h.Frm_CatId, i, h.EventId); // + (Request.QueryString["ID"] != null ? string.Empty : "id+" + Request.QueryString["ID"].ToString());
            // Response.Redirect(s);
            //hl.NavigateUrl = s;

        }

        public string createRedirectString()
        {
            int i = h.UserId * 62;
            string s = string.Format("window.open('TTbwp_p.aspx?CID={0}&F={1}&E={2}&ID={3}','_blank')", h.CustomerId, h.Frm_CatId, i, h.EventId); // + (Request.QueryString["ID"] != null ? string.Empty : "id+" + Request.QueryString["ID"].ToString());

            return s;

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
        }


        protected bool isUpdatable()
        {
            return h.Isupdatable();
        }




        protected void tb_PreRender(object sender, System.EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Attributes.Add("onchange", "myValues = myValues + '|' + 'Text=' + this.value");
        }
        protected void lvIdexes_PreRender(object sender, System.EventArgs e)
        {
            ListView lv = (ListView)sender;
            ListViewItem lvi = (ListViewItem)lv.NamingContainer;
            HiddenField hdn = (HiddenField)lvi.FindControl("hdnWPId");
            int id = (hdn.Value == string.Empty ? 0 : int.Parse(hdn.Value));
            if (id != 0) h.getSupportIndexes(id, lv);
        }

        protected void lnkbedit_Click(object sender, EventArgs e)
        {
            Button lnkb = (Button)sender;
            ListViewItem lvi = (ListViewItem)lnkb.NamingContainer;
            ListView lv = (ListView)lvi.NamingContainer;
            lv.EditIndex = lvi.DisplayIndex;
            lv.DataBind();
        }

        //protected void DSGRAPH_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        //{
        //    //find FormId of the sheelon lenihul tmihot
        //    string sql = string.Format("select l1.CustRelateID from Book10_21.dbo.CustEventList l1 where l1.CustEventID in ("+
        //                "select   MAX(l.CustEventID)from Book10_21.dbo.CustEventList l where l.CustomerID={0} and l.CustEventTypeID=139)",h.CustomerId);

        //    string SheelonFormId = u.selectDBScalar(sql).ToString();
        //    DSGRAPH.SelectCommand =string.Format("SELECT grp, perc, gid FROM Book10_21.dbo.p5t_FormResults  WHERE FormID = {0} ORDER BY gid ",SheelonFormId);    

        //}

        //protected void DSGRAPH_PreRender(object sender, EventArgs e)
        //{
        //    string sql = string.Format("select l1.CustRelateID from Book10_21.dbo.CustEventList l1 where l1.CustEventID in (" +
        //               "select   MAX(l.CustEventID)from Book10_21.dbo.CustEventList l where l.CustomerID={0} and l.CustEventTypeID=139)", h.CustomerId);

        //    string SheelonFormId = u.selectDBScalar(sql).ToString();
        //    DSGRAPH.SelectCommand = string.Format("SELECT grp, perc, gid FROM Book10_21.dbo.p5t_FormResults  WHERE FormID = {0} ORDER BY gid ", SheelonFormId); 
        //}

        //protected void Page_Error(object sender, EventArgs e)
        //{ 
        //   //TTF.App_Code.
        //   // TTF.App_Code.
        //    TTF.App_Code.PageError.WriteErrorLog();
        //}

        protected string HyperLink1_Click()
        {

            int i = h.UserId * 62;
            //string s = string.Format("TTewp_p.aspx?CID={0}&F={1}&E={2}&ID={3}", h.CustomerId, h.Frm_CatId, i, h.EventId); // + (Request.QueryString["ID"] != null ? string.Empty : "id+" + Request.QueryString["ID"].ToString());
            string s = string.Format("window.open('TTbwp_p.aspx?CID={0}&F={1}&E={2}&ID={3}','_blank')", h.CustomerId, h.Frm_CatId, i, h.EventId); // + (Request.QueryString["ID"] != null ? string.Empty : "id+" + Request.QueryString["ID"].ToString());
            return s;
        }

        protected void lvWP_Load(object sender, EventArgs e)
        {
            //ListView lv = (ListView)sender;
            //for (int i = 0; i < lv.Items.Count; i++)
            //{
            //    if (i%2==0)
            //    {
            //        lv.Items[i].
            //    }
            //}
        }

        protected void showWeekBTN_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            ListViewItem lvi = (ListViewItem)btn.NamingContainer;
            // ListView lv = (ListView)lvi.NamingContainer;
            ListView weekLV = (ListView)lvi.FindControl("LVWeeklyPlan");

        }

        protected void DSGRAPH_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@EventTypeId"].Value = u.selectDBScalar(string.Format("select ToolForSupport from book10_21.dbo.TT_FormTypes where  CustEventTypeId = {0}", cCustEventTypeId)).ToString();
        }

        protected void tbDetails_PreRender(object sender, EventArgs e)
        {
            HtmlGenericControl c = (HtmlGenericControl)sender;
            c.Attributes.Add("onkeyup", "alert('hello');");
        }

        protected void HyperLink1_PreRender(object sender, EventArgs e)
        {
            Button hl = (Button)sender;
            int i = h.UserId * 62;
            string s = string.Format("window.open('TTbwp_p.aspx?CID={0}&F={1}&E={2}&ID={3}&ET={4}&FT={5}','_blank')", h.CustomerId, h.Frm_CatId, i, h.EventId, cCustEventTypeId, cFormTypeId); // + (Request.QueryString["ID"] != null ? string.Empty : "id+" + Request.QueryString["ID"].ToString());
            hl.Attributes.Add("onclick", s);
        }
        public string AvoidIllegalvalue(string sf)
        {
            string s = Eval(sf).ToString();
            if (s == "")
            {
                return null;
            }
            return s;
        }
    }
}
