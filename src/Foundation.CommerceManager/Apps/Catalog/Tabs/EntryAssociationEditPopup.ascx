<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Catalog.Tabs.EntryAssociationEditPopup" Codebehind="EntryAssociationEditPopup.ascx.cs" %>
<%@ Register Src="~/Apps/Core/MetaData/EditTab.ascx" TagName="MetaData" TagPrefix="ecf" %>
<script type="text/javascript">
    function ecf_UpdateEntryAssociationEditDialogControl(val)
    {
        var ctrl = $get('<%=DialogTrigger.ClientID%>');
        ctrl.value = val;
        __doPostBack('<%=DialogTrigger.UniqueID %>','');
    }
    function EntryAssociationEditPopup_CloseDialog() {
        document.getElementById('<%=DialogTrigger.ClientID%>').value = "";
        EntryAssociationEditDialog.close();
        CSManagementClient.MarkDirty();
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
                            <td class="FormLabelCell"><asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, Name %>"></asp:Label>:</td> 
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" Width="250" ID="tbAssociationName"></asp:TextBox><br />
                                <asp:Label ID="Label4" CssClass="FormFieldDescription" runat="server" Text="<%$ Resources:CatalogStrings, Entry_Enter_Association_Name %>"></asp:Label>
                                <asp:RequiredFieldValidator runat="server" ID="AssociationNameRequired" ValidationGroup="AssocValidationGroup" ControlToValidate="tbAssociationName" Display="Dynamic" ErrorMessage="<%$ Resources:CatalogStrings, Entry_Association_Name_Required %>" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"  ControlToValidate="tbAssociationName" ValidationGroup="AssocValidationGroup" ValidationExpression="\w+" ErrorMessage="*"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell"><asp:Label ID="Label2" runat="server" Text="<%$ Resources:SharedStrings, Description %>"></asp:Label>:</td> 
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" ID="tbDescription" Rows="10" TextMode="MultiLine" Columns="50"></asp:TextBox><br />
                                <asp:Label ID="Label5" CssClass="FormFieldDescription" runat="server" Text="<%$ Resources:CatalogStrings, Entry_Enter_Association_Description %>"></asp:Label>
                            
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell"><asp:Label ID="Label3" runat="server" Text="<%$ Resources:SharedStrings, Sort_Order %>"></asp:Label>:</td> 
                            <td class="FormFieldCell">
                                <asp:TextBox runat="server" ID="tbSortOrder"></asp:TextBox><br />
                                <asp:Label ID="Label6" CssClass="FormFieldDescription" runat="server" Text="<%$ Resources:CatalogStrings, Entry_Enter_Association_Sort_Order %>"></asp:Label>
                                <asp:RequiredFieldValidator runat="server" ID="SortOrderValidator" ValidationGroup="AssocValidationGroup" ControlToValidate="tbSortOrder" Display="Dynamic" ErrorMessage="<%$ Resources:CatalogStrings, Entry_Association_Sort_Order_Required %>" />
                                <asp:RangeValidator runat="server" ID="SortOrderRangeValidator" ValidationGroup="AssocValidationGroup" Type="Integer" MinimumValue="0" MaximumValue="2000000" ControlToValidate="tbSortOrder" Display="Dynamic" ErrorMessage="<%$ Resources:CatalogStrings, Entry_Wrong_Value_Enter_Valid_Integer %>"></asp:RangeValidator>
                             </td>
                        </tr>
                        <!-- Note: values are set in textboxes because otherwise validators show up when dialog is loaded for the first time -->
                        <ecf:MetaData ValidationGroup="AssocValidationGroup" runat="server" ID="MetaDataTab" />
                    </table>
                     <!-- /Content Area -->
                </td>
            </tr>
            <tr>
                <td style="background-image: url('<%= this.ResolveUrl("~/Apps/Shell/Styles/images/dialog/bottom_content.gif")%>'); height: 41px; padding-right: 10px;" align="right">
                    <asp:Button runat="server" ID="SaveChangesButton" ValidationGroup="AssocValidationGroup" OnClick="SaveChangesButton_Click" Text="<%$ Resources:CatalogStrings, Entry_Save_Changes %>" />
                </td>
                <td style="background-image: url('<%= this.ResolveUrl("~/Apps/Shell/Styles/images/dialog/bottom_content.gif")%>');
					height: 41px; padding-right: 10px;" align="right">
					<asp:Button runat="server" ID="CancelChangesButton" causesvalidation="false"
						OnClientClick="EntryAssociationEditPopup_CloseDialog()" Text="<%$ Resources:CatalogStrings, Entry_Cancel_Changes %>" />
				</td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>