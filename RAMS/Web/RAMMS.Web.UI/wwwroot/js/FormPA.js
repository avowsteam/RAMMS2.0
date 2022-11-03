
$(document).ready(function () {

    DisableHeader();
    CalculateValues();
});


function DisableHeader() {

    if ($("#FormPAHeader_PkRefNo").val() != "0") {
        $("#headerDiv * > select").attr('disabled', true).trigger("chosen:updated");

        $("#btnFindDetails").hide();
    }

    if ($("#hdnView").val() == 1) {
        $("#saveFormPABtn").hide();
        $("#SubmitFormPABtn").hide();
        
    }


}

function OnSPChange(tis) {

    var ctrl = $(tis);
    if (ctrl.val() != null)
        $('#ddlSP').val(ctrl.val());
    if ($('#ddlSP').val() != "") {
        $("#FormPAHeader_UsernameSp").val(ctrl.find("option:selected").attr("Item1"));
        $("#FormPAHeader_DesignationSp").val(ctrl.find("option:selected").attr("Item2"));

        if ($('#ddlSP').val() == "99999999") {
            $("#FormPAHeader_UsernameSp").removeAttr("readonly");
            $("#FormPAHeader_DesignationSp").removeAttr("readonly");

        } else {
            $("#FormPAHeader_UsernameSp").attr("readonly", "true");
            $("#FormPAHeader_DesignationSp").attr("readonly", "true");
        }
        $('#FormPAHeader_SignProsd').prop('checked', true);
    }
    else {
        $("#FormPAHeader_UsernameSp").val('');
        $("#FormPAHeader_DesignationSp").val('');
        $('#FormPAHeader_SignProsdsp').prop('checked', false);
    }
}

function OnECChange(tis) {

    var ctrl = $(tis);
    if (ctrl.val() != null)
        $('#ddlEC').val(ctrl.val());
    if ($('#ddlEC').val() != "") {
        $("#FormPAHeader_UsernameEc").val(ctrl.find("option:selected").attr("Item1"));
        $("#FormPAHeader_DesignationEc").val(ctrl.find("option:selected").attr("Item2"));

        if ($('#ddlEC').val() == "99999999") {
            $("#FormPAHeader_UsernameEc").removeAttr("readonly");
            $("#FormPAHeader_DesignationEc").removeAttr("readonly");

        } else {
            $("#FormPAHeader_UsernameEc").attr("readonly", "true");
            $("#FormPAHeader_DesignationEc").attr("readonly", "true");
        }
        $('#FormPAHeader_SignProsd').prop('checked', true);
    }
    else {
        $("#FormPAHeader_UsernameEc").val('');
        $("#FormPAHeader_DesignationEc").val('');
        $('#FormPAHeader_SignProsdEc').prop('checked', false);
    }
}

function OnSOChange(tis) {

    var ctrl = $(tis);
    if (ctrl.val() != null)
        $('#ddlSO').val(ctrl.val());
    if ($('#ddlSO').val() != "") {
        $("#FormPAHeader_UsernameSo").val(ctrl.find("option:selected").attr("Item1"));
        $("#FormPAHeader_DesignationSo").val(ctrl.find("option:selected").attr("Item2"));

        if ($('#ddlSO').val() == "99999999") {
            $("#FormPAHeader_UsernameSo").removeAttr("readonly");
            $("#FormPAHeader_DesignationSo").removeAttr("readonly");

        } else {
            $("#FormPAHeader_UsernameSo").attr("readonly", "true");
            $("#FormPAHeader_DesignationSo").attr("readonly", "true");
        }
        $('#FormPAHeader_SignProsd').prop('checked', true);
    }
    else {
        $("#FormPAHeader_UsernameSo").val('');
        $("#FormPAHeader_DesignationSo").val('');
        $('#FormPAHeader_SignProsdSo').prop('checked', false);
    }
}



