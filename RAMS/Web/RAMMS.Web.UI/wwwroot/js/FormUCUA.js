
function Save(GroupName, SubmitType) {
    
    if (SubmitType == "Submitted") {
        $("#SubmitYn").val(true);
      //  $("#ddlCrew").addClass("validate");
    }

    if (ValidatePage('#headerFindDiv')) {

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

   
    if ($("#hdnView").val() == 1) {
        $("#saveFormUCUABtn").hide();
        $("#SubmitFormUCUABtn").hide();
    }


    $('.editable').on('click', function (e) {
        var $this = $(this);
        typein($this);
        e.preventDefault();
    });

    fnUnsafeActOnchage();
    fnUnsafeCondOnchage();

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