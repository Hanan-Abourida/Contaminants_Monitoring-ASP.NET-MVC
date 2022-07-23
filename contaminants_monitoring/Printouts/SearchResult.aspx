<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchResult.aspx.cs" Inherits="Contaminants_Monitoring.Printouts.SearchResult" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
             <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <rsweb:ReportViewer Width="1000" 
                ID="ReportViewer1" runat="server" AsyncRendering="false" ShowExportControls="true" ShowPrintButton="true" ShowToolBar="true">
            </rsweb:ReportViewer>
        </div>
    </form>
        <script src="../Scripts/jquery-3.3.1.js"></script>
    <script>
        $(document).ready(function () {
          //  $("a[title='Excel']").parent().hide(); // Remove Excel from export dropdown.
            $("a[title='Word']").parent().hide(); // Remove Word from export dropdown.
            $("a[title='PDF']").parent().hide(); // Remove PDF from export dropdown.
        })
    </script>
</body>
</html>
