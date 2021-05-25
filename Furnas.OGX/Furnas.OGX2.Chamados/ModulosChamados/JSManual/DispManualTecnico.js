$(document).ready(function () {

    var urlQuery = _spPageContextInfo.webAbsoluteUrl + "/_vti_bin/listdata.svc/ManualTécnicoDeCampo?$filter=ID eq " + getWQuerystring("ID");
    var tipo = ""; var status = "";
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
                tipo = data.d.results[0].TipoDeConteúdo;
                status = data.d.results[0].ControleDeStatusValue;
            }
        }
    });

    if (tipo != "Legado") {//solicitação
        if (status == "Aguardando Aprovação") {
            $('td.ms-formlabel:contains("Consecutivo")').parent().hide();
            $('td.ms-formlabel:contains("Revisão")').parent().hide();
            $('td.ms-formlabel:contains("Aprovado por")').parent().hide();
            $('td.ms-formlabel:contains("Data Aprovação")').parent().hide();
            $('td.ms-formlabel:contains("Data de Início de Vigência")').parent().hide();
            $('td.ms-formlabel:contains("Revisores")').parent().hide();
            $('td.ms-formlabel:contains("Justificativa Cancelamento")').parent().hide();
            $('td.ms-formlabel:contains("Data Limite Comentário")').parent().hide();
            //$('td.ms-formlabel:contains("Comentários")').parent().hide();
            if ($("td.ms-formlabel:contains('Comentários')").closest("tr").find("td:eq(1)").text().trim() == "Não há entradas existentes.")
                $('td.ms-formlabel:contains("Comentários")').parent().hide();
        }
        else if (status == "Cancelado") {
            $('td.ms-formlabel:contains("Consecutivo")').parent().hide();
            $('td.ms-formlabel:contains("Revisão")').parent().hide();
            $('td.ms-formlabel:contains("Aprovado por")').parent().hide();
            $('td.ms-formlabel:contains("Data Aprovação")').parent().hide();
            $('td.ms-formlabel:contains("Data de Início de Vigência")').parent().hide();
            $('td.ms-formlabel:contains("Revisores")').parent().hide();
            $('td.ms-formlabel:contains("Data Limite Comentário")').parent().hide();
            //$('td.ms-formlabel:contains("Comentários")').parent().hide();
            if ($("td.ms-formlabel:contains('Comentários')").closest("tr").find("td:eq(1)").text().trim() == "Não há entradas existentes.")
                $('td.ms-formlabel:contains("Comentários")').parent().hide();
        }
        else if (status == "Vigente") {
            $('td.ms-formlabel:contains("Data Limite Comentário")').parent().hide();
            $('td.ms-formlabel:contains("Comentários")').parent().hide();
        }
    }
});

function getWQuerystring(ji) {
    hu = location.search.substring(1).replace("[^a-zA-Z0-9/]", "-");
    gy = hu.split("&");
    for (i = 0; i < gy.length; i++) {
        ft = gy[i].split("=");
        if (ft[0] == ji) {
            return ft[1];
        }
    }
}