var Base64 = {
    // private property
    _keyStr : "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=",
    
    // public method for encoding
    encode : function (input) {
        var output = "";
        var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
        var i = 0;
    
        input = Base64._utf8_encode(input);
    
        while (i < input.length) {
    
            chr1 = input.charCodeAt(i++);
            chr2 = input.charCodeAt(i++);
            chr3 = input.charCodeAt(i++);
    
            enc1 = chr1 >> 2;
            enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
            enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
            enc4 = chr3 & 63;
    
            if (isNaN(chr2)) {
                enc3 = enc4 = 64;
            } else if (isNaN(chr3)) {
                enc4 = 64;
            }
    
            output = output +
            Base64._keyStr.charAt(enc1) + Base64._keyStr.charAt(enc2) +
            Base64._keyStr.charAt(enc3) + Base64._keyStr.charAt(enc4);
    
        }
    
        return output;
    },
    
    
    // private method for UTF-8 encoding
    _utf8_encode : function (string) {
        string = string.replace(/\r\n/g,"\n");
        var utftext = "";
    
        for (var n = 0; n < string.length; n++) {
    
            var c = string.charCodeAt(n);
    
            if (c < 128) {
                utftext += String.fromCharCode(c);
            }
            else if((c > 127) && (c < 2048)) {
                utftext += String.fromCharCode((c >> 6) | 192);
                utftext += String.fromCharCode((c & 63) | 128);
            }
            else {
                utftext += String.fromCharCode((c >> 12) | 224);
                utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                utftext += String.fromCharCode((c & 63) | 128);
            }
        }
    
        return utftext;
    }
}

var fileLoadingHandler = null;

function setupDragAndDrop() {
	fileLoadingHandler = fileLoadingHandler || {};
	fileLoadingHandler.onDragOver = function(evt) {
	  evt.stopPropagation();
	  evt.preventDefault();
	}

	fileLoadingHandler.onDragFileDrop = function(evt) {
	  evt.stopPropagation();
	  evt.preventDefault();

	  var files = evt.dataTransfer.files;
	  var output = [];
	  for (var i = 0, f; f = files[i]; i++) {
		fileLoadingHandler.readFile(f);
	  }
	  document.getElementById('file_list2').innerHTML = '<ul>' + output.join('') + '</ul>';			
	}

	// Setup the dnd listeners.

	var drop_zone = document.getElementById('drop_zone');
	drop_zone.addEventListener('dragover', fileLoadingHandler.onDragOver, false);
	drop_zone.addEventListener('drop', fileLoadingHandler.onDragFileDrop, false);		
	
	fileLoadingHandler.readFile = function(file) {
		var reader = new FileReader();
		reader.onload = (function(theFile) {
			return function(e) {
				loadImageFromNet(e.target.result)
			};
		})(file);

		// Read in the image file as a data URL.
		reader.readAsDataURL(file);
	}
}


function showFileModificationTime()
{
    var lastmod = document.lastModified;
    var lastmoddate = Date.parse(lastmod);
    if (lastmoddate == 0) {               // unknown date (or January 1, 1970 GMT)
        document.getElementById('modifiedDate').innerHTML = "Last Modified: Unknown";
    } else {
        document.getElementById('modifiedDate').innerHTML = "Last Modified: " + lastmod;
    }
}

function performRequest(params) {

    //$('body,html').animate({
    //    scrollTop: 0
    //}, 800);

	var requestData = null;
	if("requestData" in params) {
		if(params["requestData"].constructor == String) {
        	requestData = params["requestData"];
		}
		else {
        	requestData = JSON.stringify(params["requestData"]);
		}
	}
	
    var requestElement  = document.getElementById('divRequest');
    requestElement.innerHTML = "";

    var responseElement = document.getElementById('divResponse');
    responseElement.innerHTML = "";
    
    var requestUrl = null;
    
    if("requestUrl" in params) {
        requestUrl = "http://dev-rombeservices.azurewebsites.net/api/" + params["requestUrl"];
    }
    else if("requestRelativeUrl" in params) {
        requestUrl = params["requestRelativeUrl"];
    }
    
   // var username = document.getElementById('field_username').value;
    //var password = document.getElementById('field_password').value;
    //var credentials = Base64.encode(username + ":" + password);
    
    requestElement.innerHTML += "Type: [" + params["requestType"] + "]\nUrl: [" + requestUrl + "]\nData: " + requestData + '\n';

    $.ajax({
        type: params["requestType"],
        url: requestUrl,
        data: requestData,
        dataType: "json",
        headers: {
                'Content-Type' : 'application/json; charset=utf-8',
                'Cache-Control' : 'no-cache',
                'X-Requested-With': 'XMLHttpRequest',
                'Access-Control-Allow-Origin' : '*'
        },
        beforeSend: function (req) {
            requestElement.innerHTML += ('Request: ' + JSON.stringify(req, null, 4))
        },
        success: function (data, code, jqXHR) { 
            var obj = jQuery.parseJSON( jqXHR.responseText );
            responseElement.innerHTML += "ResponseText: " + JSON.stringify(obj, null, 4) + "\n";
            responseElement.innerHTML += "Code: " + code + "\n";
            responseElement.innerHTML += "Data: " + data + "\n";
        },
        error: function(jqXHR, textStatus, errorThrown) {
            $("#divResponse").html(jqXHR.responseText); 
        }
    }).fail(function () { 
    });
}

function createGUID() {
    var S4 = function() {
       return (((1+Math.random())*0x10000)|0).toString(16).substring(1);
    };
    //return (S4()+S4()+"-"+S4()+"-"+S4()+"-"+S4()+"-"+S4()+S4()+S4());
    return (S4()+S4()+S4()+S4()+S4()+S4()+S4()+S4());
}
