
$(document).ready(function () {

    DisableHeader();

    $("#ddlYear").on("change", function () {
        getRevisionNo($("#ddlYear").val());
    });

    if ($("#hdnView").val() == 1) {
        $("#saveFormB13Btn").hide();
        $("#SubmitFormB13Btn").hide();
    }

    CalculateValuesonLoad();



});

function DisableHeader() {

    if ($("#FormB13_PkRefNo").val() != "0") {
        $("#headerDiv * > select").attr('disabled', true).trigger("chosen:updated");
        $("#FormB13_RevisionDate").attr("readonly", "true");
        $("#btnFindDetails").hide();
    }

}


function OnProposedbyChange(tis) {

    var ctrl = $(tis);
    if (ctrl.val() != null)
        $('#ddlProposedby').val(ctrl.val());
    if ($('#ddlProposedby').val() != "") {
        $("#FormB13_UserNameProsd").val(ctrl.find("option:selected").attr("Item1"));
        $("#FormB13_UserDesignationProsd").val(ctrl.find("option:selected").attr("Item2"));

        if ($('#ddlProposedby').val() == "99999999") {
            $("#FormB13_UserNameProsd").removeAttr("readonly");
            $("#FormB13_UserDesignationProsd").removeAttr("readonly");

        } else {
            $("#FormB13_UserNameProsd").attr("readonly", "true");
            $("#FormB13_UserDesignationProsd").attr("readonly", "true");
        }
        $('#FormB13_SignProsd').prop('checked', true);
    }
    else {
        $("#FormV1_UsernameSch").val('');
        $("#FormB13_UserDesignationProsd").val('');
        $('#FormB13_SignProsd').prop('checked', false);
    }
}

function OnFacilitatedChange(tis) {

    var ctrl = $(tis);
    if (ctrl.val() != null)
        $('#ddlFacilitatedby').val(ctrl.val());
    if ($('#ddlFacilitatedby').val() != "") {
        $("#FormB13_UserNameFclitd").val(ctrl.find("option:selected").attr("Item1"));
        $("#FormB13_UserDesignationFclitd").val(ctrl.find("option:selected").attr("Item2"));

        if ($('#ddlFacilitatedby').val() == "99999999") {
            $("#FormB13_UserNameFclitd").removeAttr("readonly");
            $("#FormB13_UserDesignationFclitd").removeAttr("readonly");

        } else {
            $("#FormB13_UserNameFclitd").attr("readonly", "true");
            $("#FormB13_UserDesignationFclitd").attr("readonly", "true");
        }
        $('#FormB13_SignFclitd').prop('checked', true);
    }
    else {
        $("#FormV1_UsernameSch").val('');
        $("#FormB13_UserDesignationFclitd").val('');
        $('#FormB13_SignFclitd').prop('checked', false);
    }
}


function OnAgreedbyChange(tis) {

    var ctrl = $(tis);
    if (ctrl.val() != null)
        $('#ddlAgreedby').val(ctrl.val());
    if ($('#ddlAgreedby').val() != "") {
        $("#FormB13_UserNameAgrd").val(ctrl.find("option:selected").attr("Item1"));
        $("#FormB13_UserDesignationAgrd").val(ctrl.find("option:selected").attr("Item2"));

        if ($('#ddlAgreedby').val() == "99999999") {
            $("#FormB13_UserNameAgrd").removeAttr("readonly");
            $("#FormB13_UserDesignationAgrd").removeAttr("readonly");

        } else {
            $("#FormB13_UserNameAgrd").attr("readonly", "true");
            $("#FormB13_UserDesignationAgrd").attr("readonly", "true");
        }
        $('#FormB13_SignAgrd').prop('checked', true);
    }
    else {
        $("#FormV1_UsernameSch").val('');
        $("#FormB13_UserDesignationAgrd").val('');
        $('#FormB13_SignAgrd').prop('checked', false);
    }
}

