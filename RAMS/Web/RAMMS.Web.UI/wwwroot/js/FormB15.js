
var frmB15 = new function () {
    this.HeaderData = {};
    this.IsEdit = true;
    this.IsAdd = false;
    this.FindDetails = function () {
        if (ValidatePage("#divFindDetailsB15")) {
            //debugger;
            //var tis = this;
            //GetResponseValue("FindDetails", "FormB15", FormValueCollection("#divFindDetailsB15"), function (data) {
            //    if (data) {
            //        $("[finddetailhide]").hide();
            //        if (data.SubmitSts) {
            //            window.location = _APPLocation + "FormB15/View/" + data.PkRefNo;
            //        }
            //        $('#txtFormMRefNum').val(data.RefId)
            //        tis.HeaderData = data;
            //        //tis.PageInit();
            //    }
            //}, "Finding");
            GetfindDetails();
        }
    }

    this.NavToList = function () {
        window.location = _APPLocation + "FormB15";
    }

    this.Cancel = function () {
        jsMaster.ConfirmCancel(() => { frmB15.NavToList(); });
    }

    this.UserIdChange = function (tis) {
        debugger;
        var sel = $(tis);
        var opt = sel.find(":selected");
        var par = sel.closest("[userIdGroup]");
        var item1 = opt.attr("item1") ? opt.attr("item1") : "";
        if (item1 == "others") {
            par.find("[userName]").val("").addClass("validate").prop("disabled", false);
            par.find("[userDest]").val("").addClass("validate").prop("disabled", false);
            //par.find("[userOffice]").val("").addClass("validate").prop("disabled", false);
        }
        else {
            var item2 = opt.attr("Item2") ? opt.attr("item2") : "";
            var item3 = opt.attr("Item3") ? opt.attr("item3") : "";
            par.find("[userName]").val(item1).removeClass("validate").prop("disabled", true);
            par.find("[userDest]").val(item2).removeClass("validate").prop("disabled", true);
            //par.find("[userOffice]").val(item3).addClass("validate").prop("disabled", true);
        }
    }

    this.Save = function (isSubmit, IsReload) {
        InitAjaxLoading();
        var failed = false;
        var action = isSubmit ? "/FormB15/Submit" : "/FormB15/SaveFormB15";
        var isvalid = true;
        if (IsReload == 0) {
            var j = 0;
            $('#tblLabour tbody tr').each(function () {
                if ($('#txt' + j + "SubTotal").val() == "");
                {
                    isvalid = false;
                    return false;
                }
                j = j + 1;
            });
        }
        debugger
        if (!isvalid) {
            app.ShowErrorMessage("SubTotal values are empty");
        }

        var FormB15HDR = new Object();
        var FormB15 = new Object();

        FormB15HDR.PkRefNo = $("#pkRefNo").val();
        FormB15HDR.PkRefId = $('#txtFormB15RefNum').val();
        FormB15HDR.RmuCode = $("#formB15RMU").val();
        FormB15HDR.RmuName = $('#formB15RMU').find("option:selected").text();
        FormB15HDR.RevisionYear = $("#formB15Year").val();
        FormB15HDR.RevisionDate = $("#date").val();
        FormB15HDR.RevisionNo = $("#RevisionNo").val();
        FormB15HDR.CrBy = $("#UserId").val();
        FormB15HDR.CrDt = $("#date").val();

        var B15History = []
        var i = 0;
        $('#tblLabour > tbody  > tr').each(function (index, tr) {
            debugger;
            var B15 = new Object();

            B15.PkRefNoHistory = frmB15.HeaderData.RmB15History.length == 0 ? 0 : frmB15.HeaderData.RmB15History[i].PkRefNoHistory;
            B15.B15hPkRefNo = $("#pkRefNo").val();
            B15.Feature = $('#spx' + i).text().trim();
            B15.ActCode = $(this).find("td.x01").text().trim();
            B15.ActName = $(this).find("td.x02").text().trim();
            B15.Jan = $('#txt' + i + '1').val();
            B15.Feb = $('#txt' + i + '2').val();
            B15.Mar = $('#txt' + i + '3').val();
            B15.Apr = $('#txt' + i + '4').val();
            B15.May = $('#txt' + i + '5').val();
            B15.Jun = $('#txt' + i + '6').val();
            B15.Jul = $('#txt' + i + '7').val();
            B15.Aug = $('#txt' + i + '8').val();
            B15.Sep = $('#txt' + i + '9').val();
            B15.Oct = $('#txt' + i + '10').val();
            B15.Nov = $('#txt' + i + '11').val();
            B15.Dec = $('#txt' + i + '12').val();
            B15.SubTotal = $('#txt' + i + 'SubTotal').val();
            B15.Remarks = $('#txt' + i + 'Remarks').val();
            B15.Order = i;
            B15History.push(B15);
            i = i + 1;
        });

        FormB15 = B15History;
        var FormB15Data = JSON.stringify(FormB15);
        var FormB15HDRData = JSON.stringify(FormB15HDR);


        $.ajax({
            url: action,
            data: { formb15hdrdata: FormB15HDRData, formb15data: FormB15Data, reload: IsReload },
            type: 'POST',
            success: function (data) {
                HideAjaxLoading();
                if (data == -1) {
                    app.ShowErrorMessage(data.errorMessage);
                }
                else {
                    if (IsReload == 0) {
                        app.ShowSuccessMessage('Saved Successfully', false);
                        location.href = "/FormB15";
                    }
                }
            }
        });

    }

    this.NavToList = function () {
        window.location = _APPLocation + "FormB15";
    }

    this.Cancel = function () {
        jsMaster.ConfirmCancel(() => { frmB15.NavToList(); });
    }

    this.HeaderGrid = new function () {
        this.ActionRender = function (data, type, row, meta) {
            var actionSection = "<div class='btn-group dropright' rowidx='" + meta.row + "'><button type='button' class='btn btn-sm btn-themebtn dropdown-toggle' data-toggle='dropdown'> Click Me </button>";
            actionSection += "<div class='dropdown-menu'>";//dorpdown menu start

            if (data.Status != "Approved" && tblFB15HGrid.Base.IsModify) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB15.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='edit-icon'></span> Edit </button>";
            }
            if (tblFB15HGrid.Base.IsView) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB15.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='view-icon'></span> View </button>";
            }
            if (tblFB15HGrid.Base.IsDelete) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB15.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='del-icon'></span> Delete </button>";
            }
            actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB15.HeaderGrid.ActionClick(this);'>";
            actionSection += "<span class='print-icon'></span> Print </button>";

            actionSection += "</div>"; //dorpdown menu close
            actionSection += "</div>"; // action close

            return actionSection;
        }
        this.ActionClick = function (tis) {
            debugger;
            var obj = $(tis);
            var type = $.trim(obj.text());
            var rowidx = parseInt(obj.closest("[rowidx]").attr("rowidx"), 10);
            if (rowidx >= 0) {
                var data = tblFB15HGrid.dataTable.row(rowidx).data();
                switch (type.toLowerCase()) {
                    case "edit":
                        window.location = _APPLocation + "FormB15/Edit/" + data.RefNo;
                        break;
                    case "view":
                        window.location = _APPLocation + "FormB15/View/" + data.RefNo;
                        break;
                    case "delete":
                        app.Confirm("Are you sure you want to delete this record? <br/>(Ref: " + data.RefNo + ")", (status) => {
                            if (status) {
                                DeleteRequest("Delete/" + data.RefNo, "FormB15", {}, function (sdata) {
                                    if (sdata.id == "-1") {
                                        app.ShowErrorMessage("Form B15 cannot be deleted, first delete Form F3");
                                        return false;
                                    }
                                    tblFB15HGrid.Refresh();
                                    app.ShowSuccessMessage("Deleted Sucessfully! <br/>(Ref: " + data.RefNo + ")");
                                });
                            }
                        }, "Yes", "No");
                        break;
                    case "print":
                        window.location = _APPLocation + "FormB15/download?id=" + data.RefNo;
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

    this.typein = function ($this) {

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

    this.PageInit = function () {
        debugger;
        if (this.IsEdit) {
            //if (this.HeaderData.FormB15Header && this.HeaderData.FormB15Header.PkRefNo && this.HeaderData.FormB15Header.PkRefNo > 0) {
            if ($("#pkRefNo").val() != "" && $("#pkRefNo").val() > 0) {
                $("#formB15RMU").prop("disabled", true).trigger("chosen:updated");
                $("#formB15Year").prop("disabled", true).trigger("chosen:updated");
                if (this.HeaderData.FormB15Header != undefined)
                    AppendData($("#pkRefNo").val(), this.HeaderData.FormB15Header.Status);
                else
                    AppendData($("#pkRefNo").val(), this.HeaderData.Status);
                $("[finddetailsdep]").show();
                $("#btnFindDetails").hide();
            }
            else {
                $('#formB15RMU').attr("disabled", false).trigger("chosen:updated");
                $("#formB15Year").prop("disabled", false).trigger("chosen:updated");
                $("[finddetailsdep]").hide();
                $("#btnFindDetails").show();
            }
        }
        else {
            $("#formB15RMU").prop("disabled", true).trigger("chosen:updated");
            $("#formB15Year").prop("disabled", true).trigger("chosen:updated");
            $("[finddetailsdep]").hide();
            $("#btnFindDetails").hide();
            ViewData($("#pkRefNo").val());
        }
    }
}

$(document).ready(function () {
    debugger;
    $("#smartSearch").focus();//Header Grid focus    

    frmB15.PageInit();

    $("[useridChange]").on("change", function () {
        frmB15.UserIdChange(this);
    });

    if (!frmB15.IsEdit) {
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
        if (frmB15.IsEdit) {
            var $this = $(this);
            frmB15.typein($this);
        }
        e.preventDefault();
    });

    $('.dropdown').on('click', function (e) {
        if (frmB15.IsEdit) {
            var $this = $(this);
            showComboBox($this);
        }
        e.preventDefault();
        e.stopPropagation();
    });

    $("#formB15Year").on("change", function () {
        getRevisionNo($("#formB15Year").val(), $("#formB15RMU").val());
    });

});

function Type(obj, e) {
    if (frmB15.IsEdit) {
        var $this = $(obj);
        frmB15.typein($this);
    }
    e.preventDefault();
}
function DDL(obj, e) {
    if (frmB15.IsEdit) {
        var $this = $(obj);
        showComboBox($this);
    }
    e.preventDefault();
    e.stopPropagation();
}

function getRevisionNo(id, rmuCode) {
    var req = {};
    req.Year = id;
    req.RmuCode = rmuCode;
    $.ajax({
        url: '/FormB15/GetMaxRev',
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

function enter($input, e) {
    if (e.which === 13) {
        $input.blur();
    };
}

function GetfindDetails() {
    debugger;
    InitAjaxLoading();
    var FormB15 = new Object();

    FormB15.RmuCode = $("#formB15RMU").val();
    FormB15.RmuName = $('#formB15RMU').find("option:selected").text();
    FormB15.RevisionYear = $("#formB15Year").val();
    FormB15.RevisionDate = $("#date").val();
    FormB15.RevisionNo = $("#RevisionNo").val();
    var FormB15Data = JSON.stringify(FormB15);
    $.ajax({
        url: '/FormB15/FindDetails',
        type: 'POST',
        data: { formb15data: FormB15Data },
        dataType: "json",
        success: function (data) {
            debugger;
            HideAjaxLoading();
            if (data == -1) {
                app.ShowErrorMessage(data.errorMessage);
            }
            else {
                $("[finddetailhide]").hide();
                //if (data.SubmitSts) {
                //    window.location = _APPLocation + "FormB15/View/" + data.PkRefNo;
                //}
                frmB15.HeaderData = data;
                $('#txtFormB15RefNum').val(data.PkRefId);
                $("#pkRefNo").val(frmB15.HeaderData.PkRefNo);
                frmB15.PageInit();

            }
        }
    });
}

function AppendData(id, Status) {
    debugger;
    var req = {};
    req.HistoryID = id;
    $.ajax({
        url: '/FormB15/GetHistoryData',
        type: 'POST',
        data: req,
        dataType: "json",
        success: function (data) {
            if (data.result.length > 0) {
                var i = 0;
                $('#tblLabour tbody tr').each(function () {
                    debugger;
                    var jan = data.result[i].jan == null ? "" : data.result[i].jan;
                    var feb = data.result[i].feb == null ? "" : data.result[i].feb;
                    var mar = data.result[i].mar == null ? "" : data.result[i].mar;
                    var apr = data.result[i].apr == null ? "" : data.result[i].apr;
                    var may = data.result[i].may == null ? "" : data.result[i].may;
                    var jun = data.result[i].jun == null ? "" : data.result[i].jun;
                    var jul = data.result[i].jul == null ? "" : data.result[i].jul;
                    var aug = data.result[i].aug == null ? "" : data.result[i].aug;
                    var sep = data.result[i].sep == null ? "" : data.result[i].sep;
                    var oct = data.result[i].oct == null ? "" : data.result[i].oct;
                    var nov = data.result[i].nov == null ? "" : data.result[i].nov;
                    var dec = data.result[i].dec == null ? "" : data.result[i].dec;
                    var subTotal = data.result[i].subTotal == null ? "" : data.result[i].subTotal;
                    var remarks = data.result[i].remarks == null ? "" : data.result[i].remarks;


                    if (Status == "Agreed" || Status == "Approved") {
                        $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '1" onkeyup="LabourCal(this)" value="' + jan + '" class="form-control" disabled  /></td>');
                        $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '2" onkeyup="LabourCal(this)" value="' + feb + '" class="form-control" disabled /></td>');
                        $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '3" onkeyup="LabourCal(this)" value="' + mar + '" class="form-control" disabled /></td>');
                        $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '4" onkeyup="LabourCal(this)" value="' + apr + '" class="form-control" disabled /></td>');
                        $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '5" onkeyup="LabourCal(this)" value="' + may + '" class="form-control" disabled /></td>');
                        $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '6" onkeyup="LabourCal(this)" value="' + jun + '" class="form-control" disabled /></td>');
                        $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '7" onkeyup="LabourCal(this)" value="' + jul + '" class="form-control" disabled /></td>');
                        $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '8" onkeyup="LabourCal(this)" value="' + aug + '" class="form-control" disabled /></td>');
                        $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '9" onkeyup="LabourCal(this)" value="' + sep + '" class="form-control" disabled /></td>');
                        $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '10" onkeyup="LabourCal(this)" value="' + oct + '" class="form-control" disabled /></td>');
                        $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '11" onkeyup="LabourCal(this)" value="' + nov + '" class="form-control" disabled /></td>');
                        $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '12" onkeyup="LabourCal(this)" value="' + dec + '" class="form-control" disabled /></td>');
                        $(this).find("td:last").after('<td> <input type="text" style="width:100px;" id="txt' + i + 'SubTotal" class="form-control" disabled value="' + subTotal + '"/></td>');
                        $(this).find("td:last").after('<td> <input type="text" style="width:150px;" id="txt' + i + 'Remarks" class="form-control" value="' + remarks + '"/></td>');
                        i = i + 1;
                    }
                    else {
                        $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '1" onkeyup="AddLabourCal(this,' + i + ')" value="' + jan + '" class="form-control" /></td>');
                        $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '2" onkeyup="AddLabourCal(this,' + i + ')" value="' + feb + '" class="form-control" /></td>');
                        $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '3" onkeyup="AddLabourCal(this,' + i + ')" value="' + mar + '" class="form-control" /></td>');
                        $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '4" onkeyup="AddLabourCal(this,' + i + ')" value="' + apr + '" class="form-control" /></td>');
                        $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '5" onkeyup="AddLabourCal(this,' + i + ')" value="' + may + '" class="form-control" /></td>');
                        $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '6" onkeyup="AddLabourCal(this,' + i + ')" value="' + jun + '" class="form-control" /></td>');
                        $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '7" onkeyup="AddLabourCal(this,' + i + ')" value="' + jul + '" class="form-control" /></td>');
                        $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '8" onkeyup="AddLabourCal(this,' + i + ')" value="' + aug + '" class="form-control" /></td>');
                        $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '9" onkeyup="AddLabourCal(this,' + i + ')" value="' + sep + '" class="form-control" /></td>');
                        $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '10" onkeyup="AddLabourCal(this,' + i + ')" value="' + oct + '" class="form-control" /></td>');
                        $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '11" onkeyup="AddLabourCal(this,' + i + ')" value="' + nov + '" class="form-control" /></td>');
                        $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '12" onkeyup="AddLabourCal(this,' + i + ')" value="' + dec + '" class="form-control" /></td>');
                        $(this).find("td:last").after('<td> <input type="text" style="width:100px;" id="txt' + i + 'SubTotal" class="form-control" disabled value="' + subTotal + '"/></td>');
                        $(this).find("td:last").after('<td> <input type="text" style="width:150px;" id="txt' + i + 'Remarks" class="form-control" value="' + remarks + '"/></td>');
                        i = i + 1;
                    }
                });
            }
            else {
                var k = 0;
                var i = 0;
                $('#tblLabour tbody tr').each(function () {
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '1" onkeyup="AddLabourCal(this,' + i + ')" class="form-control" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '2" onkeyup="AddLabourCal(this,' + i + ')" class="form-control" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '3" onkeyup="AddLabourCal(this,' + i + ')" class="form-control" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '4" onkeyup="AddLabourCal(this,' + i + ')" class="form-control" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '5" onkeyup="AddLabourCal(this,' + i + ')" class="form-control" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '6" onkeyup="AddLabourCal(this,' + i + ')" class="form-control" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '7" onkeyup="AddLabourCal(this,' + i + ')" class="form-control" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '8" onkeyup="AddLabourCal(this,' + i + ')" class="form-control" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '9" onkeyup="AddLabourCal(this,' + i + ')" class="form-control" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '10" onkeyup="AddLabourCal(this,' + i + ')" class="form-control" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '11" onkeyup="AddLabourCal(this,' + i + ')" class="form-control" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '12" onkeyup="AddLabourCal(this,' + i + ')" class="form-control" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:100px;" id="txt' + i + 'SubTotal" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:150px;" id="txt' + i + 'Remarks" class="form-control" /></td>');
                    i = i + 1;
                });
            }
            if (Status != "Verified" || Status != "Agreed" || Status != "Approved") {
                AppendPlannedData();
            }
            if (Status == "Agreed" || Status == "Approved") {
                $("[finddetailsdep]").hide();
            }
        }
    });


}

