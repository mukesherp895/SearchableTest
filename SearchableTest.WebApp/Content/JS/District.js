$(document).ready(function () {
    BindDataTable();
})

var BindDataTable = function () {
    $("#dataTable").DataTable({
        "stateSave": false,
        "searchHighlight": true,
        "bServerSide": true,
        "sAjaxSource": "/District/GetDistrictRecords",
        "fnServerData": function (sSource, aoData, fnCallback) {
            $.ajax({
                type: "Get",
                data: aoData,
                url: sSource,
                success: fnCallback
            })
        },
        "displayLength": 10,
        "aoColumns": [
            { "mData": "Id" },
            { "mData": "DistrictName" }
        ],
    });
}