function OnEndorsedbyChange(tis) {

    var ctrl = $(tis);
    if (ctrl.val() != null)
        $('#ddlEndorsedby').val(ctrl.val());
    if ($('#ddlEndorsedby').val() != "") {
        $("#FormB13_UserNameEdosd").val(ctrl.find("option:selected").attr("Item1"));
        $("#FormB13_UserDesignationEdosd").val(ctrl.find("option:selected").attr("Item2"));

        if ($('#ddlEndorsedby').val() == "99999999") {
            $("#FormB13_UserNameEdosd").removeAttr("readonly");
            $("#FormB13_UserDesignationEdosd").removeAttr("readonly");

        } else {
            $("#FormB13_UserNameEdosd").attr("readonly", "true");
            $("#FormB13_UserDesignationEdosd").attr("readonly", "true");
        }
        $('#FormB13_SignEdosd').prop('checked', true);
    }
    else {
        $("#FormV1_UsernameSch").val('');
        $("#FormB13_UserDesignationEdosd").val('');
        $('#FormB13_SignEdosd').prop('checked', false);
    }
}



function getRevisionNo(id) {
    var req = {};
    req.Year = id;
    req.RMU = $("#ddlRMU").val();
    $.ajax({
        url: '/FormB13/GetMaxRev',
        dataType: 'JSON',
        data: req,
        type: 'Post',
        success: function (data) {
            $("#RevisionNo").val(data)
        },
        error: function (data) {
            console.error(data);
        }
    });
}





