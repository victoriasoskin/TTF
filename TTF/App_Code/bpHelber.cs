using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;

namespace TTF.App_Code
{
    public class bpHelber
    {

        #region definitions and initialization

        #region properties definitions

        public Int64 CustomerId { get { return getCustomerId(); } }
        public int UserId { get { return getUserId(); } }
        public int Frm_CatId { get { return getFrm_CatId(); } }
        public int CustEventTypeId { get; set; }
        public int OldEventId { get; set; }
        public int EventId { get { return getEventId(); } }
        public int MPEventId { get { return getMPEventId(); } }
        public int FormId { get; set; }
        public bool CanUpdate { get; set; }
        public DateTime EventDate { get; set; }
        public int FormTypeId { get; set; }
        public int WpId { get; set; }
        public int MpId { get; set; }
        public string text { get; set; }
        public int DetailsId { get; set; }
        public Page page { get; set; }
        public int WwId { get; set; }
        public int MasterEventId { get { return GetMasterEventId(); }  }      //eventTypeId that comes before the current event. 
        //  for Example: 132 is event for folows in Diur so his MasterEventId is 131 becouse it(132) relise on 131 

        #endregion

        #region collection definitions

        public global::System.Web.UI.WebControls.ListView lvWP;
        public global::System.Web.UI.WebControls.ListView lvMP;
        public global::System.Web.UI.WebControls.ListView LVWeeklyPlan;
        public global::System.Web.UI.WebControls.SqlDataSource DSWeeklyPlan;
        public global::System.Web.UI.HtmlControls.HtmlGenericControl divWeeklytimetable;
        public global::System.Web.UI.WebControls.HiddenField hdnCurrentEventId;
        static public List<KeyValuePair<string, string>> wpIndexesColumsctl = new List<KeyValuePair<string, string>>();
        static public List<KeyValuePair<string, string>> wpDetailsColumsctl = new List<KeyValuePair<string, string>>();
        static public List<KeyValuePair<string, string>> wpReportHeaderColumsctl = new List<KeyValuePair<string, string>>();
        static public List<KeyValuePair<string, string>> wpReportsColumsctl = new List<KeyValuePair<string, string>>();
        static public List<KeyValuePair<string, string>> wpColumsctl = new List<KeyValuePair<string, string>>();
        static public List<KeyValuePair<string, string>> mpColumsctl = new List<KeyValuePair<string, string>>();
        static public List<KeyValuePair<string, string>> ctlParam = new List<KeyValuePair<string, string>>();
        static public List<KeyValuePair<string, string>> ddls0Item = new List<KeyValuePair<string, string>>();
        static public List<KeyValuePair<string, string>> ctlOtherTB = new List<KeyValuePair<string, string>>();
        static public List<KeyValuePair<string, string>> ctlSpecial = new List<KeyValuePair<string, string>>();

        #endregion

        #region init Classes

        TTF.App_Code.Util u = new TTF.App_Code.Util();
        TTF.App_Code.bpClass bp = new TTF.App_Code.bpClass();
        TTF.App_Code.TT_WP Wp = new TTF.App_Code.TT_WP();
        TTF.App_Code.TT_WP_Indexes Ind = new TTF.App_Code.TT_WP_Indexes();
        TTF.App_Code.TT_WP_Details Det = new TTF.App_Code.TT_WP_Details();
        TTF.App_Code.TT_Header hdr = new TTF.App_Code.TT_Header();
        TTF.App_Code.TT_WP_Reports rep = new TTF.App_Code.TT_WP_Reports();
        TTF.App_Code.TT_MP Mp = new TTF.App_Code.TT_MP();
        TTF.App_Code.TT_WP_Week wk = new TTF.App_Code.TT_WP_Week();

        Control cRoot;
        private int IndexId;
        
        public bpHelber(Page pg, int ft, int ce)
        {
            FormTypeId = ft;
            CustEventTypeId = ce;
            cRoot = (Control)page;
            page = pg;
        }

        #endregion

        #region load and work with collections

        internal void initForm(int FormTypeId)
        {

            loadCollections(FormTypeId);

        }

        internal void initForm()
        {

            loadCollections();

        }
        private void loadCollections()
        {

            //clear original
            wpColumsctl.Clear();
            mpColumsctl.Clear();
            wpIndexesColumsctl.Clear();
            wpDetailsColumsctl.Clear();
            ctlParam.Clear();
            ddls0Item.Clear();
            ctlOtherTB.Clear();
            ctlSpecial.Clear();
            //-----------13/11/14 Vicky ------------------
            // checking formtypeid 

            //   if (ctlParam.Count == 0)
            //        {
            string sql = string.Format("SELECT c.Id,c.Name,k.[Key],k.Value " +
                                    "FROM B10Sec.dbo.TT_Collecions c " +
                                    "LEFT OUTER JOIN B10Sec.dbo.TT_KeyValuePair k ON k.CollectionId=c.Id " +
                                    "WHERE k.FormTypeId={0}", FormTypeId);
            DataTable dtx = u.selectDBQuesry(sql);
            foreach (DataRow r in dtx.Rows)
            {
                switch ((string)r["Name"])
                {
                    case "wpColumsctl":
                        wpColumsctl.Add(new KeyValuePair<string, string>((string)r["Key"], (string)r["Value"]));
                        break;
                    case "mpColumsctl":
                        mpColumsctl.Add(new KeyValuePair<string, string>((string)r["Key"], (string)r["Value"]));
                        break;
                    case "wpIndexesColumsctl":
                        wpIndexesColumsctl.Add(new KeyValuePair<string, string>((string)r["Key"], (string)r["Value"]));
                        break;
                    case "wpDetailsColumsctl":
                        wpDetailsColumsctl.Add(new KeyValuePair<string, string>((string)r["Key"], (string)r["Value"]));
                        break;
                    case "ctlParam":
                        ctlParam.Add(new KeyValuePair<string, string>((string)r["Key"], (string)r["Value"]));
                        break;
                    case "ddls0Item":
                        ddls0Item.Add(new KeyValuePair<string, string>((string)r["Key"], (string)r["Value"]));
                        break;
                    case "ctlOtherTB":
                        ctlOtherTB.Add(new KeyValuePair<string, string>((string)r["Key"], (string)r["Value"]));
                        break;
                    case "ctlSpecial":
                        ctlSpecial.Add(new KeyValuePair<string, string>((string)r["Key"], (string)r["Value"]));
                        break;
                }
            }
            dtx.Dispose();
            // }
        }

