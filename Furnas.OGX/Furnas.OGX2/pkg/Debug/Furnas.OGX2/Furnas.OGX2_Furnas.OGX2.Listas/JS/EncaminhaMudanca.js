$(document).ready(function () {
    var param = getQueryString("Acoes");

    var paramSource = getQueryString("Source");

    if (param == "Sim" || paramSource == null)
        $('#s4-workspace').animate({ scrollTop: $('#MSOZoneCell_WebPartWPQ2')[0].scrollHeight });

    $("#idHomePageNewItem").hide();

    var url = _spPageContextInfo.webAbsoluteUrl + "/SitePages/SolicitacaoExclusao.aspx";//?IDE=" + getQueryString("ID");
    $("#excluirPlano").attr("onclick", "OpenDialogExclusao('" + url + "','Solicitação de Exclusão');return false;");
    /*var spans = document.getElementsByTagName("span");
    for (var i = 0; i < spans.length; i++) {
        if (spans[i].innerHTML == "novo item") {
            spans[i].innerHTML = "Solicitar	mudança no Plano";
            break;
        }
    }

    var paramID = getQueryString("ID");
    var tipo = TipoPlano(paramID);

    if (tipo == "Plano de Melhoria") {
        var u = _spPageContextInfo.webAbsoluteUrl + "/Lists/PlanosMudanca/NewForm.aspx?ContentTypeId=0x010033F536D5074F4318A40229399F349CC100F66CBCAA26E2134A95FDAEE6150C06B4&Source=" + _spPageContextInfo.webAbsoluteUrl + "/Lists/Planos/DispForm.aspx?ID=" + paramID;
        $("#idHomePageNewItem").attr("href", u);
        $("#idHomePageNewItem").attr("Onclick", "");
    }
    else if (tipo == "Plano de Reforço") {
        var u = _spPageContextInfo.webAbsoluteUrl + "/Lists/PlanosMudanca/NewForm.aspx?ContentTypeId=0x010033F536D5074F4318A40229399F349CC100F66CBCAA26E2134A95FDAEE6150C06B4&Source=" + _spPageContextInfo.webAbsoluteUrl + "/Lists/Planos/DispForm.aspx?ID=" + paramID;
        $("#idHomePageNewItem").attr("href", u);
        $("#idHomePageNewItem").attr("Onclick", "");

    }
    else {
        var u = _spPageContextInfo.webAbsoluteUrl + "/Lists/PlanosMudanca/NewForm.aspx?ContentTypeId=0x010033F536D5074F4318A40229399F349CC100F66CBCAA26E2134A95FDAEE6150C06B4&Source=" + _spPageContextInfo.webAbsoluteUrl + "/Lists/Planos/DispForm.aspx?ID=" + paramID;
        $("#idHomePageNewItem").attr("href", u);
        $("#idHomePageNewItem").attr("Onclick", "");
    }*/
});

function TipoPlano(id) {
    var urlQuery = _spPageContextInfo.webAbsoluteUrl + "/_vti_bin/listdata.svc/Planos?$filter=ID eq " + id;
    var ret;
    $.ajax({
        type: "GET",
        contentType: "application/json;charset=ISO-8859-1",
        url: encodeURI(urlQuery),
        cache: false,
        async: false,
        dataType: 'json',
        headers: { "Accept": "application/json; odata=verbose" },
        success: function (data) {
            if (data.d.results.length > 0) {
                ret = data.d.results[0].TipoDeConteúdo;
            }
        }
    });

    return ret;
}

function getQueryString() {
    var key = false, res = {}, itm = null;
    // get the query string without the ?
    var qs = location.search.substring(1);
    // check for the key as an argument
    if (arguments.length > 0 && arguments[0].length > 1)
        key = arguments[0];
    // make a regex pattern to grab key/value
    var pattern = /([^&=]+)=([^&]*)/g;
    // loop the items in the query string, either
    // find a match to the argument, or build an object
    // with key/value pairs
    while (itm = pattern.exec(qs)) {
        if (key !== false && decodeURIComponent(itm[1]) === key)
            return decodeURIComponent(itm[2]);
        else if (key === false)
            res[decodeURIComponent(itm[1])] = decodeURIComponent(itm[2]);
    }

    return key === false ? res : null;
}

function OpenDialog(url, title) {
    var options = {
        url: url,
        width: 250,
        height: 350,
        title: title,
        function(dialogResult) {  
	        if (dialogResult != SP.UI.DialogResult.cancel) {  
	            alert('Exclusão do plano solicitado com sucesso!');
	            var url = _spPageContextInfo.webAbsoluteUrl + "/Lists/Planos/DispForm.aspx?ID=" + getQueryString("ID") + "&Source=" + _spPageContextInfo.webAbsoluteUrl + "/Lists/Planos" + "&Acoes=Sim";
	            window.location.href = url;
	        }  
}
//Function.createDelegate(null, CloseDialogCallbackExclusao)
};
SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
}

function CloseDialogCallback(dialogResult, returnValue) {
    //location.reload();
}

function OpenDialogExclusao(url, title) {
    var options = {
        url: url,
        width: 500,
        height: 450,
        title: title,
        dialogReturnValueCallback: Function.createDelegate(null, CloseDialogCallbackExclusao)
    };
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
}

function CloseDialogCallbackExclusao(dialogResult, returnValue) {
    if(dialogResult == 1){
        alert('Exclusão do plano solicitada com sucesso!');
        var url = _spPageContextInfo.webAbsoluteUrl + "/Lists/Planos/DispForm.aspx?ID=" + getQueryString("ID") + "&Source=" + _spPageContextInfo.webAbsoluteUrl + "/Lists/Planos" + "&Acoes=Sim";
        window.location.href = url;
    }
}