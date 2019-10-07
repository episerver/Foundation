
var iTimerID = -1;
var oCurFrame = null;
var oCurDiv = null;
var iCurPos = 0;
var iStep = 20;

function TopFrameVisible()
{
	var oFSRight = window.top.document.getElementById("fs3");
	if (!oFSRight) return false;
	var sRows = oFSRight.rows;
	if (!sRows) return false;
	return (sRows.substr(0, 1) != "0");
}

function TopFrameReload(str)
{
	str = str.toLowerCase();
	var oFSRight = window.top.document.getElementById("fs3");
	if (oFSRight && oFSRight.rows && oFSRight.rows.substr(0, 1) != "0")
	{
		var oTopRight = window.top.document.getElementById("topright");
		if (oTopRight && oTopRight.src && oTopRight.src.toLowerCase().indexOf(str) > 0)
		{
			var s = oTopRight.src;
			oTopRight.src = "";
			oTopRight.src = s;
		}
	}
}

function ShowFrame(url)
{
	try
	{
		if (window.event)
			window.event.cancelBubble = true;

		if (!TopFrameVisible())	// iframe
		{
			oCurFrame = document.getElementById("frmList");
			if (!oCurFrame)	return;
			oCurFrame.src = url;
			if (oCurFrame.style.display == "none")
			{
				iCurPos = 0;
				oCurFrame.style.height = iCurPos;
				oCurFrame.style.display = "block";
				
				oCurDiv = document.getElementById("divTopMenu");
				if (oCurDiv)
				{
					oCurDiv.style.top = iCurPos;
					oCurDiv.style.position = "relative";
				}
				iTimerID = window.setInterval(Enhance, 1);
			}
		}
		else	// top frame
		{
			var oTopRight = window.top.document.getElementById("topright");
			if (oTopRight)
				oTopRight.src = url;
		}
	}
	catch(e) {}
}

function disableEnterKey(e) 
{ 
	var _key = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
	try {
		if (_key == 13 && e.srcElement.type != "textarea")
			e.keyCode = 0;
		else
			return true;
	}
	catch (e) {return true;}
}

function ShowWizard(sLink,w,h)
{
	if (w == null)
		w = 650;
	if (h == null)
		h = 400;
	var l = (screen.width - w) / 2;
	var t = (screen.height - h) / 2;
	winprops = 'resizable=0, height='+h+',width='+w+',top='+t+',left='+l;
	var f = window.open(sLink, "_blank", winprops);
}

function ShowResizableWizard(sLink,w,h)
{
	if (w == null)
		w = 650;
	if (h == null)
		h = 400;
	var l = (screen.width - w) / 2;
	var t = (screen.height - h) / 2;
	winprops = 'resizable=1, height='+h+',width='+w+',top='+t+',left='+l;
	var f = window.open(sLink, "_blank", winprops);
}
function OpenPopUpWindow(sLink,w,h)
{
	if (w == null)
		w = 650;
	if (h == null)
		h = 400;
	var l = (screen.width - w) / 2;
	var t = (screen.height - h) / 2;
	winprops = 'scrollbars=1, resizable=0, height='+h+',width='+w+',top='+t+',left='+l;
	var f = window.open(sLink, "_blank", winprops);
}
function OpenPopUpNoScrollWindow(sLink,w,h)
{
	if (w == null)
		w = 650;
	if (h == null)
		h = 400;
	var l = (screen.width - w) / 2;
	var t = (screen.height - h) / 2;
	winprops = 'scrollbars=0, resizable=1, height='+h+',width='+w+',top='+t+',left='+l;
	var f = window.open(sLink, "_blank", winprops);
}
function OpenSizableWindow(sLink,w,h)
{
	if (w == null)
		w = 650;
	if (h == null)
		h = 400;
	var l = (screen.width - w) / 2;
	var t = (screen.height - h) / 2;
	winprops = 'scrollbars=1, resizable=1, height='+h+',width='+w+',top='+t+',left='+l;
	var f = window.open(sLink, "_blank", winprops);
}

var isCSS, isW3C, isIE4, isNN4, isIE6CSS;
var isInitialized = false;
function initDHTMLAPI()
{
	if (isInitialized)
		return;
	
	if (document.images)
	{
		isCSS = (document.body && document.body.style) ? true : false;
		isW3C = (isCSS && document.getElementById) ? true : false;
		isIE4 = (isCSS && document.all) ? true : false;
		isNN4 = (document.layers) ? true : false;
		isIE6CSS = (document.compatMode && document.compatMode.indexOf("CSS1") >= 0) ? true : false;
	}
	isInitialized = true;
}