        // 16.11.14 by Vicky - for school use
        private void loadCollections(int formtypeId)
        {

            //clear original
            wpColumsctl.Clear();
            mpColumsctl.Clear();
            wpIndexesColumsctl.Clear();
            wpDetailsColumsctl.Clear();
            ctlParam.Clear();
            ddls0Item.Clear();
            ctlOtherTB.Clear();
            ctlSpecial.Clear();
            //-----------13/11/14 Vicky ------------------
            // checking formtypeid 
            
         //   if (ctlParam.Count == 0)
    //        {
                string sql = string.Format("SELECT c.Id,c.Name,k.[Key],k.Value " +
                                        "FROM B10Sec.dbo.TT_Collecions c " +
                                        "LEFT OUTER JOIN B10Sec.dbo.TT_KeyValuePair k ON k.CollectionId=c.Id " +
                                        "WHERE k.FormTypeId={0}", FormTypeId);
                DataTable dtx = u.selectDBQuesry(sql);
                foreach (DataRow r in dtx.Rows)
                {
                    switch ((string)r["Name"])
                    {
                        case "wpColumsctl":
                            wpColumsctl.Add(new KeyValuePair<string, string>((string)r["Key"], (string)r["Value"]));
                            break;
                        case "mpColumsctl":
                            mpColumsctl.Add(new KeyValuePair<string, string>((string)r["Key"], (string)r["Value"]));
                            break;
                        case "wpIndexesColumsctl":
                            wpIndexesColumsctl.Add(new KeyValuePair<string, string>((string)r["Key"], (string)r["Value"]));
                            break;
                        case "wpDetailsColumsctl":
                            wpDetailsColumsctl.Add(new KeyValuePair<string, string>((string)r["Key"], (string)r["Value"]));
                            break;
                        case "ctlParam":
                            ctlParam.Add(new KeyValuePair<string, string>((string)r["Key"], (string)r["Value"]));
                            break;
                        case "ddls0Item":
                            ddls0Item.Add(new KeyValuePair<string, string>((string)r["Key"], (string)r["Value"]));
                            break;
                        case "ctlOtherTB":
                            ctlOtherTB.Add(new KeyValuePair<string, string>((string)r["Key"], (string)r["Value"]));
                            break;
                        case "ctlSpecial":
                            ctlSpecial.Add(new KeyValuePair<string, string>((string)r["Key"], (string)r["Value"]));
                            break;
                    }
                }
                dtx.Dispose();
           // }
        }

        public void SetSelectList(ListViewItem lvi, DropDownList ddl)
        {

            // get ccontrol parameter

            string s = getControlParameter(ctlParam, ddl.ID, lvi);

            if (s != null)
            {

                // get zero item text

                string sZ = getkplValue(ddls0Item, ddl.ID);

                DataTable dt = bp.getbpClass(s);

                ddl.DataTextField = "Name";
                ddl.DataValueField = "id";
                ddl.Items.Clear();
                ListItem li = new ListItem(sZ, string.Empty);
                ddl.Items.Add(li);

                ddl.DataSource = dt;
            }
        }

        private string getControlParameter(List<KeyValuePair<string, string>> ctlParam, string ddlId, ListViewItem lvi)
        {
            string s = getkplValue(ctlParam, ddlId);
            if (s != null)
            {
                if (s.Substring(0, 1) == "_")
                {
                    s = s.Substring(1);
                }
                else
                {
                    DropDownList ddlp = (DropDownList)lvi.FindControl(s);
                    s = (ddlp.SelectedValue == string.Empty ? "NULL" : ddlp.SelectedValue);
                }
            }
            return s;
        }


        public string getkplValue(List<KeyValuePair<string, string>> l, string id)
        {
            foreach (KeyValuePair<string, string> vp in l) if (vp.Key == id) return vp.Value;
            return null;
        }

        public string getkplKey(List<KeyValuePair<string, string>> l, string id)
        {
            foreach (KeyValuePair<string, string> vp in l) if (vp.Value == id) return vp.Key;
            return null;
        }

        public void datasBindCasscadingCtrl(DropDownList ddl, ListViewItem lvi)
        {
            string s = getkplKey(ctlParam, ddl.ID);
            if (s != null)
            {
                ddl = (DropDownList)lvi.FindControl(s);
                if (ddl != null)
                {
                    ddl.ClearSelection();
                    ddl.DataBind();
                }
            }

        }

        public void showTextBoxifOther(DropDownList ddl, ListViewItem lvi)
        {
            string s = getkplValue(ctlOtherTB, ddl.ID);
            if (s != null)
            {
                TextBox tb = (TextBox)lvi.FindControl(s);
                if (tb != null)
                {
                    s = "var e = document.getElementById('" + ddl.ClientID + "'); var s = e.options[e.selectedIndex].text; " +
                                       "var tb = document.getElementById('" + tb.ClientID + "'); " +
                                       "if(s.substring(s.length-5) == '(...)') tb.style.display = 'block'; else {tb.style.display = 'none'; tb.value = ''; } ";
                    ddl.Attributes.Add("onchange", s);

                    //s=ddl.SelectedItem.Text;
                    //s= "display: '" + (s.Length>=5 && s.Substring(s.Length-5)=="(...)" ? "block'" : "none'") + ";";
                    //tb.Attributes.Add("style", s);
                }
            }
        }

        internal void spcialActs(DropDownList ddl, ListViewItem lvi)
        {
            string s = getkplValue(ctlSpecial, ddl.ID);
            switch (s)
            {
                case "SetTarget":

                    break;
            }
        }

        #endregion

        #region header

        public void getHeader(ListView lv)
        {
            lv.DataSource = hdr.getHeader(EventId, CustomerId, Frm_CatId);
            lv.DataBind();
        }

        #endregion

        #region doPostBackActions

