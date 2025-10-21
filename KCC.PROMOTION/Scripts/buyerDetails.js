var tblItems = null;
var itemRow = null;
var selectedRowElem = null;
var tblFreeListItems = null;
var freeListItemRow = null;
var varTest;
var theElement;
var dateIdCntr = 0;
var transactionDetail = null;
var conflictCnt = 0;
var negativeMUP = 0;
$(function () {
    var $scrollingLoader = $("#loadingcontainer");
    $(window).scroll(function (e) {
        $scrollingLoader.stop().animate({ "marginTop": ($(window).scrollTop() + 2000) + "px" }, "slow");
    });
    //gets the head Id from the hidden field
    var headId = $('#hfHeadId').val();
    $('#divFreeList').hide();
    $('#chkSet').prop('checked', false);
    $('#chkFreeSet').prop('checked', false);
    getTransactionDetail(headId);
    initEvent();

});

// ADDED BUTTON REMOVE.ONCLICK FUNCTION - JAYSON SALINAS V 2.0.0.7
function initEvent() {
    $("#lnkLogoutCntr").on("click", function () {
        if (confirm("Do you want to logout?")) {
            javascript: __doPostBack('ctl00$MainContent$lnklogout', '');
        }
    })

    $('#btnPreview').on('click', function () {
        window.location.href = "buyerReportViewer.aspx?headId=" + $('#hfHeadId').val();
        // window.open("buyerReportViewer.aspx?headId=" + $('#hfHeadId').val(),"_blank");
    })

    $("#txtDiscount").delimit({
        numbers: true,
        backSpace: true,
        dot: true
    })

    $("#txtDiscountMultiple").delimit({
        numbers: true,
        backSpace: true,
        dot: true
    })

    $('#btnSubmit').on('click', function () {
        if (transactionDetail.PROMO_TYPE == '1' && checkIfSetlAllItems() == false) {
            //check if the promo type is simple
            //if not, it should not check settingOfAllItems
            //multibuy and threshold dont require discounts
            notifyWarning("Please Set Discount for all Items", true);
        }
            //else if (checkIfSetAllDate() == false) {
            //    notifyWarning("Please Select Date Before Setting new Date Or Saving", true);
            //}
        else if ($('#txtPromoDescription').val().length < 35) {
            notifyWarning("Description must be 35 characters and above", true);
        } else {
            if (negativeMUP == 1) {
                swal({
                    title: '<h1 style="color:red"><strong>WARNING!</strong></h1>',
                    type: 'error',
                    html:
                      '<b>THE TRANSACTION HAS NEGATIVE MARK-UP!</b> <br> ' +
                      '<p style ="color:Red">DO <B><u>NOT</u></b> SUBMIT TRANSACTION!</p>',
                    focusConfirm: false,
                    confirmButtonText:
                      '<i class="fa fa-thumbs-up"></i> OKAY!',
                    confirmButtonAriaLabel: 'Thumbs up, great!',
                }).then(function () {
                    if (confirm("Do you still want to submit this transaction with negative mark-up?")) {
                        if (conflictCnt > 0) {
                            if (confirm("There are Items with Ongoing Discount. Proceed Anyways?")) {
                              

                                submit();
                                $("#btnPreview").prop("disabled", false);
                                //var tranNo = document.getElementById('lblTranNo').innerHTML;
                                //emailTrigger(tranNo);
                            }
                        } else {

                            submit();
                            $("#btnPreview").prop("disabled", false);
                                //var tranNo = document.getElementById('lblTranNo').innerHTML;
                                //emailTrigger(tranNo);
                            
                        }
                    }
                })
            } else if (negativeMUP != 1) {
                if (confirm("Do you want to submit this transaction?")) {
                    if (conflictCnt > 0) {
                        if (confirm("There are Items with Ongoing Discount. Proceed Anyways?")) {
                            submit();
                        }
                    } else {
                        submit();
                    }
                }
            } else {
                notifyWarning("WARNING:", "Please contact System Administrator!");
            }         
            
        }
    })

    $('#btnBack').on('click', function () {
        if ($('#txtPromoDescription').val().length >= 35) {
            if (confirm("This will return the status to previous,do You want to continue?")) {
                updateTransactionStatus();
            }
        } else {
            notifyWarning("Description must be 35 characters and above", true);
        }
    })

    $('#txtDiscount').onEnter(function () {
        if (validateDiscountField()) {
            updateItemDiscount();
        }
    })

    $('#txtDiscount').on('focusout', function () {
        this.value = this.value.replace(/[^0-9.]/g, '');
        return false;
    })

    $('#txtDiscountMultiple').on('focusout', function () {
        this.value = this.value.replace(/[^0-9.]/g, '');
        return false;
    })

    $('#btnUpdateItem').on('click', function () {
        if (validateDiscountField()) {
            updateItemDiscount();
        }
    })

    $('#btnCancelUpdate').on('click', function () {
        $(selectedRowElem).removeClass('selected');
        $('#dvRowEditorMain').css("display", "none");
        clearRowEditor();
    })

    $('#btnSet').on('click', function () {
        $('#txtDiscountMultiple').val('');
        revealModal('PopUpSetMultipleOutline');
    })

    $("#btnRemove").on("click", function () {
        if ($("#tblItems tbody input[type='checkbox']").filter(':checked').length > 0) {
            if (confirm("Do you want to remove Selected Items?")) {
                removeItem(1);
            }
        } else {
            notifyWarning("Please Select item to remove", true);
        }
    })

    $("#btnRemoveFreeList").on("click", function () {
        if ($("#tblItemsFreeList tbody input[type='checkbox']").filter(':checked').length > 0) {
            if (confirm("Do you want to remove Selected Items?")) {
                removeItem(0);
            }
        } else {
            notifyWarning("Please Select item to remove", true);
        }
    })


    $('#btnSetMultipleDiscount').on('click', function () {
        if (validateMultipleDiscountField()) {
            if ($("#tblItems tbody input[type='checkbox']").filter(':checked').length > 0) {
                if (confirm("Do you want to Set Promotion for Selected Items?")) {
                    updateMultipleItemDiscount();
                }
            } else {
                notifyWarning("Please Select item to update", true);
            }
        }
    })


} //end of initialization

