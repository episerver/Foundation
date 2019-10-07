<%@ Control Language="C#" AutoEventWireup="true" Inherits="Apps_Catalog_GridTemplates_NodeHyperlinkTemplate2" Codebehind="NodeHyperlinkTemplate2.ascx.cs" %>
<%@ Import Namespace="Mediachase.Web.Console.Common" %>
<%@ Import Namespace="Mediachase.Web.Console.Controls" %>
<asp:Image ID="Image1" runat="server" 
    ImageUrl='<%# DataBinder.Eval(DataItem, "[Type]", "~/Apps/Shell/styles/images/icons/{0}.gif")%>'
    Visible='<%# String.Compare((string)DataBinder.Eval(DataItem, "[Type]"), "LevelUp", true)!=0 %>' />
<asp:HyperLink ID="HyperLink1" runat="server" 
    NavigateUrl='<%# String.Format("javascript:CSCatalogClient.ViewItem(\"{0}\",\"{1}\");", DataBinder.Eval(DataItem,"[Type]"), DataBinder.Eval(DataBinder.GetDataItem(Container), "[ID]")) %>' 
    Text='<%#  GetCellValue(DataBinder.Eval(DataBinder.GetDataItem(Container),"[Name]")) %>' 
    Visible='<%# String.Compare((string)DataBinder.Eval(DataItem, "[Type]"), "LevelUp", true)!=0 && (((string)DataBinder.Eval(DataItem, "[Type]")).ToUpperInvariant()!="NODE") %>'>
</asp:HyperLink>
<asp:HyperLink ID="HyperLink2" runat="server" 
NavigateUrl='<%# String.Format("javascript:CSManagementClient.ChangeView(\"Catalog\", \"Node-List\",\"catalogid={0}&catalognodeid={1}&grandparentid={2}\");", ManagementHelper.GetIntFromQueryString("catalogid"), DataBinder.Eval(DataItem, "[ID]"), ManagementHelper.GetIntFromQueryString("catalognodeid")) %>' 
Text='<%# GetCellValue(DataBinder.Eval(DataItem, "[Name]")) %>' 
Visible='<%# (((string)DataBinder.Eval(DataItem, "[Type]")).ToUpperInvariant()=="NODE") %>'></asp:HyperLink>
<asp:HyperLink ID="HyperLink3" runat="server" 
NavigateUrl='<%# String.Format("javascript:CSManagementClient.ChangeView(\"Catalog\", \"Node-List\",\"catalogid={0}&catalognodeid={1}\");", ManagementHelper.GetIntFromQueryString("catalogid"), ManagementHelper.GetIntFromQueryString("grandparentid")) %>' 
Text='<%# GetCellValue(DataBinder.Eval(DataItem, "[Name]")) %>' 
Visible='<%# String.Compare((string)DataBinder.Eval(DataItem, "[Type]"), "LevelUp", true)==0 %>'></asp:HyperLink>