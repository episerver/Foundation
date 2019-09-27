
Type.registerNamespace("Mediachase");

Mediachase.ListToListSelector = function () {
    ///<summary>
    ///ListToList is resposible for selecting several items by 
    /// moving them from one list box to other one.
    ///</summary>
    //control components ids
    Mediachase.ListToListSelector.initializeBase(this);
    this._sourceListId = "";
    this._targetListId = "";
    this._itemUpButtonId = "";
    this._itemDownButtonId = "";
    this._oneItemToTargetButtonId = "";
    this._oneItemToSourceButtonId = "";
    this._allItemsToTargetButtonId = "";
    this._allItemsToSourceButtonId = "";
    this._itemValuesSeparator = ",";
    this._sourceItemsHiddenId = "";
    this._targetItemsHiddenId = "";
    this._selectAllSourceItemsButtonId = "";
    this._selectAllTargetItemsButtonId = "";
    //control components
    this._sourceList = null;
    this._targetList = null;
    this._itemUpButton = null;
    this._itemDownButton = null;
    this._oneItemToTargetButton = null;
    this._oneItemToSourceButton = null;
    this._allItemsToTargetButton = null;
    this._allItemsToSourceButton = null;
    this._sourceItemsHidden = null;
    this._targetItemsHidden = null;
    this._selectAllSourceItemsButton = null;
    this._selectAllTargetItemsButton = null;
}

