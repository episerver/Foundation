var CSManagementClient = null;
ManagementClientProxy_GetMC();

function escapeWithAmp(str) {
    var re = /&/gi;
    var ampEncoded = "%26";
    return escape(str.replace(re, ampEncoded));
}

function ManagementClientProxy_GetMC() {
try {
    var win = window;
        while (win.parent != null) {
            if (typeof (win.GetManagementClient) != "undefined") {
            CSManagementClient = win.GetManagementClient();
                break;
        }
            if (win == win.parent)
                break;
        win = win.parent;
       }
//03.03.2011 ET. Comment. Do not working OpenWindow commands.
//    if (CSManagementClient == null) {
//            // TODO: handle session timeout (redirect after login page)
//            //alert('ManagementClientProxy_GetMC: CSManagementClient = null!!! ');
//            var newPath = win.location.pathname.substr(0, win.location.pathname.lastIndexOf('/'));
//            alert('location.href=' + location.href);
//            var rightUrl = '#right=' + encodeURIComponent(escapeWithAmp(location.href));
//            win.location.href = newPath + "/default.aspx" + rightUrl;
//        }
    
    CSManagementClient.IsPageDirty = false;
}
    catch (ex) {
    }
}
