<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Core.MetaData.MetaControls.FileControl" EnableViewState="true" Codebehind="FileControl.ascx.cs" %>
<%@ Register TagPrefix="mc" Namespace="Mediachase.FileUploader.Web.UI" Assembly="Mediachase.FileUploader" %>

<asp:UpdatePanel runat="server" ID="upFile" ChildrenAsTriggers="false" UpdateMode="Conditional">
<ContentTemplate>
<tr>
    <td class="FormLabelCell"><asp:Label id="MetaLabelCtrl" runat="server"></asp:Label>:</td>
    <td class="FormFieldCell">
        <asp:Table ID="tblUpload" runat="server">
            <asp:TableRow>
                <asp:TableCell>
                    <mc:FileUploadActions ID="fuActions" runat="server" FileUploadControlID="FileUpMetaValueCtrl" 
                        CancelActionText="Cancel uploading" DisplayCancel="True" DisplayHide="True" DisplayShow="True" DisplayUpload="True" HideActionText="Hide" ScriptPath="" ShowActionText="Show" UploadActionText="Upload">
                        <ShowTextTemplate><div class="FileUploadAddBtnBlock"><img src="../../../Apps/Shell/Styles/images/Uploader/document_add.png" width="16" height="16" border="0" title="Add" align="middle" />&nbsp;<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:SharedStrings, Add_New_File%>" /></div></ShowTextTemplate>
                        <HideTextTemplate><img src="../../../Apps/Shell/Styles/images/Uploader/scrollup_hover.gif" width="16" height="16" border="0" title="Hide" /><span class="FileUploadHideBlock">&nbsp;<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:SharedStrings, Hide_Panel%>" /></span></HideTextTemplate>
                        <UploadTextTemplate><img src="../../../Apps/Shell/Styles/images/Uploader/publish.gif" width="16" height="16" border="0" title="Upload" /><span class="FileUploadUploadBlock">&nbsp;<asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:SharedStrings, Upload_Files%>" /></span></UploadTextTemplate>
                    </mc:FileUploadActions>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="trUploadedFiles" runat="server">
                <asp:TableCell CssClass="FileUploadFilesForSaving">
                    <table border="0" cellpadding="5" cellspacing="0" width="100%" >
                        <tr>
                            <td>
	                            <b><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:SharedStrings, File_For_Saving%>" /></b>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" visible="false">
                                <mc:UploadedFileList ID="fvControl" runat="server" FileUploadControlID="FileUpMetaValueCtrl" ClientBinderProvider="XML"  >
                                    <DeleteCommandTemplate><img src="../../../Apps/Shell/Styles/images/Uploader/delete2.png" width="16px" height="16px" border="0" title="Delete" align="absmiddle" /></DeleteCommandTemplate>
                                </mc:UploadedFileList>
                            </td>
                        </tr>
                    </table>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell Width="500px">
                    <asp:HiddenField runat="server" ID="hfFileUpMetaValueCtrlSessionUID" />
                    <mc:FileUploadControl ID="FileUpMetaValueCtrl" runat="server" ModeType="IFrame" BlockHeight="50px" Height="45px">
                    </mc:FileUploadControl>
                    <mc:FileUploadProgress ID="fuProgress" runat="server" FileUploadControlID="FileUpMetaValueCtrl" 
                        ProgressBarBoundaryStyle-BorderColor="DarkGoldenrod" ProgressBarBoundaryStyle-BorderStyle="Solid" 
                        ProgressBarBoundaryStyle-BorderWidth="1px" ProgressBarBoundaryStyle-Height="17px" 
                        ProgressBarBoundaryStyle-Width="250px" ProgressBarStyle-BackColor="Lime" 
                        ProgressBarStyle-Height="15px" ScriptPath="">
                      <WaitTemplate><i><asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:SharedStrings, Upload_Instructions%>" /></i></WaitTemplate>
                        <InfoTemplate>
                            <em><asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:SharedStrings, File_Uploading%>" /></em>
                        </InfoTemplate>
                    </mc:FileUploadProgress>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:HyperLink ID="FileUrl" Runat="server" Font-Bold="true"></asp:HyperLink>&nbsp;&nbsp;&nbsp;
        <asp:CheckBox Visible="false" runat="server" ID="RemoveFile" Text="<%$ Resources:SharedStrings, Delete_File %>" />
        <br />
        <asp:Label id="MetaDescriptionCtrl" runat="server" CssClass="FormFieldDescription"></asp:Label>
    </td>
</tr>
</ContentTemplate>
</asp:UpdatePanel>