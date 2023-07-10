﻿var _hd = {


    ddlInspectedby: $("#ddlInspectedby"),
    txtServiceProvidername: $("#txtServiceProvidername"),
    txtServiceProviderDesignation: $("#txtServiceProviderDesignation"),
    txtServiceProviderDate: $("#txtServiceProviderDate")
};
var formFC = new function () {
    this.HeaderData = {};
    this.Patter = "";
    this.IsEdit = true;
    this.Assets = {};
    this.FindDetails = function () {
        if (ValidatePage("#frmFCHeaderInformation")) {
            var tis = this;
            var rd = $("#selRoadCode option:selected");
            var post = {};
            post.RoadId = rd.attr("pid");
            post.RmuName = $("#selRMU option:selected").attr("cvalue");
            GetResponseValue("FindDetails", "FormFC", FormValueCollection("#frmFCHeaderInformation", post), function (data) {
                if (data && !data._error) {
                    $("[finddetailhide]").hide();
                    $("#selRoadCode,#formFCInsYear").prop("disabled", true).trigger("chosen:updated");
                    tis.HeaderData = data;
                    tis.PageInit();
                    getLoginUserid();
                }
                else {
                    app.ShowErrorMessage(data._error);
                }
            }, "Finding");
        }
    }
    this.PageInit = function () {
        //debugger;
        if (this.HeaderData && this.HeaderData.PkRefNo > 0) {
            $("[finddetailsdep]").show();
            $("#btnFindDetails").hide();
            this.BindData();
            this.InitConditionTable();
        }
        else {
            $("[finddetailsdep]").hide();
            $("#btnFindDetails").show();
        }
    }
    this.BindData = function () {
        if (this.HeaderData && this.HeaderData.PkRefNo && this.HeaderData.PkRefNo > 0) {
            if (this.IsEdit) { this.IsEdit = this.HeaderData.SubmitSts ? false : true; }
            var tis = this;
            var assignFormat = jsMaster.AssignFormat;
            $("#frmFCHeader").find("input,select,textarea").filter("[name]").each(function () {
                var obj = $(this);
                var name = obj.attr("name");
                if (tis.HeaderData[name] != null) {
                    if (this.type == "select-one") { obj.val("" + tis.HeaderData[name]).trigger("change").trigger("chosen:updated"); }
                    else if (this.type == "date") { obj.val((new Date(tis.HeaderData[name])).ToString(assignFormat)); }
                    else {
                        obj.val(tis.HeaderData[name]);
                    }
                }
                else { obj.val(""); }
                if (!tis.IsEdit) {
                    obj.prop("disabled", true);
                    if (this.type == "select-one") { obj.trigger("chosen:updated"); };
                }
            });
            $("#dtInspection").attr("min", this.HeaderData.YearOfInsp + "-01-01").attr("max", this.HeaderData.YearOfInsp + "-12-31");
            if (!this.IsEdit) {
                $(".custom-footer [finddetailsdep]").hide();
            }
        }
    }
    this.InitCTableStructure = function (tbl) {
        //debugger;
        var body = tbl.find("tbody");
        body.empty();
        var data = this.HeaderData.AssetTypes;
        if (data) {
            this.Assets = eval("[" + data + "]")[0];
            this.InitLeftRows(body, this.Assets.ELM);
            this.InitCenterRows(body, this.Assets.CW, this.Assets.CLM)
            this.InitRightRows(body, this.Assets.ELM);
            var txtAvgWidth = body.find("input:text[txtavgwidth]");
            if (this.IsEdit) {
                txtAvgWidth.on("blur", function () {
                    var obj = $(this);
                    var tr = obj.closest("tr");
                    var ctype = tr.attr("ctype");
                    if (ctype == "L") {
                        tr[0].Asset.LAvgWidth = obj.val();
                    }
                    else if (ctype == "R") {
                        tr[0].Asset.RAvgWidth = obj.val();
                    }
                    else {
                        tr[0].Asset.AvgWidth = obj.val();
                    }
                    if (typeof (JSON.stringify) == "function") {
                        formFC.HeaderData.AssetTypes = JSON.stringify(formFC.Assets);
                    }
                });
            }
            else {
                txtAvgWidth.prop("disabled", true);
            }
            Validation.OnKeyPressInit();
        }
    }
    this.GetRSAsset = function (pos) {
        var result = null;
        $.each(this.Assets.RS, function (idx, obj) {
            if (obj.Desc == pos) { result = obj; }
        });
        return result;
    }
    this.InitLeftRows = function (body, elm) {
        var rs = this.GetRSAsset("Left");
        if (elm) {
            for (var i = 0; i < elm.length; i++) {
                var tr = $("<tr ctype='L'/>");
                if (i == 0) {
                    tr.append("<td class='fixed rowspantd' rowspan='" + (elm.length + 1) + "' style='line-height:13px;'>L<br />E<br/>F<br/>T</td>");
                    tr.append("<td class='fixed rowspantd' rowspan='" + elm.length + "' style='width:70px; line-height:13px;'>Edge Line</td>");
                }
                tr.attr("asset", "ELM_Left_" + elm[i].Desc);
                tr[0].Asset = elm[i];
                tr.append("<td class='fixed' style='width: 100px;'>" + elm[i].Desc + "</td>");
                tr.append("<td class='fixed'>" + elm[i].Value + "</td>");
                tr.append("<td class='fixed'><input value='" + (elm[i].LAvgWidth ? elm[i].LAvgWidth : '') + "' txtavgwidth type='text' style='width:100px;' onkeypressvalidate='cdecimal,5,3,Left " + elm[i].Desc + "'/></td>");
                tr.append("<td class='fixed'>m</td>");
                tr.append("<td condaftertd class='rfixed'>Km</td>");
                tr.append("<td 1con class='rfixed'></td>");
                tr.append("<td 2con class='rfixed'></td>");
                tr.append("<td 3con class='rfixed'></td>");
                tr.append("<td tcon class='rfixed'></td>");
                body.append(tr);
            }
            var tr = $("<tr ctype='L'/>"); //road struds
            tr.attr("asset", "RS_Left");
            tr[0].Asset = rs;
            tr.append("<td class='fixed' colspan='2'>Road Studs</td>");
            tr.append("<td class='fixed'>R</td>");
            tr.append("<td class='fixed'><input value='" + (rs.LAvgWidth ? rs.LAvgWidth : '') + "' txtavgwidth type='text' style='width:100px;' onkeypressvalidate='cdecimal,5,3,Left Road Studs'/></td>");
            tr.append("<td class='fixed'>m</td>");
            tr.append("<td condaftertd class='rfixed'>Km</td>");
            tr.append("<td 1con class='rfixed'></td>");
            tr.append("<td 2con class='rfixed'></td>");
            tr.append("<td 3con class='rfixed'></td>");
            tr.append("<td tcon class='rfixed'></td>");
            body.append(tr);
        }
        else { //only road struds // Not Tested
            var tr = $("<tr ctype='L'/>");
            tr.attr("asset", "RS_Left");
            tr[0].Asset = rs;
            tr.append("<td class='fixed rowspantd' style='line-height:13px;'>L<br />e<br />f<br />t</td>");
            tr.append("<td class='fixed' colspan='2'>Road Studs</td>");
            tr.append("<td class='fixed'>R</td>");
            tr.append("<td class='fixed'><input value='" + (rs.LAvgWidth ? rs.LAvgWidth : '') + "' txtavgwidth type='text' style='width:100px;' onkeypressvalidate='cdecimal,5,3,Left Road Studs'/></td>");
            tr.append("<td class='fixed'>m</td>");
            tr.append("<td condaftertd class='rfixed'>Km</td>");
            tr.append("<td 1con class='rfixed'></td>");
            tr.append("<td 2con class='rfixed'></td>");
            tr.append("<td 3con class='rfixed'></td>");
            tr.append("<td tcon class='rfixed'></td>");
            body.append(tr);
        }
    }
    this.InitCenterRows = function (body, cw, clm) {
        var rowCount = cw ? cw.length : 0;
        rowCount += clm ? clm.length : 0;
        var rs = this.GetRSAsset("Centre");
        if (cw) {
            for (var i = 0; i < cw.length; i++) {
                var tr = $("<tr ctype='C'/>");
                if (i == 0) {
                    tr.append("<td class='fixed rowspantd' rowspan='" + (rowCount + 1) + "' style='line-height:13px;'>C<br/>A<br/>R<br/>R<br/>I<br/>A<br/>G<br/>E<br/>W<br/>A<br/>Y</td>");
                    tr.append("<td class='fixed rowspantd' rowspan='" + cw.length + "' style='width:70px; line-height:13px;'>PAVEMENT</td>");
                }
                tr[0].Asset = cw[i];
                tr.attr("asset", "CW_" + cw[i].Desc);
                tr.append("<td class='fixed' style='width: 100px;'>" + cw[i].Desc + "</td>");
                tr.append("<td class='fixed'>" + cw[i].Value + "</td>");
                tr.append("<td class='fixed'><input value='" + (cw[i].AvgWidth ? cw[i].AvgWidth : '') + "' txtavgwidth type='text' style='width:100px;' onkeypressvalidate='cdecimal,5,3,Carriageway " + cw[i].Desc + "'/></td>");
                tr.append("<td class='fixed'>m</td>");
                tr.append("<td condaftertd class='rfixed'>Km</td>");
                tr.append("<td 1con class='rfixed'></td>");
                tr.append("<td 2con class='rfixed'></td>");
                tr.append("<td 3con class='rfixed'></td>");
                tr.append("<td tcon class='rfixed'></td>");
                body.append(tr);
            }
        }
        var tr = $("<tr ctype='C'/>"); //road struds
        tr.attr("asset", "RS_Centre");
        tr[0].Asset = rs;
        tr.append("<td class='fixed' colspan='2'>Centre Road Studs</td>");
        tr.append("<td class='fixed'>R</td>");
        tr.append("<td class='fixed'><input value='" + (rs.AvgWidth ? rs.AvgWidth : '') + "' txtavgwidth type='text' style='width:100px;' onkeypressvalidate='cdecimal,5,3,Centre Road Studs'/></td>");
        tr.append("<td class='fixed'>m</td>");
        tr.append("<td condaftertd class='rfixed'>Km</td>");
        tr.append("<td 1con class='rfixed'></td>");
        tr.append("<td 2con class='rfixed'></td>");
        tr.append("<td 3con class='rfixed'></td>");
        tr.append("<td tcon class='rfixed'></td>");
        body.append(tr);

        if (clm) {
            for (var i = 0; i < clm.length; i++) {
                var tr = $("<tr ctype='C'/>");
                if (i == 0) {
                    tr.append("<td class='fixed rowspantd' rowspan='" + clm.length + "' style='width:70px; line-height:13px;'>Centre Line</td>");
                }
                tr[0].Asset = clm[i];
                tr.attr("asset", "CLM_" + clm[i].Desc);
                tr.append("<td class='fixed' style='width: 100px;'>" + clm[i].Desc + "</td>");
                tr.append("<td class='fixed'>" + clm[i].Value + "</td>");
                tr.append("<td class='fixed'><input value='" + (clm[i].AvgWidth ? clm[i].AvgWidth : '') + "' txtavgwidth type='text' style='width:100px;' onkeypressvalidate='cdecimal,5,3,Carriageway " + clm[i].Desc + "'/></td>");
                tr.append("<td class='fixed'>m</td>");
                tr.append("<td condaftertd class='rfixed'>Km</td>");
                tr.append("<td 1con class='rfixed'></td>");
                tr.append("<td 2con class='rfixed'></td>");
                tr.append("<td 3con class='rfixed'></td>");
                tr.append("<td tcon class='rfixed'></td>");
                body.append(tr);
            }
        }

    }
    this.InitRightRows = function (body, elm) {
        var rs = this.GetRSAsset("Right");
        if (elm) {
            var tr = $("<tr ctype='R'/>"); //road struds
            tr.attr("asset", "RS_Right");
            tr[0].Asset = rs;
            tr.append("<td class='fixed rowspantd' rowspan='" + (elm.length + 1) + "' style='line-height:13px;'>R<br/>I<br/>G<br/>H<br/>T</td>");
            tr.append("<td class='fixed' colspan='2' style='width: 70px; line - height: 13px;'>Road Studs</td>");
            tr.append("<td class='fixed'>R</td>");
            tr.append("<td class='fixed'><input value='" + (rs.RAvgWidth ? rs.RAvgWidth : '') + "' txtavgwidth type='text' style='width:100px;' onkeypressvalidate='cdecimal,5,3,Right Road Studs'/></td>");
            tr.append("<td class='fixed'>m</td>");
            tr.append("<td condaftertd class='rfixed'>Km</td>");
            tr.append("<td 1con class='rfixed'></td>");
            tr.append("<td 2con class='rfixed'></td>");
            tr.append("<td 3con class='rfixed'></td>");
            tr.append("<td tcon class='rfixed'></td>");
            body.append(tr);
            for (var i = 0; i < elm.length; i++) {
                var tr = $("<tr ctype='R'/>");
                if (i == 0) {
                    tr.append("<td class='fixed rowspantd' rowspan='" + elm.length + "'>Edge line</td>");
                }
                tr[0].Asset = elm[i];
                tr.attr("asset", "ELM_Right_" + elm[i].Desc);
                tr.append("<td class='fixed' style='width: 100px;'>" + elm[i].Desc + "</td>");
                tr.append("<td class='fixed'>" + elm[i].Value + "</td>");
                tr.append("<td class='fixed'><input value='" + (elm[i].RAvgWidth ? elm[i].RAvgWidth : '') + "' txtavgwidth type='text' style='width:100px;' onkeypressvalidate='cdecimal,5,3,Right " + elm[i].Desc + "'/></td>");
                tr.append("<td class='fixed'>m</td>");
                tr.append("<td condaftertd class='rfixed'>Km</td>");
                tr.append("<td 1con class='rfixed'></td>");
                tr.append("<td 2con class='rfixed'></td>");
                tr.append("<td 3con class='rfixed'></td>");
                tr.append("<td tcon class='rfixed'></td>");
                body.append(tr);
            }
        }
        else { //only road struds // Not Tested
            var tr = $("<tr ctype='R'/>");
            tr.attr("asset", "RS_Right");
            tr[0].Asset = rs;
            tr.append("<td class='fixed rowspantd' style='line-height:13px;'>R<br/>i<br/>g<br/>h<br/>t</td>");
            tr.append("<td class='fixed' colspan='2'>Road Studs</td>");
            tr.append("<td class='fixed'>R</td>");
            tr.append("<td class='fixed'><input value='" + (rs.RAvgWidth ? rs.RAvgWidth : '') + "' txtavgwidth type='text' style='width:100px;' onkeypressvalidate='cdecimal,5,3,Right Road Studs'/></td>");
            tr.append("<td class='fixed'>m</td>");
            tr.append("<td condaftertd class='rfixed'>Km</td>");
            tr.append("<td 1con class='rfixed'></td>");
            tr.append("<td 2con class='rfixed'></td>");
            tr.append("<td 3con class='rfixed'></td>");
            tr.append("<td tcon class='rfixed'></td>");
            body.append(tr);
        }
    }
    this.InitConditionTable = function () {
        //debugger;
        var tbl = $("#tblFormFC");
        if (this.HeaderData && this.HeaderData.PkRefNo > 0) {
            this.InitCTableStructure(tbl);
            var dtl = this.HeaderData.InsDtl;
            var minKM = this.HeaderData.FrmCh;
            var maxKM = this.HeaderData.ToCh;
            //if (minKM > maxKM) { var maxTemp = minKM; minKM = maxKM; maxKM = maxTemp; }
            var asset = "";
            if (dtl && dtl.length > 0) {
                var th1 = tbl.find("thead tr:eq(0)");
                var th2 = tbl.find("thead tr:eq(1)");
                var thCon = th2.find("[condafterhd]");
                var iColCount = 0; var minchar, maxchar;
                var trs = tbl.find("tbody tr");
                if (minKM > maxKM) {
                    //debugger;
                    while (minKM > maxKM) {
                        iColCount += 5;
                        minchar = minKM.toFixed(3).replace(".", "+");
                        for (var i = 0; i < 5; i++) {
                            if (i > 0) { minKM -= 0.1; }
                            trs.each(function () {
                                $(this).find("[condaftertd]").before("<td title='" + minKM.toFixed(3) + "' km='" + minKM.toFixed(3) + "' fccondition class='fcconditiontd fcblock'><div condtion con=''> </div></td>");
                            });
                        }
                        maxchar = (minKM + 0.099).toFixed(3).replace(".", "+");
                        thCon.before("<th colspan='5' class='kmrange'>" + minchar + " to " + maxchar + "</th>");
                        minKM -= 0.1;
                    }
                }
                else {
                    while (minKM < maxKM) {
                        iColCount += 5;
                        minchar = minKM.toFixed(3).replace(".", "+");
                        for (var i = 0; i < 5; i++) {
                            if (i > 0) { minKM += 0.1; }
                            trs.each(function () {
                                $(this).find("[condaftertd]").before("<td title='" + minKM.toFixed(3) + "' km='" + minKM.toFixed(3) + "' fccondition class='fcconditiontd fcblock'><div condtion con=''> </div></td>");
                            });
                        }
                        maxchar = (minKM + 0.099).toFixed(3).replace(".", "+");
                        if ((minKM + 0.1).toFixed(1) == maxKM) maxchar = (minKM + 0.1).toFixed(3).replace(".", "+");
                        thCon.before("<th colspan='5' class='kmrange'>" + minchar + " to " + maxchar + "</th>");
                        minKM += 0.1;
                    }
                }

                th1.find("[condhd1]").attr("colspan", iColCount > 35 ? 35 : iColCount);
                if (iColCount > 35) {
                    var condhd1 = th1.find("[condhd1after]");
                    var icount = 35; var icol = 0;
                    while (iColCount > icount) {
                        icol = 5;
                        icount += icol;
                        if (iColCount < icount) { icol = icol - (icount - iColCount); }
                        condhd1.before("<th colspan='5'></th>");
                    }
                }
                $.each(dtl, function (idx, obj) {
                    ////debugger;
                    asset = "";
                    var bound = obj.AiBound;
                    bound = bound == "L" ? "Left" : (bound == "R" ? "Right" : bound);
                    switch (obj.AiAssetGrpCode) {
                        case "RS":
                            asset = "RS_" + ((bound == "Left" || bound == "Right") ? bound : "Centre");
                            break;
                        case "ELM":
                            asset = "ELM_" + bound + "_" + obj.AiGrpType;
                            break;
                        case "CW":
                            asset = "CW_" + obj.AiGrpType;
                            break;
                        case "CLM":
                            asset = "CLM_" + obj.AiGrpType;
                            break;
                    }
                    if (asset != "") {
                        var _td = trs.filter("[asset='" + asset + "']").find("[km='" + obj.FromCHKm.toFixed(3) + "']");
                        if (_td.length > 0) {
                            _td.removeClass("fcblock").addClass("fcconactive");
                            _td[0].Asset = obj;
                            if (obj.Condition && obj.Condition > 0) { _td.find("[condtion]").attr("con", obj.Condition).text(obj.Condition); }
                        }
                    }
                });
            }
            tbl.show();
        }
        else {
            tbl.hide();
        }
        this.RefreshCondition();
        setTimeout(function () { formFC.RefreshConditionTable(); }, 200);

    }
    this.RefreshConditionTable = function () {
        var twidth = $("#tblFormFC").width();
        $("#tblFormFC .fixed").each(function () {
            var obj = $(this);
            obj.css("left", (obj.position().left - 1) + "px");
        });
        $("#tblFormFC .rfixed").each(function () {
            var obj = $(this);
            var rgt = twidth - (obj.position().left + obj.width()) - 3;
            rgt = rgt > 6 ? rgt : 0;
            obj.css("right", rgt + "px");
        });
        if (this.IsEdit) {
            $("#tblFormFC td[fccondition]:not(.fcblock)").on("click", function () {
                //debugger;
                var obj = $(this);
                var csel = obj.find("div.fcconditionsel");
                if (csel.length == 0) { csel = $("#fcconditionseltemplate .fcconditionsel").clone(); obj.append(csel); }
            });
        }

        //if (this.IsEdit) {
        //    $("#tblFormFC td[fccondition]:not(.fcblock)").on("click", function () {
        //        //debugger;
        //        var obj = $(this);
        //        var csel = obj.find("div.fcconditionsel");
        //        if (csel.length == 0) { csel = $("#fcconditionseltemplate .fcconditionsel").clone(); csel.hide(); obj.append(csel); }
        //        if (csel.is(":visible")) { csel.slideUp(); }
        //        else { csel.slideDown(); }
        //        //DetailListGrid
        //        var $panel = $(this).closest('#tblFormFC');
        //        $('#DetailListGrid').animate({
        //            scrollTop: $panel.offset().top + 100
        //        }, 500);
        //    });
        //    $("#DetailListGrid").on("mouseleave", function () { $(this).find("div.fcconditionsel:visible").hide(); });
        //}
    }
    this.ConSelect = function (tis, evt) {
        debugger
        if (evt.stopImmediatePropagation) { event.stopImmediatePropagation(); }
        if (evt.stopPropagation) { event.stopPropagation(); }
        var obj = $(tis);
        var td = obj.closest("td");
        td[0].Asset.Condition = obj.attr("val");
        td.find("[condtion]").attr("con", obj.attr("val")).text(obj.attr("val"));
        td.find("div.fcconditionsel").hide();        
        this.RefreshCondition(td.closest("tr"));
    }

    this.InputKeyPress = function (tis, e) {
        if (e.shiftKey || e.ctrlKey || e.altKey) {
            e.preventDefault();
        } else {
            var key = e.keyCode;
            if (!((key == 8) || (key == 46) || (key >= 35 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105)) || ((parseInt(e.key) < 1) || (parseInt(e.key) > 3))) {          
                e.preventDefault();
            }
        }
        
        if (e.stopImmediatePropagation) { event.stopImmediatePropagation(); }
        if (e.stopPropagation) { event.stopPropagation(); }
        var obj = $(tis);
        var td = obj.closest("td");
        td[0].Asset.Condition = e.key;
        td.find("[condtion]").attr("con", e.key).text(e.key);
        td.find("[condtion]").hide();
        this.RefreshCondition(td.closest("tr"));
    }

    this.RefreshCondition = function (trs) {
        var len = 0; tlen = 0;
        if (!trs) { trs = $("#tblFormFC tbody tr"); }

        trs.each(function () {
            var obj = $(this);
            len = obj.find("[con='1']").length
            obj.find("td[1con]").text(len > 0 ? (len / 10) : "");
            tlen = len;
            len = obj.find("[con='2']").length
            obj.find("td[2con]").text(len > 0 ? (len / 10) : "");
            tlen += len;
            len = obj.find("[con='3']").length
            obj.find("td[3con]").text(len > 0 ? (len / 10) : "");
            tlen += len;
            obj.find("td[tcon]").text(tlen > 0 ? (tlen / 10) : "");
        });
    }
    this.Search = new function () {
        this.SecCodeChange = function (tis) {
            var sel = $(tis);
            var obj = sel.find("option:selected");
            $("#txtSectionName").val(sel.val());
            var rmu = $("#selRMU");
            var val = obj.attr("cvalue");
            if (val) { rmu.val(val == "Batu Niah" ? "BTN" : "MRI").trigger("chosen:updated"); }
            var div = $("#selDivision");
            if (div.val() == "") { div.val("MIRI").trigger("chosen:updated"); }
            formFC.FilterRoadCode();
        }
        this.RoadCodeChange = function (tis) {
            var sel = $(tis);
            var obj = sel.find("option:selected")
            $("#txtRoadName").val(obj.attr("Item1"));
            if (sel.val() != "") {
                var sec = $("#selSectionCode");
                if (sec.val() == "") { sec.val(sec.find("[code='" + obj.attr("item2") + "']").val()).trigger("chosen:updated"); $("#txtSectionName").val(sec.val()); }
                var rmu = $("#selRMU");
                if (rmu.val() == "") { rmu.val(obj.attr("cvalue")).trigger("chosen:updated"); }
                var div = $("#selDivision");
                if (div.val() == "") { div.val("MIRI").trigger("chosen:updated"); }
            }
            formFC.BindRefNumber();
        }
        this.RmuChange = function (tis) {
            if (tis.value != "") {
                var sec = $("#selSectionCode"); sec.find("option").hide().filter("[cvalue='" + $(tis).find("option:selected").attr("cvalue") + "']").show();
                if (sec.find("option:selected:visible").length == 0) { sec.val("").trigger("change"); }
                sec.trigger("chosen:updated");
            }
            else {
                var ctrl = $("#selSectionCode"); ctrl.find("option:hidden").show(); ctrl.val("").trigger("change").trigger("chosen:updated");
            }
            formFC.FilterRoadCode();
            var div = $("#selDivision");
            if (div.val() == "") { div.val("MIRI").trigger("chosen:updated"); }
        }
    }
    this.FilterRoadCode = function () {
        var asset = $("#selRoadCode");
        if (asset.length > 0) {
            var opt = asset.find("option").show();

            var rmu = $("#selRMU");
            if (rmu.val() != "") { opt.filter(":not([cvalue='" + rmu.val() + "'])").hide(); }

            var sec = $("#selSectionCode");
            if (sec.val() != "") { opt.filter(":not([item2='" + sec.find("option:selected").attr("code") + "'])").hide(); }
            asset.val("").trigger("change").trigger("chosen:updated");
        }
    }
    this.InsYearChange = function (tis) { this.BindRefNumber(); }
    this.BindRefNumber = function () {
        var tis = this;
        var yr = $("#formFCInsYear").val();
        var assid = $("#selRoadCode");
        if (yr && yr != "" && assid.val() != "") {
            $("#formFCRefNO").val(tis.Pattern.replace("{RoadCode}", assid.val()).replace("{Year}", yr));
        }
        else {
            $("#formFCRefNO").val("");
        }
    }
    this.CrewLeaderChange = function (tis) {
        var sel = $(tis);
        var opt = sel.find(":selected");
        var item1 = opt.attr("item1") ? opt.attr("item1") : "";
        if (item1 == "others") {
            $("#txtCrewLeaderName").val("").addClass("validate").prop("disabled", false);
        }
        else {
            $("#txtCrewLeaderName").val(item1).removeClass("validate").prop("disabled", true);
        }
    }
    this.UserIDChange = function (tis) {
        var sel = $(tis);
        var opt = sel.find(":selected");
        getUserDetail(opt, function (data) {
            par.find("#txtUserNameInspBy").val(data.userName);
            par.find("#txtUserDesignationInspBy").val(data.position);
            par.find("#txtUserDesignationInspBy").attr("readonly", "true");
        });
        var item1 = opt.attr("item1") ? opt.attr("item1") : "";
        if (item1 == "others") {
            $("#txtUserNameInspBy").val("").addClass("validate").prop("disabled", false);
            $("#txtUserDesignationInspBy").val("").addClass("validate").prop("disabled", false);
        }
        else {
            var item2 = opt.attr("Item2") ? opt.attr("item2") : "";
            $("#txtUserNameInspBy").val(item1).removeClass("validate").prop("disabled", true);
            $("#txtUserDesignationInspBy").val(item2).removeClass("validate").prop("disabled", true);
        }
    }
    this.Save = function (isSubmit) {
        //debugger;
        var tis = this;
        if (isSubmit) {
            $("#hdnFormFCCond").val($("#tblFormFC tbody td:not(.fcblock)[km] [con='']").length > 0 ? "" : "sucess");
            $("#frmFCHeader .svalidate").addClass("validate");
        }
        if (ValidatePage("#frmFCHeader", "", "")) {
            var action = isSubmit ? "Submit" : "Save";
            GetResponseValue(action, "FormFC", FormValueCollection("#frmFCHeader", tis.HeaderData), function (data) {
                app.ShowSuccessMessage('Successfully Saved', false);
                setTimeout(tis.NavToList, 2000);
            }, "Saving");
        }
        if (isSubmit) {
            $("#frmFCHeader .svalidate").removeClass("validate");
        }
    }
    this.NavToList = function () {
        window.location = _APPLocation + "FormFC";
    }
    this.Cancel = function () {
        jsMaster.ConfirmCancel(() => { formFC.NavToList(); });
    }
    this.HeaderGrid = new function () {
        this.ActionRender = function (data, type, row, meta) {
            var actionSection = "<div class='btn-group dropright' rowidx='" + meta.row + "'><button type='button' class='btn btn-sm btn-themebtn dropdown-toggle' data-toggle='dropdown'> Click Me </button>";
            actionSection += "<div class='dropdown-menu'>";//dorpdown menu start

            if (data.Status != "Submitted" && tblFCHGrid.Base.IsModify) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='formFC.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='edit-icon'></span> Edit </button>";
            }
            if (tblFCHGrid.Base.IsView) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='formFC.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='view-icon'></span> View </button>";
            }
            if (tblFCHGrid.Base.IsDelete) {
                actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='formFC.HeaderGrid.ActionClick(this);'>";
                actionSection += "<span class='del-icon'></span> Delete </button>";
            }
            actionSection += "<button type='button' class='dropdown-item editdel-btns' onclick='formFC.HeaderGrid.ActionClick(this);'>";
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
                var data = tblFCHGrid.dataTable.row(rowidx).data();
                switch (type.toLowerCase()) {
                    case "edit":
                        window.location = _APPLocation + "FormFC/Edit/" + data.RefNo;
                        break;
                    case "view":
                        window.location = _APPLocation + "FormFC/View/" + data.RefNo;
                        break;
                    case "delete":
                        app.Confirm("Are you sure you want to delete this record? <br/>(Ref: " + data.RefID + ")", (status) => {
                            if (status) {
                                DeleteRequest("Delete/" + data.RefNo, "FormFC", {}, function (sdata) {
                                    tblFCHGrid.Refresh();
                                    app.ShowSuccessMessage("Deleted Sucessfully! <br/>(Ref: " + data.RefID + ")");
                                });
                            }
                        }, "Yes", "No");
                        break;
                    case "print":
                        window.location = _APPLocation + "FormFC/download?id=" + data.RefNo;
                        break;
                }
            }
        }
        this.DateOfEntry = (data, type, row, meta) => {
            var result = "";
            if (data && data != null && data != "") {
                result = (new Date(data)).ToString(jsMaster.GridFormat);
            }
            return result;
        }
    }
}
$(document).ready(function () {
    formFC.PageInit();
    $("#smartSearch").focus();
    if ($("#btnFindDetails:visible").length > 0) {
        setTimeout(function () { $('#selDivision').trigger('chosen:activate'); }, 200);
    }
    else {
        setTimeout(function () { $("#selCrewLeaderName").trigger('chosen:activate'); }, 200);
    }
    $("#selUserIdInspBy").on("change", function () {
        var value = this.value;
        if (value == "") {
            $("#txtUserNameInspBy").val('');
            $("#txtUserDesignationInspBy").val('');
            $("#txtUserNameInspBy").prop("disabled", true);
            $("#txtUserDesignationInspBy").prop("disabled", true);
        }
        else if (value == "99999999") {
            $("#txtUserNameInspBy").val('');
            $("#txtUserDesignationInspBy").val('');
            $("#txtUserNameInspBy").prop("disabled", false);
            $("#txtUserDesignationInspBy").prop("disabled", false);
        }
        else {
            getUserDetail(value, function (data) {

                $("#txtUserNameInspBy").val(data.userName);
                $("#txtUserDesignationInspBy").val(data.position);
                $("#txtUserNameInspBy").prop("disabled", true);
                $("#txtUserDesignationInspBy").prop("disabled", true);
            });
        }
    });
    $("#selUserIdInspBy").trigger('change');
});

function getUserDetail(id, callback) {
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
function getLoginUserid() {
    $.ajax({
        url: '/NOD/GetUserId',
        dataType: 'JSON',
        type: 'GET',
        success: function (data) {
            $("#selUserIdInspBy").val(data).trigger("change").trigger("chosen:updated");
        },
        error: function (data) {
            console.error(data);
        }
    });
}