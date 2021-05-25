<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WP_ExclusaoMudancaJustificativa.ascx.cs" Inherits="Furnas.OGX2.Webparts.WP_ExclusaoMudancaJustificativa.WP_ExclusaoMudancaJustificativa" %>

<style>
    .btMod {width:70px;}
    .ms-webpartPage-root {border-spacing:10px!important;}
    #DeltaPlaceHolderMain {width: auto!important;}
    .ms-cui-ribbonTopBars {display:none!important;}
    #pageTitle{display:none!important;}

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

<script type="text/javascript" src="../SiteAssets/JS/justificativaExclusaoMudanca.js"></script>
<div style="width:400px">
    <div>
        <span style="font-weight:bold;font-size:16px;">Justificativa da Exclusão:</span>
        <br /><br />
        <textarea runat="server" id="justificativa" style="width:400px;height:200px;"></textarea>
     </div>
    <div style="margin-top:20px;text-align:center;">
        <asp:Button ID="btSalvar" runat="server" Text="Salvar" ClientIDMode="Static" CssClass="btMod" OnClick="btSalvar_Click" />&nbsp;&nbsp;
        <asp:Button ID="btCancel" runat="server" Text="Cancelar" CssClass="btMod" OnClientClick="CloseFormCancel()" />
    </div>
</div>
<asp:TextBox ID="NumSolicitacao" runat="server" ClientIDMode="Static"></asp:TextBox>
<div class="loading">
    <br />Carregando, por favor aguarde...<br /><br />
    <img src="../SiteAssets/CSS/ajax-loader.gif" alt="" />
</div>