function Save(SubmitType) {


    if (SubmitType == "Submitted") {
        $("#FormB13_SubmitSts").val(true);

    }

    InitAjaxLoading();

    if ($("#FormB13_Status").val() == "")
        $("#FormB13_Status").val("Initialize");
    else if ($("#FormB13_Status").val() == "Initialize")
        $("#FormB13_Status").val("Saved");


    var FormB13 = new Object();
    FormB13.PkRefNo = $("#FormB13_PkRefNo").val()
    FormB13.PkRefId = $("#FormB13_PkRefId").val()
    FormB13.Rmu = $("#ddlRMU").val()
    FormB13.RevisionYear = $("#ddlYear").val()
    FormB13.RevisionNo = $("#RevisionNo").val()
    FormB13.RevisionDate = $("#FormB13_RevisionDate").val()
    FormB13.Description = $("#Desc").val()


    FormB13.UseridProsd = $('#ddlProposedby').val();
    FormB13.UserNameProsd = $('#FormB13_UserNameProsd').val();
    FormB13.DtProsd = $('#FormB13_DtProsd').val();
    FormB13.UserDesignationProsd = $('#FormB13_UserDesignationProsd').val();
    FormB13.SignProsd = $('#FormB13_SignProsd').val();

    FormB13.UseridFclitd = $('#ddlFacilitatedby').val();
    FormB13.UserNameFclitd = $('#FormB13_UserNameFclitd').val();
    FormB13.DtFclitd = $('#FormB13_DtFclitd').val();
    FormB13.UserDesignationFclitd = $('#FormB13_UserDesignationFclitd').val();
    FormB13.SignFclitd = $('#FormB13_SignFclitd').val();

    FormB13.UseridAgrd = $('#ddlAgreedby').val();
    FormB13.UserNameAgrd = $('#FormB13_UserNameAgrd').val();
    FormB13.DtAgrd = $('#FormB13_DtAgrd').val();
    FormB13.UserDesignationAgrd = $('#FormB13_UserDesignationAgrd').val();
    FormB13.SignAgrd = $('#FormB13_SignAgrd').val();

    FormB13.UseridEdosd = $('#FormB13_UseridEdosd').val();
    FormB13.UserNameEdosd = $('#FormB13_UserNameEdosd').val();
    FormB13.DtEdosd = $('#FormB13_DtEdosd').val();
    FormB13.UserDesignationEdosd = $('#FormB13_UserDesignationEdosd').val();
    FormB13.SignEdosd = $('#FormB13_SignEdosd').val();

    FormB13.AdjustableQuantity = $('#FormB13_AdjustableQuantity').val();
    FormB13.RoutineMaintenance = $('#FormB13_RoutineMaintenance').val();
    FormB13.PeriodicMaintenance = $('#FormB13_PeriodicMaintenance').val();
    FormB13.OtherMaintenance = $('#FormB13_OtherMaintenance').val();

    FormB13.Status = $('#FormB13_Status').val();
    FormB13.SubmitSts = $('#FormB13_SubmitSts').val();

    var B13History = []
    var i = 0;
    var Feature = "";
    $('#tblPPB > tbody  > tr').each(function (index, tr) {

        var B13 = new Object();
        B13.B13pPkRefNo = $("#FormB13_PkRefNo").val();
        if ($(this).find("td:nth-child(1)").attr("rowspan") != undefined) {
            B13.Feature = $(this).find("td:nth-child(1)").text().trim();
            Feature = B13.Feature;
        }
        else {
            B13.Feature = Feature;
        }
        B13.Code = $(this).find(".Code").text().trim();
        B13.Name = $(this).find(".Name").text().trim();
        B13.InvCond1 = $(this).find(".IC1").val().trim();
        B13.InvCond2 = $(this).find(".IC2").val().trim();
        B13.InvCond3 = $(this).find(".IC3").val().trim();
        B13.InvTotal = $(this).find(".TotQty").text().trim();
        B13.SlCond1 = $(this).find(".SLC1").text().trim();
        B13.SlCond2 = $(this).find(".SLC2").text().trim();
        B13.SlCond3 = $(this).find(".SLC3").text().trim();
        B13.AwqCond1 = $(this).find(".AWQC1").text().trim();
        B13.AwqCond2 = $(this).find(".AWQC2").text().trim();
        B13.AwqCond3 = $(this).find(".AWQC3").text().trim();
        B13.AwqTotal = $(this).find(".AWQTot").text().trim();
        B13.CrewDaysRequired = $(this).find(".CrewDayReq").text().trim();
        B13.CdcLabour = $(this).find(".Lab").text().trim();
        B13.CdcEquipment = $(this).find(".Equ").text().trim();
        B13.CdcMaterial = $(this).find(".Mat").text().trim();
        B13.CrewDaysCost = $(this).find(".CrewDayCost").text().trim();
        B13.AverageDailyProduction = $(this).find(".ADP").text().trim();
        B13.UnitOfService = $(this).find(".Unit").text().trim();
        B13.SlDesired = $(this).find(".SLDesired").text().trim();
        B13.SlPlanned = $(this).find(".SLPlan").text().trim();
        B13.SlAvgDesired = $(this).find(".Desired").val().trim();
        B13.SlAnnualWorkQuantity = $(this).find(".AWQ").text().trim();
        B13.SlCrewDaysPlanned = $(this).find(".CDP").text().trim();
        B13.SlTotalByActivity = $(this).find(".ActTotal").text().trim();
        B13.SlPercentageByActivity = $(this).find(".ActPer").text().trim();

        B13History.push(B13);

    });


    FormB13.FormB13History = B13History;


    $.ajax({
        url: '/FormB13/UpdateFormB13',
        data: FormB13,
        type: 'POST',
        success: function (data) {
            HideAjaxLoading();
            if (data == -1) {
                app.ShowErrorMessage(data.errorMessage);
            }
            else {

                app.ShowSuccessMessage('Saved Successfully', false);
                location.href = "/FormB13";
            }
        }
    });



}


