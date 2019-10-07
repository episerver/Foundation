<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReturnOrderLineItemEdit.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.ReturnOrderLineItemEdit" %>
<%@ Register TagPrefix="mc2" Assembly="Mediachase.BusinessFoundation" Namespace="Mediachase.BusinessFoundation" %>
<div style="padding: 5px;">
	<asp:Label runat="server" ID="lblErrorInfo" Style="color: Red"></asp:Label>
	<asp:Panel runat="server" ID="FormPanel">
	    <table width="100%" class="DataForm">
	        <asp:UpdatePanel UpdateMode="Conditional" ID="MainContentPanel" runat="server" RenderMode="Block">
		        <ContentTemplate>
	                <tr runat="server" id="LineItemSelectRow">
			            <td class="FormLabelCell">
				            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:OrderStrings, Line_Item %>"></asp:Label>:
			            </td>
			            <td class="FormFieldCell">
				            <asp:DropDownList runat="server" ID="OriginalLineItems" DataValueField="LineItemId" DataTextField="DisplayName" 
				                AutoPostBack="true" Width="300px"></asp:DropDownList>
			            </td>
		            </tr>
		            <tr>
			            <td class="FormLabelCell">
				            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:SharedStrings, Code %>"></asp:Label>:
			            </td>
			            <td class="FormFieldCell">
				            <asp:Label ID="CodeLabel" runat="server"></asp:Label>
			            </td>
		            </tr>
		            <tr>
			            <td class="FormLabelCell">
				            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, Display_Name %>"></asp:Label>:
			            </td>
			            <td class="FormFieldCell">
				            <asp:Label runat="server" ID="DisplayName" Width="300px"></asp:Label>
			            </td>
		            </tr>
		            <tr>
			            <td class="FormLabelCell">
				            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:SharedStrings, List_Price %>"></asp:Label>:
			            </td>
			            <td class="FormFieldCell">
				            <asp:Label runat="server" ID="ListPrice" Width="300px"></asp:Label>
			            </td>
		            </tr>
                    <tr>
                        <td class="FormLabelCell">
                            <asp:Label runat="server" ID="LabelPlacePrice" Text="<%$ Resources:SharedStrings, Placed_Price %>"></asp:Label>:
                        </td>
                        <td class="FormFieldCell">
                            <asp:Label runat="server" ID="PlacedPrice" Width="300px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabelCell">
                            <asp:Label runat="server" ID="LabelDiscount" Text="<%$ Resources:SharedStrings, Discount %>"></asp:Label>:
                        </td>
                        <td class="FormFieldCell">
                            <asp:Label runat="server" ID="Discount" Width="300px"></asp:Label>
                        </td>
                    </tr>
		            <tr>
			            <td class="FormLabelCell">
				            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:SharedStrings, Quantity %>"></asp:Label>:
			            </td>
			            <td class="FormFieldCell">
				            <asp:Label runat="server" ID="QuantityText" Width="300px"></asp:Label>
			            </td>
		            </tr>
		            <tr>
			            <td class="FormLabelCell">
				            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:OrderStrings, Return_Quantity %>"></asp:Label>:
			            </td>
			            <td class="FormFieldCell">
				            <asp:TextBox runat="server" ID="ReturnQuantity"></asp:TextBox>
				            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ValidationGroup="LineItemDetails"
					            ControlToValidate="ReturnQuantity" Display="Dynamic" ErrorMessage="*" />
				            <asp:RangeValidator ID="QuantityRangeValidator" runat="server" ValidationGroup="LineItemDetails"
					            ControlToValidate="ReturnQuantity" Display="Dynamic" ErrorMessage="*" Type="Double"
					            MinimumValue="0" MaximumValue="1000000000"></asp:RangeValidator>
                            <asp:CompareValidator ID="CompareValidator" runat="server"
                                ControlToValidate="ReturnQuantity" ErrorMessage="*"
                                Operator="GreaterThan" Type="Double"
                                ValueToCompare="0" />
			            </td>
		            </tr>
		            <tr>
			            <td class="FormLabelCell" id="DivReturnReason" runat="server">
				            <asp:Label ID="LabelReturnReason" runat="server" Text="<%$ Resources:OrderStrings, Return_Reason %>"></asp:Label>:
			            </td>
			            <td class="FormFieldCell">
				            <asp:DropDownList runat="server" ID="ReturnReasonList"></asp:DropDownList>
			            </td>
		            </tr>
		        </ContentTemplate>
	        </asp:UpdatePanel>
		    <tr>
			    <td align="right" colspan="2" style="padding-top: 10px; padding-right: 10px;">
				    <mc2:IMButton ID="btnSave" runat="server" style="width: 105px;" OnServerClick="btnSave_ServerClick">
				    </mc2:IMButton>
				    &nbsp;
				    <mc2:IMButton ID="btnCancel" runat="server" style="width: 105px;" CausesValidation="false">
				    </mc2:IMButton>
			    </td>
		    </tr>
	    </table>
	</asp:Panel>
</div>