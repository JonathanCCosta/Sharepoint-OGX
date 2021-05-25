<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WP_Pesquisa.ascx.cs" Inherits="Furnas.OGX2.MTC.WebPartsPaginas.WP_Pesquisa.WP_Pesquisa" %>

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
    width:200px;
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
.resultVazio {
    font-size:17px; 
    color:#262626;
    font-family: "Segoe UI Light","Segoe UI","Segoe",Tahoma,Helvetica,Arial,sans-serif;
    font-weight:bold;
}
.tbCentro {
    text-align: center;
}
</style>



<script src="../SiteAssets/JSManual/jquery-1.12.4.js"></script>
<script src="../SiteAssets/JSManual/jquery-ui.js"></script>
<script type="text/javascript" src="../SiteAssets/JSManual/CC.js"></script>

<link rel="stylesheet" type="text/css" href="../SiteAssets/JSManual/jquery-ui.theme.min.css" />
<link rel="stylesheet" type="text/css" href="../SiteAssets/JSManual/jquery-ui.theme.css" />
<link rel="stylesheet" type="text/css" href="../SiteAssets/JSManual/jquery-ui.structure.min.css" />
<link rel="stylesheet" type="text/css" href="../SiteAssets/JSManual/jquery-ui.structure.css" />
<link rel="stylesheet" type="text/css" href="../SiteAssets/JSManual/jquery-ui.min.css" />
<link rel="stylesheet" type="text/css" href="../SiteAssets/JSManual/jquery-ui.css" />


<%--<link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/v/dt/dt-1.10.22/datatables.min.css"/>--%>
<link rel="stylesheet" type="text/css" href="../SiteAssets/JSManual/jquery.dataTables.min.css" />
<script type="text/javascript" src="https://cdn.datatables.net/v/dt/dt-1.10.22/datatables.min.js"></script>
<%--<link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/v/bs4-4.1.1/jq-3.3.1/dt-1.10.18/sl-1.2.6/datatables.min.css"/>--%>



<script>
    $(function () {
        $('#<%=gridTeste.ClientID %>').DataTable({
            "language": {
                "url": "//cdn.datatables.net/plug-ins/1.10.22/i18n/Portuguese-Brasil.json"
            },
            "searching": false,
            "lengthChange": false,
            "pageLength": 50,
            columnDefs: [
                {
                    targets: 0,
                    "className": "tbCentro",
                    render: function (data) {
                        if(data !="-" && data != "&nbsp;")
                            return '<img src="' + data + '">'
                        else
                            return ""
                    }
                },
                {
                    targets: 1,
                    render: function (data) {
                        var links = [];
                        if (data != "") {
                            links = data.split('|');
                            return '<a href="' + _spPageContextInfo.webAbsoluteUrl + '/Lists/ManualTecnico/DispForm.aspx?ID=' + links[1] + '" >' + links[0] + '</a><br/>'
                        }
                    }
                },
                {
                    targets: 3,
                    "className": "tbCentro"
                },
                {
                    targets: 7,
                    "className": "tbCentro",
                    render: function (data) {
                        var links = [];
                        if (data != "") 
                            links = data.split('|');
                        
                        var link="";
                        $.each(links, function (index, value) {
                            if (value != "") {
                                if (value.includes('pdf')) {
                                    var im = value.split('#');
                                    link += '<a href="' + _spPageContextInfo.webAbsoluteUrl + '/SitePages/DocumentosMTC.aspx?Doc=' + im[0] + '&Link=' + im[1] + '" target="_blank">PDF</a><br/>'
                                }
                                else
                                    link += '<a href="' + value + '" >DOC</a><br/>'
                            }
                        });
                        
                        return link
                    }
                }
            ]
        });
    });

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
    });
    </script>