function clearRowEditor() {
    $('#lblOrin').text('');
    $('#lblBarcode').text('');
    $('#lblVPN').text('');
    $('#lblItemDesc').text('');
    $('#lblUnitCost').text('');
    $('#lblSRP').text('');
    $('#lblPromoSRP').text('');
    $('#lblPromoMarkUp').text('');
}

function updateTransactionStatus() {
    var head = $('#hfHeadId').val();
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
                    autohide: true,
                    multiline: true,
                    position: "center"
                });

                if (response.transactionStatus == "") {
                    window.location.href = "BuyerDashboard.aspx";
                } else if (response.transactionStatus.toUpperCase() == "WORKSHEET" || response.transactionStatus.toUpperCase() == "SUBMITTED") {
                    window.location.href = "BuyerDetails.aspx?headId=" + head;
                    //getTransactionDetail(head);
                    //clear all date fields before populating new
                    // $("#tblDates tr").remove();
                    //getPromoDate(head);
                }

            } else if (response.status == 0) {
                notifyWarning(response.message);
            } else {
                notifyError(response.message);
            }
        } else if (xmlhttp.status == 500) {
            notifyError('Server Connection Error');
        }
    }
    xmlhttp.open("POST", "WebMethods/generalWebMethod.aspx/updateTransactionStatus", true);
    xmlhttp.responseType = "json"
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xmlhttp.send("{'headId':'" + head + "'}");
}


function getPromoDate(head, tranStatus) {
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
                populateDateFields(response.dates, tranStatus);
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


function populateDateFields(dateObj, tranStatus) {
    for (var x = 0; x < dateObj.length; x++) {
        var tbl = document.getElementById('tblDates');
        var row = tbl.insertRow(-1);
        var cellStartDate = row.insertCell(0);
        var cellEndDate = row.insertCell(1);
        var cellRemoveAddDate = row.insertCell(2);

        cellStartDate.innerHTML = "<input type='text' id='txtStartDate" + dateIdCntr + "' value='" + dateObj[x].START_DATE + "' class='inputs' readonly='readonly'/>";
        cellEndDate.innerHTML = "<input type='text' id='txtEndDate" + dateIdCntr + "' value='" + dateObj[x].END_DATE + "' class='inputs' readonly='readonly'/>";

        //if (x == 0) {
        //    cellRemoveAddDate.innerHTML = "<input id='btnAddDate' type='button' value='Add' class='button-primary'/>";

        //    $("#btnAddDate").on("click", function () {
        //        if (checkIfSetAllDate()) {
        //            var tbl = document.getElementById('tblDates');
        //            var row = tbl.insertRow(-1);
        //            var cellStartDate = row.insertCell(0);
        //            var cellEndDate = row.insertCell(1);
        //            var cellRemove = row.insertCell(2);

        //            cellStartDate.innerHTML = "<input id='txtStartDate" + dateIdCntr + "'  type='text' class='inputs' readonly='readonly' placeholder='Start Date'/>";
        //            cellEndDate.innerHTML = "<input id='txtEndDate" + dateIdCntr + "'  type='text' class='inputs' readonly='readonly' placeholder='End Date'/>";
        //            cellRemove.innerHTML = '<input type="button" class="button-primary removeDate" value="Remove"/>';

        //            initDateField('txtStartDate' + dateIdCntr, 'txtEndDate' + dateIdCntr);
        //            initRemoveDate();
        //            disableAllDateElementsNotLast();
        //            dateIdCntr += 1;
        //        } else {
        //            notifyWarning("Please Select Date Before Setting new Date Or Saving", true); 
        //        }
        //    })

        //} else {
        //    cellRemoveAddDate.innerHTML = "<input type='button' value='Remove' class='button-primary removeDate'/>";
        //}
        initDateField('txtStartDate' + dateIdCntr, 'txtEndDate' + dateIdCntr);
        dateIdCntr += 1;
    }

    disableAllDateElements();

    //initRemoveDate();
    //if (tranStatus != 'WORKSHEET') {
    //    disableAllDateElements();
    //} else {
    //    disableAllDateElementsNotLast();
    //}  
}

function checkIfSetAllDate() {
    var cntr = true;
    $('#tblDates tbody tr input[type="text"]').each(function () {
        if ($(this).val() == '') {
            var elemId = $(this).prop('id');
            $("#" + elemId).focus();
            // notifyOnInput("Please Select Date Before Setting new Date Or Saving", "warning", "#" + elemId);
            cntr = false;
            return false;
        }
    })

    if (cntr == true)
        return true;
    else
        return false;
} //end of checkIfSetAllDate function


function initRemoveDate() {
    $('.removeDate').click(function () {
        $(this).closest('tr').remove();
        enableLastDateElement();
        return false;
    })
}


function getPromoDates() {
    //this function gets all dates
    //and returns an array of dates
    //similar function (getPromodate) -- please dont be confused
    var promoDateObjArr = [];

    $('#tblDates tbody tr').each(function () {
        var listDates = { startDate: $(this).find('input[type="text"]')[0].value, endDate: $(this).find('input[type="text"]')[1].value };
        promoDateObjArr.push(listDates);
    })
    //promoDateObjArr = JSON.stringify(promoDateObjArr);
    return promoDateObjArr;
}


function checkConflict(txtDateElementId, startDate, endDate) {
    var loc = $('#locId').val();
    var dept = $('#deptId').val();
    var headId = $('#hfHeadId').val()
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
                document.getElementById(txtDateElementId).value = "";
            } else {
                notifyError(response.message);
                document.getElementById(txtDateElementId).value = "";
            }
        } else if (xmlhttp.status == 500) {
            notifyError('Server Connection Error');
            document.getElementById(txtDateElementId).value = "";
        }
    }
    xmlhttp.open("POST", "WebMethods/buyerWebMethod.aspx/getPromotionConflict", true);
    xmlhttp.responseType = "json";
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xmlhttp.send("{'headId':'" + headId + "','location':" + loc + ",'department':" + dept + ",'startDate':'" + startDate + "','endDate':'" + endDate + "'}");

}

