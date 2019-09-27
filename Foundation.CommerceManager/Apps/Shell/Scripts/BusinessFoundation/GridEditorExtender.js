Type.registerNamespace("Mediachase");

// ----------------------
// ------- Grid ---------
// ----------------------


//Event args for onGridColumnResize
Mediachase.GridOnColumnResize = function(columnIndex, newSize)
{
    Mediachase.GridOnColumnResize.initializeBase(this);
    this.ColumnIndex = columnIndex;
    this.NewSize = newSize;
}

Mediachase.GridOnColumnResize.prototype =
{
  get_ColumnIndex: function()
  {
    return this.ColumnIndex;
  }, 
  set_ColumnIndex: function(value)
  {
    this.ColumnIndex = value;
  },
  get_NewSize: function()
  {
    return this.NewSize;
  }, 
  set_NewSize: function(value)
  {
    this.NewSize = value;
  }  
}

Mediachase.GridOnColumnResize.registerClass("Mediachase.GridOnColumnResize", Sys.EventArgs);

//Event args for onGridHiddenChange
Mediachase.GridOnHiddenChange = function(columnName, hidden)
{
    Mediachase.GridOnHiddenChange.initializeBase(this);
    this.ColumnName = columnName;
    this.Hidden = hidden;
}

Mediachase.GridOnHiddenChange.prototype =
{
  get_ColumnName: function()
  {
    return this.ColumnName;
  }, 
  set_ColumnName: function(value)
  {
    this.ColumnName = value;
  },
  get_Hidden: function()
  {
    return this.Hidden;
  }, 
  set_Hidden: function(value)
  {
    this.Hidden = value;
  }  
}

Mediachase.GridOnHiddenChange.registerClass("Mediachase.GridOnHiddenChange", Sys.EventArgs);


//Event args for onGridOrderChange
Mediachase.GridOnOrderChange = function(oldIndex, newIndex)
{
    Mediachase.GridOnOrderChange.initializeBase(this);
    this.OldIndex = oldIndex;
    this.NewIndex = newIndex;
}

Mediachase.GridOnOrderChange.prototype =
{
  get_OldIndex: function()
  {
    return this.OldIndex;
  }, 
  set_OldIndex: function(value)
  {
    this.OldIndex = value;
  },
  get_NewIndex: function()
  {
    return this.NewIndex;
  }, 
  set_NewIndex: function(value)
  {
    this.NewIndex = value;
  }  
}

Mediachase.GridOnOrderChange.registerClass("Mediachase.GridOnOrderChange", Sys.EventArgs);



Mediachase.GridOnEditEventArgs = function(id, columnIndex, allowEdit)
{
  Mediachase.GridOnEditEventArgs.initializeBase(this);
  this.Id = id;
  this.ColumnIndex = columnIndex;
  this.AllowEdit = allowEdit;
}
 
Mediachase.GridOnEditEventArgs.prototype =
{
  get_Id: function()
  {
    return this.Id;
  }, 
  set_Id: function(value)
  {
    this.Id = value;
  },
  
  get_ColumnIndex: function()
  {
    return this.ColumnIndex;
  },
  set_ColumnIndex: function(value)
  {
    this.ColumnIndex = value;
  },
  
  get_AllowEdit: function()
  {
    return this.AllowEdit;
  },
  set_AllowEdit: function(value)
  {
    this.AllowEdit = value;
  }    
}
 
Mediachase.GridOnEditEventArgs.registerClass("Mediachase.GridOnEditEventArgs", Sys.EventArgs);

Mediachase.GridOnCheckChangeArgs = function(checkCollection)
{
    Mediachase.GridOnCheckChangeArgs.initializeBase(this);
    this.CheckCollection = checkCollection;
}

Mediachase.GridOnCheckChangeArgs.prototype =
{
  get_CheckCollection: function()
  {
    return this.CheckCollection;
  }, 
  set_CheckCollection: function(value)
  {
    this.CheckCollection = value;
  }
}

Mediachase.GridOnCheckChangeArgs.registerClass("Mediachase.GridOnCheckChangeArgs", Sys.EventArgs);
 
// Grid Constructor
Mediachase.GridEditor = function(element) 
{
    Mediachase.GridEditor.initializeBase(this, [element]);
    
	var containerElement = null; // DOM Element - container
	var tableElement = null; // DOM Element - content table 
	var viewName = null; // MetaViewName
	var hfUpdate = null; // DOM Element - hidden field for generating client events
	var hfCheck = null; // DOM Element - hidden field for storing only chekced values in grid
	var pagingContainer = null; //Paging container
	var layoutContainer = null; //Layout container
	var disableSorting = null; // if true, disable client sorting
	var _enableModalPopup = null;
	var loadingImgUrl = null; // url to image in popup
	var showOnlyHeader = false;
	var servicePath = false; // path to webservice
	var contextKey = '';
	var customColumns = null; // number of custom columns (all custom columns must be before MetaView columns)
	var editEnable = null; 
	var checkColumnId =  null; //base id of checked columns checkColumnId_{num}
	
	var colArr = null;
	var extGrid = null;
	// TODO: 
	var selector = null;	
	var selector_ctrl = null;
	//for Ext.Grid binding
	var _data = null;
	var _colModel = null;
	var _divPagingContainer = null;
	var _divPagingInnerContainer = null;
	var _hasPaging = null;
	var _allowClientDrag = null;
	var _allowRowDragAndDrop = null;
	var _updateInterval = null;
	var _checkBoxEnable = null;
	
	//internal vars
	var _parent = null;
	var _windowsResizeHandler = null;
	var _cancelRowSelect = null;
	var _firstSelected = null;
	var _beginRequestHandler = null;
	var _endRequestHandler = null;
	var _scrollTimerId = null;
	
	var _componentArtDelegate = null;
	var _componentArtTimer = null;
	var _timer = null;
	var _timerId = null;
	var _timerScroll = null;
	var _updateArray = null;
	var _noItemInGridMsg = null;
	var _gridAttributes = null;	
	
	var _layoutResizer = null;
	var toolbarId = null;
	
	//events delegates
	var editList = null;
	var checkChange = null;
	var columnResizeList = null;
	var orderChangeList = null;
	var hiddenChangeList = null;
	
	var clientProp = null;	
	
	var inputsArr = null;
	var ie6OrLess = null;
	
	var __counter = 0;
	var __tmp = "";
}

