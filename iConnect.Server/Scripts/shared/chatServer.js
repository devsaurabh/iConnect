var ChatHubServer = function (connectionObject) {
    var self = this;
    var chatHub = $.connection.chatHub;

    self.connectToServer = function (userName) {
        connectionObject.done(function () {
            markOnline(userName);
            chatHub.server.connect(userName).
                fail(function () {
                console.log("Unable to connect to server");
            });
        });
    }

    self.broadcast = function (message, callbackFunction) {
        message = emoticons.parseString(message);
        if (message.length > 0) {
            chatHub.server.broadcastToAll(message).done(function () {
                $('#ulChatMessages').append('<li><strong>You: </strong>' + message + '</li>');
                $(".chat-area").animate({ scrollTop: $('.chat-area')[0].scrollHeight }, 600);
                $('#txtMessage').val("");
                $('#txtMessage').focus();
                if (typeof callbackFunction == "function") callbackFunction();
            });
        }
    };

    self.sendPrivate = function (sendTo) {
        alert(sendTo);
        var message = $('#txtPrivateMessage').val();
        message = emoticons.parseString(message);
        console.log(message);
        if (message.length > 0) {
            chatHub.server.sendPrivateMessage(sendTo, message).done(function () {
                $('#ulChatMessages').append('<li><strong>You: </strong>' + message + '</li>');
                $(".chat-area").animate({ scrollTop: $('.chat-area')[0].scrollHeight }, 600);
                $('#txtPrivateMessage').val("");
                $('#txtPrivateMessage').focus();
                //if (typeof callbackFunction == "function") callbackFunction();
            });
        }
    }

    

    self.Logout = function (userName) {
        chatHub.server.disconnect(userName);
        markOffline(userName);
    }
}