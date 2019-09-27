<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditorControl.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Core.Controls.Editors.TinyMCE.EditorControl" %>
<script type="text/javascript">
    tinyMCE.init({
        mode : "exact",
        elements : "<%=tiny.ClientID %>",
        theme: "advanced",
        dialog_type: "window",
        plugins: "media,paste,searchreplace,fullscreen,advimage",
        skin: "epi-light",
        convert_urls : false,
        entitie : "160,nbsp",
        theme_advanced_container_statusBar: "mceElementpath",
        theme_advanced_statusbar_location : "bottom",
        theme_advanced_toolbar_align : "left",
        theme_advanced_resizing : true,
        theme_advanced_path : true,
        theme_advanced_toolbar_location : "top",
        theme_advanced_buttons1 : "bold, italic, separator, bullist, numlist, styleselect, undo, redo, separator, image, media, separator, cut, copy, paste, pastetext, pasteword, separator, search, fullscreen,code",
        theme_advanced_buttons2 : "",
        theme_advanced_buttons3 : "",
        /* extended_valid_elements is added so that TinyMCE doesnt strip CMS content fragment attributes from divs*/
        extended_valid_elements: "div[align<center?justify?left?right|class|dir<ltr?rtl|id|lang|onclick|ondblclick|onkeydown|onkeypress|onkeyup|onmousedown|onmousemove|onmouseout|onmouseover|onmouseup|style|title|data-classid|data-contentguid|data-contentname|data-contentlink|data-epi-content-display-option|data-dynamicclass|data-state|data-hash|data-groups|data-contentgroup]",
        setup: function (ed) {
            ed.onSaveContent.add(function(ed, o) {
                //cleaning line breaks so that CMS content fragment validation doesn't fail
                o.content = o.content.replace(/(\r\n|\n|\r)/gm, "");
            });
        }
	});
</script>
<asp:TextBox ID="tiny" runat="server" TextMode="MultiLine"></asp:TextBox>
