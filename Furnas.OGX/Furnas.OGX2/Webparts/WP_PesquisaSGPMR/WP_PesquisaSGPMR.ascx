<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WP_PesquisaSGPMR.ascx.cs" Inherits="Furnas.OGX2.Webparts.WP_PesquisaSGPMR.WP_PesquisaSGPMR" %>


<style>
    .modal{
        position: fixed;
        top: 0;
        left: 0;
        background-color: black;
        z-index: 99;
        opacity: 0.3;
        filter: alpha(opacity=30);
        -moz-opacity: 0.3;
        min-height: 100%;
        width: 100%;
    }

    .loading{
        text-align: center;
        font-family: Arial;
        font-size: 10pt;
        border: 2px solid #67CFF5;
        width: 200px;
        height: 110px;
        display: none;
        position: fixed;
        background-color: White;
        z-index: 999;
        font-weight:bold;
    }
    .ms-webpartPage-root {
        border-spacing: 0px!important;
        margin-top: 20px!important;
    }
    .custom-combobox {
    position: relative;
    display: inline-block;
  }
  .custom-combobox-toggle {
    position: absolute;
    top: 0;
    bottom: 0;
    margin-left: -1px;
    padding: 0;
  }
  .custom-combobox-input {
    margin: 0;
    padding: 5px 10px;
  }
  span#DeltaPlaceHolderPageTitleInTitleArea {
    display: none;
}
.labels {
    width:230px;
    float:left;
}
.pesq {
    background-color: #f1f1f1;
    padding:10px;
    font-family: "Segoe UI Light","Segoe UI","Segoe",Tahoma,Helvetica,Arial,sans-serif;
    color: #262626;
    font-size:14px;
    border-radius: 10px;
    border: 1px solid #ababab;
}
.tituloPesquisa {
    font-family: "Segoe UI Light","Segoe UI","Segoe",Tahoma,Helvetica,Arial,sans-serif;
    font-size: 25px;
    margin-bottom: 10px;
    font-weight:bold;
}
.fields {
    padding-bottom:8px;
}
.btDireita {
    margin-left: 0px!important;
    background: url(../SiteAssets/magnifier.png) left 40px top 8px no-repeat;
    cursor: pointer;
    border-radius: 5px;
    font-size: 13px!important;
    border: 1px solid #666!important;
    padding-left: 25px!important;
}
.btEsquerda {
    margin-left: 0px!important;
    background: url(../SiteAssets/cross.png) left 20px top 9px no-repeat;
    cursor: pointer;
    font-size: 13px!important;
    text-align: right;
    padding-right: 15px!important;
    border-radius: 5px;
    border: 1px solid #666!important;
}
.btExport {
    margin-left: 0px!important;
    background: url(../SiteAssets/excel.png) left 20px top 9px no-repeat;
    cursor: pointer;
    font-size: 13px!important;
    text-align: right;
    padding-right: 15px!important;
    border-radius: 5px;
    border: 1px solid #666!important;
}
.resultVazio {
    font-size:17px; 
    color:#262626;
    font-family: "Segoe UI Light","Segoe UI","Segoe",Tahoma,Helvetica,Arial,sans-serif;
    font-weight:bold;
}
.tbCentro {
    text-align: center;
}
select {
    min-width: 415px;
}
</style>

<link rel="stylesheet" type="text/css" href="../SiteAssets/JSManual/jquery-ui.theme.min.css" />
<link rel="stylesheet" type="text/css" href="../SiteAssets/JSManual/jquery-ui.theme.css" />
<link rel="stylesheet" type="text/css" href="../SiteAssets/JSManual/jquery-ui.structure.min.css" />
<link rel="stylesheet" type="text/css" href="../SiteAssets/JSManual/jquery-ui.structure.css" />
<link rel="stylesheet" type="text/css" href="../SiteAssets/JSManual/jquery-ui.min.css" />
<link rel="stylesheet" type="text/css" href="../SiteAssets/JSManual/jquery-ui.css" />

