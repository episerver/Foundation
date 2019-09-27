<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Order.Tabs.AddressEditTab" Codebehind="AddressEditTab.ascx.cs" %>
<%@ Register Src="~/Apps/Core/MetaData/EditTab.ascx" TagName="MetaData" TagPrefix="ecf" %>
<script type="text/javascript">
    function ecf_UpdateAddressDialogControl(val) {
        var btn = $get('<%= SaveChangesButton.ClientID %>');
        if (btn != null)
            btn.disabled = false;
        
        var ctrl = $get('<%=DialogTrigger.ClientID%>');
        ctrl.value = val;
        __doPostBack('<%=DialogTrigger.UniqueID %>', '');
    }

    function OrderAddressSaveChangesButton_onClientClick(btn) {
        if (Page_ClientValidate('AddressMetaData')) {
            btn.disabled = true;
            __doPostBack('<%= SaveChangesButton.UniqueID %>', 'addressChanged');
        }
    }
</script>

<asp:HiddenField runat="server" id="DialogTrigger"/>

<asp:UpdatePanel UpdateMode="Conditional" ID="DialogContentPanel" runat="server" RenderMode="Inline">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="DialogTrigger" />
    </Triggers>
    <ContentTemplate>
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr>
                <td valign="middle" style="padding: 1px; width: 5px;">
                </td>
                <td style="padding: 1px;" align="left" valign="middle">
                    <!-- Content Area -->
                    <table class="DataForm">
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:SharedStrings, Name %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" ID="Name"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, First_Name %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" ID="FirstName" ValidationGroup="AddressMetaData"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator0" ValidationGroup="AddressMetaData" ControlToValidate="FirstName" Display="Dynamic" ErrorMessage="*" />
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:SharedStrings, Last_Name %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" ID="LastName" ValidationGroup="AddressMetaData"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ValidationGroup="AddressMetaData" ControlToValidate="LastName" Display="Dynamic" ErrorMessage="*" />
                            </td>
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
                            <td class="FormLabelCell">
                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:SharedStrings, Line_1 %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" ID="Line1" ValidationGroup="AddressMetaData"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="AddressMetaData" ControlToValidate="Line1" Display="Dynamic" ErrorMessage="*" />
                            </td>
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
                            <td class="FormLabelCell">
                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:SharedStrings, City %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" ID="City" ValidationGroup="AddressMetaData"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ValidationGroup="AddressMetaData" ControlToValidate="City" Display="Dynamic" ErrorMessage="*" />
                            </td>
                        </tr>    
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:SharedStrings, State %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" ID="State" ValidationGroup="AddressMetaData"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ValidationGroup="AddressMetaData" ControlToValidate="State" Display="Dynamic" ErrorMessage="*" />
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:SharedStrings, Country_Code %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" ID="CountryCode" ValidationGroup="AddressMetaData"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ValidationGroup="AddressMetaData" ControlToValidate="CountryCode" Display="Dynamic" ErrorMessage="*" />
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:SharedStrings, Country_Name %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" ID="CountryName"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:SharedStrings, Postal_Code %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" ID="PostalCode" ValidationGroup="AddressMetaData"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" ValidationGroup="AddressMetaData" ControlToValidate="PostalCode" Display="Dynamic" ErrorMessage="*" />
                            </td>
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
                            <td class="FormLabelCell">
                                <asp:Label ID="Label14" runat="server" Text="<%$ Resources:SharedStrings, Region_Name %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" ID="RegionName"></asp:TextBox>
                            </td>
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
                            <td class="FormLabelCell">
                                <asp:Label ID="Label16" runat="server" Text="<%$ Resources:SharedStrings, Evening_Phone %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" ID="EveningPhone"></asp:TextBox>
                            </td>
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
                            <td class="FormLabelCell">
                                <asp:Label ID="Label18" runat="server" Text="<%$ Resources:SharedStrings, Email %>"></asp:Label>:
                            </td>
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" ID="Email" ValidationGroup="AddressMetaData"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" ValidationGroup="AddressMetaData" ControlToValidate="Email" Display="Dynamic" ErrorMessage="*" />
                                <asp:regularexpressionvalidator id="RegularExpressionValidator1" runat="server" ValidationGroup="AddressMetaData" ControlToValidate="Email" Display="Dynamic" ErrorMessage="*" ValidationExpression="[\w\.-]+(\+[\w-]*)?@([\w-]+\.)+[\w-]+"></asp:regularexpressionvalidator>
                            </td>
                        </tr>
                        <asp:PlaceHolder runat="server" ID="MetaPlaceHolder"></asp:PlaceHolder>
                        <ecf:MetaData ValidationGroup="AddressMetaData" runat="server" ID="MetaDataTab" />                                 
                    </table>
                    <!-- /Content Area -->
                </td>
            </tr>
            <tr>
                <td colspan="2" style="background-image: url(../../../Apps/Shell/Styles/images/dialog/bottom_content.gif);
                    height: 41px; padding-right: 10px;" align="right">
                    <asp:Button runat="server" ID="SaveChangesButton" ValidationGroup="AddressMetaData" OnClientClick="OrderAddressSaveChangesButton_onClientClick(this);return false;" text="<%$ Resources:SharedStrings, Save_Changes %>" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>