var headId = null;
var promoType = null;
var tranId = null;
var tranStatus = null;
var tblItems;
var tblFreeListItems;
var itemRow = null;
var freeListItemRow = null;
var varTest = null;
var dateIdCntr = 0;

//------------not in use
var _earliestDate = null;
var _latestDate = null;
//------------

$(function () {
    var $scrollingDiv1 = $("#PopContainer");

    $(window).scroll(function (e) {
        $scrollingDiv1.stop().animate({ "marginTop": ($(window).scrollTop() + 200) + "px" }, "slow");
    });

    //gets the head Id from the hidden field
    headId = $('#hfHeadId').val();
    promoType = $('#hfPromoType').val();
    initDateField('txtStartDate', 'txtEndDate');
    EmptyFields();

    if (headId == '' || headId == null || headId == 'null') {
        //new transaction
        tblItems = $('#tblItems').DataTable({
            "scrollY": "330px",
            "paging": false,
            "info": false,
            "columns": [
               { "data": "ORIN" },
               { "data": "BARCODE" },
               { "data": "VPN" },
               { "data": "ITEM_DESC" },
               {
                   "data": "AGE_CODE",
                   "width": "60px",
                   "render": function (data, type, row, meta) {
                       if (data == null) {
                           return "<input type='text' class='inputs ageCodeClass' tabindex='1' name='ageCode'  maxlength='2' style='width:50px;' value='' />";
                       } else {
                           return "<input type='text' class='inputs ageCodeClass' tabindex='1' name='ageCode'  maxlength='2' style='width:50px;' value='" + data + "' />";
                       }

                   }
               },
               {
                   "data": "QTY",
                   "width": "40px",
                   "render": function (data, type, row, meta) {
                       if (data == null) {
                           return "<input type='text' class='inputs qtyClass' tabindex='1' name='qty'  style='width:40px;' value='' />";
                       } else {
                           return "<input type='text' class='inputs qtyClass' tabindex='1' name='qty' style='width:40px;' value='" + data + "' />";
                       }

                   }
               },
               { "data": "SRP" }]
        });

        tblFreeListItems = $('#tblItemsFreeList').DataTable({
            "scrollY": "330px",
            "paging": false,
            "info": false,
            "columns": [
               { "data": "ORIN" },
               { "data": "BARCODE" },
               { "data": "VPN" },
               { "data": "ITEM_DESC" },
                {
                    "data": "AGE_CODE",
                    "width": "60px",
                    "render": function (data, type, row, meta) {
                        if (data == null) {
                            return "<input type='text' class='inputs ageCodeClass' name='ageCode'  maxlength='2' style='width:50px;' value='' />";
                        } else {
                            return "<input type='text' class='inputs ageCodeClass' name='ageCode'  maxlength='2' style='width:50px;' value='" + data + "' />";
                        }
                    }
                },
               {
                   "data": "QTY",
                   "width": "40px",
                   "render": function (data, type, row, meta) {
                       if (data == null) {
                           return "<input type='text' class='inputs qtyClass' name='qty' style='width:40px;' value='' />";
                       } else {
                           return "<input type='text' class='inputs qtyClass' name='qty' style='width:40px;' value='" + data + "' />";
                       }

                   }
               },
               { "data": "SRP" }]
        });
        $('#divFreeList').hide();

        initTblItemsEvents();
        initTblFreeListItemsEvents();
        $('#chkFreeList').prop("disabled", false);

        ShowAllDateElements(false);
    } else {
        $('#divFreeList').hide();

        getPromoDate(headId);
        getTransactionDetail(headId);
    }

    initEvents();

})

function EmptyFields() {
    $('#locId').val('');
    $('#locName').val('');
    $('#deptId').val('');
    $('#deptName').val('');
    $('#txtPromoDescription').val('');
    $('#txtStartDate').val('');
    $('#txtEndDate').val('');
    $('#txtbarcode').val('');
    $('#txtBarcodeFreeList').val('');
    $('#deptName').prop('disabled', true);
    $('#chkFreeList').prop("checked", false);
    $('#btnRemove').prop('disabled', true);
}

function initTblItemsEvents() {
    //initializes all events used by "tblItems"
    $("#tblItems tbody").off('click', 'tr');
    $("#tblItems tbody").off('click', 'tr input[type="text"]');
    $("#tblItems tbody tr input[name='ageCode']").off('focusout');
    $("#tblItems tbody tr input[name='qty']").off('focusout');

    $("#tblItems tbody").on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
            selectedBarcode = null;
            $("#btnRemove").prop('disabled', 'disabled');
        } else {
            $("#tblItems tr.selected").removeClass('selected');
            $(this).addClass('selected');
            var tr = $(this).closest('tr');
            var row = tblItems.row(tr);
            itemRow = row.data();

            //can only removed items if the transaction status 
            //is just ''
            if (tranStatus == null) {
                $("#btnRemove").prop('disabled', false);
            }
        }

    });

    $("#tblItems tbody").on('click', 'tr input[type="text"]', function (e) {
        e.stopPropagation();
    })

    $("#tblItems tbody tr input[name='ageCode']").on('focusout', function () {
        this.value = this.value.replace(/[^A-Z,a-z]/g, '');
        if (this.value != '') {
            var tr = $(this).closest('tr');
            var row = tblItems.row(tr);
            var itmRow = row.data();
            updateAgeCode(itmRow.BARCODE, 1, this.value.toUpperCase());
        }

        return false;
    })


    $("#tblItems tbody tr input[name='qty']").on('focusout', function () {
        this.value = this.value.replace(/[^0-9]/g, '');
        if (this.value != '0' && this.value != '') {
            var tr = $(this).closest('tr');
            var row = tblItems.row(tr);
            var itmRow = row.data();
            updateQty(itmRow.BARCODE, 1, this.value);
        }

        return false;
    })

    $("#tblItems_filter").css('padding-right', '10px');
    $("#tblItems_filter").css('font-weight', '600');
}

