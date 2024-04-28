var DataTable = {
    Initialize: function () {
        DataTable.constructor();
    },
    constructor: function () {
    },

    // LoadList method is used to load List page
    LoadList: function (controllerName, methodName) {
        var dataTableModel = BindDataTableModel($('#DataTables_PageIndexId').val());
        $.ajax(
            {
                cache: false,
                type: "POST",
                dataType: "html",
                url: "/" + controllerName + "/" + methodName,
                data: JSON.stringify(dataTableModel),
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    //Rebind Grid Data
                    $('#DataTablesDivId').html(result);
                    $("#DataTables_PageSizeId").attr("disabled", false)
                    $("#DataTables_SearchById").attr("disabled", false)
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    Notification.DisplayNotificationMessage("Failed to display record.", "error")
                }
            });
    },

    // LoadList method is used to load List page
    LoadListFirst: function (controllerName, methodName) {
        var dataTableModel = BindDataTableModel(1);
        $.ajax(
            {
                cache: false,
                type: "POST",
                dataType: "html",
                url: "/" + controllerName + "/" + methodName,
                data: JSON.stringify(dataTableModel),
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    //Rebind Grid Data
                    $('#DataTablesDivId').html(result);
                    $("#DataTables_PageSizeId").attr("disabled", false)
                    $("#DataTables_SearchById").attr("disabled", false)
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    Notification.DisplayNotificationMessage("Failed to display record.", "error")
                }
            });
    },

    LoadListPrevious: function (controllerName, methodName) {
        var PageIndex = $('#DataTables_PageIndexId').val() == "1" ? 1 : dataTableModel.PageIndex = parseInt($('#DataTables_PageIndexId').val()) - 1;
        var dataTableModel = BindDataTableModel(PageIndex);
        $.ajax(
            {
                cache: false,
                type: "POST",
                dataType: "html",
                url: "/" + controllerName + "/" + methodName,
                data: JSON.stringify(dataTableModel),
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    //Rebind Grid Data
                    $('#DataTablesDivId').html(result);
                    $("#DataTables_PageSizeId").attr("disabled", false)
                    $("#DataTables_SearchById").attr("disabled", false)
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    Notification.DisplayNotificationMessage("Failed to display record.", "error")
                }
            });
    },
    // LoadList method is used to load List page
    LoadListLast: function (controllerName, methodName, pageSize) {
        var dataTableModel = BindDataTableModel(pageSize);
        $.ajax(
            {
                cache: false,
                type: "POST",
                dataType: "html",
                url: "/" + controllerName + "/" + methodName,
                data: JSON.stringify(dataTableModel),
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    //Rebind Grid Data
                    $('#DataTablesDivId').html(result);
                    $("#DataTables_PageSizeId").attr("disabled", false)
                    $("#DataTables_SearchById").attr("disabled", false)
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    Notification.DisplayNotificationMessage("Failed to display record.", "error")
                }
            });
    },

    LoadListNext: function (controllerName, methodName) {
        var PageIndex = parseInt($('#DataTables_PageIndexId').val()) + 1;
        var dataTableModel = BindDataTableModel(PageIndex);

        $.ajax(
            {
                cache: false,
                type: "POST",
                dataType: "html",
                url: "/" + controllerName + "/" + methodName,
                data: JSON.stringify(dataTableModel),
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    //Rebind Grid Data
                    $('#DataTablesDivId').html(result);
                    $("#DataTables_PageSizeId").attr("disabled", false)
                    $("#DataTables_SearchById").attr("disabled", false)
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    Notification.DisplayNotificationMessage("Failed to display record.", "error")
                }
            });
    },

    LoadListSortBy: function (controllerName, methodName, e) {
        var dataTableModel = BindDataTableModel($('#DataTables_PageIndexId').val());
        $.ajax(
            {
                cache: false,
                type: "POST",
                dataType: "html",
                url: "/" + controllerName + "/" + methodName,
                data: JSON.stringify(dataTableModel),
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    //Rebind Grid Data
                    $('#DataTablesDivId').html(result);
                    $("#DataTables_PageSizeId").attr("disabled", false)
                    $("#DataTables_SearchById").attr("disabled", false)
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    Notification.DisplayNotificationMessage("Failed to display record.", "error")
                }
            });
    },
}

function BindDataTableModel(PageIndex) {
    $("#notificationDivId").hide();
    var dataTableModel = new Object();
    dataTableModel.SearchBy = $('#DataTables_SearchById').val();
    dataTableModel.SortByColumn = "";
    dataTableModel.SortBy = "";
    dataTableModel.PageIndex = PageIndex;
    dataTableModel.PageSize = $('#DataTables_PageSizeId').val();
    dataTableModel.SelectedCentreCode = $("#SelectedCentreCode").length > 0 ? $("#SelectedCentreCode").val() : "";
    dataTableModel.SelectedDepartmentID = $("#SelectedDepartmentID").length > 0 ? $("#SelectedDepartmentID").val() : 0;
    $("#DataTables_PageSizeId").attr("disabled", true);
    $("#DataTables_SearchById").attr("disabled", true);
    return dataTableModel;
}
