<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Core.Controls.MetaToolbar" Codebehind="MetaToolbar.ascx.cs" %>
<%@ Register Namespace="Mediachase.BusinessFoundation" Assembly="Mediachase.BusinessFoundation" TagPrefix="mc" %>
<script type="text/javascript">
function Toolbar_GridHasItemsSelected(params)
{
    var cmdObj = null;
    var gridId = '';
    try
    {
        cmdObj = Sys.Serialization.JavaScriptSerializer.deserialize(params);
        gridId = cmdObj.CommandArguments.GridClientId;
    }
    catch(e)
    {
        alert('A problem occured with retrieving GridId');
        return;
    }
    
    var grid = null;

    try
    {
        grid = $find(gridId);
    }
    catch(e) {  }
    
    if(grid == null)
    {
        alert('Grid not found: ' + gridId);
        return false;
    }
    var csManagementClient = GetCSManagementClient();
    if (csManagementClient.ListHasItemsSelected2(grid))
    {
    	cmdObj.CommandArguments.GridSelectedItems = csManagementClient.GetSelectedGridItems(grid);
		params = Sys.Serialization.JavaScriptSerializer.serialize(cmdObj);
		return params;//CSManagementClient.ListHasItemsSelected2(grid);
	}
	else
	{
		return false;
	}
}

function Toolbar_GetSelectedGridItems(params)
{
    var cmdObj = null;
    var gridId = '';
    try
    {
        cmdObj = Sys.Serialization.JavaScriptSerializer.deserialize(params);
        gridId = cmdObj.CommandArguments.GridClientId;
    }
    catch(e)
    {
        alert('A problem occured with retrieving GridId');
        return;
    }
    
    var grid = null;
    try
    {
        grid = $find(gridId);
    }
    catch(e) {  }
    
    if(grid == null)
    {
        alert('Grid not found!');
        return false;
    }

    return GetCSManagementClient().GetSelectedGridItems(grid);
   }

   function GetCSManagementClient() {
   	var retVal = null;
   	if (typeof (CSManagementClient) == "undefined") {
   		var win = window;
   		while (win.parent != null && win.parent != win) {
   			if (typeof (win.CSManagementClient) != "undefined") {
   				retVal = win.CSManagementClient;
   			}
   			win = win.parent;
   		}
   	}
   	else {
   		retVal = CSManagementClient;
   	}
   	return retVal;
   };
</script>
<div id="toolbarContainer" style="height: 24px">
	<mc:JsToolbar runat="server" ID="MainToolbar" />
</div>