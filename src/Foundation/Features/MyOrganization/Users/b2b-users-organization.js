export default class B2bUsersOrganization {
    init() {
        this.lookupUser();
        this.searchUsersEvent();
    }
    
    onChooseEvent(element) {
        let selectedItemData = element.getSelectedItemData();
        let form = document.querySelector('#addUserForm');
        form.querySelector('input[name*=Email]').value = selectedItemData.Email;
        form.querySelector('input[name*=FirstName]').value = selectedItemData.FirstName;
        form.querySelector('input[name*=LastName]').value = selectedItemData.LastName;
    }

    lookupUser() {
        let autocompleteInput = document.querySelector('#addUsersAutocomplete');
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
        autocompleteInput.easyAutocomplete(options);
    }

    searchUsersEvent() {
        let inst = this;
        document.querySelector('#jsSearchUsersOrganizationBtn').addEventListener("click", function () {
            inst.searchUsers();
        })

        document.querySelector('#jsSearchUsersOrganizationTxt').addEventListener("keyup", function (e) {
            if (e.keyCode == 13) {
                inst.searchUsers();
            }
        })
    }

    searchUsers() {
        let query = document.querySelector('#jsSearchUsersOrganizationTxt').value.toLowerCase();
        let users = document.querySelector('.jsUsersOrganiztionListing').querySelectorAll('.jsRowUser');
        users.forEach(function (el, i) {
            if (el.getAttribute('data-name').toLowerCase().includes(query) || el.getAttribute('data-email').toLowerCase().includes(query)) {
                el.style.display ='table-row';
            } else {
                el.style.display = 'table-row';
            }
        })
    }
}