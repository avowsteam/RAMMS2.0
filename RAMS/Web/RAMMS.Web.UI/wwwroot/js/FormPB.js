
$(document).ready(function () {

    $('#tblDetails > tbody  > tr').each(function (i, tr) {
        if (i < jsonObj.length) {
            $(this).find("td:nth-child(1)").find("input").val(jsonObj[i].IwRef)
            $(this).find("td:nth-child(2)").find("input").val(jsonObj[i].ProjectTitle)

            if (jsonObj[i].CompletionDate != null) {
                var indexvalue = (jsonObj[i].CompletionDate).indexOf("T");
                var d = (jsonObj[i].CompletionDate).substring(0, indexvalue);
                $(this).find("td:nth-child(3)").find("input").val(d);
            }

            $(this).find("td:nth-child(4)").find("input").val(jsonObj[i].CompletionRefNo)
            $(this).find("td:nth-child(5)").find("input").val(jsonObj[i].AmountBeforeLad == null ? jsonObj[i].AmountBeforeLad: (jsonObj[i].AmountBeforeLad).toFixed(2))
            $(this).find("td:nth-child(6)").find("input").val(jsonObj[i].LaDamage == null ? jsonObj[i].LaDamage : (jsonObj[i].LaDamage).toFixed(2))
            $(this).find("td:nth-child(7)").find("input").val(jsonObj[i].FinalPayment == null ? jsonObj[i].FinalPayment : (jsonObj[i].FinalPayment).toFixed(2))
        }
    });


    DisableHeader();
    CalculateValues();
});


function DisableHeader() {

    if ($("#FormPBHeader_PkRefNo").val() != "0") {
        $("#headerDiv * > select").attr('disabled', true).trigger("chosen:updated");

        $("#btnFindDetails").hide();
    }

    //if ($("#hdnView").val() == 1) {
    //    $("#saveFormPBBtn").hide();
    //    $("#SubmitFormPBBtn").hide();

    //}


}

function OnSPChange(tis) {

    var ctrl = $(tis);
    if (ctrl.val() != null)
        $('#ddlSP').val(ctrl.val());
    if ($('#ddlSP').val() != "") {
        $("#FormPBHeader_UsernameSp").val(ctrl.find("option:selected").attr("Item1"));
        $("#FormPBHeader_DesignationSp").val(ctrl.find("option:selected").attr("Item2"));

        if ($('#ddlSP').val() == "99999999") {
            $("#FormPBHeader_UsernameSp").removeAttr("readonly");
            $("#FormPBHeader_DesignationSp").removeAttr("readonly");

        } else {
            $("#FormPBHeader_UsernameSp").attr("readonly", "true");
            $("#FormPBHeader_DesignationSp").attr("readonly", "true");
        }
        $('#FormPBHeader_SignProsd').prop('checked', true);
    }
    else {
        $("#FormPBHeader_UsernameSp").val('');
        $("#FormPBHeader_DesignationSp").val('');
        $('#FormPBHeader_SignProsdsp').prop('checked', false);
    }
}

function OnECChange(tis) {

    var ctrl = $(tis);
    if (ctrl.val() != null)
        $('#ddlEC').val(ctrl.val());
    if ($('#ddlEC').val() != "") {
        $("#FormPBHeader_UsernameEc").val(ctrl.find("option:selected").attr("Item1"));
        $("#FormPBHeader_DesignationEc").val(ctrl.find("option:selected").attr("Item2"));

        if ($('#ddlEC').val() == "99999999") {
            $("#FormPBHeader_UsernameEc").removeAttr("readonly");
            $("#FormPBHeader_DesignationEc").removeAttr("readonly");

        } else {
            $("#FormPBHeader_UsernameEc").attr("readonly", "true");
            $("#FormPBHeader_DesignationEc").attr("readonly", "true");
        }
        $('#FormPBHeader_SignProsd').prop('checked', true);
    }
    else {
        $("#FormPBHeader_UsernameEc").val('');
        $("#FormPBHeader_DesignationEc").val('');
        $('#FormPBHeader_SignProsdEc').prop('checked', false);
    }
}

