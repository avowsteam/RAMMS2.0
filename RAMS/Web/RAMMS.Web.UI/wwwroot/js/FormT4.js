
$(document).ready(function () {

    DisableHeader();

    $("#ddlYear").on("change", function () {
        getRevisionNo($("#ddlYear").val());
    });

    if ($("#hdnView").val() == 1) {
        $("#saveFormT4Btn").hide();
        $("#SubmitFormT4Btn").hide();
    }

    CalculateValuesonLoad();

   

});

function DisableHeader() {

    if ($("#FormT4Header_PkRefNo").val() != "0") {
        $("#headerDiv * > select").attr('disabled', true).trigger("chosen:updated");
        $("#FormT4Header_RevisionDate").attr("readonly", "true");
        $("#btnFindDetails").hide();
    }

}

 


function getRevisionNo(id) {
    var req = {};
    req.Year = id;
    req.RMU = $("#ddlRMU").val();
    $.ajax({
        url: '/FormT4/GetMaxRev',
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
        $("#FormT4Header_SubmitSts").val(true);
        
    }

    InitAjaxLoading();

    if ($("#FormT4Header_Status").val() == "")
        $("#FormT4Header_Status").val("Initialize");
    else if ($("#FormT4Header_Status").val() == "Initialize")
        $("#FormT4Header_Status").val("Saved");


    var FormT4Header = new Object();
   FormT4Header.PkRefNo = $("#FormT4Header_PkRefNo").val()
   FormT4Header.PkRefId = $("#FormT4Header_PkRefId").val()
   FormT4Header.Rmu = $("#ddlRMU").val()
   FormT4Header.RevisionYear = $("#ddlYear").val()
   FormT4Header.RevisionNo = $("#RevisionNo").val()
   FormT4Header.RevisionDate = $("#FormT4Header_RevisionDate").val()

   FormT4Header.AdjustableQuantity = $('#FormT4Header_AdjustableQuantity').val();
   FormT4Header.RoutineMaintenance = $('#FormT4Header_RoutineMaintenance').val();
   FormT4Header.PeriodicMaintenance = $('#FormT4Header_PeriodicMaintenance').val();
   FormT4Header.OtherMaintenance = $('#FormT4Header_OtherMaintenance').val();

   FormT4Header.Status = $('#FormT4Header_Status').val();
   FormT4Header.SubmitSts = $('#FormT4Header_SubmitSts').val();

    var T4History = []
    var i = 0;
    var Feature = "";
    $('#tblPPB > tbody  > tr').each(function (index, tr) {

        var T4 = new Object();
        T4.T4pdbhPkRefNo = $("#FormT4Header_PkRefNo").val();
        if ($(this).find("td:nth-child(1)").attr("rowspan") != undefined) {
            T4.Feature = $(this).find("td:nth-child(1)").text().trim();
            Feature = T4.Feature;
        }
        else {
            T4.Feature = Feature;
        }
        T4.Code = $(this).find(".Code").text().trim();
        T4.Name = $(this).find(".Name").text().trim();
        T4.InvCond1 = $(this).find(".IC1").text().trim();
        T4.InvCond2 = $(this).find(".IC2").text().trim();
        T4.InvCond3 = $(this).find(".IC3").text().trim();
        T4.InvTotal = $(this).find(".TotQty").text().trim();
        T4.SlCond1 = $(this).find(".SLC1").text().trim();
        T4.SlCond2 = $(this).find(".SLC2").text().trim();
        T4.SlCond3 = $(this).find(".SLC3").text().trim();
        T4.SlAsl = $(this).find(".ASL").text().trim();
        T4.AwqCond1 = $(this).find(".AWQC1").text().trim();
        T4.AwqCond2 = $(this).find(".AWQC2").text().trim();
        T4.AwqCond3 = $(this).find(".AWQC3").text().trim();
        T4.AwqTotal = $(this).find(".AWQTot").text().trim();
        T4.CrewDaysRequired = $(this).find(".CrewDayReq").text().trim();
        T4.AverageDailyProduction = $(this).find(".ADP").text().trim();
        T4.UnitOfService = $(this).find(".Unit").text().trim();
        T4.CdcLabour = $(this).find(".Lab").text().trim();
        T4.CdcEquipment = $(this).find(".Equ").text().trim();
        T4.CdcMaterial = $(this).find(".Mat").text().trim();
        T4.CrewDaysCost = $(this).find(".CrewDayCost").text().trim();
       
        T4.DbActTotal = $(this).find(".ActTotal").text().trim();
        T4.DbActPercentage = $(this).find(".ActPer").text().trim();
       
        T4History.push(T4);

    });


   FormT4Header.FormT4 = T4History;


    $.ajax({
        url: '/FormT4/UpdateFormT4',
        data: FormT4Header,
        type: 'POST',
        success: function (data) {
            HideAjaxLoading();
            if (data == -1) {
                app.ShowErrorMessage(data.errorMessage);
            }
            else {

                app.ShowSuccessMessage('Saved Successfully', false);
                location.href = "/FormT4";
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
        
        if ($("#FormT4Header_Status").val() == "")
            $("#FormT4Header_Status").val("Initialize");
        else if ($("#FormT4Header_Status").val() == "Initialize")
            $("#FormT4Header_Status").val("Saved");

        InitAjaxLoading();
        var FormT4Header = new Object();
       FormT4Header.PkRefNo = $("#FormT4Header_PkRefNo").val()
       FormT4Header.Rmu = $("#ddlRMU").val()
       FormT4Header.RevisionYear = $("#ddlYear").val()
       FormT4Header.RevisionNo = $("#RevisionNo").val()
       FormT4Header.RevisionDate = $("#FormT4Header_RevisionDate").val()
       FormT4Header.Status = $('#FormT4Header_Status').val();

        $.ajax({
            url: '/FormT4/SaveFormT4',
            data: FormT4Header,
            type: 'POST',
            success: function (data) {
                HideAjaxLoading();
                if (data == -1) {
                    app.ShowErrorMessage(data.errorMessage);
                }
                else {
                    app.ShowSuccessMessage('Saved Successfully', false);
                    location.href = "/FormT4/Add?Id=" + data + "&view=0";
                }
            }
        });

    }
}


function GoBack() {
    if ($("#hdnView").val() == "0") {
        if (app.Confirm("Unsaved changes will be lost. Are you sure you want to cancel?", function (e) {
            if (e) {
                location.href = "/FormT4";

            }
        }));
    }
    else
        location.href = "/FormT4";
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
 

function CalculateValuesonLoad() {

    $('#tblPPB > tbody  > tr').each(function (index, tr) {

        var objtr = $(this);
        ICTotalQty(objtr); //Total Quantity Calculation
        AnnualWorkQtyDesired(objtr);// Annual Work Qty C1 C2 C3 and Total Calculation
        ASL(objtr);//Annual Service Level
        CrewDayreq(objtr); //Crew Days Required
        SubTotalbyActivity(objtr);// SubTotal by Activity
        SumofSubTotalbyActivity();// Sum of SubTotal by Activity
        PlannedPercentage(objtr);// Planned Percentage
        
        SubTotalbyFeature(objtr);// SubTotal by Feature

        CalAdjustableQty();
        CalRoutineMaintenance();
        CalPeriodicMaintenance();
        CalOtherMaintenance();
    });
}

function ICTotalQty(obj) {
    var IC1 = $(obj).find(".IC1").text().trim() != "" ? parseFloat($(obj).find(".IC1").text().trim()) : 0
    var IC2 = $(obj).find(".IC2").text().trim() != "" ? parseFloat($(obj).find(".IC2").text().trim()) : 0
    var IC3 = $(obj).find(".IC3").text().trim() != "" ? parseFloat($(obj).find(".IC3").text().trim()) : 0
    var tot = (IC1 + IC2 + IC3).toFixed(2);
    if (tot == 0)
        tot = "";

    $(obj).find(".TotQty").html(tot);
}

function AnnualWorkQtyDesired(obj) {
    var IC1 = $(obj).find(".IC1").text().trim() != "" ? parseFloat($(obj).find(".IC1").text().trim()) : 0
    var SLC1 = $(obj).find(".SLC1").text().trim() != "" ? parseFloat($(obj).find(".SLC1").text().trim()) : 0
    var C1 = IC1 * SLC1;
    $(obj).find(".AWQC1").html(C1.toFixed(2));

    var IC2 = $(obj).find(".IC2").text().trim() != "" ? parseFloat($(obj).find(".IC2").text().trim()) : 0
    var SLC2 = $(obj).find(".SLC2").text().trim() != "" ? parseFloat($(obj).find(".SLC2").text().trim()) : 0
    var C2 = IC2 * SLC2;
    $(obj).find(".AWQC2").html(C2.toFixed(2));

    var IC3 = $(obj).find(".IC3").text().trim() != "" ? parseFloat($(obj).find(".IC3").text().trim()) : 0
    var SLC3 = $(obj).find(".SLC3").text().trim() != "" ? parseFloat($(obj).find(".SLC3").text().trim()) : 0
    var C3 = IC3 * SLC3;
    $(obj).find(".AWQC3").html(C3.toFixed(2));
    var tot = (C1 + C2 + C3).toFixed(2);
    if (tot == 0)
        tot = "";

    $(obj).find(".AWQTot").html(tot);

}


function ASL(obj) {
    if ($(obj).find(".AWQTot").text().trim() != "" && $(obj).find(".TotQty").text().trim() != "") {
        $(obj).find(".ASL").html((parseFloat($(obj).find(".AWQTot").text().trim()) / parseFloat($(".TotQty").text().trim())).toFixed(2));
    }
    else {
        $(obj).find(".ASL").html("");
    }
}

function CrewDayreq(obj) {
    if ($(obj).find(".AWQTot").text().trim() != "" && $(obj).find(".ADP").text().trim() != "") {
        $(obj).find(".CrewDayReq").html((parseFloat($(obj).find(".AWQTot").text().trim()) / parseFloat($(obj).find(".ADP").text().trim())).toFixed(2));
    }
    else {
        $(obj).find(".CrewDayReq").html("");
    }
}

 

 

function SubTotalbyActivity(obj) {

    if ($(obj).find(".CrewDayCost").text().trim() != "" && $(obj).find(".CrewDayReq").text().trim() != "") {
       // $(obj).find(".ActTotal").html((parseFloat($(obj).find(".CrewDayCost").text().trim()) * parseFloat($(obj).find(".CrewDayReq").text().trim())).toFixed(2));
        $(obj).find(".ActTotal").html(Number((parseFloat($(obj).find(".CrewDayCost").text().trim()) * parseFloat($(obj).find(".CrewDayReq").text().trim())).toFixed(2)).toLocaleString('en'));
    }
    else {
        $(obj).find(".ActTotal").html("");
    }
}

function SumofSubTotalbyActivity(obj) {
    var tot = 0;
    $('#tblPPB > tbody  > tr').each(function (index, tr) {
        if ($(this).find(".ActTotal").text().trim() != "") {
            tot = tot + parseFloat($(this).find(".ActTotal").text().trim().replace(/,/g,''));
        }
    });

    $("#SumofSubTotal").html(Number(parseFloat(tot.toFixed(2))).toLocaleString('en'));
}


function PlannedPercentage(obj) {
    if ($(obj).find(".ActTotal").text().trim() != "") {
        $(obj).find(".ActPer").html((parseFloat($(obj).find(".ActTotal").text().trim().replace(/,/g, '')) / parseFloat($("#SumofSubTotal").text().trim().replace(/,/g, "")) * 100).toFixed(2));
    }
    else {
        $(obj).find(".ActPer").html("");
    }
}
 

function SubTotalbyFeature() {
    var Lasttd;
    var tot = 0;
    var totPer = 0;

    $('#tblPPB > tbody  > tr').each(function (index, tr) {


        if ($(this).find("td:nth-child(1)").attr("rowspan") != undefined) {
            if (Lasttd != undefined)
                $(Lasttd).html(Number(parseFloat(tot.toFixed(2))).toLocaleString('en') + "<br>" + totPer.toFixed(2) + "%");
            Lasttd = $(this).find("td:last");
            tot = 0;
            totPer = 0;
        }
        if ($(this).find(".ActTotal").text().trim() != "") {
            tot = tot + parseFloat($(this).find(".ActTotal").text().trim().replace(/,/g, ''));
        }
        if ($(this).find(".ActPer").text().trim() != "") {
            totPer = totPer + parseFloat($(this).find(".ActPer").text().trim());
        }
    });

    $(Lasttd).html(Number(parseFloat(tot.toFixed(2))).toLocaleString('en') + "<br>" + totPer.toFixed(2) +"%");
}

function CalAdjustableQty() {

    var tot = 0;

    if ($('#tblPPB > tbody  > tr:eq(0)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(0)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(1)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(1)').find(".ActTotal").text().trim().replace(/,/g, ''));

    $("#FormT4Header_AdjustableQuantity").val(Number(parseFloat(tot.toFixed(2))).toLocaleString('en'));

    

}

function CalRoutineMaintenance() {
    var tot = 0;

    if ($('#tblPPB > tbody  > tr:eq(9)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(9)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(13)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(13)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(14)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(14)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(15)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(15)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(18)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(18)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(19)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(19)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(22)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(22)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(24)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(24)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(27)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(27)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(28)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(28)').find(".ActTotal").text().trim().replace(/,/g, ''));

    $("#FormT4Header_RoutineMaintenance").val(Number(parseFloat(tot.toFixed(2))).toLocaleString('en'));
}

function CalPeriodicMaintenance() {
    var tot = 0;

    if ($('#tblPPB > tbody  > tr:eq(2)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(2)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(3)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(3)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(4)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(4)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(5)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(5)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(6)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(6)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(7)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(7)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(8)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(8)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(9)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(9)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(27)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(27)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(10)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(10)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(11)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(11)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(12)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(12)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(17)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(17)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(20)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(20)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(21)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(21)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(22)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(22)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(23)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(23)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(24)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(24)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(25)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(25)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(26)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(26)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(30)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(30)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(31)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(31)').find(".ActTotal").text().trim().replace(/,/g, ''));


    $("#FormT4Header_PeriodicMaintenance").val(Number(parseFloat(tot.toFixed(2))).toLocaleString('en'));
}

function CalOtherMaintenance() {
    var tot = 0;

    if ($('#tblPPB > tbody  > tr:eq(33)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(33)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(34)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(34)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(35)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(35)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(36)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(36)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(37)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(37)').find(".ActTotal").text().trim().replace(/,/g, ''));

    if ($('#tblPPB > tbody  > tr:eq(38)').find(".ActTotal").text().trim() != "")
        tot = tot + parseFloat($('#tblPPB > tbody  > tr:eq(38)').find(".ActTotal").text().trim().replace(/,/g, ''));

    $("#FormT4Header_OtherMaintenance").val(Number(parseFloat(tot.toFixed(2))).toLocaleString('en'));
}