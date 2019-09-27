var mainLayout_initialFrameSrc;

function initIframe() {
    Sys.Application.add_navigate(onStateChanged);

    var iframe = document.getElementById("right");
    if (iframe) {
        if (iframe.addEventListener) {
            iframe.addEventListener("load", iframeSrcChangeHandler, false);
        }
        else {
            iframe.attachEvent("onload", iframeSrcChangeHandler);
        }
    }
}

function onStateChanged(sender, e) {
    var val = e.get_state().right || mainLayout_initialFrameSrc;
    // Checking state url even load new url
    val = validateUrl(unescapeWithAmp(val));

    var currentFrameSrc = window.frames["right"].document.location.href;
    if (val.toLowerCase() != currentFrameSrc.toLowerCase()) {
        var iframe = document.getElementById("right");
        iframe.setAttribute("src", val);
    }
}

function iframeSrcChangeHandler(e) {
    var currentState = Sys.Application._state.right;
    if (!currentState)
        currentState = "";
    else
        currentState = unescapeWithAmp(currentState);
    try {
        var newFrameSrc = window.frames["right"].document.location.href;
        if (newFrameSrc.toLowerCase() != currentState.toLowerCase())
            Sys.Application.addHistoryPoint({ right: escapeWithAmp(newFrameSrc) });
    }
    catch (e) { }
}

function mainLayout_initialize() {
    var val = Sys.Application._state.right || mainLayout_initialFrameSrc;
    val = unescapeWithAmp(val);

    var currentFrameSrc = window.frames["right"].document.location.href;
    if (val.toLowerCase() != currentFrameSrc.toLowerCase()) {
        var iframe = document.getElementById("right");
        iframe.setAttribute("src", val);
    }
}

function unescapeWithAmp(str) {
    var re = /%26/gi;
    var ampDecoded = "&";
    return unescape(str).replace(re, ampDecoded);
}

function escapeWithAmp(str) {
    var re = /&/gi;
    var ampEncoded = "%26";
    return escape(str.replace(re, ampEncoded));
}

// Need to validate hash url input to iframe, to avoid cross-domain and phishing attack.
function validateUrl(url) {
    if (url) {
        var siteLocation = document.location;
        var unescapedUrl = unescapeWithAmp(url);
        // Need use 'a' element to parse href because IE is not support new URL instance.
        var frameSrc = document.createElement('a');
        frameSrc.href = unescapedUrl;
        
        // Checking domain name and protocol of hash url match with local site.
        if (siteLocation.protocol.toLowerCase() === frameSrc.protocol.toLowerCase() &&
            siteLocation.hostname.toLowerCase() === frameSrc.hostname.toLowerCase()) {
            return unescapedUrl;
        }
    }

    return "";
}