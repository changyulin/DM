RegisterNamespace("DM.Util.Xml");

DM.Util.Xml.loadXMLString = function (txt) {
    if (window.DOMParser) {
        parser = new DOMParser();
        xmlDoc = parser.parseFromString(txt, "text/xml");
    }
    else // Internet Explorer
    {
        xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
        xmlDoc.async = "false";
        xmlDoc.loadXML(txt);
    }
    return xmlDoc;
}

DM.Util.Xml.formatXML = function (dom) {
    var str;
    if (window.XMLSerializer) {
        var serializer = new XMLSerializer();
        str = serializer.serializeToString(dom);
    }
    else {
        str = dom.xml;
    }
    return str;
};
