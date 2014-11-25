$(function () {
    var chatHub = $.connection.chatHub;

    chatHub.client.onBroadcast = function (userName, message, alias) {
        var n = noty({ text: alias + ' said: ' + message, type: "information", killer: true, theme: "relax" });
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
});