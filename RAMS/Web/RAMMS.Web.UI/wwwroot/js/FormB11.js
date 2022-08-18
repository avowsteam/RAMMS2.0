
var frmB11 = new function () {
    this.HeaderData = {};
    this.IsEdit = true;

    this.Save = function () {
        debugger;
        var failed = false;

        if (failed)
            return;

        //  if (ValidatePage('#myModal')) {
        InitAjaxLoading();

        var FormB11 = new Object();
        FormB11.B11hPkRefNo = $("#FormB11Header_B11hPkRefNo").val();
        FormB11.B11hRmuCode = $("#txtB11RmuCode").val();
        FormB11.B11hRmuName = $("#txtB11RmuCode").val();
        FormB11.B11hRevisionNo = $("#RevisionNo").val();
        FormB11.B11hRevisionDate = $("#date").val();
        FormB11.B11hRevisionYear = $("#formB11Year").val();
        FormB11.B11hCrBy = $("#UserId").val();
        FormB11.B11hCrByName = $("#UserName").val();
        FormB11.B11hCrDt = $("#date").val();

        var B11LabourCost = []

        var labourDataOrder = 0;
        $('#tblLabour > tbody  > tr').each(function (index, tr) {
            var labCount = $(this).find("td.xl700").attr("data-lab");
            for (var i = 0; i < labCount; i++) {
                debugger;
                var B11 = new Object();
                B11.B11lcB11hPkRefNo = $("#FormB11Header_B11hPkRefNo").val();
                B11.B11lcActivityId = $(this).find("td.xl68").text().trim();
                B11.B11lcLabourId = $(this).find("td.xl70" + i + " span:eq(3)").text().trim();
                B11.B11lcLabourName = $(this).find("td.xl70" + i + " span:eq(0)").text().trim();
                B11.B11lcLabourPerUnitPrice = $(this).find("td.xl70" + i + " span:eq(1)").text().trim();
                B11.B11lcLabourNoOfUnits = $('#txt' + $(this).find("td.xl70" + i + " span:eq(1)").attr("id").slice(2)).val().trim();
                B11.B11lcLabourTotalPrice = $(this).find("td.xl70" + i + " span:eq(2)").text().trim();
                B11.B11lcLabourOrderID = labourDataOrder;
                B11LabourCost.push(B11);
            }
            labourDataOrder = labourDataOrder + 1;
        });

        var B11MaterialCost = []

        var MaterialDataOrder = 0;
        $('#tblMaterial > tbody  > tr').each(function (index, tr) {
            var labCount = $(this).find("td.xl700").attr("data-lab");
            for (var i = 0; i < labCount; i++) {
                debugger;
                var B11 = new Object();
                B11.B11mcB11hPkRefNo = $("#FormB11Header_B11hPkRefNo").val();
                B11.B11mcActivityId = $(this).find("td.xl68").text().trim();
                B11.B11mcMaterialId = $(this).find("td.xl70" + i + " span:eq(3)").text().trim();
                B11.B11mcMaterialName = $(this).find("td.xl70" + i + " span:eq(0)").text().trim();
                B11.B11mcMaterialPerUnitPrice = $(this).find("td.xl70" + i + " span:eq(1)").text().trim();
                B11.B11mcMaterialNoOfUnits = $('#mtxt' + $(this).find("td.xl70" + i + " span:eq(1)").attr("id").slice(3)).val().trim();
                B11.B11mcMaterialTotalPrice = $(this).find("td.xl70" + i + " span:eq(2)").text().trim();
                B11.B11mcMaterialOrderId = MaterialDataOrder;
                B11MaterialCost.push(B11);
            }
            MaterialDataOrder = MaterialDataOrder + 1;
        });

        var B11EquipmentCost = []

        var EquipmentDataOrder = 0;
        $('#tblEquipment > tbody  > tr').each(function (index, tr) {
            var labCount = $(this).find("td.xl700").attr("data-lab");
            for (var i = 0; i < labCount; i++) {
                debugger;
                var B11 = new Object();
                B11.B11ecB11hPkRefNo = $("#FormB11Header_B11hPkRefNo").val();
                B11.B11ecActivityId = $(this).find("td.xl68").text().trim();
                B11.B11ecEquipmentId = $(this).find("td.xl70" + i + " span:eq(3)").text().trim();
                B11.B11ecEquipmentName = $(this).find("td.xl70" + i + " span:eq(0)").text().trim();
                B11.B11ecEquipmentPerUnitPrice = $(this).find("td.xl70" + i + " span:eq(1)").text().trim();
                B11.B11ecEquipmentNoOfUnits = $('#etxt' + $(this).find("td.xl70" + i + " span:eq(1)").attr("id").slice(3)).val().trim();
                B11.B11ecEquipmentTotalPrice = $(this).find("td.xl70" + i + " span:eq(2)").text().trim();
                B11.B11ecEquipmentOrderId = EquipmentDataOrder;
                B11EquipmentCost.push(B11);
            }
            EquipmentDataOrder = EquipmentDataOrder + 1;
        });

        debugger;
        FormB11.RmB11LabourCost = B11LabourCost;
        FormB11.RmB11EquipmentCost = B11EquipmentCost;
        FormB11.RmB11MaterialCost = B11MaterialCost;

        var FormB11Data = JSON.stringify(FormB11);
        $.ajax({
            url: '/FormB11/SaveFormB11',
            type: 'POST',
            data: { formb11data: FormB11Data },
            dataType: "json",
            success: function (data) {
                HideAjaxLoading();
                if (data == -1) {
                    app.ShowErrorMessage(data.errorMessage);
                }
                else {
                    app.ShowSuccessMessage('Saved Successfully', false);
                    location.href = "/FormB11";
                }
            }
        });
    }

    this.NavToList = function () {
        window.location = _APPLocation + "FormB11";
    }

    this.Cancel = function () {
        jsMaster.ConfirmCancel(() => { frmB11.NavToList(); });
    }


    this.HeaderGrid = new function () {
        this.ActionRender = function (data, type, row, meta) {
            debugger;
            var actionSection = "<div class='btn-group dropright' rowidx='" + meta.row + "'><button type='button' class='btn btn-sm btn-themebtn dropdown-toggle' data-toggle='dropdown'> Click Me </button>";
            actionSection += "<div class='dropdown-menu'>";//dorpdown menu start

            if (data.MaxRecord) {//if (tblFB11HGrid.Base.IsModify) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB11.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='edit-icon'></span> Edit </button>";
            }
            if (tblFB11HGrid.Base.IsView) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB11.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='view-icon'></span> View </button>";
            }
            //if (tblFB7HGrid.Base.IsDelete) {
            //    actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB7.HeaderGrid.ActionClick(this);'>";
            //    actionSection += "<span class='del-icon'></span> Delete </button>";
            //}
            actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB11.HeaderGrid.ActionClick(this);'>";
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
                var data = tblFB11HGrid.dataTable.row(rowidx).data();
                debugger;
                switch (type.toLowerCase()) {
                    case "edit":
                        window.location = _APPLocation + "FormB11/Edit/" + data.RefNo;
                        break;
                    case "view":
                        window.location = _APPLocation + "FormB11/View/" + data.RefNo;
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
                        window.location = _APPLocation + "FormB11/download?id=" + data.RefNo + "&Rmucode=" + data.RMUCode;
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
            if ($('#formB11Year').val() != "" && $('#formB11Year').val() != null) {
                AppendLabourData();
                AppendMaterialData();
                AppendEquipmentData();
            }
        }
        else {
            ViewLabourData($("#FormB11Header_B11hPkRefNo").val());
            ViewMaterialData($("#FormB11Header_B11hPkRefNo").val());
            ViewEquipmentData($("#FormB11Header_B11hPkRefNo").val());
        }
    }
}

