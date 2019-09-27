Type.registerNamespace("Mediachase");

var _leftMenuResizer_DefaultElementSize = 37; // 25;
var _leftMenuResizer_MaxHeight = 200;
var _leftMenuResizer_IconDivHeight = '32px'; //'20px';

// JsLeftTemplateResizer Constructor
Mediachase.JsLeftTemplateResizer = function(element) 
{
    Mediachase.JsLeftTemplateResizer.initializeBase(this, [element]);
    
    //internal variables
    var leftDiv = null;
    
    var resizeBottomPanel = null;
    var topPanel = null;
    var bottomPanel = null;
    
    //bottom new div
    var iconDiv = null;    
    var iconOuterDiv = null;
    var scrollProxy = null;
    //bottom new images
    var leftImage = null;
    var rightImage = null;    
    
    var topHeightPanel = null;
    
    var imageBaseUrl = null; // path to Image dir
    
    var maxHeight = null;
    var elemDefaultSize = null;
    
    var fullShowElems = null;
    var shorShowElems = null;
    
    var tempStorage = null;
    
    var _resizeHandlerDelegate = null;    
}


Mediachase.JsLeftTemplateResizer.prototype = 
{
	// -========= Properties =========-		
	get_leftDiv: function () {
		return this.leftDiv;
	},
	set_leftDiv: function (value) {
		this.leftDiv = value;
	},	
	get_resizeBottomPanel: function () {
		return this.resizeBottomPanel;
	},
	set_resizeBottomPanel: function (value) {
		this.resizeBottomPanel = value;
	},
	get_topPanel: function () {
		return this.topPanel;
	},
	set_topPanel: function (value) {
		this.topPanel = value;
	},
	get_bottomPanel: function () {
		return this.bottomPanel;
	},
	set_bottomPanel: function (value) {
		this.bottomPanel = value;
	},		
	get_iconDiv: function () {
		return this.iconDiv;
	},
	set_iconDiv: function (value) {
		this.iconDiv = value;
	},			
	get_imageBaseUrl: function () {
		return this.imageBaseUrl;
	},
	set_imageBaseUrl: function (value) {
		this.imageBaseUrl = value;
	},			
	
	// -========= Methods =========-
	// ctor()
	initialize : function() {
		//TODO: create and init
        Mediachase.JsLeftTemplateResizer.callBaseMethod(this, 'initialize');
        
		this.maxHeight = _leftMenuResizer_MaxHeight;
        this.elemDefaultSize = _leftMenuResizer_DefaultElementSize;
        
        this.tempStorage = new Array();       
        
        this.getElements();
        this.generateLayout();      
        
        this._resizeHandlerDelegate = Function.createDelegate(this, this.resizeHandler);
        $addHandler(window, "resize", this._resizeHandlerDelegate);
        window.setTimeout(Function.createDelegate(this, this.doResize), 250, null);
        window.setTimeout(Function.createDelegate(this, this.resizeHandler), 500, null);
        //this.resizeHandler(null);
    },
    
    dispose: function() {
    
		if (this.iconOuterDiv)
		{
			if (this.leftImage)
				this.iconOuterDiv.removeChild(this.leftImage);
			if (this.rightImage)
				this.iconOuterDiv.removeChild(this.rightImage);
			if (this.iconDiv)
				this.iconOuterDiv.removeChild(this.iconDiv);
			if (this.leftDiv)
				this.leftDiv.removeChild(this.iconOuterDiv);
		}
		this.iconDiv = null;
		this.leftDiv = null;
		
		if (this._resizeHandlerDelegate)
			$removeHandler(window, "resize", this._resizeHandlerDelegate);
		
		Mediachase.JsLeftTemplateResizer.callBaseMethod(this, 'dispose');
    },
    
    generateLayout: function()
    {
		//alert('gen Layout in');
		//TODO: generate layout
		// <div outerDiv width="100%">
		//    <img left>
		//		<div.iconDiv>			<--- scrolling is here (fixed width)
		//			<div scrollProxy>   <-- element has max width (items container)
		//			...items...
		//			</div scrollProxy>
		//		</div icondiv>
		//    <img right>				
		// </div> (outerDiv)
		this.scrollProxy = document.createElement('DIV');
		this.scrollProxy.style.width = '1000px';
		
		this.iconOuterDiv = document.createElement('DIV');
		//this.iconOuterDiv.style.width = '100%';
		this.iconOuterDiv.className = 'NavigationNavBarItem';
		this.iconOuterDiv.style.height = this.elemDefaultSize + 'px';		
		
		this.iconDiv = document.createElement('DIV');
		this.iconDiv.id = 'iconDiv_leftTemplate';
		this.iconDiv.style.width = '100%';
		this.iconDiv.style.height = _leftMenuResizer_IconDivHeight;
		this.iconDiv.style.cssFloat = 'left';
		this.iconDiv.style.styleFloat = 'left';
		this.iconDiv.style.overflow = 'hidden';
		
//		this.fullShowElems = this.leftDiv.childNodes.length;
		//this.fullShowElems = 1;
//		for (var i = 0; i < this.leftDiv.childNodes.length; i++)
//		{
//			if (this.leftDiv.childNodes[i].nodeType == 1 && this.leftDiv.childNodes[i].tagName == 'DIV')
//				this.fullShowElems++;
//		}
		
		var obj = this.leftDiv.firstChild;
		var tmpObj = null;
		while (obj != null)
		{
			if (obj.nodeType != 1)
			{
				tmpObj = obj;
				obj = obj.nextSibling;
				tmpObj.parentNode.removeChild(tmpObj);
				continue;
			}
			obj = obj.nextSibling;
		}
		
		this.fullShowElems = this.leftDiv.childNodes.length;

		this.shortShowElems = 0;
		
		this.leftImage = document.createElement('IMG');
		this.rightImage = document.createElement('IMG');
		
		var delegateLeft = Function.createDelegate(this, this.doScrolling);
		
		this.leftImage.src = this.imageBaseUrl + 'page-prev-disabled.gif';
		this.leftImage.style.cssFloat = 'left';	
		this.leftImage.style.styleFloat = 'left';			
		this.leftImage.style.paddingTop = '3px';
		this.leftImage.onclick = function() { delegateLeft('left');}
		
		this.rightImage.src = this.imageBaseUrl + 'page-next-disabled.gif';
		this.rightImage.style.cssFloat = 'right';
		this.rightImage.style.styleFloat = 'right';
		this.rightImage.style.paddingTop = '3px';
		this.rightImage.onclick = function() { delegateLeft('right');}
		
		this.iconDiv.appendChild(this.scrollProxy);
		
		this.iconOuterDiv.appendChild(this.leftImage);
		this.iconOuterDiv.appendChild(this.iconDiv);
		this.iconOuterDiv.appendChild(this.rightImage);		
		
		
		this.leftDiv.appendChild(this.iconOuterDiv);
		
		this.iconDiv.style.width = this.iconDiv.offsetWidth - 34 + 'px'; //16*2 + 2 (size of left img + right img)
    },      
    
    //get ico from <div> template element
    getIconFromItem: function(obj)
    {
		var retVal = null;
		var icons = obj.getElementsByTagName('IMG');
		if (icons.length > 0)
		{
			retVal = icons[0];
			retVal.onclick = obj.onclick;
			retVal.style.paddingLeft = '3px';
			retVal.style.paddingTop = '3px';
			return retVal;
		}
		else
		{
			return obj;
		}
    },
    
    getElements: function()
    {
		if (this._element && this._element.childNodes.length > 1)
		{
			this.topPanel = this._element.childNodes[0];
			this.bottomPanel = this._element.childNodes[1];
			this.resizeBottomPanel = this.leftDiv.parentNode;
			
			var tmpElem = this.topPanel;
			while (tmpElem.firstChild != null)
			{
				if (tmpElem.style.height != '')
					break;
					
				tmpElem = tmpElem.firstChild;
			}
			
			this.topHeightPanel = tmpElem;
		}
    },
    
    // perform scroliing with animate
    // and also if called with string.Empty validate images for scrolling (enable/disable)
    doScrolling: function(direction)
    {
		var attributes = null;

		if (direction == 'left')
		{
			attributes = 
			{ 
				scroll: { to: [this.iconDiv.scrollLeft - 33, 0] }
			};			
		}
		else if (direction == 'right')
		{			
			if (this.rightImage.src.indexOf('page-next-disabled.gif') == -1)
			{
				attributes = 
				{ 
					scroll: { to: [this.iconDiv.scrollLeft + 33, 0] }
				};
			}
		}
		
		if (direction != '')
		{			
			var anim = new YAHOO.util.Scroll(this.iconDiv, attributes);			
			anim.duration = 0.45;			
			anim.animate();
			var _delegate = Function.createDelegate(this, this.doScrolling);
			window.setTimeout( function () { _delegate(''); }, 550);
		}
		
		//validate icons
		var maxScroll = 0;
		
		for (var i = 0; i < this.scrollProxy.childNodes.length; i++)
		{
			maxScroll += this.scrollProxy.childNodes[i].offsetWidth;
		}
		
		maxScroll -= this.iconDiv.offsetWidth;	
		
		if (maxScroll <= 0)
		{
			this.leftImage.src = this.imageBaseUrl + 'page-prev-disabled.gif';
			this.rightImage.src = this.imageBaseUrl + 'page-next-disabled.gif';
		}
		else
		{
			if (this.iconDiv.scrollLeft + 20 <= maxScroll)
				this.rightImage.src = this.imageBaseUrl + 'page-next.gif';
			else
				this.rightImage.src = this.imageBaseUrl + 'page-next-disabled.gif';
			
			if (this.iconDiv.scrollLeft > 0)	
				this.leftImage.src = this.imageBaseUrl + 'page-prev.gif';
			else
				this.leftImage.src = this.imageBaseUrl + 'page-prev-disabled.gif';
		}
		
		
    },   
    
    //perform resize and recount height of all panels
    doResize: function(incSize)
    {
		var wrongDetected = null;
		//alert('container height: ' + this.leftDiv.offsetHeight + ' | offsetTop = ' + this.iconOuterDiv.offsetTop);		
		if (this.bottomPanel)
		{
			var elem = Ext.get(this.topHeightPanel.id).query("div[class=x-panel x-panel-noborder x-tree] > div:last-child > div{height*=px}");			
			
			//for detecting bug with resize
			if (elem.length > 0)
				wrongDetected = elem[0].offsetHeight >= this.topHeightPanel.offsetHeight || elem[0].offsetWidth != this.topHeightPanel.offsetWidth;
			
			if (this.topHeightPanel.offsetHeight == 0)
				wrongDetected = true;			
			//alert('elem_inner.offsetHeight = ' + elem[0].offsetHeight + ' | topHeightPanel.offsetHeight = ' + this.topHeightPanel.offsetHeight);
			
			if (incSize == null)
			{
				if (parseInt(this.bottomPanel.style.top) - this.elemDefaultSize > 0)
					this.bottomPanel.style.top = parseInt(this.bottomPanel.style.top) - this.elemDefaultSize + 'px';
					
				if (parseInt(this.resizeBottomPanel.style.height) + this.elemDefaultSize > 0)
					this.resizeBottomPanel.style.height = parseInt(this.resizeBottomPanel.style.height) + this.elemDefaultSize + 'px';				
				
				if (parseInt(this.topHeightPanel.style.height) - this.elemDefaultSize > 0)
					this.topHeightPanel.style.height = parseInt(this.topHeightPanel.style.height) - this.elemDefaultSize + 'px';
					
				if (elem.length > 0 && parseInt(elem[0].style.height) >= this.elemDefaultSize)
				{
					elem[0].style.height = parseInt(elem[0].style.height) - this.elemDefaultSize + 'px';
				}				
			}
			else
			{				
				//if (incSize != 0) alert('resize for ' + incSize);
				if (incSize < 0)
				{	
					this.bottomPanel.style.top = parseInt(this.bottomPanel.style.top) - incSize + 'px';
					this.resizeBottomPanel.style.height = parseInt(this.resizeBottomPanel.style.height) + incSize + 'px';
					this.topHeightPanel.style.height = parseInt(this.topHeightPanel.style.height) - incSize + 'px';
				}
				else
				{
					this.bottomPanel.style.top = parseInt(this.bottomPanel.style.top) - incSize + 'px';
					this.resizeBottomPanel.style.height = parseInt(this.resizeBottomPanel.style.height) + incSize + 'px';
					this.topHeightPanel.style.height = parseInt(this.topHeightPanel.style.height) - incSize + 'px';
					
					this.leftImage.nextSibling.scrollLeft = 0;
					this.doScrolling('');
				}
				
				if (elem.length > 0)
				{
					elem[0].style.height = parseInt(elem[0].style.height) - incSize + 'px';
					
					//if true then bug "first run with small window" occurs. Resize some elements manually
					var delta = parseInt(this.topPanel.parentNode.style.height) - ( parseInt(this.topHeightPanel.style.height) + parseInt(this.leftDiv.parentNode.style.height));
					//alert(delta);
					if (wrongDetected || delta != 0)
					{						
						//alert('wrong: ' + delta + ' 1: ' + this.topPanel.parentNode.style.height + ' 2: ' + this.topHeightPanel.style.height + ' 3: ' + this.leftDiv.parentNode.style.height);
						
						if (delta != 0)
						{
							this.bottomPanel.style.top = parseInt(this.bottomPanel.style.top) + delta + 'px';
							//elem[0].style.height = parseInt(elem[0].style.height) + incSize + 'px';
						}
						
						//perform some manual resize
						elem[0].style.width = this.bottomPanel.style.width;
						elem[0].parentNode.parentNode.style.width = this.bottomPanel.style.width;
						
						if (parseInt(this.bottomPanel.style.top) - parseInt(elem[0].parentNode.previousSibling.offsetHeight) >= 0)
							elem[0].style.height = parseInt(this.bottomPanel.style.top) - parseInt(elem[0].parentNode.previousSibling.offsetHeight) + 'px';
						else
							elem[0].style.height = '0px';
							
						this.topHeightPanel.style.height = parseInt(this.bottomPanel.style.top) + 'px';
						this.topPanel.style.height = parseInt(this.bottomPanel.style.top) + 'px';
						//additional recalculate small icons and big elements
						this.resizeHandler(null);
					}
				}					
			}
		}
		//this.resizeHandler(null);
    },
    
    //create outer div for small image shortcut in bottom panel
    createOuterDiv: function()
    {
		var retVal = document.createElement('DIV');
		retVal.className = 'NavigationNavBarItemSmall';
		retVal.onclick = function () 
		{ 
			var obj = this.parentNode;
			for (var i = 0; i < obj.childNodes.length; i++)
			{
				obj.childNodes[i].className= 'NavigationNavBarItemSmall';
				for (var j = 0; j < obj.childNodes[i].childNodes.length; j++)
					obj.childNodes[i].childNodes[j].className = '';
			}
			
			this.className = 'NavigationNavBarItemSmallActive';
		};
		
		retVal.onmousemove = function()
		{
			if (this.className != 'NavigationNavBarItemSmallActive')
				this.className = 'NavigationNavBarItemSmallHover';
		}
		
		retVal.onmouseout = function()
		{
			if (this.className != 'NavigationNavBarItemSmallActive')
				this.className = 'NavigationNavBarItemSmall';
		}		
		return retVal;
    },
        
    getDomElement: function(obj, direction)
    {
		if (direction == null || direction == 'left')
		{
			while (obj.previousSibling != null)
			{
				if (obj.nodeType == 1 && obj.tagName == 'DIV')
					return obj;
					
				obj = obj.previousSibling;
			}
		}
		else if (direction == 'right')
		{
			while (obj != null)
			{
				if (obj.nodeType == 1 && obj.tagName == 'DIV' && obj.nextSibling == null)
					return obj;
					
				obj = obj.nextSibling;
			}			
		}
		
		return null;
    },
   
    resizeHandler: function(e)
    {
		//ie bug fix
		if (document.all && e != null)
		{			
			window.setTimeout(Function.createDelegate(this, this.resizeHandler), 150, null);
		}
		
		if (this.topHeightPanel.offsetHeight < this.maxHeight)
		{
			var deltaY = this.maxHeight - this.topHeightPanel.offsetHeight;
			//alert('deltaY = ' + deltaY);
			var numberToHide = Math.round(deltaY / this.elemDefaultSize);
			if (numberToHide > this.fullShowElems)
				numberToHide = this.fullShowElems;
			
			if (numberToHide <= this.fullShowElems && numberToHide != 0)
			{
				//alert('try to hide ' + numberToHide + 'node(s)');
				for (var i = 0; i < numberToHide; i++)
				{
					var oldNode = this.getDomElement(this.leftDiv.childNodes[this.fullShowElems - i - 1], 'left');
					
					if (oldNode == null)
						oldNode = this.leftDiv.childNodes[this.fullShowElems - i - 1];
					
					if (oldNode != null)
					{					
						oldNode = this.leftDiv.removeChild(oldNode);
						this.tempStorage.push(oldNode.cloneNode(true));
						
						//alert('BEFORE: ' + oldNode.innerHTML);
						oldNode = this.getIconFromItem(oldNode);
						var newDiv = this.createOuterDiv();
						newDiv.appendChild(oldNode);
						
						//alert('AFTER: ' + this.tempStorage[this.tempStorage.length - 1].innerHTML);
						this.scrollProxy.appendChild(newDiv);
					}
				}
				
				this.fullShowElems -= numberToHide;
				this.shortShowElems += numberToHide;
			}
			
			this.doResize(-numberToHide*this.elemDefaultSize);
		}
		else
		{
			if (this.shortShowElems > 0)
			{
				var deltaY = this.topHeightPanel.offsetHeight - this.maxHeight;
				//alert('deltaY = ' + deltaY);
				if (deltaY > this.elemDefaultSize)
				{
					var numberToShow = Math.round(deltaY / this.elemDefaultSize);
					if (numberToShow > this.shortShowElems)
						numberToShow = this.shortShowElems;
						
					if (numberToShow != 0 && numberToShow <= this.shortShowElems)
					{
						//alert('!!! try to show '+ numberToShow +' node(s); total: ' + this.shortShowElems);
						for (var i = 0; i < numberToShow; i++)
						{
							var newNode = this.getDomElement(this.scrollProxy.childNodes[0], 'right');
														
							if (newNode != null)
							{								
								newNode = this.scrollProxy.removeChild(newNode);
								
								if (this.tempStorage.length > 0)
									newNode = this.tempStorage.pop();
								else
									alert('Invalid temporary storage (length == 0)');
									
								this.leftDiv.insertBefore(newNode, this.iconOuterDiv);
							}
						}
						
						this.fullShowElems += numberToShow;
						this.shortShowElems -= numberToShow;
					}
					//alert('number = ' + numberToShow + ' | defaultSize = ' + this.elemDefaultSize);
					this.doResize(numberToShow*this.elemDefaultSize);					
				}
			}
		}
		this.doScrolling('');
    }
}

Mediachase.JsLeftTemplateResizer.registerClass("Mediachase.JsLeftTemplateResizer", Sys.UI.Control);
if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();