        public void selectAction()
        {
            string sT = HttpContext.Current.Request.Params.Get("__EVENTTARGET");
            string sE = HttpContext.Current.Request.Params.Get("__EVENTARGUMENT");
            if (sT != null && sT.Length > 3 && sT.Substring(0, 3) == "+_+")
            {
                ListViewItem lvi;
                ListView subLv;
                string[] s = sE.Split('|');
                switch (sT)
                {
                    case "+_+Index_Edit":
                        {
                            if (s[0] == "-1")
                                lvi = lvWP.InsertItem;
                            else
                                lvi = lvWP.Items[int.Parse(s[0])];

                            subLv = (ListView)lvi.FindControl("dlIndexes");
                            subLv.EditIndex = int.Parse(s[1]);
                            break;
                        }
                    case "+_+Index_Update":
                        {
                            UpdeteIndexes(sE);
                            break;
                        }
                    case "+_+Details_Insert":
                        {
                            break;
                        }
                    case "+_+Details_Edit":
                        {
                            if (s[0] == "-1")
                                lvi = lvWP.InsertItem;
                            else
                                lvi = lvWP.Items[int.Parse(s[0])];

                            subLv = (ListView)lvi.FindControl("dlDetails");
                            subLv.EditIndex = int.Parse(s[1]);
                            break;
                        }
                    case "+_+Details_Update":
                        {
                            break;
                        }
                    case "+_+Details_Delete":
                        {
                            break;
                        }
                    case "+_+week":
                        {
                            switch (s[0])
                            {
                                case "d":
                                    {
                                        deleteWeeklyPlan(s);
                                        break;
                                    }
                                default:
                                    {
                                        updateWeeklyPlan(s);
                                        break;
                                    }
                            }

                            LVWeeklyPlan.DataBind();
                            divWeeklytimetable.Attributes.Remove("class");
                            divWeeklytimetable.Attributes.Add("class", "wkshow");
                            break;
                        }
                }
            }

        }

        #endregion

        #endregion

        #region Master Plan

        internal void getMP(int p, ListView dl)
        {

            //done by sqlDataSource

            //DataTable dt = Det.getDetails(p); 
            //dl.DataSource = dt;
            //dl.DataBind();
        }  //done by sqldatasourse

        private int getMpId(int lvIndex)
        {
            int i = 0;

            if (lvMP != null)
            {
                ListViewItem lvi;
                if (lvIndex < 0) lvi = lvMP.InsertItem; else lvi = lvMP.Items[lvIndex];
                HiddenField hdn = (HiddenField)lvi.FindControl("hdnMpId");
                if (hdn.Value == string.Empty)
                {
                    i = UpdateMp(lvi);
                    hdn.Value = i.ToString();
                }
                else
                {
                    i = int.Parse(hdn.Value);
                    Mp.loadMP(i);
                }
                FormId = Mp.FormId;
            }
            return i;
        }

        internal int UpdateMp(ListViewItem lvi)
        {
            Mp.CustomerId = CustomerId;

            Mp.EventId = EventId;
            Mp.FormId = FormId;
            Mp.UserId = UserId;

            Mp.SupportCoordinator = getColStrValue("SupportCoordinator", mpColumsctl, lvi);
            Mp.Supporter = getColStrValue("Supporter", mpColumsctl, lvi);
            Mp.Location = getColStrValue("Location", mpColumsctl, lvi);
            Mp.FamilyStatusId = getColIntValue("FamilyStatusId", mpColumsctl, lvi);
            Mp.LayerId = getColIntValue("LayerId", mpColumsctl, lvi);
            Mp.ClassId = getColIntValue("ClassId", mpColumsctl, lvi);
            Mp.Strength = getColStrValue("Strength", mpColumsctl, lvi);
            Mp.Hobby = getColStrValue("Hobby", mpColumsctl, lvi);
            Mp.PreviousFrame = getColStrValue("PreviousFrame", mpColumsctl, lvi);
            Mp.OtherInfo = getColStrValue("OtherInfo", mpColumsctl, lvi);

            openNewTTPlan();

            Mp.EventId = EventId;
            Mp.FormId = FormId;
            Mp.CustEventTypeId = CustEventTypeId;
            HiddenField hdn = (HiddenField)lvi.FindControl("hdnMpId");
            MpId = (hdn.Value == string.Empty ? 0 : int.Parse(hdn.Value));
            MpId = Mp.UpdateRecord(MpId);
            hdn.Value = MpId.ToString();
            
            return MpId;
        }

        #endregion

        #region work plan

        internal void getWp(Int64 p, ListView dl, List<KeyValuePair<string, string>> ctlParam)
        {
            DataTable dt = Wp.getWP(p, 0);
            dl.DataSource = dt;
            dl.DataBind();
        }

        public int getWpId(int lvIndex)
        {
            int i = 0;
            HiddenField hdn = null;
            ListViewItem lvi = (lvIndex < 0 ? lvWP.InsertItem : lvWP.Items[lvIndex]);
            if (lvi != null)
            {
                hdn = (HiddenField)lvi.FindControl("hdnWpId");
                if (hdn != null) i = (hdn.Value == string.Empty ? 0 : int.Parse(hdn.Value));
            }
            if (i == 0)
            {
                hdn = (HiddenField)u.FindControlRecursive(page, "hdnCurrentWpId");
                i = (hdn.Value == string.Empty ? 0 : int.Parse(hdn.Value));
            }

            if (i != 0)
            {
                Wp.loadWP(i);
                FormId = Wp.FormId;
                return i;
            }

            Wp.CustomerId = CustomerId;
            if (lvWP != null)
            {
                if (lvIndex < 0) lvi = lvWP.InsertItem; else lvi = lvWP.Items[lvIndex];
                bool b = lvi != null;
                if (b)
                {
                    hdn = (HiddenField)lvi.FindControl("hdnWpId");
                    b = hdn.Value != string.Empty;
                }

                if (!b)
                {
                    i = UpdateWp(lvi);
                    hdn.Value = i.ToString();
                }
                else
                {
                    i = int.Parse(hdn.Value);
                    Wp.loadWP(i);
                }
                FormId = Wp.FormId;
            }
            return i;
        }

        internal int DeleteWp(ListViewItem lvi)
        {
            HiddenField hdn = (HiddenField)lvi.FindControl("hdnWpId");
            if (hdn.Value != string.Empty) Wp.deleteRecord(int.Parse(hdn.Value));
            return 0;
        }

