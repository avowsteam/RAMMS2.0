﻿$(document).ready(function () {
  
   
});





$("#formV1rmu").on("change", function () {
    var val = $(this).find(":selected").text();
    val = val.split("-").length > 0 ? val.split("-")[1] : val;
    $("#formV1rmuDesc").val(val);
    var req = {};
    req.Section = '';
    req.RoadCode = '';
    req.RMU = $("#formV1rmu option:selected").text().split("-")[1];

    debugger;
    //RM_div_RMU_Sec_Master
    $.ajax({
        url: '/ERT/RMUSecRoad',
        dataType: 'JSON',
        data: req,
        type: 'Post',
        success: function (data) {
            if (data != null) {
                $("#formV1SecCode").empty();
                $("#formV1SecCode").append($("<option></option>").val("").html("Select Section Code"));
                $.each(data.section, function (index, v) {
                    $("#formV1SecCode").append($("<option></option>").val(v.value).html(v.text));
                });

                if (onloadFlag) {
                    $("#formV1SecCode").val($("#hdnSecCode").val());
                    onloadFlag = false;
                }

                $('#formV1SecCode').trigger("chosen:updated");
                $("#formV1SecCode").trigger("change");
                document.getElementById("formV1SecDesc").disabled = true;


            } else {
                document.getElementById("formV1SecDesc").disabled = false;
            }
        },
        error: function (data) {

            console.error(data);
        }
    });


});

$("#formV1rmu").trigger("change");


$("#formV1SecCode").on("change", function () {
    //var d = new Date();
    var ddldata = $(this).val();

    if (ddldata != "") {
        $.ajax({
            url: '/ERT/GetAllRoadCodeDataBySectionCode',
            dataType: 'JSON',
            data: { secCode: $("#formV1SecCode option:selected").text().split("-")[0] },
            type: 'Post',
            success: function (data) {
                if (data != null) {
                    if (data._RMAllData != undefined && data._RMAllData != null) {
                        $("#formV1SecDesc").val(data._RMAllData.secName);
                        $("#hdnformV1SecCode").val($("#formV1SecCode option:selected").text().split("-")[0]);
                        $("#formV1DivisionDesc").val(data._RMAllData.divisionCode);

                    }
                    document.getElementById("formV1DivisionDesc").disabled = true;
                } else {
                    document.getElementById("formV1DivisionDesc").disabled = false;
                }
            },
            error: function (data) {

                console.error(data);
            }
        });
    }
    else {
        $("#formV1SecDesc").val("");
        $("#formV1DivisionDesc").val("");
    }

    return false;
});


$("#formV1Crew").on("change", function () {
    var id = $("#formV1Crew option:selected").val();
    if (id != "99999999" && id != "") {
        $.ajax({
            url: '/ERT/GetUserById',
            dataType: 'JSON',
            data: { id },
            type: 'Post',
            success: function (data) {
                $("#formV1CrewName").val(data.userName);
                $("#formV1CrewName").prop("disabled", true);

                //if ($("#FDHRef_No").val() == "0" || $("#FDHRef_No").val() == "") {

                //var text = "ERT/Form D/" + $("#formDroadCode").val() + "/" + month + "/" + maxcount + "-" + year
                SetReferenceId();
                //}
            },
            error: function (data) {
                console.error(data);
            }
        });
    }
    else if (id == "99999999") {
        $("#formV1CrewName").prop("disabled", false);
        $("#formV1CrewName").val('');
    }
    else {
        $("#formV1CrewName").prop("disabled", true);
        $("#formV1CrewName").val('');
    }

    return false;
});

$("#formV1Crew").trigger("change");



