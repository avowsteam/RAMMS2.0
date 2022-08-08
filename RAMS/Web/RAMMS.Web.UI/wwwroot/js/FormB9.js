


$(document).ready(function () {




    $('.typein').on('click', function (e) {
        var $this = $(this);
        typein($this);
        e.preventDefault();
    });

    $('.dropdown').on('click', function (e) {
        showComboBox($this);
        e.preventDefault();
        e.stopPropagation();
    });



    $("#ddlYear").on("change", function () {
        generateHeaderReference();
    });



});


function getRevisionNo(id, callback) {
    var req = {};
    req.id = id;
    $.ajax({
        url: '/NOD/GetUserById',
        dataType: 'JSON',
        data: req,
        type: 'Post',
        success: function (data) {
            callback(data);
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

    var FormB9 = new Object();
    FormB9.PkRefNo = $("#FormB9_PkRefNo").val()
    FormB9.RevisionYear = $("#ddlYear").val()
    FormB9.RevisionNo = $("#RevisionNo").val()
    FormB9.RevisionDate = $("#FormB9_RevisionDate").val()
    FormB9.UserId = $("#FormB9_UserId").val();
    FormB9.UserName = $("#UserName").val();


    var B9History = []

    $('#tblB9History > tbody  > tr').each(function (index, tr) {

        var B9 = new Object();
        B9.B9dsPkRefNo = $("#FormB9_PkRefNo").val();
        B9.Feature = $(this).find("td:nth-child(1)").html().trim();
        B9.Code = $(this).find("td:nth-child(2)").html().trim();
        B9.Name = $(this).find("td:nth-child(3)").html().trim();
        B9.Cond1 = $(this).find("td:nth-child(4)").html().trim();
        B9.Cond2 = $(this).find("td:nth-child(5)").html().trim();
        B9.Cond3 = $(this).find("td:nth-child(6)").html().trim();
        B9.UnitOfService = $(this).find("td:nth-child(7)").html().trim();
        B9.Remarks = $(this).find("td:nth-child(8)").html().trim();

        B9History.push(B9);
    });


    FormB9.FormB9History = B9History;

    $.ajax({
        url: '/FormB9/SaveFormB9',
        data: FormB9,
        type: 'POST',
        success: function (data) {
            HideAjaxLoading();
            if (data == -1) {
                app.ShowErrorMessage(data.errorMessage);
            }
            else {
                ClearFormTDtl()

                InitializeGrid();
                app.ShowSuccessMessage('Saved Successfully', false);
            }
        }
    });



    // }
}


function GoBack() {
    if ($("#hdnView").val() == "0") {
        if (app.Confirm("Unsaved changes will be lost. Are you sure you want to cancel?", function (e) {
            if (e) {
                location.href = "/FrmT";

            }
        }));
    }
    else
        location.href = "/FrmT";
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

    var oldstrText = $.trim($(obj).text());
    if (oldstrText != "") oldstrText = $(obj).attr("itemid");

    if ($(obj).attr("CellType").toLowerCase() == "propchoices") {
        oldstrText = $.trim($(obj).html());
    }

    $(obj).removeAttr("onclick");

    $(obj).html().promise().done(function () {

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



    EditMode = true;
    return false;
}

function HideCombo(obj) {
    var str;
    var td = $(obj).parent();

    //  str= $(obj).find("option:selected").val();

    if (typeof str == "undefined" || str == "" || typeof SymbolType != "undefined") {
        str = $(td).attr("preval");
    }

    if (typeof str == "undefined" || str == "")
        str = ""

    $(td).html(str);
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

