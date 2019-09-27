FTB_FreeTextBox.prototype.InsertDiv = function() {
	var div = window.document.createElement("div");
	div.style.width = "200px";
	div.style.height = "200px";
	div.style.border = "dotted 1px gray";
	this.InsertElement(div);

};
FTB_FreeTextBox.prototype.EditStyle = function() {
	// custom implimentation of GetParentElement() and GetSelection() and GetRange()
	el = this.GetParentElement();

	this.EditElementStyle(el);
};
FTB_FreeTextBox.prototype.EditElementStyle = function(el) {

	var styleWin = window.open("","propWin","width=530,height=420,status=0,toolbars=0");
	if (styleWin) {
		styleWin.focus();
	} else {
		alert("Please turn off your PopUp blocking software");
		return;
	}
	//return;
	html = FTB_StyleEditorHtml;
		
	styleWin.document.body.innerHTML = '';
	styleWin.document.open();
	styleWin.document.write( html );
	styleWin.document.close();	
	
	launchParameters = new Object();
	launchParameters['ftb'] = this;
	launchParameters['element'] = el;	
	styleWin.launchParameters = launchParameters;
	styleWin.load();
};
/* START: Table Functions 
These functions are derived from HTMLAREA who
gave permission for them to be used in FreeTextBox
- Thanks HTMLAREA!!
----------------------------------------------- */

FTB_FreeTextBox.prototype.InsertTableColumnBefore = function() {
	this.InsertTableColumn(false);
};
FTB_FreeTextBox.prototype.InsertTableColumnAfter = function() {
	this.InsertTableColumn(true);
};
FTB_FreeTextBox.prototype.InsertTableColumn = function(after) {
   var td = this.GetNearest("td");
   if (!td) {
      return;
   }
   var rows = td.parentNode.parentNode.rows;
   var index = td.cellIndex;
   for (var i = rows.length; --i >= 0;) {
      var tr = rows[i];
      var otd = this.designEditor.document.createElement("td");
      otd.innerHTML = (FTB_Browser.isIE) ? "" : "<br />";

      //if last column and insert column after is select append child
      if (index==tr.cells.length-1 && after) {
         tr.appendChild(otd);
      } else {
         var ref = tr.cells[index + ((after) ? 1 : 0)]; // 0 
         tr.insertBefore(otd, ref);
      } 
   }
};
FTB_FreeTextBox.prototype.InsertTableRowBefore = function() {
	this.InsertTableRow(false);
};
FTB_FreeTextBox.prototype.InsertTableRowAfter = function() {
	this.InsertTableRow(true);
};
FTB_FreeTextBox.prototype.InsertTableRow = function(after) {
	var tr = this.GetNearest("tr");
	if (!tr) return;
	var otr = tr.cloneNode(true);
	this.ClearRow(otr);
	tr.parentNode.insertBefore(otr, ((after) ? tr.nextSibling : tr));
};
FTB_FreeTextBox.prototype.DeleteTableColumn = function() {
	var td = this.GetNearest("td");
	if (!td) {
		return;
	}
	var index = td.cellIndex;
	if (td.parentNode.cells.length == 1) {
		return;
	}
	// set the caret first to a position that doesn't disappear
	this.SelectNextNode(td);
	var rows = td.parentNode.parentNode.rows;
	for (var i = rows.length; --i >= 0;) {
		var tr = rows[i];
		tr.removeChild(tr.cells[index]);
	}
};
FTB_FreeTextBox.prototype.DeleteTableRow = function() {
	var tr = this.GetNearest("tr");
	if (!tr) {
		return;
	}
	var par = tr.parentNode;
	if (par.rows.length == 1) {
		return;
	}
	// set the caret first to a position that doesn't disappear.
	this.SelectNextNode(tr);
	par.removeChild(tr);
};
// helper table
FTB_FreeTextBox.prototype.ClearRow = function(tr) {
	var tds = tr.getElementsByTagName("td");
	for (var i = tds.length; --i >= 0;) {
		var td = tds[i];
		td.rowSpan = 1;
		td.innerHTML = (FTB_Browser.isIE) ? "" : "<br />";
	}
};
FTB_FreeTextBox.prototype.SplitRow = function(td) {
	var n = parseInt("" + td.rowSpan);
	var nc = parseInt("" + td.colSpan);
	td.rowSpan = 1;
	tr = td.parentNode;
	var itr = tr.rowIndex;
	var trs = tr.parentNode.rows;
	var index = td.cellIndex;
	while (--n > 0) {
		tr = trs[++itr];
		var otd = editor._doc.createElement("td");
		otd.colSpan = td.colSpan;
		otd.innerHTML = mozbr;
		tr.insertBefore(otd, tr.cells[index]);
	}
};
FTB_FreeTextBox.prototype.SplitCol = function(td) {
	var nc = parseInt("" + td.colSpan);
	td.colSpan = 1;
	tr = td.parentNode;
	var ref = td.nextSibling;
	while (--nc > 0) {
		var otd = editor._doc.createElement("td");
		otd.rowSpan = td.rowSpan;
		otd.innerHTML = mozbr;
		tr.insertBefore(otd, ref);
	}
}

