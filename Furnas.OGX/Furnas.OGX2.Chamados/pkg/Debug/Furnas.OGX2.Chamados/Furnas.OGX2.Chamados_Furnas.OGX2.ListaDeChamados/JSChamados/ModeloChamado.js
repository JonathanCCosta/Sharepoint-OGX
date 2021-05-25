$(document).ready(function () {
    $("input[title='Título']").closest("tr").hide();

    if (getWQuerystring("Mode") == "Upload") {
        $("select[title='Tipo de Chamado Campo Obrigatório']").prepend("<option value='0' selected='selected'>Selecione uma opção</option>");
    }
    else {
        if ($("select[title='Tipo de Chamado Campo Obrigatório']").val() != "0")
            $("select[title='Tipo de Chamado Campo Obrigatório']").attr('disabled', 'disabled');
    }

});

function PreSaveAction() {
    var retorno = true;

    var urlQuery = _spPageContextInfo.webAbsoluteUrl + "/_vti_bin/listdata.svc/ModeloDeChamado?$filter=Nome eq '" + $("input[title='Nome Campo Obrigatório']").val() + $(".ms-fileField-fileExt").text() + "'";

    $.ajax({
        type: "GET",
        contentType: "application/json;charset=ISO-8859-1",
        url: urlQuery,
        cache: false,
        async: false,
        dataType: 'json',
        headers: { "Accept": "application/json; odata=verbose" },
        success: function (data) {
            if (data.d.results.length > 0) {
                $.each(data.d.results, function (i, item) {
                    if (getWQuerystring("ID") != item.ID) {
                        //alert("O nome do Modelo de Chamado não pode ser igual para um mesmo Tipo de Chamado existente.");
                        alert("Este valor já existe na lista.");
                        retorno = false;
                        return false;
                    }
                });
            }
        }
    });

    if (!retorno)
        return retorno;

    urlQuery = _spPageContextInfo.webAbsoluteUrl + "/_vti_bin/listdata.svc/ModeloDeChamado?$filter=ModeloDeChamado eq '" + $("input[title='Modelo de Chamado Campo Obrigatório']").val() + "'";

    $.ajax({
        type: "GET",
        contentType: "application/json;charset=ISO-8859-1",
        url: urlQuery,
        cache: false,
        async: false,
        dataType: 'json',
        headers: { "Accept": "application/json; odata=verbose" },
        success: function (data) {
            if (data.d.results.length > 0) {
                $.each(data.d.results, function (i, item) {
                    if (getWQuerystring("ID") != item.ID) {
                        //alert("O nome do Modelo de Chamado não pode ser igual para um mesmo Tipo de Chamado existente.");
                        alert("Este valor já existe na lista.");
                        retorno = false;
                        return false;
                    }
                });
            }
        }
    });

    if (!retorno)
        return retorno;

    urlQuery = _spPageContextInfo.webAbsoluteUrl + "/_vti_bin/listdata.svc/ModeloDeChamado?$filter=ModeloDeChamado eq '" + $("input[title='Modelo de Chamado Campo Obrigatório']").val() + "' and TipoDeChamadoId eq " + $("select[title='Tipo de Chamado Campo Obrigatório']").val();

    $.ajax({
        type: "GET",
        contentType: "application/json;charset=ISO-8859-1",
        url: urlQuery,
        cache: false,
        async: false,
        dataType: 'json',
        headers: { "Accept": "application/json; odata=verbose" },
        success: function (data) {
            if (data.d.results.length > 0) {
                $.each(data.d.results, function (i, item) {
                    if (getWQuerystring("ID") != item.ID) {
                        //alert("O nome do Modelo de Chamado não pode ser igual para um mesmo Tipo de Chamado existente.");
                        alert("Este valor já existe na lista.");
                        retorno = false;
                        return false;
                    }
                });
            }
        }
    });

    try {
        $('#ErroDataCustomizado1').remove(); $('#ErroDataCustomizado2').remove(); $('#ErroDataCustomizado3').remove();
    }
    catch (ex) { }


    if ($("select[title='Tipo de Chamado Campo Obrigatório']").val() == "0") {
        $("select[title='Tipo de Chamado Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado1' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opção.</span>");
        retorno = false;
    }

    if ($("input[title='Nome Campo Obrigatório']").val() == "") {
        $("input[title='Nome Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado2' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
        retorno = false;
    }

    if ($("input[title='Modelo de Chamado Campo Obrigatório']").val() == "") {
        $("input[title='Modelo de Chamado Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado3' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
        retorno = false;
    }

    return retorno;
}

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