function ViewData(id) {
    debugger;
    var req = {};
    req.HistoryID = id;
    $.ajax({
        url: '/FormB15/GetHistoryData',
        type: 'POST',
        data: req,
        dataType: "json",
        success: function (data) {
            if (data.result.length > 0) {
                var i = 0;
                $('#tblLabour tbody tr').each(function () {
                    var jan = data.result[i].jan == null ? "" : data.result[i].jan;
                    var feb = data.result[i].feb == null ? "" : data.result[i].feb;
                    var mar = data.result[i].mar == null ? "" : data.result[i].mar;
                    var apr = data.result[i].apr == null ? "" : data.result[i].apr;
                    var may = data.result[i].may == null ? "" : data.result[i].may;
                    var jun = data.result[i].jun == null ? "" : data.result[i].jun;
                    var jul = data.result[i].jul == null ? "" : data.result[i].jul;
                    var aug = data.result[i].aug == null ? "" : data.result[i].aug;
                    var sep = data.result[i].sep == null ? "" : data.result[i].sep;
                    var oct = data.result[i].oct == null ? "" : data.result[i].oct;
                    var nov = data.result[i].nov == null ? "" : data.result[i].nov;
                    var dec = data.result[i].dec == null ? "" : data.result[i].dec;
                    var subTotal = data.result[i].subTotal == null ? "" : data.result[i].subTotal;
                    var remarks = data.result[i].remarks == null ? "" : data.result[i].remarks;

                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '1" onkeyup="LabourCal(this)" value="' + jan + '" class="form-control" disabled  /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '2" onkeyup="LabourCal(this)" value="' + feb + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '3" onkeyup="LabourCal(this)" value="' + mar + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '4" onkeyup="LabourCal(this)" value="' + apr + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '5" onkeyup="LabourCal(this)" value="' + may + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '6" onkeyup="LabourCal(this)" value="' + jun + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '7" onkeyup="LabourCal(this)" value="' + jul + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '8" onkeyup="LabourCal(this)" value="' + aug + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '9" onkeyup="LabourCal(this)" value="' + sep + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '10" onkeyup="LabourCal(this)" value="' + oct + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '11" onkeyup="LabourCal(this)" value="' + nov + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + '12" onkeyup="LabourCal(this)" value="' + dec + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:100px;" id="txt' + i + 'SubTotal" class="form-control" disabled value="' + subTotal + '" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:150px;" id="txt' + i + 'Remarks" class="form-control" value="' + remarks + '" /></td>');
                    i = i + 1;
                });
            }
        }
    });


}

