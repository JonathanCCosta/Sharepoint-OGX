var plano = ""; 
$(document).ready(function () {
	$(".ms-standardheader:contains('Nome do Ciclo')").closest("tr").hide();
	$(".ms-standardheader:contains('Tipo de Conteúdo')").closest("tr").hide();
	
	var param = $("select[title='Tipo de Conteúdo'] option:selected").text(); //getQueryString("ContentTypeId");
	
	if(param == "Plano Por Superação"){
		plano = "Superação";
	}
	else{
		plano = "Melhoria e Reforço";
	}
});

function PreSaveAction() {

	var dt = new Date();
	var nomeCiclo = $("select[title='Ciclo Campo Obrigatório'] option:selected").text() + "-" + dt.getFullYear() + "/" + (dt.getMonth() > 9 ? dt.getMonth() : "0" + dt.getMonth()) + "/" + (dt.getDay() > 9 ? dt.getDay() : "0" + dt.getDay());
	$("input[title='Nome do Ciclo']").val(nomeCiclo);
	
	if(plano == "Superação"){
		return Superacao();
	}
	else{
		return MelhoriaReforco();
	}
}

function Superacao(){ 
	try{
		$('#ErroDataCustomizado').remove();$('#ErroDataCustomizado1').remove();$('#ErroDataCustomizado2').remove();$('#ErroDataCustomizado3').remove();
		$('#ErroDataCustomizado4').remove();$('#ErroDataCustomizado5').remove();$('#ErroDataCustomizado6').remove();$('#ErroDataCustomizado7').remove();
		$('#ErroDataCustomizado8').remove();$('#ErroDataCustomizado9').remove();$('#ErroDataCustomizado10').remove();$('#ErroDataCustomizado11').remove();
		$('#ErroDataCustomizado12').remove();
	}
	catch(ex){}
	
	var dtNS = $("input[title='Data de Necessidade']").val();
	var dtAutEmissao = $("input[title='Data da Autorização']").val();
	var dtPrevImplant= $("input[title='Previsão da Implantação']").val();
	var dtEO= $("input[title='Data de Entrada em Operação']").val();
	var dtAMI = $("input[title='Aquisição de Materiais - Início']").val();
	var dtAMF = $("input[title='Aquisição de Materiais - Fim']").val();
	var dtOCI = $("input[title='Obras Civis - Início']").val();
	var dtOCF = $("input[title='Obras Civis - Fim']").val();
	var dtMEI = $("input[title='Montagem Eletromecânica - Início']").val();
	var dtMEF = $("input[title='Montagem Eletromecânica - Fim']").val();
	var dtCI = $("input[title='Comissionamento - Início']").val();
	var dtCF = $("input[title='Comissionamento - Fim']").val();
	var dtOC = $("input[title='Operação Comercial']").val();	

	var retorno = true;
	
	if(dtNS != "" && !isDate(dtNS)){
		$($("input[title='Data de Necessidade']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
		retorno = false;
	}
	if(dtAutEmissao!= "" && !isDate(dtAutEmissao)){
		$($("input[title='Data da Autorização']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado1' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
		retorno = false;
	}
	if(dtPrevImplant!= "" && !isDate(dtPrevImplant)){
		$($("input[title='Previsão da Implantação']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado2' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
		retorno = false;
	}
	if(dtEO!= "" && !isDate(dtEO)){
		$($("input[title='Data de Entrada em Operação']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado12' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
		retorno = false;
	}
	if(dtAMI!= "" && !isDate(dtAMI)){
		$($("input[title='Aquisição de Materiais - Início']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado3' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
		retorno = false;
	}
	if(dtAMF!= "" && !isDate(dtAMF)){
		$($("input[title='Aquisição de Materiais - Fim']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado4' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
		retorno = false;
	}
	if(dtOCI!= "" && !isDate(dtOCI)){
		$($("input[title='Obras Civis - Início']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado5' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
		retorno = false;
	}
	if(dtOCF!= "" && !isDate(dtOCF)){
		$($("input[title='Obras Civis - Fim']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado6' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
		retorno = false;
	}
	if(dtMEI != "" && !isDate(dtMEI)){
		$($("input[title='Montagem Eletromecânica - Início']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado7' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
		retorno = false;
	}

	if(dtMEF!= "" && !isDate(dtMEF)){
		$($("input[title='Montagem Eletromecânica - Fim']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado8' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
		retorno = false;
	}

	if(dtCI != "" && !isDate(dtCI)){
		$($("input[title='Comissionamento - Início']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado9' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
		retorno = false;
	}

	if(dtCF != "" && !isDate(dtCF)){
		$($("input[title='Comissionamento - Fim']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado10' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
		retorno = false;
	}

	if(dtOC != "" && !isDate(dtOC)){
		$($("input[title='Operação Comercial']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado11' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
		retorno = false;
	}
	
	
	if(retorno == true){
		if(dtAMI != "" && dtAMF!= ""){
			if((gerarData(dtAMF) < gerarData(dtAMI))) {
				//alert("Data de Aquisição de Materias - Início tem que ser menor que a data de Aquisição de Materiais - Fim!");
				$($("input[title='Aquisição de Materiais - Fim']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado4' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Data de Aquisição de Materias - Início tem que ser menor que a data de Aquisição de Materiais - Fim</span>");
				retorno = false;
			}
		}
		else if(dtOCI != "" && dtOCF != ""){
			if((gerarData(dtOCF) < gerarData(dtOCI))) {
				//alert("Data de Obras Civis - Início tem que ser menor que a data de Obras Civis - Fim!");
				$($("input[title='Obras Civis - Fim']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado6' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Data de Obras Civis - Início tem que ser menor que a data de Obras Civis - Fim</span>");
				retorno = false;
			}
		}
		else if(dtMEI != "" && dtMEF != ""){
			if((gerarData(dtMEF) < gerarData(dtMEI))) {
				//alert("Data de Montagem Eletromecânica - Início tem que ser menor que a data de Montagem Eletromecânica - Fim!");
				$($("input[title='Montagem Eletromecânica - Fim']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado8' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Data de Montagem Eletromecânica - Início tem que ser menor que a data de Montagem Eletromecânica - Fim</span>");
				retorno = false;
			}
		}
		else if(dtCI != "" && dtCF != ""){
			if((gerarData(dtCF) < gerarData(dtCI))) {
				//alert("Data de Comissionamento - Início tem que ser menor que a data de Comissionamento - Fim!");
				$($("input[title='Comissionamento - Fim']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado10' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Data de Comissionamento - Início tem que ser menor que a data de Comissionamento - Fim</span>");
				retorno = false;
			}
		}
	}

	return retorno;
}


function MelhoriaReforco(){ 
	try{
		$('#ErroDataCustomizado').remove();$('#ErroDataCustomizado1').remove();$('#ErroDataCustomizado2').remove();$('#ErroDataCustomizado3').remove();
		$('#ErroDataCustomizado4').remove();$('#ErroDataCustomizado5').remove();$('#ErroDataCustomizado6').remove();$('#ErroDataCustomizado7').remove();
		$('#ErroDataCustomizado8').remove();$('#ErroDataCustomizado9').remove();$('#ErroDataCustomizado10').remove();$('#ErroDataCustomizado11').remove();
	}
	catch(ex){}
	
	var dtNS = $("input[title='Data de Necessidade Sistêmica']").val();
	var dtAutEmissao = $("input[title='Data da Autorização/Emissão PMI']").val();
	var dtPrevImplant= $("input[title='Previsão da Implantação']").val();
	var dtAMI = $("input[title='Aquisição de Materiais - Início']").val();
	var dtAMF = $("input[title='Aquisição de Materiais - Fim']").val();
	var dtOCI = $("input[title='Obras Civis - Início']").val();
	var dtOCF = $("input[title='Obras Civis - Fim']").val();
	var dtMEI = $("input[title='Montagem Eletromecânica - Início']").val();
	var dtMEF = $("input[title='Montagem Eletromecânica - Fim']").val();
	var dtCI = $("input[title='Comissionamento - Início']").val();
	var dtCF = $("input[title='Comissionamento - Fim']").val();
	var dtOC = $("input[title='Operação Comercial']").val();	

	var retorno = true;
	
	if(dtNS != "" && !isDate(dtNS)){
		$($("input[title='Data de Necessidade Sistêmica']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
		retorno = false;
	}
	if(dtAutEmissao!= "" && !isDate(dtAutEmissao)){
		$($("input[title='Data da Autorização/Emissão PMI']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado1' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
		retorno = false;
	}
	if(dtPrevImplant!= "" && !isDate(dtPrevImplant)){
		$($("input[title='Previsão da Implantação']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado2' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
		retorno = false;
	}
	if(dtAMI!= "" && !isDate(dtAMI)){
		$($("input[title='Aquisição de Materiais - Início']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado3' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
		retorno = false;
	}
	if(dtAMF!= "" && !isDate(dtAMF)){
		$($("input[title='Aquisição de Materiais - Fim']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado4' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
		retorno = false;
	}
	if(dtOCI!= "" && !isDate(dtOCI)){
		$($("input[title='Obras Civis - Início']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado5' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
		retorno = false;
	}
	if(dtOCF!= "" && !isDate(dtOCF)){
		$($("input[title='Obras Civis - Fim']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado6' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
		retorno = false;
	}
	if(dtMEI != "" && !isDate(dtMEI)){
		$($("input[title='Montagem Eletromecânica - Início']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado7' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
		retorno = false;
	}

	if(dtMEF!= "" && !isDate(dtMEF)){
		$($("input[title='Montagem Eletromecânica - Fim']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado8' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
		retorno = false;
	}

	if(dtCI != "" && !isDate(dtCI)){
		$($("input[title='Comissionamento - Início']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado9' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
		retorno = false;
	}

	if(dtCF != "" && !isDate(dtCF)){
		$($("input[title='Comissionamento - Fim']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado10' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
		retorno = false;
	}

	if(dtOC != "" && !isDate(dtOC)){
		$($("input[title='Operação Comercial']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado11' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
		retorno = false;
	}
	
	
if(retorno == true){
	if(dtAMI != "" && dtAMF!= ""){
		if((gerarData(dtAMF) < gerarData(dtAMI))) {
			//alert("Data de Aquisição de Materias - Início tem que ser menor que a data de Aquisição de Materiais - Fim!");
			$($("input[title='Aquisição de Materiais - Fim']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado4' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Data de Aquisição de Materias - Início tem que ser menor que a data de Aquisição de Materiais - Fim</span>");
			retorno = false;
		}
	}
	else if(dtOCI != "" && dtOCF != ""){
		if((gerarData(dtOCF) < gerarData(dtOCI))) {
			//alert("Data de Obras Civis - Início tem que ser menor que a data de Obras Civis - Fim!");
			$($("input[title='Obras Civis - Fim']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado6' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Data de Obras Civis - Início tem que ser menor que a data de Obras Civis - Fim</span>");
			retorno = false;
		}
	}
	else if(dtMEI != "" && dtMEF != ""){
		if((gerarData(dtMEF) < gerarData(dtMEI))) {
			//alert("Data de Montagem Eletromecânica - Início tem que ser menor que a data de Montagem Eletromecânica - Fim!");
			$($("input[title='Montagem Eletromecânica - Fim']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado8' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Data de Montagem Eletromecânica - Início tem que ser menor que a data de Montagem Eletromecânica - Fim</span>");
			retorno = false;
		}
	}
	else if(dtCI != "" && dtCF != ""){
		if((gerarData(dtCF) < gerarData(dtCI))) {
			//alert("Data de Comissionamento - Início tem que ser menor que a data de Comissionamento - Fim!");
			$($("input[title='Comissionamento - Fim']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado10' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Data de Comissionamento - Início tem que ser menor que a data de Comissionamento - Fim</span>");
			retorno = false;
		}
	}
}
	return retorno;
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

function gerarData(str) {
    var partes = str.split("/");
    return new Date(partes[2], partes[1] - 1, partes[0]);
}

function isDate(txtDate)
{
  var currVal = txtDate;
  if(currVal == '')
    return false;

  //Declare Regex 
  var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
  var dtArray = currVal.match(rxDatePattern); // is format OK?

  if (dtArray == null)
     return false;

  //Checks for mm/dd/yyyy format.
  dtMonth = dtArray[3];
  dtDay= dtArray[1];
  dtYear = dtArray[5];
  
  if (dtMonth < 1 || dtMonth > 12)
      return false;
  else if (dtDay < 1 || dtDay> 31)
      return false;

  else if ((dtMonth==4 || dtMonth==6 || dtMonth==9 || dtMonth==11) && dtDay ==31)
      return false;

  else if (dtMonth == 2)
  {
     var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
     if (dtDay> 29 || (dtDay ==29 && !isleap))
          return false;
  }
  return true;
}
