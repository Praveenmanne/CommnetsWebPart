$(function () {
    $('.CommentBox:odd').addClass('BoxEven');
    $(".submitComment").click(function () {
        var comment = $(".CommentInputBox").val();
        var dataString = 'comment=' + comment + "&WebPartId=" + $('.CommentBoxContainer [id$=HdnWebPartID]').val();
        if (comment == '') {
           // alert('Please input valid comment');
        }
        else {
            $.ajax({
                type: "POST",
                url: "/_layouts/TillidSocialNetworking/PostComment.aspx",
                data: dataString,
                cache: false,
                beforeSend: function () {
                    $("#flash").show();
                    $("#flash").fadeIn(400);
                    $(".submitComment").attr('disabled', 'disabled');
                    $(".submitComment").val('Posting Comment');
                },
                success: function (html) {
                    var elements = $(html).find('.CommentBox');
                    elements.hide();
                    $('.CommentsHolder').append(elements);
                    elements.fadeIn(500);
                    $('.CommentBox:odd').addClass('BoxEven');
                    $(".CommentInputBox").html('');
                    $(".submitComment").removeAttr('disabled');
                    $(".submitComment").val('Post Comment');
                    $("#flash").hide();
                },
                error: function (e) {
                }
            });
        }
        return false;
    });
});