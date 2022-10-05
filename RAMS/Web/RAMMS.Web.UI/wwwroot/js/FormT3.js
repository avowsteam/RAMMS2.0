
var frmT3 = new function () {
    this.HeaderData = {};
    this.IsEdit = true;
    this.IsAdd = false;
    this.FindDetails = function () {
        if (ValidatePage("#divFindDetailsT3")) {
            //debugger;
            //var tis = this;
            //GetResponseValue("FindDetails", "FormT3", FormValueCollection("#divFindDetailsT3"), function (data) {
            //    if (data) {
            //        $("[finddetailhide]").hide();
            //        if (data.SubmitSts) {
            //            window.location = _APPLocation + "FormT3/View/" + data.PkRefNo;
            //        }
            //        $('#txtFormMRefNum').val(data.RefId)
            //        tis.HeaderData = data;
            //        //tis.PageInit();
            //    }
            //}, "Finding");
            if (parseInt($("#RevisionNo").val()) > 4) {
                app.ShowErrorMessage("More than 4 revisions are not allowed");
                return;
            }
            else {
                GetfindDetails();
            }
        }
    }

    this.NavToList = function () {
        window.location = _APPLocation + "FormT3";
    }

    this.Cancel = function () {
        jsMaster.ConfirmCancel(() => { frmT3.NavToList(); });
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
        var action = isSubmit ? "/FormT3/Submit" : "/FormT3/SaveFormT3";
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
        //if (!isvalid) {
        //    app.ShowErrorMessage("SubTotal values are empty");
        //}

        var FormT3HDR = new Object();
        var FormT3 = new Object();

        FormT3HDR.PkRefNo = $("#pkRefNo").val();
        FormT3HDR.PkRefId = $('#txtFormT3RefNum').val();
        FormT3HDR.RmuCode = $("#formT3RMU").val();
        FormT3HDR.RmuName = $('#formT3RMU').find("option:selected").text();
        FormT3HDR.RevisionYear = $("#formT3Year").val();
        FormT3HDR.RevisionDate = $("#date").val();
        FormT3HDR.RevisionNo = $("#RevisionNo").val();
        FormT3HDR.CrBy = $("#UserId").val();
        FormT3HDR.CrDt = $("#date").val();

        var T3History = []
        var i = 0;
        $('#tblLabour > tbody  > tr').each(function (index, tr) {
            debugger;
            var T3 = new Object();

            T3.PkRefNoHistory = frmT3.HeaderData.RmT3History.length == 0 ? 0 : frmT3.HeaderData.RmT3History[i].PkRefNoHistory;
            T3.T3hPkRefNo = $("#pkRefNo").val();
            T3.Feature = $('#spx' + i).text().trim();
            T3.ActCode = $(this).find("td.x01").text().trim();
            T3.ActName = $(this).find("td.x02").text().trim();
            T3.Jan = $('#txt' + i + 'M1').val().replace(/,/g, "");
            T3.Feb = $('#txt' + i + 'M2').val().replace(/,/g, "");
            T3.Mar = $('#txt' + i + 'M3').val().replace(/,/g, "");
            T3.Apr = $('#txt' + i + 'M4').val().replace(/,/g, "");
            T3.May = $('#txt' + i + 'M5').val().replace(/,/g, "");
            T3.Jun = $('#txt' + i + 'M6').val().replace(/,/g, "");
            T3.Jul = $('#txt' + i + 'M7').val().replace(/,/g, "");
            T3.Aug = $('#txt' + i + 'M8').val().replace(/,/g, "");
            T3.Sep = $('#txt' + i + 'M9').val().replace(/,/g, "");
            T3.Oct = $('#txt' + i + 'M10').val().replace(/,/g, "");
            T3.Nov = $('#txt' + i + 'M11').val().replace(/,/g, "");
            T3.Dec = $('#txt' + i + 'M12').val().replace(/,/g, "");
            T3.SubTotal = $('#txt' + i + 'SubTotal').val().replace(/,/g, "");
            T3.UnitOfService = $('#txt' + i + 'Unit').val();
            T3.Remarks = $('#txt' + i + 'Remark').val();
            T3.Order = i;
            T3History.push(T3);
            i = i + 1;
        });

        FormT3 = T3History;
        var FormT3Data = JSON.stringify(FormT3);
        var FormT3HDRData = JSON.stringify(FormT3HDR);


        $.ajax({
            url: action,
            data: { formT3hdrdata: FormT3HDRData, formT3data: FormT3Data, reload: IsReload },
            type: 'POST',
            success: function (data) {
                HideAjaxLoading();
                if (data == -1) {
                    app.ShowErrorMessage(data.errorMessage);
                }
                else {
                    if (IsReload == 0) {
                        app.ShowSuccessMessage('Saved Successfully', false);
                        location.href = "/FormT3";
                    }
                }
            }
        });

    }

    this.NavToList = function () {
        window.location = _APPLocation + "FormT3";
    }

    this.Cancel = function () {
        jsMaster.ConfirmCancel(() => { frmT3.NavToList(); });
    }

    this.HeaderGrid = new function () {
        this.ActionRender = function (data, type, row, meta) {
            var actionSection = "<div class='btn-group dropright' rowidx='" + meta.row + "'><button type='button' class='btn btn-sm btn-themebtn dropdown-toggle' data-toggle='dropdown'> Click Me </button>";
            actionSection += "<div class='dropdown-menu'>";//dorpdown menu start

            if (data.Status != "Approved" && tblFT3HGrid.Base.IsModify) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmT3.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='edit-icon'></span> Edit </button>";
            }
            if (tblFT3HGrid.Base.IsView) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmT3.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='view-icon'></span> View </button>";
            }
            if (tblFT3HGrid.Base.IsDelete) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmT3.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='del-icon'></span> Delete </button>";
            }
            actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmT3.HeaderGrid.ActionClick(this);'>";
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
                var data = tblFT3HGrid.dataTable.row(rowidx).data();
                switch (type.toLowerCase()) {
                    case "edit":
                        window.location = _APPLocation + "FormT3/Edit/" + data.RefNo;
                        break;
                    case "view":
                        window.location = _APPLocation + "FormT3/View/" + data.RefNo;
                        break;
                    case "delete":
                        app.Confirm("Are you sure you want to delete this record? <br/>(Ref: " + data.RefNo + ")", (status) => {
                            if (status) {
                                DeleteRequest("Delete/" + data.RefNo, "FormT3", {}, function (sdata) {
                                    //if (sdata.id == "-1") {
                                    //    app.ShowErrorMessage("Form T3 cannot be deleted, first delete Form F3");
                                    //    return false;
                                    //}
                                    tblFT3HGrid.Refresh();
                                    app.ShowSuccessMessage("Deleted Sucessfully! <br/>(Ref: " + data.RefNo + ")");
                                });
                            }
                        }, "Yes", "No");
                        break;
                    case "print":
                        window.location = _APPLocation + "FormT3/download?id=" + data.RefNo;
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
            //if (this.HeaderData.FormT3Header && this.HeaderData.FormT3Header.PkRefNo && this.HeaderData.FormT3Header.PkRefNo > 0) {
            if ($("#pkRefNo").val() != "" && $("#pkRefNo").val() > 0) {
                $("#formT3RMU").prop("disabled", true).trigger("chosen:updated");
                $("#formT3Year").prop("disabled", true).trigger("chosen:updated");
                if (this.HeaderData.FormT3Header != undefined)
                    AppendData($("#pkRefNo").val(), this.HeaderData.FormT3Header.Status);
                else
                    AppendData($("#pkRefNo").val(), this.HeaderData.Status);
                $("[finddetailsdep]").show();
                $("#btnFindDetails").hide();
            }
            else {
                $('#formT3RMU').attr("disabled", false).trigger("chosen:updated");
                $("#formT3Year").prop("disabled", false).trigger("chosen:updated");
                $("[finddetailsdep]").hide();
                $("#btnFindDetails").show();
            }
        }
        else {
            $("#formT3RMU").prop("disabled", true).trigger("chosen:updated");
            $("#formT3Year").prop("disabled", true).trigger("chosen:updated");
            $("[finddetailsdep]").hide();
            $("#btnFindDetails").hide();
            ViewData($("#pkRefNo").val());
        }
    }
}

