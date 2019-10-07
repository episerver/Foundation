<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Core.MetaData.Admin.MetaClassesControl" Codebehind="MetaClassesControl.ascx.cs" %>
<%@ Register TagPrefix="console" Namespace="Mediachase.Web.Console.Controls" Assembly="Mediachase.WebConsoleLib" %>
<%@ Register TagPrefix="mc" TagName="MetaToolbar" Src="~/Apps/Core/Controls/MetaToolbar.ascx" %>
<%@ Register Src="~/Apps/Core/SaveControl.ascx" TagName="SaveControl" TagPrefix="ecf" %>
<IbnWebControls:McDock ID="DockTop" runat="server" Anchor="top" EnableSplitter="False" DefaultSize="26">
    <DockItems>
        <asp:UpdatePanel runat="server" id="panelToolbar" UpdateMode="Conditional">
			<ContentTemplate>
                <table runat="server" id="topTable" cellspacing="0" cellpadding="0" border="0" width="100%">
	                <tr>
		                <td style="padding-left: 0px; padding-right: 0px;">
			                <mc:MetaToolbar runat="server" ID="MetaToolbar1" />
		                </td>
	                </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </DockItems>
</IbnWebControls:McDock>
<div id="FormMultiPage">
    <table class="FormMultiPage">
        <tr>
            <td class="FormLabelCell">
                <table>
                    <tr>
                        <td class="FormLabelCell">
                            <asp:Label ID="LblElement" runat="server"><%=RM.GetString("ATTRIBUTECLASSES_ELEMENT")%>:</asp:Label>
                        </td>
                        <td class="FormFieldCell">
                            <asp:DropDownList runat="server" ID="ddlElement" AutoPostBack="True" Width="200"
                                OnSelectedIndexChanged="ddlElement_SelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr runat="server" id="trType">
                        <td class="FormLabelCell">
                            <asp:Label ID="LblType" runat="server"><%=RM.GetString("ATTRIBUTECLASSES_TYPE")%>:</asp:Label>
                        </td>
                        <td class="FormFieldCell">
                            <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="True" Width="200" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <asp:PlaceHolder runat="server" ID="tblMetaClass">
                        <tr>
                            <td class="FormLabelCell">
                                <%=RM.GetString("ATTRIBUTECLASSEDIT_NAME")%>:</td>
                            <td class="FormFieldCell">
                                <asp:TextBox ID="Name" Width="400" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <%=RM.GetString("ATTRIBUTECLASSEDIT_FRIENDLYNAME")%>:</td>
                            <td class="FormFieldCell">
                                <asp:TextBox ID="FriendlyName" Width="400" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorFriendlyName" ControlToValidate="FriendlyName"
                                    Display="Dynamic" Font-Name="verdana" Font-Size="9pt" ErrorMessage="<%$ Resources:SharedStrings, Friendly_Name_Required %>"
                                    runat="server"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabelCell">
                                <%=RM.GetString("ATTRIBUTECLASSEDIT_DESCRIPTION")%>:</td>
                            <td class="FormFieldCell">
                                <asp:TextBox ID="Description" TextMode="MultiLine" Columns="80" Rows="8" Width="400"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trMultiLanguageFieldInOrderClassWarning" runat="server" Visible="false">
                            <td colspan="2" class="FormFieldCell">
                                <asp:Label runat="server" ForeColor="Red"><%=RM.GetString("ATTRIBUTECLASSES_MULTILANGUAGEORDERFIELD_WARNING")%></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="FormFieldCell">
                                <asp:DataGrid ID="ItemsGrid" runat="server" CssClass="Grid" AllowPaging="False" Width="100%"
                                    ShowFooter="False" DataKeyField="MetaFieldId" AutoGenerateColumns="False">
                                    <ItemStyle CssClass="GridItem"></ItemStyle>
									<AlternatingItemStyle CssClass="AlternatingRow_GridItem"></AlternatingItemStyle>
                                    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                                    <FooterStyle CssClass="GridFooter"></FooterStyle>
                                    <Columns>
                                        <console:RowSelectorColumn ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center" allowselectall="false" SelectionMode="Multiple"
                                            HeaderText="Select" AutoPostBack="True" OnSelectionChanged="ItemsGrid_SelectionChanged" />
                                        <asp:TemplateColumn HeaderText="Sort">
                                            <ItemTemplate>
                                                <asp:TextBox Width="50" runat="server" ID="Weight"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Name" ItemStyle-Width="80%">
                                            <ItemTemplate>
                                                <asp:HyperLink runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FriendlyName")%>'
                                                    NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.MetaFieldId", "javascript:CSManagementClient.ChangeView(\""+AppId+"\", \""+MFView+"\",\"id={0}&fieldnamespace="+FieldNamespace+"\");") %>'
                                                    ID="Hyperlink2" NAME="Hyperlink1">
                                                </asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                </table>
            </td>
        </tr>
    </table>
</div>
<ecf:SaveControl ID="EditSaveControl" ShowDeleteButton="true" runat="server"></ecf:SaveControl>