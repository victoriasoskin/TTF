<%@ Page Title="" Language="C#" MasterPageFile="~/TTFSite.Master" AutoEventWireup="true" CodeBehind="ttdfupList.aspx.cs" Inherits="TTF.ttdfupList" %>

<%--<%@ Page Title="" Language="C#" MasterPageFile="~/TTFSite.Master" AutoEventWireup="true" CodeBehind="TTemupList.aspx.cs" Inherits="TTF.TTemupList" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css"></style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%--    ---------------------------
        כפתור סגירה
        -------------------------- ---%>

    <a href="javascript:window.open('', '_self', ''); window.close();" style="position: fixed; top: 0px; right: 9px; z-index: 888;">סגירה</a>

    <%--    ---------------------------
        קודים נשמרים
        -------------------------- ---%>

    <asp:HiddenField runat="server" ID="hdnItemHandled" />
    <asp:HiddenField runat="server" ID="hdnCurrentWpId" />
    <asp:HiddenField runat="server" ID="hdnForm127" />
    <asp:HiddenField runat="server" ID="hdnDate127" />

    <%--    ---------------------------
        כותרת הדף (fixed)
        -------------------------- ---%>

    <div id="divheader">
        <asp:ListView runat="server" ID="lvHdr" OnDataBinding="lvHdr_DataBind" Style="top: 0px; right: 5px">
            <LayoutTemplate>
                <table id="itemPlaceholderContainer" runat="server" border="0" class="lstv" style="width: 950px; position: fixed; top: 0px; right: 5px; background-color: #EEEEEE; border: 1px solid gray;">
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
                                    <span style="font-size: xx-large; font-weight: bold;"><%#"מעקב תמיכות - " + Eval("Name") %></span>

                                </td>
                            </tr>
                            <tr>
                                <td style="font-weight: bold; width: 100px;">ת.ז:
                                </td>
                                <td>
                                    <%#Eval("CustomerID")%>
                                    <asp:HiddenField runat="server" ID="hdnFormHeader" Value='<%#"תוכנית תמיכות חינוך " + Eval("Name") %>' />
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
                                    <asp:Label runat="server" ID="lblStart" Text='<%#Eval("EnterDate", "{0:dd/MM/yyyy}")%>' />
                                </td>
                            </tr>
                            <tr>
                                <td style="font-weight: bold;">מעדכן התוכנית:
                                </td>
                                <td>
                                    <%#Eval("URName")%>
                                </td>
                                <td style="font-weight: bold;">ת.פתיחה:
                                </td>
                                <td>
                                    <%#Eval("OpenDate", "{0:dd/MM/yyyy}")%>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
        <asp:HyperLink runat="server" ID="hlback" NavigateUrl="~/CustEventReport.aspx" />
    </div>

    <%--    ---------------------------
        נתוני התמיכה
        -------------------------- ---%>

    <div style="position: absolute; top: 108px; right: 0px; padding-right: 5px; width: 950px;">
        <asp:ListView runat="server" ID="lvDdetHdr" DataSourceID="DSDet">
            <LayoutTemplate>
                <table runat="server" id="itemPlaceholderContainer" class="ptbl" style="width: 950px;">
                    <tr runat="server" id="itemPlaceholder"></tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr class="phdr">
                    <td colspan="6">התמיכה</td>
                </tr>

                <tr>
                    <td class="rowHeader" style="width: 50px;">תחום
                    </td>
                    <td colspan="5">
                        <%#Eval("Range") %>
                    </td>
                </tr>
                <tr>
                    <td class="rowHeader">נושא
                    </td>
                    <td colspan="5">
                        <%#Eval("Subject") %>
                    </td>
                </tr>
                <tr>
                    <td class="rowHeader">מטרת על 
                    </td>
                    <td colspan="5">
                        <%#Eval("Purpose") %>
                    </td>
                </tr>
                <tr>
                    <td class="rowHeader">מטרה 
                    </td>
                    <td colspan="5">
                        <%#Eval("purposeText") %>
                    </td>
                </tr>
                <tr>
                    <td class="rowHeader">חשיבות 
                    </td>
                    <td colspan="5">
                        <%#Eval("weight") %>
                    </td>
                </tr>
                <tr style="font-weight: bold; font-size: medium; text-align: center; background-color: gray; color: white;">
                    <td colspan="6">המעקבים</td>
                </tr>
            </ItemTemplate>
        </asp:ListView>

        <%--    ---------------------------
        רשימת המעקבים 
        -------------------------- ---%>

        <asp:ListView runat="server" ID="lvMainReports" DataSourceID="DSReport" DataKeyNames="Id" OnItemUpdating="lvRep_ItemUpdating">
            <LayoutTemplate>
                <table runat="server" id="itemPlaceholderContainer" class="ptbl" style="width: 100%;">
                    <tr class="psubhdr">
                        <td>פירוט</td>
                        <td>שם המדווח</td>
                        <td>מועד הדיווח</td>
                        <td>ערך נבחר</td>
                        <td></td>
                    </tr>
                    <tr runat="server" id="itemPlaceholder"></tr>
                </table>
            </LayoutTemplate>
            <EmptyDataTemplate>
                <div>אין מעקבים להצגה</div>
            </EmptyDataTemplate>

            <%--    ---------------------------
        הצגת המעקבים 
        -------------------------- ---%>

            <ItemTemplate>
                <tr>
                    <td>
                        <%#Eval("text") %>
                        <asp:HiddenField runat="server" ID="hdnRepId" Value='<%#Eval("Id") %>' />
                    </td>

                    <td><%#Eval("UserName") %></td>
                    <td><%#string.Format("{0:dd/MM/yyyy}",Eval("LoadTime")) %></td>
                    <td><%#Eval("Val") %> </td>
                    <td style="white-space: nowrap;">
                        <asp:LinkButton runat="server" ID="lnkbEdit" Text="עריכה" Visible='<%#isUpdatable()%>' OnClick="lnkbEdit_Click" CommandName="edit" />
                        <%--                        <asp:LinkButton runat="server" ID="LinkButton1" Text="עריכה" CommandName="edit" Visible="true" /><%-- <%#isUpdatable() %>' />--%>
                        <%--OnClick="lnkbEdit_Click"/>--%>
                        <asp:LinkButton runat="server" ID="lnkbDelete" Text="מחיקה" CommandName="delete" OnClientClick="return confirm('האם למחוק?');" Visible="true" />
                    </td>

                </tr>
            </ItemTemplate>

            <%--    ---------------------------
        עריכת מעקב
        -------------------------- ---%>

            <EditItemTemplate>
                <tr style="background-color:salmon" >
                    <td colspan="4">
                        <table>
                            <tr class="editbck" >
                                <td colspan="4">
                                    <asp:HiddenField runat="server" ID="hdnSelectedRowId" Value='<%#Eval("Id") %>' />
                                    <asp:ListView runat="server" ID="lvIndexes" OnPreRender="lvIndexes_PreRender">
                                        <LayoutTemplate>
                                            <table runat="server" id="itemPlaceholderContainer" class="ptbl">
                                                <tr runat="server" id="itemPlaceholder"></tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="rowHeader" style="direction: ltr; width: 50px; text-align: right;">
                                                    <asp:Label runat="server" ID="lblIndexName" Text='<%#Eval("Name") %>' OnPreRender="lblIndexName_PreRender" />
                                                </td>
                                                <td class="dataCell" style="width: 1010px;">
                                                    <asp:Label runat="server" ID="lblIndex" Text='<%#Eval("Text") %>' />
                                                    <asp:HiddenField runat="server" ID="hdnIndexId" Value='<%#Eval("IndexId") %>' />
                                                </td>
                                                <td style="background-color: lightblue">
                                                    <asp:RadioButton runat="server" ID="rb" GroupName="grp" OnPreRender="rb_PreRender" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:TextBox Width="100%" ID="TBUpdateIndexReport" runat="server" Text='<%#Eval("text") %>'>                                   
                                    </asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="white-space: nowrap;" >
                        <asp:LinkButton runat="server" ID="lnkbSave" Text="שמירה" CommandName="Update" OnClick="lnkbSave_Click" />
                        <asp:LinkButton runat="server" ID="lnkbMainCancel" Text="ביטול" CommandName="cancel" CausesValidation="false" />
                    </td>
                </tr>
            </EditItemTemplate>
        </asp:ListView>
    </div>

    <%--    ---------------------------
        שאילתות
        -------------------------- ---%>

    <asp:SqlDataSource runat="server" ID="DSReport" ConnectionString="<%$ ConnectionStrings:Book10PE %>" OnDeleting="DSReport_Deleting"
        SelectCommand="SELECT r.Id, r.WpId , r.text,r.LoadTime ,u.UserName,r.val as Val
                       FROM Book10.dbo.TT_WP_Reports r
                       LEFT OUTER JOIN BOOK10_21.dbo.p0t_NtB u on u.userId= r.userID
                       WHERE r.WpId=@Id AND r.status not in(1) AND r.UserId is NOT NULL 
                       ORDER BY r.LoadTime DESC"
        DeleteCommand="UPDATE    TT_WP_Reports
                        SET Status = 1
                        WHERE      (Id = @Id) "
        UpdateCommand="">
        <SelectParameters>
            <asp:QueryStringParameter Name="Id" QueryStringField="RId" Type="Int32" DefaultValue="1" />
            <asp:QueryStringParameter Name="EventId" QueryStringField="Id" Type="Int32" DefaultValue="1" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="val1" Type="Int32" />
            <asp:QueryStringParameter Name="Id" QueryStringField="RId" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource runat="server" ID="DSDet" ConnectionString="<%$ ConnectionStrings:Book10PE %>"
        SelectCommand="SELECT cR.Name Range,cS.Name Subject,Cp.Name Purpose,W.PurposeText purposeText,cw.Name weight,u.URName Helper
FROM book10.dbo.TT_WP w 
LEFT OUTER JOIN B10Sec.dbo.TT_Classes cR ON cR.Id = w.RangeId
LEFT OUTER JOIN B10Sec.dbo.TT_Classes cS ON cS.Id = w.SubjectId
LEFT OUTER JOIN Book10.DBO.p0t_NtB u on u.UserID=w.UserId
LEFT OUTER JOIN B10Sec.dbo.TT_Classes Cp on Cp.Id=w.PurposeId
LEFT OUTER JOIN B10Sec.dbo.TT_Classes cH ON cH.Id = w.UserId
LEFT OUTER JOIN B10Sec.dbo.TT_Classes cW ON cW.Id=w.WeightId
WHERE w.Id=@Rid
">
        <SelectParameters>
            <asp:QueryStringParameter Name="RId" QueryStringField="Rid" Type="Int32" DefaultValue="1" />
        </SelectParameters>
    </asp:SqlDataSource>

</asp:Content>