function LabourCal(obj) {
    debugger;
    var Qty = 0;
    var Unit = 0;
    $('#' + obj.id).val($('#' + obj.id).val().replace(/[^\d.]/g, ''));
    Qty = $('#' + obj.id).val();
    //if (Qty != "" && Qty != null) {
    //    Unit = $('#sp' + obj.id.slice(3)).text();
    //    var tot = Qty * Unit;
    //    $('#span' + obj.id.slice(3)).text(tot.toFixed(2))
    //    var sum = 0;
    //    $("#tblLabour tbody tr td .x100" + k).each(function (index, tr) {

    //        if (!isNaN(this.innerText) && this.innerText != "") {
    //            sum += parseFloat(this.innerText);
    //        }
    //    });
    //    $('#sptot' + k).text(sum.toFixed(3));

    //    //GrandTotal
    //    $('.x' + k).text(parseFloat($('#sptot' + k).text()) + parseFloat($('#esptot' + k).text()) + parseFloat($('#msptot' + k).text())).toFixed(3);
    //}
}

function AddLabourCal(obj, ActivityID) {
    debugger;
    var Qty = 0;
    var Unit = 0;
    $('#' + obj.id).val($('#' + obj.id).val().replace(/[^\d.]/g, ''));
    Qty = $('#' + obj.id).val();

    var subTotal = $("#txt" + ActivityID + "SubTotal").val() == "" ? 0 : $("#txt" + ActivityID + "SubTotal").val();
    //var total = (parseFloat($("#txt" + ActivityID + "1").val()) + parseFloat($("#txt" + ActivityID + "2").val()) + parseFloat($("#txt" + ActivityID + "3").val()) + parseFloat($("#txt" + ActivityID + "4").val()) + parseFloat($("#txt" + ActivityID + "5").val()) + parseFloat($("#txt" + ActivityID + "6").val()) + parseFloat($("#txt" + ActivityID + "7").val()) + parseFloat($("#txt" + ActivityID + "8").val()) + parseFloat($("#txt" + ActivityID + "9").val()) + parseFloat($("#txt" + ActivityID + "10").val()) + parseFloat($("#txt" + ActivityID + "11").val()) + parseFloat($("#txt" + ActivityID + "12").val()));
    var total = $("#txt" + ActivityID + "1").val() == "" ? 0 : parseFloat($("#txt" + ActivityID + "1").val());
    total = parseFloat(total) + ($("#txt" + ActivityID + "2").val() == "" ? 0 : parseFloat($("#txt" + ActivityID + "2").val()));
    total = parseFloat(total) + ($("#txt" + ActivityID + "3").val() == "" ? 0 : parseFloat($("#txt" + ActivityID + "3").val()));
    total = parseFloat(total) + ($("#txt" + ActivityID + "4").val() == "" ? 0 : parseFloat($("#txt" + ActivityID + "4").val()));
    total = parseFloat(total) + ($("#txt" + ActivityID + "5").val() == "" ? 0 : parseFloat($("#txt" + ActivityID + "5").val()));
    total = parseFloat(total) + ($("#txt" + ActivityID + "6").val() == "" ? 0 : parseFloat($("#txt" + ActivityID + "6").val()));
    total = parseFloat(total) + ($("#txt" + ActivityID + "7").val() == "" ? 0 : parseFloat($("#txt" + ActivityID + "7").val()));
    total = parseFloat(total) + ($("#txt" + ActivityID + "8").val() == "" ? 0 : parseFloat($("#txt" + ActivityID + "8").val()));
    total = parseFloat(total) + ($("#txt" + ActivityID + "9").val() == "" ? 0 : parseFloat($("#txt" + ActivityID + "9").val()));
    total = parseFloat(total) + ($("#txt" + ActivityID + "10").val() == "" ? 0 : parseFloat($("#txt" + ActivityID + "10").val()));
    total = parseFloat(total) + ($("#txt" + ActivityID + "11").val() == "" ? 0 : parseFloat($("#txt" + ActivityID + "11").val()));
    total = parseFloat(total) + ($("#txt" + ActivityID + "12").val() == "" ? 0 : parseFloat($("#txt" + ActivityID + "12").val()));
    if (parseFloat(subTotal) < parseFloat(total)) {
        $('#' + obj.id).val("");
        app.ShowErrorMessage("Month values are not equal");
    }

    //if (Qty != "" && Qty != null) {
    //    Unit = $('#sp' + obj.id.slice(3)).text();
    //    var tot = Qty * Unit;
    //    $('#span' + obj.id.slice(3)).text(tot.toFixed(2))
    //    var sum = 0;
    //    $("#tblLabour tbody tr td .x100" + k).each(function (index, tr) {

    //        if (!isNaN(this.innerText) && this.innerText != "") {
    //            sum += parseFloat(this.innerText);
    //        }
    //    });
    //    $('#sptot' + k).text(sum.toFixed(3));

    //    //GrandTotal
    //    $('.x' + k).text(parseFloat($('#sptot' + k).text()) + parseFloat($('#esptot' + k).text()) + parseFloat($('#msptot' + k).text())).toFixed(3);
    //}
}

function AppendPlannedData() {
    debugger;
    var req = {};
    var rmucode = $("#formB15RMU").val();
    var year = $("#formB15Year").val();
    req.RmuCode = rmucode;
    req.Year = year;
    $.ajax({
        url: '/FormB15/GetPlannedBudgetData',
        dataType: 'JSON',
        data: req,
        type: 'Post',
        success: function (data) {
            if (data.result.length > 0) {
                for (var i = 0; i < data.result.length; i++) {
                    $('#spx' + i).text(data.result[i].feature);
                    $('#txt' + i + "SubTotal").val(data.result[i].slCrewDaysPlanned);
                }
            }
        },
        error: function (data) {
            console.error(data);
        }
    });
}