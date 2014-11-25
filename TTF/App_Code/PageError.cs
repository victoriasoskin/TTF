using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

using System.Configuration;

namespace TTF.App_Code
{
    public class PageError
    {

        public static void WriteErrorLog(bool bSurvey = false, string SpecComment ="")
        {
            //if (Strings.LCase(Strings.Left(HttpContext.Current.Request.Url.AbsoluteUri, 16)) == "http://localhost")
            //    return;
            Exception ex = HttpContext.Current.Server.GetLastError();
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["Book10PE"].ConnectionString;
            System.Data.SqlClient.SqlConnection dbConnection = new System.Data.SqlClient.SqlConnection(connStr);
            string sSessID = HttpContext.Current.Session.SessionID;
            dynamic sHostIP = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName()).GetValue(1).ToString();
            string s = HttpContext.Current.Request.Url.AbsoluteUri;
            string sWeb = null;
            string sBrswr = WhichBrowser();
            string sErr = "Unidentified Error";
            if (ex != null)
            {
                sErr = ex.ToString();
                // HttpContext.Current.Error.Message 'If(Server.GetLastError() Is Nothing, vbNullString, Server.GetLastError().InnerException.ToString.Replace("'", "''"))
            }
            string sU = HttpContext.Current.Session["UserID"] == null ? "0" : HttpContext.Current.Session["UserID"].ToString();

            try
            {

            }
            catch (Exception exxxx)
            {
            }
            SqlCommand cd = new SqlCommand("INSERT INTO book10.dbo.AA_errLog(ERRTime,UserID,errMessage,Page,SessionID,ComputerName,SourceID,Browser,SpecComment) VALUES(GETDATE()," + "0" + sU + ",'" + sErr.Replace("'", "''") + "','" + s.Replace("'", "''") +  "','" + sSessID + "','" + sHostIP + "',0,'" + sBrswr + "'," + (SpecComment == "" ? "NULL" : "'" + SpecComment.Replace("'", "''") + "'") + ")", dbConnection);
            dbConnection.Open();
            try
            {
                cd.ExecuteNonQuery();
            }
            catch (Exception exx)
            {
                throw exx;
            }
            finally
            {
                dbConnection.Close();
            }
            if (!bSurvey)
            {
                s = "<html><head><title></title></head><body style='direction:rtl;'><div style='position:absolute;top:30%;right:40%;height:150px;width:250px;background-color:#DDDDDD;border:2px outset #AAAAAA;text-align:center;'><br />";
                if (sU == "0")
                {
                    sWeb = System.Configuration.ConfigurationManager.AppSettings["ReturnToEntry"];
                    s += "לצערנו, נותקת מהמערכת<br />עליך לחזור ולהתחבר למערכת.<br /><br /><a href='" + sWeb + "'>לדף הכניסה למערכת</a></div></body></html>";
                }
                else
                {
                    sWeb = System.Configuration.ConfigurationManager.AppSettings["ReturnToDefault"];
                    s += "לצערנו ארעה תקלה. <br />התקלה דווחה לצוות המערכת. <br /><br /><br /><a href='" + sWeb + "'>בחזרה למערכת</a></div></body></html>";
                }
                HttpContext.Current.Server.ClearError();
                HttpContext.Current.Response.Write(s);
            }


        }
        public static void WriteEntryLog()
        {
            string connStr = ConfigurationManager.ConnectionStrings["Book10PE"].ConnectionString;
            System.Data.SqlClient.SqlConnection dbConnection = new System.Data.SqlClient.SqlConnection(connStr);
            string sSessID = HttpContext.Current.Session.SessionID;
            dynamic sHostIP = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName()).GetValue(1).ToString();

            string client_ip = HttpContext.Current.Request.UserHostAddress;

            string sBrswr = WhichBrowser();
            string s = HttpContext.Current.Request.Url.AbsoluteUri;
            string sWeb = HttpContext.Current.Request.Url.Host;
            string sU = HttpContext.Current.Session["UserID"] == null ? "0" : HttpContext.Current.Session["UserID"].ToString();
            SqlCommand cd = new SqlCommand("", dbConnection);
            string sql = "INSERT INTO book10.dbo.AA_Log(LogTime,UserID,Page,SessionID,ClientIP,HostIP,SourceID,Browser)VALUES(GETDATE()," + "0" + sU + ",'" + s.Replace("'", "''") +  "','" + sSessID + "','" + client_ip + "','" + sHostIP + "',0,'" + sBrswr + "')";
            cd.CommandText = sql;
            dbConnection.Open();
            try
            {
                cd.ExecuteNonQuery();
            }
            catch (Exception exx)
            {
                throw exx;
            }
            finally
            {
                dbConnection.Close();
            }
        }
        public static string WhichBrowser()
        {
            string s = "";
            try
            {
                var _with1 = HttpContext.Current.Request.Browser;
                s += "Browser Capabilities\n"; 
                s += "Type = " + _with1.Type ;
                s += "\nName = " + _with1.Browser;
                s += "\nVersion = " + _with1.Version ;
                s += "\nMajor Version = " + _with1.MajorVersion;
                s += "\nMinor Version = " + _with1.MinorVersion;
                s += "\nPlatform = " + _with1.Platform ;
                s += "\nIs Beta = " + _with1.Beta ;
                s += "\nIs Crawler = " + _with1.Crawler ;
                s += "\nIs AOL = " + _with1.AOL ;
                s += "\nIs Win16 = " + _with1.Win16 ;
                s += "\nIs Win32 = " + _with1.Win32;
                s += "\nSupports Frames = " + _with1.Frames;
                s += "\nSupports Tables = " + _with1.Tables ;
                s += "\nSupports Cookies = " + _with1.Cookies ;
                s += "\nSupports VBScript = " + _with1.VBScript ;
                s += "\nSupports JavaScript = " + _with1.EcmaScriptVersion.ToString() ;
                s += "\nSupports Java Applets = " + _with1.JavaApplets ;
                s += "\nSupports ActiveX Controls = " + _with1.ActiveXControls ;
                s += "\nSupports JavaScript Version = " + HttpContext.Current.Request.Browser["JavaScriptVersion"] ;
            }
            catch (Exception ex)
            {
            }
            return s;
        }
    }
}