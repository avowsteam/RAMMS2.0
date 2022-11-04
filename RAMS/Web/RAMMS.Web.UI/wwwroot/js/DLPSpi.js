
$(document).ready(function () {
   
    $("#formDlpYear").on("change", function () {
        getData($("#formDlpYear").val());
    });

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
                if ($this.val().substring($this.val().indexOf('.')).length > 2) {
                    $this.val($this.val().substring(0, $this.val().indexOf('.') + 2));
                }
            }, 1);
        }

        if ((text.indexOf('.') != -1) &&
            (text.substring(text.indexOf('.')).length > 2) &&
            (event.which != 0 && event.which != 8) &&
            ($(this)[0].selectionStart >= text.length - 2)) {
            event.preventDefault();
        }
    });


});

function getData(year) {
    var req = {};
    req.Year = year;
    $.ajax({
        url: '/DLP/GetSPIByYearFormSPI',
        dataType: 'JSON',
        data: req,
        type: 'Post',
        success: function (data) {
            $('#tblDivision tbody tr').remove();
            if (data.divisionMiri.length == 0) {
                $('#tblDivision tbody').append("<tr><td colspan='13'>No Records Found</td>"
                    + "</tr>");
            }
            for (var i = 0; i < data.divisionMiri.length; i++) {

                $('#tblDivision tbody').append("<tr>"
                    + "<td><b>" + GetMonthName(data.divisionMiri[i].month) + "</b></td>"
                    + "<td>RM " + assignZero(data.divisionMiri[i].mPlanned) + "</td>"
                    + "<td>RM " + assignZero(data.divisionMiri[i].mActual) + "</td>"
                    + "<td>RM " + assignZero(data.divisionMiri[i].cPlan) + "</td>"
                    + "<td>RM " + assignZero(data.divisionMiri[i].cActual) + "</td>"
                    + "<td>RM " + assignZero(data.divisionMiri[i].piWorkActual) + "</td>"
                    + "<td>" + assignZero(data.divisionMiri[i].pai) + "</td>"
                    + "<td>" + assignZero(data.divisionMiri[i].eff) + "</td>"
                    + "<td>" + assignZero(data.divisionMiri[i].rb) + "</td>"
                    + "<td>" + assignZero(data.divisionMiri[i].iw) + "</td>"
                    + "<td>" + assignZero(data.divisionMiri[i].spi) + "</td>"
                    + "<td>" + assignZero(data.divisionMiri[i].plannedPer) + " %</td>"
                    + "<td>" + assignZero(data.divisionMiri[i].actualPer) + " %</td>"
                    + "</tr>");
            }
            $('#tblMiri tbody tr').remove();
            $("#editMiri").show();
            if (data.rmuMiri.length == 0) {
                $('#tblMiri tbody').append("<tr><td colspan='13'>No Records Found</td>"
                    + "</tr>");
                $("#editMiri").hide();
                $("#saveMiri").hide();
                $("#cancelMiri").hide();

            }
            for (var i = 0; i < data.rmuMiri.length; i++) {
                $('#tblMiri tbody').append("<tr id='miri_" + i + "' pkid='" + data.rmuMiri[i].pkRefNo + "'>"
                    + "<td><b>" + GetMonthName(data.rmuMiri[i].month) + "</b></td>"
                    + "<td>RM " + assignZero(data.rmuMiri[i].mPlanned) + "</td>"
                    + "<td>RM " + assignZero(data.rmuMiri[i].mActual) + "</td>"
                    + "<td>RM " + assignZero(data.rmuMiri[i].cPlan) + "</td>"
                    + "<td>RM " + assignZero(data.rmuMiri[i].cActual) + "</td>"
                    + "<td>RM " + assignZero(data.rmuMiri[i].piWorkActual) + "</td>"
                    + "<td class='editpi'>" + assignZero(data.rmuMiri[i].pai) + "</td>"
                    + "<td class='editeff allow_numeric'>" + assignZero(data.rmuMiri[i].eff) + "</td>"
                    + "<td class='editrb allow_numeric'>" + assignZero(data.rmuMiri[i].rb) + "</td>"
                    + "<td class='editiw allow_numeric'>" + assignZero(data.rmuMiri[i].iw) + "</td>"
                    + "<td class='editspi'>" + assignZero(data.rmuMiri[i].spi) + "</td>"
                    + "<td>" + assignZero(data.rmuMiri[i].plannedPer) + " %</td>"
                    + "<td>" + assignZero(data.rmuMiri[i].actualPer) + " %</td>"
                    + "</tr>");
            }
            $('#tblBtn tbody tr').remove();
            $("#editBtn").show();
            if (data.rmuBTN.length == 0) {
                $('#tblBtn tbody').append("<tr><td colspan='13'>No Records Found</td>"
                    + "</tr>");
                $("#editBtn").hide();
                $("#saveBtn").hide();
                $("#cancelBtn").hide();
            }
            for (var i = 0; i < data.rmuBTN.length; i++) {
                $('#tblBtn tbody').append("<tr id='btn_" + i + "' pkid='" + data.rmuBTN[i].pkRefNo + "'>"
                    + "<td><b>" + GetMonthName(data.rmuBTN[i].month) + "</b></td>"
                    + "<td>RM " + assignZero(data.rmuBTN[i].mPlanned) + "</td>"
                    + "<td>RM " + assignZero(data.rmuBTN[i].mActual) + "</td>"
                    + "<td>RM " + assignZero(data.rmuBTN[i].cPlan) + "</td>"
                    + "<td>RM " + assignZero(data.rmuBTN[i].cActual) + "</td>"
                    + "<td>RM " + assignZero(data.rmuBTN[i].piWorkActual) + "</td>"
                    + "<td class='editpi'>" + assignZero(data.rmuBTN[i].pai) + "</td>"
                    + "<td class='editeff allow_numeric'>" + assignZero(data.rmuBTN[i].eff) + "</td>"
                    + "<td class='editrb allow_numeric'>" + assignZero(data.rmuBTN[i].rb) + "</td>"
                    + "<td class='editiw allow_numeric'>" + assignZero(data.rmuBTN[i].iw) + "</td>"
                    + "<td class='editspi'>" + assignZero(data.rmuBTN[i].spi) + "</td>"
                    + "<td>" + assignZero(data.rmuBTN[i].plannedPer) + " %</td>"
                    + "<td>" + assignZero(data.rmuBTN[i].actualPer) + " %</td>"
                    + "</tr>");
            }
        },
        error: function (data) {
            console.error(data);
        }
    });
}

