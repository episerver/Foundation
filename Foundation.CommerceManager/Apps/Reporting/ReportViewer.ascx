<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Reporting.ReportViewer" %>
<%@ Register src="ReportViewerContent.ascx" tagname="ReportViewerContent" tagprefix="reporting" %>
<div class="report-view">
    <div class="report-filter" style="z-index: 1000">
        <asp:PlaceHolder runat="server" ID="ReportFilter"></asp:PlaceHolder>
    </div>
    <div class="report-content">
        <reporting:ReportViewerContent ID="ReportViewerContent1" runat="server" />
    </div>
</div>
