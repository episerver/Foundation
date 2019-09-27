<%@ Page Language="C#" AutoEventWireup="true" ClassName="UploadProgress" %>
<%@ Register Assembly="Mediachase.FileUploader" Namespace="Mediachase.FileUploader.Web.UI" TagPrefix="mcf" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:CoreStrings, Uploader_File_Uploading_Progress %>"/></title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <style type="text/css">
      html, body {
        height:100%;
        margin:0;
        width: 100%;

        font-family: verdana,Arial,Helvetica,sans-serif;
        font-size: 8pt;
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
          font-size: 8pt;
          color: black;
      }

      p {
        padding:5px 0px 5px 0px;
        margin: 0px;
      }
    </style>
</head>
<body>
    <div style="padding: 10px;">
        <form id="ProgressBar" method="post" runat="server">
            <mcf:FileUploadProgress ID="fuProgress" runat="server" ProgressBarBoundaryStyle-BorderColor="#8080FF"
                ProgressBarBoundaryStyle-BorderStyle="Solid" ProgressBarBoundaryStyle-BorderWidth="1px"
                ProgressBarStyle-BackColor="MediumBlue" ProgressBarStyle-Height="15px" ScriptPath=""
                ProgressBarBoundaryStyle-Height="17px" ProgressBarBoundaryStyle-Width="250px">
                <InfoTemplate>
                    <%# DataBinder.Eval(Container, "UploadBytesReceived")%>
                    &nbsp;<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:SharedStrings, From_Lowercase %>"/>&nbsp;<%# DataBinder.Eval(Container, "UploadBytesTotal")%><br /><br />
                    <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:SharedStrings, Estimated %>"/>:&nbsp;<%# DataBinder.Eval(Container, "UploadEstimatedTime")%><br />
                    <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:SharedStrings, Remaining %>"/>:&nbsp;<%# DataBinder.Eval(Container, "UploadTimeRemaining")%>
                </InfoTemplate>
            </mcf:FileUploadProgress>
        </form>
    </div>
</body>
</html>
