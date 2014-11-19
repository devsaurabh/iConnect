var loggedInUser = "";
var chatHub = $.connection.chatHub;
var connection;
var chatServer;

// Common Methods
$(function () {
    connection = $.connection.hub.start();
    chatServer = new chatHubServer(connection);    
});

function markOnline(userName) {
    var user = $(".list-group-item").find("[data-user='" + userName + "']");
    user.addClass("label label-success");
    user.html("Online");
}

function markOffline(userName) {
    var user = $(".list-group-item").find("[data-user='" + userName + "']");
    user.removeClass("label label-success");
    user.html("");
}
















