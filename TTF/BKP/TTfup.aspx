<%@ Page Title="" Language="C#" MasterPageFile="~/TTFSite.Master" AutoEventWireup="true" CodeBehind="TTfup.aspx.cs" Inherits="TTF.TTfup" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                                    <h1><%#"מעקב תמיכות - " + Eval("Name") %></h1>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-weight: bold; width: 100px;">ת.ז:
                                </td>
                                <td>
                                    <%#Eval("CustomerID")%>
                                    <asp:HiddenField runat="server" ID="hdnFormHeader" Value='<%#"מעקב תמיכות " + Eval("Name") %>' OnPreRender="hdn_PreRender" />
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
        <div>
            <asp:Chart ID="Chart1" runat="server" Width="950px" Height="500px" DataSourceID="DSGRAPH" BorderlineColor="gray" BackColor="LightGray">
           
                <Titles>
                    <asp:Title Text="סיכום מעקבים" Font="Arial, 18pt, style=Bold, Italic">                  
                    </asp:Title>
                </Titles>
                <Series>
                    <asp:Series Name="Series1" ChartType="Line" XValueMember="D" YValueMembers="val" BorderWidth="3"></asp:Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1">
                        <AxisX IsReversed="true" />
                        <AxisY Minimum="0" Maximum="100" />
                    </asp:ChartArea>
                </ChartAreas>
            </asp:Chart>
        </div>
        <asp:HiddenField runat="server" ID="hdnInsertPosision" />
        <asp:ListView runat="server" ID="lvWP" DataKeyNames="Id" DataSourceID="DSWP">
            <LayoutTemplate>
                <table runat="server" id="itemPlaceholderContainer" class="ptbl">
                    <thead>
                        <tr>
                            <td colspan="5" style="font-weight: bold; font-size: medium; text-align: center; background-color: gray; color: white;">מעקב תמיכות
                            </td>
                        </tr>
                    </thead>
                    <tr runat="server" id="itemPlaceholder">
                    </tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr class="phdr">
                    <td colspan="2">תמיכה מספר <%#Eval("Ln") %>
                        <asp:HiddenField runat="server" ID="hdnWPId" Value='<%#Eval("Id") %>' />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
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
                    <td style="width:200px;background-color:#BBBBFF;font-weight:bold;">דיווח מעקב</td>
                </tr>
                <tr>
                    <td>
                        <asp:ListView runat="server" ID="dlIndexes" OnPreRender="dlIdexes_PreRender">
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
                                    <td style="width: 400px;height:40px;">
                                        <%#Eval("Text") %>
                                        <asp:HiddenField runat="server" ID="hdnIndexId" Value='<%#Eval("IndexId") %>' />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </td>
                    <td>
                        <table class="editbck">
                            <tr  style="width:100%;">
                                <td style="width:230px;height:40px;">
                                  <asp:RadioButton runat="server" ID="rbP2" Text="2+" GroupName="Grp" style="float:right;" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height:40px;">
                                  <asp:RadioButton runat="server" ID="rbP1" Text="1+" GroupName="Grp" style="float:right;" />
                                </td>
                            </tr>
                           <tr>
                                <td style="height:40px;">
                                  <asp:RadioButton runat="server" ID="rb00" Text="0&nbsp;" GroupName="Grp" style="float:right;" />
                                </td>
                            </tr>
                           <tr>
                                <td style="height:40px;">
                                  <asp:RadioButton runat="server" ID="rbM1" Text="1-" GroupName="Grp" style="float:right;" />
                                </td>
                            </tr>
                           <tr>
                                <td style="height:40px;">
                                  <asp:RadioButton runat="server" ID="rbM2" Text="2-" GroupName="Grp" style="float:right;" />
                                </td>
                            </tr>
                       </table>
                    </td>
                </tr>
                <tr>
                    <td>

                    </td>
                    <td style="width:200px;background-color:#BBBBFF;">
                        <asp:TextBox Textmode="MultiLine" Columns="25" Rows="2" runat="server" ID="tbReport" Font-Size="Smaller" />
                    </td>
                </tr>
                <tr>
                    <td>

                    </td>
                    <td style="width:200px;background-color:#BBBBFF;">
                        <asp:Linkbutton runat="server" ID="lnkbSaveRep" text="שמירה" OnClick="SaveRep_Click"/>
                    </td>
                </tr>
                <tr class="psubhdr">
                    <td colspan="2">פרטי התוכנית</td>
                </tr>
                <tr>
                    <td colspan="2">
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
                                    <td><%#(Eval("Amount").ToString().Trim() == "1" ? "פעם" : Eval("Amount") + " פעמים")  %><br />
                                        <%#"ב" + Eval("Frequency") %></td>
                                    <td><%#Eval("length") %></td>
                                    <td><%#Eval("Helper") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </td>
                </tr>
                <tr style="width: 100%; height: 20px; background-color: #EEEEFF; text-align: left;">
                    <td colspan="2">
                        <asp:LinkButton runat="server" ID="lnkbShowFup" Text='<%#Eval("Ln","הצג מעקבים לתמיכה מספר {0}")%>' OnClick="ShowFup_Click" /></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:ListView runat="server" ID="lvFup">
                            <LayoutTemplate>
                                <table runat="server" id="itemPlaceholderContainer" class="ptbl">
                                    <tr class="phdr">
                                        <td style="width: 54px; direction: ltr; text-align: right;">ערך</td>
                                        <td>הערות</td>
                                        <td style="width: 70px;">שם המדווח</td>
                                        <td style="width: 70px;">מועד הדיווח</td>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder"></tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="direction: ltr; text-align: right;">
                                        <%#Eval("val") %>
                                    </td>
                                    <td>
                                        <%#Eval("Text") %>
                                    </td>
                                    <td>
                                        <%#Eval("URName") %>
                                    </td>
                                    <td>
                                        <%#Eval("LoadTime","{0:dd/MM/yyyy}") %>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </td>
                </tr>
                <tr style="width: 100%; height: 20px; background-color: gray;">
                    <td colspan="2"></td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
    </div>
    <asp:SqlDataSource ID="DSWP" runat="server" ConnectionString="<%$ ConnectionStrings:Book10PE %>" SelectCommand="SELECT *,ROW_NUMBER() OVER(ORDER BY Ord) Ln FROM [TT_vWP] WHERE ([CustomerId] = @CustomerId) ORDER BY [Ord]">
        <SelectParameters>
            <asp:QueryStringParameter DefaultValue="50631332" Name="CustomerId" QueryStringField="ID" Type="Int64" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="DSGRAPH" runat="server" ConnectionString="<%$ ConnectionStrings:Book10PE %>" SelectCommand="SELECT * FROM Book10.dbo.TT_Graph(@CustomewrID)" CancelSelectOnNullParameter="False">
        <SelectParameters>
            <asp:Parameter DefaultValue="50631332" Name="CustomewrID" />
        </SelectParameters>
    </asp:SqlDataSource>

</asp:Content>
