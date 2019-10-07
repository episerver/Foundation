define([
    "dojo/_base/declare",
    "dojo/_base/lang",
    "dojo/_base/array",
    "dojo/aspect",
    "dojo/dom-style",
    "dojo/dom-class",
    "epi-cms/contentediting/CreateContent",
    "epi/shell/widget/SearchBox"
], function (
    declare,
    lang,
    array,
    aspect,
    domStyle,
    domClass,
    CreateContent,
    SearchBox
) {
        return declare([CreateContent], {

            postCreate: function () {
                this.inherited(arguments);
                // search box
                this.own(this._searchBox = new SearchBox({}));
                this._searchBox.placeAt(this.namePanel, "last");
                domStyle.set(this._searchBox.domNode, "width", "auto");
                domClass.add(this.namePanel, "epi-gadgetInnerToolbar");
                this.own(
                    this._searchBox.on("searchBoxChange", lang.hitch(this, this._onSearchTextChanged)),

                    aspect.before(this.contentTypeList, "refresh", lang.hitch(this, function () {
                        // reset the search box and _originalGroups
                        this._searchBox.clearValue();
                        this._originalGroups = null;
                    })),

                    aspect.after(this.contentTypeList, "setVisibility", lang.hitch(this, function (display) {
                        if (!display) {
                            domStyle.set(this._searchBox.domNode, "display", "none");
                        }
                    }), true)
                );
            },
            _onSearchTextChanged: function (queryText) {
                this._originalGroups = this._originalGroups || lang.clone(this.contentTypeList.model.groups);
                var groupKeys = Object.keys(this._originalGroups);

                array.forEach(groupKeys, function (key) {
                    var contentTypes = this._originalGroups[key].get("contentTypes");
                    contentTypes = array.filter(contentTypes, function (item) {
                        return item.name.toLowerCase().indexOf(queryText.toLowerCase()) !== -1;
                    });
                    if (!contentTypes.length) {
                        domStyle.set(this.contentTypeList.model.groups[key].domNode, "display", "none");
                    }
                    else {
                        domStyle.set(this.contentTypeList.model.groups[key].domNode, "display", "");
                        this.contentTypeList.model.groups[key].set("contentTypes", contentTypes);
                    }
                }, this);
            }
        });
    });