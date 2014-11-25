using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;

namespace TTF.App_Code
{
    public class Util
    {
        public Util()
        {

        }
        public SqlConnection sqlConn
        {
            get
            {
                return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Book10PE"].ConnectionString);
            }
        }

        public Control FindControlRecursive(Control rootControl, string controlID)
        {
            if (rootControl.ID == controlID) return rootControl;

            foreach (Control controlToSearch in rootControl.Controls)
            {
                Control controlToReturn =
                    FindControlRecursive(controlToSearch, controlID);
                if (controlToReturn != null) return controlToReturn;
            }
            return null;
        }
        //returns Data Table after select query execution
        public DataTable selectDBQuesry(string sql)
        {
            SqlConnection cn = sqlConn;
            DataTable dt = new DataTable();
            SqlCommand cD = new SqlCommand(sql, cn);
            cD.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cD;
            da.Fill(dt);

            return dt;
        }
        // returns object after select query execution 
        public object selectDBScalar(string sql)
        {
            SqlConnection cn = sqlConn;
            SqlCommand cD = new SqlCommand(sql, cn);
            cD.CommandType = CommandType.Text;
            cn.Open();
            object o = null;
            try
            {
                o = cD.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cn.Close();
            }
            return o;
        }
        public Exception executeSql(string sql)
        {
            Exception eex = null;
            SqlConnection cn = sqlConn;
            DataTable dt = new DataTable();
            SqlCommand cD = new SqlCommand(sql, cn);
            cD.CommandType = CommandType.Text;
            cn.Open();
            object o = null;
            try
            {
                o = cD.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                eex = ex;
                throw ex;
            }
            finally
            {
                cn.Close();
            }
            return eex;

        }

        public void ExportIntoOffice(List<ListView> lvlist, string FileName, string App,string style)
        {
            try
            {
                string contentType = string.Empty;
                string addHeader = string.Empty;
                switch (App)
                {
                    case "word":
                        {
                            contentType = "application/vnd.ms-word";
                            addHeader = "attachment;filename=" + FileName + ".doc";
                            break;
                        }
                    case "excel":
                        {
                            contentType = "application/vnd.ms-excel";
                            addHeader = "attachment;filename=" + FileName + ".xls";
                            break;
                        }
                    default:
                        {
                            return;
                            //                         break;
                        }
                }
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.ContentType = contentType;
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", addHeader);
                System.Web.HttpContext.Current.Response.Charset = "";
                System.Web.HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                StringWriter stringWrite = new StringWriter();
                stringWrite.Write("<html><header><style type='css/text'>" + style + "</style></header><body style='direction:rtl; font-family:Arial;'>");
                HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
                HtmlForm frm = new HtmlForm();
                ListView lv = lvlist[0];
                lv.Parent.Controls.Add(frm);
                frm.Controls.Add(lv);
                for (int i = 1; i < lvlist.Count; i++)
                {
                    lv = lvlist[i];
                    frm.Controls.Add(lv);
                }
               
                frm.RenderControl(htmlWrite);
                System.Web.HttpContext.Current.Response.Write(stringWrite.ToString() + "</body></html>");

            }
            catch (Exception ex)
            {
            }
            finally
            {
                System.Web.HttpContext.Current.Response.End();
            }
        }
        public string getVersionDetails(string tbl, string firstIdField,int id)
        {
             return string.Format("DECLARE @OldEvent int DECLARE @FirstId int SELECT @FirstId = ISNULL({0},Id), @OldEvent = EventId FROM {1} WHERE Id = {2}", firstIdField, tbl, id);
        }
        public void ExportIntoOffice(GridView gv, string FileName, string App, string style, string title, string subTitle)
        {
            try
            {
                string contentType = string.Empty;
                string addHeader = string.Empty;
                switch (App)
                {
                    case "word":
                        {
                            contentType = "application/ms-word";
                            addHeader = "attachment;filename=" + FileName + ".doc";
                            break;
                        }
                    case "excel":
                        {
                            contentType = "application/ms-excel";
                            addHeader = "attachment;filename=" + FileName + ".xls";
                            break;
                        }
                    default:
                        {
                            return;
                            //                         break;
                        }
                }
                //--------------------------------------------
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.ContentType = contentType;
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", addHeader);
                System.Web.HttpContext.Current.Response.Charset = "";

                System.Web.HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                StringWriter stringWrite = new StringWriter();
                //stringWrite.Write("<html><header><style type='css/text'>" + style + "</style></header><body style='direction:rtl; font-family:Arial;'>" + "<h1>" + title + "</h1><h2>" + subTitle + "</h2>");

                stringWrite.Write(style + " <body style=\"direction:rtl; font-family:Arial;\">" + "<h1>" + title + "</h1><h2>" + subTitle + "</h2>");
                HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
                HtmlForm frm = new HtmlForm();
                //--------------------------------------------



               // System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;      filename=FileName.xls");

               // System.Web.HttpContext.Current.Response.Charset = "";

               // // If you want the option to open the Excel file without saving than

               // // comment out the line below

               // // Response.Cache.SetCacheability(HttpCacheability.NoCache);

               // System.Web.HttpContext.Current.Response.ContentType = "application/vnd.xls";

               // System.IO.StringWriter stringWrite = new System.IO.StringWriter();

               // System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
               ////gv.DataBind();
               // int i = gv.Rows.Count;
               // gv.RenderControl(htmlWrite);

               // System.Web.HttpContext.Current.Response.Write(stringWrite.ToString());

               // System.Web.HttpContext.Current.Response.End();


               //--------------------------------------------------------------
                //gv.AllowPaging = false;
                gv.Parent.Controls.Add(frm);
                frm.Controls.Add(gv);

                frm.RenderControl(htmlWrite);
                System.Web.HttpContext.Current.Response.Write(stringWrite.ToString() + "</body></html>");

            }
            catch (Exception ex)
            {
            }
            finally
            {
                System.Web.HttpContext.Current.Response.End();
            }
        }

    }

}
