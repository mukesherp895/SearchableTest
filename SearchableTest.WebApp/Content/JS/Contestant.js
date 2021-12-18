$(document).ready(function () {
    BindDataTable();
})

var BindDataTable = function () {
    $("#dataTable").DataTable({
        "stateSave": false,
        "searchHighlight": true,
        "bServerSide": true,
        "sAjaxSource": "/Contestant/GetContestantRecords",
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
            { "mData": "FullName" },
            { "mData": "Lastname" },
            { "mData": "DOB" },
            { "mData": "Gender" },
            { "mData": "Address" },
            { "mData": "DistrictName" },
            { "mData": "IsActive" },
            {
                "width": "10%",
                "mData": "ButtonAction",
                bSortable: false,
                aTargets: ['noSort']
            }
        ]
    });
}

var AddEditContestant = function (contestantId) {
    var url = "/Contestant/AddEdit?Id=" + contestantId;
    $("#myModalBodyDiv").load(url, function () {
        if (contestantId == 0) {
            $("#lblHeading").text("Contestant Add");
        }
        else {
            $("#lblHeading").text("Contestant Update");
        }
        $("#AddContestant").modal("show");
    })
}