<%@ Page Title="" Language="C#" MasterPageFile="~/TTFSite.Master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="TTF.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Scripts/jquery-ui-1.10.3.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            $('#ddd').progressbar({ value: 20 });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="ddd">ihjuiohuighpuihgpuihiui</div>
</asp:Content>
