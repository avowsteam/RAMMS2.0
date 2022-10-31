var frmMap = new function () {
    this.HeaderData = {};
    this.IsEdit = true;
    this.IsAdd = false;
    this.FindDetails = function () {
        if (ValidatePage("#divFindDetailsMap")) {
            GetfindDetails();
        }
    }

    this.NavToList = function () {
        window.location = _APPLocation + "FormMap";
    }

    this.Cancel = function () {
        jsMaster.ConfirmCancel(() => { frmMap.NavToList(); });
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
        var action = isSubmit ? "/FormMap/Submit" : "/FormMap/SaveFormMap";
        var isvalid = true;
        debugger;

        var FormMapHDR = new Object();
        var FormMap = new Object();

        FormMapHDR.PkRefNo = $("#pkRefNo").val();
        FormMapHDR.RefId = $('#txtFormMapRefNum').val();
        FormMapHDR.RmuCode = $("#formMapRMU").val();
        FormMapHDR.RmuName = $('#formMapRMU').find("option:selected").text();
        FormMapHDR.Year = $("#formMapYear").val();
        FormMapHDR.Month = $("#formMapMonth").val();
        FormMapHDR.CrBy = $("#UserId").val();
        FormMapHDR.CrDt = $("#date").val();

        var MapDetails = []
        var j = 0;
        $('#tblLabour > tbody  > tr:even').each(function (index, tr) {
            debugger;
            var Map = new Object();
            var Year = $("#formMapYear").val();
            var Month = $("#formMapMonth").val();
            var dayCount = daysInMonth(Month, Year);
            for (var i = 1; i <= dayCount; i++) {
                var Map = new Object();
                var d = new Date(Year + '-' + Month + '-' + i);
                Map.PkRefNoDetails = frmMap.HeaderData.RmMapDetails.length == 0 ? 0 : frmMap.HeaderData.RmMapDetails[i].PkRefNoDetails;
                Map.PkRefNo = $("#pkRefNo").val();
                Map.ActivityId = $(this).find("td.x01").text().trim();
                Map.ActivityDate = Year + '-' + Month + '-' + i;
                Map.ActivityWeekDayNo = i;
                Map.ActivityLocationCode = $('#tdloc' + Map.ActivityId + i).text().trim();
                Map.QuantityKm = $('#tdqan' + Map.ActivityId + i).text().trim();
                Map.ProductUnit = $('#sp0' + Map.ActivityId).text();
                Map.Order = j;
                MapDetails.push(Map);
                j = j + 1;
            }
        });

        FormMap = MapDetails;
        var FormMapData = JSON.stringify(FormMap);
        var FormMapHDRData = JSON.stringify(FormMapHDR);


        $.ajax({
            url: action,
            data: { formMaphdrdata: FormMapHDRData, formMapdata: FormMapData, reload: IsReload },
            type: 'POST',
            async: false,
            success: function (data) {
                HideAjaxLoading();
                if (data == -1) {
                    app.ShowErrorMessage(data.errorMessage);
                }
                else {
                    if (IsReload == 0) {
                        app.ShowSuccessMessage('Saved Successfully', false);
                        location.href = "/FormMap";
                    }
                }
            }
        });

    }

    this.NavToList = function () {
        window.location = _APPLocation + "FormMap";
    }

    this.Cancel = function () {
        jsMaster.ConfirmCancel(() => { frmMap.NavToList(); });
    }

    this.HeaderGrid = new function () {
        this.ActionRender = function (data, type, row, meta) {
            var actionSection = "<div class='btn-group dropright' rowidx='" + meta.row + "'><button type='button' class='btn btn-sm btn-themebtn dropdown-toggle' data-toggle='dropdown'> Click Me </button>";
            actionSection += "<div class='dropdown-menu'>";//dorpdown menu start

            if (data.Status != "Submitted" && tblFMapHGrid.Base.IsModify) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmMap.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='edit-icon'></span> Edit </button>";
            }
            if (tblFMapHGrid.Base.IsView) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmMap.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='view-icon'></span> View </button>";
            }
            if (data.Status != "Submitted" && tblFMapHGrid.Base.IsDelete) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmMap.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='del-icon'></span> Delete </button>";
            }
            actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmMap.HeaderGrid.ActionClick(this);'>";
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
                var data = tblFMapHGrid.dataTable.row(rowidx).data();
                switch (type.toLowerCase()) {
                    case "edit":
                        window.location = _APPLocation + "FormMap/Edit/" + data.RefNo;
                        break;
                    case "view":
                        window.location = _APPLocation + "FormMap/View/" + data.RefNo;
                        break;
                    case "delete":
                        app.Confirm("Are you sure you want to delete this record? <br/>(Ref: " + data.RefNo + ")", (status) => {
                            if (status) {
                                DeleteRequest("Delete/" + data.RefNo, "FormMap", {}, function (sdata) {
                                    //if (sdata.id == "-1") {
                                    //    app.ShowErrorMessage("Form Map cannot be deleted, first delete Form F3");
                                    //    return false;
                                    //}
                                    tblFMapHGrid.Refresh();
                                    app.ShowSuccessMessage("Deleted Sucessfully! <br/>(Ref: " + data.RefNo + ")");
                                });
                            }
                        }, "Yes", "No");
                        break;
                    case "print":
                        window.location = _APPLocation + "FormMap/download?id=" + data.RefNo;
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
                $("#formMapRMU").prop("disabled", true).trigger("chosen:updated");
                $("#formMapYear").prop("disabled", true).trigger("chosen:updated");
                $("#formMapMonth").prop("disabled", true).trigger("chosen:updated");
                if (this.HeaderData.FormMapHeader != undefined)
                    AppendData($("#pkRefNo").val(), this.HeaderData.FormMapHeader.Status);
                else
                    AppendData($("#pkRefNo").val(), this.HeaderData.Status);
                $("[finddetailsdep]").show();
                $("#btnFindDetails").hide();
            }
            else {
                $('#formMapRMU').attr("disabled", false).trigger("chosen:updated");
                $("#formMapYear").prop("disabled", false).trigger("chosen:updated");
                $("#formMapMonth").prop("disabled", false).trigger("chosen:updated");
                $("[finddetailsdep]").hide();
                $("#btnFindDetails").show();
            }
        }
        else {
            $("#formMapRMU").prop("disabled", true).trigger("chosen:updated");
            $("#formMapYear").prop("disabled", true).trigger("chosen:updated");
            $("#formMapMonth").prop("disabled", true).trigger("chosen:updated");
            $("[finddetailsdep]").hide();
            $("#btnFindDetails").hide();
            ViewData($("#pkRefNo").val());
        }
    }
}

