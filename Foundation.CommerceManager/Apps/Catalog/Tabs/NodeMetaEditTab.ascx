<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Catalog.Tabs.NodeMetaEditTab" Codebehind="NodeMetaEditTab.ascx.cs" %>
<%@ Register Src="~/Apps/Core/MetaData/EditTab.ascx" TagName="MetaData" TagPrefix="ecf" %>

<div id="DataForm">
 <table class="DataForm"> 
   <tr>  
     <td colspan="2"></td> 
   </tr>

     <ecf:MetaData runat="server" ID="MetaDataTab" MDContext="<%# Mediachase.Commerce.Catalog.CatalogContext.MetaDataContext %>" />
    </table>
</div>