var test;

function initTblFreeListItemsEvents() {
    //initializes all events used by "tblItemsFreeList"
    $("#tblItemsFreeList tbody").on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
            selectedBarcode = null;
            $("#btnFreeListRemove").prop('disabled', 'disabled');
        } else {
            $("#tblItemsFreeList tr.selected").removeClass('selected');
            $(this).addClass('selected');
            var tr = $(this).closest('tr');
            var row = tblFreeListItems.row(tr);
            freeListItemRow = row.data();

            //can only removed items if the transaction status 
            //is just ''
            if (tranStatus == null) {
                $("#btnFreeListRemove").prop('disabled', false);
            }
        }

    });




    $("#tblItemsFreeList tbody").on('click', 'tr input[type="text"]', function (e) {
        e.stopPropagation();
    })

    $("#tblItemsFreeList tbody tr input[name='ageCode']").on('focusout', function () {
        this.value = this.value.replace(/[^A-Z,a-z]/g, '');
        if (this.value != '') {
            test = this;
            var tr = $(this).closest('tr');
            var row = tblFreeListItems.row(tr);
            var itmRow = row.data();
            updateAgeCode(itmRow.BARCODE, 0, this.value.toUpperCase());
        }

        return false;
    })


    $("#tblItemsFreeList tbody tr input[name='qty']").on('focusout', function () {
        this.value = this.value.replace(/[^0-9]/g, '');
        if (this.value != '0' && this.value != '') {
            var tr = $(this).closest('tr');
            var row = tblFreeListItems.row(tr);
            var itmRow = row.data();
            updateQty(itmRow.BARCODE, 0, this.value);
        }

        return false;
    })

    $("#tblItemsFreeList_filter").css('padding-right', '10px');
    $("#tblItemsFreeList_filter").css('font-weight', '600');

}