        internal int UpdateWp(ListViewItem lvi)
        {

            Wp.CustomerId = CustomerId;

            Wp.EventId = EventId;
            Wp.FormId = FormId;
            Wp.Frm_CatID = Frm_CatId;
            Wp.UserId = UserId;
            try
            {
                Wp.RangeId = getColIntValue("RangeId", wpColumsctl, lvi);
            }
            catch (Exception ex)
            {
                Wp.RangeId=9999;
            }
            try
            {
                Wp.SubjectId = getColIntValue("SubjectId", wpColumsctl, lvi);
            }
            catch (Exception ex)
            {
                Wp.SubjectId = 9999;
            }
             try
            {
                Wp.PurposeId = getColIntValue("PurposeId", wpColumsctl, lvi);
            }
            catch (Exception ex)
            {
                Wp.PurposeId = 9999;
            }
             try
            {
                Wp.Purpose = getColStrValue("Purpose", wpColumsctl, lvi);
            }
            catch (Exception ex)
            {
                Wp.Purpose = "999";
            }
             try
            {
                Wp.TargetId = getColIntValue("TargetId", wpColumsctl, lvi);
            }
            catch (Exception ex)
            {
                Wp.TargetId = 9999;
            }
             try
            {
                Wp.WeightId = getColIntValue("WeightId", wpColumsctl, lvi);
            }
            catch (Exception ex)
            {
                Wp.WeightId = 9999;
            }
            

            openNewTTPlan();

            Wp.EventId = EventId;
            Wp.FormId = FormId;
            HiddenField hdn = (HiddenField)lvi.FindControl("hdnWpId");
            WpId = (hdn.Value == string.Empty ? 0 : int.Parse(hdn.Value));

            if (WpId != 0) copytoNewVersion("TT_WP", "WpId", WpId);

            WpId = Wp.UpdateRecord(WpId);
            hdn.Value = WpId.ToString();
            return WpId;
        }

        #endregion

        #region Details
        //for reports
        internal void getDetails(int p, ListView dl )
        {
            DataTable dt = Det.getDetails(p, EventId);
            dl.DataSource = dt;
            try
            {
                dl.DataBind();
            }
            catch (Exception ex)
            {
                dl.EditIndex = 0;
                dl.DataBind();
            }
        }
        //for reports
        internal void getWPDetails(int p, ListView dl  )
        {
            int evnt = getWPEventId();
            DataTable dt = Det.getDetails(p, evnt);
            dl.DataSource = dt;
            try
            {
                dl.DataBind();
            }
            catch (Exception ex)
            {
                dl.EditIndex = 0;
                dl.DataBind();
            }
        }


        internal int DeleteDetails(ListViewItem lvi)
        {
            HiddenField hdn = (HiddenField)lvi.FindControl("hdnDetId");
            if (hdn.Value != string.Empty)
            {
                Det.UserId = UserId;
                Det.deleteRecord(int.Parse(hdn.Value));
            }
            return 0;
        }

        internal void UpdeteDetails(ListViewItem lvi, int lvIndex)
        {

            WpId = getWpId(lvIndex); // Get WpId // if not found save and get it

            Det.CustomerId = CustomerId;
            Det.EventId = EventId;
            Det.FormId = FormId;
            Det.WPId = WpId;
            Det.UserId = UserId;
            Det.Frm_CatId = Frm_CatId;

            openNewTTPlan();


            Det.EventId = EventId;
            Det.FormId = FormId;

            int Id = 0;
            HiddenField hdn = (HiddenField)lvi.FindControl("hdnId");
            if (hdn != null) Id = (hdn.Value == string.Empty ? 0 : int.Parse(hdn.Value));

            bool b = false;
            if (Id != 0) b = copytoNewVersion("TT_WP_Details", "DetailsId", Id);

            if (DetailsId != 0) Id = DetailsId;
            Det.WPId = WpId;

            Det.text = getColStrValue("Text", wpDetailsColumsctl, lvi);
            try
            {
                Det.text1 = getColStrValue("Text1", wpDetailsColumsctl, lvi);
            }
            catch (Exception ex) { }
            try
            {

                Det.text2 = getColStrValue("Text2", wpDetailsColumsctl, lvi);
            }
            catch (Exception ex) { }
            Det.PeriodId = getColIntValue("PeriodId", wpDetailsColumsctl, lvi);
            Det.PeriodText = getColStrValue("PeriodText", wpDetailsColumsctl, lvi);
            Det.AmountId = getColIntValue("AmountId", wpDetailsColumsctl, lvi);
            Det.FrequencyId = getColIntValue("FrequencyId", wpDetailsColumsctl, lvi);
            Det.LengthId = getColIntValue("LengthId", wpDetailsColumsctl, lvi);
            Det.HelperId = getColIntValue("HelperId", wpDetailsColumsctl, lvi);
            Det.HelperText = getColStrValue("HelperText", wpDetailsColumsctl, lvi);
            try
            {
                Det.SupporterName = getColStrValue("SupporterName", wpDetailsColumsctl, lvi);
            }
            catch (Exception ex) { }
            Det.UpdateRecord(Id);

            if (b && lvIndex >= 0)
            {
                lvWP.DataBind();
                lvWP.EditIndex = lvIndex;
            }

            return;
        }

        #endregion

        #region weekly plan

        private void updateWeeklyPlan(string[] s)
        {
            if (s[0] != string.Empty)
            {
                wk.FormId = FormId;
                wk.EventId = EventId;
                wk.CustomerId = CustomerId;
                wk.Frm_CatId = Frm_CatId;
                wk.DayId = char.Parse(s[1]);
                wk.DetailsId = int.Parse(s[0]);
                wk.UserId = UserId;

                openNewTTPlan();

                if (DetailsId != 0) copytoNewVersion("TT_WP_Details", "DetailsId", DetailsId);  // no updating

                wk.updateRecord();
            }
        }
        private void deleteWeeklyPlan(string[] s)
        {
            int Id = (int)Math.Round(double.Parse(s[2]) / 100000);
            openNewTTPlan();
            if (Id != 0) copytoNewVersion("TT_WP_Week", "WwId", DetailsId);  // no updating

            wk.deleteRecord(Id);
        }