function OnSOChange(tis) {

    var ctrl = $(tis);
    if (ctrl.val() != null)
        $('#ddlSO').val(ctrl.val());
    if ($('#ddlSO').val() != "") {
        $("#FormPBHeader_UsernameSo").val(ctrl.find("option:selected").attr("Item1"));
        $("#FormPBHeader_DesignationSo").val(ctrl.find("option:selected").attr("Item2"));

        if ($('#ddlSO').val() == "99999999") {
            $("#FormPBHeader_UsernameSo").removeAttr("readonly");
            $("#FormPBHeader_DesignationSo").removeAttr("readonly");

        } else {
            $("#FormPBHeader_UsernameSo").attr("readonly", "true");
            $("#FormPBHeader_DesignationSo").attr("readonly", "true");
        }
        $('#FormPBHeader_SignProsd').prop('checked', true);
    }
    else {
        $("#FormPBHeader_UsernameSo").val('');
        $("#FormPBHeader_DesignationSo").val('');
        $('#FormPBHeader_SignProsdSo').prop('checked', false);
    }
}



function Save(SubmitType) {


    if (SubmitType == "Submitted") {
        $("#FormPBHeader_SubmitSts").val(true);

    }

    InitAjaxLoading();

    if ($("#FormPBHeader_Status").val() == "")
        $("#FormPBHeader_Status").val("Initialize");
    else if ($("#FormPBHeader_Status").val() == "Initialize")
        $("#FormPBHeader_Status").val("Saved");

    var FormPB = new Object();
    FormPB.PkRefNo = $("#FormPBHeader_PkRefNo").val()
    FormPB.RefId = $("#FormPBHeader_RefId").val()
    FormPB.SubmissionMonth = $("#ddlMonth").val()
    FormPB.SubmissionYear = $("#ddlYear").val()
    FormPB.AmountBeforeLad = $('.totAmountBeforeLad').text().trim();
    FormPB.LaDamage = $('.totLaDamage').text().trim();
    FormPB.FinalPayment = $('.totFinalPayment').text().trim();
    FormPB.UseridSp = $('#ddlSP').val();
    FormPB.UsernameSp = $('#FormPBHeader_UsernameSp').val();
    FormPB.DesignationSp = $('#FormPBHeader_DesignationSp').val();
    FormPB.SignDateSp = $('#FormPBHeader_SignDateSp').val();
    FormPB.SignSp = $('#FormPBHeader_SignSp').val();
    FormPB.UseridEc = $('#ddlEC').val();
    FormPB.UsernameEc = $('#FormPBHeader_UsernameEc').val();
    FormPB.DesignationEc = $('#FormPBHeader_DesignationEc').val();
    FormPB.SignDateEc = $('#FormPBHeader_SignDateEc').val();
    FormPB.SignEc = $('#FormPBHeader_SignEc').val();
    FormPB.UseridSo = $('#ddlSO').val();
    FormPB.UsernameSo = $('#FormPBHeader_UsernameSo').val();
    FormPB.DesignationSo = $('#FormPBHeader_DesignationSo').val();
    FormPB.SignDateSo = $('#FormPBHeader_SignDateSo').val();
    FormPB.SignSo = $('#FormPBHeader_SignSo').val();

    FormPB.Status = $('#FormPBHeader_Status').val();
    FormPB.SubmitSts = $('#FormPBHeader_SubmitSts').val();

    var Details = []

    $('#tblDetails > tbody  > tr').each(function (index, tr) {

        var PB = new Object();
        PB.PbiwPkRefNo = $("#FormPBHeader_PkRefNo").val();
        PB.IwRef = $(this).find("td:nth-child(1)").find("input").val();
        PB.ProjectTitle = $(this).find("td:nth-child(2)").find("input").val();
        PB.CompletionDate = $(this).find("td:nth-child(3)").find("input").val();
        PB.CompletionRefNo = $(this).find("td:nth-child(4)").find("input").val();
        PB.AmountBeforeLad = $(this).find("td:nth-child(5)").find("input").val();
        PB.LaDamage = $(this).find("td:nth-child(6)").find("input").val();
        PB.FinalPayment = $(this).find("td:nth-child(6)").find("input").val();
        Details.push(PB);

    });

    FormPB.FormPBDetails = Details;

    $.ajax({
        url: '/FormPB/UpdateFormPB',
        data: FormPB,
        type: 'POST',
        success: function (data) {
            HideAjaxLoading();
            if (data == -1) {
                app.ShowErrorMessage(data.errorMessage);
            }
            else {

                app.ShowSuccessMessage('Saved Successfully', false);
                location.href = "/FormPB";
            }
        }
    });



}