// ADDED  ON ENTER BARCODE INPUT TO CHECK ONGOING TRANSACTIONS FOR CONFLICT  -JAYSON SALINAS  V 2.0.0.7
function initEvents() {
    $("#lnkLogoutCntr").on("click", function () {
        if (confirm("Do you want to logout?")) {
            javascript: __doPostBack('ctl00$MainContent$lnklogout', '');
        }
    })

    $("#txtbarcode").delimit({
        numbers: true,
        backSpace: true
    });

    $("#txtBarcodeFreeList").delimit({
        numbers: true,
        backSpace: true
    });

    $("#txtbarcode").onEnter(function () {
        this.value = this.value.replace(/[^0-9]/g, '');
        if ($(this).val() == "") {
            notifyWarning("No Barcode Entered");
        } else if (checkIfItemExist("tblItems") == true) {
            var obj = tblItems.rows().data();
            for (var x = 0; x < obj.length; x++) {
                if (obj[x].BARCODE == $("#txtbarcode").val()) {
                    var tr = $("#tblItems tbody>tr:eq(" + x + ")")
                    $("#tblItems tr.selected").removeClass('selected');
                    $(tr).addClass('selected');
                    var container = $('#tblItems,div.dataTables_scrollBody');
                    var scrollTo = $('#tblItems tbody tr.selected');
                    container.scrollTop(scrollTo.offset().top - container.offset().top);
                    x = obj.length;

                    setTimeout(function () {
                        $("#tblItems tr.selected").removeClass('selected');
                    }, 5000);
                }
            }

            notifyWarning("Item already exist in the list");
            $('#txtbarcode').val('');

        } else {

            if (checkPromotionFields()) {
                var item = $("#txtbarcode").val();
                checkOngoingTran(item);
            }
            //if (checkPromotionFields()) {
            //    if (checkIfSetAllDate()) {
            //        $('#txtbarcode').prop('disabled', 'disabled');
            //        insertItem("buylist");
            //    }
            //}
        }
    });

    $("#txtBarcodeFreeList").onEnter(function () {
        this.value = this.value.replace(/[^0-9]/g, '');
        if ($(this).val() == "") {
            notifyWarning("Please Specify Barcode");
        } else if (checkIfItemExist("tblItemsFreeList") == true) {
            notifyWarning("Item already exist in the Free list");
            $('#txtBarcodeFreeList').val('');
        } else if (tblHasItem("tblItems") == false) {
            notifyWarning("Buylist needs item before adding Free list Item");
        } else {
            var item = $("#txtBarcodeFreeList").val();
            checkOngoingTran(item);
            insertItem("freelist");
        }
    });

    $('#cbPromoTypes').on('change', function () {
        if ($(this).find(":selected").val() == 0) {
            $('#chkFreeList').prop("disabled", false);
        } else {
            $('#chkFreeList').prop("disabled", "disabled");
            $('#chkFreeList').trigger("change");
            $('#divFreeList').hide();
        }
        $('#chkFreeList').prop("checked", false);
    });  //end of cbPromoTypes listener

    $('#chkFreeList').on('change', function () {

        if (this.checked) {
            initTblFreeListItemsEvents();
            $('#divFreeList').show();
        } else {
            $('#divFreeList').hide();
        }
    });  //end of chkFreeList listener

    $("#btnDone").on("click", function () {
        if (tblHasItem("tblItems")) {
            if ($('#chkFreeList').prop("checked") == true && tblHasItem("tblItemsFreeList") == false) {
                notifyWarning("Can't save transaction without Free List Item", true);
            } else if (chechHasEmptyQty('tblItems')) {
                notifyWarning("Can't save transaction,Plese Fill all Quantity in Buylist Fields", true);
            } else if (checkHasEmptyAgeCode('tblItems')) {
                notifyWarning("Can't save transaction,Please Fill all Age Code in Buylist Fields ", true);
            } else if (chechHasEmptyQty('tblItemsFreeList')) {
                notifyWarning("Can't save transaction,Plese Fill all Quantity in Freelist Fields", true);
            } else if (checkHasEmptyAgeCode('tblItemsFreeList')) {
                notifyWarning("Can't save transaction,Please Fill all Age Code in Freelist Fields ", true);
            } else if ($('#txtPromoDescription').val().length == '') {
                notifyWarning("Please Write Description for this Promotion", true);
            } else if ($('#txtPromoDescription').val().length < 35) {
                notifyWarning("Description must be 35 characters and above", true);
            } else {
                if (confirm("Do you really want to Save this Transaction?")) {
                    saveTransaction();
                }
            }
        } else {
            notifyWarning("Can't save transaction without Item", true);
        }
    });  //end of btnSaveTransaction listener



    $("#btnCancelTransaction").on("click", function () {
        if (tranStatus != null) {
            notifyWarning("Cannot Dispose this Transaction, it was already sent");
        } else if (isEmpty(headId)) {
            notifyWarning("Cannot Dispose this Transaction");
        } else {
            if (confirm("Do you really want to Dispose/Remove this Transaction?")) {
                cancelTransaction();
            }
        }
    })

    $("#btnRemove").on("click", function () {
        if (tranStatus != null) {
            notifyWarning("cannot remove items,Transaction already sent");
        } else {
            if (itemRow == undefined) {
                notifyWarning("Please select item to Remove");
            } else {
                if (confirm("Do you really want to remove this Item?")) {
                    removeItem(1);
                }
            }
        }
    })

    $("#btnFreeListRemove").on("click", function () {
        if (tranStatus != null) {
            notifyWarning("cannot remove items,Transaction already sent");
        } else {
            if (freeListItemRow == undefined) {
                notifyWarning("Please select item to Remove");
            } else {
                if (confirm("Do you really want to remove this Free List Item?")) {
                    removeItem(0);
                }
            }
        }
    })



    $("#btnAddDate").on("click", function () {
        if (checkIfSetAllDate()) {
            var tbl = document.getElementById('tblDates');
            var row = tbl.insertRow(-1);
            var cellStartDate = row.insertCell(0);
            var cellEndDate = row.insertCell(1);
            var cellRemove = row.insertCell(2);

            cellStartDate.innerHTML = "<input id='txtStartDate" + dateIdCntr + "' type='text' class='inputs' placeholder='Start Date' readonly='readonly'/>";
            cellEndDate.innerHTML = "<input id='txtEndDate" + dateIdCntr + "' type='text' class='inputs' placeholder='End Date' readonly='readonly'/>";
            cellRemove.innerHTML = '<input type="button" class="button-primary removeDate" value="Remove"/>';

            initDateField('txtStartDate' + dateIdCntr, 'txtEndDate' + dateIdCntr);
            initRemoveDate();


            dateIdCntr += 1;
        }
    })
}

//-----------------------------------------
//Region for multiple Date selection
//-----------------------------------------

function getEarliestDate() {
    var earliestDate = null;
    var dtCntr;
    $('#tblDates tbody tr').each(function () {
        if (earliestDate == null) {
            earliestDate = new Date($(this).find('input[type="text"]')[0].value);
        } else {
            dtCntr = new Date($(this).find('input[type="text"]')[0].value);
            if (dtCntr < _earliestDate) {
                earliestDate = dtCntr;
            }
        }
    })
    return earliestDate.format('MMM dd, yyyy');
}

function getLatestDate() {
    var latestDate = null;
    var dtCntr;
    $('#tblDates tbody tr').each(function () {
        if (latestDate == null) {
            latestDate = new Date($(this).find('input[type="text"]')[1].value);
        } else {
            dtCntr = new Date($(this).find('input[type="text"]')[1].value);
            if (dtCntr > latestDate) {
                latestDate = dtCntr;
            }
        }
    })
    return latestDate.format('MMM dd, yyyy');
}

function populateDateFields(dateObj) {
    for (var x = 0; x < dateObj.length; x++) {
        if (x == 0) {
            document.getElementById('txtStartDate').value = dateObj[x].START_DATE;
            document.getElementById('txtEndDate').value = dateObj[x].END_DATE
        } else {
            var tbl = document.getElementById('tblDates');
            var row = tbl.insertRow(-1);
            var cellStartDate = row.insertCell(0);
            var cellEndDate = row.insertCell(1);

            cellStartDate.innerHTML = "<input type='text' value='" + dateObj[x].START_DATE + "' class='inputs'/>";
            cellEndDate.innerHTML = "<input type='text' value='" + dateObj[x].END_DATE + "' class='inputs'/>";
        }
    }
}

function disableAllDateElements() {
    $('#tblDates tbody tr input').prop('disabled', 'disabled');
}

