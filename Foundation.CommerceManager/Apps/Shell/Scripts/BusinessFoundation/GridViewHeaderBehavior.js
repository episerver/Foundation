Type.registerNamespace('Mediachase');


Mediachase.GridViewBaseRowEventArgs = function(rowIndex, primaryKeyId) {
    Mediachase.GridViewBaseRowEventArgs.initializeBase(this);
    this.RowIndex = rowIndex;
    this.PrimaryKeyId = primaryKeyId;
}

Mediachase.GridViewBaseRowEventArgs.prototype =
{
    get_RowIndex: function() {
        return this.RowIndex;
    },
    set_RowIndex: function(value) {
        this.RowIndex = value;
    },
    get_PrimaryKeyId: function() {
        return this.PrimaryKeyId;
    },
    set_PrimaryKeyId: function(value) {
        this.PrimaryKeyId = value;
    }
}

Mediachase.GridViewColumnResizeEventArgs = function(cellIndex, newSize) {
    Mediachase.GridViewColumnResizeEventArgs.initializeBase(this);
    this.CellIndex = cellIndex;
    this.NewSize = newIndex;
}

Mediachase.GridViewColumnResizeEventArgs.prototype =
{
    get_CellIndex: function() {
        return this.CellIndex;
    },
    set_CellIndex: function(value) {
        this.CellIndex = value;
    },
    get_NewSize: function() {
        return this.NewSize;
    },
    set_NewSize: function(value) {
        this.NewSize = value;
    }
}

Mediachase.GridViewColumnResizeEventArgs.registerClass("Mediachase.GridViewColumnResizeEventArgs", Sys.EventArgs);

Mediachase.GridViewHeaderBehavior = function(element) {
	/// <summary>
	/// The GridViewHeaderBehavior is used to fix a GridView control header and make the control scrollable
	/// </summary>
	/// <param name="element" type="Sys.UI.DomElement">
	/// The GridView element this behavior is associated with
	/// </param>
	Mediachase.GridViewHeaderBehavior.initializeBase(this, [element]);
	this.grid = null;
	this.divResizeProxy = null;
	this.gridStyleInfo = null;
	this.gridStyleInfoObj = null;
	this.columnsInfo = null; //JSON string from server about columns info
	this.columnsInfoList = null; // deserialized array about columns info
	this.isEmpty = null; // if true, then grid must be empty
	this.getCssFromColumn = null; // if true, get css class for row in last cell of the row
	this.dashboardMode = null; // if true, call function applyDashboard after render
	this.percentWidth = null;
	this.paddingWidth = null; // padding in grid container

	//header cell resize-config
	this.gridColumnResizeCssClass = null;
	this.gridFirstTr = null;
	this.gridTBody = null;
	this.isColumnResizing = null;
	this.startColumnResizingX = null; //start width 
	this.startColumnResizingLeft = null; // start left position of active element
	this.activeColumnResizing = null; //DOM element which is being resized
	this.minColumnWidth = null; // minimal column resize width    
	this.dWidth = null;

	//private variables
	this.debugMode = null;
	this.lastSelected = null;
	this.deltaColumnWidth = null; // autocalculated, shows the value = (actualWidth(width + paddings) - styleWidth)
	this.floatColumnWidth = null; // shows difference errors in calculating columns width (like 734.453px - the diffrence will be 0.453)

	this._layout = null; //store layout if found
	this._layoutHandler = null; // handler for layout

	// flags 
	this.autoHeaderHeight = null;
	this.autoBottomHeight = null;
	this.layoutDetected = null;
	this._isRendered = null;
	this.noRecords = null;

	this.headDiv = null;  //header div container
	this.bottomDiv = null;  //bottom div container     
	this.divWrapper = null;
	this.leftResizer = null; //display, when column being resized
	this.rightResizer = null;
	this.headerHeight = null;
	this.bottomHeight = null;
	this.absoluteDivRender = null;

	//event lists
	this.rowClickList = null;
	this.rowDblClickList = null;
	this.rowSelectList = null;
	this.columnResizeList = null;

	//const
	this.rowAttrName = null;
	this.elemResizerName = null;

	this.tmpDiv = null;
}

