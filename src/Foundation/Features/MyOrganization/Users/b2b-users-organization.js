export default class B2bUsersOrganization {
    init() {
        this.lookupUser();
        this.searchUsersEvent();
    }
    
    onChooseEvent(element) {
        let selectedItemData = element.getSelectedItemData();
        let form = $('#addUserForm');
        form.find('input[name*=Email]').val(selectedItemData.Email);
        form.find('input[name*=FirstName]').val(selectedItemData.FirstName);
        form.find('input[name*=LastName]').val(selectedItemData.LastName);
    }

    lookupUser() {
        let $autocompleteInput = $('#addUsersAutocomplete');
        let options = {
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

    searchUsersEvent() {
        let inst = this;
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
        let query = $('#jsSearchUsersOrganizationTxt').val().toLowerCase();
        let users = $('.jsUsersOrganiztionListing').find('.jsRowUser');
        users.each(function (i, e) {
            if ($(e).data('name').toLowerCase().includes(query) || $(e).data('email').toLowerCase().includes(query)) {
                $(e).css('display', 'table-row');
            } else {
                $(e).css('display', 'none');
            }
        })
    }
}