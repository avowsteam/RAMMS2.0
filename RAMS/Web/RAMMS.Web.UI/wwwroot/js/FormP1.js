
$(document).ready(function () {

    SetServiceProviderValues();
    DisableHeader();

    CalculateValuesonLoad();

});

function SetServiceProviderValues() {

    $(".PayAmt.SPCR").val($("#FormP1Header_ContractRoadLength").val());
    $(".PayAdd.SPCR").val($("#FormP1Header_NetValueAddition").val());
    $(".PayDed.SPCR").val($("#FormP1Header_NetValueDeduction").val());

    $(".PayAmt.SPIC").val($("#FormP1Header_NetValueInstructedWork").val());
    $(".PayDed.SPIC").val($("#FormP1Header_NetValueLadInstructedWork").val());
}

function ServiceProviderCal() {

    var tot = 0;
    if ($(".PayAmt.SPCR").val() != "")
        tot = tot + parseFloat($(".PayAmt.SPCR").val()).toFixed(2);
    if ($(".PayAdd.SPCR").val() != "")
        tot = tot + parseFloat($(".PayAdd.SPCR").val()).toFixed(2);
    if ($(".PayDed.SPCR").val() != "")
        tot = tot + parseFloat($(".PayDed.SPCR").val()).toFixed(2);
    if ($(".TotPrevPay.SPCR").val() != "")
        tot = tot + parseFloat($(".TotPrevPay.SPCR").val()).toFixed(2);
    $(".TottoDate.SPCR").val(tot);

    tot = 0;
    if ($(".PayAmt.SPIC").val() != "")
        tot = tot + parseFloat($(".PayAmt.SPIC").val()).toFixed(2);
    if ($(".PayDed.SPIC").val() != "")
        tot = tot + parseFloat($(".PayDed.SPIC").val()).toFixed(2);
    if ($(".TotPrevPay.SPIC").val() != "")
        tot = tot + parseFloat($(".TotPrevPay.SPIC").val()).toFixed(2);
    $(".TottoDate.SPIC").val(tot);

    tot = 0;
    if ($(".TotPrevPay.SPCR").val() != "")
        tot = tot + parseFloat($(".TotPrevPay.SPCR").val()).toFixed(2);
    if ($(".TottoDate.SPCR").val() != "")
        tot = tot + parseFloat($(".PayDed.SPCR").val()).toFixed(2);
    $(".AmtincPC.SPCR").val(tot);

    tot = 0;
    if ($(".TotPrevPay.SPIC").val() != "")
        tot = tot + parseFloat($(".TotPrevPay.SPIC").val()).toFixed(2);
    if ($(".TottoDate.SPIC").val() != "")
        tot = tot + parseFloat($(".PayDed.SPIC").val()).toFixed(2);
    $(".AmtincPC.SPIC").val(tot);



}

function TotalCal() {

    //Amount Contract roads
    $(".PayAmt.STCR").val(TotalCal("PayAmt", "CR") );

    //Amount Instructed Works
    $(".PayAmt.STIC").val(TotalCal("PayAmt", "IC"));


   
    //Amount Contract roads
    $(".PayAdd.STCR").val(TotalCal("PayAdd", "CR"));
    
    //Deduction Contract roads
    $(".PayDed.STCR").val(TotalCal("PayDed", "CR"));
    
    //Deduction Instructed Works
    $(".PayDed.STIC").val(TotalCal("PayDed", "IC"));

    //tot Prev Payment Contract roads
    $(".TotPrevPay.STCR").val(TotalCal("TotPrevPay", "CR"));

    //tot Prev Payment Instructed Works
    $(".TotPrevPay.STIC").val(TotalCal("TotPrevPay", "IC"));

    //tot to date Contract roads
    $(".TottoDate.STCR").val(TotalCal("TottoDate", "CR"));

    //tot to date Instructed Works
    $(".TottoDate.STIC").val(TotalCal("TottoDate", "IC"));

    //Amt Payment certificate Contract roads
    $(".AmtincPC.STCR").val(TotalCal("AmtincPC", "CR"));

    //Amt Payment certificate Instructed Works
    $(".AmtincPC.STIC").val(TotalCal("AmtincPC", "IC"));


}

