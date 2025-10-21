var tblReprintTransaction = null;
var tblPurgeTransactions = null;
var transactionRow = null;
var selectedTransReprintRow = null;
var itemRow = null;

$(function () {
    changeClass('liReprintPromo', 'gridviewReprintPromo');
    getReprintTransactions();

    initEvents();
})

$("#liReprintPromo").on("click", function () {
    changeClass('liReprintPromo', 'gridviewReprintPromo');
    getReprintTransactions();
})

$("#liPurgePromo").on("click", function () {
    changeClass('liPurgePromo', 'gridviewPurgePromo');
    getPurgeTransactions();
})

$("#btnEnable").on("click", function () {
    revealModal("printPreview");
})


function initEvents() {
    $("#lnkLogoutCntr").on("click", function () {
        if (confirm("Do you want to logout?")) {
            javascript: __doPostBack('ctl00$MainContent$lnklogout', '');

        }
    })

    $("#btnPurgeAll").on("click", function () {
        if (tblHasItem("tblPurgeTransactions")) {
            if (confirm("Do you really want to Purge All Transaction?")) {
                purgeAllTransaction();
            }
        } else {
            notifyWarning("Cannot Purge without List of Transactions", true);
        }
    })

    $("#btnOKPincode").on("click", function () {
        if (confirm("Do you want to grant reprinting for this transaction?")) {
            if (isEmpty($("#txtPincode").val())) {
                notifyOnInput("Please Enter Pin Code", 'warning', '#txtPincode');
            } else {
                var headId = transactionRow.HEAD_ID;
                var pinCode = $("#txtPincode").val();
                grantRequest(headId, pinCode);
            }
        }
    })

    $("#btnCancelPincode").on("click", function () {
        hideModal("PopUpOutline");
        $("#txtPincode").val("");
    })

    $("#btnCanceltran").on("click", function () {
        hideModal("printPreview");
        $("#tranNo").val("");
    })

    $("#btnOKtran").on("click", function () {
        var moduleId = $("#moduleID").val();
        if (isEmpty($("#tranNo").val())) {
            notifyOnInput("Please Enter Transaction number", 'warning', '#tranNo');
        } else {
            var tranNo = $("#tranNo").val();
            if (confirm("Do you want to enable Print preview button for this transaction? (" + moduleId + " MODULE)")) {
                enableButton(tranNo, moduleId);
            }
        }
    })
}

function changeClass(linkId, tableId) {
    $("#liReprintPromo").removeClass("activeLink");
    $("#liPurgePromo").removeClass("activeLink");

    $("#" + linkId).addClass("");
    $("#" + linkId).addClass("activeLink");

    $("#gridviewReprintPromo").hide();
    $("#gridviewPurgePromo").hide();
    $("#gridviewReprintPromo").removeClass("activeDiv");
    $("#gridviewReprintPromo").removeClass("activeDiv");

    $("#" + tableId).show();
    $("#" + tableId).addClass("");
    $("#" + tableId).addClass("activeDiv");
};

function getReprintTransactions() {
    var xmlhttp;
    if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {// code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            var response;
            response = jQuery.parseJSON(xmlhttp.response.d);
            if (response.status == 1) {

                tblReprintTransaction = $('#tblReprintTransaction').DataTable({
                    "scrollY": "364px",
                    "paging": false,
                    "retrieve": true,
                    "info": false,
                    "data": response.transactions,
                    "columns": [
                        { "data": "TRAN_ID" },
                        { "data": "REQUEST_CODE" },
                        { "data": "STORE_NAME" },
                        { "data": "DEPT_NAME" },
//                        { "data": "DATES" },
                        { "data": "STATUS" },
                        {
                            "targets": [5],
                            "data": null,
                            "width": "70px",
                            "render": function (data, type, row, meta) {
                                if (row.STATUS == 'PENDING') {
                                    return '<input class="table-button-primary" type="button" value="GRANT" style="width:70px"/>';
                                } else if (row.STATUS == 'GRANTED') {
                                    return '<input class="table-button-primary" type="button" value="REPRINT" style="width:70px"/>';
                                } else {
                                    return "";
                                }
                            }
                            // "defaultContent": '<input class="table-button-primary" style="width:70px"  type="button" value="GRANT"/>'

                        }
                    ]
                });

                initTblReprintTransactionEvents();
            } else if (response.status == 0) {
                notifyWarning(response.message);
            } else {
                notifyError(response.message);
            }
        } else if (xmlhttp.status == 500) {
            notifyError('Server Connection Error');
        }
    }
    xmlhttp.open("POST", "WebMethods/adminWebMethod.aspx/getReprintTransactions", true);
    xmlhttp.responseType = "json"
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xmlhttp.send();
};

