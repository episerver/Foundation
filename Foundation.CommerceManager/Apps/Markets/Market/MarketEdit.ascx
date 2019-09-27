<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Mediachase.Commerce.Manager.Markets.Market.MarketEdit" Codebehind="MarketEdit.ascx.cs" %>
<%@ Register Src="~/Apps/Core/SaveControl.ascx" TagName="SaveControl" TagPrefix="ecf" %>
<%@ Register Src="~/Apps/Core/Controls/EditViewControl.ascx" TagName="EditViewControl" TagPrefix="ecf" %>
<div class="editDiv">
<ecf:EditViewControl AppId="Markets" ViewId="Market-Edit" id="ViewControl" runat="server"></ecf:EditViewControl>
<ecf:SaveControl id="EditSaveControl" 
    CancelMessage="<%$ Resources:SharedStrings, Market_Changes_Discarded %>" 
    SavedMessage="<%$ Resources:SharedStrings, Market_Updated %>" 
    CancelClientScript="CSManagementClient.CloseTab();CSManagementClient.ChangeView('Markets','Market-List');" 
    SavedClientScript="CSManagementClient.CloseTab();CSManagementClient.ChangeView('Markets', 'Market-List');" runat="server"></ecf:SaveControl>
</div>