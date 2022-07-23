<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetailedFindingsRpt_NoGroup.aspx.cs" Inherits="Contaminants_Monitoring.Printouts.DetailedFindingsRpt_NoGroup" %>
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
</body>
</html>
