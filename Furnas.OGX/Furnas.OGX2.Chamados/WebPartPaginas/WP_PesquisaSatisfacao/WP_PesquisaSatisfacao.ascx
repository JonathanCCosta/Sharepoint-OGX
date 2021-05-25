<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WP_PesquisaSatisfacao.ascx.cs" Inherits="Furnas.OGX2.Chamados.WebPartPaginas.WP_PesquisaSatisfacao.WP_PesquisaSatisfacao" %>


<script src="../SiteAssets/JSChamados/Avaliacao.js"></script>
<style type="text/css">
    #ContainerDuplicidade {
        /*display:none;*/
        width:100%;
        text-align:center;
    }

    #containerDone {
        /*display:none;*/
        width:100%;
        text-align:center;
    }
.container{
	
	width: 100%;
	text-align: center;
}
	
.tipoPonteiro{
	
	cursor:pointer !important;
}

.botaoEnviar{
	
	background-color: #0072c6 !important;
    color: white !important;
    width: 10% !important;
    min-height: 40px !important;
    cursor: pointer !important;
    font-weight: bold !important;
    font-size: 13px !important;
    margin-top:5px !important;
    
    }

.controles{
	    width: 100%;
}

.tiulo-solicitacao{
	
	
	font-size: 2.8em;
    /*margin-bottom: 40px;*/
}

.texto-titulo-solicitacao{
	
	font-weight:bold;
	font-size: 1.6em;
	
}

.ms-webpartPage-root {
    border-spacing: 10px!important;
}

</style>

<div class="container" id="Cta" runat="server" ClientIDMode="Static">
    <div>
    <div class="tiulo-solicitacao">
        Pesquisa de Satisfação

        <div><label id="codigo" style="font-size:14px;"></label></div>
    </div>

    <a href="javascript:void(0)" onclick="Avaliar(1)">
            <img style="cursor:pointer" alt="Pessimo" class="tipoPonteiro" src="../SiteAssets/star0.png" id="s1" /></a>

        <a href="javascript:void(0)" onclick="Avaliar(2)">
            <img style="cursor:pointer" alt="Ruim" src="../SiteAssets/star0.png" class="tipoPonteiro" id="s2" /></a>

        <a href="javascript:void(0)" onclick="Avaliar(3)">
            <img style="cursor:pointer" alt="Bom" src="../SiteAssets/star0.png" class="tipoPonteiro" id="s3" /></a>

        <a href="javascript:void(0)" onclick="Avaliar(4)">
            <img style="cursor:pointer" alt="Muito Bom" src="../SiteAssets/star0.png" class="tipoPonteiro" id="s4" /></a>

        <a href="javascript:void(0)" onclick="Avaliar(5)">
            <img style="cursor:pointer" alt="Otimo" src="../SiteAssets/star0.png" class="tipoPonteiro" id="s5" /></a>
        <p><asp:Label ID="tipoAvaliacao" runat="server" ClientIDMode="Static" Text=" -- "></asp:Label></p>

    </div>
    <div class="controles-principal">
        <div class="controles">
            <asp:TextBox ID="comentario" runat="server" placeholder="Deixe seu comentário..." TextMode="MultiLine" Rows="10" Columns="100"></asp:TextBox>
            <div>
                <asp:Button CssClass="botaoEnviar" Text="Enviar" runat="server" ID="Enviar" OnClick="Enviar_Click" ClientIDMode="Static" />
            </div>
        </div>
    </div>
    <asp:TextBox runat="server" ID="IDChamado" ClientIDMode="Static"></asp:TextBox>
    <asp:TextBox runat="server" ID="NumeroChamado" ClientIDMode="Static"></asp:TextBox>
    <asp:TextBox runat="server" ID="TextoClassicacao" ClientIDMode="Static"></asp:TextBox>
</div>

<div id="containerDone" runat="server" ClientIDMode="Static" class="tiulo-solicitacao">
    <p style="font-size:30px;" class="tiulo-solicitacao">Pesquisa de Satisfação enviada com sucesso!</p>
</div>

<div id="ContainerDuplicidade" runat="server" ClientIDMode="Static" class="tiulo-solicitacao">
    <p style="font-size:20px;">Pesquisa de Satisfação para este Chamado já foi enviada!</p>
</div>