function TotalCal(col, type) {
    var tot = 0;
    if (type == "CR") {
       
        if ($("." + col + ".SPCR").val() != "")
            tot = tot + parseFloat($("." + col + ".SPCR").val()).toFixed(2);
        if ($("." + col + ".NCCR").val() != "")
            tot = tot + parseFloat($("." + col + ".NCCR").val()).toFixed(2);
        if ($("." + col + ".NSCR").val() != "")
            tot = tot + parseFloat($("." + col + ".NSCR").val()).toFixed(2);
        $("." + col + ".STCR").val(tot);
    }
    else {
 
        if ($("." + col + ".SPIC").val() != "")
            tot = tot + parseFloat($("." + col + ".SPIC").val()).toFixed(2);
        if ($("." + col + ".NCIC").val() != "")
            tot = tot + parseFloat($("." + col + ".NCIC").val()).toFixed(2);
        if ($("." + col + ".NSIC").val() != "")
            tot = tot + parseFloat($("." + col + ".NSIC").val()).toFixed(2);
        $("." + col + ".STIC").val(tot);
    }

    return tot;
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
    FormP1.PkRefId = $("#FormP1Header_RefId").val()
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
    FormP1.SubmitSts = $('#FormP1Header_SubmitSts').val();

    var P1Details = []
    var i = 0;
    var Description = "";
    $('#tblPPB > tbody  > tr').each(function (index, tr) {

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
        P1.TottoDate = $(this).find(".TotalToDate").val().trim();
        P1.AmountIncludedInPc = $(this).find(".TotalToDate").val().trim();

        P1Details.push(P1);

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

function CalculateValues(obj) {
    var objtr = $(obj).parent(0).parent(0);
    ICTotalQty(objtr); //Total Quantity Calculation
    AnnualWorkQtyDesired(objtr);// Annual Work Qty C1 C2 C3 and Total Calculation
    CrewDayreq(objtr); //Crew Days Required
    ServiceLevel(objtr);// Service Level Calculation
    AnnualWorkQtyPlanned(objtr); // Annual Work Qty Planned
    CrewDayplanned(objtr); //Crew Days Planned
    SubTotalbyActivity(objtr);// SubTotal by Activity
    SumofSubTotalbyActivity();// Sum of SubTotal by Activity
    PlannedPercentage(objtr);// Planned Percentage
    SumofSubTotalPercentage(); // Sum of SubTotal Percentage
    SubTotalbyFeature(objtr);// SubTotal by Feature

    CalAdjustableQty();
    CalRoutineMaintenance();
    CalPeriodicMaintenance();
    CalOtherMaintenance();
}

function CalculateValuesonLoad() {

    $('#tblPPB > tbody  > tr').each(function (index, tr) {

        var objtr = $(this);
        ICTotalQty(objtr); //Total Quantity Calculation
        AnnualWorkQtyDesired(objtr);// Annual Work Qty C1 C2 C3 and Total Calculation
        CrewDayreq(objtr); //Crew Days Required
        ServiceLevel(objtr);// Service Level Calculation
        AnnualWorkQtyPlanned(objtr); // Annual Work Qty Planned
        CrewDayplanned(objtr); //Crew Days Planned
        SubTotalbyActivity(objtr);// SubTotal by Activity
        SumofSubTotalbyActivity();// Sum of SubTotal by Activity
        PlannedPercentage(objtr);// Planned Percentage
        SumofSubTotalPercentage(); // Sum of SubTotal Percentage
        SubTotalbyFeature(objtr);// SubTotal by Feature

        CalAdjustableQty();
        CalRoutineMaintenance();
        CalPeriodicMaintenance();
        CalOtherMaintenance();
    });
}

function ICTotalQty(obj) {
    var IC1 = $(obj).find(".IC1").val() != "" ? parseFloat($(obj).find(".IC1").val()) : 0
    var IC2 = $(obj).find(".IC2").val() != "" ? parseFloat($(obj).find(".IC2").val()) : 0
    var IC3 = $(obj).find(".IC3").val() != "" ? parseFloat($(obj).find(".IC3").val()) : 0
    var tot = (IC1 + IC2 + IC3).toFixed(2);
    if (tot == 0)
        tot = "";

    $(obj).find(".TotQty").html(tot);
}

function AnnualWorkQtyDesired(obj) {
    var IC1 = $(obj).find(".IC1").val() != "" ? parseFloat($(obj).find(".IC1").val()) : 0
    var SLC1 = $(obj).find(".SLC1").text().trim() != "" ? parseFloat($(obj).find(".SLC1").text().trim()) : 0
    var C1 = IC1 * SLC1;
    $(obj).find(".AWQC1").html(C1.toFixed(2));

    var IC2 = $(obj).find(".IC2").val() != "" ? parseFloat($(obj).find(".IC2").val()) : 0
    var SLC2 = $(obj).find(".SLC2").text().trim() != "" ? parseFloat($(obj).find(".SLC2").text().trim()) : 0
    var C2 = IC2 * SLC2;
    $(obj).find(".AWQC2").html(C2.toFixed(2));

    var IC3 = $(obj).find(".IC3").val() != "" ? parseFloat($(obj).find(".IC3").val()) : 0
    var SLC3 = $(obj).find(".SLC3").text().trim() != "" ? parseFloat($(obj).find(".SLC3").text().trim()) : 0
    var C3 = IC3 * SLC3;
    $(obj).find(".AWQC3").html(C3.toFixed(2));
    var tot = (C1 + C2 + C3).toFixed(2);
    if (tot == 0)
        tot = "";

    $(obj).find(".AWQTot").html(tot);

}

function CrewDayreq(obj) {
    if ($(obj).find(".AWQTot").text().trim() != "" && $(obj).find(".ADP").text().trim() != "") {
        $(obj).find(".CrewDayReq").html((parseFloat($(obj).find(".AWQTot").text().trim()) / parseFloat($(obj).find(".ADP").text().trim())).toFixed(2));
    }
    else {
        $(obj).find(".CrewDayReq").html("");
    }
}

function ServiceLevel(obj) {

    //Desirer cal
    if ($(obj).find(".AWQTot").text().trim() != "" && $(obj).find(".TotQty").text().trim() != "") {
        $(obj).find(".SLDesired").html((parseFloat($(obj).find(".AWQTot").text().trim()) / parseFloat($(obj).find(".TotQty").text().trim())).toFixed(2));
    }
    else {
        $(obj).find(".SLDesired").html("");
    }

    //Planned Cal

    if ($(obj).find(".SLDesired").text().trim() != "" && $(obj).find(".Desired").val().trim() != "") {
        $(obj).find(".SLPlan").html((parseFloat($(obj).find(".SLDesired").text().trim()) * parseFloat($(obj).find(".Desired").val().trim())).toFixed(2));
    }
    else {
        $(obj).find(".SLPlan").html("");
    }


}

function AnnualWorkQtyPlanned(obj) {

    if ($(obj).find(".Desired").val().trim() != "" && $(obj).find(".AWQTot").text().trim() != "") {
        $(obj).find(".AWQ").html((parseFloat($(obj).find(".Desired").val().trim()) * parseFloat($(obj).find(".AWQTot").text().trim())).toFixed(2));
    }
    else {
        $(obj).find(".AWQ").html("");
    }
}


function CrewDayplanned(obj) {

    if ($(obj).find(".AWQ").text().trim() != "" && $(obj).find(".ADP").text().trim() != "") {
        $(obj).find(".CDP").html((parseFloat($(obj).find(".AWQ").text().trim()) / parseFloat($(obj).find(".ADP").text().trim())).toFixed(2));
    }
    else {
        $(obj).find(".CDP").html("");
    }
}

function SubTotalbyActivity(obj) {

    if ($(obj).find(".CrewDayCost").text().trim() != "" && $(obj).find(".CDP").text().trim() != "") {
        $(obj).find(".ActTotal").html((parseFloat($(obj).find(".CrewDayCost").text().trim()) * parseFloat($(obj).find(".CDP").text().trim())).toFixed(2));
    }
    else {
        $(obj).find(".ActTotal").html("");
    }
}

function SumofSubTotalbyActivity(obj) {
    var tot = 0;
    $('#tblPPB > tbody  > tr').each(function (index, tr) {
        if ($(this).find(".ActTotal").text().trim() != "") {
            tot = tot + parseFloat($(this).find(".ActTotal").text().trim());
        }
    });

    $("#SumofSubTotal").html(Number(parseFloat(tot.toFixed(2))).toLocaleString('en'));
}


function PlannedPercentage(obj) {
    if ($(obj).find(".ActTotal").text().trim() != "" && $(obj).find(".CDP").text().trim() != "") {
        $(obj).find(".ActPer").html((parseFloat($(obj).find(".ActTotal").text().trim()) / parseFloat($("#SumofSubTotal").text().trim().replace(/,/g, "")) * 100).toFixed(2));
    }
    else {
        $(obj).find(".ActPer").html("");
    }
}

function SumofSubTotalPercentage(obj) {
    var tot = 0;
    $('#tblPPB > tbody  > tr').each(function (index, tr) {
        if ($(this).find(".ActPer").text().trim()) {
            tot = tot + parseFloat($(this).find(".ActPer").text().trim());
        }
    });

    $("#SumofSubTotalPerc").html(tot.toFixed(2));
}

function SubTotalbyFeature() {
    var Lasttd;
    var tot = 0;
    var totPer = 0;

    $('#tblPPB > tbody  > tr').each(function (index, tr) {


        if ($(this).find("td:nth-child(1)").attr("rowspan") != undefined) {
            if (Lasttd != undefined)
                $(Lasttd).html(tot.toFixed(2) + "<br>" + totPer.toFixed(2));
            Lasttd = $(this).find("td:last");
            tot = 0;
            totPer = 0;
        }
        if ($(this).find(".ActTotal").text().trim() != "") {
            tot = tot + parseFloat($(this).find(".ActTotal").text().trim());
        }
        if ($(this).find(".ActPer").text().trim() != "") {
            totPer = totPer + parseFloat($(this).find(".ActPer").text().trim());
        }
    });

    $(Lasttd).html(tot.toFixed(2) + "<br>" + totPer.toFixed(2));
}

function CalAdjustableQty() {

    var tot = 0;

    if ($('#tblPPB > tbody  > tr:eq(0)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(0)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(1)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(1)').find(".ActTotal").text().trim());

    $("#FormP1Header_AdjustableQuantity").val(Number(parseFloat(tot.toFixed(2))).toLocaleString('en'));



}

function CalRoutineMaintenance() {
    var tot = 0;

    if ($('#tblPPB > tbody  > tr:eq(9)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(9)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(13)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(13)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(14)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(14)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(15)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(15)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(18)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(18)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(19)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(19)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(22)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(22)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(24)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(24)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(27)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(27)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(28)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(28)').find(".ActTotal").text().trim());

    $("#FormP1Header_RoutineMaintenance").val(Number(parseFloat(tot.toFixed(2))).toLocaleString('en'));
}

function CalPeriodicMaintenance() {
    var tot = 0;

    if ($('#tblPPB > tbody  > tr:eq(2)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(2)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(3)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(3)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(4)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(4)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(5)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(5)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(6)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(6)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(7)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(7)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(8)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(8)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(9)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(9)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(27)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(27)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(10)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(10)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(11)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(11)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(12)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(12)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(17)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(17)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(20)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(20)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(21)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(21)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(22)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(22)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(23)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(23)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(24)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(24)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(25)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(25)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(26)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(26)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(30)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(30)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(31)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(31)').find(".ActTotal").text().trim());


    $("#FormP1Header_PeriodicMaintenance").val(Number(parseFloat(tot.toFixed(2))).toLocaleString('en'));
}

function CalOtherMaintenance() {
    var tot = 0;

    if ($('#tblPPB > tbody  > tr:eq(33)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(33)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(34)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(34)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(35)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(35)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(36)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(36)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(37)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(37)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(38)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(38)').find(".ActTotal").text().trim());

    $("#FormP1Header_OtherMaintenance").val(Number(parseFloat(tot.toFixed(2))).toLocaleString('en'));
}