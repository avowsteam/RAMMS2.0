
var frmB11 = new function () {
    this.HeaderData = {};
    this.IsEdit = true;

    this.Save = function () {
        var failed = false;

        if (failed)
            return;

        //  if (ValidatePage('#myModal')) {
        InitAjaxLoading();

        var FormB7 = new Object();
        FormB7.B7hPkRefNo = $("#FormB7Header_B7hPkRefNo").val()
        FormB7.B7hRevisionYear = $("#formB7Year").val()
        FormB7.B7hRevisionNo = $("#RevisionNo").val()
        FormB7.B7hRevisionDate = $("#date").val()
        FormB7.B7hCrBy = $("#UserId").val();
        FormB7.B7hCrByName = $("#UserName").val();
        FormB7.B7hCrDt = $("#date").val()

        var B7LabourHistory = []

        $('#tblLabour > tbody  > tr').each(function (index, tr) {

            var B7 = new Object();
            B7.B7lhB7hPkRefNo = $("#FormB7Header_B7hPkRefNo").val();
            B7.B7lhCode = $(this).find("td:nth-child(1)").html().trim();
            B7.B7lhName = $(this).find("td:nth-child(2)").html().trim();
            B7.B7lhUnitInHrs = $(this).find("td:nth-child(3)").html().trim();
            B7.B7lhUnitPriceBatuNiah = $(this).find("td:nth-child(4)").html().trim();
            B7.B7lhUnitPriceMiri = $(this).find("td:nth-child(5)").html().trim();
            B7.B7lhRevisionNo = $("#RevisionNo").val();
            B7LabourHistory.push(B7);
        });

        FormB7.RmB7LabourHistory = B7LabourHistory;


        var B7MaterialHistory = [];

        $('#tblMaterial > tbody  > tr').each(function (index, tr) {

            var B7 = new Object();
            B7.B7mhB7hPkRefNo = $("#FormB7Header_B7hPkRefNo").val();
            B7.B7mhCode = $(this).find("td:nth-child(1)").html().trim();
            B7.B7mhName = $(this).find("td:nth-child(2)").html().trim();
            B7.B7mhUnits = $(this).find("td:nth-child(3)").html().trim();
            B7.B7mhUnitPriceBatuNiah = $(this).find("td:nth-child(4)").html().trim();
            B7.B7mhUnitPriceMiri = $(this).find("td:nth-child(5)").html().trim();
            B7MaterialHistory.push(B7);
        });

        FormB7.RmB7MaterialHistory = B7MaterialHistory;

        var B7EquipmentHistory = [];

        $('#tblEquipment > tbody  > tr').each(function (index, tr) {

            var B7 = new Object();
            B7.B7ehB7hPkRefNo = $("#FormB7Header_B7hPkRefNo").val();
            B7.B7ehCode = $(this).find("td:nth-child(1)").html().trim();
            B7.B7ehName = $(this).find("td:nth-child(2)").html().trim();
            B7.B7ehUnitInHrs = $(this).find("td:nth-child(3)").html().trim();
            B7.B7ehUnitPriceBatuNiah = $(this).find("td:nth-child(4)").html().trim();
            B7.B7ehUnitPriceMiri = $(this).find("td:nth-child(5)").html().trim();
            B7EquipmentHistory.push(B7);
        });

        FormB7.RmB7EquipmentsHistory = B7EquipmentHistory;

        $.ajax({
            url: '/FormB7/SaveFormB7',
            data: FormB7,
            type: 'POST',
            success: function (data) {
                HideAjaxLoading();
                if (data == -1) {
                    app.ShowErrorMessage(data.errorMessage);
                }
                else {
                    app.ShowSuccessMessage('Saved Successfully', false);
                    location.href = "/FormB7";
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
            var actionSection = "<div class='btn-group dropright' rowidx='" + meta.row + "'><button type='button' class='btn btn-sm btn-themebtn dropdown-toggle' data-toggle='dropdown'> Click Me </button>";
            actionSection += "<div class='dropdown-menu'>";//dorpdown menu start

            if (tblFB11HGrid.Base.IsModify) {
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
                        window.location = _APPLocation + "FormB11/download?id=" + data.RefNo;
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
        if ($('#formB11Year').val() != "" && $('#formB11Year').val() != null) {
            AppendLabourData();
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

                $('#tblLabour tbody tr').each(function () {
                    for (var i = 0; i < data.result.length; i++) {
                        var id = k + data.result[i].b7lhCode;
                        $(this).find("td:last").after('<td class="xl65"> <input type="text" id="txt' + id + '" onkeyup="LabourCal(this)" class="form-control" /></td>');
                        $(this).find("td:last").after('<td class="xl65" x:str><span id="sp' + id + '" style="display:none;">' + data.result[i].b7lhUnitPriceBatuNiah+'</span><span id="span' + id + '" style="width:150px;float:left;text-align:center"></span></td>');
                    }
                    $(this).find("td:last").after('<td class="xl65" x:str><span id="sptot' + k + '" style="width:150px;float:left;text-align:center"></span></td>');
                    k = k + 1;
                });
                $('#tblLabour thead tr th:last').after('<th class="xl65" x:str><span style="width:150px;float:left;text-align:center"> Labour Unit </span></th>');

            }
            //$("#RevisionNo").val(data)
        },
        error: function (data) {
            console.error(data);
        }
    });
}

function LabourCal(obj) {
    debugger;
    var l = 0;
}