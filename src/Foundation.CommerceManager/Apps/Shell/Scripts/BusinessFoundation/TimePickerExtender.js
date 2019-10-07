Type.registerNamespace("Ibn");

// ---------------------------------
// ------- TimePickerExtender ---------
// ---------------------------------

// TimePickerExtender Constructor
Ibn.TimePickerExtender = function(element) 
{
    Ibn.TimePickerExtender.initializeBase(this, [element]);
    
    //internals
    this._element = element;
    this._timeValue = null;
    this._enabled = true;
    this._isAM = false;
    this._divCssClass = "";
    this._storeValue = null;
    
    this._isOpen = false;
    this._width = 95;
    this._height = 120;
    this._popupDiv = null;
    this._innerContainer = null;
    this._updateElement = null;
    
    this._calendar = null;
    this._calendarHandler = null;
    
    this._blur = new Ibn.DeferredOperation(((Sys.Browser.agent === Sys.Browser.Safari) ? 1000 : 100), this, this._onBlur);
    this._focus = new Ibn.DeferredOperation(((Sys.Browser.agent === Sys.Browser.Safari) ? 1000 : 100), this, this._onFocus);
    this._slowScroll = new Ibn.DeferredOperation(((Sys.Browser.agent == Sys.Browser.InternetExplorer) ? 100 : 1), this, this._setScroll);
    
    this._element$delegates = {
        focus : Function.createDelegate(this, this._element_onFocus),
        focusout : Function.createDelegate(this, this._element_onBlur),
        blur : Function.createDelegate(this, this._element_onBlur),
        keyup : Function.createDelegate(this, this._element_onkeyup)
    }
    
    this._cell$delegates = {
        mouseover : Function.createDelegate(this, this._cell_onmouseover),
        mouseout : Function.createDelegate(this, this._cell_onmouseout),
        click : Function.createDelegate(this, this._cell_onclick)
    }
    
    this._popup$delegates = { 
		activate : Function.createDelegate(this, this._popup_onfocus),
        focus : Function.createDelegate(this, this._popup_onfocus),
        dragstart: Function.createDelegate(this, this._popup_ondragstart),
        select: Function.createDelegate(this, this._popup_onselect)
    }
}

