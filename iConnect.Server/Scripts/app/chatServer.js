var chatHubServer = function (connectionObject) {
    var self = this;

    self.connectToServer = function (userName) {
        connectionObject.done(function () {
            markOnline(userName);
            chatHub.server.connect(userName).
                fail(function () {
                console.log("Unable to connect to server");
            });
        });
    }

    self.sendMessage = function () {
        var message = $('#txtMessage').val();

        if (message.length > 0) {
            chatHub.server.broadcastToAll(message);
            message = message.replace(":(", "<img src='/iConnect/Content/Emoticons/sad_smile.png' style='width: 24px' />");

            $('#ulChatMessages').append('<li><strong>You: </strong>' + message + '</li>');            
            $(".chat-area").animate({ scrollTop: $('.chat-area')[0].scrollHeight }, 600);
            $('#txtMessage').val("");
            $('#txtMessage').focus();
        }
    }

    self.Logout = function (userName) {
        chatHub.server.disconnect(userName);
        markOffline(userName);
    }
}