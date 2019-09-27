/**
 * $Id: editor_plugin_src.js 686 2008-03-09 18:13:49Z spocke $
 *
 * @author Moxiecode
 * @copyright Copyright © 2004-2008, Moxiecode Systems AB, All rights reserved.
 * MODIFIED by EPiServer AB. Search/Replace plugin modified to work with non editable content.
 */

(function(tinymce, $) {
	tinymce.create('tinymce.plugins.episearchreplace', {
		init : function(ed, url) {
			function open(m) {
				ed.windowManager.open({
					file : url + '/searchreplace.htm',
					width : 420 + parseInt(ed.getLang('searchreplace.delta_width', 0)),
					height : 183 + parseInt(ed.getLang('searchreplace.delta_height', 0)),
					inline : 1,
					auto_focus : 0
				}, {
					mode : m,
					search_string : ed.selection.getContent({format : 'text'}),
					plugin_url : url
				});
			};

			// Register commands
			ed.addCommand('mceSearch', function() {
				open('search');
			});

			ed.addCommand('mceReplace', function() {
				open('replace');
			});

			// Register buttons
			ed.addButton('search', {title : 'searchreplace.search_desc', cmd : 'mceSearch'});
			ed.addButton('replace', {title : 'searchreplace.replace_desc', cmd : 'mceReplace'});

			ed.addShortcut('ctrl+f', 'searchreplace.search_desc', 'mceSearch');
		},

		getInfo : function() {
			return {
				longname : 'Search/Replace modified by EPiServer',
				author : 'Moxiecode Systems AB/EPiServer AB',
				authorurl : 'http://www.episerver.com',
				infourl: 'http://www.episerver.com',
				version : tinymce.majorVersion + "." + tinymce.minorVersion
			};
		}
	});

	// Register plugin
	tinymce.PluginManager.add('episearchreplace', tinymce.plugins.episearchreplace);
}(tinymce, epiJQuery));