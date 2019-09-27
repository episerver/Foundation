var __currentFocusElement = null;
var __scriptToExecute = null;
function closeAllDropDowns(obj, e)
{
	
	if (e.type === 'keypress')
	{
	
		if (e.keyCode == 13)
		{
			e.cancelBubble = true;
			if (typeof(e.target) !== 'undefined')
			{
				e.preventDefault();
				e.stopPropagation();
			}
		}


		if ((typeof(e.target) !== 'undefined' && e.target.className.indexOf('dropLabel') > -1 && e.target.tagName == 'INPUT' && e.keyCode == 13)
		|| (typeof(e.srcElement) !== 'undefined' && e.srcElement.className.indexOf('dropLabel') > -1 && e.srcElement.tagName == 'INPUT' && e.keyCode == 13))
		//if (e.target.className.indexOf('dropLabel') > -1 && e.target.tagName == 'INPUT' && e.keyCode == 13)
		{
			if (__scriptToExecute)
			{
				var tmpExecute = __scriptToExecute;
				__scriptToExecute = null;
				eval(tmpExecute);
			}
		}
		
		return true;
	}
	
	if (e.target !== null && typeof(e.target) !== 'undefined')
	{
		if (e.target.className.indexOf('dropLabel') > -1 || e.target.tagName == 'SELECT' || e.target.tagName == 'OPTION' || e.target.tagName == 'INPUT')
		{
			return true;
		}
	} else if (e.srcElement !== null) {	
		if (e.srcElement.className.indexOf('dropLabel') > -1 || e.srcElement.tagName == 'SELECT' || e.srcElement.tagName == 'OPTION' || e.srcElement.tagName == 'INPUT')
			return true;
	}
	
	__currentFocusElement = null;
	if (__scriptToExecute)
	{
		var tmpExecute = __scriptToExecute;
		__scriptToExecute = null;
		eval(tmpExecute);
	}
	
	var arr = document.getElementsByTagName('div');
	for (var i = 0; i < arr.length; i++)
	{
		if ((arr[i].className.indexOf('dropLabel') > -1 && arr[i].innerHTML && arr[i].innerHTML.length > 0))
		{
			if (arr[i].previousSibling != null) {
				arr[i].previousSibling.style.display = 'none';
			}
			arr[i].style.display = 'inline';
		}
		else if (arr[i].getAttribute('changeVisibility'))
		{
			if (arr[i].nextSibling != null) {
				arr[i].nextSibling.style.display = 'inline';
			}
			arr[i].style.display = 'none';	
		}
	}
	
	var arrInputs = document.getElementsByTagName('input');
	for (var i = 0; i < arrInputs.length; i++)
	{
		if (arrInputs[i].getAttribute('changeVisibility'))
		{
			if (arrInputs[i].nextSibling != null) {
				arrInputs[i].nextSibling.style.display = 'inline';
			}
			arrInputs[i].style.display = 'none';
		}
	}
	
	var arrSelects = document.getElementsByTagName('select');
	for (var i = 0; i < arrSelects.length; i++)
	{
		if (arrSelects[i].getAttribute('changeVisibility'))
		{
			if (arrSelects[i].nextSibling != null) {
				arrSelects[i].nextSibling.style.display = 'inline';
			}
			arrSelects[i].style.display = 'none';
		}
	}
}

function internalAttachEventToMainContainer(obj)
{
	if (document.attachEvent)
	{
		document.attachEvent("onclick", function(e) { closeAllDropDowns(obj, e) })
		document.attachEvent("onkeypress",  function(e) { closeAllDropDowns(obj, e) })
	}
	else
	{
		document.addEventListener("click", function(e) { closeAllDropDowns(obj, e) }, true);
		document.addEventListener("keypress", function(e) { closeAllDropDowns(obj, e) }, true)
	}
}
	
function attachEventToMainContainer(objId)
{
	var obj = document.getElementById(objId);
	if (obj)
	{
		internalAttachEventToMainContainer(obj);
	}
	else
	{
		window.setTimeout(function() {attachEventToMainContainer(objId); }, 500);
	}
}

function onfocusDefaultHandler(obj, scriptToExecute)
{
	__currentFocusElement = obj;
	__scriptToExecute = scriptToExecute;
}


// eval sctipt [scriptToExecute], if array [controlsCanFocus] doesnt have focus
function onblurDefaultHandler(controlsCanFocus, scriptToExecute)
{
	if (__currentFocusElement != null)
	{
		var _id = '';
		if (__currentFocusElement.id)
			_id = __currentFocusElement.id;
		
		for (var i = 0; i < controlsCanFocus.length; i++)
		{
			if (controlsCanFocus[i].id == _id)
			{
				return;
			}
		}
		
		eval(scriptToExecute);
	}
}