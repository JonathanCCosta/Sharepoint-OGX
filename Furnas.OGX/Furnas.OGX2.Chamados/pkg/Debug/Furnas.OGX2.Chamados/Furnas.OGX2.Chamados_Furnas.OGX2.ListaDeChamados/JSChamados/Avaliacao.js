$(document).ready(function () {
    $("#pageTitle").hide();

    var ID = location.search.split("IDChamado=").pop();
    $("#IDChamado").val(ID);
    $("#IDChamado").hide(); $("#NumeroChamado").hide(); $("#TextoClassicacao").hide();

    $("#tipoAvaliacao").attr("style", "display:none");

    /*var duplicidade = false;

    var urlQuery = _spPageContextInfo.webServerRelativeUrl + "/_vti_bin/listdata.svc/PesquisaDeSatisfação?$filter=NúmeroDoChamadoId eq " + ID;
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
                $(".container").hide();
                $("#ContainerDuplicidade").show();
                duplicidade = true;
            }
        }
    });

    if (!duplicidade) {*/
        urlQuery = _spPageContextInfo.webServerRelativeUrl + "/_vti_bin/listdata.svc/Chamados?$filter=ID eq " + ID;
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
                    $("#codigo").text("Número do Chamado: " + data.d.results[0].NúmeroDoChamado);
                    $("#NumeroChamado").val(data.d.results[0].NúmeroDoChamado);
                }
            }
        });
    //}


    $("#Enviar").click(function () {
        if ($("#tipoAvaliacao").text() == " -- ") {
            alert("Selecione as estrelas para avaliar!");
            return false;
        }
    });
});


function Avaliar(estrela) {
    var url = window.location;
    url = url.toString()
    url = url.split("SitePages");
    url = url[0];

    var s1 = document.getElementById("s1").src;
    var s2 = document.getElementById("s2").src;
    var s3 = document.getElementById("s3").src;
    var s4 = document.getElementById("s4").src;
    var s5 = document.getElementById("s5").src;
    //var avaliacao = "Avaliar";
    var avaliacao = 0;

    if (estrela == 5) {
        if (s5 == url + "SiteAssets/star0.png") {
            document.getElementById("s1").src = "../SiteAssets/star1.png";
            document.getElementById("s2").src = "../SiteAssets/star1.png";
            document.getElementById("s3").src = "../SiteAssets/star1.png";
            document.getElementById("s4").src = "../SiteAssets/star1.png";
            document.getElementById("s5").src = "../SiteAssets/star1.png";
            //avaliacao = "Ótima";
            avaliacao = "5";
        } else {
            document.getElementById("s1").src = "../SiteAssets/star1.png";
            document.getElementById("s2").src = "../SiteAssets/star1.png";
            document.getElementById("s3").src = "../SiteAssets/star1.png";
            document.getElementById("s4").src = "../SiteAssets/star1.png";
            document.getElementById("s5").src = "../SiteAssets/star0.png";
            //avaliacao = "Muito Boa";
            avaliacao = "4";
        }
    }

    //ESTRELA 4
    if (estrela == 4) {
        if (s4 == url + "SiteAssets/star0.png") {
            document.getElementById("s1").src = "../SiteAssets/star1.png";
            document.getElementById("s2").src = "../SiteAssets/star1.png";
            document.getElementById("s3").src = "../SiteAssets/star1.png";
            document.getElementById("s4").src = "../SiteAssets/star1.png";
            document.getElementById("s5").src = "../SiteAssets/star0.png";
            //avaliacao = "Muito Boa";
            avaliacao = "4";
        } else {
            document.getElementById("s1").src = "../SiteAssets/star1.png";
            document.getElementById("s2").src = "../SiteAssets/star1.png";
            document.getElementById("s3").src = "../SiteAssets/star1.png";
            document.getElementById("s4").src = "../SiteAssets/star0.png";
            document.getElementById("s5").src = "../SiteAssets/star0.png";
            //avaliacao = "Boa";
            avaliacao = "3";
        }
    }

    //ESTRELA 3
    if (estrela == 3) {
        if (s3 == url + "SiteAssets/star0.png") {
            document.getElementById("s1").src = "../SiteAssets/star1.png";
            document.getElementById("s2").src = "../SiteAssets/star1.png";
            document.getElementById("s3").src = "../SiteAssets/star1.png";
            document.getElementById("s4").src = "../SiteAssets/star0.png";
            document.getElementById("s5").src = "../SiteAssets/star0.png";
            //avaliacao = "Boa";
            avaliacao = "3";
        } else {
            document.getElementById("s1").src = "../SiteAssets/star1.png";
            document.getElementById("s2").src = "../SiteAssets/star1.png";
            document.getElementById("s3").src = "../SiteAssets/star0.png";
            document.getElementById("s4").src = "../SiteAssets/star0.png";
            document.getElementById("s5").src = "../SiteAssets/star0.png";
            //avaliacao = "Ruim";
            avaliacao = "2";
        }
    }

    //ESTRELA 2
    if (estrela == 2) {
        if (s2 == url + "SiteAssets/star0.png") {
            document.getElementById("s1").src = "../SiteAssets/star1.png";
            document.getElementById("s2").src = "../SiteAssets/star1.png";
            document.getElementById("s3").src = "../SiteAssets/star0.png";
            document.getElementById("s4").src = "../SiteAssets/star0.png";
            document.getElementById("s5").src = "../SiteAssets/star0.png";
            //avaliacao = "Ruim";
            avaliacao = "2";
        } else {
            document.getElementById("s1").src = "../SiteAssets/star1.png";
            document.getElementById("s2").src = "../SiteAssets/star0.png";
            document.getElementById("s3").src = "../SiteAssets/star0.png";
            document.getElementById("s4").src = "../SiteAssets/star0.png";
            document.getElementById("s5").src = "../SiteAssets/star0.png";
            //avaliacao = "Péssima";
            avaliacao = "1";
        }
    }

    //ESTRELA 1
    if (estrela == 1) {
        //if (s1 == url + "SiteAssets/star0.png") {
        document.getElementById("s1").src = "../SiteAssets/star1.png";
        document.getElementById("s2").src = "../SiteAssets/star0.png";
        document.getElementById("s3").src = "../SiteAssets/star0.png";
        document.getElementById("s4").src = "../SiteAssets/star0.png";
        document.getElementById("s5").src = "../SiteAssets/star0.png";
        //avaliacao = "Péssima";
        avaliacao = "1";
        //} 
    }

    document.getElementById('tipoAvaliacao').innerHTML = avaliacao;

    $("#TextoClassicacao").val(avaliacao);
    $("#tipoAvaliacao").attr("style", "display:none");
    
}

