<%@ Page Language="C#" AutoEventWireup="true" ClassName="uploadpopup" %>
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
      div.fu-toolbar {
         background-image: url('images/toolgrad.gif');
         background-repeat: repeat-x;
         border: 1px solid #95b7f3;
         background-repeat: repeat-x;
         background-color: #9ebff6;
         padding:2px;
         text-align:right;
      }
      .fu-toolbar {
	      font-family: verdana; 
          font-size: 8pt; 
          text-decoration: none; 
          color: #003399; 
}
    </style>
    <script type="text/javascript">
       function StartUpload()
      {
        if (location.search)
				{
				  var srchArray = getSearchData();
					if (srchArray["obj_id"])
					{
					  var obj_id = srchArray["obj_id"];
					  window.opener.MCFU_Array[obj_id].Upload();
					}
				}
      }
      function getSearchData()
			{
				var results = new Object();
				if (location.search.substr)
				{
					var input = location.search.substr(1);
					if (input)
					{
						var srchArray = input.split("&");
						var tempArray = new Array();
						for (var i = 0; i < srchArray.length; i++)
						{
							tempArray = srchArray[i].split("=");
							results[tempArray[0]] = unescape(tempArray[1]);
						}
					}
				}
				return results;
			}
    </script>
</head>
<body>
    <form id="uploadForm" method="post" runat="server" enctype="multipart/form-data">
    <div class="fu-toolbar">
    <a class="fu-toolbar" href="javascript:StartUpload();"><img align="absmiddle" style="border:0;" width="16" alt="" src="images/upload.gif" />&nbsp;Upload</a>&nbsp;
    </div>
    <div style="padding:10px;">
    <p><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:CoreStrings, Uploader_Browse_Instructions %>"/>:</p>
    <mcf:McHtmlInputFile id="McFileUp" runat="server" Size="60" style="width:60%;" MultiFileUpload="false"></mcf:McHtmlInputFile>
    </div>
    </form>
</body>
</html>