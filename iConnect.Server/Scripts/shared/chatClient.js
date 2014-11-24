$(function () {
    var chatHub = $.connection.chatHub;

    chatHub.client.onBroadcast = function (userName, message, alias) {
        //$('#ulChatMessages').append('<li><strong>' + alias + ": </strong>" + message + '</li>');
        //$(".chat-area").animate({ scrollTop: $('.chat-area')[0].scrollHeight }, 600);
        showAlert(userName, message);
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