<%--<script type="text/javascript" src="../SiteAssets/JSManual/datatables.min.js"></script>--%>
<%--<link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/v/dt/dt-1.10.22/datatables.min.css"/>--%>
<link rel="stylesheet" type="text/css" href="../SiteAssets/JSManual/jquery.dataTables.min.css" />
<script type="text/javascript" src="https://cdn.datatables.net/v/dt/dt-1.10.22/datatables.min.js"></script>
<%--<link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/v/bs4-4.1.1/jq-3.3.1/dt-1.10.18/sl-1.2.6/datatables.min.css"/>--%>
<script type="text/javascript" src="../SiteAssets/JS/tabelaR.js"></script>
<script>


    function ShowProgress() {
        setTimeout(function () {
            var modal = $('<div />');
            modal.addClass("modal");
            $('body').append(modal);
            var loading = $(".loading");
            loading.show();
            var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
            var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
            loading.css({ top: top, left: left });
        }, 200);
    }

    $(document).ready(function () {
        $("#btPesquisar").click(function () {
            ShowProgress();
        });

        $("#btLimpar").click(function () {
            ShowProgress();
        });

        $("#BtExportar").click(function () {
            ShowProgress();
        });
    });
</script>

<div><h2 class="tituloPesquisa">Pesquisar Planos SGPMR</h2></div>
<div class="pesq">
    <div class="fields">
        <span class="labels">Tipo de Conteúdo </span>
        <select id="drpTipo" runat="server">
            <option value="0">Planos de Melhorias e Reforços</option>
            <option value="1">Plano de Reforços por Superação</option>
        </select>
    </div>
    <div class="fields">
        <span class="labels">Ciclo </span>
        <asp:DropDownList ID="drpCiclo" runat="server">
            <asp:ListItem Value="0">-- Selecione um Ciclo --</asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="fields">
        <span class="labels">Numeração </span>
        <asp:TextBox ID="txtNumeracao" runat="server" Width="200"></asp:TextBox>
    </div>
    <div class="fields">
        <span class="labels">Instalação </span>
        <select id="drpInstalacao" runat="server">
            <option value="0">-- Selecione uma Instalação --</option>
        </select>
    </div>
    <div class="fields" id="divOutraInstalacao" runat="server">
        <span class="labels">&nbsp;&nbsp;</span>
        <asp:TextBox ID="txtOutraInstalacao" runat="server" Width="200"></asp:TextBox>
        <span style="color:red;font-size: 10px;">* Digite Outra Instalação</span>
    </div>
    <div class="fields" runat="server" id="divErroInstalacao" visible="false">
        <span class="labels">&nbsp;&nbsp;</span>
         <asp:Label ID="lblErroInstalacao" ClientIDMode="Static" runat="server" ForeColor="Red" Text="Insira o nome da Instalação."></asp:Label>
    </div>
    <div class="fields" runat="server" id="divTipoEquipamento">
        <span class="labels">Tipo de Equipamento </span>
        <asp:DropDownList ID="drpTipoEquipamento" runat="server" >
            <asp:ListItem Value="0">-- Selecione um Tipo de Equipamento --</asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="fields" runat="server" id="divCodIdentificacao">
        <span class="labels">Código de Identificação </span>
        <asp:TextBox ID="txtCodIdentificacao" runat="server" Width="200"></asp:TextBox>
    </div>
    <div class="fields">
        <span class="labels">Função de Transmissão </span>
        <select id="drpFuncaoTransmissao" runat="server">
            <option value="0">-- Selecione uma Função de Transmissão --</option>
        </select>
    </div>
    <div class="fields" runat="server" id="divOutraFuncaoTransmissao">
        <span class="labels">&nbsp;&nbsp;</span>
        <asp:TextBox runat="server" ID="txtFuncaoTransmissao" ClientIDMode="Static" Width="200"></asp:TextBox>
        <span style="color:red;font-size: 10px;">* Digite Outra Função de Transmissão</span>
    </div>
    <div class="fields" runat="server" id="divOutraFuncaoTransmissaoErro" visible="false">
        <span class="labels">&nbsp;&nbsp;</span>
         <asp:Label ID="lblOutraFuncaoTransmissao" ClientIDMode="Static" runat="server" ForeColor="Red" Text="Insira o nome da Função de Transmissão."></asp:Label>
    </div>
    <div class="fields">
        <span class="labels">Status da Revitalização </span>
        <asp:DropDownList ID="drpRevitalizacao" runat="server" >
            <asp:ListItem Value="0">-- Selecione um Status da Revitalização --</asp:ListItem>
            <asp:ListItem Value="Não Iniciada">Não Iniciada</asp:ListItem>
            <asp:ListItem Value="Em Andamento">Em Andamento</asp:ListItem>
            <asp:ListItem Value="Executada">Executada</asp:ListItem>
        </asp:DropDownList>
    </div>

    <div class="fields">
        <span class="labels">Indicações </span>
        <asp:DropDownList ID="drpIndicacoes" runat="server" >
            <asp:ListItem Value="0">-- Selecione uma Indicações --</asp:ListItem>
        </asp:DropDownList>
    </div>

    <div class="fields">
        <span class="labels">Previsão da Instalação </span>
        <asp:TextBox runat="server" ID="dtInicio" ClientIDMode="Static" TextMode="Month" MaxLength="10"></asp:TextBox>
        &nbsp;até&nbsp;
        <asp:TextBox runat="server" ID="dtInicioFim" ClientIDMode="Static" TextMode="Month" MaxLength="10"></asp:TextBox>
    </div>
    <div class="fields" runat="server" id="ErrosDT" visible="false">
        <span class="labels">&nbsp;&nbsp;</span>
        <asp:Label ID="ErroDt" ClientIDMode="Static" Visible="false" runat="server" ForeColor="Red" Text="As duas datas de Previsão da Instalação devem ser preenchidas."></asp:Label>
        <asp:Label ID="ErroDtFim" ClientIDMode="Static" Visible="false" runat="server" ForeColor="Red" Text="A data final de Previsão da Instalação deve ser maior que a data inicial."></asp:Label>
    </div>

    <div class="fields">
        <span class="labels">Órgão Solicitante </span>
        <asp:DropDownList ID="drpOrgaoSolicitante" runat="server" >
            <asp:ListItem Value="0">-- Selecione um Órgão Solicitante --</asp:ListItem>
        </asp:DropDownList>
    </div>

    <div class="fields">
        <span class="labels">Órgão Gestor </span>
        <asp:DropDownList ID="drpOrgaoGestor" runat="server" >
            <asp:ListItem Value="0">-- Selecione um Órgão Gestor --</asp:ListItem>
        </asp:DropDownList>
    </div>

    <div class="fields">
        <span class="labels">Responsável no Órgão Gestor </span>
        <asp:DropDownList ID="drpOrgaoGestorResponsavel" runat="server" >
            <asp:ListItem Value="0">-- Selecione um Responsável no Órgão Gestor --</asp:ListItem>
        </asp:DropDownList>
    </div>

    <div style="margin-top:5px;padding-left:230px">
        <asp:Button ID="btPesquisar" runat="server" ClientIDMode="Static" Text="Buscar" Width="150" CssClass="btDireita" OnClick="btPesquisar_Click" />
        &nbsp;&nbsp;
        <asp:Button ID="btLimpar" runat="server" ClientIDMode="Static" Text="Limpar Pesquisa" Width="150" CssClass="btEsquerda" OnClick="btLimpar_Click" />
    </div>
</div>
<div style="margin-top:10px">
    <div style="height:40px;float:right;">
        <asp:Button ID="BtExportar" runat="server" ClientIDMode="Static" Text="Exportar para o Excel" Width="180" CssClass="btExport" Visible="false" OnClick="BtExportar_Click" />
    </div>
    <asp:GridView ID="grid" ClientIDMode="Static" CssClass="cell-border stripe hover" runat="server" Visible="false"></asp:GridView>
    <asp:Label ID="lblresultadoVazio" runat="server" Text="Nenhum Plano encontrado." Visible="false" CssClass="resultVazio"></asp:Label>
</div>

<div class="loading">
    <br />Carregando, por favor aguarde...<br /><br />
    <img src="../SiteAssets/CSS/ajax-loader.gif" alt="" />
</div>