var Notification = {
    Initialize: function () {
        Notification.constructor();
    },
    constructor: function () {
    },

    DisplayNotificationMessage: function (message, messageType) {
        var notificationStyle = "";
        switch (messageType) {
            case "success":
                notificationStyle = "bg-success";
                break;
            case "error":
                notificationStyle = "bg-danger";
                break;
            case "info":
                notificationStyle = "bg-info";
                break;
            case "warning":
                notificationStyle = "bg-warning";
                break;
        }
        $("#notificationMessageId").html("").html(message);
        $("#notificationDivId").addClass(notificationStyle);
        $("#notificationDivId").show();
    },
}

$("#notificationCloseId").click(function () {
    $("#notificationDivId").fadeOut(1000);
});
$("#notificationDivId").fadeOut(10000);