$(document).ready(function () {

    $("h1").text("IMPORTAR ARQUIVOS DO SGPMR");
    $($("h2")[0]).text("Nome do Ciclo");
    $($("h2")[1]).text("Tipo de Plano");

    LoadModelos();

    try {
        if ($("#ddlTipos").val() == "0") {
            var param = getQueryString("Tipo");
            $("#ddlTipos").val(param);
        }
    }
    catch (e) { }

    $("#btnUpload").click(function () {
        if ($("#ddlCiclo").val() == 0) { alert("Selecione um Ciclo, um Modelo e um Arquivo para a realizaÃ§Ã£o a importaÃ§Ã£o!"); return false; }
        if ($("#ddlTipos").val() == 0) { alert("Selecione um Ciclo, um Modelo e um Arquivo para a realizaÃ§Ã£o a importaÃ§Ã£o!"); return false; }//alert("Selecione um Modelo de ImportaÃ§Ã£o!"); return false; }

        if (document.getElementById('flpArquivo').files.length === 0) {
            //alert("Selecione um arquivo para carregamento!");
            alert("Selecione um Ciclo, um Modelo e um Arquivo para a realizaÃ§Ã£o a importaÃ§Ã£o!");
            return false;
        }
        else {
            if (confirm("Deseja continuar o carregamento do modelo selecionado?")) {
                ShowProgress();
                return true;
            }

            return false;
        }
    });
});
function LoadModelos() {
    var urlQuery = _spPageContextInfo.webServerRelativeUrl + "/_vti_bin/listdata.svc/Templates";
    var retornoHTML = [];
    $.getJSON(urlQuery,
        function (data) {
            retornoHTML.push('<span>');
            retornoHTML.push('<h3>Modelos para Download</h3>');
            retornoHTML.push('</span>');

            $.each(data.d.results, function (i, item) {
                if (item.PlanoValue != "Gestão Interna") {
                    var fileType = GetFileTypeImage();
                    retornoHTML.push('<div class="itemModelo">');
                    retornoHTML.push('<a href="' + item.__metadata.media_src + '">');
                    retornoHTML.push('<img src="' + fileType + '" alt="' + item.PlanoValue + '" />');//</span>');
                    retornoHTML.push('<h2>' + item.PlanoValue + '</h2></a>');
                    retornoHTML.push('</div>');
                }
            });
            document.getElementById('containerModelos').innerHTML = retornoHTML.join('');
        });
}

function GetFileTypeImage() {
    return "/_layouts/15/images/lg_icxlsx.png";
}

function OpenNewProcesso() {

    var options = {
        url: "../Lists//Ciclos/NewForm.aspx",
        //width: 900,
        //height: 1000,
        dialogReturnValueCallback: silentCallback
    }

    SP.UI.ModalDialog.showModalDialog(options);
    return false;
}

function silentCallback(dialogResult, returnValue) {

    if (dialogResult === 1) {
        //PreencheCiclo();
        alert("Ciclo criado com sucesso!");
        location.href = _spPageContextInfo.webServerRelativeUrl + "/SitePages/Importacao.aspx?Tipo=" + $("#ddlTipos").val();
    }
}

function PreencheCiclo() {
    var urlQuery = _spPageContextInfo.webAbsoluteUrl + "/_vti_bin/listdata.svc/Ciclos?$orderby=NomeDoCiclo asc";

    $.ajax({
        type: "GET",
        contentType: "application/json;charset=ISO-8859-1",
        url: urlQuery,
        cache: false,
        async: false,
        dataType: 'json',
        headers: { "Accept": "application/json; odata=verbose" },
        success: function (data) {
            $("select[id='ddlCiclo']").empty();
            $("select[id='ddlCiclo']").append('<option selected="selected" value="0">-- Selecione um Ciclo --</option>');

            if (data.d.results.length > 0) {
                $.each(data.d.results, function (j, item) {
                    $("select[id='ddlCiclo']").append('<option value="' + item.ID + '">' + item.NomeDoCiclo + '</option>');
                });
            }
        }
    });

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