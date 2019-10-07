<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SalesFilter.ascx.cs"
    Inherits="Mediachase.Commerce.Manager.Apps.Reporting.Filters.SalesFilter" %>
<%@ Register Src="~/Apps/Core/Controls/CalendarDatePicker.ascx" TagName="CalendarDatePicker" TagPrefix="ecf" %>    

<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:SharedStrings, Start_Date %>"/>:
<ecf:CalendarDatePicker runat="server" ID="StartDate" />
<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:SharedStrings, End_Date %>"/>:
<ecf:CalendarDatePicker runat="server" ID="EndDate" />
<asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:SharedStrings, Group_By %>"/>:
<asp:DropDownList runat="server" ID="GroupBy">
    <asp:ListItem Value="day" Text="<%$ Resources:SharedStrings, Day %>"></asp:ListItem>
    <asp:ListItem Value="month" Text="<%$ Resources:SharedStrings, Month %>"></asp:ListItem>
    <asp:ListItem Value="year" Text="<%$ Resources:SharedStrings, Year %>"></asp:ListItem>
</asp:DropDownList>
<asp:Button ID="btnSearch" runat="server" Width="100" Text="<%$ Resources:ReportingStrings, Apply_Filter %>" />