function ShowAllDateElements(type) {
    if (type == true) {
        $('#tblDates tbody tr input').css('display', 'block');
    } else {
        $('#tblDates tbody tr input').css('display', 'none');
    }
    disableAllDateElements();
}

function getPromoDates() {
    var promoDateObjArr = [];

    $('#tblDates tbody tr').each(function () {
        var listDates = { startDate: $(this).find('input[type="text"]')[0].value, endDate: $(this).find('input[type="text"]')[1].value };
        promoDateObjArr.push(listDates);
    })
    //promoDateObjArr = JSON.stringify(promoDateObjArr);
    return promoDateObjArr;
}

function checkIfSetAllDate() {
    var cntr = true;
    $('#tblDates tbody tr input[type="text"]').each(function () {
        if ($(this).val() == '') {
            var elemId = $(this).prop('id');
            notifyOnInput("Please Select Date Before Setting new Date Or Saving", "warning", "#" + elemId);
            cntr = false;
            return false;
        }
    })

    if (cntr == true)
        return true;
    else
        return false;
} //end of checkIfSetAllDate function


function checkDateHasConflict(toBeComparedDate, dateFieldId) {
    var dtfrom;
    var dtto;
    var dateClassObj = new date_class();

    var currentRow = $('#tblDates tbody tr:eq(' + $('#' + dateFieldId).parent().parent().index() + ')');
    var dtfrom_current_selected = $(currentRow).find('input[type="text"]')[0].value;
    var dtto_current_selected = $(currentRow).find('input[type="text"]')[1].value;

    //    var currentRow = $('#tblDates tbody tr:last');
    //    var dtfrom_current_selected = $(currentRow).find('input[type="text"]')[0].value;
    //    var dtto_current_selected = $(currentRow).find('input[type="text"]')[1].value;
    var ret = false;
    $('#tblDates tbody tr:not(:eq(' + $('#' + dateFieldId).parent().parent().index() + '))').each(function () {

        dtfrom = $(this).find('td input[type="text"]')[0].value;
        dtto = $(this).find('td input[type="text"]')[1].value;
        //if it is true;means the date selected has conflict

        if (dateClassObj.dates.isBetween(toBeComparedDate, dtfrom, dtto)) {
            ret = true;

            //to end each function
            return false;
            //("there's an overlap/Conflict in Specified Date", true);
        } else {
            //if the prev_from_date and prev_to_date is not null
            //double check if the current_from_date and the current_to_date
            //doesn't overlap or prev_from_date and prev_to_date is not between 
            //the current_from_date and the current_to_date
            //if it is true means there is a conflict and overlapping of dates
            if (dtfrom_current_selected != "" && dtto_current_selected != "") {
                if (dateClassObj.dates.isBetween(dtfrom, dtfrom_current_selected, dtto_current_selected) || dateClassObj.dates.isBetween(dtto, dtfrom_current_selected, dtto_current_selected)) {
                    //if true means there is a conflict in selected date
                    ret = true;
                    //to end each function
                    return false;
                }
            }
        }

    })//end of each

    return ret;
}


function initRemoveDate() {
    $('.removeDate').click(function () {
        $(this).closest('tr').remove();
        return false;
    })
}



function initDateField(startDateFieldId, endDateFieldId) {
    var dtNow = new Date();
    dtNow = dtNow.format('yyyy-MM-dd');

    var pickerStart = new Pikaday({
        field: document.getElementById(startDateFieldId),
        firstDay: 1,
        numberOfMonths: 1,
        format: 'MMM DD, YYYY',
        onSelect: function () {
            var dateClassObj = new date_class();
            var toDate = document.getElementById(endDateFieldId).value;
            if (toDate != "") {
                if (dateClassObj.dates.getdifference(this, toDate) < 0) {
                    notifyWarning("From Date Must be Less than To Date", true);
                    document.getElementById(startDateFieldId).value = "";
                }
            }
            var selectedDate = new Date(this);
            selectedDate = selectedDate.format('yyyy-MM-dd');
            if (selectedDate < dtNow) {
                notifyWarning("Start Date Must be Today and Above", true);
                document.getElementById(startDateFieldId).value = "";
            } else if (checkDateHasConflict(this, startDateFieldId) == true) {
                notifyWarning("there's an overlap/Conflict in Specified Date", true);
                document.getElementById(startDateFieldId).value = "";
            }
            //alert(this.getMoment().format('Do MMMM YYYY'));
        }
    });

    var pickerEnd = new Pikaday({
        field: document.getElementById(endDateFieldId),
        firstDay: 1,
        numberOfMonths: 1,
        format: 'MMM DD, YYYY',
        onSelect: function () {
            var dateClassObj = new date_class();
            //var fromDate = $('#' + endDateFieldId).closest('tr').find('td input[type="text"]')[0].value;
            var fromDate = document.getElementById(startDateFieldId).value;
            if (fromDate != "") {
                if (dateClassObj.dates.getdifference(fromDate, this) < 0) {
                    notifyWarning("To Date Must be greater than From Date", true);
                    document.getElementById(endDateFieldId).value = "";
                }
            }
            var selectedDate = new Date(this);
            selectedDate = selectedDate.format('yyyy-MM-dd');
            if (selectedDate < dtNow) {
                notifyWarning("End Date Must be Today and Above", true);
                document.getElementById(endDateFieldId).value = "";
            } else if (checkDateHasConflict(this, endDateFieldId) == true) {
                notifyWarning("there's an overlap/Conflict in Specified Date", true);
                document.getElementById(endDateFieldId).value = "";
            }
            //alert(this.getMoment().format('Do MMMM YYYY'));
        }
    });
}
//-----------------------------------------
//End of Region for multiple Date selection
//-----------------------------------------




