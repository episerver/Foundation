function ManagementClient(contentFrame, centerCmpId) {
	// Constants
	this.EcfListView_PrimaryKeySeparator = '';

	// Properties
	this.BASE_URL = ''; // CommerceManager root; shoud be initialized in the code
	this.HELP_URL = '';
	this.PageTitleControlId = '';
	this.ContentFrame = contentFrame;
	this.CenterCmpId = centerCmpId;
	this.CurrentView = null;
	this.Views = null;
	this.MenuCount = 0;
	this.ShowSaveStatus = false;
	this.IsPageDirty = false; // we should display warning if page is dirty when navigating away

	// Method Mappings    
	this.SetPageTitle = MC_SetPageTitle; // changes text in the page's up panel
	this.ChangeView = MC_ChangeView; // Change the content view, pass the relative control as a parameter
	this.ChangeBafView = MC_ChangeBafView; // Change the content view fot metaclass, pass the relative control as a parameter
	this.OpenTran = MC_ChangeTransition; // Change the content view, pass the relative control as a parameter
	this.ChangeMenuView2 = MC_ChangeMenuView2; // Change the menu view
	this.RegisterMenuView = MC_RegisterMenuView;
	this.CloseTab = MC_CloseTab;
	this.RefreshMenuView = MC_RefreshMenuView; // Refreshes the menu view
	this.Initialize = MC_Initialize; // Initialize the management client
	this.SetSaveStatus = MC_ChangeSaveStatus; // Change save status notifying user about previous save operation
	this.ControlHandler = new MC_ControlClientHandler;
	this.Session = new MC_SessionHandler;
	this.OpenExternal = MC_OpenExternal; // Opens External Link
	this.OpenInternal = MC_OpenInternal; // Opens Internal Link
	this.ResolveUrl = MC_ResolveUrl;

	// View information is stored on the client in the Views variable.
	// Format:
	//  Views[index][0] = appId
	//  Views[index][1] = viewId
	//  Views[index][2] = pageTitle (name)
	//  Views[index][3] = isNameDynamic (set page title from client or server)
	//  Views[index][4] = transition data
	//  Views[index][5] = isMetaClass

	this.QueryString = function(variable)
	{
		var searchString = '';

		//if(searchString == '')
		{
			/*
			if(Ext.isGecko)
			{                
			alert(eval(this.ContentFrame).location.search);
			searchString = eval(this.ContentFrame).contentDocument.location.search;
			}
			else
			*/
			searchString = eval(this.ContentFrame).location.search;
		}

		//alert(searchString);
		if (searchString != null && searchString.length > 0)
		{
			var query = searchString.substring(1);
			var vars = query.split("&");
			for (var i = 0; i < vars.length; i++)
			{
				var pair = vars[i].split("=");
				if (pair[0].toLowerCase() == variable.toLowerCase())
				{
					return pair[1];
				}
			}
		}

		return "";
	};
	this.OpenWindow = MC_OpenWindow; // Opens the modal window
	this.FindView = MC_FindView; // Finds view in the local array    
	this.MarkDirty = function()
	{
		this.IsPageDirty = true;
	};

	this.ListHasItemsSelected = function(source)
	{
		if (this.GetListCheckedItems(source, 0).length == 0)
		{
			alert("You must select one or more records before you can perform this action.");
			return false;
		}
		else
		{
			return true;
		}
	};

	this.ListHasItemsSelected2 = function(source)
	{
		if (source != null)
		{
			if (source.isChecked())
				return true;
			else
			{
				alert("You must select one or more records before you can perform this action.");
				return false;
			}
		}
		else
		{
			alert('Grid is null!');
			return false;
		}
	};

	this.GetSelectedGridItems = function(source)
	{
		if (source != null)
		{
			return source.getCheckedCollection();
		}
		else
		{
			alert('Grid is null!');
			return false;
		}
	};

	this.GetListCheckedItems = function(grid, columnNumber)
	{
		var checkedItems = new Array();

		var gridItem;
		var itemIndex = 0;

		while (gridItem = grid.get_table().getRow(itemIndex))
		{
			if (gridItem.get_cells()[columnNumber].get_value())
			{
				checkedItems[checkedItems.length] = gridItem;
			}

			itemIndex++;
		}

		return checkedItems;
	};

	this.confirmSubmit = function(msg)
	{
		var newmsg = "Are you sure you wish to continue?";
		if (msg != null || msg.length == 0)
			newmsg = msg;

		var agree = confirm(newmsg);
		if (agree)
			return true;
		else
			return false;
	};

	this.submitForm = function(action, params)
	{
		var actionCtrl = eval(this.ContentFrame + '.theForm._action');
		var paramCtrl = eval(this.ContentFrame + '.theForm._params');

		actionCtrl.value = action;
		paramCtrl.value = params;
		eval(this.ContentFrame).theForm.submit();
	};

	this.OpenHelp = function()
	{
	    this.OpenExternal(this.HELP_URL);
    };

    this.getEcfMainFrame = function() {
        var win = window;
        var retVal = win;
        // use try catch to ignore permission denied error when call cross-frame scripting
        try {
            while (win.parent != win) {
                if (typeof (CSManagementClient) != "undefined")
                    retVal = win;
                win = win.parent;
            }
        }
        catch (err) {
        }

        return retVal;
    };
};

