/*  Add events to objects
--------------------------------------- */

var FTB_EventCache = [];
function FTB_AddEvents(obj, evTypes, fn) {
	for (i=0; i<evTypes.length; i++) FTB_AddEvent(obj, evTypes[i], fn);
};
function FTB_AddEvent(obj, evType, fn, useCapture) {
	//if (useCapture==undefined) 
	useCapture=true;
	if (obj.addEventListener) {
		obj.addEventListener(evType, fn, useCapture);
	} else if (obj.attachEvent) {
		obj.attachEvent('on'+evType, fn);
	}
	FTB_EventCache.push([obj, evType, fn, useCapture]);
};
function FTB_RemoveEvent(obj, evType, fn, useCapture) {
    if (useCapture==undefined) useCapture=true;
	if (obj.removeEventListener) {
      obj.removeEventListener(evType, fn, useCapture);
    } else if (obj.detachEvent) {
      obj.detachEvent('on' + evType, fn);
    }
};
function FTB_Unload() {
    for (var i = 0; i < FTB_EventCache.length; i++) {
      FTB_RemoveEvent.apply(this, FTB_EventCache[i]);
      FTB_EventCache[i][0] = null;
    }
    FTB_EventCache = false;	
};
FTB_AddEvent(window,'unload',FTB_Unload, false);

/*  API for holding all FreeTextBox's
--------------------------------------- */
var FTB_API = new Object();
var FTB_Names = [];

/* Browser Detection 'FTB_Browser'
--------------------------------------- */
function FTB_BrowserDetect() {
	doc=window.document;
	navVersion=navigator.appVersion.toLowerCase();
	this.ie4=(!doc.getElementById&&doc.all)?true:false;
	this.ie5=(navVersion.indexOf("msie 5.0")!=-1)?true:false;
	this.ie55=(navVersion.indexOf("msie 5.5")!=-1)?true:false;
	this.ie6=(navVersion.indexOf("msie 6.0")!=-1)?true:false;
	this.ie7=(navVersion.indexOf("msie 7.0")!=-1)?true:false;
	this.isIE=(this.ie5||this.ie55||this.ie6||this.ie7)?true:false;
	this.isGecko=!this.isIE;
};
FTB_Browser = new FTB_BrowserDetect();

/* OOP Timeout Manager 'FTB_Timeout'
--------------------------------------- */
function FTB_TimeoutManager() {
	this.pendingCalls = {};		
};
FTB_TimeoutManager.prototype.addMethod = function(name,obj,method,delay,arg1,arg2) {
	this.clearMethod(name);
	this.pendingCalls[name] = new FTB_TimeoutCall(obj,method,arg1,arg2);
	this.pendingCalls[name].timeout = 
		setTimeout('FTB_Timeout.executeMethod("' + name + '");',delay);
};
FTB_TimeoutManager.prototype.executeMethod = function(name) {
	call = this.pendingCalls[name];
	if (call != null) {
		call.obj[call.method](call.arg1,call.arg2);
		this.clearMethod(name);
	}
};
FTB_TimeoutManager.prototype.clearMethod = function(name) {
	if (this.pendingCalls[name]) 
		delete this.pendingCalls[name];
};
//* Object to hold timeout reference
function FTB_TimeoutCall(obj,method,arg1,arg2) {
	this.obj = obj;
	this.method = method;
	this.arg1 = arg1;
	this.arg2 = arg2;
	this.timeout = null;
};
FTB_Timeout = new FTB_TimeoutManager();

/* Constants 
----------------------------------------- */
FTB_MODE_HTML = 0;
FTB_MODE_DESIGN = 1;
FTB_MODE_PREVIEW = 2;
//
FTB_PASTE_DEFAULT = 0;
FTB_PASTE_DISABLED = 1;
FTB_PASTE_TEXT = 2;
//
FTB_TAB_DISABLED = 0;
FTB_TAB_NEXTCONTROL = 1;
FTB_TAB_INSERTSPACES = 2;
//
FTB_BUTTON_ON = 0;
FTB_BUTTON_OFF = 1;
//
FTB_BREAK_P = 0;
FTB_BREAK_BR = 1;
//
FTB_KEY_TAB = 9;
FTB_KEY_ENTER = 13;
FTB_KEY_QUOTE = 222;
FTB_KEY_V = 86;
FTB_KEY_P = 86;
FTB_KEY_B = 66;
FTB_KEY_I = 73;
FTB_KEY_U = 85;
FTB_KEY_Z = 90;
FTB_KEY_Y = 89;
//
FTB_CODE_OPENCURLY = '&#8220;';
FTB_CODE_CLOSECURLY = '&#8221;';
//
FTB_BUTTON_STYLEDBACKGROUNDS = 0;
FTB_BUTTON_IMAGEBACKGROUNDS = 1;