function tblHasItem(tableId) {
    //checks if item already exists in the table
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

function updateQty(barcode, rewardApplication, qty) {
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

            } else if (response.status == 0) {
                notifyWarning(response.message);
            } else {
                notifyError(response.message);
            }
        } else if (xmlhttp.status == 500) {
            notifyError('Server Connection Error');
        }
    }
    xmlhttp.open("POST", "WebMethods/sellingAreaWebMethod.aspx/updateQty", true);
    xmlhttp.responseType = "json"
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xmlhttp.send("{'headId':'" + headId + "','barcode':'" + barcode + "','rewardApplication':" + rewardApplication + ",'qty':" + qty + "}");
}


function updateAgeCode(barcode, rewardApplication, ageCode) {
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

            } else if (response.status == 0) {
                notifyWarning(response.message);
            } else {
                notifyError(response.message);
            }
        } else if (xmlhttp.status == 500) {
            notifyError('Server Connection Error');
        }
    }
    xmlhttp.open("POST", "WebMethods/sellingAreaWebMethod.aspx/updateAgeCode", true);
    xmlhttp.responseType = "json"
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xmlhttp.send("{'headId':'" + headId + "','barcode':'" + barcode + "','rewardApplication':" + rewardApplication + ",'ageCode':'" + ageCode + "'}");
}



function removeItem(rewardApplication) {
    //REWARD APPLICATION
    //0 - freelist items
    //1 - buylist items
    var orin;
    var barcode;
    if (rewardApplication == 1) {
        orin = itemRow.ORIN;
        barcode = itemRow.BARCODE;
    } else {
        orin = freeListItemRow.ORIN;
        barcode = freeListItemRow.BARCODE;;
    }


    if (barcode == "" || barcode == null) {
        notifyWarning("Please select Item to remove");
    } else if (headId == null || headId == "") {
        notifyError("Head ID is null");
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

                    if (rewardApplication == 1) {
                        //removes the selected row from list
                        tblItems.row('.selected').remove().draw(false);
                    } else {
                        //removes the selected row from list
                        tblFreeListItems.row('.selected').remove().draw(false);
                    }

                    var total_items = $('#tblItems').find('tbody>tr').length;
                    $('#total_items').text(total_items);

                    notif({
                        msg: response.message,
                        type: "info",
                        timeout: 2000,
                        multiline: true,
                        position: "center"
                    });
                    $('#btnRemove').prop('disabled', true);
                } else if (response.status == 0) {
                    notifyWarning(response.message);
                } else {
                    notifyError(response.message);

                }
            } else if (xmlhttp.status == 500) {
                notifyError('Server Connection Error');
            }
        }
        xmlhttp.open("POST", "WebMethods/sellingAreaWebMethod.aspx/removeItem", true);
        xmlhttp.responseType = "json"
        xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        xmlhttp.send("{'headId':'" + headId + "','orin':'" + orin + "','barcode':'" + barcode + "','rewardApplication':" + rewardApplication + "}");
    }
}


//    function getPromoTypes() {
//------------the population of promo type has been created/populated statically-----------
//        //this function gets all list of promotypes 
//        //this populates the list to dropdown list
//        var xmlhttp;
//        if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
//            xmlhttp = new XMLHttpRequest();
//        }
//        else {// code for IE6, IE5
//            xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
//        }
//        xmlhttp.onreadystatechange = function () {
//            if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
//                var response;
//                response = jQuery.parseJSON(xmlhttp.response.d);
//                if (response.status == 1) {
//                    var types = response.types;
//                    varTest = types;
//                    for (var x = 0; x < types.length; x++) {
//                        var opt = document.createElement("option");

//                        opt.value = types[x].PROMO_TYPE_ID;
//                        opt.text = types[x].DESCRIPTION;
//                        document.getElementById("cbPromoTypes").appendChild(opt);
//                    }
//                    document.getElementById("cbPromoTypes").selectedIndex = 1;

//                    $('#cbPromoTypes').on('change', function () {
//                        if ($(this).find(":selected").val() == 0) {
//                            $('#chkFreeList').prop("disabled",false);
//                        } else {
//                            $('#chkFreeList').prop("checked", false);
//                            $('#chkFreeList').prop("disabled","disabled")
//                        }
//                    })

//                } else if (response.status == 0) {
//                    notifyWarning(response.message);
//                } else {
//                    notifyError(response.message);
//                }
//            } else if (xmlhttp.status == 500) {
//                notifyError('Server Connection Error');
//            }
//        }
//        xmlhttp.open("POST", "WebMethods/sellingAreaWebMethod.aspx/getPromoTypes", true);
//        xmlhttp.responseType = "json"
//        xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
//        xmlhttp.send();
//    }

function checkPromotionFields() {
    var loc = $("#locId").val();
    var dept = $("#deptId").val();

    if (loc == "") {
        notifyOnInput("Please Select Location", "warning", "#locName");
        return false;
    } else if (dept == "") {
        notifyOnInput("Please Select Department", "warning", "#deptName");
        return false;
    } else {
        return true;
    }
}


function chechHasEmptyQty(tableID) {
    var cntr = false;
    $('#' + tableID + ' tbody tr input[name="qty"]').each(function () {
        if (isEmpty($(this).val())) {
            cntr = true;
            return;
        }
    })
    return cntr;
}

