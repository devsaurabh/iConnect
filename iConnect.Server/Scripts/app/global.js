// Declare a proxy to reference the hub. 
$(function () {

    var chatHub = $.connection.chatHub;

    chatHub.client.onConnect = function (userName) {
        $('#response').append('<p>' + userName + ' connected</p>');
    };

    chatHub.client.onDisconnect = function (userName) {
        $('#response').append('<p>' + userName + ' is offline</p>');
    };
 
    $.connection.hub.start().done(function () {
      
        chatHub.server.connect("sysadmin@cardinalts.com");
    });
});



