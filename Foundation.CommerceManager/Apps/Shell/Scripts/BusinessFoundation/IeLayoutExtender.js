
Type.registerNamespace("Ibn");

var is_ie = document.all;

Ibn.LayoutEventArgs = function(height, width, name, blockHeight, blockWidth)
{
  Ibn.LayoutEventArgs.initializeBase(this);
  this._height = height;
  this._width = width;
  this._name = name;
  this._blockHeight = blockHeight;
  this._blockWidth = blockWidth;
}
 
Ibn.LayoutEventArgs.prototype =
{
  get_height: function()
  {
    return this._height;
  },
  set_height: function(value)
  {
    this._height = value;
  },
  get_width: function()
  {
    return this._width;
  },
  set_width: function(value)
  {
    this._width = value;
  },
  get_name: function()
  {
	return this._name;
  },
  set_name: function(value)
  {
	this._name = value;
  },
  get_blockHeight: function()
  {
	return this._blockHeight;
  },
  set_blockWidth: function(value)
  {
	this._blockWidth = value;
  }  
}

Ibn.LayoutEventArgs.registerClass
  ('Ibn.LayoutEventArgs', Sys.EventArgs); 

// Layout constructor
Ibn.LayoutExtender = function(element) 
{
    Ibn.LayoutExtender.initializeBase(this, [element]);
    
	var _containerId = null;
	
	var _mLeft = null;
	var _mRight = null;	
	var _mTop = null;
	var _mBottom = null;
	var _clientOnResize = null;
	var _borderSplitSize = null;
	var getHtmlAsForm = null;
	
	var resize_cell = null;
	var resize_start_cell_x = -1;
	var resize_start_cell_y = -1;	
	var resize_start_x = -1;  
	var resize_start_y = -1;
	var resize_start_coord = -1;
	var index = 0;
	var is_ie = null;	

	var _resizeClientHandler = null;		
	var splitersArray = null;
	var _enableScrolling = null;
}


