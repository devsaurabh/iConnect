

var loggedInUser = "";

var connection;

// Common Methods
$(function () {
    connection = $.connection.hub.start();
    




$('#broadcast-alert').on('closed.bs.alert', function () {
    $(body).remove($("#broadcast-alert"));
    // do something…
});

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

function showAlert(fromUser, message) {
    var div = '<div class="alert alert-success alert-dismissible" role="alert" id="broadcast-alert">' +
              '<button type="button" class="close" data-dismiss="alert">' +
              '<span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button><strong>' + fromUser +
         + '</strong>' + message + '</div>';
    $('body').prepend(div);
    $(div).show();
}











