Type.registerNamespace("Mediachase");


Mediachase.GridViewUpdateExtender = function(element) 
{
    Mediachase.GridViewUpdateExtender.initializeBase(this, [element]);
    
	var jsGrid = null; 
	var controlUpdate = null; 
	
	/*
	actionList - JSON with information about actions
	actionList = array of actionListElem
	actionListElem = {string ActionName, string ActionParams, string ControlUpdateId, int EventName, int EventType }
	
	int EventName =
	{
		0 - RowSelect (supported)
		1 - OnEdit (doesnt support)
		2 - OnDblClick (supported)
		3 - onClick (supported)
	}
	
	int EventType =
	{
		0 - Postback type (additional info @ ControlUpdateId)
		1 - Action type (additional info @ ActionName/ActionParams)
	}
	*/
	var actionList = null;
	
	var actionArray = null;
}

Mediachase.GridViewUpdateExtender.prototype = 
{
	// -========= Properties =========-	
	get_jsGrid: function () {
		return this.jsGrid;
	},
	set_jsGrid: function (value) {
		this.jsGrid = value;
	},
	
	get_controlUpdate: function () {
		return this.controlUpdate;
	},
	set_controlUpdate: function (value) {
		this.controlUpdate = value;
	},
	
	get_actionArray: function () {
		return this.actionArray;
	},
	set_actionArray: function (value) {
		this.actionArray = value;
	},	
	
	get_actionList: function () {
		return this.actionList;
	},
	set_actionList: function (value) {
		this.actionList = value;
	},		
	
	// -========= Methods =========-
	// ctor()
	initialize : function() 
	{		
        Mediachase.GridViewUpdateExtender.callBaseMethod(this, 'initialize');       
        
        if (this.actionList && this.jsGrid)
        {
			//TODO: Clean code
			
			//register all handlers
			//this.jsGrid.extGrid.selModel.on("rowselect", Function.createDelegate(this, this.selectionHandler), this.jsGrid.extGrid);
			
			this.jsGrid.add_rowClick(Function.createDelegate(this, this.clickHandler));
			this.jsGrid.add_dblClick(Function.createDelegate(this, this.dblClickHandler));
			this.jsGrid.add_rowSelect(Function.createDelegate(this, this.rowSelect));
			
			this.actionArray = Sys.Serialization.JavaScriptSerializer.deserialize(this.actionList);
        }
	},
	dispose: function()
	{
		this.jsGrid = null;
		this.controlUpdate = null;
		this.actionArray = null;
		Mediachase.GridViewUpdateExtender.callBaseMethod(this, 'dispose');
	},
	
	//ѕровер€ет зарегистрированный обработчик дл€ событи€ eventName и возвращает actionListElem, в противном случае null
	checkHandler: function(eventName)
	{
		var retVal = new Array();
		if (!this.actionArray)
			this.actionArray = Sys.Serialization.JavaScriptSerializer.deserialize(this.actionList);
			
		for (var i = 0; i < this.actionArray.length; i++)
		{
			if (this.actionArray[i].EventName == eventName)
			{
				retVal.push(this.actionArray[i]);
			}
		}
		
		return retVal;
	},

	defaultHandler: function(keyId, actionElem)
	{
		switch(actionElem.EventType)
		{
			//Postback type
			case 0:
			{
				if (actionElem.ControlUpdateId && $get(actionElem.ControlUpdateId))
				{
					$get(actionElem.ControlUpdateId).value = keyId + '^' + (new Date()).getTime();
					__doPostBack(actionElem.ControlUpdateId, '');
				}
				break;
			}
			//Action type
			case 1:
			{
				if (actionElem.ActionScript && actionElem.ActionScript.length > 0)
				{
					var scriptToEval = actionElem.ActionScript.replace('%primaryKeyId%', keyId);
					eval(scriptToEval);
				}
				break;
			}
			default:
			{
				alert('Unkonwn EventType: ' + actionElem.EventType);
			}
		}
	},
	
	
	//------ Grid event hanlers -------	
	
	rowSelect: function(obj, args)
	{
		var actionElem = this.checkHandler(0);
		if (actionElem.length > 0)
		{
			for (var i = 0; i < actionElem.length; i++)
			{
				this.defaultHandler(args.PrimaryKeyId ,actionElem[i]);
			}			
		}
	},
	
	dblClickHandler: function(obj, args)
	{
		var actionElem = this.checkHandler(2);
		if (actionElem.length > 0)
		{
			for (var i = 0; i < actionElem.length; i++)
			{
				this.defaultHandler(args.PrimaryKeyId ,actionElem[i]);
			}			
		}
	},	
	
	clickHandler: function(obj, args)
	{
		var actionElem = this.checkHandler(3);
		if (actionElem.length > 0)
		{
			for (var i = 0; i < actionElem.length; i++)
			{
				this.defaultHandler(args.PrimaryKeyId, actionElem[i]);
			}
		}
	}
}

Mediachase.GridViewUpdateExtender.registerClass("Mediachase.GridViewUpdateExtender", Sys.UI.Control);
if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();	