function getRawObject(obj)
{
	initDHTMLAPI();

	var theObj;
	if (typeof obj == "string")
	{
		if (isW3C)
		{
			theObj = document.getElementById(obj);
		}
		else if (isIE4)
		{
			theObj = document.all(obj);
		}
	}
	else
	{
		theObj = obj;
	}
	return theObj;
}

function getObjectLeft(obj)
{
	var elem = getRawObject(obj);
	var result = 0;
	if (document.defaultView)
	{
		var style = document.defaultView;
		var cssDecl = style.getComputedStyle(elem, "");
		result = cssDecl.getPropertyValue("left");
	}
	else if (elem.currentStyle)
	{
		result = elem.currentStyle.left;
	}
	else if (elem.style)
	{
		result = elem.style.left;
	}
	else if (isNN4)
	{
		result = elem.left;
	}
	
	if (result == "auto")
	{
		result = 0;
		if (isW3C)
		{
			var offsetTrail = elem;
			while (offsetTrail)
			{
				result += offsetTrail.offsetLeft;
				offsetTrail = offsetTrail.offsetParent
			}
			if (navigator.userAgent.indexOf("Mac") != -1 && typeof document.body.leftMargin != "undefined")
			{
				result += document.body.leftMargin;
			}
		}
	}
	else
	{
		result = parseInt(result);
	}
	
	return result;
}

function getObjectTop(obj)
{
	var elem = getRawObject(obj);
	var result = 0;
	if (document.defaultView)
	{
		var style = document.defaultView;
		var cssDecl = style.getComputedStyle(elem, "");
		result = cssDecl.getPropertyValue("top");
	}
	else if (elem.currentStyle)
	{
		result = elem.currentStyle.top;
	}
	else if (elem.style)
	{
		result = elem.style.top;
	}
	else if (isNN4)
	{
		result = elem.top;
	}
	
	if (result == "auto")
	{
		result = 0;
		if (isW3C)
		{
			var offsetTrail = elem;
			while (offsetTrail)
			{
				result += offsetTrail.offsetTop;
				offsetTrail = offsetTrail.offsetParent
			}
			if (navigator.userAgent.indexOf("Mac") != -1 && typeof document.body.topMargin != "undefined")
			{
				result += document.body.topMargin;
			}
		}
	}
	else
	{
		result = parseInt(result);
	}
	
	return result;
}

function getObjectWidth(obj)
{
	var elem = getRawObject(obj);
	var result = 0;
	if (elem.offsetWidth)
	{
		result = elem.offsetWidth;
	}
	else if (elem.clip && elem.clip.width)
	{
		result = elem.clip.width;
	}
	else if (elem.style && elem.style.pixelWidth)
	{
		result = elem.style.pixelWidth;
	}
	return parseInt(result);
}

function getObjectHeight(obj)
{
	var elem = getRawObject(obj);
	var result = 0;
	if (elem.offsetHeight)
	{
		result = elem.offsetHeight;
	}
	else if (elem.clip && elem.clip.height)
	{
		result = elem.clip.height;
	}
	else if (elem.style && elem.style.pixelHeight)
	{
		result = elem.style.pixelHeight;
	}
	return parseInt(result);
}

function hideSelects(objId)
{
	if(window.opera) return;
	
	var objTop = getObjectTop(objId);
	var objLeft = getObjectLeft(objId);
	var objBottom = objTop + getObjectHeight(objId);
	var objRight = objLeft + getObjectWidth(objId);
	
	var selectColl = document.getElementsByTagName("select");
	var oTotalOffset, x1, x2, y1, y2;
	for(j=0;j<selectColl.length;j++)
	{
		var elem = selectColl[j];
		if(elem.document == null) continue;
		if(elem.mcIsHidden && elem.mcIsHidden > 0)
			continue; //already hidden

		x1 = getObjectLeft(elem);
		x2 = x1 + getObjectWidth(elem);
		y1 = getObjectTop(elem);
		y2 = y1 + getObjectHeight(elem);

		if( !( (x1 >= objRight) || (x2 <= objLeft) || (y1 >= objBottom) || (y2 <= objTop) ) )
		{	
			if(elem.style.visibility) 
				elem.mcHiddenLast = elem.style.visibility;
			else
				elem.mcHiddenLast = "inherit";
			elem.style.visibility = "hidden";
			elem.mcIsHidden = 1;
		}
	}
}