Mediachase.ListToListSelector.prototype =
{
    //getters and setters for properties
    get_SourceListId: function () {
        return this._sourceListId;
    },
    set_SourceListId: function (value) {
        this._sourceListId = value;
    },
    get_SourceList: function () {
        return this._sourceList;
    },
    set_SourceList: function (value) {
        this._sourceList = value;
    },
    get_TargetListId: function () {
        return this._targetListId;
    },
    set_TargetListId: function (value) {
        this._targetListId = value;
    },
    get_TargetList: function () {
        return this._targetList;
    },
    set_TargetList: function (value) {
        this._targetList = value;
    },
    get_ItemUpButtonId: function () {
        return this._itemUpButtonId;
    },
    set_ItemUpButtonId: function (value) {
        this._itemUpButtonId = value;
    },
    get_ItemUpButton: function () {
        return this._itemUpButton;
    },
    set_ItemUpButton: function (value) {
        this._itemUpButton = value;
    },
    get_ItemDownButtonId: function () {
        return this._itemDownButtonId;
    },
    set_ItemDownButtonId: function (value) {
        this._itemDownButtonId = value;
    },
    get_ItemDownButton: function () {
        return this._itemDownButton;
    },
    set_ItemDownButton: function (value) {
        this._itemDownButton = value;
    },
    get_OneItemToTargetButtonId: function () {
        return this._oneItemToTargetButtonId;
    },
    set_OneItemToTargetButtonId: function (value) {
        this._oneItemToTargetButtonId = value;
    },
    get_OneItemToTargetButton: function () {
        return this._oneItemToTargetButton;
    },
    set_OneItemToTargetButton: function (value) {
        this._oneItemToTargetButton = value;
    },
    get_OneItemToSourceButtonId: function () {
        return this._oneItemToSourceButtonId;
    },
    set_OneItemToSourceButtonId: function (value) {
        this._oneItemToSourceButtonId = value;
    },
    get_OneItemToSourceButton: function () {
        return this._oneItemToSourceButton;
    },
    set_OneItemToSourceButton: function (value) {
        this._oneItemToSourceButton = value;
    },
    get_AllItemsToTargetButtonId: function () {
        return this._allItemsToTargetButtonId;
    },
    set_AllItemsToTargetButtonId: function (value) {
        this._allItemsToTargetButtonId = value;
    },
    get_AllItemsToTargetButton: function () {
        return this._allItemsToTargetButton;
    },
    set_AllItemsToTargetButton: function (value) {
        this._allItemsToTargetButton = value;
    },
    get_AllItemsToSourceButtonId: function () {
        return this._allItemsToSourceButtonId;
    },
    set_AllItemsToSourceButtonId: function (value) {
        this._allItemsToSourceButtonId = value;
    },
    get_AllItemsToSourceButton: function () {
        return this._allItemsToSourceButton;
    },
    set_AllItemsToSourceButton: function (value) {
        this._allItemsToSourceButton = value;
    },
    get_ItemValuesSeparator: function () {
        return this._itemValuesSeparator;
    },
    set_ItemValuesSeparator: function (value) {
        this._itemValuesSeparator = value;
    },
    get_SourceItemsHiddenId: function () {
        return this._sourceItemsHiddenId;
    },
    set_SourceItemsHiddenId: function (value) {
        this._sourceItemsHiddenId = value;
    },
    get_SourceItemsHidden: function () {
        return this._sourceItemsHidden;
    },
    set_SourceItemsHidden: function (value) {
        this._sourceItemsHidden = value;
    },
    get_TargetItemsHiddenId: function () {
        return this._targetItemsHiddenId;
    },
    set_TargetItemsHiddenId: function (value) {
        this._targetItemsHiddenId = value;
    },
    get_SelectAllSourceItemsButtonId: function () {
        return this._selectAllSourceItemsButtonId;
    },
    set_SelectAllSourceItemsButtonId: function (value) {
        this._selectAllSourceItemsButtonId = value;
    },
    get_SelectAllTargetItemsButtonId: function () {
        return this._selectAllTargetItemsButtonId;
    },
    set_SelectAllTargetItemsButtonId: function (value) {
        this._selectAllTargetItemsButtonId = value;
    },
    get_TargetItemsHidden: function () {
        return this._targetItemsHidden;
    },
    set_TargetItemsHidden: function (value) {
        this._targetItemsHidden = value;
    },
    get_SelectAllSourceItemsButton: function () {
        return this._selectAllSourceItemsButton;
    },
    set_SelectAllSourceItemsButton: function (value) {
        this._selectAllSourceItemsButton = value;
    },
    get_SelectAllTargetItemsButton: function () {
        return this._selectAllTargetItemsButton;
    },
    set_SelectAllTargetItemsButton: function (value) {
        this._selectAllTargetItemsButton = value;
    },


    //getters and setters for properties

    AddOption: function (objTo, Option) {
        var oOption = document.createElement("OPTION");
        oOption.text = Option.text;
        oOption.value = Option.value;
        if (objTo != null)
            objTo.options[objTo.options.length] = oOption;
    },

    AddOption2: function (objTo, oText, oValue) {
        var oOption = document.createElement("OPTION");
        oOption.text = oText;
        oOption.value = oValue;
        if (objTo != null)
            objTo.options[objTo.options.length] = oOption;
    },

    CheckExistenceValue: function (objTo, strValue) {
        if (objTo != null) {
            for (var j = 0; j < objTo.options.length; j++)
                if (objTo.options[j].value == strValue)
                    return true;
        }
        return false;
    },

    MoveOneItem: function (source, target, fromHidden, toHidden) {
        if (source != null && target != null && fromHidden != null && toHidden != null) {
            for (var i = 0; i < source.options.length; i++) {
                if (source.options[i].selected) {
                    if (!this.CheckExistenceValue(target, source.options[i].value)) {
                        this.AddOption(target, source.options[i]);
                        var str = source.options[i].value;
                        if (toHidden.value == '')
                            toHidden.value = str;
                        else
                            toHidden.value += this.get_ItemValuesSeparator() + str;
                    }

                    if (fromHidden.value.indexOf(this.get_ItemValuesSeparator() + str) >= 0)
                        fromHidden.value = fromHidden.value.replace(this.get_ItemValuesSeparator() + str, '');
                    else if (fromHidden.value.indexOf(str + this.get_ItemValuesSeparator()) >= 0)
                        fromHidden.value = fromHidden.value.replace(str + this.get_ItemValuesSeparator(), '');
                    else if (fromHidden.value.indexOf(str) >= 0)
                        fromHidden.value = fromHidden.value.replace(str, '');

                    source.remove(i);
                    i--;
                }
            }
            for (var i = 0; i < source.options.length; i++)
                if (source.options[i].selected) {
                    source.options[i] = null;
                    source.selectedIndex = (i < source.options.length) ? i : 0;
                    return true;
                }
        }
    },

    SelectAllItems: function (source) {
        if (source != null) {
            for (var i = 0; i < source.options.length; i++) {
                source.options[i].selected = true;
            }
        }
    },

    MoveAllItems: function (source, target, fromHidden, toHidden) {
        if (source != null && target != null && fromHidden != null && toHidden != null) {
            for (var i = 0; i < source.options.length; i++) {
                if (!this.CheckExistenceValue(target, source.options[i].value)) {
                    this.AddOption(target, source.options[i]);
                }
            }
            fromHidden.value = '';
            arr = new Array();
            for (var i = 0; i < target.options.length; i++) {
                arr.push(target.options[i].value);
            }
            toHidden.value = arr.join(this.get_ItemValuesSeparator());
            do {
                source.options[0] = null;
            }
            while (source.options.length > 0)
        }
    },

    MoveItemUp: function (source, sHidden) {
        if (source != null && sHidden != null && source.selectedIndex > 0 && sHidden != null) {
            var pos = source.selectedIndex;
            var opt = source.options[pos];
            var arr = sHidden.value.split(this.get_ItemValuesSeparator());
            if (arr != null && arr.length > pos) {
                var tmp = arr[pos - 1];
                arr[pos - 1] = arr[pos];
                arr[pos] = tmp;
            }
            source.options[pos] = null;
            //control.options.remove(pos);
            source.options.add(opt, pos - 1);
            sHidden.value = arr.join(this.get_ItemValuesSeparator());
        }
    },

    MoveItemDown: function (source, sHidden) {
        if (source != null && source.selectedIndex >= 0
			&& source.selectedIndex < source.options.length - 1 && sHidden != null) {
            var pos = source.selectedIndex;
            var opt = source.options[pos];
            var arr = sHidden.value.split(this.get_ItemValuesSeparator());
            if (arr != null && arr.length > pos) {
                var tmp = arr[pos + 1];
                arr[pos + 1] = arr[pos];
                arr[pos] = tmp;
            }
            source.options[pos] = null;
            //		control.options.remove(pos);
            source.options.add(opt, pos + 1);
            sHidden.value = arr.join(this.get_ItemValuesSeparator());
        }
    },

    ButtonUpClick: function () {
        this.MoveItemUp(this.get_TargetList(), this.get_TargetItemsHidden());
        return false;
    },

    ButtonDownClick: function () {
        this.MoveItemDown(this.get_TargetList(), this.get_TargetItemsHidden());
        return false;
    },

    OneItemToTargetClick: function () {
        this.MoveOneItem(this.get_SourceList(), this.get_TargetList(),
			this.get_SourceItemsHidden(), this.get_TargetItemsHidden());
        return false;
    },

    OneItemToSourceClick: function () {
        this.MoveOneItem(this.get_TargetList(), this.get_SourceList(),
			this.get_TargetItemsHidden(), this.get_SourceItemsHidden());
        return false;
    },

    AllItemsToTargetClick: function () {
        this.MoveAllItems(this.get_SourceList(), this.get_TargetList(),
			this.get_SourceItemsHidden(), this.get_TargetItemsHidden());
        return false;
    },

    AllItemsToSourceClick: function () {
        this.MoveAllItems(this.get_TargetList(), this.get_SourceList(),
			this.get_TargetItemsHidden(), this.get_SourceItemsHidden());
        return false;
    },

    SelectAllSourceItemsClick: function () {

        this.SelectAllItems(this.get_SourceList());
        return false;
    },

    SelectAllTargetItemsClick: function () {

        this.SelectAllItems(this.get_TargetList());
        return false;
    },

    initialize: function () {
        this.set_SourceList($get(this.get_SourceListId()));
        this.set_TargetList($get(this.get_TargetListId()));
        this.set_ItemUpButton($get(this.get_ItemUpButtonId()));
        this.set_ItemDownButton($get(this.get_ItemDownButtonId()));
        this.set_OneItemToTargetButton($get(this.get_OneItemToTargetButtonId()));
        this.set_OneItemToSourceButton($get(this.get_OneItemToSourceButtonId()));
        this.set_AllItemsToTargetButton($get(this.get_AllItemsToTargetButtonId()));
        this.set_AllItemsToSourceButton($get(this.get_AllItemsToSourceButtonId()));
        this.set_SourceItemsHidden($get(this.get_SourceItemsHiddenId()));
        this.set_TargetItemsHidden($get(this.get_TargetItemsHiddenId()));
        this.set_SelectAllSourceItemsButton($get(this.get_SelectAllSourceItemsButtonId()));
        this.set_SelectAllTargetItemsButton($get(this.get_SelectAllTargetItemsButtonId()));

        if (this.get_SourceList() != null && this.get_SourceItemsHidden() != null && this.get_SourceList().options.length > 0) {
            this.get_SourceItemsHidden().value = '';
            for (var i = 0; i < this.get_SourceList().options.length - 1; i++) {
                this.get_SourceItemsHidden().value +=
					this.get_SourceList().options[i].value + this.get_ItemValuesSeparator();
            }
            this.get_SourceItemsHidden().value += this.get_SourceList().options[this.get_SourceList().options.length - 1].value;
        }

        if (this.get_TargetList() != null && this.get_TargetItemsHidden() != null && this.get_TargetList().options.length > 0) {
            this.get_TargetItemsHidden().value = '';
            for (var i = 0; i < this.get_TargetList().options.length - 1; i++) {
                this.get_TargetItemsHidden().value +=
					this.get_TargetList().options[i].value + this.get_ItemValuesSeparator();
            }
            this.get_TargetItemsHidden().value += this.get_TargetList().options[this.get_TargetList().options.length - 1].value;
        }

        this._moveOneItemToTargetHandler = Function.createDelegate(this, this.OneItemToTargetClick);
        this._moveOneItemToSourceHandler = Function.createDelegate(this, this.OneItemToSourceClick);

        if (this.get_SourceList() != null) {
            $addHandler(this.get_SourceList(), "dblclick", this._moveOneItemToTargetHandler);
        }
        if (this.get_TargetList() != null) {
            $addHandler(this.get_TargetList(), "dblclick", this._moveOneItemToSourceHandler);
        }

        if (this.get_ItemUpButton() != null)
            $addHandler(this.get_ItemUpButton(), "click", Function.createDelegate(this, this.ButtonUpClick));
        if (this.get_ItemDownButton() != null)
            $addHandler(this.get_ItemDownButton(), "click", Function.createDelegate(this, this.ButtonDownClick));
        if (this.get_OneItemToTargetButton() != null)
            $addHandler(this.get_OneItemToTargetButton(), "click", Function.createDelegate(this, this.OneItemToTargetClick));
        if (this.get_OneItemToSourceButton() != null)
            $addHandler(this.get_OneItemToSourceButton(), "click", Function.createDelegate(this, this.OneItemToSourceClick));
        if (this.get_AllItemsToTargetButton() != null)
            $addHandler(this.get_AllItemsToTargetButton(), "click", Function.createDelegate(this, this.AllItemsToTargetClick));
        if (this.get_AllItemsToSourceButton() != null)
            $addHandler(this.get_AllItemsToSourceButton(), "click", Function.createDelegate(this, this.AllItemsToSourceClick));
        if (this.get_SelectAllSourceItemsButton() != null)
            $addHandler(this.get_SelectAllSourceItemsButton(), "click", Function.createDelegate(this, this.SelectAllSourceItemsClick));
        if (this.get_SelectAllTargetItemsButton() != null)
            $addHandler(this.get_SelectAllTargetItemsButton(), "click", Function.createDelegate(this, this.SelectAllTargetItemsClick));
        Mediachase.ListToListSelector.callBaseMethod(this, 'initialize');
    },

    dispose: function () {
        if (this.get_SourceList() != null && this._moveOneItemToTargetHandler != null) {
            $removeHandler(this.get_SourceList(), "dblclick", this._moveOneItemToTargetHandler);
        }
        if (this.get_TargetList() != null && this._moveOneItemToSourceHandler != null) {
            $removeHandler(this.get_TargetList(), "dblclick", this._moveOneItemToSourceHandler);
        }

        if (this.get_ItemUpButton() != null)
            $clearHandlers(this.get_ItemUpButton());
        if (this.get_ItemDownButton() != null)
            $clearHandlers(this.get_ItemDownButton());
        if (this.get_OneItemToTargetButton() != null)
            $clearHandlers(this.get_OneItemToTargetButton());
        if (this.get_OneItemToSourceButton() != null)
            $clearHandlers(this.get_OneItemToSourceButton());
        if (this.get_AllItemsToTargetButton() != null)
            $clearHandlers(this.get_AllItemsToTargetButton());
        if (this.get_AllItemsToSourceButton() != null)
            $clearHandlers(this.get_AllItemsToSourceButton());
        if (this.get_SelectAllSourceItemsButton() != null)
            $clearHandlers(this.get_SelectAllSourceItemsButton());
        Mediachase.ListToListSelector.callBaseMethod(this, 'dispose');
    }
}

Mediachase.ListToListSelector.registerClass('Mediachase.ListToListSelector', Sys.Component);

if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();