function MC_SessionHandler()
{
	this.Save = function(name, value, days)
	{
		if (days)
		{
			var date = new Date();
			date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
			var expires = "; expires=" + date.toGMTString();
		}
		else var expires = "";
		document.cookie = name + "=" + value + expires + "; path=/";
	};

	this.Read = function(name)
	{
		var nameEQ = name + "=";
		var ca = document.cookie.split(';');
		for (var i = 0; i < ca.length; i++)
		{
			var c = ca[i];
			while (c.charAt(0) == ' ') c = c.substring(1, c.length);
			if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
		}
		return null;
	};

	this.Remove = function(name)
	{
		createCookie(name, "", -1);
	};

}

function MC_ControlClientHandler()
{
	this.trim = function(val)
	{
		return val.replace(/^\s+|\s+$/g, '');
	};

	this.ShowContextMenu = function(sender, item)
	{
		sender.showContextMenu(item);
	};

	this.MainNavBar_onItemSelect = function(sender, eventArgs)
	{
		CSManagementClient.ChangeMenuView(eventArgs.get_item().get_id(), eventArgs.get_item().get_text());
	};

	this.onNavMenuSelect = function(button, index)
	{
		var buttons = Ext.get('menu-buttons');
		buttons.select("div").each(function(member) { member.replaceClass('NavigationNavBarItemActive', 'NavigationNavBarItem'); });

		button.className = 'NavigationNavBarItemActive';
		CSManagementClient.ChangeMenuView2(index);
	};
}

function MC_ChangeTransition(name, params) {
    if (this.CurrentView == null || typeof (this.CurrentView) == "undefined")
		return;

	var trans = this.CurrentView[4];

	if (trans == null)
		return;

	var tran = null;
	for (var index = 0; index < trans.length; index++)
	{
		if (trans[index][0] == name)
		{
			tran = trans[index];
			break;
		}
	}

	if (tran == null)
		return;

	this.ChangeView(this.CurrentView[0], tran[1], params);
}

function MC_SetPageTitle(newTitle)
{
	var titleCtrl = $get(this.PageTitleControlId);

	if (titleCtrl != null)
	{
		if (MC_IsMSIE() > 0)
			titleCtrl.innerText = newTitle; // css styles in upper control get corrupted when using innerText in IE
		else
			titleCtrl.innerHTML = newTitle;
	}
}

function MC_ChangeView(appId, viewId, params)
{
	var foundView = this.FindView(appId, viewId);

	if (foundView == null || typeof (foundView) == "undefined")
		return;

	this.CurrentView = foundView;

	var name = foundView[2];
	var isNameDynamic = foundView[3];

	var isMetaClass = foundView[5];

	// don't set page title if isNameDynamic is true. In this case Name must be set in the code.
	if (!isNameDynamic)
		CSManagementClient.SetPageTitle(name);

	if (this.ContentFrame != null)
	{
	    // $get is a shortcut for document.getElementById(id);
		if (!isMetaClass)
		{
			if (params != undefined) {
				window.location = this.BASE_URL + 'EPiServerContentFrame.aspx?_a=' + appId + '&_v=' + viewId + '&' + params;
			}
			else
			{
			    window.location = this.BASE_URL + 'EPiServerContentFrame.aspx?_a=' + appId + '&_v=' + viewId;
			}
		}
		else
		{
			if (params != undefined)
			{
			    window.location = this.BASE_URL + 'EPiServerContentFrame.aspx?ClassName=' + appId + '&_v=' + viewId + '&' + params;
			}
			else
			{
			    window.location = this.BASE_URL + 'EPiServerContentFrame.aspx?ClassName=' + appId + '&_v=' + viewId;
			}
		}
	}

	CSManagementClient.SetSaveStatus('');
}