Mediachase.GridEditor.prototype = 
{
	// -========= Properties =========-	
	get_toolbarId: function () {
		return this.toolbarId;
	},
	set_toolbarId: function (value) {
		this.toolbarId = value;
	},
	
	get_updateInterval: function () {
		return this._updateInterval;
	},
	set_updateInterval: function (value) {
		this._updateInterval = value;
	},
	
	get_disableSorting: function () {
		return this.disableSorting;
	},
	set_disableSorting: function (value) {
		this.disableSorting = value;
	},
	
	get_allowClientDrag: function () {
		return this._allowClientDrag;
	},
	set_allowClientDrag: function (value) {
		this._allowClientDrag = value;
	},	

	get_allowRowDragAndDrop: function () {
		return this._allowRowDragAndDrop;
	},
	set_allowRowDragAndDrop: function (value) {
		this._allowRowDragAndDrop = value;
	},	
		
	get_gridAttributes: function () {
		return this._gridAttributes;
	},
	set_gridAttributes: function (value) {
		this._gridAttributes = value;
	},	
		
	get_containerElement: function () {
		return this.containerId;
	},
	set_containerElement: function (value) {
		this.containerElement = value;
	},
	
	get_tableElement: function () {
		return this.tableElement;
	},
	set_tableElement: function (value) {
		this.tableElement = value;
	},

	get_viewName: function () {
		return this.viewName;
	},
	set_viewName: function (value) {
		this.viewName = value;
	},
	
	get_noItemInGridMsg: function () {
		return this._noItemInGridMsg;
	},		
	set_noItemInGridMsg: function (value) {
		this._noItemInGridMsg = value;
	},
	
	get_colArr: function () {
		return this.colArr;
	},		
	set_colArr: function (value) {
		this.colArr = value;
	},	
	
	get_hfUpdate: function () {
		return this.hfUpdate;
	},
	set_hfUpdate: function (value) {
		this.hfUpdate = value;
	},
	
	get_pagingContainer: function () {
		return this.pagingContainer;
	},
	set_pagingContainer: function (value) {
		this.pagingContainer = value;
	},	
	
	get_layoutContainer: function () {
		return this.layoutContainer;
	},
	set_layoutContainer: function (value) {
		this.layoutContainer = value;
	},
	
	get_checkBoxEnable: function () {
		return this._checkBoxEnable;
	},
	set_checkBoxEnable: function (value) {
		this._checkBoxEnable = value;
	},

	get_enableModalPopup: function () {
		return this._enableModalPopup;
	},
	set_enableModalPopup: function (value) {
		this._enableModalPopup = value;
	},	

	get_hfCheck: function () {
		return this.hfCheck;
	},
	set_hfCheck: function (value) {
		this.hfCheck = value;
	},		
	
	get_loadingImgUrl: function () {
		return this.loadingImgUrl;
	},
	set_loadingImgUrl: function (value) {
		this.loadingImgUrl = value;
	},
	
	get_showOnlyHeader: function () {
		return this.showOnlyHeader;
	},
	set_showOnlyHeader: function (value) {
		this.showOnlyHeader = value;
	},	
	
	get_servicePath: function () {
		return this.servicePath;
	},
	set_servicePath: function (value) {
		this.servicePath = value;
	},		

	get_contextKey: function () {
		return this.contextKey;
	},
	set_contextKey: function (value) {
		this.contextKey = value;
	},
	
	get_customColumns: function () {
		return this.customColumns;
	},
	set_customColumns: function (value) {
		this.customColumns = value;
	},	
	
	get_editEnable: function () {
		return this.editEnable;
	},
	set_editEnable: function (value) {
		this.editEnable = value;
	},		
	
	get_checkColumnId: function () {
		return this.checkColumnId;
	},
	set_checkColumnId: function (value) {
		this.checkColumnId = value;
	},		
	

	// -========= Methods =========-
	// ctor()
	initialize : function() {
		//TODO: create and init selector
		//alert('start grid loader');
        Mediachase.GridEditor.callBaseMethod(this, 'initialize');
		
		//If no items in grid, then show "noItemInGridMsg"
		if (this.tableElement == null && !this.showOnlyHeader)
		{
			this.containerElement.style.border = '0px solid Black';
			this.containerElement.style.textAlign = 'center';
			this.containerElement.innerHTML = this._noItemInGridMsg;
			return;
		}
		
		if (typeof(document.body.style.maxHeight) === "undefined")
			this.ie6OrLess = true;
		else
			this.ie6OrLess = false;
		
		//Attach to start/finish update panel events
		this._beginRequestHandler = Function.createDelegate(this, this.onBeginReqestHandler);
		this._endRequestHandler = Function.createDelegate(this, this.onEndReqestHandler);
					
		Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(this._beginRequestHandler);
		Sys.WebForms.PageRequestManager.getInstance().add_endRequest(this._endRequestHandler);
		
		//$addHandler(window, "resize", Function.createDelegate(this, this.layoutHandler));
		
		this.__counter = 0;
		this.__tmp = "";
		this._firstSelected = true;
		
		this._cancelRowSelect = true;
		this.selector_ctrl = false;
		
		//enable events
		this.editList = new Sys.EventHandlerList();
		this.beforeCreateList = new Sys.EventHandlerList();
		this.checkChange = new Sys.EventHandlerList();
		this.columnResizeList = new Sys.EventHandlerList();
		this.orderChangeList = new Sys.EventHandlerList();
		this.hiddenChangeList = new Sys.EventHandlerList();		
		
		//render paging
		this.generatePaging();
		//render grid
		this.generateGrid();
		
		//attach to events for resizing height/width
		this._layoutResizer = Function.createDelegate(this, this.layoutHandler);
		this.attachToLayout();
		
		//ie bug fix
		if (this._divPagingContainer)
		{
			this._divPagingContainer.style.marginLeft = '0px';			 
		}			
		
		this._updateInterval = 5000;
		//this._timerId = window.setInterval(Function.createDelegate(this, this.updateToServer), 1000);
		this._updateArray = new Array();
		
		if (typeof(Ibn) !== 'undefined' && typeof(Ibn.LayoutEventArgs) !== 'undefined' && this._element)
			this.layoutHandler(null, new Ibn.LayoutEventArgs(this._element.clientHeight, this._element.clientWidth, null, null, null));

        $addHandler(this.extGrid.getView().scroller.dom, "scroll", Function.createDelegate(this, this.onScrollGrid));        

		//alert('ctor: ' + this._gridAttributes.value);
		if (this._gridAttributes && this._gridAttributes.value != '')
		{	
			//this.clientProp is storing any values between postbacks
		    this.clientProp = Sys.Serialization.JavaScriptSerializer.deserialize(this._gridAttributes.value);

			//scroll restore
		    if (this.clientProp.scrollTop && this.extGrid)
		        this._timerScroll = window.setInterval(Function.createDelegate(this, this.restoreScrolling), 100);
		}
		
		//var _inputsArr = document.getElementsByTagName("INPUT");
		this.inputsArr = document.getElementsByTagName("INPUT"); //new Array();	
		//alert('end grid loader');
	},
	dispose: function()
	{
	    if (this._endRequestHandler)
	    {
	        Sys.WebForms.PageRequestManager.getInstance().remove_endRequest(this._endRequestHandler);
	        this._endRequestHandler = null;
	    }
	    
	    if (this._beginRequestHandler)
	    {
	        Sys.WebForms.PageRequestManager.getInstance().remove_beginRequest(this._beginRequestHandler);
	        this._beginRequestHandler = null;
	    }	    
	    
	    if (this._timerScroll)
	    {
	        window.clearTimeout(this._timerScroll);
	    }
	    
		if (this._componentArtInterval != null)
		{
			window.clearInterval(this._componentArtInterval);
		}
		
		if (this._timerId != null)
		{
			window.clearInterval(this._timerId);
		}
		
		if (this.extGrid != null && !this.showOnlyHeader)
		{
		    $clearHandlers(this.extGrid.getView().scroller.dom);
			this.extGrid.purgeListeners();
			if (!document.all)
				this.extGrid.destroy(true);
			this.extGrid = null;
		}
		
		this._updateArray = null;
		this.editList = null;
		this.beforeCreateList = null;
		this.checkChange = null;
		Mediachase.GridEditor.callBaseMethod(this, 'dispose');
	},
	//restore scroll position in grid after postback
	restoreScrolling: function(scrollTop)
	{
		if (this.showOnlyHeader)
			return;
			
	    var clientProp = Sys.Serialization.JavaScriptSerializer.deserialize(this._gridAttributes.value);
	    if (this.extGrid.getView().scroller.dom.scrollHeight > clientProp.scrollTop)
	    {
	        this.extGrid.getView().scroller.scrollTo("top", clientProp.scrollTop, false);
	        window.clearTimeout(this._timerScroll);
	    }
	},
	//autosize handler
	layoutHandler: function(sender, args)
	{
		if (args && this._element)
		{
			var obj = this._element.parentNode;
			var newGridHeight = args._height;
			var newGridWidth = args._width;
			
			if (this.ie6OrLess)
			{
				newGridWidth -= 14;
				newGridHeight -= 50;
			}
		    	    
			if (newGridHeight <= 0)
				return;
		    
			this.containerElement.style.height = newGridHeight + "px"; // -30
			
			if (this.ie6OrLess)
				this.containerElement.style.width = newGridWidth + "px";

			if (this.containerElement.parentNode && this.containerElement.parentNode.style)
			{
				this.containerElement.parentNode.style.height = newGridHeight + "px";
				
				if (this.ie6OrLess)
					this.containerElement.parentNode.style.width = newGridWidth + "px";
			}
				
			
			if (this.extGrid != null)
			{
				this.extGrid.autoHeight = false;
				//this.extGrid.doLayout();			
				this.extGrid.setHeight(newGridHeight - 1);
				
				if (this.ie6OrLess)
				{
					this.extGrid.setWidth(newGridWidth);
					this.extGrid.autoWidth = false;
				}
			}	 
		}  
	},
	attachToLayout: function()
	{
		var startPoint = this.containerElement;
		while (startPoint.parentNode != null)
		{
			if (startPoint.nodeType == 1 && startPoint.tagName == "FORM")
			{	
				//alert('failed initializing');
				return;
			}
			
			if (startPoint != "undefined" && startPoint.control != null && startPoint.control.resizeList != null)
			{
				//alert('attached');
				startPoint.control.add_resize(Function.createDelegate(this, this.layoutHandler));

				this.containerElement.style.height = startPoint.control._containerId.parentNode.offsetHeight - startPoint.control._mTop - startPoint.control._mBottom + "px"; // -30
				this.containerElement.parentNode.style.height = startPoint.control._containerId.parentNode.offsetHeight - startPoint.control._mTop - startPoint.control._mBottom + "px";
				this.extGrid.autoHeight = false;
				//this.extGrid.doLayout();
				
				return;
			}
			startPoint = startPoint.parentNode;
		}			
	},
	//raise postback event
	updateToServer: function ()
	{
		if (this._timer == - 1)
			return;
		if ((new Date).getTime() - this._timer	> this._updateInterval)
		{
			this._timer = -1;
			//testTask.ListHandler.UpdateGrid(this.viewName, this._updateArray);
			this._updateArray = new Array();
			this.refresh();
		}
	},	

	// -========= Internal functions =========-	
	// show "Loading" screen
	showModalGrid: function ()
	{
		if (!this._enableModalPopup)
			return;
			
		divGrid = document.createElement("DIV");
		parentDiv = this.layoutContainer;
	    
		if (parentDiv == null || divGrid == null)
		{
			alert("Error @showModalGrid() some arguments are null /r/n parentDiv = "+parentDiv);
			return;
		}
	    
		divGrid.id = parentDiv.id + "_popupDiv";
		divGrid.style.position = "absolute";
		divGrid.style.left = "0px";
		divGrid.style.top = "0px";
		//divGrid.style.left = parentDiv.offsetLeft + "px";
		//divGrid.style.top = parentDiv.offsetTop + "px";
		divGrid.style.width = "100%";
		divGrid.style.height = "100%";
		//divGrid.style.width = parentDiv.style.width;
		//divGrid.style.height = parentDiv.style.height;
		divGrid.style.backgroundColor = "Transparent";
		// todo: divGrid.style.backgroundImage = 'url(../images/point8.gif)';
		divGrid.style.backgroundRepeat = "repeat";
		divGrid.style.zIndex = 1100;
		divGrid.align="center";
		divGrid.style.verticalAlign = "middle";
	    
		divGridCenter = document.createElement("DIV");
		divGridCenter.id = parentDiv.id + "_popupDivCentered";
		divGridCenter.style.position = "absolute";
		divGridCenter.style.left = "40%";
		divGridCenter.style.top = "35%";
		divGridCenter.style.width = "20%";
		divGridCenter.style.height = "15%";
		divGridCenter.style.backgroundColor = "White";    
		divGridCenter.style.zIndex = 1200;
		divGridCenter.style.border = "solid Black 2px";
	    
		parentDiv.appendChild(divGrid);
		divGrid.appendChild(divGridCenter);
	    
		_innerHtml = ""; 
		_height = (parseInt(divGridCenter.clientHeight) / 2) - 10 + 'px';
		_innerHtml += "<div align='center' style='margin-left: 10px; margin-top: " + _height + ";'><div style='width:20px;height:20px;padding:3px;background-color: White;border:1px solid #aaa;'><img border='0' src='" + this.loadingImgUrl + "' /></div></div>";
		divGridCenter.innerHTML = _innerHtml;  	
	},
	
	// hide "Loading" screen
	hideModalGrid: function ()
	{
		if (this.layoutContainer)
		{
			parentDiv = this.layoutContainer;
			divGrid = document.getElementById(parentDiv.id+"_popupDiv");
	    
			if (divGrid == null || parentDiv == null)
				return;
	        
			parentDiv.removeChild(divGrid);	
		}
	},
	//grid render
	generateGrid: function ()
	{
		this.extGrid = new Ext.grid.TableGrid(this);
		this.extGrid.enableHdMenu = false;
		this.extGrid.enableColumnHide = false;
		this.extGrid.collapsible = false;
		
		if (this.extGrid.store.data.length == 0 && !this.showOnlyHeader)
		{
			this.showOnlyHeader = true;
//			var divGrid = document.createElement("DIV");
//			divGrid.style.height = '100%';
//			divGrid.style.width = '100%';
//			divGrid.style.position = 'absolute';
			
		}
			
		if (this._gridAttributes && this._gridAttributes.value != '')
		{
		    //alert('in before create');
		    //this._gridAttributes.value = this._gridAttributes.value.replace("';", "'");
		    //alert(this._gridAttributes.value);
		    this.clientProp = Sys.Serialization.JavaScriptSerializer.deserialize(this._gridAttributes.value);	    
		    //alert(this._gridAttributes.value)
            this.restoreCheckCoollection();	
        }
        
		this.extGrid.autoWidth = false;
		this.extGrid.autoHeight = false;
		//alert('grid render');
			
		this.extGrid.render();
		
		this.renderPaging();
		window.setTimeout(Function.createDelegate(this, this.applyPagingPanel), 50);
		
		this.extGrid.enableHdMenu = false;
		this.extGrid.enableColumnHide = false;
		this.extGrid.collapsible = false;		
		
		this.extGrid.on("columnresize", Function.createDelegate(this, this.onColumnResize), this.extGrid);
		this.extGrid.on("headerclick", Function.createDelegate(this, this.onHeaderClick), this.extGrid);
		this.extGrid.on("mousedown", Function.createDelegate(this, this.onMouseDown), this.extGrid);
		this.extGrid.on("rowclick", Function.createDelegate(this, this.onRowClick), this.extGrid);
		this.extGrid.on("click", Function.createDelegate(this, this.onClick), this.extGrid);
		this.extGrid.on("headercontextmenu", Function.createDelegate(this, this.onHeaderContextMenu), this.extGrid);
		this.extGrid.getColumnModel().on("columnmoved", Function.createDelegate(this, this.onOrderChange), this.extGrid);
		this.extGrid.getColumnModel().on("hiddenchange", Function.createDelegate(this, this.onHiddenChange), this.extGrid);
		this.extGrid.getColumnModel().isCellEditable = Function.createDelegate(this, this.onGridCellEditor);
		
		this.extGrid.selModel.on("rowselect", Function.createDelegate(this, this.onRowSelect), this.extGrid);
		this.extGrid.selModel.on("rowdeselect", Function.createDelegate(this, this.onRowDeselect), this.extGrid);
		this.extGrid.selModel.on("beforerowselect", Function.createDelegate(this, this.onBeforeRowSelect), this.extGrid);
		this.extGrid.on("afteredit", Function.createDelegate(this, this.onAfterUpdate), this.extGrid);
		this.extGrid.on("beforeedit", Function.createDelegate(this, this.onBeforeUpdate), this.extGrid);
		this.extGrid.on("validateedit", Function.createDelegate(this, this.onValidateEdit), this.extGrid);
		
		//TODO: height as param
		if (this.showOnlyHeader)
		{
			this.containerElement.style.height = "100px"; // -30
			this.containerElement.parentNode.style.height = "100px";			
			this.extGrid.setHeight(100);
		}				
	},
	//render paging
	renderPaging: function ()
	{
		var footPanel = this.extGrid.getBottomToolbar().el.dom;
		
		if (footPanel.childNodes.length > 0)
			footPanel.removeChild(footPanel.childNodes[0]);
		
		// Need to add check here, if grid didn't return any matching columns/rows
		if (this._divPagingContainer != null) 
		{
		    footPanel.appendChild(this._divPagingInnerContainer);
		    //ie bug fix
		    this._divPagingContainer.style.marginLeft = '1px';
		}
		
//		var tableObj = document.createElement("TABLE");
//		var tableRow = document.createElement("TR");
//		tableObj.appendChild(tableRow);
		
		if (this.pagingContainer != null)
		{
			for (i = 0; i < this.pagingContainer.childNodes.length; i++)
			{
				if (this.pagingContainer.childNodes[i].nodeType == 1)
				{
					if (this._divPagingContainer != null)
					{
						//var tableCell = document.createElement
						newObject = this.pagingContainer.childNodes[i].cloneNode(true);
			
						//IE BUG:
						//When IE process cloneNode() it reset selectionIndex for <select> nodes
						if (!!document.all)
						{
							newObject = this.pagingContainer.childNodes[i].cloneNode(false);
							newObject.innerHTML = this.pagingContainer.childNodes[i].innerHTML;
						}
						
						this.pagingContainer.childNodes[i].style.display = 'none';
						newObject.style.display = 'inline';
						this._divPagingContainer.appendChild(newObject);
					}
					else
					{
						footPanel.appendChild(this.pagingContainer.childNodes[i]);
					}
				}
			}
			
			//remove all controls from old paging container
			while (this.pagingContainer.childNodes.length != 0)
			{
				this.pagingContainer.removeChild(this.pagingContainer.childNodes[this.pagingContainer.childNodes.length - 1]);
			}
		}	
		
		var topBar = this.extGrid.getTopToolbar();
		//topBar.contentEl = this.toolbarId;
	},
	generatePaging: function ()
	{
		tBody = null; lastRow = null; firstTd = null; innerTable = null;
		tableObj = this.tableElement;
		
		//if grid has 0 records
		if (tableObj == null) return;
		
		for (i = 0; i < tableObj.childNodes.length; i++) 
		{
			if (tableObj.childNodes[i].nodeType == 1 && tableObj.childNodes[i].tagName == 'TBODY') 
			{
				tBody = tableObj.childNodes[i]; 
				break;
			}
		}
		if (!tBody) return;
		for (i = tBody.childNodes.length - 1; i > 0; i--) 
		{
			if (tBody.childNodes[i].nodeType == 1 && tBody.childNodes[i].tagName == 'TR') 
			{
				lastRow = tBody.childNodes[i]; 
				break;
			}
		}
		if (!lastRow) return;
		for (var i = 0; i < lastRow.childNodes.length; i++) 
		{
			if (lastRow.childNodes[i].nodeType == 1 && lastRow.childNodes[i].tagName == 'TD') 
			{
				firstTd = lastRow.childNodes[i]; 
				break;
			}
		}
		for (var i = 0; i < firstTd.childNodes.length; i++) 
		{
			if (firstTd.childNodes[i].nodeType == 1 && firstTd.childNodes[i].tagName == 'TABLE') 
			{
				innerTable = firstTd.childNodes[i];
			}
		}
		
		// If no paging
		//if (innerTable == null) { return; }
		
		parentDiv = this.containerElement;
		
		this._divPagingInnerContainer = document.createElement('DIV');
		this._divPagingInnerContainer.id = this.tableElement.id + '_innerpaging';
		this._divPagingInnerContainer.style.height = '28px'; //was: 22px
		this._divPagingInnerContainer.style.position = 'static';
		//this._divPagingInnerContainer.style.position = 'relative'; //doesnt work in IE
		this._divPagingInnerContainer.style.width = '100%';
		this._divPagingInnerContainer.style.padding = '0px';
		this._divPagingInnerContainer.style.border = '0px';
		this._divPagingInnerContainer.style.borderBottom ='solid 1px #6B92CE';
		this._divPagingInnerContainer.style.zIndex = 999;
		this._divPagingInnerContainer.className = 'x-toolbar x-small-editor';
		
		
		this._divPagingContainer = document.createElement('DIV');
		this._divPagingContainer.id = this._divPagingInnerContainer.id + '_paging';
				
		this._hasPaging = (innerTable != null);
		
		if (innerTable != null)
		{
			pagingNode = innerTable.cloneNode(true);			
			innerTable.parentNode.removeChild(innerTable);
			this._divPagingContainer.appendChild(pagingNode);
			//tBody.removeChild(lastRow);
			pagingNode.style.display = 'inline';
			innerTable.style.display = 'none';
		}
		
		this._divPagingContainer.style.height = '28px'; //was: 22px
		this._divPagingContainer.style.position = 'relative';
		//this._divPagingContainer.style.position = 'relative'; //doesnt work in IE
		this._divPagingContainer.style.width = '100%';
		this._divPagingContainer.style.padding = '0px';
		this._divPagingContainer.style.border = '0px';
		this._divPagingContainer.style.zIndex = 999;
		this._divPagingContainer.className = 'x-toolbar x-small-editor';
		this._divPagingInnerContainer.appendChild(this._divPagingContainer);
	},
	
	//call after rendering grid & paging for apllying paging elemnts left/right
	applyPagingPanel: function()
	{
		if (this.showOnlyHeader)
			return;
			
		var totalWidth = this._divPagingContainer.offsetWidth;
		var oldOffset = 2;
		for (var i = 0; i < this._divPagingContainer.childNodes.length; i++)
		{
			if (this._divPagingContainer.childNodes[i].getAttribute("ibnalign") == 'right')
			{
				totalWidth -= this._divPagingContainer.childNodes[i].offsetWidth;				
				this._divPagingContainer.childNodes[i].style.position = 'absolute';
				this._divPagingContainer.childNodes[i].style.left = totalWidth + 'px';
			}
			else
			{				
				this._divPagingContainer.childNodes[i].style.position = 'absolute';
				this._divPagingContainer.childNodes[i].style.left = oldOffset + 'px';
				oldOffset += this._divPagingContainer.childNodes[i].offsetWidth;
			}
		}
	},
	
	refresh: function ()
	{
		this.hfUpdate.value = 'Refresh'+(new Date).getTime();
		__doPostBack(this.hfUpdate.id, '');
	},
	//store checked elements before postback
	fillCheckCollection: function(rowIndex)
	{
	    if (typeof(this.clientProp) !== 'undefined' && this._gridAttributes.value != '')
	        this.clientProp = Sys.Serialization.JavaScriptSerializer.deserialize(this._gridAttributes.value);
        else
            this.clientProp = {};
//	    if (this.clientProp)
//	    {
	        var checkedArr = '';
	        if (this.clientProp.checkedCollection)
	            checkedArr = this.clientProp.checkedCollection;
	        	        
	        if (typeof(rowIndex) === 'number' && rowIndex != -1 && this.checkColumnId)
	        {
				var checkItem = document.getElementById(this.checkColumnId + '_' + rowIndex);
				if (checkItem )
				{
					if (this.extGrid && checkItem.checked && checkedArr.indexOf(this.extGrid.getStore().data.items[rowIndex].data.primaryKeyId + ';') == -1)
					{
						checkedArr += this.extGrid.getStore().data.items[rowIndex].data.primaryKeyId + ';';
					}
					else if (this.extGrid && !checkItem.checked && checkedArr.indexOf(this.extGrid.getStore().data.items[rowIndex].data.primaryKeyId + ';') != -1)
					{
						checkedArr = checkedArr.replace(this.extGrid.getStore().data.items[rowIndex].data.primaryKeyId + ';', '');
					}
				}
	        }
	        else
	        {
		        var rows = Ext.get(this.containerElement).query("div.x-panel-bwrap div.x-panel-body div.x-grid3 div.x-grid3-viewport div.x-grid3-scroller div.x-grid3-body div.x-grid3-row"); // table tbody tr
	        
				//bezhim po vsem elementam dataSource	        
				for (var i = 0; i < rows.length; i++)
				{
					var checkItem = Ext.get(rows[i]).query("table tbody tr td.x-grid3-cell-first div input");
					if (checkItem.length > 0)
					{
						if (checkItem[0].checked)
						{
							if (this.extGrid && checkedArr.indexOf(this.extGrid.getStore().data.items[i].data.primaryKeyId + ';') == -1 )
							{
								checkedArr += this.extGrid.getStore().data.items[i].data.primaryKeyId + ';';
							}
						}
					}
				}
	        }
	        
	        //alert('fillCheckCollection :' + checkedArr);
	        if (this._gridAttributes.value.length > 0 )
            {
                var oldObj = Sys.Serialization.JavaScriptSerializer.deserialize(this._gridAttributes.value);
                oldObj.checkedCollection = checkedArr;
                this._gridAttributes.value = Sys.Serialization.JavaScriptSerializer.serialize(oldObj);
            }
            else
            {
                this._gridAttributes.value = "{ checkedCollection: '" + checkedArr + "' }";
            }
            
            if (this.hfCheck)
				this.hfCheck.value = checkedArr;
            
            //alert(this._gridAttributes.value);
	    //}
	},
	//restore checked elements after postback
	restoreCheckCoollection: function()
	{
		if (this.showOnlyHeader)
			return;
			
	    if (this.clientProp)
	    {
	        var checkedArr = '';
	        var newCheckedArr = '';
	        
	        if (this.clientProp.checkedCollection)
	            checkedArr = this.clientProp.checkedCollection;
	        
	        //alert('restoreCheckCoollection: ' + checkedArr);
	        //bezhim po vsem elementam dataSource
	        for (var i = 0; i < this.extGrid.getStore().data.items.length; i++)
	        {
	            if (checkedArr.indexOf(this.extGrid.getStore().data.items[i].data.primaryKeyId + ';') > -1)
	            {
	                this.extGrid.getStore().data.items[i].data.customData += 'checked=true;';
	                newCheckedArr += this.extGrid.getStore().data.items[i].data.primaryKeyId + ';';
	            }
	        }
	        
	        this.clientProp.checkedCollection = newCheckedArr;
	        this._gridAttributes.value = Sys.Serialization.JavaScriptSerializer.serialize(this.clientProp);
	        
            if (this.hfCheck)
				this.hfCheck.value = newCheckedArr;	        
	    }	
	       
	},
	//save scroll position in grid before postback
	fillScrollInfo: function()
	{
	    if (this._gridAttributes)
	    {
			this.fillCheckCollection(-1);
	        if (this._gridAttributes.value != '')
	        {
	            var oldObj = Sys.Serialization.JavaScriptSerializer.deserialize(this._gridAttributes.value);
	            oldObj.scrollTop = this.extGrid.getView().scroller.dom.scrollTop;
	            this._gridAttributes.value = Sys.Serialization.JavaScriptSerializer.serialize(oldObj);
	            //alert('scrolling: ' + this._gridAttributes.value);
	        }
	        else
	        {
	            this._gridAttributes.value = "{scrollTop: " + this.extGrid.getView().scroller.dom.scrollTop + "}";
	            //alert('scroll only scrolling: ' + this._gridAttributes.value);
	        }
	    }		
	},
	//get checked items in grid
	getCheckCollection: function()
	{
        var checkedArr = '';
        
	    if (!this.clientProp && this._gridAttributes.value != '')
	        this.clientProp = Sys.Serialization.JavaScriptSerializer.deserialize(this._gridAttributes.value);
        else
            this.clientProp = {};        
        
        if (this.clientProp.checkedCollection)
            checkedArr = this.clientProp.checkedCollection;
            
        var rows = Ext.get(this.containerElement).query("div.x-panel-bwrap div.x-panel-body div.x-grid3 div.x-grid3-viewport div.x-grid3-scroller div.x-grid3-body div.x-grid3-row");//div div.x-grid-viewport div.x-grid-body table tbody tr
        //loop through all dataSource elements
        for (var i = 0; i < rows.length; i++)
        {
            var checkItem = Ext.get(rows[i]).query("table tbody tr td.x-grid3-cell-first div input"); //td div div input
            if (checkItem.length > 0)
            {
                if (checkItem[0].checked)
                {
                    if (this.extGrid && checkedArr.indexOf(this.extGrid.getStore().data.items[i].data.primaryKeyId + ';') == -1 )
                    {
                        checkedArr += this.extGrid.getStore().data.items[i].data.primaryKeyId + ';';
                    }
                }
            }
        } 
        
        if (checkedArr.length > 0)
            checkedArr = checkedArr.substr(0, checkedArr.length - 1);
        
        var regExp = /([;])+/g;
        
        return checkedArr.replace(regExp,',');        
	},
		
	// -========= Events =========-
	//events handlers for extGrid 	
	onGridCellEditor: function(colIndex, rowIndex)
	{
		if (!this._checkBoxEnable)
		{
			return true;
		}
		else
		{
			if (colIndex > 0)
				return true;
			else
				return false;
		}
	},
	onHeaderContextMenu: function (obj, columnIndex, e)
	{
		//obj.view.hmenu.hide();
	},
		
	onHiddenChange: function (obj, columnIndex, hidden)
	{	    
		if (hidden == false)
		{
			this.get_hfUpdate().value = 'Show,' + obj.getColumnId(columnIndex);
			__doPostBack(this.get_hfUpdate().id, '');
		}
		else
		{			
			//this.showModalGrid();
			this.onGridHiddenChange(obj.getColumnId(columnIndex), hidden);
			if (this.servicePath != '')
			{			
				var params = {}; //Sys.Serialization.JavaScriptSerializer.serialize("{\"ViewName\": \"" + this.viewName + "\", \"ColumnName\": \"" + obj.getColumnId(columnIndex) + "\"}");
				params.ContextKey = this.contextKey;
				params.ColumnName = obj.getColumnId(columnIndex);
				Sys.Net.WebServiceProxy.invoke(this.servicePath, "HideColumn", false, params, Function.createDelegate(this, this.onRequestSuccess), Function.createDelegate(this, this.onRequestFailed));
			}
			//Mediachase.UI.Web.IbnNext.WebServices.ListHandler.HideColumn(this.viewName, obj.getColumnId(columnIndex), Function.createDelegate(this, this.onRequestSuccess), Function.createDelegate(this, this.onRequestFailed));			
		}
	},
	
	onOrderChange: function (obj, oldIndex, newIndex)
	{
		//this.showModalGrid();
		this.onGridOrderChange(oldIndex, newIndex);
		//uncomment this lines for handling columns OrderChange event at server
		if (this.servicePath != '')
		{
			var params = {}; 
			params.ContextKey = this.contextKey;
			if (this._checkBoxEnable)
			{
				params.OldIndex = oldIndex - 1;
				params.NewIndex = newIndex - 1;
			}
			else
			{
				params.OldIndex = oldIndex;
				params.NewIndex = newIndex;
			}
			Sys.Net.WebServiceProxy.invoke(this.servicePath, "OrderChange", false, params, Function.createDelegate(this, this.onRequestSuccess), Function.createDelegate(this, this.onRequestFailed));
		}
		//Mediachase.UI.Web.IbnNext.WebServices.ListHandler.OrderChange(this.viewName, oldIndex, newIndex, Function.createDelegate(this, this.onRequestSuccess), Function.createDelegate(this, this.onRequestFailed)); 
	},
	
	onHeaderClick: function (obj, columnIndex, e)
	{		
		if (this.disableSorting)
			return;
			
		if (this._checkBoxEnable && columnIndex == 0)
		{
			this.fillCheckCollection(-1);
			return;
		}
			
		e.stopEvent();
		e.stopPropagation();
		//this.showModalGrid();
		this.get_hfUpdate().value = 'Sort,' + obj.getColumnModel().getColumnId(columnIndex) + ',' + (new Date).getTime(); 
		__doPostBack(this.get_hfUpdate().id, '');	
	},
	
	onColumnResize: function (columnIndex, newSize)
	{
	    if (this._checkBoxEnable)
	    {
	        if (columnIndex > 0)
				this.onGridColumnResize(columnIndex - 1, newSize);
				
			if (this.servicePath != '')
			{
				var params = {}; 
				params.ContextKey = this.contextKey;
				params.ColumnIndex = columnIndex - 1;
				params.NewSize = newSize;
				Sys.Net.WebServiceProxy.invoke(this.servicePath, "ColumnResize", false, params, Function.createDelegate(this, this.onRequestSuccess), Function.createDelegate(this, this.onRequestFailed));
			}
				//uncomment this lines for handling ColumnResize event at server   
		        //Mediachase.UI.Web.IbnNext.WebServices.ListHandler.ColumnResize(this.viewName, columnIndex - 1, newSize, Function.createDelegate(this, this.onRequestSuccess), Function.createDelegate(this, this.onRequestFailed));
		}
		else
		{
			this.onGridColumnResize(columnIndex, newSize);
			//uncomment this lines for handling ColumnResize event at server
		    //Mediachase.UI.Web.IbnNext.WebServices.ListHandler.ColumnResize(this.viewName, columnIndex, newSize, Function.createDelegate(this, this.onRequestSuccess), Function.createDelegate(this, this.onRequestFailed));
			if (this.servicePath != '')
			{
				var params = {}; 
				params.ContextKey = this.contextKey;
				params.ColumnIndex = columnIndex;
				params.NewSize = newSize;
				Sys.Net.WebServiceProxy.invoke(this.servicePath, "ColumnResize", false, params, Function.createDelegate(this, this.onRequestSuccess), Function.createDelegate(this, this.onRequestFailed));
			}		    
		}
	},
	onMouseDown: function (e)
	{
	    this.__counter++;
	    //this.__tmp += this.__counter + ' onMouseDown \r\n';
	
	    //alert(this.__tmp);
	    //this._cancelRowSelect = null;
	    //alert('mouaedown');
		this.selector_ctrl = e.ctrlKey;
		//this.__counter = 0;
		//this.__tmp = "";
	},
	onClick: function(e)
	{
	    this.__counter++;
	    //this.__tmp += this.__counter + ' onClick \r\n';
	    //alert('onClick : ' + this.__counter);
	   //alert('on click ' + e.target.getAttribute("ecfgridcheckbox"));
	        	    
	},
	onRowClick: function(obj, rowIndex, e)
	{	    
	    this.__counter++;
	    this.fillCheckCollection(rowIndex);
	    
	    //this.__tmp += this.__counter + ' onRowClick ' + e.target.checked + '\r\n';	
	    
	    // uncomment to change checkbox status on rowclick event
	    this.onCheckChange();
	    
//	    if (!e.target.checked)
//	    {			
//			for (var i = this.inputsArr.length - 1; i >= 0; i--)
//			{					
//				if (this.inputsArr[i].getAttribute("ecfmaincheckbox"))
//				{
//					this.inputsArr[i].checked = false;
//					return;
//				}
//			}	    
//	    }
//	    
//	    if (e && e.target.getAttribute("ecfgridcheckbox") == 0 && e.target.getAttribute("ecfparentblock") == 0)
//	    {
//			var parentId = e.target.getAttribute("ecfgridcheckboxid");
//			for (var i = this.inputsArr.length - 1; i >= 0; i--)
//			{
//				if (!this.inputsArr[i].getAttribute("ecfparentblock") && parentId == this.inputsArr[i].getAttribute("ecfgridcheckboxid"))
//				{
//					this.inputsArr[i].checked = e.target.checked;
//				}
//			}
//	    }
//	    
//	    if (e && e.target.getAttribute("ecfgridcheckbox") == 0 && !e.target.getAttribute("ecfparentblock") && !e.target.checked)
//	    {
//			var parentId = e.target.getAttribute("ecfgridcheckboxid");
//			for (var i = this.inputsArr.length - 1; i >= 0; i--)
//			{
//				if (this.inputsArr[i].getAttribute("ecfparentblock") && parentId == this.inputsArr[i].getAttribute("ecfgridcheckboxid"))
//				{
//					this.inputsArr[i].checked = false;
//					return;
//				}
//			}
//	    }
   
//        if (e && e.target.getAttribute("ecfgridcheckbox") == 0)
//            this._cancelRowSelect = false;
//        else
//            this._cancelRowSelect = true;	           
//        
//        if (this._cancelRowSelect)
//        {
//            this.extGrid.selModel.selectRow(rowIndex);
//        }
//        
//        this._firstSelected = false;
//        this._cancelRowSelect = false;
        
        //uncomment for debug
	    //alert(this.__tmp);
	    //this.__tmp = "";
	    this.__counter = 0;	        
	},
	onRequestSuccess: function (result)
	{
		//TO DO:
		this.hideModalGrid();
	},
	onRequestFailed: function (error)
	{
		alert('Request Failed: ' + error.toString());
		//TO DO:
		this.hideModalGrid();
	},
	onBeginReqestHandler: function (sender, args)
	{	    	    
		//this.fillCheckCollection(-1);
		//this.showModalGrid();
		
		if (sender._panelsToRefreshIDs == null) return;								
		
		if (this.hfCheck)
			this.hfCheck.value = this.getCheckCollection();
		
		if (args._postBackElement != null)
		{		    
			if (args._postBackElement.id == this.containerElement.id || args._postBackElement.id == this.tableElement.id || args._postBackElement.id == this.hfUpdate.id)
			{
			}
		}	
	},
	onEndReqestHandler: function (sender, args)
	{
	    this.hideModalGrid();
	},
	onScrollGrid: function ()
	{
		if (this._scrollTimerId)
			window.clearTimeout(this._scrollTimerId);
			
		this._scrollTimerId = window.setTimeout(Function.createDelegate(this, this.fillScrollInfo), 500);
	    //save client attributes to hidden field
	},
	onRowSelect: function(obj, rowIndex, r)
	{
	    this.__counter++;
	    //this.__tmp += this.__counter + ' onRowSelect \r\n';
	    
	    this._cancelRowSelect = true;
	    //alert('row select');
	    
	    // uncomment to change checkbox status on rowclick event
//	    var checkbox = document.getElementById('ecfGridCheckBox'+rowIndex);
//	    //this.__tmp += this.__counter + ' CHECKBOX=' + checkbox.checked +'\r\n';
//	    
//	    if (checkbox /*&& !this._firstSelected*/)
//	    {
//	        checkbox.checked = true; 	            	        
//	    }	    

//	    if (checkbox && checkbox.getAttribute("ecfparentblock") == 0)
//	    {
//			//var inputs = document.getElementsByTagName("INPUT");
//			var parentId = checkbox.getAttribute("ecfgridcheckboxid");
//			for (var i = this.inputsArr.length - 1; i >= 0; i--)
//			{
//				if (!this.inputsArr[i].getAttribute("ecfparentblock") && parentId == this.inputsArr[i].getAttribute("ecfgridcheckboxid"))
//				{
//					this.inputsArr[i].checked = checkbox.checked;
//				}
//			}
//	    }
	    
	    this._firstSelected = false;
	    //this.__tmp += this.__counter + ' after CHECKBOX=' + checkbox.checked +'\r\n';
	    //this.extGrid.fireEvent("click", null);
	},
	onRowDeselect: function(obj, rowIndex)
	{
	    var checkBox = document.getElementById("ecfGridCheckBox" + rowIndex);	    

	    this.__counter++;	    
	    //this.__tmp += this.__counter + ' onRowDeselect CHECKBOX=' + checkBox.checked + '\r\n';
	    
	    // uncomment to change checkbox status on rowclick event
//	    if (checkBox && !checkBox.getAttribute("ecfparentblock"))
//	    {
//			//var inputs = document.getElementsByTagName("INPUT");
//			var parentId = checkBox.getAttribute("ecfgridcheckboxid");
//			for (var i = this.inputsArr.length - 1; i >= 0; i--)
//			{
//				if (this.inputsArr[i].getAttribute("ecfparentblock") && parentId == this.inputsArr[i].getAttribute("ecfgridcheckboxid"))
//				{
//					this.inputsArr[i].checked = false;
//					return;
//				}
//			}

//	    }

//	    if (checkBox && checkBox.getAttribute("ecfparentblock") == 0 && checkBox.checked)
//	    {
//			
//			//var inputs = document.getElementsByTagName("INPUT");
//			var parentId = checkBox.getAttribute("ecfgridcheckboxid");
//			for (var i = this.inputsArr.length - 1; i >= 0; i--)
//			{
//				if (!this.inputsArr[i].getAttribute("ecfparentblock") && parentId == this.inputsArr[i].getAttribute("ecfgridcheckboxid"))
//				{
//					this.inputsArr[i].checked = false;
//				}
//			}
//			checkBox.checked = false;
//			//alert('unchecking parent');
//	    }
	},
	onBeforeRowSelect: function(obj, rowIndex, keepExisting)
	{
	    this.__counter++;	    
	    return (this._cancelRowSelect); // && !this._firstSelected);
	},
	onAfterUpdate: function (e)
	{
		this._updateArray[this._updateArray.length] = {primaryKeyId: e.record.data.primaryKeyId, column: e.column, value: e.value };
		//uncomment this lines for handling AfterUpdate event at server
		//Mediachase.UI.Web.IbnNext.WebServices.ListHandler.UpdateGrid(this.viewName, this._updateArray);
		this._timer = (new Date).getTime();
		this._updateArray = new Array();
	},
	onBeforeUpdate: function (e)
	{
		//raise on edit event			
		if (!this.onEdit(e.record.data.primaryKeyId, e.column, e.record.data.allowEdit))
		    return false;
		
		if (e.record.data.primaryKeyId.indexOf("-") > -1)
		{
			return false;
		}
		
		if (e.record.data.allowEdit.indexOf("true") == -1)
		{
			return false;
		}
						
		if (e.grid.selModel.selections.length > 0 && e.grid.selModel.selections.keys[0] != e.record.id)
		{
			return false;
		}
		
		if (!this.editEnable)
			return false;		
						    
		//this._timer = -1;
	},	
	onValidateEdit: function (e)
	{
		//return true;
		newMas = null;
		divider = null;
		for (i = 0; i < editorDivider.length; i++)
		{
			if (e.value.indexOf(editorDivider[i]) > -1)
			{
				newMas = e.value.split(editorDivider[i]);
				break;
			}
		}
		
		if (newMas == null)
		{
			newMas = e.value;
			
			while (newMas.length < 2)
			{
				newMas = '0'+newMas;
			}
			
			e.value = newMas+":00";
			return;
		}					
		
		if (parseInt(newMas[0]) >= 25)
		{
			e.cancel = true;
			return;
		}
		
		if (parseInt(newMas[0]) == 24 && parseInt(newMas[1]) != 0)
		{
			e.cancel = true;
			return;
		}
			
		hours = newMas[0];
		minutes = newMas[1];
		
		while (hours.length < 2)
		{
			hours = '0'+hours;
		}
		
		while (minutes.length < 2)
		{
			minutes = '0'+minutes;
		}
		
		e.value = hours+':'+minutes;
	},	
	onLayoutResize: function (obj)
	{
		this.extGrid.doLayout();
	},
	
    onEdit: function(primaryId, column, allowEdit) {
       var handler = this.editList.getHandler("mc_grid_onedit");
       if (handler) 
       {		   
           var retVal = handler(this, new Mediachase.GridOnEditEventArgs(primaryId, column, allowEdit));
           
           if (!retVal)
               return false;
       }
       else
       {
           return true;
       }
    },
    add_edit: function(handler)
    {
        if (this.editList)
		    this.editList.addHandler("mc_grid_onedit", handler);
    },
    remove_edit: function(handler)
    {
        if (this.editList != null)
    	    this.editList.removeHandler("mc_grid_onedit", handler);
    },	
    
    onCheckChange: function()
    {
        var handler = this.checkChange.getHandler("mc_grid_checkChange");
       if (handler) 
       {		   
           handler(this.extGrid.selModel, new Mediachase.GridOnCheckChangeArgs(this.getCheckCollection()));
       }   
    },
    add_checkChange: function(handler)
    {
        if (this.checkChange)
		    this.checkChange.addHandler("mc_grid_checkChange", handler);
    },
    remove_checkChange: function(handler)
    {
        if (this.checkChange != null)
    	    this.checkChange.removeHandler("mc_grid_checkChange", handler);
    },	  
    
    // ----- Column resize Event ------
    onGridColumnResize: function(columnIndex, newSize)
    {
       var handler = this.columnResizeList.getHandler("mc_grid_columnResize");
       if (handler) 
       {		   
           handler(this, new Mediachase.GridOnColumnResize(columnIndex, newSize));
       }   
    },
    add_columnResize: function(handler)
    {
        if (this.columnResizeList)
		    this.columnResizeList.addHandler("mc_grid_columnResize", handler);
    },
    remove_columnResize: function(handler)
    {
        if (this.columnResizeList != null)
    	    this.columnResizeList.removeHandler("mc_grid_columnResize", handler);
    },
    
    // ----- Column resize Event ------
    onGridHiddenChange: function(columnName, hidden)
    {
       var handler = this.hiddenChangeList.getHandler("mc_grid_hiddenChange");
       if (handler) 
       {		   
           handler(this, new Mediachase.GridOnHiddenChange(columnName, hidden));
       }   
    },
    add_hiddenChange: function(handler)
    {
        if (this.hiddenChangeList)
		    this.hiddenChangeList.addHandler("mc_grid_hiddenChange", handler);
    },
    remove_hiddenChange: function(handler)
    {
        if (this.hiddenChangeList != null)
    	    this.hiddenChangeList.removeHandler("mc_grid_hiddenChange", handler);
    },    
    
    // ----- Column order change Event ------
    onGridOrderChange: function(oldIndex, newIndex)
    {
       var handler = this.columnResizeList.getHandler("mc_grid_orderChange");
       if (handler) 
       {		   
           handler(this, new Mediachase.GridOnOrderChange(oldIndex, newIndex));
       }   
    },
    add_orderChange: function(handler)
    {
        if (this.columnResizeList)
		    this.columnResizeList.addHandler("mc_grid_orderChange", handler);
    },
    remove_orderChange: function(handler)
    {
        if (this.columnResizeList != null)
    	    this.columnResizeList.removeHandler("mc_grid_orderChange", handler);
    }          
    
}

