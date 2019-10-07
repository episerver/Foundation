<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Core.StoreLogs.ApplicationLog" Codebehind="ApplicationLog.ascx.cs" %>
<%@ Register Src="~/Apps/Core/Controls/EcfListViewControl.ascx" TagName="EcfListViewControl" TagPrefix="core" %>
<%@ Register Src="~/Apps/Core/Controls/CalendarDatePicker.ascx" TagName="CalendarDatePicker" TagPrefix="ecf" %>
<IbnWebControls:McDock ID="DockTop" runat="server" Anchor="Top" EnableSplitter="False"
    DefaultSize="91">
    <DockItems>
        <asp:Panel runat="server" ID="pnlMain" DefaultButton="" Height="90px" BackColor="#F8F8F8">
            <div id="DataForm">
                <table width="100%" border="0">
                  <col width="60%" />
                  <col width="40%" />
                  <tr>
                    <td>
                      <table cellpadding="4" style="background-color: #F8F8F8;" width="100%" border="0">
                        <tr>
                            <td colspan="5" class="FormSectionCell">
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:SharedStrings, Filter_Settings %>" />
                            </td>
                        </tr>                      
                         <tr>
                            <td class="FormLabelCell">
                                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:SharedStrings, Source_Type %>" />:
                            </td>
                            <td class="FormFieldCell">
                              <asp:TextBox runat="server" ID="SourceKey" Width="200"/>
                            </td>
                            <td></td>
                            <td class="FormLabelCell">
                                <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:SharedStrings, Object_Type %>" />:
                            </td>
                            <td class="FormFieldCell">
                              <asp:TextBox runat="server" ID="ObjectType" Width="200"/>
                            </td>
                          </tr>
                          <tr>
                            <td class="FormLabelCell">
                                <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:SharedStrings, Operation %>" />:
                            </td>
                            <td class="FormFieldCell">
                              <asp:TextBox runat="server" ID="Operation" Width="200"/>
                            </td>
                            <td></td>
                            <td class="FormLabelCell">
                                <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:SharedStrings, Created_Before %>" />:
                            </td>
                            <td class="FormFieldCell">
                                <ecf:CalendarDatePicker runat="server" ID="Created" />
<%--                                <asp:TextBox runat="server" ID="Created" Width="200" />
                                <asp:RangeValidator ID="RngAge" runat="server" ControlToValidate="Created" Display="Dynamic" ErrorMessage="" MinimumValue="1900/1/1" MaximumValue = "2100/1/1" Type="Date">*</asp:RangeValidator>
--%>                            </td>
                          </tr>
                      </table>
                  </td>
                    <td valign="bottom">
                    <table border="0" cellspacing="5" style="background-color: #F8F8F8;" width="100%">
                      <tr>
                        <td valign="bottom">
                          <asp:UpdatePanel ID="upSearchButton" ChildrenAsTriggers="true" runat="server" UpdateMode="Conditional">
                              <ContentTemplate>
                                  <asp:Button ID="btnSearch" runat="server" Width="90" Text="<%$ Resources:SharedStrings, Apply_Filter %>" />
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
<core:EcfListViewControl id="MyListView1" DataSourceID="ApplicationLogDataSource1" runat="server" AppId="Core" ViewId="ApplicationLog" ShowTopToolbar="true"></core:EcfListViewControl>
<core:ApplicationLogDataSource runat="server" ID="ApplicationLogDataSource1" DataMode="ApplicationLog">
</core:ApplicationLogDataSource>