function FindDetails() {


    if (ValidatePage('#headerDiv')) {

        if (parseInt($("#RevisionNo").val()) > 4) {
            app.ShowErrorMessage("More than 4 revisions are not allowed");
            return;
        }

        if ($("#FormB13_Status").val() == "")
            $("#FormB13_Status").val("Initialize");
        else if ($("#FormB13_Status").val() == "Initialize")
            $("#FormB13_Status").val("Saved");

        InitAjaxLoading();
        var FormB13 = new Object();
        FormB13.PkRefNo = $("#FormB13_PkRefNo").val()
        FormB13.Rmu = $("#ddlRMU").val()
        FormB13.RevisionYear = $("#ddlYear").val()
        FormB13.RevisionNo = $("#RevisionNo").val()
        FormB13.RevisionDate = $("#FormB13_RevisionDate").val()
        FormB13.UseridProsd = $('#ddlProposedby').val();
        FormB13.UserNameProsd = $('#FormB13_UserNameProsd').val();
        FormB13.DtProsd = $('#FormB13_DTProsd').val();
        FormB13.UserDesignationProsd = $('#FormB13_UserDesignationProsd').val();
        FormB13.SignProsd = $('#FormB13_SignProsd').val();

        FormB13.Status = $('#FormB13_Status').val();

        $.ajax({
            url: '/FormB13/SaveFormB13',
            data: FormB13,
            type: 'POST',
            success: function (data) {
                HideAjaxLoading();
                if (data == -1) {
                    app.ShowErrorMessage(data.errorMessage);
                }
                else {
                    app.ShowSuccessMessage('Saved Successfully', false);
                    location.href = "/FormB13/Add?Id=" + data + "&view=0";
                }
            }
        });

    }
}


function GoBack() {
    if ($("#hdnView").val() == "0") {
        if (app.Confirm("Unsaved changes will be lost. Are you sure you want to cancel?", function (e) {
            if (e) {
                location.href = "/FormB13";

            }
        }));
    }
    else
        location.href = "/FormB13";
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
        $(obj).find(".SLPlan").html((parseFloat($(obj).find(".SLDesired").text().trim()) / 100 * parseFloat($(obj).find(".Desired").val().trim())).toFixed(2));
    }
    else {
        $(obj).find(".SLPlan").html("");
    }


}

function AnnualWorkQtyPlanned(obj) {

    if ($(obj).find(".Desired").val().trim() != "" && $(obj).find(".AWQTot").text().trim() != "") {
        $(obj).find(".AWQ").html((parseFloat($(obj).find(".Desired").val().trim()) / 100 * parseFloat($(obj).find(".AWQTot").text().trim())).toFixed(2));
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
        if (parseFloat($(obj).find(".ActTotal").text().trim()) > 0 && parseFloat($("#SumofSubTotal").text().trim().replace(/,/g, "")) > 0)
            $(obj).find(".ActPer").html((parseFloat($(obj).find(".ActTotal").text().trim()) / parseFloat($("#SumofSubTotal").text().trim().replace(/,/g, "")) * 100).toFixed(2));
        else
            $(obj).find(".ActPer").html("");
    }
    else {
        $(obj).find(".ActPer").html("");
    }
}

function SumofSubTotalPercentage(obj) {
    var tot = 0;
    $('#tblPPB > tbody  > tr').each(function (index, tr) {
        if ($(this).find(".ActPer").text().trim()) {
            tot = tot + $(this).find(".ActPer").text().trim() != "" ? parseFloat($(this).find(".ActPer").text().trim()) : "";
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
                $(Lasttd).html(Number(parseFloat(tot.toFixed(2))).toLocaleString('en') + "<br>" + totPer.toFixed(2));
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

    $(Lasttd).html(Number(parseFloat(tot.toFixed(2))).toLocaleString('en') + "<br>" + totPer.toFixed(2));
}

function CalAdjustableQty() {

    var tot = 0;

    if ($('#tblPPB > tbody  > tr:eq(0)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(0)').find(".ActTotal").text().trim());

    if ($('#tblPPB > tbody  > tr:eq(1)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(1)').find(".ActTotal").text().trim());

    $("#FormB13_AdjustableQuantity").val(Number(parseFloat(tot.toFixed(2))).toLocaleString('en'));



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

    $("#FormB13_RoutineMaintenance").val(Number(parseFloat(tot.toFixed(2))).toLocaleString('en'));
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


    $("#FormB13_PeriodicMaintenance").val(Number(parseFloat(tot.toFixed(2))).toLocaleString('en'));
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

    $("#FormB13_OtherMaintenance").val(Number(parseFloat(tot.toFixed(2))).toLocaleString('en'));
}