
var frmB8 = new function () {
    this.HeaderData = {};
    this.IsEdit = true;

    this.Save = function () {
        var failed = false;

        if (failed)
            return;

        //  if (ValidatePage('#myModal')) {
        InitAjaxLoading();

        var FormB8 = new Object();
        FormB8.B8hPkRefNo = $("#FormB8Header.B8hPkRefNo").val()
        FormB8.B8hRevisionYear = $("#formB8Year").val()
        FormB8.B8hRevisionNo = $("#RevisionNo").val()
        FormB8.B8hRevisionDate = $("#date").val()
        FormB8.B8hCrBy = $("#UserId").val();
        FormB8.B8hCrByName = $("#UserName").val();
        FormB8.B8hCrDt = $("#date").val()

        var B8History = []

        $('#tblB8History > tbody  > tr').each(function (index, tr) {

            var B8 = new Object();
            B8.B8hiB8hPkRefNo = $("#FormB8Header.B8hPkRefNo").val();
            B8.B8hiItemNo = $(this).find("td:nth-child(2)").text().trim();
            B8.B8hiDescription = $(this).find("td:nth-child(3)").text().trim();
            B8.B8hiUnit = $(this).find("td:nth-child(4)").text().trim();
            B8.B8hiDivision = $(this).find("td:nth-child(5)").text().trim();
            B8History.push(B8);
        });

        FormB8.RmB8History = B8History;

        $.ajax({
            url: '/FormB8/SaveFormB8',
            data: FormB8,
            type: 'POST',
            success: function (data) {
                HideAjaxLoading();
                if (data == -1) {
                    app.ShowErrorMessage(data.errorMessage);
                }
                else {
                    app.ShowSuccessMessage('Saved Successfully', false);
                    location.href = "/FormB8";
                }
            }
        });
    }

    this.NavToList = function () {
        window.location = _APPLocation + "FormB8";
    }

    this.Cancel = function () {
        jsMaster.ConfirmCancel(() => { frmB8.NavToList(); });
    }


    this.HeaderGrid = new function () {
        this.ActionRender = function (data, type, row, meta) {
            var actionSection = "<div class='btn-group dropright' rowidx='" + meta.row + "'><button type='button' class='btn btn-sm btn-themebtn dropdown-toggle' data-toggle='dropdown'> Click Me </button>";
            actionSection += "<div class='dropdown-menu'>";//dorpdown menu start

            if (data.MaxRecord) { //if (tblFB8HGrid.Base.IsModify) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB8.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='edit-icon'></span> Edit </button>";
            }
            if (tblFB8HGrid.Base.IsView) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB8.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='view-icon'></span> View </button>";
            }
            //if (tblFB8HGrid.Base.IsDelete) {
            //    actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB8.HeaderGrid.ActionClick(this);'>";
            //    actionSection += "<span class='del-icon'></span> Delete </button>";
            //}
            actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='frmB8.HeaderGrid.ActionClick(this);'>";
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
                var data = tblFB8HGrid.dataTable.row(rowidx).data();
                switch (type.toLowerCase()) {
                    case "edit":
                        window.location = _APPLocation + "FormB8/Edit/" + data.RefNo;
                        break;
                    case "view":
                        window.location = _APPLocation + "FormB8/View/" + data.RefNo;
                        break;
                    //case "delete":
                    //    app.Confirm("Are you sure you want to delete this record? <br/>(Ref: " + data.RefID + ")", (status) => {
                    //        if (status) {
                    //            DeleteRequest("Delete/" + data.RefNo, "FormB8", {}, function (sdata) {
                    //                if (sdata.id == "-1") {
                    //                    app.ShowErrorMessage("Form G1G2 cannot be deleted, first delete Form F3");
                    //                    return false;
                    //                }
                    //                tblFB8HGrid.Refresh();
                    //                app.ShowSuccessMessage("Deleted Sucessfully! <br/>(Ref: " + data.RefID + ")");
                    //            });
                    //        }
                    //    }, "Yes", "No");
                    //    break;
                    case "print":
                        window.location = _APPLocation + "FormB8/download?id=" + data.RefNo;
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

}

$(document).ready(function () {
    $("#smartSearch").focus();//Header Grid focus    

    if (!frmB8.IsEdit) {
        $("#formB8Year").chosen("destroy");
        $("#divFindDetails *").attr("disabled", "disabled").off("click");
    }

    element = document.querySelector("#btnAdvSearch");
    if (element) {
        element.addEventListener("keyup", () => {
            if (event.keyCode === 13) {
                $('[SearchSectionBtn]').trigger('onclick');
            }
        });
    }
    $("#smartSearch").keyup(function () {
        if (event.keyCode === 13) {
            $('[SearchSectionBtn]').trigger('onclick');
        }
    })


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
        if (frmB8.IsEdit) {
            var $this = $(this);
            frmB8.typein($this);
        }
        e.preventDefault();
    });

    $('.dropdown').on('click', function (e) {
        if (frmB8.IsEdit) {
            var $this = $(this);
            showComboBox($this);
        }
        e.preventDefault();
        e.stopPropagation();
    });



    $("#formB8Year").on("change", function () {
        getRevisionNo($("#formB8Year").val());
    });

});


function Type(obj, e) {
    if (frmB8.IsEdit) {
        var $this = $(obj);
        frmB8.typein($this);
    }
    e.preventDefault();
}


function getRevisionNo(id) {
    var req = {};
    req.Year = id;
    $.ajax({
        url: '/FormB8/GetMaxRev',
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


function AddRow() {
    var temp = 'His_' + Math.floor(Date.now() / 1000);
    var row = '<tr ' + temp + '><td><div class="btn-group dropright" id="actiondropdown"><button id="actionclick" type="button" class="btn btn-sm btn-themebtn dropdown-toggle" data-toggle="dropdown"> Click Me </button> <div class="dropdown-menu"> <button type="button" class="dropdown-item editdel-btns" onclick="javascript:Delete(\'' + temp + '\');"><span class="del-icon"></span>Delete</button></div></div></td>'
        + '<td><input type="text"  onblur="javascript:save(this)" onkeyup="javascript:enter(this,event)"/></td>'
        + '<td><input type="text" onblur="javascript:save(this)" onkeyup="javascript:enter(this,event)"/></td>'
        + '<td class="typein" datatype="int" onclick="Type(this,event)"><input type="text" onblur="javascript:save(this)" onkeyup="javascript:enter(this,event)"/></td>'
        + '<td class="typein"  onclick="Type(this,event)"><input type="text"  onblur="javascript:save(this)" onkeyup="javascript:enter(this,event)"/></td></tr> ';
    $("#tblB8History>tbody").append(row)
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

