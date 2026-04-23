export default class NotificationHelper {
    success(message, encodeMess) {
        $.notify({
            message: message
        }, {
            type: 'success',
            offset: {
                x: 20,
                y: 40,
            },
            z_index: 3000
        });
    }

    error(message, encodeMess) {
        $.notify({
            message: message
        }, {
            type: 'danger',
            offset: {
                x: 20,
                y: 40,
            },
            z_index: 3000
        });
    }

    warning(message, encodeMess) {
        $.notify({
            message: message
        }, {
            type: 'warning',
            offset: {
                x: 20,
                y: 40,
            },
            z_index: 3000
        });
    }

    info(message, encodeMess) {
        $.notify({
            message: message
        }, {
            type: 'info',
            offset: {
                x: 20,
                y: 40,
            },
            z_index: 3000
        });
    }
}