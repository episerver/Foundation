<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RoleFilter.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Customer.Modules.RoleFilter" %>
<ComponentArt:ComboBox ID="CbxRoleFilter" runat="server" 
	AutoHighlight="false" 
	AutoFilter="true" 
	AutoComplete="true" 
	ItemClientTemplateId="itemTemplate" 
	Width="320" 
	DropDownPageSize="7"
	CssClass="comboBox"
	HoverCssClass="comboBoxHover"
	FocusedCssClass="comboBoxHover"
	TextBoxCssClass="comboTextBox"
	TextBoxHoverCssClass="comboBoxHover"
	DropDownCssClass="comboDropDown"
	ItemCssClass="comboItem"
	ItemHoverCssClass="comboItemHover"
	SelectedItemCssClass="comboItemHover"
	DropHoverImageUrl="~/Apps/Shell/styles/images/combobox/drop_hover.gif"
	DropImageUrl="~/Apps/Shell/styles/images/combobox/drop.gif"
	>
    <ClientTemplates>
        <ComponentArt:ClientTemplate ID="itemTemplate">
            <img src="## DataItem.getProperty('icon') ##" />
            ## DataItem.getProperty('Text') ##</ComponentArt:ClientTemplate>
    </ClientTemplates>
</ComponentArt:ComboBox>
<asp:RequiredFieldValidator ID="RequiredValidator" runat="server" ControlToValidate="CbxRoleFilter" Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
