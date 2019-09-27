c<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Catalog.Modules.TemplateTab" Codebehind="TemplateTab.ascx.cs" %>
   <tr>  
     <td class="FormLabelCell"><asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, Title %>"> (<%#LanguageCode %>):</asp:Label></td> 
     <td class="FormFieldCell">
        <asp:TextBox runat="server" Width="250" ID="Title"></asp:TextBox><br />
     </td> 
   </tr> 
   <tr>  
     <td colspan="2" class="FormSpacerCell"></td> 
   </tr>        
   <tr>  
     <td class="FormLabelCell"><asp:Label ID="Label10" runat="server" Text="<%$ Resources:SharedStrings, Url %>"> (<%#LanguageCode %>):</asp:Label></td> 
     <td class="FormFieldCell">
        <asp:TextBox runat="server" Width="350" ID="UrlText"></asp:TextBox><br />
     </td> 
   </tr> 
   <tr>  
     <td colspan="2" class="FormSpacerCell"></td> 
   </tr>    
   <tr>  
     <td class="FormLabelCell"><asp:Label ID="Label9" runat="server" Text="<%$ Resources:SharedStrings, Description %>"> (<%#LanguageCode %>):</asp:Label></td> 
     <td class="FormFieldCell">
        <asp:TextBox runat="server" Width="350" TextMode="MultiLine" ID="Description"></asp:TextBox><br />
     </td> 
   </tr>    
   <tr>  
     <td colspan="2" class="FormSpacerCell"></td> 
   </tr>       
   <tr>  
     <td class="FormLabelCell"><asp:Label ID="Label11" runat="server" Text="<%$ Resources:SharedStrings, Keywords %>"> (<%#LanguageCode %>):</asp:Label></td> 
     <td class="FormFieldCell">
        <asp:TextBox runat="server" Width="350" TextMode="MultiLine" ID="Keywords"></asp:TextBox><br />
     </td> 
   </tr>          
   <tr>  
     <td colspan="2" class="FormSpacerCell"></td> 
   </tr>        
