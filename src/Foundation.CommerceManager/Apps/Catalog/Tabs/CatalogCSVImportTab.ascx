<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Catalog.Tabs.CatalogCSVImportTab" Codebehind="CatalogCSVImportTab.ascx.cs" %>
<%@ Register Assembly="Mediachase.FileUploader" Namespace="Mediachase.FileUploader.Web.UI" TagPrefix="mc" %>
<%@ Register Src="~/Apps/Core/Controls/FileListControl.ascx" TagName="FileList" TagPrefix="core" %>
<%@ Register src="~/Apps/Core/Controls/ProgressControl.ascx" tagname="ProgressControl" tagprefix="core" %>

<%--<script type="text/javascript" src="<%= ResolveUrl ("~/Apps/Catalog/Scripts/DragAndDropUpload.js")%>"></script>--%>

<script type="text/javascript">
    function StateChanged(fuList, fuActionsObj, uploadedFilesRowObj) 
	{ 
		if(!MCFU_Array || !MCFU_Array[fuList]) 
		{ 
			setTimeout('StateChanged(\"'+fuList+'\", \"'+fuActionsObj+'\", \"'+uploadedFilesRowObj+'\");', 50); 
			return;
        }
        var uploadedFile = $.trim($('#' + fuList).text());
        if (uploadedFile != null && uploadedFile != '') {
            $("#<%=FilesPanel.ClientID %> tr td div").each(function () {
                if ($(this).text() == uploadedFile) {
                    $('#<%=lbSaveFiles.ClientID %>').text('<asp:Literal ID="litButtonTextOverwrite" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Overwrite_File %>" />');
                    return false;
                }
                $('#<%=lbSaveFiles.ClientID %>').text('<asp:Literal ID="litButtonTextSave" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Save_File %>" />');
            });
        }
        
		var fa = MCFU_Array[fuActionsObj];
		var uc = MCFU_Array[fuList]; 
		if(uc!=null) 
		{ 
			var h = $get(uploadedFilesRowObj); 
			if(h!=null) 
			{ 
				if(uc.FilesCount>0) 
					{ 
						h.style.display = "block"; 
						h.cells[0].style.width = h.offsetWidth+"px"; 
						if(fa != null) {
							fa.mainDiv.style.display = "none";
						}
    				} 
				else {
					h.style.display = "none"; 
					if(fa != null) {
					   fa.mainDiv.style.display = "block";
						}
				}
			} 
		} 
	}

</script>
<style type="text/css">
    .uploadBox
{
    height: 150px;
    width: 940px;
    float: left;
    border: 2px solid #666666;
    background-color: #F1F1F1;
    padding:3px;
    margin:5px;
    text-align: center;
    overflow: auto;
}
</style>
<script type="text/javascript">
    //The query string "dndUpload=true" is essential to bypass the Mediachase File Upload HTTP Module.
    var uploadHandler = '<%= Page.ResolveUrl("~/Apps/Core/Uploader/DnDUploadHandler.ashx?dndUpload=true&datatype=catalog") %>';
    var initialized = false;

    function CheckDragAndDropPanel() {      
        //Test for HTML5 capability
        if ('draggable' in document.createElement('span')) {
            //HTML5 capability available - Hide traditional file upload and initialize the file drop zone.
            document.getElementById('<%= tblUpload.ClientID %>').style.display = "none";
            if (initialized) {
                return;
            }
            initialized = true;
            DragAndDropUploadInit(document.getElementById('DragMappingUploadBox'), uploadHandler);
            DragAndDropUploadInit(document.getElementById('DragCSVUploadBox'), uploadHandler);
        }
        else {
            //HTML5 capability unavialable - Hide the file drop zone.
            document.getElementById('<%= CSVFiles.ClientID %>').style.display = "none";
            document.getElementById('<%= XMLFiles.ClientID %>').style.display = "none";

            document.getElementById('DragCSVUploadBox').style.display = "none";
            document.getElementById('DragMappingUploadBox').style.display = "none";
        }
    }

    $(document).ready(function () {
        CheckDragAndDropPanel();      
    });

    function childPageLoad(sender, args) {
        CheckDragAndDropPanel();
    }

</script>