function checkDateHasConflict(toBeComparedDate) {
    var dtfrom;
    var dtto;
    var dateClassObj = new date_class();

    var currentRow = $('#tblDates tbody tr:last');
    var dtfrom_current_selected = $(currentRow).find('input[type="text"]')[0].value;
    var dtto_current_selected = $(currentRow).find('input[type="text"]')[1].value;
    var ret = false;

    $('#tblDates tbody tr:not(:last-child)').each(function () {

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
            selectedDate = selectedDate.format('yyyy-MM-dd')

            if (selectedDate < dtNow) {
                notifyWarning("Start Date Must be Today and Above", true);
                document.getElementById(startDateFieldId).value = "";
            } else if (checkDateHasConflict(this) == true) {
                notifyWarning("there's an overlap/Conflict in Specified Date", true);
                document.getElementById(startDateFieldId).value = "";
            } else if (document.getElementById(endDateFieldId).value != "") {
                checkConflict(startDateFieldId, this, toDate)
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
            selectedDate = selectedDate.format('yyyy-MM-dd')
            if (selectedDate < dtNow) {
                notifyWarning("End Date Must be Today and Above", true);
                document.getElementById(endDateFieldId).value = "";
            } else if (checkDateHasConflict(this) == true) {
                notifyWarning("there's an overlap/Conflict in Specified Date", true);
                document.getElementById(endDateFieldId).value = "";
            } else if (document.getElementById(startDateFieldId).value != "") {
                checkConflict(endDateFieldId, fromDate, this)
            }
        }
    });
}

function disableAllDateElements() {
    $('#tblDates tbody tr input').prop('disabled', 'disabled');
}
function disableAllDateElementsNotLast() {
    $('#tblDates tbody tr:not(:last-child) input[type="text"]').prop('disabled', 'disabled');
}

function enableLastDateElement() {
    $('#tblDates tbody tr:last-child input[type="text"]').prop('disabled', '');
}


function submit() {
    var headId = $('#hfHeadId').val();
    var description = $('#txtPromoDescription').val();
    var promoDates = getPromoDates();

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
                    multiline: true,
                    position: "center"
                });

                $("#btnSubmit").prop("disabled", "disabled");
                $('#txtPromoDescription').prop('disabled', 'disabled');
                $('#btnSet').prop('disabled', 'disabled');
                $('#btnRemove').prop('disabled', 'disabled');

                $('#btnSet').hide();
                $('#btnRemove').hide();

                //$("#btnPreview").prop("disabled", "disabled");

                $('#dvRowEditorMain').css("display", "none");
                clearRowEditor();

                $("#tblItems tbody input[type='checkbox']").prop('disabled', 'disabled');
                $("#chkSet").prop('disabled', 'disabled');

                $("#tblItemsFreeList tbody input[type='checkbox']").prop('disabled', 'disabled');
                $("#chkFreeSet").prop('disabled', 'disabled');

                //to unbind/off the click eventHandler of the table
                $("#tblItems tbody").off('click', 'tr td:not(:first-child)');
                disableAllDateElements();

                getItemsTransaction(headId, 1);

            } else if (response.status == 0) {
                notifyWarning(response.message);
            } else {
                notifyError(response.message);
            }
        } else if (xmlhttp.status == 500) {
            notifyError('Server Connection Error');
        }
    }
    xmlhttp.open("POST", "WebMethods/buyerWebMethod.aspx/submit", true);
    xmlhttp.responseType = "json"
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    // xmlhttp.send("{'headId':'" + headId + "','description':'" + description + "','promo_datesObj': '" + promoDates +"'}");
    xmlhttp.send(JSON.stringify({ headId: headId, description: description, promo_datesObj: promoDates }));
} //end of submit Function


function checkIfSetlAllItems() {
    var cntr = true;
    $("#tblItems tbody tr").each(function () {
        var row = tblItems.row(this);
        var rowData = row.data();
        if (rowData.DISCOUNT == 'null' || rowData.DISCOUNT == null || rowData.DISCOUNT == '') {
            cntr = false;
            return false;
        }
    });   //end of each;

    if (cntr == false)
        return false;
    else
        return true;

}

