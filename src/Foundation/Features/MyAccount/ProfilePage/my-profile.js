export default class MyProfile {
    saveProfile(options) {
        $('.loading-box').show();
        axios(options)
            .then(function (result) {
                $('.jsFirstName').html(result.data.FirstName);
                $('.jsLastName').html(result.data.LastName);
                notification.success("Update profile successfully.");
            })
            .catch(function (error) {
                notification.error(error);
            })
            .finally(function () {
                $('.loading-box').hide();
            })
    }

    editProfileClick() {
        $('.jsEditProfile').each(function (i, e) {
            $(e).click(function () {
                let targetSelector = $(this).data('target');
                $(targetSelector).slideToggle();
            })
        })
    }

    saveProfileClick() {
        let inst = this;
        $('.jsSaveProfile').click(function () {
            let container = $(this).parents('.jsProfileContainerEdit').first();
            let firstName = $(container).find('.jsProfileFirstNameEdit').first().val();
            let lastName = $(container).find('.jsProfileLastNameEdit').first().val();
            let birth = $(container).find('.jsProfileBirthDateEdit').first().val();
            let newsLetter = $(container).find('.jsProfileNewsLetterEdit').first().is(':checked');
            let token = $(container).find('.jsTokenProfileEdit').first().find('input').first().val();

            let data = new FormData();
            data.append("FirstName", firstName)
            data.append("LastName", lastName)
            data.append("DateOfBirth", birth)
            data.append("SubscribesToNewsletter", newsLetter)
            data.append("__RequestVerificationToken", token)

            let options = {
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