function FindDetails() {


    if (ValidatePage('#headerDiv')) {


        if ($("#FormPBHeader_Status").val() == "")
            $("#FormPBHeader_Status").val("Initialize");
        else if ($("#FormPBHeader_Status").val() == "Initialize")
            $("#FormPBHeader_Status").val("Saved");

        InitAjaxLoading();
        var FormPB = new Object();
        FormPB.PkRefNo = $("#FormPBHeader_PkRefNo").val()
        FormPB.SubmissionMonth = $("#ddlMonth").val()
        FormPB.SubmissionYear = $("#ddlYear").val()
        FormPB.UseridSp = $('#ddlSp').val();
        FormPB.UsernameSp = $('#FormPBHeader_UsernameSp').val();
        FormPB.DesignationSp = $('#FormPBHeader_DesignationSp').val();
        FormPB.SignSp = $('#FormPBHeader_SignSp').val();
        FormPB.SignDateSp = $('#FormPBHeader_SignDateSp').val();
        FormPB.Status = $('#FormPBHeader_Status').val();

        $.ajax({
            url: '/FormPB/SaveFormPB',
            data: FormPB,
            type: 'POST',
            success: function (data) {
                HideAjaxLoading();
                if (data == -1) {
                    app.ShowErrorMessage(data.errorMessage);
                }
                else {
                    app.ShowSuccessMessage('Saved Successfully', false);
                    location.href = "/FormPB/Add?Id=" + data + "&view=0";
                }
            }
        });

    }
}


function GoBack() {
    if ($("#hdnView").val() == "0") {
        if (app.Confirm("Unsaved changes will be lost. Are you sure you want to cancel?", function (e) {
            if (e) {
                location.href = "/FormPB";

            }
        }));
    }
    else
        location.href = "/FormPB";
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

    var totBeforeLad = 0;
    var totLAD = 0;
    var totFinalPay = 0;


    $('#tblDetails > tbody  > tr').each(function (index, tr) {

        var BeforeLad = 0;
        var LAD = 0;
        var FinalPay = 0;

        if ($(this).find("td:nth-child(5)").find("input").val() != "") {
            BeforeLad = parseFloat($(this).find("td:nth-child(5)").find("input").val());
        }
        if ($(this).find("td:nth-child(6)").find("input").val() != "") {
            LAD = parseFloat($(this).find("td:nth-child(6)").find("input").val());
        }

        var FinalPay = (parseFloat(BeforeLad) - parseFloat(LAD)).toFixed(2);

        $(this).find("td:nth-child(7)").find("input").val(FinalPay);


        totBeforeLad = (parseFloat(totBeforeLad) + parseFloat(BeforeLad)).toFixed(2);
        totLAD = (parseFloat(totLAD) + parseFloat(LAD)).toFixed(2);
        totFinalPay = (parseFloat(totFinalPay) + parseFloat(FinalPay)).toFixed(2);

    });


    $(".totAmountBeforeLad").text(totBeforeLad);
    $(".totLaDamage").text(totLAD);
    $(".totFinalPayment").text(totFinalPay);

}

