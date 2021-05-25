//$(document).ready(function () {

//});

function PreSaveAction() {
    try {
        $('#ErroDataCustomizado').remove(); $('#ErroDataCustomizado1').remove(); $('#ErroDataCustomizado2').remove(); $('#ErroDataCustomizado3').remove();
    }
    catch (ex) { }

    var flag = true;

    if ($("select[id='ctl00_SPWebPartManager1_g_36da9f4d_cf51_4148_811b_bd1064d911e9_ctl00_ctl05_ctl02_ctl00_ctl00_ctl05_ctl00_ctl00_ddlExistingPicture'] option:selected").val() != "-1") {
        flag = true;
    }
    else {
        flag = false;
    }

    if ($("input[title='Título Campo Obrigatório']").val() == "") {
        $("input[title='Título Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado1' class='ms-formvalidation ms-csrformvalidation'>Você não pode deixar em branco.</span>");
        flag = false;
    }
    if ($("input[aria-label='Digite o endereço Web: Campo Obrigatório']").val() == "http://" || $("input[aria-label='Digite o endereço Web: Campo Obrigatório']").val() == "") {
        $("input[aria-label='Digite a descrição:']").closest("td").append("<span id='ErroDataCustomizado2' class='ms-formvalidation ms-csrformvalidation'>Você não pode deixar em branco.</span>");
        flag = false;
    }
    if ($("input[title='Ordem de exibição Banner Campo Obrigatório']").val() == "") {
        $("input[title='Ordem de exibição Banner Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado3' class='ms-formvalidation ms-csrformvalidation'>Você não pode deixar em branco.</span>");
        flag = false;
    }
    if ($("input[id*='imageFieldPicture']").val() == "" && flag == false) {
        $("input[id*='imageFieldPicture']").closest("td").append("<span id='ErroDataCustomizado' class='ms-formvalidation ms-csrformvalidation'>Faça upload de uma imagem ou use uma existente.</span>");
        flag = false;
    }

    if (flag) {
        var query = _spPageContextInfo.webAbsoluteUrl + "/_api/web/lists/getbytitle('Banner')/ItemCount";

        $.ajax({
            type: "GET",
            contentType: "application/json;charset=ISO-8859-1",
            url: encodeURI(query),
            cache: false,
            async: false,
            dataType: 'json',
            headers: { "Accept": "application/json; odata=verbose" },
            success: function (data) {
                if (data.d.ItemCount >= 6) {
                    alert("Só permitir incluir no máximo 6 banners para exibição no sistema.");
                    flag = false;
                }
            }
        });
    }

    return flag;
}