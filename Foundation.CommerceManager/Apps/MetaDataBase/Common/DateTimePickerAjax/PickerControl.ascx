<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PickerControl.ascx.cs" Inherits="Mediachase.Ibn.Web.UI.PickerControl" %>
<%@ Register TagPrefix="ibn" Namespace="Mediachase.BusinessFoundation.DateTimePicker" Assembly="Mediachase.BusinessFoundation" %>
<table cellpadding="0" cellspacing="0" border="0" id="tblCalendar" runat="server">
	<tr>
		<td id="tdDate" runat="server"><asp:TextBox runat="server" ID="txtDate" Width="250px" autocomplete="off" /></td>
		<td runat="server" id="imgCell"><asp:Image runat="server" ImageAlign="AbsMiddle" ID="img1" width="25px" height="19px" CssClass="ajax__calendar_btnImage" /></td>
		<td id="tdSeparator" runat="server" style="width:10px;">&nbsp;</td>
		<td runat="server" id="tdTime"><asp:TextBox ID="txtTime" runat="server" Width="90px" autocomplete="off"></asp:TextBox></td>
		<td><asp:RequiredFieldValidator ID="rfDate" runat="server" ControlToValidate="txtDate" CssClass="text" Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
		<asp:RangeValidator ID="rvDate" runat="server" ControlToValidate="txtDate" CssClass="text"></asp:RangeValidator>
		</td>
	</tr>
</table>
<ibn:CalendarExtender ID="calendarButtonExtender" runat="server"
     TargetControlID="txtDate"
     PopupButtonID="img1" />
<ibn:TimePickerExtender id="tpExt" runat="server" TargetControlID="txtTime" CssClass="text"></ibn:TimePickerExtender>
<asp:HiddenField runat="server" ID="hfUpdateCalendar" />