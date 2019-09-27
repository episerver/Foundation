/*!
 * Linkselect jQuery Plug-in
 *
 * Copyright 2008 Giva, Inc. (http://www.givainc.com/labs/) 
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 * 	http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * Date: 2009-07-30
 * Rev:  1.2.08
 */
;(function($){
	// set the version of the link select
	$.linkselect = {version: "1.2.08"};
	
	$.fn.linkselect = function(options) {
		var method = typeof arguments[0] == "string" && arguments[0];
		var args = method && Array.prototype.slice.call(arguments, 1) || arguments;

		// if a method is supplied, execute it for non-empty results
		if( method && this.length ){
			// get a reference to the first linkselect found
			var self = $.data(this[0], "linkselect");

			// if request a copy of the object, return it			
			if( method.toLowerCase() == "object" ) return self;
			// if method is defined, run it and return either it's results or the chain
			else if( self[method] ){
				// define a result variable to return to the jQuery chain
				var result;
				this.each(function (i){
					// apply the method to the current element
					var r = $.data(this, "linkselect")[method].apply(self, args);
					// if first iteration we need to check if we're done processing or need to add it to the jquery chain
					if( i == 0 && r ){
						// if this is a jQuery item, we need to store them in a collection
						if( !!r.jquery ){
							result = $([]).add(r);
						// otherwise, just store the result and stop executing
						} else {
							result = r;
							// since we're a non-jQuery item, just cancel processing further items
							return false;
						}
					// keep adding jQuery objects to the results
					} else if( !!r && !!r.jquery ){
						result = result.add(r);
					}
				});

				// return either the results (which could be a jQuery object) or the original chain
				return result || this;
			// everything else, return the chain
			} else return this;
		// initializing request
		} else {
			// create a new linkselect for each object found
			return this.each(function (){
				new $.LinkSelect(this, options);
			});
		};
	};

	$.LinkSelect = function (el, options){
		options = $.extend({}, $.LinkSelect.defaults, options);
		
		var self = this, select = el, $select = $(el), shortcuts = {}, disabled = false, current_char_pos = 0, last_char_code, is_open = false;
		
		// store the global properties
		this.id = $select.attr("id");
		
		// get/set the value of the field
		this.val = function (value, doCallback){
			// update the selected value
			if( arguments.length > 0 ){
				setSelectedItem(value, doCallback);
				return $a;
			} 
			else return $input.val();
		};

		// set the focus to the field		
		this.focus = function (){
			// place focus asynchronously to avoid click collisions
			setTimeout(function(){$a.focus();}, 1);
			return $a;
		};
		
		// set the blur to the field		
		this.blur = function (){
			// place focus asynchronously to avoid click collisions
			setTimeout(function(){$a.blur();}, 1);
			return $a;
		};
		
		// place focus and open the menu
		this.open = function (callback, bDoFocus){
			// if disabled, skip processing
			if( disabled ) return $a;
			// close any open linkselects
			$(document).triggerHandler("click.linkselect");
			// place focus
			if( bDoFocus !== false ) $a.trigger("focus");
			// show the options asynchronously to avoid click collisions
			setTimeout(function(){ showOptions(callback); }, 1);
			return $a;
		}
		
		// disable the link
		this.disable = function (status){
			disabled = status;
			// remove any existing disabled label
			$a.parent().find("span." + options.classDisabled).remove();
			// show/hide the anchor depending on the disabled status
			$a[disabled ? "hide" : "show"]();
			// if disabled, add a span			
			if( disabled ) $a.after('<span class="' + options.classDisabled + '">' + $a.html() + '</span>');
			return $a;
		}
		
		// replace the options
		this.replaceOptions = function (options, doCallback){
			// remove the current options
			$select.children('option').remove();

			// add each new item
			$.each(options, function (i){
				// create the new option
				var $option = $("<option/>").attr("value", this.value).html(this.text);
				// check to see if the option should be selected
				if( this.selected == true ) $option.attr("selected", "selected");
				// if any classes were specified, add them
				if( this.className ) $option.addClass(this.className);
				// add the new option to the array
				$option.appendTo($select);
			});
			
			// we need to repaint the dropdown so the widht/height all get recalculated based on the new properties
			repaint();
			// bind the new triggers
			bindItems();
			// update the selected value
			getSelectedItem().trigger("click.linkselect", [true, doCallback]);
		};
		
		var $html = createHtml();

		// insert the new html and remove the old stuff
		$select.after($html).remove();
		// get the hidden input field
		var $input = $html.filter("input");
		// get anchor
		var $a = $html.filter("a");
		// get container
		var $container = $html.filter("div");
		// get scrollable list
		var $scrollable = $html.find(".scrollable");
		// get the title
		var $title = $html.find(".title");
		// get the list
		var $ul = $container.find("ul");
		// track last event
		var lastCurrentEvent;
		
		// copy the class statements to the input field (this is so class based selectors still work)
		$input.addClass($select.attr("className"));
		
		// store a reference to this link select
		$.data($input[0], "linkselect", this);
		
		// move the container to the main body so it can be correctly positioned absolutely
		$container
			// add the container to the body (this is so we can correctly absolutely position it)
			.appendTo("body")
			// if user is moving the mouse, we want to make sure to trigger the mouseover event
			.bind("mousemove.linkselect", function (e){
				lastCurrentEvent = e;
			});
		
		// this function will bind the elements
		function bindItems(){
			$ul.find("li")
				.bind("mouseover.linkselect", function (e){
					// if we're moving via the keyboard, then kill processing
					if( lastCurrentEvent && lastCurrentEvent.type == "keydown" ) return;
					// highlight the current option
					setHighlightedItem($(this));
					lastCurrentEvent = e;
				})
				.bind("click.linkselect", function (e, nofocus, doCallback){
					// stop the default behavior
					e.preventDefault();
					// remove the selected class from the previous option
					var $previous = getSelectedItem().removeClass(options.classSelected);
					// get the currently selected item and remove the class
					var $current = $(this).addClass(options.classSelected);
					var value = $current.attr("rel") || "";
					var text = $current.find("." + options.classValue).html();
					// hide the container
					hideOptions(nofocus);

					// trigger the change callback if it's false, stop processing
					if( (doCallback !== false) && (($.isFunction(options.change) && (options.change.apply(self, [this, value, text, doCallback]) === false)) || ($.isFunction($select[0].onchange) && ($select[0].onchange.apply(self, [this, value, text, doCallback]) === false))) ){
						// restore the selected classes (since we're not selecting this option)
						$previous.addClass(options.classSelected);
						$current.removeClass(options.classSelected)
						return;
					}
					// update the hidden value
					$input.val(value);
					// update the text of the anchor and trigger the focus handler
					$a.html(text)[(nofocus !== true)?"trigger":"triggerHandler"]("focus", [nofocus]);
					// if the field is disabled, update the disabled label
					if( disabled ) $a.parent().find("span." + options.classDisabled).html(text);
				});
		};
		// bind events to the list items
		bindItems();
			
		$a.bind("click.linkselect", function(e){
			e.preventDefault();
			toggleOptions();
			// make sure the link has focus for IE6
			if( $.browser.msie ){
				setTimeout(function (){
					$a.trigger("focus.linkselect");
				}, 0);
			}
		})
		.bind("focus.linkselect", function (e, nofocus){
			// we need to make sure not to re-add the class for IE6 since we have to reset the focus
			if( !$container.is(":visible") && (nofocus !== true) ){
				$a.addClass(options.classLinkFocus);
			}
		})
		.bind("blur.linkselect", function (e){
			// if not coming from a live user event, then hide the options
			if( isJQueryEvent(e) ) hideOptions();
			$a.removeClass(options.classLinkFocus);
		})
		.bind(($.browser.safari ? "keydown" : "keypress" ) + ".linkselect", function(e, e2){
			// if we've passed a copy of an event object, use that instead
			if( !!e2 ) var e = e2;
			
			var key = e.keyCode || e.charCode, cur_char = String.fromCharCode(key).toLowerCase();

			switch(key) {
				case 38: // up
				case 40: // down
					e.preventDefault();
					moveSelect((key == 38) ? -1 : 1);
					lastCurrentEvent = e;
					break;
				case 13: // return
					e.preventDefault();
					// only select the item if the menu is visible
					if( $container.is(":visible") ){
						$container.find('li.' + options.classCurrent).trigger('click.linkselect');
					} else {
						$a.trigger('click.linkselect');
					}
					break;
				case 9: // tab
				case 27: // escape
					hideOptions();
				  break;
				case 35: // end
					e.preventDefault();
					moveLast();
					lastCurrentEvent = e;
				  break;
				case 36: // home
					e.preventDefault();
					moveFirst();
					lastCurrentEvent = e;
				  break;
				case 33: // page up
				case 34: // page down
					e.preventDefault();
					var isVisible = $container.is(":visible");
					// show the container so we can get heights
					if( !isVisible ) $container.show();
					var iItemsPerPage = parseInt($scrollable.height()/$ul.find("li:first").outerHeight(), 10);
					// now hide the container
					if( !isVisible ) $container.hide();
					// move up/down the total number of items visible
					moveSelect((key == 33) ? iItemsPerPage * -1 : iItemsPerPage);
				  break;
			}
			
			// if we're pressed a different key, then restart the array position
			if( cur_char != last_char_code ) current_char_pos = 0;
			// store the current character
			last_char_code = cur_char;
			
			// check to see if there's a keyboard shortcut configured for the key press
			if( typeof shortcuts[cur_char] != "undefined" ){
				// if we're outside the bounds of the array, go back to zero
				if( current_char_pos >= shortcuts[cur_char].length ) current_char_pos = 0;

				// if there is, trigger the click event
				$ul.find('#' + self.id + '_li_' + shortcuts[cur_char][current_char_pos]).trigger("click.linkselect");
				e.preventDefault();
				e.stopPropagation();

				// increase the array position
				current_char_pos++;
			}
		});

		// IE6 doesn't register certain keypress events, so we must catch them during the keydown event
		if( $.browser.msie ) $a.bind("keydown.linkselect", function (e){
			// check to see if a key was pressed that IE6 doesn't trigger a keypress event for
			if( ",8,9,33,34,35,36,37,38,39,40,".indexOf("," + e.keyCode + ",") > -1 ) return $(this).triggerHandler("keypress.linkselect", [e]);
		});

		// we need to close the menu if we don't click on it
		$(document).bind("click.linkselect", function (e){
			if( (e.target !== $a[0]) && (e.target !== $scrollable[0]) && $container.is(":visible") ){
				hideOptions();
				// make sure we remove the focus class
				$a.removeClass(options.classLinkFocus);
			}
		});
		
		// move the layer on a resize
		$(window).resize(function (){
			if( is_open )
				anchorTo($a, $container, true)
		});

		// create the replacement html
		function createHtml(){
			var id = self.id;
			var title = $select.attr("title");
			// get the selected text
			var text = select.selectedIndex == -1 ? "" : select[select.selectedIndex].text;
			var value = select.selectedIndex == -1 ? "" : select[select.selectedIndex][($.browser.msie && $.browser.version <= 7 && !(select[select.selectedIndex].attributes['value'].specified)) ? "text" : "value"];
			// get the current tab index and make sure to copy it
			var tabindex = $select.attr("tabindex");
			
			var aHtml = [
				'<a href="#' + self.id + '" id="' + self.id + '_link" class="' + options.classLink + '"' + (tabindex ? ' tabindex="' + tabindex + '"' : '') + '>' + text + '</a>'
				, '<input type="hidden" name="' + self.id + '" id="' + self.id + '" value="' + value + '" />'
				, '<div class="' + options.classContainer + '">'
					, (title) ? '<div class="title"><span>' + title + '</span></div>' : ''
					, '<div class="scrollable"><ul id="' + self.id + '_list">'
					, generateOptions($select.children('option'))
					, '</ul></div>'
				, '</div>'
			];

			// generate and return the new HTML			
			return $(aHtml.join(""));
		}
		
		function generateOptions($options){
			// purge the shortcut list
			shortcuts = [];
			// an array to store the <li/> tags we're creating
			var lis = [];
			$options.each(function(i){
				var $option = $(this);
				var bSelected = $option.is(":selected");
				var label = $.trim($option.text());
				var html = '<span class="' + options.classValue + '">' + label + '</span>';
				var value = $.browser.msie && $.browser.version <= 7 && !(this.attributes['value'].specified) ? this.text : this.value
				
				// if a special format renderer has been specified, use it (will use original HTML if format function returns false or undefined)
				if( $.isFunction(options.format) ) html = options.format.apply(self, [html, value, label, i, $option, options]) || html;
				
				var first_char = (label.length > 1) ? label.substring(0, 1).toLowerCase() : "";
				
				// create an array of shortcut keys
				if( !shortcuts[first_char] ) shortcuts[first_char] = [];
				// push the position into the array
				shortcuts[first_char].push(i);
				
				// get the current class statement				
				var cn = $.trim(this.className + ' ' + (bSelected ? options.classSelected : ''));
				
				lis.push('<li id="' + self.id + '_li_' + i + '" rel="' + value + (cn.length > 0 ? '" class="' + cn : '') + '">' + html + '</li>');
			});
			
			return lis.join("");
		}
		
		function repaint(){
			// make sure we re-init the menu width on first show
			bFixedWidth = false;
			// clear styles that will effect recalculating the width of the drop down
			$container[0].style.width = "";
			if( $title.length ){
				$title[0].style.width = "";
				$title.css("float", "");
			}
			
			// re-generate get the html making up all the new options and update the cache
			$ul.html(generateOptions($select.children('option')));
		}

		// set the currently selected item
		function setSelectedItem(value, doCallback){
			// look for the currently selected item
			var $selected = $ul.find("li[rel=" + value + "]");
			
			// if nothing is selected, get the first item
			if( $selected.length == 0 ) $selected = $ul.find("li:eq(0)");

			// return the selected item			
			return $selected.trigger("click.linkselect", [true, doCallback]);
		}

		// get the currently selected item
		function getSelectedItem(){
			// look for the currently selected item
			var $selected = $ul.find("li.selected");

			// if nothing is selected, get the first item
			if( $selected.length == 0 ) $selected = $ul.find("li:eq(0)");

			// return the selected item			
			return $selected;
		}
		
		// get the currently selected item
		function getHighlightedItem(){
			// look for the currently selected item
			var $selected = $ul.find("li.current");

			// if nothing is selected, get the first item
			if( $selected.length == 0 ) $selected = getSelectedItem();

			// return the selected item			
			return $selected;
		}
		
		function setHighlightedItem($el){
			// remove the currently selected item
			$container.find(".current").removeClass(options.classCurrent);
			// highlight the current row
			$el.addClass(options.classCurrent);
			return $el;
		}

		// move the selected item
		function moveSelect(step){
			// get the currently selected item
			var $current = getHighlightedItem();
			var pos = parseInt($current.attr("id").replace(/(.+)(_(\d+$))/gi, "$3"), 10);

			moveTo(pos + step);
		}

		// move the selected item
		function moveTo(pos){
			var $li = $("li", $container), $next;
			// if the options are empty, cancel request
			if( !$li || $li.length == 0 ) return false;
			
			// get the currently selected item
			var $current = getHighlightedItem().removeClass(options.classCurrent);

			// don't go lower than first item	
			if( isNaN(pos) || pos < 0 ){
				$next = $li.eq(0);
			// if greater than last position	
			} else if( pos > $li.length - 1 ){
				$next = $li.eq($li.length - 1);
			// get the first item
			} else {
				$next = $li.eq(pos);
			}
			
			// if the container is open
			if( $container.is(":visible") ){
				// change highlighted option
				$next.addClass(options.classCurrent);
				// make sure the item is in the viewable area
				scrollIntoView($next);
			} else {
				// only trigger the click if we've moved positions
				if( $current[0] !== $next[0] ) $next.trigger("click.linkselect");
			}
		}
		
		function moveFirst(){
			moveTo(0);
		}

		function moveLast(){
			moveTo($select.children('option').length-1);
		}

		function scrollIntoView($el, center){
			var el = $el[0];
			var scrollable = $scrollable[0];
			// get the padding which is need to adjust the scrollTop
			var s = {pTop: parseInt($scrollable.css("paddingTop"), 10)||0, pBottom: parseInt($scrollable.css("paddingBottom"), 10)||0, bTop: parseInt($scrollable.css("borderTopWidth"), 10)||0, bBottom: parseInt($scrollable.css("borderBottomWidth"), 10)||0};
	
			// scrolling down
			if( (el.offsetTop + el.offsetHeight) > (scrollable.scrollTop + scrollable.clientHeight) ){
				scrollable.scrollTop = $el.offset().top + (scrollable.scrollTop - $scrollable.offset().top) - ((scrollable.clientHeight/((center == true) ? 2 : 1)) - ($el.outerHeight() + s.pBottom));
			// scrolling up
			} else if( el.offsetTop - s.bTop - s.bBottom <= (scrollable.scrollTop + s.pTop + s.pBottom) ){
				scrollable.scrollTop = $el.offset().top + (scrollable.scrollTop - $scrollable.offset().top) - s.pTop;
			}
		}
	
		function toggleOptions(){
			if( $container.is(":visible") ) hideOptions();
			else showOptions();
		}
		
		var bFixedWidth = false;
		function showOptions(callback){
			// get the currently selected item
			var $selected = getSelectedItem();
			// add the a class to the anchor showing we're open
			$a.removeClass(options.classLinkFocus).addClass(options.classLinkOpen);
			// display the options
			$container.show();
			// fix the width of the container
			if( !bFixedWidth ){
				// get the width of the anchor link unless inline, then grab width of parent
				var aw = ($a.css("display").indexOf("inline") > -1) ? $a.parent().outerWidth() : $a.outerWidth();
				// get the original width of the container
				var width = options.fixedWidth ? aw : $container.width();
				// make sure the width of the drop down is at least as wide as the link
				if( width < aw ) width = aw;
				
				// get the list element
				var maxHeight = parseInt($scrollable.css("max-height"), 10);

				// if IE6, we need to apply max-width/max-height rules (which are not support in IE)
				if( $.browser.msie && $.browser.version <= 6 ){
					if( (maxHeight > 0) && ($scrollable.height() > maxHeight) ) $scrollable.height(maxHeight);
				}
				
				// if scrolling list, adjust the width for the scrollbars
				if( $ul.height() > maxHeight ) width += 25;

				// make sure the width of everything isn't greater than the max-width CSS property
				var maxWidth = parseInt("0" + $container.css("max-width"), 10);
				if( (maxWidth > 0) && (width > maxWidth) ) width = aw = maxWidth;
				
				// adjust the width of the dropdown
				$container.width(width);

				// webkit has a bug with detecting "max-width", so we must double check after setting the container width
				if( $.browser.safari ){
					var cw = $container.width();
					if( aw > cw ) width = aw = cw;
				}
				
				// make the title bar the width of the anchor
				$title.width(aw);

				// adjust the width for any padding in the title bar				
				if( $title.outerWidth() > aw ) $title.width(aw - ($title.outerWidth()-aw));

				// check to see if we should right-align the title
				if( options.titleAlign.toLowerCase() == "right" && !options.fixedWidth ) $title.css("float", "right");

				// mark that we've fixed the width
				bFixedWidth = true;
			}
			// anchor the options to the link
			anchorTo($a, $container, true);
			// if the bgIframe exists, use the plug-in
			if( !!$.fn.bgIframe ) $container.bgIframe();
			// scroll the selected item into view
			scrollIntoView($selected, true);
			// highlight the current option
			setHighlightedItem($selected);
			// run the open callback
			if( $.isFunction(options.open) ) options.open.apply(this, [$container, $a, $selected, $title]);
			// run any manual callback
			if( $.isFunction(callback) ) callback.apply(this, [$container, $a, $selected, $title]);
			is_open = true;
		}

		function hideOptions(nofocus){
			// tell the anchor we're no longer open
			if( nofocus !== true ) $a.addClass(options.classLinkFocus).removeClass(options.classLinkOpen);
			$container.hide();
			if( $.isFunction(options.close) ) options.close.apply(this, [$container, $a, getSelectedItem(), $title]);
			is_open = false;
		}

		function position($el){
			var bHidden = false;
			// if the element is hidden we must make it visible to the DOM to get
			if ($el.is(":hidden")) {
				bHidden = !!$el.css("visibility", "hidden").show();
			}
			
			// use offset to get the actual viewport position of the element (and not it's relative position)
			var pos = $.extend($el.offset(),{
				  width: $el.outerWidth()
				, height: $el.outerHeight()
				, marginLeft: parseInt($.curCSS($el[0], "marginLeft", true), 10) || 0
				, marginRight: parseInt($.curCSS($el[0], "marginRight", true), 10) || 0
				, marginTop: parseInt($.curCSS($el[0], "marginTop", true), 10) || 0
				, marginBottom: parseInt($.curCSS($el[0], "marginBottom", true), 10) || 0
			});
			
			if( pos.marginTop < 0 ) pos.top += pos.marginTop;
			if( pos.marginLeft < 0 ) pos.left += pos.marginLeft;
			
			pos["bottom"] = pos.top + pos.height;
			pos["right"] = pos.left + pos.width;
			
			// hide the element again
			if( bHidden ) $el.hide().css("visibility", "visible");
	
			return pos;
		}
		
		function anchorTo($anchor, $target){
			var pos = position($anchor);
			// get the screen dimensions
			var sd = getScreenDimensions();

			// get the far right edge of the option container
			var farRight = $container.outerWidth() + pos.left;
			
			// if we're going to be off the screen, adjust the position
			if( farRight > sd.x ){
				// adjust to the left by subtracting the list width and then adding the width of the title (which will cover up the anchor text)
				//pos.left = (pos.left - $container.outerWidth()) + $title.outerWidth();
			} else {
				// since the container isn't offscreen, see if we need to expand the width of the title bar
				var cow = $container.outerWidth(), tow = $title.outerWidth();
				// expand the title bar to the width of the container, subtracting the difference in padding
				if( cow > tow ) $title.width(cow - (tow - $title.width()));
			}

			$target.css({
				  position: "absolute"
				, top: pos[options.yAxis]
				, left: pos.left
			});
			
			return pos.bottom;
		}

		function getScreenDimensions(){
			var d = {
				  scrollLeft: $(window).scrollLeft()
				, scrollTop:  $(window).scrollTop()
				, width:      $("body").width()     // changed from innerWidth
				, height:     $("body").height()    // changed from innerHeight			
			};
			
			// calculate the correct x/y positions
			d.x = d.scrollLeft + d.width;
			d.y = d.scrollTop + d.height;
			
			return d;
		}
		
		function isJQueryEvent(e){
			return !("bubbles" in e || "cancelBubble" in e);
		}
		
		// invoke the init callback
		if( $.isFunction(options.init) ) options.init.apply(this, [$select, $input, $a, $container, $scrollable, $title, $ul]);
	};

	$.LinkSelect.defaults = {
		  classLink: "linkselectLink"             // the class to use for the anchor tag
		, classLinkOpen: "linkselectLinkOpen"     // the anchor's class when the dropdown is shown
		, classLinkFocus: "linkselectLinkFocus"   // the anchor's class when it has focus
		, classContainer: "linkselectContainer"   // the class to the container that holds the options
		, classSelected: "selected"               // the class of the currently selected item
		, classCurrent: "current"                 // the class of the highlighted item
		, classDisabled: "linkselectDisabled"     // the class used on the text when the linkselect is disabled
		, classValue: "linkselectValue"           // the class used for each item in the dropdown
		, yAxis: "top"                            // the position of the dropdown relative to the link (can be either "top" or "bottom")
		, titleAlign: "right"                     // location of dropdown's title bar if dropdown is on right edge of the screen (can be either "right" or "left")
		, fixedWidth: false                       // false = dropdown sizes to width of options, true = dropdown uses width of link
		, init: null                              // callback that occurs when a linkselect menu is initialized
		, change: null                            // callback that occurs when an option is selected
		, format: null                            // callback that occurs when rendering the HTML to use for an item in the dropdown
		, open: null                              // callback that occurs when the menu is opened
		, close: null                             // callback that occurs when the menu is closed
	};

})(jQuery);