Mediachase.GridViewHeaderBehavior.prototype = {
    initialize: function() {
        /// <summary>
        /// Initialize the behavior
        /// </summary>
        Mediachase.GridViewHeaderBehavior.callBaseMethod(this, 'initialize');
        this.grid = this.get_element();
        this.rowAttrName = 'ibn_serverGridRow';
        var retVal = '';
        this.elemResizerName = 'ibn_elemresizer';
        this.minColumnWidth = 15;
        this.layoutDetected = false;
        this._isRendered = false;

        this.deltaColumnWidth = 7;
        this.floatColumnWidth = 3;

        if (document.all)
            this.floatColumnWidth = 1;

        //alert('grid init');
        //this.ShowGridModalPopup();

        this.rowClickList = new Sys.EventHandlerList();
        this.rowDblClickList = new Sys.EventHandlerList();
        this.rowSelectList = new Sys.EventHandlerList();
        this.columnResizeList = new Sys.EventHandlerList();

        if (!this.headerHeight || this.headerHeight == -1)
            this.autoHeaderHeight = true;
        else
            this.autoHeaderHeight = false;

        if (!this.bottomHeight || this.bottomHeight == -1)
            this.autoBottomHeight = true;
        else
            this.autoBottomHeight = false;

        this.debugMode = false;

        var _forms = document.getElementsByTagName('FORM');

        if (this.layoutResizeEnable) {
            if (_forms && _forms.length > 0)
                _forms[0].style.overflow = 'hidden';
        }

        this.columnsInfoList = Sys.Serialization.JavaScriptSerializer.deserialize(this.columnsInfo);

        if (typeof (this.gridStyleInfo) !== 'undefined' && this.gridStyleInfo != '') {
            this.gridStyleInfoObj = Sys.Serialization.JavaScriptSerializer.deserialize(this.gridStyleInfo);
        }

        this.isColumnResizing = false;
        //this.applyLayout();

        this.render();
        if (this.dashboardMode) {
            window.setTimeout(Function.createDelegate(this, this.afterRender), 1000);
        }
        else {
            //this.afterRender();
            this.afterRender();
        }

        if (!this.layoutDetected || !this.layoutResizeEnable) {
            $addHandler(window, "resize", Function.createDelegate(this, this.onWindowResize));
            window.setTimeout(Function.createDelegate(this, this.onWindowResize), 1000);
        }

        //		if (!this.layoutDetected || !this.layoutResizeEnable) {
        //			$addHandler(window, "resize", Function.createDelegate(this, this.onWindowResize));
        //			window.setTimeout(Function.createDelegate(this, this.onWindowResize), 500);
        //		}

        //        if (document.all && navigator.appVersion.indexOf('MSIE 7.0') > -1)
        //            window.setTimeout(Function.createDelegate(this, this.onWindowResize), 1000);
        //this.HideGridModalPopup();

    },

    dispose: function() {
        /// <summary>
        /// Dispose the behavior
        /// </summary>
        var element = this.get_element();

        if (this.grid && this.grid.parentNode)
            $clearHandlers(this.grid.parentNode);

        if (this.grid)
            $clearHandlers(this.grid);

        if (this._layout && this._layoutHandler)
            this._layout.remove_resize(this._layoutHandler);

        //to FIX IT!
        $clearHandlers(document.body);

        Mediachase.GridViewHeaderBehavior.callBaseMethod(this, 'dispose');
    },

    //some special methods, when grid at dashboard(like supporting expand/collapse and resize on drop)
    applyDashboard: function() {
        this.debugMsg('applyDashboard called');
        var startPoint = this.divResizeProxy.parentNode;

        if (Ibn.DDLayoutExtender && Ibn.DDLayoutExtender.getCurrent) {
            this.debugMsg('call static Ibn.DDLayoutExtender.getCurrent');
            if (Ibn.DDLayoutExtender.getCurrent() == null) {
                window.setTimeout(Function.createDelegate(this, this.applyDashboard), 100);
            }
            else {
                Ibn.DDLayoutExtender.getCurrent().add_drop(Function.createDelegate(this, this.ddLayoutHandler));
                Ibn.DDLayoutExtender.getCurrent().add_expand(Function.createDelegate(this, this.ddLayoutExpand));
            }
        }

        while (startPoint.parentNode != null) {
            if (startPoint.nodeType == 1 && startPoint.tagName == "BODY")
                return;

            if (startPoint.nodeType == 1 && startPoint.tagName == "DIV")
                startPoint.style.position = 'relative';

            startPoint = startPoint.parentNode;
        }
    },

    autoSize: function() {
        var startPoint = this.grid;
        //this.recalculateHeader();
        while (startPoint.parentNode != null) {
            if (startPoint.nodeType == 1 && startPoint.tagName == "FORM") {
                this.debugMsg('dynamic layout failed initializing');
                if (!this.dashboardMode && !this.layoutResizeEnable) {
                    if (this.divResizeProxy.parentNode.offsetHeight - this.divResizeProxy.offsetTop > 0)
                        this.divResizeProxy.style.height = this.divResizeProxy.parentNode.offsetHeight - this.divResizeProxy.offsetTop + 'px';
                } else if (!this.dashboardMode) {
                    this.divResizeProxy.style.height = document.body.offsetHeight - this.divResizeProxy.offsetTop + 'px';
                }

                return;
            }

            if (startPoint != "undefined" && startPoint.control != null && startPoint.control.resizeList != null) {
                this.debugMsg('dynamic layout attached');
                this._layout = startPoint.control;
                this._layoutHandler = Function.createDelegate(this, this.layoutHandler);
                this.layoutDetected = true;
                startPoint.control.add_resize(this._layoutHandler);
                return;
            }
            startPoint = startPoint.parentNode;
        }
    },

    layoutHandler: function(sender, args) {
        if (!this.layoutResizeEnable)
            return;
        if (args && this.grid) {
            var newGridHeight = args._height;
            var newGridWidth = args._width;

            if (this.ie6OrLess) {
                newGridWidth -= 14;
                newGridHeight -= 50;
            }

            if (this.grid != null) {
                this.setHeight(newGridHeight);
                this.setWidth(newGridWidth);
            }
        }
    },

    //onDrop event handler(works only for dashboard)
    ddLayoutHandler: function(sender, node) {
        //TODO: check node, if it apply for grid then perform resize
        if (this.divResizeProxy && this.divResizeProxy.parentNode && this.divResizeProxy.offsetWidth != this.divResizeProxy.parentNode.offsetWidth) {
            this.onWindowResize(null);
            //this.divResizeProxy.style.width = this.divResizeProxy.parentNode.offsetWidth + 'px';			
            if (this.percentWidth) {
                this.recalculateColumnsPercent(0);
                //this.grid.style.width = parseInt(this.grid.style.width) - 2 + 'px';
            }

            this.onWindowResize(null);
        }
    },

    //onExpand event handler (works only for dashboard)
    ddLayoutExpand: function(sender, node) {
        //mc_checkParent(node.el.dom)
        //		if (mc_checkParent(node.el.dom, this.grid) && this.divResizeProxy.offsetHeight < 32)
        //		{
        //			window.setTimeout(Function.createDelegate(this, this.onWindowResize), 250);
        //			if (this.percentWidth)
        //			{
        //				window.setTimeout(Function.createDelegate(this, this.recalculateColumnsPercent), 275);
        //				window.setTimeout(Function.createDelegate(this, this.onWindowResize), 300);
        //window.setTimeout(Function.createDelegate(this, this.ddLayoutHandler), 400);
        //			}
        //		}

        this.ddLayoutHandler(this, null);
        if (this.headDiv && this.headDiv.firstChild && !isNaN(parseInt(this.headDiv.firstChild.style.width))) {
            this.headDiv.firstChild.style.width = parseInt(this.headDiv.firstChild.style.width) * 3 + 'px';
        }
    },

    //window resize handler
    onWindowResize: function(e) {
        if (this.divResizeProxy.parentNode != null) {
            //2 == border-left(1px) + border-right(1px)
            if (this.divResizeProxy.parentNode.offsetWidth >= 0 && this.layoutResizeEnable === true) {
                this.divResizeProxy.style.width = this.divResizeProxy.parentNode.offsetWidth - 1 + 'px';
            }
            else if (document.body.clientWidth - this.divResizeProxy.offsetLeft - 7 > 0) {
            
				var curNode = this.divResizeProxy;
				var isPageWithScroll = false;
				var deltaWidth = 6;
				while (curNode != null)
				{
					if (curNode.nodeType == 1 && curNode.tagName == "BODY")
						break;
					
					if (curNode.scrollHeight > curNode.offsetHeight)
					{
						isPageWithScroll = true;
					}
					
					curNode = curNode.parentNode;
				}
				
				if (isPageWithScroll === true)
					deltaWidth += 15;
            
                if (document.all)
                    this.divResizeProxy.style.width = document.body.clientWidth - this.divResizeProxy.offsetLeft - deltaWidth + 'px';
                else
                    this.divResizeProxy.style.width = document.body.clientWidth - this.divResizeProxy.offsetLeft - deltaWidth - 1 + 'px';
            }

            if (this._layoutHandler == null && !this.dashboardMode && !this.layoutResizeEnable) {
                //fix dla Nadi
                this.divResizeProxy.style.height = this.get_element().offsetHeight + this.bottomDiv.offsetHeight + this.headDiv.offsetHeight + 16 + 'px'; //16 - eto zapas dla horizontal scrolla
            } else if (this._layoutHandler == null && !this.dashboardMode) {
                this.divResizeProxy.style.height = document.body.offsetHeight - this.divResizeProxy.offsetTop - 5 + 'px';
            }
            else {
                var elem = typeof (typeof(this.get_element) == "function") ? this.get_element() : this.grid;
                this.divResizeProxy.style.height = (elem != null ? elem.offsetHeight : 0) + this.bottomDiv.offsetHeight + this.headDiv.offsetHeight + 16 + 'px'; //16 - eto zapas dla horizontal scrolla
                // versija esli nado grid na ves content raszhimat this.layoutResizeHandler == false
                //                if (!this.layoutResizeEnable)
                //                    this.divResizeProxy.style.height = document.body.offsetHeight - this.divResizeProxy.offsetTop - 4 + 'px'; //this.get_element().offsetHeight + this.bottomDiv.offsetHeight + this.headDiv.offsetHeight - this.get_element().offsetTop + 17 + 'px'; //16 - eto zapas dla horizontal scrolla
                //                else
                //                    this.divResizeProxy.style.height = this.get_element().offsetHeight + this.bottomDiv.offsetHeight + this.headDiv.offsetHeight + 16 + 'px'; //16 - eto zapas dla horizontal scrolla
                //            }
            }
            this.divWrapper.style.top = this.headDiv.offsetHeight + 'px';

            //			if (parseInt(this.divResizeProxy.style.height) - (this.bottomDiv.offsetHeight + this.headDiv.offsetHeight + 16 ) >= 0)
            //				this.divWrapper.style.height = parseInt(this.divResizeProxy.style.height) - (this.bottomDiv.offsetHeight + this.headDiv.offsetHeight + 16 ) + 'px';

            //			if (this.percentWidth && e != null)
            //			{
            //				this.recalculateColumnsPercent(0);
            //				this.grid.style.width = parseInt(this.grid.style.width) - 1 + 'px';
            //			}
        }
    },

    //makes dom elem - unselectable
    setUnselectable: function(elem) {
        if (elem) {
            if (typeof (elem.style.MozUserSelect) != 'undefined') {
                elem.style.MozUserSelect = 'none';
            }
            else {
                elem.setAttribute('unselectable', 'on');
            }
        }
    },

    //visibility: 'hidden', for last cell
    hideLastColumn: function() {
        for (var i = 0; i < this.gridTBody.rows.length; i++) {
            var lastCell = this.gridTBody.rows[i].cells[this.gridTBody.rows[i].cells.length - 1];
            lastCell.style.visibility = 'hidden';
        }
    },

    // util method for web service
    convertPixelToPercent: function(width) {
        if (this.grid.offsetWidth - this.countSystemColumnsWidth() > 0)
            return (width / (this.grid.offsetWidth - this.countSystemColumnsWidth()));

        return 0;
    },

    // generate from <tr> <th>1</th> </tr> -> <div tr> <div.th>1</div> </div>
    createHeaderRow: function(headRow) {
        var headElemArray = null;

        if (!document.all)
            headElemArray = headRow.cells;

        //if (headElemArray == null && typeof(headElemArray[0]) == 'undefined')
        if (headElemArray == null)
            headElemArray = headRow.childNodes;

        var newHeaderDiv = document.createElement('DIV');
        this.setUnselectable(newHeaderDiv);
        newHeaderDiv.id = this.grid.id + '_newHeaderDiv';
        newHeaderDiv.style.width = this.grid.style.width;
        newHeaderDiv.style.height = this.headerHeight + 'px';
        newHeaderDiv.style.position = 'relative';

        if (this.gridStyleInfoObj != null)
            this.headDiv.className = this.gridStyleInfoObj.HeaderCssClass;

        for (var i = 0; i < headElemArray.length; i++) {
            if (headElemArray[i].nodeType == 1) {
                var newDiv = document.createElement('DIV');
                var newInnerDiv = document.createElement('DIV');

                newDiv.style.height = this.headerHeight + 'px';
                if (typeof (this.columnsInfoList[i]) != 'undefined') {
                    newDiv.style.width = parseInt(this.columnsInfoList[i].Width) + 'px';
                }
                else {
                    newDiv.style.width = '0px';
                }

                if (newDiv.style.width == '0px')
                    continue;

                newInnerDiv.innerHTML = headElemArray[i].innerHTML;

                this.setUnselectable(newDiv);
                this.setUnselectable(newInnerDiv);

                if (this.gridStyleInfoObj != null)
                    newDiv.className = this.gridStyleInfoObj.HeaderInnerCssClass;

                newInnerDiv.className = 'serverGridHeaderInnerNoPadding';
                newDiv.appendChild(newInnerDiv);
                newHeaderDiv.appendChild(newDiv);
            }
        }

        if (newHeaderDiv.childNodes.length != 0) {
            newHeaderDiv.childNodes[newHeaderDiv.childNodes.length - 1].style.borderRight = '1px solid #ededed';

            if (document.all) {
                newHeaderDiv.childNodes[newHeaderDiv.childNodes.length - 1].style.width = parseInt(newHeaderDiv.childNodes[newHeaderDiv.childNodes.length - 1].style.width) + 1 + 'px';
            }
            else {
                newHeaderDiv.childNodes[newHeaderDiv.childNodes.length - 1].style.width = parseInt(newHeaderDiv.childNodes[newHeaderDiv.childNodes.length - 1].style.width) + 1 + 'px';
            }
        }

        this.headDiv.appendChild(newHeaderDiv);
    },

    //create div's for resize in header
    createHeaderResizer: function(headRow) {
        var newHeaderDiv = this.headDiv.firstChild;
        var len = newHeaderDiv.childNodes.length;
        for (var i = 0; i < len; i++) {
            //header cell resize element
            var headerResizer = document.createElement('DIV');
            this.setUnselectable(headerResizer);

            headerResizer.id = this.grid.id + '_headerResizer';

            if (this.columnsInfoList && this.columnsInfoList[i].Resizable)
                headerResizer.style.cursor = 'e-resize';

            headerResizer.style.position = 'absolute';
            //headerResizer.style.height = newHeaderDiv.childNodes[i].offsetHeight + 'px';
            headerResizer.style.bottom = '0px';
            headerResizer.style.width = '5px';
            headerResizer.style.top = '0px';
            headerResizer.style.backgroundColor = 'Transparent'; //'Red';

            if (this.debugMode)
                headerResizer.style.backgroundColor = 'Red';

            headerResizer.style.left = newHeaderDiv.childNodes[i].offsetLeft + newHeaderDiv.childNodes[i].offsetWidth - 2 + 'px'; //parseInt(headRow.childNodes[i].offsetWidth) +  parseInt(headRow.childNodes[i].style.width) + 'px';			
            headerResizer.setAttribute(this.elemResizerName, i);

            newHeaderDiv.appendChild(headerResizer);
        }

        //$addHandler(document.body, "mousedown", Function.createDelegate(this, this._columnResizeMouseDown));
        $addHandler(this.divResizeProxy, "mousedown", Function.createDelegate(this, this._columnResizeMouseDown));
        $addHandler(document.body, "mousemove", Function.createDelegate(this, this._columnResizeMouseMove));
        $addHandler(document.body, "mouseup", Function.createDelegate(this, this._columnResizeMouseUp));
    },

    recalculateHeader: function() {
        var newHeaderDiv = this.headDiv.firstChild;
        newHeaderDiv.style.width = this.divResizeProxy.style.width;
        if (this.noRecords)
            this.grid.style.width = '';

        var len = newHeaderDiv.childNodes.length;
        for (var i = 0; i < len; i++) {
            if (newHeaderDiv.childNodes[i].id && newHeaderDiv.childNodes[i].id.indexOf('_headerResizer') > 0) {
                //compare width for basic columns (first len /2 basic, after i/2 + 1 headerResizers)
                newHeaderDiv.childNodes[i].style.left = newHeaderDiv.childNodes[parseInt(i - (len / 2))].offsetLeft + newHeaderDiv.childNodes[parseInt(i - (len / 2))].offsetWidth - 2 + 'px';
                newHeaderDiv.childNodes[i].style.bottom = '0px';
            }
        }
    },

    createBottomRow: function(bottomRow) {
        this.bottomDiv.style.width = '100%';
        this.bottomDiv.style.height = this.bottomHeight + 'px';

        if (this.gridStyleInfoObj != null)
            this.bottomDiv.className = this.gridStyleInfoObj.FooterCssClass;

        var innerTds = bottomRow.getElementsByTagName('TD');
        if (innerTds && typeof (innerTds.length) != 'undefined') {
            var innerTd = innerTds[0];
            if (this.dashboardMode) {
                this.bottomDiv.appendChild(innerTd.firstChild);
            }
            else {
                var innerDiv = innerTd.getElementsByTagName('DIV');
                if (innerDiv && innerDiv.length > 0)
                    this.bottomDiv.appendChild(innerDiv[0]);
            }
        }

    },

    //for IE only, makes all dom element hasLayout == true
    applyLayout: function() {
        if (document.all) {
            var arr = document.all;
            for (var i = 0; i < arr.length; i++) {
                arr[i].style.hasLayout = true;
                arr[i].style.zoom = 1;
            }
        }
    },

    // create header row(from first grid row), create bottom row(from last row)
    render: function() {
        var grid = this.get_element();
        if (grid != 'undefined') {
            var divWrapper = grid.parentNode;
            if (!mc_checkVisibility(divWrapper))
                return;

            if (this.dashboardMode) {
                var tmpDiv = document.createElement("DIV");
                var tmpDivWindow = document.createElement("DIV");
                tmpDiv.id = 'temp_renderer_div';
                tmpDivWindow.id = 'temp_renderer_div2';
                this.absoluteDivRender = document.createElement("DIV");
                this.absoluteDivRender.style.position = 'absolute';
                this.absoluteDivRender.style.height = '1px';
                this.absoluteDivRender.style.width = '1px';
                this.absoluteDivRender.style.overflow = 'scroll';

                tmpDiv.style.width = this.divResizeProxy.parentNode.clientWidth + 'px';
                tmpDiv.style.height = this.divResizeProxy.parentNode.clientHeight + 'px';

                tmpDivWindow.style.width = tmpDiv.style.width;
                tmpDivWindow.style.height = tmpDiv.style.height;

                this.divResizeProxy.parentNode.appendChild(this.absoluteDivRender);
                this.divResizeProxy.parentNode.appendChild(tmpDivWindow);

                this.absoluteDivRender.appendChild(tmpDiv);

                tmpDiv.appendChild(this.divResizeProxy);
            }

            grid.style.visibility = 'hidden';
            grid.style.tableLayout = 'fixed';
            //grid.style.borderCollapse = 'separate';
            grid.style.borderRight = '1px solid #ededed';
            //grid.style.borderSpacing = '0px';         

            var tags = grid.getElementsByTagName('TBODY');

            this.debugMsg('GridViewHeaderBehavior.render: TBODY.length = ' + tags.length);

            if (tags != 'undefined') {
                this._isRendered = true;
                var tbody = tags[0];
                var trs = new Array();
                var tmpArr = tbody.getElementsByTagName('TR');

                for (var i = 0; i < tmpArr.length; i++) {
                    if (tmpArr[i].parentNode == tbody)
                        trs.push(tmpArr[i]);
                }

                if (this.autoHeaderHeight == true)
                    this.headerHeight = 0;
                if (this.autoBottomHeight == true)
                    this.bottomHeight = 4;

                if (trs != 'undefined') {
                    //if grid has no records
                    if (trs.length == 1) {
                        grid.style.visibility = 'visible';
                        return;
                    }

                    this.noRecords = trs.length <= 3;

                    if (this.autoHeaderHeight == true)
                        this.headerHeight += trs[0].offsetHeight;
                    if (this.autoBottomHeight == true)
                        this.bottomHeight += trs[trs.length - 1].offsetHeight;

                    var bottomTR = tbody.removeChild(trs[trs.length - 1]);
                    var headTR = tbody.removeChild(trs[0]);
                    this.gridFirstTr = headTR;

                    this.headDiv = document.createElement('DIV');
                    var headTmp = document.createElement('DIV');
                    var headTable = document.createElement('TABLE');

                    this.headDiv.style.position = 'absolute';
                    this.headDiv.style.height = this.headerHeight + 'px';
                    this.headDiv.style.width = '100%';
                    this.headDiv.style.overflowX = 'hidden';
                    this.headDiv.style.overflowY = 'hidden';
                    this.headDiv.style.top = '0px';
                    this.headDiv.style.left = '0px';

                    this.createHeaderRow(headTR);
                    $addHandler(divWrapper, "scroll", Function.createDelegate(this, this.headScrollHandler));
                    this.divResizeProxy.insertBefore(this.headDiv, this.divResizeProxy.firstChild);
                    this.divResizeProxy.style.left = '0px';

                    var bottomDiv = document.createElement('DIV');
                    bottomDiv.style.position = 'absolute';
                    bottomDiv.style.height = this.bottomHeight + 'px';
                    bottomDiv.style.bottom = '0px';
                    bottomDiv.style.width = '100%';
                    bottomDiv.style.left = '0px';

                    this.bottomDiv = bottomDiv;
                    this.createBottomRow(bottomTR);
                    this.divResizeProxy.appendChild(this.bottomDiv);

                    for (var i = trs.length - 1; i >= 0; i--) {
                        trs[i].setAttribute(this.rowAttrName, 1);
                    }
                }

                this.gridTBody = tbody;
            }

            if (this.gridStyleInfoObj != null)
                divWrapper.className = this.gridStyleInfoObj.GridCssClass;

            divWrapper.style.position = 'absolute';
            divWrapper.style.top = this.headerHeight + 'px';
            divWrapper.style.bottom = this.bottomHeight + 'px';
            divWrapper.style.width = '100%';
            divWrapper.style.left = '0px';
            this.divWrapper = divWrapper;

            this.divResizeProxy.style.position = 'relative';
            this.divResizeProxy.style.height = '100%';
            this.divResizeProxy.style.width = '100%';
            this.divResizeProxy.style.border = 'solid 1px #8EABD8';
            this.divResizeProxy.appendChild(divWrapper);

            this.leftResizer = document.createElement('DIV');
            this.leftResizer.id = this.grid.id + '_leftResizer';
            this.leftResizer.style.position = 'absolute';
            this.leftResizer.style.display = 'none';
            this.leftResizer.style.top = '0px';
            this.leftResizer.style.bottom = this.bottomHeight + 'px';
            this.leftResizer.style.width = '1px';
            this.leftResizer.style.backgroundColor = '#666666';
            this.divResizeProxy.appendChild(this.leftResizer);

            this.rightResizer = document.createElement('DIV');
            this.rightResizer.id = this.grid.id + '_rightResizer';
            this.rightResizer.style.position = 'absolute';
            this.rightResizer.style.display = 'none';
            this.rightResizer.style.top = '0px';
            this.rightResizer.style.bottom = this.bottomHeight + 'px';
            this.rightResizer.style.width = '1px';
            this.rightResizer.style.backgroundColor = '#666666';
            this.divResizeProxy.appendChild(this.rightResizer);

            grid.style.width = this.calculateTotalWidth() + 'px';

            //grid.style.visibility = 'visible';

            //if custom styles has border-left/border-right
            if (this.divResizeProxy.clientWidth - (this.headDiv.offsetWidth - this.headDiv.clientWidth) - 1 >= 0) {
                this.divResizeProxy.style.width = this.divResizeProxy.clientWidth - (this.headDiv.offsetWidth - this.headDiv.clientWidth) - 1 + 'px';

            }

            this.headDiv.firstChild.style.width = this.grid.offsetWidth + 18 + 'px';

            $addHandler(this.grid, "click", Function.createDelegate(this, this.onClickGeneralHandler));
            $addHandler(this.grid, "dblclick", Function.createDelegate(this, this.onDblClickGeneralHandler));

            if (this.divResizeProxy.offsetHeight == 0)
                this.divResizeProxy.style.height = '125px';
        }
    },

    //perform recalculate cells with percent width
    afterRender: function() {
        if (this._isRendered) {
            if (this.dashboardMode)
                this.applyDashboard();

            if (this.gridTBody != null && this.gridTBody.rows.length > 0)
                this.performCssClass();

            this.autoSize();

            if (this.gridFirstTr)
                this.createHeaderResizer(this.gridFirstTr);

            if (this.percentWidth) {
                this.recalculateColumnsPercent(0);

                if (parseInt(this.grid.style.width) - 1 >= 0)
                    this.grid.style.width = parseInt(this.grid.style.width) - 1 + 'px';
            }

            //			if (this.dashboardMode)
            //				this.onWindowResize(null);

            this.grid.style.visibility = 'visible';

            if (this.getCssFromColumn)
                this.hideLastColumn();

            if (this.dashboardMode && this.absoluteDivRender && this.absoluteDivRender.parentNode) {
                var divToRemove = document.getElementById('temp_renderer_div2');

                if (divToRemove)
                    this.absoluteDivRender.parentNode.removeChild(divToRemove);

                this.absoluteDivRender.parentNode.appendChild(this.divResizeProxy);
                this.absoluteDivRender.parentNode.removeChild(this.absoluteDivRender);

                this.ddLayoutHandler(this, null);
            }

        }

        //window.status = 'this.divResizeProxy = ' + this.divResizeProxy.offsetWidth + ' | ' + this.divResizeProxy.parentNode.offsetWidth + 'px';

    },

    //recalculate all system columns with percent width
    recalculateColumnsPercent: function(delta) {
        if (delta == null || typeof (delta) == 'undefined')
            delta = 0;

        if (this.percentWidth) {
            for (var i = 0; i < this.columnsInfoList.length; i++) {
                if (!this.columnsInfoList[i].IsSystem) {
                    var newColumnWidth = (this.columnsInfoList[i].Width / 100) * (this.divResizeProxy.clientWidth - this.countSystemColumnsWidth() + delta);

                    this.floatColumnWidth += newColumnWidth - parseInt(newColumnWidth);

                    if (i == this.columnsInfoList.length - 1)
                        newColumnWidth += Math.round(this.floatColumnWidth);

                    this.changeColumnWidth(i, newColumnWidth);
                }
            }
            //this.grid.style.width = parseInt(this.grid.style.width) - 1 + 'px';
        }

        //reset values		
        this.floatColumnWidth = 2;

        if (document.all)
            this.floatColumnWidth = 0;
    },

    calculateTotalWidth: function() {
        var retVal = 0;

        for (var i = 0; i < this.columnsInfoList.length; i++) {
            retVal += this.headDiv.firstChild.childNodes[i].offsetWidth;
        }

        return retVal;
    },

    performCssClass: function() {
        if (!this.isEmpty && !this.getCssFromColumn)
            return;

        for (var i = 0; i < this.gridTBody.rows.length; i++) {
            if (this.isEmpty) {
                //this.gridTBody.rows[i].style.display = 'none';
                this.gridTBody.rows[i].style.visibility = 'hidden';
            }
            else {
                if (this.getCssFromColumn)
                    this.gridTBody.rows[i].className = this.gridTBody.rows[i].cells[this.gridTBody.rows[i].cells.length - 1].innerHTML;

                //fix red points in IE
                this.gridTBody.rows[i].cells[this.gridTBody.rows[i].cells.length - 1].style.display = 'none';
            }
        }
    },

    ShowGridModalPopup: function() {
        var modalDiv = document.createElement("DIV");

        modalDiv.style.position = 'absolute';
        modalDiv.style.left = '0px';
        modalDiv.style.top = '0px';
        modalDiv.style.bottom = '0px';
        modalDiv.style.right = '0px';
        modalDiv.style.backgroundColor = 'White';
        modalDiv.setAttribute('mcModalDiv', '1');

        this.divResizeProxy.appendChild(modalDiv);
    },

    HideGridModalPopup: function() {
        for (var i = 0; i < this.divResizeProxy.childNodes.lenght; i++) {
            if (this.divResizeProxy.childNodes[i].getAttribute('mcModalDiv')) {
                this.divResizeProxy.removeChild(this.divResizeProxy.childNodes[i]);
                return;
            }
        }
    },

    headScrollHandler: function(e) {
        if (this.headDiv) {
            this.headDiv.scrollLeft = e.target.scrollLeft;
        }
    },

    //--- methods for work with checked collections ---

    //get id of checked elements split with ';'
    getCheckedCollection: function() {
        var check = this.grid.getElementsByTagName('input');
        var retVal = '';

        for (var i = 0; i < check.length; i++) {
            if (check[i].getAttribute('ibn_serverGridCheckbox') && check[i].getAttribute('ibn_serverGridCheckbox') == '1' && check[i].checked) {
                if (check[i].getAttribute('ibn_primarykeyid'))
                    retVal += check[i].getAttribute('ibn_primarykeyid') + ';';
                else if (check[i].parentNode.getAttribute('ibn_primarykeyid'))
                    retVal += check[i].parentNode.getAttribute('ibn_primarykeyid') + ';';
            }
        }

        return retVal;
    },

    //returns true if any element is checked, otherwise false
    isChecked: function() {
        return this.getCheckedCollection().length > 0;
    },

    //return true if checkboxes are shown, otherwise false
    isCheckboxes: function() {
        var check = this.grid.getElementsByTagName('input');
        if (check.length == 0)
            return false;

        for (var i = 0; i < check.length; i++) {
            if (check[i].getAttribute('ibn_serverGridCheckbox') && check[i].getAttribute('ibn_serverGridCheckbox') == '1')
                return true;
        }

        return false;
    },

    getSelectedElement: function() {
        if (this.lastSelected && this.lastSelected.getAttribute('ibn_primaryKeyId'))
            return this.lastSelected.getAttribute('ibn_primaryKeyId');
        else
            return '';
    },

    setHeight: function(newHeight) {
        if (this.grid != null && newHeight >= 0) {
            if (this.headDiv != null && this.bottomDiv != null) {
                //if custom styles has border-left/border-right	
                this.headDiv.style.height = this.headerHeight - (this.headDiv.offsetHeight - this.headDiv.clientHeight) + 'px'; //this.headDiv.offsetHeight + 'px';
                this.divWrapper.style.top = parseInt(this.headDiv.style.height) + (this.headDiv.offsetHeight - this.headDiv.clientHeight) + 'px'; //this.headDiv.offsetHeight + 'px';
                this.divResizeProxy.style.height = newHeight - (this.headDiv.offsetHeight - this.headDiv.clientHeight) - (this.bottomDiv.offsetHeight - this.bottomDiv.clientHeight) + 'px';
            }
            else {
                if (newHeight - 2 >= 0)
                    this.grid.style.height = newHeight - 2 + 'px';
            }
        }
    },

    setWidth: function(newWidth) {
        if (this.grid != null && newWidth >= 0) {
            if (this.headDiv != null && this.bottomDiv != null) {
                //if custom styles has border-left/border-right
                if (newWidth - this.paddingWidth - (this.headDiv.offsetWidth - this.headDiv.clientWidth) - 1 >= 0)
                    this.divResizeProxy.style.width = newWidth - this.paddingWidth - (this.headDiv.offsetWidth - this.headDiv.clientWidth) - 1 + 'px';
            }
            else {
                if (newWidth - this.paddingWidth /*- 2*/ >= 0)
                    this.grid.style.width = newWidth - this.paddingWidth /*- 2*/ + 'px';
            }
        }
    },

    onClickGeneralHandler: function(e) {
        if (!e)
            e = event;

        var row = this._getGridRow(e.target);

        if (row == null)
            return;

        //raise onRowClick event
        this.onRowClick(row.rowIndex, row.getAttribute('ibn_primaryKeyId'));

        if (!(this.lastSelected && this.lastSelected.getAttribute('ibn_primaryKeyId') == row.getAttribute('ibn_primaryKeyId'))) {
            //if lastSelected != currentClickedRow, then raise onRowSelect event

            if (this.lastSelected) {
                //remove old selected items css class
                this.lastSelected.className = this.lastSelected.className.replace(this.gridStyleInfoObj.GridSelectedRowCssClass, '');
            }

            this.lastSelected = row;
            this.lastSelected.className += ' ' + this.gridStyleInfoObj.GridSelectedRowCssClass;
            this.onRowSelect(row.rowIndex, row.getAttribute('ibn_primaryKeyId'));
        }

    },

    onDblClickGeneralHandler: function(e) {
        if (!e)
            e = event;

        var row = this._getGridRow(e.target);

        if (row == null)
            return;

        this.onDblClick(row.rowIndex, row.getAttribute('ibn_primaryKeyId'));
    },

    _getGridRow: function(objRow) {
        if (objRow != null & typeof (objRow) != 'undefined') {
            while (objRow && objRow.parentNode != null) {
                if (objRow.nodeType == 1 && objRow.tagName == 'FORM')
                    return null;

                if (objRow.getAttribute(this.rowAttrName) == '1') {
                    return objRow;
                }

                objRow = objRow.parentNode;
            }
        }

        return null;
    },

    debugMsg: function(msg) {
        if (this.debugMode) {
            alert(msg);
        }
    },

    // ----- column resize methods -----
    changeColumnWidth: function(index, newSize) {
        this.activeColumnResizing = this.headDiv.firstChild.childNodes[this.columnsInfoList.length + index];
        //alert('changeColumnWidth: ' + newSize);
        //if (this.showCh

        this.activeColumnResizing.style.left = parseInt(this.activeColumnResizing.style.left) + parseInt(newSize) - this.headDiv.firstChild.childNodes[index].offsetWidth + 'px';
        this.performColumnResize(parseInt(newSize) - this.headDiv.firstChild.childNodes[index].offsetWidth);

        //		if (index == this.columnsInfoList.length - 1)
        //		{
        //			if (document.all)
        //				this.performColumnResize(-2);	
        //			else
        //				this.performColumnResize(-1);
        //		}
    },

    performColumnResize: function(dWidth) {
        //resize and calculate all new width after column reize
        dWidth = parseInt(dWidth);

        if (parseInt(this.grid.style.width) + dWidth <= 0 || parseInt(this.headDiv.firstChild.style.width) + dWidth < 0)
            return;
        //change width of header column
        this.headDiv.firstChild.style.width = parseInt(this.headDiv.firstChild.style.width) + dWidth + 'px';
        //change width for table
        this.grid.style.width = parseInt(this.grid.style.width) + dWidth + 'px';

        //get Index of column which was resizing
        var indexColumn = parseInt(this.activeColumnResizing.getAttribute(this.elemResizerName));
        var objColumn = this.headDiv.firstChild.childNodes[indexColumn];
        var objColumnResizer = this.activeColumnResizing;

        if (parseInt(objColumn.style.width) + dWidth >= 0)
            objColumn.style.width = parseInt(objColumn.style.width) + dWidth + 'px';

        //old fast version (little problems in IE)
        //this.gridTBody.rows[0].cells[indexColumn].style.width = parseInt(this.gridTBody.rows[0].cells[indexColumn].style.width) + dWidth + 'px';		
        for (var i = 0; i < this.gridTBody.rows.length; i++) {
            //var sum = parseInt(this.gridTBody.rows[i].cells[indexColumn].style.width) + dWidth;
            //alert('before width: ' + parseInt(this.gridTBody.rows[i].cells[indexColumn].style.width) + ' dwidth = ' + dWidth + ' |  sum = ' + sum);
            if (parseInt(this.gridTBody.rows[i].cells[indexColumn].style.width) + dWidth >= 0)
                this.gridTBody.rows[i].cells[indexColumn].style.width = parseInt(this.gridTBody.rows[i].cells[indexColumn].style.width) + dWidth + 'px';
            //alert('inside: ' + this.gridTBody.rows[i].cells[indexColumn].style.width);
        }

        //perform resize objColumnProxyResizers
        while (objColumnResizer.nextSibling) {
            objColumnResizer = objColumnResizer.nextSibling;
            if (parseInt(objColumnResizer.style.left) + dWidth >= 0)
                objColumnResizer.style.left = parseInt(objColumnResizer.style.left) + dWidth + 'px';
        }
    },

    // ----- column resize handlers -----
    _columnResizeMouseDown: function(e) {
        if (!e)
            e = event;

        if (e.target.getAttribute && e.target.getAttribute(this.elemResizerName) !== '' && e.target.getAttribute(this.elemResizerName) !== null) {
            //no records in grid
            if (this.gridTBody && this.gridTBody.rows.length == 1 && this.gridTBody.rows[0].cells.length == 1)
                return;
            //get Index of column which was resizing
            this.activeColumnResizing = e.target;
            var indexColumn = parseInt(this.activeColumnResizing.getAttribute(this.elemResizerName));
            var objColumn = this.headDiv.firstChild.childNodes[indexColumn];

            if (typeof (this.columnsInfoList[indexColumn]) != 'undefined' && !this.columnsInfoList[indexColumn].Resizable)
                return;

            this.isColumnResizing = true;
            this.startColumnResizingX = parseInt(e.clientX) + this.divWrapper.scrollLeft;
            this.startColumnResizingLeft = parseInt(e.target.style.left);

            this.dWidth = 0;

            this.leftResizer.style.display = 'block';
            this.rightResizer.style.display = 'block';
            this.leftResizer.style.left = objColumn.offsetLeft - this.divWrapper.scrollLeft + 1 + 'px';
            this.rightResizer.style.left = objColumn.offsetLeft - this.divWrapper.scrollLeft + 1 + objColumn.offsetWidth + 'px';
        }
    },

    _columnResizeMouseUp: function(e) {
        if (!e)
            e = event;
        if (this.isColumnResizing) {
            this.isColumnResizing = false;
            this.debugMsg('delta Width = ' + this.dWidth);
            this.performColumnResize(this.dWidth);

            var cellIndex = parseInt(this.activeColumnResizing.getAttribute(this.elemResizerName));
            var newSize = parseInt(this.headDiv.firstChild.childNodes[cellIndex].style.width);

            // TODO: Test			
            if (this.percentWidth && !this.columnsInfoList[cellIndex].IsSystem) {
                newSize = this.convertPixelToPercent(newSize);
                newSize = Math.round(newSize * 100);
            }

            this.leftResizer.style.display = 'none';
            this.rightResizer.style.display = 'none';
            //raise: onColumnResize event
            this.onColumnResize(cellIndex, newSize);
        }
    },

    _columnResizeMouseMove: function(e) {
        if (this.isColumnResizing) {
            if (!e)
                e = event;

            if (this.checkMinCellWidth(e)) {
                this.dWidth = parseInt(e.clientX) + this.divWrapper.scrollLeft - this.startColumnResizingX;
                this.activeColumnResizing.style.left = this.startColumnResizingLeft + this.dWidth + 'px';
                this.rightResizer.style.left = this.startColumnResizingLeft + this.dWidth - this.divWrapper.scrollLeft + 1 + 'px';
            }
            //e.target.style.left = parseInt(e.target.style.left) + e.screenX - this.startColumnResizingX + 'px';
            //move header cell resizer
        }
    },

    checkMinCellWidth: function(e) {
        var minWidth = 0;
        var newDWidth = parseInt(e.clientX) - this.startColumnResizingX;
        minWidth = this.startColumnResizingLeft + newDWidth - this.activeColumnResizing.offsetWidth - this.minColumnWidth;

        if (this.activeColumnResizing.getAttribute(this.elemResizerName) != '0') {
            minWidth -= parseInt(this.activeColumnResizing.previousSibling.style.left);
        }

        minWidth += this.divWrapper.scrollLeft;

        if (this.debugMode)
            window.status = 'minWidth = ' + minWidth + ' | dWidth = ' + this.dWidth;

        return minWidth > 0;
    },

    //total system columns width
    countSystemColumnsWidth: function() {
        var retVal = 2; // default border width

        for (var i = 0; i < this.columnsInfoList.length; i++) {
            if (this.columnsInfoList[i].IsSystem)
                retVal += this.columnsInfoList[i].Width + this.deltaColumnWidth;
        }

        return retVal;
    },


    // ----- OnRowClick Event ------
    onRowClick: function(rowIndex, primaryKeyId) {
        var handler = this.rowClickList.getHandler("mc_grid_rowClick");
        if (handler) {
            handler(this, new Mediachase.GridViewBaseRowEventArgs(rowIndex, primaryKeyId));
        }
    },
    add_rowClick: function(handler) {
        if (this.rowClickList)
            this.rowClickList.addHandler("mc_grid_rowClick", handler);
    },
    remove_rowClick: function(handler) {
        if (this.rowClickList != null)
            this.rowClickList.removeHandler("mc_grid_rowClick", handler);
    },

    // ----- OnRowDblClick Event ------
    onDblClick: function(rowIndex, primaryKeyId) {
        var handler = this.rowDblClickList.getHandler("mc_grid_dblClick");
        if (handler) {
            handler(this, new Mediachase.GridViewBaseRowEventArgs(rowIndex, primaryKeyId));
        }
    },
    add_dblClick: function(handler) {
        if (this.rowDblClickList)
            this.rowDblClickList.addHandler("mc_grid_dblClick", handler);
    },
    remove_dblClick: function(handler) {
        if (this.rowDblClickList != null)
            this.rowDblClickList.removeHandler("mc_grid_dblClick", handler);
    },

    // ----- OnRowSelect Event ------
    onRowSelect: function(rowIndex, primaryKeyId) {
        var handler = this.rowSelectList.getHandler("mc_grid_rowSelect");
        if (handler) {
            handler(this, new Mediachase.GridViewBaseRowEventArgs(rowIndex, primaryKeyId));
        }
    },
    add_rowSelect: function(handler) {
        if (this.rowSelectList)
            this.rowSelectList.addHandler("mc_grid_rowSelect", handler);
    },
    remove_rowSelect: function(handler) {
        if (this.rowSelectList != null)
            this.rowSelectList.removeHandler("mc_grid_rowSelect", handler);
    },

    // ----- OnColumnResize Event ------
    onColumnResize: function(cellIndex, newSize) {
        if (this.servicePath != '') {
            var params = {};
            params.ContextKey = this.contextKey;
            params.ColumnIndex = cellIndex;
            params.NewSize = newSize;
            Sys.Net.WebServiceProxy.invoke(this.servicePath, "ColumnResize", false, params, null, null);
        }
        var handler = this.columnResizeList.getHandler("mc_grid_columnResize");
        if (handler) {
            handler(this, new Mediachase.GridViewColumnResizeEventArgs(cellIndex, newSize));
        }
    },
    add_columnResize: function(handler) {
        if (this.columnResizeList)
            this.columnResizeList.addHandler("mc_grid_columnResize", handler);
    },
    remove_columnResize: function(handler) {
        if (this.columnResizeList != null)
            this.columnResizeList.removeHandler("mc_grid_columnResize", handler);
    },

    get_layoutResizeEnable: function() {
        return this.layoutResizeEnable;
    },
    set_layoutResizeEnable: function(value) {
        this.layoutResizeEnable = value;
    },

    get_percentWidth: function() {
        return this.percentWidth;
    },
    set_percentWidth: function(value) {
        this.percentWidth = value;
    },

    get_dashboardMode: function() {
        return this.dashboardMode;
    },
    set_dashboardMode: function(value) {
        this.dashboardMode = value;
    },

    get_servicePath: function() {
        return this.servicePath;
    },
    set_servicePath: function(value) {
        this.servicePath = value;
    },

    get_contextKey: function() {
        return this.contextKey;
    },
    set_contextKey: function(value) {
        this.contextKey = value;
    },

    get_columnsInfo: function() {
        return this.columnsInfo;
    },
    set_columnsInfo: function(value) {
        this.columnsInfo = value;
    },

    get_divResizeProxy: function() {
        return this.divResizeProxy;
    },
    set_divResizeProxy: function(value) {
        this.divResizeProxy = value;
        this.raisePropertyChanged('divResizeProxy');
    },

    get_gridStyleInfo: function() {
        return this.gridStyleInfo;
    },
    set_gridStyleInfo: function(value) {
        this.gridStyleInfo = value;
        this.raisePropertyChanged('gridStyleInfo');
    },

    get_isEmpty: function() {
        return this.isEmpty;
    },
    set_isEmpty: function(value) {
        this.isEmpty = value;
        this.raisePropertyChanged('isEmpty');
    },

    get_getCssFromColumn: function() {
        return this.getCssFromColumn;
    },
    set_getCssFromColumn: function(value) {
        this.getCssFromColumn = value;
    },

    get_bottomHeight: function() {
        return this.bottomHeight;
    },
    set_bottomHeight: function(value) {
        this.bottomHeight = value;
    },

    get_headerHeight: function() {
        return this.headerHeight;
    },
    set_headerHeight: function(value) {
        this.headerHeight = value;
    },

    get_paddingWidth: function() {
        return this.paddingWidth;
    },
    set_paddingWidth: function(value) {
        this.paddingWidth = value;
    },

    get_gridStyleInfoObj: function() {
        return this.gridStyleInfoObj;
    },
    set_gridStyleInfoObj: function(value) {
        this.gridStyleInfoObj = value;
    }
}

