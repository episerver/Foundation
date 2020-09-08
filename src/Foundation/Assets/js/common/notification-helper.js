export default class NotificationHelper {
    success(message, encodeMess) {
        $.notify({
            message: message
        }, {
            type: 'success'
        });
    }

    error(message, encodeMess) {
        $.notify({
            message: message
        }, {
            type: 'error'
        });
    }

    warning(message, encodeMess) {
        $.notify({
            message: message
        }, {
            type: 'warning'
        });
    }

    info(message, encodeMess) {
        $.notify({
            message: message
        }, {
            type: 'info'
        });
    }
}