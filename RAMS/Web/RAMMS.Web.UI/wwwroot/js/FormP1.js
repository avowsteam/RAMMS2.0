
$(document).ready(function () {

    SetServiceProviderValues();
    DisableHeader();

    CalculateValues();

});

function SetServiceProviderValues() {

    $(".PayAmt.SPCR").val($("#FormP1Header_ContractRoadLength").val());
    $(".PayAdd.SPCR").val($("#FormP1Header_NetValueAddition").val());
    $(".PayDed.SPCR").val($("#FormP1Header_NetValueDeduction").val());

    $(".PayAmt.SPIC").val($("#FormP1Header_NetValueInstructedWork").val());
    $(".PayDed.SPIC").val($("#FormP1Header_NetValueLadInstructedWork").val());
}

function HorizontalRowCal(rowtype) {

    var tot = 0;
    if ($(".PayAmt." + rowtype).val() != "")
        tot = parseFloat(tot) + parseFloat($(".PayAmt." + rowtype).val());
    if ($(".PayAdd." + rowtype).val() != "" && $(".PayAdd." + rowtype).val() != "LAD:")
        tot = parseFloat(tot) + parseFloat($(".PayAdd." + rowtype).val());
    if ($(".PayDed." + rowtype).val() != "")
        tot = parseFloat(tot) + parseFloat($(".PayDed." + rowtype).val());
    if ($(".TotPrevPay." + rowtype).val() != "")
        tot = parseFloat(tot) + parseFloat($(".TotPrevPay." + rowtype).val());
    $(".TottoDate." + rowtype).val(tot);

    tot = 0;
    if ($(".TotPrevPay." + rowtype).val() != "")
        tot = parseFloat($(".TotPrevPay." + rowtype).val());
    if ($(".TottoDate." + rowtype).val() != "")
        tot = parseFloat($(".TottoDate." + rowtype).val()) - parseFloat(tot);
   

    return parseFloat(tot).toFixed(2);

}

function CalculateValues() {

    //Row total Cal
    $(".AmtincPC.SPCR").val(HorizontalRowCal("SPCR"));
    $(".AmtincPC.SPIC").val(HorizontalRowCal("SPIC"));
    $(".AmtincPC.NCCR").val(HorizontalRowCal("NCCR"));
    $(".AmtincPC.NCIC").val(HorizontalRowCal("NCIC"));
    $(".AmtincPC.NSCR").val(HorizontalRowCal("NSCR"));
    $(".AmtincPC.NSIC").val(HorizontalRowCal("NSIC"));


    //Column total cal
     
    //Amount Contract roads
    $(".PayAmt.STCR").val(TotalCal("PayAmt", "CR") );

    //Amount Instructed Works
    $(".PayAmt.STIC").val(TotalCal("PayAmt", "IC"));

    $(".totamt").text(parseFloat(($(".PayAmt.STCR").val()) + parseFloat($(".PayAmt.STIC").val())).toFixed(2));

   
    //Addition Contract roads
    $(".PayAdd.STCR").val(TotalCal("PayAdd", "CR"));

    //Addition Instructed Works
    $(".PayAdd.STIR").val(TotalCal("PayAdd", "IC"));

    $(".totadd").text(parseFloat($(".PayAdd.STCR").val()).toFixed(2));
    
    //Deduction Contract roads
    $(".PayDed.STCR").val(TotalCal("PayDed", "CR"));

    //Deduction Instructed Works
    $(".PayDed.STIC").val(TotalCal("PayDed", "IC"));

    $(".totDed").text((parseFloat($(".PayDed.STCR").val()) + parseFloat($(".PayDed.STIC").val())).toFixed(2));

    //tot Prev Payment Contract roads
    $(".TotPrevPay.STCR").val(TotalCal("TotPrevPay", "CR"));

    //tot Prev Payment Instructed Works
    $(".TotPrevPay.STIC").val(TotalCal("TotPrevPay", "IC"));

    $(".TotPrevPay").text((parseFloat($(".TotPrevPay.STCR").val()) + parseFloat($(".TotPrevPay.STIC").val())).toFixed(2));

    //tot to date Contract roads
    $(".TottoDate.STCR").val(TotalCal("TottoDate", "CR"));

    //tot to date Instructed Works
    $(".TottoDate.STIC").val(TotalCal("TottoDate", "IC"));

    $(".TotToDate").text((parseFloat($(".TottoDate.STCR").val()) + parseFloat($(".TottoDate.STIC").val())).toFixed(2));

    //Amt Payment certificate Contract roads
    $(".AmtincPC.STCR").val(TotalCal("AmtincPC", "CR"));

    //Amt Payment certificate Instructed Works
    $(".AmtincPC.STIC").val(TotalCal("AmtincPC", "IC"));

    $(".TotAmtIncPC").text((parseFloat($(".AmtincPC.STCR").val()) + parseFloat($(".AmtincPC.STIC").val())).toFixed(2));

    $("#txtGrandTotal").val((parseFloat($(".AmtincPC.STCR").val()) + parseFloat($(".AmtincPC.STIC").val())).toFixed(2));
}

