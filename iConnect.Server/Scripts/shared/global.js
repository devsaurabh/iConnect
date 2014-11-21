var loggedInUser = "";

var connection;

// Common Methods
$(function () {
    connection = $.connection.hub.start();
    
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
