function showSelects()
{
	if(window.opera) return;

	var selectColl = document.getElementsByTagName("select");
	for(j=0;j<selectColl.length;j++)
	{
		var elem = selectColl[j];
		if(elem.mcIsHidden && elem.mcIsHidden > 0 && elem.mcHiddenLast)
		{
			elem.style.visibility = elem.mcHiddenLast;
			elem.mcIsHidden = 0;
		}
	}
}

function Enhance()
{
	iCurPos = iCurPos + iStep;
	oCurFrame.style.height = iCurPos;
	oCurFrame.style.display = "block";
	oCurDiv.style.top = iCurPos;
	oCurDiv.style.position = "relative";
	if (iCurPos >= 200)
	{
		window.clearInterval(iTimerID);
		
		var otdDock = getRawObject("tdDock");
		if (otdDock)
			otdDock.style.visibility = "visible";
			
		hideSelects("divTopMenu");
	}
}

function HideFrames(e)
{
	try
	{
		e = (e) ? e : ((window.event) ? event : null);
		if (e)
		{
			var src = (e.target) ? e.target : ((e.srcElement) ? e.srcElement : null);
			if (src && src.className && src.className == "tabLink") return;
			if(src && src.className && (src.className=="cellclass" || src.className=="hovercellclass" || src.className=="btndown" || src.className=="btndown2")) return;
		}
		
		try{
			closeMenu();
		}
		catch(e){}
		
		oCurFrame = document.getElementById("frmList");
			
		if (oCurFrame && oCurFrame.style.display == "block")
		{
			oCurFrame.style.display = "none";

			oCurDiv = document.getElementById("divTopMenu");
			if (oCurDiv) oCurDiv.style.position = "static";
			
			var otdDock = document.getElementById("tdDock");
			if (otdDock)
			{
				if (TopFrameVisible()) otdDock.style.visibility = "visible";
				else otdDock.style.visibility = "hidden";
			}
			
			showSelects();
		}
	}
	catch (e) {}
}

function ExpandLeftTab(str1)
{
	if(!str1 || str1=='')
	{
		var _href = location.href;
		if(_href.toLowerCase().indexOf('/admin/')>=0)
			str1 = "MAdmin";
		else if(_href.toLowerCase().indexOf('/directory/')>=0)
			str1 = "MGroups";
		else if(_href.toLowerCase().indexOf('/documents/')>=0)
			str1 = "MDocuments";
		else if(_href.toLowerCase().indexOf('/filelibrary/')>=0)
			str1 = "MLibrary";
		else if(_href.toLowerCase().indexOf('/filestorage/')>=0)
			str1 = "MLibrary";
		else if(_href.toLowerCase().indexOf('/incidents/')>=0)
			str1 = "MIssues";
		else if(_href.toLowerCase().indexOf('/lists/')>=0)
			str1 = "MLists";
		else if(_href.toLowerCase().indexOf('/projects/')>=0)
			str1 = "MProjects";
		else if(_href.toLowerCase().indexOf('/userreports/')>=0)
			str1 = "MMain";
		else if(_href.toLowerCase().indexOf('/workspace/')>=0)
			str1 = "MMain";
		else
			str1="no";	
		/*else if(_href.toLowerCase().indexOf('/calendar/')>=0)
			str1 = "no";
		else if(_href.toLowerCase().indexOf('/events/')>=0)
			str1 = "no";
		else if(_href.toLowerCase().indexOf('/external/')>=0)
			str1 = "no";
		else if(_href.toLowerCase().indexOf('/tasks/')>=0)
			str1 = "no";
		else if(_href.toLowerCase().indexOf('/timetracking/')>=0)
			str1 = "no";
		else if(_href.toLowerCase().indexOf('/todo/')>=0)
			str1 = "no";*/
	}
	if(str1 && str1!='' && str1!='no')
		window.top.left.ExpandNodeItem(str1);
}

function getXMLHTTP()
{
	var request = null;
	if(window.XMLHttpRequest)
	{
		request = new XMLHttpRequest();
	} 
	else if (window.ActiveXObject)
	{
		request = new ActiveXObject("Msxml2.XMLHTTP");
		if (! request)
			request = new ActiveXObject("Microsoft.XMLHTTP");
	}
	return request;
}
