<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReturnSummary.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.ReturnSummary" %>
<div style="padding:10px;">
	<asp:Label runat="server" ID="InfoLabel" Visible="false" CssClass="ibn-alerttext"></asp:Label>
	<table cellpadding="0" cellspacing="0" width="100%" style="table-layout:fixed;">
		<tr>
			<td style="width:80px;" class="FormLabelCell">Return #:</td>
			<td class="FormFieldCell">
				<asp:Label runat="server" ID="ReturnIdLabel"></asp:Label>
			</td>
		</tr>
		<tr>
			<td style="width:100px;" class="FormLabelCell">Order #:</td>
			<td class="FormFieldCell">
				<asp:Label runat="server" ID="OrderIdLabel"></asp:Label>
			</td>
		</tr>
		<tr>
			<td class="FormLabelCell" style="padding-bottom:10px;">Customer:</td>
			<td class="FormFieldCell" style="padding-bottom:10px;">
				<asp:Label runat="server" ID="CustomerLabel"></asp:Label>
			</td>
		</tr>
		<tr>
			<td colspan="2" style="border: solid 1px #999999;">
				<asp:DataGrid runat="server" ID="MainGrid" Width="100%" AutoGenerateColumns="false" AllowPaging="false"
				 AllowSorting="false" CellSpacing="0" CellPadding="5" CssClass="Grid" GridLines="None">
					<HeaderStyle BackColor="#eeeeee" />
					<Columns>
						<asp:BoundColumn DataField="LineItemId" Visible="false"></asp:BoundColumn>
						<asp:BoundColumn DataField="Code" HeaderText="Code" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2"></asp:BoundColumn>
						<asp:BoundColumn DataField="DisplayName" HeaderText="Name" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2"></asp:BoundColumn>
						<asp:BoundColumn DataField="Quantity" HeaderText="Exp Qty" HeaderStyle-Width="60" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2" DataFormatString="{0:F}"></asp:BoundColumn>
						<asp:TemplateColumn HeaderText="Real Qty" HeaderStyle-Width="80" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2"> 
							<ItemTemplate>
								<asp:TextBox runat="server" ID="ReturnQuantytyText" Width="60" Text='<%# ((decimal)Eval("ReturnQuantity")).ToString("F") %>'></asp:TextBox>
								<asp:CustomValidator runat="server" ID="ReturnQuantytyTextValidator" 
									ControlToValidate="ReturnQuantytyText" Display="Dynamic" ErrorMessage="*" 
									onservervalidate="ReturnQuantytyTextValidator_ServerValidate"></asp:CustomValidator>
							</ItemTemplate>
						</asp:TemplateColumn>
					</Columns>
				</asp:DataGrid>
			</td>
		</tr>
		<tr>
			<td class="FormLabelCell" style="padding-top:10px;">Comments:</td>
			<td class="FormFieldCell" style="padding-top:10px;">
				<asp:TextBox runat="server" ID="CommentsText" TextMode="MultiLine" Width="95%" Rows="3"></asp:TextBox>
			</td>
		</tr>
	</table>
	
	<div style="padding-top:15px; text-align:center;">
		<asp:Button runat="server" ID="SaveButton" Text="Save" Width="100" onclick="SaveButton_Click" />
		&nbsp;
		<asp:Button runat="server" ID="CancelButton" Text="Cancel" Width="100" CausesValidation="false" />
	</div>
</div>