        #endregion

        #region Indexes

        internal void getSupportIndexes(int p, ListView lv)
        {
            int r = 0;
            string s = getkplValue(ctlParam, lv.ID);
            if (s == null)
            {
                loadCollections();
                s = getkplValue(ctlParam, lv.ID);
            }
            if (s.Substring(0, 1) == "_") r = int.Parse(s.Substring(1));
            DataTable dt = Ind.getIndexes(p, r);
            lv.DataSource = dt;
            //dl.EditIndex = 0;
            lv.DataBind();
        }

        internal void UpdeteIndexes(string sE)
        {

            // If we don't Have FatherRecord - Create One 

            string[] sFields = sE.Split('|');
            int lvIndex = int.Parse(sFields[0]);

            for (int i = 2; i < sFields.Length; i++)
            {
                string[] sVals = sFields[i].Split('=');
                switch (sVals[0])
                {
                    case "IndexId":
                        Ind.IndexId = int.Parse(sVals[1]);
                        break;
                    case "WpId":
                        Ind.WPId = int.Parse(sVals[1]);
                        break;
                    case "val":
                        Ind.val = int.Parse(sVals[1]);
                        break;
                    case "Text":
                        Ind.text = sVals[1].Replace("'", "''");
                        break;
                }
            }


            Ind.WPId = getWpId(lvIndex); // Get WpId // if not found save and get it

            Ind.FormId = FormId;
            Ind.EventId = EventId;
            Ind.CustomerId = CustomerId;
            Ind.Frm_CatId = Frm_CatId;
            Ind.UserId = UserId;

            ListViewItem lvi;
            if (sFields[0] == "-1")
                lvi = lvWP.InsertItem;
            else
                lvi = lvWP.Items[int.Parse(sFields[0])];

            ListView lv = (ListView)lvi.FindControl("dlIndexes");
            Ind.UpdateRecord();
            lv.EditIndex = -1;
            lv.DataBind();
        }

        internal void InsertFirstTimeIndexes(int WpId, ListViewItem lvi)
        {
            TextBox tp2 = (TextBox)lvi.FindControl("tbTxtIndexp2");
            TextBox tp1 = (TextBox)lvi.FindControl("tbTxtIndexp1");
            TextBox tp0 = (TextBox)lvi.FindControl("tbTxtIndex00");
            TextBox tm1 = (TextBox)lvi.FindControl("tbTxtIndexm1");
            TextBox tm2 = (TextBox)lvi.FindControl("tbTxtIndexm2");
            Ind.FormId = FormId;
            Ind.EventId = EventId;
            Ind.WPId = WpId;
            Ind.CustomerId = CustomerId;
            Ind.Frm_CatId = Frm_CatId;
            Ind.UserId = UserId;

            Ind.insertFirstTimeIndexes(WpId, tp2.Text, tp1.Text, tp0.Text, tm1.Text, tm2.Text);
        }

        internal void InsertDetailsInHelper(int p, ListViewItem lvi)
        {
            string txtbDetails = ((TextBox)lvi.FindControl("tbDetails")).Text;
            string period = ((DropDownList)lvi.FindControl("ddlPeriod")).SelectedValue;
            string frequency_1 = ((DropDownList)lvi.FindControl("ddlFrequesncy_1")).SelectedValue;
            string frequency_2 = ((DropDownList)lvi.FindControl("ddlFrequesncy_2")).SelectedValue;
            string lasting = ((DropDownList)lvi.FindControl("ddlLasting")).SelectedValue;
            string helper = ((DropDownList)lvi.FindControl("ddlHelper")).SelectedValue; 
            string helperName = ((TextBox)lvi.FindControl("tbHelper")).Text;
            if (helperName=="")
            {
                helperName = "לא ידוע";
            }
            Det.FormId = FormId; Det.EventId = EventId;
            Det.WPId = WpId;
            Det.CustomerId = CustomerId;
            Det.Frm_CatId = Frm_CatId;
            Det.UserId = UserId;
            //Det.InsertDetailsInTT_WP_Details(WpId, txtbDetails, period, frequency_1, frequency_2, lasting, helper, helperName);
            Det.InsertDetailsInTT_WP_Details(WpId, txtbDetails, period, frequency_1, frequency_2, lasting, helper, helperName);
            
        }
        //    string sp2 = tp2.Text;
        //    string sp1 =  tp1.Text;
        //    string s0 =  tp0.Text ;
        //    string sm1 =tm1.Text ;
        //    string sm2 = tm2.Text;
        //    string sqlp2 = string.Format("insert into [Book10].[dbo].[TT_WP_Indexes]  (FormId ,EventId,wpid, text ,Val,Status,CustomerId,Frm_CatId, UserId) values  ( '{0}' , '{1}' ,'{2}','{3}', '{4}' , '{5}' ,'{6}','{7}','{8}' )"
        //                ,FormId, EventId, WpId, sp2, 4,0,CustomerId,Frm_CatId, LoadTime ,UserId);
        //    string sqlp1 = string.Format("insert into [Book10].[dbo].[TT_WP_Indexes]  (FormId ,EventId,wpid, text ,Val,Status,CustomerId,Frm_CatId, UserId) values  ( '{0}' , '{1}' ,'{2}','{3}', '{4}' , '{5}' ,'{6}','{7}','{8}' )"
        //                , FormId, EventId, WpId, sp1, 3, 0, CustomerId, Frm_CatId, UserId);
        //    string sql0 = string.Format("insert into [Book10].[dbo].[TT_WP_Indexes]   (FormId ,EventId,wpid, text ,Val,Status,CustomerId,Frm_CatId, UserId) values  ( '{0}' , '{1}' ,'{2}','{3}', '{4}' , '{5}' ,'{6}','{7}' ,'{8}')"
        //                 , FormId, EventId, WpId, s0, 2, 0, CustomerId, Frm_CatId, UserId);
        //    string sqlm1 = string.Format("insert into [Book10].[dbo].[TT_WP_Indexes]  (FormId ,EventId,wpid, text ,Val,Status,CustomerId,Frm_CatId, UserId) values  ( '{0}' , '{1}' ,'{2}','{3}', '{4}' , '{5}' ,'{6}','{7}','{8}' )"
        //                 , FormId, EventId, WpId, sm1, 1, 0, CustomerId, Frm_CatId, UserId);
        //    string sqlm2 = string.Format("insert into [Book10].[dbo].[TT_WP_Indexes]  (FormId ,EventId,wpid, text ,Val,Status,CustomerId,Frm_CatId, UserId) values  ( '{0}' , '{1}' ,'{2}','{3}', '{4}' , '{5}' ,'{6}','{7}' ,'{8}')"
        //                , FormId, EventId, WpId, sm1, 0, 0, CustomerId, Frm_CatId, UserId);
        //    var eventId = EventId;
            
