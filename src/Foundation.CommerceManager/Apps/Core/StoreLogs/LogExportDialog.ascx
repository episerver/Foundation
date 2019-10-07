<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LogExportDialog.ascx.cs" Inherits="Mediachase.Commerce.Manager.Core.StoreLogs.LogExportDialog" %>
<table width="100%" border="0">
  <tr>
    <td style="padding:5px;" align="center">
      <table>
        <tr>
          <td align="left">
            <asp:RadioButtonList runat="server" ID="rbList" >
              <asp:ListItem Value="All" Text="<%$ Resources:CoreStrings, LogExport_All_Logs_Export %>" Selected="True"></asp:ListItem>
              <asp:ListItem Value="View" Text="<%$ Resources:CoreStrings, LogExport_View_Logs_Export %>"></asp:ListItem>
              <asp:ListItem Value="CurrentPage" Text="<%$ Resources:CoreStrings, LogExport_Current_Page_Logs_Export %>"></asp:ListItem>
            </asp:RadioButtonList>
          </td>
        </tr>
      </table>
    </td>
  </tr>
  <tr>
    <td align="center" style="padding:20px;">
      <asp:Button runat="server" ID="btnExport" Text="<%$ Resources:SharedStrings, Export %>" Width="100"/>
    </td>
  </tr>
</table>
