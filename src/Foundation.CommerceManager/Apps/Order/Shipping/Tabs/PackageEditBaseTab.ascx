<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PackageEditBaseTab.ascx.cs" Inherits="Mediachase.Commerce.Manager.Order.Shipping.Tabs.PackageEditBaseTab" %>
<script type="text/javascript">
function ToggleDimensionsTable()
{
    var fs = document.getElementById('fsDimensions');
    var chb = $get('<%= chbDimensions.ClientID %>');
    
    var oWidth = $get('<%= tbWidth.ClientID %>');
    var oLength = $get('<%= tbLength.ClientID %>');
    var oHeight = $get('<%= tbHeight.ClientID %>');
    
    if(fs && chb)
    {
        if(chb.checked)
            fs.disabled = false;
        else
            fs.disabled = true;

        if(oWidth)
            oWidth.disabled = !chb.checked;
            
        if(oLength)
            oLength.disabled = !chb.checked;
            
        if(oHeight)
            oHeight.disabled = !chb.checked;
    }
}
</script>
<div id="DataForm">
    <table class="DataForm">
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="lblName" runat="server" Text="<%$ Resources:SharedStrings, Name %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="tbName" Width="200px" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfName" runat="server" ErrorMessage="<%$ Resources:SharedStrings, Name_Required %>"
                    Font-Size="9pt" Font-Names="verdana" Display="Dynamic" ControlToValidate="tbName" />
                 <asp:RegularExpressionValidator ID="regexTextBox1" Text="<%$ Resources:SharedStrings, Name_MaxLength %>" 
                     ControlToValidate="tbName" runat="server" ValidationExpression="^[\s\S]{0,100}$"
                     Font-Size="9pt" Font-Names="verdana" Display="Dynamic"/>
                <asp:CustomValidator runat="server" ID="NameCustomValidator" ControlToValidate="tbName" OnServerValidate="ShippingPackageNameCheck" Display="Dynamic" ErrorMessage="<%$ Resources:OrderStrings, Package_Name_Exists %>" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="lblDescription" runat="server" Text="<%$ Resources:SharedStrings, Description %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="tbDescription" Rows="4" TextMode="MultiLine" Width="350px"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Text="<%$ Resources:SharedStrings, Description_MaxLength %>" 
                     ControlToValidate="tbDescription" runat="server" ValidationExpression="^[\s\S]{0,225}$"
                     Font-Size="9pt" Font-Names="verdana" Display="Dynamic"/>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
            <td class="FormLabelCell" style="vertical-align: middle;" colspan="2">
                 <asp:CheckBox runat="server" ID="chbDimensions" OnCheckedChanged="Check_Clicked" AutoPostBack="true" />
                <label for="<%= chbDimensions.ClientID %>">&nbsp;<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:SharedStrings, Set_Dimensions %>"/></label>
            </td>
        </tr>
        <tr>
            <td class="FormFieldCell" colspan="2">
                <fieldset id="fsDimensions">
                    <legend>
                        <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:SharedStrings, Dimensions %>" />
                    </legend>
                    <table id="tblDimensions" runat="server">
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="lblWidth" runat="server" Text="<%$ Resources:SharedStrings, Width %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" ID="tbWidth" Width="150px"></asp:TextBox>
                                <asp:RequiredFieldValidator id="vldValue_Required" runat="server" ErrorMessage="*" ControlToValidate="tbWidth"	Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:CustomValidator id="WidthValidator" Runat="server" OnServerValidate="WidthValidate" ControlToValidate="tbWidth" Display="Dynamic">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="FormSpacerCell"></td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:SharedStrings, Length %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" ID="tbLength" Width="150px"></asp:TextBox>
                                <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="tbLength"	Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:CustomValidator id="LengthValidator" Runat="server" OnServerValidate="LengthValidate" ControlToValidate="tbLength" Display="Dynamic">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="FormSpacerCell"></td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:SharedStrings, Height %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" ID="tbHeight" Width="150px"></asp:TextBox>
                                <asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="tbHeight"	Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:CustomValidator id="HeightValidator" Runat="server" OnServerValidate="HeightValidate" ControlToValidate="tbHeight" Display="Dynamic">*</asp:CustomValidator>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
    </table>
</div>
