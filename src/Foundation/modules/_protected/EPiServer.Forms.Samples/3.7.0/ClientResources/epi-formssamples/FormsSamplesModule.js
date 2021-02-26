define([
// dojo
    'dojo/_base/declare',
    'dojo/aspect',
// dijit
    'dijit/Destroyable',
// epi
    'epi/_Module',
    'epi/dependency'
],
function (
// dojo
    declare,
    aspect,
// dijit
    Destroyable,
// epi
    _Module,
    dependency
) {

    // module:
    //      epi-formssamples/FormsSamplesModule
    // summary:
    //      EPiServer Form Sample module
    // tags:
    //      public
    return declare([_Module, Destroyable], {

        _settings: null,

        // =======================================================================
        // Public, overrided stubs
        // =======================================================================
        constructor: function (/*Object*/settings) {
            this._settings = settings;
        },

        initialize: function () {
            // summary:
            //      Initialize module
            // tags:
            //      public, extensions

            var contentTypeService = dependency.resolve('epi.cms.ContentTypeService'),
                self = this;
            aspect.around(contentTypeService, '_getImagePathByTypeIdentifier', function (originalFunction) {
                return function (/*String*/typeIdentifier, /*Array*/registeredTypes, /*String*/clientResourcePath) {
                    var types = self._settings.registeredElementContentTypes;
                    if (types instanceof Array && types.indexOf(typeIdentifier) >= 0) {
                        // show the .png as big icon for creating FormElement in the ContentArea
                        return self._settings.clientResourcePath + '/ClientResources/epi-formssamples/themes/sleek/images/contenttypes/' + typeIdentifier.split('.').pop() + '.png';
                    }

                    return originalFunction.apply(contentTypeService, arguments);
                };
            });
        }
    });

});