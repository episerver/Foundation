// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.
Type.registerNamespace("AjaxControlToolkit");

AjaxControlToolkit.MCWeekCalendarBehavior = function(element) {
    /// <summary>
    /// A behavior that attaches a calendar date selector to a textbox
    /// </summmary>
    /// <param name="element" type="Sys.UI.DomElement">The element to attach to</param>
    
    AjaxControlToolkit.MCWeekCalendarBehavior.initializeBase(this, [element]);

    this._format = "d";
    this._cssClass = "ajax__calendar";
    
    this._selectedMode = "day";
    
    this._updateElement = null;
    
    this._enabled = true;
    this._animated = true;
    this._buttonID = null;
    this._layoutRequested = 0;
    this._layoutSuspended = false;

    this._selectedDate = null;
    this._visibleDate = null;
    this._todaysDate = null;
    this._firstDayOfWeek = AjaxControlToolkit.FirstDayOfWeek.Default;

    this._popupDiv = null;
    this._prevArrow = null;
    this._prevArrowImage = null;
    this._nextArrow = null;
    this._nextArrowImage = null;
    this._title = null;
    this._today = null;
    this._daysRow = null;
    this._monthsRow = null;
    this._yearsRow = null;
    this._daysBody = null;
    this._monthsBody = null;
    this._yearsBody = null;
    this._button = null;
    
    this._popupBehavior = null;
    this._modeChangeAnimation = null;
    this._modeChangeMoveTopOrLeftAnimation = null;
    this._modeChangeMoveBottomOrRightAnimation = null;
    this._mode = "days";
    this._selectedDateChanging = false;
    this._isOpen = false;
    this._isAnimating = false;
    this._width = 170;
    this._height = 139;
    this._modes = {"days" : null, "months" : null, "years" : null};
    this._modeOrder = {"days" : 0, "months" : 1, "years" : 2 };
    
    this._editValue = null;
    
    // Safari needs a longer delay in order to work properly
    this._blur = new AjaxControlToolkit.DeferredOperation(((Sys.Browser.agent === Sys.Browser.Safari) ? 1000 : 1), this, this._onblur);
    this._focus = new AjaxControlToolkit.DeferredOperation(((Sys.Browser.agent === Sys.Browser.Safari) ? 1000 : 1), this, this._onfocus);
    
    this._button$delegates = {
        click : Function.createDelegate(this, this._button_onclick)
    }
    this._element$delegates = {
        focus : Function.createDelegate(this, this._element_onfocus),
        focusout : Function.createDelegate(this, this._element_onblur),
        blur : Function.createDelegate(this, this._element_onblur),
        change : Function.createDelegate(this, this._element_onchange)
    }
    this._popup$delegates = { 
        activate : Function.createDelegate(this, this._popup_onfocus),
        focus : Function.createDelegate(this, this._popup_onfocus),
        dragstart: Function.createDelegate(this, this._popup_ondragstart),
        select: Function.createDelegate(this, this._popup_onselect)
    }
    this._cell$delegates = {
        mouseover : Function.createDelegate(this, this._cell_onmouseover),
        mouseout : Function.createDelegate(this, this._cell_onmouseout),
        click : Function.createDelegate(this, this._cell_onclick)
    }
}
AjaxControlToolkit.MCWeekCalendarBehavior.prototype = {    

    get_animated : function() {
        /// <summary>
        /// Whether changing modes is animated
        /// </summary>
        /// <value type="Boolean" />
           
        return this._animated;
    },
    set_animated : function(value) {
        if (this._animated != value) {
            this._animated = value;
            this.raisePropertyChanged("animated");
        }
    },

    get_enabled : function() {
        /// <value type="Boolean">
        /// Whether this behavior is available for the current element
        /// </value>
           
        return this._enabled;
    },
    set_enabled : function(value) {
        if (this._enabled != value) {
            this._enabled = value;
            this.raisePropertyChanged("enabled");
        }
    },
    
    get_button : function() {
        /// <value type="Sys.UI.DomElement">
        /// The button to use to show the calendar (optional)
        /// </value>
        
        return this._button;
    },
    set_button : function(value) {
        if (this._button != value) {
            if (this._button && this.get_isInitialized()) {
                $common.removeHandlers(this._button, this._button$delegates);
            }
            this._button = value;
            if (this._button && this.get_isInitialized()) {
                $addHandlers(this._button, this._button$delegates);
            }
            this.raisePropertyChanged("button");
        }
    },

    get_format : function() { 
        /// <value type="String">
        /// The format to use for the date value
        /// </value>

        return this._format; 
    },
    set_format : function(value) { 
        if (this._format != value) {
            this._format = value; 
            this.raisePropertyChanged("format");
        }
    },
    
    get_selectedDate : function() {
        /// <value type="Date">
        /// The date value represented by the text box
        /// </value>

        if (this._selectedDate == null) {
            var elt = this.get_element();
            if (elt.value) {
                this._selectedDate = this._parseTextValue(elt.value);
            }
        }
        return this._selectedDate;
    },
    set_selectedDate : function(value) {
        var elt = this.get_element();
        if (this._selectedDate != value) {
			
			var firstValue = value;
			if(this._selectedMode == "week" &&  value.getDay() != this._firstDayOfWeek)
			{
				var dayOfWeek = value.getDay();
				if(this._firstDayOfWeek > 0 && dayOfWeek == 0)
					dayOfWeek = 7;
				var delta = dayOfWeek - this._firstDayOfWeek;
				var deltaMS = delta*24*60*60*1000;
				firstValue = new Date(Date.parse(value) - deltaMS);
			}
			//GA Added - Begin
			var displayLimits = null;
			if(this._selectedMode == "month")
			{
				displayLimits = this.getMonthViewLimits(value);
			}
			//GA Added - End
        
            this._selectedDate = firstValue;
            
            this._selectedDateChanging = true;            
            var text = "";
            if (firstValue) 
            {
				if(this._selectedMode == "week")
				{
					var endValue = new Date(Date.parse(firstValue) + 6*24*60*60*1000);
					text = firstValue.localeFormat(this._format) + " - " + endValue.localeFormat(this._format);
                }
                
                else if(this._selectedMode == "month" && displayLimits!=null)//GA Added - Begin
                {
					text = displayLimits.startDate.localeFormat(this._format) + " - " + displayLimits.endDate.localeFormat(this._format);
                } 
                else if(this._selectedMode == "workweek")
                {
					var sd = new Date(value.getTime());
					if(this._firstDayOfWeek>0 || (this._firstDayOfWeek==0 && sd.getDay()>0))
					{
						while(sd.getDay()!=1)
						{
							sd = new Date(sd.getTime()-24*60*60*1000);
						}
					}
					else
					{
						while(sd.getDay()!=1)
						{
							sd = new Date(sd.getTime()+24*60*60*1000);
						}
					}
					var ed = new Date(sd.getTime()+4*24*60*60*1000);
					text = sd.localeFormat(this._format) + " - " + ed.localeFormat(this._format);
                }
                else if(this._selectedMode == "year")
                {
					var sd = new Date(this._selectedDate.getFullYear(), 0, 1, 0,0,0);
					var ed = new Date(this._selectedDate.getFullYear(), 11, 31, 23,59,59);
					text = sd.localeFormat(this._format) + " - " + ed.localeFormat(this._format);
                }//GA Added - End
                else
					text = firstValue.localeFormat(this._format);
            } 
            if (text != elt.value) {
                elt.value = text;
                this._fireChanged();
            }
            this._selectedDateChanging = false;
            this.invalidate();
            this.raisePropertyChanged("selectedDate");
        }
    },

    get_visibleDate : function() {
        /// <summary>
        /// The date currently visible in the calendar
        /// </summary>
        /// <value type="Date" />

        return this._visibleDate;
    },
    set_visibleDate : function(value) {
        if (value) value = value.getDateOnly();
        if (this._visibleDate != value) {
            this._switchMonth(value, !this._isOpen);
            this.raisePropertyChanged("visibleDate");
        }
    },

    get_todaysDate : function() {
        /// <value type="Date">
        /// The date to use for "Today"
        /// </value>
        
        if (this._todaysDate != null) {
            return this._todaysDate;
        }
        return new Date().getDateOnly();
    },
    set_todaysDate : function(value) {
        if (value) value = value.getDateOnly();
        if (this._todaysDate != value) {
            this._todaysDate = value;
            this.invalidate();
            this.raisePropertyChanged("todaysDate");
        }
    },
    
    get_firstDayOfWeek : function() {
        /// <value type="AjaxControlToolkit.FirstDayOfWeek">
        /// The day of the week to appear as the first day in the calendar
        /// </value>
        
        return this._firstDayOfWeek;
    },
    set_firstDayOfWeek : function(value) {
        if (this._firstDayOfWeek != value) {
            this._firstDayOfWeek = value;
            this.invalidate();
            this.raisePropertyChanged("firstDayOfWeek");
        }
    },
    
    get_selectedMode : function() {
       /// <value type="Sys.UI.DomElement">
        /// mode day/week
        /// </value>
        
        return this._selectedMode;
    },
    set_selectedMode : function(value) {
        if (this._selectedMode != value) {
            this._selectedMode = value;
            this.invalidate();
            this.raisePropertyChanged("selectedMode");
        }
    },
    
    get_updateElement: function ()
	{
		return this._updateElement;
	},
	set_updateElement: function (value)
	{
		this._updateElement = document.getElementById(value);
	},		
        
    get_cssClass : function() {
        /// <value type="Sys.UI.DomElement">
        /// The CSS class selector to use to change the calendar's appearance
        /// </value>

        return this._cssClass;
    },
    set_cssClass : function(value) {
        if (this._cssClass != value) {
            if (this._cssClass && this.get_isInitialized()) {
                Sys.UI.DomElement.removeCssClass(this._container, this._cssClass);
            }
            this._cssClass = value;
            if (this._cssClass && this.get_isInitialized()) {
                Sys.UI.DomElement.addCssClass(this._container, this._cssClass);
            }
            this.raisePropertyChanged("cssClass");
        }
    },
    
    add_showing : function(handler) {
        /// <summary>
        /// Adds an event handler for the <code>showiwng</code> event.
        /// </summary>
        /// <param name="handler" type="Function">
        /// The handler to add to the event.
        /// </param>
        /// <returns />

        this.get_events().addHandler("showing", handler);
    },
    remove_showing : function(handler) {
        /// <summary>
        /// Removes an event handler for the <code>showing</code> event.
        /// </summary>
        /// <param name="handler" type="Function">
        /// The handler to remove from the event.
        /// </param>
        /// <returns />

        this.get_events().removeHandler("showing", handler);
    },
    raiseShowing : function() {
        /// <summary>
        /// Raise the <code>showing</code> event
        /// </summary>
        /// <returns />

        var handlers = this.get_events().getHandler("showing");
        if (handlers) {
            handlers(this, Sys.EventArgs.Empty);
        }
    },
    
    add_shown : function(handler) {
        /// <summary>
        /// Adds an event handler for the <code>shown</code> event.
        /// </summary>
        /// <param name="handler" type="Function">
        /// The handler to add to the event.
        /// </param>
        /// <returns />

        this.get_events().addHandler("shown", handler);
    },
    remove_shown : function(handler) {
        /// <summary>
        /// Removes an event handler for the <code>shown</code> event.
        /// </summary>
        /// <param name="handler" type="Function">
        /// The handler to remove from the event.
        /// </param>
        /// <returns />

        this.get_events().removeHandler("shown", handler);
    },
    raiseShown : function() {
        /// <summary>
        /// Raise the <code>shown</code> event
        /// </summary>
        /// <returns />

        var handlers = this.get_events().getHandler("shown");
        if (handlers) {
            handlers(this, Sys.EventArgs.Empty);
        }
    },
    
    add_hiding : function(handler) {
        /// <summary>
        /// Adds an event handler for the <code>hiding</code> event.
        /// </summary>
        /// <param name="handler" type="Function">
        /// The handler to add to the event.
        /// </param>
        /// <returns />

        this.get_events().addHandler("hiding", handler);
    },
    remove_hiding : function(handler) {
        /// <summary>
        /// Removes an event handler for the <code>hiding</code> event.
        /// </summary>
        /// <param name="handler" type="Function">
        /// The handler to remove from the event.
        /// </param>
        /// <returns />

        this.get_events().removeHandler("hiding", handler);
    },
    raiseHiding : function() {
        /// <summary>
        /// Raise the <code>hiding</code> event
        /// </summary>
        /// <returns />

        var handlers = this.get_events().getHandler("hiding");
        if (handlers) {
            handlers(this, Sys.EventArgs.Empty);
        }
    },
    
    add_hidden : function(handler) {
        /// <summary>
        /// Adds an event handler for the <code>hidden</code> event.
        /// </summary>
        /// <param name="handler" type="Function">
        /// The handler to add to the event.
        /// </param>
        /// <returns />

        this.get_events().addHandler("hidden", handler);
    },
    remove_hidden : function(handler) {
        /// <summary>
        /// Removes an event handler for the <code>hidden</code> event.
        /// </summary>
        /// <param name="handler" type="Function">
        /// The handler to remove from the event.
        /// </param>
        /// <returns />

        this.get_events().removeHandler("hidden", handler);
    },
    raiseHidden : function() {
        /// <summary>
        /// Raise the <code>hidden</code> event
        /// </summary>
        /// <returns />

        var handlers = this.get_events().getHandler("hidden");
        if (handlers) {
            handlers(this, Sys.EventArgs.Empty);
        }
    },
    
    add_dateSelectionChanged : function(handler) {
        /// <summary>
        /// Adds an event handler for the <code>dateSelectionChanged</code> event.
        /// </summary>
        /// <param name="handler" type="Function">
        /// The handler to add to the event.
        /// </param>
        /// <returns />

        this.get_events().addHandler("dateSelectionChanged", handler);
    },
    remove_dateSelectionChanged : function(handler) {
        /// <summary>
        /// Removes an event handler for the <code>dateSelectionChanged</code> event.
        /// </summary>
        /// <param name="handler" type="Function">
        /// The handler to remove from the event.
        /// </param>
        /// <returns />

        this.get_events().removeHandler("dateSelectionChanged", handler);
    },
    raiseDateSelectionChanged : function() {
        /// <summary>
        /// Raise the <code>dateSelectionChanged</code> event
        /// </summary>
        /// <returns />

        var handlers = this.get_events().getHandler("dateSelectionChanged");
        if (handlers) {
            handlers(this, Sys.EventArgs.Empty);
        }
    },
    
    raiseEditValue: function() {
       var handler = this._editValue.getHandler("mc_calendar_onedit");
       if (handler) 
       {		   
           var retVal = handler(this, Sys.EventArgs.Empty);
           
           if (!retVal)
               return false;
       }
       else
       {
           return true;
       }
    },
    
    add_editValue: function(handler)
    {
        if (this._editValue)
		    this._editValue.addHandler("mc_calendar_onedit", handler);
    },
    
    remove_editValue: function(handler)
    {
        if (this._editValue != null)
    	    this._editValue.removeHandler("mc_calendar_onedit", handler);
    },	

    initialize : function() {
        /// <summary>
        /// Initializes the components and parameters for this behavior
        /// </summary>
        
        AjaxControlToolkit.MCWeekCalendarBehavior.callBaseMethod(this, "initialize");
        
        var elt = this.get_element();
        $addHandlers(elt, this._element$delegates);
        
        if (this._button) 
            $addHandlers(this._button, this._button$delegates);
        
        this._modeChangeMoveTopOrLeftAnimation = new AjaxControlToolkit.Animation.LengthAnimation(null, null, null, "style", null, 0, 0, "px");
        this._modeChangeMoveBottomOrRightAnimation = new AjaxControlToolkit.Animation.LengthAnimation(null, null, null, "style", null, 0, 0, "px");
        this._modeChangeAnimation = new AjaxControlToolkit.Animation.ParallelAnimation(null, .25, null, [ this._modeChangeMoveTopOrLeftAnimation, this._modeChangeMoveBottomOrRightAnimation ]);

        var value = this.get_selectedDate();
        if (value) {
            this.set_selectedDate(value);
        } 
        
        //enable events
		this._editValue = new Sys.EventHandlerList();
        this._ensureDateString();
        //AK ADDED - BEGIN
        MC_WCalendar.push(this);
        //AK ADDED - END
        
    },
    dispose : function() {
        /// <summary>
        /// Disposes this behavior's resources
        /// </summary>
        
        if (this._popupBehavior) {
            this._popupBehavior.dispose();
            this._popupBehavior = null;
        }
        this._modes = null;
        this._modeOrder = null;
        if (this._modeChangeMoveTopOrLeftAnimation) {
            this._modeChangeMoveTopOrLeftAnimation.dispose();
            this._modeChangeMoveTopOrLeftAnimation = null;
        }
        if (this._modeChangeMoveBottomOrRightAnimation) {
            this._modeChangeMoveBottomOrRightAnimation.dispose();
            this._modeChangeMoveBottomOrRightAnimation = null;
        }
        if (this._modeChangeAnimation) {
            this._modeChangeAnimation.dispose();
            this._modeChangeAnimation = null;
        }
        if (this._container) {
            this._container.parentNode.removeChild(this._container);
            this._container = null;
        }
        if (this._popupDiv) {
            $common.removeHandlers(this._popupDiv, this._popup$delegates);
            this._popupDiv = null;
        }        
        if (this._prevArrow) {
            $common.removeHandlers(this._prevArrow, this._cell$delegates);
            this._prevArrow = null;
        }
        if (this._prevArrowImage) {
            $common.removeHandlers(this._prevArrowImage, this._cell$delegates);
            this._prevArrowImage = null;
        }
        if (this._nextArrow) {
            $common.removeHandlers(this._nextArrow, this._cell$delegates);
            this._nextArrow = null;
        }
        if (this._nextArrowImage) {        
            $common.removeHandlers(this._nextArrowImage, this._cell$delegates);
            this._nextArrowImage = null;
        }
        if (this._title) {
            $common.removeHandlers(this._title, this._cell$delegates);
            this._title = null;
        }
        if (this._today) {
            $common.removeHandlers(this._today, this._cell$delegates);
            this._today = null;
        }
        if (this._daysRow) {
            for (var i = 0; i < this._daysBody.rows.length; i++) {
                var row = this._daysBody.rows[i];
                for (var j = 0; j < row.cells.length; j++) {
                    $common.removeHandlers(row.cells[j].firstChild, this._cell$delegates);
                }
            }
            this._daysRow = null;
        }
        if (this._monthsRow) {
            for (var i = 0; i < this._monthsBody.rows.length; i++) {
                var row = this._monthsBody.rows[i];
                for (var j = 0; j < row.cells.length; j++) {
                    $common.removeHandlers(row.cells[j].firstChild, this._cell$delegates);
                }
            }
            this._monthsRow = null;
        }
        if (this._yearsRow) {
            for (var i = 0; i < this._yearsBody.rows.length; i++) {
                var row = this._yearsBody.rows[i];
                for (var j = 0; j < row.cells.length; j++) {
                    $common.removeHandlers(row.cells[j].firstChild, this._cell$delegates);
                }
            }
            this._yearsRow = null;
        }
        if (this._button) {
            $common.removeHandlers(this._button, this._button$delegates);
            this._button = null;
        }
        this._updateElement = null;
        var elt = this.get_element();
        $common.removeHandlers(elt, this._element$delegates);
        AjaxControlToolkit.MCWeekCalendarBehavior.callBaseMethod(this, "dispose");
    },
    
    //GA Added - Begin
    getMonthViewLimits : function(value)
    {
		var limits = null;
		if(value)
		{
			var curDate = new Date(value.getTime());
			while(curDate.getDate()>1)
			{
				curDate = new Date(curDate.getTime() - 24*60*60*1000);
			}
			while(curDate.getDay()!=this._firstDayOfWeek)
			{
				curDate = new Date(curDate.getTime() - 24*60*60*1000);
			}
			limits = new Object();
			limits.startDate = new Date(curDate.getTime());
			limits.endDate = new Date(curDate.getTime()+41*24*60*60*1000);
        } 
        return limits;
    },
    //GA Added - End
    
    show : function() {
        /// <summary>
        /// Shows the calendar
        /// </summary>
        
        
        //AK ADDED - BEGIN
        MC_WCalendar_CloseAll();
        
        if (!isNewHandlerSet_MC_WCalendar)
		{
			savedHandler_MC_WCalendar = document.onclick;
			document.onclick = MC_WCalendar_CloseAll;
			isNewHandlerSet_MC_WCalendar = true;
		}
		//AK ADDED - END
		
        this._ensureCalendar();
        
        if (!this._isOpen) {
            this.raiseShowing();
            this._isOpen = true;
            this._switchMonth(null, true);
            this._popupBehavior.show();
            this.raiseShown();
        }
		
    },
    hide : function() {
        /// <summary>
        /// Hides the calendar
        /// </summary>
        this.raiseHiding();
        if (this._container) {         
            this._popupBehavior.hide();        
            this._switchMode("days", true);            
        }
        this._isOpen = false;        
        this.raiseHidden();
        
        //AK ADDED - BEGIN
        if (isNewHandlerSet_MC_WCalendar)
		{
			document.onclick = savedHandler_MC_WCalendar;
			savedHandler_MC_WCalendar = null;
			isNewHandlerSet_MC_WCalendar = false;
		}
		//AK ADDED - END
    },
    
    suspendLayout : function() {
        /// <summary>
        /// Suspends layout of the behavior while setting properties
        /// </summary>

        this._layoutSuspended++;
    },
    resumeLayout : function() {
        /// <summary>
        /// Resumes layout of the behavior and performs any pending layout requests
        /// </summary>

        this._layoutSuspended--;
        if (this._layoutSuspended <= 0) {
            this._layoutSuspended = 0;
            if (this._layoutRequested) {
                this._performLayout();
            }
        }
    },
    invalidate : function() {
        /// <summary>
        /// Performs layout of the behavior unless layout is suspended
        /// </summary>
        
        if (this._layoutSuspended > 0) {
            this._layoutRequested = true;
        } else {
            this._performLayout();
        }
    },
    
    _buildCalendar : function() {
        /// <summary>
        /// Builds the calendar's layout
        /// </summary>
        
        var elt = this.get_element();
        
        this._container = $common.createElementFromTemplate({
            nodeName : "div",
            cssClasses : [this._cssClass]
        }, document.body);

        this._popupDiv = $common.createElementFromTemplate({ 
            nodeName : "div",
            events : this._popup$delegates, 
            properties : {
                tabIndex : 0
            },
            cssClasses : ["ajax__calendar_container"], 
            visible : false 
        }, this._container);
    },
    _buildHeader : function() {
        /// <summary>
        /// Builds the header for the calendar
        /// </summary>
        
        this._header = $common.createElementFromTemplate({ 
            nodeName : "div",
            cssClasses : [ "ajax__calendar_header" ]
        }, this._popupDiv);
        
        var prevArrowWrapper = $common.createElementFromTemplate({ nodeName : "div" }, this._header);
        this._prevArrow = $common.createElementFromTemplate({ 
            nodeName : "div",
            properties : { mode : "prev" }, 
            events : this._cell$delegates,
            cssClasses : [ "ajax__calendar_prev" ] 
        }, prevArrowWrapper);
        
        var nextArrowWrapper = $common.createElementFromTemplate({ nodeName : "div" }, this._header);
        this._nextArrow = $common.createElementFromTemplate({ 
            nodeName : "div",
            properties : { mode : "next" },
            events : this._cell$delegates, 
            cssClasses : [ "ajax__calendar_next" ] 
        }, nextArrowWrapper);        
        
        var titleWrapper = $common.createElementFromTemplate({ nodeName : "div" }, this._header);        
        this._title = $common.createElementFromTemplate({ 
            nodeName : "div",
            properties : { mode : "title" },
            events : this._cell$delegates, 
            cssClasses : [ "ajax__calendar_title" ] 
        }, titleWrapper);
    },
    _buildBody : function() {
        /// <summary>
        /// Builds the body region for the calendar
        /// </summary>
        
        this._body = $common.createElementFromTemplate({ 
            nodeName : "div",
            cssClasses : [ "ajax__calendar_body" ]
        }, this._popupDiv);

        this._buildDays();
        this._buildMonths();
        this._buildYears();
    },
    _buildFooter : function() {
        /// <summary>
        /// Builds the footer for the calendar
        /// </summary>
        
        var todayWrapper = $common.createElementFromTemplate({ nodeName : "div" }, this._popupDiv);
        this._today = $common.createElementFromTemplate({
            nodeName : "div",
            properties : { mode : "today" },
            events : this._cell$delegates,
            cssClasses : [ "ajax__calendar_footer", "ajax__calendar_today" ]
        }, todayWrapper);
    },
    _buildDays : function() {
        /// <summary>
        /// Builds a "days of the month" view for the calendar
        /// </summary>
        
        var dtf = Sys.CultureInfo.CurrentCulture.dateTimeFormat;

        this._days = $common.createElementFromTemplate({ 
            nodeName : "div",
            cssClasses : [ "ajax__calendar_days" ]
        }, this._body);
        this._modes["days"] = this._days;
        
        this._daysTable = $common.createElementFromTemplate({ 
            nodeName : "table",
            properties : {
                cellPadding : 0,
                cellSpacing : 0,
                border : 0,
                style : { margin : "auto" }
            } 
        }, this._days);
        
        this._daysTableHeader = $common.createElementFromTemplate({ nodeName : "thead" }, this._daysTable);
        this._daysTableHeaderRow = $common.createElementFromTemplate({ nodeName : "tr" }, this._daysTableHeader);
        this._daysBody = $common.createElementFromTemplate({ nodeName: "tbody" }, this._daysTable);
        
        for (var i = 0; i < 7; i++) {
            var dayCell = $common.createElementFromTemplate({ nodeName : "td" }, this._daysTableHeaderRow);
            var dayDiv = $common.createElementFromTemplate({
                nodeName : "div",
                cssClasses : [ "ajax__calendar_dayname" ]
            }, dayCell);
        }

        for (var i = 0; i < 6; i++) {
            var daysRow = $common.createElementFromTemplate({ nodeName : "tr" }, this._daysBody);
            for(var j = 0; j < 7; j++) {
                var dayCell = $common.createElementFromTemplate({ nodeName : "td" }, daysRow);
                var dayDiv = $common.createElementFromTemplate({
                    nodeName : "div",
                    properties : {
                        mode : "day",
                        innerHTML : "&nbsp;"
                    },
                    events : this._cell$delegates,
                    cssClasses : [ "ajax__calendar_day" ]
                }, dayCell);
            }
        }
    },
    _buildMonths : function() {
        /// <summary>
        /// Builds a "months of the year" view for the calendar
        /// </summary>
        
        var dtf = Sys.CultureInfo.CurrentCulture.dateTimeFormat;        

        this._months = $common.createElementFromTemplate({
            nodeName : "div",
            cssClasses : [ "ajax__calendar_months" ],
            visible : false
        }, this._body);
        this._modes["months"] = this._months;
        
        this._monthsTable = $common.createElementFromTemplate({
            nodeName : "table",
            properties : {
                cellPadding : 0,
                cellSpacing : 0,
                border : 0,
                style : { margin : "auto" }
            }
        }, this._months);

        this._monthsBody = $common.createElementFromTemplate({ nodeName : "tbody" }, this._monthsTable);
        for (var i = 0; i < 3; i++) {
            var monthsRow = $common.createElementFromTemplate({ nodeName : "tr" }, this._monthsBody);
            for (var j = 0; j < 4; j++) {
                var monthCell = $common.createElementFromTemplate({ nodeName : "td" }, monthsRow);
                var monthDiv = $common.createElementFromTemplate({
                    nodeName : "div",
                    properties : {
                        mode : "month",
                        month : (i * 4) + j,
                        innerHTML : "<br />" + dtf.AbbreviatedMonthNames[(i * 4) + j]
                    },
                    events : this._cell$delegates,
                    cssClasses : [ "ajax__calendar_month" ]
                }, monthCell);
            }
        }
    },
    _buildYears : function() {
        /// <summary>
        /// Builds a "years in this decade" view for the calendar
        /// </summary>
        
        this._years = $common.createElementFromTemplate({
            nodeName : "div",
            cssClasses : [ "ajax__calendar_years" ],
            visible : false
        }, this._body);
        this._modes["years"] = this._years;
        
        this._yearsTable = $common.createElementFromTemplate({
            nodeName : "table",
            properties : {
                cellPadding : 0,
                cellSpacing : 0,
                border : 0,
                style : { margin : "auto" }
            }
        }, this._years);

        this._yearsBody = $common.createElementFromTemplate({ nodeName : "tbody" }, this._yearsTable);        
        for (var i = 0; i < 3; i++) {
            var yearsRow = $common.createElementFromTemplate({ nodeName : "tr" }, this._yearsBody);
            for (var j = 0; j < 4; j++) {
                var yearCell = $common.createElementFromTemplate({ nodeName : "td" }, yearsRow);
                var yearDiv = $common.createElementFromTemplate({ 
                    nodeName : "div", 
                    properties : { 
                        mode : "year",
                        year : ((i * 4) + j) - 1
                    },
                    events : this._cell$delegates,
                    cssClasses : [ "ajax__calendar_year" ]
                }, yearCell);
            }
        }
    },
    
    _performLayout : function() {
        /// <summmary>
        /// Updates the various views of the calendar to match the current selected and visible dates
        /// </summary>
        
        var elt = this.get_element();
        if (!elt) return;
        if (!this.get_isInitialized()) return;
        if (!this._isOpen) return;

        var dtf = Sys.CultureInfo.CurrentCulture.dateTimeFormat;        
        var selectedDate = this.get_selectedDate();
        var visibleDate = this._getEffectiveVisibleDate();
        var todaysDate = this.get_todaysDate(); 
        
        switch (this._mode) {
            case "days":
                
                var firstDayOfWeek = this._getFirstDayOfWeek();
                var daysToBacktrack = visibleDate.getDay() - firstDayOfWeek;
                if (daysToBacktrack <= 0)
                    daysToBacktrack += 7;
                    
                var startDate = new Date(visibleDate.getFullYear(), visibleDate.getMonth(), visibleDate.getDate() - daysToBacktrack);
                var currentDate = startDate;

                for (var i = 0; i < 7; i++) {
                    var dayCell = this._daysTableHeaderRow.cells[i].firstChild;
                    if (dayCell.firstChild) {
                        dayCell.removeChild(dayCell.firstChild);
                    }
                    dayCell.appendChild(document.createTextNode(dtf.ShortestDayNames[(i + firstDayOfWeek) % 7]));
                }
                for (var week = 0; week < 6; week ++) {
                    var weekRow = this._daysBody.rows[week];
                    for (var dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++) {
                        var dayCell = weekRow.cells[dayOfWeek].firstChild;
                        if (dayCell.firstChild) {
                            dayCell.removeChild(dayCell.firstChild);
                        }
                        dayCell.appendChild(document.createTextNode(currentDate.getDate()));
                        dayCell.title = currentDate.localeFormat("D");
                        dayCell.date = currentDate;
                        $common.removeCssClasses(dayCell.parentNode, [ "ajax__calendar_other", "ajax__calendar_active" ]);
                        Sys.UI.DomElement.addCssClass(dayCell.parentNode, this._getCssClass(dayCell.date, 'd'));
                        currentDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate() + 1);
                    }
                }
                
                this._prevArrow.date = new Date(visibleDate.getFullYear(), visibleDate.getMonth() - 1, 1);
                this._nextArrow.date = new Date(visibleDate.getFullYear(), visibleDate.getMonth() + 1, 1);
                if (this._title.firstChild) {
                    this._title.removeChild(this._title.firstChild);
                }
                this._title.appendChild(document.createTextNode(visibleDate.localeFormat("MMMM, yyyy")));
                this._title.date = visibleDate;

                break;
            case "months":

                for (var i = 0; i < this._monthsBody.rows.length; i++) {
                    var row = this._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        var cell = row.cells[j].firstChild;
                        cell.date = new Date(visibleDate.getFullYear(), cell.month, 1);
                        $common.removeCssClasses(cell.parentNode, [ "ajax__calendar_other", "ajax__calendar_active" ]);
                        Sys.UI.DomElement.addCssClass(cell.parentNode, this._getCssClass(cell.date, 'M'));
                    }
                }
                
                if (this._title.firstChild) {
                    this._title.removeChild(this._title.firstChild);
                }
                this._title.appendChild(document.createTextNode(visibleDate.localeFormat("yyyy")));
                this._title.date = visibleDate;
                this._prevArrow.date = new Date(visibleDate.getFullYear() - 1, 0, 1);
                this._nextArrow.date = new Date(visibleDate.getFullYear() + 1, 0, 1);

                break;
            case "years":

                var minYear = (Math.floor(visibleDate.getFullYear() / 10) * 10);
                for (var i = 0; i < this._yearsBody.rows.length; i++) {
                    var row = this._yearsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        var cell = row.cells[j].firstChild;
                        cell.date = new Date(minYear + cell.year, 0, 1);
                        if (cell.firstChild) {
                            cell.removeChild(cell.lastChild);
                        } else {
                            cell.appendChild(document.createElement("br"));
                        }
                        cell.appendChild(document.createTextNode(minYear + cell.year));
                        $common.removeCssClasses(cell.parentNode, [ "ajax__calendar_other", "ajax__calendar_active" ]);
                        Sys.UI.DomElement.addCssClass(cell.parentNode, this._getCssClass(cell.date, 'y'));
                    }
                }

                if (this._title.firstChild) {
                    this._title.removeChild(this._title.firstChild);
                }
                this._title.appendChild(document.createTextNode(minYear.toString() + "-" + (minYear + 9).toString()));
                this._title.date = visibleDate;
                this._prevArrow.date = new Date(minYear - 10, 0, 1);
                this._nextArrow.date = new Date(minYear + 10, 0, 1);

                break;
        }
        if (this._today.firstChild) {
            this._today.removeChild(this._today.firstChild);
        }
        this._today.appendChild(document.createTextNode(String.format(AjaxControlToolkit.Resources.Calendar_Today, todaysDate.localeFormat("MMMM d, yyyy"))));
        this._today.date = todaysDate;        
    },
    
    _ensureCalendar : function() {
    
        if (!this._container) {
            
            var elt = this.get_element();
        
            this._buildCalendar();
            this._buildHeader();
            this._buildBody();
            this._buildFooter();
            
            this._popupBehavior = new $create(AjaxControlToolkit.PopupBehavior, { parentElement : elt, positioningMode : AjaxControlToolkit.PositioningMode.BottomLeft }, {}, {}, this._popupDiv);         
        }    
    },

    
    _fireChanged : function() {
        /// <summary>
        /// Attempts to fire the change event on the attached textbox
        /// </summary>
        
        var elt = this.get_element();
        if (document.createEventObject) {
            elt.fireEvent("onchange");
        } else if (document.createEvent) {
            var e = document.createEvent("HTMLEvents");
            e.initEvent("change", true, true);
            elt.dispatchEvent(e);
        }
    },
    _switchMonth : function(date, dontAnimate) {
        /// <summary>
        /// Switches the visible month in the days view
        /// </summary>
        /// <param name="date" type="Date">The visible date to switch to</param>
        /// <param name="dontAnimate" type="Boolean">Prevents animation from occuring if the control is animated</param>
        
        // Check _isAnimating to make sure we don't animate horizontally and vertically at the same time
        if (this._isAnimating) {
            return;
        }
        
        var visibleDate = this._getEffectiveVisibleDate();
        if ((date && date.getFullYear() == visibleDate.getFullYear() && date.getMonth() == visibleDate.getMonth())) {
            dontAnimate = true;
        }
        
        if (this._animated && !dontAnimate) {
            this._isAnimating = true;
            
            var newElement = this._modes[this._mode];
            var oldElement = newElement.cloneNode(true);
            this._body.appendChild(oldElement);
            if (visibleDate > date) {

                // animating down
                // the newIndex element is the top
                // the oldIndex element is the bottom (visible)
                
                // move in, fade in
                $common.setLocation(newElement, {x:-162,y:0});
                Sys.UI.DomElement.setVisible(newElement, true);
                this._modeChangeMoveTopOrLeftAnimation.set_propertyKey("left");
                this._modeChangeMoveTopOrLeftAnimation.set_target(newElement);
                this._modeChangeMoveTopOrLeftAnimation.set_startValue(-this._width);
                this._modeChangeMoveTopOrLeftAnimation.set_endValue(0);
                
                // move out, fade out
                $common.setLocation(oldElement, {x:0,y:0});
                Sys.UI.DomElement.setVisible(oldElement, true);
                this._modeChangeMoveBottomOrRightAnimation.set_propertyKey("left");
                this._modeChangeMoveBottomOrRightAnimation.set_target(oldElement);
                this._modeChangeMoveBottomOrRightAnimation.set_startValue(0);
                this._modeChangeMoveBottomOrRightAnimation.set_endValue(this._width);

            } else {
                // animating up
                // the oldIndex element is the top (visible)
                // the newIndex element is the bottom
                
                // move out, fade out
                $common.setLocation(oldElement, {x:0,y:0});
                Sys.UI.DomElement.setVisible(oldElement, true);
                this._modeChangeMoveTopOrLeftAnimation.set_propertyKey("left");
                this._modeChangeMoveTopOrLeftAnimation.set_target(oldElement);
                this._modeChangeMoveTopOrLeftAnimation.set_endValue(-this._width);
                this._modeChangeMoveTopOrLeftAnimation.set_startValue(0);

                // move in, fade in
                $common.setLocation(newElement, {x:162,y:0});
                Sys.UI.DomElement.setVisible(newElement, true);
                this._modeChangeMoveBottomOrRightAnimation.set_propertyKey("left");
                this._modeChangeMoveBottomOrRightAnimation.set_target(newElement);
                this._modeChangeMoveBottomOrRightAnimation.set_endValue(0);
                this._modeChangeMoveBottomOrRightAnimation.set_startValue(this._width);
            }
            this._visibleDate = date;
            this.invalidate();
            
            var endHandler = Function.createDelegate(this, function() { 
                this._body.removeChild(oldElement);
                oldElement = null;
                this._isAnimating = false;
                this._modeChangeAnimation.remove_ended(endHandler);
            });
            this._modeChangeAnimation.add_ended(endHandler);
            this._modeChangeAnimation.play();
        } else {
            this._visibleDate = date;
            this.invalidate();
        }
    },
    _switchMode : function(mode, dontAnimate) {
        /// <summary>
        /// Switches the visible view from "days" to "months" to "years"
        /// </summary>
        /// <param name="mode" type="String">The view mode to switch to</param>
        /// <param name="dontAnimate" type="Boolean">Prevents animation from occuring if the control is animated</param>
        
        // Check _isAnimating to make sure we don't animate horizontally and vertically at the same time
        if (this._isAnimating || (this._mode == mode)) {
            return;
        }
        
        var moveDown = this._modeOrder[this._mode] < this._modeOrder[mode];
        var oldElement = this._modes[this._mode];
        var newElement = this._modes[mode];
        this._mode = mode;
                
        if (this._animated && !dontAnimate) { 
            this._isAnimating = true;
            
            this.invalidate();
            
            if (moveDown) {
                // animating down
                // the newIndex element is the top
                // the oldIndex element is the bottom (visible)
                
                // move in, fade in
                $common.setLocation(newElement, {x:0,y:-this._height});
                Sys.UI.DomElement.setVisible(newElement, true);
                this._modeChangeMoveTopOrLeftAnimation.set_propertyKey("top");
                this._modeChangeMoveTopOrLeftAnimation.set_target(newElement);
                this._modeChangeMoveTopOrLeftAnimation.set_startValue(-this._height);
                this._modeChangeMoveTopOrLeftAnimation.set_endValue(0);
                
                // move out, fade out
                $common.setLocation(oldElement, {x:0,y:0});
                Sys.UI.DomElement.setVisible(oldElement, true);

                this._modeChangeMoveBottomOrRightAnimation.set_propertyKey("top");
                this._modeChangeMoveBottomOrRightAnimation.set_target(oldElement);
                this._modeChangeMoveBottomOrRightAnimation.set_startValue(0);
                this._modeChangeMoveBottomOrRightAnimation.set_endValue(this._height);

            } else {
                // animating up
                // the oldIndex element is the top (visible)
                // the newIndex element is the bottom
                
                // move out, fade out
                $common.setLocation(oldElement, {x:0,y:0});
                Sys.UI.DomElement.setVisible(oldElement, true);
                this._modeChangeMoveTopOrLeftAnimation.set_propertyKey("top");
                this._modeChangeMoveTopOrLeftAnimation.set_target(oldElement);
                this._modeChangeMoveTopOrLeftAnimation.set_endValue(-this._height);
                this._modeChangeMoveTopOrLeftAnimation.set_startValue(0);

                // move in, fade in
                $common.setLocation(newElement, {x:0,y:139});
                Sys.UI.DomElement.setVisible(newElement, true);
                this._modeChangeMoveBottomOrRightAnimation.set_propertyKey("top");
                this._modeChangeMoveBottomOrRightAnimation.set_target(newElement);
                this._modeChangeMoveBottomOrRightAnimation.set_endValue(0);
                this._modeChangeMoveBottomOrRightAnimation.set_startValue(this._height);
            }
            var endHandler = Function.createDelegate(this, function() { 
                this._isAnimating = false;
                this._modeChangeAnimation.remove_ended(endHandler);
            });
            this._modeChangeAnimation.add_ended(endHandler);
            this._modeChangeAnimation.play();
        } else {
            this._mode = mode;
            Sys.UI.DomElement.setVisible(oldElement, false);
            this.invalidate();
            Sys.UI.DomElement.setVisible(newElement, true);
            $common.setLocation(newElement, {x:0,y:0});
        }
    },
    _isSelected : function(date, part) {
        /// <summary>
        /// Gets whether the supplied date is the currently selected date
        /// </summary>
        /// <param name="date" type="Date">The date to match</param>
        /// <param name="part" type="String">The most significant part of the date to test</param>
        /// <returns type="Boolean" />
        
        var value = this.get_selectedDate();
        if (!value) 
        {
			if(part == "d")
			{
				value = this.get_todaysDate();
				if (date.getDate() == value.getDate() && date.getMonth() == value.getMonth() && date.getFullYear() == value.getFullYear()) 
					return true;
			}	
			return false;
        }
        switch (part) {
            case 'd':
            
				if(this._selectedMode == "week")
				{
					var firstValue = value;
					var endValue = new Date(Date.parse(firstValue) + 6*24*60*60*1000);
					if(date < firstValue ||
						date > endValue)
						return false;
					else
						return true;
				}
				
                else if (date.getDate() != value.getDate()) 
					return false;
                // goto case 'M';
            case 'M':
                if (date.getMonth() != value.getMonth()) return false;
                // goto case 'y';
            case 'y':
                if (date.getFullYear() != value.getFullYear()) return false;
                break;
        }
        return true;
    },
    _isOther : function(date, part) {
        /// <summary>
        /// Gets whether the supplied date is in a different view from the current visible month
        /// </summary>
        /// <param name="date" type="Date">The date to match</param>
        /// <param name="part" type="String">The most significant part of the date to test</param>
        /// <returns type="Boolean" />

        var value = this._getEffectiveVisibleDate();
        switch (part) {
            case 'd': 
                return (date.getFullYear() != value.getFullYear() || date.getMonth() != value.getMonth());
            case 'M': 
                return false;
            case 'y': 
                var minYear = (Math.floor(value.getFullYear() / 10) * 10);
                return date.getFullYear() < minYear || (minYear + 10) <= date.getFullYear();
        }
        return false;
    },
    _getCssClass : function(date, part) {
        /// <summary>
        /// Gets the cssClass to apply to a cell based on a supplied date
        /// </summary>
        /// <param name="date" type="Date">The date to match</param>
        /// <param name="part" type="String">The most significant part of the date to test</param>
        /// <returns type="String" />

        if (this._isSelected(date, part)) {
            return "ajax__calendar_active";
        } else if (this._isOther(date, part)) {
            return "ajax__calendar_other";
        } else {
            return "";
        }
    },
    _getEffectiveVisibleDate : function() {
        var value = this.get_visibleDate();
        if (value == null) 
            value = this.get_selectedDate();
        if (value == null)
            value = this.get_todaysDate();
        return new Date(value.getFullYear(), value.getMonth(), 1);
    },
    _getFirstDayOfWeek : function() {
        /// <summary>
        /// Gets the first day of the week
        /// </summary>
        
        if (this.get_firstDayOfWeek() != AjaxControlToolkit.FirstDayOfWeek.Default) {
            return this.get_firstDayOfWeek();
        }
        return Sys.CultureInfo.CurrentCulture.dateTimeFormat.FirstDayOfWeek;
    },
    _parseTextValue : function(text) {
        /// <summary>
        /// Converts a text value from the textbox into a date
        /// </summary>
        /// <param name="text" type="String" mayBeNull="true">The text value to parse</param>
        /// <returns type="Date" />
        
        var value = null;
        if(text) 
        {
			if(this._selectedMode == "week" || this._selectedMode == "month" || this._selectedMode == "year")
			{
				if(text.indexOf("-") >= 0)
					text = text.substr(0, text.indexOf("-"));
			}
            value = Date.parseLocale(text, this.get_format());
        }
        if(isNaN(value)) {
            value = null;
        }
        return value;
    },
    
    _onblur : function() {
        /// <summary>
        /// Handles the completion of a deferred blur operation
        /// </summary>
        
        this._focus.cancel();
        this._ensureDateString();
        this.hide(); 
    },
    _onfocus : function() {
        /// <summary>
        /// Handles the completion of a deferred focus operation
        /// </summary>
        
        this._blur.cancel();
        this.get_element().focus();
    },
    
    _element_onfocus : function(e) {
        /// <summary> 
        /// Handles the focus event of the element
        /// </summary>
        /// <param name="e" type="Sys.UI.DomEvent">The arguments for the event</param>
        
        if (this._enabled && this._button == null) {
            this._focus.cancel();
            this._blur.cancel();
            this.show();
        }
    },
    _element_onblur : function(e) {
        /// <summary> 
        /// Handles the blur event of the element
        /// </summary>
        /// <param name="e" type="Sys.UI.DomEvent">The arguments for the event</param>
        
        if ((e.type == 'blur' && Sys.Browser.agent != Sys.Browser.InternetExplorer) ||
            (e.type == 'focusout' && Sys.Browser.agent == Sys.Browser.InternetExplorer)) {
            if (this._button == null) {
                this._focus.cancel();
                this._blur.post();
            }
        }
    },
    _element_onchange : function(e) {
        /// <summary> 
        /// Handles the change event of the element
        /// </summary>
        /// <param name="e" type="Sys.UI.DomEvent">The arguments for the event</param>
        if(this._updateElement)
        {
            //DV FIX
            this._selectedDate.setHours(12);            
            //alert(this._selectedDate + '_' +this._selectedDate.toGMTString());
			this._updateElement.value = this._selectedDate.toGMTString()+'^^'+(new Date).getTime();
			var _uniqueId = this._updateElement.getAttribute("name").toString();
			__doPostBack(_uniqueId, '');
        }
        if (!this._selectedDateChanging) {
			var elt = this.get_element();
            this._selectedDate = this._parseTextValue(elt.value);
            this._switchMonth(this._selectedDate, this._selectedDate == null);
        }
        this._ensureDateString();
    },
    
    _ensureDateString : function() {
		var elt = this.get_element();
		if(elt)
		{
			var o = this._parseTextValue(elt.value);
			if(o == null)
				elt.value = "";
		}
		this.raiseEditValue();
    },

    _popup_onfocus : function(e) {
        /// <summary> 
        /// Handles the focus event of the popup
        /// </summary>
        /// <param name="e" type="Sys.UI.DomEvent">The arguments for the event</param>
        
        if ((e.type == 'focus' && Sys.Browser.agent != Sys.Browser.InternetExplorer) ||
            (e.type == 'activate' && Sys.Browser.agent == Sys.Browser.InternetExplorer) ||
            (Sys.Browser.agent === Sys.Browser.Safari) ||
            (Sys.Browser.agent === Sys.Browser.Opera)) {
            if (this._button == null) {
                this._blur.cancel();
                this._focus.post();
            }
        }
    },
    _popup_ondragstart : function(e) {
        /// <summary> 
        /// Handles the drag-start event of the popup calendar
        /// </summary>
        /// <param name="e" type="Sys.UI.DomEvent">The arguments for the event</param>
        
        e.stopPropagation();
        e.preventDefault();
    },
    _popup_onselect : function(e) {
        /// <summary> 
        /// Handles the select event of the popup calendar
        /// </summary>
        /// <param name="e" type="Sys.UI.DomEvent">The arguments for the event</param>
        
        e.stopPropagation();
        e.preventDefault();
    },

    _cell_onmouseover : function(e) {
        /// <summary> 
        /// Handles the mouseover event of a cell
        /// </summary>
        /// <param name="e" type="Sys.UI.DomEvent">The arguments for the event</param>

        if (Sys.Browser.agent === Sys.Browser.Safari) {
            // Safari doesn't reliably call _cell_onmouseout, so clear other cells here to keep the UI correct
            for (var i = 0; i < this._daysBody.rows.length; i++) {
                var row = this._daysBody.rows[i];
                for (var j = 0; j < row.cells.length; j++) {
                    Sys.UI.DomElement.removeCssClass(row.cells[j].firstChild.parentNode, "ajax__calendar_hover");
                }
            }
        }

        var target = e.target;

		if(this._selectedMode == "week" && target.mode == "day")
			target = target.parentNode;
			
        Sys.UI.DomElement.addCssClass(target.parentNode, "ajax__calendar_hover");

        e.stopPropagation();
    },
    _cell_onmouseout : function(e) {
        /// <summary> 
        /// Handles the mouseout event of a cell
        /// </summary>
        /// <param name="e" type="Sys.UI.DomEvent">The arguments for the event</param>

        var target = e.target;

		if(this._selectedMode == "week" && target.mode == "day")
			target = target.parentNode;
			
        Sys.UI.DomElement.removeCssClass(target.parentNode, "ajax__calendar_hover");

        e.stopPropagation();
    },
    _cell_onclick : function(e) {
        /// <summary> 
        /// Handles the click event of a cell
        /// </summary>
        /// <param name="e" type="Sys.UI.DomEvent">The arguments for the event</param>

        if ((Sys.Browser.agent === Sys.Browser.Safari) ||
            (Sys.Browser.agent === Sys.Browser.Opera)) {
            // _popup_onfocus doesn't get called on Safari or Opera, so we call it manually now
            this._popup_onfocus(e);
        }

        if (!this._enabled) 
            return;

        var target = e.target;
        var visibleDate = this._getEffectiveVisibleDate();
        Sys.UI.DomElement.removeCssClass(target.parentNode, "ajax__calendar_hover");
        switch(target.mode) {
            case "prev":
            case "next":
                this._switchMonth(target.date);
                break;
            case "title":
                switch (this._mode) {
                    case "days": this._switchMode("months"); break;
                    case "months": this._switchMode("years"); break;
                }
                break;
            case "month":
                if (target.month == visibleDate.getMonth()) {
                    this._switchMode("days");
                } else {
                    this._visibleDate = target.date;
                    this._switchMode("days");
                }
                break;
            case "year":
                if (target.date.getFullYear() == visibleDate.getFullYear()) {
                    this._switchMode("months");
                } else {
                    this._visibleDate = target.date;
                    this._switchMode("months");
                }
                break;
            case "day":
                this.set_selectedDate(target.date);
                this._switchMonth(target.date);
                if (this._button != null) {
                    this.hide();
                }
                this.raiseDateSelectionChanged();
                break;
            case "today":
                this.set_selectedDate(target.date);
                this._switchMonth(target.date);
                if (this._button != null) {
                    this.hide();
                }
                this.raiseDateSelectionChanged();
                break;
        }
		
        e.stopPropagation();
        e.preventDefault();
    },

    _button_onclick : function(e) {
        /// <summary> 
        /// Handles the click event of the asociated button
        /// </summary>
        /// <param name="e" type="Sys.UI.DomEvent">The arguments for the event</param>

        if (!this._isOpen) 
        {
            e.preventDefault();
            e.stopPropagation();
            if (this._enabled) 
            {
				this.show();
			}
        } 
        else 
        {
            this.hide();
        }
    }
}
AjaxControlToolkit.MCWeekCalendarBehavior.registerClass("AjaxControlToolkit.MCWeekCalendarBehavior", AjaxControlToolkit.BehaviorBase);

//AK ADDED - BEGIN
var savedHandler_MC_WCalendar = null;
var isNewHandlerSet_MC_WCalendar = false;
var MC_WCalendar = [];
function MC_WCalendar_CloseAll()
{
	for(var i=0; i<MC_WCalendar.length; i++)
	{
		MC_WCalendar[i]._ensureDateString();
		MC_WCalendar[i].hide();
	}
}
//AK ADDED - END