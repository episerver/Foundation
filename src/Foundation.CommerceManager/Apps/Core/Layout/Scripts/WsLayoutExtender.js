Type.registerNamespace("Ibn");

var layoutExtender_tools = null;
var layoutExtender_tools2 = null;
var onDeleteDelegate = null;
var onPropertyPage = null;
var onCollapseDelegate = null;

var __curentInstance = null;

Ibn.WsLayoutExtender = function(element) {
    Ibn.WsLayoutExtender.initializeBase(this, [element]);

    var jsonItems = null;

    //popup div
    var popupElement = null;
    var popupId = null;

    var propertyCommand = null;

    var addElementContainer = null;
    var addIds = null;
    var wsPageUid = null;
    var contextKey = null;

    //internal variables
    var mouseOffset = null;
    var iMouseDown = null;
    var lMouseState = null;
    var dragObject = null;
    var DragDrops = null;
    var curTarget = null;
    var lastTarget = null;
    var dragHelper = null;
    var tempDiv = null;
    var rootParent = null;
    var rootSibling = null;
    var fictiveNode = null;
    var scrollTimerId = null;
    var mousePosition = null;
    var itemsArray = null;
    
    var activeHeight = null;
    var activeWidth = null;
    
    var oldColumnIndex = null;
    var oldItemIndex = null;
    var newColumnIndex = null;
    var newItemIndex = null;

    var deleteMsg = null;

    // onDropEvent list
    var onDropEventList = null;

    var originalContainer = null;

    var viewport = null;

    //if true, debug alert will be shown
    var __debug = null;
}