function TotalCal(col, type) {
    var tot = 0;
    if (type == "CR") {
       
        if ($("." + col + ".SPCR").val() != "")
            tot = tot + parseFloat($("." + col + ".SPCR").val());
        if ($("." + col + ".NCCR").val() != "")
            tot = tot + parseFloat($("." + col + ".NCCR").val());
        if ($("." + col + ".NSCR").val() != "")
            tot = tot + parseFloat($("." + col + ".NSCR").val());
        $("." + col + ".STCR").val(tot);
    }
    else {
 
        if ($("." + col + ".SPIC").val() != "")
            tot = tot + parseFloat($("." + col + ".SPIC").val());
        if ($("." + col + ".NCIC").val() != "")
            tot = tot + parseFloat($("." + col + ".NCIC").val());
        if ($("." + col + ".NSIC").val() != "")
            tot = tot + parseFloat($("." + col + ".NSIC").val());
        $("." + col + ".STIC").val(tot);
    }

    return parseFloat(tot).toFixed(2);
}



function DisableHeader() {

    if ($("#FormP1Header_PkRefNo").val() != "0") {
        $("#headerDiv * > select").attr('disabled', true).trigger("chosen:updated");

        $("#btnFindDetails").hide();
    }

    if ($("#hdnView").val() == 1) {
        $("#saveFormP1Btn").hide();
        $("#SubmitFormP1Btn").hide();
        $("#FormP1Header_Bank").attr("readonly", "true");
        $("#FormP1Header_BankAccNo").attr("readonly", "true");
        $("#FormP1Header_Assignee").attr("readonly", "true");
        $("#FormP1Header_Address").attr("readonly", "true");
        $("#FormP1Header_Address").attr("readonly", "true");
    }


}


function OnSOChange(tis) {

    var ctrl = $(tis);
    if (ctrl.val() != null)
        $('#ddlSO').val(ctrl.val());
    if ($('#ddlSO').val() != "") {
        $("#FormP1Header_UsernameSo").val(ctrl.find("option:selected").attr("Item1"));
        $("#FormP1Header_DesignationSo").val(ctrl.find("option:selected").attr("Item2"));

        if ($('#ddlSO').val() == "99999999") {
            $("#FormP1Header_UsernameSo").removeAttr("readonly");
            $("#FormP1Header_DesignationSo").removeAttr("readonly");

        } else {
            $("#FormP1Header_UsernameSo").attr("readonly", "true");
            $("#FormP1Header_DesignationSo").attr("readonly", "true");
        }
        $('#FormP1Header_SignProsd').prop('checked', true);
    }
    else {
        $("#FormP1Header_UsernameSo").val('');
        $("#FormP1Header_DesignationSo").val('');
        $('#FormP1Header_SignSo').prop('checked', false);
    }
}




