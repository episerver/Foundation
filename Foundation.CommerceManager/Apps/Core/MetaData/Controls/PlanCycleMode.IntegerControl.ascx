<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Core.MetaData.MetaControls.PlanCycleMode_IntegerControl" Codebehind="PlanCycleMode.IntegerControl.ascx.cs" %>
<tr>
	<td class="FormLabelCell"><asp:Label id="MetaLabelCtrl" runat="server" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>:</td>
	<td class="FormFieldCell">
		<asp:DropDownList runat="server" ID="MetaValueCtrl">
		    <asp:ListItem Value="0" Text="<%$ Resources:CoreStrings, Cycle_No %>"></asp:ListItem>
		    <asp:ListItem Value="1" Text="<%$ Resources:CoreStrings, Cycle_Daily %>"></asp:ListItem>
		    <asp:ListItem Value="2" Text="<%$ Resources:CoreStrings, Cycle_Weekly %>"></asp:ListItem>
		    <asp:ListItem Value="3" Text="<%$ Resources:CoreStrings, Cycle_Monthly %>"></asp:ListItem>
		    <asp:ListItem Value="4" Text="<%$ Resources:CoreStrings, Cycle_Yearly %>"></asp:ListItem>
		    <asp:ListItem Value="5" Text="<%$ Resources:CoreStrings, Cycle_Custom1 %>"></asp:ListItem>
		    <asp:ListItem Value="6" Text="<%$ Resources:CoreStrings, Cycle_Custom2 %>"></asp:ListItem>		    
		</asp:DropDownList>
		<br><asp:Label CssClass="FormFieldDescription" id="MetaDescriptionCtrl" runat="server" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>
	</td>
</tr>
