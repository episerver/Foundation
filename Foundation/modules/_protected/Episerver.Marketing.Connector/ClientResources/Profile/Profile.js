dojo.require("epi-cms/ErrorDialog");
dojo.require('dojo.data.ItemFileReadStore');
dojo.require('dojo.store.DataStore');

(function () {
    // TECHNOTE: because we don't want to use ASPX page to render this script, this path will be hardcode here as the ModuleName
    var MODULE_ROOT_PATH = '../../Episerver.Marketing.Connector';


    return {
        createUI: function (namingContainer, container, settings, containerCallback) {
            var self = this;

            this.prototype.createUI.apply(this, arguments);
        },

        uiCreated: function (namingContainer, settings) {
            require([
                // Dojo
                "dojo/_base/array",
                "dojo/dom-style"
            ], function (
                // Dojo
                array,
                domStyle
            ) {
                    var connectorSelection = dijit.byId(namingContainer + 'ConnectorId'),
                        databaseSelection = dijit.byId(namingContainer + 'DataSource'),
                        connectorRowNode = dijit.getEnclosingWidget(connectorSelection.domNode).domNode.parentNode,
                        databaseRowNode = dijit.getEnclosingWidget(databaseSelection.domNode).domNode.parentNode;

                    connectorSelection.set('searchAttr', 'Key');
                    connectorSelection.set('required', true);
                    connectorSelection.set('missingMessage', 'DataSource selection is required');
                    dojo.connect(connectorSelection, 'onChange', this, function (item) {
                        if (!loadingComplete || !connectorSelection.isValid())
                            return;
                        databaseSelection.reset();
                        columnSelection.reset();
                        columnSelection.set('disabled', true);
                        loadDatabases({ ConnectorId: item });
                    });
                    connectorSelection.set('selectOnClick', true);

                    databaseSelection.set('searchAttr', 'Key');
                    databaseSelection.set('required', true);
                    databaseSelection.set('missingMessage', 'DataSource selection is required');
                    dojo.connect(databaseSelection, 'onChange', this, function (item) {
                        if (!loadingComplete || !databaseSelection.isValid())
                            return;
                        columnSelection.reset();
                        loadColumns({ ConnectorId: connectorSelection.get("value"), DataSource: item });
                    });
                    databaseSelection.set('selectOnClick', true);

                    var columnSelection = dijit.byId(namingContainer + 'Field');
                    columnSelection.set('searchAttr', 'Key');
                    columnSelection.set('disabled', true);

                    var conditionSelection = dijit.byId(namingContainer + "Condition"),
                        conditionValue = conditionSelection.get("value"),
                        valueInput = dijit.byId(namingContainer + "Value"),
                        valueRowNode = dijit.getEnclosingWidget(valueInput.domNode).domNode.parentNode;
                    dojo.connect(conditionSelection, "onChange", this, updateUIElements);

                    function loadConnectors(settings, callback) {
                        dojo.xhrGet({
                            url: MODULE_ROOT_PATH + '/Profile/GetConnectors',
                            handleAs: 'json',
                            error: epi.cms.ErrorDialog.showXmlHttpError,
                            load: function (jsonData) {
                                if (jsonData) {
                                    var store = new dojo.data.ItemFileReadStore({ data: jsonData });
                                    connectorSelection.store = dojo.store.DataStore({ store: store });

                                    if (settings) {
                                        connectorSelection.set('value', settings.ConnectorId);
                                        loadDatabases(settings, callback)
                                    } else {
                                        if (callback) {
                                            callback();
                                        }
                                    }
                                }
                            }
                        });
                    }

                    function loadDatabases(settings, callback) {
                        if (!settings) {
                            if (callback) {
                                callback();
                            }
                            return;
                        }

                        dojo.xhrGet({
                            url: MODULE_ROOT_PATH + '/Profile/GetDatabaseRecords?connectorId=' + settings.ConnectorId,
                            handleAs: 'json',
                            error: epi.cms.ErrorDialog.showXmlHttpError,
                            load: function (jsonData) {
                                if (jsonData.items == null || jsonData.items.length == 0) {
                                    // In case there are no database returned, there might only 1 common database existing,
                                    // hide the database row and try to load the columns.
                                    domStyle.set(databaseRowNode, 'display', 'none');
                                    loadColumns({ DataSource: -1 });
                                    return;
                                }
                                var store = new dojo.data.ItemFileReadStore({ data: jsonData });
                                databaseSelection.store = dojo.store.DataStore({ store: store });
                                if (settings.DataSource) {
                                    if (settings.DataSource && array.some(jsonData.items, function (item) { return item.Value == settings.DataSource })) {
                                        databaseSelection.set('value', settings.DataSource);
                                    }
                                    loadColumns(settings, callback);
                                } else {
                                    if (callback) {
                                        callback();
                                    }
                                }
                            }
                        });
                    }

                    function loadColumns(settings, callback) {
                        if (!settings) {
                            if (callback) {
                                callback();
                            }
                            return;
                        }

                        dojo.xhrGet({
                            url: MODULE_ROOT_PATH + '/Profile/GetDatabaseColumnRecords?connectorId=' + settings.ConnectorId + '&databaseId=' + settings.DataSource,
                            handleAs: 'json',
                            error: epi.cms.ErrorDialog.showXmlHttpError,
                            load: function (jsonData) {
                                var store = new dojo.data.ItemFileReadStore({ data: jsonData });
                                columnSelection.store = dojo.store.DataStore({ store: store });
                                columnSelection.set('disabled', false);
                                if (settings.Field && array.some(jsonData.items, function (item) { return item.Value == settings.Field })) {
                                    columnSelection.set('value', settings.Field);
                                    loadingComplete = true;
                                } else {
                                    columnSelection.set('value', '*');
                                }
                            }
                        });
                    }

                    function updateUIElements(value) {
                        // Update UI when condition is changed.
                        if (value == '10' || value == '11') { // when condition is 'is null' or 'is not null', don't display the value field
                            domStyle.set(valueRowNode, 'display', 'none');
                            valueInput.set('value', '');
                        } else {
                            domStyle.set(valueRowNode, 'display', '');
                        }
                    }

                    var loadingComplete = false;

                    // Load initial values and use selections from settings, and set loadingComplete to true after everything's complete.
                    loadConnectors(settings, function () {
                        loadingComplete = true;
                        //containerCallback(namingContainer, container);
                    });

                    // initialize UI
                    updateUIElements(conditionValue);
                });
        },

        getSettings: function (namingContainer) {
            return {
                ConnectorId: dijit.byId(namingContainer + "ConnectorId").get('value'),
                DataSource: dijit.byId(namingContainer + 'DataSource').get('value'),
                Field: dijit.byId(namingContainer + 'Field').get('value'),
                Condition: dijit.byId(namingContainer + 'Condition').get('value'),
                Value: dijit.byId(namingContainer + 'Value').get('value')
            };
        }
    };
})();