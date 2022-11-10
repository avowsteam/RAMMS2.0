
function Save(GroupName, SubmitType) {
    debugger;

    if (SubmitType == "Submitted") {
        $("#FormT_SubmitSts").val(true);
        $("#ddlCrew").addClass("validate");
    }

    if (ValidatePage('#headerFindDiv')) {

        if ($("#FormT_Status").val() == "")
            $("#FormT_Status").val("Initialize");
        else if ($("#FormT_Status").val() == "Initialize")
            $("#FormT_Status").val("Saved");

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
function EnableDisableElements(state) {

    $('#headerDiv * > select').prop('disabled', state).trigger("chosen:updated");

}