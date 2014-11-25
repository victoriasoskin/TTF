<%@ Page Title="" Language="C#" MasterPageFile="~/TTFSite.Master" AutoEventWireup="true" CodeBehind="TTbwp.aspx.cs" Inherits="TTF.TTbwp" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>בניית תוכנית תמיכה</title>
    <script type="text/javascript">
        var myValues = '';
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <asp:HiddenField runat="server" ID="hdnCurrentEventId" />
    <asp:HiddenField runat="server" ID="HiddenField1" />
     <asp:HiddenField runat="server" ID="hdnCurrentWpId" />


    <div id="divheader">
        <asp:ListView runat="server" ID="lvHdr" OnDataBinding="lvHdr_DataBind">
            <LayoutTemplate>
                <table id="itemPlaceholderContainer" runat="server" border="0" class="lstv" style="width: 950px; position: fixed; top: 0px; right: 5px; background-color: #EEEEEE; border: 1px solid gray; ">
                    <tr id="itemPlaceholder" runat="server">
                    </tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <table style="width: 100%; font-size: small;">
                            <tr>
                                <td colspan="6" style="text-align: center;">
                                    <h1><%#Eval("FormType") + "  - " + Eval("Name") %></h1>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-weight: bold; width: 100px;">ת.ז:
                                </td>
                                <td>
                                    <%#Eval("CustomerID")%>
                                    <asp:HiddenField runat="server" ID="hdnFormHeader" Value='<%#Eval("FormType") + " " + Eval("Name") %>' />
                                </td>
                                <td style="font-weight: bold;">ת.לידה:
                                </td>
                                <td>
                                    <%#Eval("CustBirthDate", "{0:dd/MM/yyyy}")%>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-weight: bold;">מסגרת:
                                </td>
                                <td>
                                    <%#Eval("FrameName")%>
                                </td>
                                <td style="font-weight: bold; width: 60Px;">ת.קליטה:
                                </td>
                                <td style="width: 60px;">
                                    <%#Eval("EnterDate", "{0:dd/MM/yyyy}")%>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-weight: bold;">מעדכן התוכנית:
                                </td>
                                <td>
                                    <%#Eval("URName")%>
                                </td>
                                <td style="font-weight: bold;">ת.עדכון:
                                </td>
                                <td>
                                    <%#Eval("CustEventDate", "{0:dd/MM/yyyy}")%>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
    </div>
    <div style="position: absolute; top: 135px; right: 0px; padding-right: 5px; width: 950px;">
        <asp:HiddenField runat="server" ID="hdnItemHandled" />

        <%--תוכנית תמיכות--%> 

        <asp:ListView runat="server" ID="lvWP" DataSourceID="DSWP" OnItemEditing="lv_ItemEditing" OnItemCanceling="lv_ItemCanceling" InsertItemPosition="FirstItem" OnPreRender="lvWP_PreRender">
            <LayoutTemplate>
                <table runat="server" id="itemPlaceholderContainer" class="ptbl">
                    <thead>
                        <tr>
                            <td colspan="5" style="font-weight: bold; font-size: medium; text-align: center; background-color: gray; color: white;">בניית תוכנית תמיכות
                                <asp:LinkButton runat="server" ID="lnkbShowAdd" Text="הוספת תמיכה חדשה" CssClass="btns" ForeColor="White" OnPreRender="lnkbShowAdd_PreRender" OnClick="lnkbShowAdd_Click"></asp:LinkButton>
                            </td>
                        </tr>
                    </thead>
                    <tr runat="server" id="itemPlaceholder">
                    </tr>
                </table>
            </LayoutTemplate>

            <%-- תוכנית תמיכות  - תצוגה --%>

            <ItemTemplate>
                <tr class="phdr">
                    <td colspan="2">תמיכה מספר <%#Eval("Ln") %>
                        <asp:LinkButton runat="server" ID="lnkbEdit" Text="עריכה" CssClass="btns" CommandName="edit"></asp:LinkButton>
                        <asp:LinkButton runat="server" ID="lnkbDelete" Text="מחיקה" CssClass="btns" OnClientClick="return confirm('האם למחוק?');"></asp:LinkButton>
                        <asp:HiddenField runat="server" ID="hdnWPId" Value='<%#Eval("Id") %>' />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table class="ptbl">
                            <tr>
                                <td style="width: 50px;">תחום
                                </td>
                                <td>
                                    <%#Eval("Range") %>
                                </td>
                                <tr>
                                    <td>נושא
                                    </td>
                                    <td>
                                        <%#Eval("Subject") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2">מטרת על
                                    </td>
                                    <td>
                                        <%#Eval("Purpose") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:HiddenField runat="server" ID="hdnTargetId" Value='<%#Eval("TargetId") %>' />
                                        <%#Eval("Target") %>
                                    </td>
                                </tr>
                            </tr>
                            <tr>
                                <td style="width: 50px;">דרגת חשיבות</td>
                                <td style="padding-right: 5px;">
                                    <%#Eval("Weight") %></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr class="psubhdr">
                    <td>יעדים ומדדים</td>
                </tr>
                <tr>
                    <td colspan="2">

                       <%--אינדקסים - תצוגה בתצוגה של תמיכה--%>

                        <asp:ListView runat="server" ID="dlIndexes" OnPreRender="dlIdexes_PreRender" >
                            <LayoutTemplate>
                                <table runat="server" id="itemPlaceholderContainer" class="ptbl">
                                    <tr runat="server" id="itemPlaceholder"></tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="direction: ltr; width: 22px; text-align: right;">
                                        <asp:Label runat="server" ID="lblIndexName" Text='<%#Eval("Name") %>' OnPreRender="lblIndexName_PreRender" />
                                    </td>
                                    <td style="width: 400px;">
                                        <asp:Label runat="server" ID="lblIndex" Text='<%#Eval("Text") %>' />
                                        <asp:HiddenField runat="server" ID="hdnIndexId" Value='<%#Eval("IndexId") %>' />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </td>
                </tr>
                <tr class="psubhdr">
                    <td>פרטי התוכנית</td>
                </tr>
                <tr>
                    <td>
  