function validateMultipleDiscountField() {
    var discount = $('#txtDiscountMultiple').val();
    var clr_type = $('#ddClrTypeMultiple').val();
    if (discount == '') {
        notifyWarning("Please Enter Discount Value", true);
        return false;
    } else if (clr_type == null) {
        notifyWarning("Please Select Promotion Type", true);
        return false;
    } else {
        return true;
    }

}

function validateDiscountField() {
    var discount = $('#txtDiscount').val();
    var clr_type = $('#ddClrType').val();
    if (discount == '') {
        notifyWarning("Please Enter Discount Value", true);
        return false;
    } else if (clr_type == null) {
        notifyWarning("Please Select Promotion Type", true);
        return false;
    } else {
        return true;
    }
}


// ADDED BTNREMOVE FUNCTIONS - JAYSON SALINAS V 2.0.0.7
function initTblItemsEvents() {


    $("#chkSet").change(function (e) {
        if (this.checked) {
            $('tbody tr td input[type="checkbox"]').prop('checked', this.checked);
            $('#dvRowEditorMain').css("display", "none");
            $('#btnSet').show();
            $('#btnRemove').show();
        } else {
            $('tbody tr td input[type="checkbox"]').prop('checked', false);
            $('#btnSet').hide();
            $('#btnRemove').hide();
        }
    })


    $("#tblItems tbody tr td:first-child input[type='checkbox']").change(function () {
        $('#tblItems tr.selected').removeClass('selected');
        $('#dvRowEditorMain').css("display", "none");

        var totalCheckBox = $("#tblItems tbody input[type='checkbox']").length;
        var totalUnchecked = $("#tblItems tbody input[type='checkbox']").not(':checked').length;
        var totalChecked = $("#tblItems tbody input[type='checkbox']").filter(':checked').length;

        if ($("#cbPromoTypes").val() != 1) {
            if (totalChecked == totalCheckBox) {
                $('#chkSet').prop('checked', true);
                $('#btnRemove').show();
                $('#btnSet').hide();
            } else if (totalUnchecked == totalCheckBox) {
                $('#chkSet').prop('checked', false);
                $('#btnRemove').hide();
                $('#btnSet').hide();
            } else if (totalChecked >= 1) {
                if (totalCheckBox != totalChecked) {
                    $('#chkSet').prop('checked', false);
                }
                $('#btnRemove').show();
                $('#btnSet').hide();
            } else {
                $('#btnRemove').hide();
                $('#btnSet').hide();
            }
        } else {
            if (totalChecked == totalCheckBox) {
                $('#chkSet').prop('checked', true);
                $('#btnRemove').show();
                $('#btnSet').show();
            } else if (totalUnchecked == totalCheckBox) {
                $('#chkSet').prop('checked', false);
                $('#btnRemove').hide();
                $('#btnSet').hide();
            } else if (totalChecked >= 1) {
                if (totalCheckBox != totalChecked) {
                    $('#chkSet').prop('checked', false);
                }
                $('#btnRemove').show();
                $('#btnSet').show();
            } else {
                $('#btnRemove').hide();
                $('#btnSet').hide();
            }
        }


    })






    //initializes all events used by "tblItems"
    $("#tblItems tbody").on('click', 'tr td:not(:first-child)', function () {

        var tr = $(this).closest('tr');
        if ($(tr).hasClass('selected')) {
            $(tr).removeClass('selected');
            selectedBarcode = null;
            $('#dvRowEditorMain').css("display", "none");
            clearRowEditor();
        } else {
            $("#tblItems tr.selected").removeClass('selected');

            if (transactionDetail.PROMO_TYPE == "1") {
                //to show the editor for Simple(type) only
                //threshold and multibuy depend on the description of the promotion
                $('#dvRowEditorMain').css("display", "block");
            }

            $(tr).addClass('selected');
            var row = tblItems.row(tr);
            itemRow = row.data();
            selectedRowElem = tr;

            $('#lblOrin').text(itemRow.ORIN);
            $('#lblBarcode').text(itemRow.BARCODE);
            $('#lblVPN').text(itemRow.VPN);
            $('#lblItemDesc').text(itemRow.ITEM_DESC);
            $('#lblUnitCost').text(itemRow.UNIT_COST);
            $('#lblSRP').text(itemRow.SRP);
            $('#lblPromoSRP').text(itemRow.PROMO_SRP);
            $('#lblPromoMarkUp').text(itemRow.PROMO_MARK_UP);
            $('#txtDiscount').val(itemRow.DISCOUNT);
            $('#ddClrType').val(itemRow.CLR_TYPE);

        }

        //        elementPos = theElement.position();
        //        var posY = elementPos.top;
        //        var parentElem = document.getElementById('tblItems').parentElement;
        //        var finalCalc = (posY - parentElem.scrollHeight) + parentElem.scrollTop;

        //        $('#dvRowEditorMain').animate({ top: +finalCalc + "px" }, 500);
        //        $('#dvRowEditorButtons').css("display", "block");
        //        $('#dvRowEditorButtons').animate({ top: +finalCalc + "px" }, 500);
    });




}



