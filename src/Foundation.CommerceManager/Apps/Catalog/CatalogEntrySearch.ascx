<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Catalog.CatalogEntrySearch"
    CodeBehind="CatalogEntrySearch.ascx.cs" %>
<%@ Register Src="../Core/Controls/EcfListViewControl.ascx" TagName="EcfListViewControl"
    TagPrefix="core" %>
<IbnWebControls:McDock ID="DockTop" runat="server" Anchor="Top" EnableSplitter="False"
    DefaultSize="152">
    <DockItems>
        <asp:Panel runat="server" ID="pnlMain" DefaultButton="btnSearch" Height="152px" CssClass="searchPanel">
            <div id="DataForm">
                <table cellpadding="4" style="background-color: #F8F8F8;">
                    <tr>
                        <td class="FormLabelCell">
                            <b><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Search_By_Keyword %>" />:</b>
                        </td>
                        <td class="FormFieldCell">
                            <asp:TextBox ID="tbKeywords" Width="240" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="upSearchButton" ChildrenAsTriggers="true" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="btnSearch" runat="server" Width="100" Text="<%$ Resources:SharedStrings, Search %>" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" class="FormSectionCell">
                            <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Additional_Filters %>" />
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabelCell">
                            <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Filter_By_Language %>" />:
                        </td>
                        <td class="FormFieldCell" colspan="2">
                            <asp:DropDownList runat="server" ID="ListLanguages" Width="300" DataValueField="LanguageId"
                                DataTextField="Name">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabelCell">
                            <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Filter_By_Catalog %>" />:
                        </td>
                        <td class="FormFieldCell" colspan="2">
                            <asp:DropDownList runat="server" ID="ListCatalogs" Width="300" DataValueField="CatalogId"
                                DataTextField="Name">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabelCell">
                            <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Search_By_Code_Id %>" />:
                        </td>
                        <td class="FormFieldCell" colspan="2">
                            <asp:TextBox ID="UniqueId" Width="100" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
    </DockItems>
</IbnWebControls:McDock>
<core:EcfListViewControl ID="MyListView" runat="server" AppId="Catalog" ViewId="CatalogEntrySearch-List"
    ShowTopToolbar="true"></core:EcfListViewControl>
<catalog:CatalogSearchDataSource runat="server" ID="CatalogSearchDataSource1" EnableViewState="false" DataMode="DataSet">
</catalog:CatalogSearchDataSource>
