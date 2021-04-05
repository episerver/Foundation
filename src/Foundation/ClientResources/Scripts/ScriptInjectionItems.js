define([
    "dojo/_base/array",
    "dojo/_base/declare",
    "dojo/_base/lang",
    "epi-cms/contentediting/editors/CollectionEditor",
    "foundation/ScriptInjectionItems"
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
                result.scriptFiles.formatter = function (values) {
                    return values.map(sf => sf.text).join();
                };
                return result;
            }
        });
    });