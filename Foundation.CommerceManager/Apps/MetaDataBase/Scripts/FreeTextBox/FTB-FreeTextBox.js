
/* main FTB object
-------------------------------------- */
function FTB_FreeTextBox(id, enableToolbars, readOnly, buttons, dropdownlists, breakMode, pasteMode, tabMode, startMode, clientSideTextChanged, designModeCss, designModeBodyTagCssClass, baseUrl, textDirection, buttonImageFormat, imageGalleryUrl, imageGalleryPath, receiveFocus, buttonWidth, buttonHeight) {
	this.debug = document.getElementById('debug');
	
	this.id = id;
	this.enableToolbars = enableToolbars;
	this.readOnly = readOnly;
	this.buttons = buttons;
	this.dropdownlists = dropdownlists;
	this.breakMode = breakMode;
	this.pasteMode = pasteMode;
	this.tabMode = tabMode;
	this.startMode = startMode;
	this.clientSideTextChanged = clientSideTextChanged;

	this.designModeCss = designModeCss;
	this.designModeBodyTagCssClass = designModeBodyTagCssClass;
	this.baseUrl = baseUrl;
	this.textDirection = textDirection;
	this.buttonImageFormat = buttonImageFormat; // currently unused
	this.imageGalleryUrl = imageGalleryUrl;
	this.imageGalleryPath = imageGalleryPath;	
	this.hasFocus = false;
	this.mode = FTB_MODE_DESIGN;
	this.initialized = false;
	this.undoArray = new Array();
	this.undoArrayMax = 16;
	this.undoArrayPos = -1;
	this.lastEvent = null;
//};
//FTB_FreeTextBox.prototype.Initialize = function() {
	var ftb = this;
	

	// 2. Find everything
	//* windows
	this.htmlEditor = document.getElementById(this.id);

	if (FTB_Browser.isIE) {
		this.previewPane = eval(this.id + "_previewPane");
		this.designEditor = eval(this.id + "_designEditor");
		this.designEditor.ftb = this;
		this.designEditor.document.ftb = this;
		document.getElementById(this.id + "_designEditor").document.ftb = this;
	} else {
		this.previewPane = document.getElementById(this.id + "_previewPane").contentWindow;
		this.designEditor = document.getElementById(this.id + "_designEditor").contentWindow;
		this.designEditor.document.ftb = this;		
	}

	//* areas
	this.toolbarArea = document.getElementById(this.id + "_toolbarArea");
	this.designEditorArea = document.getElementById(this.id + "_designEditorArea");
	this.htmlEditorArea = document.getElementById(this.id + "_htmlEditorArea");
	this.previewPaneArea = document.getElementById(this.id + "_previewPaneArea");

	//* tabs
	this.designModeTab = document.getElementById(this.id + "_designModeTab");
	if (this.designModeTab) {
		this.designModeTab.ftb = this;
		this.designModeTab.onclick = function() { if (!this.ftb.readOnly) {this.ftb.GoToDesignMode(); this.ftb.Focus(); this.ftb.UpdateToolbars(); }}
	}

	this.htmlModeTab = document.getElementById(this.id + "_htmlModeTab");
	if (this.htmlModeTab) {
		
		this.htmlModeTab.ftb = this;
		this.htmlModeTab.onclick = function() { if (!this.ftb.readOnly) {this.ftb.GoToHtmlMode(); this.ftb.Focus();  this.ftb.UpdateToolbars(); }}
	}

	this.previewModeTab = document.getElementById(this.id + "_previewModeTab");
	if (this.previewModeTab && !this.ftb.readOnly) {
		
			this.previewModeTab.ftb = this;		
		this.previewModeTab.onclick = function() { if (!this.ftb.readOnly) {this.ftb.GoToPreviewMode();}}
	}
	
	//* ancestor area
	this.ancestorArea = document.getElementById(this.id + "_AncestorArea");

	// 3. Tell buttons who owns them
	//* setup buttons & dropdowns
	if (this.enableToolbars) {
		for(var i=0; i<this.buttons.length; i++) {
			button = this.buttons[i];
			button.ftb = this;
			if (!this.readOnly)
				button.Initialize();
		}
		for(var i=0; i<this.dropdownlists.length; i++) {
			dropdownlist = this.dropdownlists[i];
			dropdownlist.ftb = this;
		}
	}
       
	
	// 4. Setup editor for use
	if (!this.readOnly) {
		this.designEditor.document.designMode = 'On';

		if (FTB_Browser.isGecko) this.designEditor.document.contentEditable = true;	
   	}
	
	//((this.designModeCss != '') ? "<style type='text/css'>@import url(" + this.designModeCss + ");</style>" : "") + 
	this.designEditor.document.open();
	this.designEditor.document.write("<html" + ((this.textDirection == "rtl") ? " dir='rtl'" : "") + ">" + 
			"<head>" + 
			((this.designModeCss != '') ? "<link rel='stylesheet' href='" + this.designModeCss + "' type='text/css' />" : "") + 
			((this.baseUrl != '') ? "<base href='" + this.baseUrl + "' />" : "") + 
			"</head>" + 
			"<body" + ((this.designModeBodyTagCssClass != '') ? " class='" + this.designModeBodyTagCssClass + "'" : "") + ">" + 
				this.StoreUrls(this.htmlEditor.value) + 
			"</body>" + 
		"</html>");
	this.designEditor.document.close();
	
	if (FTB_Browser.isIE) 	 {
		this.designEditor.document.execCommand("2D-Position", true, true);
		this.designEditor.document.execCommand("MultipleSelection", true, true);	
	}
	
	if (!this.readOnly) {	
		if (FTB_Browser.isIE) this.designEditor.document.body.contentEditable = true;		
		// enable this html area
		this.htmlEditor.disabled = '';
	}	
	
	// IE can't get the style right until now...
	if (FTB_Browser.isIE) {
		this.designEditor.document.body.style.border = '0';		
	}
	
	
	// 5. Add events
    if (!this.readOnly) {
		if (FTB_Browser.isIE) {
			FTB_AddEvents(this.designEditor.document,
				new Array("keydown","keypress","mousedown"),
				function(e) { ftb.hasFocus=true; return ftb.Event(e); } 
			);

			FTB_AddEvent(this.designEditor.document.body,
				"blur",
				function(e) { ftb.Event(e); ftb.hasFocus=false; ftb.StoreHtml(); } 
			);			
		} else {
			var evt = function(e) {	
				//alert(this.document.ftb);
				if (this.document.ftb != null) {					
					this.document.ftb.hasFocus=true; 
					this.document.ftb.Event(e);
				}
				return false;
			}
			this.designEditor.addEventListener("keydown", evt, true);			
			this.designEditor.addEventListener("keypress", evt, true);			
			this.designEditor.addEventListener("mousedown", evt, true);

      		// no paste event in Mozilla
		}
		FTB_AddEvents(this.designEditor,
			new Array("blur"),
			function(e) { ftb.hasFocus=false; ftb.Event(e); ftb.StoreHtml(); } 
		);
	}
     	
	if (this.startMode == FTB_MODE_HTML)
		this.GoToHtmlMode();
		
	if (this.readOnly) 
		this.DisableAllToolbarItems();
	else
		this.UpdateToolbars();
	
	this.undoArray[0] = this.htmlEditorArea.value;
	this.initialized = true;
	
	if (FTB_Browser.isGecko && !this.readOnly && this.startMode != FTB_MODE_HTML) {
		this.designEditor.document.designMode = 'On';
		this.designEditor.document.execCommand("useCSS", false, true);
	}	
	
	if (this.receiveFocus) this.Focus();	
};