//Mediachase.Grid.inheritsFrom(Sys.UI.Control);
Mediachase.GridEditor.registerClass("Mediachase.GridEditor", Sys.UI.Control);
/*table, config, data, colModel, colArray, container, hasPaging, showHeaderOnly,*/
/*this.get_tableElement(), null, this._data, this._colModel, this.get_colArr(), this._element, this._hasPaging, this.showOnlyHeader, */
Ext.grid.TableGrid = function(extJsGrid)
{
	var config = {};
	var cf = config.fields || [];
	var ch = config.columns || [];
	if (extJsGrid.get_tableElement() == null)	
		return;

	var tbl = Ext.get(extJsGrid.get_tableElement().id);
	var ct = tbl.insertSibling();
	
	var fields = [];
	var headers = tbl.query('tbody th');
	var index = -1;
	

    // TODO: DOCUMENT EACH FIELD
	for (var i = 0, h; h = headers[i]; i++) 
	{	    
	    // Skip the special fields?
		if (i == headers.length - 3) 
		{
			index = i;
			break;
		}
		
		
		var name = 'tcol-'+i;
		fields.push(Ext.applyIf(cf[i] || {}, {
			name: name,
			mapping: 'td:nth('+(i+1)+')/@innerHTML'
			}));
	}

	//add PrimaryKey 
	//index++;
	fields.push(Ext.applyIf(cf[index] || {}, {
		name: 'primaryKeyId',
		mapping: 'td:nth('+(index+1)+')/@innerHTML'
		}));
	
	//add RowCssClass
	index++;
	fields.push(Ext.applyIf(cf[index] || {}, {
		name: 'rowCssClass',
		mapping: 'td:nth('+(index+1)+')/@innerHTML'
		}));	
		
	//add AllowEdit column
	index++;
	fields.push(Ext.applyIf(cf[index] || {}, {
		name: 'allowEdit',
		mapping: 'td:nth('+(index+1)+')/@innerHTML'
		}));		
		
	//add CustomData column
	index++;
	fields.push(Ext.applyIf(cf[index] || {}, {
		name: 'customData'//,
		//mapping: 'td:nth('+(index)+')/@innerHTML'
		}));

	var data = new Ext.data.Store({ reader: new Ext.data.XmlReader({record:'tbody tr' }, fields)});
	data.loadData(tbl.dom);
	
	//Delete first row (headerRow)
	if (!extJsGrid.showOnlyHeader)
	{
		data.remove(data.getAt(0));
	}
	else
	{
		data.getAt(0).data.rowCssClass = "NoBorderRow";
	}
	
		
	if (extJsGrid._hasPaging)
	{
		data.remove(data.getAt(data.getTotalCount() - 2));
		//data.remove(data.getAt(data.getTotalCount() - 3));
	}
	
	data.commitChanges();
	var obj = Sys.Serialization.JavaScriptSerializer.deserialize(extJsGrid.get_colArr());
	var colModel = new Ext.grid.ColumnModel(obj);
	//alert(colModel.getColumnHeader(0));
	
	this.enableColumnMove = extJsGrid._allowClientDrag;
	this.enableDragDrop = extJsGrid._allowRowDragAndDrop;
	
	if (typeof document.body.style.maxHeight == "undefined")
	{	
		Ext.applyIf(this, {
			'ds': data,
			'cm': colModel,
			'sm': new Ext.grid.RowSelectionModel(),
			'width': extJsGrid._element.offsetWidth,
			bbar: new Ext.Panel(),
			viewConfig:
			{
				getRowClass : function (record, index) 			
				{
					if (record.data["rowCssClass"] != null)
					{
						return record.data["rowCssClass"];
					}
					else
					{
						return "x-grid-row";
					}
				}        
			}
			/*,
			layout: 'fit',
			viewConfig: {
				forceFit: true
			}*/
		});
    }
    else
    {
		Ext.applyIf(this, {
			'ds': data,
			'cm': colModel,
			'sm': new Ext.grid.RowSelectionModel(),
			bbar: new Ext.Panel(),
			viewConfig:
			{
				getRowClass : function (record, index) 			
				{
					if (record.data["rowCssClass"] != null)
					{
						return record.data["rowCssClass"];
					}
					else
					{
						return "x-grid-row";
					}
				}     
			}
		});    
    }
    Ext.grid.TableGrid.superclass.constructor.call(this, extJsGrid._element, {});	
}

