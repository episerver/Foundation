define([
    "dojo/_base/declare",
    "dojo/aspect",

    "epi/_Module",
    "epi/routes",
    "epi/dependency",
], function (
    declare,
    aspect,

    _Module,
    routes,
    dependency
) {
        return declare([_Module], {
            initialize: function () {
                this.inherited(arguments);

                this._replaceCreateCommand();
            },

            _replaceCreateCommand: function () {
                var widgetFactory = dependency.resolve("epi.shell.widget.WidgetFactory");
                aspect.after(widgetFactory, "onWidgetCreated", function (widget, componentDefinition) {
                    if (componentDefinition.widgetType === "epi/shell/widget/WidgetSwitcher") {
                        aspect.around(widget, "viewComponentChangeRequested", function (originalMethod) {
                            return function () {
                                if (arguments[0] === "epi-cms/contentediting/CreateContent") {
                                    arguments[0] = "foundation/contentediting/CreateContent";
                                }
                                originalMethod.apply(this, arguments);
                            };
                        });
                    }
                }, true);
            }
        });
    });