$(document).ready(function () {
    $("#smartSearch").focus();//Header Grid focus    

    frmT3.PageInit();

    $("[useridChange]").on("change", function () {
        frmT3.UserIdChange(this);
    });

    if (!frmT3.IsEdit) {
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
        if (frmT3.IsEdit) {
            var $this = $(this);
            frmT3.typein($this);
        }
        e.preventDefault();
    });

    $('.dropdown').on('click', function (e) {
        if (frmT3.IsEdit) {
            var $this = $(this);
            showComboBox($this);
        }
        e.preventDefault();
        e.stopPropagation();
    });

    $("#formT3Year").on("change", function () {
        getRevisionNo($("#formT3Year").val(), $("#formT3RMU").val());
    });

});

function Type(obj, e) {
    if (frmT3.IsEdit) {
        var $this = $(obj);
        frmT3.typein($this);
    }
    e.preventDefault();
}
function DDL(obj, e) {
    if (frmT3.IsEdit) {
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
        url: '/FormT3/GetMaxRev',
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
    var FormT3 = new Object();

    FormT3.RmuCode = $("#formT3RMU").val();
    FormT3.RmuName = $('#formT3RMU').find("option:selected").text();
    FormT3.RevisionYear = $("#formT3Year").val();
    FormT3.RevisionDate = $("#date").val();
    FormT3.RevisionNo = $("#RevisionNo").val();
    var FormT3Data = JSON.stringify(FormT3);
    $.ajax({
        url: '/FormT3/FindDetails',
        type: 'POST',
        data: { formT3data: FormT3Data },
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
                //    window.location = _APPLocation + "FormT3/View/" + data.PkRefNo;
                //}
                frmT3.HeaderData = data;
                $('#txtFormT3RefNum').val(data.PkRefId);
                $("#pkRefNo").val(frmT3.HeaderData.PkRefNo);
                frmT3.PageInit();

            }
        }
    });
}