function MC_ChangeBafView(className, mode, params) {
	this.ChangeView(className, mode, params);
}

function MC_CloseTab()
{
	//    var contentView = Ext.getCmp(this.CenterCmpId); // TODO: make it configurable
	//    if(contentView.items.item(0) != contentView.getLayout().activeItem)
	//        {
	//            contentView.remove(contentView.getLayout().activeItem);
	//            contentView.getLayout().setActiveItem(0);
	//            contentView.show();
	//        }  
}

function MC_OpenWindow(appId, viewId, params, arguments, width, height)
{
	var foundView = this.FindView(appId, viewId);

	if (foundView == null)
		return;

	if (params != undefined)
	    window.showModalDialog(this.BASE_URL + 'EPiServerContentFrame.aspx?_a=' + appId + '&_v=' + viewId + '&' + params, arguments, 'dialogHeight:' + height + 'px;dialogWeight:' + width + 'px;');
	else
	    window.showModalDialog(this.BASE_URL + 'EPiServerContentFrame.aspx?_a=' + appId + '&_v=' + viewId, arguments, features);
}

function MC_ChangeMenuView2(index)
{
	if (typeof Ext == 'undefined')
		return;

	var menuView = Ext.getCmp('menu-panel'); // TODO: make it configurable
	menuView.getLayout().setActiveItem(index);

	// Save state info
	Ext.state.Manager.set("menu-view-selected", index);
}

function MC_RegisterMenuView(view)
{
	if (typeof Ext == 'undefined')
		return;

	var menuView = Ext.getCmp('menu-panel');

	if (!menuView.items.containsKey(view.id));
	menuView.add(view);
}

function MC_RefreshMenuView()
{
	alert(MainMultiPage.SelectedPage.DomElement.id);
}

function MC_Initialize()
{
	// Hide save option
	CSManagementClient.SetSaveStatus('');

	var pageTitle = '';

	if (this.CurrentView != null && typeof(this.CurrentView) != "undefined") {
		pageTitle = this.CurrentView[2];

		var isNameDynamic = this.CurrentView[3];

		// don't set page title if isNameDynamic is true. In this case Name must be set in the code.
		if (!isNaN(isNameDynamic) && !isNameDynamic)
			CSManagementClient.SetPageTitle(pageTitle);
	}
}

function MC_ChangeSaveStatus(status)
{
	try
	{
		var textCtrl = document.getElementById('SaveStatusCtrl'); //document.getElementById(this.SaveStatusId);

		if (textCtrl != null)
		{
		    if (status==null || status.length == 0)
				textCtrl.style.visibility = 'hidden';
			else 
			{
			    textCtrl.innerHTML = status;
			    textCtrl.style.visibility = 'visible';
			    window.setTimeout(MC_ChangeSaveStatus, 10000);
			}
        }
		//window.setTimeout(this.SaveStatusId+".style.visibility = 'hidden'", 10000);
	}
    catch (e) 
	{
		alert('failed to change save status: ' + e.Message);
	}
}


/* PRIVATE FUNCTIONS */
function MC_FindView(appId, viewId)
{
	// Find view
	var foundView = null;
	for (var index = 0; index < this.Views.length; index++)
	{
		if (this.Views[index][0] == appId && this.Views[index][1] == viewId)
		{
			foundView = this.Views[index];
			break;
		}
	}

	if (foundView == null)
	{
		alert("Specified view '" + viewId + "' can not be located in '" + appId + "' app");
		return null;
	}

	return foundView;
}

// detects if it is IE browser; returns IE version or 0
function MC_IsMSIE()
{
	var ua = window.navigator.userAgent.toLowerCase();
	var msie = ua.indexOf("msie");

	if (msie >= 0)      // if IE, return version number
		return parseInt(ua.substring(msie + 5, ua.indexOf(".", msie)));
	else              // otherwise, return 0
		return 0;
}

function MC_OpenExternal(url)
{
	window.open(url, 'ecfpopup');
	MC_ChangeSaveStatus('Page has been opened in another window.');
}

function MC_OpenInternal(url)
{
    //top.location.href = MC_ResolveUrl(url);
    CSManagementClient.getEcfMainFrame().location.href = MC_ResolveUrl(url);
}

function MC_ResolveUrl(url) {
    if (url.indexOf("~/") == 0) {
        url = CSManagementClient.BASE_URL + url.substring(2);
    }
    return url;
}

