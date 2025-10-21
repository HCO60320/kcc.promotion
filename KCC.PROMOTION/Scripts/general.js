(function ($) {
    $.fn.onEnter = function (func) {
        this.bind('keypress', function (e) {
            if (e.keyCode == 13) func.apply(this, [e]);
        });
        return this;
    }
})(jQuery);

//reveals modalpopup
function revealModal(divID) {
    window.onscroll = function () { document.getElementById(divID).style.top = document.body.scrollTop; };
    document.getElementById(divID).style.display = "block";
    document.getElementById(divID).style.top = document.body.scrollTop;
};

//hides modalpopup
function hideModal(divID) {
    document.getElementById(divID).style.display = "none";
};

function character_counter(textElement,targetId,maxCount) {
    var tmp_desc = $(textElement).val();
    if (parseInt(tmp_desc.length) <= parseInt(maxCount)) {
        $("#" + targetId).html("(" + tmp_desc.length + "/" + maxCount + ")");
    };
};

function notifyWarning(msg,autohide) {
    if (autohide == undefined) {
        //default value for autohide is false;
        notif({
            msg: msg,
            type: "warning",
            autohide: false,
            multiline: true,
            position: "center"
        });
    } else {
        notif({
            msg: msg,
            type: "warning",
            autohide: autohide,
            multiline: true,
            position: "center"
        });
    }

    
}


function notifyError(msg){
    notif({
        msg: msg,
        type: "error",
        autohide: false,
        multiline:true,
        position: "center"
    });
}

function notifyInfo(msg) {
    notif({
        msg: msg,
        type: "info",
        autohide: true,
        multiline: true,
        position: "center"
    });
}

function notifyOnInput(msg, type, elementId) {
    notif({
        msg: msg,
        type: type,
        position: "center"
    });
    $(elementId).focus();
}

function notifySuccess(msg) {
    notif({
        msg: msg,
        type: "success",
        autohide: true,
        multiline: true,
        position: "center"
    });
}

function locselected(source, e) {
    var node;
    var value = e.get_value();

    if (value) node = e.get_item();
    else {
        value = e.get_item().parentNode._value;
        node = e.get_item().parentNode;
    }

    var text = (node.innerText) ? node.innerText : (node.textContent) ? node.textContent : node.innerHtml;
    source.get_element().value = text;
    var split = value.split(';');
    document.getElementById('locId').value = split[0];
    var tmp = $('#locName').val();
    $('#locName').val(split[0] + ' - ' + tmp);
    if ($('#locId').val() != "") {
        $('#deptName').val('');
        $('#deptName').prop('disabled', false);
        $('#deptName').focus();
    } else {
        $('#deptName').val('');
        $('#deptName').prop('disabled', true);
        $('#locName').focus();
    }    
};

function depselected(source, e) {
    var node;
    var value = e.get_value();
    if (value) node = e.get_item();
    else {
        value = e.get_item().parentNode._value;
        node = e.get_item().parentNode;
    }

    var text = (node.innerText) ? node.innerText : (node.textContent) ? node.textContent : node.innerHtml;
    source.get_element().value = text;
    var split = value.split(';');
    document.getElementById('deptId').value = split[0];
    var tmp = $('#deptName').val();
    $('#deptName').val(split[0] + ' - ' + tmp);
};

var sessionTimeoutCntr;

function keepSessionAlive(timeout) {
    if (timeout != undefined) {
        sessionTimeoutCntr = (1000 * (timeout -1)) * 60;
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
            setInterval(keepSessionAlive, sessionTimeoutCntr);
        }
    }
    xmlhttp.open("POST", "WebMethods/generalWebMethod.aspx/keepSessionAlive", true);
    xmlhttp.responseType = "json"
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xmlhttp.send();
}


function isEmpty(value) {
    if (value == null || value.toLowerCase() == 'null' || value == '') {
        return true;
    } else {
        return false;
    }
}



//------------------------------------------------------------------------------Date Class------------------------------------------------------------------////
function date_class() {
    this.dates = { getdifference: getdifference,
        getLastDayOfMonth: getLastDayOfMonth,
        isBetween: isBetween
    };

    function addYear(date, NumberOfMonths) {
        var newdate = new Date(date)
        newdate.setMonth(newdate.getMonth() + NumberOfMonths);
        return newdate;
    }


    //to get the difference between to Dates
    //date1 is < date2
    this.setToFirstDayOfMonth = function (date, format) {
        if (arguments.length == 1) {
            return setToFirstDayOfMonthonlydate(date);
        } else if (arguments.length == 2) {
            return setToFirstDayOfMonthwithformat(date, format)
        }
    };

    this.setToLastDayOfMonth = function (date, format) {
        if (arguments.length == 1) {
            return setToLastDayOfMonthonlydate(date);
        } else if (arguments.length == 2) {
            return setToLastDayOfMonthwithformat(date, format);
        }
    };

    //==========================-------------FUNCTIONS FOR DATE CLASS----------------=======================

    function setToLastDayOfMonthonlydate(date) {
        var d = new Date(date);
        var month = d.getMonth();
        var yr = d.getFullYear();
        return month + 1 + "/" + getLastDayOfMonth(yr, month) + "/" + yr;
    }

    function setToLastDayOfMonthwithformat(date, format) {
        var d = new Date(date);
        var month = d.getMonth();
        var yr = d.getFullYear();
        var newdate = new Date(month + 1 + "/" + getLastDayOfMonth(yr, month) + "/" + yr);
        return newdate.format("" + format);
    }


    function setToFirstDayOfMonthonlydate(date) {
        var d = new Date(date);
        var month = d.getMonth();
        var yr = d.getFullYear();
        return month + 1 + "/" + "01/" + yr;
    }

    function setToFirstDayOfMonthwithformat(date, format) {
        var d = new Date(date);
        var month = d.getMonth();
        var yr = d.getFullYear();
        var newdate = new Date(month + 1 + "/" + "01/" + yr);
        return newdate.format("" + format);
    }



    function getdifference(date1, date2) {
        var date1_temp = new Date(date1);
        var date2_temp = new Date(date2);
        return (date2_temp.getTime() - date1_temp.getTime()) / (24 * 3600 * 1000);
    } //end of getdifference function

    //to get the last day of the specified month
    function getLastDayOfMonth(Year, Month) {
        return (new Date((new Date(Year, Month + 1, 1)) - 1)).getDate();
    } //end of getLastDayOfMonth function


    function isBetween(ToBeComparedDate, fromDate, toDate) {
        var result_from = getdifference(fromDate, ToBeComparedDate);
        var result_to = getdifference(toDate, ToBeComparedDate);

        if (result_from >= 0 && result_to <= 0) {
            return true;
        } else {
            return false;
        }
    } //end of isBetween function

    //==========================-------------END OF FUNCTIONS FOR DATE CLASS----------------=======================



} //end of date_class



