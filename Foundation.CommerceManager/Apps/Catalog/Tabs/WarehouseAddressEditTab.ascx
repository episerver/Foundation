<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WarehouseAddressEditTab.ascx.cs" Inherits="Mediachase.Commerce.Manager.Catalog.Tabs.WarehouseAddressEditTab" %>
<style type="text/css">
.ajax__validatorcallout_popup_table
{
	top: 0px;
}
</style>
<div id="DataForm">
    <table class="DataForm">
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, First_Name %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="FirstName" ></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator0" ControlToValidate="FirstName" Display="Dynamic" ErrorMessage="<%$ Resources:CatalogStrings, Address_Required %>" />
            </td>
        </tr>
        <tr>  
            <td colspan="2" class="FormSpacerCell"></td> 
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:SharedStrings, Last_Name %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="LastName" ></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1"  ControlToValidate="LastName" Display="Dynamic" ErrorMessage="*" />
            </td>
        </tr>
        <tr>  
            <td colspan="2" class="FormSpacerCell"></td> 
        </tr>        
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:SharedStrings, Organization %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="Organization"></asp:TextBox>
            </td>
        </tr>
        <tr>  
            <td colspan="2" class="FormSpacerCell"></td> 
        </tr>        
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:SharedStrings, Line_1 %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="Line1" ></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2"  ControlToValidate="Line1" Display="Dynamic" ErrorMessage="*" />
            </td>
        </tr>
        <tr>  
            <td colspan="2" class="FormSpacerCell"></td> 
        </tr>        
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:SharedStrings, Line_2 %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="Line2"></asp:TextBox>
            </td>
        </tr>
        <tr>  
            <td colspan="2" class="FormSpacerCell"></td> 
        </tr>        
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:SharedStrings, City %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="City" ></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3"  ControlToValidate="City" Display="Dynamic" ErrorMessage="*" />
            </td>
        </tr>
        <tr>  
            <td colspan="2" class="FormSpacerCell"></td> 
        </tr>        
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:SharedStrings, State %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="State" ></asp:TextBox>
            </td>
        </tr>
        <tr>  
            <td colspan="2" class="FormSpacerCell"></td> 
        </tr>        
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:SharedStrings, Country_Code %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="CountryCode" ></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5"  ControlToValidate="CountryCode" Display="Dynamic" ErrorMessage="*" />
            </td>
        </tr>
        <tr>  
            <td colspan="2" class="FormSpacerCell"></td> 
        </tr>        
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:SharedStrings, Country_Name %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="CountryName"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator8"  ControlToValidate="CountryName" Display="Dynamic" ErrorMessage="*" />
            </td>
        </tr>
        <tr>  
            <td colspan="2" class="FormSpacerCell"></td> 
        </tr>        
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:SharedStrings, Postal_Code %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="PostalCode" ></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6"  ControlToValidate="PostalCode" Display="Dynamic" ErrorMessage="*" />
            </td>
        </tr>
        <tr>  
            <td colspan="2" class="FormSpacerCell"></td> 
        </tr>        
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label13" runat="server" Text="<%$ Resources:SharedStrings, Region_Code %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="RegionCode"></asp:TextBox>
            </td>
        </tr>
        <tr>  
            <td colspan="2" class="FormSpacerCell"></td> 
        </tr>        
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label14" runat="server" Text="<%$ Resources:SharedStrings, Region_Name %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="RegionName"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator9"  ControlToValidate="RegionName" Display="Dynamic" ErrorMessage="*" />
            </td>
        </tr>
        <tr>  
            <td colspan="2" class="FormSpacerCell"></td> 
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label15" runat="server" Text="<%$ Resources:SharedStrings, Day_Phone %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="DayTimePhone"></asp:TextBox>
            </td>
        </tr>
                <tr>  
            <td colspan="2" class="FormSpacerCell"></td> 
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label16" runat="server" Text="<%$ Resources:SharedStrings, Evening_Phone %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="EveningPhone"></asp:TextBox>
            </td>
        </tr>
        <tr>  
            <td colspan="2" class="FormSpacerCell"></td> 
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label17" runat="server" Text="<%$ Resources:SharedStrings, Fax_Number %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="FaxNumber"></asp:TextBox>
            </td>
        </tr>
        <tr>  
            <td colspan="2" class="FormSpacerCell"></td> 
        </tr>        
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label18" runat="server" Text="<%$ Resources:SharedStrings, Email %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="Email" ></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" ControlToValidate="Email" Display="Dynamic" ErrorMessage="*" />
                <asp:regularexpressionvalidator id="RegularExpressionValidator1" runat="server" ControlToValidate="Email" Display="Dynamic" ErrorMessage="*" ValidationExpression="[\w\.-]+(\+[\w-]*)?@([\w-]+\.)+[\w-]+"></asp:regularexpressionvalidator>
            </td>
        </tr>
    </table>
</div>