        //    u.executeSql(sqlp2);
        //    u.executeSql(sqlp1);
        //    u.executeSql(sql0);
        //    u.executeSql(sqlm1);
        //    u.executeSql(sqlm2);
            
        //}

        #endregion

        #region Reports

        internal void updateReport(ListViewItem lvi)
        {
            rep.Frm_CatId = Frm_CatId;
            rep.CustomerId = CustomerId;

            openNewTTPlan();

            rep.EventId = EventId;
            rep.FormId = FormId;
            rep.UserId = UserId;

            HiddenField hdn = (HiddenField)lvi.FindControl("hdnWpId");
            WpId = (hdn.Value == string.Empty ? 0 : int.Parse(hdn.Value));
            rep.WPId = WpId;

            string[] ls = { "rbP2", "rbP1", "rb00", "rbM1", "rbM2" };
            int[] ils = { 2, 1, 0, -1, -2 };
            int val = -99;
            for (int i = 0; i < ls.Length; i++)
            {
                RadioButton rb = (RadioButton)lvi.FindControl(ls[i]);
                if (rb.Checked)
                {
                    val = ils[i];
                    rb.Checked = false;
                    break;
                }
            }

            rep.val = val;

            TextBox tb = (TextBox)lvi.FindControl("tbReport");
            text = tb.Text;
            tb.Text = null;
            rep.text = text;

            //int Id = rep.UpdateRecord();

        }

        internal string BuildReportHeader()
        {
            openNewTTPlan();

            rep.FormId = FormId;
            rep.EventId = EventId;
            rep.CustomerId = CustomerId;
            rep.WPId = WpId;
            rep.DetailsId = DetailsId;
            rep.Frm_CatId = Frm_CatId;
            rep.UserId = UserId;


            return rep.buildHeader();
        }

        internal string BuildReport(ListViewItem lvi, string valCntrlId, string textCntrlId, int questionId)
        {
            RadioButtonList rbl = (RadioButtonList)lvi.FindControl(valCntrlId);
            TextBox tb = (TextBox)lvi.FindControl(textCntrlId);
            string s = (tb == null ? string.Empty : tb.Text);
            string sql = rep.buildReport(questionId, int.Parse(rbl.SelectedValue), s);
            return sql;
        }

        internal bool updateReport(string sql)
        {
            rep.Frm_CatId = Frm_CatId;
            rep.CustomerId = CustomerId;

            openNewTTPlan();
            rep.EventId = EventId;
            rep.FormId = FormId;
            rep.UserId = UserId;

            int i = rep.UpdateRecord(sql);
            return true;
        }

        internal DataTable getMainReports(int WpId)
        {
            return rep.getMainReports(WpId);
        }

        internal DataTable getDetReports(int DetId)
        {
            return rep.getDetReports(DetId);
        }

        internal void DeleteMainReport(int Id)
        {
            rep.deleteReport(Id);
        }

        #endregion