function OnRoadChange(tis) {

    var ctrl = $("#ddlRoadCode");
    $('#FormV1Dtl_RoadCode').val(ctrl.val());

    if (ctrl.find("option:selected").attr("fromkm") != undefined)
        $("#FormV1Dtl_FrmCh").val(ctrl.find("option:selected").attr("fromkm"));
    else
        $("#FormV1Dtl_FrmCh").val(ctrl.find("option:selected").attr("Item1"));

    if (ctrl.find("option:selected").attr("fromm") != undefined)
        $("#FormV1Dtl_ToChDeci").val(ctrl.find("option:selected").attr("fromm"));
    else
        $("#FormV1Dtl_ToChDeci").val(ctrl.find("option:selected").attr("Item2"));

    var obj = new Object();
    obj.TypeCode = ctrl.val();
    obj.Type = "RD_Code";
    getNameByCode(obj)

}

function getNameByCode(obj) {
    $.ajax({
        url: '/InstructedWorks/GetNameByCode',
        data: obj,
        type: 'Post',
        success: function (data) {

            if (obj.Type == "RD_Code") {
                $("#FormV1Dtl_RoadName").val(data);
            }
        }
    })
}


function OnAcknowledgedChange(tis) {

    var ctrl = $(tis);
    if (ctrl.val() != null)
        $('#FormV1_UseridAck').val(ctrl.val());
    if ($('#FormV1_UseridAck').val() != "") {
        $("#FormV1_UsernameAck").val(ctrl.find("option:selected").attr("Item1"));
        $("#FormV1_DesignationAck").val(ctrl.find("option:selected").attr("Item2"));

        if ($('#FormV1_UseridAck').val() == "99999999") {
            $("#FormV1_UsernameAck").removeAttr("readonly");
            $("#FormV1_DesignationAck").removeAttr("readonly");

        } else {
            $("#FormV1_UsernameAck").attr("readonly", "true");
            $("#FormV1_DesignationAck").attr("readonly", "true");
        }
        $('#FormV1_SignAck').prop('checked', true);
    }
    else {
        $("#FormV1_UsernameAck").val('');
        $("#FormV1_DesignationAck").val('');
        $('#FormV1_SignAck').prop('checked', false);
    }
}

function OnAgreedbyChange(tis) {

    var ctrl = $(tis);
    if (ctrl.val() != null)
        $('#FormV1_UseridAgr').val(ctrl.val());
    if ($('#FormV1_UseridAgr').val() != "") {
        $("#FormV1_UsernameAgr").val(ctrl.find("option:selected").attr("Item1"));
        $("#FormV1_DesignationAgr").val(ctrl.find("option:selected").attr("Item2"));

        if ($('#FormV1_UseridAgr').val() == "99999999") {
            $("#FormV1_UsernameAgr").removeAttr("readonly");
            $("#FormV1_DesignationAgr").removeAttr("readonly");

        } else {
            $("#FormV1_UsernameAgr").attr("readonly", "true");
            $("#FormV1_DesignationAgr").attr("readonly", "true");
        }
        $('#FormV1_SignAgr').prop('checked', true);
    }
    else {
        $("#FormV1_UsernameAgr").val('');
        $("#FormV1_DesignationAgr").val('');
        $('#FormV1_SignAgr').prop('checked', false);
    }
}

function OnScheduledbyChange(tis) {

    var ctrl = $(tis);
    if (ctrl.val() != null)
        $('#FormV1_UseridSch').val(ctrl.val());
    if ($('#FormV1_UseridSch').val() != "") {
        $("#FormV1_UsernameSch").val(ctrl.find("option:selected").attr("Item1"));
        $("#FormV1_DesignationSch").val(ctrl.find("option:selected").attr("Item2"));

        if ($('#FormV1_UseridSch').val() == "99999999") {
            $("#FormV1_UsernameSch").removeAttr("readonly");
            $("#FormV1_DesignationSch").removeAttr("readonly");

        } else {
            $("#FormV1_UsernameSch").attr("readonly", "true");
            $("#FormV1_DesignationSch").attr("readonly", "true");
        }
        $('#FormV1_SignSch').prop('checked', true);
    }
    else {
        $("#FormV1_UsernameSch").val('');
        $("#FormV1_DesignationSch").val('');
        $('#FormV1_SignSch').prop('checked', false);
    }
}



