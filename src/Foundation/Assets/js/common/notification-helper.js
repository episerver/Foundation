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
            type: 'danger'
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