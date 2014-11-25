<%@ Page Title="" Language="C#" MasterPageFile="~/TTFSite.Master" CodeBehind="TTewp_r.aspx.cs" Inherits="TTF.TTewp_r" AutoEventWireup="true" EnableEventValidation="false" ValidateRequest="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .wkshow {
            width: 640px;
            background-color: #EEEEEE;
            border: 3px outset gray;
        }

        .dayw {
            width: 114px;
            min-height: 40px;
            background-color: white;
            white-space: normal;
            font-size: 11px;
            background-color: white;
            border: 2px outset lightgray;
            cursor: pointer;
            vertical-align: top;
            position: relative;
        }

        .delbtn {
            height: 20px;
            width: 20px;
            position: absolute;
            bottom: 0px;
            left: 0px;
        }

        @media print {
            .header, .hide {
                visibility: hidden;
            }
        }
    </style>
    <script type="text/javascript">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="width: 640px">

        <div class="header">

            <%--    ---------------------------
        כפתור סגירה
        -------------------------- ---%>

            <asp:LinkButton runat="server" ID="lnkbClose" OnClientClick="window.open('', '_self', ''); window.close();" Style="position: fixed; top: 15px; right: 15px; z-index: 888; background-color: #EEEEEE;">סגירה</asp:LinkButton>
            <asp:LinkButton runat="server" ID="lnkbPrint" OnClientClick="javascript:window.focus();window.print();window.open('', '_self', ''); window.close();" Style="position: fixed; top: 15px; right: 55px; z-index: 888; background-color: #EEEEEE;">הדפסה</asp:LinkButton>
            <asp:LinkButton runat="server" ID="lnkbword" OnClick="W_Click" Style="position: fixed; top: 33px; right: 15px; z-index: 888; background-color: #EEEEEE;">Word</asp:LinkButton>
            <asp:LinkButton runat="server" ID="lnkbexcel" OnClick="E_Click" Style="position: fixed; top: 33px; right: 55px; z-index: 888; background-color: #EEEEEE;">Excel</asp:LinkButton>

        </div>

        <%--    ---------------------------
        קודים נשמרים
        -------------------------- ---%>

        <asp:HiddenField runat="server" ID="hdnItemHandled" />
        <asp:HiddenField runat="server" ID="hdnCurrentWpId" />
        <asp:HiddenField runat="server" ID="hdnForm127" />
        <asp:HiddenField runat="server" ID="hdnDate127" />
        <asp:HiddenField runat="server" ID="hdnWeekShow" Value="wkhide" />


        <%--    ---------------------------
        כותרת הדף
        -------------------------- ---%>

        <div id="divheader">
            <asp:ListView runat="server" ID="lvHdr" OnDataBinding="lvHdr_DataBind" Style="top: 0px; right: 5px">
                <LayoutTemplate>
                    <table id="itemPlaceholderContainer" runat="server" style="width: 640px; background-color: #EEEEEE; border: 1px solid gray; font-size: 11px;">
                        <tr id="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td colspan="6" style="text-align: center;">
                            <asp:Label runat="server" ID="lblName" Style="font-size: xx-large; font-weight: bold;" Text='<%#"תוכנית אב - " + Eval("Name") %>'></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; width: 100px;">ת.ז:
                        </td>
                        <td style="text-align: right; width: 300px;">
                            <%#Eval("CustomerID")%>
                        </td>
                        <td style="width: 40px;"></td>
                        <td style="width: 40px;"></td>
                        <td style="font-weight: bold; text-align: left; width: 80px;">ת.לידה:
                        </td>
                        <td style="text-align: left; width: 80px;">
                            <%#Eval("CustBirthDate", "{0:dd/MM/yyyy}")%>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold;">מסגרת:
                        </td>
                        <td colspan="3" style="text-align: right;">
                            <%#Eval("FrameName")%>
                        </td>
                        <td style="font-weight: bold; text-align: left;">ת.קליטה:
                        </td>
                        <td style="text-align: left;">
                            <asp:Label runat="server" ID="lblStart" Text='<%#Eval("EnterDate", "{0:dd/MM/yyyy}")%>' />
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold;">מעדכן התוכנית:
                        </td>
                        <td colspan="3" style="text-align: right;">
                            <%#Eval("URName")%>
                        </td>
                        <td style="font-weight: bold; text-align: left;">ת.פתיחה:
                        </td>
                        <td style="text-align: left;">
                            <%#Eval("OpenDate", "{0:dd/MM/yyyy}")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>

        <%--    ---------------------------
            תוכנית אב
            -------------------------- ---%>

        <div style="width: 640px;">
            <asp:ListView runat="server" ID="lvMP" DataSourceID="DSMP">
                <LayoutTemplate>
                    <table runat="server" id="itemPlaceholderContainer" class="ptbl" style="width: 640px;">
                        <tr runat="server" id="itemPlaceholder">
                        </tr>
                    </table>
                </LayoutTemplate>

                <%--    ---------------------------
            תוכנית אב תצוגה
            -------------------------- ---%>

                <ItemTemplate>
                    <tr>
                        <td class="rowHeader">שם מתאם תמיכות
                        </td>
                        <td class="dataCell">
                            <%#Eval("SupportCoordinator") %>
                        </td>
                        <td class="rowHeader">שם מדריך
                        </td>
                        <td colspan="3" class="dataCell">
                            <%#Eval("Supporter") %>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">מילוי שאלון איכות חיים
                        </td>
                        <td class="dataCell">
                            <asp:Label runat="server" ID="lblD51" OnPreRender="lblDxxx_PreRnder" />
                        </td>
                        <td class="rowHeader">מילוי שאלון כלי לניהול תמיכות
                        </td>
                        <td colspan="3" class="dataCell">
                            <asp:Label runat="server" ID="lblD127" OnPreRender="lblDxxx_PreRnder" />
                        </td>
                    </tr>
                    <tr class="psubhdr" style="font-weight: bold;">
                        <td colspan="2">רקע
                        </td>
                        
                    </tr>
                    <tr>
                        <td class="rowHeader">גיל</td>
                        <td class="dataCell">
                            <asp:Label runat="server" ID="lblAge" OnPreRender="lblAge_PreRender" />
                        </td>
                        <td class="rowHeader">שנת קבלה למסגרת</td>
                        <td class="dataCell">
                            <asp:Label runat="server" ID="lblStartYear" OnPreRender="lblStart_Prerender"></asp:Label>
                        </td>
                        <td class="rowHeader">מקום מגורים</td>
                        <td class="dataCell">
                            <%#Eval("Location") %>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">מצב משפחתי</td>
                        <td class="dataCell">
                            <%#Eval("FamilyStatus") %>
                        </td>
                        <td class="rowHeader">כיתה</td>
                        <td colspan="3" class="dataCell" style="white-space: nowrap;">
                            <%#Eval("Class") %>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">חוזקות
                        </td>
                        <td colspan="9" class="dataCell">
                            <%#Eval("Strength") %>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">פעילות פנאי
                        </td>
                        <td colspan="9" class="dataCell">
                            <%#Eval("Hobby") %>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">מסגרת קודמת
                        </td>
                        <td colspan="9" class="dataCell">
                            <%#Eval("PreviousFrame") %>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">מידע נוסף

                        </td>
                        <td colspan="9" class="dataCell">
                            <%#Eval("OtherInfo") %>
                        </td>
                    </tr>
                   
                </ItemTemplate>

            </asp:ListView>

            <%--    ---------------------------
        תוכנית תמיכות
        -------------------------- ---%>
            <div style="page-break-before: always;">

                <%--    ---------------------------
        כותרת הדף
        -------------------------- ---%>

                <div style="page-break-after: avoid;">
                    <asp:ListView runat="server" ID="lvHdr1" OnDataBinding="lvHdr_DataBind" Style="top: 0px; right: 5px">
                        <LayoutTemplate>
                            <table id="Table2" runat="server" style="width: 640px; background-color: #EEEEEE; border: 1px solid gray; font-size: 11px;">
                                <tr id="Tr2" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td colspan="6" style="text-align: center;">
                                    <asp:Label runat="server" ID="Label1" Style="font-size: xx-large; font-weight: bold;" Text='<%#"תוכנית תמיכות - " + Eval("Name") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-weight: bold; width: 100px;">ת.ז:
                                </td>
                                <td style="text-align: right; width: 300px;">
                                    <%#Eval("CustomerID")%>
                                </td>
                                <td style="width: 40px;"></td>
                                <td style="width: 40px;"></td>
                                <td style="font-weight: bold; text-align: left; width: 80px;">ת.לידה:
                                </td>
                                <td style="text-align: left; width: 80px;">
                                    <%#Eval("CustBirthDate", "{0:dd/MM/yyyy}")%>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-weight: bold;">מסגרת:
                                </td>
                                <td colspan="3" style="text-align: right;">
                                    <%#Eval("FrameName")%>
                                </td>
                                <td style="font-weight: bold; text-align: left;">ת.קליטה:
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label runat="server" ID="Label2" Text='<%#Eval("EnterDate", "{0:dd/MM/yyyy}")%>' />
                                </td>
                            </tr>
                            <tr>
                                <td style="font-weight: bold;">מעדכן התוכנית:
                                </td>
                                <td colspan="3" style="text-align: right;">
                                    <%#Eval("URName")%>
                                </td>
                                <td style="font-weight: bold; text-align: left;">ת.עדכון:
                                </td>
                                <td style="text-align: left;">
                                    <%#Eval("CustEventDate", "{0:dd/MM/yyyy}")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
               
            </div>
        </div>
        <%--    ---------------------------
            תוכנית שבועית
            -------------------------- ---%>

        <br />
        <br />

        <%--    ---------------------------
            שאילתות
            -------------------------- ---%>
    </div>
    <asp:SqlDataSource ID="DSMP" runat="server" ConnectionString="<%$ ConnectionStrings:Book10PE %>" SelectCommand="SELECT * FROM Book10.dbo.TT_EduMPNew(@CustomerID,@EventID,@NewFormTypeId)" CancelSelectOnNullParameter="false" OnSelecting="DSMP_Selecting">
        <SelectParameters>
            <asp:QueryStringParameter Name="CustomerId" QueryStringField="CID" Type="Int32" />
            <asp:QueryStringParameter Name="EventId" QueryStringField="ID" Type="Int32" />
            <asp:Parameter  Name="NewFormTypeId" DefaultValue="127" />
        </SelectParameters>
    </asp:SqlDataSource>


   

    <asp:SqlDataSource ID="DSGRAPH" runat="server" ConnectionString="<%$ ConnectionStrings:Book10PE %>"
        SelectCommand="
SELECT grp, perc, Ld 
FROM (SELECT grp,perc,gid,LoadTime As Ld 
		FROM Book10_21.dbo.p5t_Forms f 
        LEFT OUTER JOIN Book10_21.dbo.p5t_FormResults r ON r.FormID = f.FormID
        WHERE f.FormID = @FormId) x
ORDER BY Ld Desc,gid        
        "
        CancelSelectOnNullParameter="False">
        <SelectParameters>
            <asp:ControlParameter Name="FormId" ControlID="hdnForm127" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>

    

</asp:Content>