<div class="wh100" style="padding-left:10px; padding-right:5px; padding-top:10px; padding-bottom:5px;">
    <asp:Label runat="server" ID="lblWarning" Font-Bold="true" Text="<%$ Resources:CatalogStrings, Catalog_Import_Warning %>"></asp:Label>
    <br /><br />
   <asp:Label runat="server" ID="lblImportDescription" Text="<%$ Resources:CatalogStrings, Catalog_Import_Instructions %>"></asp:Label><br /><br />
    <!-- START: Upload block -->
    <asp:Table ID="tblUpload" runat="server" CssClass="FileUpload" Width="100%">
        <asp:TableRow>
            <asp:TableCell>
                <mc:FileUploadActions ID="fuActions" runat="server" FileUploadControlID="FileUpCtrl" 
                    CancelActionText="<%$ Resources:SharedStrings, Cancel_Uploading %>" DisplayCancel="True" DisplayHide="True" DisplayShow="True" DisplayUpload="True" HideActionText="<%$ Resources:SharedStrings, Hide %>" ScriptPath="" ShowActionText="<%$ Resources:SharedStrings, Show %>" UploadActionText="<%$ Resources:SharedStrings, Upload %>">
                    <ShowTextTemplate><div class="FileUploadAddBtnBlock"><img src='<%= Page.ResolveUrl("~/Apps/Shell/styles/Images/Uploader/document_add.png") %>' width="16" height="16" border="0" title="Add" align="middle" />&nbsp;<asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Add_New_File %>" /></div></ShowTextTemplate>
                    <HideTextTemplate><img src='<%= Page.ResolveUrl("~/Apps/Shell/styles/Images/Uploader/scrollup_hover.gif") %>' width="16" height="16" border="0" title="Hide" /><span class="FileUploadHideBlock">&nbsp;<asp:Literal ID="Literal9" runat="server" Text="<%$ Resources:SharedStrings, Hide_Panel %>" /></span></HideTextTemplate>
                    <UploadTextTemplate><img src='<%= Page.ResolveUrl("~/Apps/Shell/styles/Images/Uploader/publish.gif") %>' width="16" height="16" border="0" title="Upload" /><span class="FileUploadUploadBlock">&nbsp;<asp:Literal ID="Literal10" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Upload_File %>" /></span></UploadTextTemplate>
                </mc:FileUploadActions>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="trUploadedFiles" runat="server">
            <asp:TableCell CssClass="FileUploadFilesForSaving">
                <table border="0" cellpadding="5" cellspacing="0" width="100%" >
                    <tr>
                        <td style="width: 311px">
                            <b><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Uploaded_File %>" />:</b>
                        </td>
                    </tr>
                    <tr visible="false" align="left">
                        <td height="45px" width="100%">
                            <mc:UploadedFileList ID="fvControl" runat="server" FileUploadControlID="FileUpCtrl" ClientBinderProvider="XML"  >
                                <DeleteCommandTemplate><img src='<%= Page.ResolveUrl("~/Apps/Shell/styles/Images/Uploader/delete2.png") %>' width="16px" height="16px" border="0" title="Delete" align="absmiddle" /></DeleteCommandTemplate>
                            </mc:UploadedFileList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div align="left">
		                                <img src='<%= Page.ResolveUrl("~/Apps/Shell/styles/Images/Uploader/disk_blue_ok.png") %>' width="16px" height="16px" align="absmiddle" />
			                            <asp:LinkButton runat="server" ID="lbSaveFiles" Text="<%$ Resources:CatalogStrings, Catalog_Save_File %>" CssClass="text"></asp:LinkButton>
		                            </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Width="100%">
                <mc:FileUploadControl ID="FileUpCtrl" runat="server" ModeType="IFrame" BlockHeight="100px" AllowedFileTypes="csv,zip">
                </mc:FileUploadControl>
                <mc:FileUploadProgress ID="fuProgress" runat="server" FileUploadControlID="FileUpCtrl" 
                    ProgressBarBoundaryStyle-BorderColor="DarkGoldenrod" ProgressBarBoundaryStyle-BorderStyle="Solid" 
                    ProgressBarBoundaryStyle-BorderWidth="1px" ProgressBarBoundaryStyle-Height="17px" 
                    ProgressBarBoundaryStyle-Width="250px" ProgressBarStyle-BackColor="Lime" 
                    ProgressBarStyle-Height="15px" ScriptPath="">
                  <WaitTemplate><i><asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Upload_Wait %>" /></i></WaitTemplate>
                    <InfoTemplate>
                        <em><asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Upload_Info %>" /></em>
                    </InfoTemplate>
                </mc:FileUploadProgress>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <!-- END: Upload block -->
    <!-- START: Upload result block -->
    <asp:UpdatePanel ID="UploadResultPanel" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label runat="server" ID="lblUploadResult"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- END: Upload result block -->
    <br />
    <!-- START: Files list block -->
    <div style="padding-left:5px; padding-right:5px; padding-top:5px;">
        <b><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Import_Files_Available %>" />:</b>
        <br />
        <table>
            <tr>
                <td align="left">
                    <asp:UpdatePanel ID="FilesPanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <core:FileList ID="FilesControl" runat="server" GridAppId="Catalog" GridViewId="FilesList-CSVImport" KeyFieldIndex="1" /> 
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <b><asp:Label ID="CSVFiles" runat="server" Text="<%$ Resources:CatalogStrings, Upload_CSV_Files %>" /></b>
                    <br />
                    <div id="DragCSVUploadBox" class="uploadBox"></div>
                </td>
            </tr>
            <tr>
                <td>
                    <b><asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Choose_Mapping_File %>" /></b>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:FileUpload ID="File_Upload" runat="server" Style="display: none" />
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:UpdatePanel ID="MappingFilesPanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <core:FileList ID="MappingFilesControl" runat="server" Folder='<%# Mediachase.Web.Console.Common.ManagementHelper.GetImportExportFolderPath("csv/rule") %>' GridAppId="Catalog" GridViewId="FilesList-CSVImport" KeyFieldIndex="1" /> 
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <b><asp:Label ID="XMLFiles" runat="server" Text="<%$ Resources:CatalogStrings, Upload_XML_Files %>" /></b>
                    <br />
                    <div id="DragMappingUploadBox" class="uploadBox"></div>
                </td>
            </tr>
        </table>
    </div>
    <!-- END: Files list block -->
    <table>
      <tr>
          <td colspan="2" class="FormSectionCell">
              <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Choose_Catalog %>" />
          </td>
      </tr>                      
      <tr>
        <td class="FormLabelCell">
            <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:SharedStrings, Catalog %>" />:
        </td>
        <td class="FormFieldCell">  
            <asp:DropDownList runat="server" ID="ListCatalogs" Width="200" DataValueField="CatalogId" DataTextField="Name"/>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ListCatalogs" ValidationGroup="Import" Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
        </td>
      </tr>
    </table>
    <br />
    
    <!-- START: Import Button -->
    <asp:UpdatePanel ID="MainPanel" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button runat="server" ID="btnImport" Text="<%$ Resources:SharedStrings, Start_Import %>" Enabled="true" ValidationGroup="Import" Width="100px" Height="30px"/>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- END: Import Button -->
    <core:ProgressControl Title="Importing catalog" ID="ProgressControl1" runat="server" />
</div>