function updateMultipleItemDiscount() {
    hideModal('PopUpSetMultipleOutline');
    revealModal('loading');

    var _discount = $('#txtDiscountMultiple').val();
    var _clr_type = $('#ddClrTypeMultiple').val();
    var itemDiscountObjArr = [];
    var isChecked = false;
    var row;
    var rowData;
    $("#tblItems tbody tr").each(function () {
        isChecked = $(this).find("input[type='checkbox']").prop('checked');
        if (isChecked) {
            row = tblItems.row(this);
            rowData = row.data();

            rowData.DISCOUNT = $('#txtDiscountMultiple').val();
            rowData.CLR_TYPE = $('#ddClrTypeMultiple').val();
            rowData.CLR_TYPE_DESC = $('#ddClrTypeMultiple option:selected').text();

            itemDiscountObjArr.push(rowData);
        }
    })//end of each


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

                var headId = $('#hfHeadId').val();
                tblItems.destroy();
                negativeMUP = 0;
                getItemsTransaction(headId, 1);

                $('#chkSet').prop('checked', false);
                $('#chkFreeSet').prop('checked', false);
                $('#txtDiscountMultiple').val("");
                hideModal('loading');

                alert(response.message);
            } else if (response.status == 0) {
                notifyWarning(response.message);
            } else {
                notifyError(response.message);
            }
        } else if (xmlhttp.status == 500) {
            notifyError('Server Connection Error');
        }
    }
    xmlhttp.open("POST", "WebMethods/buyerWebMethod.aspx/updateMultipleItemDiscount", true);
    xmlhttp.responseType = "json"
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xmlhttp.send(JSON.stringify({ bodyObj: itemDiscountObjArr }));

}



function updateItemDiscount() {
    //itemDiscountObjArr = [];
    itemRow.DISCOUNT = $('#txtDiscount').val();
    itemRow.CLR_TYPE = $('#ddClrType').val();
    itemRow.CLR_TYPE_DESC = $('#ddClrType option:selected').text();
    // itemDiscountObjArr.push(itemRow);

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
                var item = response.item;
                itemRow.PROMO_MARK_UP = item.promo_mark_up;
                itemRow.PROMO_SRP = item.promo_srp;

                $('#lblPromoSRP').text(itemRow.PROMO_SRP);
                $('#lblPromoMarkUp').text(itemRow.PROMO_MARK_UP);

                tblItems.row(selectedRowElem).data(itemRow).draw();
                var headId = $('#hfHeadId').val();
                tblItems.destroy();
                negativeMUP = 0;
                getItemsTransaction(headId, 1);
                $('#dvRowEditorMain').css("display", "none");


            } else if (response.status == 0) {
                notifyWarning(response.message);
            } else {
                notifyError(response.message);
            }
        } else if (xmlhttp.status == 500) {
            notifyError('Server Connection Error');
        }
    }
    xmlhttp.open("POST", "WebMethods/buyerWebMethod.aspx/updateItemDiscount", true);
    xmlhttp.responseType = "json"
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xmlhttp.send('{bodyObj:' + JSON.stringify(itemRow) + '}');
    //   xmlhttp.send(JSON.stringify({ bodyObj: itemDiscountObjArr }));

}


