define([
    "dojo/_base/array",
    "dojo/_base/declare",
    "dojo/_base/lang",
    "epi-cms/contentediting/editors/CollectionEditor",
    "foundation/VariantOptionPrices"
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
                //Override: Showing the prices, not [object Object]
                result.prices.formatter = function (values) {
                    return values.map(p => p.amount + p.currency).join();
                };
                return result;
            }
        });
    });