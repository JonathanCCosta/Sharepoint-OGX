$(document).ready(function () {
    //$($($("span a")[12])[0]).text("Consulta projetos Importados do SGPMR");
    //$("#DeltaPlaceHolderPageTitleInTitleArea").hide();
    if (e_Lista()) {
        $("#DeltaPlaceHolderPageTitleInTitleArea").text("Planos");
        $("#DeltaPlaceHolderPageTitleInTitleArea").css("color", "black");
        //$("#pageTitle").css("margin-bottom", "-5px");

        $("a[accesskey='W']").attr("href", "http://do-furnasnetd/donet/gga/Lists/Planos");
        $("a[accesskey='W']").css("pointer-events", "none");

        var spans = document.getElementsByTagName("span");
        for (var i = 0; i < spans.length; i++) {
            if (spans[i].innerHTML == "novo item") {
                $(spans[i]).closest("td").hide();
                break;
            }
        }
        var url = _spPageContextInfo.webAbsoluteUrl + "/SitePages/SolicitacaoInclusao.aspx";
        $("#idHomePageNewItem").attr("onclick", "OpenDialog('" + url + "','Solicitação de Inclusão');return false;");
    }
    else {
        var spans = document.getElementsByTagName("span");
        for (var i = 0; i < spans.length; i++) {
            if (spans[i].innerHTML == "novo item") {
                spans[i].innerHTML = "Solicitar inclusão de um novo Plano";
                break;
            }
        }

        var url = _spPageContextInfo.webAbsoluteUrl + "/SitePages/SolicitacaoInclusao.aspx";
        $("#idHomePageNewItem").attr("onclick", "OpenDialog('" + url + "','Solicitação de Inclusão');return false;");
    }

});

function e_Lista() {
    var pl = location.href.split('/');
    for (i = 0; i < pl.length; i++) {
        if (pl[i] == "Planos") {
            return true;
        }
    }
}


function OpenDialog(url, title) {
    var options = {
        url: url,
        width: 250,
        height: 350,
        title: title,
        dialogReturnValueCallback: Function.createDelegate(null, CloseDialogCallback)
    };
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
}

function CloseDialogCallback(dialogResult, returnValue) {
    //location.reload();
}