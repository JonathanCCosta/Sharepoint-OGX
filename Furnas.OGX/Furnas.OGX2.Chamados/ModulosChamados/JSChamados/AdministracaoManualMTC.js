$(document).ready(function () {
    var arrLinksUteis = LoadRESTAPIParametros(_spPageContextInfo.webAbsoluteUrl, "Links Administrativos Manuais MTC", "$orderby=Title")
    LoadAdminItens(arrLinksUteis);
});

function LoadRESTAPIParametros(webURL, listname, parametro) {
    var urlWeb = webURL + "/_api/web/lists/getbytitle('" + listname + "')/items?" + parametro;
    var retorno = [];
    $.ajax({
        url: urlWeb,
        type: "GET",
        headers: { "accept": "application/json;odata=verbose" },
        dataType: "json",
        cache: true,
        async: false,
        success: function (data) {

            retorno = data.d.results;
        },
        error: function (error) {
            alert(JSON.stringify(error));
        }
    });
    return retorno;
}


function LoadAdminItens(data) {
    var categoriaatual = "";

    var builderHTMLPagina = [];
    $.each(data, function (i, value) {
        var urlFigura = '';

        if (value.BackgroundImageLocation.Url != null || value.BackgroundImageLocation.Url != undefined) {
            urlFigura = value.BackgroundImageLocation.Url;
        }

        /*if (categoriaatual != value.Grupo) {
            categoriaatual = value.Grupo;
            builderHTMLPagina.push('<div class="atalhosCategorias"><h3 class="titulo-categoria">' + value.Grupo + '</h3>');
        }*/
        builderHTMLPagina.push('<div class="atalhos">');
        builderHTMLPagina.push('<a class="atalho" href="' + value.LinkLocation.Url + '">');
        builderHTMLPagina.push('<img class="imagemPagina"src="' + urlFigura + '">');
        builderHTMLPagina.push('<span class="atalho-label">' + value.Title + '</span>');
        builderHTMLPagina.push('</a>');
        builderHTMLPagina.push('</div>');

        if (categoriaatual != value.Grupo) {
            builderHTMLPagina.push('</div>');
        }


    });
    document.getElementById('MontarHTMLPagina').innerHTML = builderHTMLPagina.join('');
}

