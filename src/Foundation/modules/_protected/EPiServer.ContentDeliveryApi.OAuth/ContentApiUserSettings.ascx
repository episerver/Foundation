<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ContentApiUserSettings.ascx.cs" Inherits="EPiServer.ContentApi.OAuth.UI.ContentApiUserSettings" %>

<div class="epi-padding">
    <div class="epi-formArea">
        <div class="epi-size10">
            <h2><asp:Literal runat="server" Text="<%$ Resources: EPiServer, admin.edituser.contentapisettings.refreshtokens.name %>"></asp:Literal></h2>
            <p><asp:Literal runat="server" Text="<%$ Resources: EPiServer, admin.edituser.contentapisettings.refreshtokens.description %>"></asp:Literal></p>
            <asp:Repeater Id="rptRefreshTokens" ItemType="EPiServer.ContentApi.OAuth.IRefreshToken" OnItemCommand="rptRefreshTokens_OnItemCommand" runat="server">
                <HeaderTemplate>
                    <table class="epi-default">
                    <tr>
                        <th scope="col"><asp:Literal runat="server" Text="<%$ Resources: EPiServer, admin.edituser.contentapisettings.refreshtokens.fields.clientid %>" /></th>
                        <th scope="col"><asp:Literal runat="server" Text="<%$ Resources: EPiServer, admin.edituser.contentapisettings.refreshtokens.fields.issued %>" /></th>
                        <th scope="col"><asp:Literal runat="server" Text="<%$ Resources: EPiServer, admin.edituser.contentapisettings.refreshtokens.fields.expires %>" /></th>
                        <th scope="col"></th>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tbody>
                        <tr>
                            <td><%#:Item.ClientId %></td>
                            <td><%#:Item.IssuedUtc.ToLocalTime() %></td>
                            <td><%#:Item.ExpiresUtc.ToLocalTime() %></td>
                            <td><asp:LinkButton runat="server" Text="<%$ Resources: EPiServer, admin.edituser.contentapisettings.refreshtokens.actions.revoke %>" CommandName="DeleteToken" CommandArgument="<%#:Item.Guid %>"/></td>
                        </tr>
                    </tbody>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                    <asp:Label ID="defaultItem" runat="server" 
                               Visible='<%# rptRefreshTokens.Items.Count == 0 %>' Text="<%$ Resources: EPiServer, admin.edituser.contentapisettings.refreshtokens.messages.notokens %>" />
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>
