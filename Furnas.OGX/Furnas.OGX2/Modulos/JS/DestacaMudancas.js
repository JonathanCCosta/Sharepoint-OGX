$(document).ready(function () {
    $('input[value=Fechar]').click(function () { history.go(-1); });
    var param = getQueryString("ID");

    var item = PlanoAlterado(param);

    var aTags = document.getElementsByTagName("a");

    var colunas = item.ColunasAlteradas.split(";");

    if (item.AlteracaoSolicitada != "Exclusão") {
        $.each(colunas, function (j, itens) {
            $(".ms-standardheader").filter(function () {
                return this.innerText == itens;
            }).closest("tr").css("background-color", "rgba(172, 254, 174, 1)");
            /*for (var i = 0; i < aTags.length; i++) {
				if (aTags[i].textContent == itens) {
				    aTags[i];
				}
			}*/
        });
    }
    else {
        $(".ms-standardheader").filter(function () {
            return this.innerText == "Justificativa da Exclusão";
        }).closest("tr").css("background-color", "rgba(172, 254, 174, 1)");
    }


});

function PlanoAlterado(id) {
    var urlQuery = _spPageContextInfo.webAbsoluteUrl + "/_vti_bin/listdata.svc/SolicitaçãoDeMudanças?$filter=ID eq " + id;
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
                //ret = data.d.results[0].ColunasAlteradas;
                ret = data.d.results[0];
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