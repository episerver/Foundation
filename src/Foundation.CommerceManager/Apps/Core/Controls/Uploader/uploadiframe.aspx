<%@ Page Language="C#" AutoEventWireup="true" ClassName="uploadiframe" %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
<%@ Register TagPrefix="mcf" Namespace="Mediachase.FileUploader.Web.UI" Assembly="Mediachase.FileUploader" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <link href="<%= CommerceHelper.GetAbsolutePath("~/Apps/Shell/Styles/css/FileUploaderStyle.css") %>" rel="stylesheet" type="text/css" />
</head>
<body class="FileUpload">
    <form id="uploadForm" method="post" runat="server" enctype="multipart/form-data">
    <p><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:CoreStrings, Uploader_Browse_Instructions %>"/>:</p>
    <mcf:McHtmlInputFile id="McFileUp" runat="server" Size="60" style="width:100%;" MultiFileUpload="false"></mcf:McHtmlInputFile>
    </form>
</body>
</html>