Ibn.LayoutExtender.prototype =
{
	get_containerId: function ()
	{
		return this._containerId;
	},
	set_containerId: function (value)
	{
		this._containerId = value;
	},
	
	get_mLeft: function ()
	{
		return this._mLeft;
	},
	set_mLeft: function (value)
	{
		this._mLeft = value;
	},	
	get_mRight: function ()
	{
		return this._mRight;
	},
	set_mRight: function (value)
	{
		this._mRight = value;
	},	
	get_mTop: function ()
	{
		return this._mTop;
	},
	set_mTop: function (value)
	{
		this._mTop = value;
	},	
	get_mBottom: function ()
	{
		return this._mBottom;
	},
	set_mBottom: function (value)
	{
		this._mBottom = value;
	},	
	
	get_enableScrolling: function ()
	{
		return this._enableScrolling;
	},
	set_enableScrolling: function (value)
	{
		this._enableScrolling = value;
	},	
	
	get_clientOnResize: function ()
	{
		return this._clientOnResize;
	},
	set_clientOnResize: function (value)
	{
		this._clientOnResize = value;
	},				

	get_borderSplitSize: function ()
	{
		return this._borderSplitSize;
	},
	set_borderSplitSize: function (value)
	{
		this._borderSplitSize = value;
	},
	
	get_getHtmlAsForm: function ()
	{
		return this.getHtmlAsForm;
	},
	set_getHtmlAsForm: function (value)
	{
		this.getHtmlAsForm = value;
	},		
		
	initialize: function()
	{
		Ibn.LayoutExtender.callBaseMethod(this, 'initialize');
		
		//alert('ie 6 version loaded');
		this.splitersArray = new Array();		
		this.is_ie = document.all;
		this.resizeList = new Sys.EventHandlerList();
		//alert('before init split');
		this.initSplitArray();
		//alert('after init split');
		this.sortArray();
		//alert('before attach events');
		this.attachEvents();
		//return;
		if (this._enableScrolling)
			this.updateClientRegion();		
		
		if (this._clientOnResize != null && this._clientOnResize.length > 0)
		{
			this._resizeClientHandler = Function.createDelegate(this, eval(this._clientOnResize));
			this.add_resize(this._resizeClientHandler);
		}		
		
		$addHandler(window, "resize", Function.createDelegate(this, this.onWindowResize));
	},
	dispose: function ()
	{
		var _mLeft = null;
		var _mRight = null;	
		var _mTop = null;
		var _mBottom = null;	
		
		var _containerId = null;
	
		var splitersArray = null;
		Ibn.LayoutExtender.callBaseMethod(this, 'dispose');
	},
	
	initSplitArray: function()
	{
		var allDivs = document.getElementsByTagName("DIV");
		//alert(allDivs.length);
		for (i = 0; i <	allDivs.length; i++)
		{
		    if (i > 80)
		    {
//		        alert(i);
//		        alert(allDivs[i].childNodes.length + '__' + allDivs[i].getAttribute("ibntype") + '__' + allDivs[i].getAttribute("ibnwidth"));
//		        alert(allDivs[i].outerHTML);
		    }
			if (allDivs[i].getAttribute("ibntype"))
			{
			    //alert('in ' + i);
				//allDivs[i].id = newGuid();
				if (allDivs[i].getAttribute("ibnwidth"))
				{
					allDivs[i].style.setExpression("width", allDivs[i].getAttribute("ibnwidth"));
				}				
				if (allDivs[i].getAttribute("ibnheight"))
				{
					allDivs[i].style.setExpression("height", allDivs[i].getAttribute("ibnheight"));
				}		
				//document.recalc(true);		
				this.splitersArray[this.splitersArray.length] = allDivs[i];
				//alert('attached: ' + this.splitersArray.length);
			}
		}	
	},
	attachEvents: function()
	{
		for (var i = 0; i < this.splitersArray.length; i++)
		{
			if (this.splitersArray[i].getAttribute("ibntype") == "splitter")
			{
				//TO DO: attach events
				$addHandler(this.splitersArray[i], "mousedown", Function.createDelegate(this, this.split_resize_cell_down));
				$addHandler(this.splitersArray[i], "mouseup", Function.createDelegate(this, this.split_resize_cell_up));
			}
		}
	},
	updateMargins: function()
	{
		if (this._containerId != null && this._containerId != 'undefined')
		{
			this._containerId.style.marginLeft = this._mLeft + "px";
			this._containerId.style.marginRight = this._mRight + "px";
			this._containerId.style.marginTop = this._mTop + "px";
			this._containerId.style.marginBottom = this._mBottom + "px";		
		}
	},
	updateClientRegion: function()
	{
		//alert('none');
		//return;			
		this._containerId.style.height = parseInt(this._containerId.parentNode.offsetHeight) - parseInt(this._mTop) - parseInt(this._mBottom) + "px";
		this._containerId.style.overflow = 'scroll';
	},	
	sortArray: function()
	{
		for (i = 0; i < this.splitersArray.length - 1; i++)
			for (j = i; j < this.splitersArray.length; j++)
			{
				if ( parseInt(this.splitersArray[i].getAttribute("ibnindex")) > parseInt(this.splitersArray[j].getAttribute("ibnindex")))
				{
					//alert('cur index: ' + i + '/r/n/ new index: ' + this.splitersArray[i].getAttribute("ibnindex") + '\r\n' +
					var tmp = this.splitersArray[i];
					this.splitersArray[i] = this.splitersArray[j];
					this.splitersArray[j] = tmp;
				}
			}
	},	
	printArr: function(msg)
	{
		var s = "";
		for (i = 0; i < this.splitersArray.length; i++)
			s += i + ',' + this.splitersArray[i].getAttribute("ibnindex") + '\r\n';
		//alert(msg + '\r\n' + s);		
	},
	// ------- for splitter events ------
	recalculateArrayLeft: function(deltaX, startX, senderId)
	{
		var tHeight = 0; 
		var tWidth = deltaX; 
		var flag = false;
		//alert(tHeight);
			
		for (i = 0; i < this.splitersArray.length; i++)
		{
			if (this.splitersArray[i].getAttribute("ibnorientation") == "Top" ||
				this.splitersArray[i].getAttribute("ibnorientation") == "Bottom")
				tHeight = parseInt(tHeight) + parseInt(this.splitersArray[i].style.height);

			if (this.splitersArray[i].getAttribute("ibnorientation") == "Left" ||
				this.splitersArray[i].getAttribute("ibnorientation") == "Right")
				tWidth = parseInt(tWidth) + parseInt(this.splitersArray[i].style.width);
					
			if (flag)
			{
				//alert(i + '!'+this.splitersArray[i].getAttribute("ibnorientation")+'!'+tWidth+'!'+countWidth(tWidth, 0, this._containerId.id)+', '+parseInt(document.getElementById(this._containerId.id).clientWidth));
				if (this.splitersArray[i].getAttribute("ibnorientation") != "Right")
				{
					if (this.splitersArray[i].getAttribute("ibnorientation") == "Left")
					{
						this.splitersArray[i].style.left = parseInt(this.splitersArray[i].style.left) + deltaX + "px";
					}
					else // orientation == top or bottom
					{
						//alert(this.splitersArray[i].style.removeExpression("width")+','+this.splitersArray[i].style.removeExpression("width"));
						this.splitersArray[i].style.removeExpression("width");
						this.splitersArray[i].style.setExpression("width", "countWidth(" + tWidth + " , 0, '" + this._containerId.id + "')");
						this.splitersArray[i].style.left = parseInt(this.splitersArray[i].style.left) + parseInt(deltaX) + "px";
					}
				}
			}
			
			if (this.splitersArray[i].id == senderId)
			{
				flag = true;				
			}
		}	
		
		var regionId = document.getElementById(senderId).getAttribute("ibntyperegion");
		var newWidth = parseInt(document.getElementById(regionId).style.width) + parseInt(deltaX);
		document.getElementById(regionId).style.width = newWidth + "px";
		this._mLeft = parseInt(this._mLeft) + parseInt(deltaX);
		
		this.updateMargins();	
	},
	recalculateArrayRight: function(deltaX, startX, senderId)
	{
		var tHeight = 0; 
		var tWidth = deltaX; 
		var flag = false;
		//alert(tHeight);
			
		for (i = 0; i < this.splitersArray.length; i++)
		{
				
			if (this.splitersArray[i].getAttribute("ibnorientation") == "Top" ||
				this.splitersArray[i].getAttribute("ibnorientation") == "Bottom")
				tHeight += parseInt(this.splitersArray[i].style.height);

			if (this.splitersArray[i].getAttribute("ibnorientation") == "Left" ||
				this.splitersArray[i].getAttribute("ibnorientation") == "Right")
				tWidth += parseInt(this.splitersArray[i].style.width);
					
			if (flag)
			{
				if (this.splitersArray[i].getAttribute("ibnorientation") != "Left")
				{
					if (this.splitersArray[i].getAttribute("ibnorientation") == "Right")
					{
						this.splitersArray[i].style.right = parseInt(this.splitersArray[i].style.right) + parseInt(deltaX) + "px";
					}
					else // orientation == top or bottom
					{
						this.splitersArray[i].style.removeExpression("width");
						this.splitersArray[i].style.setExpression("width", "countWidth(" + tWidth + " , 0, '" + this._containerId.id + "')");
						//splitArray[i].style.left = parseInt(splitArray[i].style.left) + deltaX + "px";
					}
				}
			}
			
			if (this.splitersArray[i].id == senderId)
			{
				flag = true;				
			}
		}	
		
		
		var regionId = document.getElementById(senderId).getAttribute("ibntyperegion");
		var newWidth = parseInt(document.getElementById(regionId).style.width) + parseInt(deltaX);
		document.getElementById(regionId).style.width = newWidth + "px";
		
		this._mRight = parseInt(this._mRight) + parseInt(deltaX);
		this.updateMargins();
	},
	recalculateArrayTop: function(deltaY, startY, senderId)	
	{
		var tHeight = deltaY; //document.getElementById(senderId).parentNode.parentNode.offsetHeight;
		var tWidth = 0; //document.getElementById(senderId).parentNode.parentNode.offsetWidth;
		var flag = false;
		//alert(tHeight);
			
		for (i = 0; i < this.splitersArray.length; i++)
		{
				
			if (this.splitersArray[i].getAttribute("ibnorientation") == "Top" ||
				this.splitersArray[i].getAttribute("ibnorientation") == "Bottom")
				tHeight += parseInt(this.splitersArray[i].style.height);

			if (this.splitersArray[i].getAttribute("ibnorientation") == "Left" ||
				this.splitersArray[i].getAttribute("ibnorientation") == "Right")
				tWidth += parseInt(this.splitersArray[i].style.width);
					
			if (flag)
			{
				if (this.splitersArray[i].getAttribute("ibnorientation") != "Bottom")
				{
					if (this.splitersArray[i].getAttribute("ibnorientation") == "Top")
					{
						this.splitersArray[i].style.top = parseInt(this.splitersArray[i].style.top) + deltaY + "px";
					}
					else // orientation == left or right
					{
						//alert(tHeight);
						this.splitersArray[i].style.setExpression("height", "countHeight(" + tHeight + " , -1, '" + this._containerId.id + "')");
						this.splitersArray[i].style.top = parseInt(this.splitersArray[i].style.top) + deltaY + "px";
					}
				}
			}
			
			if (this.splitersArray[i].id == senderId)
			{
				flag = true;				
			}
		}
			
		var regionId = document.getElementById(senderId).getAttribute("ibntyperegion");
		var newHeight = parseInt(document.getElementById(regionId).style.height) + parseInt(deltaY);
		
		document.getElementById(regionId).style.height = newHeight + "px";
		this._mTop = parseInt(this._mTop) + parseInt(deltaY);
		this.updateMargins();		
	},
	recalculateArrayBottom: function(deltaY, startY, senderId)
	{
		var tHeight = deltaY; //document.getElementById(senderId).parentNode.parentNode.offsetHeight;
		var tWidth = 0; //document.getElementById(senderId).parentNode.parentNode.offsetWidth;
		var flag = false;
			
		for (i = 0; i < this.splitersArray.length; i++)
		{
			if (this.splitersArray[i].getAttribute("ibnorientation") == "Top" ||
				this.splitersArray[i].getAttribute("ibnorientation") == "Bottom")
				tHeight += parseInt(this.splitersArray[i].style.height);

			if (this.splitersArray[i].getAttribute("ibnorientation") == "Left" ||
				this.splitersArray[i].getAttribute("ibnorientation") == "Right")
				tWidth += parseInt(this.splitersArray[i].style.width);
					
			if (flag)
			{
				if (this.splitersArray[i].getAttribute("ibnorientation") != "Top")
				{
					if (this.splitersArray[i].getAttribute("ibnorientation") == "Bottom")
					{
						this.splitersArray[i].style.bottom = parseInt(this.splitersArray[i].style.bottom) + parseInt(deltaY) + "px";
					}
					else // orientation == left or right
					{
						//alert(tHeight);
						this.splitersArray[i].style.removeExpression("height");
						this.splitersArray[i].style.setExpression("height", "countHeight(" + tHeight + " , -1, '" + this._containerId.id + "')");
					}
				}
			}
			
			if (this.splitersArray[i].id == senderId)
			{
				flag = true;				
			}
		}
			
		var regionId = document.getElementById(senderId).getAttribute("ibntyperegion");		
		var newHeight = parseInt(document.getElementById(regionId).style.height) + parseInt(deltaY);
		//alert('newHeight: '+newHeight + "px");
		document.getElementById(regionId).style.height = newHeight + "px";
		this._mBottom = parseInt(this._mBottom) + parseInt(deltaY);
		this.updateMargins();
	},
	
	// --------- splitter event handlers --------
	
	split_resize_cell_down: function(e)
	{
	  if (!e)
		e = event;
	
	  this.resize_cell = document.getElementById(e.target.id);
	    
	  this.resize_start_coord = e.clientX;
  	  if (this.resize_cell.getAttribute("ibnorientation") == "Left")
		  this.resize_start_cell_x = parseInt(this.resize_cell.style.left);
	  else if (this.resize_cell.getAttribute("ibnorientation") == "Right")
		  this.resize_start_cell_x = parseInt(this.resize_cell.style.right);
	  else if (this.resize_cell.getAttribute("ibnorientation") == "Top")
		  this.resize_start_cell_y = parseInt(this.resize_cell.style.top);
	  else if (this.resize_cell.getAttribute("ibnorientation") == "Bottom")
		  this.resize_start_cell_y = parseInt(this.resize_cell.style.bottom);
		  
	  document.onmousemove = Function.createDelegate(this, this.split_resize_callback);
	  document.onmouseup = Function.createDelegate(this, this.split_resize_cell_up);
	  
	  if (!is_ie) 
	  {
	    document.captureEvents(Event.MOUSEMOVE);
	    document.captureEvents(Event.MOUSEUP);
	  } else {
	    while ( ! this.resize_cell )
	    {
	      this.resize_cell = document.getElementById(e.target.id);
	    }
	  }
	  return false;	
	},
	split_resize_cell_up: function()
	{
	  if ( !this.resize_cell ) return;
	
	  //?
	  //if ( this.resize_cell.old_className ) this.resize_cell.className = this.resize_cell.old_className;
	
	  document.onmousemove = null;
	  document.onmouseup = null;
	
	  if ( ! is_ie )
	  {
	    document.releaseEvents(Event.MOUSEMOVE);
	    document.releaseEvents(Event.MOUSEUP);
	  }
	  
	  if (this.resize_cell.getAttribute("ibnorientation") == "Left")
		  this.recalculateArrayLeft(parseInt(this.resize_cell.style.left) - parseInt(this.resize_start_cell_x), parseInt(this.resize_start_cell_x), this.resize_cell.id);
	  else if (this.resize_cell.getAttribute("ibnorientation") == "Right")
	  	  this.recalculateArrayRight(parseInt(this.resize_cell.style.right) - parseInt(this.resize_start_cell_x), parseInt(this.resize_start_cell_x), this.resize_cell.id);
	  else if (this.resize_cell.getAttribute("ibnorientation") == "Top")
	  	  this.recalculateArrayTop(parseInt(this.resize_cell.style.top) - parseInt(this.resize_start_cell_y), parseInt(this.resize_start_cell_y), this.resize_cell.id);
	  else if (this.resize_cell.getAttribute("ibnorientation") == "Bottom")
	  	  this.recalculateArrayBottom(parseInt(this.resize_cell.style.bottom) - parseInt(this.resize_start_cell_y), parseInt(this.resize_start_cell_y), this.resize_cell.id);
	  
	  //raise resize event
	  this.resize(this.resize_cell.getAttribute("ibnfriendlyname"));
	  
	  this.resize_flag = 1;
	  this.resize_cell = null;
	},
	split_resize_callback: function(e)
	{
	  if (!e)
		e = event;
		
	  var cur_x = parseInt(event.pageX);
	  var cur_y = parseInt(event.pageY);  
	  var cur_bottom_y = parseInt(document.body.clientHeight) - parseInt(event.pageY);
      var cur_right_x = parseInt(document.body.clientWidth) - parseInt(event.pageX);
	
      //var cur_x = e.clientX;
      //var cur_y = e.clientY;  
      //var cur_bottom_y = parseInt(document.body.clientHeight) - e.clientY;
      //var cur_right_x = parseInt(document.body.clientWidth) - e.clientX;
	  
	  if (this.resize_cell.getAttribute("ibnorientation") == "Left")
		  this.resize_cell.style.left = cur_x + "px";
	  else if (this.resize_cell.getAttribute("ibnorientation") == "Right")
		  this.resize_cell.style.right = cur_right_x + "px";
	  else if (this.resize_cell.getAttribute("ibnorientation") == "Top")
		  this.resize_cell.style.top = cur_y + "px";
	  else if (this.resize_cell.getAttribute("ibnorientation") == "Bottom")
		  this.resize_cell.style.bottom = cur_bottom_y + "px";	
	  
	  //window.status = this.resize_cell.style.bottom + " | " + cur_bottom_y;
	},
	getForm: function()
	{
		var p = this._containerId;
		var tmpForm = null;
		while (p.parentNode != null)		
		{
			if (p.tagName.toLowerCase() == "html")
				return p;
		
			if (p.tagName.toLowerCase() == "form")
			{
				if (this.getHtmlAsForm)
					return p;
					
				tmpForm = p;
			}
				
			p = p.parentNode;
		}
		
		if (tmpForm != null)
			return tmpForm;
		
		return p;
	},	
	onWindowResize: function(e)
	{
		this.resize("window.resize");
	},
	// ----- Resize Event -----
    resize: function(regionName) {
       var handler = this.resizeList.getHandler("ibn_layout_onresize");
       
		if (this._enableScrolling)
			this.updateClientRegion();       
       
       //alert(regionName+':'+_newBlockHeight+'x'+_newBlockWidth);
       if (handler) 
       {
		   var _newWidth = this.getForm().offsetWidth - this._mLeft - this._mRight;
		   var _newHeight = this.getForm().offsetHeight - this._mTop - this._mBottom;
//		   _newHeight -= 50; //ie 6 incredeble bug fix
//		   _newWidth -= 14; //ie 6 incredeble bug fix
		   if (regionName != "window.resize")
		   {
			   var _newBlockHeight = document.getElementById(this.resize_cell.getAttribute("ibntyperegion")).offsetHeight;
			   var _newBlockWidth = document.getElementById(this.resize_cell.getAttribute("ibntyperegion")).offsetWidth;
			   args = new Ibn.LayoutEventArgs(_newHeight, _newWidth, regionName, _newBlockHeight, _newBlockWidth);
			}
			else
			{
				args = new Ibn.LayoutEventArgs(_newHeight, _newWidth, regionName, -1, -1);
			}
		   		   		   
		   //alert(_newHeight + 'x' + _newWidth);

           handler(this, args);
       }
    },
    add_resize: function(handler)
    {
		this.resizeList.addHandler("ibn_layout_onresize", handler);
    },
    remove_resize: function(handler)
    {
		this.resizeList.removeHandler("ibn_layout_onresize", handler);
    }    
	
	
}
	//IE6 Expression function
	function countWidth(marginLeft, marginRight, containerId)
	{
		var retVal = 0;
//		if (marginRight != -1)
			retVal = parseInt(document.getElementById(containerId).parentNode.clientWidth);
//		else
//			retVal = parseInt(document.getElementById(containerId).clientWidth);
			
		retVal -= marginLeft;
		retVal -= marginRight;
		retVal -= 2;
		//window.status = 'countWidth with params:'+marginLeft+'|'+marginRight+' = '+retVal;
		return retVal;
	}

	//IE6 Expression function	
	function countHeight(marginTop, marginBottom, containerId)
	{
		var retVal = 0;
//		if (marginBottom != -1)
			retVal = parseInt(document.getElementById(containerId).parentNode.clientHeight);
//		else
//			retVal = parseInt(document.getElementById(containerId).clientHeight);

		retVal -= marginTop;
		retVal -= marginBottom;
		retVal -= 2;
	//	if (marginTop == 0 && marginBottom == 0)
	//	window.status = 'countHeight with params:'+marginTop+'|'+marginBottom+' = '+retVal;	
		return retVal;
	}

	//Generate GUID
	function newGuid()
	{
		var _guid = "";
		for(var i = 0; i < 32; i++)
		_guid += Math.floor(Math.random() * 0xF).toString(0xF) + (i == 8 || i == 12 || i == 16 || i == 20 ? "-" : "")
		return _guid;
	}	

Ibn.LayoutExtender.registerClass("Ibn.LayoutExtender", Sys.UI.Control);

if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();	