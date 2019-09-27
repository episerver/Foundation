<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PaymentMethodSelectControl.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.PaymentMethodSelectControl" %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
<%@ Register Src="~/Apps/Core/MetaData/PaymentSelectEditTab.ascx" TagName="MetaData" TagPrefix="ecf" %>
<script type="text/javascript" src="<%= CommerceHelper.GetAbsolutePath("~/Apps/Shell/Scripts/jquery.js") %>" ></script>
<script type="text/javascript">
	function OptimizeMetaForm() {
		$("input[name$='MetaValueCtrl']").width(400);
	}
</script>
<table cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td style="padding: 7px">
			<table class="DataForm" width="100%">
				<col width="150" />
				<asp:Panel runat="server" ID="pnlAmount">
					<tr>
						<td class="FormLabelCell">
							<asp:Label ID="Label1" runat="server" Text="<%$ Resources:OrderStrings, Amount %>"></asp:Label>:
						</td>
						<td class="FormFieldCell">
							<asp:TextBox runat="server" ValidationGroup="PaymentMetaData" ID="tbAmount" Width="150"></asp:TextBox>
							<asp:RequiredFieldValidator runat="server" ValidationGroup="PaymentMetaData" ControlToValidate="tbAmount" ErrorMessage="*"></asp:RequiredFieldValidator>
							<asp:RangeValidator ID="RangeValidator1" ValidationGroup="PaymentMetaData" runat="server" ControlToValidate="tbAmount" ErrorMessage="*" Type="Double" MinimumValue="0" MaximumValue="1000000"></asp:RangeValidator>
						</td>
					</tr>					
				</asp:Panel>
				<tr>
					<td class="FormLabelCell">
						<asp:Label ID="Label2" runat="server" Text="<%$ Resources:OrderStrings, Payment_Method %>"></asp:Label>:
					</td>
					<td class="FormFieldCell">
						<asp:DropDownList runat="server" ID="PaymentMethodList" DataMember="PaymentMethod"
							DataTextField="Name" DataValueField="PaymentMethodId" AutoPostBack="true" Width="150" CssClass="FixedWidthOption">
						</asp:DropDownList>
					</td>
				</tr>
				<asp:PlaceHolder runat="server" ID="MetaPlaceHolder"></asp:PlaceHolder>
				<ecf:MetaData ValidationGroup="PaymentMetaData" runat="server" ID="MetaDataTab" />
			</table>

		</td>
	</tr>
</table>