//Ext.form.VTypes["timeReg"] = /^([0-2]?[0-9])[':',' ']([0-5]?[0-9])$|^((2[0-4])|([01]?[0-9]))$/i; //([0-2]?[0-9])[':',' ']([0-5]?[0-9])
//Ext.form.VTypes["timeReg"] = /^([0-2]?[0-9])[':',' ']+([' ']*)([0-5]?[0-9])$|^((2[0-4]([' ']*))|([01]?[0-9])([' ']*))$/i;

//}

var mcGridCheckbox_groupData = "";

//extJs grid checkbock cell renderer
function checkBoxRenderer(dataValue, cell, record, row, column, store)
{
    var retVal = "";
    var parentBlock = false;
    var primaryId = record.data.primaryKeyId;
        
    if (record.data.primaryKeyId.indexOf('-') > -1)
    {
		mcGridCheckbox_groupData = record.data.primaryKeyId;
		parentBlock = true;
    }
    else if (record.data.primaryKeyId == "0")
    {
		mcGridCheckbox_groupData = "";
    }
    
    if (record.data.rowCssClass.indexOf('TotalRow') > -1)
    {
        return retVal;
    }
    
    if (record.data.customData.indexOf('checked=true') > -1)
    {
        retVal = "<input id='" + this.id + "_" + row + "' type='checkbox' checked='true' ecfgridcheckbox=0 ecfgridcheckboxid='" + mcGridCheckbox_groupData + "'";
    }
    else
    {
        retVal = "<input id='" + this.id + "_" + row + "' type='checkbox' ecfgridcheckbox=0 ecfgridcheckboxid='" + mcGridCheckbox_groupData + "'";
    }
    
    if (parentBlock)
    {
		retVal += " ecfparentblock=0";
    }
    
    retVal += " primaryKeyId='" + primaryId + "' ";
    retVal += " />";
    
    return retVal;
}