function EnviarAvaliacao() {

}

/*

function EnviarAvaliacao(idSolicitacao) {

    var avaliacao = $("#tipo-avaliacao").text();

    if (avaliacao == "Avaliar") {
        alert("Você deve avaliar a solicitação para que ela possa ser enviada.")
    }

    else {


        var comentario = $("#comentario").val();

        if (comentario == "") {
            comentario = "Nehum comentrário";
        }





        //var parametro = "?$filter=ID eq " +idNoticia;

        var idUsuarioLogado = parseInt(UsuarioLogado());

        $.ajax({
            url: _spPageContextInfo.webServerRelativeUrl + "/_api/web/lists/getbytitle('Avaliações')/items",
            type: "POST",
            data: JSON.stringify({
                '__metadata': { 'type': 'SP.Data.AvaliacoesListItem' },
                'codigoId': idSolicitacao,
                'comentario': comentario,
                'avaliacao': avaliacao,
                'usuarioId': idUsuarioLogado

            }),

            headers: {
                "Accept": "application/json;odata=verbose",
                "content-type": "application/json;odata=verbose",
                "X-RequestDigest": $("#__REQUESTDIGEST").val()
            },

            success: function (data) {
                alert("Avaliação enviada com sucesso ! Você será enviado para página principal.");

                window.location.href = _spPageContextInfo.webServerRelativeUrl;

            },

            error: function (err) {
                alert("Ocorreu um erro" + JSON.stringify(err));
            }



        })

    }

}



function UsuarioLogado() {

    var idUsuarioLogado = "";

    $.ajax({
        url: _spPageContextInfo.webServerRelativeUrl + "/_api/web/currentuser",
        type: "GET",
        headers: { "Accept": "application/json;odata=verbose" },

        async: false,

        success: function (data) {

            idUsuarioLogado = data.d.Id;


        },


        error: function (err) {
            alert("Ocorreu um erro" + JSON.stringify(err));
        }
    });
    return idUsuarioLogado;

}*/