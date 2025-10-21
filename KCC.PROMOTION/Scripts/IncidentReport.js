var IR_HEAD = [];
function IncidentReport() {
    var data = [];
    this.populateDetails = function (data) {
        $('#tblIRdetail').DataTable({
            destroy: true,
            "scrollY": "50vh",
            "paging": false,
            "info": false,
            "filter": true,
            "dom": 'lrtip',
            "data": (data ? data: []),
            "columns": [
                { "data": "IR_DTL_ID" },
                { "data": "ORIN" },
                { "data": "BARCODE" },
                { "data": "VPN" },
                { "data": "ITEM_DESC" },
                { "data": "QTY" },
                { "data": "ACTUAL_QTY" }
            ]
        });
        var table = $('#tblIRdetail').DataTable();
        table.order([0, 'desc']).draw();
        $('#tblIRdetail tbody').off('click', 'tr');
        $('#tblIRdetail tbody').on('click', 'tr', function (e) {
            var table = $('#tblIRdetail').DataTable();
            var tr = $(this).closest('tr');
            var row = table.row(tr);
            var rowData = row.data();
            table.row({selected: true}).deselect();
            table.row(tr).select();
            cancelEdit();
        });
    }
}

    IncidentReport.prototype.getData = function (tranId) {
        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            url: 'WebMethods/IncidentReportWebMethod.aspx/GetItem',
            data: JSON.stringify({ tranId: tranId }),
            success: function (resp) {
                var resp = JSON.parse(resp.d);

                switch (resp.status) {
                    case 1:
                    case 0:
                        var ir = new IncidentReport();
                        ir.populateDetails(resp.irItems);
                        IR_HEAD = resp.irHead[0];
                        $('#spanLocation').text(IR_HEAD.LOC);
                        $('#spanTranId').text(IR_HEAD.TRAN_ID);
                        $('#spanDept').text(IR_HEAD.DEPT);
                        break;
                    case 2:
                        notifyError(resp.message);
                        break;

                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                notifyError('Error: ' + jqXHR.status + " - " + errorThrown);
            }
        });
    }
    IncidentReport.prototype.addItem = function (orin, tranId, actualQuantity) {
        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            url: 'WebMethods/IncidentReportWebMethod.aspx/AddItem',
            data: JSON.stringify({ tranId: tranId, orin: orin, actualQuantity: actualQuantity }),
            success: function (resp) {
                var resp = JSON.parse(resp.d);

                switch (resp.status) {
                    case 1: 
                        notifySuccess('Item successfully added');
                        var table = $('#tblIRdetail').DataTable();
                        table.row.add(resp.addedItem[0]).draw();
                        //var ir = new IncidentReport();
                        //ir.populateDetails(table.rows().data());
                        $('#txtItem').val('');
                        $('#txtQty').val('');
                        break;
                    case 0:
                        notifyWarning(resp.message);
                        break;
                    case 2:
                        notifyError('Error: ' + resp.message);
                        break;
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                notifyError('Error: ' + jqXHR.status + " - " + errorThrown);
            }
        });
}
    IncidentReport.prototype.removeItem = function (irDetailId) {
        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            url: 'WebMethods/IncidentReportWebMethod.aspx/RemoveITem',
            data: JSON.stringify({ irDetailId: irDetailId }),
            success: function (resp) {
                var resp = JSON.parse(resp.d);

                if (resp.status = 1) {
                    var table = $('#tblIRdetail').DataTable();
                    table.rows({ selected: true }).remove().draw();
                    notifySuccess('Message: ' + resp.message);
                }    
                else {
                    notifyError('Error: ' + resp.message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                notifyError('Error: ' + jqXHR.status + " - " + errorThrown);
            }
        });
    }
    IncidentReport.prototype.modifyItem = function (irDetailId, actualQty) {
        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            url: 'WebMethods/IncidentReportWebMethod.aspx/ModifyItem',
            data: JSON.stringify({ irDetailId: irDetailId, actualQty: actualQty }),
            success: function (resp) {
                var resp = JSON.parse(resp.d);

                if (resp.status == 1) {
                    var table = $('#tblIRdetail').DataTable();
                    var aq = resp.data[0].ACTUAL_QTY;
                    var table = $('#tblIRdetail').DataTable();
                    var rowToEdit = table.row({ selected: true }).data();
                    rowToEdit.ACTUAL_QTY = aq;
                    table.row({ selected: true }).data(rowToEdit).invalidate();
                    cancelEdit();
                    notifySuccess(resp.message);
                } else if (resp.status == 0) {
                    notifyWarning(resp.message);
                }
                else {
                    notifyError('Error: ' + resp.message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                notifyError('Error: ' + jqXHR.status + " - " + errorThrown);
            }
        });
    }

    $(document).ready(function () {
    $('#txtItem').bind('keyup change  paste', function () {
        this.value = this.value.replace(/[^0-9]/g, '');
    });
    $('#txtQty').bind('keyup change  paste', function () {
        this.value = this.value.replace(/[^0-9]/g, '');
    });
    $('#btnAddIrItem').click(function () {
        var orin = $('#txtItem').val();
        var qty = $('#txtQty').val();
        var tranId = IR_HEAD.TRAN_ID;
        
        if (!orin || !qty ) {
            notifyInfo('Please complete your input');
            return;
        }
        var ir = new IncidentReport();
        ir.addItem(orin, tranId, qty);
    });
    $('#btnDeleteIrItem').click(function () {
        var table = $('#tblIRdetail').DataTable();
        var rowToDelete = table.rows({ selected: true }).data();
        var irItemId = rowToDelete[0].IR_DTL_ID;
        if (irItemId) {
            var isYes = confirm('Are sure you want to delete this item?');
            if (isYes) {
                var ir = new IncidentReport();
                ir.removeItem(irItemId);
            }
        }
    });

    $('#btnIrOk').click(function () {
        var table = $('#tblIRdetail').DataTable();
        var data = table.rows().data();
        if (data.length) {
            hideModal('modalIRdetails');
            revealModal('popupComposEmail');
        } else {
            notifyInfo('No item added on the Incident Report');
        }
    });

    $('#btnEditIrItem').click(function () {
        var table = $('#tblIRdetail').DataTable();
        var rowToEdit = table.rows({ selected: true }).data();
        var irItemId = rowToEdit[0].IR_DTL_ID;
        if (irItemId) {
            $('#IRdetailContainer input.button-primary').css('display', 'none');
            $('#btnModifyIrItemQty').css('display', 'inherit');
            $('#btnCancelIrItemQty').css('display', 'inherit');
            $('#txtItem').prop('disabled', true);
            $('#txtQty').val(rowToEdit[0].ACTUAL_QTY);
            $('#txtItem').val(rowToEdit[0].ORIN);
        }
    });

    $('#btnCancelIrItemQty').click(function () {
        cancelEdit();
    });
    $('#btnModifyIrItemQty').click(function () {
        var table = $('#tblIRdetail').DataTable();
        var rowToEdit = table.rows({ selected: true }).data();
        var actualQty =  $('#txtQty').val();
        if (!actualQty) {
            notifyWarning('Please input Actual Quantity');
            return;
        }
        if (parseInt(actualQty) == rowToEdit[0].ACTUAL_QTY) {
            notifyWarning('You entered the same Actual Quantity with the current Actual Quantity');
            return;
        }
        if (rowToEdit) {
            var ir = new IncidentReport();
            ir.modifyItem(rowToEdit[0].IR_DTL_ID, parseInt(actualQty));
        }
    });
    });

    function cancelEdit() {
        $('#IRdetailContainer input.button-primary').css('display', 'inherit');
        $('#btnModifyIrItemQty').css('display', 'none');
        $('#btnCancelIrItemQty').css('display', 'none');
        $('#txtItem').prop('disabled', false);
        $('#txtQty').val('');
        $('#txtItem').val('');
    }