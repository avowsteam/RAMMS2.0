
var frmB7 = new function () {
    this.HeaderData = {};
    this.IsEdit = true;
      
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

    this.typein = function($this) {

        if ($this[0].childElementCount > 0)
            return;

        var width = ($($this).css('max-width').substr(0, 3)) - 10;

        if (isNaN(width)) {
            width = ($($this).css('width').substr(0, 3)) - 10;
        }

        var $input = $('<input>', {
            value: $($this).html().trim(),
            type: 'text',
            width: width,
            blur: function () {

                if ($($input).parent().attr("datatype") == "int") {
                    if (isNaN($($input).val())) {
                        $($input).parent().html("");
                        app.ShowErrorMessage("Please enter numeric values");
                    }
                    else {

                        $($input).parent().html($($input).val());
                    }
                }
                else {
                    $($input).parent().html($($input).val());
                }


            },
            keyup: function (e) {
                if (e.which === 13) {
                    // saveEdit(event, $input);
                    $input.blur();
                };

            }
        }).appendTo($this.empty()).focus();

        SetCaretAtEnd($input[0]);
    }

}

$(document).ready(function () {    
    $("#smartSearch").focus();//Header Grid focus    
    
    //Listener for Smart and Detail Search
    $("#FB7SrchSection").find("#smartSearch").focus();
    element = document.querySelector("#formB7AdvSearch");
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

    $('.typein').on('click', function (e) {
        var $this = $(this);
        frmB7.typein($this);
        e.preventDefault();
    });

    $('.dropdown').on('click', function (e) {
        var $this = $(this);
        showComboBox($this);
        e.preventDefault();
        e.stopPropagation();
    });



    $("#ddlYear").on("change", function () {
        getRevisionNo($("#ddlYear").val());
    });

});


function SetCaretAtEnd(elem) {
    var elemLen = elem.value.length;
    // For IE Only
    if (document.selection) {
        // Set focus
        elem.focus();
        // Use IE Ranges
        var oSel = document.selection.createRange();
        // Reset position to 0 & then set at end
        oSel.moveStart('character', -elemLen);
        oSel.moveStart('character', elemLen);
        oSel.moveEnd('character', 0);
        oSel.select();
    }
    else if (elem.selectionStart || elem.selectionStart == '0') {
        // Firefox/Chrome
        elem.selectionStart = elemLen;
        elem.selectionEnd = elemLen;
        elem.focus();
    } // if
}



function showComboBox(obj) {

    if (obj[0].childElementCount > 0)
        return;

    var selectHtml = "<select onchange='setSelected(this)'>";

    if (UnitServiceLevelObj.length > 0) {
        var selected = "";
        var selectedvalue = $(obj).parent().attr("preval");
        selectHtml = selectHtml + "<option value='0'></option>";

        $.each(UnitServiceLevelObj, function (index, v) {
            if (selectedvalue == v.value) { selected = "selected"; }
            else { selected = ""; }
            selectHtml = selectHtml + "<option value='" + v.Value + "' " + selected + " >" + v.Text + "</option>";
        });

        selectHtml = selectHtml + "</select>";
    }

    $(obj).html(selectHtml).promise().done(function () {

        $($(obj).find('select')).focus();
        $($(obj).find('select')).blur(function () {
            HideCombo(this);
        });
        var width = ($(obj).css('max-width').substr(0, 3)) - 10;
        if (isNaN(width)) {
            width = ($(obj).css('width').substr(0, 3)) - 10;
        }
        $($(obj).find('select')).css("width", width);
    });




    return false;
}


function HideCombo(obj) {

    var td = $(obj).parent();

    $(td).html($(obj).parent().attr("preval"));

    return false;
}


function setSelected(obj, event) {

    var strText = $(obj).find("option:selected").text();
    var val = $(obj).find("option:selected").val();

    if (val == -1) {
        strText = "";
        $(obj).parent().attr("preval", 0)
        $(obj).parent().attr("prevtext", "")
    } else {
        $(obj).parent().html(strText);
        $(obj).parent().attr("preval", val)
        $(obj).parent().attr("prevtext", strText)

    }

    return false;
}
