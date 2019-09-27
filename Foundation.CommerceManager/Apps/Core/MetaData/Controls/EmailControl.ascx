<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Core.MetaData.Controls.EmailControl" Codebehind="EmailControl.ascx.cs" %>

   <tr>  
     <td class="FormLabelCell"><asp:Label id="MetaLabelCtrl" runat="server" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>:</td> 
     <td class="FormFieldCell">
		<asp:TextBox MaxLength="255" Width="255" id="MetaValueCtrl" runat="server"></asp:TextBox>
		<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="MetaValueCtrl"></asp:RequiredFieldValidator>		
		<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ErrorMessage="<%$ Resources:SharedStrings, Valid_Email_Required %>" Display="Dynamic" ControlToValidate="MetaValueCtrl" ValidationExpression="^([-.\w']+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"></asp:RegularExpressionValidator>
		<br /><asp:Label CssClass="FormFieldDescription" id="MetaDescriptionCtrl" runat="server" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>
     </td> 
   </tr>