function assignZero(val) {
    return typeof val == 'number' ? val.toFixed(2) : '0.00';
}

function GetMonthName(mnth) {
    switch (mnth) {
        case 1:
            return "January";
        case 2:
            return "February";
        case 3:
            return "March";
        case 4:
            return "April";
        case 5:
            return "May";
        case 6:
            return "June";
        case 7:
            return "July";
        case 8:
            return "August";
        case 9:
            return "September";
        case 10:
            return "October";
        case 11:
            return "November";
        case 12:
            return "December";
        default:
            return "";
    }
}

function Edit(obj) {
    $("#tbl" + obj + " tbody tr").each(function () {
        var one = $(this).find('.editeff').html();
        $(this).find('.editeff').html("<input class='allow_numeric editp {lt,15,PI (EFF)}' type='text' style='width:80px' preval='" + one + "' value='" + one + "' />")
        var two = $(this).find('.editrb').html();
        $(this).find('.editrb').html("<input  class='allow_numeric editr {lt,5,PI (RB)}'  type='text' style='width:80px'   preval='" + two + "'  value='" + two + "' />")
        var three = $(this).find('.editiw').html();
        $(this).find('.editiw').html("<input type='text'  class='allow_numeric editr {lt,5,PI (IW)}'  style='width:80px'   preval='" + three + "'  value='" + three + "' />")
        var four = $(this).find('.editspi').html();
        $(this).find('.editspi').html("<input type='text' disabled  style='width:80px'   preval='" + four + "'  value='" + four + "' />")
    });

    $("#edit" + obj).hide();
    $("#save" + obj).show();
    $("#cancel" + obj).show();

    $('.editp').blur(function (event) {

        var eff = $(this).val();

        if (eff != "" && +eff > 15) {
            app.ShowErrorMessage("Value cannot be more than 15.00");
            $(this).val('');
            $(this).focus();
            return false;
        }
        Calculate($(this));
        return true;
    });

    $('.editr').blur(function (event) {
        var rb = $(this).val();
        if (rb != "" && +rb > 5) {
            app.ShowErrorMessage("Value cannot be more than 5.00");
            $(this).val('');
            $(this).focus();
            return false;
        }
        Calculate($(this));
        return true;
    });
}

