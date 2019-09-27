<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderSummary.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.OrderSummary" %>
<div style="padding:10px;">
	<asp:Label runat="server" ID="InfoLabel" Visible="false" CssClass="ibn-alerttext"></asp:Label>
	<table cellpadding="0" cellspacing="0" width="100%">
		<tr>
			<td style="width:80px;" class="FormLabelCell">OrderGroupId:</td>
			<td class="FormFieldCell">
				<asp:Label runat="server" ID="OrderGroupIdLabel"></asp:Label>
			</td>
            <td style="width:20px;" class="FormLabelCell">&nbsp;</td>
            <td style="width:80px;" class="FormLabelCell">Order #:</td>
			<td class="FormFieldCell">
				<asp:Label runat="server" ID="OrderIdLabel"></asp:Label>
			</td>
            <td style="width:20px;" class="FormLabelCell">&nbsp;</td>
            <td style="width:80px;" class="FormLabelCell">&nbsp;</td>
			<td class="FormFieldCell">
				&nbsp;
			</td>
		</tr>
		<tr>
			<td style="width:80px;" class="FormLabelCell">Customer:</td>
			<td class="FormFieldCell">
				<asp:Label runat="server" ID="CustomerLabel"></asp:Label>
			</td>
            <td style="width:20px;" class="FormLabelCell">&nbsp;</td>
            <td style="width:80px;" class="FormLabelCell">E-mail:</td>
			<td class="FormFieldCell">
				<asp:Label runat="server" ID="CustomerEmail"></asp:Label>
			</td>
            <td style="width:20px;" class="FormLabelCell">&nbsp;</td>
            <td style="width:80px;" class="FormLabelCell">Phone:</td>
			<td class="FormFieldCell">
				<asp:Label runat="server" ID="CustomerPhone"></asp:Label>
			</td>
		</tr>		
        </table>
    <h3>Order Forms</h3>
    <table cellpadding="0" cellspacing="0" width="100%" style="table-layout:fixed;">
		<tr>
			<td colspan="2" style="border: solid 1px #999999;">
				<asp:DataGrid runat="server" ID="MainGrid" Width="100%" AutoGenerateColumns="false" AllowPaging="false"
				 AllowSorting="false" CellSpacing="0" CellPadding="5" CssClass="Grid" GridLines="None">
					<HeaderStyle BackColor="#eeeeee" />
					<Columns>
					    <asp:BoundColumn DataField="OrderFormId" HeaderText="Form Id" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2"></asp:BoundColumn>
						<asp:BoundColumn DataField="LineItemId" Visible="false"></asp:BoundColumn>
						<asp:BoundColumn DataField="Code" HeaderText="Code" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2"></asp:BoundColumn>
						<asp:BoundColumn DataField="DisplayName" HeaderText="Name" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2"></asp:BoundColumn>
						<asp:BoundColumn DataField="Quantity" HeaderText="Quantity" HeaderStyle-Width="60" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2" DataFormatString="{0:F}"></asp:BoundColumn>						
                        <asp:BoundColumn DataField="Price" HeaderText="Price" HeaderStyle-Width="60" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2" DataFormatString="{0:F}"></asp:BoundColumn>						
                        <asp:BoundColumn DataField="ItemLevelDiscount" HeaderText="Item Discount" HeaderStyle-Width="60" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2" DataFormatString="{0:F}"></asp:BoundColumn>						
                        <asp:BoundColumn DataField="OrderLevelDiscount" HeaderText="Order Level Discount" HeaderStyle-Width="60" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2" DataFormatString="{0:F}"></asp:BoundColumn>						                        
                        <asp:BoundColumn DataField="Total" HeaderText="Total" HeaderStyle-Width="60" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2" DataFormatString="{0:F}"></asp:BoundColumn>						
					</Columns>
				</asp:DataGrid>
			</td>
		</tr>
        </table>
    <h3 id="PaymentsHeader" runat="server">Payments</h3>
    <table cellpadding="0" cellspacing="0" width="100%" style="table-layout:fixed;">
        <tr>
            <td colspan="2" style="border: solid 1px #999999;">
				<asp:DataGrid runat="server" ID="PaymentsGrid" Width="100%" AutoGenerateColumns="false" AllowPaging="false"
				 AllowSorting="false" CellSpacing="0" CellPadding="5" CssClass="Grid" GridLines="None">
					<HeaderStyle BackColor="#eeeeee" />
					<Columns>
					    <asp:BoundColumn DataField="OrderFormId" HeaderText="Form Id" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2"></asp:BoundColumn>						
						<asp:BoundColumn DataField="PaymentMethodName" HeaderText="<%$ Resources:SharedStrings, Name %>" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2"></asp:BoundColumn>
						<asp:BoundColumn DataField="TransactionType" HeaderText="<%$ Resources:SharedStrings, TransactionType %>" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2"></asp:BoundColumn>
						<asp:BoundColumn DataField="TransactionID" HeaderText="<%$ Resources:SharedStrings, TransactionId %>" HeaderStyle-Width="60" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2"></asp:BoundColumn>						
                        <asp:BoundColumn DataField="Amount" HeaderText="<%$ Resources:SharedStrings, Amount %>" HeaderStyle-Width="60" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2" DataFormatString="{0:F}"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Status" HeaderText="<%$ Resources:SharedStrings, Status %>" HeaderStyle-Width="60" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2"></asp:BoundColumn>						
					</Columns>
				</asp:DataGrid>
			</td>
        </tr>
    </table>
    
    <h3 id="ReturnHeader" runat="server">Returns</h3>
    <table cellpadding="0" cellspacing="0" width="100%" style="table-layout:fixed;">
		<tr>
			<td colspan="2" style="border: solid 1px #999999;">			    
				<asp:DataGrid runat="server" ID="ReturnGrid" Width="100%" AutoGenerateColumns="false" AllowPaging="false"
				 AllowSorting="false" CellSpacing="0" CellPadding="5" CssClass="Grid" GridLines="None">
					<HeaderStyle BackColor="#eeeeee" />
					<Columns>
					    <asp:BoundColumn DataField="OrderFormId" HeaderText="Form Id" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2"></asp:BoundColumn>
						<asp:BoundColumn DataField="LineItemId" Visible="false"></asp:BoundColumn>
						<asp:BoundColumn DataField="Code" HeaderText="Code" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2"></asp:BoundColumn>
						<asp:BoundColumn DataField="DisplayName" HeaderText="Name" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2"></asp:BoundColumn>
						<asp:BoundColumn DataField="Quantity" HeaderText="Quantity" HeaderStyle-Width="60" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2" DataFormatString="{0:F}"></asp:BoundColumn>						
                        <asp:BoundColumn DataField="Price" HeaderText="Price" HeaderStyle-Width="60" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2" DataFormatString="{0:F}"></asp:BoundColumn>						
                        <asp:BoundColumn DataField="ItemLevelDiscount" HeaderText="Item Discount" HeaderStyle-Width="60" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2" DataFormatString="{0:F}"></asp:BoundColumn>						
                        <asp:BoundColumn DataField="OrderLevelDiscount" HeaderText="Order Level Discount" HeaderStyle-Width="60" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2" DataFormatString="{0:F}"></asp:BoundColumn>						                        
                        <asp:BoundColumn DataField="Total" HeaderText="Total" HeaderStyle-Width="60" ItemStyle-CssClass="ibn-vb2" HeaderStyle-CssClass="ibn-vh2" DataFormatString="{0:F}"></asp:BoundColumn>						
					</Columns>
				</asp:DataGrid>
			</td>
		</tr>
        </table>

	<div style="padding-top:15px; text-align:right;">		
		<asp:Button runat="server" ID="CancelButton" Text="Cancel" Width="100" CausesValidation="false" />
	</div>
</div>
