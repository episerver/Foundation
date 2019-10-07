var page = require('webpage').create(),
    system = require('system'),
    pageLink = system.args[1],
    pageImage = system.args[2],
    authCookie = system.args[4].split('|'),
    cookieCount = system.args.length - 4;

if (system.args.length === 1) {
    phantom.exit(1);
} else {
    var cookieValues = system.args.splice(4, cookieCount);

    cookieValues.forEach(function (value, i) {
        phantom.addCookie({
            "name": value.split(';')[0],
            "value": value.split(';')[1],
            "domain":system.args[3]
        });
    });
}

//Force process to exit when encountering an error. Prevents process from freezing.
phantom.onError = function () {
    phantom.exit(1);
};

page.viewportSize = { width: 1024, height: 768 };

page.open(pageLink, function () { });

page.onLoadFinished = function () {
    var me = this;
    setTimeout(function () {
        page.zoomFactor = 0.75;
        page.render(pageImage);
        phantom.exit();
    }, 1000);
};