function checkHasEmptyAgeCode(tableID) {
    var cntr = false;
    $('#' + tableID + ' tbody tr input[name="ageCode"]').each(function () {
        if (isEmpty($(this).val())) {
            cntr = true;
            return;
        }
    })
    return cntr;
}


function checkIfItemExist(tableID) {
    var barcode;
    var rows;
    if (tableID == "tblItems") {
        barcode = $("#txtbarcode").val().trim();
        rows = $('#tblItems').find('tbody>tr');
    } else {
        barcode = $("#txtBarcodeFreeList").val().trim();
        rows = $('#tblItemsFreeList').find('tbody>tr');
    }


    //checks if the table is empty and only shows the default message "No data available in table"
    //if it is empty, disregard checking
    if (rows[0].cells.length != 1) {
        for (var x = 0; x < rows.length; x++) {
            if (rows[x].cells[1].innerHTML.trim() == barcode) {
                return true;
            }
        }
    }
    return false;
}

function getPromoDate(head) {
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
                populateDateFields(response.dates);
                ShowAllDateElements(true);
            } else if (response.status == 0) {
                //notifyWarning(response.message);
            } else {
                notifyError(response.message);
            }
        } else if (xmlhttp.status == 500) {
            notifyError('Server Connection Error');
        }
    }
    xmlhttp.open("POST", "WebMethods/sellingAreaWebMethod.aspx/getPromoDate", true);
    xmlhttp.responseType = "json"
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xmlhttp.send("{'headId':'" + head + "'}");
}

function getTransactionDetail(head) {
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

                var transactionDetail = response.transactionDetail[0];
                $('#locName').val(transactionDetail.LOC + ' - ' + transactionDetail.LOC_NAME);
                $('#locId').val(transactionDetail.LOC);

                $('#deptName').val(transactionDetail.DEPT + ' - ' + transactionDetail.DEPT_NAME);
                $('#deptId').val(transactionDetail.DEPT);
                $('#txtPromoDescription').val(transactionDetail.PROMOLIST_DESC);
                $('#lblTranNo').text(transactionDetail.TRAN_ID);
                $('#cbPromoTypes').val(transactionDetail.PROMO_TYPE);
                disableFilterFields();

                getItemsTransaction(headId, 1);

                //this is to check if the transaction is multibuy and has a freelist items
                if (transactionDetail.PROMO_TYPE == 0 && transactionDetail.HAS_FREELIST == 1) {
                    $('#chkFreeList').prop("checked", 'checked');
                    $('#divFreeList').show();
                    getItemsTransaction(headId, 0);


                }


                tranStatus = transactionDetail.STATUS;



                //this is to restrict adding of items
                if (tranStatus == 'null' || tranStatus == null) {
                    tranStatus = null;
                    if (transactionDetail.TRAN_ID != null) {
                        $("#txtPromoDescription").prop('disabled', 'disabled');
                    }

                } else {
                    $("#txtbarcode").prop('disabled', 'disabled');
                    $("#txtBarcodeFreeList").prop('disabled', 'disabled');
                    $("#btnRemove").prop('disabled', 'disabled');
                    $('#btnDone').prop('disabled', 'disabled');
                    $("#btnCancelTransaction").prop('disabled', 'disabled');
                    $("#txtPromoDescription").prop('disabled', 'disabled');


                }
            } else if (response.status == 0) {
                notifyWarning(response.message);
            } else {
                notifyError(response.message);

            }
        }
    }
    xmlhttp.open("POST", "WebMethods/sellingAreaWebMethod.aspx/getTransactionDetail", true);
    xmlhttp.responseType = "json"
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xmlhttp.send("{'headId':'" + head + "'}");

}

function insertItem(listType) {
    var barcode;
    var rewardApplication;
    if (listType == "buylist") {
        barcode = $("#txtbarcode").val();
        rewardApplication = 1;
    } else {
        barcode = $("#txtBarcodeFreeList").val();
        rewardApplication = 0;
    }


    var loc = $('#locId').val();
    var dept = $('#deptId').val();
    var promoType = $('#cbPromoTypes').val();
    var hasFreeList;
    var promoDates = getPromoDates();


    if ($('#chkFreeList').prop("checked") == true) {
        hasFreeList = 1;
    } else {
        hasFreeList = 0;
    }


    //    if (headId == '' || headId == null || headId == 'null') {
    //        promoDates = getPromoDates();
    //    } else {
    //        promoDates = [];
    //    }

    //    if(_earliestDate == null){
    //        _earliestDate = getEarliestDate();
    //    }

    //    if(_latestDate == null){
    //        _latestDate = getLatestDate();
    //    }

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
                headId = response.headId;
                var item = response.item[0];

                if (listType == "buylist") {
                    disableBarcodeElement(listType);

                    tblItems.row.add({
                        "ORIN": item.ORIN,
                        "BARCODE": item.BARCODE,
                        "VPN": item.VPN,
                        "ITEM_DESC": item.ITEM_DESC,
                        "AGE_CODE": item.AGE_CODE,
                        "QTY": item.QTY,
                        "SRP": item.SRP
                    }).draw();

                    var total_items = $('#tblItems').find('tbody>tr').length;
                    $('#total_items').text(total_items);

                    initTblItemsEvents();
                    disableAllDateElements();

                } else {
                    disableBarcodeElement(listType);

                    tblFreeListItems.row.add({
                        "ORIN": item.ORIN,
                        "BARCODE": item.BARCODE,
                        "VPN": item.VPN,
                        "ITEM_DESC": item.ITEM_DESC,
                        "AGE_CODE": item.AGE_CODE,
                        "QTY": item.QTY,
                        "SRP": item.SRP
                    }).draw();

                    var total_freeList_items = $('#tblItemsFreeList').find('tbody>tr').length;
                    $('#total_freeList_items').text(total_freeList_items);

                    initTblFreeListItemsEvents();
                }

                $(".qtyClass").delimit({
                    numbers: true,
                    backSpace: true
                });

                $(".ageCodeClass").delimit({
                    letters: true,
                    backSpace: true
                });


                disableFilterFields();

            } else if (response.status == 0) {
                disableBarcodeElement(listType);
                notifyWarning(response.message);
            } else {
                disableBarcodeElement(listType);
                notifyError(response.message);
            }
        } else if (xmlhttp.status == 500) {
            notifyError('Server Connection Error');
            disableBarcodeElement(listType);
        }
    }
    xmlhttp.open("POST", "WebMethods/sellingAreaWebMethod.aspx/insertItem", true);
    xmlhttp.responseType = "json"
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    //xmlhttp.send("{'barcode':'" + barcode + "','headId':'" + headId + "','location':'" + loc + "', 'department':'" + dept + "','promo_datesObj':'"+ promoDates +"'}");
    xmlhttp.send(JSON.stringify({ barcode: barcode, headId: headId, location: loc, department: dept, promoType: promoType, hasFreeList: hasFreeList, rewardApplication: rewardApplication, promo_datesObj: promoDates }));
}

