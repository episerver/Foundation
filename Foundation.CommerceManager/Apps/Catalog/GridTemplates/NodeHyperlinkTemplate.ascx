<%@ Control Language="C#" AutoEventWireup="true" Inherits="Apps_Catalog_GridTemplates_NodeHyperlinkTemplate" Codebehind="NodeHyperlinkTemplate.ascx.cs" %>
<%@ Import Namespace="Mediachase.Web.Console.Controls" %>
<asp:Image ID="Image2" runat="server" ImageUrl='<%# DataBinder.Eval(DataItem, "ClassTypeId", "~/Apps/Shell/styles/images/icons/{0}.gif")%>' />
<asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%# String.Format("javascript:CSCatalogClient.OpenItem(\"{0}\",\"{1}\", \"searchMode=1\");", DataBinder.Eval(DataItem,"ClassTypeId"), DataBinder.Eval(DataItem, "CatalogEntryId")) %>' Text='<%# HttpUtility.HtmlAttributeEncode((string)DataBinder.Eval(DataItem,"Name")) %>'>
</asp:HyperLink>