function GetfindDetails() {
    debugger;
    InitAjaxLoading();
    var FormMap = new Object();

    FormMap.RmuCode = $("#formMapRMU").val();
    FormMap.RmuName = $('#formMapRMU').find("option:selected").text();
    FormMap.Year = $("#formMapYear").val();
    FormMap.Month = $("#formMapMonth").val();
    var FormMapData = JSON.stringify(FormMap);
    $.ajax({
        url: '/FormMap/FindDetails',
        type: 'POST',
        data: { formMapdata: FormMapData },
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
                //    window.location = _APPLocation + "FormMap/View/" + data.PkRefNo;
                //}
                frmMap.HeaderData = data;
                $('#txtFormMapRefNum').val(data.RefId);
                $("#pkRefNo").val(frmMap.HeaderData.PkRefNo);
                frmMap.PageInit();

            }
        }
    });
}

$(document).ready(function () {
    $("#smartSearch").focus();//Header Grid focus    

    frmMap.PageInit();

    $("[useridChange]").on("change", function () {
        frmMap.UserIdChange(this);
    });

    if (!frmMap.IsEdit) {
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

});

function daysInMonth(month, year) {
    return new Date(year, month, 0).getDate();
}

function GetDayName(date) {
    var d = new Date(date).getDay();
    var weekday = new Array(7);
    weekday[0] = "Sun";
    weekday[1] = "Mon";
    weekday[2] = "Tue";
    weekday[3] = "Wed";
    weekday[4] = "Thu";
    weekday[5] = "Fri";
    weekday[6] = "Sat";

    var d = new Date(date);
    return weekday[d.getDay()];
}

function AppendData(id, Status) {
    debugger;
    var Year = $("#formMapYear").val();
    var Month = $("#formMapMonth").val();
    var dayCount = daysInMonth(Month, Year);
    for (var i = 1; i <= dayCount; i++) {
        debugger;
        $('#tblLabour thead tr:eq(1) th:last').after('<th class="xl65" x:str><span style="width:70px;float:left;text-align:center">' + GetDayName(Month + '/' + i + '/' + Year) + ' </span></th>');
        $('#tblLabour thead tr:eq(2) th:last').after('<th class="xl65" x:str><span style="width:70px;float:left;text-align:center">' + i + ' </span></th>');
        $('#tblLabour tbody tr:even').each(function () {
            var actCode = $(this).find(".x01").text();
            $(this).find("td:last").after('<td id="tdloc' + actCode + i + '" style="width:80px;border-left:1px solid #dee2e6;"></td>');
        });
        $('#tblLabour tbody tr:odd').each(function () {
            var actCode = $(this).find(".sp02").text();
            $(this).find("td:last").after('<td id="tdqan' + actCode + i + '" style="width:80px;border-left:1px solid #dee2e6;"></td>');
        });
    }
    AppendPlannedData();
    //AppendWeek();
}

function AppendPlannedData() {
    debugger;
    var req = {};
    var rmucode = $('#formMapRMU').val();
    var year = $("#formMapYear").val();
    var month = $("#formMapMonth").val();
    req.RmuCode = rmucode;
    req.Year = year;
    req.Month = month;
    $.ajax({
        url: '/FormMap/GetForDDetails',
        dataType: 'JSON',
        data: req,
        async: false,
        type: 'Post',
        success: function (data) {
            debugger;
            if (data.result.length > 0) {
                for (var i = 0; i < data.result.length; i++) {
                    var d = new Date(data.result[i].weekDate);
                    var dateId = d.getDate();
                    var actCode = "";
                    var prodUnit = "";
                    var prodQty = 0;
                    var roadCode = "";
                    for (var j = 0; j < data.result[i].formDDetails.length; j++) {
                        if (j == 0) {
                            actCode = data.result[i].formDDetails[j].actCode;
                            prodUnit = data.result[i].formDDetails[j].prodUnit;
                            prodQty = data.result[i].formDDetails[j].prodQty;
                            roadCode = data.result[i].formDDetails[j].roadCode;
                            $('#tdloc' + actCode + dateId).text(roadCode);
                            $('#tdqan' + actCode + dateId).text(prodQty);
                            $('#sp0' + actCode).text("Quantity (" + prodUnit + ")");
                            $('#sp' + actCode).text(data.result[i].weekDate);
                        }
                        else if (actCode == data.result[i].formDDetails[j].actCode) {
                            prodQty = parseInt(prodQty) + parseInt(data.result[i].formDDetails[j].prodQty);
                            roadCode = roadCode + "," + data.result[i].formDDetails[j].roadCode;
                            $('#tdloc' + actCode + dateId).text(roadCode);
                            $('#tdqan' + actCode + dateId).text(prodQty);
                            $('#sp' + actCode).text(data.result[i].weekDate);
                        }
                        else {
                            actCode = data.result[i].formDDetails[j].actCode;
                            prodUnit = data.result[i].formDDetails[j].prodUnit;
                            prodQty = data.result[i].formDDetails[j].prodQty;
                            roadCode = data.result[i].formDDetails[j].roadCode;
                            $('#tdloc' + actCode + dateId).text(roadCode);
                            $('#tdqan' + actCode + dateId).text(prodQty);
                            $('#sp0' + actCode).text("Quantity (" + prodUnit + ")");
                            $('#sp' + actCode).text(data.result[i].weekDate);
                        }


                    }
                }
            }
        },
        error: function (data) {
            console.error(data);
        }
    });
}

function ViewData(id) {
    var Year = $("#formMapYear").val();
    var Month = $("#formMapMonth").val();
    var dayCount = daysInMonth(Month, Year);
    for (var i = 1; i <= dayCount; i++) {
        $('#tblLabour thead tr:eq(1) th:last').after('<th class="xl65" x:str><span style="width:70px;float:left;text-align:center">' + GetDayName(Month + '/' + i + '/' + Year) + ' </span></th>');
        $('#tblLabour thead tr:eq(2) th:last').after('<th class="xl65" x:str><span style="width:70px;float:left;text-align:center">' + i + ' </span></th>');
        $('#tblLabour tbody tr:even').each(function () {
            var actCode = $(this).find(".x01").text();
            $(this).find("td:last").after('<td id="tdloc' + actCode + i + '" style="width:80px;border-left:1px solid #dee2e6;"></td>');
        });
        $('#tblLabour tbody tr:odd').each(function () {
            var actCode = $(this).find(".sp02").text();
            $(this).find("td:last").after('<td id="tdqan' + actCode + i + '" style="width:80px;border-left:1px solid #dee2e6;"></td>');
        });
    }

    //Append Data
    var req = {};
    var detailID = id;
    req.ID = detailID;
    $.ajax({
        url: '/FormMap/GetForMapDetails',
        dataType: 'JSON',
        data: req,
        async: false,
        type: 'Post',
        success: function (data) {
            debugger;
            if (data.result.length > 0) {
                for (var i = 0; i < data.result.length; i++) {
                    var actCode = data.result[i].activityId;
                    var weekdayNo = data.result[i].activityWeekDayNo;
                    var prodUnit = data.result[i].productUnit;
                    var prodQty = data.result[i].quantityKm;
                    var roadCode = data.result[i].activityLocationCode;
                    $('#tdloc' + actCode + weekdayNo).text(roadCode);
                    $('#tdqan' + actCode + weekdayNo).text(prodQty);
                    $('#sp0' + actCode).text("Quantity (" + prodUnit + ")");
                }
            }
        },
        error: function (data) {
            console.error(data);
        }
    });

    //AppendWeek();
}

function GetWeekNumber(year, month, day) {
    debugger;
    //var target = new Date(year, month, day);
    //var dayNr = (target.getDay() + 6) % 7;
    //target.setDate(target.getDate() - dayNr + 3);
    //var firstThursday = target.valueOf();
    //target.setMonth(0, 1);
    //if (target.getDay() != 4) {
    //    target.setMonth(0, 1 + ((4 - target.getDay()) + 7) % 7);
    //}
    //return 1 + Math.ceil((firstThursday - target) / 604800000);
    Date.prototype.getWeek = function () {
        var onejan = new Date(year, month, day);
        return Math.ceil((((this - onejan) / 86400000) + onejan.getDay() + 1) / 7);
    }

    return (new Date()).getWeek();
}

function AppendWeek() {
    var Year = $("#formMapYear").val();
    var Month = $("#formMapMonth").val();
    var dayCount = daysInMonth(Month, Year);
    var weekCount = 0;
    var ExistweekNumber = 0;
    var currweekNumber = 0;
    for (var i = 1; i <= dayCount; i++) {
        debugger;
        currweekNumber = GetWeekNumber(Year, Month, i);
        if (i == 1) {
            ExistweekNumber = currweekNumber;
        }
        if (currweekNumber != ExistweekNumber) {
            $('#tblLabour thead tr:eq(0) th:last').after('<th class="xl65" colspan=' + weekCount + ' x:str style="border-left:1px solid #dee2e6;"><span style="width:70px;float:left;text-align:center"> Week' + ExistweekNumber + ' </span></th>');
            ExistweekNumber = currweekNumber;
            weekCount = 1;
        }
        else {
            weekCount = weekCount + 1;
        }
        if (i == dayCount) {
            $('#tblLabour thead tr:eq(0) th:last').after('<th class="xl65" colspan=' + weekCount + ' x:str style="border-left:1px solid #dee2e6;"><span style="width:70px;float:left;text-align:center"> Week' + currweekNumber + ' </span></th>');
        }
    }
}