function initTblReprintTransactionEvents() {
    $("#tblReprintTransaction tbody").on('click', 'input', function (e) {
        transactionRow = null;
        var tr = $(this).closest('tr');
        selectedTransReprintRow = tr;
        var row = tblReprintTransaction.row(tr);
        transactionRow = row.data();

        if (this.value == 'GRANT') {
            revealModal("PopUpOutline");
        } else {
            window.location.href = "pricerReportViewer.aspx?headId=" + transactionRow.HEAD_ID;
        }

        //this is to prevent firing the other parent Events
        e.stopPropagation();

    });

    $("#tblReprintTransaction_filter").css('padding-right', '10px');
    $("#tblReprintTransaction_filter").css('font-weight', '600');
}


function grantRequest(headId, pinCode) {
    var xmlhttp;
    if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {// code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            var response;
            response = jQuery.parseJSON(xmlhttp.response.d);
            if (response.status == 1) {
                transactionRow.STATUS = 'GRANTED';
                tblReprintTransaction.row(selectedTransReprintRow).data(transactionRow).draw();
                notif({
                    msg: response.message,
                    type: "info",
                    timeout: 2000,
                    multiline: true,
                    position: "center"
                });

                hideModal("PopUpOutline");
                $("#txtPincode").val("");

            } else if (response.status == 0) {
                notifyWarning(response.message);
            } else {
                notifyError(response.message);
            }
        } else if (xmlhttp.status == 500) {
            notifyError('Server Connection Error');
        }
    }
    xmlhttp.open("POST", "WebMethods/adminWebMethod.aspx/grantRequest", true);
    xmlhttp.responseType = "json"
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xmlhttp.send("{'headId':" + headId + ",'pinCode':'" + pinCode + "'}");
}

function getPurgeTransactions() {
    var xmlhttp;
    if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {// code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            var response;
            response = jQuery.parseJSON(xmlhttp.response.d);
            if (response.status == 1) {
                if (tblPurgeTransactions != null) {
                    tblPurgeTransactions.destroy();
                }

                tblPurgeTransactions = $('#tblPurgeTransactions').DataTable({
                    "scrollY": "324px",
                    "paging": false,
                    "retrieve": true,
                    "info": false,
                    "data": response.transactions,
                    "columns": [
                        { "data": "TRAN_ID" },
                        { "data": "STORE_NAME" },
                        { "data": "DEPT_NAME" },
                        { "data": "STATUS" },
                        { "data": "PREPARED_BY" },
                        {
                            "data": "LAST_UPDATE_DATE",
                            "type": "date",
                            "def": function () { return new Date(); },
                            "dateFormat": 'dd-MMM-yy'
                        },
                        {
                            "targets": [5],
                            "data": null,
                            "width": "70px",
                            "defaultContent": '<input class="table-button-primary" style="width:70px"  type="button" value="PURGE"/>'

                        }
                    ]
                });
                if (tblHasItem("tblPurgeTransactions")) {
                    var total_items = $('#tblPurgeTransactions').find('tbody>tr').length;
                    $('#total_MD').text(total_items);
                }

                initTblPurgeTransactionEvents();
            } else if (response.status == 0) {
                notifyWarning(response.message);
            } else {
                notifyError(response.message);
            }
        } else if (xmlhttp.status == 500) {
            notifyError('Server Connection Error');
        }
    }
    xmlhttp.open("POST", "WebMethods/adminWebMethod.aspx/getPurgeTransactions", true);
    xmlhttp.responseType = "json"
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xmlhttp.send();
};


