/* DM.crossdomain 
 * - provides utility functions for crossdomain support, specifically postMessage and receiveMessage
 * 
 * Code inspired by Ben Alman's jQuery postMessage and Josh Fraser's notes on a Backwards compatible
 * window.postMessage.
 * 
 * All Rights Reserved - Morningstar, Inc.
 */

// Create DM namespace if it doesn't already exist
var DM = DM || {};

// Create crossdomain utilities
DM.crossdomain = function () {
    var self = new Object(),
		attached_callback,
		cache_bust = 1,
		hashTagRegExp = /#.*$/,
		hostNameRegExp = /([^:]+:\/\/[^\/]+).*/;

    self.postMessage = function (message, target_url, target) {
        // target url is needed to use the postMessage functionality
        if (!target_url) return;

        // use target if defined or defaults to parent
        target = target || parent;

        // post the message
        if (window['postMessage']) {
            target['postMessage'](message, target_url.replace(hostNameRegExp, '$1'));
        } else {
            target.location = target_url.replace(hashTagRegExp, '') + '#' + (+new Date) + (cache_bust++) + '&' + message;
        }
    };

    self.receiveMessage = function (callback, source_origin) {
        var hostNameRegExp = /([^:]+:\/\/[^\/]+).*/;

        if (window['postMessage']) {
            if (callback) {
                attached_callback = function (e) {
                    // ensure we're comparing only the hostname and not the full uri
                    source_origin = source_origin.replace(hostNameRegExp, '$1');

                    // don't issue the callback if the source origin doesn't match
                    if ((typeof source_origin === 'string' && e.origin !== source_origin)
                    || (Object.prototype.toString.call(source_origin) === "[object Function]" && source_origin(e.origin) === false)) {
                        return false;
                    }
                    callback(e);
                };
            }

            if (window['addEventListener']) {
                window[callback ? 'addEventListener' : 'removeEventListener']('message', attached_callback, false);
            } else {
                window[callback ? 'attachEvent' : 'detachEvent']('onmessage', attached_callback);
            }
        } else {
            // a polling loop is started & callback is called whenever the location.hash changes
            hashPoller(callback, document.location.hash);
        }
    };

    /* 
	 * Private Functions
	 */

    // hashPoller - used instead of setInterval so that callbacks to receiveMessage poll the document.location.hash 
    // less aggressively in all browsers
    var hashPoller = function (callback, last_hash) {
        var hash = document.location.hash,
  			hashRegExp = /^#?\d+&/,
  			delay = 500;

        if (hash !== last_hash && hashRegExp.test(hash)) {
            last_hash = hash;
            callback({ data: hash.replace(hashRegExp, '') });
        }

        setTimeout(function () {
            hashPoller(callback, last_hash);
        }, delay);
    };

    return {
        postMessage: self.postMessage,
        receiveMessage: self.receiveMessage
    }
}();