<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Catalog.Tabs.EntryOverviewEditTab" Codebehind="EntryOverviewEditTab.ascx.cs" %>
<%@ Register Src="~/Apps/Core/Controls/BooleanEditControl.ascx" TagName="BooleanEditControl" TagPrefix="ecf" %>
<%@ Register Src="~/Apps/Core/Controls/CalendarDatePicker.ascx" TagName="CalendarDatePicker" TagPrefix="ecf" %>
<%@ Register Src="~/Apps/Core/MetaData/EditTab.ascx" TagName="MetaData" TagPrefix="ecf" %>
<style type="text/css">
.ajax__validatorcallout_popup_table
{
	top: 100px;
	left:365px;
}
</style>
<script type="text/javascript">


    function extendToolkitHidePopupTimer() {
        if (AjaxControlToolkit && AjaxControlToolkit.PopupBehavior) {
            extendToolkitHidePopup();
        }
        else {
            window.setTimeout(extendToolkitHidePopup, 500);
        }
    }

    function extendToolkitHidePopup() {
        AjaxControlToolkit.PopupBehavior.prototype.hide = function() {
            var eventArgs = new Sys.CancelEventArgs();
            this.raiseHiding(eventArgs);
            if (eventArgs.get_cancel()) {
                return;
            }

            // Either hide the popup or play an animation that does
            this._visible = false;
            if (this._onHide) {
                this.onHide();
            } else {
                this._hidePopup();
                this._hideCleanup();
            }
        }
    }
    
    extendToolkitHidePopupTimer();

</script>
<div id="DataForm">
 <table class="DataForm"> 
   <tr>  
     <td class="FormLabelCell"><asp:Label runat="server" Text="<%$ Resources:SharedStrings, Name %>"></asp:Label>:</td> 
     <td class="FormFieldCell">
        <asp:TextBox runat="server" Width="250" ID="Name"></asp:TextBox><br />
        <asp:RequiredFieldValidator runat="server" ID="NameRequired" ControlToValidate="Name" Display="Dynamic" 
            ErrorMessage="<%$ Resources:CatalogStrings, Catalog_Name_Required %>" />
        <asp:CustomValidator runat="server" ID="NameLengthValidator" ControlToValidate="Name" Display="Dynamic"
            OnServerValidate="NameLengthValidator_Validate"
            ErrorMessage="<%$ Resources:CatalogStrings, Catalog_Catalog_Name_TooLong %>" />
     </td> 
   </tr> 
   <tr>  
     <td colspan="2" class="FormSpacerCell"></td> 
   </tr>   
   <tr>  
     <td class="FormLabelCell"><asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, Available_From %>"></asp:Label>:</td> 
     <td class="FormFieldCell">
         <ecf:CalendarDatePicker runat="server" ID="AvailableFrom" />
         <asp:CustomValidator runat="server" ID="AvailableFromValidator" ControlToValidate="AvailableFrom" Display="Dynamic"
            OnServerValidate="AvailableFromValidator_Validate" ErrorMessage="<%$ Resources:CatalogStrings, Entry_AvailableFrom_Too_Late %>" />
     </td> 
   </tr>   
   <tr>  
     <td colspan="2" class="FormSpacerCell"></td>
   </tr>     
   <tr>  
     <td class="FormLabelCell"><asp:Label ID="Label2" runat="server" Text="<%$ Resources:SharedStrings, Expires_On %>"></asp:Label>:</td> 
     <td class="FormFieldCell">
        <ecf:CalendarDatePicker runat="server" ID="ExpiresOn" />
        <asp:CustomValidator runat="server" ID="ExpiresOnValidator" ControlToValidate="ExpiresOn" Display="Dynamic"
            OnServerValidate="ExpiresOnValidator_Validate" ErrorMessage="<%$ Resources:CatalogStrings, Entry_ExpiresOn_Too_Early %>" />
     </td> 
   </tr>    
   <tr>  
     <td colspan="2" class="FormSpacerCell"></td> 
   </tr>
   <tr>  
     <td class="FormLabelCell"><asp:Label ID="Label9" runat="server" Text="<%$ Resources:SharedStrings, Code %>"></asp:Label>:</td> 
     <td class="FormFieldCell">
        <asp:TextBox runat="server" Width="250" ID="CodeText"></asp:TextBox>
        <asp:RequiredFieldValidator ID="CodeRequiredValidator" runat="server" ControlToValidate="CodeText" ErrorMessage="<%$ Resources:SharedStrings, Code_Required %>" Display="Dynamic"></asp:RequiredFieldValidator>
        <asp:CustomValidator runat="server" id="custPrimeCheck"
            ControlToValidate="CodeText"
            OnServerValidate="EntryCodeCheck" Display="Dynamic"
            ErrorMessage="<%$ Resources:CatalogStrings, Entry_Code_Exists %>" />        
     </td> 
   </tr> 
   <tr>  
     <td colspan="2" class="FormSpacerCell"></td> 
   </tr>   
   <tr>  
     <td class="FormLabelCell"><asp:Label ID="Label11" runat="server" Text="<%$ Resources:SharedStrings, Sort_Order %>"></asp:Label>:</td> 
     <td class="FormFieldCell">
        <asp:TextBox runat="server" Width="50" ID="SortOrder" Text="0"></asp:TextBox>
        <asp:RequiredFieldValidator runat="server" ControlToValidate="SortOrder" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
        <asp:RangeValidator runat="server" Type="Integer" ControlToValidate="SortOrder" MinimumValue="-2147483648" MaximumValue="2147483647" ErrorMessage="Invalid SortOrder value" Display="Dynamic"></asp:RangeValidator>
        <br /><asp:Label CssClass="FormFieldDescription" runat="server" Text="<%$ Resources:CatalogStrings, Entry_Sort_Order %>"></asp:Label>
     </td> 
   </tr>    
   <tr>  
     <td colspan="2" class="FormSpacerCell"></td> 
   </tr>      
   <tr>  
     <td class="FormLabelCell"><asp:Label ID="Label5" runat="server" Text="<%$ Resources:SharedStrings, Available %>"></asp:Label>:</td> 
     <td class="FormFieldCell">
        <ecf:BooleanEditControl id="IsActive" runat="server" MDContext="<%# Mediachase.Commerce.Catalog.CatalogContext.MetaDataContext %>"></ecf:BooleanEditControl>
     </td> 
   </tr>   
   <tr>  
     <td colspan="2" class="FormSpacerCell"></td>
   </tr>    
   <tr>  
     <td class="FormLabelCell"><asp:Label ID="Label6" runat="server" Text="<%$ Resources:CatalogStrings, Meta_Class %>"></asp:Label>:</td> 
     <td class="FormFieldCell">
        <asp:DropDownList runat="server" AutoPostBack="true" id="MetaClassList" DataTextField="FriendlyName" DataValueField="Id">
        </asp:DropDownList>
     </td> 
   </tr>
   <tr>  
     <td colspan="2" class="FormSpacerCell"></td> 
   </tr>      
   <ecf:MetaData runat="server" ID="MetaDataTab" MDContext="<%# Mediachase.Commerce.Catalog.CatalogContext.MetaDataContext %>" />
 </table>
</div>