function Calculate(obj) {
    var tr = $(obj).closest("tr");
    var pi = $(tr).find(".editpi").html();
    pi = (pi != "" ? parseFloat(pi) : 0) * 80;

    var eff = $(tr).find(".editeff").find("input").val();
    eff = (eff != "" ? parseFloat(eff) : 0);

    var rb = $(tr).find(".editrb").find("input").val();
    rb = (rb != "" ? parseFloat(rb) : 0);

    var iw = $(tr).find(".editiw").find("input").val();
    iw = (iw != "" ? parseFloat(iw) : 0);

    var spi = $(tr).find(".editspi").find("input");
    spi.val(parseFloat(pi + eff + rb - iw))
}

function Save(obj) {
    InitAjaxLoading();
    var Spi = [];

    $("#tbl" + obj + " tbody tr").each(function () {
        var one = $(this).find('.editeff').find("input");
        var two = $(this).find('.editrb').find("input");
        var three = $(this).find('.editiw').find("input");
        var four = $(this).find('.editspi').find("input");

        var eff = one.val();
        var rb = two.val();
        var iw = three.val();
        var spi = four.val();

        if (eff != "" && +eff > 15) {
            $(one).addClass("validate");
        } else {
            $(one).removeClass("validate");
            $(one).removeClass("border-error");
        }


        if (rb != "" && +rb > 5) {
            $(two).addClass("validate");
        } else {
            $(two).removeClass("validate");
            $(two).removeClass("border-error");
        }

        if (iw != "" && +iw > 5) {
            $(three).addClass("validate");
        } else {
            $(three).removeClass("validate");
            $(three).removeClass("border-error");
        }


        var row = new Object();
        row.id = $(this).attr("pkid");
        row.eff = eff;
        row.rb = rb;
        row.iw = iw;
        row.spi = spi;
        Spi.push(row);
    });
    if (ValidatePage('#divSPI')) {
        $.ajax({
            url: '/DLP/Save',
            data: { spiData: Spi },
            type: 'POST',
            success: function (data) {
                HideAjaxLoading();
                if (data == -1) {
                    app.ShowErrorMessage(data.errorMessage);
                }
                else {
                    app.ShowSuccessMessage('Saved Successfully', false);
                    location.href = "/DLP/FormSpi?year=" + $("#formDlpYear").val();
                }
            }
        });

        $("#edit" + obj).show();
        $("#save" + obj).hide();
        $("#cancel" + obj).hide();
    }
    else
        HideAjaxLoading();

}

function Cancel(obj) {
    $("#tbl" + obj + " tbody tr").each(function () {
        var one = $(this).find('.editeff').find("input").attr("preval");
        $(this).find('.editeff').html(one);
        var two = $(this).find('.editrb').find("input").attr("preval");
        $(this).find('.editrb').html(two);
        var three = $(this).find('.editiw').find("input").attr("preval");
        $(this).find('.editiw').html(three);
        var four = $(this).find('.editspi').find("input").attr("preval");
        $(this).find('.editspi').html(four);
    });

    $("#edit" + obj).show();
    $("#save" + obj).hide();
    $("#cancel" + obj).hide();
}

function Sync() {
    InitAjaxLoading();
    $.ajax({
        url: '/DLP/Sync',
        data: { year: $("#formDlpYear").val() },
        type: 'POST',
        success: function (data) {
            HideAjaxLoading();
            if (data == -1) {
                app.ShowErrorMessage(data.errorMessage);
            }
            else {
                app.ShowSuccessMessage('Saved Successfully', false);
                location.href = "/DLP/FormSpi?year=" + $("#formDlpYear").val();
            }
        }
    });
}