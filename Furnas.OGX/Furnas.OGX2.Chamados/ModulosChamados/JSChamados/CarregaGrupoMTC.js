

/* SRINT 7 */
/* 28960 */
/* Carrega o valor padrão 00 para o campo */
/* SharePoint remove 0 a esquerda */
/* Usado em Grupo MTC e Subgrupo MTC */
$(document).ready(function () {


    //if ($("input[title='Código Campo Obrigatório'").val() == ""){
    //$("input[title='Código Campo Obrigatório'").val("00");
    $("input[title='Código do Grupo Campo Obrigatório'").attr('placeholder', '00');
    $("input[title='Código do Subgrupo Campo Obrigatório'").attr('placeholder', '00');

    $("select[title='Código do Grupo Campo Obrigatório']").prepend("<option value='0' selected='selected'>Selecione uma opção</option>");

    //}

    $("input[title='Código do Grupo Campo Obrigatório'").attr('maxlength', '2');

    $("input[title='Código do Subgrupo Campo Obrigatório'").attr('maxlength', '2');


    $("input[title='Código do Grupo Campo Obrigatório'").attr('onkeypress', 'return event.charCode >= 48 && event.charCode <= 57');
    $("input[title='Código do Subgrupo Campo Obrigatório'").attr('onkeypress', 'return event.charCode >= 48 && event.charCode <= 57');


    if ($("input[title='Código do Grupo Campo Obrigatório'").val() == "") {

        $("select[title='Código do Grupo Campo Obrigatório']").prepend("<option value='0' selected='selected'>Selecione uma opção</option>");

    }

    /*if($("input[title='Descricao Campo Obrigatório'").val() != "")
	{

	$("select[title='Grupo Campo Obrigatório']").prepend("<option value='0'>Selecione uma opção</option>");
	
	}*/

    /*var input = $("input[title='Código Campo Obrigatório'");
    input.addEventListener('keydown', function (e) {
    var original = this.getAttribute('value');
    var novo = this.value;
    if (novo == original) this.value = '';
});*/



});



function PreSaveAction() {


    var retorno = true;

    $('#ErroDataCustomizado11').remove();

    if ($("select[title='Código do Grupo Campo Obrigatório']").val() == "0") {
        $("select[title='Código do Grupo Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado11' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opção.</span>");
        retorno = false;
        return retorno;
    }


    if (retorno == true) {

        $('#ErroDataCustomizado11').remove();

        var grupo = $("input[title='Código do Grupo Campo Obrigatório']");
        var sugbgrupo = $("input[title='Código do Subgrupo Campo Obrigatório']");

        if (grupo.length > 0) {
            if ($("input[title='Código do Grupo Campo Obrigatório']").val().length == 2)
                return retorno;

            if ($("input[title='Código do Grupo Campo Obrigatório']").val().length < 2) {

                var valor = $("input[title='Código do Grupo Campo Obrigatório']").val();

                $("input[title='Código do Grupo Campo Obrigatório']").val("0" + valor);

                retorno = true;


            }


        }

        if (sugbgrupo.length > 0) {

            if ($("input[title='Código do Subgrupo Campo Obrigatório']").val().length == 2)
                return retorno;


            if ($("input[title='Código do Subgrupo Campo Obrigatório']").val().length < 2) {

                var valor = $("input[title='Código do Subgrupo Campo Obrigatório']").val();

                $("input[title='Código do Subgrupo Campo Obrigatório']").val("0" + valor);

                retorno = true;


            }

        }

    }

    return retorno;


}

