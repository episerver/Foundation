<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WarehouseEdit.ascx.cs" Inherits="Mediachase.Commerce.Manager.Catalog.WarehouseEdit" %>
<%@ Register Src="~/Apps/Core/SaveControl.ascx" TagName="SaveControl" TagPrefix="ecf" %>
<%@ Register Src="~/Apps/Core/Controls/EditViewControl.ascx" TagName="EditViewControl" TagPrefix="ecf" %>
<div class="editDiv">
<script type="text/javascript">


function extendToolkitHidePopupTimer()
{
	if (AjaxControlToolkit && AjaxControlToolkit.PopupBehavior)
	{
		extendToolkitHidePopup();
	}
	else
	{
		window.setTimeout(extendToolkitHidePopup, 500);
	}
}

function extendToolkitHidePopup()
{
	AjaxControlToolkit.PopupBehavior.prototype.hide = function()
	{
		var eventArgs = new Sys.CancelEventArgs();
		this.raiseHiding(eventArgs);
		if (eventArgs.get_cancel()) {
			return;
		}

		// Either hide the popup or play an animation that does
		this._visible = false;
		if (this._onHide) {
			this.onHide();
		} else {
			this._hidePopup();
			this._hideCleanup();
		}
	}
}

extendToolkitHidePopupTimer();

</script>
<ecf:EditViewControl AppId="Catalog" ViewId="Warehouse-Edit" id="ViewControl" runat="server"></ecf:EditViewControl>
<ecf:SaveControl id="EditSaveControl" CancelClientScript="CSManagementClient.ChangeView('Catalog','Warehouse-List');" SavedClientScript="CSManagementClient.ChangeView('Catalog', 'Warehouse-List');" runat="server"></ecf:SaveControl>
</div>