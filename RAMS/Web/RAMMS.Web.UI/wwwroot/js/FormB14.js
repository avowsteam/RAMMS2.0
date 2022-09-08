
var frmB14 = new function () {
    this.HeaderData = {};
    this.IsEdit = true;
      
    this.Save = function () {
        var failed = false;

        if (failed)
            return;

        //  if (ValidatePage('#myModal')) {
        InitAjaxLoading();

        var FormB14 = new Object();
        FormB14.B14hPkRefNo = $("#FormB14Header_B14hPkRefNo").val()
        FormB14.B14hRevisionYear = $("#formB14Year").val()
        FormB14.B14hRevisionNo = $("#RevisionNo").val()
        FormB14.B14hRevisionDate = $("#date").val()
        FormB14.B14hCrBy = $("#UserId").val();
        FormB14.B14hCrByName = $("#UserName").val();
        FormB14.B14hCrDt = $("#date").val()

       

        $.ajax({
            url: '/FormB14/SaveFormB14',
            data: FormB14,
            type: 'POST',
            success: function (data) {
                HideAjaxLoading();
                if (data == -1) {
                    app.ShowErrorMessage(data.errorMessage);
                }
                else {
                    app.ShowSuccessMessage('Saved Successfully', false);
                    location.href = "/FormB14";
                }
            }
        });
    }

    this.NavToList = function () {
        window.location = _APPLocation + "FormB14";
    }

    this.Cancel = function () {
        jsMaster.ConfirmCancel(() => { frmB14.NavToList(); });
    }

    this.HeaderGrid = new function () {
        this.ActionRender = function (data, type, row, meta) {
            var actionSection = "<div class='btn-group dropright' rowidx='" + meta.row + "'><button type='button' class='btn btn-sm btn-themebtn dropdown-toggle' data-toggle='dropdown'> Click Me </button>";
            actionSection += "<div class='dropdown-menu'>";//dorpdown menu start

            if (data.MaxRecord) { //if (tblFB14HGrid.Base.IsModify) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB14.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='edit-icon'></span> Edit </button>";
            }
            if (tblFB14HGrid.Base.IsView) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB14.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='view-icon'></span> View </button>";
            }
            //if (tblFB14HGrid.Base.IsDelete) {
            //    actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB14.HeaderGrid.ActionClick(this);'>";
            //    actionSection += "<span class='del-icon'></span> Delete </button>";
            //}
            actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB14.HeaderGrid.ActionClick(this);'>";
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
                var data = tblFB14HGrid.dataTable.row(rowidx).data();
                switch (type.toLowerCase()) {
                    case "edit":
                        window.location = _APPLocation + "FormB14/Edit/" + data.RefNo;
                        break;
                    case "view":
                        window.location = _APPLocation + "FormB14/View/" + data.RefNo;
                        break;
                    //case "delete":
                    //    app.Confirm("Are you sure you want to delete this record? <br/>(Ref: " + data.RefID + ")", (status) => {
                    //        if (status) {
                    //            DeleteRequest("Delete/" + data.RefNo, "FormB14", {}, function (sdata) {
                    //                if (sdata.id == "-1") {
                    //                    app.ShowErrorMessage("Form B14 cannot be deleted, first delete Form F3");
                    //                    return false;
                    //                }
                    //                tblFB14HGrid.Refresh();
                    //                app.ShowSuccessMessage("Deleted Sucessfully! <br/>(Ref: " + data.RefID + ")");
                    //            });
                    //        }
                    //    }, "Yes", "No");
                    //    break;
                    case "print":
                        window.location = _APPLocation + "FormB14/download?id=" + data.RefNo;
                        break;
                }
            }
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

    if (!frmB14.IsEdit) {
        $("#formB8Year").chosen("destroy");
        $("#divFindDetails *").attr("disabled", "disabled").off("click");
    }

    element = document.querySelector("#btnAdvSearch");
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
        if (frmB14.IsEdit) {
            var $this = $(this);
            frmB14.typein($this);
        }
        e.preventDefault();
    });

    $('.dropdown').on('click', function (e) {
        if (frmB14.IsEdit) {
            var $this = $(this);
            showComboBox($this);
        }
        e.preventDefault();
        e.stopPropagation();
    });

    $("#formB14Year").on("change", function () {
        getRevisionNo($("#formB14Year").val());
    });

});

