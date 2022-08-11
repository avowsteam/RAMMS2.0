


$(document).ready(function () {
 
    $('.typein').on('click', function (e) {
        if ($("#hdnview").val() != 1) {
            var $this = $(this);
            typein($this);
            e.preventDefault();
        }
    });

    $('.dropdown').on('click', function (e) {
        if ($("#hdnview").val() != 1) {
            var $this = $(this);
            showComboBox($this);
            e.preventDefault();
            e.stopPropagation();
        }
    });



    $("#ddlYear").on("change", function () {
        getRevisionNo($("#ddlYear").val());
    });



});


function getRevisionNo(id) {
    var req = {};
    req.Year = id;
    $.ajax({
        url: '/FormB10/GetMaxRev',
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





function Save() {

    var failed = false;


    if (failed)
        return;

    //  if (ValidatePage('#myModal')) {
    InitAjaxLoading();

    var FormB10 = new Object();
    FormB10.PkRefNo = $("#FormB10_PkRefNo").val()
    FormB10.RevisionYear = $("#ddlYear").val()
    FormB10.RevisionNo = $("#RevisionNo").val()
    FormB10.RevisionDate = $("#FormB10_RevisionDate").val()
    FormB10.UserId = $("#UserId").val();
    FormB10.UserName = $("#UserName").val();


    var B10History = []

    $('#tblB10History > tbody  > tr').each(function (index, tr) {

        var B10 = new Object();
        B10.B10dpPkRefNo = $("#FormB10_PkRefNo").val();
        B10.Feature = $(this).find("td:nth-child(1)").html().trim();
        B10.Code = $(this).find("td:nth-child(2)").html().trim();
        B10.Name = $(this).find("td:nth-child(3)").html().trim();
        B10.AdpValue = $(this).find("td:nth-child(4)").html().trim();
        B10.AdpUnit = $(this).find("td:nth-child(5)").html().trim();
        B10.AdpUnitDescription = $(this).find("td:nth-child(6)").html().trim();

        B10History.push(B10);
    });


    FormB10.FormB10History = B10History;

    $.ajax({
        url: '/FormB10/SaveFormB10',
        data: FormB10,
        type: 'POST',
        success: function (data) {
            HideAjaxLoading();
            if (data == -1) {
                app.ShowErrorMessage(data.errorMessage);
            }
            else {
               
                app.ShowSuccessMessage('Saved Successfully', false);
                location.href = "/FormB10";
            }
        }
    });


 
}


function GoBack() {
    if ($("#hdnView").val() == "0") {
        if (app.Confirm("Unsaved changes will be lost. Are you sure you want to cancel?", function (e) {
            if (e) {
                location.href = "/FormB10";

            }
        }));
    }
    else
        location.href = "/FormB10";
}


function typein($this) {

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

function GoBack() {
    if ($("#hdnView").val() == "0") {
        if (app.Confirm("Unsaved changes will be lost. Are you sure you want to cancel?", function (e) {
            if (e) {
                location.href = "/FormB10";

            }
        }));
    }
    else
        location.href = "/FormB10";
}