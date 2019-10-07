<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Primitives.Integer_Manage" Codebehind="Integer.Manage.ascx.cs" %>
<table cellpadding="3" cellspacing="1" border="0" width="100%" class="ibn-propertysheet">
	<tr>
		<td class="ibn-label" width="120px">
		<asp:Literal ID="Literal1" runat="server" Text="<%$Resources : GlobalFieldManageControls, MinimumValue%>" />:
		</td>
		<td>
			<asp:TextBox Runat="server" ID="txtMinValue" Width="150px" MaxLength="11" Text="-2147483647"></asp:TextBox>
			<asp:RequiredFieldValidator id="rfvMinValue" runat="server" ErrorMessage="*" ControlToValidate="txtMinValue" Display="Dynamic"></asp:RequiredFieldValidator>
			<asp:RangeValidator ID="rvMinValue" runat="server" ErrorMessage="*" ControlToValidate="txtMinValue" Display="Dynamic" Type="Integer" MinimumValue="-2147483648" MaximumValue="2147483646"></asp:RangeValidator>
		</td>
	</tr>
	<tr>
		<td class="ibn-label" width="120px">
		<asp:Literal ID="Literal2" runat="server" Text="<%$Resources : GlobalFieldManageControls, MaximumValue%>" />:
		</td>
		<td>
			<asp:TextBox Runat="server" ID="txtMaxValue" Width="150px" MaxLength="11" Text="2147483647"></asp:TextBox>
			<asp:RequiredFieldValidator id="rfvMaxValue" runat="server" ErrorMessage="*" ControlToValidate="txtMaxValue" Display="Dynamic"></asp:RequiredFieldValidator>
			<asp:RangeValidator ID="rvMaxValue" runat="server" ErrorMessage="*" ControlToValidate="txtMaxValue" Display="Dynamic" Type="Integer" MinimumValue="-2147483647" MaximumValue="2147483647"></asp:RangeValidator>
			<asp:CompareValidator ID="cvMaxValue" runat="server" ErrorMessage="*" ControlToValidate="txtMaxValue" ControlToCompare="txtMinValue" Display="dynamic" Type="Integer" Operator="GreaterThan"></asp:CompareValidator>
		</td>
	</tr>
	<tr>
		<td class="ibn-label" width="120px">
		<asp:Literal ID="Literal3" runat="server" Text="<%$Resources : GlobalFieldManageControls, DefaultValue%>" />:
		</td>
		<td>
			<asp:TextBox Runat="server" ID="txtDefaultValue" Width="150px" MaxLength="11" Text="0"></asp:TextBox>
			<asp:CompareValidator ID="cvDefaultValueMin" runat="server" ErrorMessage="*" ControlToValidate="txtDefaultValue" ControlToCompare="txtMinValue" Display="dynamic" Type="Integer" Operator="GreaterThanEqual"></asp:CompareValidator>
			<asp:CompareValidator ID="cvDefaultValueMax" runat="server" ErrorMessage="*" ControlToValidate="txtDefaultValue" ControlToCompare="txtMaxValue" Display="dynamic" Type="Integer" Operator="LessThanEqual"></asp:CompareValidator>
		</td>
	</tr>
</table>