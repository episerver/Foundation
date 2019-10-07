<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportViewerContent.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Reporting.ReportViewerContent" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<rsweb:ReportViewer ID="MyReportViewer" runat="server" ShowDocumentMapButton="False"
    ShowExportControls="True" ShowFindControls="False" ShowPageNavigationControls="True"
    ShowPrintButton="True" ShowPromptAreaButton="False" ShowRefreshButton="True"
    ShowZoomControl="True" Font-Names="Verdana" Font-Size="8pt" DocumentMapWidth="100%" 
    Width="100%">
      <LocalReport EnableHyperlinks="True">
            </LocalReport>
</rsweb:ReportViewer>
