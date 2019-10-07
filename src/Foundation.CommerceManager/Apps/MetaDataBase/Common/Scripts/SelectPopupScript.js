var MC_SELECT_POPUPS = new Object();

function MCSelectPopup(id, hfClassNameId, divQuickAddId, backgroundContainerId, tbMainId)
{
	this.id = id;
	this.hfClassName = document.getElementById(hfClassNameId);
	this.divQuickAdd = document.getElementById(divQuickAddId);
	this.backgroundContainer = document.getElementById(backgroundContainerId);
	this.tbMain = document.getElementById(tbMainId);
	if (this.divQuickAdd.addEventListener)
	{
		this.divQuickAdd.addEventListener("keypress", function(e) { keyHandler_SelectPopups(e, id); }, false);
	}
	else
	{
		this.divQuickAdd.attachEvent("onkeypress", function(event) { keyHandler_SelectPopups(event, id); });
	}
	
	if (this.backgroundContainer.addEventListener)
	{
		this.backgroundContainer.addEventListener("mousedown", function(e) { GlobalClose_SelectPopups(e, id); }, false);
	}
	else
	{
		this.backgroundContainer.attachEvent("onmousedown", function(event) { GlobalClose_SelectPopups(event, id); });
	}
}

MCSelectPopup.prototype.getId = function() {
	return this.id;
}

MCSelectPopup.prototype.openSelectPopup = function(e, params) {
	CancelBubble_SelectPopups(e);
 
	this.hfClassName.value = params;
	this.backgroundContainer.style.display = "";
	this.divQuickAdd.style.display = "";
	this.tbMain.focus();
	this.tbMain.select();
	__doPostBack(this.tbMain.id, "");
};

MCSelectPopup.prototype.hideSelectPopup = function() {
  
	this.backgroundContainer.style.display = "none";
	this.divQuickAdd.style.display = "none";
};

MCSelectPopup.prototype.selectObject = function(objId, val) {
	this.hideSelectPopup();
	__doPostBack(objId, val);
};

//************************************************************************************
function keyHandler_SelectPopups(e, id)
{
	if (!e)
		e = event;

	if (e.keyCode == 27)
	{
		MC_SELECT_POPUPS[id].hideSelectPopup();
	}
}

function GlobalClose_SelectPopups(e, id)
{
	if (!e)
		e = event;

	if (e)
	{
		MC_SELECT_POPUPS[id].hideSelectPopup();
	}
}

function GetTotalOffset(eSrc)
{
	this.Top = 0;
	this.Left = 0;
	while (eSrc)
	{
		this.Top += eSrc.offsetTop;
		this.Left += eSrc.offsetLeft;
		eSrc = eSrc.offsetParent;
	}
	return this;
}

function CancelBubble_SelectPopups(e)
{
 e = (e) ? e : ((event) ? event : null);
 if (e)
 {
  e.cancelBubble = true;
  if(e.stopPropagation)
   e.stopPropagation();
 }
}