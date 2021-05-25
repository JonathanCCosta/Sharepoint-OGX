function PreSaveAction() {
    var retorno = true;
    var urlQuery = _spPageContextInfo.webAbsoluteUrl + "/_vti_bin/listdata.svc/Ciclos?$filter=NomeDoCiclo eq '" + $("input[title='Nome do Ciclo Campo Obrigatório']").val() + "'";

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
                if (document.location.pathname.indexOf("/EditForm.aspx") > -1) {
                    if (getWQuerystring("ID") != data.d.results[0].ID) {
                        alert("O nome do ciclo não pode ser repetido.");
                        retorno = false;
                    }
                }
                else {
                    alert("O nome do ciclo não pode ser repetido.");
                    retorno = false;
                }

            }
        }
    });


    return retorno;
}

function getWQuerystring(ji) {
    hu = parent.location.search.substring(1).replace("[^a-zA-Z0-9/]", "-");
    gy = hu.split("&");
    for (i = 0; i < gy.length; i++) {
        ft = gy[i].split("=");
        if (ft[0] == ji) {
            return ft[1];
        }
    }
}