Ibn.TimePickerExtender.prototype = 
{
	// -========= Properties =========-	
	get_timeValue: function () {
		return this._timeValue;
	},
	set_timeValue: function (value) {
		this._timeValue = value;
	},
	
	get_enabled: function () {
		return this._enabled;
	},
	set_enabled: function (value) {
		this._enabled = value;
	},
	
	get_isAM: function () {
		return this._isAM;
	},
	set_isAM: function (value) {
		this._isAM = value;
	},
	
	get_divCssClass: function () {
		return this._divCssClass;
	},
	set_divCssClass: function (value) {
		this._divCssClass = value;
	},
	
	get_calendar: function () {
		return this._calendar;
	},
	set_calendar: function (value) {
		this._calendar = value;
	},
	
	get_storeValue: function () {
		return this._storeValue;
	},
	set_storeValue: function (value) {
		this._storeValue = value;
	},
	
	//added by DVS
	get_updateElement: function () {
		return this._updateElement;
	},
	set_updateElement: function (value) {
		this._updateElement = value;
	},	
	// -========= Methods =========-
	// ctor()
	initialize : function() 
	{		
		Ibn.TimePickerExtender.callBaseMethod(this, 'initialize');
		
		if(this._element)
			$addHandlers(this._element, this._element$delegates);
		
		if (this._calendar) {
			this._calendarHandler = Function.createDelegate(this, this._onCalendarEditHandler);
			this._calendar.add_editValue(this._calendarHandler);
		}
		
		if(this._timeValue)
			this._element.value = this._timeValue;
		
		this._ensureTimeString();
		if(this._calendar)
			this._calendar._ensureDateString();
	},
	
	dispose: function() {
		if (this._calendar && this._calendarHandler) {
			this._calendar.remove_editValue(this._calendarHandler);
		}

		if (this._element) {
			$clearHandlers(this._element);
		}
		if(this._innerContainer)
			this._innerContainer = null;
		if(this._popupDiv)
			this._popupDiv = null;
		this._calendar = null;
		
		Ibn.TimePickerExtender.callBaseMethod(this, 'dispose');
	},
	
	_onCalendarEditHandler : function(obj, args) {
		if(!this._enabled)
			return;
		var elt = obj.get_element();
		if(elt && elt.value == "")
		{
			if(this._element.value != "")
				this._storeValue = this._element.value;
			this._element.value = "";
			this._element.disabled = true;
		}
		else
		{
			if (typeof(this._element) == 'undefined' && this._updateElement)
				this._element = $get(this._updateElement.replace(/\$/g,'_'));
			this._element.disabled = false;
			if(this._storeValue != null && this._element.value == "")
			{
				this._element.value = this._storeValue;
				this._ensureTimeString();
			}
		}
	},
	
	_onBlur : function() {
        this._focus.cancel();
        this._hide(); 
        this._ensureTimeString();       
    },
    _onFocus : function() {
        this._blur.cancel();
        this._element.focus();
    },
	
	_element_onFocus : function(e) {
        if (this._enabled) {
            this._focus.cancel();
            this._blur.cancel();
            this._show();
        }
    },
    _element_onBlur : function(e) {
        if ((e.type == 'blur' && Sys.Browser.agent != Sys.Browser.InternetExplorer) ||
            (e.type == 'focusout' && Sys.Browser.agent == Sys.Browser.InternetExplorer)) {
                this._focus.cancel();
                this._blur.post();
        }
    },
    
    _element_onkeyup : function(e) {
        if (this._enabled) {
            this._focus.cancel();
            this._blur.cancel();
            
           var _keyCode = e.keyCode;
           if(_keyCode == 38)
           {
				//todo - move div up
           }
           else if(_keyCode == 40)
           {
				//todo - move div down
           }
        }
    },
    
	_show : function() {
	
		this._ensureTicks();
		
		if (!this._isOpen) {
			var t = this._getTotalOffset(this._element);
			if(t && t.Left)
				this._popupDiv.style.left = t.Left + "px";
			if(t && t.Top)
				this._popupDiv.style.top = t.Top + this._element.offsetHeight + 1 + "px";
			
			this._popupDiv.style.display = "";
			this._isOpen = true;
			this._ensureSelected();
		}
	},
	
	_getTotalOffset : function(eSrc) {
		var retVal = new Object();
		retVal.Top = 0;
		retVal.Left = 0;
		while (eSrc)
		{
			retVal.Top += eSrc.offsetTop;
			retVal.Left += eSrc.offsetLeft;
			eSrc = eSrc.offsetParent;
		}
		return retVal;
	},
	
	_hide : function() {
		if(this._popupDiv)
		{
			this._popupDiv.style.display = "none";
		}
		this._isOpen = false;
	},
	
	_ensureTicks : function() {    
		if (!this._popupDiv) {
			this._buildPopup();
		}
    },
    
    _isNearest : function(elemValue) {
		var retVal = false;
		
		var sH = "";
		var sM = "";
		var sAM = "";
		
		if(this._element.value != "")
		{
			var str = this._element.value;
			var idouble = str.indexOf(":");
			if(idouble > 0)
			{
				sH = str.substring(0, idouble);
				sM = str.substr(idouble + 1, 2);
				if(this._isAM)
					sAM = str.substr(idouble + 3);
			}
		}
		
		var sElH = "";
		var sElM = "";
		var sElAM = "";
		
		if(elemValue != "")
		{
			var str = elemValue;
			var idouble = str.indexOf(":");
			if(idouble > 0)
			{
				sElH = str.substring(0, idouble);
				sElM = str.substr(idouble + 1, 2);
				if(this._isAM)
					sElAM = str.substr(idouble + 3);
			}
		}
		if(sElH == sH && sAM == sElAM)
		{
			var iM = parseInt(sM, 10);
			var iElM = parseInt(sElM, 10);
			if(iM - iElM >= 0 && iM - iElM < 30)
				retVal = true;
		}
		return retVal;
    },
    
    _ensureSelected : function() {    
		var iSel = 0;
		var iOffHeight = 0;
		if(!this._innerContainer)
			return;
		var s = "";
		for (var i = 0; i < this._innerContainer.childNodes.length; i++) {
			var d0 = this._innerContainer.childNodes[i];
			if(this._isNearest(d0.innerHTML))
			{
				s = s + " " + i.toString();
				iSel = i;
				iOffHeight = d0.offsetHeight;
				d0.style.backgroundColor = "#0000ff";
				d0.style.color = "#ffffff";
			}
			else
			{
				d0.style.backgroundColor = "#ffffff";
				d0.style.color = "#000000";
			}
		}
		this._slowScroll.post(iSel, iOffHeight);
		
    },
    
    _setScroll : function (x,y){
		if(this._popupDiv)
			this._popupDiv.scrollTop = x*y;
    },
    
    _buildPopup : function() {
        /// <summary>
        /// Builds the calendar's layout
        /// </summary>
        
        this._popupDiv = this._createElementFromTemplate({ 
            nodeName : "div",
            events : this._popup$delegates,
            visible : true 
        }, document.body);
        
        if(this._divCssClass != "")
			this._popupDiv.className = this._divCssClass;
        this._popupDiv.style.backgroundColor = "#ffffff";
        this._popupDiv.style.border = "1px solid";
        this._popupDiv.style.borderColor = "#000000";
        this._popupDiv.style.width = this._width + "px";
        this._popupDiv.style.height = this._height + "px";
        this._popupDiv.style.overflow = "auto";
        this._popupDiv.style.zIndex = "2";
        this._popupDiv.style.position = "absolute";
        this._popupDiv.style.left = "50px";
        this._popupDiv.style.top = "50px";
        
        this._innerContainer = this._createElementFromTemplate({ 
            nodeName : "div",
            events : null, 
            visible : true
        }, this._popupDiv);
        
        for(var i=0;i<24;i++)
        {
			var d0 = this._createElementFromTemplate({ 
				nodeName : "div",
				events : this._cell$delegates, 
				visible : true
			}, this._innerContainer);
			d0.innerHTML = this._getTimeString(i.toString(),"0");
			d0.style.cursor = "pointer";
			d0.style.padding = "2px";
			d0.style.backgroundColor = "#ffffff";
			d0.style.color = "#000000";
			
			var d30 = this._createElementFromTemplate({ 
				nodeName : "div",
				events : this._cell$delegates, 
				visible : true
			}, this._innerContainer);
			d30.innerHTML = this._getTimeString(i.toString(),"30");
			d30.style.cursor = "pointer";
			d30.style.padding = "2px";
			d30.style.backgroundColor = "#ffffff";
			d30.style.color = "#000000";
        }
        
        this._popupDiv.style.display = "none";
    },
    
    _getTimeString : function(h,m) {
		var am = "";
		if(!this._isAM && parseInt(h, 10) < 10)
			h = "0" + h;
		else if(this._isAM)
		{
			if(parseInt(h, 10) == 0)
			{
				h = "12";
				am = "AM";
			}
			else if(parseInt(h, 10) < 12)
				am = "AM";
			else if(parseInt(h, 10) > 12)
			{
				h = (parseInt(h, 10) - 12).toString();
				am = "PM";
			}
			else
				am = "PM";
		}
		var retVal = h + ":";
		var sM = m;
		if(parseInt(m, 10)<10)
			sM = "0" + sM;
		retVal = retVal + sM;
		if(this._isAM)
			retVal = retVal + am;
		return retVal;
    },
    
    _ensureTimeString : function() {
		if(this._element.value == "")
			return;
		
		var str = this._element.value;
		
		var idouble = str.indexOf(":");
		if(idouble < 0)
		{
			this._element.value = "";
			return;
		}
		
		try {
			var sH = str.substring(0, idouble);
			if(sH == "")
				sH = "0";
			var sM = str.substr(idouble + 1, 2);
			if(sM == "")
				sM = "0";
			if(isNaN(parseInt(sM, 10)))
			{
				this._element.value = "";
				return;
			}
			if(parseInt(sH, 10) > 24 || parseInt(sH, 10)<0)
			{
				this._element.value = "";
				return;
			}
			if(parseInt(sM, 10)>59 || parseInt(sM, 10)<0)
			{
				this._element.value = "";
				return;
			}
			if(parseInt(sM, 10)<10)
				sM = "0" + parseInt(sM, 10).toString();
				
			var sAM = str.substr(idouble + 3);
			sAM = sAM.replace(" ", "");
			if(this._isAM)
			{
				if(parseInt(sH, 10) == 0)
				{
					sH = "12";
					sAM = "AM";
				}
				if(sAM == "")
				{
					if(parseInt(sH, 10) < 12)
						sAM = "AM";
					else
						sAM = "PM";
				}
				if(sAM.toLowerCase() == "am")
					sAM = "AM";
				else if(sAM.toLowerCase() == "pm")
					sAM = "PM";
				else
					sAM = "AM";
				
				if(parseInt(sH, 10)<10)
					sH = parseInt(sH, 10).toString();
				if(parseInt(sH, 10)>12)
				{
					sH = (parseInt(sH, 10) - 12).toString();
					sAM = "PM";
				}
			}
						
			if(!this._isAM)
			{
				if(sAM.toLowerCase() == "am")
				{
					if(parseInt(sH, 10) == 12)
						sH = "00";
				}
				if(sAM.toLowerCase() == "pm")
				{
					if(parseInt(sH, 10) < 12)
						sH = (parseInt(sH, 10) + 12).toString();
				}
				
				if(parseInt(sH, 10)<10)
					sH = "0" + parseInt(sH, 10).toString();
					
				sAM = "";
			}
			
			this._element.value = sH + ":" + sM + sAM.toUpperCase();
		}
		catch(e)
		{
			this._element.value = "";
			return;
		}
    },
    
    _cell_onmouseover : function(e) {
        for (var i = 0; i < this._innerContainer.childNodes.length; i++) {
			var d0 = this._innerContainer.childNodes[i];
			d0.style.backgroundColor = "#ffffff";
			d0.style.color = "#000000";
        }

        var target = e.target;

		target.style.backgroundColor = "#0000ff";
		target.style.color = "#ffffff";
			
        e.stopPropagation();
    },
    
    _cell_onmouseout : function(e) {
        var target = e.target;

		target.style.backgroundColor = "#ffffff";
		target.style.color = "#000000";

        e.stopPropagation();
    },
    
     _cell_onclick : function(e) {
		if ((Sys.Browser.agent === Sys.Browser.Safari) ||
            (Sys.Browser.agent === Sys.Browser.Opera)) {
            this._popup_onfocus(e);
        }
        
		var target = e.target;
		this._element.value = target.innerHTML;
		this._focus.cancel();
		this._blur.post();
		//this._hide();
		e.stopPropagation();
        e.preventDefault();
        
        if (this._updateElement)
        {
			__doPostBack(this._updateElement, '');
        }
     },
    
    _popup_onfocus : function(e) {
		if ((e.type == 'focus' && Sys.Browser.agent != Sys.Browser.InternetExplorer) ||
            (e.type == 'activate' && Sys.Browser.agent == Sys.Browser.InternetExplorer) ||
            (Sys.Browser.agent === Sys.Browser.Safari) ||
            (Sys.Browser.agent === Sys.Browser.Opera)) {
            if (this._isOpen) {
                this._blur.cancel();
                this._focus.post();
            }
        }
    },
    
    _popup_ondragstart : function(e) {
        e.stopPropagation();
        e.preventDefault();
    },
    _popup_onselect : function(e) {
        e.stopPropagation();
        e.preventDefault();
    },
    
    _createElementFromTemplate : function(template, appendToParent, nameTable) {
       // if we wish to override the name table we do so here
        if (typeof(template.nameTable)!='undefined') {
            var newNameTable = template.nameTable;
            if (String.isInstanceOfType(newNameTable)) {
                newNameTable = nameTable[newNameTable];
            }
            if (newNameTable != null) {
                nameTable = newNameTable;
            }
        }
        
        // get a name for the element in the nameTable
        var elementName = null;
        if (typeof(template.name)!=='undefined') {
            elementName = template.name;
        }
        
        // create or acquire the element
        var elt = document.createElement(template.nodeName);
        
        // if our element is named, add it to the name table
        if (typeof(template.name)!=='undefined' && nameTable) {
            nameTable[template.name] = elt;
        }
        
        // if we wish to supply a default parent we do so here
        if (typeof(template.parent)!=='undefined' && appendToParent == null) {
            var newParent = template.parent;
            if (String.isInstanceOfType(newParent)) {
                newParent = nameTable[newParent];
            }
            if (newParent != null) {
                appendToParent = newParent;
            }
        }
        
        // events are added to the dom element using $addHandlers
        if (typeof(template.events)!=='undefined' && template.events != null) {
            $addHandlers(elt, template.events);
        }
        
        // if the element is visible or not its visibility is set
        if (typeof(template.visible)!=='undefined' && template.visible != null) {
            if(template.visible)
				elt.style.display = "";
			else
				elt.style.display = "none";
        }
        
        // if we have an appendToParent we will now append to it
        if (appendToParent) {
            appendToParent.appendChild(elt);
        }

        // if we have child templates, process them
        if (typeof(template.children)!=='undefined' && template.children != null) {
            for (var i = 0; i < template.children.length; i++) {
                var subtemplate = template.children[i];
                createElementFromTemplate(subtemplate, elt, nameTable);
            }
        }
        
        // if we have a content presenter for the element get it (the element itself is the default presenter for content)
        var contentPresenter = elt;
        if (typeof(template.contentPresenter)!=='undefined' && template.contentPresenter != null) {
            contentPresenter = nameTable[contentPresenter];
        }
        
        // if we have content, add it
        if (typeof(template.content)!=='undefined' && template.content != null) {
            var content = template.content;
            if (String.isInstanceOfType(content)) {
                content = nameTable[content];
            }
            if (content.parentNode) {
                wrapElement(content, elt, contentPresenter);
            } else {
                contentPresenter.appendChild(content);
            }
        }
        
        // return the created element
        return elt;
    },
    
    wrapElement : function(innerElement, newOuterElement, newInnerParentElement) {
        var parent = innerElement.parentNode;
        parent.replaceChild(newOuterElement, innerElement);        
        (newInnerParentElement || newOuterElement).appendChild(innerElement);
    }
}