function Type(obj,e) {
    if (frmB14.IsEdit) {
        var $this = $(obj);
        frmB14.typein($this);
    }
    e.preventDefault();
}
function DDL(obj,e) {
    if (frmB14.IsEdit) {
        var $this = $(obj);
        showComboBox($this);
    }
    e.preventDefault();
    e.stopPropagation();
}

function getRevisionNo(id) {
    var req = {};
    req.Year = id;
    $.ajax({
        url: '/FormB14/GetMaxRev',
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

function Delete(obj) {
    $("[" + obj + "]").remove();
}

function AddRow(obj) {
    var temp = obj.substring(0, 3).trim() + Math.floor(Date.now() / 1000);
    var row = '<tr ' + temp + '><td><div class="btn-group dropright" id="actiondropdown"><button id="actionclick" type="button" class="btn btn-sm btn-themebtn dropdown-toggle" data-toggle="dropdown"> Click Me </button> <div class="dropdown-menu"> <button type="button" class="dropdown-item editdel-btns" onclick="javascript:Delete(\'' + temp + '\');"><span class="del-icon"></span>Delete</button></div></div></td>'
        + '<td><input type="text"  onblur="javascript:save(this)" onkeyup="javascript:enter(this,event)"/></td>'
        + '<td><input type="text" onblur="javascript:save(this)" onkeyup="javascript:enter(this,event)"/></td>'
        + '<td ' + (obj == 'Material' ? ' class="dropdown" preval="" onclick="DDL(this,event)" ' : ' class="typein" datatype="int" onclick="Type(this,event)"') + '>' + (obj == 'Material' ? GETDLL() : ' <input type="text" onblur="javascript:save(this)" onkeyup="javascript:enter(this,event)"/>') + '</td>'
        + '<td class="typein"' + (obj == 'Material' ? '' : ' datatype="int"') + ' onclick="Type(this,event)"><input type="text"  onblur="javascript:save(this)" onkeyup="javascript:enter(this,event)"/></td>'
        + '<td class="typein" datatype="int"  onclick="Type(this,event)"><input type="text"  onblur="javascript:save(this)" onkeyup="javascript:enter(this,event)"/></td>'
        + (obj == 'Material' ? '<td class="typein"><input type="text" onblur="javascript:save(this)" onkeyup="javascript:enter(this,event)"/></td>' : "")
        + '</tr> ';

    switch (obj) {
        case 'Labour':
            $("#tblLabour>tbody").append(row)
            break;
        case 'Material':
            $("#tblMaterial>tbody").append(row)
            break;
        case 'Equipment':
            $("#tblEquipment>tbody").append(row)
            break;
        default:
            break;
    }
}

function showComboBox(obj) {

    if (obj[0].childElementCount > 0)
        return;

    var selectHtml = "<select onchange='setSelected(this)'>";

    if (UnitObj.length > 0) {
        var selected = "";
        var selectedvalue = $(obj).attr("preval");
        selectedvalue = typeof selectedvalue == "undefined" ? "" : selectedvalue
        selectHtml = selectHtml + "<option value='0'></option>";

        $.each(UnitObj, function (index, v) {
            if (selectedvalue == v.Value) { selected = "selected"; }
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


function GETDLL() {

    var selectHtml = "<select onchange='setSelected(this)' style='width:200px' onblue='HideCombo(this)'>";

    if (UnitObj.length > 0) {
        var selected = "";
        selectHtml = selectHtml + "<option value='0'></option>";

        $.each(UnitObj, function (index, v) {
            selectHtml = selectHtml + "<option value='" + v.Value + "'>" + v.Text + "</option>";
        });

        selectHtml = selectHtml + "</select>";
    }

    return selectHtml;
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

function save($input) {

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


}

function enter($input,e) {
    if (e.which === 13) {
        $input.blur();
    };
}