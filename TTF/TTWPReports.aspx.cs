using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows;

namespace TTF
{
    public partial class TTWPReports : System.Web.UI.Page
    {
        TTF.App_Code.Util u;

        protected void Page_Load(object sender, EventArgs e)
        {
            u = new App_Code.Util();
            Session["UserId"] = getUserId();
            if (!Page.IsPostBack)
            {
                GridView1.DataBind();
            }
            
        }

        public int getUserId()
        {

            int i = (HttpContext.Current.Request.QueryString["E"] == null ? 0 * 62 : int.Parse(HttpContext.Current.Request.QueryString["E"].ToString()));
            // if (i == 0) HttpContext.Current.Response.Redirect("~/CustEventReport.Aspx");
            i = i / 62;
            return i;
        }

        protected void DDLSecLvlRep_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedVal1 = DDLMainLvlRep.SelectedValue;
            string selectedVal2 = DDLSecLvlRep.SelectedValue;
            string hideControls = "SELECT VisibleControls FROM B10SEC.dbo.TT_Class_AdditionalColumns WHERE ClassId = 999999";
            string[] splitV;
            string visibileControlsString = "";
            //ClearAllControls();
            //CleanGridView();
            //GridView1.Visible = false;
            BTNTableToChart.ImageUrl = "~/Pictures/images.jpg";
            visibileControlsString = u.selectDBScalar(hideControls).ToString();
            splitV = visibileControlsString.Split('|');
            for (int i = 0; i < splitV.Length; i++)
            {
                Control o1 = this.FindControl(splitV[i]);
                Control o = u.FindControlRecursive(this, splitV[i]);
                o.Visible = false;
                //להפוך לדדל
                //ddl.visible=true
            }
            ReportLBL.Text = DDLMainLvlRep.SelectedItem.Text;
            SubReportLBL.Text = DDLSecLvlRep.SelectedItem.Text;
            string sql = string.Format("SELECT VisibleControls FROM B10SEC.dbo.TT_Class_AdditionalColumns WHERE ClassId = 0{0}", DDLSecLvlRep.SelectedValue);

            if (u.selectDBScalar(sql) == null)
            {
                visibileControlsString = string.Empty;
            }
            else
            {
                visibileControlsString = u.selectDBScalar(sql).ToString();
            }
            splitV = visibileControlsString.Split('|');

            for (int i = 0; i < splitV.Length; i++)
            {
                if (splitV.Length == 1)
                {
                    break;
                }
                Control o1 = this.FindControl(splitV[i]);
                Control o = u.FindControlRecursive(this, splitV[i]);
                o.Visible = true;
            }

        }

