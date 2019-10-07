(function (tinymce, $) {
    tinymce.create('tinymce.plugins.epifilemanagerdragdrop', {
        /**
        * Initializes the Filemanager drag and drop plugin.
        *
        * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
        * @param {string} url Absolute URL to where the plugin is located.
        */
        init: function (ed, url) {
            // Check for jQuery before doing anything since we want a consistent way to bind to drop event.
            if ($) {

                ed.onInit.add(function (ed) {
                    $(ed.getBody()).bind("dragover", function (e) {
                        // check user agent to detect Chrome browser
                        if (/chrome/.test(navigator.userAgent.toLowerCase())) {
                            if (e.originalEvent.dataTransfer.types && $.inArray("text/html", e.originalEvent.dataTransfer.types) != -1) {
                                // might be dragging inside TinyMCE. Let's the browser processes this request.
                                return;
                            }
                            // dipslay drop indicator in Chrome
                            var caretRange = e.target.ownerDocument.caretRangeFromPoint(e.originalEvent.clientX, e.originalEvent.clientY);
                            if (caretRange && caretRange.startContainer) { // google chrome
                                var range = ed.selection.getRng();
                                range.setStart(caretRange.startContainer, caretRange.startOffset); // set text range as current cursor location
                                range.collapse(true);

                                ed.focus();

                                var selection = e.target.ownerDocument.getSelection();
                                selection.removeAllRanges();
                                selection.addRange(range);

                                // prevent default, to be able to trigger drop event in Chrome
                                e.preventDefault();
                            }
                        }
                    });
                    // Bind event handler to drop event of editor body.
                    $(ed.getBody()).bind("drop", function (e) {
                        var validImageFormats = /^\S+\.*(jpg|jpeg|jpe|ico|gif|bmp|png)$/;
                        var originalEvent = e.originalEvent;
                        if (originalEvent) {
                            var html = "";

                            // File manager (DhtmlSupport) drag event handler sets dataTransfer object setData("text/plain") to the path of file.
                            var data = originalEvent.dataTransfer.getData('Text');
                            // try to parse data to JSON object, to support new edit mode
                            try {
                                // if data is a JSON string, parse it to object, and assign the URL to data.
                                var dragData = tinymce.util.JSON.parse(data);
                                if (dragData) {
                                    data = dragData.data;
                                }
                            } catch (ex) { }

                            if (data && data.length && data.charAt(0) == "/") {
                                // If a valid web image format is dropped create an image element, otherwise a link to the file.
                                if (data.toLowerCase().match(validImageFormats, "i")) {
                                    html = '<img src="' + data + '" alt="" />';
                                } else {
                                    var text = data;
                                    var index = data.lastIndexOf("/");
                                    if (index > -1) {
                                        text = decodeURI(data.substring(index + 1, data.length));
                                    }
                                    html = '<a target="_blank" href="' + data + '">' + text + '</a>';
                                }

                                ed.focus();
                                var s = ed.selection;
                                // In gecko and Chrome (and possibly other non IE) it's necessary to change selection to where we actually have the drop indicator.
                                if (originalEvent.rangeParent) {
                                    var range = s.getRng();
                                    range.setStart(originalEvent.rangeParent, originalEvent.rangeOffset);
                                    range.collapse(true);
                                    s.setRng(range);
                                } else if (/chrome/.test(navigator.userAgent.toLowerCase())) { // check user agent to detect Chrome browser
                                    var caretRange = e.target.ownerDocument.caretRangeFromPoint(e.originalEvent.clientX, e.originalEvent.clientY);
                                    if (caretRange && caretRange.startContainer) {
                                        var range = s.getRng();
                                        range.setStart(caretRange.startContainer, caretRange.startOffset); // set text range as current cursor location
                                        range.collapse(true);
                                        s.setRng(range);
                                    }
                                }
                                s.setContent(html);
                                // Need to create undo level since it will not be created automatically in IE.
                                ed.undoManager.add();
                                return false;
                            }
                        }
                    });
                });
            }
        },

        /**
        * Returns information about the plugin as a name/value array.
        *
        * @return {Object} Name/value array containing information about the plugin.
        */
        getInfo: function () {
            return {
                longname: 'Drag & Drop from FileManager plugin',
                author: 'EPiServer AB',
                authorurl: 'http://www.episerver.com',
                infourl: 'http://www.episerver.com',
                version: "1.0"
            };
        }
    });

    // Register plugin
    tinymce.PluginManager.add('epifilemanagerdragdrop', tinymce.plugins.epifilemanagerdragdrop);
} (tinymce, epiJQuery));