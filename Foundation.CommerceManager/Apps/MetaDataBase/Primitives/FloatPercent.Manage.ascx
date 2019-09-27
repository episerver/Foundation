<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FloatPercent.Manage.ascx.cs" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Primitives.FloatPercent_Manage" %>
<table cellpadding="3" cellspacing="1" border="0" width="100%" class="ibn-propertysheet">
	<tr>
		<td class="ibn-label" width="120px">
		<asp:Literal ID="Literal1" runat="server" Text="<%$Resources : GlobalFieldManageControls, MinimumValue%>" />:
		</td>
		<td>
			<asp:TextBox Runat="server" ID="txtMinValue" Width="100%" MaxLength="20"></asp:TextBox>
		</td>
		<td width="20px">
			<asp:RequiredFieldValidator id="rfvMinValue" runat="server" ErrorMessage="*" ControlToValidate="txtMinValue" Display="Dynamic"></asp:RequiredFieldValidator>
		</td>
	</tr>
	<tr>
		<td class="ibn-label" width="120px">
		<asp:Literal ID="Literal2" runat="server" Text="<%$Resources : GlobalFieldManageControls, MaximumValue%>" />:
		</td>
		<td>
			<asp:TextBox Runat="server" ID="txtMaxValue" Width="100%" MaxLength="20"></asp:TextBox>
		</td>
		<td width="20px">
			<asp:RequiredFieldValidator id="rfvMaxValue" runat="server" ErrorMessage="*" ControlToValidate="txtMaxValue" Display="Dynamic"></asp:RequiredFieldValidator>
			<asp:CompareValidator ID="cvMaxValue" runat="server" ErrorMessage="*" ControlToValidate="txtMaxValue" ControlToCompare="txtMinValue" Display="dynamic" Type="Double" Operator="GreaterThan"></asp:CompareValidator>
		</td>
	</tr>
	<tr>
		<td class="ibn-label" width="120px">
		<asp:Literal ID="Literal3" runat="server" Text="<%$Resources : GlobalFieldManageControls, DefaultValue%>" />:
		</td>
		<td>
			<asp:TextBox Runat="server" ID="txtDefaultValue" Width="100%" MaxLength="20"></asp:TextBox>
		</td>
		<td width="20px">
			<asp:CompareValidator ID="cvDefaultValueMin" runat="server" ErrorMessage="*" ControlToValidate="txtDefaultValue" ControlToCompare="txtMinValue" Display="dynamic" Type="Double" Operator="GreaterThanEqual"></asp:CompareValidator>
			<asp:CompareValidator ID="cvDefaultValueMax" runat="server" ErrorMessage="*" ControlToValidate="txtDefaultValue" ControlToCompare="txtMaxValue" Display="dynamic" Type="Double" Operator="LessThanEqual"></asp:CompareValidator>
		</td>
	</tr>
</table>