        private void CleanGridView()
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
        }

        protected void DDLMainLvlRep_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedVal = DDLMainLvlRep.SelectedValue;
            CleanGridView();
            //clearSecondaryLevelReports();
        }
     
        protected void Gridview1_RowsDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e) // כפתור הביצוע
        {        
            string query = "";
            string select_query = "";
            string mainDef = " ";
            DataTable dt;
            mainDef = CreateWhereString();
            if (u.selectDBScalar(string.Format("select Text from b10Sec.dbo.TT_Classes where Id = {0} ", DDLSecLvlRep.SelectedValue)).ToString() != null)

            //if (u.selectDBScalar(string.Format("select Text from b10Sec.dbo.TT_Classes where Parent = {0} and Val = {1}", DDLSecLvlRep.SelectedValue, RadioButtonList1.SelectedIndex)).ToString()!=null)
            {
                query = u.selectDBScalar(string.Format("select Text from b10Sec.dbo.TT_Classes where Id = {0}", DDLSecLvlRep.SelectedValue)).ToString();
            }
            select_query = string.Format(query, mainDef);
           // ChartCreateFunc();
            dt = u.selectDBQuesry(select_query);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    BoundField bf = new BoundField();
                    bf.DataField = dt.Columns[i].ColumnName;

                    GridView1.Columns.Add(bf);
                }
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            else // במידה ולא קיימים נתונים
            {
                dt.Rows.Add(dt.NewRow());
                GridView1.DataSource = dt;
                GridView1.DataBind();
                int totalcolums = GridView1.Rows[0].Cells.Count;
                GridView1.Rows[0].Cells.Clear();
                GridView1.Rows[0].Cells.Add(new TableCell());
                GridView1.Rows[0].Cells[0].ColumnSpan = totalcolums;
                GridView1.Rows[0].Cells[0].Text = "לא נמצאו נתונים";
            }
            //GridViewDiv.Visible = true; 
            GridView1.Visible = true;
            Chart1.Visible = false;
            UCBtn.Visible = true;
        }

        private void ClearAllControls()
        {
            string clearControls = "SELECT VisibleControls FROM B10SEC.dbo.TT_Class_AdditionalColumns WHERE ClassId = 999999";
            string[] splitV;
            string visibileControlsString = "";
            visibileControlsString = u.selectDBScalar(clearControls).ToString();
            splitV = visibileControlsString.Split('|');
            for (int i = 0; i < splitV.Length; i++)
            {
                Control o1 = this.FindControl(splitV[i]);
                Control o = u.FindControlRecursive(this, splitV[i]);
                if (o.GetType() == typeof(DropDownList))
                {
                    ((DropDownList)o).SelectedItem.Text = "";
                }
                else if (o.GetType() == typeof(TextBox))
                {
                    ((TextBox)o).Text = "";
                }

            }
        }
        //creating the chart
        private void ChartCreateFunc()
        {
            string chartString = "";
            string mainDef = " ";
            mainDef = CreateWhereString();
            chartString = CreateChartQuery(mainDef);
            SqlDataSource1.SelectCommand = chartString;
            Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Angle =45;
            Chart1.Series[0].Color = System.Drawing.Color.Blue;
           // Chart1.Series[1].Color = System.Drawing.Color.Blue;
        }

        private string CreateChartQuery(string whereString)
        {
            string sql = "";

            string res = "";
            string zeroParam = "";
            string firstParam = "";
            string secParam = "";
            //string exceptionString = "יש לבחור שדה כמותי";
            string exceptionString1 = "גרפים קיימים רק עבור דוח תוכניות תמיכה לתלמיד ודוח תלמידים ללא תוכניות תמיכה.";

            //if (RadioButtonList1.SelectedValue.ToString() != "0")
            //{
            //    System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //    sb.Append("<script type = 'text/javascript'>");
            //    sb.Append("window.onload=function(){");
            //    sb.Append("alert('");
            //    sb.Append(exceptionString);
            //    sb.Append("')};");
            //    sb.Append("</script>");

            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
            //    return string.Empty;

            //}
            if (DDLSecLvlRep.SelectedValue != string.Empty)
            {
                if ((DDLRange.Text != "") && (DDLSecLvlRep.SelectedValue.ToString() == "326"))//ת"ת לתלמיד - אם בחרתי תחום
                {
                    zeroParam = " c1.Name [Subject]  ";
                    firstParam = " COUNT(c1.Name) [CountCustomersInSubjects]";
                    secParam = " c1.Name";
                    Chart1.Series[0].XValueMember = "Subject";
                    Chart1.Series[0].YValueMembers = "CountCustomersInSubjects";
                }
                else if ((DDLRange.Text == "") && (DDLSecLvlRep.SelectedValue.ToString() == "326"))//ת"ת לתלמיד - אם לא בחרתי תחום 
                {
                    zeroParam = " c.Name  [Range]";
                    firstParam = " COUNT(c.Name) [CountCustomersInRanges]";
                    secParam = "c.Name";
                    Chart1.Series[0].XValueMember = "Range";
                    Chart1.Series[0].YValueMembers = "CountCustomersInRanges";
                }
                else if ((DDLRegions.Text != "") && (DDLSecLvlRep.SelectedValue.ToString() == "327"))  // תלמידים ללא ת"ת - אם בחרתי איזור => הגרף יוצג לפי מסגרות
                {
                    zeroParam = " f.FrameName [Frame]  ";
                    firstParam = " COUNT(tt_cl.CustomerId) [CustWithoutTT] ";
                    secParam = " f.FrameName ";
                    Chart1.Series[0].XValueMember = "Frame";
                    Chart1.Series[0].YValueMembers = "CustWithoutTT";
                }
                else if ((DDLRegions.Text == "") && (DDLSecLvlRep.SelectedValue.ToString() == "327"))  // תלמידים ללא ת"ת - אם לא בחרתי איזור => הגרף יוצג לפי איזורים 
                {
                    zeroParam = " s.ServiceName [Region]  ";
                    firstParam = " COUNT(tt_cl.CustomerId) [CustWithoutTT] ";
                    secParam = "  s.ServiceName";
                    Chart1.Series[0].XValueMember = "Region";
                    Chart1.Series[0].YValueMembers = "CustWithoutTT";
                }
                else if ((DDLRegions.Text == "") && (DDLSecLvlRep.SelectedValue.ToString() == "339"))  // תלמידים ללא ת"ת - אם לא בחרתי איזור => הגרף יוצג לפי איזורים 
                {
                    zeroParam = " s.ServiceName [Region]  ";
                    firstParam = " COUNT(tt_cl.CustomerId) [CustWithoutTT] ";
                    secParam = "  s.ServiceName";
                   // Chart1.Series[0].XValueMember = "Region";
                    Chart1.Series[0].YValueMembers = "noQf";
                    Chart1.Series[1].YValueMembers = "withQF";
                }
                else if ((DDLRegions.Text == "") && (DDLSecLvlRep.SelectedValue.ToString() == "341"))  // תלמידים ללא ת"ת - אם לא בחרתי איזור => הגרף יוצג לפי איזורים 
                {
                    zeroParam = " s.ServiceName [Region]  ";
                    firstParam = " COUNT(tt_cl.CustomerId) [CustWithoutTT] ";
                    secParam = "  s.ServiceName";
                    // Chart1.Series[0].XValueMember = "Region";
                    Chart1.Series[0].YValueMembers = "perc";
                    Chart1.Series[0].XValueMember = "grp";
                   // Chart1.Series[1].YValueMembers = "withQF";
                }
                try
                {
                    sql = u.selectDBScalar(string.Format("SELECT Queries FROM B10SEC.dbo.TT_Class_AdditionalColumns WHERE ClassId = (select Id from b10Sec.dbo.TT_Classes where Id = {0} )", DDLSecLvlRep.SelectedValue)).ToString();
                    if (sql != null)
                    {
                        sql = string.Format(sql, zeroParam, firstParam, whereString, secParam); //if (DDLRange.Text !="")
                    }
                }
                catch 
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("<script type = 'text/javascript'>");
                        sb.Append("window.onload=function(){");
                        sb.Append("alert('");
                        sb.Append(exceptionString1);
                        sb.Append("')};");
                        sb.Append("</script>");

                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                        BTNTableToChart.ImageUrl = "~/Pictures/images.jpg";
                    }
            }
            return sql;

        }

        private Exception Exception(string p)
        {
            throw new NotImplementedException(p);
        }


        // function that passes over all controlls and prepares the whereString  for the select query
        private string CreateWhereString()
        {
            string mainDef = "";// string.Format("where f.FrameId in (SELECT Id FROM [Book10_21].[dbo].[tf_Frames] ({0},null,null,null,2))  ", getUserId());
            
            if (DDLRegions.Text != "")      //אם נבחר אזור
            {
                //level = 1;
                if (mainDef=="")
                {
                    mainDef += "where s.ServiceId = " + DDLRegions.Text;
                }
                else
                mainDef += "and s.ServiceId = " + DDLRegions.Text;
            }
            if (DDLFrames.Text != "")     // אם נבחרה מסגרת
            {
                //level = 2;
                mainDef += " AND f.FrameId = " + DDLFrames.Text;
            }



            if (DDLClass.Text != "")  //בחירת שכבה
            {

                mainDef += "  AND LayerId = " + DDLClass.SelectedItem.Text;
            }

            if (DDLClassNumber.Text != "")  //בחירת מספר הכיתה
            {

                mainDef += " AND ClassId = " + DDLClassNumber.Text + DDLClassNumber.Text;
            }

            if ((TBXClientId.Text != string.Empty)&&(TBXClientId.Visible==true)) // תיבת טקסט לת.ז.
            {
                mainDef += "AND cl.CustomerId = " + TBXClientId.Text;
            }
            if ((TXBClientLastName.Text != string.Empty)&&(TXBClientLastName.Visible==true)) // תיבת טקסט לשם משפחה
            {
                if (TBXClientId.Text == string.Empty)
                {
                    mainDef += "AND cl.CustLastName = '" + TXBClientLastName.Text + "'";
                }
                mainDef += " AND cl.CustLastName = '" + TXBClientLastName.Text + "'";
            }
            if ((TXBClientFirstName.Text != string.Empty)&&(TXBClientFirstName.Visible==true))  //תיבת טקסט לשם פרטי 
            {
                if ((TXBClientLastName.Text == string.Empty) && (TBXClientId.Text == string.Empty))
                {
                    mainDef += "AND cl.CustFirstName = '" + TXBClientFirstName.Text + "'";
                }
                mainDef += " AND cl.CustFirstName = '" + TXBClientFirstName.Text + "'";
            }
            if (DDLRange.Text != "" && (DDLSecLvlRep.SelectedValue.ToString() != "350")) //אם נבחר תחום 
            {
                if (mainDef == "where ")
                {
                    mainDef = mainDef += " w.RangeId= " + DDLRange.SelectedValue;
                }
                else mainDef += " AND w.RangeId= " + DDLRange.SelectedValue;
            }
            if ((DDLSubject.Text != "") &&(DDLSubject.Visible==true))//אם נבחר נושא
            {
                if (mainDef == "where ")
                {
                    mainDef = mainDef += "w.SubjectId = " + DDLSubject.SelectedValue;
                }
                mainDef += " AND w.SubjectId = " + DDLSubject.SelectedValue;
            }
            if ((DDLQuestions.Text != "")&&(DDLQuestions.Visible==true))
            {
                if (mainDef == "where ")
                {
                    mainDef = mainDef += "q.questionId = " + DDLQuestions.SelectedValue;
                }
                mainDef += " AND q.questionId = " + DDLQuestions.SelectedValue;
            }
            //mainDef += "'";
            if ((mainDef == "where ") &&
                ((DDLSecLvlRep.SelectedValue.ToString() == "327")
                || (DDLSecLvlRep.SelectedValue.ToString() == "351")
                || (DDLSecLvlRep.SelectedValue.ToString() == "378")
                || (DDLSecLvlRep.SelectedValue.ToString() == "340")
                || (DDLSecLvlRep.SelectedValue.ToString() == "338")
                || (DDLSecLvlRep.SelectedValue.ToString() == "350"))) //אם בחרנו דוח תלמידים ללא תוכניות תמיכה
            {
                mainDef = "where ";
            }
            else if ((mainDef == "where "))
            {
                mainDef = "";
            }
            if ((mainDef != "where ") &&
                ((DDLSecLvlRep.SelectedValue.ToString() == "327")
                || (DDLSecLvlRep.SelectedValue.ToString() == "351")
                || (DDLSecLvlRep.SelectedValue.ToString() == "378")
                || (DDLSecLvlRep.SelectedValue.ToString() == "340")
                || (DDLSecLvlRep.SelectedValue.ToString() == "338")
                || (DDLSecLvlRep.SelectedValue.ToString() == "350"))) //אם בחרנו דוח תלמידים ללא תוכניות תמיכה ועוד שדות נוספים
            {
                mainDef += " and ";
            }
            return mainDef;
        }

        //creats and shows the chart after we putsh the button
        protected void BTNTableToChart_Click(object sender, ImageClickEventArgs e)
        {
            ChartCreateFunc();      //function that creats the chart 
            if (GridView1.Visible == false) // אם מוצג גרף 
            {
                BTNTableToChart.ImageUrl = "~/Pictures/images.jpg";
                GridView1.Visible = true;
                Chart1.Visible = false;
            }
            else // אם מוצגת טבלה
            {
                BTNTableToChart.ImageUrl = "~/Pictures/table_1.jpg";
                GridView1.Visible = false;
                Chart1.Visible = true;
            }
        }

        protected void Chart1_PrePaint(object sender, System.Web.UI.DataVisualization.Charting.ChartPaintEventArgs e)
        {

            //System.Web.UI.DataVisualization.Charting.Chart chrt = (System.Web.UI.DataVisualization.Charting.Chart)sender;
            //System.Web.UI.DataVisualization.Charting.Series srs = chrt.Series[0];
            //System.Web.UI.DataVisualization.Charting.DataPoint p = srs.Points[srs.Points.Count - 1];
            //p.BackHatchStyle = System.Web.UI.DataVisualization.Charting.ChartHatchStyle.BackwardDiagonal;
        }

        protected void Chart1_Load(object sender, EventArgs e)
        {

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string select_query = "";
            string mainDef = " ";
            string query = "";
            DataTable dt;

            mainDef = CreateWhereString();
            if (u.selectDBScalar(string.Format("select Text from b10Sec.dbo.TT_Classes where Parent = {0} ", DDLSecLvlRep.SelectedValue)).ToString() != null)
            {
                query = u.selectDBScalar(string.Format("select Text from b10Sec.dbo.TT_Classes where Parent = {0}", DDLSecLvlRep.SelectedValue)).ToString();
            }
            //select_query = string.Format(query, level, mainDef);
            select_query = string.Format(query, mainDef);
            dt = u.selectDBQuesry(select_query);
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void ToExcelBtn_Click(object sender, EventArgs e)
        {
            u.ExportIntoOffice(GridView1, "Rep", "excel", string.Empty, DDLMainLvlRep.SelectedItem.Text, DDLSecLvlRep.SelectedItem.Text);
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }


        // export gridview to word
        protected void ToWordBtn_Click(object sender, ImageClickEventArgs e)
        {
            GridView1.AllowPaging = false;
            u.ExportIntoOffice(GridView1, "Rep", "word", string.Empty, DDLMainLvlRep.SelectedItem.Text, DDLSecLvlRep.SelectedItem.Text);
        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            //e.Command.Parameters[]
        }
        //Drill down to the main chart to see the subjects 
        protected void ChartDrill(object sender, ImageMapEventArgs e)
        {
            string zeroParam = "";
            string firstParam = "";
            string secParam = "";
            string sql = "";
            string mainDef = string.Format("where f.FrameId in (SELECT Id FROM [Book10_21].[dbo].[tf_Frames] ({0},null,null,null,2))  ", getUserId());
            string range = "";
            DataTable dt;
            zeroParam = " c1.Name [Subject]  ";
            firstParam = " COUNT(c1.Name) [CountCustomersInSubjects]";
            secParam = " c1.Name";
            Chart1.Series[0].XValueMember = "Subject";
            Chart1.Series[0].YValueMembers = "CountCustomersInSubjects";
            System.Web.UI.DataVisualization.Charting.Chart chrt = (System.Web.UI.DataVisualization.Charting.Chart)sender;
            string s = e.PostBackValue; // e.postBackValue = התחום שנבחר בגרף ע"י הלחיצה על העמודה
            mainDef = CreateWhereString();
            s = s.Trim().Replace("'", "''"); // מחיקת הרווחים מהתחום הנסחר
            range = "'" + s + "'";
            if (mainDef == "") // בדיקה האם יש תנאי שטח 
            {
                mainDef = " where c.Name = " + range;
            }
            else mainDef += "  c.Name = " + range;
            sql = u.selectDBScalar(string.Format("SELECT Queries FROM B10SEC.dbo.TT_Class_AdditionalColumns WHERE ClassId = (select Id from b10Sec.dbo.TT_Classes where Id = {0} )", DDLSecLvlRep.SelectedValue)).ToString();
            if (sql != null)
            {
                sql = string.Format(sql, zeroParam, firstParam, mainDef, secParam, s); //if (DDLRange.Text !="")
            }
            dt = u.selectDBQuesry(sql);
            if (dt.Rows.Count > 1)
            {
                SqlDataSource1.SelectCommand = sql;
                string ssss = dt.Rows[0]["CountCustomersInSubjects"].ToString();
                System.Web.UI.DataVisualization.Charting.Title ttl = new System.Web.UI.DataVisualization.Charting.Title();
                ttl.Text = s;
                ttl.Font = new System.Drawing.Font("Arial", 16);
                chrt.Titles.Add(ttl);
                //lnkb.Visible = true;
            }
            else
            {
                chrt.DataSourceID = null;
                chrt.DataSource = SqlDataSource1;
                //lnkb.Visible = false;
            }
            goBackBtn.Visible = true;

            chrt.Visible = true;
            chrt.DataBind();

        }
        //go back from the subChart to the Main chart
        protected void goBackBtn_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton lnkb = (ImageButton)sender;
            System.Web.UI.DataVisualization.Charting.Chart chrt = (System.Web.UI.DataVisualization.Charting.Chart)lnkb.FindControl("Chart1");
            ChartCreateFunc();
            Chart1.Series[0].XValueMember = "Range";
            Chart1.Series[0].YValueMembers = "CountCustomersInRanges";


            chrt.Visible = true;
            goBackBtn.Visible = false;
        }

        protected void FROMDateBTN_Click(object sender, EventArgs e)
        {
            this.Calendar2.Visible = true;
            FromDateTXB.Text = Calendar2.SelectedDate.ToString();
        }

        protected void ToDateBTN_Click(object sender, EventArgs e)
        {
            this.Calendar1.Visible = true;
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            this.ToDateTXB.Text = this.Calendar1.SelectedDate.ToString();
        }
        protected void Calendar2_SelectionChanged(object sender, EventArgs e)
        {
            this.FromDateTXB.Text = this.Calendar2.SelectedDate.ToString();
        }

        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            GridView1.Visible = true;
            GridView gv = (GridView)sender;
            if (gv.Rows.Count > 1)
            {
                foreach (GridViewRow gvr in gv.Rows)
                {
                    setRowStyle(gvr);
                }
            }

        }

        private void setRowStyle(GridViewRow gvr)
        {
            if (gvr.Cells[0].Text == "סה&quot;כ")
            {
                gvr.BackColor = System.Drawing.ColorTranslator.FromHtml("#808080");
                gvr.Font.Bold = true;
            }
            else if (gvr.Cells[1].Text == "סה&quot;כ")
            {
                gvr.BackColor = System.Drawing.ColorTranslator.FromHtml("#A6A6A6");
                gvr.Font.Bold = true;
            }
            else if (gvr.Cells[2].Text == "סה&quot;כ")
            {
                gvr.BackColor = System.Drawing.ColorTranslator.FromHtml("#C0C0C0");
                gvr.Font.Bold = true;
            }
            else if (gvr.Cells.Count > 3)
            {
                if (gvr.Cells[3].Text == "סה&quot;כ")
                {
                    gvr.BackColor = System.Drawing.ColorTranslator.FromHtml("#D9D9D9");
                    gvr.Font.Bold = true;
                }
            }
        }

        protected void DSQuestions_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            try
            {
                string s = e.Command.Parameters[0].Value.ToString();
            }
            catch (Exception ex) { }

        }


    }
}