function AppendData(id, Status) {
    debugger;
    var req = {};
    req.HistoryID = id;
    $.ajax({
        url: '/FormT3/GetHistoryData',
        type: 'POST',
        data: req,
        dataType: "json",
        success: function (data) {
            if (data.result.length > 0) {
                var i = 0;
                $('#tblLabour tbody tr').each(function () {
                    var jan = data.result[i].jan == null ? "" : Number(data.result[i].jan).toLocaleString('en');
                    var feb = data.result[i].feb == null ? "" : Number(data.result[i].feb).toLocaleString('en');
                    var mar = data.result[i].mar == null ? "" : Number(data.result[i].mar).toLocaleString('en');
                    var apr = data.result[i].apr == null ? "" : Number(data.result[i].apr).toLocaleString('en');
                    var may = data.result[i].may == null ? "" : Number(data.result[i].may).toLocaleString('en');
                    var jun = data.result[i].jun == null ? "" : Number(data.result[i].jun).toLocaleString('en');
                    var jul = data.result[i].jul == null ? "" : Number(data.result[i].jul).toLocaleString('en');
                    var aug = data.result[i].aug == null ? "" : Number(data.result[i].aug).toLocaleString('en');
                    var sep = data.result[i].sep == null ? "" : Number(data.result[i].sep).toLocaleString('en');
                    var oct = data.result[i].oct == null ? "" : Number(data.result[i].oct).toLocaleString('en');
                    var nov = data.result[i].nov == null ? "" : Number(data.result[i].nov).toLocaleString('en');
                    var dec = data.result[i].dec == null ? "" : Number(data.result[i].dec).toLocaleString('en');
                    var subTotal = data.result[i].subTotal == null ? "" : Number(data.result[i].subTotal).toLocaleString('en');
                    var UnitOfService = data.result[i].unitOfService == null ? "" : data.result[i].unitOfService;
                    var Remarks = data.result[i].remarks == null ? "" : data.result[i].remarks;

                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M1"  value="' + jan + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M2" value="' + feb + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M3" value="' + mar + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M4" value="' + apr + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M5" value="' + may + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M6" value="' + jun + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M7" value="' + jul + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M8" value="' + aug + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M9" value="' + sep + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M10" value="' + oct + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M11" value="' + nov + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M12" value="' + dec + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:100px;" id="txt' + i + 'SubTotal" class="form-control" disabled value="' + subTotal + '"/></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:140px;" id="txt' + i + 'Unit" class="form-control" disabled value="' + UnitOfService + '" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:140px;" id="txt' + i + 'Remark" class="form-control" value="' + Remarks + '" /></td>');
                    i = i + 1;
                });
            }
            else {
                var k = 0;
                var i = 0;
                $('#tblLabour tbody tr').each(function () {
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M1" disabled class="form-control" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M2" disabled class="form-control" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M3" disabled class="form-control" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M4" disabled class="form-control" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M5" disabled class="form-control" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M6" disabled class="form-control" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M7" disabled class="form-control" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M8" disabled class="form-control" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M9" disabled class="form-control" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M10" disabled class="form-control" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M11" disabled class="form-control" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M12" disabled class="form-control" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:100px;" id="txt' + i + 'SubTotal" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:140px;" id="txt' + i + 'Unit" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:140px;" id="txt' + i + 'Remark" class="form-control" /></td>');
                    i = i + 1;
                });
            }
            if (Status != "Verified" || Status != "Agreed" || Status != "Approved") {
                AppendPlannedData();
                AppendUnitData();
            }           
            $('#tblLabour tfoot').after('<tr><td style="text-align:right;" colspan="3"><span>SUB-TOTAL:</span></td><td style="text-align:center;"><span id="spJan"></span></td><td style="text-align:center;"><span id="spFeb"></span></td><td style="text-align:center;"><span id="spMar"></span></td><td style="text-align:center;"><span id="spApr"></span></td><td style="text-align:center;"><span id="spMay"></span></td><td style="text-align:center;"><span id="spJun"></span></td><td style="text-align:center;"><span id="spJul"></span></td><td style="text-align:center;"><span id="spAug"></span></td><td style="text-align:center;"><span id="spSep"></span></td><td style="text-align:center;"><span id="spOct"></span></td><td style="text-align:center;"><span id="spNov"></span></td><td style="text-align:center;"><span id="spDec"></span></td><td style="text-align:center;"><span id="spTotal"></span></td></tr>');
            MonthSumTotal();
        }
    });


}

