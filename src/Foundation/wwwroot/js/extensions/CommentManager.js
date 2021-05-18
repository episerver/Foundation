var CommentManager = {
    init: function () {
        $('.grid-icon__approve')
            .on('click', CommentManager.approve);
        $('.grid-icon__delete')
            .on('click', CommentManager.delete);
    },
    approve: function (e) {
        var c = confirm("Are you sure you want to approve a comment?");
        if (c === true) {
            $('.grid-icon__loading').show();
            $.ajax({
                url: '/moderation/Approve',
                type: 'POST',
                data: {
                    id: e.currentTarget.attributes["commentId"].value
                },
                success: function (result) {
                    location.reload();
                    $('.grid-icon__loading').hide();
                },
                error: function (result) {
                    $('.grid-icon__loading').hide();
                }
            });
        }
    },
    delete: function (e) {
        var c = confirm("Are you sure you want to delete a comment?");
        if (c === true) {
            $('.grid-icon__loading').show();
            $.ajax({
                url: '/moderation/Delete',
                type: 'POST',
                dataType: 'text',
                data: {
                    id: e.currentTarget.attributes["commentId"].value
                },
                success: function (result) {
                    location.reload();
                    $('.grid-icon__loading').hide();
                },
                error: function (result) {
                    $('.grid-icon__loading').hide();
                }
            });
        }
    }
};