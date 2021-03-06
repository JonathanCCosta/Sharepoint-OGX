<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WP_ImportaGestaoInterna.ascx.cs" Inherits="Furnas.OGX2.Webparts.WP_ImportaGestaoInterna.WP_ImportaGestaoInterna" %>

<link rel="stylesheet" type="text/css" href="../SiteAssets/CSS/importexcel.css" />
<script type="text/javascript" src="../SiteAssets/JS/importGestao.js"></script>
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
    #spe-report-aporte header h1 {
    color: #b5def0;
    font-size: 2.6em!important;
    font-weight: 100;
    }
</style>

<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

<script type="text/javascript">

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

</script>

<header>
    <h1 class="tituloPesquisa">IMPORTAR GESTÃO INTERNA DO SGPMR</h1>
</header>
<div id="spe-report-aporte" class="ConteudoPagina">
    <div class="propriedades">
        <div class="propriedadesTipo filter2">
            <h2 style="color:#262626;">Nome do Ciclo</h2>
            <br />
            <asp:DropDownList runat="server" ID="ddlCiclo" ClientIDMode="Static">
                <asp:ListItem Selected="True" Text="-- Selecione um Ciclo --" Value="0"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="propriedadesTipo filter2">
            <h2 style="color:#262626;">Tipo de Plano</h2>
            <br />
            <asp:DropDownList ID="ddlTipos" runat="server" ClientIDMode="Static">
                <asp:ListItem Selected="True" Text="-- Selecione o Plano --" Value="0"></asp:ListItem>
                <asp:ListItem Value="1" Text="Plano de Melhoria" />
                <asp:ListItem Value="2" Text="Plano de Reforço" />
                <asp:ListItem Value="3" Text="Plano Por Superação" />
            </asp:DropDownList>
        </div>
    </div>
    <div class="tabelaDados">
        <div class="propriedadesfileInterfaceSPE">
            <asp:FileUpload runat="server" ID="flpArquivo" ViewStateMode="Enabled" ClientIDMode="Static" />
            <asp:Button Text="Carregar para o Sistema" runat="server" CssClass="btnUpload" ID="btnUpload" ClientIDMode="Static" OnClick="btnUpload_Click" />
        </div>
    </div>
</div>
<div id="containerModelos" class="modelosN">
        <!-- Preenchido Via REST API -->
</div>
<div class="loading">
    <br />Carregando, por favor aguarde...<br /><br />
    <img src="../SiteAssets/CSS/ajax-loader.gif" alt="" />
</div>