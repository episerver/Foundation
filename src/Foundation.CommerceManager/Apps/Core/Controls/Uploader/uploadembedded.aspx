<%@ Page Language="C#" AutoEventWireup="true" ClassName="uploadembedded" %>
<%@ Register TagPrefix="mc" Assembly="Mediachase.FileUploader" Namespace="Mediachase.FileUploader.Web.UI" %>
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
    <form id="form1" runat="server">
    <div style="padding:10px;">
    <mc:FileUploadProgress ID="fuProgress" runat="server">
      <InfoTemplate>
        <%# DataBinder.Eval(Container, "UploadFileName")%><br/>
        <%# DataBinder.Eval(Container, "UploadBytesReceived")%>&nbsp;<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:SharedStrings, From_Lowercase %>"/>&nbsp;<%# DataBinder.Eval(Container, "UploadBytesTotal")%><br/>
        <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:SharedStrings, Estimated %>"/>:&nbsp;<%# DataBinder.Eval(Container, "UploadEstimatedTime")%><br/>
        <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:SharedStrings, Remaining %>"/>:&nbsp;<%# DataBinder.Eval(Container, "UploadTimeRemaining")%>
      </InfoTemplate>
    </mc:FileUploadProgress>
    </div>
    </form>
</body>
</html>