function ibn_serverGridCheckboxHandler(obj) {
    var parentTable = null;

    if (obj) {
        parentTable = obj;
        while (parentTable.parentNode != null) {
            if (parentTable.nodeType == 1 && parentTable.tagName == "TABLE")
                break;

            parentTable = parentTable.parentNode;
        }
    }

    var checkBoxes = null;

    if (parentTable)
        checkBoxes = parentTable.getElementsByTagName('INPUT');
    else
        checkBoxes = document.getElementsByTagName('INPUT');

    for (var i = 0; i < checkBoxes.length; i++) {
        if (checkBoxes[i].type == "checkbox" && checkBoxes[i].getAttribute("ibn_serverGridCheckbox")) {
            checkBoxes[i].checked = obj.checked;
        }
    }
}

// return true if parent contains node, otherwise false
function mc_checkParent(parent, node) {
    if (!node)
        return false;

    var obj = node;
    while (obj.parentNode != null) {
        if (obj.nodeType == 1 && obj.tagName == "BODY")
            return false;

        if (obj.nodeType == 1 && obj == parent)
            return true;

        obj = obj.parentNode;
    }

    return false;
}


function mc_checkVisibility(node) {
    if (!node)
        return false;

    var _obj = node;

    while (_obj.parentNode != null) {
        if (_obj.nodeType == 1) {
            if (_obj.style.display == 'none')
                return false;

            if (_obj.style.visibility == 'hidden')
                return false;

            if (_obj.tagName == "body")
                return true;

            //alert(_obj.nodeType + ':' + _obj.tagName + ' / ' + _obj.style.visibility + ' / ' + _obj.style.display);
        }
        _obj = _obj.parentNode;
    }

    return true;
}

Mediachase.GridViewHeaderBehavior.registerClass('Mediachase.GridViewHeaderBehavior', Sys.UI.Control);
