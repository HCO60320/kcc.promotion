var tblTransactions = null;
var varTest = null;
var transactionRow = null;

$(function () {
    getAllTransactions();
    initEvents();
    $('#tblTransactions').DataTable();
})
//keyup change  paste
$('#txtFilterExecPR').on('keyup', function () {
    var table = $('#tblTransactions').DataTable();
    if (table != null) {
        table.search($(this).val()).draw();
    }
    // #myInput is a <input type="text"> element
})


function tblHasTransaction() {
    var row = $('#tblTransactions >tbody >tr');
    if (row.length > 0) {
        if (row[0].cells.length == 1 && row[0].cells[0].textContent == "No data available in table") {
            return false;
        } else {
            return true;
        }
    } else {
        return false;
    }
}

function initEvents() {
    $("#lnkLogoutCntr").on("click", function () {
        if (confirm("Do you want to logout?")) {
            javascript: __doPostBack('ctl00$MainContent$lnklogout', '');
        }
    })

    $("#btnSearchTran").on("click", function () {
        ValidateParameters(1);
    });

    $("#btnSearchTran").onEnter(function () {
        ValidateParameters(1);
    });

    $("#btnSearchLocation").on("click", function () {
        ValidateParameters(2);
    });

    $("#btnSearchLocation").onEnter(function () {
        ValidateParameters(2);
    });

    $("#btnSearchDepartment").on("click", function () {
        ValidateParameters(3);
    });

    $("#btnSearchDepartment").onEnter(function () {
        ValidateParameters(3);
    });
    $("#btnSearchOrin").on("click", function () {
        ValidateParameters(4);
    });

    $("#btnSearchOrin").onEnter(function () {
        ValidateParameters(4);
    });

    $("#btnSearchBarcode").on("click", function () {
        ValidateParameters(5);
    });

    $("#btnSearchBarcode").onEnter(function () {
        ValidateParameters(5);
    });

}
function ValidateParameters(type) {
    /// <summary>
    /// Validate Parameter
    /// </summary>
    /// <param name="type" type="type">Parameter of Type</param>

    var value = $("#txtFilterExecPR").val();
    if (type != 0 && value != "") {
        $.when(GetPromoFilter(type, value)).done(function () {
            initTblTransactionEvents();
            document.getElementById("setID").selectedIndex = "0";
        });
    } else {
        notif({
            msg: "Please input value before searching",
            type: "warning",
            autohide: true,
            position: "center"
        });
        document.getElementById("setID").selectedIndex = "0";
    }
}
function GetPromoFilter(searchType, searchValue) {
    /// <summary>
    /// Get Pricer Markdown Filter
    /// </summary>
    /// <param name="searchType" type="type">Parameter of Search Type</param>
    /// <param name="searchValue" type="type">Parameter of Search Value</param>
    /// <returns type="object">Json Object</returns>
    return $.ajax({
        url: "WebMethods/pricerWebMethod.aspx/getfilterTransactions",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ searchType: searchType, searchValue: searchValue }),
        success: function (result) {
            var response = jQuery.parseJSON(result.d);

            if (response.status == 1) {
                LoadTableDashboard(response.transactions);
            } else if (response.status == 0) {
                notif({
                    msg: "No data found.",
                    type: "warning",
                    autohide: true,
                    position: "center"
                });
            } else {
                notif({
                    msg: response.message,
                    type: "warning",
                    autohide: true,
                    position: "center"
                });
            }


        },
        error: function (error) {
            notif({
                msg: JSON.parse(error).message,
                type: "warning",
                autohide: true,
                position: "center"
            });
        }
    });
}

function LoadTableDashboard(responsedata) {
    /// <summary>
    /// Load Transaction Details
    /// </summary>
    /// <param name="data" type="type">Parameter of Data Object</param>

    var table = $('#tblTransactions').DataTable();
    if (table != null) {
        table.destroy();
    }

    tblTransactions = $('#tblTransactions').DataTable({
        "scrollY": "376px",
        //"searching": false,
        "paging": false,
        "info": false,
        "dom": 'lrtip',
        "data": responsedata,
        "columns": [
            { "data": "TRAN_ID" },
            { "data": "LOC_NAME" },
            { "data": "DEPT_NAME" },
            { "data": "PROMO_TYPE_DESCRIPTION" },
            { "data": "STATUS" }, {
                "targets": [5],
                "data": null,
                "width": "70px",
                "defaultContent": '<input class="table-button-primary" style="width:70px"  type="button" value="Show"/>'
            }
        ],
        "columnDefs": [{
            "targets": [0],
            "width": "70px",
        }], drawCallback: function () {
            initTblTransactionEvents();
        }
    });
    if (tblHasTransaction()) {
        var total_items = $('#tblTransactions').find('tbody>tr').length;
        $('#lblTotalTransCount').text(total_items);
    } else {
        $('#lblTotalTransCount').text('0');
    }

    varTest = tblTransactions;
    initTblTransactionEvents();
}

function initTblTransactionEvents() {
    //initializes all events used by "tblTransactions"
    $("#tblTransactions tbody").on('click', 'tr', function () {
        transactionRow = null;
        var tr = $(this).closest('tr');
        var row = tblTransactions.row(tr);
        transactionRow = row.data();
        if (transactionRow != undefined) {
            varTest = transactionRow;
            window.location.href = "BuyerDetails.aspx?headId=" + transactionRow.HEAD_ID;
        }
    });

    $("#tblTransactions tbody").on('click', 'input', function (e) {
        revealModal("PopUpRemarks");
        transactionRow = null;
        var tr = $(this).closest('tr');
        var row = tblTransactions.row(tr);
        transactionRow = row.data();

        $('#txtPromoDescription').val("");
        $('#txtPromoDescription').val(transactionRow.PROMOLIST_DESC);

        //this is to prevent firing the other parent Events
        e.stopPropagation();
    });

    $("#tblTransactions_filter").css('padding-right', '10px');
    $("#tblTransactions_filter").css('font-weight', '600');
}



function getAllTransactions() {
    var xmlhttp;
    if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {// code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            response = jQuery.parseJSON(xmlhttp.response.d);

            if (tblTransactions != null) {
                tblTransactions.destroy();
            }

            tblTransactions = $('#tblTransactions').DataTable({
                "scrollY": "376px",
                "paging": true,
                "pageLength" : 15,
                "info": false,
                "dom": 'lrtip',
                "destroy": true,
                "data": response.transactions,
                "columns": [
                    { "data": "TRAN_ID" },
                    { "data": "LOC_NAME" },
                    { "data": "DEPT_NAME" },
                    { "data": "PROMO_TYPE_DESCRIPTION" },
                    { "data": "STATUS" }, {
                        "targets": [5],
                        "data": null,
                        "width": "70px",
                        "defaultContent": '<input class="table-button-primary" style="width:70px"  type="button" value="Show"/>'
                    }
                ],
                "columnDefs": [{
                    "targets": [0],
                    "width": "70px",
                }]
            });

            if (tblHasTransaction()) {
                var total_items = $('#tblTransactions').find('tbody>tr').length;
                $('#lblTotalTransCount').text(total_items);
            } else {
                $('#lblTotalTransCount').text('0');
            }

            varTest = tblTransactions;
            initTblTransactionEvents();
        }
    }
    xmlhttp.open("POST", "WebMethods/buyerWebMethod.aspx/getAllTransactions", true);
    xmlhttp.responseType = "json"
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xmlhttp.send();
}

