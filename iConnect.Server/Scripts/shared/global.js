$(function () {

    $('a.modal-link').click(function () {
        var url = $(this).data("modal");
        $.get(url, function (data) {
            $('#modal').html(data);

            $('#modal').modal('show');
            $('body').removeClass("modal-open");
        });
    });
});