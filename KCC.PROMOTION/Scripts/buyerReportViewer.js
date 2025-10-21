$(function () {
    document.getElementById("CrystalReportViewer1_toptoolbar_export").style.display = "none"
    document.getElementById("CrystalReportViewer1_toptoolbar_print").style.display = "none"
    $(".leftPanel").css("display", "none");
    var colPrint = document.getElementById("_btnGrp_palette").getElementsByTagName("table")[0].firstChild.firstChild.insertCell(0);
    //colPrint.innerHTML = "<input type='button' id='btnExport' disabled='disabled'  value='Export' onclick='exportDoc();' >";
    colPrint.innerHTML = "<input type='button' id='btnExport' value='Export' onclick='exportDoc();' >";
    getTransactionDetail();

    $("#lnkTransactionDetail").on("click", function () {
        window.location.href = "buyerDetails.aspx?headId=" + $('#hfHeadId').val();
    })
});

function getTransactionDetail() {
        var headId = $('#hfHeadId').val();
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
                var detailsObj = response.transactionDetail[0];
                if (response.status == 1) {
                    if (detailsObj.STATUS == 'SUBMITTED') {
                        $("#btnExport").prop("disabled", "");
                    } else if (detailsObj.STATUS == 'WORKSHEET') {
                        $("#btnExport").prop("disabled", "");
                    } else {
                        $("#btnExport").prop("disabled", "disabled");
                        window.location.href = "BuyerDashboard.aspx";
                    }
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
        xmlhttp.send("{'headId':'" + headId + "'}");

    }





function exportDoc() { 

    if(confirm("Do you want to Export This Transaction?")){
        $("#printme").click();
        //$("#btnExport").prop("disabled", "disabled");
        $("#btnExport").prop("disabled", false);
    }

}

