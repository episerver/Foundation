export default class NotificationHelper {
    success(message, encodeMess) {
        let options = this.setOptions("success", encodeMess);
        $.notify(message, options);
    }

    error(message, encodeMess) {
        let options = this.setOptions("error", encodeMess);
        $.notify(message, options);
    }

    warning(message, encodeMess) {
        let options = this.setOptions("warning", encodeMess);
        $.notify(message, options);
    }

    info(message, encodeMess) {
        let options = this.setOptions("info", encodeMess);
        $.notify(message, options);
    }

    setOptions(className, encodeMess) {
        let options = {
            className: className
        }

        if (encodeMess != undefined) {
            options.encodeMess = encodeMess;
        } else {
            options.encodeMess = true;
        }

        return options;
    }
}