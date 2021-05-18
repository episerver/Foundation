define([
    "dojo/_base/array",
    "dojo/_base/declare",
    "dojo/_base/lang",
    "epi-cms/contentediting/editors/CollectionEditor",
    "foundation/MenuChildItems"
],
    function (
        array,
        declare,
        lang,
        CollectionEditor
    ) {
        return declare([CollectionEditor], {
            _getGridDefinition: function () {
                var result = this.inherited(arguments);
                //Override: Showing the name of the child items, not [object Object]
                result.listCategories.formatter = function (values) {
                    return values.map(msc => msc.text).join();
                };
                return result;
            }
        });
    });