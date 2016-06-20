
window.onload = function() {
	
	document.getElementById('fromText').value = getParameterByName("from");
	document.getElementById('countText').value = getParameterByName("count");

};

function getSample () {
	
	var f = document.getElementById('fromText').value
	var c = document.getElementById('countText').value
	
	window.location = '../sample?from=' + f + '&count=' + c;
	
}

function getParameterByName(name) {
	
    var match = RegExp('[?&]' + name + '=([^&]*)').exec(window.location.search);
	
    return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
	
}