
define([
    "dojo/query",
    "dojo/_base/connect",
    "dojo/_base/declare",
    "dijit/_CssStateMixin",
    "dijit/_Widget",
    "dijit/_TemplatedMixin",
    "dijit/_WidgetsInTemplateMixin",
    "epi/shell/widget/_ValueRequiredMixin",
],

    function (
        query,
        connect,
        declare,
        _CssStateMixin,
        _Widget,
        _TemplatedMixin,
        _WidgetsInTemplateMixin,
        _ValueRequiredMixin,
    ) {

        return declare("foundation/editors/ColorPicker", [_Widget, _TemplatedMixin, _WidgetsInTemplateMixin, _CssStateMixin, _ValueRequiredMixin],
            {
                templateString: 
                    `<div style="position: absolute; min-width: 250px" 
                        data-dojo-attach-point=\"pickerElement\" class=\"dijitInline\" tabindex=\"-1\" role=\"presentation\">
                            <div data-dojo-attach-point=\"stateNode, tooltipNode\">
                                <input data-dojo-attach-point=\"colorPicker\" data-dojo-props="class: 'color'" data-dojo-type=\"dijit.form.TextBox\"></input>
                            </div>\
                        </div>`,
                
                intermediateChanges: false,

                value: null,
                picker: null,
                onClick: function () {
                    this.picker.openHandler();
                },
                onChange: function (value) {
                    // summary:

                    //    Called when the value in the widget changes.

                    // tags:

                    //    public callback

                    this.colorPicker.set("value", value);
                },

                startup: function () {
                    var parentBasic = this.pickerElement;
                    var inst = this;
                    this.picker = new Picker({ parent: parentBasic, color: this.value });
                    this.picker.onDone = function (color) {
                        inst._onColorPickerChanged(color);
                    };
                },

                postCreate: function () {
                    
                    if (this.value != null) {
                        this.set("value", this.value);
                    } else {
                        this._set("value", "");
                        this.onChange(this.value);
                    }
                    this.inherited(arguments);
                    this.colorPicker.set("intermediateChanges", this.intermediateChanges);
                    this.connect(this.pickerElement, "onChange", this._onColorPickerChanged);
                    this.connect(this.pickerElement, "onClick", this.onClick);
                },

                _onIntermediateChange: function (event) {
                    if (this.intermediateChanges) {
                        this._set("value", event.target.value);
                        this.onChange(this.value);
                    }
                },

                focus: function () {
                    dijit.focus(this.colorPicker);
                },

                isValid: function () {
                    return !this.required || this.colorPicker.value.length > 0;
                },

                _setValueAttr: function (value) {
                    if (value != null) {
                        this._set("value", value);
                        this.colorPicker.set("value", value);
                        this.picker.setColor(value, true);
                    }
                },

                _setReadOnlyAttr: function (value) {
                    this._set("readOnly", value);
                    this.colorPicker.set("readOnly", value);
                },

                _setIntermediateChangesAttr: function (value) {
                    this.colorPicker.set("intermediateChanges", value);
                    this._set("intermediateChanges", value);
                },

                _onColorPickerChanged: function (value) {
                    if (value && value.hex != this.colorPicker.value) {
                        this._set("value", value.hex);
                        this.onChange(this.value);
                    }
                },
            }
        );

    });