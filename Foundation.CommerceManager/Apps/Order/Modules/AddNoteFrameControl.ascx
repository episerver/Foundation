<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddNoteFrameControl.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.AddNoteFrameControl" %>
<div style="padding:5px; font-weight:bold;"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Order.OrderStrings, Comment%>" />
:</div>
<div style="padding:5px;">
	<asp:TextBox ID="txtNote" runat="server" Width="90%" Height="100px" TextMode="MultiLine"></asp:TextBox>
</div>