/* Misc Methods
------------------------------------------ */
function FTB_SetListValue(list, value, checkText) {
	checkText = checkText || false;
	value = String(value).toLowerCase();

	for (var i=0; i<list.options.length; i++) {
		if (list.options[i].value.toLowerCase() == value || (checkText && list.options[i].text.toLowerCase() == value)) {
			list.selectedIndex = i;
			return;
		}
	}
};
function FTB_ParseUnit(styleString) {
	var unit = new Object();
	unit.value = 0;
	unit.unitType = '';
	for(var i=0; i<styleString.length; i++) {
		if (isNaN(styleString.charAt(i)))
			unit.unitType += styleString.charAt(i);
		else 
			unit.value = parseInt(unit.value.toString() + styleString.charAt(i));
	}
	return unit;
};
function FTB_DecToHex(dec) {
	return parseInt(dec).toString(16); 
};
function FTB_RgbToHex(r,g,b) {
	return "#" + FTB_IntToHex(r) + FTB_IntToHex(g) + FTB_IntToHex(b);
};
function FTB_IntToHexColor( intColor ) {
	if (!intColor) return null;
	intColor = intColor.toString(16).toUpperCase();
	while (intColor.length < 6) intColor = "0" + intColor;
	return "#" + intColor.substring(4,6) + intColor.substring(2,4) + intColor.substring(0,2);
};
function FTB_RgbStringToHex(rgbString){ 
	var r, g, b;
	rgbString = rgbString.toString().toLowerCase();	
	if(rgbString.indexOf("rgb(") == -1 || rgbString.indexOf(")") == -1) return rgbString;
	
	rgbString = rgbString.substring(rgbString.indexOf("(")+1, rgbString.indexOf(")")-1);
	rgb = rgbString.split(',');
	r = rgb[0];
	g = rgb[1];
	if (rgb.length == 2) b = rbg[2]; else b = 0;	
	return FTB_RgbToHex(r,g,b);
};
function FTB_IntToHex(dec){ 
	var result = (parseInt(dec).toString(16)); 
	if(result.length ==1) 
	result= ("0" +result); 
	return result.toUpperCase(); 
};
function FTB_GetQueryStringValues(url) {	
	url = new String(url);
	
	var queryStringValues=new Object();
	var querystring=url.substring(url.indexOf('?')+1, url.length);
	var querystringSplit = querystring.split('&');
	
	for (i=0; i < querystringSplit.length; i++){
		var pair=querystringSplit[i].split('=');
		var name=pair[0];
		var value=pair[1];
		queryStringValues[ name ]=value;
	}
	return queryStringValues;
};
/* Static Popup HTML
---------------------------------------- */
var FTB_PopUpStyle = "\
html, body { \
	background-color: #ECE9D8; \
	color: #000000; \
	font: 11px Tahoma,Verdana,sans-serif; \
	padding: 0px; \
} \
body { margin: 5px; } \
form { margin: 0px; padding: 0px;} \
table { \
  font: 11px Tahoma,Verdana,sans-serif; \
} \
form p { \
  margin-top: 5px; \
  margin-bottom: 5px; \
} \
h3 { margin: 0; margin-top: 4px;  margin-bottom: 5px; font-size: 12px; border-bottom: 2px solid #90A8F0; color: #90A8F0;} \
.fl { width: 9em; float: left; padding: 2px 5px; text-align: right; } \
.fr { width: 7em; float: left; padding: 2px 5px; text-align: right; } \
fieldset { padding: 0px 10px 5px 5px; } \
button { width: 75px; } \
select, input, button { font: 11px Tahoma,Verdana,sans-serif; } \
.space { padding: 2px; } \
.title { background: #ddf; color: #000; font-weight: bold; font-size: 120%; padding: 3px 10px; margin-bottom: 10px; \
border-bottom: 1px solid black; letter-spacing: 2px; \
} \
.f_title { text-align:right; }\
.footer { border-top:2px solid #90A8F0; padding-top: 3px; margin-top: 4px; text-align:right; }\
";

var FTB_PopUpHeader = new String("<html><body> \
<head>\
<title>POPUP_TITLE</title>\
<style type='text/css'>\
html, body { \
	background-color: #ECE9D8; \
	color: #000000; \
	font: 11px Tahoma,Verdana,sans-serif; \
	padding: 0px; \
} \
body { margin: 5px; } \
form { margin: 0px; padding: 0px;} \
table { \
  font: 11px Tahoma,Verdana,sans-serif; \
} \
form p { \
  margin-top: 5px; \
  margin-bottom: 5px; \
} \
h3 { margin: 0; margin-top: 4px;  margin-bottom: 5px; font-size: 12px; border-bottom: 2px solid #90A8F0; color: #90A8F0;} \
.fl { width: 9em; float: left; padding: 2px 5px; text-align: right; } \
.fr { width: 7em; float: left; padding: 2px 5px; text-align: right; } \
fieldset { padding: 0px 10px 5px 5px; } \
button { width: 75px; } \
select, input, button { font: 11px Tahoma,Verdana,sans-serif; } \
.space { padding: 2px; } \
.title { background: #ddf; color: #000; font-weight: bold; font-size: 120%; padding: 3px 10px; margin-bottom: 10px; \
border-bottom: 1px solid black; letter-spacing: 2px; \
} \
.f_title { text-align:right; }\
.footer { border-top:2px solid #90A8F0; padding-top: 3px; margin-top: 4px; text-align:right; }\</style>\
<script type='text/javascript'>\
POPUP_SCRIPT\
</script>\
</head>\
<body>\
<form action=''> \
<h3>POPUP_TITLE</h3> \
POPUP_HTML \
</form> \
</body> \
</html>");