FTB_FreeTextBox.prototype.StoreUrls = function(input) {
	// store urls in temporary attribute (if not already done)
	if (!input.match(/(temp_src|temp_href)/gi, input)) {
		input = input.replace(new RegExp('src\\s*=\\s*\"([^ >\"]*)\"', 'gi'), 'src="$1" temp_src="$1"');
		input = input.replace(new RegExp('href\\s*=\\s*\"([^ >\"]*)\"', 'gi'), 'href="$1" temp_href="$1"');
	}
	return input;
}

FTB_FreeTextBox.prototype.RemoveTempUrls = function(input) {	
	input = input.replace(new RegExp('\\s*temp_src\\s*=\\s*\"([^ >\"]*)\"', 'gi'), '');
	input = input.replace(new RegExp('\\s*temp_href\\s*=\\s*\"([^ >\"]*)\"', 'gi'), '');
	return input;
}

FTB_FreeTextBox.prototype.DesignTabClick = function() {
	this.GoToDesignMode(); 
	this.Focus(); 
	this.UpdateToolbars(); 	
}

FTB_FreeTextBox.prototype.HtmlTabClick = function() {
	this.GoToHtmlMode(); 
	this.Focus();
	this.UpdateToolbars();
}

FTB_FreeTextBox.prototype.RefreshDesignMode = function() {
	if (!this.readOnly && FTB_Browser.isGecko) {
		this.designEditor.document.designMode = 'on'; 
		this.designEditor.document.execCommand('useCSS', false, true); 
	}
}

FTB_FreeTextBox.prototype.AddStyle = function(css) {
	var styleEl=document.createElement('style');
	styleEl.type='text/css';
	styleEl.appendChild(css);
	this.designEditor.document.appendChild(styleEl);
};