// ADDED CREATEDROW FUNCTION TO LOOP EACH BARCODE AND CHECK CONFLICT OF EACH ITEM -- JAYSON SALINAS  V 2.0.0.7
function getItemsTransaction(head, rewardApplication) {
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
            if (response.status == 1) {
                var ScrollSize = "";
                if ($("#cbPromoTypes").val() == 1) {
                    ScrollSize = "483px";
                } else {
                    ScrollSize = "498px";
                }
                //this means that items to be loaded are buylist items
                if (rewardApplication == 1) {
                    tblItems = $('#tblItems').DataTable({
                        "scrollY": ScrollSize,
                        "paging": false,
                        "filter": false,
                        "info": false,
                        "data": response.item,
                        "destroy": true,
                        "order": [],
                        "createdRow": function (row, data, index) {
                            var bCode = data["BARCODE"];
                            checkOngoingTran(bCode, index, row, 1);
                        },
                        "columns": [
                         {
                             "targets": [0],
                             //                             "data": null,
                             //                             "orderable": false,
                             "width": "5px",
                             "defaultContent": '<input class=""  type="checkbox" />'
                         }
                         ,
                        { "data": "ORIN" },
                        { "data": "BARCODE" },
                        { "data": "VPN" },
                        { "data": "ITEM_DESC" },
                        {
                            "data": "UNIT_COST",
                            "width": "20px"
                        },
                        {
                            "data": "SRP",
                            "width": "25px"
                        },
                        {
                            "data": "AGE_CODE",
                            "width": "20px"
                        },
                        {
                            "data": "QTY",
                            "width": "20px"
                        },
                        {
                            "data": "REGULAR_MARK_UP",
                            "width": "30px",
                            "render": function (data, type, row, meta) {
                                
                                if (data != null) {
                                    if (row.SRP < row.UNIT_COST) {
                                        return "<span style='color:red'>" + data + "%</span>";
                                    } else {
                                        return data + "%";
                                    }                                 
                                } else {
                                    return data;
                                }
                               
                            }
                        },
                        { "data": "CLR_TYPE_DESC" },
                        {
                            "data": "DISCOUNT",
                            "width": "20px"
                        },
                        {
                            "data": "PROMO_MARK_UP",
                            "width": "30px",
                            "render": function (data, type, row) {
                                if (data != null) {
                                    if (data < 0) {
                                        negativeMUP = 1;
                                    }
                                    if (row.SRP < row.UNIT_COST) {
                                        return "<span style='color:red'>" + data + "%</span>";
                                    } else {
                                        return data + "%";
                                    }
                                } else {
                                    return data;
                                }
                            }
                        },
                        {
                            "data": "PROMO_SRP",
                            "width": "35px"
                        }
                        ,
                        {
                            "targets": [14],
                            "data": "BARCODE",
                            "width": "35px",
                            "render": function (data, type, row, meta) {
                                return '';
                            }
                        }
                        ,
                        {
                            "targets": [15],
                            "data": "BARCODE",
                            "width": "35px",
                            "render": function (data, type, row, meta) {
                                return '';
                            }
                        }

                        ]
                    });

                    if ($("#cbPromoTypes").val() != 1) {
                        //hide unused column
                        //if it's multibuy or threshold there are columns 
                        //that were unused and must be hidden
                        hideUnusedColumn();
                    }

                    if (tblHasItem("tblItems")) {
                        var total_items = $('#tblItems').find('tbody>tr').length;
                        $('#lblTotalItems').text(total_items);
                    }

                } else {

                    tblFreeListItems = $('#tblItemsFreeList').DataTable({
                        "scrollY": "315px",
                        "paging": false,
                        "info": false,
                        "data": response.item,
                        "createdRow": function (row, data, index) {
                            var bCode = data["BARCODE"];
                            checkOngoingTran(bCode, index, row, 0);
                        },
                        "columns": [{
                            "targets": [0],
                            //                             "data": null,
                            //                             "orderable": false,
                            "width": "5px",
                            "defaultContent": '<input class=""  type="checkbox" />'
                        }
                         ,
                                     { "data": "ORIN" },
                                     { "data": "BARCODE" },
                                     { "data": "VPN" },
                                     { "data": "ITEM_DESC" },
                                     {
                                         "data": "AGE_CODE",
                                         "width": "60px"
                                     },
                                     {
                                         "data": "QTY",
                                         "width": "40px"
                                     },
                                     { "data": "SRP" },
                        {
                            "targets": [7],
                            "data": "BARCODE",
                            "width": "35px",
                            "render": function (data, type, row, meta) {
                                return '';
                            }
                        }
                        ,
                        {
                            "targets": [8],
                            "data": "BARCODE",
                            "width": "35px",
                            "render": function (data, type, row, meta) {
                                return '';
                            }
                        }
                        ], drawCallback: function () {
                            initTblFreeItems();
                        }
                    });

                    if (tblHasItem("tblItemsFreeList")) {
                        var total_freeList_items = $('#tblItemsFreeList').find('tbody>tr').length;
                        $('#total_freeList_items').text(total_freeList_items);
                    }
                    $("#tblItemsFreeList_filter").css('padding-top', '5px');
                    $("#tblItemsFreeList_filter").css('padding-right', '10px');
                    $("#tblItemsFreeList_filter").css('font-weight', '600');
                }

            } else if (response.status == 0) {
                notifyWarning(response.message);
            } else {
                notifyError(response.message);
            }
        } else if (xmlhttp.status == 500) {
            notifyError('Server Connection Error');
        }

    }
    xmlhttp.open("POST", "WebMethods/BUYERWebMethod.aspx/getItemsTransaction", true);
    xmlhttp.responseType = "json"
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xmlhttp.send("{'headId':'" + head + "','rewardApplication':" + rewardApplication + "}");
}

function hideUnusedColumn() {
    //var column = tblItems.column(0);
    //column.visible(!column.visible());


    column = tblItems.column(9);
    column.visible(!column.visible());

    column = tblItems.column(10);
    column.visible(!column.visible());

    column = tblItems.column(11);
    column.visible(!column.visible());

    column = tblItems.column(12);
    column.visible(!column.visible());

    column = tblItems.column(13);
    column.visible(!column.visible());

    document.getElementById('chkSet').style.visibility = "hidden";
    document.getElementById('chkFreeSet').style.visibility = "hidden";
}