<div><h2 class="tituloPesquisa">Pesquisar Manual Técnico de Campo</h2></div>
<div class="pesq">
    <div class="fields">
        <span class="labels">Tipo </span>

        <asp:DropDownList ID="drpTipo" runat="server" >
            <asp:ListItem Value="0">-- Selecione um Tipo --</asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="fields">
        <span class="labels">Grupo </span>
        <asp:DropDownList ID="drpGrupo" runat="server" OnSelectedIndexChanged="drpGrupo_SelectedIndexChanged" AutoPostBack="true">
            <asp:ListItem Value="0">-- Selecione um Grupo --</asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="fields">
        <span class="labels">Subgrupo </span>
        <asp:DropDownList ID="drpSubgrupo" runat="server" >
            <asp:ListItem Value="0">-- Selecione um Subgrupo --</asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="fields">
        <span class="labels">Fabricante </span>
        <asp:DropDownList ID="drpFabricante" runat="server" >
            <asp:ListItem Value="0">-- Selecione um Fabricante --</asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="fields">
        <span class="labels">Órgão Resposável </span>
        <asp:DropDownList ID="drpOrgao" runat="server" >
            <asp:ListItem Value="0">-- Selecione um Órgão Resposável --</asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="fields">
        <span class="labels">Inicio de Vigência </span>
        De <asp:TextBox runat="server" ID="dtInicio" ClientIDMode="Static" TextMode="Date" MaxLength="10"></asp:TextBox>
        &nbsp;até&nbsp;
        <asp:TextBox runat="server" ID="dtInicioFim" ClientIDMode="Static" TextMode="Date" MaxLength="10"></asp:TextBox>
    </div>
    <div class="fields" runat="server" id="ErrosDT" visible="false">
        <span class="labels">&nbsp;&nbsp;</span>
        <asp:Label ID="ErroDt" ClientIDMode="Static" Visible="false" runat="server" ForeColor="Red" Text="As datas de - até de Inicio de Vigência devem ser preenchidas."></asp:Label>
        <asp:Label ID="ErroDtFim" ClientIDMode="Static" Visible="false" runat="server" ForeColor="Red" Text="A data final de Inicio de Vigência deve ser maior que a data inicial."></asp:Label>
    </div>
    <div class="fields">
        <span class="labels">Fim de Vigência </span>
        De <asp:TextBox runat="server" ID="dtFim" ClientIDMode="Static" TextMode="Date" MaxLength="10"></asp:TextBox>
        &nbsp;até&nbsp;
        <asp:TextBox runat="server" ID="dtFimFim" ClientIDMode="Static" TextMode="Date" MaxLength="10"></asp:TextBox>
    </div>
    <div class="fields" runat="server" id="ErrosDT2" visible="false">
        <span class="labels">&nbsp;&nbsp;</span>
        <asp:Label ID="ErroDt2" ClientIDMode="Static" Visible="false" runat="server" ForeColor="Red" Text="As datas De - até de Fim de Vigência devem ser preenchidas."></asp:Label>
        <asp:Label ID="ErroDtFim2" ClientIDMode="Static" Visible="false" runat="server" ForeColor="Red" Text="A data final de Fim de Vigência deve ser maior que a data inicial."></asp:Label>
    </div>
    <div class="fields">
        <span class="labels">Controle de Status </span>
        <asp:DropDownList ID="dprControle" runat="server" >
            <asp:ListItem Value="0">-- Selecione um Status --</asp:ListItem>
            <asp:ListItem Value="Aguardando Aprovação">Aguardando Aprovação</asp:ListItem>
            <asp:ListItem Value="Vigente">Vigente</asp:ListItem>
            <asp:ListItem Value="Para comentários">Para comentários</asp:ListItem>
            <asp:ListItem Value="Em consolidação">Em consolidação</asp:ListItem>
            <asp:ListItem Value="Histórico">Histórico</asp:ListItem>
            <asp:ListItem Value="Cancelado">Cancelado</asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="ui-widget fields">
        <span class="labels">Palavra Chave </span>
        <select runat="server" ID="combobox" ClientIDMode="Static">
            <option value=""></option>
        </select>
    </div>
    <div class="fields">
        <span class="labels">Digite sua busca </span>
        <asp:TextBox ID="txtlivre" runat="server" Width="200"></asp:TextBox>
    </div>
    <div style="margin-top:5px;padding-left:200px">
        <asp:Button ID="btPesquisar" runat="server" ClientIDMode="Static" Text="Buscar" Width="150" CssClass="btDireita" OnClick="btPesquisar_Click" />
        &nbsp;&nbsp;
        <asp:Button ID="btLimpar" runat="server" ClientIDMode="Static" Text="Limpar Pesquisa" Width="150" CssClass="btEsquerda" OnClick="btLimpar_Click" />
    </div>
</div>
<div style="margin-top:10px">
    <asp:GridView ID="gridTeste" CssClass="cell-border stripe hover" runat="server" Visible="false"></asp:GridView>
    <asp:Label ID="lblresultadoVazio" runat="server" Text="Nenhum MTC encontrado." Visible="false" CssClass="resultVazio"></asp:Label>
</div>
<asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" OnRowDataBound="GridView1_RowDataBound" AutoGenerateColumns="False">
    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    <Columns>
        <asp:TemplateField HeaderText="Ver detalhes" HeaderStyle-Width="120" ItemStyle-Width="120">
            <ItemTemplate>
                <asp:HyperLink ID="detalhes" runat="server">Ver detalhes</asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:BoundField HeaderText="ID" DataField="ID" />
        <asp:BoundField HeaderText="Código MTC" DataField="CodigoMTC" ItemStyle-Width="150" />
        <asp:BoundField HeaderText="Tipo MTC" DataField="TipoMTC" ItemStyle-Width="80" />
        <asp:BoundField HeaderText="Grupo" DataField="Grupo" ItemStyle-Width="120" />
        <asp:BoundField HeaderText="Subgrupo" DataField="Subgrupo" ItemStyle-Width="120" />
        <asp:BoundField HeaderText="Fabricante" DataField="Fabricante1" ItemStyle-Width="80" />
        <asp:BoundField HeaderText="Revisão" DataField="Revisao" ItemStyle-Width="60" />
        <asp:BoundField HeaderText="Controle de Status" DataField="ControleStatus" ItemStyle-Width="120" />
        
    </Columns>
    <EditRowStyle BackColor="#999999" />
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <HeaderStyle font-size="1em" />
    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
    <SortedAscendingCellStyle BackColor="#E9E7E2" />
    <SortedAscendingHeaderStyle BackColor="#506C8C" />
    <SortedDescendingCellStyle BackColor="#FFFDF8" />
    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
</asp:GridView>
<%--row-border stripe hover--%>

<div class="loading">
    <br />Carregando, por favor aguarde...<br /><br />
    <img src="../SiteAssets/CSS/ajax-loader.gif" alt="" />
</div>