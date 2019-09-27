<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaUI.Primitives.MultiReference_Edit" Codebehind="MultiReference.Edit.ascx.cs" %>
<table cellpadding="0" cellspacing="0" border="0" width="100%" class="ibn-propertysheet" style="table-layout:fixed">
	<tr>
		<td>
			<div style="border: 2px inset; height:20px; overflow-y: auto; padding-top:1px; margin-top:1px; padding-left:3px;">
				<asp:Label runat="server" ID="lblReference"></asp:Label>
			</div>
		</td>
		<td width="20px" runat="server" id="tdSelect">
			<asp:ImageButton Runat="server" ToolTip="Select" Width="16" Height="16" ID="ibSelect" ImageAlign="AbsMiddle" TabIndex="-1"></asp:ImageButton> 
		</td>
		<td width="20px" runat="server" id="tdClear">
			<asp:ImageButton Runat="server" ToolTip="Clear" Width="16" Height="16" ID="ibClear" ImageAlign="AbsMiddle" OnClick="ibClear_Click" CausesValidation="false" TabIndex="-1"></asp:ImageButton> 
		</td>
		<td width="20px">
			<asp:CustomValidator runat="server" ID="vldCustom" ErrorMessage="*" Display="dynamic" OnServerValidate="vldCustom_ServerValidate"></asp:CustomValidator>
		</td>
	</tr>
</table>
<asp:Button id="btnRefresh" runat="server" CausesValidation="False" style="display:none;" OnClick="btnRefresh_Click"></asp:Button>