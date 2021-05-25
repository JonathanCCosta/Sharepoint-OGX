
/* SPRINT 7 */


function PreSaveAction() {

    var retorno = true;

    $('#ErroDataCustomizado11').remove();

    if ($("input[title='Código do Fabricante Campo Obrigatório']").val().length != 0) {

        if ($("input[title='Código do Fabricante Campo Obrigatório']").val().length < 3) {

            $("input[title='Código do Fabricante Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado11' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>* O Código do Fabricante deve ter no mínimo 3 caracteres.</span>");
            retorno = false;


        }
        
    }

    return retorno;

}