Type.registerNamespace("Ibn");

var is_ie = document.all;

// ----------------------
// -----  Layout  -------
// ----------------------
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
	
	var resize_cell = null;
	var resize_start_cell_x = -1;
	var resize_start_cell_y = -1;	
	var resize_start_x = -1;  
	var resize_start_y = -1;
	var resize_start_coord = -1;
	var is_ie = null;	
	
	var _resizeClientHandler = null;	
	var splitersArray = null;
	
	var _collapseHandler = null;
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
	
	initialize: function()
	{
		Ibn.LayoutExtender.callBaseMethod(this, 'initialize');
		
		this.splitersArray = new Array();
		this.is_ie = document.all;
		this.resizeList = new Sys.EventHandlerList();
		this.initSplitArray();
		this.attachEvents();
		
		if (this._clientOnResize != null && this._clientOnResize.length > 0)
		{
			this._resizeClientHandler = Function.createDelegate(this, eval(this._clientOnResize));
			this.add_resize(this._resizeClientHandler);
		}
		this._containerId.style.height = this._containerId.parentNode.offsetHeight - this._containerId.offsetTop + "px";
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
		for (i = 0; i <	allDivs.length; i++)
		{
			if (allDivs[i].getAttribute("ibntype"))
			{
				this.splitersArray[this.splitersArray.length] = allDivs[i];
			}
		}	
	},
	attachEvents: function()
	{
	    this._collapseHandler = Function.createDelegate(this, this.onCollapseHandler);
		for (var i = 0; i < this.splitersArray.length; i++)
		{
			if (this.splitersArray[i].getAttribute("ibntype") == "splitter")
			{
				$addHandler(this.splitersArray[i], "mousedown", Function.createDelegate(this, this.split_resize_cell_down));
				$addHandler(this.splitersArray[i], "mouseup", Function.createDelegate(this, this.split_resize_cell_up));
				
				
				//Expand/Collapse handlers
				$addHandler(this.splitersArray[i], "dblclick", this._collapseHandler);
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
	
	// ------- for splitter events ------
	recalculateArrayLeft: function(deltaX, startX, senderId)
	{
	    if (deltaX != 0)
	    {
		    for (i = 0; i < this.splitersArray.length; i++)
		    {
			    if (parseInt(this.splitersArray[i].style.left)- this._borderSplitSize > startX && this.splitersArray[i].id != senderId)
			    {
				    this.splitersArray[i].style.left = parseInt(this.splitersArray[i].style.left) + deltaX + "px";
			    }
		    }
    		
		    var regionId = document.getElementById(senderId).getAttribute("ibntyperegion");
            
            if (parseInt(document.getElementById(regionId).style.width) + deltaX >= 0)
		        document.getElementById(regionId).style.width = parseInt(document.getElementById(regionId).style.width) + deltaX + "px";
		    else
		        document.getElementById(regionId).style.width = "0px";
		    
		    this._mLeft = parseInt(this._mLeft) + parseInt(deltaX);
		    this.updateMargins();	
		}
	},
	recalculateArrayRight: function(deltaX, startX, senderId)
	{
	    if  (deltaX != 0)
	    {
		    for (i = 0; i < this.splitersArray.length; i++)
		    {
			    if (parseInt(this.splitersArray[i].style.right)- this._borderSplitSize > startX && this.splitersArray[i].id != senderId)
			    {
				    this.splitersArray[i].style.right = parseInt(this.splitersArray[i].style.right) + deltaX + "px";
			    }
		    }
    		
		    var regionId = document.getElementById(senderId).getAttribute("ibntyperegion");
		    
		    if (parseInt(document.getElementById(regionId).style.width) + deltaX >= 0)
		        document.getElementById(regionId).style.width = parseInt(document.getElementById(regionId).style.width) + deltaX + "px";
		    else
		        document.getElementById(regionId).style.width = "0px";
		        
		    this._mRight = parseInt(this._mRight) + parseInt(deltaX);
		    this.updateMargins();
		}
	},
	recalculateArrayTop: function(deltaY, startY, senderId)	
	{
	    if (deltaY != 0)
	    {
		    for (i = 0; i < this.splitersArray.length; i++)
		    {
			    if (parseInt(this.splitersArray[i].style.top) - this._borderSplitSize /*- deltaY */> startY && this.splitersArray[i].id != senderId)
			    {
				    this.splitersArray[i].style.top = parseInt(this.splitersArray[i].style.top) + deltaY + "px";			
			    }
		    }
		    var regionId = document.getElementById(senderId).getAttribute("ibntyperegion");
		    if (parseInt(document.getElementById(regionId).style.height) + deltaY >= 0)
		        document.getElementById(regionId).style.height = parseInt(document.getElementById(regionId).style.height) + deltaY + "px";
		    else
		        document.getElementById(regionId).style.height = "0px";
		    this._mTop = parseInt(this._mTop) + parseInt(deltaY);
		    this.updateMargins();
		}		
	},
	recalculateArrayBottom: function(deltaY, startY, senderId)
	{
	    if (deltaY != 0)
	    {
		    for (i = 0; i < this.splitersArray.length; i++)
		    {
			    if (parseInt(this.splitersArray[i].style.bottom) - this._borderSplitSize > startY && this.splitersArray[i].id != senderId)
			    {
				    this.splitersArray[i].style.bottom = parseInt(this.splitersArray[i].style.bottom) + deltaY + "px";
			    }
		    }
		    var regionId = document.getElementById(senderId).getAttribute("ibntyperegion");
		    
		    if (parseInt(document.getElementById(regionId).style.height) + deltaY >= 0)
		        document.getElementById(regionId).style.height = parseInt(document.getElementById(regionId).style.height) + deltaY + "px";
		    else
		        document.getElementById(regionId).style.height = "0px";
		    
		    this._mBottom = parseInt(this._mBottom) + parseInt(deltaY);
		    this.updateMargins();
		}
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
	  {
		  this.recalculateArrayLeft(parseInt(this.resize_cell.style.left) - this.resize_start_cell_x, this.resize_start_cell_x, this.resize_cell.id);
		  if (parseInt(this.resize_cell.style.left) == this.resize_start_cell_x) return; // do not raise change event
      }
	  else if (this.resize_cell.getAttribute("ibnorientation") == "Right")
	  {
	  	  this.recalculateArrayRight(parseInt(this.resize_cell.style.right) - this.resize_start_cell_x, this.resize_start_cell_x, this.resize_cell.id);
	  	  if (parseInt(this.resize_cell.style.right) == this.resize_start_cell_x) return;
	  }
	  else if (this.resize_cell.getAttribute("ibnorientation") == "Top")
	  {	  
	  	  this.recalculateArrayTop(parseInt(this.resize_cell.style.top) - this.resize_start_cell_y, this.resize_start_cell_y, this.resize_cell.id);
	  	  if (parseInt(this.resize_cell.style.top) == this.resize_start_cell_y) return;
	  }
	  else if (this.resize_cell.getAttribute("ibnorientation") == "Bottom")
	  {
	  	  this.recalculateArrayBottom(parseInt(this.resize_cell.style.bottom) - this.resize_start_cell_y, this.resize_start_cell_y, this.resize_cell.id);
	  	  if (parseInt(this.resize_cell.style.bottom) == this.resize_start_cell_y) return;
	  }	  	  
	  
	  //raise resize event
	  this.resize(this.resize_cell.getAttribute("ibnfriendlyname"));
	  
	  this.resize_flag = 1;
	  this.resize_cell = null;
	},
	split_resize_callback: function(e)
	{
	  if (!e)
		e = event;
			
      //var cur_x = e.pageX;
      //var cur_y = e.pageY;  
      
      //if (is_ie)
      var scrollTop = 0;
      var p = this.resize_cell;
      
      while (p != null)
      {
        if (p.nodeType == 1 && p.tagName == "HTML")
        {
			scrollTop = p.offsetHeight - p.clientHeight - p.scrollTop;
			break;
		}
		p = p.parentNode;
      }
      
	  var cur_x = e.clientX;
      //if (is_ie)
	  var cur_y = e.clientY - scrollTop;		
	  
	  if (is_ie)
		cur_y -= 16;
		
      var cur_bottom_y = parseInt(document.body.clientHeight) - cur_y ;
      var cur_right_x = parseInt(document.body.clientWidth) - cur_x;
      
      
	  
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
	//collapse / expand handlers
	//get splitter full size
	getSplitterSize: function (obj)
	{
	    if (obj.getAttribute("ibnorientation") == "Top")
	        return parseInt(obj.style.top);
	    if (obj.getAttribute("ibnorientation") == "Bottom")
	        return parseInt(obj.style.bottom);
	    if (obj.getAttribute("ibnorientation") == "Left")
	        return parseInt(obj.style.left);
	    if (obj.getAttribute("ibnorientation") == "Right")
	        return parseInt(obj.style.right);
	        
	    return -1;
	},
	//get dX
	getSplitterActualSize: function(obj)
	{
	    var prevSize = 0;
	    var totalSize = 0;
	    var count = 0;
		for (i = 0; i < this.splitersArray.length; i++)
		{
		    if (this.splitersArray[i].id == obj.id)
		    {
		        count = i;
		        break;
		    }
		        
		    if (this.splitersArray[i].getAttribute("ibnorientation") == obj.getAttribute("ibnorientation"))
		    {
		        prevSize = this.getSplitterSize(this.splitersArray[i]);
		    }
		}
		
	    if (obj.getAttribute("ibnorientation") == "Top")
	    {	        
	        totalSize = parseInt(obj.style.top);
	        if (prevSize == 0 && count > 0)
	            return parseInt(this.splitersArray[count - 1].style.height);
	    }
	    if (obj.getAttribute("ibnorientation") == "Bottom")
	    {
	        totalSize = parseInt(obj.style.bottom);
	        if (prevSize == 0 && count > 0)
	            return parseInt(this.splitersArray[count - 1].style.height);
	    }	        
	    if (obj.getAttribute("ibnorientation") == "Left")
	    {
	        totalSize = parseInt(obj.style.left);
	        if (prevSize == 0 && count > 0)
	            return parseInt(this.splitersArray[count - 1].style.width);
	    }	        
	    if (obj.getAttribute("ibnorientation") == "Right")
	    {
	        totalSize = parseInt(obj.style.right);
	        if (prevSize == 0 && count > 0)
	            return parseInt(this.splitersArray[count - 1].style.width);
	    }	        
	        
	    return totalSize - prevSize;
	},
	onCollapseHandler: function(obj)
	{
	    if (obj.target)
	    {
	        var newSize = this.getSplitterActualSize(obj.target);
	        if (obj.target.getAttribute("ibnorientation") == "Bottom")
	        {
	            newSize -= parseInt(obj.target.style.height);
	            this.recalculateArrayBottom(-newSize, newSize, obj.target.id);
	            obj.target.style.bottom = parseInt(obj.target.style.bottom) - newSize  + "px";

	        } else
	        if (obj.target.getAttribute("ibnorientation") == "Top")
	        {
	            newSize -= parseInt(obj.target.style.height); //mb +
	            this.recalculateArrayTop(-newSize, parseInt(obj.target.style.top), obj.target.id);
	            obj.target.style.top = parseInt(obj.target.style.top) - newSize  + "px";
	        } else
	        if (obj.target.getAttribute("ibnorientation") == "Left")
	        {
	            newSize -= parseInt(obj.target.offsetWidth);
	            this.recalculateArrayLeft(-newSize, newSize, obj.target.id);
	            obj.target.style.left = parseInt(obj.target.style.left) - newSize  + "px";	        
	        } else
	        if (obj.target.getAttribute("ibnorientation") == "Right")
	        {
	            newSize -= parseInt(obj.target.offsetWidth);
	            this.recalculateArrayRight(-newSize, newSize, obj.target.id);
	            obj.target.style.right = parseInt(obj.target.style.right) - newSize  + "px";
	        }	        
	            
	        //raise resize event
	        this.resize(obj.target.getAttribute("ibnfriendlyname"));
	        
	        $clearHandlers(obj.target);
			
			$addHandler(obj.target, "mousedown", Function.createDelegate(this, this.split_resize_cell_down));
			$addHandler(obj.target, "mouseup", Function.createDelegate(this, this.split_resize_cell_up));	
			var expandHandler = Function.createDelegate(this, this.onExpandHandler);
			$addHandler(obj.target, "dblclick", function() { expandHandler(obj.target, newSize) });
	    }
	},
	onExpandHandler: function(obj, size)
	{
	    //alert(size);
        if (obj.getAttribute("ibnorientation") == "Bottom")
        {
            size += parseInt(obj.style.height);
            this.recalculateArrayBottom(size, this.getSplitterSize(obj), obj.id);
            obj.style.bottom = parseInt(obj.style.bottom) + size + "px";

        } else
        if (obj.getAttribute("ibnorientation") == "Top")
        {
            size += parseInt(obj.style.height); //mb +
            this.recalculateArrayTop(size, this.getSplitterSize(obj), obj.id);
            obj.style.top = parseInt(obj.style.top) + size + "px";
        } else
        if (obj.getAttribute("ibnorientation") == "Left")
        {
            size += parseInt(obj.offsetWidth);
            this.recalculateArrayLeft(size, this.getSplitterSize(obj), obj.id);
            obj.style.left = parseInt(obj.style.left) + size + "px";	        
        } else
        if (obj.getAttribute("ibnorientation") == "Right")
        {
            size += parseInt(obj.offsetWidth);
            this.recalculateArrayRight(size, this.getSplitterSize(obj), obj.id);
            obj.style.right = parseInt(obj.style.right) + size + "px";
        }	        
            
        //raise resize event
        this.resize(obj.getAttribute("ibnfriendlyname"));	    

	    $clearHandlers(obj);
			
		$addHandler(obj, "mousedown", Function.createDelegate(this, this.split_resize_cell_down));
		$addHandler(obj, "mouseup", Function.createDelegate(this, this.split_resize_cell_up));		    
		$addHandler(obj, "dblclick", this._collapseHandler);
	},
	
	onWindowResize: function(e)
	{
		this.resize("window.resize");
	},	
	// ----- Resize Event -----
    resize: function(regionName) {
       var handler = this.resizeList.getHandler("ibn_layout_onresize");       
       this._containerId.style.height = this._containerId.parentNode.offsetHeight - this._containerId.offsetTop + "px";
       //alert(regionName+':'+_newBlockHeight+'x'+_newBlockWidth);
       if (handler) 
       {
		   var _newWidth = this._containerId.parentNode.offsetWidth - this._mLeft - this._mRight;
		   var _newHeight = this._containerId.parentNode.offsetHeight - this._mTop - this._mBottom;
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

Ibn.LayoutExtender.registerClass("Ibn.LayoutExtender", Sys.UI.Control);

if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();

