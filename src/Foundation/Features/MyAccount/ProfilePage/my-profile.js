import * as $ from "jquery";
import * as axios from "axios";

export default class MyProfile {
    saveProfile(options) {
        document.querySelector(".loading-box").style.display = 'block'; // show
        axios.post(options.url, options.data)
            .then(function (result) {
                document.querySelector('.jsFirstName').innerHTML = result.data.firstName;
                document.querySelector('.jsLastName').innerHTML = result.data.lastName;
                notification.success("Update profile successfully.");
            })
            .catch(function (error) {
                notification.error(error);
            })
            .finally(function () {
                document.querySelector(".loading-box").style.display = 'none'; // show
            })
    }

    editProfileClick() {
        let inst = this;
        Array.from(document.getElementsByClassName("jsEditProfile")).forEach(function (el, i) {
            el.addEventListener("click", function () {
/*                ToggleEditProfile() {*/
                    let targetSelector = ".jsProfileContainerEdit"; //$(this).data('bs-target');
                    let container = document.querySelector(targetSelector);

                    if (!container.classList.contains('active')) {
                        container.classList.add('active');
                        container.style.height = 'auto';

                        let height = container.clientHeight + "px";

                        container.style.height = '0px';

                        setTimeout(function () {
                            container.style.height = height;
                        }, 0);
                    } else {
                        container.style.height = '0px';

                        container.addEventListener('transitionend', function () {
                            container.classList.remove('active');
                        }, {
                            once: true
                        });
                    }
                //}
            })
        })
    }

    ToggleEditProfile() {
        let targetSelector = ".jsProfileContainerEdit"; //$(this).data('bs-target');
        let container = document.querySelector(targetSelector);

        if (!container.classList.contains('active')) {
            container.classList.add('active');
            container.style.height = 'auto';

            let height = container.clientHeight + "px";

            container.style.height = '0px';

            setTimeout(function () {
                container.style.height = height;
            }, 0);
        } else {
            container.style.height = '0px';

            container.addEventListener('transitionend', function () {
                container.classList.remove('active');
            }, {
                once: true
            });
        }
    }

    saveProfileClick() {
        let inst = this;
        Array.from(document.getElementsByClassName("jsSaveProfile")).forEach(function (el, i) {
            el.addEventListener("click", function () {
                let container = document.querySelector('.jsProfileContainerEdit');
                let firstName = document.querySelector('.jsProfileFirstNameEdit').value;
                let lastName = document.querySelector('.jsProfileLastNameEdit').value;
                let birth = document.querySelector('.jsProfileBirthDateEdit').value;
                let newsLetter = $(container).find('.jsProfileNewsLetterEdit').first().is(':checked');
                let token = document.getElementsByName('__RequestVerificationToken')[0].value;
                let url = el.closest('form').getAttribute("action");

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
                    url: url
                }

                inst.saveProfile(options);
                inst.ToggleEditProfile();
                //$(this).parents('.jsProfileContainerEdit').first().fadeToggle();

                return false;
            });

        });
    }
}