Type.registerNamespace("Mediachase");


Mediachase.ControlUpdateExtender = function(element) 
{
    Mediachase.ControlUpdateExtender.initializeBase(this, [element]);
    
	var jsGrid = null; 
	var controlUpdate = null; 
	
	/*
	actionList - это JSON, в котором хранитс€ информаци€ о оброботчиках событий грида
	actionList = array of actionListElem
	actionListElem = {string ActionName, string ActionParams, string ControlUpdateId, int EventName, int EventType }
	
	int EventName =
	{
		0 - RowSelect (supported)
		1 - OnEdit (supported)
		2 - OnDblClick (supported)
		3 - onClick (doesn't support)
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

Mediachase.ControlUpdateExtender.prototype = 
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
        Mediachase.ControlUpdateExtender.callBaseMethod(this, 'initialize');       
        
        if (this.jsGrid != null && this.jsGrid.extGrid != null && this.actionList)
        {
			//TODO: Clean code
			
			//register all handlers
			this.jsGrid.extGrid.selModel.on("rowselect", Function.createDelegate(this, this.selectionHandler), this.jsGrid.extGrid);
			this.jsGrid.add_edit(Function.createDelegate(this, this.editHandler));
			this.jsGrid.extGrid.on("rowdblclick", Function.createDelegate(this, this.dblClickHandler), this.jsGrid.extGrid);
			
			this.actionArray = Sys.Serialization.JavaScriptSerializer.deserialize(this.actionList);
        }
        
        //TO DO: kak varian rewenie problemy s odnovremennim posylaniem bolee 1 XmlHttpRequest'a
        // podumat kak realizovat na buduwee
        //Sys.Net.WebRequestManager.add_invokingRequest(Function.createDelegate(this, this.invokingRequestHandler));
	},
	dispose: function()
	{
		this.jsGrid = null;
		this.controlUpdate = null;
		this.actionArray = null;
		Mediachase.ControlUpdateExtender.callBaseMethod(this, 'dispose');
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
	editHandler: function(obj, args)
	{
		var actionElem = this.checkHandler(1);
		if (actionElem.length > 0)
		{
			for (var i = 0; i < actionElem.length; i++)
			{
				this.defaultHandler(args.Id, actionElem[i]);
			}
		}
	},	
	
	selectionHandler: function(obj, rowIndex, r)
	{
		var actionElem = this.checkHandler(0);
		if (actionElem.length > 0)
		{
			for (var i = 0; i < actionElem.length; i++)
			{
				var func = Function.createDelegate(this, this.selectionDelayedHandler);
				var actionElemStatic = actionElem[i];
				window.setTimeout(function() { func(obj, r.data.primaryKeyId , actionElemStatic) }, 800);
			}
		}
	},
	
	selectionDelayedHandler: function(obj, key, actionElem)
	{
		if (this.jsGrid != null && obj && obj.grid === this.jsGrid.extGrid)
		{
			this.defaultHandler(key ,actionElem);
		}
	},
	
	dblClickHandler: function(obj, rowIndex, e)
	{
		var actionElem = this.checkHandler(2);
		if (actionElem.length > 0)
		{
			for (var i = 0; i < actionElem.length; i++)
			{
				this.defaultHandler(obj.view.ds.data.items[rowIndex].data.primaryKeyId ,actionElem[i]);
			}			
		}
	}	
}

Mediachase.ControlUpdateExtender.registerClass("Mediachase.ControlUpdateExtender", Sys.UI.Control);
if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();	

