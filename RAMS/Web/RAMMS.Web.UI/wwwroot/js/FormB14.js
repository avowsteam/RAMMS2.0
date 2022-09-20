
var frmB14 = new function () {
    this.HeaderData = {};
    this.IsEdit = true;
    this.IsAdd = false;
    this.FindDetails = function () {
        if (ValidatePage("#divFindDetailsB14")) {
            //debugger;
            //var tis = this;
            //GetResponseValue("FindDetails", "FormB14", FormValueCollection("#divFindDetailsB14"), function (data) {
            //    if (data) {
            //        $("[finddetailhide]").hide();
            //        if (data.SubmitSts) {
            //            window.location = _APPLocation + "FormB14/View/" + data.PkRefNo;
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
        window.location = _APPLocation + "FormB14";
    }

    this.Cancel = function () {
        jsMaster.ConfirmCancel(() => { frmB14.NavToList(); });
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
        var action = isSubmit ? "/FormB14/Submit" : "/FormB14/SaveFormB14";
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

        var FormB14HDR = new Object();
        var FormB14 = new Object();

        FormB14HDR.PkRefNo = $("#pkRefNo").val();
        FormB14HDR.PkRefId = $('#txtFormB14RefNum').val();
        FormB14HDR.RmuCode = $("#formB14RMU").val();
        FormB14HDR.RmuName = $('#formB14RMU').find("option:selected").text();
        FormB14HDR.RevisionYear = $("#formB14Year").val();
        FormB14HDR.RevisionDate = $("#date").val();
        FormB14HDR.RevisionNo = $("#RevisionNo").val();
        FormB14HDR.CrBy = $("#UserId").val();
        FormB14HDR.CrDt = $("#date").val();

        var B14History = []
        var i = 0;
        $('#tblLabour > tbody  > tr').each(function (index, tr) {
            debugger;
            var B14 = new Object();

            B14.PkRefNoHistory = frmB14.HeaderData.RmB14History.length == 0 ? 0 : frmB14.HeaderData.RmB14History[i].PkRefNoHistory;
            B14.B14hPkRefNo = $("#pkRefNo").val();
            B14.Feature = $('#spx' + i).text().trim();
            B14.ActCode = $(this).find("td.x01").text().trim();
            B14.ActName = $(this).find("td.x02").text().trim();
            B14.Jan = $('#txt' + i + '1').val();
            B14.Feb = $('#txt' + i + '2').val();
            B14.Mar = $('#txt' + i + '3').val();
            B14.Apr = $('#txt' + i + '4').val();
            B14.May = $('#txt' + i + '5').val();
            B14.Jun = $('#txt' + i + '6').val();
            B14.Jul = $('#txt' + i + '7').val();
            B14.Aug = $('#txt' + i + '8').val();
            B14.Sep = $('#txt' + i + '9').val();
            B14.Oct = $('#txt' + i + '10').val();
            B14.Nov = $('#txt' + i + '11').val();
            B14.Dec = $('#txt' + i + '12').val();
            B14.SubTotal = $('#txt' + i + 'SubTotal').val();
            B14.UnitOfService = $('#txt' + i + 'Unit').val();
            B14.Order = i;
            B14History.push(B14);
            i = i + 1;
        });

        FormB14 = B14History;
        var FormB14Data = JSON.stringify(FormB14);
        var FormB14HDRData = JSON.stringify(FormB14HDR);


        $.ajax({
            url: action,
            data: { formb14hdrdata: FormB14HDRData, formb14data: FormB14Data, reload: IsReload },
            type: 'POST',
            success: function (data) {
                HideAjaxLoading();
                if (data == -1) {
                    app.ShowErrorMessage(data.errorMessage);
                }
                else {
                    if (IsReload == 0) {
                        app.ShowSuccessMessage('Saved Successfully', false);
                        location.href = "/FormB14";
                    }
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

            if (data.Status != "Approved" && tblFB14HGrid.Base.IsModify) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB14.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='edit-icon'></span> Edit </button>";
            }
            if (tblFB14HGrid.Base.IsView) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB14.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='view-icon'></span> View </button>";
            }
            if (tblFB14HGrid.Base.IsDelete) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB14.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='del-icon'></span> Delete </button>";
            }
            actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB14.HeaderGrid.ActionClick(this);'>";
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
                var data = tblFB14HGrid.dataTable.row(rowidx).data();
                switch (type.toLowerCase()) {
                    case "edit":
                        window.location = _APPLocation + "FormB14/Edit/" + data.RefNo;
                        break;
                    case "view":
                        window.location = _APPLocation + "FormB14/View/" + data.RefNo;
                        break;
                    case "delete":
                        app.Confirm("Are you sure you want to delete this record? <br/>(Ref: " + data.RefNo + ")", (status) => {
                            if (status) {
                                DeleteRequest("Delete/" + data.RefNo, "FormB14", {}, function (sdata) {
                                    if (sdata.id == "-1") {
                                        app.ShowErrorMessage("Form B14 cannot be deleted, first delete Form F3");
                                        return false;
                                    }
                                    tblFB14HGrid.Refresh();
                                    app.ShowSuccessMessage("Deleted Sucessfully! <br/>(Ref: " + data.RefNo + ")");
                                });
                            }
                        }, "Yes", "No");
                        break;
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
            //if (this.HeaderData.FormB14Header && this.HeaderData.FormB14Header.PkRefNo && this.HeaderData.FormB14Header.PkRefNo > 0) {
            if ($("#pkRefNo").val() != "" && $("#pkRefNo").val() > 0) {
                $("#formB14RMU").prop("disabled", true).trigger("chosen:updated");
                $("#formB14Year").prop("disabled", true).trigger("chosen:updated");
                if (this.HeaderData.FormB14Header != undefined)
                    AppendData($("#pkRefNo").val(), this.HeaderData.FormB14Header.Status);
                else
                    AppendData($("#pkRefNo").val(), this.HeaderData.Status);
                $("[finddetailsdep]").show();
                $("#btnFindDetails").hide();
            }
            else {
                $('#formB14RMU').attr("disabled", false).trigger("chosen:updated");
                $("#formB14Year").prop("disabled", false).trigger("chosen:updated");
                $("[finddetailsdep]").hide();
                $("#btnFindDetails").show();
            }
        }
        else {
            $("#formB14RMU").prop("disabled", true).trigger("chosen:updated");
            $("#formB14Year").prop("disabled", true).trigger("chosen:updated");
            $("[finddetailsdep]").hide();
            $("#btnFindDetails").hide();
            ViewData($("#pkRefNo").val());
        }
    }
}

$(document).ready(function () {
    $("#smartSearch").focus();//Header Grid focus    

    frmB14.PageInit();

    $("[useridChange]").on("change", function () {
        frmB14.UserIdChange(this);
    });

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
        getRevisionNo($("#formB14Year").val(), $("#formB14RMU").val());
    });

});