function Save(SubmitType) {


    if (SubmitType == "Submitted") {
        $("#FormPAHeader_SubmitSts").val(true);

    }

    InitAjaxLoading();

    if ($("#FormPAHeader_Status").val() == "")
        $("#FormPAHeader_Status").val("Initialize");
    else if ($("#FormPAHeader_Status").val() == "Initialize")
        $("#FormPAHeader_Status").val("Saved");

    var FormPA = new Object();
    FormPA.PkRefNo = $("#FormPAHeader_PkRefNo").val()
    FormPA.RefId = $("#FormPAHeader_RefId").val()
    FormPA.SubmissionMonth = $("#ddlMonth").val()
    FormPA.SubmissionYear = $("#ddlYear").val()
    FormPA.WorkValueDeduction = $('.tottodateded').text().trim();
    FormPA.WorkValueAddition = $('.tottodateAdd').text().trim();
    FormPA.TotalPayment = $('.totAmtGrand').text().trim();
    FormPA.UseridSp = $('#ddlSP').val();
    FormPA.UsernameSp = $('#FormPAHeader_UsernameSp').val();
    FormPA.DesignationSp = $('#FormPAHeader_DesignationSp').val();
    FormPA.SignDateSp = $('#FormPAHeader_SignDateSp').val();
    FormPA.SignSp = $('#FormPAHeader_SignSp').val();
    FormPA.UseridEc = $('#ddlEC').val();
    FormPA.UsernameEc = $('#FormPAHeader_UsernameEc').val();
    FormPA.DesignationEc = $('#FormPAHeader_DesignationEc').val();
    FormPA.SignDateEc = $('#FormPAHeader_SignDateEc').val();
    FormPA.SignEc = $('#FormPAHeader_SignEc').val();
    FormPA.UseridSo = $('#ddlSO').val();
    FormPA.UsernameSo = $('#FormPAHeader_UsernameSo').val();
    FormPA.DesignationSo = $('#FormPAHeader_DesignationSo').val();
    FormPA.SignDateSo = $('#FormPAHeader_SignDateSo').val();
    FormPA.SignSo = $('#FormPAHeader_SignSo').val();

    FormPA.Status = $('#FormPAHeader_Status').val();
    FormPA.SubmitSts = $('#FormPAHeader_SubmitSts').val();

    var PACRR = []

    $('#tblCRR > tbody  > tr').each(function (index, tr) {

        var PA = new Object();
        PA.PcmamwPkRefNo = $("#FormPAHeader_PkRefNo").val();
        PA.Division = $(this).find("td:nth-child(1)").text().trim();
        PA.Paved = $(this).find("td:nth-child(2)").find("input").val();
        PA.Unpaved = $(this).find("td:nth-child(3)").find("input").val();
        PA.SubTotal = $(this).find("td:nth-child(4)").find("input").val();
        PA.ContractRate = $(this).find("td:nth-child(5)").find("input").val();
        PA.TotalAmount = $(this).find("td:nth-child(6)").find("input").val();
        PACRR.push(PA);

    });

    FormPA.RmPaymentCertificateCrr = PACRR;

    var PACRRD = []

    $('#tblCRRD > tbody  > tr').each(function (index, tr) {

        var PA = new Object();
        PA.PcmamwPkRefNo = $("#FormPAHeader_PkRefNo").val();
        PA.Description = $(this).find("td:nth-child(1)").text().trim();
        PA.ThisPayment = $(this).find("td:nth-child(2)").find("input").val();
        PA.TillLastPayment = $(this).find("td:nth-child(3)").find("input").val();
        PA.TotalToDate = $(this).find("td:nth-child(4)").find("input").val();
        PACRRD.push(PA);

    });

    FormPA.RmPaymentCertificateCrrd = PACRRD;

    var PACRRA = []

    $('#tblCRRA > tbody  > tr').each(function (index, tr) {

        var PA = new Object();
        PA.PcmamwPkRefNo = $("#FormPAHeader_PkRefNo").val();
        PA.Description = $(this).find("td:nth-child(1)").text().trim();
        PA.ThisPayment = $(this).find("td:nth-child(2)").find("input").val();
        PA.TillLastPayment = $(this).find("td:nth-child(3)").find("input").val();
        PA.TotalToDate = $(this).find("td:nth-child(4)").find("input").val();
        PACRRA.push(PA);

    });

    FormPA.RmPaymentCertificateCrra = PACRRA;


    $.ajax({
        url: '/FormPA/UpdateFormPA',
        data: FormPA,
        type: 'POST',
        success: function (data) {
            HideAjaxLoading();
            if (data == -1) {
                app.ShowErrorMessage(data.errorMessage);
            }
            else {

                app.ShowSuccessMessage('Saved Successfully', false);
                location.href = "/FormPA";
            }
        }
    });



}


function FindDetails() {


    if (ValidatePage('#headerDiv')) {


        if ($("#FormPAHeader_Status").val() == "")
            $("#FormPAHeader_Status").val("Initialize");
        else if ($("#FormPAHeader_Status").val() == "Initialize")
            $("#FormPAHeader_Status").val("Saved");

        InitAjaxLoading();
        var FormPA = new Object();
        FormPA.PkRefNo = $("#FormPAHeader_PkRefNo").val()
        FormPA.SubmissionMonth = $("#ddlMonth").val()
        FormPA.SubmissionYear = $("#ddlYear").val()
        FormPA.WorkValueDeduction = $('.tottodateded').text().trim();
        FormPA.WorkValueAddition = $('.tottodateAdd').text().trim();
        FormPA.TotalPayment = $('.totAmtGrand').text().trim();
        FormPA.UseridSp = $('#ddlSp').val();
        FormPA.UsernameSp = $('#FormPAHeader_UsernameSp').val();
        FormPA.DesignationSp = $('#FormPAHeader_DesignationSp').val();
        FormPA.SignSp = $('#FormPAHeader_SignSp').val();
        FormPA.SignDateSp = $('#FormPAHeader_SignDateSp').val();
        FormPA.Status = $('#FormPAHeader_Status').val();

        $.ajax({
            url: '/FormPA/SaveFormPA',
            data: FormPA,
            type: 'POST',
            success: function (data) {
                HideAjaxLoading();
                if (data == -1) {
                    app.ShowErrorMessage(data.errorMessage);
                }
                else {
                    app.ShowSuccessMessage('Saved Successfully', false);
                    location.href = "/FormPA/Add?Id=" + data + "&view=0";
                }
            }
        });

    }
}