function Save(GroupName, SubmitType) {

    debugger;

    $("#ddlUseridReq").removeClass("validate");
    $("#ddlUseridVer").removeClass("validate");
    $("#ddlUseridRep").removeClass("validate");
    $("#ddlRMU").removeClass("validate");

    $("#FormW1_SignReq").removeClass("validate");
    $("#FormW1_SignVer").removeClass("validate");
    $("#FormW1_SignRep").removeClass("validate");

    if (SubmitType != "") {

        $("#FormW1page .svalidate").addClass("validate");

        if (SubmitType == "Submitted") {
            $("#FormW1_Status").val("Submitted");
            $("#FormW1_SubmitSts").val(true);
            $("#ddlUseridReq").addClass("validate");
            $("#FormW1_SignReq").addClass("validate");


        }
        else if (SubmitType == "Verified") {
            $("#ddlUseridReq").addClass("validate");
            $("#ddlUseridVer").addClass("validate");
            $("#ddlRMU").addClass("validate");
            $("#ddlRMU").addClass("validate");
            $("#FormW1_IwRefNo").addClass("validate");
            $("#FormW1_SignVer").removeClass("validate");

            if ($("#hdnRecommondedValue").val() == 1 || $("#hdnRecommondedValue").val() == 2) {
                $("#ddlUseridRep").addClass("validate");
                $("#FormW1_SignRep").removeClass("validate");
            }
        }
    }
    else {
        $("#FormW1_Status").val("Saved");
    }

    if (ValidatePage('#FormW1page')) {
        InitAjaxLoading();
        $.post('/MAM/SaveFormV1', $("form").serialize(), function (data) {
            HideAjaxLoading();
            if (data == -1) {
                app.ShowErrorMessage(data.errorMessage);
            }
            else {

                if ($("#FormV1_PkRefNo").val() == "" || $("#FormV1_PkRefNo").val() == "0") {
                    $("#FormV1_PkRefNo").val(data);
                    $("#hdnPkRefNo").val(data);
                }

                if (SubmitType == "") {
                    app.ShowSuccessMessage('Saved Successfully', false);
                    $("#divFormV1Content").html(data);
                }
                else if (SubmitType == "Saved") {
                    app.ShowSuccessMessage('Saved Successfully', false);
                    location.href = "/MAM/FormV1";
                }
                else if (SubmitType == "Submitted") {
                    app.ShowSuccessMessage('Submitted Successfully', false);
                    location.href = "/MAM/FormV1";
                }
                else if (SubmitType == "Verified") {
                    process.ShowApprove(GroupName, SubmitType);
                }
            }
        });
    }



}



function SaveFormV1WorkSchedule() {

    debugger;

    if (ValidatePage('#FormW1page')) {
        InitAjaxLoading();
        $.post('/MAM/SaveFormV1WorkSchedule', $("form").serialize(), function (data) {
            HideAjaxLoading();
            if (data == -1) {
                app.ShowErrorMessage(data.errorMessage);
            }
            else {
                ClearWorkSchedule()

                //if (data != 0)
                //    $("#FormV1Dtl_PkRefNo").val(data)
                InitializeGrid();
                app.ShowSuccessMessage('Saved Successfully', false);
            }
        });
    }


}

function ClearWorkSchedule() {

    $('#ddlRoadCode').val("0");
    $('#ddlRoadCode').trigger('chosen:updated');
    $('#ddlSiteRef').val("0");
    $('#ddlSiteRef').trigger('chosen:updated');
    $("#FormV1Dtl_Remarks").val("");
    $("#FormV1Dtl_FrmCh").val("");
    $("#FormV1Dtl_FrmCh").val("");
    $("#FormV1Dtl_RoadName").val("");
    $("#WorkScheduleModal").modal("hide");
}

function DeleteV1WorkSchedule(id) {

    InitAjaxLoading();
    $.post('/MAM/DeleteFormV1WorkSchedule?id=' + id, function (data) {
        HideAjaxLoading();
        if (data == -1) {
            app.ShowErrorMessage(data.errorMessage);
        }
        else {
            InitializeGrid();
            app.ShowErrorMessage('Deleted Successfully', false);
        }
    });
}
