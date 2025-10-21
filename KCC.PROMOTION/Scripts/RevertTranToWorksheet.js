function Transaction() {
    var tranNo = parseInt($('#txtRevertToWorksheet').val());
    Object.defineProperty(this, 'tranNo', {
        get: function () {
            return tranNo;
        }
    });
    this.revertToWorksheet = function () {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: 'WebMethods/adminWebMethod.aspx/RevertToWorksheet',
            data: JSON.stringify({ tranNo: tranNo }),
            success: function (resp) {
                var resp = JSON.parse(resp.d);

                if (resp.status == 1) {
                    notifyInfo(tranNo + ' - ' + resp.message);
                    hideModal("divRevertToWorksheet");
                    $('#txtRevertToWorksheet').val('');
                }
                else if (resp.status == 0) {
                    notifyWarning(tranNo + ' - ' + resp.message);
                } else {
                    notifyError(resp.message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                notifyError('Error: ' + jqXHR.status + " - " + errorThrown);
            }
        });
    }
}

$(document).ready(function () {
    $("#btnRevertToWorksheet").on("click", function () {
        revealModal("divRevertToWorksheet");
    });
    $("#btnCancelRevertToWorksheet").on("click", function () {
        hideModal("divRevertToWorksheet");
    })

    $('#btnOKRevertToWorksheet').click(function () {
        var tran = new Transaction();
        if (!tran.tranNo) {
            notifyWarning('Please input transaction number');
            return;
        }
        var isConfirm = confirm('Are sure you want to Revert the transaction (' + tran.tranNo + ') to Worksheet');
        if (isConfirm) {
            tran.revertToWorksheet();
        }
    });

    $('#txtRevertToWorksheet').bind('keyup change  paste', function () {
        this.value = this.value.replace(/[^0-9]/g, '');
    });
    $('#txtRevertToWorksheet').on('keypress',function (e) {
        if (e.keyCode == 13 && e.key == 'Enter') {
            var tran = new Transaction();
            if (!tran.tranNo) {
                notifyWarning('Please input transaction number');
                return;
            }
            var isConfirm = confirm('Are sure you want to Revert the transaction (' + tran.tranNo + ') to Worksheet');
            if (isConfirm) {
                tran.revertToWorksheet();
            }
        }
    });
});