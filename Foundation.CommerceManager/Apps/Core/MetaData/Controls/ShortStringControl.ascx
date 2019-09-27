<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Core.MetaData.Controls.ShortStringControl" Codebehind="ShortStringControl.ascx.cs" %>

   <tr>  
     <td class="FormLabelCell"><asp:Label id="MetaLabelCtrl" runat="server" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>:</td> 
     <td class="FormFieldCell">
		<asp:TextBox MaxLength="255" Width="350" id="MetaValueCtrl" runat="server"></asp:TextBox><br/>
		<asp:Label CssClass="FormFieldDescription" id="MetaDescriptionCtrl" runat="server" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>      
		<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="MetaValueCtrl"></asp:RequiredFieldValidator>	
        <asp:RegularExpressionValidator id="RegExValidator" runat="server" ErrorMessage="The only special character allowed is dash (-)" Display="Dynamic" ControlToValidate="MetaValueCtrl" ValidationExpression="^[a-zA-Z0-9-]*$"></asp:RegularExpressionValidator>
     </td> 
   </tr>