function mcGridSelectAll(obj)
{
    var checkboxes = document.getElementsByTagName("INPUT");
    
    for (var i = 0; i < checkboxes.length; i++)
    {
        if (checkboxes[i].type == "checkbox" && checkboxes[i].getAttribute("ecfgridcheckbox"))
        {
            checkboxes[i].checked = obj.checked;
        }
    }

}


Ext.extend(Ext.grid.TableGrid, Ext.grid.EditorGridPanel);

Ext.enableGarbageCollector = false;
Ext.enableListenerCollection = true;

var editorDivider = [" ",":"];

function mcShowModal(obj)
{
	//slow method if obj is undefined
	if (!obj)
	{
		var allDivs = document.getElementsByTagName("DIV");
		
		if (allDivs.length == 0)
			return;
			
		for (var i = allDivs.length - 1; i >= 0; i--)
		{
			if (allDivs[i].control != null && allDivs[i].control != 'undefined' &&  typeof(allDivs[i].control.showModalGrid) == "function")
			{
				allDivs[i].control.showModalGrid();
				return;			
			}
		}
	}
	
	var objPanel = obj;
	while (obj.parentNode != null)
	{
		if (obj.nodeType == 1)
		{ 
			if (obj.tagName == "FORM")
				return;
			// try to find grid Div
			if (obj.tagName == "DIV" && obj.control != null && obj.control != 'undefined')
			{
				//if we find attached object to grid then success
				if (obj.control.extGrid != null && obj.control.extGrid != 'undefined')
				{
					obj.control.showModalGrid();
					return;
				}
			}
		}
			
		obj = obj.parentNode;
	}
}

function mcHideModal(obj)
{
	var objPanel = obj;
	while (obj.parentNode != null)
	{
		if (obj.nodeType == 1)
		{ 
			if (obj.tagName == "FORM")
				return;
			// try to find grid Div
			if (obj.tagName == "DIV" && obj.control != null && obj.control != 'undefined')
			{
				//if we find attached object to grid then success
				if (obj.control.extGrid != null && obj.control.extGrid != 'undefined')
				{
					obj.control.hideModalGrid();
					return;
				}
			}
		}
			
		obj = obj.parentNode;
	}
}


if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();