FTB_FreeTextBox.prototype.Event = function(ev) {
 	this.hasFocus = true;
	
	if (ev != null) { 	

 		if (FTB_Browser.isIE) {
 			sel = this.GetSelection();
 			r = this.CreateRange(sel);	 		
	 		
 			// check for undo && redo
 			if (ev.ctrlKey && ev.keyCode == FTB_KEY_Z) {
 				this.Undo();
				this.CancelEvent(ev);			
 			} else if (ev.ctrlKey && ev.keyCode == FTB_KEY_Y) { 			
 				this.Redo(); 
				this.CancelEvent(ev);
 			} else {
		 		
 				if (ev.keyCode == FTB_KEY_ENTER) {
 					if (this.breakMode == FTB_BREAK_BR || ev.ctrlKey) {
						if (sel.type == 'Control') {
							return;
						}
						if ((!this.CheckTag(r.parentElement(),'LI'))&&(!this.CheckTag(r.parentElement(),'H'))) {
							r.pasteHTML('<br>');
							this.CancelEvent(ev);
							r.select();
							r.collapse(false);
							return false;
						} 			
 					} 
				} else if ((ev.ctrlKey && !ev.shiftKey && !ev.altKey)) {					
					if (ev.keyCode == FTB_KEY_V || ev.keyCode == 118) {										
						this.CapturePaste();
						this.CancelEvent(ev);
					}
 				} else if (ev.keyCode == FTB_KEY_TAB) {	
	 				if (this.CheckTag(r.parentElement(),'LI')) {
	 					if (ev.shiftKey)
	 						this.ExecuteCommand("outdent");
	 					else
	 						this.ExecuteCommand("indent");
	 					this.CancelEvent(ev);
	 				} else {	 				
	 					switch (this.tabMode) {
	 						default:
	 						case FTB_TAB_NEXTCONTROL:
	 							break;
	 						case FTB_TAB_INSERTSPACES:
	 							this.InsertHtml("&nbsp;&nbsp;&nbsp;");
	 							this.CancelEvent(ev);
	 							break;
	 						case FTB_TAB_DISABLED:
	 							this.CancelEvent(ev);
	 							break;	 						
	 					}
	 				}
 				}
 			}
	 	
 		} else { 	 	
	 		if (ev.type == "keypress" || ev.type == "keydown") {
	 			
	 			
				// check for undo && redo
				if (ev.ctrlKey && ev.which && ev.which == FTB_KEY_Z) {	 			
					this.Undo();
					this.CancelEvent(ev);		
				} else if (ev.ctrlKey && ev.which && ev.which == FTB_KEY_Y) {	 			
					this.Redo(); 
					this.CancelEvent(ev);		
				} else {

					if (ev.keyCode == FTB_KEY_ENTER) {
						if (this.breakMode == FTB_BREAK_P) {
							/*
							var insertP = true;
							var parent = this.GetParentElement();

							if ( parent != null ) 
								if (this.CheckTag(this.GetParentElement(),'LI') )
									insertP = false;

							if (!insertP) return;

							if ( parent != null ) {
								if ( !this.CheckTag(this.GetParentElement(),'P') )
									this.ExecuteCommand('formatblock','','p');
							} else {
								this.ExecuteCommand('formatblock','','p');
							}
							var parent = this.GetParentElement();
							p = this.designEditor.document.createElement('p');

							sel = this.GetSelection();
							r = this.CreateRange(sel);
							r.insertNode(p);
							r.selectNode(p);
							//p.focus();

							//
							//parent.insertBefore(p);

							this.CancelEvent(ev);	
							*/
						}

					// check for control+commands (not in Mozilla by default)
					} else if ((ev.ctrlKey && !ev.shiftKey && !ev.altKey)) {
						
						if (ev.which == FTB_KEY_V || ev.which == 118) {										
							if (ev.which == 118 && this.pasteMode != FTB_PASTE_DEFAULT) {
								this.CapturePaste();
								this.CancelEvent(ev);
							}
						} else if (ev.which == FTB_KEY_B || ev.which == 98) {
							if (ev.which == FTB_KEY_B) this.ExecuteCommand('bold');
							this.CancelEvent(ev);
						} else if (ev.which == FTB_KEY_I || ev.which == 105) {
							if (ev.which == FTB_KEY_I) this.ExecuteCommand('italic');
							this.CancelEvent(ev);
						} else if (ev.which == FTB_KEY_U || ev.which == 117) {
							if (ev.which == FTB_KEY_U) this.ExecuteCommand('underline');				 						
							this.CancelEvent(ev);
						}
					} else if (ev.which == FTB_KEY_TAB) {
						if (this.CheckTag(r.parentElement,'LI')) {
							// do it's own thing!
						} else {	 				
							switch (this.tabMode) {
								default:
								case FTB_TAB_NEXTCONTROL:
									// unsupported in Mozilla
									break;
								case FTB_TAB_INSERTSPACES:
									// do it's own thing
									break;
								case FTB_TAB_DISABLED:
									this.CancelEvent(ev);
									break;	 						
							}
						}
					}
				}
  			} 	
 		}
 	}
 
	if (this.mode == FTB_MODE_DESIGN) {
		FTB_Timeout.addMethod(this.id+'_UpdateToolbars',this,'UpdateToolbars',200);
	}
	
	if (this.clientSideTextChanged)
		this.clientSideTextChanged(this);
};
FTB_FreeTextBox.prototype.CancelEvent = function(ev) {
	if (FTB_Browser.isIE) {
		ev.cancelBubble = true;
		ev.returnValue = false;
	} else {
		ev.preventDefault();
		ev.stopPropagation();
	}
};
FTB_FreeTextBox.prototype.InsertElement = function(el) {
	var sel = this.GetSelection();
	var range = this.CreateRange(sel);
	
	if (FTB_Browser.isIE) {
		range.pasteHTML(el.outerHTML);
	} else {
		this.InsertNodeAtSelection(el);
	}
};
FTB_FreeTextBox.prototype.RecordUndoStep = function() {	
	if (!this.initialized) return;
	++this.undoArrayPos;
	if (this.undoArrayPos >= this.undoArrayMax) {
		// remove the first element
		this.undoArray.shift();
		--this.undoArrayPos;
	}
	
	var take = true;
	var html = this.designEditor.document.body.innerHTML;
	if (this.undoArrayPos > 0)
		take = (this.undoArray[this.undoArrayPos - 1] != html);
	if (take) {
		this.undoArray[this.undoArrayPos] = html;
	} else {
		this.undoArrayPos--;
	}
};
FTB_FreeTextBox.prototype.Undo = function() {
	if (this.undoArrayPos > 0) {
		var html = this.undoArray[--this.undoArrayPos];
		if (html)
			this.designEditor.document.body.innerHTML = html;
		else 
			++this.undoArrayPos;
	}
};
FTB_FreeTextBox.prototype.CanUndo = function() {
	return true;
	return (this.undoArrayPos > 0);
};
FTB_FreeTextBox.prototype.Redo = function() {
	if (this.undoArrayPos < this.undoArray.length - 1) {
		var html = this.undoArray[++this.undoArrayPos];
		if (html) 
			this.designEditor.document.body.innerHTML = html;
		else 
			--this.undoArrayPos;
	}
	
};
FTB_FreeTextBox.prototype.CanRedo = function() {
	return true;
	return (this.undoArrayPos < this.undoArray.length - 1);
};
FTB_FreeTextBox.prototype.CapturePaste = function() {
 
 	switch (this.pasteMode) {
 		case FTB_PASTE_DISABLED:
 			return false;
 		case FTB_PASTE_TEXT:
 			if (window.clipboardData) {
				var text = window.clipboardData.getData('Text');
				text = text.replace(/<[^>]*>/gi,'');
				this.InsertHtml(text);
			} else {
				alert("Your browser does not support pasting rich content");
			}
			return false; 				
 		default:
 		case FTB_PASTE_DEFAULT:
			try {
				this.ExecuteCommand('paste'); 
			} catch (e) {
				alert('Your security settings to not allow you to use this command.  Please visit http://www.mozilla.org/editor/midasdemo/securityprefs.html for more information.');
			}	
 			return true;
 	}		
};
FTB_FreeTextBox.prototype.Debug = function(text) {
	if (this.debug)
		this.debug.value += text + '\r';
};
FTB_FreeTextBox.prototype.UpdateToolbars = function() {
	
	if (this.hasFocus) {

		if (this.mode == FTB_MODE_DESIGN) {
			if (this.enableToolbars) {
				for (var i=0; i<this.buttons.length; i++) {
					button = this.buttons[i];

					if (button.customStateQuery)
						button.state = button.customStateQuery();			
					else if (button.commandIdentifier != null && button.commandIdentifier != '')
						button.state = this.QueryCommandState(button.commandIdentifier);

					button.SetButtonBackground("Out");
				}
				for (var i=0; i<this.dropdownlists.length; i++) {
					dropdownlist = this.dropdownlists[i];

					if (dropdownlist.customStateQuery)
						dropdownlist.SetSelected(dropdownlist.customStateQuery());
					else if (dropdownlist.commandIdentifier != null && dropdownlist.commandIdentifier != '') 
						dropdownlist.SetSelected(this.QueryCommandValue(dropdownlist.commandIdentifier));

				}
			}
			this.UpdateAncestorTrail();
		}	
		
	} else {
	
		if (this.enableToolbars) {
			for (var i=0; i<this.buttons.length; i++) {
				button = this.buttons[i];
				button.state = FTB_BUTTON_OFF;
				button.SetButtonBackground("Out");
			}
			for (var i=0; i<this.dropdownlists.length; i++) {
				dropdownlist = this.dropdownlists[i];
				dropdownlist.list.selectedIndex = 0;
			}
		}
		this.UpdateAncestorTrail();		
	}

	if (!this.undoTimer) {
		this.RecordUndoStep();
		var editor = this;
		this.undoTimer = setTimeout(function() {
			editor.undoTimer = null;
		}, 500);
	}
	
	this.StoreHtml();
	
	this.SetToolbarItemsEnabledState();	
	
	if (this.timerToolbar) 
		this.timerToolbar = null;	
};
FTB_FreeTextBox.prototype.UpdateAncestorTrail = function() {	
	if (this.ancestorArea)  {
	
		if (this.hasFocus) {
			ancestors = this.GetAllAncestors();

			this.ancestorArea.innerHTML = "Path(" + ancestors.length + "): ";	

			for (var i = ancestors.length-1; i>-1; i--) {
				var el = ancestors[i];
				if (!el) {
					continue;
				}
				var a = document.createElement("a");
				a.href = "javascript:void();";
				a.el = el;
				a.ftb = this;
				a.onclick = function() {
					this.blur();
					this.ftb.SelectNodeContents(this.el);
					this.ftb.UpdateToolbars();
					return false;
				};
				a.oncontextmenu = function () {
					this.ftb.EditElementStyle(this.el);
					return false;
				}

				var txt = el.tagName.toLowerCase();
				if (txt == "input") txt = el.type;
				a.title = el.style.cssText;
				if (el.id) {
					txt += "#" + el.id;
				}
				if (el.className) {
					txt += "." + el.className;
				}
				a.appendChild(document.createTextNode("<" + txt + ">"));
				this.ancestorArea.appendChild(a);
				//if (i != 0)
				//	this.ancestorArea.appendChild(document.createTextNode(String.fromCharCode(0xbb)));

			}
		} else {
			this.ancestorArea.innerHTML = "";
		}
	}
};
FTB_FreeTextBox.prototype.SetToolbarItemsEnabledState = function() {	
	if (!this.enableToolbars) return;
	if (this.hasFocus || !this.initialized) {
	
		if (this.mode == FTB_MODE_DESIGN ) {		
			for (i=0; i<this.buttons.length; i++) {
				button = this.buttons[i];

				if (button.customEnabled)
					button.customEnabled();
				else 
					button.disabled = false;

				if (button.disabled)
					this.DisableButton(button);			
				else
					this.EnableButton(button);
			}

			for (i=0; i<this.dropdownlists.length; i++) {
				this.dropdownlists[i].list.disabled=false;
			}		
		} else {
			for (i=0; i<this.buttons.length; i++) {
				button = this.buttons[i];

				if (button.htmlModeEnabled) 
					button.disabled=false
				else
					button.disabled = true;

				if (button.disabled)
					this.DisableButton(button);			
				else
					this.EnableButton(button);
			}

			for (i=0; i<this.dropdownlists.length; i++) {
				this.dropdownlists[i].list.selectedIndex=0;
				this.dropdownlists[i].list.disabled=true;
			}	
		}
	} else {
		// do nothing: uncomment code to disable buttons when the editor does not have focus
		/*
		for (i=0; i<this.buttons.length; i++)
			this.DisableButton(this.buttons[i]);			

		for (i=0; i<this.dropdownlists.length; i++)
			this.dropdownlists[i].list.disabled=true;
		*/
	}
};
FTB_FreeTextBox.prototype.DisableAllToolbarItems = function() {	
	if (this.enableToolbars) {
		for (i=0; i<this.buttons.length; i++) {
			this.DisableButton(this.buttons[i]);			
		}

		for (i=0; i<this.dropdownlists.length; i++) {
			this.dropdownlists[i].list.disabled=true;
		}
	}
};
FTB_FreeTextBox.prototype.EnableButton = function(button) {
	if (FTB_Browser.isIE)
		button.buttonImage.style.filter = "alpha(opacity = 100);";
		//button.td.style.filters.alpha.opacity = 100;
	else 
		button.buttonImage.style.MozOpacity = 1;
};
FTB_FreeTextBox.prototype.DisableButton = function(button) {
	button.state = FTB_BUTTON_OFF;
	button.SetButtonStyle("Out");

	if (FTB_Browser.isIE)
		button.buttonImage.style.filter = "alpha(opacity = 25);";
		//button.td.style.filters.alpha.opacity = 25;		
	else 
		button.buttonImage.style.MozOpacity = 0.25;
};
FTB_FreeTextBox.prototype.CopyHtmlToIframe = function(iframe) {
   	if (this.initialized) {
		html = this.htmlEditor.value;
		iframe.document.body.innerHTML = this.StoreUrls(html);
		
		//this.Debug(html.replace('\r','<R>').replace('\t','<T>').replace('\n','<N>'));
   	} else {
		iframe.document.open();
		iframe.document.write("<html>" + 
			"<head>" + 
			((this.designModeCss != '' && FTB_Browser.isGecko) ? "<style type='text/css'>@import url(" + this.designModeCss + ");</style>" : "") + 
			((this.baseUrl != '') ? "<base href='" + this.baseUrl + "' />" : "") + 
			"</head>" + 
			"<body>" + 
				this.StoreUrls(this.htmlEditor.value) + 
			"</body>" + 
		"</html>");
		//iframe.document.write(this.htmlEditor.value);
		iframe.document.close();
	}
};
FTB_FreeTextBox.prototype.CopyDesignToHtml = function() {
	
	// set all stored URLs
	var links = this.designEditor.document.getElementsByTagName('a');
	var imgs = this.designEditor.document.getElementsByTagName('img');
	
	for (var i=0; i<links.length; i++) {
		var stored = links[i].getAttribute('temp_href');
		if (stored) {
			links[i].setAttribute('href',stored);
		}
	}
	for (var i=0; i<imgs.length; i++) {
		var stored = imgs[i].getAttribute('temp_src');
		if (stored) {
			imgs[i].setAttribute('src',stored);
		}
	}
	
	var html = this.designEditor.document.body.innerHTML;
	
	html = this.RemoveTempUrls(html);
	
	this.htmlEditor.value = html;
	
	// clear out default moz & ie properties
	if (this.htmlEditor.value == '<br>' || this.htmlEditor.value == '<br>\r\n' || // Moz
		this.htmlEditor.value == '<P>&nbsp;</P>') { // IE
		this.htmlEditor.value = '';
	}	
};
FTB_FreeTextBox.prototype.GoToHtmlMode = function() {
    if (this.mode == FTB_MODE_DESIGN) this.CopyDesignToHtml();
	
	if (FTB_Browser.isGecko)
		this.designEditor.document.designMode = 'Off';
		
    this.designEditorArea.style.display = 'none';
    this.htmlEditorArea.style.display = '';
    this.previewPaneArea.style.display = 'none';
   
	if (this.ancestorArea) this.ancestorArea.innerHTML = "";    
    this.SetActiveTab(this.htmlModeTab);    
         
	this.mode = FTB_MODE_HTML;
    //this.Focus();    
    return true;
};
FTB_FreeTextBox.prototype.GoToDesignMode = function() {
	if (this.mode == FTB_MODE_DESIGN) return false;

	this.CopyHtmlToIframe(this.designEditor);
	this.designEditorArea.style.display = '';
	this.htmlEditorArea.style.display = 'none';
	this.previewPaneArea.style.display = 'none';	

	// reset for Gecko	
	if (FTB_Browser.isGecko) {
		this.designEditor.document.designMode = 'On';
		this.designEditor.document.execCommand("useCSS", false, true);
	}
    
    if (this.ancestorArea) 
		this.ancestorArea.innerHTML = "";
    
	if (this.designModeTab)
		this.SetActiveTab(this.designModeTab);
    
    //this.SetToolbarItemsEnabledState();
    
    this.mode = FTB_MODE_DESIGN;
    //this.Focus();
    return true;
};
FTB_FreeTextBox.prototype.GoToPreviewMode = function() {
    if (this.mode == FTB_MODE_DESIGN) this.CopyDesignToHtml();
    this.CopyHtmlToIframe(this.previewPane);

    this.designEditorArea.style.display = 'none';
    this.htmlEditorArea.style.display = 'none';
    this.previewPaneArea.style.display = '';
      
    this.SetActiveTab(this.previewModeTab);
    if (this.ancestorArea) this.ancestorArea.innerHTML = "";
    
    this.mode = FTB_MODE_PREVIEW;
    return true;
};
FTB_FreeTextBox.prototype.HtmlEncode = function( text ) {
	if ( typeof( text ) != "string" )
		text = text.toString() ;

	text = text.replace(/&/g, "&amp;") ;
	text = text.replace(/"/g, "&quot;") ;
	text = text.replace(/</g, "&lt;") ;
	text = text.replace(/>/g, "&gt;") ;
	text = text.replace(/'/g, "&#146;") ;

	return text ;
};
FTB_FreeTextBox.prototype.ExecuteCommand = function(commandName, middle, commandValue) {
	if (this.mode != FTB_MODE_DESIGN) return;
	this.designEditor.focus();
	if (commandName == 'backcolor' && !FTB_Browser.isIE) commandName = 'hilitecolor';
	
	if (!FTB_Browser.isIE) {
		this.designEditor.document.execCommand("useCSS",null,true);
	}
	
	this.designEditor.document.execCommand(commandName,middle,commandValue);
	if (this.clientSideTextChanged)
		this.clientSideTextChanged(this);
};
FTB_FreeTextBox.prototype.QueryCommandState = function(commandName) {
	if (this.mode != FTB_MODE_DESIGN) return false;
	try {
		if (this.designEditor.document.queryCommandState(commandName)) {
			return FTB_BUTTON_ON;
		} else {
			// special case for paragraph on IE
			if (commandName == 'justifyleft') {
				if (this.designEditor.document.queryCommandState('justifyright') == false &&
					this.designEditor.document.queryCommandState('justifycenter') == false &&
					this.designEditor.document.queryCommandState('justifyfull') == false ) {
					return FTB_BUTTON_ON;
				} else {
					return FTB_BUTTON_OFF;
				}
			} else { 
				return FTB_BUTTON_OFF;
			}
		}
	} catch(exp) {
		//document.getElementById('debug').value += '\n' + 'BUT ERROR: ' + commandName + '\n' + exp;
		return FTB_BUTTON_OFF;
	}
};
FTB_FreeTextBox.prototype.QueryCommandValue = function(commandName) {
	if (this.mode != FTB_MODE_DESIGN) return false;
	
	try	{
		value = this.designEditor.document.queryCommandValue(commandName);
	} catch (err) {
		this.RefreshDesignMode();
		value = '';
	}	
	
	switch (commandName) {
		case "backcolor":
			if (FTB_Browser.isIE) {
				value = FTB_IntToHexColor(value);
			} else {
				if (value == "") value = "#FFFFFF";
			}
			break;
		case "forecolor":
			if (FTB_Browser.isIE) {
				value = FTB_IntToHexColor(value);
			} else {
				if (value == "") value = "#000000";
			}
			break;
		case "formatBlock":
			if (!FTB_Browser.isIE) {
				if (value == "" || value == "<x>")
					value = "<p>";
				else
					value = "<" + value + ">";
			}
			break;
	}
	if (value == '' || value == null) {
		if (commandName == 'fontsize') return '3';
		if (commandName == 'fontname') return 'Times New Roman';	
		if (commandName == 'forecolor') return '#000000';	
		if (commandName == 'backcolor') return '#ffffff';
	}
		
	return value;

};
FTB_FreeTextBox.prototype.SurroundHtml = function(start,end) {
	if (this.mode == FTB_MODE_HTML) return;
	this.designEditor.focus();
	
	if (FTB_Browser.isIE) {
		var sel = this.designEditor.document.selection.createRange();
		html = start + sel.htmlText + end;
		sel.pasteHTML(html);		
	} else {
        selection = this.designEditor.window.getSelection();
        if (selection) {
            range = selection.getRangeAt(0);
        } else {
            range = this.designEditor.document.createRange();
        } 
        
        this.InsertHtml(start + selection + end);
        //this.InsertHtml(start + selection.toString().replace(/</g,"&lt;").replace(/</g,"&lt;").replace(/\n/g,"<br>\n") + end);
	}	
};
FTB_FreeTextBox.prototype.InsertHtml = function(html) {
	if (this.mode != FTB_MODE_DESIGN) return;
	this.designEditor.focus();
	if (FTB_Browser.isIE) {
		sel = this.designEditor.document.selection.createRange();
		sel.pasteHTML(html);
	} else {

        selection = this.designEditor.window.getSelection();
		if (selection) {
			range = selection.getRangeAt(0);
		} else {
			range = editor.document.createRange();
		}

        var fragment = this.designEditor.document.createDocumentFragment();
        var div = this.designEditor.document.createElement("div");
        div.innerHTML = html;

        while (div.firstChild) {
            fragment.appendChild(div.firstChild);
        }

        selection.removeAllRanges();
        range.deleteContents();

        var node = range.startContainer;
        var pos = range.startOffset;

        switch (node.nodeType) {
            case 3:
                if (fragment.nodeType == 3) {
                    node.insertData(pos, fragment.data);
                    range.setEnd(node, pos + fragment.length);
                    range.setStart(node, pos + fragment.length);
                } else {
                    node = node.splitText(pos);
                    node.parentNode.insertBefore(fragment, node);
                    range.setEnd(node, pos + fragment.length);
                    range.setStart(node, pos + fragment.length);
                }
                break;

            case 1:
                node = node.childNodes[pos];
                node.parentNode.insertBefore(fragment, node);
                range.setEnd(node, pos + fragment.length);
                range.setStart(node, pos + fragment.length);
                break;
        }
        selection.addRange(range);
	}
};
/* ------------------------------------------------
START: Node and Selection Methods */

FTB_FreeTextBox.prototype.CheckTag = function(item,tagName) {
	if (!item) return null;
	if (item.tagName.search(tagName)!=-1) {
		return item;
	}
	if (item.tagName=='BODY') {
		return false;
	}
	item=item.parentElement;
	return this.CheckTag(item,tagName);
};
FTB_FreeTextBox.prototype.GetParentElement = function() {

	var sel = this.GetSelection();
	var range = this.CreateRange(sel);
	if (FTB_Browser.isIE) {
		switch (sel.type) {
		    case "Text":
		    case "None":
				return range.parentElement();
		    case "Control":
				return range.item(0);
		    default:
				return this.designEditor.document.body;
		}
	} else try {
		var p = range.commonAncestorContainer;
		if (!range.collapsed && range.startContainer == range.endContainer &&
		    range.startOffset - range.endOffset <= 1 && range.startContainer.hasChildNodes())
			p = range.startContainer.childNodes[range.startOffset];
		/*
		alert(range.startContainer + ":" + range.startOffset + "\n" +
		      range.endContainer + ":" + range.endOffset);
		*/
		while (p.nodeType == 3) {
			p = p.parentNode;
		}
		return p;
	} catch (e) {
		return null;
	}
};
FTB_FreeTextBox.prototype.InsertNodeAtSelection = function(toBeInserted) {
	if (!FTB_Browser.isIE) {
		var sel = this.GetSelection();
		var range = this.CreateRange(sel);
		// remove the current selection
		sel.removeAllRanges();
		range.deleteContents();
		var node = range.startContainer;
		var pos = range.startOffset;
		switch (node.nodeType) {
		    case 3: // Node.TEXT_NODE
			// we have to split it at the caret position.
			if (toBeInserted.nodeType == 3) {
				// do optimized insertion
				node.insertData(pos, toBeInserted.data);
				range = this._createRange();
				range.setEnd(node, pos + toBeInserted.length);
				range.setStart(node, pos + toBeInserted.length);
				sel.addRange(range);
			} else {
				node = node.splitText(pos);
				var selnode = toBeInserted;
				if (toBeInserted.nodeType == 11 /* Node.DOCUMENT_FRAGMENT_NODE */) {
					selnode = selnode.firstChild;
				}
				node.parentNode.insertBefore(toBeInserted, node);
				this.SelectNodeContents(selnode);
			}
			break;
		    case 1: // Node.ELEMENT_NODE
			var selnode = toBeInserted;
			if (toBeInserted.nodeType == 11 /* Node.DOCUMENT_FRAGMENT_NODE */) {
				selnode = selnode.firstChild;
			}
			node.insertBefore(toBeInserted, node.childNodes[pos]);
			this.SelectNodeContents(selnode);
			break;
		}
	}
};
FTB_FreeTextBox.prototype.SelectNodeContents = function(node, pos) {
	var range;
	var collapsed = (typeof pos != "undefined");
	if (isIE) {
		range = this.designEditor.document.body.createTextRange();
		range.moveToElementText(node);
		(collapsed) && range.collapse(pos);
		range.select();
	} else {
		var sel = this.GetSelection();
		range = this.designEditor.document.createRange();
		range.selectNodeContents(node);
		(collapsed) && range.collapse(pos);
		sel.removeAllRanges();
		sel.addRange(range);
	}
};
FTB_FreeTextBox.prototype.SelectNextNode = function(el) {
	var node = el.nextSibling;
	while (node && node.nodeType != 1) {
		node = node.nextSibling;
	}
	if (!node) {
		node = el.previousSibling;
		while (node && node.nodeType != 1) {
			node = node.previousSibling;
		}
	}
	if (!node) {
		node = el.parentNode;
	}
	this.SelectNodeContents(node);
};
FTB_FreeTextBox.prototype.GetSelection = function() {
	if (FTB_Browser.isIE) {
		return this.designEditor.document.selection;
	} else {
		return this.designEditor.getSelection();
	}
};
FTB_FreeTextBox.prototype.CreateRange = function(sel) {
	if (FTB_Browser.isIE) {
		return sel.createRange();
	} else {
		if (typeof sel != "undefined") {
			try {
				return sel.getRangeAt(0);
			} catch(e) {
				return this.designEditor.document.createRange();
			}
		} else {
			return this.designEditor.document.createRange();
		}
	}
};
FTB_FreeTextBox.prototype.SelectNodeContents = function(node, pos) {
	var range;
	var collapsed = (typeof pos != "undefined");
	if (FTB_Browser.isIE) {
		range = this.designEditor.document.body.createTextRange();
		range.moveToElementText(node);
		(collapsed) && range.collapse(pos);
		range.select();
	} else {
		var sel = this.GetSelection();
		range = this.designEditor.document.createRange();
		range.selectNodeContents(node);
		(collapsed) && range.collapse(pos);
		sel.removeAllRanges();
		sel.addRange(range);
	}
};
FTB_FreeTextBox.prototype.GetNearest = function(tagName) {
	var ancestors = this.GetAllAncestors();
	var ret = null;
	tagName = ("" + tagName).toLowerCase();
	for (var i=0;i<ancestors.length;i++) {
		var el = ancestors[i];
		if (el) {
			if (el.tagName.toLowerCase() == tagName) {
				ret = el;
				break;
			}
		}
	}
	return ret;
};
FTB_FreeTextBox.prototype.GetAllAncestors = function() {
	var p = this.GetParentElement();
	var a = [];
	while (p && (p.nodeType == 1) && (p.tagName.toLowerCase() != 'body')) {
		a.push(p);
		p = p.parentNode;
	}
	a.push(this.designEditor.document.body);
	return a;
};
FTB_FreeTextBox.prototype.GetStyle = function() {
	var parent = this.GetParentElement();
	return parent.className;	
};
FTB_FreeTextBox.prototype.SetActiveTab = function(theTD) {
	if (theTD) {
		parentTR = theTD.parentElement;
		parentTR = document.getElementById(this.id + "_TabRow");

		selectedTab = 1;
		totalButtons = parentTR.cells.length-1;
		for (var i=1;i< totalButtons;i++) {
			parentTR.cells[i].className = this.id + "_TabOffRight";
			if (theTD == parentTR.cells[i]) { selectedTab = i; }
		}

		if (selectedTab==1) {
			parentTR.cells[0].className = this.id + "_StartTabOn";
		} else {
			parentTR.cells[0].className = this.id + "_StartTabOff";
			parentTR.cells[selectedTab-1].className = this.id + "_TabOffLeft";
		}

		theTD.className = this.id + "_TabOn";
	}
};
FTB_FreeTextBox.prototype.Focus = function() {
	if (this.mode == FTB_MODE_DESIGN) {
		this.designEditor.focus();
		this.UpdateToolbars();
	} else if (this.mode == FTB_MODE_HTML) {
		this.htmlEditor.focus();
	}
	this.hasFocus = true;
};
FTB_FreeTextBox.prototype.SetStyle = function(className) {
	
	// retrieve parent element of the selection
	var parent = this.GetParentElement();
	
	var surround = true;
	var isSpan = (parent && parent.tagName.toLowerCase() == "span");
	
	/*
	// remove class stuff??
	if (isSpan && index == 0 && !/\S/.test(parent.style.cssText)) {
		while (parent.firstChild) {
			parent.parentNode.insertBefore(parent.firstChild, parent);
		}
		parent.parentNode.removeChild(parent);
		this.UpdateToolbars();
		return;
	}
	*/
	
	// if we're already in a SPAN
	if (isSpan) {
		if (parent.childNodes.length == 1) {
			parent.className = className;
			surround = false;
			this.UpdateToolbars();
			return;
		}
	} else {
		
	}

	if (surround) {
		this.SurroundHtml("<span class='" + className + "'>", "</span>");
	}
};
FTB_FreeTextBox.prototype.GetHtml = function() {
	if (this.mode == FTB_MODE_DESIGN)
		this.CopyDesignToHtml();
		
	return this.htmlEditor.value;
};
FTB_FreeTextBox.prototype.SetHtml = function(html) {
	this.htmlEditor.value = html;
	this.mode = FTB_MODE_HTML;	
	this.GoToDesignMode();
};
FTB_FreeTextBox.prototype.StoreHtml = function() {
	if (!this.initialized) return;
	
	if (this.mode == FTB_MODE_DESIGN)
		this.CopyDesignToHtml();
		
	return true;
};
/* START: Button Methods 
-------------------------------- */
FTB_FreeTextBox.prototype.DeleteContents = function() {
	if (confirm('Do you want to delete all the HTML and text presently in the editor?')) {	
		this.designEditor.document.body.innerHTML = '';
		this.htmlEditor.value='';
		this.GoToDesignMode();
	}
};
FTB_FreeTextBox.prototype.Cut = function() {
	if (this.mode == FTB_MODE_DESIGN) {

		try {
			this.ExecuteCommand('cut'); 
		} catch (e) {
			alert('Your security settings to not allow you to use this command.  Please visit http://www.mozilla.org/editor/midasdemo/securityprefs.html for more information.');
		}	
	} else {
		//alert("TODO");
	}
};
FTB_FreeTextBox.prototype.Copy = function() {
	if (this.mode == FTB_MODE_DESIGN) {
		try {
			this.ExecuteCommand('copy');
		} catch (e) {
			alert('Your security settings to not allow you to use this command.  Please visit http://www.mozilla.org/editor/midasdemo/securityprefs.html for more information.');
		}	
	} else {
		//alert("TODO");
	}
};
FTB_FreeTextBox.prototype.Paste = function() {
	if (this.mode == FTB_MODE_DESIGN) this.CapturePaste();
}
FTB_FreeTextBox.prototype.SelectAll = function() {
	if (this.mode == FTB_MODE_DESIGN) {		
		this.SelectNodeContents(this.designEditor.document.body);
	}
};
FTB_FreeTextBox.prototype.Print = function() {
	if (this.mode == FTB_MODE_DESIGN) {
		if (FTB_Browser.isIE) {
			this.ExecuteCommand('print'); 
		} else {
			this.designEditor.print();
		}	
	} else {
		printWindow = window.open('','','');
		printWindow.document.open();
		printWindow.document.write("<html><body><pre>" + this.HtmlEncode(this.htmlEditor.value) + "</code></body></html>");
		printWindow.document.close();
		printWindow.document.body.print();
		printWindow.close();
	}
};
FTB_FreeTextBox.prototype.CreateLink = function() {
	var link = this.GetNearest('a');
	var url = '';
	if (link) {
		var url = link.getAttribute('temp_href');
		if (!url) url = link.getAttribute('href');
	} else {
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
	}
	url = prompt('Enter a URL:', (url != '') ? url : 'http://');
	
	if (url != null) {
		if (!link) {
			var tempUrl = 'http://tempuri.org/tempuri.html';
			this.ExecuteCommand('createlink',null,tempUrl);
			var links = this.designEditor.document.getElementsByTagName('a');
			for (var i=0;i<links.length;i++) {
				if (links[i].href == tempUrl) {
					link = links[i];
					break;
				}
			}		
		}
		link.href = url;
		link.setAttribute('temp_href',url);
	}

};
FTB_FreeTextBox.prototype.IeSpellCheck = function() {
	if (!FTB_Browser.isIE) {
		alert('IE Spell is not supported in Mozilla');
		return;
	}
	try {
		var tspell = new ActiveXObject('ieSpell.ieSpellExtension');
		tspell.CheckAllLinkedDocuments(window.document);
	} catch (err){
		if (window.confirm('You need ieSpell to use spell check. Would you like to install it?')){
			window.open('http://www.iespell.com/download.php');
		};
	}
};
FTB_FreeTextBox.prototype.NetSpell = function() {
	if (typeof(checkSpellingById) == 'function') {
		checkSpellingById(this.id + '_designEditor');
	} else {
		alert('Netspell libraries not properly linked.');
	}
};
FTB_FreeTextBox.prototype.InsertImage = function() {

	var img = this.GetNearest('img');
	var imgSrc = '';
	if (img) {
		var imgSrc = img.getAttribute('temp_src');
		if (!imgSrc) imgSrc = img.getAttribute('src');
	}
	
	imgSrc = prompt('Enter a URL:', (imgSrc != '') ? imgSrc : 'http://');
	
	if (imgSrc != '') {
		if (!img) {
			var tempUrl = 'http://tempuri.org/tempuri.html';
			this.ExecuteCommand('insertimage',null,tempUrl);
			var imgs = this.designEditor.document.getElementsByTagName('img');
			for (var i=0;i<imgs.length;i++) {
				if (imgs[i].src == tempUrl) {
					img = imgs[i];
					break;
				}
			}		
		}
		img.src = imgSrc;
		img.setAttribute('temp_src',imgSrc);
	}
};
FTB_FreeTextBox.prototype.SaveButton = function() {
	this.StoreHtml();	
	dotNetName = this.id.split('_').join(':');
	__doPostBack(dotNetName,'Save');
};
FTB_FreeTextBox.prototype.InsertImageFromGallery = function() {
	
	url = this.imageGalleryUrl.replace(/\{0\}/g,this.imageGalleryPath);	
	url += "&ftb=" + this.id;

	var gallery = window.open(url,'gallery','width=700,height=600,toolbars=0,resizable=1');
	gallery.focus();	
}
FTB_FreeTextBox.prototype.Preview = function() {
	this.CopyDesignToHtml();

	printWindow = window.open('','','toolbars=no');
	printWindow.document.open();
	printWindow.document.write("<html><head><link rel='stylesheet' href='" + this.designModeCss + "' type='text/css' />" + ((this.baseUrl != '') ? "<base href='" + this.baseUrl + "' />" : "") + "</head><body>" + this.htmlEditor.value + "</body></html>");
	printWindow.document.close();	
};
/* START: InsertTable */
FTB_FreeTextBox.prototype.InsertTable = function(cols,rows,width,widthUnit,align,cellpadding,cellspacing,border) {
	this.designEditor.focus();
	var sel = this.GetSelection();
	var range = this.CreateRange(sel);	
	
	var doc = this.designEditor.document;
	// create the table element
	var table = doc.createElement("table");	

	// assign the given arguments
	table.style.width 	= width + widthUnit;
	table.align	 		= align;
	table.border	 	= border;
	table.cellSpacing 	= cellspacing;
	table.cellPadding 	= cellpadding;
	
	var tbody = doc.createElement("tbody");
	table.appendChild(tbody);
	
	for (var i = 0; i < rows; ++i) {
		var tr = doc.createElement("tr");
		tbody.appendChild(tr);
		for (var j = 0; j < cols; ++j) {
			var td = doc.createElement("td");
			tr.appendChild(td);			
			if (!FTB_Browser.isIE) td.appendChild(doc.createElement("br"));
		}
	}
	
	if (FTB_Browser.isIE) {
		range.pasteHTML(table.outerHTML);
	} else {
		this.InsertNodeAtSelection(table);
	}
	
	return true;
};
FTB_FreeTextBox.prototype.InsertTableWindow = function() {
	this.LaunchTableWindow(false);
};
FTB_FreeTextBox.prototype.EditTable = function() {
	this.LaunchTableWindow(true);
};
FTB_FreeTextBox.prototype.LaunchTableWindow = function(editing) {
		
	var tableWin = window.open("","tableWin","width=400,height=200");
	if (tableWin) {
		tableWin.focus();
	} else {
		alert("Please turn off your PopUp blocking software");
		return;
	}
		
	tableWin.document.body.innerHTML = '';
	tableWin.document.open();
	tableWin.document.write(FTB_TablePopUpHtml);
	tableWin.document.close();
	
	launchParameters = new Object();
	launchParameters['ftb'] = this;
	launchParameters['table'] = (editing) ? this.GetNearest("table") : null;	
	tableWin.launchParameters = launchParameters;
	tableWin.load();
};
var FTB_TablePopUpHtml = new String("\
<html><body> \
<head>\
<title>Table Editor</title>\
<style type='text/css'>\
html, body { \
	background-color: #eee; \
	color: #000; \
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
function doTable() { \
	ftb = window.launchParameters['ftb'];\
	table = window.launchParameters['table'];\
	if (table) { \
		table.style.width = document.getElementById('f_width').value + document.getElementById('f_unit').options[document.getElementById('f_unit').selectedIndex].value; \
		table.align = document.getElementById('f_align').options[document.getElementById('f_align').selectedIndex].value;  \
		table.cellPadding = (document.getElementById('f_padding').value.length > 0 && !isNaN(document.getElementById('f_padding').value)) ? parseInt(document.getElementById('f_padding').value) : ''; \
		table.cellSpacing = (document.getElementById('f_spacing').value.length > 0 && !isNaN(document.getElementById('f_spacing').value)) ? parseInt(document.getElementById('f_spacing').value) : ''; \
		table.border = parseInt(document.getElementById('f_border').value); \
	} else {\
		cols = parseInt(document.getElementById('f_cols').value); \
		rows = parseInt(document.getElementById('f_rows').value); \
		width = document.getElementById('f_width').value; \
		widthUnit = document.getElementById('f_unit').options[document.getElementById('f_unit').selectedIndex].value; \
		align = document.getElementById('f_align').value; \
		cellpadding = document.getElementById('f_padding').value; \
		cellspacing = document.getElementById('f_spacing').value; \
		border = document.getElementById('f_border').value; \
		ftb.InsertTable(cols,rows,width,widthUnit,align,cellpadding,cellspacing,border); \
	} \
	window.close(); \
}\
</script>\
</head>\
<body>\
<form action=''> \
<h3>Table Editor</h3> \
<table border='0' style='padding: 0px; margin: 0px'> \
  <tbody> \
  <tr> \
    <td style='width: 4em; text-align: right'>Rows:</td> \
    <td><input type='text' name='rows' id='f_rows' size='5' title='Number of rows' value='2' /></td> \
    <td></td> \
    <td></td> \
    <td></td> \
  </tr> \
  <tr> \
    <td style='width: 4em; text-align: right'>Cols:</td> \
    <td><input type='text' name='cols' id='f_cols' size='5' title='Number of columns' value='4' /></td> \
    <td style='width: 4em; text-align: right'>Width:</td> \
    <td><input type='text' name='width' id='f_width' size='5' title='Width of the table' value='100' /></td> \
    <td><select size='1' name='unit' id='f_unit' title='Width unit'> \
      <option value='%' selected='1'  >Percent</option> \
      <option value='px'              >Pixels</option> \
      <option value='em'              >Em</option> \
    </select></td> \
  </tr> \
  </tbody> \
</table> \
<table width='100%'>\
<tr><td valign='top'>\
	<fieldset>\
	<legend>Layout</legend> \
		<table>\
		<tr><td class='f_title'>Alignment:</td><td>\
			<select size='1' name='align' id='f_align' \
			title='Positioning of the table'> \
			<option value='' selected='1'                >Not set</option> \
			<option value='left'                         >Left</option> \
			<option value='center'                       >Center</option> \
			<option value='right'                        >Right</option> \
			</select> \
		</td></tr>\
		<tr><td class='f_title'>Border thickness:</td><td>\
			<input type='text' name='border' id='f_border' size='5' value='1' title='Leave empty for no border' /> \
		</td></tr></table>\
	</fieldset>\
</td><td valign='top'>\
	<fieldset>\
	<legend>Spacing</legend> \
		<table>\
		<tr><td class='f_title'>Cell spacing:</td><td>\
			<input type='text' name='spacing' id='f_spacing' size='5' value='1' title='Space between adjacent cells' /> \
		</td></tr>\
		<tr><td class='f_title'>Cell padding:</td><td>\
			<input type='text' name='padding' id='f_padding' size='5' value='1' title='Space between content and border in cell' /> \
		</td></tr></table>\
	</fieldset> \
</td></tr></table>\
<div class='footer'> \
<button type='button' name='ok' id='f_goButton' onclick='doTable();window.close();'>OK</button> \
<button type='button' name='cancel' onclick='window.close();'>Cancel</button> \
</div> \
<script language='JavaScript'> \
function load() { \
	ftb = window.launchParameters['ftb'];\
	table = window.launchParameters['table'];\
	if (table) { \
		width = window.opener.FTB_ParseUnit(table.style.width); \
		document.getElementById('f_width').value = width.value; \
		window.opener.FTB_SetListValue(document.getElementById('f_align'),table.align,true); \
		window.opener.FTB_SetListValue(document.getElementById('f_unit'),width.unitType,true); \
		document.getElementById('f_border').value = table.border; \
		document.getElementById('f_padding').value = table.cellPadding; \
		document.getElementById('f_spacing').value = table.cellSpacing; \
		document.getElementById('f_cols').disabled = true; \
		document.getElementById('f_rows').disabled = true; \
	} \
} \
</script></form> \
</body> \
</html>");

/* END: InsertTable */
/* --------------------------------
END: Button Methods */