<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Catalog.CatalogBatchUpdate"
    CodeBehind="CatalogBatchUpdate.ascx.cs" %>
<%@ Register Src="../Core/Controls/EcfListViewControl.ascx" TagName="EcfListViewControl"
    TagPrefix="core" %>
<IbnWebControls:McDock ID="DockTop" runat="server" Anchor="Top" EnableSplitter="False"
    DefaultSize="145">
    <DockItems>
        <asp:Panel runat="server" ID="pnlMain" DefaultButton="btnSearch" Height="145px" BackColor="#F8F8F8"
            BorderColor="Gray" BorderWidth="0">
            <div id="DataForm">
                <table width="100%">
                  <col width="30%" />
                  <col width="30%" />
                  <col width="30%" />
                  <tr>
                    <td>
                      <table cellpadding="4" style="background-color: #F8F8F8;" width="100%">
                        <tr>
                            <td colspan="3" class="FormSectionCell">
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Main_Adjustment %>" />
                            </td>
                        </tr>                      
                         <tr>
                            <td class="FormLabelCell">
                                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Entry_Type %>" />:
                            </td>
                            <td class="FormFieldCell" colspan="2">
                              <asp:DropDownList runat="server" ID="ListTypes" EnableViewState="true" Width="200"  AutoPostBack="true" OnSelectedIndexChanged="ListTypes_SelectedIndexChanged">
                                <asp:ListItem Value="[all]" Text="<%$ Resources:CatalogStrings, Catalog_All_Types %>" Selected="True"/>
                                <asp:ListItem Value="Product" Text="<%$ Resources:CatalogStrings, Catalog_Product %>"/>
                                <asp:ListItem Value="Variation" Text="<%$ Resources:CatalogStrings, Catalog_Variation_Sku %>"/>
                                <asp:ListItem Value="Package" Text="<%$ Resources:SharedStrings, Package %>"/>
                                <asp:ListItem Value="Bundle" Text="<%$ Resources:SharedStrings, Bundle %>"/>
                                <asp:ListItem Value="DynamicPackage" Text="<%$ Resources:CatalogStrings, Catalog_Dynamic_Package %>"/>
                              </asp:DropDownList>
                                
                            </td>
                          </tr>
                          <tr>
                              <td class="FormLabelCell">
                                  <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:CatalogStrings, Meta_Class %>" />:
                              </td>
                              <td class="FormFieldCell" colspan="2">
                                <asp:DropDownList runat="server" ID="ddlMetaClassList" AutoPostBack="true" EnableViewState="true" Width="200" DataTextField="FriendlyName" DataValueField="Id" OnSelectedIndexChanged="ddlMetaClassList_SelectedIndexChanged"/>
                              </td>
                          </tr>
                          <tr>
                              <td class="FormLabelCell">
                                  <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:SharedStrings, Field %>" />:
                              </td>
                              <td class="FormFieldCell" colspan="2">
                                <asp:DropDownList runat="server" ID="ddlFieldList" EnableViewState="true" Width="200" AutoPostBack="true" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged"/>
                              </td>
                          </tr>
                          <tr>
                              <td class="FormLabelCell">
                                  <asp:Literal ID="Literal9" runat="server" Text="Warehouse" />:
                              </td>
                              <td class="FormFieldCell" colspan="2">
                                <asp:DropDownList runat="server" ID="ddlWarehouseId" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged"/>
                              </td>
                          </tr>
                      </table>
                  </td>
                  <td>
                    <table cellpadding="4" style="background-color: #F8F8F8;" width="100%">
                        <tr>
                            <td colspan="3" class="FormSectionCell">
                                <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Additional_Filters %>" />
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:SharedStrings, Language %>" />:
                            </td>
                            <td class="FormFieldCell" colspan="2">
                                <asp:DropDownList runat="server" ID="ListLanguages" Width="200" DataValueField="LanguageId" AutoPostBack="true" DataTextField="Name" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Catalog_Category %>" />:
                            </td>
                            <td class="FormFieldCell" colspan="2">
                                <asp:DropDownList runat="server" ID="ListCatalogs" Width="200" AutoPostBack="true" OnSelectedIndexChanged="ddlCategoryList_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Keyword %>" />:
                            </td>
                            <td class="FormFieldCell">
                                <asp:TextBox ID="tbKeywords" Width="196" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                      </table>
                    </td>
                    <td valign="bottom">
                    <table border="0" cellspacing="5" style="background-color: #F8F8F8;" width="100%">
                      <tr>
                        <td valign="bottom">
                          <asp:UpdatePanel ID="upSearchButton" ChildrenAsTriggers="true" runat="server" UpdateMode="Conditional">
                              <ContentTemplate>
                                  <asp:Button ID="btnSearch" runat="server" Width="90" Text="<%$ Resources:CatalogStrings, Catalog_Apply_Filter %>" />
                              </ContentTemplate>
                          </asp:UpdatePanel>
                        </td>
                      </tr>
                    </table>
                    </td>
                  </tr>
                 </table>
            </div>
        </asp:Panel>
    </DockItems>
</IbnWebControls:McDock>
<core:EcfListViewControl ID="MyListView" runat="server" AppId="Catalog" ViewId="CatalogBatchUpdate-List" ShowTopToolbar="true"></core:EcfListViewControl>
<catalog:CatalogSearchDataSource runat="server" ID="CatalogSearchDataSource1" DataMode="DataSet">
</catalog:CatalogSearchDataSource>