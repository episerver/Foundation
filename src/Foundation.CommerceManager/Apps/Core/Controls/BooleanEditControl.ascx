<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Core.Controls.BooleanEditControl" Codebehind="BooleanEditControl.ascx.cs" %>
<asp:RadioButtonList id="MySelectList" Width="120px" runat="server" RepeatDirection="Horizontal">
    <asp:ListItem Value="True" Text="<%$ Resources:SharedStrings, Yes %>"></asp:ListItem>
	<asp:ListItem Value="False" Text="<%$ Resources:SharedStrings, No %>" Selected="True"></asp:ListItem>
</asp:RadioButtonList>