function tblItemsInitChecker(status) {
    //the role of this function is to check whether the
    //initTblItemsEvents() function must be initialized and called or not

    //*initTblItemsEvents() function must not be called if the status is not worksheet
    //this is to prevent the editing of items
    setTimeout(function () {
        if (tblItems == null) {
            tblItemsInitChecker(status);
        } else {
            if (status == 'WORKSHEET') {
                $("#chkSet").prop('disabled', false);
                initTblItemsEvents();
            } else {
                $("#tblItems tbody input[type='checkbox']").prop('disabled', 'disabled');
                $("#chkSet").prop('disabled', 'disabled');
            }
        }
    }, 400);
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

                transactionDetail = response.transactionDetail[0];
                $('#locName').val(transactionDetail.LOC + ' - ' + transactionDetail.LOC_NAME);
                $('#locId').val(transactionDetail.LOC);

                $('#deptName').val(transactionDetail.DEPT + ' - ' + transactionDetail.DEPT_NAME);
                $('#deptId').val(transactionDetail.DEPT);
                $('#lblTranNo').text(transactionDetail.TRAN_ID);
                $('#txtPromoDescription').val(transactionDetail.PROMOLIST_DESC);
                $('#cbPromoTypes').val(transactionDetail.PROMO_TYPE);
                if (transactionDetail.STATUS == 'WORKSHEET') {
                    $('#btnSubmit').prop('disabled', false);
                    //$('#btnPreview').prop('disabled', 'disabled');
                    $('#btnPreview').prop('disabled', false);
                } else if (transactionDetail.STATUS == 'SUBMITTED') {
                    $('#btnSubmit').prop('disabled', 'disabled');
                    $('#txtPromoDescription').prop('disabled', 'disabled');
                    if (negativeMUP == 1) {
                        $('#btnPreview').prop('disabled', false);
                    } else {
                        $('#btnPreview').prop('disabled', 'disabled');
                    }
                } else if (transactionDetail.STATUS == 'PRINTED') {
                    $('#btnSubmit').prop('disabled', 'disabled');
                    $('#txtPromoDescription').prop('disabled', 'disabled');
                    $('#btnPreview').prop('disabled', 'disabled');
                    $('#btnBack').prop('disabled', false);
                } else {
                    $('#btnSubmit').prop('disabled', 'disabled');
                    $('#btnPreview').prop('disabled', 'disabled');
                    $('#btnBack').prop('disabled', 'disabled');
                }


                //get buyer list
                if (tblItems != null) {
                    tblItems.destroy();
                    tblItems.draw();
                }
                getItemsTransaction(head, 1);

                //this is to check if the transaction is multibuy and has a freelist items
                if (transactionDetail.PROMO_TYPE == 0 && transactionDetail.HAS_FREELIST == 1) {
                    $('#chkFreeList').prop("checked", 'checked');
                    $('#divFreeList').show();
                    //get free list

                    if (tblFreeListItems != null) {
                        tblFreeListItems.destroy();
                    }
                    getItemsTransaction(head, 0);
                }

                getPromoDate(head, transactionDetail.STATUS);
                tblItemsInitChecker(transactionDetail.STATUS);


            } else if (response.status == 0) {
                notifyWarning(response.message);
            } else {
                notifyError(response.message);
            }
        }
    }
    xmlhttp.open("POST", "WebMethods/buyerWebMethod.aspx/getTransactionDetail", true);
    xmlhttp.responseType = "json"
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xmlhttp.send("{'headId':'" + head + "'}");
}

function tblHasItem(tableId) {
    var row = $('#' + tableId + '>tbody >tr');
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


// FUNCTION TO CHECK EACH ITEM IF THERE IS ONGOING TRANSACTION (PROMOTION OR MARKDOWN) - JAYSON SALINAS V 2.0.0.7
function checkOngoingTran(item, index, row, appReward) {
    var loc = $('#locId').val();
    var dept = $('#deptId').val();
    var tranNo = document.getElementById('lblTranNo').innerHTML;
    var trans = "";
    if (appReward == 1) {
        if ($("#cbPromoTypes").val() != 1) {
            var table = "tblItems";
            var indIndex = 9;
            var tranIndex = 10;
        } else {
            var table = "tblItems";
            var indIndex = 14;
            var tranIndex = 15;
        }
    } else {
        var table = "tblItemsFreeList";
        var indIndex = 8;
        var tranIndex = 9;
    }

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
            if (response.status == "1") {
                if (response.message == null) {
                    checkExistingTran(item, index, row, appReward);
                } else {
                    for (var x = 0; x < response.data.length; x++) {
                        trans = trans + " " + response.data[x].TRAN_ID;
                    }
                    $('td', row).css('background-color', '#0ed641');
                    document.getElementById(table).rows[index + 1].cells[tranIndex].innerHTML = "P - " + trans;
                    document.getElementById(table).rows[index + 1].cells[indIndex].innerHTML = "Y";
                    conflictCnt++;
                }
            } else if (response.status == "0") {
                for (var x = 0; x < response.data.length; x++) {
                    trans = trans + " " + response.data[x].TRAN_ID;
                }
                $('td', row).css('background-color', '#0ed641');
                document.getElementById(table).rows[index + 1].cells[tranIndex].innerHTML = "M - " + trans;
                document.getElementById(table).rows[index + 1].cells[indIndex].innerHTML = "Y";
                conflictCnt++;
            } else {
                document.getElementById(table).rows[index + 1].cells[indIndex].innerHTML = "N";
            }
        } else if (xmlhttp.status == 500) {
            notifyError('Server Connection Error');
            document.getElementById(txtDateElementId).value = "";
        }
    }
    xmlhttp.open("POST", "WebMethods/generalWebMethod.aspx/checkOngoingTran", true);
    xmlhttp.responseType = "json";
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xmlhttp.send("{'item':'" + item + "','location':" + loc + ",'department':" + dept + " ,'tranId':" + tranNo + "}");
}