function ViewData(id) {
    debugger;
    var req = {};
    req.HistoryID = id;
    $.ajax({
        url: '/FormT3/GetHistoryData',
        type: 'POST',
        data: req,
        dataType: "json",
        success: function (data) {
            if (data.result.length > 0) {
                var i = 0;
                $('#tblLabour tbody tr').each(function () {
                    var jan = data.result[i].jan == null ? "" : Number(data.result[i].jan).toLocaleString('en');
                    var feb = data.result[i].feb == null ? "" : Number(data.result[i].feb).toLocaleString('en');
                    var mar = data.result[i].mar == null ? "" : Number(data.result[i].mar).toLocaleString('en');
                    var apr = data.result[i].apr == null ? "" : Number(data.result[i].apr).toLocaleString('en');
                    var may = data.result[i].may == null ? "" : Number(data.result[i].may).toLocaleString('en');
                    var jun = data.result[i].jun == null ? "" : Number(data.result[i].jun).toLocaleString('en');
                    var jul = data.result[i].jul == null ? "" : Number(data.result[i].jul).toLocaleString('en');
                    var aug = data.result[i].aug == null ? "" : Number(data.result[i].aug).toLocaleString('en');
                    var sep = data.result[i].sep == null ? "" : Number(data.result[i].sep).toLocaleString('en');
                    var oct = data.result[i].oct == null ? "" : Number(data.result[i].oct).toLocaleString('en');
                    var nov = data.result[i].nov == null ? "" : Number(data.result[i].nov).toLocaleString('en');
                    var dec = data.result[i].dec == null ? "" : Number(data.result[i].dec).toLocaleString('en');
                    var total = data.result[i].subTotal == null ? "" : Number(data.result[i].subTotal).toLocaleString('en');
                    var UnitOfService = data.result[i].unitOfService == null ? "" : data.result[i].unitOfService;
                    var Remarks = data.result[i].remarks == null ? "" : data.result[i].remarks;

                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M1"  value="' + jan + '" class="form-control" disabled  /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M2"  value="' + feb + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M3"  value="' + mar + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M4"  value="' + apr + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M5"  value="' + may + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M6"  value="' + jun + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M7"  value="' + jul + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M8"  value="' + aug + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M9"  value="' + sep + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M10" value="' + oct + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M11" value="' + nov + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:70px;" id="txt' + i + 'M12" value="' + dec + '" class="form-control" disabled /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:100px;" id="txt' + i + 'SubTotal" class="form-control" disabled value="' + total + '" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:140px;" id="txt' + i + 'Unit" class="form-control" disabled value="' + UnitOfService + '" /></td>');
                    $(this).find("td:last").after('<td> <input type="text" style="width:140px;" id="txt' + i + 'Remark" class="form-control" disabled value="' + Remarks + '" /></td>');
                    i = i + 1;
                });
                $('#tblLabour tfoot').after('<tr><td style="text-align:right;" colspan="3"><span>SUB-TOTAL:</span></td><td style="text-align:center;"><span id="spJan"></span></td><td style="text-align:center;"><span id="spFeb"></span></td><td style="text-align:center;"><span id="spMar"></span></td><td style="text-align:center;"><span id="spApr"></span></td><td style="text-align:center;"><span id="spMay"></span></td><td style="text-align:center;"><span id="spJun"></span></td><td style="text-align:center;"><span id="spJul"></span></td><td style="text-align:center;"><span id="spAug"></span></td><td style="text-align:center;"><span id="spSep"></span></td><td style="text-align:center;"><span id="spOct"></span></td><td style="text-align:center;"><span id="spNov"></span></td><td style="text-align:center;"><span id="spDec"></span></td><td style="text-align:center;"><span id="spTotal"></span></td></tr>');
                MonthSumTotal();
            }
        }
    });


}