$(document).ready(function () {
    frmB11.PageInit();
    $("#smartSearch").focus();//Header Grid focus    

    //Listener for Smart and Detail Search
    $("#FB11SrchSection").find("#smartSearch").focus();
    element = document.querySelector("#formB11AdvSearch");
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
        if (frmG1G2.IsEdit) {
            var $this = $(this);
            frmB11.typein($this);
        }
        e.preventDefault();
    });

    $('.dropdown').on('click', function (e) {
        if (frmG1G2.IsEdit) {
            var $this = $(this);
            showComboBox($this);
        }
        e.preventDefault();
        e.stopPropagation();
    });



    $("#formB11Year").on("change", function () {
        getRevisionNo($("#formB11Year").val());
    });

});

function getRevisionNo(id) {
    var req = {};
    req.Year = id;
    $.ajax({
        url: '/FormB11/GetMaxRev',
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

function showComboBox(obj) {

    if (obj[0].childElementCount > 0)
        return;

    var selectHtml = "<select onchange='setSelected(this)'>";

    if (UnitObj.length > 0) {
        var selected = "";
        var selectedvalue = $(obj).parent().attr("preval");
        selectHtml = selectHtml + "<option value='0'></option>";

        $.each(UnitObj, function (index, v) {
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

function AppendLabourData(id) {
    var req = {};
    var year = $("#formB11Year").val();
    req.Year = year;
    $.ajax({
        url: '/FormB11/GetLabourHistoryData',
        dataType: 'JSON',
        data: req,
        type: 'Post',
        success: function (data) {
            debugger;
            if (data.result.length > 0) {
                var k = 0;
                for (var i = 0; i < data.result.length; i++) {
                    $('#tblLabour thead tr th:last').after('<th class="xl65" x:str><span style="width:150px;float:left;text-align:center">' + data.result[i].b7lhName + ' Unit </span></th>');
                    $('#tblLabour thead tr th:last').after('<th class="xl65" x:str><span style="width:150px;float:left;text-align:center">' + data.result[i].b7lhName + ' Rate </span></th>');
                }

                if ($("#txtB11RmuCode").val() == "Miri") {
                    $('#tblLabour tbody tr').each(function () {
                        for (var i = 0; i < data.result.length; i++) {
                            var id = k + data.result[i].b7lhCode;
                            $(this).find("td:last").after('<td class="xl69"> <input type="text" id="txt' + id + '" onkeyup="LabourCal(this, ' + k + ')" class="form-control" /></td>');
                            $(this).find("td:last").after('<td class="xl70' + i + '" data-lab=' + data.result.length + '><span style="display:none;">' + data.result[i].b7lhName + '</span><span id="sp' + id + '" style="display:none;">' + data.result[i].b7lhUnitPriceMiri + '</span><span class="x100' + k + '" id="span' + id + '" style="width:150px;float:left;text-align:center"></span><span style="display:none;">' + data.result[i].b7lhCode + '</span></td>');
                        }
                        $(this).find("td:last").after('<td class="xl71" x:str><span id="sptot' + k + '" style="width:150px;float:left;text-align:center"></span></td>');
                        k = k + 1;
                    });
                }
                else {
                    $('#tblLabour tbody tr').each(function () {
                        for (var i = 0; i < data.result.length; i++) {
                            var id = k + data.result[i].b7lhCode;
                            $(this).find("td:last").after('<td class="xl69"> <input type="text" id="txt' + id + '" onkeyup="LabourCal(this, ' + k + ')" class="form-control" /></td>');
                            $(this).find("td:last").after('<td class="xl70' + i + '" data-lab=' + data.result.length + '><span style="display:none;">' + data.result[i].b7lhName + '</span><span id="sp' + id + '" style="display:none;">' + data.result[i].b7lhUnitPriceBatuNiah + '</span><span class="x100' + k + '" id="span' + id + '" style="width:150px;float:left;text-align:center"></span><span style="display:none;">' + data.result[i].b7lhCode + '</span></td>');
                        }
                        $(this).find("td:last").after('<td class="xl71" x:str><span id="sptot' + k + '" style="width:150px;float:left;text-align:center"></span></td>');
                        k = k + 1;
                    });
                }
                $('#tblLabour thead tr th:last').after('<th class="xl65" x:str><span style="width:150px;float:left;text-align:center"> Labour Unit </span></th>');

            }
            //$("#RevisionNo").val(data)
        },
        error: function (data) {
            console.error(data);
        }
    });
}

function LabourCal(obj, k) {
    debugger;
    var Qty = 0;
    var Unit = 0;
    Qty = $('#' + obj.id).val();
    Unit = $('#sp' + obj.id.slice(3)).text();
    var tot = Qty * Unit;
    $('#span' + obj.id.slice(3)).text(tot.toFixed(2))
    var sum = 0;
    $("#tblLabour tbody tr td .x100" + k).each(function (index, tr) {
        debugger;
        if (!isNaN(this.innerText) && this.innerText != "") {
            sum += parseFloat(this.innerText);
        }
    });
    $('#sptot' + k).text(sum.toFixed(3));
}

function ViewLabourData(id) {
    debugger;
    var req = {};
    req.id = id;
    $.ajax({
        url: '/FormB11/GetLabourViewHistoryData',
        dataType: 'JSON',
        data: req,
        type: 'Post',
        success: function (data) {
            debugger;
            if (data.result.length > 0) {
                var k = 0;
                var cRow = 0;
                var labB7 = data.result.filter(function (el) { return el.b11lcLabourOrderId == 0; }).length;
                var labourB11 = data.result.map(p => p.b11lcLabourOrderId).filter((b11lcLabourOrderId, index, arr) => arr.indexOf(b11lcLabourOrderId) == index).length;
                for (var i = 0; i < labB7; i++) {
                    $('#tblLabour thead tr th:last').after('<th class="xl65" x:str><span style="width:150px;float:left;text-align:center">' + data.result.filter(function (el) { return el.b11lcLabourOrderId == 0; })[i].b11lcLabourName + ' Unit </span></th>');
                    $('#tblLabour thead tr th:last').after('<th class="xl65" x:str><span style="width:150px;float:left;text-align:center">' + data.result.filter(function (el) { return el.b11lcLabourOrderId == 0; })[i].b11lcLabourName + ' Rate </span></th>');
                }


                $('#tblLabour tbody tr').each(function () {
                    var totallab = 0;
                    var activityID = 0;
                    for (var i = 0; i < labB7; i++) {
                        activityID = data.result.filter(function (el) { return el.b11lcLabourOrderId == cRow; })[0].b11lcActivityId;
                        var id = k + data.result.filter(function (el) { return el.b11lcLabourOrderId == cRow; })[i].b11lcLabourId;
                        var LabourUnit = data.result.filter(function (el) { return el.b11lcLabourOrderId == cRow; })[i].b11lcLabourNoOfUnits != null ? data.result.filter(function (el) { return el.b11lcLabourOrderId == cRow; })[i].b11lcLabourNoOfUnits : "";
                        var LabourUnitPrice = data.result.filter(function (el) { return el.b11lcLabourOrderId == cRow; })[i].b11lcLabourTotalPrice != null ? data.result.filter(function (el) { return el.b11lcLabourOrderId == cRow; })[i].b11lcLabourTotalPrice : "";
                        $(this).find("td:last").after('<td class="xl69"> <input type="text" id="txt' + id + '" disabled value="' + LabourUnit + '"  class="form-control" /></td>');

                        $(this).find("td:last").after('<td class="xl70' + i + '" data-lab=' + labourB11 + '><span style="display:none;">' + data.result.filter(function (el) { return el.b11lcLabourOrderId == cRow; })[i].b11lcLabourName + '</span><span id="sp' + id + '" style="display:none;">' + data.result.filter(function (el) { return el.b11lcLabourOrderId == cRow; })[i].b11lcLabourPerUnitPrice + '</span><span class="x100' + k + '" id="span' + id + '" style="width:150px;float:left;text-align:center">' + LabourUnitPrice + '</span><span style="display:none;">' + data.result.filter(function (el) { return el.b11lcLabourOrderId == cRow; })[i].b11lcLabourId + '</span></td>');

                        var rowlaboutot = data.result.filter(function (el) { return el.b11lcLabourOrderId == cRow; })[i].b11lcLabourTotalPrice != null ? data.result.filter(function (el) { return el.b11lcLabourOrderId == cRow; })[i].b11lcLabourTotalPrice : 0
                        totallab = totallab + parseFloat(rowlaboutot)
                    }
                    $('#' + activityID).text(parseFloat(totallab));
                    $(this).find("td:last").after('<td class="xl71" x:str><span id="sptot' + k + '" style="width:150px;float:left;text-align:center">' + totallab + '</span></td>');
                    k = k + 1;
                    cRow = cRow + 1;
                });

                $('#tblLabour thead tr th:last').after('<th class="xl65" x:str><span style="width:150px;float:left;text-align:center"> Labour Unit </span></th>');
            }
        },
        error: function (data) {
            console.error(data);
        }
    });
}

function AppendMaterialData(id) {
    var req = {};
    var year = $("#formB11Year").val();
    req.Year = year;
    $.ajax({
        url: '/FormB11/GetMaterialHistoryData',
        dataType: 'JSON',
        data: req,
        type: 'Post',
        success: function (data) {
            debugger;
            if (data.result.length > 0) {
                var k = 0;
                for (var i = 0; i < data.result.length; i++) {
                    $('#tblMaterial thead tr th:last').after('<th class="xl65" x:str><span style="width:200px;float:left;text-align:center">' + data.result[i].b7mhName + ' Unit </span></th>');
                    $('#tblMaterial thead tr th:last').after('<th class="xl65" x:str><span style="width:200px;float:left;text-align:center">' + data.result[i].b7mhName + ' Rate </span></th>');
                }

                if ($("#txtB11RmuCode").val() == "Miri") {
                    $('#tblMaterial tbody tr').each(function () {
                        for (var i = 0; i < data.result.length; i++) {
                            var id = k + data.result[i].b7mhCode;
                            $(this).find("td:last").after('<td class="xl69"> <input type="text" id="mtxt' + id + '" onkeyup="MaterialCal(this, ' + k + ')" class="form-control" /></td>');
                            $(this).find("td:last").after('<td class="xl70' + i + '" data-lab=' + data.result.length + '><span style="display:none;">' + data.result[i].b7mhName + '</span><span id="msp' + id + '" style="display:none;">' + data.result[i].b7mhUnitPriceMiri + '</span><span class="x100' + k + '" id="mspan' + id + '" style="width:150px;float:left;text-align:center"></span><span style="display:none;">' + data.result[i].b7mhCode + '</span></td>');
                        }
                        $(this).find("td:last").after('<td class="xl71" x:str><span id="msptot' + k + '" style="width:150px;float:left;text-align:center"></span></td>');
                        k = k + 1;
                    });
                }
                else {
                    $('#tblMaterial tbody tr').each(function () {
                        for (var i = 0; i < data.result.length; i++) {
                            var id = k + data.result[i].b7mhCode;
                            $(this).find("td:last").after('<td class="xl69"> <input type="text" id="mtxt' + id + '" onkeyup="MaterialCal(this, ' + k + ')" class="form-control" /></td>');
                            $(this).find("td:last").after('<td class="xl70' + i + '" data-lab=' + data.result.length + '><span style="display:none;">' + data.result[i].b7mhName + '</span><span id="msp' + id + '" style="display:none;">' + data.result[i].b7mhUnitPriceBatuNiah + '</span><span class="x100' + k + '" id="mspan' + id + '" style="width:150px;float:left;text-align:center"></span><span style="display:none;">' + data.result[i].b7mhCode + '</span></td>');
                        }
                        $(this).find("td:last").after('<td class="xl71" x:str><span id="msptot' + k + '" style="width:150px;float:left;text-align:center"></span></td>');
                        k = k + 1;
                    });
                }
                $('#tblMaterial thead tr th:last').after('<th class="xl65" x:str><span style="width:150px;float:left;text-align:center"> Material Unit </span></th>');

            }
            //$("#RevisionNo").val(data)
        },
        error: function (data) {
            console.error(data);
        }
    });
}

function MaterialCal(obj, k) {
    debugger;
    var Qty = 0;
    var Unit = 0;
    Qty = $('#' + obj.id).val();
    Unit = $('#msp' + obj.id.slice(4)).text();
    var tot = Qty * Unit;
    $('#mspan' + obj.id.slice(4)).text(tot.toFixed(2))
    var sum = 0;
    $("#tblMaterial tbody tr td .x100" + k).each(function (index, tr) {
        debugger;
        if (!isNaN(this.innerText) && this.innerText != "") {
            sum += parseFloat(this.innerText);
        }
    });
    $('#msptot' + k).text(sum.toFixed(3));
}

function ViewMaterialData(id) {
    debugger;
    var req = {};
    req.id = id;
    $.ajax({
        url: '/FormB11/GetMaterialViewHistoryData',
        dataType: 'JSON',
        data: req,
        type: 'Post',
        success: function (data) {
            debugger;
            if (data.result.length > 0) {
                var k = 0;
                var cRow = 0;
                var labB7 = data.result.filter(function (el) { return el.b11mcMaterialOrderId == 0; }).length;
                var labourB11 = data.result.map(p => p.b11mcMaterialOrderId).filter((b11mcMaterialOrderId, index, arr) => arr.indexOf(b11mcMaterialOrderId) == index).length;
                for (var i = 0; i < labB7; i++) {
                    $('#tblMaterial thead tr th:last').after('<th class="xl65" x:str><span style="width:200px;float:left;text-align:center">' + data.result.filter(function (el) { return el.b11mcMaterialOrderId == 0; })[i].b11mcMaterialName + ' Unit </span></th>');
                    $('#tblMaterial thead tr th:last').after('<th class="xl65" x:str><span style="width:200px;float:left;text-align:center">' + data.result.filter(function (el) { return el.b11mcMaterialOrderId == 0; })[i].b11mcMaterialName + ' Rate </span></th>');
                }


                $('#tblMaterial tbody tr').each(function () {
                    var totallab = 0;
                    var activityID = 0;
                    for (var i = 0; i < labB7; i++) {
                        activityID = data.result.filter(function (el) { return el.b11mcMaterialOrderId == cRow; })[0].b11mcActivityId;
                        var id = k + data.result.filter(function (el) { return el.b11mcMaterialOrderId == cRow; })[i].b11mcMaterialId;
                        var LabourUnit = data.result.filter(function (el) { return el.b11mcMaterialOrderId == cRow; })[i].b11mcMaterialNoOfUnits != null ? data.result.filter(function (el) { return el.b11mcMaterialOrderId == cRow; })[i].b11mcMaterialNoOfUnits : "";
                        var LabourUnitPrice = data.result.filter(function (el) { return el.b11mcMaterialOrderId == cRow; })[i].b11mcMaterialTotalPrice != null ? data.result.filter(function (el) { return el.b11mcMaterialOrderId == cRow; })[i].b11mcMaterialTotalPrice : "";
                        $(this).find("td:last").after('<td class="xl69"> <input type="text" id="mtxt' + id + '" disabled value="' + LabourUnit + '"  class="form-control" /></td>');

                        $(this).find("td:last").after('<td class="xl70' + i + '" data-lab=' + labourB11 + '><span style="display:none;">' + data.result.filter(function (el) { return el.b11mcMaterialOrderId == cRow; })[i].b11mcMaterialName + '</span><span id="msp' + id + '" style="display:none;">' + data.result.filter(function (el) { return el.b11mcMaterialOrderId == cRow; })[i].b11mcMaterialPerUnitPrice + '</span><span class="x100' + k + '" id="mspan' + id + '" style="width:150px;float:left;text-align:center">' + LabourUnitPrice + '</span><span style="display:none;">' + data.result.filter(function (el) { return el.b11mcMaterialOrderId == cRow; })[i].b11mcMaterialId + '</span></td>');

                        var rowlaboutot = data.result.filter(function (el) { return el.b11mcMaterialOrderId == cRow; })[i].b11mcMaterialTotalPrice != null ? data.result.filter(function (el) { return el.b11mcMaterialOrderId == cRow; })[i].b11mcMaterialTotalPrice : 0
                        totallab = totallab + parseFloat(rowlaboutot)
                    }
                    $('#' + activityID).text() != "" ? $('#' + activityID).text(parseFloat($('#' + activityID).text()) + totallab) : $('#' + activityID).text(parseFloat(totallab));
                    $(this).find("td:last").after('<td class="xl71" x:str><span id="msptot' + k + '" style="width:150px;float:left;text-align:center">' + totallab + '</span></td>');
                    k = k + 1;
                    cRow = cRow + 1;
                });

                $('#tblMaterial thead tr th:last').after('<th class="xl65" x:str><span style="width:150px;float:left;text-align:center"> Material Unit </span></th>');
            }
        },
        error: function (data) {
            console.error(data);
        }
    });
}

function AppendEquipmentData(id) {
    var req = {};
    var year = $("#formB11Year").val();
    req.Year = year;
    $.ajax({
        url: '/FormB11/GetEquipmentHistoryData',
        dataType: 'JSON',
        data: req,
        type: 'Post',
        success: function (data) {
            debugger;
            if (data.result.length > 0) {
                var k = 0;
                for (var i = 0; i < data.result.length; i++) {
                    $('#tblEquipment thead tr th:last').after('<th class="xl65" x:str><span style="width:200px;float:left;text-align:center">' + data.result[i].b7ehName + ' Unit </span></th>');
                    $('#tblEquipment thead tr th:last').after('<th class="xl65" x:str><span style="width:200px;float:left;text-align:center">' + data.result[i].b7ehName + ' Rate </span></th>');
                }

                if ($("#txtB11RmuCode").val() == "Miri") {
                    $('#tblEquipment tbody tr').each(function () {
                        for (var i = 0; i < data.result.length; i++) {
                            var id = k + data.result[i].b7ehCode;
                            $(this).find("td:last").after('<td class="xl69"> <input type="text" id="etxt' + id + '" onkeyup="EquipmentCal(this, ' + k + ')" class="form-control" /></td>');
                            $(this).find("td:last").after('<td class="xl70' + i + '" data-lab=' + data.result.length + '><span style="display:none;">' + data.result[i].b7ehName + '</span><span id="esp' + id + '" style="display:none;">' + data.result[i].b7ehUnitPriceMiri + '</span><span class="x100' + k + '" id="espan' + id + '" style="width:150px;float:left;text-align:center"></span><span style="display:none;">' + data.result[i].b7ehCode + '</span></td>');
                        }
                        $(this).find("td:last").after('<td class="xl71" x:str><span id="esptot' + k + '" style="width:150px;float:left;text-align:center"></span></td>');
                        k = k + 1;
                    });
                }
                else {
                    $('#tblEquipment tbody tr').each(function () {
                        for (var i = 0; i < data.result.length; i++) {
                            var id = k + data.result[i].b7ehCode;
                            $(this).find("td:last").after('<td class="xl69"> <input type="text" id="etxt' + id + '" onkeyup="EquipmentCal(this, ' + k + ')" class="form-control" /></td>');
                            $(this).find("td:last").after('<td class="xl70' + i + '" data-lab=' + data.result.length + '><span style="display:none;">' + data.result[i].b7ehName + '</span><span id="esp' + id + '" style="display:none;">' + data.result[i].b7ehUnitPriceBatuNiah + '</span><span class="x100' + k + '" id="espan' + id + '" style="width:150px;float:left;text-align:center"></span><span style="display:none;">' + data.result[i].b7ehCode + '</span></td>');
                        }
                        $(this).find("td:last").after('<td class="xl71" x:str><span id="esptot' + k + '" style="width:150px;float:left;text-align:center"></span></td>');
                        k = k + 1;
                    });
                }

                $('#tblEquipment thead tr th:last').after('<th class="xl65" x:str><span style="width:150px;float:left;text-align:center"> Equipment Unit </span></th>');

            }
            //$("#RevisionNo").val(data)
        },
        error: function (data) {
            console.error(data);
        }
    });
}

function EquipmentCal(obj, k) {
    debugger;
    var Qty = 0;
    var Unit = 0;
    Qty = $('#' + obj.id).val();
    Unit = $('#esp' + obj.id.slice(4)).text();
    var tot = Qty * Unit;
    $('#espan' + obj.id.slice(4)).text(tot.toFixed(2))
    var sum = 0;
    $("#tblEquipment tbody tr td .x100" + k).each(function (index, tr) {
        debugger;
        if (!isNaN(this.innerText) && this.innerText != "") {
            sum += parseFloat(this.innerText);
        }
    });
    $('#esptot' + k).text(sum.toFixed(3));
}

function ViewEquipmentData(id) {
    debugger;
    var req = {};
    req.id = id;
    $.ajax({
        url: '/FormB11/GetEquipmentViewHistoryData',
        dataType: 'JSON',
        data: req,
        type: 'Post',
        success: function (data) {
            debugger;
            if (data.result.length > 0) {
                var k = 0;
                var cRow = 0;
                var labB7 = data.result.filter(function (el) { return el.b11ecEquipmentOrderId == 0; }).length;
                var labourB11 = data.result.map(p => p.b11ecEquipmentOrderId).filter((b11ecEquipmentOrderId, index, arr) => arr.indexOf(b11ecEquipmentOrderId) == index).length;
                for (var i = 0; i < labB7; i++) {
                    $('#tblEquipment thead tr th:last').after('<th class="xl65" x:str><span style="width:200px;float:left;text-align:center">' + data.result.filter(function (el) { return el.b11ecEquipmentOrderId == 0; })[i].b11ecEquipmentName + ' Unit </span></th>');
                    $('#tblEquipment thead tr th:last').after('<th class="xl65" x:str><span style="width:200px;float:left;text-align:center">' + data.result.filter(function (el) { return el.b11ecEquipmentOrderId == 0; })[i].b11ecEquipmentName + ' Rate </span></th>');
                }


                $('#tblEquipment tbody tr').each(function () {
                    var totallab = 0;
                    var activityID = 0;
                    for (var i = 0; i < labB7; i++) {
                        activityID = data.result.filter(function (el) { return el.b11ecEquipmentOrderId == cRow; })[0].b11ecActivityId;
                        var id = k + data.result.filter(function (el) { return el.b11ecEquipmentOrderId == cRow; })[i].b11ecEquipmentId;
                        var LabourUnit = data.result.filter(function (el) { return el.b11ecEquipmentOrderId == cRow; })[i].b11ecEquipmentNoOfUnits != null ? data.result.filter(function (el) { return el.b11ecEquipmentOrderId == cRow; })[i].b11ecEquipmentNoOfUnits : "";
                        var LabourUnitPrice = data.result.filter(function (el) { return el.b11ecEquipmentOrderId == cRow; })[i].b11ecEquipmentTotalPrice != null ? data.result.filter(function (el) { return el.b11ecEquipmentOrderId == cRow; })[i].b11ecEquipmentTotalPrice : "";
                        $(this).find("td:last").after('<td class="xl69"> <input type="text" id="etxt' + id + '" disabled value="' + LabourUnit + '"  class="form-control" /></td>');

                        $(this).find("td:last").after('<td class="xl70' + i + '" data-lab=' + labourB11 + '><span style="display:none;">' + data.result.filter(function (el) { return el.b11ecEquipmentOrderId == cRow; })[i].b11ecEquipmentName + '</span><span id="esp' + id + '" style="display:none;">' + data.result.filter(function (el) { return el.b11ecEquipmentOrderId == cRow; })[i].b11ecEquipmentPerUnitPrice + '</span><span class="x100' + k + '" id="espan' + id + '" style="width:150px;float:left;text-align:center">' + LabourUnitPrice + '</span><span style="display:none;">' + data.result.filter(function (el) { return el.b11ecEquipmentOrderId == cRow; })[i].b11ecEquipmentId + '</span></td>');

                        var rowlaboutot = data.result.filter(function (el) { return el.b11ecEquipmentOrderId == cRow; })[i].b11ecEquipmentTotalPrice != null ? data.result.filter(function (el) { return el.b11ecEquipmentOrderId == cRow; })[i].b11ecEquipmentTotalPrice : 0
                        totallab = totallab + parseFloat(rowlaboutot)
                    }
                    $('#' + activityID).text() != "" ? $('#' + activityID).text(parseFloat($('#' + activityID).text()) + totallab) : $('#' + activityID).text(parseFloat(totallab));
                    $(this).find("td:last").after('<td class="xl71" x:str><span id="esptot' + k + '" style="width:150px;float:left;text-align:center">' + totallab + '</span></td>');
                    k = k + 1;
                    cRow = cRow + 1;
                });

                $('#tblEquipment thead tr th:last').after('<th class="xl65" x:str><span style="width:150px;float:left;text-align:center"> Equipment Unit </span></th>');
            }
        },
        error: function (data) {
            console.error(data);
        }
    });
}