function GoBack() {
    if ($("#hdnView").val() == "0") {
        if (app.Confirm("Unsaved changes will be lost. Are you sure you want to cancel?", function (e) {
            if (e) {
                location.href = "/FormPA";

            }
        }));
    }
    else
        location.href = "/FormPA";
}



function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [year, month, day].join('-');
}

function CalculateValues() {
    ContractRoadCal();
    DeductionCal();
    AdditionCal()
}


function ContractRoadCal() {

    var totPaved = 0;
    var totUnPaved = 0;
    var totSubTotal = 0;
    var totTotalAmount = 0;

    $('#tblCRR > tbody  > tr').each(function (index, tr) {
        var Paved = 0;
        var Unpaved = 0;

        if ($(this).find("td:nth-child(2)").find("input").val() != "") {
            Paved = parseFloat($(this).find("td:nth-child(2)").find("input").val());
        }
        if ($(this).find("td:nth-child(3)").find("input").val() != "") {
            Unpaved = parseFloat($(this).find("td:nth-child(3)").find("input").val());
        }

        var SubTotal = (Paved + Unpaved).toFixed(2);

        $(this).find("td:nth-child(4)").find("input").val(SubTotal);

        var ContractRate = 0;
        if ($(this).find("td:nth-child(5)").find("input").val() != "") {
            ContractRate = $(this).find("td:nth-child(5)").find("input").val();
        }

        var TotalAmount = (SubTotal * ContractRate).toFixed(2);

        $(this).find("td:nth-child(6)").find("input").val(TotalAmount);

        totPaved = (parseFloat(totPaved) + parseFloat(Paved)).toFixed(2);
        totUnPaved = (parseFloat(totUnPaved) + parseFloat(Unpaved)).toFixed(2);
        totSubTotal = (parseFloat(totSubTotal) + parseFloat(SubTotal)).toFixed(2);
        totTotalAmount = (parseFloat(totTotalAmount) + parseFloat(TotalAmount)).toFixed(2);

    });

    $(".totPaved").text(totPaved);
    $(".totunPaved").text(totUnPaved);
    $(".totsubtotal").text(totSubTotal);
    $(".totAmtGrand").text(totTotalAmount);
}

function DeductionCal() {

    var totThisPay = 0;
    var totTillPay = 0;
    var totTottoDate = 0;

    $('#tblCRRD > tbody  > tr').each(function (index, tr) {
        var ThisPay = 0;
        var TillPay = 0;

        if ($(this).find("td:nth-child(2)").find("input").val() != "") {
            ThisPay = parseFloat($(this).find("td:nth-child(2)").find("input").val());
        }
        if ($(this).find("td:nth-child(3)").find("input").val() != "") {
            TillPay = parseFloat($(this).find("td:nth-child(3)").find("input").val());
        }

        var TottoDate = (parseFloat(ThisPay) + parseFloat(TillPay)).toFixed(2);

        $(this).find("td:nth-child(4)").find("input").val(TottoDate);

        totThisPay = (parseFloat(totThisPay) + parseFloat(ThisPay)).toFixed(2);
        totTillPay = (parseFloat(totTillPay) + parseFloat(TillPay)).toFixed(2);
        totTottoDate = (parseFloat(totTottoDate) + parseFloat(TottoDate)).toFixed(2);


    });

    $(".totThisPayAdd").text(totThisPay);
    $(".totTillPayAdd").text(totTillPay);
    $(".tottodateAdd").text(totTottoDate);

}


function AdditionCal() {

    var totThisPay = 0;
    var totTillPay = 0;
    var totTottoDate = 0;

    $('#tblCRRA > tbody  > tr').each(function (index, tr) {
        var ThisPay = 0;
        var TillPay = 0;

        if ($(this).find("td:nth-child(2)").find("input").val() != "") {
            ThisPay = parseFloat($(this).find("td:nth-child(2)").find("input").val());
        }
        if ($(this).find("td:nth-child(3)").find("input").val() != "") {
            TillPay = parseFloat($(this).find("td:nth-child(3)").find("input").val());
        }

        var TottoDate = (parseFloat(ThisPay) + parseFloat(TillPay)).toFixed(2);

        $(this).find("td:nth-child(4)").find("input").val(TottoDate);

        totThisPay = (parseFloat(totThisPay) + parseFloat(ThisPay)).toFixed(2);
        totTillPay = (parseFloat(totTillPay) + parseFloat(TillPay)).toFixed(2);
        totTottoDate = (parseFloat(totTottoDate) + parseFloat(TottoDate)).toFixed(2);


    });

    $(".totThisPayded").text(totThisPay);
    $(".totTillPayded").text(totTillPay);
    $(".tottodateded").text(totTottoDate);

}