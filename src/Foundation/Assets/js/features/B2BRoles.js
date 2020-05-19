class B2BRoles {
  Search() {
    $('.jsSearchRoleBtn').click(function () {
      let keyword = $('.jsSearchRoleKeyword').val();
      $('.jsRoleTr').each(function (i, e) {
        let name = $(e).data('name');
        let pers = $(e).data('pers');
        if (name.toLowerCase().includes(keyword) || pers.toLowerCase().includes(keyword)) {
          $(e).show();
        } else {
          $(e).hide();
        }
      })
    })
  }

  Delete() {
    $('.jsDeleteRole').each(function (i, e) {
      $(e).click(function () {
        if (confirm("Are you sure?")) {
          let roleId = $(e).data('roleid');
          let url = $(e).data('url');
          axios.post(url, { roleId: roleId })
            .then(function () {
              window.location.reload();
            })
            .catch(function (e) {
              notification.Error(e);
            })
        }
      })
    })
  }

  Init() {
    this.Search();
    this.Delete();
  }
}