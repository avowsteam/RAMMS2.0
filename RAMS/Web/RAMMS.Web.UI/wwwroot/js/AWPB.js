
var AWPB = new function () {
    this.HeaderData = {};

    //this.NavToList = function () {
    //    window.location = _APPLocation + "FormB14";
    //}

    //this.Cancel = function () {
    //    jsMaster.ConfirmCancel(() => { AWPB.NavToList(); });
    //}

    this.HeaderGrid = new function () {
       /* this.ActionRender = function (data, type, row, meta) {
            var actionSection = "<div class='btn-group dropright' rowidx='" + meta.row + "'><button type='button' class='btn btn-sm btn-themebtn dropdown-toggle' data-toggle='dropdown'> Click Me </button>";
            actionSection += "<div class='dropdown-menu'>";//dorpdown menu start

            if (data.MaxRecord) { //if (tblFB14HGrid.Base.IsModify) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='AWPB.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='edit-icon'></span> Edit </button>";
            }
            if (tblFB14HGrid.Base.IsView) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='AWPB.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='view-icon'></span> View </button>";
            }
            //if (tblFB14HGrid.Base.IsDelete) {
            //    actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='AWPB.HeaderGrid.ActionClick(this);'>";
            //    actionSection += "<span class='del-icon'></span> Delete </button>";
            //}
            actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='AWPB.HeaderGrid.ActionClick(this);'>";
            actionSection += "<span class='print-icon'></span> Print </button>";

            actionSection += "</div>"; //dorpdown menu close
            actionSection += "</div>"; // action close

            return actionSection;
        }
       */
    }

  

}

$(document).ready(function () {    
    $("#smartSearch").focus();//Header Grid focus    

    if (!AWPB.IsEdit) {
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

   

    $("#formB14Year").on("change", function () {
        getRevisionNo($("#formB14Year").val());
    });

});
