$(document).ready(function () {
    LoadModelos();

    $("#btnUpload").click(function () {
        if ($("#ddlCiclo").val() == 0) { alert("Selecione um Ciclo e Tipo para a Importação!"); return false; }
        if ($("#ddlTipos").val() == 0) { alert("Selecione um Ciclo e Tipo para a Importação!"); return false; }

        if (document.getElementById('flpArquivo').files.length === 0) {
            alert("Selecione um Ciclo, um Modelo e um Arquivo para a realização a importação!");
            return false;
        }
        else {
            if (confirm("Deseja continuar o carregamento do modelo selecionado?")) {
                ShowProgress();
                return true;
            }

            return false;
        }

        return false;

    });
});

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

function LoadModelos() {
    var urlQuery = _spPageContextInfo.webServerRelativeUrl + "/_vti_bin/listdata.svc/Templates";
    var retornoHTML = [];
    $.getJSON(urlQuery,
        function (data) {
            retornoHTML.push('<span>');
            retornoHTML.push('<h3>Modelo para Download</h3>');
            retornoHTML.push('</span>');

            $.each(data.d.results, function (i, item) {
                if (item.PlanoValue == "Gestão Interna") {
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