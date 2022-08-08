
var frmB7 = new function () {
    this.HeaderData = {};
    this.IsEdit = true;
    this.Pattern = "";
   
    this.BindData = function () {
        //debugger;
        if (this.HeaderData && this.HeaderData.PkRefNo && this.HeaderData.PkRefNo > 0) {
            if (this.IsEdit) { this.IsEdit = this.HeaderData.Status == "Approved" ? false : true; }
            if (!this.IsEdit) {
                $("#ancAddImage").remove(); $("#ImageListTRTemplate td[deleteImg]").text("");
                $("[finddetailsdep]").hide();
            }
            var tis = this;
            var assignFormat = jsMaster.AssignFormat;
            $("#divG1G2").find("input,select,textarea").filter("[name]").each(function () {
                var obj = $(this);
                var name = obj.attr("name");
                if (tis.HeaderData[name] != null) {
                    if (this.type == "select-one") { obj.val("" + tis.HeaderData[name]).trigger("change").trigger("chosen:updated"); }
                    else if (this.type == "date") { obj.val((new Date(tis.HeaderData[name])).ToString(assignFormat)); }
                    else {
                        obj.val(tis.HeaderData[name]);
                    }
                }
                else { obj.val(""); }
                if (!tis.IsEdit) {
                    obj.prop("disabled", true);
                    if (this.type == "select-one") { obj.trigger("chosen:updated"); };
                }
            });
            $("#pkRefNo").val(tis.HeaderData.PkRefNo);
            tis.LoadG2(tis.HeaderData.FormG2);
            tis.RefreshImageList();
            $("#dtInspection").attr("min", this.HeaderData.YearOfInsp + "-01-01").attr("max", this.HeaderData.YearOfInsp + "-12-31");
        }
    }
  
    
    this.Save = function (isSubmit, isApproveSave) {
        //debugger;
        var tis = this;
        if (isSubmit) {
            $("#frmB7Data .svalidate").addClass("validate");
        }
        Validation.ResetErrStyles("#frmB7Data");
        $("#txtPhotoValidate").val(this.IsUploadAllImage(isSubmit) ? "valid" : "");
        if (ValidatePage("#frmB7Data", "", "")) {
            //var refNo = $("#txtS1RefNumber");
            var action = isSubmit ? "Submit" : "Save";
            if (isApproveSave == 1) {
                GetResponseValue(action, "FormB7", FormValueCollection("#AccordPage1,#AccordPage2,#AccordPage6,#FormG2TabPage2,#divApprovedInfo", tis.HeaderData), function (data) {
                }, "Saving");
            }
            else {
                GetResponseValue(action, "FormB7", FormValueCollection("#AccordPage1,#AccordPage2,#AccordPage6,#FormG2TabPage2,#divApprovedInfo", tis.HeaderData), function (data) {
                    app.ShowSuccessMessage('Successfully Saved', false);
                    setTimeout(tis.NavToList, 2000);
                }, "Saving");
            }
        }
        if (isSubmit) {
            $("#frmB7Data .svalidate").removeClass("validate");
        }
    }
    this.NavToList = function () {
        window.location = _APPLocation + "FormB7";
    }
    this.Cancel = function () {
        jsMaster.ConfirmCancel(() => { frmB7.NavToList(); });
    }
  
    this.PageInit = function () {
        if (frmB7.HeaderData && frmB7.HeaderData.PkRefNo && frmB7.HeaderData.PkRefNo > 0) {
            $("[finddetailsdep]").show();
            $("#btnFindDetails").hide();
        }
        else {
            $("[finddetailsdep]").hide();
            $("#btnFindDetails").show();
            $("#selRMU").trigger("change");
        }
        this.BindData();
    }

    this.HeaderGrid = new function () {
        this.ActionRender = function (data, type, row, meta) {
            var actionSection = "<div class='btn-group dropright' rowidx='" + meta.row + "'><button type='button' class='btn btn-sm btn-themebtn dropdown-toggle' data-toggle='dropdown'> Click Me </button>";
            actionSection += "<div class='dropdown-menu'>";//dorpdown menu start

            if (tblFB7HGrid.Base.IsModify) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB7.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='edit-icon'></span> Edit </button>";
            }
            if (tblFB7HGrid.Base.IsView) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB7.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='view-icon'></span> View </button>";
            }
            //if (tblFB7HGrid.Base.IsDelete) {
            //    actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB7.HeaderGrid.ActionClick(this);'>";
            //    actionSection += "<span class='del-icon'></span> Delete </button>";
            //}
            actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB7.HeaderGrid.ActionClick(this);'>";
            actionSection += "<span class='print-icon'></span> Print </button>";

            actionSection += "</div>"; //dorpdown menu close
            actionSection += "</div>"; // action close

            return actionSection;
        }
        this.ActionClick = function (tis) {
            var obj = $(tis);
            var type = $.trim(obj.text());
            var rowidx = parseInt(obj.closest("[rowidx]").attr("rowidx"), 10);
            if (rowidx >= 0) {
                var data = tblFB7HGrid.dataTable.row(rowidx).data();
                switch (type.toLowerCase()) {
                    case "edit":
                        window.location = _APPLocation + "FormB7/Edit/" + data.RefNo;
                        break;
                    case "view":
                        window.location = _APPLocation + "FormB7/View/" + data.RefNo;
                        break;
                    //case "delete":
                    //    app.Confirm("Are you sure you want to delete this record? <br/>(Ref: " + data.RefID + ")", (status) => {
                    //        if (status) {
                    //            DeleteRequest("Delete/" + data.RefNo, "FormB7", {}, function (sdata) {
                    //                if (sdata.id == "-1") {
                    //                    app.ShowErrorMessage("Form G1G2 cannot be deleted, first delete Form F3");
                    //                    return false;
                    //                }
                    //                tblFB7HGrid.Refresh();
                    //                app.ShowSuccessMessage("Deleted Sucessfully! <br/>(Ref: " + data.RefID + ")");
                    //            });
                    //        }
                    //    }, "Yes", "No");
                    //    break;
                    case "print":
                        window.location = _APPLocation + "FormB7/download?id=" + data.RefNo;
                        break;
                }
            }
        }
        this.DateOfEntry = (data, type, row, meta) => {
            var result = "";
            if (row.RevisionDate && row.RevisionDate != null && row.RevisionDate != "") {
                result = (new Date(row.RevisionDate)).ToString(jsMaster.DisplayDateFormat);
                result = " (" + result + ")";
            }
            result = data + result;
            return result;
        }
        this.DateOfIns = (data, type, row, meta) => {
            var result = "";
            if (data && data != "") {
                result = (new Date(data)).ToString(jsMaster.GridFormat);
            }
            return result;
        }
    }
}

