(function () {
    var MODULE_ROOT_PATH = '../../EPiServer.Forms.Samples';

    return {
        createUI: function (namingContainer, container, settings, containerCallback) {
            var self = this;

            this.prototype.createUI.apply(this, arguments);
        },

        uiCreated: function (namingContainer, settings) {
            require([
                // Dojo
                "dojo/_base/array",
                "dojo/_base/connect",
                "dojo/dom-style",
                "dojo/store/Memory",
                "dojo/request/xhr",
                "dojo/on",

                // Dijit
                "dijit/registry"
            ], function (
                // Dojo
                array,
                connect,
                domStyle,
                Memory,
                xhr,
                on,

                // Dijit
                registry
            ) {

                var formSelection = registry.byId(namingContainer + 'SelectedForm');
                formSelection.set('required', true);
                connect.connect(formSelection, 'onChange', this, function (formGuid) {
                    if (!formGuid || !formSelection.isValid()) {
                        return;
                    }

                    loadFormFields({ SelectedForm: formGuid });
                });
                formSelection.set('selectOnClick', true);

                var fieldSelection = registry.byId(namingContainer + 'SelectedField');
                fieldSelection.set('searchAttr', 'FriendlyName');

                function loadFormFields(settings) {

                    if (!settings || !settings.SelectedForm) {
                        return;
                    }

                    var model = settings,
                        url = MODULE_ROOT_PATH + '/FormInfo/GetElementFriendlyNames?formGuid=' + settings.SelectedForm;
                    xhr(url, { handleAs: 'json' }).then(
                        function (jsonData) {
                            var store = new Memory({ idProperty: "ElementId", data: jsonData });
                            fieldSelection.set("store", store);

                            var isExisting = array.some(jsonData, function (item) { return item.ElementId == model.SelectedField });
                            if (model.SelectedField && isExisting) {
                                fieldSelection.set('value', model.SelectedField);
                            } else {
                                fieldSelection.set('value', '');
                            }
                        },
                        function (err) {
                            epi.cms.ErrorDialog.showXmlHttpError(err);
                        });
                }

                // if newly DnD criteria, load fields of the first form which is displaying in formSelection widget
                if (!settings) {
                    var formGuid = formSelection.get("value");
                    loadFormFields({ SelectedForm: formGuid });
                // otherwise, load fields of the stored form
                } else {
                    loadFormFields(settings)
                }
            });
        },

        // return settings model of the criteria.
        getSettings: function (namingContainer) {

            return {
                SelectedForm: dijit.byId(namingContainer + 'SelectedForm').get('value'),
                SelectedField: dijit.byId(namingContainer + 'SelectedField').get('value'),
                Condition: dijit.byId(namingContainer + 'Condition').get('value'),
                Value: dijit.byId(namingContainer + 'Value').get('value')
            };
        }
    };
})();