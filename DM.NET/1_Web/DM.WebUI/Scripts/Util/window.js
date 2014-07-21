RegisterNamespace("DM.Util.Window");

var mobileDevices = "ipad|ipod|iphone|android";

DM.Util.isMobileDevice = function () {
    var uagent = navigator.userAgent.toLowerCase();
    var mobileArray = mobileDevices.split("|");
    for (var i = 0; i < mobileArray.length; i++) {
        if (uagent.search(mobileArray[i]) > -1)
            return true;
    }
    return false;
};

DM.Util.isAndroid = function () {
    return navigator.userAgent.toLowerCase().search("android") != -1;
};

DM.Util.isIPad = function () {
    return navigator.platform.toLowerCase().indexOf("ipad") != -1;
};