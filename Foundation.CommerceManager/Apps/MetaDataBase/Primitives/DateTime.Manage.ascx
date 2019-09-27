<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Primitives.DateTime_Manage" Codebehind="DateTime.Manage.ascx.cs" %>
<table cellpadding="3" cellspacing="1" border="0" width="100%" class="ibn-propertysheet">
	<tr>
		<td class="ibn-label" width="120px">
		<asp:Literal ID="Literal1" runat="server" Text="<%$Resources : GlobalFieldManageControls, MinimumValue%>" />:
		</td>
		<td>
			<asp:TextBox Runat="server" ID="txtMinValue" Width="150px" MaxLength="10" Text="1900-01-01"></asp:TextBox>
			<asp:RequiredFieldValidator id="rfvMinValue" runat="server" ErrorMessage="*" ControlToValidate="txtMinValue" Display="Dynamic"></asp:RequiredFieldValidator>
			<asp:RangeValidator ID="rvMinValue" runat="server" ErrorMessage="*" ControlToValidate="txtMinValue" Display="Dynamic" Type="Date" MinimumValue="1900-01-01" MaximumValue="3000-01-01"></asp:RangeValidator>
		</td>
	</tr>
	<tr>
		<td class="ibn-label" width="120px">
		<asp:Literal ID="Literal2" runat="server" Text="<%$Resources : GlobalFieldManageControls, MaximumValue%>" />:
		</td>
		<td>
			<asp:TextBox Runat="server" ID="txtMaxValue" Width="150px" MaxLength="10" Text="3000-01-01"></asp:TextBox>
			<asp:RequiredFieldValidator id="rfvMaxValue" runat="server" ErrorMessage="*" ControlToValidate="txtMaxValue" Display="Dynamic"></asp:RequiredFieldValidator>
			<asp:RangeValidator ID="rvMaxValue" runat="server" ErrorMessage="*" ControlToValidate="txtMaxValue" Display="Dynamic" Type="date" MinimumValue="1900-01-01" MaximumValue="3000-01-01"></asp:RangeValidator>
			<asp:CompareValidator ID="cvMaxValue" runat="server" ErrorMessage="*" ControlToValidate="txtMaxValue" ControlToCompare="txtMinValue" Display="dynamic" Type="Date" Operator="GreaterThan"></asp:CompareValidator>
		</td>
	</tr>
	<tr>
		<td></td>
		<td>
			<asp:CheckBox runat="server" ID="chkCurrentDateAsDefault" Text="<%$Resources : GlobalFieldManageControls, CurrentDateAsDefault%>" Checked="true" />
		</td>
	</tr>
	<tr>
		<td></td>
		<td>
			<asp:CheckBox runat="server" ID="chkUseTimeOffset" Text="<%$Resources : GlobalFieldManageControls, UseTimeOffset%>" Checked="true" />
		</td>
	</tr>
</table>