FTB_FreeTextBox.prototype.SplitCell = function(td) {
	var nc = parseInt("" + td.colSpan);
	splitCol(td);
	var items = td.parentNode.cells;
	var index = td.cellIndex;
	while (nc-- > 0) {
		this.SplitRow(items[index++]);
	}
};
/* FORM Functions
-------------------------------------- */
FTB_FreeTextBox.prototype.IsInForm = function() {
	return (this.GetNearest("form")) ? true : false ;
};
FTB_FreeTextBox.prototype.InsertForm = function() {
	var form = window.document.createElement("form");
	this.InsertElement(form);
};
FTB_FreeTextBox.prototype.InsertCheckBox = function() {
	this.InsertInputElement("","checkbox");
};
FTB_FreeTextBox.prototype.InsertTextBox = function() {
	this.InsertInputElement("","text");
};
FTB_FreeTextBox.prototype.InsertRadioButton = function() {
	this.InsertInputElement("","radio");
};
FTB_FreeTextBox.prototype.InsertButton = function() {
	this.InsertInputElement("","button");
};
FTB_FreeTextBox.prototype.InsertDropDownList = function() {
	var select = window.document.createElement("select");
	this.InsertElement(select);
};
FTB_FreeTextBox.prototype.InsertTextArea = function() {
	var textarea = window.document.createElement("textarea");
	this.InsertElement(textarea);
};
FTB_FreeTextBox.prototype.InsertInputElement = function(id,type) {
	var input = window.document.createElement("input");
	input.id = id;

	input.type = type;
	this.InsertElement(input);
}
/* Color picker Functions
-------------------------------------- */
FTB_FreeTextBox.prototype.FontForeColorPicker = function() {
	this.LaunchColorPickerWindow('forecolor');
};
FTB_FreeTextBox.prototype.FontBackColorPicker = function() {
	this.LaunchColorPickerWindow('backcolor');
};
FTB_FreeTextBox.prototype.LaunchColorPickerWindow = function(commandName, startValue) {

	var pickerWin = window.open("","colorPickerWin","width=290,height=180");
	if (pickerWin) {
		pickerWin.focus();
	} else {
		alert("Please turn off your PopUp blocking software");
		return;
	}
	
	pickerWin.document.body.innerHTML = '';
	pickerWin.document.open();
	pickerWin.document.write(FTB_ColorPickerHtml);
	pickerWin.document.close();

	launchParameters = new Object();
	launchParameters['ftb'] = this;
	launchParameters['commandName'] = commandName;
	pickerWin.launchParameters = launchParameters;
	pickerWin.load();
};
FTB_FreeTextBox.prototype.InsertImage = function() {
	var imageWin = window.open("","imageWin","width=500,height=310");
	if (imageWin) {
		imageWin.focus();
	} else {
		alert("Please turn off your PopUp blocking software");
		return;
	}


	//imageWin.document.body.innerHTML = '';
	imageWin.document.open();
	imageWin.document.write(FTB_ImagePopUpHtml);
	imageWin.document.close();
	
	launchParameters = new Object();
	launchParameters['ftb'] = this;
	imageWin.launchParameters = launchParameters;
	imageWin.load();
};
/* Misc Pro features
--------------------------------------- */
FTB_FreeTextBox.prototype.WordClean = function() {

	var text = this.designEditor.document.body.innerHTML;
	
	text=text.replace(/<FONT[^>]*>/gi,"");
	text=text.replace(/<\/FONT>/gi,"");
	text=text.replace(/<U>/gi,"");
	text=text.replace(/<\/U>/gi,"");
	text=text.replace(/<H[^>]*>/gi,"");
	text=text.replace(/<\/H[^>]*>/gi,"");
	
	// save BRs
	text=text.replace(/<BR[^>]*>/gi,"&linebreak");
	
	// Change these tags.
	text=text.replace(/<B>]*>/gi,"&bold");
	text=text.replace(/<\/B[^>]*>/gi,"&cbold");
	text=text.replace(/<STRONG[^>]*>/gi,"&bold");
	text=text.replace(/<\/STRONG[^>]*>/gi,"&cbold");

	text=text.replace(/<I[^>]*>/gi,"&ital");
	text=text.replace(/<\/I[^>]*>/gi,"&cital");
	text=text.replace(/<EM[^>]*>/gi,"&ital");
	text=text.replace(/<\/EM[^>]*>/gi,"&cital");
	
	text=text.replace(/<UL[^>]*>/gi,"&ultag");
	text=text.replace(/<LI[^>]*>/gi,"&litag");
	text=text.replace(/<OL[^>]*>/gi,"&oltag");
	text=text.replace(/<\/OL>/gi,"&olctag");
	text=text.replace(/<\/LI>/gi,"&lictag");
	text=text.replace(/<\/UL>/gi,"&ulctag");

	text=text.replace(/<P[^>]*>/gi,"&parag");
	text=text.replace(/<\/P>/gi,"");
	
	/*
	text=text.replace(/”/gi,'\"');
	text=text.replace(/“/gi,'\"');
	text=text.replace(/„/gi,'\"');
	text=text.replace(/mailto:/gi,'\"');
	text=text.replace(/Ä/g,"&Auml;");
	text=text.replace(/Ö/g,"&Ouml;");
	text=text.replace(/Ü/g,"&Uuml;");
	text=text.replace(/ä/g,"&auml;");
	text=text.replace(/ö/g,"&ouml;");
	text=text.replace(/ü/g,"&uuml;");
	text=text.replace(/ß/gi,"&szlig;");	
	*/
	
	text=text.replace(/&lt;[^>]&gt*;/gi,"");
	text=text.replace(/&lt;\/[^>]&gt*;/gi," ");
	text=text.replace(/<o:[^>]*>/gi,"");
	text=text.replace(/<\/o:[^>]*>/gi,"");
	text=text.replace(/<\?xml:[^>]*>/gi,"");
	text=text.replace(/<\/?st[^>]*>/gi,"");
	text=text.replace(/<[^>]*</gi,"<");
	text=text.replace(/<SPAN[^>]*>/gi,"");
	text=text.replace(/<SPAN[^class]*>/gi,"");
	text=text.replace(/<\/SPAN>/gi,"");
	//text=text.replace(/<\/A>/gi,"");
	
	// Clear the inner parts of other tags.
	text=text.replace(/style=[^>]*"/g,' ');
	text=text.replace(/style=[^>]*'/g," ");
	text=text.replace(/style=[^>]*>/g,">");
	text=text.replace(/lang=[^>]*>/g,">");
	text=text.replace(/name=[^>]* /g,"");
	text=text.replace(/name=[^>]*>/g,">");
	text=text.replace(/<A[^>]*>/g,"");
	
	//text=text.replace(/<p[^>]*>/gi,"<p>");

	
	// Put the tags back
	text=text.replace(/&linebreak/g,"<br />");
	
	text=text.replace(/&bold/g,"<B>");
	text=text.replace(/&cbold/g,"</B>");

	text=text.replace(/&ital/g,"<EM>");
	text=text.replace(/&cital/g,"</EM>");

	text=text.replace(/&ultag/g,"<UL>");
	text=text.replace(/&litag/g,"<LI>");
	text=text.replace(/&oltag/g,"<OL>");
	text=text.replace(/&olctag/g,"<\/OL>");	
	text=text.replace(/&lictag/g,"<\/LI>");
	text=text.replace(/&ulctag/g,"<\/UL>");

	text=text.replace(/&parag/g,"<BR>");
	
	this.designEditor.document.body.innerHTML = text;
};
FTB_FreeTextBox.prototype.CreateLink = function() {
	// impliment pro feature of PopUp window
	
	var sel = this.GetSelection();
	var text = '';
	if (FTB_Browser.isIE) {
		text = sel.createRange().text;
	} else {
		text = new String(sel);
	}
	
	if (text == '') {
		alert('Please select some text');
		return;
	}

	
	var linkWin = window.open("","linkWin","width=350,height=155");
	if (linkWin) {
		linkWin.focus();
	} else {
		alert("Please turn off your PopUp blocking software");
		return;
	}
	
	linkWin.document.body.innerHTML = '';
	linkWin.document.open();	
	linkWin.document.write(FTB_LinkPopUpHtml);
	linkWin.document.close();	
	
	launchParameters = new Object();
	launchParameters['ftb'] = this;
	linkWin.launchParameters = launchParameters;
	linkWin.load();
};

/* PopUpScripts 
---------------------------------------- */


var FTB_ImagePopUpHtml = new String("\
<html><body> \
<head>\
<title>Image Editor</title>\
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
function updateImage() {\
	ftb = window.launchParameters['ftb'];\
	img = ftb.GetNearest('img');\
	src = document.getElementById('image_src');\
	if (src.value == '') {\
		alert('You must enter an image url');\
		return false;\
	}\
	if (!img) {\
		var tempUrl = 'http://tempuri.org/tempuri.html';\
		ftb.ExecuteCommand('insertimage',null,tempUrl);\
		var imgs = ftb.designEditor.document.getElementsByTagName('img');\
		for (var i=0;i<imgs.length;i++) {\
			if (imgs[i].src == tempUrl) {\
				img = imgs[i];\
				break;\
			}\
		}\
	}\
	updateImageProperties(img);\
}\
function updateImageProperties(img) {\
	if (img) {\
		src = document.getElementById('image_src');\
		alt = document.getElementById('image_alt');\
		align = document.getElementById('image_align');\
		border = document.getElementById('image_border');\
		hspace = document.getElementById('image_hspace');\
		vspace = document.getElementById('image_vspace');\
		width = document.getElementById('image_width');\
		height = document.getElementById('image_height');\
		img.src = src.value;\
		img.setAttribute('temp_src',src.value);\
		if (alt.value != '') img.alt = alt.value;\
		if (align.value != '') img.align = align.value;\
		if (border.value != '') img.border = border.value;\
		if (hspace.value != '') img.hspace = hspace.value;\
		if (vspace.value != '') img.vspace = vspace.value;\
		if (width.value != '') img.width = width.value;\
		if (height.value != '') img.height = height.value;\
	}\
}\
function updatePreview() {\
	src = document.getElementById('image_src');\
	alt = document.getElementById('image_alt');\
	align = document.getElementById('image_align');\
	border = document.getElementById('image_border');\
	hspace = document.getElementById('image_hspace');\
	vspace = document.getElementById('image_vspace');\
	preview = document.getElementById('preview');\
	width = document.getElementById('image_width');\
	height = document.getElementById('image_height');\
	\
	if (width.value == ''|| height.value == '') preview.src = new Image();\
	preview.src = src.value;\
	if (alt.value != '') preview.alt = alt.value;\
	if (align.value != '') preview.align = align.value;\
	if (border.value != '') preview.border = border.value;\
	if (hspace.value != '') preview.hspace = hspace.value;\
	if (vspace.value != '') preview.vspace = vspace.value;\
	if (width.value != '') preview.width = width.value;\
	if (height.value != '') preview.height = height.value;\
}\
</script>\
</head>\
<body>\
<form action='' onsubmit='updateImage();window.close();'> \
<h3>Image Editor</h3> \
<table><tr><td>\
	<table>\
	<tr><td class='f_title'>Image Source:</td>\
	<td><input type='text' id='image_src' style='width:200px;' onblur='updatePreview();' /></td></tr>\
	<tr><td class='f_title'>Alternate Text:</td>\
	<td><input type='text' id='image_alt' style='width:200px;' onblur='updatePreview();' /></td></tr>\
	</table>\
</td><td rowspan='3' valign='top'>\
	<fieldset><legend>Preview</legend>\
	<div style='width:180px;height:180px;overflow:scroll;background-color:#fff;'>\
		<p><img id='preview' />Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Vestibulum accumsan, ipsum ut dapibus dapibus, nunc arcu congue velit, sit amet pretium est felis ut libero. Suspendisse hendrerit vestibulum pede.</p>\
	</div>\
	</fieldset>\
</td></tr><tr><td>\
	<fieldset><legend>Layout</legend><table>\
	<tr><td class='f_title'>Alignment:</td>\
	<td><select id='image_align' style='width:70px;' onchange='updatePreview();'>\
	<option value='' selected='1'>Not set</option> \
	<option value='left'         >Left</option> \
	<option value='center'       >Center</option> \
	<option value='right'        >Right</option> \
	<option value='texttop'      >TextTop</option> \
	<option value='AbsMiddle'    >AbsMiddle</option> \
	<option value='Baseline'     >Baseline</option> \
	<option value='AbsBottom'    >AbsBottom</option> \
	<option value='Middle'       >Middle</option> \
	<option value='Top'          >Top</option> \
	</select> \
	</td></tr>\
	<tr><td class='f_title'>Border Thickness:</td>\
	<td><input type='text' id='image_border' style='width:70px;' onblur='updatePreview();' /></td></tr>\
	</table>\
	</fieldset>\
</td></tr><tr><td>\
	<table cellspacing='0' cellpadding='0'><tr><td style='padding-right:5px;'>\
		<fieldset><legend>Spacing</legend><table>\
		<tr><td class='f_title'>Horizontal:</td>\
		<td><input type='text' id='image_hspace' style='width:30px;' onblur='updatePreview();' /></td></tr>\
		<tr><td class='f_title'>Vertical:</td>\
		<td><input type='text' id='image_vspace' style='width:30px;' onblur='updatePreview();' /></td></tr>\
		</table>\
		</fieldset>\
	</td><td>\
		<fieldset><legend>Dimensions</legend><table>\
		<tr><td class='f_title'>Width:</td>\
		<td><input type='text' id='image_width' style='width:40px;' onblur='updatePreview();' /></td></tr>\
		<tr><td class='f_title'>Height:</td>\
		<td><input type='text' id='image_height' style='width:40px;' onblur='updatePreview();' /></td></tr>\
		</table>\
		</fieldset>\
	</td></tr></table>\
</td></tr></table>\
<div class='footer'>\
<button type='button' name='updateImageButton' id='updateImageButton' onclick='updateImage();window.close();'>OK</button>\
<button type='button' name='cancel' id='cancelButton' onclick='window.close();'>Cancel</button>\
</div>\
<script type='text/javascript'>\
function load() {\
	ftb = window.launchParameters['ftb'];\
	img = ftb.GetNearest('img');\
	src = document.getElementById('image_src');\
	alt = document.getElementById('image_alt');\
	align = document.getElementById('image_align');\
	border = document.getElementById('image_border');\
	hspace = document.getElementById('image_hspace');\
	vspace = document.getElementById('image_vspace');\
	width = document.getElementById('image_width');\
	height = document.getElementById('image_height');\
	if (img) {\
		var imgUrl = img.getAttribute('temp_src');\
		src.value = (imgUrl) ? imgUrl : img.src;\
		alt.value = img.alt;\
		window.opener.FTB_SetListValue(align,img.align,true);\
		border.value = img.border;\
		hspace.value = img.hspace;\
		vspace.value = img.vspace;\
		width.value = img.width;\
		height.value = img.height;\
		updatePreview();\
	}\
}\
</script>\
</form> \
</body> \
</html>");

var FTB_LinkPopUpHtml = new String("\
<html><body> \
<head>\
<title>Link Editor</title>\
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
.footer { border-top:2px solid #90A8F0; padding-top: 3px; margin-top: 4px; text-align:right; }\
</style>\
<script type='text/javascript'>\
function insertLink() {\
	ftb = window.launchParameters['ftb'];\
	link = ftb.GetNearest('a');\
	href = document.getElementById('link_href');\
	if (href.value == '') {\
		alert('You must enter a link');\
		return false;\
	}\
	if (!link) {\
		var tempUrl = 'http://tempuri.org/tempuri.html';\
		ftb.ExecuteCommand('createlink',null,tempUrl);\
		var links = ftb.designEditor.document.getElementsByTagName('a');\
		for (var i=0;i<links.length;i++) {\
			if (links[i].href == tempUrl) {\
				link = links[i];\
				break;\
			}\
		}\
	}\
	updateLink(link);\
}\
function updateLink(link) {\
	if (link) {\
		href = document.getElementById('link_href');\
		title = document.getElementById('link_title');\
		target = document.getElementById('link_target');\
		cssClass = document.getElementById('link_cssClass');\
		targetVal = target.options[target.selectedIndex].value;\
		customtarget = document.getElementById('link_customtarget');\
		link.href = href.value;\
		link.setAttribute('temp_href', href.value) ;\
		if (title.value != '') \
			link.title = title.value;\
		if (cssClass.value != '') \
			link.className = cssClass.value;\
		if (targetVal == '_custom') {\
			if (customtarget.value != '') \
				link.target = customtarget.value; \
		} else { \
			if (targetVal != '') \
				link.target = targetVal;\
			else\
				link.removeAttribute('target');\
		} \
	}\
}\
function link_target_changed() {\
	list = document.getElementById('link_target');\
	customtarget = document.getElementById('link_customtarget');\
	if (list.options[list.options.selectedIndex].value == '_custom')\
		customtarget.style.display = '';\
	else\
		customtarget.style.display = 'none';\
}\
</script>\
</head>\
<body>\
<form action='' onsubmit='insertLink();window.close();'> \
<h3>Link Editor</h3> \
<fieldset><legend>Link Properties</legend><table>\
<tr><td class='f_title'>URL</td>\
<td><input type='text' id='link_href' style='width:250px;' /></td></tr>\
<tr><td class='f_title'>Title</td>\
<td><input type='text' id='link_title' style='width:250px;' /></td></tr>\
<tr><td class='f_title'>Target</td>\
<td><select id='link_target' style='width:150px;' onchange='link_target_changed();'>\
<option value=''>None</option>\
<option value='_blank'>New Window (_blank)</option>\
<option value='_top'>Top Frame (_top)</option>\
<option value='_parent'>Parent Frame (_parent)</option>\
<option value='_self'>Same Frame (_self)</option>\
<option value='_custom'>Custom Target</option>\
</select>&nbsp;\
<input type='text' id='link_customtarget' style='width:95px;display:none;' /> \
</td></tr>\
<tr style='display:none;'><td class='f_title'>Class</td>\
<td><input type='text' id='link_cssClass' style='width:250px;' /></td></tr>\
</table>\
</fieldset>\
<div class='footer'>\
<button type='button' name='insertLinkButton' id='insertLinkButton' onclick='insertLink();window.close();'>OK</button>\
<button type='button' name='cancel' id='cancelButton' onclick='window.close();'>Cancel</button>\
</div>\
<script type='text/javascript'>\
function load() {\
	ftb = window.launchParameters['ftb'];\
	link = ftb.GetNearest('a');\
	href = document.getElementById('link_href');\
	title = document.getElementById('link_title');\
	target = document.getElementById('link_target');\
	customtarget = document.getElementById('link_customtarget');\
	cssClass = document.getElementById('link_cssClass');\
	if (link) {\
		var url = link.getAttribute('temp_href');\
		href.value = (url != '') ? url : link.href;\
		title.value = link.title;\
		cssClass.value = link.className;\
		if (link.target == '' || link.target == '_blank' || link.target == '_top' || link.target == '_self' || link.target == '_parent')\
			window.opener.FTB_SetListValue(target,link.target,true);\
		else\
			window.opener.FTB_SetListValue(target,'_custom',false);\
		\
		if (target.options[target.options.selectedIndex].value == '_custom') {\
			customtarget.style.display='';\
			customtarget.value = link.target;\
		}\
	}\
}\
</script>\
</form> \
</body> \
</html>");


/* ColorPicker
---------------------------------------- */
var FTB_ColorPickerHtml = new String("\
<html><body> \
<head>\
<title>Image Editor</title>\
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
.footer { border-top:2px solid #90A8F0; padding-top: 3px; margin-top: 4px; text-align:right; }\
</style>\
<script type='text/javascript'>\
function cO(theTD) { \
	color = theTD.style.backgroundColor; \
	if (color.toString().toLowerCase().indexOf('rgb') > -1) color = window.opener.FTB_RgbStringToHex(color); \
	previewColor(color); \
	setTextField(color); \
} \
function cC(theTD) { \
	color = theTD.style.backgroundColor; \
	if (color.toString().toLowerCase().indexOf('rgb') > -1) color = window.opener.FTB_RgbStringToHex(color); \
	setTextField(color); \
	returnColor(color); \
} \
function setTextField(ColorString) { \
	document.getElementById('ColorText').value = ColorString.toUpperCase(); \
} \
function returnColor(ColorString) { \
	ftb = window.launchParameters['ftb'];\
	if (ftb) {\
		command = new String(window.launchParameters['commandName']);\
		ftb.ExecuteCommand(command,'',ColorString);\
	} else {\
	}\
	\
	window.close();	 \
} \
function userInput(theinput) {	 \
	previewColor(theinput.value); \
} \
function previewColor(theColor) { \
	try { \
		document.getElementById('PreviewDiv').style.backgroundColor = theColor; \
	} catch (e) {} \
}\
function okButton() { \
	theColor = document.getElementById('ColorText').value; \
	returnColor(theColor);\
}\
</script>\
</head>\
<body>\
<form action='' onsubmit='okButton();window.close();'> \
<h3>Image Editor</h3> \
	<style> \
	.cc { width:10;height:8; } \
	</style> \
	<table><tr><td>\
		<table cellpadding=0 cellspacing=1 style='background-color:ffffff;' border=0 ><tr> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FFFFFF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FFFFCC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FFFF99;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FFFF66;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FFFF33;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FFFF00;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FFCCFF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FFCCCC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FFCC99;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FFCC66;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FFCC33;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FFCC00;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FF99FF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FF99CC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FF9999;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FF9966;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FF9933;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FF9900;' class=cc></td> \
	</tr> \
	<tr> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CCFFFF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CCFFCC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CCFF99;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CCFF66;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CCFF33;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CCFF00;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CCCCFF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CCCCCC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CCCC99;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CCCC66;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CCCC33;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CCCC00;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CC99FF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CC99CC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CC9999;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CC9966;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CC9933;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CC9900;' class=cc></td> \
	</tr> \
	<tr> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:99FFFF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:99FFCC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:99FF99;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:99FF66;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:99FF33;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:99FF00;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:99CCFF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:99CCCC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:99CC99;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:99CC66;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:99CC33;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:99CC00;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:9999FF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:9999CC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:999999;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:999966;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:999933;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:999900;' class=cc></td> \
	</tr> \
	<tr> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:66FFFF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:66FFCC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:66FF99;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:66FF66;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:66FF33;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:66FF00;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:66CCFF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:66CCCC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:66CC99;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:66CC66;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:66CC33;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:66CC00;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:6699FF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:6699CC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:669999;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:669966;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:669933;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:669900;' class=cc></td> \
	</tr> \
	<tr> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:33FFFF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:33FFCC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:33FF99;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:33FF66;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:33FF33;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:33FF00;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:33CCFF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:33CCCC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:33CC99;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:33CC66;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:33CC33;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:33CC00;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:3399FF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:3399CC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:339999;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:339966;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:339933;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:339900;' class=cc></td> \
	</tr> \
	<tr> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:00FFFF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:00FFCC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:00FF99;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:00FF66;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:00FF33;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:00FF00;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:00CCFF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:00CCCC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:00CC99;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:00CC66;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:00CC33;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:00CC00;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:0099FF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:0099CC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:009999;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:009966;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:009933;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:009900;' class=cc></td> \
	</tr> \
	<tr> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FF66FF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FF66CC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FF6699;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FF6666;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FF6633;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FF6600;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FF33FF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FF33CC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FF3399;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FF3366;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FF3333;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FF3300;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FF00FF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FF00CC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FF0099;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FF0066;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FF0033;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:FF0000;' class=cc></td> \
	</tr> \
	<tr> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CC66FF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CC66CC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CC6699;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CC6666;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CC6633;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CC6600;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CC33FF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CC33CC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CC3399;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CC3366;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CC3333;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CC3300;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CC00FF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CC00CC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CC0099;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CC0066;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CC0033;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:CC0000;' class=cc></td> \
	</tr> \
	<tr> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:9966FF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:9966CC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:996699;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:996666;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:996633;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:996600;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:9933FF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:9933CC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:993399;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:993366;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:993333;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:993300;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:9900FF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:9900CC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:990099;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:990066;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:990033;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:990000;' class=cc></td> \
	</tr> \
	<tr> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:6666FF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:6666CC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:666699;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:666666;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:666633;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:666600;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:6633FF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:6633CC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:663399;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:663366;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:663333;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:663300;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:6600FF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:6600CC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:660099;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:660066;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:660033;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:660000;' class=cc></td> \
	</tr> \
	<tr> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:3366FF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:3366CC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:336699;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:336666;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:336633;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:336600;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:3333FF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:3333CC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:333399;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:333366;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:333333;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:333300;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:3300FF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:3300CC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:330099;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:330066;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:330033;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:330000;' class=cc></td> \
	</tr> \
	<tr> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:0066FF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:0066CC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:006699;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:006666;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:006633;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:006600;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:0033FF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:0033CC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:003399;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:003366;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:003333;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:003300;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:0000FF;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:0000CC;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:000099;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:000066;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:000033;' class=cc></td> \
		<td onmouseover='cO(this);' onclick='cC(this);' style='background-color:000000;' class=cc></td> \
	</tr> \
	</table> \
	</td><td valign='top'>\
		<div id='PreviewDiv' style='width:60px;height:40px;border: 1px solid black; background-color: START_VALUE;'>&nbsp;</div><br /> \
		<input type='text' name='ColorText' id='ColorText' style='width:60px;' onkeyup='userInput(this);' value=''> \
	</td></tr></table>\
	<div class='footer'>\
	<input type='button' id='ColorButton' value='OK' onclick='okButton();window.close();' />\
	<input type='button' id='CancelButton' value='Cancel' onclick='window.close();' >\
</div>\
</form>\
<script type='text/javascript'>\
function load() {\
	ftb = window.launchParameters['ftb'];\
	color = ftb.QueryCommandValue(window.launchParameters['commandName']);\
	if (color.toString().toLowerCase().indexOf('rgb') > -1) color = window.opener.FTB_RgbStringToHex(color); \
	previewColor(color); \
	setTextField(color); \
}\
</script>\
</body>\
</html>");


var FTB_StyleEditorHtml = "<html>\
<head>\
<style type='text/css'>\
h1 { margin: 0; margin-top: 4px;  margin-bottom: 5px; font-size: 12px; border-bottom: 2px solid #90A8F0; color: #90A8F0;}\
h2 { margin: 0; margin-top: 4px;  margin-bottom: 5px; font-size: 12px; border-bottom: 1px solid #90A8F0; color: #90A8F0;}\
#s_container { height: 240px;  clear: both;}\
#s_sampleContainer { background-color: #fff;}\
.tabMenu { list-style:none; margin:0px; padding:0px; }\
.tabMenu li {  margin:0px; padding:0px; }\
a:link, a:visited, a:active, a:hover { text-decoration:none; }\
a.selectedTab { color: #000; display: block; width:80px; border: 1px solid #aaa; border-top: 1px solid #fff; border-left: 1px solid #fff; padding: 4px; margin:0px; background-color: #Edf0D9;}\
a.unselectedTab { color: #000; display: block; width:80px; border: 0; padding: 5px; margin:0px; background-color: #ECE9D8;}\
html, body {  background-color: #ECE9D8;  color: #000000; font: 11px Tahoma,Verdana,sans-serif; padding: 0px; }\
body { margin: 5px; }\
form { margin: 0px; padding: 0px;}\
table { font: 11px Tahoma,Verdana,sans-serif; }\
fieldset { padding: 0px 10px 5px 5px; }\
button { width: 75px; }\
select, input, button { font: 11px Tahoma,Verdana,sans-serif; }\
.f_title { text-align:right; width: 70px;}\
</style>\
<script type='text/javascript'>\
function FTB_StyleEditor(id) {\
	this.id = id;\
};\
FTB_StyleEditor.prototype.UpdateStylePreview = function() {\
	this.ChangeElementStyle(this.previewDiv);\
};\
FTB_StyleEditor.prototype.ReturnStyle = function() {\
	el = window.launchParameters['element'];\
	this.ChangeElementStyle(el);\
	ftb = window.launchParameters['ftb'];\
};\
FTB_StyleEditor.prototype.Initialize = function() {\
	styleEditor = this;\
	\
	this.ftb = window.launchParameters['ftb'];\
	\
	this.previewDiv = document.getElementById('s_sample');\
	\
	this.fontLayer = document.getElementById('s_layer_font');\
	this.textLayer = document.getElementById('s_layer_text');\
	this.backgroundLayer = document.getElementById('s_layer_background');\
	this.spacingLayer = document.getElementById('s_layer_spacing');\
	this.bordersLayer = document.getElementById('s_layer_borders');\
	\
	this.fontTab = document.getElementById('s_tab_font');\
	this.fontTab.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.fontTab,'click',function() {this.styleEditor.ShowLayer(this.styleEditor.fontTab,this.styleEditor.fontLayer); });\
	this.textTab = document.getElementById('s_tab_text');\
	this.textTab.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.textTab,'click',function() {this.styleEditor.ShowLayer(this.styleEditor.textTab,this.styleEditor.textLayer); });\
	this.backgroundTab = document.getElementById('s_tab_background');\
	this.backgroundTab.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.backgroundTab,'click',function() {this.styleEditor.ShowLayer(this.styleEditor.backgroundTab,this.styleEditor.backgroundLayer); });\
	this.spacingTab = document.getElementById('s_tab_spacing');\
	this.spacingTab.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.spacingTab,'click',function() {this.styleEditor.ShowLayer(this.styleEditor.spacingTab,this.styleEditor.spacingLayer); });\
	this.bordersTab = document.getElementById('s_tab_borders');\
	this.bordersTab.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.bordersTab,'click',function() {this.styleEditor.ShowLayer(this.styleEditor.bordersTab,this.styleEditor.bordersLayer); });\
\
	\
	this.fontFamily = document.getElementById('s_fontFamily');\
	this.fontFamily.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.fontFamily,'keyup',function() {this.styleEditor.UpdateStylePreview(); } );\
	this.color = document.getElementById('s_color');\
	this.color.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.color,'keyup',function() {this.styleEditor.UpdateStylePreview(); });\
	this.size = document.getElementById('s_fontSize');\
	this.size.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.size,'keyup',function() {this.styleEditor.UpdateStylePreview(); });\
	this.sizeUnit = document.getElementById('s_fontSizeUnit');\
	this.sizeUnit.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.sizeUnit,'change',function() {this.styleEditor.UpdateStylePreview(); });\
\
	\
	this.bold = document.getElementById('s_bold');\
	this.bold.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.bold,'click',function() {this.styleEditor.UpdateStylePreview(); });\
	this.italic = document.getElementById('s_italic');\
	this.italic.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.italic,'click',function() {this.styleEditor.UpdateStylePreview(); });\
	this.smallcaps = document.getElementById('s_smallcaps');\
	this.smallcaps.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.smallcaps,'click',function() {this.styleEditor.UpdateStylePreview(); });\
	this.strikethrough = document.getElementById('s_strikethrough');\
	this.strikethrough.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.strikethrough,'click',function() {this.styleEditor.UpdateStylePreview(); });\
	this.underline = document.getElementById('s_underline');\
	this.underline.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.underline,'click',function() {this.styleEditor.UpdateStylePreview(); });\
	this.overline = document.getElementById('s_overline');\
	this.overline.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.overline,'click',function() {this.styleEditor.UpdateStylePreview(); });\
	this.capitalization = document.getElementById('s_capitalization');\
	this.capitalization.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.capitalization,'click',function() {this.styleEditor.UpdateStylePreview(); });\
	\
	this.halign = document.getElementById('s_halign');\
	this.halign.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.halign,'change',function() {this.styleEditor.UpdateStylePreview(); });\
	this.valign = document.getElementById('s_valign');\
	this.valign.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.valign,'change',function() {this.styleEditor.UpdateStylePreview(); });\
	this.textIndent = document.getElementById('s_textIndent');\
	this.textIndent.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.textIndent,'keyup',function() {this.styleEditor.UpdateStylePreview(); });\
	this.textIndentUnit = document.getElementById('s_textIndentUnit');\
	this.textIndentUnit.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.textIndentUnit,'change',function() {this.styleEditor.UpdateStylePreview(); });\
	this.direction = document.getElementById('s_direction');\
	this.direction.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.direction,'change',function() {this.styleEditor.UpdateStylePreview(); });\
\
	\
	this.backcolor = document.getElementById('s_backcolor');\
	this.backcolor.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.backcolor,'keyup',function() {this.styleEditor.UpdateStylePreview(); });\
	\
	this.letterSpacing = document.getElementById('s_letterSpacing');\
	this.letterSpacing.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.letterSpacing,'keyup',function() {this.styleEditor.UpdateStylePreview(); });\
	this.letterSpacingUnit = document.getElementById('s_letterSpacingUnit');\
	this.letterSpacingUnit.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.letterSpacingUnit,'change',function() {this.styleEditor.UpdateStylePreview(); });\
	this.lineHeight = document.getElementById('s_lineHeight');\
	this.lineHeight.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.lineHeight,'keyup',function() {this.styleEditor.UpdateStylePreview(); });\
	this.lineHeightUnit = document.getElementById('s_lineHeightUnit');\
	this.lineHeightUnit.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.lineHeightUnit,'change',function() {this.styleEditor.UpdateStylePreview(); });\
	\
	this.marginLeft = document.getElementById('s_marginLeft');\
	this.marginLeft.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.marginLeft,'keyup',function() {this.styleEditor.UpdateStylePreview(); });\
	this.marginLeftUnit = document.getElementById('s_marginLeft_unit');\
	this.marginLeftUnit.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.marginLeftUnit,'change',function() {this.styleEditor.UpdateStylePreview(); });\
	this.marginRight = document.getElementById('s_marginRight');\
	this.marginRight.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.marginRight,'keyup',function() {this.styleEditor.UpdateStylePreview(); });\
	this.marginRightUnit = document.getElementById('s_marginRight_unit');\
	this.marginRightUnit.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.marginRightUnit,'change',function() {this.styleEditor.UpdateStylePreview(); });\
	this.marginTop = document.getElementById('s_marginTop');\
	this.boldmarginTopstyleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.marginTop,'keyup',function() {this.styleEditor.UpdateStylePreview(); });\
	this.marginTopUnit = document.getElementById('s_marginTop_unit');\
	this.marginTopUnit.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.marginTopUnit,'change',function() {this.styleEditor.UpdateStylePreview(); });\
	this.marginBottom = document.getElementById('s_marginBottom');\
	this.marginBottom.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.marginBottom,'keyup',function() {this.styleEditor.UpdateStylePreview(); });\
	this.marginBottomUnit = document.getElementById('s_marginBottom_unit');\
	this.marginBottomUnit.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.marginBottomUnit,'change',function() {this.styleEditor.UpdateStylePreview(); });\
	\
	this.paddingLeft = document.getElementById('s_paddingLeft');\
	this.paddingLeft.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.paddingLeft,'keyup',function() {this.styleEditor.UpdateStylePreview(); });\
	this.paddingLeftUnit = document.getElementById('s_paddingLeft_unit');\
	this.paddingLeftUnit.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.paddingLeftUnit,'change',function() {this.styleEditor.UpdateStylePreview(); });\
	this.paddingRight = document.getElementById('s_paddingRight');\
	this.paddingRight.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.paddingRight,'keyup',function() {this.styleEditor.UpdateStylePreview(); });\
	this.paddingRightUnit = document.getElementById('s_paddingRight_unit');\
	this.paddingRightUnit.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.paddingRightUnit,'change',function() {this.styleEditor.UpdateStylePreview(); });\
	this.paddingTop = document.getElementById('s_paddingTop');\
	this.paddingTop.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.paddingTop,'keyup',function() {this.styleEditor.UpdateStylePreview(); });\
	this.paddingTopUnit = document.getElementById('s_paddingTop_unit');\
	this.paddingTopUnit.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.paddingTopUnit,'change',function() {this.styleEditor.UpdateStylePreview(); });\
	this.paddingBottom = document.getElementById('s_paddingBottom');\
	this.paddingBottom.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.paddingBottom,'keyup',function() {this.styleEditor.UpdateStylePreview(); });\
	this.paddingBottomUnit = document.getElementById('s_paddingBottom_unit');\
	this.paddingBottomUnit.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.paddingBottomUnit,'change',function() {this.styleEditor.UpdateStylePreview(); });\
	\
	this.borderFullWidth = document.getElementById('s_borderFull_width');\
	this.borderFullWidth.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.borderFullWidth,'keyup',function() {this.styleEditor.UpdateStylePreview(); });\
	this.borderFullWidthUnit = document.getElementById('s_borderFull_unit');\
	this.borderFullWidthUnit.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.borderFullWidthUnit,'change',function() {this.styleEditor.UpdateStylePreview(); });\
	this.borderFullStyle = document.getElementById('s_borderFull_style');\
	this.borderFullStyle.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.borderFullStyle,'change',function() {this.styleEditor.UpdateStylePreview(); });\
	this.borderFullColor = document.getElementById('s_borderFull_color');\
	this.borderFullColor.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.borderFullColor,'keyup',function() {this.styleEditor.UpdateStylePreview(); });\
	this.borderLeftWidth = document.getElementById('s_borderLeft_width');\
	this.borderLeftWidth.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.borderLeftWidth,'keyup',function() {this.styleEditor.UpdateStylePreview(); });\
	this.borderLeftWidthUnit = document.getElementById('s_borderLeft_unit');\
	this.borderLeftWidthUnit.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.borderLeftWidthUnit,'change',function() {this.styleEditor.UpdateStylePreview(); });\
	this.borderLeftStyle = document.getElementById('s_borderLeft_style');\
	this.borderLeftStyle.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.borderLeftStyle,'change',function() {this.styleEditor.UpdateStylePreview(); });\
	this.borderLeftColor = document.getElementById('s_borderLeft_color');\
	this.borderLeftColor.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.borderLeftColor,'keyup',function() {this.styleEditor.UpdateStylePreview(); });\
	this.borderRightWidth = document.getElementById('s_borderRight_width');\
	this.borderRightWidth.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.borderRightWidth,'keyup',function() {this.styleEditor.UpdateStylePreview(); });\
	this.borderRightWidthUnit = document.getElementById('s_borderRight_unit');\
	this.borderRightWidthUnit.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.borderRightWidthUnit,'change',function() {this.styleEditor.UpdateStylePreview(); });\
	this.borderRightStyle = document.getElementById('s_borderRight_style');\
	this.borderRightStyle.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.borderRightStyle,'change',function() {this.styleEditor.UpdateStylePreview(); });\
	this.borderRightColor = document.getElementById('s_borderRight_color');\
	this.borderRightColor.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.borderRightColor,'keyup',function() {this.styleEditor.UpdateStylePreview(); });\
	this.borderTopWidth = document.getElementById('s_borderTop_width');\
	this.borderTopWidth.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.borderTopWidth,'keyup',function() {this.styleEditor.UpdateStylePreview(); });\
	this.borderTopWidthUnit = document.getElementById('s_borderTop_unit');\
	this.borderTopWidthUnit.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.borderTopWidthUnit,'change',function() {this.styleEditor.UpdateStylePreview(); });\
	this.borderTopStyle = document.getElementById('s_borderTop_style');\
	this.borderTopStyle.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.borderTopStyle,'change',function() {this.styleEditor.UpdateStylePreview(); });\
	this.borderTopColor = document.getElementById('s_borderTop_color');\
	this.borderTopColor.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.borderTopColor,'keyup',function() {this.styleEditor.UpdateStylePreview(); });\
	this.borderBottomWidth = document.getElementById('s_borderBottom_width');\
	this.borderBottomWidth.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.borderBottomWidth,'keyup',function() {this.styleEditor.UpdateStylePreview(); });\
	this.borderBottomWidthUnit = document.getElementById('s_borderBottom_unit');\
	this.borderBottomWidthUnit.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.borderBottomWidthUnit,'change',function() {this.styleEditor.UpdateStylePreview(); });\
	this.borderBottomStyle = document.getElementById('s_borderBottom_style');\
	this.borderBottomStyle.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.borderBottomStyle,'change',function() {this.styleEditor.UpdateStylePreview(); });\
	this.borderBottomColor = document.getElementById('s_borderBottom_color');\
	this.borderBottomColor.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.borderBottomColor,'keyup',function() {this.styleEditor.UpdateStylePreview(); });\
	this.allBorders = document.getElementById('s_borders_type_all');\
	this.allBorders = document.getElementById('s_borderBottom_style');\
	window.opener.FTB_AddEvent(this.allBorders,'click',function() {this.styleEditor.UpdateStylePreview(); });\
	this.individualBorders = document.getElementById('s_borders_type_individual');\
	this.individualBorders = document.getElementById('s_borderBottom_style');\
	window.opener.FTB_AddEvent(this.individualBorders,'click',function() {this.styleEditor.UpdateStylePreview(); });\
	this.updateButton = document.getElementById('s_updateButton');\
	this.updateButton.styleEditor = styleEditor;\
	window.opener.FTB_AddEvent(this.updateButton,'click',function() {this.styleEditor.ReturnStyle();window.close(); });\
};\
FTB_StyleEditor.prototype.ChangeElementStyle = function(el) {\
\
	\
	try { el.style.fontFamily = this.fontFamily.value; } catch (e) {}\
	try { el.style.color = this.color.value; } catch (e) {}\
	if (this.size.value != '') try { el.style.fontSize = this.size.value + this.sizeUnit.options[this.sizeUnit.selectedIndex].value; } catch(e) {} else el.style.fontSize = '';\
	\
	el.style.fontWeight = (this.bold.checked) ? 'bold' : '' ;\
	el.style.fontStyle = (this.italic.checked) ? 'italic' : '' ;\
	el.style.fontVariant = (this.smallcaps.checked) ? 'small-caps' : '' ;\
	textDecoration = '';\
	textDecoration += (this.strikethrough.checked) ? 'line-through ' : '' ;\
	textDecoration += (this.underline.checked) ? 'underline ' : '' ;\
	textDecoration += (this.overline.checked) ? 'overline' : '' ;\
	el.style.textDecoration = textDecoration;\
	el.style.textTransform = this.capitalization.options[this.capitalization.selectedIndex].value;\
	\
	el.style.textAlign = this.halign.options[this.halign.selectedIndex].value;\
	el.style.verticalAlign = this.valign.options[this.valign.selectedIndex].value;\
	if (this.textIndent.value != '') try { el.style.textIndent = this.textIndent.value + this.textIndentUnit.options[this.textIndentUnit.selectedIndex].value; } catch(e) {} else el.style.textIdent = '';\
	el.style.direction = this.direction.options[this.direction.selectedIndex].value;\
	\
	try { el.style.backgroundColor = this.backcolor.value; } catch (e) {}\
	\
	if (this.letterSpacing.value != '') try { el.style.letterSpacing = this.letterSpacing.value + this.letterSpacingUnit.options[this.letterSpacingUnit.selectedIndex].value; } catch(e) {} else el.style.letterSpacing = '';\
	if (this.lineHeight.value != '') try { el.style.lineHeight = this.lineHeight.value + this.lineHeightUnit.options[this.lineHeightUnit.selectedIndex].value; } catch(e) {} else el.style.lineHeight = '';\
	\
	if (this.marginLeft.value != '') try { el.style.marginLeft = this.marginLeft.value + this.marginLeftUnit.options[this.marginLeftUnit.selectedIndex].value; } catch(e) {} else el.style.marginLeft = '';\
	if (this.marginRight.value != '') try { el.style.marginRight = this.marginRight.value + this.marginRightUnit.options[this.marginRightUnit.selectedIndex].value; } catch(e) {} else el.style.marginRight = '';\
	if (this.marginTop.value != '') try { el.style.marginTop = this.marginTop.value + this.marginTopUnit.options[this.marginTopUnit.selectedIndex].value; } catch(e) {} else el.style.marginTop = '';\
	if (this.marginBottom.value != '') try { el.style.marginBottom = this.marginBottom.value + this.marginBottomUnit.options[this.marginBottomUnit.selectedIndex].value; } catch(e) {} else el.style.marginBottom = '';\
	\
	if (this.paddingLeft.value != '') try { el.style.paddingLeft = this.paddingLeft.value + this.paddingLeftUnit.options[this.paddingLeftUnit.selectedIndex].value; } catch(e) {} else el.style.paddingLeft = '';\
	if (this.paddingRight.value != '') try { el.style.paddingRight = this.paddingRight.value + this.paddingRightUnit.options[this.paddingRightUnit.selectedIndex].value; } catch(e) {} else el.style.paddingRight = '';\
	if (this.paddingTop.value != '') try { el.style.paddingTop = this.paddingTop.value + this.paddingTopUnit.options[this.paddingTopUnit.selectedIndex].value; } catch(e) {} else el.style.paddingTop = '';\
	if (this.paddingBottom.value != '') try { el.style.paddingBottom = this.paddingBottom.value + this.paddingBottomUnit.options[this.paddingBottomUnit.selectedIndex].value; } catch(e) {} else el.style.paddingBottom = '';\
	\
	if (this.allBorders.checked) {\
		if (this.borderFullWidth.value != '' && this.borderFullColor.value != '') {\
			try {el.style.border = this.borderFullWidth.value + this.borderFullWidthUnit.options[this.borderFullWidthUnit.selectedIndex].value + ' ' + this.borderFullStyle.options[this.borderFullStyle.selectedIndex].value + ' ' + this.borderFullColor.value; } catch(e) {}\
		} else {\
			if (el.style.border != '') el.style.border = '';\
		}\
	} else {\
		if (this.borderLeftWidth.value != '' && this.borderLeftColor.value != '') {\
			try {el.style.borderLeft = this.borderLeftWidth.value + this.borderLeftWidthUnit.options[this.borderLeftWidthUnit.selectedIndex].value + ' ' + this.borderLeftStyle.options[this.borderLeftStyle.selectedIndex].value + ' ' + this.borderLeftColor.value; } catch(e) {}\
		} else {\
			if (el.style.borderLeft != '') el.style.borderLeft = '';\
		}\
		if (this.borderRightWidth.value != '' && this.borderRightColor.value != '') {\
			try {el.style.borderRight = this.borderRightWidth.value + this.borderRightWidthUnit.options[this.borderRightWidthUnit.selectedIndex].value + ' ' + this.borderRightStyle.options[this.borderRightStyle.selectedIndex].value + ' ' + this.borderRightColor.value; } catch(e) {}\
		} else {\
			if (el.style.borderRight != '') el.style.borderRight = '';\
		}\
		if (this.borderTopWidth.value != '' && this.borderTopColor.value != '') {\
			try {el.style.borderTop = this.borderTopWidth.value + this.borderTopWidthUnit.options[this.borderTopWidthUnit.selectedIndex].value + ' ' + this.borderTopStyle.options[this.borderTopStyle.selectedIndex].value + ' ' + this.borderTopColor.value; } catch(e) {}\
		} else {\
			if (el.style.borderTop != '') el.style.borderTop = '';\
		}\
		if (this.borderBottomWidth.value != '' && this.borderBottomColor.value != '') {\
			try {el.style.borderBottom = this.borderBottomWidth.value + this.borderBottomWidthUnit.options[this.borderBottomWidthUnit.selectedIndex].value + ' ' + this.borderBottomStyle.options[this.borderBottomStyle.selectedIndex].value + ' ' + this.borderBottomColor.value; } catch(e) {}\
		} else {\
			if (el.style.borderBottom != '') el.style.borderBottom = '';\
		}\
	}\
};\
FTB_StyleEditor.prototype.ReadElementStyle = function(el) {\
	\
	this.fontFamily.value = el.style.fontFamily;\
	this.color.value = el.style.color;\
	this.SetUnitStyle(el.style.fontSize, this.size, this.sizeUnit);\
	\
	this.bold.checked = (el.style.fontWeight.indexOf('bold') > -1) ? true : false;\
	this.italic.checked = (el.style.fontStyle.indexOf('italic') > -1) ? true : false;\
	this.smallcaps.checked = (el.style.fontVariant.indexOf('small-caps') > -1) ? true : false;\
	this.strikethrough.checked = (el.style.textDecoration.indexOf('line-through') > -1) ? true : false;\
	this.underline.checked = (el.style.textDecoration.indexOf('underline') > -1) ? true : false;\
	this.overline.checked = (el.style.textDecoration.indexOf('overline') > -1) ? true : false;\
	window.opener.FTB_SetListValue(this.capitalization,el.style.textTransform);\
	\
	window.opener.FTB_SetListValue(this.halign,el.style.textAlign);\
	window.opener.FTB_SetListValue(this.valign,el.style.verticalAlign);\
	this.SetUnitStyle(el.style.textIndent, this.textIndent, this.textIndentUnit);\
	window.opener.FTB_SetListValue(this.direction,el.style.direction);\
	\
	this.backcolor.value = el.style.backgroundColor;\
	\
	this.SetUnitStyle(el.style.letterSpacing, this.letterSpacing, this.letterSpacingUnit);\
	this.SetUnitStyle(el.style.lineHeight, this.lineHeight, this.lineHeightUnit);\
	\
	this.SetUnitStyle(el.style.paddingLeft, this.paddingLeft, this.paddingLeftUnit);\
	this.SetUnitStyle(el.style.paddingRight, this.paddingRight, this.paddingRightUnit);\
	this.SetUnitStyle(el.style.paddingTop, this.paddingTop, this.paddingTopUnit);\
	this.SetUnitStyle(el.style.paddingBottom, this.paddingBottom, this.paddingBottomUnit);\
	\
	this.SetUnitStyle(el.style.marginLeft, this.marginLeft, this.marginLeftUnit);\
	this.SetUnitStyle(el.style.marginRight, this.marginRight, this.marginRightUnit);\
	this.SetUnitStyle(el.style.marginTop, this.marginTop, this.marginTopUnit);\
	this.SetUnitStyle(el.style.marginBottom, this.marginBottom, this.marginBottomUnit);\
	\
	if (el.style.border != '') {\
		this.allBorders.checked = true;\
		this.individualBorders.checked = false;\
		this.SetBorderStyle(el.style.border, this.borderFullWidth, this.borderFullWidthUnit, this.borderFullStyle, this.borderFullColor);\
	} else {\
		this.allBorders.checked = false;\
		this.individualBorders.checked = true;\
		this.SetBorderStyle(el.style.borderLeft, this.borderLeftWidth, this.borderLeftWidthUnit, this.borderLeftStyle, this.borderLeftColor);\
		this.SetBorderStyle(el.style.borderRight, this.borderRightWidth, this.borderRightWidthUnit, this.borderRightStyle, this.borderRightColor);\
		this.SetBorderStyle(el.style.borderTop, this.borderTopWidth, this.borderTopWidthUnit, this.borderTopStyle, this.borderTopColor);\
		this.SetBorderStyle(el.style.borderBottom, this.borderBottomWidth, this.borderBottomWidthUnit, this.borderBottomStyle, this.borderBottomColor);\
	}\
\
};\
FTB_StyleEditor.prototype.SetUnitStyle = function(elStyleProperty,valueControl,unitControl) {\
	if (elStyleProperty != '') {\
		property = window.opener.FTB_ParseUnit(elStyleProperty);\
		valueControl.value = property.value;\
		window.opener.FTB_SetListValue(unitControl,property.unitType);\
	}\
};\
FTB_StyleEditor.prototype.SetBorderStyle = function(elBorderProperty,widthControl,widthUnitControl,styleControl,colorControl) {\
	if (elBorderProperty != '') {\
		var props = elBorderProperty.split(' ');\
		if (props.length == 3) {\
			this.SetUnitStyle(props[1], widthControl, widthUnitControl);\
			window.opener.FTB_SetListValue(styleControl,props[2]);\
			colorControl.value = props[0];\
		}\
	}\
};\
FTB_StyleEditor.prototype.SetBorder = function(elStyleProperty,valueControl,unitControl) {\
	if (elStyleProperty != '') {\
		property = window.opener.FTB_ParseUnit(elStyleProperty);\
		valueControl.value = property.value;\
		window.opener.FTB_SetListValue(unitControl,property.unitType);\
	}\
};\
FTB_StyleEditor.prototype.ReadCurrentElement = function() {\
	el = window.launchParameters['element'];\
	this.previewDiv.innerHTML = 'Sample text within the current &lt;' + el.tagName + '&gt; element';\
	this.ReadElementStyle(el);\
};\
FTB_StyleEditor.prototype.ShowLayer = function(tab,layer) {\
	\
	this.fontLayer.style.display = 'none';\
	this.textLayer.style.display = 'none';\
	this.backgroundLayer.style.display = 'none';\
	this.spacingLayer.style.display = 'none';\
	this.bordersLayer.style.display = 'none';\
	\
	this.fontTab.className = 'unselectedTab';\
	this.textTab.className = 'unselectedTab';\
	this.backgroundTab.className = 'unselectedTab';\
	this.spacingTab.className = 'unselectedTab';\
	this.bordersTab.className = 'unselectedTab';\
	tab.className = 'selectedTab';\
	layer.style.display = 'block';\
};\
</script>\
</head>\
<body >\
<h1>Style Editor</h1>\
<table width='500' cellpadding='0' cellspacing='0'><tr><td valign='top' width='80' style='border: solid 1px #aaa; border-right: solid 1px #fff; border-bottom: solid 1px #fff;'>\
<ul class='tabMenu'>\
	<li><a href='javascript:void(0);' id='s_tab_font' class='selectedTab'>Font</a></li>\
	<li><a href='javascript:void(0);' id='s_tab_text' class='unselectedTab'>Text</a></li>\
	<li><a href='javascript:void(0);' id='s_tab_background' class='unselectedTab'>Background</a></li>\
	<li><a href='javascript:void(0);' id='s_tab_spacing' class='unselectedTab'>Spacing</a></li>\
	<li><a href='javascript:void(0);' id='s_tab_borders' class='unselectedTab'>Borders</a></li>\
</ul>\
</td><td style='padding: 2px;'>\
<div id='s_container'>\
<div id='s_layer_font'>\
<h2>Font</h2>\
	<fieldset>\
	<legend>Properties</legend>\
		<table cellpadding='1' cellspacing='0' border='0'>\
			<tr><td class='f_title'>Family</td>\
				<td><input type='text' id='s_fontFamily' style='width: 100px;' /></td>\
			</tr>\
			<tr><td class='f_title'>Color</td>\
				<td><input type='text' id='s_color' style='width: 100px;' /></td>\
			</tr>\
			<tr><td class='f_title'>Size</td>\
				<td>\
					<input type='text' id='s_fontSize' style='width: 100px;' />\
					<select id='s_fontSizeUnit' >\
						<option value='px'>px</option>\
						<option value='pt'>pt</option>\
						<option value='%'>%</option>\
						<option value='em'>em</option>\
						<option value='in'>in</option>\
						<option value='mm'>mm</option>\
						<option value='cm'>cm</option>\
					</select>\
				</td>\
			</tr>\
			<tr><td class='f_title'>Capitalization</td>\
				<td>\
					<select id='s_capitalization' >\
						<option value=''>Not Set</option>\
						<option value='uppercase'>UPPERCASE</option>\
						<option value='lowercase'>lowercase</option>\
						<option value='capitalize'>First Letter</option>\
					</select>\
				</td>\
			</tr>\
		</table>\
	</fieldset>\
	<fieldset>\
		<legend>Effects</legend>\
			<table><tr><td>\
		<input type='checkbox' id='s_bold'  /><label for='s_bold'>Bold</label><br />\
		<input type='checkbox' id='s_italic' /><label for='s_italic'>Italic</label><br />\
		<input type='checkbox' id='s_smallcaps' /><label for='s_smallcaps'>Small Caps</label><br />\
		</td><td>\
		<input type='checkbox' id='s_strikethrough' /><label for='s_strikethrough'>Strikethrough</label><br />\
		<input type='checkbox' id='s_underline' /><label for='s_underline'>Underline</label><br />\
		<input type='checkbox' id='s_overline' /><label for='s_overline'>Overline</label><br />\
		</td></tr>\
		</table>\
	</fieldset>\
</div>\
<div id='s_layer_text' style='display:none;'>\
<h2>Text</h2>\
<fieldset>\
	<legend>Paragraph</legend>\
	<table cellpadding='1' cellspacing='0' border='0'	>\
		<tr><td class='f_title'>Horz. Align</td>\
			<td>\
				<select id='s_halign' style='width: 100px;' >\
					<option value=''>Not Set</option>\
					<option value='left'>Left</option>\
					<option value='right'>Right</option>\
					<option value='center'>Center</option>\
				</select>\
			</td>\
		</tr>\
		<tr><td class='f_title'>Vert. Align</td>\
			<td>\
				<select id='s_valign'  style='width: 100px;'>\
					<option value=''>Not Set</option>\
					<option value='baseline'>baseline</option>\
					<option value='super'>super</option>\
					<option value='sub'>sub</option>\
					<option value='top'>top</option>\
					<option value='text-top'>text-top</option>\
					<option value='middle'>middle</option>\
					<option value='bottom'>bottom</option>\
					<option value='text-bottom'>text-bottom</option>\
				</select>\
			</td>\
		</tr>\
	</table>\
</fieldset>\
<fieldset>\
	<legend>Spacing Between</legend>\
	<table cellpadding='1' cellspacing='0' border='0'>\
		<tr><td class='f_title'>Letters</td>\
			<td>\
				<input type='text' id='s_letterSpacing' style='width: 50px;' />\
				<select id='s_letterSpacingUnit'>\
					<option value='pt'>pt</option>\
					<option value='pc'>pc</option>\
					<option value='px'>px</option>\
					<option value='em'>em</option>\
					<option value='in'>in</option>\
					<option value='mm'>mm</option>\
					<option value='cm'>cm</option>\
				</select>\
			</td>\
		</tr>\
		<tr><td class='f_title'>Lines</td>\
			<td>\
				<input type='text' id='s_lineHeight' style='width: 50px;' />\
				<select id='s_lineHeightUnit'>\
					<option value='pt'>pt</option>\
					<option value='pc'>pc</option>\
					<option value='px'>px</option>\
					<option value='em'>em</option>\
					<option value='in'>in</option>\
					<option value='mm'>mm</option>\
					<option value='cm'>cm</option>\
				</select>\
			</td>\
		</tr>\
	</table>\
</fieldset>\
\
<fieldset>\
	<legend>Text Flow</legend>\
	<table cellpadding='1' cellspacing='0' border='0'	>\
		<tr><td class='f_title'>Text Indent</td>\
			<td>\
				<input type='text' id='s_textIndent' style='width:80px;' />\
				<select id='s_textIndentUnit'>\
					<option value='pt'>pt</option>\
					<option value='pc'>pc</option>\
					<option value='px'>px</option>\
					<option value='em'>em</option>\
					<option value='in'>in</option>\
					<option value='mm'>mm</option>\
					<option value='cm'>cm</option>\
				</select>\
			</td>\
		</tr>\
		<tr><td class='f_title'>Text Direction</td>\
			<td>\
				<select id='s_direction' style='width: 120px;'>\
					<option value=''>Not Set</option>\
					<option value='ltr'>Left to Right</option>\
					<option value='rtl'>Right to Left</option>\
				</select>\
			</td>\
		</tr>\
	</table>\
</fieldset>\
\
</div>\
<div id='s_layer_background' style='display:none;'>\
<h2>Background</h2>\
<fieldset>\
	<legend>Color</legend>\
	<table cellpadding='1' cellspacing='0' border='0'>\
		<tr><td class='f_title'>Color</td>\
			<td><input type='text' id='s_backcolor' style='width: 100px;' />\
				<input type='checkbox' id='s_backcolor_transparent' /><label for='s_backcolor_transparent'>Transparent</label>\
			</td>\
		</tr>\
	</table>\
</fieldset>\
</div>\
<div id='s_layer_spacing' style='display:none;'>\
<h2>Spacing</h2>\
<table cellpadding='1' cellspacing='0' border='0' width='100%'><tr><td>\
<fieldset>\
	<legend>Margins</legend>\
	<table cellpadding='1' cellspacing='0' border='0'>\
		<tr><td class='f_title'>Left</td>\
			<td>\
				<input type='text' id='s_marginLeft' style='width: 50px;' />\
				<select id='s_marginLeft_unit'>\
					<option value='px'>px</option>\
					<option value='pt'>pt</option>\
					<option value='%'>%</option>\
					<option value='em'>em</option>\
					<option value='in'>in</option>\
					<option value='mm'>mm</option>\
					<option value='cm'>cm</option>\
				</select>\
			</td>\
		</tr>\
		<tr><td class='f_title'>Right</td>\
			<td>\
				<input type='text' id='s_marginRight' style='width: 50px;' />\
				<select id='s_marginRight_unit'>\
					<option value='px'>px</option>\
					<option value='pt'>pt</option>\
					<option value='%'>%</option>\
					<option value='em'>em</option>\
					<option value='in'>in</option>\
					<option value='mm'>mm</option>\
					<option value='cm'>cm</option>\
				</select>\
			</td>\
		</tr>\
		<tr><td class='f_title'>Top</td>\
			<td>\
				<input type='text' id='s_marginTop' style='width: 50px;' />\
				<select id='s_marginTop_unit'>\
					<option value='px'>px</option>\
					<option value='pt'>pt</option>\
					<option value='%'>%</option>\
					<option value='em'>em</option>\
					<option value='in'>in</option>\
					<option value='mm'>mm</option>\
					<option value='cm'>cm</option>\
				</select>\
			</td>\
		</tr>\
		<tr><td class='f_title'>Bottom</td>\
			<td>\
				<input type='text' id='s_marginBottom' style='width: 50px;' />\
				<select id='s_marginBottom_unit'>\
					<option value='px'>px</option>\
					<option value='pt'>pt</option>\
					<option value='%'>%</option>\
					<option value='em'>em</option>\
					<option value='in'>in</option>\
					<option value='mm'>mm</option>\
					<option value='cm'>cm</option>\
				</select>\
			</td>\
		</tr>\
	</table>\
</fieldset>\
</td><td>\
<fieldset>\
	<legend>Padding</legend>\
	<table cellpadding='1' cellspacing='0' border='0'>\
		<tr><td class='f_title'>Left</td>\
			<td>\
				<input type='text' id='s_paddingLeft' style='width: 50px;' />\
				<select id='s_paddingLeft_unit'>\
					<option value='px'>px</option>\
					<option value='pt'>pt</option>\
					<option value='%'>%</option>\
					<option value='em'>em</option>\
					<option value='in'>in</option>\
					<option value='mm'>mm</option>\
					<option value='cm'>cm</option>\
				</select>\
			</td>\
		</tr>\
		<tr><td class='f_title'>Right</td>\
			<td>\
				<input type='text' id='s_paddingRight' style='width: 50px;' />\
				<select id='s_paddingRight_unit'>\
					<option value='px'>px</option>\
					<option value='pt'>pt</option>\
					<option value='%'>%</option>\
					<option value='em'>em</option>\
					<option value='in'>in</option>\
					<option value='mm'>mm</option>\
					<option value='cm'>cm</option>\
				</select>\
			</td>\
		</tr>\
		<tr><td class='f_title'>Top</td>\
			<td>\
				<input type='text' id='s_paddingTop' style='width: 50px;' />\
				<select id='s_paddingTop_unit'>\
					<option value='px'>px</option>\
					<option value='pt'>pt</option>\
					<option value='%'>%</option>\
					<option value='em'>em</option>\
					<option value='in'>in</option>\
					<option value='mm'>mm</option>\
					<option value='cm'>cm</option>\
				</select>\
			</td>\
		</tr>\
		<tr><td class='f_title'>Bottom</td>\
			<td>\
				<input type='text' id='s_paddingBottom' style='width: 50px;' />\
				<select id='s_paddingBottom_unit'>\
					<option value='px'>px</option>\
					<option value='pt'>pt</option>\
					<option value='%'>%</option>\
					<option value='em'>em</option>\
					<option value='in'>in</option>\
					<option value='mm'>mm</option>\
					<option value='cm'>cm</option>\
				</select>\
			</td>\
		</tr>\
	</table>\
</fieldset>\
</td></tr></table>\
</div>\
<div id='s_layer_borders' style='display:none;'>\
<h2>Borders</h2>\
\
<fieldset>\
	<legend><input type='radio' id='s_borders_type_all' name='s_borders_types' checked='checked'><label for='s_borders_type_all'>Full Border</label></legend>\
	<table cellpadding='1' cellspacing='0' border='0'>\
		<tr><td class='f_title'>Border</td>\
			<td>\
				<input type='text' id='s_borderFull_width' style='width: 20px;' />\
			</td>\
			<td>\
				<select id='s_borderFull_unit'>\
					<option value='px'>px</option>\
					<option value='pt'>pt</option>\
					<option value='em'>em</option>\
					<option value='in'>in</option>\
					<option value='mm'>mm</option>\
					<option value='cm'>cm</option>\
				</select>\
			</td>\
			<td>\
				<select id='s_borderFull_style'>\
					<option value='solid'>Solid Line</option>\
					<option value='double'>Double Line</option>\
					<option value='dashed'>Dashed</option>\
					<option value='dotted'>Dotted</option>\
					<option value='groove'>Groove</option>\
					<option value='ridge'>Ridge</option>\
					<option value='inset'>Inset</option>\
					<option value='outset'>Outset</option>\
				</select>\
			</td>\
			<td>\
				<input type='text' id='s_borderFull_color' style='width: 70px;' />\
			</td>\
		</tr>\
	</table>\
</fieldset>\
\
<fieldset>\
	<legend><input type='radio' id='s_borders_type_individual' name='s_borders_types'><label for='s_borders_type_individual'>Individual Borders</label></legend>\
	<table cellpadding='1' cellspacing='0' border='0'>\
		<tr><td class='f_title'>Left</td>\
			<td>\
				<input type='text' id='s_borderLeft_width' style='width: 20px;' />\
			</td>\
			<td>\
				<select id='s_borderLeft_unit'>\
					<option value='px'>px</option>\
					<option value='pt'>pt</option>\
					<option value='em'>em</option>\
					<option value='in'>in</option>\
					<option value='mm'>mm</option>\
					<option value='cm'>cm</option>\
				</select>\
			</td>\
			<td>\
				<select id='s_borderLeft_style'>\
					<option value='solid'>Solid Line</option>\
					<option value='double'>Double Line</option>\
					<option value='dashed'>Dashed</option>\
					<option value='dotted'>Dotted</option>\
					<option value='groove'>Groove</option>\
					<option value='ridge'>Ridge</option>\
					<option value='inset'>Inset</option>\
					<option value='outset'>Outset</option>\
				</select>\
			</td>\
			<td>\
				<input type='text' id='s_borderLeft_color' style='width: 70px;' />\
			</td>\
		</tr>\
		<tr><td class='f_title'>Right</td>\
			<td>\
				<input type='text' id='s_borderRight_width' style='width: 20px;' />\
			</td>\
			<td>\
				<select id='s_borderRight_unit'>\
					<option value='px'>px</option>\
					<option value='pt'>pt</option>\
					<option value='em'>em</option>\
					<option value='in'>in</option>\
					<option value='mm'>mm</option>\
					<option value='cm'>cm</option>\
				</select>\
			</td>\
			<td>\
				<select id='s_borderRight_style'>\
					<option value='solid'>Solid Line</option>\
					<option value='double'>Double Line</option>\
					<option value='dashed'>Dashed</option>\
					<option value='dotted'>Dotted</option>\
					<option value='groove'>Groove</option>\
					<option value='ridge'>Ridge</option>\
					<option value='inset'>Inset</option>\
					<option value='outset'>Outset</option>\
				</select>\
			</td>\
			<td>\
				<input type='text' id='s_borderRight_color' style='width: 70px;' />\
			</td>\
		</tr>\
		<tr><td class='f_title'>Top</td>\
			<td>\
				<input type='text' id='s_borderTop_width' style='width: 20px;' />\
			</td>\
			<td>\
				<select id='s_borderTop_unit'>\
					<option value='px'>px</option>\
					<option value='pt'>pt</option>\
					<option value='em'>em</option>\
					<option value='in'>in</option>\
					<option value='mm'>mm</option>\
					<option value='cm'>cm</option>\
				</select>\
			</td>\
			<td>\
				<select id='s_borderTop_style'>\
					<option value='solid'>Solid Line</option>\
					<option value='double'>Double Line</option>\
					<option value='dashed'>Dashed</option>\
					<option value='dotted'>Dotted</option>\
					<option value='groove'>Groove</option>\
					<option value='ridge'>Ridge</option>\
					<option value='inset'>Inset</option>\
					<option value='outset'>Outset</option>\
				</select>\
			</td>\
			<td>\
				<input type='text' id='s_borderTop_color' style='width: 70px;' />\
			</td>\
		</tr>\
		<tr><td class='f_title'>Bottom</td>\
			<td>\
				<input type='text' id='s_borderBottom_width' style='width: 20px;' />\
			</td>\
			<td>\
				<select id='s_borderBottom_unit'>\
					<option value='px'>px</option>\
					<option value='pt'>pt</option>\
					<option value='em'>em</option>\
					<option value='in'>in</option>\
					<option value='mm'>mm</option>\
					<option value='cm'>cm</option>\
				</select>\
			</td>\
			<td>\
				<select id='s_borderBottom_style'>\
					<option value='solid'>Solid Line</option>\
					<option value='double'>Double Line</option>\
					<option value='dashed'>Dashed</option>\
					<option value='dotted'>Dotted</option>\
					<option value='groove'>Groove</option>\
					<option value='ridge'>Ridge</option>\
					<option value='inset'>Inset</option>\
					<option value='outset'>Outset</option>\
				</select>\
			</td>\
			<td>\
				<input type='text' id='s_borderBottom_color' style='width: 70px;' />\
			</td>\
		</tr>\
	</table>\
</fieldset>\
</div>\
</div>\
<div id='sampleHolder'>\
	<fieldset>\
		<legend>Sample</legend>\
		<table style='width:100%;height:100px;border: solid 1px #ddd; border-right: solid 1px #fff; border-bottom: solid 1px #fff;' id='s_sampleContainer'><tr><td align='center'>\
			<div id='s_sample' >\
\
			</div>\
		</td></tr></table>\
	</fieldset>\
</div>\
<div style='text-align:center;'>\
	<input type='button' id='s_updateButton' value='Update Style'  />\
	<input type='button' value='Cancel' onclick='window.close();' />\
</div>\
</td></tr>\
</table>\
<script type='text/javascript'>\
function load() {\
	StyleEditor = new FTB_StyleEditor('style');\
	StyleEditor.Initialize();\
	StyleEditor.ReadCurrentElement();\
	StyleEditor.UpdateStylePreview();\
}\
</script>\
</body>\
</html>";