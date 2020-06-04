var popup, dataTable;
var entity = 'TimeClock';
var apiurl = '/api/' + entity;

$(document).ready(function () {
    //alert(entity);
    var organizationId = $('#organizationId').val();
    dataTable = $('#grid').DataTable({
        "ajax": {
            "url": apiurl + '/' + organizationId,
            "type": 'GET',
            "datatype": 'json'
        },
        "columns": [
            { "data": "idNumber" },
            { "data": "fullName" },
            {
                "data": function (data) {
                    var empty = "";
                    var btnRH = "<a class='btn btn-danger btn-xs btnRH' data-id='" + data["idNumber"] + "'>Regular</a>";
                    var btnSH = "<a class='btn btn-danger btn-xs btnSH' data-id='" + data["idNumber"] + "'>Special</a>";
                    if (data["totalTimeIn"] == data["totalTimeOut"]) {
                        return empty;
                    }
                    else if (data["totalTimeIn"] != data["totalTimeOut"]) {
                        return btnRH + "  " + btnSH;
                    }
                }
            },
            {
                "data": function (data) {
                    var btnTimeIn = "<a class='btn btn-success btn-xs btnTimeIn' data-id='" + data["idNumber"] + "'>Time in</a>";
                    var btnTimeOut = "<a class='btn btn-success btn-xs btnTimeOut' data-id='" + data["idNumber"] + "'>Time out</a>";
                    var btnOvertime = "<a class='btn btn-danger btn-xs btnOvertime' data-id='" + data["idNumber"] + "'>Overtime</a>";
                    if (data["totalTimeIn"] == data["totalTimeOut"]) {
                        return btnTimeIn;
                    }
                    else if (data["totalTimeIn"] != data["totalTimeOut"]) {
                        return btnTimeOut + "  " + btnOvertime;
                    }
                }
            }
        ],
        "language": {
            "emptyTable": "no data found."
        },
        "lengthChange": false,
    });
});
$("#grid").on("click", ".btnTimeIn", function (e) {
    e.preventDefault();
    var id = $(this).attr("data-id");
    //alert(id);
    var param = { idNumber: id };
    swal({
        title: "Are you sure want to complete this transaction?",
        text: "You will not be able to restore the file!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#dd4b39",
        confirmButtonText: "Yes, update it!",
        closeOnConfirm: true
    }, function () {
        $.ajax({
            type: 'POST',
            url: apiurl + '/PostTimeIn',
            data: param,
            success: function (data) {
                if (data.success) {
                    ShowMessage(data.message);
                    dataTable.ajax.reload();
                } else {
                    ShowMessageError(data.message);
                }
            }
        });
    });
});
$("#grid").on("click", ".btnTimeOut", function (e) {
    e.preventDefault();
    var id = $(this).attr("data-id");
    //alert(id);
    var param = { idNumber: id };
    swal({
        title: "Are you sure want to complete this transaction?",
        text: "You will not be able to restore the file!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#dd4b39",
        confirmButtonText: "Yes, update it!",
        closeOnConfirm: true
    }, function () {
        $.ajax({
            type: 'POST',
            url: apiurl + '/PostTimeOut',
            data: param,
            success: function (data) {
                if (data.success) {
                    ShowMessage(data.message);
                    dataTable.ajax.reload();
                } else {
                    ShowMessageError(data.message);
                }
            }
        });
    });
});

$("#grid").on("click", ".btnOvertime", function (e) {
    e.preventDefault();
    var id = $(this).attr("data-id");
    //alert(id);
    var param = { idNumber: id };
    swal({
        title: "Are you sure want to complete this transaction?",
        text: "You will not be able to restore the file!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#dd4b39",
        confirmButtonText: "Yes, update it!",
        closeOnConfirm: true
    }, function () {
        $.ajax({
            type: 'POST',
            url: apiurl + '/PostOvertime',
            data: param,
            success: function (data) {
                if (data.success) {
                    ShowMessage(data.message);
                    dataTable.ajax.reload();
                } else {
                    ShowMessageError(data.message);
                }
            }
        });
    });
});

$("#grid").on("click", ".btnRH", function (e) {
    e.preventDefault();
    var id = $(this).attr("data-id");
    //alert(id);
    var param = { idNumber: id };
    swal({
        title: "Are you sure want to complete this transaction?",
        text: "You will not be able to restore the file!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#dd4b39",
        confirmButtonText: "Yes, update it!",
        closeOnConfirm: true
    }, function () {
        $.ajax({
            type: 'POST',
            url: apiurl + '/PostRegularHoliday',
            data: param,
            success: function (data) {
                if (data.success) {
                    ShowMessage(data.message);
                    dataTable.ajax.reload();
                } else {
                    ShowMessageError(data.message);
                }
            }
        });
    });
});

$("#grid").on("click", ".btnSH", function (e) {
    e.preventDefault();
    var id = $(this).attr("data-id");
    //alert(id);
    var param = { idNumber: id };
    swal({
        title: "Are you sure want to complete this transaction?",
        text: "You will not be able to restore the file!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#dd4b39",
        confirmButtonText: "Yes, update it!",
        closeOnConfirm: true
    }, function () {
        $.ajax({
            type: 'POST',
            url: apiurl + '/PostSpecialHoliday',
            data: param,
            success: function (data) {
                if (data.success) {
                    ShowMessage(data.message);
                    dataTable.ajax.reload();
                } else {
                    ShowMessageError(data.message);
                }
            }
        });
    });
});
const monthNames = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
];
function setClockTime(d) {
    var h = d.getHours();
    var m = d.getMinutes();
    var suffix = "AM";
    if (h > 11) { suffix = "PM"; }
    if (h > 12) { h = h - 12; }
    if (h == 0) { h = 12; }
    if (h < 10) { h = "0" + h; }
    if (m < 10) { m = "0" + m; }
    return h + ":" + m + " " + suffix;
}
function ShowPopup(url) {
    var modalId = 'modalDefault';
    var modalPlaceholder = $('#' + modalId + ' .modal-dialog .modal-content');
    $.get(url)
        .done(function (response) {
            modalPlaceholder.html(response);
            popup = $('#' + modalId + '').modal({
                keyboard: false,
                backdrop: 'static'
            });
        });
}

function Delete(id) {
    swal({
        title: "Are you sure want to Delete?",
        text: "You will not be able to restore the file!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#dd4b39",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: true
    }, function () {
        $.ajax({
            type: 'DELETE',
            url: apiurl + '/' + id,
            success: function (data) {
                if (data.success) {
                    ShowMessage(data.message);
                    dataTable.ajax.reload();
                } else {
                    ShowMessageError(data.message);
                }
            }
        });
    });


}




