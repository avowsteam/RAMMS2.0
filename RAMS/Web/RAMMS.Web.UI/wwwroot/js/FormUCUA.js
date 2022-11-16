
function Save(GroupName, SubmitType) {
    
    if (SubmitType == "Submitted") {
        
        $("#SubmitYn").val(true);
      //  $("#ddlCrew").addClass("validate");
        
      //  $("#UserName").val(security.UserName);
    }

    
    if (ValidatePage('#headerFindDiv') ) {

        if ($("#Status").val() == "")
            $("#Status").val("Initialize");
        else if ($("#Status").val() == "Initialize")
            $("#Status").val("Saved");
       
        InitAjaxLoading();
        EnableDisableElements(false);

        $.get('/FrmUCUA/SaveFormUCUA', $("form").serialize(), function (data) {
            EnableDisableElements(true)
            HideAjaxLoading();
            if (data == -1) {
                app.ShowErrorMessage(data.errorMessage);
            }
            else {

                if (SubmitType == "") {
                    if (data.formExist) {
                        location.href = "/FrmUCUA/Add?Id=" + data.pkRefNo + "&view=0";
                        return;
                    }
                    else {
                        debugger;
                        UpdateFormAfterSave(data);
                    }

                }
                else if (SubmitType == "Saved") {
                    app.ShowSuccessMessage('Saved Successfully', false);
                    location.href = "/FrmUCUA/Index";
                }
                else if (SubmitType == "Submitted") {
                    app.ShowSuccessMessage('Submitted Successfully', false);
                    location.href = "/FrmUCUA/Index";
                }
                else if (SubmitType == "Verified") {
                    process.ShowApprove(GroupName, SubmitType);
                }
            }
        });
    }

}


function GoBack() {

    if (app.Confirm("Are you sure you want to close the form?", function (e) {
        if (e) {
            location.href = "/FrmUCUA";
        }
    }));
}
    

function EnableDisableElements(state) {

    $('#headerDiv * > select').prop('disabled', state).trigger("chosen:updated");

}


