<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Core.Controls.DialogControl" Codebehind="DialogControl.ascx.cs" %>

<ComponentArt:Dialog ID="DialogCtrl" ModalMaskImage="~/Apps/Shell/styles/images/dialog/alpha.png"
    AnimationDuration="600" HeaderCssClass="headerCss" Icon="pencil.gif" Value="<%$ Resources:CoreStrings, DialogControl_Select_Line_Items %>"
    HeaderClientTemplateId="header" Title="Line Item Edit" FooterClientTemplateId="footer"
    AllowDrag="true" ZIndex="100" Alignment="MiddleCentre" runat="server" MinimumHeight="400" MinimumWidth="458">
    <ClientTemplates>
        <ComponentArt:ClientTemplate ID="header">
            <table style="filter: alpha(opacity=60);" cellpadding="0" cellspacing="0" border="0" height="35" onmousedown="## Parent.get_id() ##.StartDrag(event);">
                <tr>
                    <td width="9" height="35">
                        <img style="display: block;" src="../../../Apps/Shell/Styles/images/dialog/top-left.png" />
                    </td>
                    <td height="35" style="background-image: url(../../../Apps/Shell/Styles/images/dialog/top-mid.png);
                        height: 35px !important; width: 100%" valign="middle">
                        <span style="color: White; font-size: 15px; font-family: Arial; font-weight: bold;">
                            ## Parent.Title ##</span>
                    </td>
<%--                    <td width="40" height="35" valign="top" style="background-image: url(../../../Apps/Shell/Styles/images/dialog/top-right.png);">
                        <img src="../../../Apps/Shell/Styles/images/dialog/close.png" style="cursor: default; padding-top: 4px; margin-top: 0px; padding-left: 0px; padding-right: 0px; margin-right: 8px;"
                            width="32" height="25" onmousedown="this.src='../../../Apps/Shell/Styles/images/dialog/close-down.png';"
                            onmouseup="this.src='../../../Apps/Shell/Styles/images/dialog/close-hover.png';" onclick="## Parent.get_id() ##.Close('Close click');"
                            onmouseover="this.src='../../../Apps/Shell/Styles/images/dialog/close-hover.png';" onmouseout="this.src='../../../Apps/Shell/Styles/images/dialog/close.png';"/>
                    </td>--%>
                </tr>
            </table>
        </ComponentArt:ClientTemplate>
        <ComponentArt:ClientTemplate ID="footer">
            <table cellpadding="0" cellspacing="0" width="100%" height="7" style="filter: alpha(opacity=60);">
                <tr>
                    <td width="9" height="7">
                        <img style="display: block;" src="../../../Apps/Shell/Styles/images/dialog/bottom-left.png" /></td>
                    <td style="background-image: url(../../../Apps/Shell/Styles/images/dialog/bottom-mid.png);"
                        width="100%">
                    </td>
                    <td width="9" height="7">
                        <img style="display: block;" src="../../../Apps/Shell/Styles/images/dialog/bottom-right.png" /></td>
                </tr>
            </table>
        </ComponentArt:ClientTemplate>
    </ClientTemplates>
    <Content>
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="background-image: url('<%= this.ResolveUrl("~/Apps/Shell/Styles/images/dialog/left.png") %>'); filter: alpha(opacity=60);"
                    width="7">
                </td>
                <td style="background-color: white; font-size: 12px; font-family: Arial; overflow: auto;">
                    <!-- Content Area -->
                    <asp:Panel runat="server" ID="DialogContentPanel"></asp:Panel>
                    <!-- /Content Area -->
                </td>
                <td style="background-image: url('<%= this.ResolveUrl("~/Apps/Shell/Styles/images/dialog/right.png") %>'); filter: alpha(opacity=60);" width="7">
                </td>
            </tr>
        </table>
    </Content>
</ComponentArt:Dialog>