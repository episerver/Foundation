define("epi/shell/widget/WidgetSwitcher", [
    "dojo/_base/array",
    "dojo/_base/declare",
    "dojo/_base/lang",
    "dojo/Deferred",
    "dojo/topic",
    "dojo/when",
    "epi/shell/StickyViewSelector",
    "epi/shell/_ContextMixin",
    "epi/shell/layout/AnimatedCardContainer",
    "epi/shell/TypeDescriptorManager"
], function (
    array,
    declare,
    lang,
    Deferred,
    topic,
    when,
    StickyViewSelector,
    _ContextMixin,
    AnimatedCardContainer,
    TypeDescriptorManager
) {

        return declare([AnimatedCardContainer, _ContextMixin], {
            // summary:
            //      A container that handles showing a child widget based on either a
            //      specific view request or a change in the current context.
            // tags:
            //      internal

            // componentConstructorArgs: Object
            //      Arguments that must be passed to constructor when new component is created.
            componentConstructorArgs: null,

            // supportedContextTypes: Array
            //      String array of supported context data types.
            //      This option should be defined if we need to react only on context changes with specific data type.
            //      Context changes are ignored if set to empty array.
            supportedContextTypes: null,

            // _stickyViewSelector: epi/shell/StickyViewSelector
            //    Responsible for storing view selected by editor
            _stickyViewSelector: null,

            constructor: function () {
                this._viewHistory = [];
            },

            postMixInProperties: function () {
                // summary:
                //      Verifies and fixes the list of supported context data types.
                // tags:
                //      protected
                this.inherited(arguments);

                this._stickyViewSelector = new StickyViewSelector();

                if (this.supportedContextTypes) {
                    if (!lang.isArray(this.supportedContextTypes)) {
                        this.supportedContextTypes = [this.supportedContextTypes];
                    }
                    if (!array.every(this.supportedContextTypes, function (type) {
                        return typeof type == "string";
                    })) {
                        throw new Error("supportedContextTypes must be string, array of strings or null.");
                    }
                }
            },

            postCreate: function () {
                // summary:
                //      If the current context is available it loads the initial view component for it. Also subscribes
                //      to view component change requests.
                // tags:
                //      protected

                this.inherited(arguments);

                when(this.getCurrentContext(), lang.hitch(this, this.contextChanged));
                this.own(
                    topic.subscribe("/epi/shell/action/changeview", lang.hitch(this, "viewComponentChangeRequested")),
                    topic.subscribe("/epi/shell/action/changeview/back", lang.hitch(this, "_viewComponentBackRequested")),
                    topic.subscribe("/epi/shell/action/changeview/updatestate", lang.hitch(this, "_changeViewState"))
                );
            },

            contextChanged: function (/*Object*/context, data) {
                // summary:
                //      Called when the current context changes.
                // tags:
                //      protected

                if (!this._isContextTypeSupported(context)) {
                    return;
                }

                var view = context.customViewType || (data ? data.viewType : null);
                var currentWidgetInfo = this._getCurrentWidgetInfo();
                //NOTE: when we change so that we can route to different views this should be rewritten.
                if (data && data.contextIdSyncChange && currentWidgetInfo) {
                    view = currentWidgetInfo.type;
                    data = lang.mixin({}, data, {
                        viewName: currentWidgetInfo.viewName,
                        availableViews: currentWidgetInfo.availableViews
                    });
                }

                if (!view && context.dataType) {
                    // If there is no custom view type, fall back to the widget for the type.
                    // For backwards compatibility, check the obsoleted property mainWidgetType
                    view = TypeDescriptorManager.getValue(context.dataType, "mainWidgetType");
                }

                if (view) {
                    this._getObject(view, null, context, data);
                } else {
                    var suggestedView = TypeDescriptorManager.getValue(context.dataType, "defaultView"),
                        availableViews = this._getAvailableViews(context.dataType),
                        requestedViewName = data ? data.viewName : null;

                    // try to load last selected view
                    when(this._stickyViewSelector.get(context.hasTemplate, context.dataType))
                        .then(function (stickyView) {
                            if (stickyView) {
                                suggestedView = stickyView;
                            }

                            var viewsArr = [
                                "view",
                                "sidebysidecompare",
                                "allpropertiescompare"
                            ];

                            /* ********************************************************************************************************************************** */
                            /*  Code in this section is different than original file */
                            /* ********************************************************************************************************************************** */

                            viewsArr.push("compareWithMarkup");

                            /* ********************************************************************************************************************************** */

                            // TODO: consider removing reference to "view"(preview) when refactoring the view rerouting
                            if (currentWidgetInfo && viewsArr.indexOf(currentWidgetInfo.viewName) >= 0) {
                                // if we're viewing the preview we should keep that view as long as it's available.
                                array.some(availableViews, function (availableView) {
                                    if (availableView.key === currentWidgetInfo.viewName) {
                                        suggestedView = currentWidgetInfo.viewName;
                                        return true;
                                    }
                                    return false;
                                });
                            }

                            var viewToLoad = this._getViewByKey(requestedViewName || suggestedView, availableViews);

                            if (!viewToLoad) {
                                console.log("No default view found for " + context.dataType);
                                return;
                            }

                            this._loadViewComponentByConfiguration(viewToLoad, availableViews, null, context, data);
                        }.bind(this));
                }
            },

            _getViewByKey: function (key, availableViews) {
                return array.filter(availableViews, function (availableView) {
                    return availableView.key === key;
                })[0];
            },

            _getAvailableViews: function (dataType) {
                var availableViews = TypeDescriptorManager.getAndConcatenateValues(dataType, "availableViews"),
                    disabledViews = TypeDescriptorManager.getValue(dataType, "disabledViews");

                var filteredViews = array.filter(availableViews, function (availableView) {
                    return array.every(disabledViews, function (disabledView) {
                        return availableView.key !== disabledView;
                    });
                });
                return filteredViews.sort(function (x, y) {
                    return x.sortOrder < y.sortOrder ? -1 : 1;
                });
            },

            _loadViewComponentByConfiguration: function (view, availableViews, args, context, data, saveView) {
                data = lang.mixin({}, data, {
                    viewName: view.key,
                    availableViews: availableViews
                });

                this._getObject(view.controllerType, args, context, data, saveView);
            },

            viewComponentChangeRequested: function (/*String*/type, /*Object*/args, /*Object*/data, /*Boolean?*/saveView) {
                // summary:
                //      Called when a new view component is requested.
                // type: String
                //      Specify component widget type or a key which refers to a view configuration.
                // args: Object
                //      Arguments used to instantiate component widget.
                // data: Object
                //      data passed to the view when calling updateView.
                // saveView: Boolean?
                //      when true, then selected view will be saved
                // tags:
                //      protected

                when(this.getCurrentContext(), lang.hitch(this, function (currentContext) {

                    var dataType = currentContext ? currentContext.dataType : null;
                    var hasTemplate = currentContext ? currentContext.hasTemplate : false;
                    var availableViews = this._getAvailableViews(dataType);
                    var defaultViewName;
                    var stickyView;

                    // If no view or widget type is supplied, then use the default view for the content type
                    if (!type && dataType) {
                        defaultViewName = TypeDescriptorManager.getValue(dataType, "defaultView");
                        stickyView = this._stickyViewSelector.get(hasTemplate, dataType);
                        if (stickyView) {
                            type = stickyView;
                        } else {
                            type = defaultViewName;
                        }
                    }

                    when(type)
                        .then(function (typeResult) {
                            var view = this._getViewByKey(typeResult || defaultViewName, availableViews);

                            if (view) {
                                // A view configuration found, means that new view is being requested by key
                                this._loadViewComponentByConfiguration(view, availableViews, args, currentContext, data, saveView);
                            } else {
                                // Otherwise, by component type. (Backward compatibility)
                                this._getObject(typeResult, args, currentContext, data, saveView);
                            }
                        }.bind(this));
                }));
            },

            _viewComponentBackRequested: function (forceReload) {
                var previousWidgetInfo = this._getPreviousWidgetInfo();
                if (previousWidgetInfo) {
                    if (typeof forceReload === "undefined") {
                        // forceReload should be false by default, which means that we will skip updating view.
                        forceReload = false;
                    }
                    var data = {
                        skipUpdateView: !forceReload,
                        availableViews: previousWidgetInfo.availableViews,
                        viewName: previousWidgetInfo.viewName
                    };
                    when(this.getCurrentContext(), lang.hitch(this, function (currentContext) {
                        this._getObject(previousWidgetInfo.type, null, currentContext, data);
                    }));
                }
            },

            _changeViewState: function (state) {
                var currentWidgetInfo = this._getCurrentWidgetInfo();
                if (currentWidgetInfo) {
                    lang.mixin(currentWidgetInfo, state);
                }
            },

            _getPreviousWidgetInfo: function () {
                // summary:
                //      Gets the previous view on back/cancel and then removes the canceled view from history
                //      so as not to cause a potential 'cancel' loop
                // tags:
                //      private
                var length = this._viewHistory.length;
                var history = this._viewHistory;
                var result = (history.length > 1) ? history[history.length - 2] : null;
                this._viewHistory.splice(length - 1, 1);
                return result;
            },

            _getCurrentWidgetInfo: function () {
                var history = this._viewHistory;
                return (history.length > 0) ? history[history.length - 1] : null;
            },

            _isContextTypeSupported: function (context) {
                // summary:
                //      Verifies if data type of specified context is supported by this instance
                // tags:
                //      private
                if (!context) {
                    return false;
                }
                if (!this.supportedContextTypes) {
                    return true;
                }
                return array.some(this.supportedContextTypes, function (type) {
                    return context.type === type;
                });
            },

            _getObject: function (type, args, context, data, saveView) {
                // summary:
                //      Gets an instance for the given type if it is different from
                //      the current view component type.
                // tags:
                //      private

                if (!type) {
                    return;
                }

                var current = this.selectedChildWidget;

                var func = lang.hitch(this, function () {
                    when(this._testComponentInstanceOf(current, type), lang.hitch(this, function (result) {
                        if (saveView) {
                            // saving is deferred but it's not blocking operation
                            this._stickyViewSelector.save(context.hasTemplate, context.dataType, data.viewName);
                        }

                        if (result) {
                            this._updateHistory(type, data, context);
                            this._updateView(current, context, data);
                            this._onViewChanged(type, args, data);
                        } else {
                            this._changeViewComponent(type, args, context, data);
                        }
                    }));
                });

                if (current && current.savePendingChanges) {
                    when(current.savePendingChanges(), func);
                } else {
                    func();
                }

            },

            _changeViewComponent: function (type, args, context, data) {
                // summary:
                //      Remove the current view component and add the new one.
                // tags:
                //      private

                when(this._getChildByType(type), lang.hitch(this, function (component) {
                    var current = this.selectedChildWidget;

                    // suspend the current one before changing to the next
                    if (current) {
                        current.set("isActive", false);
                    }

                    if (component) {
                        // activate the component
                        component.set("isActive", true);

                        if (data && data.treatAsSecondaryView) {
                            this.selectSecondaryChild(component);
                        } else {
                            this.selectChild(component);
                        }
                        this._updateHistory(type, data, context);
                        this._updateView(component, context, data, true);
                        this._onViewChanged(type, args, data);
                    } else {
                        require([type], lang.hitch(this, function (componentClass) {
                            var mixedArgs = lang.mixin(lang.mixin({}, this.componentConstructorArgs), args);
                            component = new componentClass(mixedArgs);

                            component.fitContainer = true;

                            this.addChild(component);

                            var select;

                            if (data && data.treatAsSecondaryView) {
                                select = this.selectSecondaryChild(component);
                            } else {
                                select = this.selectChild(component);
                            }

                            when(select, lang.hitch(this, function () {
                                this._updateHistory(type, data, context);
                                this._updateView(component, context, data, true);
                                this._onViewChanged(type, args, data);
                            }));
                        }));
                    }
                }));
            },

            _updateView: function (component, context, data, widgetResumed) {
                if (component && !component._started && typeof component.startup === "function") {
                    // Ensure component ready.
                    component.startup();
                }

                if (component && typeof component.updateView === "function") {
                    component.updateView(data, context, { widgetResumed: widgetResumed });
                }
            },

            _getChildByType: function (type) {
                var def = new Deferred();

                require([type], lang.hitch(this, function (componentClass) {
                    var components = array.filter(this.getChildren(), function (child) {
                        return child.constructor === componentClass;
                    }, this);

                    def.resolve(components.length ? components[0] : null);
                }));

                return def;
            },

            _testComponentInstanceOf: function (component, type) {
                var def = new Deferred();

                if (component) {
                    if (typeof type === "string") {
                        require([type], function (componentClass) {
                            def.resolve(component && component.constructor === componentClass);
                        });
                    } else {
                        def.resolve(component && component.constructor === type.constructor);
                    }
                } else {
                    def.resolve(false);
                }

                return def;
            },

            _onViewChanged: function (type, args, data) {
                topic.publish("/epi/shell/action/viewchanged", type, args, data);
            },

            _updateHistory: function (/*string*/type, data, context) {
                // summary:
                //      Add data to widget history store
                //      Keep total items in store always 2
                // tags:
                //      private

                var history = this._viewHistory,
                    length = history.length,
                    item = this.createStoreItem(type, data, context);

                if (length && history[length - 1].id === item.id) {
                    return;
                }

                //  We limit the history length to a safe arbitrary number (9) so as to avoid a potential unnecessarily large history
                if (length === 9) {
                    history.shift();
                }

                history.push(item);
            },

            createStoreItem: function (/*string*/type, data, context) {
                // summary:
                //      Create new object that will be put to store
                // tags:
                //      protected

                var storeItem = { type: type, id: type };
                if (data) {
                    var availableViews = data.availableViews || (context && this._getAvailableViews(context.dataType));
                    var viewName = data.viewName || this._getViewName(availableViews, context);

                    storeItem = lang.mixin(storeItem, {
                        availableViews: availableViews,
                        viewName: viewName
                    });
                    storeItem.id += "#" + viewName;
                }
                return storeItem;
            },

            _getViewName: function (availableViews, context) {
                if (!context) {
                    return null;
                }

                var selectedView = this._getViewByKey(TypeDescriptorManager.getValue(context.dataType, "defaultView"), availableViews);
                if (!selectedView) {
                    return null;
                }
                return selectedView.key;
            }
        });
    });