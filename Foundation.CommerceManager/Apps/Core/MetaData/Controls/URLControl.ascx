<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Core.MetaData.Controls.URLControl" Codebehind="URLControl.ascx.cs" %>

   <tr>  
     <td class="FormLabelCell"><asp:Label id="MetaLabelCtrl" runat="server" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>:</td> 
     <td class="FormFieldCell">
		<asp:TextBox MaxLength="512" Width="500" id="MetaValueCtrl" runat="server"></asp:TextBox><br/>
		<asp:Label CssClass="FormFieldDescription" id="MetaDescriptionCtrl" runat="server" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>       
		<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="MetaValueCtrl"></asp:RequiredFieldValidator>		
     </td> 
   </tr>