function disableBarcodeElement(listType) {
    if (listType == "buylist") {
        $('#txtbarcode').val('');
        $('#txtbarcode').prop('disabled', false);
    } else {
        $('#txtBarcodeFreeList').val('');
        $('#txtBarcodeFreeList').prop('disabled', false);
    }
}

function saveTransaction() {

    var description = encodeURI($('#txtPromoDescription').val());
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
                hideModal('PopUpOutline');
                tranId = response.tranId;
                $('#lblTranNo').text(tranId);
                $('#btnDone').prop('disabled', 'disabled');
                $("#btnCancelTransaction").prop('disabled', 'disabled');
                $("#txtPromoDescription").prop('disabled', 'disabled');
                $("#txtbarcode").prop('disabled', 'disabled');
                $("#btnRemove").prop('disabled', 'disabled');
                $("#tblItems tbody tr input[type='text']").prop('disabled', 'disabled');
                tranStatus = 'WORKSHEET';
                notif({
                    msg: response.message,
                    type: "info",
                    timeout: 3000,
                    multiline: true,
                    position: "center"
                });

            } else if (response.status == 0) {
                notifyWarning(response.message);
            } else {
                notifyError(response.message);
            }
        } else if (xmlhttp.status == 500) {
            notifyError('Server Connection Error');
            disableBarcodeElement(listType);
        }
    }
    xmlhttp.open("POST", "WebMethods/sellingAreaWebMethod.aspx/generateTransactionNumber", true);
    xmlhttp.responseType = "json"
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    //xmlhttp.send("{'headId':'" + headId + "','description':'" + description + "'}");
    xmlhttp.send(JSON.stringify({ headId: headId, description: description }));
}


function cancelTransaction() {
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
                    timeout: 3000,
                    multiline: true,
                    position: "center"
                });


                window.location.href = "SellingAreaDashboard.aspx";
            } else if (response.status == 0) {
                notifyWarning(response.message);
            } else {
                notifyError(response.message);
            }
        } else if (xmlhttp.status == 500) {
            notifyError('Server Connection Error');
            disableBarcodeElement(listType);
        }
    }
    xmlhttp.open("POST", "WebMethods/sellingAreaWebMethod.aspx/cancelTransaction", true);
    xmlhttp.responseType = "json"
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xmlhttp.send("{'headId':'" + headId + "'}");
}


