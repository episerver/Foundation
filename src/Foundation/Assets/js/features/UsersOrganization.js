class UsersOrganization {
    Init() {
        this.LookupUser();
        this.SearchUsersEvent();
    }
    
    onChooseEvent(element) {
        var selectedItemData = element.getSelectedItemData();
        var form = $('#addUserForm');
        form.find('input[name*=Email]').val(selectedItemData.Email);
        form.find('input[name*=FirstName]').val(selectedItemData.FirstName);
        form.find('input[name*=LastName]').val(selectedItemData.LastName);
    }

    LookupUser() {
        var inst = this;
        var $autocompleteInput = $('#addUsersAutocomplete');
        var options = {
            url: function (phrase) {
                return "/Users/GetUsers?query=" + phrase;
            },
            getValue: "Email",
            requestDelay: 500,
            list: {
                match: {
                    enabled: false
                },
                onChooseEvent: () => this.onChooseEvent($autocompleteInput)
            },
            theme: "fullwidth"
        };
        $autocompleteInput.easyAutocomplete(options);
    }


    SearchUsersEvent() {
        var inst = this;
        $('#jsSearchUsersOrganizationBtn').click(function () {
            inst.searchUsers();
        })

        $('#jsSearchUsersOrganizationTxt').keyup(function (e) {
            if (e.keyCode == 13) {
                inst.searchUsers();
            }
        })
    }

    searchUsers() {
        var query = $('#jsSearchUsersOrganizationTxt').val().toLowerCase();
        var users = $('.jsUsersOrganiztionListing').find('.jsRowUser');
        users.each(function (i, e) {
            if ($(e).data('name').toLowerCase().includes(query) || $(e).data('email').toLowerCase().includes(query)) {
                $(e).css('display', 'table-row');
            } else {
                $(e).css('display', 'none');
            }
        })
    }
}