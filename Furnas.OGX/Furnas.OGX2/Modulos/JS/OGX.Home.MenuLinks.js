$(document).ready(function () {
    var paramSource = getQueryString("ContentTypeId");

    if (paramSource == "0x0100229B15713D00894FBFBB8FCC87E2160900CF1A4A0DB08A2247BE86F37E36F3B690") {
        $("input[aria-describedby='URL']").closest("tr").hide();
        $("input[title='TipoNome']").closest("tr").hide();
    }
});

function PreSaveAction() {

    var paramSource = getQueryString("ContentTypeId");

    if (paramSource == "0x0100229B15713D00894FBFBB8FCC87E2160900CF1A4A0DB08A2247BE86F37E36F3B690") {
        if ($("select[title='Número de Níveis Campo Obrigatório'] option:selected").val() == "2 Níveis")
            $("input[aria-describedby='URL']").val("http://do-furnasnetd/donet/gga/SitePages/PaginaCard.aspx?app=" + $("select[title='Tipo de Card Campo Obrigatório'] option:selected").val());
        else
            $("input[aria-describedby='URL']").val("http://do-furnasnetd/donet/gga/SitePages/PaginaCardNivel.aspx?app=" + $("select[title='Tipo de Card Campo Obrigatório'] option:selected").val());
    }

    $("input[title='TipoNome']").val($("select[title='Tipo de Card Campo Obrigatório'] option:selected").text());

    return true;
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