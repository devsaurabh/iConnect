$(function () {
    // Client-Side Events
    chatHub.client.onBroadcast = function (UserName, message, Alias) {
        $('#ulChatMessages').append('<li><strong>' + Alias + ": </strong>" + message + '</li>');
        $(".chat-area").animate({ scrollTop: $('.chat-area')[0].scrollHeight }, 600);
    }

    chatHub.client.onConnect = function (userName) {
        markOnline(userName);
    };

    chatHub.client.onPrivate = function (userName, message) {
        $('#response').append('<p>' + userName + 'said:' + message + '</p>');
    };

    chatHub.client.onDisconnect = function (userName) {
        markOffline(userName);
    };    
});