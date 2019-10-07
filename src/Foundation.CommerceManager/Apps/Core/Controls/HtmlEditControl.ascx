<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Core.Controls.HtmlEditControl" Codebehind="HtmlEditControl.ascx.cs" %>
<asp:PlaceHolder runat="server" ID="EditorControl">
    <asp:CustomValidator id="CustomValidator1" runat="server" ErrorMessage="*" Display="Dynamic" OnClientValidate="ContentValid"></asp:CustomValidator>	
</asp:PlaceHolder>