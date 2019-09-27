(function (tinymce, $) {
    /**
    * EPiServer Window Manager, extends default WindowManager to use EPi.Dialog functionality.
    */
    tinymce.create('tinymce.plugins.epiWindowManager', {
        init: function (ed, url) {
            // Replace default window manager with our own.
            ed.onBeforeRenderUI.add(function () {
                ed.windowManager = new tinymce.epiWindowManager(ed);
            });
        },

        getInfo: function () {
            return {
                longname: 'EPiServer Window Manager',
                author: 'EPiServer AB',
                authorurl: 'http://www.episerver.com',
                infourl: 'http://www.episerver.com',
                version: "1.0"
            };
        }
    });

    tinymce.create('tinymce.epiWindowManager:tinymce.WindowManager', {
        epiWindowManager: function (ed) {
            this.parent(ed);
        },
        /**
        * Opens a new window.
        *
        * @method open
        * @param {Object} f Optional name/value settings collection contains things like width/height/url etc.
        * @param {Object} p Optional parameters/arguments collection can be used by the dialogs to retrive custom parameters.
        */
        open: function (f, p) {
            var t = this, ed = t.editor, u;

            f = f || {};
            p = p || {};

            // Only store selection if the type is a normal window
            if (!f.type) {
                t.bookmark = ed.selection.getBookmark(1);
            }

            f.width = parseInt(f.width || 320) + (tinymce.isIE ? 18 : 0);
            f.height = parseInt(f.height || 240) + (tinymce.isIE ? 8 : 0);

            p.mce_inline = true; // Necessary to avoid loosing selection in editor.
            p.mce_auto_focus = f.auto_focus; // Auto focus field in dialog

            u = f.url || f.file;
            t.features = f;
            t.params = p;
            t.onOpen.dispatch(t, f, p);

            // Since dialog is opened in top window we need to set up necessary reference (used by popup script).
            window.top.tinymce = tinymce;
            window.top.tinyMCE = tinyMCE;

            var self = this;

            var closeCallBack = function() {
                self.onClose.dispatch(this);
                ed.windowManager.onClose.dispatch();
            };

            ed.windowManager.onOpen.dispatch();
            EPi.CreateDialog(u, closeCallBack, null, null, f);

        },

        /**
        * Closes the specified window and corresponding dialog. This will also dispatch out an onClose event.
        *
        * @method close
        * @param {Window} win Native window object to close.
        */
        close: function (win) {
            // Pass in dialog window object to get hold of correct EPi.Dialog object
            // and call Close to clean up and close dialog.
            EPi.GetDialog(win).Close();
        },

        /**
        * Resizes the specified window or id.
        *
        * @param {Number} dw Delta width.
        * @param {Number} dh Delta height.
        * @param {window/id} win Window if the dialog isn't inline. Id if the dialog is inline.
        */
        resizeBy: function (dw, dh, win) {
            // resizeBy is called when a dialog is opened.
            // Calculate and set new height of dialog according to actual height of content.
            // Gecko browser needs a timeout.

            win.setTimeout(function () {
                var doc = win.document;
                if (doc == null || doc.body == null) {
                    return;
                }

                var originalHeight = doc.documentElement.clientHeight !== 0 ? doc.documentElement.clientHeight : doc.documentElement.offsetHeight;
                var body = $(doc.body);

                var additionalHeight = tinymce.isIE ? 15 : 20;
                var heightChange = body.outerHeight() + body.offset().top + additionalHeight - originalHeight;

                win.resizeBy(0, heightChange > 0 ? heightChange : 0);
            }, 10);
        },

        setTitle: function (w, ti) {
            // Not implemented but necessary to make TinyMCE Popup script work,
            // since we set mce_inline to true to keep selection in editor.
        },

        focus: function (w) {
            // Not implemented but necessary to make TinyMCE Popup script work,
            // since we set mce_inline to true to keep selection in editor.
        }
    });

    // Register plugin
    tinymce.PluginManager.add('epiwindowmanager', tinymce.plugins.epiWindowManager);
} (tinymce, epiJQuery));