<%--                         פרטי תמיכה - תצוגה בתצוגה של תמיכה --%>

                      <asp:ListView runat="server" ID="dlDetails" OnPreRender="dlDetails_PreRender">
                            <LayoutTemplate>
                                <table runat="server" id="itemPlaceholderContainer" class="ptbl">
                                    <tr class="psubhdr">
                                        <td>תיאור</td>
                                        <td>תקופת התמיכה</td>
                                        <td>תדירות</td>
                                        <td>משך התמיכה</td>
                                        <td>הספק</td>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder"></tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr style="vertical-align: top;">
                                    <td style="width: 550px;">
                                        <%#Eval("Text") %>
                                    </td>
                                    <td><%#Eval("Period") %></td>
                                    <td><%#(Eval("Amount").ToString().Trim() == "1" ? "פעם" : (Eval("Amount").ToString().Trim() == "2"? "פעמיים" : Eval("Amount") + " פעמים")) %><br />
                                        <%#"ב" + Eval("Frequency") %></td>
                                    <td><%#Eval("length") %></td>
                                    <td><%#Eval("Helper") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </td>
                </tr>
                <tr style="width: 100%; height: 20px; background-color: gray;">
                    <td></td>
                </tr>
            </ItemTemplate>

           <%--  תוכנית תמיכות הוספה --%>

            <InsertItemTemplate>
                <tr>
                    <td colspan="5" class="phdr">
                        תמיכה חדשה
                        <asp:LinkButton runat="server" ID="lnkbSaveNClose" Text="שמירה וסגירה" CssClass="btns" OnClick="lnkbWP_Click"></asp:LinkButton>
                        <asp:LinkButton runat="server" ID="lnkbSave" Text="שמירה והוספת פירוט" CssClass="btns" OnClick="lnkbWP_Click"></asp:LinkButton>
                        <asp:LinkButton runat="server" ID="lnkbCancel" Text="ביטול" CssClass="btns" CommandName="cancel" OnClick="lnkbCancel_Click" CausesValidation="false"></asp:LinkButton>
                        <asp:HiddenField runat="server" ID="hdnWPId" Value='<%#Eval("Id") %>' />
                    </td>
                </tr>
                <tr class="editbck">
                    <td>
                        <table class="ptbl">
                            <tr>
                                <td style="width: 50px;">תחום
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlRange" AppendDataBoundItems="true" AutoPostBack="true" OnDataBinding="ddl_DataBinding" OnPreRender="ddl_PreRender" Width="150" OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvRange" ControlToValidate="ddlRange" ErrorMessage="יש לבחור בתחום" ForeColor="Red" Display="Dynamic" />
                                </td>
                                <tr>
                                    <td>נושא
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlSubject" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" Width="150" OnPreRender="ddl_PreRender" OnDataBinding="ddl_DataBinding"></asp:DropDownList>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvSubject" ControlToValidate="ddlSubject" ErrorMessage="יש לבחור בנושא" ForeColor="Red" Display="Dynamic" />
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2">מטרת על
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlPurpose" AppendDataBoundItems="true" OnDataBinding="ddl_DataBinding" Width="800" OnPreRender="ddl_PreRender" >
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvPurpose" ControlToValidate="ddlPurpose" ErrorMessage="יש לבחור במטרה" ForeColor="Red" Display="Dynamic" />

                                        <br />
                                        <asp:TextBox runat="server" ID="tbPurpose" Columns="50" MaxLength="50" style="display:none;" OnPreRender="tbOther_PreRender" />
                                        <%--                                       <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="tbPurpose" ErrorMessage="כאשר בוחרים ב 'אחר (...)' יש להקליד מטרה" ForeColor="Red" Display="Dynamic" />--%>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlTarget" AppendDataBoundItems="true" AutoPostBack="true" OnDataBinding="ddl_DataBinding" Width="150" OnPreRender="ddl_PreRender" OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ddlTarget" ErrorMessage="יש לבחור ביעד" ForeColor="Red" Display="Dynamic" />
                                    </td>
                                </tr>
                            </tr>
                            <tr>
                                <td style="width: 50px;">דרגת חשיבות</td>
                                <td style="padding-right: 5px;">
                                    <asp:DropDownList runat="server" ID="ddlWeight" AppendDataBoundItems="true" OnDataBinding="ddl_DataBinding" Width="150" OnPreRender="ddl_PreRender">
                                    </asp:DropDownList><br />
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="ddlWeight" ErrorMessage="יש לבחור בדרגת חשיבות" ForeColor="Red" Display="Dynamic" />
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr class="psubhdr">
                    <td>יעדים ומדדים</td>
                </tr>
                <tr>
                    <td colspan="2">

                         <%--אינדקיסים בתוך הוספת תמיכה--%>

                        <asp:ListView runat="server" ID="dlIndexes" OnPreRender="dlIdexes_PreRender" OnItemEditing="lv_ItemEditing" >
                            <LayoutTemplate>
                                <table runat="server" id="itemPlaceholderContainer" class="ptbl">
                                    <tr runat="server" id="itemPlaceholder"></tr>
                                </table>
                            </LayoutTemplate>

                             <%--תצוגה של אינדקסים בהוספת תמיכה--%>

                            <ItemTemplate>
                                <tr>
                                    <td style="direction: ltr; width: 22px; text-align: right;">
                                        <%#Eval("Name") %>
                                    </td>
                                    <td style="width: 400px;" class="editbck"><%#Eval("Text") %>
                                        <asp:Literal runat="server" ID="litIndex_Edit" OnPreRender="lit_Prerender" Text="עריכה"   />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <%--עדכון של אינדקסים בהוספת תמיכה--%> 
                            <EditItemTemplate>
                                <tr>
                                    <td style="direction: ltr; width: 44px; text-align: right;" class="editbck">
                                        <asp:Label runat="server" ID="lblName" Text='<%#Eval("Name") %>' OnPreRender="lblIndexName_PreRender" />
                                        <asp:HiddenField runat="server" ID="hdnId" Value='<%#Eval("Id") %>' />
                                        <asp:HiddenField ID="hdbWpId" runat="server" Value='<%#Eval("WpId") %>' />
                                        <asp:HiddenField ID="hdnIndexId" runat="server" Value='<%#Eval("IndexId") %>' />
                                        <asp:HiddenField ID="hdnVal" runat="server" Value='<%#Eval("val") %>' />
                                    </td>
                                    <td style="width: 400px;" class="editbck">
                                        <asp:TextBox runat="server" ID="tbIndex" Columns="90" TextMode="MultiLine" Rows="2" MaxLength="150" Text='<%#Eval("Text") %>' OnTextChanged="tbIndex_TextChanged" OnPreRender="tb_PreRender"></asp:TextBox>
                                        <asp:Literal runat="server" ID="litIndex_Update" OnPreRender="lit_Prerender" Text="שמירה" />
                                    </td>
                                </tr>
                            </EditItemTemplate>
                        </asp:ListView>
                    </td>
                </tr>
                <tr class="psubhdr">
                    <td>פרטי התוכנית</td>
                </tr>
                <tr>
                    <td>
                         <%--פירוטים בתוך הוספת תמיכה--%>
                        <asp:ListView runat="server" ID="dlDetails" OnPreRender="dlDetails_PreRender" InsertItemPosition="FirstItem" OnItemInserting="Det_Inserting" OnItemEditing="lv_ItemEditing" OnItemUpdating="Det_Updating">
                            <LayoutTemplate>
                                <table runat="server" id="itemPlaceholderContainer" class="ptbl">
                                    <tr class="psubhdr">
                                        <td>תיאור</td>
                                        <td>תקופת התמיכה</td>
                                        <td>תדירות</td>
                                        <td>משך התמיכה</td>
                                        <td>הספק</td>
                                        <td></td>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder"></tr>
                                </table>
                            </LayoutTemplate>

                             <%--תצוגה של פירוטים בהוספת תמיכה--%> 

                            <ItemTemplate>
                                <tr style="vertical-align: top;" class="editbck">
                                    <td style="width: 550px;">
                                        <%#Eval("Text") %>
                                    </td>
                                    <td><%#Eval("Period") %></td>
                                    <td><%#(Eval("Amount").ToString().Trim() == "1" ? "פעם" : (Eval("Amount").ToString().Trim() == "2"? "פעמיים" : Eval("Amount") + " פעמים")) %><br />
                                        <%#"ב" + Eval("Frequency") %></td>
                                    <td><%#Eval("length") %></td>
                                    <td><%#Eval("Helper") %></td>
                                    <td>
                                        <asp:Literal runat="server" ID="litDetails_Edit" OnPreRender="lit_Prerender" Text="עריכה" />
                                        <asp:LinkButton runat="server" ID="lnkbCancel" Text="מחיקה" CssClass="btns" CommandName="delete"></asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>

                             <%--הוספה של פירוטים בהוספת תמיכה--%> 

                            <InsertItemTemplate>
                                <tr style="vertical-align: top;" class="editbck">
                                    <td style="">
                                        <asp:TextBox runat="server" ID="tbDetails" MaxLength="150" TextMode="MultiLine" Rows="4" Columns="49" />
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlPeriod" AppendDataBoundItems="true" OnDataBinding="ddl_DataBinding"  Width="90" OnPreRender="ddl_PreRender"></asp:DropDownList>
                                        <asp:TextBox runat="server" ID="tbPeriod" Width="85" MaxLength="40"  TextMode="MultiLine" style="display:none;"  OnPreRender="tbOther_PreRender"/>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlFrequesncy_1" AppendDataBoundItems="true" OnDataBinding="ddl_DataBinding" OnPreRender="ddl_PreRender" Width="90">
                                        </asp:DropDownList><br />
                                        <asp:DropDownList runat="server" ID="ddlFrequesncy_2" AppendDataBoundItems="true" OnDataBinding="ddl_DataBinding" OnPreRender="ddl_PreRender" Width="90">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlLasting" AppendDataBoundItems="true" OnDataBinding="ddl_DataBinding" OnPreRender="ddl_PreRender" Width="90">
                                        </asp:DropDownList></td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlHelper" AppendDataBoundItems="true" OnDataBinding="ddl_DataBinding" OnPreRender="ddl_PreRender" Width="90">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:TextBox runat="server" ID="tbHelper" Width="85" MaxLength="40" style="display:none;" TextMode="MultiLine" OnPreRender="tbOther_PreRender" /></td>
                                    <td style="white-space: nowrap;">
                                        <asp:LinkButton runat="server" ID="lnkbSave" Text="שמירה" CssClass="btns" CommandName="insert"></asp:LinkButton><asp:LinkButton runat="server" ID="lnkbCancel" Text="ביטול" CssClass="btns" OnClick="lnkbCancel_Click" CausesValidation="false"></asp:LinkButton></td>
                                </tr>
                            </InsertItemTemplate>

                             <%--עדכון של פירוטים בהוספת תמיכה--%> 

                            <EditItemTemplate>
                                <tr style="vertical-align: top;" class="editbck">
                                    <td style="">
                                        <asp:HiddenField runat="server" ID="hdnId" Value='<%#Eval("Id") %>' />
                                        <asp:TextBox runat="server" ID="tbDetails" MaxLength="150" TextMode="MultiLine" Rows="4" Columns="49" Text='<%#Eval("Text") %>' />
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlPeriod" AppendDataBoundItems="true" OnDataBinding="ddl_DataBinding" Width="90" OnPreRender="ddl_PreRender" SelectedValue='<%#Eval("PeriodId") %>'></asp:DropDownList>
                                        <asp:TextBox runat="server" ID="tbPeriod" Width="85" MaxLength="40" TextMode="MultiLine" Text='<%#Eval("PeriodText") %>' style="display:none;" OnPreRender="tbOther_PreRender" />
                                        />
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlFrequesncy_1" AppendDataBoundItems="true" OnDataBinding="ddl_DataBinding" OnPreRender="ddl_PreRender" Width="90" SelectedValue='<%#Eval("AmountId") %>'>
                                        </asp:DropDownList><br />
                                        <asp:DropDownList runat="server" ID="ddlFrequesncy_2" AppendDataBoundItems="true" OnDataBinding="ddl_DataBinding" OnPreRender="ddl_PreRender" Width="90" SelectedValue='<%#Eval("FrequencyId") %>'>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlLasting" AppendDataBoundItems="true" OnDataBinding="ddl_DataBinding" OnPreRender="ddl_PreRender" Width="90" SelectedValue='<%#Eval("LengthId") %>'>
                                        </asp:DropDownList></td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlHelper" AppendDataBoundItems="true" OnDataBinding="ddl_DataBinding" OnPreRender="ddl_PreRender" Width="90" SelectedValue='<%#Eval("HelperId") %>'>
                                        </asp:DropDownList>
                                        <br />
                                        <asp:TextBox runat="server" ID="tbHelper" Width="85" MaxLength="40" style="display:none;" TextMode="MultiLine" Text='<%#Eval("HelperText") %>' OnPreRender="tbOther_PreRender" /></td>
                                    <td style="white-space: nowrap;">
                                        <asp:LinkButton runat="server" ID="lnkbSave" Text="שמירה" CssClass="btns" CommandName="update"></asp:LinkButton><asp:LinkButton runat="server" ID="lnkbCancel" Text="ביטול" CssClass="btns" OnClick="lnkbCancel_Click" CausesValidation="false"></asp:LinkButton></td>
                                </tr>
                            </EditItemTemplate>
                        </asp:ListView>
                    </td>
                </tr>
                <tr style="width: 100%; height: 20px; background-color: gray;">
                    <td></td>
                </tr>
            </InsertItemTemplate>

              <%--תוכנית תמיכות עדכון--%> 

            <EditItemTemplate>
                <tr>
                    <td colspan="5" class="phdr">עריכת תמיכה מספר
                         <%#Eval("Ln") %>
                        <asp:LinkButton runat="server" ID="lnkbSave" Text="שמירה" CssClass="btns" OnClick="lnkbWP_Click">
                        </asp:LinkButton>
                        <asp:LinkButton runat="server" ID="lnkbCancel" Text="ביטול" CssClass="btns" CommandName="cancel" OnClick="lnkbCancel_Click" CausesValidation="false">
                        </asp:LinkButton>
                        <asp:HiddenField runat="server" ID="hdnWPId" Value='<%#Eval("Id") %>' />
                    </td>
                </tr>
                
                </td>
                </tr>
                <tr class="editbck">
                    <td>
                        <table class="ptbl">
                            <td style="width: 50px;">תחום
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlRange" AppendDataBoundItems="true" AutoPostBack="true" OnDataBinding="ddl_DataBinding" OnPreRender="ddl_PreRender" Width="150" SelectedValue='<%#Eval("RangeId") %>' OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <tr>
                                <td>נושא
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlSubject" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" Width="150" SelectedValue='<%#Eval("SubjectId") %>' OnDataBinding="ddl_DataBinding" OnPreRender="ddl_PreRender"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td rowspan="2">מטרת על
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlPurpose" AppendDataBoundItems="true" OnDataBinding="ddl_DataBinding" OnPreRender="ddl_PreRender" Width="800" SelectedValue='<%#Eval("PurposeId") %>' >
                                    </asp:DropDownList>
                                    <br />
                                    <asp:TextBox runat="server" ID="tbPurpose" Columns="50" MaxLength="50" style="display:none;"  OnPreRender="tbOther_PreRender"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlTarget" AppendDataBoundItems="true" AutoPostBack="true" OnDataBinding="ddl_DataBinding" OnPreRender="ddl_PreRender" Width="150" SelectedValue='<%#Eval("TargetId") %>' OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 50px;">דרגת חשיבות</td>
                                <td style="padding-right: 5px;">
                                    <asp:DropDownList runat="server" ID="ddlWeight" AppendDataBoundItems="true" OnDataBinding="ddl_DataBinding" OnPreRender="ddl_PreRender" Width="150" SelectedValue='<%#Eval("WeightId")%>'>
                                    </asp:DropDownList><br />
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="ddlWeight" ErrorMessage="יש לבחור בדרגת חשיבות" ForeColor="Red" Display="Dynamic" />
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr class="psubhdr">
                    <td>יעדים ומדדים</td>
                </tr>
                <tr class="editbck">
                    <td colspan="2">
                         <%--אינדקיסים בתוך עדכון תמיכה--%> 
                        <asp:ListView runat="server" ID="dlIndexes" OnPreRender="dlIdexes_PreRender">
                            <LayoutTemplate>
                                <table runat="server" id="itemPlaceholderContainer" class="ptbl">
                                    <tr runat="server" id="itemPlaceholder"></tr>
                                </table>
                            </LayoutTemplate>
                             <%--תצוגה של אינדקיסים בתוך עדכון תמיכה--%> 
                            <ItemTemplate>
                                <tr>
                                    <td style="direction: ltr; width: 22px; text-align: right;">
                                        <%#Eval("Name") %>
                                    </td>
                                    <td style="width: 400px;" class="editbck"><%#Eval("Text") %>
                                        <asp:Literal runat="server" ID="litIndex_Edit" OnPreRender="lit_Prerender" Text="עריכה" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                             <%--עדכון של אינדקיסים בתוך עדכון תמיכה--%> 
                            <EditItemTemplate>
                                <tr>
                                    <td style="direction: ltr; width: 44px; text-align: right;" class="editbck">
                                        <%#Eval("Name") %>
                                        <asp:HiddenField runat="server" ID="hdnId" Value='<%#Eval("Id") %>' />
                                        <asp:HiddenField ID="hdbWpId" runat="server" Value='<%#Eval("WpId") %>' />
                                        <asp:HiddenField ID="hdnIndexId" runat="server" Value='<%#Eval("IndexId") %>' />
                                        <asp:HiddenField ID="hdnVal" runat="server" Value='<%#Eval("val") %>' />
                                    </td>
                                    <td style="width: 400px;" class="editbck">
                                        <asp:TextBox runat="server" ID="tbIndex" Columns="90" TextMode="MultiLine" Rows="2" MaxLength="150" Text='<%#Eval("Text") %>' OnTextChanged="tbIndex_TextChanged" OnPreRender="tb_PreRender"></asp:TextBox>
                                        <asp:Literal runat="server" ID="litIndex_Update" OnPreRender="lit_Prerender" Text="שמירה" />
                                    </td>
                                </tr>
                            </EditItemTemplate>
                        </asp:ListView>
                    </td>
                </tr>
                <tr class="psubhdr">
                    <td>פרטי התוכנית</td>
                </tr>
                <tr class="editbck">
                    <td>
                         <%--פרטים בתוך עדכון תמיכה--%>
                        <asp:ListView runat="server" ID="dlDetails" OnPreRender="dlDetails_PreRender" InsertItemPosition="LastItem" DataKeyNames="Id" OnItemEditing="lv_ItemEditing" OnItemCanceling="lv_ItemCanceling" OnItemUpdating="Det_Updating">
                            <LayoutTemplate>
                                <table runat="server" id="itemPlaceholderContainer" class="ptbl">
                                    <tr class="psubhdr">
                                        <td>תיאור</td>
                                        <td>תקופת התמיכה</td>
                                        <td>תדירות</td>
                                        <td>משך התמיכה</td>
                                        <td>הספק</td>
                                        <td></td>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder"></tr>
                                </table>
                            </LayoutTemplate>
                             <%--תצוגה של פרטים בתוך עדכון תמיכה--%> 
                            <ItemTemplate>
                                <tr style="vertical-align: top;">
                                    <td style="width: 550px;">
                                        <asp:HiddenField runat="server" ID="hdnId" Value='<%#Eval("Id") %>' />
                                        <%#Eval("Text") %>
                                    </td>
                                    <td><%#Eval("Period") %></td>
                                    <td><%#(Eval("Amount").ToString().Trim() == "1" ? "פעם" : (Eval("Amount").ToString().Trim() == "2"? "פעמיים" : Eval("Amount") + " פעמים")) %><br />
                                        <%#"ב" + Eval("Frequency") %></td>
                                    <td><%#Eval("length") %></td>
                                    <td><%#Eval("Helper") %></td>
                                    <td>
                                        <asp:Literal runat="server" ID="litDetails_Edit" OnPreRender="lit_Prerender" Text="עריכה" />
                                        <asp:LinkButton runat="server" ID="lnkbDelete" Text="מחיקה" CssClass="btns" OnClientClick="return confirm('האם למחוק?');"></asp:LinkButton></td>
                                </tr>
                            </ItemTemplate>
                             <%--הוספה של פרטים בתוך עדכון תמיכה--%> 
                            <InsertItemTemplate>
                                <tr style="vertical-align: top;">
                                    <td style="">
                                        <asp:TextBox runat="server" ID="tbDetails" MaxLength="150" TextMode="MultiLine" Rows="4" Columns="49" />
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlPeriod" AppendDataBoundItems="true" OnDataBinding="ddl_DataBinding" OnPreRender="ddl_PreRender" Width="90"></asp:DropDownList>
                                         <asp:TextBox runat="server" ID="tbPeriod" Width="85" MaxLength="40" style="display:none;" TextMode="MultiLine" OnPreRender="tbOther_PreRender" /></td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlFrequesncy_1" AppendDataBoundItems="true" OnDataBinding="ddl_DataBinding" OnPreRender="ddl_PreRender" Width="90">
                                        </asp:DropDownList><br />
                                       <asp:DropDownList runat="server" ID="ddlFrequesncy_2" AppendDataBoundItems="true" OnDataBinding="ddl_DataBinding" OnPreRender="ddl_PreRender" Width="90">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlLasting" AppendDataBoundItems="true" OnDataBinding="ddl_DataBinding" OnPreRender="ddl_PreRender" Width="90">
                                        </asp:DropDownList></td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlHelper" AppendDataBoundItems="true" OnDataBinding="ddl_DataBinding" OnPreRender="ddl_PreRender"  Width="90">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:TextBox runat="server" ID="tbHelper" Width="85" MaxLength="40" style="display:none;" TextMode="MultiLine" OnPreRender="tbOther_PreRender" /></td>
                                    <td style="white-space: nowrap;">
                                        <asp:LinkButton runat="server" ID="lnkbSave" Text="שמירה" CssClass="btns"></asp:LinkButton><asp:LinkButton runat="server" ID="lnkbCancel" Text="ביטול" CssClass="btns" OnClick="lnkbCancel_Click" CausesValidation="false"></asp:LinkButton></td>
                                </tr>
                            </InsertItemTemplate>
                             <%--עדכון של פרטים בתוך עדכון תמיכה--%> 
                            <EditItemTemplate>
                                <tr style="vertical-align: top;">
                                    <td style="">
                                        <asp:HiddenField runat="server" ID="hdnId" Value='<%#Eval("Id") %>' />
                                        <asp:TextBox runat="server" ID="tbDetails" MaxLength="150" TextMode="MultiLine" Rows="4" Columns="49" Text='<%#Eval("Text") %>' />
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlPeriod" AppendDataBoundItems="true" OnDataBinding="ddl_DataBinding" OnPreRender="ddl_PreRender" SelectedValue='<%#Eval("PeriodId") %>' Width="90"></asp:DropDownList><asp:TextBox runat="server" ID="tbPeriod" Width="85" MaxLength="40" style="display:none;" TextMode="MultiLine" OnPreRender="tbOther_PreRender" /></td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlFrequesncy_1" AppendDataBoundItems="true" OnDataBinding="ddl_DataBinding" OnPreRender="ddl_PreRender" Width="90" SelectedValue='<%#Eval("AmountId") %>'>
                                        </asp:DropDownList><br />
                                        <asp:DropDownList runat="server" ID="ddlFrequesncy_2" SelectedValue='<%#Eval("FrequencyId") %>' AppendDataBoundItems="true" OnDataBinding="ddl_DataBinding" OnPreRender="ddl_PreRender" Width="90">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlLasting" AppendDataBoundItems="true" OnDataBinding="ddl_DataBinding" OnPreRender="ddl_PreRender" Width="90" SelectedValue='<%#Eval("LengthId") %>'>
                                        </asp:DropDownList></td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlHelper" AppendDataBoundItems="true" OnDataBinding="ddl_DataBinding" OnPreRender="ddl_PreRender" Width="90" SelectedValue='<%#Eval("HelperId") %>'>
                                        </asp:DropDownList>
                                        <br />
                                        <asp:TextBox runat="server" ID="tbHelper" Width="85" MaxLength="40" style="display:none;"  TextMode="MultiLine" OnPreRender="tbOther_PreRender" /></td>
                                    <td style="white-space: nowrap;">
                                        <asp:LinkButton runat="server" ID="lnkbSave" Text="שמירה" CssClass="btns" CommandName="update"></asp:LinkButton><asp:LinkButton runat="server" ID="lnkbCancel" Text="ביטול" CommandName="cancel" CssClass="btns" OnClick="lnkbCancel_Click" CausesValidation="false"></asp:LinkButton></td>
                                </tr>
                            </EditItemTemplate>
                        </asp:ListView>
                    </td>
                </tr>
                <tr style="width: 100%; height: 20px; background-color: gray;">
                    <td></td>
                </tr>
            </EditItemTemplate>
        </asp:ListView>
    </div>
    <asp:SqlDataSource ID="DSWP" runat="server" ConnectionString="<%$ ConnectionStrings:Book10PE %>" SelectCommand="SELECT *,ROW_NUMBER() OVER(ORDER BY Ord) Ln FROM [TT_vWP] WHERE ([CustomerId] = @CustomerId) AND Id=ISNULL(@Id,Id) ORDER BY [Ord]" CancelSelectOnNullParameter="false">
        <SelectParameters>
            <asp:QueryStringParameter Name="CustomerId" QueryStringField="ID" Type="Int64" />
            <asp:ControlParameter ControlID="hdnItemHandled" Name="Id" PropertyName="Value" DefaultValue="" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
