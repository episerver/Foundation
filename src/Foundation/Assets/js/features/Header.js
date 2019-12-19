﻿class Header {
    constructor() {
        this.Popovers = [];
    }

    init() {
        var inst = this;
        $('.js-popover').each($.proxy(function (index, value) {
            const referenceId = $(value).attr('id');
            let options = {};

            if (referenceId === 'js-wishlist') {
                options = {
                    offset: {
                        offset: '242px, 2px'
                    }
                };
            } else if (referenceId === 'js-cart') {
                options = {
                    offset: {
                        offset: '162px, 2px'
                    }
                };
            }
        }, this));

        $(document)
            .on('click', '#jsBookmarkToggle', this.bookmark)
            .on('click', '.jsRemoveBookmark', this.removeBookmark)
            .on("change", ".jsChangeCountry", this.setRegion);

        $('.jsUsersSigninBtn').each(function (i, e) {
            $(e).click(function (event) {
                event.preventDefault();
                inst.signin($(this));
            });
        });
        $('.jsUsersSignupBtn').each(function (i, e) {
            $(e).click(function (event) {
                event.preventDefault();
                inst.signup($(this));
            });
        });
        this.megaMenu();
        this.setMarket();
    }

    bookmark(e) {
        e.preventDefault();
        if ($('#jsBookmarkToggle').attr('bookmarked') === undefined) {
            axios({
                method: 'post',
                url: "/Bookmarks/Bookmark",
                data: {
                    contentId: $('#jsBookmarkToggle').attr('contentid')
                }
            }).then(function (response) {
                $('#jsBookmarkToggle').attr('bookmarked', true);
                $('#jsBookmarkToggle').html(`<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="black" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-bookmark" style="fill: black;">
                                                        <path d="M19 21l-7-5-7 5V5a2 2 0 0 1 2-2h10a2 2 0 0 1 2 2z"></path>
                                                    </svg>`);
            }).catch(function (response) {
                console.log(response);
            });
        } else {
            axios({
                method: 'post',
                url: "/Bookmarks/Unbookmark",
                data: {
                    contentId: $('#jsBookmarkToggle').attr('contentid')
                }
            }).then(function (response) {
                $('#jsBookmarkToggle').removeAttr('bookmarked');
                $('#jsBookmarkToggle').html(`<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-bookmark" style="fill: none;">
                                                        <path d="M19 21l-7-5-7 5V5a2 2 0 0 1 2-2h10a2 2 0 0 1 2 2z"></path>
                                                    </svg>`);
            }).catch(function (response) {
                console.log(response);
            });
        }
    }

    removeBookmark(e) {
        e.preventDefault();
        var contentGuid = e.currentTarget.attributes["contentguid"].value;
        axios({
            method: 'post',
            url: "/Bookmarks/Remove",
            data: {
                contentGuid: contentGuid
            }
        }).then(function (response) {
            var rowId = '#bookmark-' + contentGuid;
            $(rowId).remove();
        }).catch(function (response) {
            console.log(response);
        });
    }


    megaMenu() {
        $('.navigation__left .navigation__item').each(function (i, e) {
            var dropdown = $(e).find('.mega-container').first();
            var top = $(e)[0].getBoundingClientRect();
            $(dropdown).css('top', top.bottom + 1 + 'px');
            $(dropdown).css('left', '0px');
        });
    }

    signin(e) {
        let form = $(e).closest("form");
        let bodyFormData = new FormData();
        bodyFormData.set('Email', $("#LoginViewModel_Email", form).val());
        bodyFormData.set('Password', $("#LoginViewModel_Password", form).val());
        bodyFormData.set('RememberMe', $("#LoginViewModel_RememberMe", form).is(':checked'));
        bodyFormData.set('ReturnUrl', $("#LoginViewModel_ReturnUrl", form).val());
        bodyFormData.set('__RequestVerificationToken', $("input[name=__RequestVerificationToken]", form).val());
        $('.loading-box').show();
        axios({
            method: 'post',
            url: form[0].action,
            data: bodyFormData,
            config: { headers: { 'Content-Type': 'multipart/form-data' } }
        })
            .then(function (response) {
                if (response.data.success == false) {
                    var errorMessage = document.getElementById('login-signin-errormessage');
                    if (errorMessage) {
                        errorMessage.innerText = '';
                        errorMessage.style.display = "block";
                        for (var error in response.data.errors) {
                            $('#login-signin-errormessage').append(response.data.errors[error] + '<br />');
                        }
                    }
                }
                else {
                    if (response.data.returnUrl) {
                        window.location.href = response.data.returnUrl;
                    } else {
                        window.location.href = "/";
                    }
                }
            })
            .catch(function (response) {
                document.getElementById('login-signin-errormessage').innerText = response;
            })
            .finally(function () {
                $('.loading-box').hide();
            });
    }

    convertToJsonObject(arrayData) {
        var indexed_array = {};

        $.map(arrayData, function (n, i) {
            indexed_array[n['name']] = n['value'];
        });

        return indexed_array;
    }

    signup(e) {
        let form = $(e).closest("form");
        let bodyFormData = new FormData();
        bodyFormData.set('Address.Name', $("#RegisterAccountViewModel_Address_Name", form).val());
        bodyFormData.set('Email', $("#RegisterAccountViewModel_Email", form).val());
        bodyFormData.set('Password', $("#RegisterAccountViewModel_Password", form).val());
        bodyFormData.set('Password2', $("#RegisterAccountViewModel_Password2", form).val());
        bodyFormData.set('Address.FirstName', $("#RegisterAccountViewModel_Address_FirstName", form).val());
        bodyFormData.set('Address.LastName', $("#RegisterAccountViewModel_Address_LastName", form).val());
        bodyFormData.set('Address.Line1', $("#RegisterAccountViewModel_Address_Line1", form).val());
        bodyFormData.set('Address.Line2', $("#RegisterAccountViewModel_Address_Line2", form).val());
        bodyFormData.set('Address.City', $("#RegisterAccountViewModel_Address_City", form).val());
        bodyFormData.set('Address.PostalCode', $("#RegisterAccountViewModel_Address_PostalCode", form).val());
        bodyFormData.set('Address.CountryCode', $('input[name="RegisterAccountViewModel.Address.CountryCode"]:checked', form).val());
        bodyFormData.set('Newsletter', $('#RegisterAccountViewModel_Newsletter', form).is(':checked'));
        if ($('input[name="RegisterAccountViewModel.Address.CountryRegion.Region"]:checked', form).length > 0) {
            bodyFormData.set('Address.CountryRegion.Region', $('input[name="RegisterAccountViewModel.Address.CountryRegion.Region"]:checked', form).val());
        } else {
            bodyFormData.set('Address.CountryRegion.Region', $('input[name="RegisterAccountViewModel.Address.CountryRegion.Region"]', form).val());
        }
        
        bodyFormData.set('__RequestVerificationToken', $("input[name=__RequestVerificationToken]", form).val());

        $('.loading-box').show();
        axios({
            method: 'post',
            url: form[0].action,
            data: bodyFormData,
            config: { headers: { 'Content-Type': 'multipart/form-data' } }
        })
            .then(function (response) {
                if (response.data) {
                    var errorMessage = document.getElementById('login-signup-errormessage');
                    if (errorMessage) {
                        errorMessage.innerText = '';
                        errorMessage.style.display = "block";
                        for (var error in response.data.errors) {
                            $('#login-signup-errormessage').append(response.data.errors[error] + '<br />');
                        }
                    }
                }
                else {
                    window.location.href = '/';
                }
            })
            .catch(function (response) {
                document.getElementById('login-signup-errormessage').innerText = response;
            })
            .finally(function () {
                $('.loading-box').hide();
            });
    }

    setRegion() {
        var $countryCode = $(this).val();
        var $addressRegionContainer = $(".address-region");
        var $region = $(".address-region-input", $addressRegionContainer).val();
        var $htmlPrefix = $("input[name='address-htmlfieldprefix']", $(this).parent()).val();
        var $url = "/AddressBook/GetRegionsForCountry/";
        axios.post($url, { countryCode: $countryCode, region: $region, htmlPrefix: $htmlPrefix })
            .then(function (response) {
                $addressRegionContainer.replaceWith($(result));
            })
            .catch(function (error) {
                console.log(error);
            });
    }

    setMarket() {
        $('.jsSelectMarket').each(function (i, e) {
            $(e).click(function () {
                var form = $(this).closest('form');
                var url = form.attr('action');
                var method = form.attr('method');
                let bodyFormData = new FormData();
                bodyFormData.set('__RequestVerificationToken', $("input[name=__RequestVerificationToken]", form).val());

                axios({
                    url: url,
                    method: method,
                    data: bodyFormData
                })
                    .then(function (result) {
                        window.location.href = result.data.returnUrl;
                    }).catch(function (e) {
                        notification.Error(e);
                    });

                return false;
            });
        });
    }
}
