var MC_EDD = new Object();
var isNewHandlerSet_entityDD = false;
var savedHandler_entityDD = null;

function MCEntityDD(id, tblMainId, divDDId, tdChId, hidSelType, hidSelId, lbSelected, OnChange)
{
  this.id = id;
  this.mainTable = document.getElementById(tblMainId);
  this.mainDiv = document.getElementById(divDDId);
  this.changeTD = document.getElementById(tdChId);
  this.hidSelType = document.getElementById(hidSelType);
  this.hidSelId = document.getElementById(hidSelId);
  this.lbSelected = document.getElementById(lbSelected);
  this.OnChange = OnChange;
}

MCEntityDD.prototype.getObjectType = function() {
  return this.hidSelType.value;
}

MCEntityDD.prototype.getObjectId = function() {
  return this.hidSelId.value;
}

MCEntityDD.prototype.openEntityDD = function(e) {
  CancelBubble_entityDD(e);
 
  entityCloseAll();
   
  this.mainDiv.firstChild.width = this.mainTable.offsetWidth;
	
	var off = GetTotalOffsetAbsolute(this.changeTD);
	
	this.mainDiv.style.left = off.Left - (this.mainTable.offsetWidth - this.changeTD.offsetWidth).toString() + "px";
	this.mainDiv.style.top = (off.Top + 22).toString() + "px";
	
	this.mainDiv.style.display = "";
	
	if(typeof(hideSelects)!="undefined")
	  hideSelects(this.mainDiv);
	if (!isNewHandlerSet_entityDD)
	{
		savedHandler_entityDD = document.onclick;
		document.onclick = offAction_entityDD;
		isNewHandlerSet_entityDD = true;
	}
};

MCEntityDD.prototype.ShowHideEntityDD = function(e) {
  
  if (this.mainDiv.style.display == "none")
		this.openEntityDD(e);
	else
		entityCloseAll();
};

MCEntityDD.prototype.SelectThis = function(obj, _type, _Id) {
	this.lbSelected.innerHTML = obj.innerHTML;
	this.hidSelType.value = _type;
	this.hidSelId.value = _Id;
	entityCloseAll();
	if(this.OnChange!="")
	  eval(this.OnChange);
};

MCEntityDD.prototype.SelectThisHTML = function(html, _type, _Id) {
	this.lbSelected.innerHTML = html;
	this.hidSelType.value = _type;
	this.hidSelId.value = _Id;
	if(this.OnChange!="")
	  eval(this.OnChange);
};

//************************************************************************************
function entityCloseAll()
{
  var div_coll = document.getElementsByTagName("div");
  for(var i=0; i<div_coll.length; i++)
  {
    if(div_coll[i].id && div_coll[i].id.indexOf("divDropDown")>=0 && div_coll[i].style.display=="")
      div_coll[i].style.display = "none";
  }
  
  if(typeof(showSelects)!="undefined")
	  showSelects();
	  
  if (isNewHandlerSet_entityDD)
  {
    document.onclick = savedHandler_entityDD;
    savedHandler_entityDD = null;
    isNewHandlerSet_entityDD = false;
  }
}

function offAction_entityDD(e)
{
 CancelBubble_entityDD(e);
 
 entityCloseAll();
}

function TdOver(obj)
{
  if(obj)
  {
    if(obj.className == "cellclass")
      obj.className = "hovercellclass";
  }
}
function TdOut(obj)
{
 if(obj)
  {
    if(obj.className == "hovercellclass")
      obj.className = "cellclass";
  }
}

function GetTotalOffsetAbsolute(eSrc)
{
	var obj = {};
	var strValue = '';
	obj.Top = 0;
	obj.Left = 0;
	while (eSrc)
	{
		if(document.defaultView && document.defaultView.getComputedStyle)
		{
			strValue = document.defaultView.getComputedStyle(eSrc, "").getPropertyValue("position");
		}
		else
		{
			if(eSrc.currentStyle)
			strValue = eSrc.currentStyle["position"];
		}

		if (eSrc.style && eSrc.style.position == 'absolute')
			return obj;
		
		if (strValue == 'absolute')
			return obj;
			
		obj.Top += eSrc.offsetTop;
		obj.Left += eSrc.offsetLeft;
		eSrc = eSrc.offsetParent;
	}
	return obj;
}

function CancelBubble_entityDD(e)
{
 e = (e) ? e : ((event) ? event : null);
 if (e)
 {
  e.cancelBubble = true;
  if(e.stopPropagation)
   e.stopPropagation();
 }
}