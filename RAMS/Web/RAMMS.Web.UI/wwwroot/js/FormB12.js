
var frmB12 = new function () {
    this.HeaderData = {};
    this.IsEdit = true;
    this.FindDetails = function () {
        if (ValidatePage("#divFindDetailsB12")) {
            GetfindDetails();
        }
    };

    this.Save = function () {
        var failed = false;

        if (failed)
            return;

        //  if (ValidatePage('#myModal')) {
        InitAjaxLoading();

        var FormB12 = new Object();
        FormB12.B12hPkRefNo = $("#pkRefNo").val()
        FormB12.B12hRevisionYear = $("#formB12Year").val()
        FormB12.B12hRevisionNo = $("#RevisionNo").val()
        FormB12.B12hRevisionDate = $("#date").val()
        FormB12.B12hCrBy = $("#UserId").val();
        FormB12.B12hCrByName = $("#UserName").val();
        FormB12.B12hCrDt = $("#date").val()

        var B12History = []

        $('#tblB12 > tbody  > tr').each(function (index, tr) {

            var B12 = new Object();
            B12.B12hPkRefNo = $("#pkRefNo").val();
            B12.ActCode = $(this).find("td:nth-child(2)").text().trim();
            B12.ActName = $(this).find("td:nth-child(3)").text().trim();
            B12.UnitOfService = $(this).find("td:nth-child(4)").text().trim();
            B12.RmuBatuniah = $(this).find("td:nth-child(5)").text().trim();
            B12.RmuMiri = $(this).find("td:nth-child(6)").text().trim();
            B12History.push(B12);
        });

        FormB12.FormB12History = B12History;

        $.ajax({
            url: '/FormB12/SaveFormB12',
            data: FormB12,
            type: 'POST',
            success: function (data) {
                HideAjaxLoading();
                if (data == -1) {
                    app.ShowErrorMessage(data.errorMessage);
                }
                else {
                    app.ShowSuccessMessage('Saved Successfully', false);
                    location.href = "/FormB12";
                }
            }
        });
    }

    this.NavToList = function () {
        window.location = _APPLocation + "FormB12";
    }

    this.Cancel = function () {
        jsMaster.ConfirmCancel(() => { frmB12.NavToList(); });
    }


    this.HeaderGrid = new function () {
        this.ActionRender = function (data, type, row, meta) {
            var actionSection = "<div class='btn-group dropright' rowidx='" + meta.row + "'><button type='button' class='btn btn-sm btn-themebtn dropdown-toggle' data-toggle='dropdown'> Click Me </button>";
            actionSection += "<div class='dropdown-menu'>";//dorpdown menu start

            if (data.MaxRecord) { //if (tblFB12HGrid.Base.IsModify) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB12.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='edit-icon'></span> Edit </button>";
            }
            if (tblFB12HGrid.Base.IsView) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB12.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='view-icon'></span> View </button>";
            }
            if (tblFB12HGrid.Base.IsDelete) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB12.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='del-icon'></span> Delete </button>";
            }
            actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB12.HeaderGrid.ActionClick(this);'>";
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
                var data = tblFB12HGrid.dataTable.row(rowidx).data();
                switch (type.toLowerCase()) {
                    case "edit":
                        window.location = _APPLocation + "FormB12/EditForm/" + data.RefNo;
                        break;
                    case "view":
                        window.location = _APPLocation + "FormB12/View/" + data.RefNo;
                        break;
                    case "delete":
                        app.Confirm("Are you sure you want to delete this record? <br/>(Ref: " + data.RefID + ")", (status) => {
                            if (status) {
                                DeleteRequest("Delete/" + data.RefNo, "FormB12", {}, function (sdata) {
                                    if (sdata.id == "-1") {
                                        app.ShowErrorMessage("Form B12 cannot be deleted, first delete Form F3");
                                        return false;
                                    }
                                    tblFB12HGrid.Refresh();
                                    app.ShowSuccessMessage("Deleted Sucessfully! <br/>(Ref: " + data.RefID + ")");
                                });
                            }
                        }, "Yes", "No");
                        break;
                    case "print":
                        window.location = _APPLocation + "FormB12/download?id=" + data.RefNo;
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

    this.PageInit = function () {
        debugger;
        if (this.IsEdit) {
            //if (this.HeaderData.FormB12Header && this.HeaderData.FormB12Header.PkRefNo && this.HeaderData.FormB12Header.PkRefNo > 0) {
            if ($("#pkRefNo").val() != "" && $("#pkRefNo").val() > 0) {
               
                $("#formB12Year").prop("disabled", true).trigger("chosen:updated");
                if (this.HeaderData.FormB12Header != undefined)
                    AppendData($("#pkRefNo").val(), this.HeaderData.FormB12Header.Status);
                else
                    AppendData($("#pkRefNo").val(), this.HeaderData.Status);
                $("[finddetailsdep]").show();
                $("#btnFindDetails").hide();
            }
            else {
               
                $("#formB12Year").prop("disabled", false).trigger("chosen:updated");
                $("[finddetailsdep]").hide();
                $("#btnFindDetails").show();
            }
        }
        else {
            
            $("#formB12Year").prop("disabled", true).trigger("chosen:updated");
            $("[finddetailsdep]").hide();
            $("#btnFindDetails").hide();
            ViewData($("#pkRefNo").val());
        }
    }
}

function AppendData(id, Status) {
    //debugger;
    var req = {};
    req.HistoryID = id;
    $.ajax({
        url: '/FormB12/GetHistoryData',
        type: 'POST',
        data: req,
        dataType: "json",
        success: function (data) {
            if (data.result.length > 0) {
                var i = 0;
                $('#tblB12 tbody tr').each(function () {
                    var UnitOfService = data.result[i].unitOfService == null ? "" : data.result[i].unitOfService;
                    var miri = data.result[i].rmuMiri == null ? "" : Number(data.result[i].rmuMiri).toLocaleString('en');
                    var btn = data.result[i].rmuBatuniah == null ? "" : Number(data.result[i].rmuBatuniah).toLocaleString('en');
                    $(this).find("td:last").after('<td stlye="float:right" id="txt' + i + 'Unit">' + UnitOfService + '</td>');
                    $(this).find("td:last").after('<td  stlye="float:right"  id="txt' + i + 'M2">' + btn + '</td>');
                    $(this).find("td:last").after('<td  stlye="float:right" id="txt' + i + 'M3">' + miri + '</td>');
                    i = i + 1;
                });
            }
            else {
                
                var i = 0;
                $('#tblB12 tbody tr').each(function () {
                    $(this).find("td:last").after('<td stlye="float:right" id="txt' + i + 'Unit"></td>');
                    $(this).find("td:last").after('<td  stlye="float:right"  id="txt' + i + 'M2">0.00</td>');
                    $(this).find("td:last").after('<td  stlye="float:right" id="txt' + i + 'M3">0.00</td>');
                    i = i + 1;
                });
            }
            AppendPlannedDataMiri();
            AppendPlannedDataBTN();
            AppendUnitData();
        }
    });

    function AppendPlannedDataMiri() {
        //debugger;
        var req = {};
        var year = $("#formB12Year").val();
        req.Year = year;
        $.ajax({
            url: '/FormB12/GetPlannedBudgetDataMiri',
            dataType: 'JSON',
            data: req,
            type: 'Post',
            success: function (data) {
                debugger
                if (data.result.length > 0) {
                    for (var i = 0; i < data.result.length; i++) {
                        var val = data.result[i].slTotalByActivity / data.result[i].slAnnualWorkQuantity;
                        val = isNaN(val) ? "0.00" : val.toFixed(2);
                        $('#txt' + i + "M3").html(val);
                    }
                }
            },
            error: function (data) {
                console.error(data);
            }
        });
    }

    function AppendPlannedDataBTN() {
        //debugger;
        var req = {};
        var year = $("#formB12Year").val();
        req.Year = year;
        $.ajax({
            url: '/FormB12/GetPlannedBudgetDataBTN',
            dataType: 'JSON',
            data: req,
            type: 'Post',
            success: function (data) {
                if (data.result.length > 0) {
                    for (var i = 0; i < data.result.length; i++) {
                        var val = data.result[i].slTotalByActivity / data.result[i].slAnnualWorkQuantity;
                        val = isNaN(val) ? "0.00" : val.toFixed(2);
                        $('#txt' + i + "M2").html(val);
                    }
                }
            },
            error: function (data) {
                console.error(data);
            }
        });
    }

    function AppendUnitData() {
     
        var req = {};
        var year = $("#formB12Year").val();
        req.Year = year;
        $.ajax({
            url: '/FormB12/GetUnitData',
            dataType: 'JSON',
            data: req,
            type: 'Post',
            success: function (data) {
                if (data.result.length > 0) {
                    debugger;
                    for (var i = 0; i < data.result.length; i++) {
                        $('#txt' + i + "Unit").html(data.result[i].adpUnit);
                    }
                }
            },
            error: function (data) {
                console.error(data);
            }
        });
    }



}


$(document).ready(function () {    
    $("#smartSearch").focus();//Header Grid focus    

    if (!frmB12.IsEdit) {
        $("#formB12Year").chosen("destroy");
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

  
    $("#formB12Year").on("change", function () {
        getRevisionNo($("#formB12Year").val());
    });

});

function getRevisionNo(id) {
    var req = {};
    req.Year = id;
    $.ajax({
        url: '/FormB12/GetMaxRev',
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

function GetfindDetails() {
    debugger;
    InitAjaxLoading();
    var FormB12 = new Object();

    FormB12.RevisionYear = $("#formB12Year").val();
    FormB12.RevisionDate = $("#date").val();
    FormB12.RevisionNo = $("#RevisionNo").val();
    var FormB12Data = JSON.stringify(FormB12);
    $.ajax({
        url: '/FormB12/FindDetails',
        type: 'POST',
        data: { formb12data: FormB12Data },
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
                //    window.location = _APPLocation + "FormB12/View/" + data.PkRefNo;
                //}
                frmB12.HeaderData = data;
                $('#txtFormB12RefNum').val(data.PkRefId);
                $("#pkRefNo").val(frmB12.HeaderData.PkRefNo);
                frmB12.PageInit();

            }
        }
    });
}
