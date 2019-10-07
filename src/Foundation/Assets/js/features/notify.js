class CustomNotify {

    constructor() {
        this.handleShow = 0;
        this.handleHide = 0;
    }

    destroy() {
        clearTimeout(this.handleShow);
        clearTimeout(this.handleHide);
    }

    CreateNotifyPane(title, message, backgroundColor) {
        this.destroy();
        $('body').append(this.createTemplate(title, message, backgroundColor));
        this.handleShow = setTimeout(function () {
            $('.notification--pane').fadeOut();
            this.handleHide = setTimeout(() => $('.notification--pane').remove(), 1000);
        }, 3000);
    }

    Success(message) {
        this.CreateNotifyPane("Success!", message, "#AFF29A");
    }

    Error(message) {
        this.CreateNotifyPane("Error!", message, "#FFABA2");
    }

    Warning(message) {
        this.CreateNotifyPane("Warning!", message, "#8FCEDD");
    }

    Info(message) {
        this.CreateNotifyPane("Infomation!", message, "#FFFFA2");
    }

    createTemplate(title, message, backgroundColor) {
        var messTemp = ``;
        if (message) messTemp = `<div class="notification--pane__message">${message}</div>`;
        var result = `<div class="notification--pane" style="background-color: ${backgroundColor}">
          <div class="notification--pane__title">${title}</div>
          ` + messTemp + `
        </div>`
        console.log(result);
        return result;
    }
}

class NotifyHelper {
    Success(message) {
        $.notify(message, "success");
    }

    Error(message) {
        $.notify(message, "error");
    }

    Warning(message) {
        $.notify(message, "warning");
    }

    Info(message) {
        $.notify(message, "info");
    }
}