$(document).ready(function () {
    
    if ($("#FormUCUA_PkRefNo").val() == 0) {
  //      $("#saveFormUCUABtn").hide();
        $("#SubmitFormUCUABtn").hide();

    }
    
    if ($("#SubmitYn").val() == "True") {
        $("#Officeuse").prop("disabled", false);

    }
    else {
        $("#Officeuse").prop("disabled", true);
    }
  

    if ($("#hdnView").val() == 1) {
        $("#saveFormUCUABtn").hide();
        $("#SubmitFormUCUABtn").hide();
    }

    if ($("#hdnView").val() == 0 && $("#PkRefNo").val() != 0) {
        $("#FormUCUA_ReportingName").attr("readonly", true);
        $("#FormUCUA_Location").attr("readonly", true);
        $("#FormUCUA_WorkScope").attr("readonly", true);
      /*  $("#FormUCUA_DateReceived").attr("readonly", true);*/
    }

    $('.editable').on('click', function (e) {
        var $this = $(this);
        typein($this);
        e.preventDefault();
    });

    fnUnsafeActOnchage();
    fnUnsafeCondOnchage();
    
    if ($("#hdnView").val() == 1 || ($("#SubmitYn").val() == "True")) {
        
        $("#FormUCUA_ReportingName").attr("readonly", true);
        $("#FormUCUA_Location").attr("readonly", true);
        $("#FormUCUA_WorkScope").attr("readonly", true);
        $("#UnsafeAct").attr("readonly", true);
        $("#hdnUnsafeAct").attr("readonly", true);
        $("#FormUCUA_UnsafeActDescription").attr("readonly", true);
        $("#hdnUnsafeCondition").attr("readonly", true);
        $("#UnsafeCondition").attr("readonly", true);
        $("#FormUCUA_UnsafeConditionDescription").attr("readonly", true);
        $("#FormUCUA_ImprovementRecommendation").attr("readonly", true);
        //$("#FormUCUA_DateReceived").attr("readonly", true);
        //$("#FormUCUA_DateCommitteeReview").attr("readonly", true);
        //$("#FormUCUA_CommentsOfficeUse").attr("readonly", true);
        //$("#FormUCUA_HseSection").attr("readonly", true);
        //$("#FormUCUA_SafteyCommitteeChairman").attr("readonly", true);
        //$("#FormUCUA_ImsRep").attr("readonly", true);
        //$("#FormUCUA_DateActionTaken").attr("readonly", true);
        //$("#FormUCUA_ActionTakenBy").attr("readonly", true);
        //$("#FormUCUA_ActionDescription").attr("readonly", true);
        //$("#FormUCUA_DateEffectivenessActionTaken").attr("readonly", true);
        //$("#FormUCUA_EffectivenessActionTakenBy").attr("readonly", true);
        //$("#FormUCUA_WorkScope").attr("readonly", true);
        //$("#FormUCUA_EffectivenessActionDescription").attr("readonly", true);

       // $("FormUCUA_ReportingName").attr("readonly", true);
        //$("FormUCUA_Location").attr("readonly", true);
        //$("FormUCUA_UnsafeAct").attr("readonly", true);
        //$("FormUCUA_UnsafeActDescription").attr("readonly", true);
        //$("FormUCUA_hdnUnsafeCondition").attr("readonly", true);
        //$("FormUCUA_UnsafeConditionDescription").attr("readonly", true);
        //$("FormUCUA_ImprovementRecommendation").attr("readonly", true);
        //$("FormUCUA_DateReceived").attr("readonly", true);
        //$("FormUCUA_DateCommitteeReview").attr("readonly", true);
        //$("FormUCUA_CommentsOfficeUse").attr("readonly", true);
        //$("FormUCUA_HseSection").attr("readonly", true);
        //$("FormUCUA_SafteyCommitteeChairman").attr("readonly", true);
        //$("FormUCUA_ImsRep").attr("readonly", true);
        //$("FormUCUA_DateActionTaken").attr("readonly", true);
        //$("FormUCUA_ActionTakenBy").attr("readonly", true);
        //$("FormUCUA_ActionDescription").attr("readonly", true);
        //$("FormUCUA_DateEffectivenessActionTaken").attr("readonly", true);
        //$("FormUCUA_EffectivenessActionTakenBy").attr("readonly", true);
        //$("FormUCUA_WorkScope").attr("readonly", true);
        //$("#FormUCUA_EffectivenessActionDescription").attr("readonly", true);
        
    }
    else {

        $("FormUCUA_ReportingName").attr("readonly", false);
        $("FormUCUA_Location").attr("readonly", false);
        $("FormUCUA_UnsafeAct").attr("readonly", false);
        $("FormUCUA_UnsafeActDescription").attr("readonly", false);
        $("FormUCUA_hdnUnsafeCondition").attr("readonly", false);
        $("FormUCUA_UnsafeConditionDescription").attr("readonly", false);
        $("FormUCUA_ImprovementRecommendation").attr("readonly", false);
        //$("FormUCUA_DateReceived").attr("readonly", false);
        //$("FormUCUA_DateCommitteeReview").attr("readonly", false);
        //$("FormUCUA_CommentsOfficeUse").attr("readonly", false);
        //$("FormUCUA_HseSection").attr("readonly", false);
        //$("FormUCUA_SafteyCommitteeChairman").attr("readonly", false);
        //$("FormUCUA_ImsRep").attr("readonly", false);
        //$("FormUCUA_DateActionTaken").attr("readonly", false);
        //$("FormUCUA_ActionTakenBy").attr("readonly", false);
        //$("FormUCUA_ActionDescription").attr("readonly", false);
        //$("FormUCUA_DateEffectivenessActionTaken").attr("readonly", false);
        //$("FormUCUA_EffectivenessActionTakenBy").attr("readonly", false);
        //$("FormUCUA_WorkScope").attr("readonly", false);
        //$("#FormUCUA_EffectivenessActionDescription").attr("readonly", false);

    }

});

function fnUnsafeActOnchage() {
    
    var UnSafeActval = $("#UnsafeAct").prop("checked");
    $("#hdnUnsafeAct").val(UnSafeActval);
   
}

function fnUnsafeCondOnchage() {
    
    var UnSafeCondval = $("#UnsafeCondition").prop("checked");
    $("#hdnUnsafeCondition").val(UnSafeCondval);
}

function UpdateFormAfterSave(data) {
    
    $("#pkRefNo").val(data.pkRefNo);
    $("#refId").val(data.refId);
    $("#Status").val(data.status)

    $("#hdnPkRefNo").val(data.pkRefNo);

   // $("#btnDtlModal").show();
    $("#saveFormUCUABtn").show();
    $("#SubmitFormUCUABtn").show();


   // InitializeGrid();
}