function Save(SubmitType) {


    if (SubmitType == "Submitted") {
        $("#FormP1Header_SubmitSts").val(true);

    }

    InitAjaxLoading();

    if ($("#FormP1Header_Status").val() == "")
        $("#FormP1Header_Status").val("Initialize");
    else if ($("#FormP1Header_Status").val() == "Initialize")
        $("#FormP1Header_Status").val("Saved");

    var FormP1 = new Object();
    FormP1.PkRefNo = $("#FormP1Header_PkRefNo").val()
    FormP1.RefId = $("#FormP1Header_RefId").val()
    FormP1.PaymentCertificateNo = $("#FormP1Header_PaymentCertificateNo").val()
    FormP1.SubmissionMonth = $("#ddlMonth").val()
    FormP1.SubmissionYear = $("#ddlYear").val()
    FormP1.Bank = $("#FormP1Header_Bank").val()
    FormP1.BankAccNo = $('#FormP1Header_BankAccNo').val();
    FormP1.Assignee = $('#FormP1Header_Assignee').val();
    FormP1.Address = $('#FormP1Header_Address').val();
    FormP1.SubmissionDate = $('#FormP1Header_SubmissionDate').val();
    FormP1.ContractRoadLength = $('#FormP1Header_ContractRoadLength').val();
    FormP1.NetValueDeduction = $('#FormP1Header_NetValueDeduction').val();
    FormP1.NetValueAddition = $('#FormP1Header_NetValueAddition').val();
    FormP1.NetValueInstructedWork = $('#FormP1Header_NetValueInstructedWork').val();
    FormP1.NetValueLadInstructedWork = $('#FormP1Header_NetValueLadInstructedWork').val();
    FormP1.TotalPayment = $('#txtGrandTotal').val();
    FormP1.DueAmount = $('#txtDueAmount').val();
    FormP1.UseridSo = $('#ddlSO').val();
    FormP1.UsernameSo = $('#FormP1Header_UsernameSo').val();
    FormP1.DesignationSo = $('#FormP1Header_DesignationSo').val();
    FormP1.SignDateSo = $('#FormP1Header_SignDateSo').val();
    FormP1.SignSo = $('#FormP1Header_SignSo').val();
    FormP1.Status = $('#FormP1Header_Status').val();
    FormP1.SubmitSts = $('#FormP1Header_SubmitSts').val();

    var P1Details = []
    var i = 0;
    var Description = "";
    $('#tbldetails > tbody  > tr').each(function (index, tr) {
        if (index < 6) {
            var P1 = new Object();
            P1.P1pPkRefNo = $("#FormP1Header_PkRefNo").val();
            if ($(this).find("td:nth-child(1)").attr("rowspan") != undefined) {
                P1.Description = $(this).find("td:nth-child(1)").text().trim();
                Description = P1.Feature;
            }
            else {
                P1.Description = Description;
            }

            P1.PaymentType = $(this).find(".PT").text().trim();
            P1.Amount = $(this).find(".PayAmt").val().trim();
            P1.Addition = $(this).find(".PayAdd").val().trim();
            P1.Deduction = $(this).find(".PayDed").val().trim();
            P1.PreviousPayment = $(this).find(".TotPrevPay").val().trim();
            P1.TottoDate = $(this).find(".TottoDate").val().trim();
            P1.AmountIncludedInPc = $(this).find(".AmtincPC").val().trim();

            P1Details.push(P1);
        }

    });


    FormP1.FormP1Details = P1Details;


    $.ajax({
        url: '/FormP1/UpdateFormP1',
        data: FormP1,
        type: 'POST',
        success: function (data) {
            HideAjaxLoading();
            if (data == -1) {
                app.ShowErrorMessage(data.errorMessage);
            }
            else {

                app.ShowSuccessMessage('Saved Successfully', false);
                location.href = "/FormP1";
            }
        }
    });



}


function FindDetails() {


    if (ValidatePage('#headerDiv')) {


        if ($("#FormP1Header_Status").val() == "")
            $("#FormP1Header_Status").val("Initialize");
        else if ($("#FormP1Header_Status").val() == "Initialize")
            $("#FormP1Header_Status").val("Saved");

        InitAjaxLoading();
        var FormP1 = new Object();
        FormP1.PkRefNo = $("#FormP1Header_PkRefNo").val()
        FormP1.PaymentCertificateNo = $("#FormP1Header_PaymentCertificateNo").val()
        FormP1.SubmissionMonth = $("#ddlMonth").val()
        FormP1.SubmissionYear = $("#ddlYear").val()
        FormP1.Bank = $("#FormP1Header_Bank").val()
        FormP1.BankAccNo = $('#FormP1Header_BankAccNo').val();
        FormP1.Assignee = $('#FormP1Header_Assignee').val();
        FormP1.Address = $('#FormP1Header_Address').val();
        FormP1.SubmissionDate = $('#FormP1Header_SubmissionDate').val();
        FormP1.ContractRoadLength = $('#FormP1Header_ContractRoadLength').val();
        FormP1.NetValueDeduction = $('#FormP1Header_NetValueDeduction').val();
        FormP1.NetValueAddition = $('#FormP1Header_NetValueAddition').val();
        FormP1.NetValueInstructedWork = $('#FormP1Header_NetValueInstructedWork').val();
        FormP1.NetValueLadInstructedWork = $('#FormP1Header_NetValueLadInstructedWork').val();
        FormP1.UseridSo = $('#ddlSO').val();
        FormP1.UsernameSo = $('#FormP1Header_UsernameSo').val();
        FormP1.DesignationSo = $('#FormP1Header_DesignationSo').val();
        FormP1.SignSo = $('#FormP1Header_SignSo').val();
        FormP1.Status = $('#FormP1Header_Status').val();

        $.ajax({
            url: '/FormP1/SaveFormP1',
            data: FormP1,
            type: 'POST',
            success: function (data) {
                HideAjaxLoading();
                if (data == -1) {
                    app.ShowErrorMessage(data.errorMessage);
                }
                else {
                    app.ShowSuccessMessage('Saved Successfully', false);
                    location.href = "/FormP1/Add?Id=" + data + "&view=0";
                }
            }
        });

    }
}


function GoBack() {
    if ($("#hdnView").val() == "0") {
        if (app.Confirm("Unsaved changes will be lost. Are you sure you want to cancel?", function (e) {
            if (e) {
                location.href = "/FormP1";

            }
        }));
    }
    else
        location.href = "/FormP1";
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
 