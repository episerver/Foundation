Type.registerNamespace("Ibn");

// -------------------------------------
// ------- TableLayoutExtender ---------
// -------------------------------------

// TextboxExtender Constructor
Ibn.TableLayoutExtender = function(element) 
{
    Ibn.TableLayoutExtender.initializeBase(this, [element]);
    
    var storage = null;
    var _selection = null;
    var _className = null;
    
    //store previous selected item
    var _prevSelection = null;
    var _prevSelectionCss = null;
    
    var _hoverCss = null;
    var _selectedCss = null;
    //internals
}

Ibn.TableLayoutExtender.prototype = 
{
	// -========= Properties =========-	
	get_storage: function () {
		return this.storage;
	},
	set_storage: function (value) {
		this.storage = value;
	},
	
	get_selection: function () {
		return this._selection;
	},
	set_selection: function (value) {
		this._selection = value;
	},	
	
	get_className: function () {
		return this._className;
	},
	set_className: function (value) {
		this._className = value;
	},	
	
	get_hoverCss: function () {
		return this._hoverCss;
	},
	set_hoverCss: function (value) {
		this._hoverCss = value;
	},
	
	get_selectedCss: function () {
		return this._selectedCss;
	},
	set_selectedCss: function (value) {
		this._selectedCss = value;
	},
	
	// ctor()
	initialize : function() 
	{	
	    
		Ibn.TableLayoutExtender.callBaseMethod(this, 'initialize');		
		this._prevSelectionCss = "";
		$addHandler(this._element, "click", Function.createDelegate(this, this.onClickHandler));
		
		$addHandler(this._element, "mouseover", Function.createDelegate(this, this.onMouseOverHandler));
		$addHandler(this._element, "mouseout", Function.createDelegate(this, this.onMouseOutHandler));
		
		if (this.storage)
		{
		    var obj = document.getElementById(this.storage.value.split('^')[0]);
		    this._selection = this.storage.value.split('^')[1];
		    this._className = this.storage.value.split('^')[3];
		    if (obj)
		    {
		        
		        this._prevSelection = obj;
		        this._prevSelectionCss = this.storage.value.split('^')[2];
		        
		        if(this._hoverCss)
					this._prevSelectionCss = this._prevSelectionCss.replace(this._hoverCss, "");
		        obj.className = this._prevSelectionCss + " " + this._selectedCss;
		    }
		}
	},
	dispose: function()
	{
	    $clearHandlers(this._element);
		Ibn.TableLayoutExtender.callBaseMethod(this, 'dispose');
	},
	
	onClickHandler: function(e)
	{
	    var elem = e.target;
	    
	    while  (elem.parentNode != null && elem.tagName != "BODY")
	    {
	        if (elem.getAttribute("mc_tableitem_uid"))
	        {	            	            	                
	            if (this._prevSelection)
	            {
	                this._prevSelection.className = this._prevSelection.className.replace(this._selectedCss, "");// = this._prevSelectionCss;
//	            
//					if(this._prevSelection.className == "" && this._prevSelectionCss != "")
//					{
//						var className = this._prevSelectionCss;
//						if(className.indexOf(" ")>=0)
//							className = className.substr(0, className.indexOf(" "));
//						this._prevSelection.className = className;
//					}
	            }
	            if (this.storage)
	                this.storage.value = elem.id + "^" + elem.getAttribute("mc_tableitem_uid") + "^" + elem.className+ "^" + elem.getAttribute("mc_tableitem_class");
	                
	            this._prevSelectionCss = elem.className;

	            elem.className += " " + this._selectedCss;
	            this._selection = elem.getAttribute("mc_tableitem_uid");
	            this._className = elem.getAttribute("mc_tableitem_class");
	            
	            this._prevSelection = elem;
	            return;
	        }
	        
	        elem = elem.parentNode;
	    }
	    
	    this._selection = "0";
	},		
	
	onMouseOverHandler: function(e)
	{
	    var elem = e.target;
	    if(this._hoverCss)
	    {
			var val = elem.getAttribute("mc_tableitem_class");
			if(val)
				elem.className += " " + this._hoverCss;
			else
			{
				if (elem.getAttribute("mc_tableitem_noneselect"))
						return;
						
				var obj = elem.parentNode;
				while (obj && obj.parentNode != null)
				{
					if (obj.nodeType == 1 && obj.tagName == 'FORM')
						return;
					
					if (obj.getAttribute("mc_tableitem_noneselect"))
						return;

					if (obj.getAttribute("mc_tableitem_class"))
					{
						obj.className += " " + this._hoverCss;
						return;
					}

					obj = obj.parentNode;
				} 
			}
		}
	},		
	
	onMouseOutHandler: function(e)
	{
	    var elem = e.target;
	    if(this._hoverCss)
	    {
			var val = elem.getAttribute("mc_tableitem_class");
			if(val)
				elem.className = elem.className.replace(this._hoverCss, "");
			else
			{
				if (elem.getAttribute("mc_tableitem_noneselect"))
						return;
						
				var obj = elem.parentNode;
				while (obj && obj.parentNode != null)
				{
					if (obj.nodeType == 1 && obj.tagName == 'FORM')
						return;
						
					if (obj.getAttribute("mc_tableitem_noneselect"))
						return;

					if (obj.getAttribute("mc_tableitem_class"))
					{
						obj.className = obj.className.replace(this._hoverCss, "");
						return;
					}

					obj = obj.parentNode;
				} 
			}
		}
	},		
	
	getSelection: function()
	{
	    return this._selection;
	},
	
	getClassName: function()
	{
	    return this._className;
	}
	
	
}	


Ibn.TableLayoutExtender.registerClass("Ibn.TableLayoutExtender", Sys.UI.Control);
if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();	