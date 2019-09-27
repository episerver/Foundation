<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IndexTab.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Core.Search.Tabs.IndexTab" %>
<%@ Register src="~/Apps/Core/Controls/ProgressControl.ascx" tagname="ProgressControl" tagprefix="core" %>
<div id="DataForm">
    <table runat="server">
        <tr>
            <td colspan="2" class="FormSectionCell">
                <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:CoreStrings, Search_Settings %>" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormLabelCell">
                <asp:Literal runat="server" Text="<%$ Resources:CoreStrings, Search_Default_Provider %>" />
                <b><asp:Literal ID="litDefaultSearchProvider" runat="server" Text="" /></b>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormLabelCell">
                <asp:Literal runat="server" Text="<%$ Resources:CoreStrings, Search_Installed_Providers %>" />
                <asp:Literal ID="litSearchProviders" runat="server" Text="" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormLabelCell">
                <asp:Literal runat="server" Text="<%$ Resources:CoreStrings, Search_Indexers %>" />
                <asp:Literal ID="litIndexers" runat="server" Text="" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSectionCell">
                <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:CoreStrings, Search_Operations %>" />
            </td>
        </tr>
        <tr id="trChangeNotificationProcessorDisableInstructions" runat="server">
            <td class="FormLabelCell" colspan="2">
                <asp:Literal ID="Literal5" runat="server" Text="Invalidate the indexing processor to enable the index build/rebuild buttons." />
            </td>
        </tr>        
        <tr id="trRebuildIndexButton" runat="server">
            <td class="FormLabelCell">
                <asp:UpdatePanel ID="MainPanel" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button runat="server" ID="RebuildIndexButton" OnClick="RebuildIndex" Text="<%$ Resources:CoreStrings, Search_Rebuild_Index %>" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td class="FormLabelCell">
                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:CoreStrings, Search_Rebuild_Instructions %>"/>
            </td>
        </tr>
        <tr id="trIndexButtonsSpacer" runat="server">
            <td colspan="2" class="FormSpacerCell">
            </td>
        </tr>        
        <tr id="trBuildIndexButton" runat="server">
            <td class="FormLabelCell">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button runat="server" ID="BuildIndexButton" OnClick="BuildIndex" Text="<%$ Resources:CoreStrings, Search_Build_Index %>" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td class="FormLabelCell">
                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:CoreStrings, Search_Build_Instructions %>"/>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell">
            </td>
        </tr>
        <tr id="trIndexingProcessorStateLabel" runat="server">
            <td colspan="2" class="FormSectionCell">
                <asp:Literal runat="server" Text="Indexing Processor State" />
            </td>
        </tr>
        <tr id="trChangeNotificationProcessorRow" runat="server">
            <td class="FormLabelCell">
                <asp:Button runat="server" Text="Recover" OnClick="TryStartRecovery" />
                <asp:Button runat="server" Text="Invalidate" OnClick="SetInvalid" />
            </td>
            <td class="FormLabelCell">
                <asp:Literal ID="litIndexingProcessorState" runat="server" Text="" />
            </td>
        </tr>        
        <tr>
            <td colspan="2" class="FormLabelCell">
                <core:ProgressControl Title="Indexing Data" ID="ProgressControl1" runat="server" DoPostBack="true" />
            </td>
        </tr>
        <tr runat="server" id="trGeneratedConfigLabel" visible="false">
            <td colspan="2" class="FormLabelCell">
                <asp:Literal runat="server" Text="<%$ Resources:CoreStrings, Search_Generated_Configuration %>" />
            </td>
        </tr>
        <tr runat="server" id="trGeneratedConfigContent" visible="false">
            <td colspan="2" class="FormLabelCell">
                <textarea runat="server" id="txtGeneratedConfigContent" rows="20" readonly="readonly" wrap="off" style="width:98%;" />
            </td>
        </tr>
    </table>
</div>