function getItemsTransaction(head, rewardApplication) {
    //REWARD APPLICATION
    //0 - freelist items
    //1 - buylist items

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

                //this means that items to be loaded are buylist items
                if (rewardApplication == 1) {
                    tblItems = $('#tblItems').DataTable({
                        "scrollY": "330px",
                        "paging": false,
                        "info": false,
                        "data": response.item,
                        "columns": [
                                 { "data": "ORIN" },
                                 { "data": "BARCODE" },
                                 { "data": "VPN" },
                                 { "data": "ITEM_DESC" },
                                 {
                                     "data": "AGE_CODE",
                                     "width": "60px",
                                     "render": function (data, type, row, meta) {
                                         if (data == null) {
                                             return "<input type='text' class='inputs ageCodeClass' tabindex='1' name='ageCode' maxlength='2' style='width:50px;' value='' />";
                                         } else {
                                             return "<input type='text' class='inputs ageCodeClass' tabindex='1' name='ageCode' maxlength='2' style='width:50px;' value='" + data + "' />";
                                         }

                                     }
                                 },
                                 {
                                     "data": "QTY",
                                     "width": "40px",
                                     "render": function (data, type, row, meta) {
                                         if (data == null) {
                                             return "<input type='text' class='inputs qtyClass' name='qty' tabindex='1' style='width:40px;' value='' />";
                                         } else {
                                             return "<input type='text' class='inputs qtyClass' name='qty' tabindex='1' style='width:40px;' value='" + data + "' />";
                                         }

                                     }
                                 },
                                 {
                                     "data": "SRP",
                                     "width": "40px"
                                 }
                        ]
                    });

                    if (tblHasItem("tblItems")) {
                        var total_items = $('#tblItems').find('tbody>tr').length;
                        $('#total_items').text(total_items);
                    }

                    if (tranStatus != null) {
                        $("#tblItems tbody tr input[type='text']").prop('disabled', 'disabled');
                    }


                    $(".qtyClass").delimit({
                        numbers: true,
                        backSpace: true,
                        control: true
                    });

                    $(".ageCodeClass").delimit({
                        letters: true,
                        backSpace: true,
                        control: true
                    });

                    initTblItemsEvents();
                } else {
                    //this means that items to be loaded are freelist items
                    tblFreeListItems = $('#tblItemsFreeList').DataTable({
                        "scrollY": "330px",
                        "paging": false,
                        "info": false,
                        "data": response.item,
                        "columns": [
                                 { "data": "ORIN" },
                                 { "data": "BARCODE" },
                                 { "data": "VPN" },
                                 { "data": "ITEM_DESC" },
                                 {
                                     "data": "AGE_CODE",
                                     "width": "60px",
                                     "render": function (data, type, row, meta) {
                                         if (data == null) {
                                             return "<input type='text' class='inputs ageCodeClass' tabindex='1' name='ageCode' maxlength='2' style='width:50px;' value='' />";
                                         } else {
                                             return "<input type='text' class='inputs ageCodeClass' tabindex='1' name='ageCode' maxlength='2' style='width:50px;' value='" + data + "' />";
                                         }

                                     }
                                 },
                                 {
                                     "data": "QTY",
                                     "width": "40px",
                                     "render": function (data, type, row, meta) {
                                         if (data == null) {
                                             return "<input type='text' class='inputs qtyClass' tabindex='1' name='qty' style='width:40px;' value='' />";
                                         } else {
                                             return "<input type='text' class='inputs qtyClass' tabindex='1' name='qty' style='width:40px;' value='" + data + "' />";
                                         }

                                     }
                                 },
                                 { "data": "SRP" }
                        ]
                    });

                    if (tblHasItem("tblItemsFreeList")) {
                        var total_freeList_items = $('#tblItemsFreeList').find('tbody>tr').length;
                        $('#total_freeList_items').text(total_freeList_items);
                    }

                    if (tranStatus != null) {
                        $("#tblItemsFreeList tbody tr input[type='text']").prop('disabled', 'disabled');
                    }

                    $(".qtyClass").delimit({
                        numbers: true,
                        backSpace: true
                    });

                    $(".ageCodeClass").delimit({
                        letters: true,
                        backSpace: true
                    });

                    initTblFreeListItemsEvents();
                }


            } else if (response.status == 0) {
                notifyWarning(response.message);
            } else {
                notifyError(response.message);
            }
        }
    }
    xmlhttp.open("POST", "WebMethods/sellingAreaWebMethod.aspx/getItemsTransaction", true);
    xmlhttp.responseType = "json"
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xmlhttp.send("{'headId':'" + head + "','rewardApplication':" + rewardApplication + "}");
}


function disableFilterFields() {
    $('#locName').prop('disabled', 'disabled');
    $('#deptName').prop('disabled', 'disabled');
    $('#cbPromoTypes').prop('disabled', 'disabled');
    $('#chkFreeList').prop("disabled", "disabled");
};


// ADDED FUNCTION TO CHECK ONGOING TRAN (PROMOTION MARKDOWN) - JAYSON SALINAS V 2.0.0.7
function checkOngoingTran(item) {
    var loc = $('#locId').val();
    var dept = $('#deptId').val();
    var tranId = 0;
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
                if (response.message == null) {
                    checkExistingTran(item);
                } else {
                    notifyWarning(response.message);
                    insertItem("buylist");
                }
            } else if (response.status == 0) {
                notifyError(response.message);
            } else {
                notifyError(response.message);
            }
        } else if (xmlhttp.status == 500) {
            notifyError('Server Connection Error');
            document.getElementById(txtDateElementId).value = "";
        }
    }
    xmlhttp.open("POST", "WebMethods/generalWebMethod.aspx/checkOngoingTran", true);
    xmlhttp.responseType = "json";
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xmlhttp.send("{'item':'" + item + "','location':" + loc + ",'department':" + dept + ",'tranId':" + tranId + "}");
}

// ADDED FUNCTION TO CHECK EXISTING TRAN (PROMOTION MARKDOWN) - JAYSON SALINAS V 2.0.0.10
function checkExistingTran(item) {
    var loc = $('#locId').val();
    var dept = $('#deptId').val();
    var tranId = 0;
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
                if (response.message == null) {
                    insertItem("buylist");
                } else {
                    insertItem("buylist");
                    notifyWarning(response.message);                    
                }
            } else if (response.status == 0) {
                insertItem("buylist");
                notifyWarning(response.message);
            } else {
                notifyError(response.message);
            }
        } else if (xmlhttp.status == 500) {
            notifyError('Server Connection Error');
            document.getElementById(txtDateElementId).value = "";
        }
    }
    xmlhttp.open("POST", "WebMethods/generalWebMethod.aspx/checkExistingTran", true);
    xmlhttp.responseType = "json";
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xmlhttp.send("{'item':'" + item + "','location':" + loc + ",'department':" + dept + ",'tranId':" + tranId + "}");
}