function initTblPurgeTransactionEvents() {
    $("#tblPurgeTransactions tbody").on('click', 'input', function (e) {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
            selectedBarcode = null;
        }
        else {
            $("#tblPurgeTransactions tr.selected").removeClass('selected');
            $(this).addClass('selected');
            var tr = $(this).closest('tr');
            var row = tblPurgeTransactions.row(tr);
            itemRow = row.data();
            if (confirm("Do you want to remove this Transaction?")) {
                purgeTransaction(itemRow.HEAD_ID);
            }
        }
        //this is to prevent firing the other parent Events
        //e.stopPropagation();
    });

    $("#tblPurgeTransactions_filter").css('padding-right', '10px');
    $("#tblPurgeTransactions_filter").css('font-weight', '600');
}

function tblHasItem(tableId) {
    var row = $('#' + tableId + ' >tbody >tr');
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

function purgeTransaction(headID) {

    if (headID == "" || headID == null) {
        notifyWarning("Please select Item to remove");
    } else {
        var xmlhttp;
        if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
            xmlhttp = new XMLHttpRequest();
        }
        else {// code for IE6, IE5
            xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
        }
        xmlhttp.onreadystatechange = function () {
            if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
                var response;
                response = jQuery.parseJSON(xmlhttp.response.d);
                if (response.status == 1) {
                    tblPurgeTransactions.row('.selected').remove().draw(false);

                    notif({
                        msg: response.message,
                        type: "info",
                        timeout: 2000,
                        multiline: true,
                        position: "center"
                    });
                    getPurgeTransactions();
                } else if (response.status == 0) {
                    notifyWarning(response.message);
                } else {
                    notifyError(response.message);

                }
            } else if (xmlhttp.status == 500) {
                notifyError('Server Connection Error');
            }
        }
        xmlhttp.open("POST", "WebMethods/adminWebMethod.aspx/RPpurgeTransaction", true);
        xmlhttp.responseType = "json"
        xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        xmlhttp.send("{'headID':'" + headID + "'}");
    }
}

var ListOfPurge = "";
function ListOfTransaction(listTransaction) {
    ListOfPurge = "";
    var obj = tblPurgeTransactions.rows().data();
    for (var x = 0; x < obj.length; x++) {
        if (obj[x].HEAD_ID != "") {
            ListOfPurge += "|" + obj[x].HEAD_ID.toString();
        }
    }
    return ListOfPurge;
}

function purgeAllTransaction() {
    var transactions = ListOfTransaction();
    var xmlhttp;
    if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {// code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            var response;
            response = jQuery.parseJSON(xmlhttp.response.d);
            if (response.status == 1) {
                tblPurgeTransactions.row('.selected').remove().draw(false);

                notif({
                    msg: response.message,
                    type: "info",
                    timeout: 2000,
                    multiline: true,
                    position: "center"
                });
                getPurgeTransactions();
            } else if (response.status == 0) {
                notifyWarning(response.message);
            } else {
                notifyError(response.message);

            }
        } else if (xmlhttp.status == 500) {
            notifyError('Server Connection Error');
        }
    }
    xmlhttp.open("POST", "WebMethods/adminWebMethod.aspx/purgeAllTransaction", true);
    xmlhttp.responseType = "json"
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xmlhttp.send("{'transactions':'" + transactions.slice(1) + "'}");
}


function enableButton(tranNo, moduleId) {
    var xmlhttp;
    if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {// code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            var response;
            response = jQuery.parseJSON(xmlhttp.response.d);
            if (response.status == 1) {
                notif({
                    msg: response.message,
                    type: "info",
                    timeout: 2000,
                    multiline: true,
                    position: "center"
                });
                hideModal("printPreview");
                $("#txtNo").val("");

            } else if (response.status == 0) {
                notifyWarning(response.message);
            } else {
                notifyError(response.message);
            }
        } else if (xmlhttp.status == 500) {
            notifyError('Server Connection Error');
        }
    }
    xmlhttp.open("POST", "WebMethods/adminWebMethod.aspx/enableButton", true);
    xmlhttp.responseType = "json"
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xmlhttp.send("{'tranNo':" + tranNo + ",'moduleId':'" + moduleId + "'}");
}