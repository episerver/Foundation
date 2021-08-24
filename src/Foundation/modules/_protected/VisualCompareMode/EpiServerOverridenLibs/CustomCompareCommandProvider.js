define("epi-cms/compare/command/CompareCommandProvider", [
    "dojo/_base/declare",
    "epi-cms/component/command/_GlobalToolbarCommandProvider",

    "./CompareSettingsModel",
    "./CompareViewSelection",
    "epi/shell/command/ToggleCommand",
    "epi-cms/command/_NonEditViewCommandMixin",
    "epi/i18n!epi/cms/nls/episerver.cms.compare.command"
], function (
    declare,
    _GlobalToolbarCommandProvider,

    CompareSettingsModel,
    CompareViewSelection,
    ToggleCommand,
    _NonEditViewCommandMixin,
    resources
) {

    var NonEditViewToggleCommand = declare([ToggleCommand, _NonEditViewCommandMixin]);

    return declare([_GlobalToolbarCommandProvider], {
        // summary:
        //      A command provider providing compare commands to the global toolbar

        postscript: function () {
            this.inherited(arguments);

            var model = new CompareSettingsModel();

            /* ********************************************************************************************************************************** */
            /*  Code in this section is different than original file */
            /* ********************************************************************************************************************************** */

            model.modeOptions.push(
                { label: "Visual comparison", value: "compareWithMarkup", iconClass: "epi-iconForms" });

            /* ********************************************************************************************************************************** */

            // Need to have a settings object as well, since the global menu builder looks a this for category
            var settings = { category: "compare" };

            this.add("commands", new NonEditViewToggleCommand({
                category: "compare",
                settings: settings,
                label: resources.togglecompare.label,
                iconClass: "epi-iconCompare",
                model: model,
                property: "enabled"
            }));
            this.add("commands", new CompareViewSelection({
                category: "compare",
                settings: settings,
                model: model,
                label: resources.compareviewselection.label,
                optionsLabel: resources.compareviewselection.label,
                optionsProperty: "modeOptions",
                property: "mode"
            }));
        }
    });
});