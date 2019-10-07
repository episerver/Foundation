
//Licenced under Creative Commons
//http://creativecommons.org/licenses/by-sa/3.0/
//Jordan Knight (2008)
//http://www.webjak.net
//http://jakkaj.wordpress.com

Type.registerNamespace('Mediachase');

//
// UpdateTrigger structure:
//
// string code: javascript code to execute (usually '__doPostBack('id', 'params');')
// bool clearAfterExecute: if true, then AFTER complete requeset all others request in queue will be removed 
// bool clearAfterAdd: if true, then AFTER add to queue all other requests in queue will be removed
//

Mediachase.AsyncPostQueue = function(updateTriggers, beginUpdateOnAddTrigger)
{    
    //construct some initial values
    this._updateTriggers = updateTriggers;    
    this._initialised = false;
    this._beginUpdateOnAddTrigger = beginUpdateOnAddTrigger;
    this._doing = false;
    this._jsonUpdateTriggerList = "";
}

Mediachase.AsyncPostQueue.prototype = {
    
    _init : function()
    {
        if(!this._initialised)
        {
            //hook up the events, ensure this only happens once
            this._initialised = true;
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            var callBackDelegate = Function.createDelegate(this, this.endRequestHandler);
            prm.add_endRequest(callBackDelegate);            
        }
    },
    
    endRequestHandler : function(sender, args)
    {
        //continue the update chain
        this.doUpdate();
    },    
    
    doUpdate : function()
    {
        if(!this._initialised)
        {
            this._init();
        }
        
        this._doing = true;
        
        //while there are triggers left, continue the update process
        if(this._updateTriggers.length > 0)
        {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
               
            if(prm.get_isInAsyncPostBack())
            {          
                var pnl = this._updateTriggers[0];
                Sys.Debug.trace("Update wait: " + pnl);
                this._doUpdateWait();
                return;
            }            
            
//            var panel = this._updateTriggers[0];
//            Array.removeAt(this._updateTriggers, 0);            
//            
//            prm._doPostBack(panel, '');
			var elemToExecute = Array.dequeue(this._updateTriggers);
			
			if (elemToExecute.code)
			{
				exec(elemToExecute.code);
			}
			
        }
        else
        {
            //if there are no triggers left, then end the process
            this._doing = false;
        }
    },
    
    _doUpdateWait : function()
    {
        //this performs some basic sychronisation between two or more Chainers running at one time
        var doUpdateDelegate = Function.createDelegate(this, this.doUpdate);
        window.setTimeout(doUpdateDelegate, '1000');
    },
    
    beginUpdating : function()
    {
        //_doing ensures we dont accidentally kick off two update chains.        
        if(!this._doing)
        {
            this.doUpdate();
        }
    },
    
    addTrigger : function(triggerName)
    {
		if (triggerName.clearAfterAdd === true)
		{
			Array.clear(this._updateTriggers);
		}
		
        //add more triggers to the update array
        Array.add(this._updateTriggers, triggerName);
        if(this._beginUpdateOnAddTrigger)
        {
            this.beginUpdating();
        }
    },
    
    set_beginUpdateOnAddTrigger : function(autoBeginMode)
    {
        this._beginUpdateOnAddTrigger = autoBeginMode;
    },
    
    set_jsonUpdateTriggerList : function(jsonString)
    {
        //this method handles adding triggers that have been sent from server
        
        var serializer = Sys.Serialization.JavaScriptSerializer;
        
        //convert the serialised json data into a proper array
        var arrAdd = serializer.deserialize(jsonString);
        
        //create a little delegate to pass into the Array.forEach below. There are easier ways, but this is good practice :)
        var arrayAdd = Function.createDelegate(this, function(item) {  
            Array.add(this._updateTriggers, item);
            Sys.Debug.trace ("Added: " + item);
            });
        
        Array.forEach(arrAdd, arrayAdd);
        
        if(this._beginUpdateOnAddTrigger)
        {
            this.beginUpdating();
        }
    }
}

Mediachase.AsyncPostQueue.registerClass('Mediachase.AsyncPostQueue');

var MediachaseChainerStatic = new Mediachase.AsyncPostQueue([], true);