Ibn.WsLayoutExtender.prototype =
{
    get_jsonItems: function() {
        return this.jsonItems;
    },
    set_jsonItems: function(value) {
        this.jsonItems = value;
    },
    get_deleteMsg: function() {
        return this.deleteMsg;
    },
    set_deleteMsg: function(value) {
        this.deleteMsg = value;
    },

    get_addElementContainer: function() {
        return this.addElementContainer;
    },
    set_addElementContainer: function(value) {
        this.addElementContainer = value;
    },

    get_wsPageUid: function() {
        return this.wsPageUid;
    },
    set_wsPageUid: function(value) {
        this.wsPageUid = value;
    },

    get_propertyCommand: function() {
        return this.propertyCommand;
    },
    set_propertyCommand: function(value) {
        this.propertyCommand = value;
    },

    get_popupElement: function() {
        return this.popupElement;
    },
    set_popupElement: function(value) {
        this.popupElement = value;
    },

    // ctor()
    initialize: function() {
        Ibn.WsLayoutExtender.callBaseMethod(this, 'initialize');
        this.__debug = false;

        onDeleteDelegate = Function.createDelegate(this, this.onClose);
        onPropertyPage = Function.createDelegate(this, this.onPropertyClick);
        onCollapseDelegate = Function.createDelegate(this, this.onCollapse);

        this.onDropEventList = new Sys.EventHandlerList();
        this.iMouseDown = false;
        this.lMouseState = false;
        this.DragDrops = [];

        this.dragHelper = document.createElement('DIV');
        this.dragHelper.style.cssText = 'position:fixed;display:none;';

        this.fictiveNode = document.createElement("div");
        this.fictiveNode.id = 'mcWsLayoutFictiveNode';
        this.fictiveNode.style.border = 'dashed 2px #B5D3FF';

        if (this._element)
            this._element.appendChild(this.dragHelper);
        //document.body.appendChild(this.dragHelper);

        document.onmousemove = Function.createDelegate(this, this.mouseMove);
        document.onmousedown = Function.createDelegate(this, this.mouseDown);
        document.onmouseup = Function.createDelegate(this, this.mouseUp);

        $addHandler(window, "resize", Function.createDelegate(this, this.onWindowResize))

        //attach to endPageRequest
        this.initLayout();
        __curentInstance = this;
    },
    dispose: function() {
        this.jsonItems = null;
        if (this.endPageRequestHandler && Sys.WebForms.PageRequestManager.getInstance())
            Sys.WebForms.PageRequestManager.getInstance().remove_endRequest(this.endPageRequestHandler)

        Ibn.WsLayoutExtender.callBaseMethod(this, 'dispose');
    },

    onWindowResize: function(e) {
        if (this._element && document.body.offsetHeight - this._element.offsetTop > 0) {
            this._element.style.height = document.body.offsetHeight - this._element.offsetTop + 'px';
            this._element.style.overflow = 'auto';

            if (document.all)//resize width for ie
            {
                for (var i = 0; i < this.itemsArray.length; i++) {
                    var innerColumnObj = document.getElementById(this.itemsArray[i].clientId);
                    if (innerColumnObj) {
                        innerColumnObj.style.width = this.itemsArray[i].columnWidth * 100 + '%';
                    }
                }
                for (var i = 0; i < this.itemsArray.length; i++) {
                    var innerColumnObj = document.getElementById(this.itemsArray[i].clientId);
                    if (innerColumnObj) {
                        innerColumnObj.style.width = innerColumnObj.offsetWidth - 12 + 'px';
                    }
                }
            }
        }
    },

    attachEventToButton: function(obj, iCol, iItem, iButton) {
        if (obj) {
            var btnObj = document.getElementById(obj.ClientId);
            if (obj.ButtonType == "close") {
                var closeHandler = Function.createDelegate(this, this.onClose);
                $addHandler(btnObj, "click", function() { closeHandler(obj.ClientId, iCol, iItem, iButton); }); //
            }

            if (obj.ButtonType == "expand") {
                var expandHandler = Function.createDelegate(this, this.onCollapse);
                $addHandler(btnObj, "click", function() { expandHandler(obj.ClientId, iCol, iItem, iButton); }); //
            }

            if (obj.ButtonType == "property") {
                var propertyHandler = Function.createDelegate(this, this.onPropertyClick);
                var internalId = this.itemsArray[iCol].items[iItem].id;
                $addHandler(btnObj, "click", function() { propertyHandler(internalId); }); //
            }
        }
    },


    attachHandlers: function(clearHandlers) {
        for (var i = 0; i < this.itemsArray.length; i++) {

            // if there are no items in the column, don't do anything in this step
            if (!this.itemsArray[i].items)
                continue;

            //attach events:
            for (var j = 0; j < this.itemsArray[i].items.length; j++) // loop for each item in column
            {
                if (typeof (this.itemsArray[i].items[j]) === 'undefined')
                    continue;
                //                if (this.itemsArray[i].items[j].collapsed === true) {
                //                    var curObj = document.getElementById(this.itemsArray[i].items[j].contentEl);
                //                    curObj.style.display = 'none';
                //                }
                if (this.itemsArray[i].items[j].buttons) {
                    for (var k = 0; k < this.itemsArray[i].items[j].buttons.length; k++) // loop for each button in item
                    {
                        if (clearHandlers === false) {
                            this.attachEventToButton(this.itemsArray[i].items[j].buttons[k], i, j, k);
                            //                            if (this.itemsArray[i].items[j].buttons[k].ButtonType == "expand" && this.itemsArray[i].items[j].collapsed == true) 
                            //                            {
                            //                                var btnObj = document.getElementById(this.itemsArray[i].items[j].buttons[k].ClientId);
                            //                                var tmpUrl = btnObj.getAttribute("changeUrl");
                            //                                btnObj.setAttribute("changeUrl", btnObj.src);
                            //                                btnObj.src = tmpUrl;
                            //                            }
                        }
                        else {
                            $clearHandlers(document.getElementById(this.itemsArray[i].items[j].buttons[k].ClientId));
                        }
                    }
                }
            }
        }
    },

    initLayout: function() {
        this.jsonItems = "[" + this.jsonItems + "]";
        var innerArray = new Array();
        var buttonsArray = new Array();
        this.itemsArray = Sys.Serialization.JavaScriptSerializer.deserialize(this.jsonItems);

        for (var i = 0; i < this.itemsArray.length; i++) {
            var innerColumnObj = document.getElementById(this.itemsArray[i].clientId);
            if (innerColumnObj) {
                innerArray.push(innerColumnObj);

                if (document.all)
                    innerColumnObj.style.width = innerColumnObj.offsetWidth - 12 + 'px';

                innerColumnObj.style.height = innerColumnObj.offsetHeight + 'px';
            }
        }

        if (this.addElementContainer && $get(this.addElementContainer)) {
            this.addIds = new Array();
            for (var i = 0; i < this.itemsArray.length; i++) {
                var tmpNode = $get(this.addElementContainer).cloneNode(true);

                tmpNode.style.display = 'block';
                tmpNode.id = this.addElementContainer + "_" + i;

                if (tmpNode.getAttribute('onclick_toExecute')) {
                    var clickHandlerScript = tmpNode.getAttribute('onclick_toExecute');
                    clickHandlerScript = clickHandlerScript.replace('%columnId%', this.itemsArray[i].id);
                    clickHandlerScript = clickHandlerScript.replace('%pageUid%', this.wsPageUid);
                    tmpNode.setAttribute('onclick_toExecute', clickHandlerScript);
                    $addHandler(tmpNode, "click", function() { eval(this.getAttribute('onclick_toExecute')); });
                }

                document.getElementById(this.itemsArray[i].clientId).appendChild(tmpNode);
                this.addIds.push(tmpNode.id);
            }
        }

        this.attachHandlers(false);
        //todo:
        this.createDragContainer(innerArray);
        this.onWindowResize(null);
    },

    createDragContainer: function() {
        /*
        Create a new "Container Instance" so that items from one "Set" can not
        be dragged into items from another "Set"
        */
        var cDrag = this.DragDrops.length;
        this.DragDrops[cDrag] = [];
        if (arguments.length > 0) {
            for (var i = 0; i < arguments[0].length; i++) {
                var cObj = arguments[0][i];
                //                var innerArray = cObj.getElementsByTagName("DIV");
                //                for (var j = 0; j < innerArray.length; j++) {
                //                    if (innerArray[j].style.position == 'relative')
                //                        innerArray[j].style.position = 'static';
                //                }

                this.DragDrops[cDrag].push(cObj);
                cObj.setAttribute('DropObj', cDrag);

                var innerObj = null;
                for (var k = 0; k < cObj.childNodes.length; k++) {
                    innerObj = cObj.childNodes[k];
                    if (innerObj.nodeType == 1)
                        break;
                }
                cObj = innerObj;
                if (cObj == null)
                    return;
            }
        }
    },

    mouseCoords: function(ev) {
        if (ev.pageX || ev.pageY) {
            return { x: ev.pageX, y: ev.pageY };
        }
        if (this._element)
            return {
                x: ev.clientX + this._element.scrollLeft - document.body.clientLeft,
                y: ev.clientY + this._element.scrollTop - document.body.clientTop
            };
        else
            return { x: 0, y: 0 };
    },

    getMouseOffset: function(target, ev) {
        ev = ev || window.event;

        var docPos = this.getPosition(target);
        var mousePos = this.mouseCoords(ev);
        return { x: mousePos.x - docPos.x, y: mousePos.y - docPos.y };
    },

    getPosition: function(e) {
        return { x: e.offsetLeft, y: e.offsetTop };
        var left = 0;
        var top = 0;

        while (e.offsetParent) {
            left += e.offsetLeft;
            top += e.offsetTop;
            e = e.offsetParent;
        }

        left += e.offsetLeft;
        top += e.offsetTop;
        //window.status += 'position: ' + left + 'x' + top;
        return { x: left, y: top };
    },

    performScroll: function() {
        //window.status += ' scrolling ' + this.mousePosition.y;
        //alert('scrolling detected');
        if (typeof (this._element) == 'undefined')
            return;
        if (this.mousePosition.y < 20 && this._element.scrollTop > 0) {
            this._element.scrollTop -= 10;
            //this.recalculateAttr(this.DragDrops[0]);
        } else if (this.mousePosition.y > this._element.offsetHeight - 15 && this._element.scrollTop < this._element.scrollHeight - this._element.clientHeight) {
            this._element.scrollTop += 10;
            //this.recalculateAttr(this.DragDrops[0]);
        }
    },

    checkScroll: function() {
        this.performScroll();
    },

    mouseMove: function(ev) {
        ev = ev || window.event;

        /*
        We are setting target to whatever item the mouse is currently on

	    Firefox uses event.target here, MSIE uses event.srcElement
        */
        var target = ev.target || ev.srcElement;
        var mousePos = this.mouseCoords(ev);
        var activeWidth = 0;
        var activeHeight = 0;
        this.mousePosition = mousePos;
        //test
        //window.status = mousePos.x + ' _ ' + mousePos.y;

        // mouseOut event - fires if the item the mouse is on has changed
        if (this.lastTarget && (target !== this.lastTarget)) {
            // reset the classname for the target element
            var origClass = this.lastTarget.getAttribute('origClass');
            if (origClass) this.lastTarget.className = origClass;
        }

        /*
        dragObj is the grouping our item is in (set from the createDragContainer function).
        if the item is not in a grouping we ignore it since it can't be dragged with this
        script.
        */

        if (typeof (target.parentNode) == 'undefined ' || typeof (target.parentNode.getAttribute) == 'undefined')
            return;

        var dragObj = target.parentNode.getAttribute('DragObj');

        // if the mouse was moved over an element that is draggable
        if (dragObj != null) {

            // mouseOver event - Change the item's class if necessary
            //            if (target != this.lastTarget) {
            //                var oClass = target.getAttribute('overClass');
            //                if (oClass) {
            //                    target.setAttribute('origClass', target.className);
            //                    target.className = oClass;
            //                }
            //            }

            // if the user is just starting to drag the element
            if (this.iMouseDown && !this.lMouseState) {

                // mouseDown target
                this.curTarget = target.parentNode;
                this.activeHeight = this.curTarget.parentNode.offsetHeight;
                this.activeWidth = this.curTarget.parentNode.offsetWidth;
                //window.status += ' start drag';

                //TO DO: hide add buttons
                for (var i = 0; i < this.addIds.length; i++) {
                    var _obj = $get(this.addIds[i]);
                    if (_obj)
                        _obj.style.display = 'none';
                }

                // Record the mouse x and y offset for the element
                this.rootParent = this.curTarget.parentNode;
                this.rootSibling = this.curTarget.nextSibling;

                this.mouseOffset = this.getMouseOffset(target, ev);

                //window.status += 'length = ' + this.dragHelper.childNodes.length;
                // We remove anything that is in our dragHelper DIV so we can put a new item in it.
                for (var i = 0; i < this.dragHelper.childNodes.length; i++) this.dragHelper.removeChild(this.dragHelper.childNodes[i]);

                // Make a copy of the current item and put it in our drag helper.
                this.dragHelper.appendChild(this.curTarget.parentNode.cloneNode(true));
                this.dragHelper.style.display = 'block';
                this.dragHelper.style.zIndex = 1000;

                // set the class on our helper DIV if necessary
                var dragClass = this.curTarget.getAttribute('dragClass');
                if (dragClass) {
                    this.dragHelper/*.firstChild*/.className = dragClass;
                    this.dragHelper.firstChild.className = dragClass;
                }

                // disable dragging from our helper DIV (it's already being dragged)
                this.dragHelper/*.firstChild*/.removeAttribute('DragObj');
                this.dragHelper.firstChild.removeAttribute('DragObj');

                /*
                Record the current position of all drag/drop targets related
                to the element.  We do this here so that we do not have to do
                it on the general mouse move event which fires when the mouse
                moves even 1 pixel.  If we don't do this here the script
                would run much slower.
                */
                var dragConts = this.DragDrops[dragObj];

                /*
                first record the width/height of our drag item.  Then hide it since
                it is going to (potentially) be moved out of its parent.
                */
                this.curTarget.setAttribute('startWidth', parseInt(this.curTarget.offsetWidth));
                this.curTarget.setAttribute('startHeight', parseInt(this.curTarget.offsetHeight));
                this.curTarget.parentNode.style.display = 'none';
                this.recalculateAttr(dragConts);

            }
        }

        // If we get in here we are dragging something
        if (this.curTarget) {
            // move our helper div to wherever the mouse is (adjusted by mouseOffset)

            if (document.all && this._element) {
                mousePos.y -= this._element.scrollTop;
                //this.dragHelper.style.top = mousePos.y - this._element.scrollTop + 'px';
            }
            this.dragHelper.style.top = mousePos.y /*- this.mouseOffset.y */ + 'px';
            this.dragHelper.style.left = mousePos.x/* - this.mouseOffset.x */ + 'px';

            //            window.status += 'coordx=' + parseInt(mousePos.x) + ' - ' + this.mouseOffset.x;
            //            window.status += 'coordy=' + parseInt(mousePos.y) + ' - ' + this.mouseOffset.y;
            if (this.scrollTimerId == null || typeof (this.scrollTimerId) == 'undefined') {
                //window.status += '[set timer]';
                var _delegate = Function.createDelegate(this, this.checkScroll);
                this.scrollTimerId = window.setInterval(_delegate, 50);
            }

            var dragConts = this.DragDrops[this.curTarget.getAttribute('DragObj')];
            var activeCont = null;

            var xPos = mousePos.x; // - this.mouseOffset.x + (parseInt(this.curTarget.getAttribute('startWidth')) / 2);
            var yPos = 0;
            if (this._element) {
                yPos = mousePos.y /*+ this.mouseOffset.y*/ + this._element.scrollTop; // + (parseInt(this.curTarget.getAttribute('startHeight')) / 2);
                //window.status += ' continue dragging(' + xPos + ' x ' + yPos + '[' + this._element.scrollTop + '])';
            }
            // check each drop container to see if our target object is "inside" the container
            for (var i = 0; i < dragConts.length; i++) {
                with (dragConts[i]) {
                    if (((parseInt(getAttribute('startLeft'))) < xPos) &&
					    ((parseInt(getAttribute('startTop'))) < yPos) &&
					    ((parseInt(getAttribute('startLeft')) + parseInt(getAttribute('startWidth'))) > xPos) &&
					    ((parseInt(getAttribute('startTop')) + parseInt(getAttribute('startHeight'))) > yPos)) {

                        /*
                        our target is inside of our container so save the container into
                        the activeCont variable and then exit the loop since we no longer
                        need to check the rest of the containers
                        */
                        activeCont = dragConts[i];
                        //window.status += ' [ active dragging ' + i + ' ' + this.activeWidth + 'x' + this.activeHeight + ' ] ';
                        //this.curTarget.parentNode.style.display = 'block';
                        this.dragHelper.style.width = this.activeWidth - 16 + 'px';
                        this.fictiveNode.style.width = this.activeWidth - 16 + 'px';
                        this.fictiveNode.style.height = this.activeHeight + 'px';
                        this.newColumnIndex = i;
                        //this.curTarget.parentNode.style.display = 'none';
                        // exit the for loop
                        break;
                    }
                }
            }

            // Our target object is in one of our containers.  Check to see where our div belongs
            if (activeCont) {
                // beforeNode will hold the first node AFTER where our div belongs
                var beforeNode = null;

                // loop through each child node (skipping text nodes).
                for (var i = activeCont.childNodes.length - 1; i >= 0; i--) {
                    textNodeCount = 0;
                    with (activeCont.childNodes[i]) {
                        if (activeCont.childNodes[i].nodeType != 1)
                            textNodeCount++;

                        if (nodeName == '#text') continue;

                        // if the current item is "After" the item being dragged
                        if (this._element &&
						    this.curTarget != activeCont.childNodes[i] &&
                        /*((parseInt(getAttribute('startLeft')) + parseInt(getAttribute('startWidth'))) > xPos) &&*/
						    ((parseInt(getAttribute('startTop')) + parseInt(getAttribute('startHeight'))) /*+ this._element.scrollTop*/ > yPos)) {
                            beforeNode = activeCont.childNodes[i];

                            if (!document.all)
                                this.newItemIndex = i - textNodeCount - 2;
                            else
                                this.newItemIndex = i - textNodeCount - 1;

                            if (this.newItemIndex < 0)
                                this.newItemIndex = 0;

                            if (this.newItemIndex > this.oldItemIndex)
                                this.newItemIndex--;
                            //window.status += ' [ 1 ] ';
                        }
                    }
                }

                // the item being dragged belongs before another item
                if (beforeNode) {
                    //                    if (beforeNode != this.curTarget.parentNode.nextSibling) {
                    if (document.getElementById('mcWsLayoutFictiveNode')) {
                        document.getElementById('mcWsLayoutFictiveNode').parentNode.removeChild(document.getElementById('mcWsLayoutFictiveNode'));
                        //activeCont.removeChild(document.getElementById('mcWsLayoutFictiveNode'));
                        activeCont.insertBefore(this.fictiveNode/*this.curTarget.parentNode*/, beforeNode);
                    }
                    else {
                        activeCont.insertBefore(this.fictiveNode/*this.curTarget.parentNode*/, beforeNode);
                    }
                    //                        window.status += ' [ 2 ] ';
                    //                        //activeCont.insertBefore(this.curTarget.parentNode, beforeNode);
                    //                    }
                    //                    else {
                    //                        window.status += ' [ !2! ] ';
                    //                    }

                    // the item being dragged belongs at the end of the current container
                } else {
                    //                    if ((this.curTarget.parentNode.nextSibling) || (this.curTarget.parentNode.parentNode != activeCont)) {
                    if (document.getElementById('mcWsLayoutFictiveNode')) {
                        document.getElementById('mcWsLayoutFictiveNode').parentNode.removeChild(document.getElementById('mcWsLayoutFictiveNode'));
                        activeCont.appendChild(this.fictiveNode/*this.curTarget.parentNode*/);
                    } else {
                        activeCont.insertBefore(this.fictiveNode/*this.curTarget.parentNode*/, beforeNode);
                    }
                    //                        activeCont.appendChild(this.curTarget.parentNode);
                    //window.status += ' [ 3 ] ';

                    //                    } else {
                    //                        window.status += ' [ !3! ] ';
                    //                    }
                }

                // make our drag item visible
                //                if (this.curTarget.parentNode.style.display != '') {
                //                    this.curTarget.parentNode.style.display = '';
                //                }
            } else {

                // our drag item is not in a container, so hide it.
                if (this.curTarget.parentNode.style.display != 'none') {
                    this.curTarget.parentNode.style.display = 'none';
                }
            }
        }

        // track the current mouse state so we can compare against it next time
        this.lMouseState = this.iMouseDown;

        // mouseMove target
        this.lastTarget = target;

        // track the current mouse state so we can compare against it next time
        this.lMouseState = this.iMouseDown;

        // this helps prevent items on the page from being highlighted while dragging
        return false;
    },


    mouseUp: function(ev) {
        if (this.curTarget) {

            if (this.scrollTimerId) {
                window.clearInterval(this.scrollTimerId);
                this.scrollTimerId = null;
            }
            // hide our helper object - it is no longer needed
            this.dragHelper.style.display = 'none';

            // if the drag item is invisible put it back where it was before moving it
            if (this.curTarget.parentNode.style.display == 'none') {
                if (this.rootSibling) {
                    this.rootParent.insertBefore(this.curTarget, this.rootSibling);
                } else {
                    this.rootParent.appendChild(this.curTarget);
                }
            }

            var fictiveNode = document.getElementById('mcWsLayoutFictiveNode');
            if (fictiveNode) {
                if (fictiveNode.nextSibling == null)
                    this.newItemIndex++;
                fictiveNode.parentNode.insertBefore(this.curTarget.parentNode, fictiveNode);
                fictiveNode.parentNode.removeChild(fictiveNode);
            }


            //window.status = 'start: ' + this.oldColumnIndex + 'x' + this.oldItemIndex + ' -> ' + this.newColumnIndex + 'x' + this.newItemIndex;
            //alert('start: ' + this.oldColumnIndex + 'x' + this.oldItemIndex + ' -> ' + this.newColumnIndex + 'x' + this.newItemIndex);
            //Change place of elements in InternalModel(JSON)
            if (this.newItemIndex >= this.itemsArray[this.newColumnIndex].items.length)
                this.newItemIndex = this.itemsArray[this.newColumnIndex].items.length - 1;

            if (this.oldColumnIndex == this.newColumnIndex) {
                var tmpObj = this.itemsArray[this.oldColumnIndex].items[this.oldItemIndex];
                //                this.itemsArray[this.oldColumnIndex].items[this.oldItemIndex] = this.itemsArray[this.newColumnIndex].items[this.newItemIndex];
                //                this.itemsArray[this.newColumnIndex].items[this.newItemIndex] = tmpObj;
                if (this.newItemIndex != 0)//this.newItemIndex < this.itemsArray[this.newColumnIndex].items.length - 1 && 
                    this.newItemIndex++;
                Array.insert(this.itemsArray[this.newColumnIndex].items, this.newItemIndex, this.itemsArray[this.oldColumnIndex].items[this.oldItemIndex]);
                if (this.newItemIndex < this.oldItemIndex)
                    this.oldItemIndex--;
                Array.removeAt(this.itemsArray[this.newColumnIndex].items, this.oldItemIndex);
            }
            else {
                Array.insert(this.itemsArray[this.newColumnIndex].items, this.newItemIndex, this.itemsArray[this.oldColumnIndex].items[this.oldItemIndex]);
                this.itemsArray[this.oldColumnIndex].items.splice(this.oldItemIndex, 1);
            }

            this.attachHandlers(true);
            this.attachHandlers(false);

            Mediachase.Commerce.Manager.WebServices.LayoutCustomizationService.ChangePosition(this.getCurrentJson(), this.contextKey, null, Function.createDelegate(this, this.onRequestFailed));

            // make sure the drag item is visible
            this.curTarget.parentNode.style.display = '';
            this.curTarget.style.display = '';
        }

        //TO DO: show add buttons
        for (var i = 0; i < this.addIds.length; i++) {
            var _obj = $get(this.addIds[i]);
            if (_obj)
                _obj.style.display = 'block';
        }

        this.curTarget = null;
        this.iMouseDown = false;
        this.onDrop();
    },


    mouseDown: function() {
        this.iMouseDown = true;
        return true;

//        if (this.lastTarget) {
//            return false;
//        }
    },

    recalculateAttr: function(dragConts) {
        // loop through each possible drop container
        for (var i = 0; i < dragConts.length; i++) {
            with (dragConts[i]) {
                var pos = this.getPosition(dragConts[i]);

                /*
                save the width, height and position of each container.

					    Even though we are saving the width and height of each
                container back to the container this is much faster because
                we are saving the number and do not have to run through
                any calculations again.  Also, offsetHeight and offsetWidth
                are both fairly slow.  You would never normally notice any
                performance hit from these two functions but our code is
                going to be running hundreds of times each second so every
                little bit helps!

					    Note that the biggest performance gain here, by far, comes
                from not having to run through the getPosition function
                hundreds of times.
                */
                setAttribute('startWidth', parseInt(offsetWidth));
                setAttribute('startHeight', parseInt(offsetHeight));
                setAttribute('startLeft', pos.x);
                setAttribute('startTop', pos.y);
                setAttribute('unselectable', 'on');
            } //with
            var textNodeCount = 0;
            // loop through each child element of each container
            for (var j = 0; j < dragConts[i].childNodes.length; j++) {
                with (dragConts[i].childNodes[j]) {

                    if (dragConts[i].childNodes[j].nodeType != 1)
                        textNodeCount++;
                    if (dragConts[i].childNodes[j].firstChild == this.curTarget) {
                        this.oldColumnIndex = i;
                        this.oldItemIndex = j - textNodeCount;
                        textNodeCount = 0
                    }
                    if ((nodeName == '#text') || (dragConts[i].childNodes[j] == this.curTarget)) continue;

                    var pos = this.getPosition(dragConts[i].childNodes[j]);

                    // save the width, height and position of each element
                    setAttribute('startWidth', parseInt(offsetWidth));
                    setAttribute('startHeight', parseInt(offsetHeight));
                    setAttribute('startLeft', pos.x);
                    setAttribute('startTop', pos.y);
                    if (dragConts[i].childNodes[j].getAttribute('onclick_toexecute')) {
                        setAttribute('startTop', 9999);
                    }
                }
            } //for
        } //for    
    },

    onClose: function(clientId, iCol, iItem, iButton) {
        //delete block
        if (this.deleteMsg != '' && confirm(this.deleteMsg) || (this.deleteMsg == '' || this.deleteMsg == null)) {
            var btnObj = document.getElementById(clientId);
            var bodyObj = null;
            if (btnObj)
                bodyObj = btnObj.parentNode.parentNode;

            if (bodyObj && bodyObj.parentNode)
                bodyObj.parentNode.removeChild(bodyObj);

            Array.removeAt(this.itemsArray[iCol].items, iItem);

            Mediachase.Commerce.Manager.WebServices.LayoutCustomizationService.Delete(this.getCurrentJson(), this.contextKey, null, Function.createDelegate(this, this.onRequestFailed));
        }
        return false;
    },

    onPropertyClick: function(id) {
        if (this.propertyCommand) {
            var scriptToExecute = this.propertyCommand.replace('%controlUid%', id);
            eval(scriptToExecute);
        }
    },

    onCollapse: function(clientId, iCol, iItem, iButton) {
        //alert('expand/collapse ' + clientId);
        //alert(iCol + '_' + iItem + '_' + iButton);
        var btnObj = document.getElementById(clientId);
        var bodyObj = null;
        if (btnObj)
            bodyObj = btnObj.parentNode.nextSibling;

        if (this.itemsArray[iCol].items[iItem].collapsed === false) {
            bodyObj.style.display = 'none';
        }
        else {
            bodyObj.style.display = 'block';
        }
        this.itemsArray[iCol].items[iItem].collapsed = !this.itemsArray[iCol].items[iItem].collapsed;


        var tmpUrl = btnObj.getAttribute("changeUrl");
        btnObj.setAttribute("changeUrl", btnObj.src);
        btnObj.src = tmpUrl;
        // run web service
        Mediachase.Commerce.Manager.WebServices.LayoutCustomizationService.ChangePosition(this.getCurrentJson(), this.contextKey, null, Function.createDelegate(this, this.onRequestFailed));
        return false;
    },

    onDrop: function() {
    },

    onBeforeDragOver: function(source) {
        //TO DO: hide add buttons
        for (var i = 0; i < this.addIds.length; i++) {
            var _obj = $get(this.addIds[i]);
            if (_obj)
                _obj.style.display = 'none';
        }
    },
    /* peredelat' */
    getCurrentJson: function() {
        var retVal = '';
        var _tmp = '';
        var storeObj = new Array();

        for (var i = 0; i < this.itemsArray.length; i++) {
            storeObj[storeObj.length] = {};
            storeObj[storeObj.length - 1].items = new Array();
            storeObj[storeObj.length - 1].id = this.itemsArray[i].id;

            if (!this.itemsArray[i].items)
                continue;

            for (var j = 0; j < this.itemsArray[i].items.length; j++) {
                _tmp += i + '_' + j + ' -> ' + this.itemsArray[i].items[j] + '\r\n';
                if (typeof (this.itemsArray[i].items[j]) === 'undefined')
                    continue;
                var newItem = {};

                newItem.id = this.itemsArray[i].items[j].id.split('_')[0];
                newItem.collapsed = this.itemsArray[i].items[j].collapsed;
                if (this.itemsArray[i].items[j].id.split('_').length > 1)
                    newItem.instanceUid = this.itemsArray[i].items[j].id.split('_')[1];

                storeObj[storeObj.length - 1].items[storeObj[storeObj.length - 1].items.length] = newItem;
            }
        }
        retVal = Sys.Serialization.JavaScriptSerializer.serialize(storeObj);
        //alert(_tmp);
        //alert(retVal);
        return retVal;
    },

    // ----- onDrop Event ------
    onExpandEvent: function(blockNode) {
        var handler = this.onDropEventList.getHandler("mc_ddLayout_onexpand");
        if (handler) {
            handler(this, blockNode);
        }
    },
    add_expand: function(handler) {
        if (this.onDropEventList)
            this.onDropEventList.addHandler("mc_ddLayout_onexpand", handler);
    },
    remove_expand: function(handler) {
        if (this.onDropEventList != null)
            this.onDropEventList.removeHandler("mc_ddLayout_onexpand", handler);
    },

    // ----- onDrop Event ------
    onDropEvent: function(blockNode) {
        var handler = this.onDropEventList.getHandler("mc_ddLayout_ondrop");
        if (handler) {
            handler(this, blockNode);
        }
    },
    add_drop: function(handler) {
        if (this.onDropEventList)
            this.onDropEventList.addHandler("mc_ddLayout_ondrop", handler);
    },
    remove_drop: function(handler) {
        if (this.onDropEventList != null)
            this.onDropEventList.removeHandler("mc_ddLayout_ondrop", handler);
    },

    onRequestFailed: function(error) {
        //TO DO:		
    },

    debugMsg: function(str) {
        if (this.__debug)
            alert(str);
    }
}

