export default class Review {
    ratingHover() {
        $('.rating.voting').each(function (i, e) {
            $(e).find('svg').each(function (j, s) {
                $(s).hover(function () {
                    for (let index = 0; index <= j; index++) {
                        $($(e).find('svg')[index]).css('fill', 'black');
                    }

                    for (let index = $(e).find('svg').length - 1; index > j; index--) {
                        $($(e).find('svg')[index]).css('fill', 'none');
                    }
                });
            });
        });

        $('.rating.voting').each(function (i, e) {
            $(e).mouseleave(function () {
                $(this).find('svg').each(function (i, e) {
                    $(e).removeAttr('style');
                });
            });
        });
    }

    ratingClick() {
        $('.rating.voting').each(function (i, e) {
            $(e).find('svg').each(function (j, s) {
                $(s).click(function () {
                    $(this).parents('.rating.voting').first().removeClass('rate-1');
                    $(this).parents('.rating.voting').first().removeClass('rate-2');
                    $(this).parents('.rating.voting').first().removeClass('rate-3');
                    $(this).parents('.rating.voting').first().removeClass('rate-4');
                    $(this).parents('.rating.voting').first().removeClass('rate-5');
                    $(this).parents('.rating.voting').first().addClass('rate-' + (j + 1));
                    $(this).parents('.rating.voting').first().attr('rate', (j + 1));
                });
            });
        });
    }

    submitReview() {
        var inst = this;
        $('#submitReview').click(function () {
            var rate = $('.rating.voting').attr('rate');
            var code = $('#ProductCode').val();
            var title = $('#Title').val();
            var nickname = $('#Nickname').val();
            var location = $('#Location').val();
            var content = $('#Body').val();
            var data = {
                ProductCode: code,
                Title: title,
                Nickname: nickname,
                Location: location,
                Body: content,
                Rating: rate
            };
            if (inst.validateReview(data)) {
                $('.loading-box').show();
                var form = $(this).closest("form");
                axios.post(form[0].action, convertFormData(data))
                    .then(function (result) {
                        if (result.status == 200) {
                            notification.success("You have added a comment to " + code);
                            $('#reviewsListing').append(result.data);
                            feather.replace();
                        }
                    })
                    .catch(function (error) {
                        notification.error(error.response.statusText);
                    })
                    .finally(function () {
                        $('.loading-box').hide();
                    });
            }
            return false;
        });
    }

    validateReview(data) {
        var isValid = true;
        if (data) {
            isValid = this.validateFiled(data, "Nickname", isValid);
            isValid = this.validateFiled(data, "Title", isValid);
            isValid = this.validateFiled(data, "Location", isValid);
            isValid = this.validateFiled(data, "Body", isValid, "Review");

            if (!($('.rating.voting').attr('rate'))) {
                $('.error[for="Rating"]').html('Rating is required.');
                isValid = false;
            } else {
                $('.error[for="Rating"]').html('');
            }
        } else {
            isValid = false;
        }

        return isValid;
    }

    validateFiled(data, fieldName, isValid, labelName) {
        if (!data[fieldName] || data[fieldName].trim() == "") {
            labelName = labelName == undefined ? fieldName : labelName;
            $('.error[for="' + fieldName + '"]').html(labelName + ' is required.');
            isValid = false;
        } else {
            $('.error[for="' + fieldName + '"]').html('');
        }

        return isValid;
    }
}