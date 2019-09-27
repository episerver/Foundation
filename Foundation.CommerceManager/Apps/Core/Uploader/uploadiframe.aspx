<%@ Page Language="C#" AutoEventWireup="true" ClassName="uploadiframe" %>
<%@ Register TagPrefix="mcf" Namespace="Mediachase.FileUploader.Web.UI" Assembly="Mediachase.FileUploader" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <style type="text/css">
      html, body {
        height:100%;
        margin:0;
        width: 100%;

        font-family: verdana,Arial,Helvetica,sans-serif;
        font-size: 11pt;
        color: black;
      }

      a {
        color: black;
        text-decoration: none;
      }
      a:hover {
        color: red;
        text-decoration: underline;
      }

      input {
          font-family: verdana,Arial,Helvetica,sans-serif;
          /*font-size: 8pt;*/
          color: black;
      }

      p {
        padding:5px 0px 5px 0px;
        margin: 0px;
      }
    </style>
</head>
<body>
    <form id="uploadForm" method="post" runat="server" enctype="multipart/form-data">
    <p><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:CoreStrings, Uploader_Browse_Instructions %>"/>:</p>
    <mcf:McHtmlInputFile id="McFileUp" runat="server" Size="60" style="width:100%;" MultiFileUpload="false"></mcf:McHtmlInputFile>
    </form>
</body>
</html>