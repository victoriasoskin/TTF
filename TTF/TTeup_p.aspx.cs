﻿using System;
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
    public partial class TTeup_p : System.Web.UI.Page
    {
        TTF.App_Code.bpHelber h;
        TTF.App_Code.Util u;

        #region load

        public TTeup_p()
        {
            h = new TTF.App_Code.bpHelber(Page, 8, 136);
            h.lvMP = lvMP;
            //h.LVWeeklyPlan = LVWeeklyPlan;

            u = new TTF.App_Code.Util();
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            h.CustEventTypeId = 135;
            h.FormTypeId = 7;

            lvHdr.DataBind();
            lvHdr1.DataBind();
            //lvHdr2.DataBind();
            //h.LVWeeklyPlan = LVWeeklyPlan;
            //h.divWeeklytimetable = divWeeklytimetable;

            h.selectAction();

            if (!IsPostBack)
            {
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
            }
        }

        #endregion

        #region header
        protected void lvHdr_DataBind(object sender, System.EventArgs e)
        {
            ListView lv = (ListView)sender;
            h.getHeader(lv);
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
            if (dl.InsertItem != null)
                if (id == 0) dl.InsertItemPosition = InsertItemPosition.None; else dl.InsertItemPosition = InsertItemPosition.FirstItem;
            h.getDetails(id, dl);
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
            lbl.Text = s.Substring(s.Length - 4);

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

        protected void W_Click(object sender, EventArgs e)
        {
            System.Web.UI.DataVisualization.Charting.Chart chrt = (System.Web.UI.DataVisualization.Charting.Chart)lvMP.Items[0].FindControl("Chart1");
            chrt.Visible = false;

            Label lbl = (Label)lvHdr1.Items[0].FindControl("lblName");
            string name = Server.UrlEncode(lbl.Text.Replace(" ", "_"));

            string style = "td {font-family: Arial;}" +
                            ".rowHeader {text-decoration: underline; text-align: right;}" +
                            ".rowHeaderb {text-decoration: underline; text-align: right;}";

            List<ListView> lvList = new List<ListView>();
            lvList.Add(lvHdr);
            lvList.Add(lvMP);
            lvList.Add(lvHdr1);
            lvList.Add(lvWP);
            //lvList.Add(lvHdr2);
            //lvList.Add(LVWeeklyPlan);
            u.ExportIntoOffice(lvList, name, "word", style);
        }
        protected void E_Click(object sender, EventArgs e)
        {
            System.Web.UI.DataVisualization.Charting.Chart chrt = (System.Web.UI.DataVisualization.Charting.Chart)lvMP.Items[0].FindControl("Chart1");
            chrt.Visible = false;

            Label lbl = (Label)lvHdr1.Items[0].FindControl("lblName");
            string name = Server.UrlEncode(lbl.Text.Replace(" ", "_"));

            string style = "td {font-family: Arial;}";

            List<ListView> lvList = new List<ListView>();
            lvList.Add(lvHdr);
            lvList.Add(lvMP);
            lvList.Add(lvHdr1);
            lvList.Add(lvWP);
            //lvList.Add(lvHdr2);
            //lvList.Add(LVWeeklyPlan);
            //
            u.ExportIntoOffice(lvList, name, "excel", style);
        }
        protected string printingStyle(string f)
        {
            int i = int.Parse(Eval(f).ToString());
            string s = (requiresPageBreak(f) ? string.Empty : "page-break-before:always;width:800px;height:30px;font-weight:bold;");
            return s;
        }
        protected string smallHdr(string f)
        {
            string s = string.Empty;
            if (requiresPageBreak(f))
            {

            }
            return s;
        }
        protected bool requiresPageBreak(string f)
        {
            int pSize = 1;
            int i = int.Parse(Eval(f).ToString());
            return false || i == 1;
        }

        protected void lvMainReports_DataBinding(object sender, EventArgs e)
        {
            int id = 0;
            ListView lv = (ListView)sender;
            ListViewItem lvi = (ListViewItem)lv.NamingContainer;
            HiddenField hdn = (HiddenField)lvi.FindControl("hdnWpId");
            if (hdn != null)
            {
                if (hdn.Value != string.Empty)
                {
                    id = int.Parse(hdn.Value);
                }

            }
            string sql = string.Format("SELECT Id, WpId ,DetailsId, val1, Q1, val2 ,Q2,val3 ,Q3 ,LoadTime ,URName " +
                       "FROM Book10.dbo.TT_vMainReport " +
                       "WHERE WpId={0} AND UserId is NOT NULL " +
                       "ORDER BY LoadTime DESC ", id);
            DataTable dt = u.selectDBQuesry(sql);
            lv.DataSource = dt;
        }

        protected void lvDetup_DataBinding(object sender, EventArgs e)
        {
            int id = 0;
            ListView lv = (ListView)sender;
            ListViewItem lvi = (ListViewItem)lv.NamingContainer;
            HiddenField hdn = (HiddenField)lvi.FindControl("hdnDetId");
            if (hdn != null)
            {
                if (hdn.Value != string.Empty)
                {
                    id = int.Parse(hdn.Value);
                }

            }
            string sql = string.Format("SELECT Id ,FormId ,EventId ,CustomerId ,WpId ,DetailsId ,val4 Val,Q4 ,Text ,UserId ,URName ,LoadTime " +
                                                  "  FROM Book10.dbo.TT_vDetReports " +
                                                "    WHERE  DetailsId = {0} " +
                                              "     ORDER BY LoadTime DESC ", id);
            DataTable dt = u.selectDBQuesry(sql);
            lv.DataSource = dt;
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
    }
}