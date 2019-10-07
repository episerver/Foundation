<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddFramePopup.ascx.cs" Inherits="Mediachase.Ibn.Web.UI.Layout.Modules.AddFramePopup" %>
<%@ Register Src="~/Apps/Core/Controls/DynamicEditViewControl.ascx" TagName="EditViewControl" TagPrefix="ecf" %>
<asp:UpdatePanel runat="server" ID="panelAddBlock" ChildrenAsTriggers="true">
	<ContentTemplate>
		<div class="editDiv">
            <ecf:EditViewControl AppId="Core" ViewId="Layout-Edit" ID="ViewControl" runat="server"></ecf:EditViewControl>
            <div style="margin: 5px 5px 0px 5px; align:right; width:90%;">
                <custom:CustomImageButton runat="server" ID="btnDefault" style="float: left;"></custom:CustomImageButton>&nbsp;
                <custom:CustomImageButton runat="server" ID="btnSaveAndClose"></custom:CustomImageButton>&nbsp;
                <custom:CustomImageButton runat="server" ID="btnCancel" />
            </div>
            <br />
        </div>
	</ContentTemplate>
</asp:UpdatePanel>