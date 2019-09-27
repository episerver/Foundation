<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Core.MetaData.Controls.DateTimeMetaControl" Codebehind="DateTimeMetaControl.ascx.cs" %>
<%@ Register TagPrefix="core" TagName="CalendarDatePicker" src="~/Apps/Core/Controls/CalendarDatePicker.ascx" %>

   <tr>  
     <td class="FormLabelCell"><asp:Label id="MetaLabelCtrl" runat="server" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>:</td> 
     <td class="FormFieldCell">
        <core:CalendarDatePicker id="DTClientControl1" runat="server"/><br />
        <asp:Label id="MetaDescriptionCtrl" runat="server" CssClass="FormFieldDescription" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>
        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="DTClientControl1" Display="Dynamic" ErrorMessage="<%$ Resources:SharedStrings, Required_Field_Missing %>" />
     </td> 
   </tr>