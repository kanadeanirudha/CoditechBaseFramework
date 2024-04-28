var RARIndiaCommanFunction = {
    Initialize: function () {
        RARIndiaCommanFunction.constructor();
    },
    constructor: function () {
    },

    GetDepartmentByCentreCode: function () {
        var selectedItem = $("#SelectedCentreCode").val();
        $('#DataTablesDivId tbody').html('');
        if (selectedItem != "") {
            $.ajax({
                cache: false,
                type: "GET",
                dataType: "html",
                url: "/GeneralCommanData/GetDepartmentsByCentreCode",
                data: { "centreCode": selectedItem },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#SelectedDepartmentID").html("").html(data);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    Notification.DisplayNotificationMessage("Failed to retrieve Departments.","error")
                }
            });
        }
        else {
            $('#DataTablesDivId tbody').html('');
            $("#SelectedDepartmentID").html("");
        }
    },
}
