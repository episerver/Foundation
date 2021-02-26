<%@ Control Language="c#" Inherits="EPiServer.Commerce.Payment.AuthorizeTokenEx.ConfigurePayment" Codebehind="ConfigurePayment.ascx.cs" %>
<div id="DataForm">
    <table cellpadding="0" cellspacing="2">
	    <tr>
		    <td class="FormLabelCell" colspan="2"><b><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:OrderStrings, Payment_Authorize_Net_Configure_Account %>" /></b></td>
	    </tr>
	    <tr>
		    <td class="FormLabelCell" colspan="2"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:OrderStrings, Payment_Authorize_Net_Get_Account %>" /></td>
	    </tr>
    </table>
    <br />
    <table class="DataForm">
	    <tr>
		    <td class="FormLabelCell"><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:OrderStrings, Payment_API_User %>" />:</td>
		    <td class="FormFieldCell"><asp:TextBox Runat="server" ID="User" Width="230"></asp:TextBox><br>
			    <asp:RequiredFieldValidator ControlToValidate="User" Display="dynamic" Font-Name="verdana" Font-Size="9pt" ErrorMessage="<%$ Resources:OrderStrings, Payment_User_Required %>"
				    runat="server" id="RequiredFieldValidator2"></asp:RequiredFieldValidator>
		    </td>
	    </tr>
	    <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
	    <tr>
		    <td class="FormLabelCell"><asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:OrderStrings, Payment_Transaction_Key %>" />:</td>
		    <td class="FormFieldCell"><asp:TextBox Runat="server" ID="Password" Width="230"></asp:TextBox>
		    </td>
	    </tr>
	    <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">Test mode:</td>
            <td class="FormFieldCell">
                <div class="margin-form">
                    <asp:RadioButton Checked="false" runat="server" ID="TestFlagYes" GroupName="TestFlag" /> Yes
                    <asp:RadioButton Checked="true" runat="server" ID="TestFlagNo" GroupName="TestFlag" /> No
                </div>
            </td>
        </tr>
	    <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormFieldCell" colspan="2">
                <fieldset>
                    <legend>
                        <asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:OrderStrings, Regular_Authorize_Payments %>"/>
                    </legend>
                    <table id="RegularPaymentsTable" runat="server">
                        <tr>
		                    <td class="FormLabelCell"><asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:OrderStrings, Payment_Options %>" />:</td>
		                    <td class="FormFieldCell">
			                    <asp:RadioButtonList id="RadioButtonListOptions" Width="147px" runat="server" Font-Size="8pt" Font-Names="Verdana">
				                    <asp:ListItem Value="AuthorizeOnly" Text="<%$ Resources:SharedStrings, Authorize_Only %>" />
				                    <asp:ListItem Value="Sale" Text="<%$ Resources:SharedStrings, Sale %>" />
			                    </asp:RadioButtonList>
			                    <asp:RequiredFieldValidator id="RequiredFieldValidator4" runat="server" Font-Name="verdana" Font-Size="9pt" ErrorMessage="<%$ Resources:OrderStrings, Payment_Options_Required %>"
				                    ControlToValidate="RadioButtonListOptions"></asp:RequiredFieldValidator>
		                    </td>
	                    </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
	    <tr>
            <td class="FormFieldCell" colspan="2">
                <fieldset>
                    <legend>
                        <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:OrderStrings, Recurring_Payments %>"/>
                    </legend>
                    <table id="RecurringPaymentsTable" runat="server">
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="lblRecurringMethod" runat="server" Text="<%$ Resources:OrderStrings, Recurring_Method %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:DropDownList runat="server" id="ddlRecurringMethod">
                                    <asp:ListItem Value="internal" Text="<%$ Resources:OrderStrings, RecurringMethod_Internal %>"></asp:ListItem>
                                    <asp:ListItem Value="authorize" Text="<%$ Resources:OrderStrings, RecurringMethod_Authorize %>"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="FormSpacerCell"></td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:OrderStrings, RecurringPayment_CancelStatus %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:DropDownList runat="server" id="ddlCancelStatus" DataMember="OrderStatus" DataTextField="Name" DataValueField="Name">
                                    <asp:ListItem Value="" Text="<%$ Resources:OrderStrings, RecurringPayment_Select_CancelStatus %>"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="CancelStatusRequired" ControlToValidate="ddlCancelStatus" ErrorMessage="<%$ Resources:OrderStrings, RecurringPayment_CancelStatus_Required %>" Display="Dynamic"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
    </table>
</div>