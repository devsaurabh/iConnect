
var Chatter = function() {
    var connection;
    var chatHub = $.connection.chatHub;
    var self = this;

    
    self.currentUser = "";
    self.userAlias = "";
    self.emoticonImagePath = "../Content/Emoticons";
    self.emoticonUrl = "GetEmoticons";
    self.emoticons = null;

    self.init = function(currentUser,alias) {
        // set current logged in user
        self.currentUser = currentUser;
        self.userAlias = alias;

        // load emoticon parser and emoticons
        self.emoticons = new EmoticonParser(self.emoticonUrl, self.emoticonImagePath);
        self.emoticons.getEmoticonCodes();

        // connect to signalR server
        connection = $.connection.hub.start();
        self.connectToServer(currentUser);
        
        // publish all client side events
        self.publishEvents();
    }

    self.markUserOnline = function(userName) {
        var user = $(".list-group-item").find("[data-user='" + userName + "']");
        user.addClass("label label-success");
        user.html("Online");
    }

    self.markUserOffline = function(userName) {
        var user = $(".list-group-item").find("[data-user='" + userName + "']");
        user.removeClass("label label-success");
        user.html("");
    }

    self.publishEvents = function () {
        chatHub.client.onBroadcast = function (userName, message, alias) { amplify.publish("client-onBroadcast", userName,message, alias); };
        chatHub.client.onConnect = function (userName) { amplify.publish("client-onConnect",  userName); };
        chatHub.client.onDisconnect = function (userName) { amplify.publish("client-onDisconnect", userName ); }
        chatHub.client.onPrivate = function (userName,message) { amplify.publish("client-onPrivate", userName, message); }
    }

    self.connectToServer = function(userName) {
        connection.done(function() {
            chatHub.server.connect(userName)
                .done(function() {
                    self.markUserOnline(userName);
                    amplify.publish("server-connect", userName);
                })
                .fail(function() {
                    console.log("Unable to connect to server");
                });
        });
    }

    self.broadcast = function(message, callbackFunction) {
        connection.done(function() {
            message = self.emoticons.parseString(message);

            if (message.length > 0) {
                chatHub.server.broadcastToAll(message).done(function() {
                    amplify.publish("server-broadcast", message);
                    if (typeof callbackFunction == "function") callbackFunction();
                });
            }
        });
    };

    self.sendPrivate = function(sendTo, message) {
        connection.done(function() {
            if (sendTo === undefined) {
                amplify.publish("sendPrivate-validationerror");
            } else {
                message = self.emoticons.parseString(message);
                if (message.length > 0) {
                    chatHub.server.sendPrivateMessage(sendTo, message).done(function() {
                        amplify.publish("server-sendPrivate", sendTo, message);
                    }).fail(function(e) {
                        amplify.publish("server-sendPrivate-failed");
                    });
                }
            }
        });
    };

    self.Logout = function (userName) {
        connection.done(function() {
            chatHub.server.disconnect(userName).done(function() {
                amplify.publish("server-disconnect");
            });
        });
    }

    amplify.subscribe("client-onBroadcast", function (userName, message, alias) {
        noty({ text: alias + ' said: ' + message, type: "information", killer: true, theme: "relax" });
        return false;
    });

    amplify.subscribe("client-onConnect", function (userName) {
        self.markUserOnline(userName);
        return false;
    });

    amplify.subscribe("client-onDisconnect", function (userName) {
        self.markUserOffline(userName);
        return false;
    });

    amplify.subscribe("client-onPrivate", function (userName, message) {
        $('#ulChatMessages').append('<li><strong>' + userName + ": </strong>" + message + '</li>');
        return false;
    });

    amplify.subscribe("server-connect", function (userName) {
        //self.markUserOnline(userName);
    });

    amplify.subscribe("server-broadcast", function (messsage) {
        //Write code like saving broadcast messages on server
    });

    amplify.subscribe("sendPrivate-validationerror", function () {
        var warnMsg = '<strong style="color:red;">Please select the user from [All Users] list...!!!</strong>';

        var n = noty({
            text: warnMsg,
            type: "warning",
            killer: false,
            theme: "relax",
            speed: 500,
            timeout: 3000,
            onClose: function() {
                alert('Bye');
            }
        });

        var x = 0;
        var backgroundInterval = setInterval(function() {
            x++;
            $("li.active a").toggleClass("warningBlink");
            if (x == 20) clearInterval(backgroundInterval);
        }, 200);
        return false;
    });

    amplify.subscribe("server-sendPrivate", function (sendTo, message) {
        $('#ulChatMessages').append('<li><strong>You: </strong>' + message + '</li>');
        $(".chat-area").animate({ scrollTop: $('.chat-area')[0].scrollHeight }, 600);
        $('#txtPrivateMessage').val("");
        $('#txtPrivateMessage').focus();
        return false;
    });

    amplify.subscribe("server-sendPrivate-failed", function () {
        alert("Unable to send message. Something went wrong");
        return false;
    });
};

    


