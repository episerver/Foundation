/* FTB_Button
---------------------------------------------- */
function FTB_Button(id, commandIdentifier, customAction, customStateQuery, htmlModeEnabled, customEnabled) {
	this.state = FTB_BUTTON_OFF;
	this.id = id;
	this.ftb = null;
	this.commandIdentifier = commandIdentifier;
	this.customAction = customAction;
	this.customStateQuery = customStateQuery;	
	
	this.disabled = false;
	this.htmlModeEnabled = htmlModeEnabled	;
	this.customEnabled = customEnabled;
	
	this.td = document.getElementById(id);
	this.td.button = this;
	
	if (FTB_Browser.isIE) {		
		this.buttonImage = this.td.childNodes[0];
	} else {
		this.buttonImage = this.td.childNodes[0];
	}
};
FTB_Button.prototype.Initialize = function() {
	var id=this.td.button.id;
	FTB_AddEvent(this.td,"click",function() { if(FTB_Browser.isIE) document.getElementById(id).button.Click(); else this.button.Click(); } );
	FTB_AddEvent(this.td,"mouseover",function() { if(FTB_Browser.isIE) document.getElementById(id).button.MouseOver(); else this.button.MouseOver(); } );
	FTB_AddEvent(this.td,"mouseout",function() { if(FTB_Browser.isIE) document.getElementById(id).button.MouseOut(); else this.button.MouseOut(); } );
};
FTB_Button.prototype.Click = function() {
	if (!this.disabled) {
		
		if (this.customAction) 			
			this.customAction();	
		else if (this.commandIdentifier != null && this.commandIdentifier != '') 
			this.ftb.ExecuteCommand(this.commandIdentifier);

		this.ftb.Event();
		
	}
};
FTB_Button.prototype.MouseOver = function() {
	if (!this.disabled) this.SetButtonBackground("Over");
};
FTB_Button.prototype.MouseOut = function() {
	if (!this.disabled) this.SetButtonBackground("Out");
};
FTB_Button.prototype.SetButtonBackground = function(mouseState) {
		this.SetButtonStyle(mouseState);
}
FTB_Button.prototype.SetButtonStyle = function(mouseState) {
	this.td.className = this.ftb.id + "_Button_" + ((this.state == FTB_BUTTON_ON) ? "On" : "Off") + "_" + mouseState;
}

/* FTB_DropDownList
---------------------------------------------- */

function FTB_DropDownList(id, commandIdentifier, customAction, customStateQuery, customEnabled) {
	this.id = id;
	this.ftb = null;
	this.commandIdentifier = commandIdentifier;
	this.customAction = customAction;
	this.customStateQuery = customStateQuery;
	this.customEnabled = customEnabled;
	
	this.list = document.getElementById(id);
	if (this.list) {
		this.list.dropDownList = this;

		FTB_AddEvent(this.list,"change",function() { if(FTB_Browser.isIE) document.getElementById(id).dropDownList.Select(); else this.dropDownList.Select(); } );
	} else {
		alert(id + ' is not setup properly');
	}
};
FTB_DropDownList.prototype.Select = function() {	
	if (this.customAction) 
		this.customAction();
	else if (this.commandIdentifier != null && this.commandIdentifier != '') 
		this.ftb.ExecuteCommand(this.commandIdentifier, '', this.list.options[this.list.selectedIndex].value);	
	
	this.list.selectedIndex = 0;
	
	this.ftb.Event();
};
FTB_DropDownList.prototype.SetSelected = function(commandValue) {
	value = String(commandValue).toLowerCase();

	for (var i=0; i<this.list.options.length; i++) {
		if (this.list.options[i].value.toLowerCase() == value || this.list.options[i].text.toLowerCase() == value) {
			this.list.selectedIndex = i;
			return;
		}
	}
};