// FUNCTION TO REMOVE ITEM BUYER - JAYSON SALINAS V 2.0.0.7
function removeItem(rewardapplication) {
    revealModal('loading');
    var isChecked = false;
    var itemRemove = [];
    var row;
    var rowData;
    var headId = $('#hfHeadId').val();
    if (rewardapplication == 1) {
        $("#tblItems tbody tr").each(function () {
            isChecked = $(this).find("input[type='checkbox']").prop('checked');
            if (isChecked) {
                row = tblItems.row(this);
                rowData = row.data();

                rowData.DISCOUNT = 0;
                rowData.CLR_TYPE = 0;
                rowData.CLR_TYPE_DESC = 0;

                itemRemove.push(rowData);
            }
        })//end of each   
    } else if (rewardapplication == 0) {
        $("#tblItemsFreeList tbody tr").each(function () {
            isChecked = $(this).find("input[type='checkbox']").prop('checked');
            if (isChecked) {
                row = tblFreeListItems.row(this);
                rowData = row.data();

                rowData.DISCOUNT = 0;
                rowData.CLR_TYPE = 0;
                rowData.CLR_TYPE_DESC = 0;

                itemRemove.push(rowData);
            }
        })//end of each   
    }

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

                if (rewardapplication == 1) {
                    tblItems.destroy();
                    getItemsTransaction(headId, 1);
                } else {
                    tblFreeListItems.destroy();
                    getItemsTransaction(headId, 0);
                }


                $('#chkSet').prop('checked', false);
                $('#chkFreeSet').prop('checked', false);
                hideModal('loading');

                alert(response.message);
            } else if (response.status == 0) {
                notifyWarning(response.message);
            } else {
                notifyError(response.message);
            }
        } else if (xmlhttp.status == 500) {
            notifyError('Server Connection Error');
        }
    }
    xmlhttp.open("POST", "WebMethods/buyerWebMethod.aspx/removeItem", true);
    xmlhttp.responseType = "json"
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xmlhttp.send(JSON.stringify({ bodyObj: itemRemove, 'headId': headId, 'rewardApplication': rewardapplication }));

}

function initTblFreeItems() {
    $("#chkFreeSet").change(function (e) {
        if (this.checked) {
            $('tbody tr td input[type="checkbox"]').prop('checked', this.checked);
            $('#btnRemoveFreeList').show();
        } else {
            $('tbody tr td input[type="checkbox"]').prop('checked', false);
            $('#btnRemoveFreeList').hide();
        }
    })

    $('#tblItemsFreeList tbody tr td:first-child').on('change', 'input[type="checkbox"]', function (e) {
        var totalCheckBoxFree = $(this).length;
        var totalCheckedFree = $('#tblItemsFreeList tbody tr td').find('input[type="checkbox"]:checked').length;
        var totalUncheckedFree = totalCheckBoxFree - totalCheckedFree;
        if (totalCheckedFree == totalCheckBoxFree) {
            $('#chkFreeSet').prop('checked', true);
            $('#btnRemoveFreeList').show();
        } else if (totalUncheckedFree == totalCheckBoxFree) {

            $('#chkFreeSet').prop('checked', false);
            $('#btnRemoveFreeList').hide();
        } else if (totalCheckedFree >= 1) {

            if (totalCheckBoxFree != totalCheckedFree) {
                $('#chkFreeSet').prop('checked', false);
            }
            $('#btnRemoveFreeList').show();
        } else {
            $('#btnRemoveFreeList').hide();
        }
    })

    //initializes all events used by "tblItemsFreeList"
    $("#tblItemsFreeList tbody").on('click', 'tr td:not(:first-child)', function () {

        var tr = $(this).closest('tr');
        if ($(tr).hasClass('selected')) {
            $(tr).removeClass('selected');
        } else {
            $("#tblItemsFreeList tr.selected").removeClass('selected');
            $(tr).addClass('selected');
            var row = tblFreeListItems.row(tr);
            itemRow = row.data();
            selectedRowElem = tr;
        }
    });
}


// FUNCTION TO CHECK EACH ITEM IF THERE IS EXISTING TRANSACTION (PROMOTION OR MARKDOWN) - JAYSON SALINAS V 2.0.0.10
function checkExistingTran(item, index, row, appReward) {
    var loc = $('#locId').val();
    var dept = $('#deptId').val();
    var tranNo = document.getElementById('lblTranNo').innerHTML;
    var trans = "";
    if (appReward == 1) {
        if ($("#cbPromoTypes").val() != 1) {
            var table = "tblItems";
            var indIndex = 9;
            var tranIndex = 10;
        } else {
            var table = "tblItems";
            var indIndex = 14;
            var tranIndex = 15;
        }
    } else {
        var table = "tblItemsFreeList";
        var indIndex = 8;
        var tranIndex = 9;
    }

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
            if (response.status == "1") {
                if (response.message == null) {
                    document.getElementById(table).rows[index + 1].cells[indIndex].innerHTML = "N";
                } else {
                    for (var x = 0; x < response.data.length; x++) {
                        trans = trans + " " + response.data[x].TRAN_ID;
                    }
                    $('td', row).css('background-color', '#33adff');
                    document.getElementById(table).rows[index + 1].cells[tranIndex].innerHTML = "P - " + trans;
                    document.getElementById(table).rows[index + 1].cells[indIndex].innerHTML = "Y"
                }
            } else if (response.status == "0") {
                for (var x = 0; x < response.data.length; x++) {
                    trans = trans + " " + response.data[x].TRAN_ID;
                }
                $('td', row).css('background-color', '#33adff');
                document.getElementById(table).rows[index + 1].cells[tranIndex].innerHTML = "M - " + trans;
                document.getElementById(table).rows[index + 1].cells[indIndex].innerHTML = "Y";
            } else {
                document.getElementById(table).rows[index + 1].cells[indIndex].innerHTML = "N";
            }
        } else if (xmlhttp.status == 500) {
            notifyError('Server Connection Error');
            document.getElementById(txtDateElementId).value = "";
        }
    }
    xmlhttp.open("POST", "WebMethods/generalWebMethod.aspx/checkExistingTran", true);
    xmlhttp.responseType = "json";
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xmlhttp.send("{'item':'" + item + "','location':" + loc + ",'department':" + dept + " ,'tranId':" + tranNo + "}");
}