function AddLabourCal(obj, ActivityID) {
    debugger;
    var Qty = 0;
    var Unit = 0;
    $('#' + obj.id).val($('#' + obj.id).val().replace(/[^\d.]/g, ''));
    Qty = $('#' + obj.id).val();

    var subTotal = $("#txt" + ActivityID + "SubTotal").val() == "" ? 0 : $("#txt" + ActivityID + "SubTotal").val().replace(/,/g, "");
    //var total = (parseFloat($("#txt" + ActivityID + "1").val()) + parseFloat($("#txt" + ActivityID + "2").val()) + parseFloat($("#txt" + ActivityID + "3").val()) + parseFloat($("#txt" + ActivityID + "4").val()) + parseFloat($("#txt" + ActivityID + "5").val()) + parseFloat($("#txt" + ActivityID + "6").val()) + parseFloat($("#txt" + ActivityID + "7").val()) + parseFloat($("#txt" + ActivityID + "8").val()) + parseFloat($("#txt" + ActivityID + "9").val()) + parseFloat($("#txt" + ActivityID + "10").val()) + parseFloat($("#txt" + ActivityID + "11").val()) + parseFloat($("#txt" + ActivityID + "12").val()));
    var total = $("#txt" + ActivityID + "M1").val() == "" ? 0 : parseFloat($("#txt" + ActivityID + "M1").val().replace(/,/g, ""));
    total = parseFloat(parseFloat(total) + ($("#txt" + ActivityID + "M2").val() == "" ? 0 : parseFloat($("#txt" + ActivityID + "M2").val().replace(/,/g, "")))).toFixed(3);
    total = parseFloat(parseFloat(total) + ($("#txt" + ActivityID + "M3").val() == "" ? 0 : parseFloat($("#txt" + ActivityID + "M3").val().replace(/,/g, "")))).toFixed(3);
    total = parseFloat(parseFloat(total) + ($("#txt" + ActivityID + "M4").val() == "" ? 0 : parseFloat($("#txt" + ActivityID + "M4").val().replace(/,/g, "")))).toFixed(3);
    total = parseFloat(parseFloat(total) + ($("#txt" + ActivityID + "M5").val() == "" ? 0 : parseFloat($("#txt" + ActivityID + "M5").val().replace(/,/g, "")))).toFixed(3);
    total = parseFloat(parseFloat(total) + ($("#txt" + ActivityID + "M6").val() == "" ? 0 : parseFloat($("#txt" + ActivityID + "M6").val().replace(/,/g, "")))).toFixed(3);
    total = parseFloat(parseFloat(total) + ($("#txt" + ActivityID + "M7").val() == "" ? 0 : parseFloat($("#txt" + ActivityID + "M7").val().replace(/,/g, "")))).toFixed(3);
    total = parseFloat(parseFloat(total) + ($("#txt" + ActivityID + "M8").val() == "" ? 0 : parseFloat($("#txt" + ActivityID + "M8").val().replace(/,/g, "")))).toFixed(3);
    total = parseFloat(parseFloat(total) + ($("#txt" + ActivityID + "M9").val() == "" ? 0 : parseFloat($("#txt" + ActivityID + "M9").val().replace(/,/g, "")))).toFixed(3);
    total = parseFloat(parseFloat(total) + ($("#txt" + ActivityID + "M10").val() == "" ? 0 : parseFloat($("#txt" + ActivityID + "M10").val().replace(/,/g, "")))).toFixed(3);
    total = parseFloat(parseFloat(total) + ($("#txt" + ActivityID + "M11").val() == "" ? 0 : parseFloat($("#txt" + ActivityID + "M11").val().replace(/,/g, "")))).toFixed(3);
    total = parseFloat(parseFloat(total) + ($("#txt" + ActivityID + "M12").val() == "" ? 0 : parseFloat($("#txt" + ActivityID + "M12").val().replace(/,/g, "")))).toFixed(3);
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
    var rmucode = $("#formT3RMU").val();
    var year = $("#formT3Year").val();
    req.RmuCode = rmucode;
    req.Year = year;
    $.ajax({
        url: '/FormT3/GetPlannedBudgetData',
        dataType: 'JSON',
        data: req,
        type: 'Post',
        success: function (data) {
            if (data.result.length > 0) {
                for (var i = 0; i < data.result.length; i++) {
                    var jan = data.result[i].jan == null ? "" : Number(data.result[i].jan).toLocaleString('en');
                    var feb = data.result[i].feb == null ? "" : Number(data.result[i].feb).toLocaleString('en');
                    var mar = data.result[i].mar == null ? "" : Number(data.result[i].mar).toLocaleString('en');
                    var apr = data.result[i].apr == null ? "" : Number(data.result[i].apr).toLocaleString('en');
                    var may = data.result[i].may == null ? "" : Number(data.result[i].may).toLocaleString('en');
                    var jun = data.result[i].jun == null ? "" : Number(data.result[i].jun).toLocaleString('en');
                    var jul = data.result[i].jul == null ? "" : Number(data.result[i].jul).toLocaleString('en');
                    var aug = data.result[i].aug == null ? "" : Number(data.result[i].aug).toLocaleString('en');
                    var sep = data.result[i].sep == null ? "" : Number(data.result[i].sep).toLocaleString('en');
                    var oct = data.result[i].oct == null ? "" : Number(data.result[i].oct).toLocaleString('en');
                    var nov = data.result[i].nov == null ? "" : Number(data.result[i].nov).toLocaleString('en');
                    var dec = data.result[i].dec == null ? "" : Number(data.result[i].dec).toLocaleString('en');
                    var total = data.result[i].subTotal == null ? "" : Number(data.result[i].subTotal).toLocaleString('en');

                    $('#spx' + i).text(data.result[i].feature);
                    $('#txt' + i + "M1").val(jan);
                    $('#txt' + i + "M2").val(feb);
                    $('#txt' + i + "M3").val(mar);
                    $('#txt' + i + "M4").val(apr);
                    $('#txt' + i + "M5").val(may);
                    $('#txt' + i + "M6").val(jun);
                    $('#txt' + i + "M7").val(jul);
                    $('#txt' + i + "M8").val(aug);
                    $('#txt' + i + "M9").val(sep);
                    $('#txt' + i + "M10").val(oct);
                    $('#txt' + i + "M11").val(nov);
                    $('#txt' + i + "M12").val(dec);
                    $('#txt' + i + "SubTotal").val(total);
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
    var year = $("#formT3Year").val();
    req.Year = year;
    $.ajax({
        url: '/FormT3/GetUnitData',
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

function MonthSumTotal() {
    debugger;
    for (var j = 0; j < 38; j++) {
        if (j == 0) {
            $('#spJan').text($('#txt' + j + 'M1').val() == "" ? 0 : parseFloat($('#txt' + j + 'M1').val().replace(/,/g, "")));
            $('#spFeb').text($('#txt' + j + 'M2').val() == "" ? 0 : parseFloat($('#txt' + j + 'M2').val().replace(/,/g, "")));
            $('#spMar').text($('#txt' + j + 'M3').val() == "" ? 0 : parseFloat($('#txt' + j + 'M3').val().replace(/,/g, "")));
            $('#spApr').text($('#txt' + j + 'M4').val() == "" ? 0 : parseFloat($('#txt' + j + 'M4').val().replace(/,/g, "")));
            $('#spMay').text($('#txt' + j + 'M5').val() == "" ? 0 : parseFloat($('#txt' + j + 'M5').val().replace(/,/g, "")));
            $('#spJun').text($('#txt' + j + 'M6').val() == "" ? 0 : parseFloat($('#txt' + j + 'M6').val().replace(/,/g, "")));
            $('#spJul').text($('#txt' + j + 'M7').val() == "" ? 0 : parseFloat($('#txt' + j + 'M7').val().replace(/,/g, "")));
            $('#spAug').text($('#txt' + j + 'M8').val() == "" ? 0 : parseFloat($('#txt' + j + 'M8').val().replace(/,/g, "")));
            $('#spSep').text($('#txt' + j + 'M9').val() == "" ? 0 : parseFloat($('#txt' + j + 'M9').val().replace(/,/g, "")));
            $('#spOct').text($('#txt' + j + 'M10').val() == "" ? 0 : parseFloat($('#txt' + j + 'M10').val().replace(/,/g, "")));
            $('#spNov').text($('#txt' + j + 'M11').val() == "" ? 0 : parseFloat($('#txt' + j + 'M11').val().replace(/,/g, "")));
            $('#spDec').text($('#txt' + j + 'M12').val() == "" ? 0 : parseFloat($('#txt' + j + 'M12').val().replace(/,/g, "")));
            $('#spTotal').text($('#txt' + j + 'SubTotal').val() == "" ? 0 : parseFloat($('#txt' + j + 'SubTotal').val().replace(/,/g, "")));
        }
        else {
            $('#spJan').text(parseFloat(parseFloat($('#spJan').text()) + ($('#txt' + j + 'M1').val() == "" ? 0 : parseFloat($('#txt' + j + 'M1').val().replace(/,/g, "")))).toFixed(2));
            $('#spFeb').text(parseFloat(parseFloat($('#spFeb').text()) + ($('#txt' + j + 'M2').val() == "" ? 0 : parseFloat($('#txt' + j + 'M2').val().replace(/,/g, "")))).toFixed(2));
            $('#spMar').text(parseFloat(parseFloat($('#spMar').text()) + ($('#txt' + j + 'M3').val() == "" ? 0 : parseFloat($('#txt' + j + 'M3').val().replace(/,/g, "")))).toFixed(2));
            $('#spApr').text(parseFloat(parseFloat($('#spApr').text()) + ($('#txt' + j + 'M4').val() == "" ? 0 : parseFloat($('#txt' + j + 'M4').val().replace(/,/g, "")))).toFixed(2));
            $('#spMay').text(parseFloat(parseFloat($('#spMay').text()) + ($('#txt' + j + 'M5').val() == "" ? 0 : parseFloat($('#txt' + j + 'M5').val().replace(/,/g, "")))).toFixed(2));
            $('#spJun').text(parseFloat(parseFloat($('#spJun').text()) + ($('#txt' + j + 'M6').val() == "" ? 0 : parseFloat($('#txt' + j + 'M6').val().replace(/,/g, "")))).toFixed(2));
            $('#spJul').text(parseFloat(parseFloat($('#spJul').text()) + ($('#txt' + j + 'M7').val() == "" ? 0 : parseFloat($('#txt' + j + 'M7').val().replace(/,/g, "")))).toFixed(2));
            $('#spAug').text(parseFloat(parseFloat($('#spAug').text()) + ($('#txt' + j + 'M8').val() == "" ? 0 : parseFloat($('#txt' + j + 'M8').val().replace(/,/g, "")))).toFixed(2));
            $('#spSep').text(parseFloat(parseFloat($('#spSep').text()) + ($('#txt' + j + 'M9').val() == "" ? 0 : parseFloat($('#txt' + j + 'M9').val().replace(/,/g, "")))).toFixed(2));
            $('#spOct').text(parseFloat(parseFloat($('#spOct').text()) + ($('#txt' + j + 'M10').val() == "" ? 0 : parseFloat($('#txt' + j + 'M10').val().replace(/,/g, "")))).toFixed(2));
            $('#spNov').text(parseFloat(parseFloat($('#spNov').text()) + ($('#txt' + j + 'M11').val() == "" ? 0 : parseFloat($('#txt' + j + 'M11').val().replace(/,/g, "")))).toFixed(2));
            $('#spDec').text(parseFloat(parseFloat($('#spDec').text()) + ($('#txt' + j + 'M12').val() == "" ? 0 : parseFloat($('#txt' + j + 'M12').val().replace(/,/g, "")))).toFixed(2));
            $('#spTotal').text(parseFloat(parseFloat($('#spTotal').text()) + ($('#txt' + j + 'SubTotal').val() == "" ? 0 : parseFloat($('#txt' + j + 'SubTotal').val().replace(/,/g, "")))).toFixed(3));
        }
    }

    $('#spJan').text(Number($('#spJan').text()).toLocaleString('en'));
    $('#spFeb').text(Number($('#spFeb').text()).toLocaleString('en'));
    $('#spMar').text(Number($('#spMar').text()).toLocaleString('en'));
    $('#spApr').text(Number($('#spApr').text()).toLocaleString('en'));
    $('#spMay').text(Number($('#spMay').text()).toLocaleString('en'));
    $('#spJun').text(Number($('#spJun').text()).toLocaleString('en'));
    $('#spJul').text(Number($('#spJul').text()).toLocaleString('en'));
    $('#spAug').text(Number($('#spAug').text()).toLocaleString('en'));
    $('#spSep').text(Number($('#spSep').text()).toLocaleString('en'));
    $('#spOct').text(Number($('#spOct').text()).toLocaleString('en'));
    $('#spNov').text(Number($('#spNov').text()).toLocaleString('en'));
    $('#spDec').text(Number($('#spDec').text()).toLocaleString('en'));
    $('#spTotal').text(Number($('#spTotal').text()).toLocaleString('en'));
}