Ibn.TimePickerExtender.registerClass("Ibn.TimePickerExtender", Sys.UI.Control);

//Threading
Ibn.DeferredOperation = function(delay, context, callback) {
    this._delay = delay;
    this._context = context;
    this._callback = callback;
    this._completeCallback = null;
    this._errorCallback = null;
    this._timer = null;
    this._callArgs = null;
    this._isComplete = false;
    this._completedSynchronously = false;
    this._asyncResult = null;
    this._exception = null;
    this._throwExceptions = true;
    this._oncomplete$delegate = Function.createDelegate(this, this._oncomplete);
    
    // post to ensure that attaching it always gets the port as its context
    this.post = Function.createDelegate(this, this.post);
}
Ibn.DeferredOperation.prototype = {
    
    get_isPending : function() { 
        /// <summary>
        /// Gets whether there is an asynchronous operation pending
        /// </summary>
        /// <returns type="Boolean" />
        
        return (this._timer != null); 
    },
    
    get_isComplete : function() { 
        /// <summary>
        /// Gets whether the asynchronous operation has completed
        /// </summary>
        /// <returns type="Boolean" />
        
        return this._isComplete; 
    },
    
    get_completedSynchronously : function() {
        /// <summary>
        /// Gets whether the operation completed synchronously
        /// </summary>
        /// <returns type="Boolean" />
        
        return this._completedSynchronously;
    },
    
    get_exception : function() {
        /// <summary>
        /// Gets the current exception if there is one
        /// </summary>
        /// <returns type="Error" />
        
        return this._exception;
    },
    
    get_throwExceptions : function() {
        /// <summary>
        /// Gets whether to throw exceptions
        /// </summary>
        /// <returns type="Boolean" />
        
        return this._throwExceptions;
    },    
    set_throwExceptions : function(value) {
        /// <summary>
        /// Sets whether to throw exceptions
        /// </summary>
        /// <param name="value" type="Boolean">True if exceptions should be thrown, otherwise false</param>
        
        this._throwExceptions = value;
    },
    
    get_delay : function() { 
        /// <summary>
        /// Gets the current delay in milliseconds
        /// </summary>
        /// <returns type="Number" integer="true" />
        
        return this._delay; 
    },
    set_delay : function(value) { 
        /// <summary>
        /// Sets the current delay in milliseconds
        /// </summary>
        /// <param name="value" type="Number" integer="true">The delay in milliseconds</param>
        
        this._delay = value; 
    },
    
    post : function(args) {
        /// <summary>
        /// A method that can be directly attached to a delegate
        /// </summary>
        /// <param name="args" type="Object" parameterArray="true">The arguments to the method</param>
        
        var ar = [];
        for (var i = 0; i < arguments.length; i++) {
            ar[i] = arguments[i];
        }
        this.beginPost(ar, null, null);
    },
    
    beginPost : function(args, completeCallback, errorCallback) {
        /// <summary>
        /// Posts a call to an async operation on this port
        /// </summary>
        /// <param name="args" type="Array">An array of arguments to the method</param>
        /// <param name="completeCallback" type="Function" optional="true" mayBeNull="true">The callback to execute after the delayed function completes</param>
        /// <param name="errorCallback" type="Function" optional="true" mayBeNull="true">The callback to execute in the event of an exception in the delayed function</param>
        
        // cancel any pending post
        this.cancel();
        
        // cache the call arguments
        this._callArgs = Array.clone(args || []);
        this._completeCallback = completeCallback;
        this._errorCallback = errorCallback;
        
        if (this._delay == -1) {            
            // if there is no delay (-1), complete synchronously
            this._oncomplete();
            this._completedSynchronously = true;
        } else {            
            // complete the post on a seperate call after a delay
            this._timer = setTimeout(this._oncomplete$delegate, this._delay);
        }
    }, 
    
    cancel : function() {
        /// <summary>
        /// Cancels a pending post
        /// </summary>
        
        if (this._timer) {
            clearTimeout(this._timer);
            this._timer = null;
        }
        this._callArgs = null;
        this._isComplete = false;
        this._asyncResult = null;
        this._completeCallback = null;
        this._errorCallback = null;
        this._exception = null;
        this._completedSynchronously = false;
    },
    
    complete : function() {
        /// <summary>
        /// Completes a pending post synchronously
        /// </summary>        
        
        if (this._timer) {
            try {
                this._oncomplete();
            } finally {
                this._completedSynchronously = true;
            }
            return this._asyncResult;
        } else if (this._isComplete) {
            return this._asyncResult;
        }
    },    
    
    _oncomplete : function() {
        /// <summary>
        /// Completes a pending post asynchronously
        /// </summary>

        var args = this._callArgs;
        var completeCallback = this._completeCallback;
        var errorCallback = this._errorCallback;
        
        // clear the post state
        this.cancel();
        try {
            // call the post callback
            if (args) {
                this._asyncResult = this._callback.apply(this._context, args);
            } else {
                this._asyncResult = this._callback.call(this._context);
            }
            this._isComplete = true;
            this._completedSynchronously = false;
            if (completeCallback) {
                completeCallback(this);
            }
        } catch (e) {
            this._isComplete = true;
            this._completedSynchronously = false;
            this._exception = e;
            if (errorCallback) {
                if (errorCallback(this)) {
                    return;
                }
            } 
            if (this._throwExceptions) {
                throw e;
            }
        }
    }
}
Ibn.DeferredOperation.registerClass("Ibn.DeferredOperation");

if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();