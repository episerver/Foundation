class NotifyHelper {
    Success(message, encodeMess) {
        var options = this.setOptions("success", encodeMess);
        $.notify(message, options);
    }

    Error(message, encodeMess) {
        var options = this.setOptions("error", encodeMess);
        $.notify(message, options);
    }

    Warning(message, encodeMess) {
        var options = this.setOptions("warning", encodeMess);
        $.notify(message, options);
    }

    Info(message, encodeMess) {
        var options = this.setOptions("info", encodeMess);
        $.notify(message, options);
    }

    setOptions(className, encodeMess) {
        var options = {
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