function Type(obj, e) {
    if (frmB14.IsEdit) {
        var $this = $(obj);
        frmB14.typein($this);
    }
    e.preventDefault();
}
function DDL(obj, e) {
    if (frmB14.IsEdit) {
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

function enter($input, e) {
    if (e.which === 13) {
        $input.blur();
    };
}

function GetfindDetails() {
    debugger;
    InitAjaxLoading();
    var FormB14 = new Object();

    FormB14.RmuCode = $("#formB14RMU").val();
    FormB14.RmuName = $('#formB14RMU').find("option:selected").text();
    FormB14.RevisionYear = $("#formB14Year").val();
    FormB14.RevisionDate = $("#date").val();
    FormB14.RevisionNo = $("#RevisionNo").val();
    var FormB14Data = JSON.stringify(FormB14);
    $.ajax({
        url: '/FormB14/FindDetails',
        type: 'POST',
        data: { formb14data: FormB14Data },
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
                //    window.location = _APPLocation + "FormB14/View/" + data.PkRefNo;
                //}
                frmB14.HeaderData = data;
                $('#txtFormB14RefNum').val(data.PkRefId);
                $("#pkRefNo").val(frmB14.HeaderData.PkRefNo);
                frmB14.PageInit();

            }
        }
    });
}

function AppendData(id, Status) {
    debugger;
    var req = {};
    req.HistoryID = id;
    $.ajax({
        url: '/FormB14/GetHistoryData',
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
                    var UnitOfService = data.result[i].unitOfService == null ? "" : data.result[i].unitOfService;

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
                        $(this).find("td:last").after('<td> <input type="text" style="width:100px;" id="txt' + i + 'SubTotal" class="form-control" disabled value="' + subTotal + '" /></td>');
                        $(this).find("td:last").after('<td> <input type="text" style="width:140px;" id="txt' + i + 'Unit" class="form-control" disabled value="' + UnitOfService + '" /></td>');
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
                        $(this).find("td:last").after('<td> <input type="text" style="width:140px;" id="txt' + i + 'Unit" class="form-control" disabled value="' + UnitOfService + '" /></td>');
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
                    $(this).find("td:last").after('<td> <input type="text" style="width:140px;" id="txt' + i + 'Unit" class="form-control" disabled /></td>');
                    i = i + 1;
                });
            }
            if (Status != "Verified" || Status != "Agreed" || Status != "Approved") {
                AppendPlannedData();
                AppendUnitData();
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
        url: '/FormB14/GetHistoryData',
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
                    var total = data.result[i].subTotal == null ? "" : data.result[i].subTotal;
                    var UnitOfService = data.result[i].unitOfService == null ? "" : data.result[i].unitOfService;

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
                    $(this).find("td:last").after('<td> <input type="text" style="width:100px;" id="txt' + i + 'SubTotal" class="form-control" disabled value="' + total + '" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:140px;" id="txt' + i + 'Unit" class="form-control" disabled value="' + UnitOfService + '" /></td>');
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
    var rmucode = $("#formB14RMU").val();
    var year = $("#formB14Year").val();
    req.RmuCode = rmucode;
    req.Year = year;
    $.ajax({
        url: '/FormB14/GetPlannedBudgetData',
        dataType: 'JSON',
        data: req,
        type: 'Post',
        success: function (data) {
            if (data.result.length > 0) {
                for (var i = 0; i < data.result.length; i++) {
                    $('#spx' + i).text(data.result[i].feature);
                    $('#txt' + i + "SubTotal").val(data.result[i].slAnnualWorkQuantity);
                }
            }
        },
        error: function (data) {
            console.error(data);
        }
    });
}


function AppendUnitData() {
    debugger;
    var req = {};
    var year = $("#formB14Year").val();
    req.Year = year;
    $.ajax({
        url: '/FormB14/GetUnitData',
        dataType: 'JSON',
        data: req,
        type: 'Post',
        success: function (data) {
            if (data.result.length > 0) {
                debugger;
                for (var i = 0; i < data.result.length; i++) {
                    $('#txt' + i + "Unit").val(data.result[i].adpUnit);
                }
            }
        },
        error: function (data) {
            console.error(data);
        }
    });
}