function scrollbarWidth() {
    var inner = document.createElement('p');
    inner.style.width = '100%';
    inner.style.height = '200px';

    var outer = document.createElement('div');
    outer.style.position = 'absolute';
    outer.style.top = '-200px';
    outer.style.left = '-300px';
    outer.style.visibility = 'hidden';
    outer.style.width = '200px';
    outer.style.height = '150px';
    outer.style.overflow = 'hidden';
    outer.appendChild (inner);

    document.body.appendChild (outer);
    var w1 = inner.offsetWidth;
    outer.style.overflow = 'scroll';
    var w2 = inner.offsetWidth;
    if (w1 == w2) w2 = outer.clientWidth;

    document.body.removeChild(outer);

    return (w1 - w2);
}

function dashboardGetRefresh(params) {
    //window.location.href = window.location.href;
    window.location.reload(true);
}

function dashboardGetRefreshLight(params) {
    if (window.location.href.indexOf('testLocation') == -1) {
        if (window.location.href.indexOf('?') == -1)
            window.location.href = window.location.href + '?testLocation=0';
        else
            window.location.href = window.location.href + '&testLocation=0';
    }
    else {
        var newPath = window.location.href.replace('&testLocation=0', '');
        newPath = window.location.href.replace('?testLocation=0', '');
        window.location.href = newPath;
    }
    //window.location.href = window.location.href;
    //window.location.reload(true);
}

Ibn.WsLayoutExtender.getCurrent = function() {
    return __curentInstance;
}

Ibn.WsLayoutExtender.registerClass("Ibn.WsLayoutExtender", Sys.UI.Control);
if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();