$(document).ready(function () {
    $("[useridChange]").on("change", function () {
        frmB7.UserIdChange(this);
    });
    //frmB7.InitDis_Severity();
    frmB7.PageInit();
    $("#smartSearch").focus();//Header Grid focus    
    

    //Listener for Smart and Detail Search
    $("#FG1G2SrchSection").find("#smartSearch").focus();
    element = document.querySelector("#formG1G2AdvSearch");
    if (element) {
        element.addEventListener("keyup", () => {
            if (event.keyCode === 13) {
                $('[searchsectionbtn]').trigger('onclick');
            }
        });
    }
    $("#smartSearch").keyup(function () {
        if (event.keyCode === 13) {
            $('[searchsectionbtn]').trigger('onclick');
        }
    })


    $('.allow_numeric').keypress(function (event) {
        var $this = $(this);
        if ((event.which != 46 || $this.val().indexOf('.') != -1) &&
            ((event.which < 48 || event.which > 57) &&
                (event.which != 0 && event.which != 8))) {
            event.preventDefault();
        }

        var text = $(this).val();
        if ((event.which == 46) && (text.indexOf('.') == -1)) {
            setTimeout(function () {
                if ($this.val().substring($this.val().indexOf('.')).length > 3) {
                    $this.val($this.val().substring(0, $this.val().indexOf('.') + 3));
                }
            }, 1);
        }

        if ((text.indexOf('.') != -1) &&
            (text.substring(text.indexOf('.')).length > 3) &&
            (event.which != 0 && event.which != 8) &&
            ($(this)[0].selectionStart >= text.length - 3)) {
            event.preventDefault();
        }
    });

    $('.allow_numericWOD').keypress(function (event) {
        var $this = $(this);
        if ((event.which != 46 || $this.val().indexOf('.') == -1) &&
            ((event.which < 48 || event.which > 57) &&
                (event.which != 0 && event.which != 8))) {
            event.preventDefault();
        }

        var text = $(this).val();
        if ((event.which == 46) && (text.indexOf('.') != -1)) {
            setTimeout(function () {
                if ($this.val().substring($this.val().indexOf('.')).length > 3) {
                    $this.val($this.val().substring(0, $this.val().indexOf('.') + 3));
                }
            }, 1);
        }
    });

});