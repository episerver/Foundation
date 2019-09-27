class MyProfile {

    saveProfile(options) {
        $('.loading-box').show();
        axios(options)
            .then(function (result) {
                $('.jsFirstName').html(result.data.FirstName);
                $('.jsLastName').html(result.data.LastName);
                notification.Success("Update profile successfully.");
            })
            .catch(function (error) {
                notification.Error(error);
            })
            .finally(function () {
                $('.loading-box').hide();
            })
    }

    EditProfileClick() {
        $('.jsEditProfile').each(function (i, e) {
            $(e).click(function () {
                var targetSelector = $(this).data('target');
                $(targetSelector).slideToggle();
            })
        })
    }


    SaveProfileClick() {
        var inst = this;
        $('.jsSaveProfile').click(function () {
            var container = $(this).parents('.jsProfileContainerEdit').first();
            var firstName = $(container).find('.jsProfileFirstNameEdit').first().val();
            var lastName = $(container).find('.jsProfileLastNameEdit').first().val();
            var birth = $(container).find('.jsProfileBirthDateEdit').first().val();
            var newsLetter = $(container).find('.jsProfileNewsLetterEdit').first().is(':checked');
            var token = $(container).find('.jsTokenProfileEdit').first().find('input').first().val();

            var data = new FormData();
            data.append("FirstName", firstName)
            data.append("LastName", lastName)
            data.append("DateOfBirth", birth)
            data.append("SubscribesToNewsletter", newsLetter)
            data.append("__RequestVerificationToken", token)

            var options = {
                method: 'post',
                headers: { 'content-type': 'application/x-www-form-urlencoded; charset=utf-8' },
                data: data,
                url: $(this).closest('form')[0].action
            }

            inst.saveProfile(options);
            $(this).parents('.jsProfileContainerEdit').first().fadeToggle();

            return false;
        })
    }
}