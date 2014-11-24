$(function () {
    var chatHub = $.connection.chatHub;

    chatHub.client.onBroadcast = function (userName, message, alias) {
        var n = noty({ text: alias + 'said: ' + message, theme: "relax", type: "success", killer: true });
        
    }

    chatHub.client.onConnect = function (userName) {
        markOnline(userName);
    };

    chatHub.client.onPrivate = function (userName, message) {
        $('#ulChatMessages').append('<li><strong>' + userName + ": </strong>" + message + '</li>');
    };

    chatHub.client.onDisconnect = function (userName) {
        markOffline(userName);
    };

    //chatHub.client.onPrivate = function (userName, message) {
    //    alert(userName);
    //    showAlert(userName, message);
    //}

    
});