        #region utility
        private int getColIntValue(string p, List<KeyValuePair<string, string>> l, ListViewItem lvi)
        {
            string s = getkplValue(l, p);
            if (s != null)
            {
                DropDownList ddl = (DropDownList)lvi.FindControl(s);
                if (ddl != null)
                {
                    if (ddl.SelectedValue == null || ddl.SelectedValue == string.Empty) return 0;
                    return int.Parse(ddl.SelectedValue);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        }
        private Int64 getColInt64Value(string p, List<KeyValuePair<string, string>> l, ListViewItem lvi)
        {
            string s = getkplValue(l, p);
            DropDownList ddl = (DropDownList)lvi.FindControl(s);
            if (ddl.SelectedValue == null) return 0;
            return Int64.Parse(ddl.SelectedValue);
        }
        private string getColStrValue(string p, List<KeyValuePair<string, string>> l, ListViewItem lvi)
        {
            string s = getkplValue(l, p);
            if (s==null)
            {
                 loadCollections();
                s = getkplValue(l, p);
            }
            if (s != null)
            {
                TextBox tb = (TextBox)lvi.FindControl(s);
                s = tb.Text;
            }
            return s;
        }
        public int getCurrentVersion()
        {
            object o = u.selectDBScalar(string.Format("DECLARE @FT nvarchar(50) " +
                                        "SELECT @FT = Name FROM book10_21.dbo.TT_FormTypes WHERE Id = {0} " +
                                        "EXEC TT_pVersionControl {1},{2},{3},@FT,{4}", FormTypeId, CustomerId, Frm_CatId, UserId, CustEventTypeId));
            Int32 i = 0;
            if (o != null) i = int.Parse(o.ToString());
            return i;
        }
        private bool copytoNewVersion(string tbl, string idName, int id)
        {
            string sql = u.getVersionDetails(tbl, idName, id);
            sql += string.Format(" IF ISNULL(@OldEvent,0) != {0} EXEC TT_pWP_NewVersion {0},{1},", EventId, UserId);

            switch (tbl)
            {
                case "TT_WP":
                    sql += string.Format("{0},NULL,NULL,NULL", id);
                    break;
                case "TT_WP_Details":
                    sql += string.Format("NULL,{0},NULL,NULL", id);
                    break;
                case "TT_WP_Indexes":
                    sql += string.Format("NULL,NULL,{0},NULL", id);
                    break;
                case "TT_WP_Week":
                    sql += string.Format("NULL,NULL,NULL,{0}", id);
                    break;
            }
            DataTable dt = u.selectDBQuesry(sql);
            bool b = false;
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                if (r["WpId"] != DBNull.Value) WpId = (int)r["WpId"];
                if (r["DetailsId"] != DBNull.Value) DetailsId = (int)r["DetailsId"];
                if (r["IndexId"] != DBNull.Value) IndexId = (int)r["IndexIdId"];
                if (r["WwId"] != DBNull.Value) WwId = (int)r["WwId"];
                b = true;
            }
            return b;
        }

        public int openNewTTPlan()
        {
            getCurrentVersion();

            if (EventId == 0)
            {
                string cr = "\n";
                string sql = "DECLARE @D datetime" + cr +
                            "DECLARE @CFrameManager nvarchar(50)" + cr +
                            "DECLARE @CustEventUpdateTypeID int" + cr +
                           "DECLARE @CustEventTypeID int" + cr +
                            "DECLARE @CustEventComment nvarchar(100)" + cr +
                           "SET @D = GETDATE()" + cr +
                            "SET @CustEventUpdateTypeID = 1" + cr +
                             "SELECT @CustEventTypeID = CustEventTypeID,@CustEventComment=Name FROM Book10_21.dbo.TT_FormTypes WHERE ID = " + FormTypeId.ToString() + cr +
                            "SELECT @CFrameManager = FrameManager FROM Book10_21.dbo.FrameList WHERE FrameID =" + Frm_CatId.ToString() + cr +
                             "INSERT INTO TT_Forms (FormTypeID,UserID,LoadTime) VALUES(" + FormTypeId.ToString() + "," + UserId.ToString() + ",@D)" + cr +
                            "SELECT ID,@CustEventTypeID as CustEventTypeID,@CFrameManager As FM,@CustEventUpdateTypeID as UT,@D As D,@CustEventComment Comment FROM TT_Forms WHERE Loadtime=@D AND UserID = " + UserId.ToString();
                DataTable dt = u.selectDBQuesry(sql);

                DataRow r = dt.Rows[0];

                EventDate = (DateTime)r["d"];
                FormId = (int)r["Id"];
                string FrameManager = (r.IsNull("FM") ? string.Empty : (string)r["FM"]);
                int ut = (int)r["UT"];
                string comment = r["Comment"].ToString();

                sql = string.Format("EXEC Book10_21.dbo.Cust_AddEvent	{0},{1},'{2:yyyy-MM-dd HH:mm:ss}','{3:yyyy-MM-dd}','{10} {4:yyyy-MM}',{5},'{6}',{7},{8},{9}",
                    CustomerId, CustEventTypeId, EventDate, EventDate, EventDate, Frm_CatId, FrameManager, UserId, ut, FormId, comment);

                Exception ex = u.executeSql(sql);

                if (ex == null) getCurrentVersion();

                // 


            }
            else
            {
                object o = u.selectDBScalar(string.Format("SELECT CustRelateId FROM Book10_21.dbo.CustEventList WHERE CustEventID = {0}", EventId));
                FormId = (o == DBNull.Value ? 0 : (int)o);
            }
            return EventId;
        }
        public string javaParam(ListViewItem lvi, string controlType, string fieldName)
        {
            switch (controlType)
            {
                case "hdn":
                    HiddenField hdn = (HiddenField)lvi.FindControl(controlType + fieldName);
                    if (hdn != null && hdn.Value != null && hdn.Value != string.Empty) return "|" + fieldName + "=" + hdn.Value;
                    break;
                case "tb":
                    break;
            }
            return string.Empty;
        }
        internal void displayIfValue(TextBox tb)
        {
            string s = getkplKey(ctlOtherTB, tb.ID);
            if (s != null)
            {
                DropDownList ddl = (DropDownList)((ListViewItem)tb.NamingContainer).FindControl(s);
                if (ddl != null)
                {
                    if (ddl.SelectedIndex >= 0)
                    {
                        s = ddl.SelectedItem.Text;
                        if (s.Length > 5)
                        {
                            s = s.Substring(s.Length - 5);
                            if (tb.Text != string.Empty || s == "(...)") tb.Attributes.Add("style", "display:block;");
                        }
                    }
                }
            }
        }
        public string CustomerAge()
        {
            return u.selectDBScalar(string.Format("select Book10_21.dbo.fnCustAge({0})", CustomerId)).ToString();
        }
        public DataTable p5Details(int eTypeId)
        {
            DataTable dt = new DataTable();
            dt = u.selectDBQuesry(String.Format("SELECT CustRelateId,CusteventDate " +
                                                        "FROM Book10_21.dbo.CustEventList " +
                                                        "WHERE CustEventId = (" +
                                                                            "SELECT Max(CustEventID) FROM Book10_21.dbo.CustEventList " +
                                                                            "WHERE CustomerId = {0} AND CustEventTypeId = {1})", CustomerId, eTypeId));
            return dt;
        }
        public int getUserId()
        {
            int i = (HttpContext.Current.Request.QueryString["E"] == null ? 0 : int.Parse(HttpContext.Current.Request.QueryString["E"].ToString()));
            // if (i == 0) HttpContext.Current.Response.Redirect("~/CustEventReport.Aspx");
            i = i / 62;
            return i;
        }
        public Int64 getCustomerId()
        {
            if (HttpContext.Current.Request.QueryString["CID"] == string.Empty)
            {
                 HttpContext.Current.Response.Redirect("~/CustEventReport.Aspx");
            }
            Int64 i = (HttpContext.Current.Request.QueryString["CID"] == null ? 0 : Int64.Parse(HttpContext.Current.Request.QueryString["CID"].ToString()));
            
            if (i == 0) HttpContext.Current.Response.Redirect("~/CustEventReport.Aspx");
            return i;
        }
        public int getFrm_CatId()
        {
            int i = (HttpContext.Current.Request.QueryString["F"] == null ? 0 : int.Parse(HttpContext.Current.Request.QueryString["F"].ToString()));
            return i;
        }
        public int getEventId()
        {
            //int TempEventId = int.Parse((HttpContext.Current.Request.QueryString["ET"].ToString()));
            int i = 0;
            if (HttpContext.Current.Request.QueryString["ID"] != null)
            {
                i = int.Parse(HttpContext.Current.Request.QueryString["ID"].ToString());
            }
            else
            {
               //if ((TempEventId == 142) || (TempEventId == 132))
               //  {
                    //object o = u.selectDBScalar(string.Format("SELECT Max(CustEventId) FROM Book10_21.dbo.CustEventList WHERE CustomerId = {0} AND CustEventTypeId = {1}", CustomerId, MasterEventId));
                    //if (o != DBNull.Value) i = int.Parse(o.ToString());
                //}
                //else
                //{
                    object o = u.selectDBScalar(string.Format("SELECT Max(CustEventId) FROM Book10_21.dbo.CustEventList WHERE CustomerId = {0} AND CustEventTypeId = {1}", CustomerId, CustEventTypeId));
                    if (o != DBNull.Value) i = int.Parse(o.ToString());
               // }
            }
            return i;
        }
        /// <summary>
        /// מחפש את האוונט שאליו נשאן האוונט הנוכחי .
        /// </summary>
        /// במידה וקיבלנו 142 יחזיר 141
        public int GetMasterEventId()
        {
            int test = int.Parse( HttpContext.Current.Request.QueryString["ET"].ToString() );
            return int.Parse(u.selectDBScalar(string.Format("select isnull(MasterEventTypeId,0) from book10_21.dbo.TT_FormTypes where CustEventTypeId ={0}",  test) ).ToString());

        }
        public int getMPEventId()
        {
            int i = 0;
            string sql = string.Format("select MasterEventTypeId from Book10_21.dbo.TT_FormTypes where CustEventTypeId = {0}", CustEventTypeId);
            int res = int.Parse(u.selectDBScalar(sql).ToString());
            if (HttpContext.Current.Request.QueryString["ID"] != null)
            {
                i = int.Parse(HttpContext.Current.Request.QueryString["ID"].ToString());
            }
            else
            {
                object o = u.selectDBScalar(string.Format("SELECT Max(CustEventId) FROM Book10_21.dbo.CustEventList WHERE CustomerId = {0} AND CustEventTypeId = {1}", CustomerId, res));
                if (o != DBNull.Value) i = int.Parse(o.ToString());
            }
            return i;
        }
        /// <summary>
        /// returns event -- used in hinuh
        /// </summary>
        /// <returns></returns>
        public int getWPEventId()
        {
            int i = 0;
            //int TempEventId = int.Parse((HttpContext.Current.Request.QueryString["ET"].ToString()));
            if (CustEventTypeId==175)
            {
                string sql = string.Format("select MasterEventTypeId from Book10_21.dbo.TT_FormTypes where CustEventTypeId = {0}", CustEventTypeId);
                int res = int.Parse(u.selectDBScalar(sql).ToString());
                i = 0;
                if (HttpContext.Current.Request.QueryString["ID"] != null)
                {
                    i = int.Parse(HttpContext.Current.Request.QueryString["ID"].ToString());
                }
                else
                {

                    object o = u.selectDBScalar(string.Format("SELECT Max(CustEventId) FROM Book10_21.dbo.CustEventList WHERE CustomerId = {0} AND CustEventTypeId = {1}", CustomerId, res));
                    if (o != DBNull.Value) i = int.Parse(o.ToString());

                }
                return i;
            }
            else { 
            i = 0;
            if (HttpContext.Current.Request.QueryString["ID"] != null)
            {
                i = int.Parse(HttpContext.Current.Request.QueryString["ID"].ToString());
            }
            else
            {
                
                  object o = u.selectDBScalar(string.Format("SELECT Max(CustEventId) FROM Book10_21.dbo.CustEventList WHERE CustomerId = {0} AND CustEventTypeId = {1}", CustomerId, CustEventTypeId));
                  if (o != DBNull.Value) i = int.Parse(o.ToString());
               
            }
            return i;
            }
        }
        /// <summary>
        /// returns main event  - used in diur&taasuka
        ///
        /// </summary>
        /// <returns></returns>
        public int getMasterEventId()
        {
            int TempEventId = int.Parse((HttpContext.Current.Request.QueryString["ET"].ToString()));

            int i = 0;
            if (HttpContext.Current.Request.QueryString["ID"] != null)
            {
                i = int.Parse(HttpContext.Current.Request.QueryString["ID"].ToString());
            }
            else
            {

                object o = u.selectDBScalar(string.Format("SELECT Max(CustEventId) FROM Book10_21.dbo.CustEventList WHERE CustomerId = {0} AND CustEventTypeId = {1}", CustomerId, MasterEventId));
                if (o != DBNull.Value) i = int.Parse(o.ToString());

            }
            return i;
        }
        public bool Isupdatable()
        {
            int i = 0;
            object o = u.selectDBScalar(string.Format("SELECT Max(CustEventId) FROM Book10_21.dbo.CustEventList WHERE CustomerId = {0} AND CustEventTypeId = {1}", CustomerId, CustEventTypeId));
            if (o != DBNull.Value) i = int.Parse(o.ToString());
            return i == EventId;
        }
        #endregion


        internal void InsertDetailsInHelper()
        {
            throw new NotImplementedException();
        }//

        public void lvIndexes(int result, string text)   
        { 
            
        }
        
        
        //inserts the reports details id diur  into database
        internal void InsertToDB(int WpId, ListViewItem lvi, int res, string text)
        {
            openNewTTPlan();
            rep.FormId = FormId; 
            rep.WPId = WpId;
            rep.CustomerId = CustomerId;
            rep.Frm_CatId = Frm_CatId;
            rep.UserId =UserId;
            rep.EventId = EventId; 
            rep.InsertDetailsIntoTT_WP_REPORTS(WpId, lvi, res, text);
        }

        internal int GetEduNewForm(int cCustEventTypeId)
        {
            string NewForm = string.Format("select ToolForSupport from  [Book10_21].[dbo].[TT_FormTypes] where [CustEventTypeID] ={0}", cCustEventTypeId);
            int res =int.Parse( u.selectDBScalar(NewForm).ToString());
            return res;
        }

        internal void UpdateIndexes(int WpId, int EventId, int selectedValue, string str, HiddenField rowId)
        {
            string sql = string.Format("update Book10.dbo.TT_WP_Reports set val = {0},text={1} where  id = {1} ", selectedValue,str, rowId.Value);
        }
    }
}