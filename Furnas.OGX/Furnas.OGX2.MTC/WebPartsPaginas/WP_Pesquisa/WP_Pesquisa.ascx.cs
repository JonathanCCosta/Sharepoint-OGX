using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Furnas.OGX2.MTC.ServicesMTC;
using Microsoft.SharePoint.Taxonomy;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Furnas.OGX2.MTC.WebPartsPaginas.WP_Pesquisa
{
    [ToolboxItemAttribute(false)]
    public partial class WP_Pesquisa : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public WP_Pesquisa()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregaFiltros();

                string perfil = Filtros.Responsavel(SPContext.Current.Web);

                if (perfil == "Publico")
                {
                    dprControle.Items.Remove("Cancelado");
                    dprControle.Items.Remove("Aguaradando Aprovação");
                }
                else if (perfil != "Admin")
                {
                    dprControle.Items.Remove("Cancelado");
                }
            }
            /*gridTeste.DataSource = Manuais(SPContext.Current.Web);
            gridTeste.DataBind();
            gridTeste.HeaderRow.TableSection = TableRowSection.TableHeader;*/
        }

        TermCollection all = null;

        public void CarregaFiltros()
        {
            SPWeb web = SPContext.Current.Web;
            Filtros.LoadDropDown(drpTipo, web, "Tipo MTC", "DescricaoTipo");
            Filtros.LoadDropDown(drpGrupo, web, "Grupo MTC", "DescricaoGrupo");
            Filtros.LoadDropDown(drpFabricante, web, "Fabricante MTC", "DescricaoFabricante");
            Filtros.LoadDropDown(drpOrgao, web, "Gerência Responsável MTC", "");
            all = Filtros.Termos(SPContext.Current.Site);
            if (all != null)
            {
                foreach (Term item in all)
                {
                    ListItem lt = new ListItem();
                    lt.Value = item.Name;
                    lt.Text = item.Name;

                    combobox.Items.Add(lt);
                }

                List<ListItem> itemsDDL = combobox.Items.Cast<ListItem>().OrderBy(lt => lt.Text).ToList();
                combobox.Items.Clear();
                combobox.Items.AddRange(itemsDDL.ToArray());
            }
        }

        public DataTable Manuais(SPWeb web)
        {
            SPList list = web.Lists["Domínio de Instalação"];//Manual Técnico"];
            //SPQuery query = new SPQuery();
            DataTable items = null;

            //query.ViewFields = "<FieldRef Name='ID'/><FieldRef Name='CodigoMTC' /><FieldRef Name='TipoMTC' /><FieldRef Name='Grupo' />"+
            //    "<FieldRef Name='Subgrupo' /><FieldRef Name='Fabricante1' /><FieldRef Name='Revisao' /><FieldRef Name='ControleStatus'/>";

            //query.ViewFieldsOnly = true;
            //query.RowLimit = 1000;
            //items = list.GetItems(query).GetDataTable();

            items = list.GetItems().GetDataTable();

            return items;
        }

        protected void GridView1_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((HyperLink)e.Row.FindControl("detalhes")).NavigateUrl = SPContext.Current.Web.Url + "/Lists/ManualTecnico/Forms/DispForm.aspx?ID=" + ((System.Data.DataRowView)(e.Row.DataItem)).Row["ID"].ToString();
            }
        }

        protected void drpGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Filtros.LoadDropDownLookup(drpSubgrupo, SPContext.Current.Web, "Subgrupo MTC", drpGrupo.SelectedItem.Value, "DescricaoSubgrupo");
        }

        protected void btPesquisar_Click(object sender, EventArgs e)
        {
            ErroDt.Visible = false; ErrosDT.Visible = false; ErroDtFim.Visible=false;
            ErroDt2.Visible = false; ErrosDT2.Visible = false; ErroDtFim2.Visible = false;

            if ((dtInicio.Text != "" && dtInicioFim.Text == "") || (dtInicio.Text == "" && dtInicioFim.Text != ""))
            {
                ErrosDT.Visible = true;
                ErroDt.Visible = true;
                return;
            }
            else
            {
                if (dtInicio.Text != "")
                {
                    try
                    {
                        DateTime ini = Convert.ToDateTime(dtInicio.Text);
                        if (dtInicioFim.Text != "")
                        {
                            try
                            {
                                DateTime fim = Convert.ToDateTime(dtInicioFim.Text);

                                if (ini >= fim)
                                {
                                    ErrosDT.Visible = true; ErroDtFim.Visible = true;
                                    return;
                                }

                            }
                            catch
                            {
                                ErrosDT.Visible = true;
                                ErroDt.Visible = true;
                                return;
                            }
                        }
                    }
                    catch
                    {
                        ErrosDT.Visible = true;
                        ErroDt.Visible = true;
                        return;
                    }
                }
            }

            if ((dtFim.Text != "" && dtFimFim.Text == "") || (dtFim.Text == "" && dtFimFim.Text != ""))
            {
                ErrosDT2.Visible = true;
                ErroDt2.Visible = true;
                return;
            }
            else
            {
                if (dtFim.Text != "")
                {
                    try
                    {
                        DateTime ini = Convert.ToDateTime(dtFim.Text);
                        if (dtFimFim.Text != "")
                        {
                            try
                            {
                                DateTime fim = Convert.ToDateTime(dtFimFim.Text);

                                if (ini >= fim)
                                {
                                    ErrosDT2.Visible = true; ErroDtFim2.Visible = true;
                                    return;
                                }

                            }
                            catch
                            {
                                ErrosDT2.Visible = true;
                                ErroDt2.Visible = true;
                                return;
                            }
                        }
                    }
                    catch
                    {
                        ErrosDT2.Visible = true;
                        ErroDt2.Visible = true;
                        return;
                    }
                }
            }

            if (drpTipo.SelectedItem.Value == "0" && drpGrupo.SelectedItem.Value == "0" && drpFabricante.SelectedItem.Value == "0" && drpOrgao.SelectedItem.Value == "0" &&
                dtInicio.Text == "" && dtFim.Text == "" && combobox.Value == "" && txtlivre.Text == "" && dprControle.SelectedItem.Value == "0")
            {
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Selecione ao menos um filtro!');", true);
                //return;
                ConsultaTodos();
            }

            /*if ((dtInicio.Text != "" && dtFim.Text == "") || (dtInicio.Text == "" && dtFim.Text != ""))
            {
                ErrosDT.Visible = true;
                ErroDt.Visible = true;
                return;
            }
            else{
                if (dtInicio.Text != "")
                {
                    try
                    {
                        DateTime ini = Convert.ToDateTime(dtInicio.Text);
                        if (dtFim.Text != "")
                        {
                            try
                            {
                                DateTime fim = Convert.ToDateTime(dtFim.Text);

                                if (ini >= fim)
                                {
                                    ErrosDT.Visible = true; ErroDtFim.Visible = true;
                                    return;
                                }

                            }
                            catch
                            {
                                ErrosDT.Visible = true;
                                ErroDt.Visible = true;
                                return;
                            }
                        }
                    }
                    catch
                    {
                        ErrosDT.Visible = true;
                        ErroDt.Visible = true;
                        return;
                    }
                }
            }*/

            ConsultaFiltros();
        }

        public void ConsultaFiltros()
        {
            SPList list = SPContext.Current.Web.Lists["Manual Técnico de Campo"];
            DataTable items = null;

            List<string> filtros = new List<string>(); List<string> valores = new List<string>();

            try
            {
                if (drpTipo.SelectedItem.Value != "0")
                { filtros.Add("TipoMTC"); valores.Add(drpTipo.SelectedItem.Value); }
                if (drpGrupo.SelectedItem.Value != "0")
                { filtros.Add("Grupo"); valores.Add(drpGrupo.SelectedItem.Value); }
                if (drpSubgrupo.SelectedItem.Value != "0" && drpSubgrupo.SelectedItem.Value != "-- Selecione um Subgrupo --")
                { filtros.Add("Subgrupo"); valores.Add(drpSubgrupo.SelectedItem.Value); }
                if (drpFabricante.SelectedItem.Value != "0")
                { filtros.Add("FabricanteMTC"); valores.Add(drpFabricante.SelectedItem.Value); }
                if (drpOrgao.SelectedItem.Value != "0")
                { filtros.Add("OrgaoResponsavelMTC"); valores.Add(drpOrgao.SelectedItem.Value); }
                if (dprControle.SelectedItem.Value != "0")
                { filtros.Add("ControleDeStatusMTC"); valores.Add(dprControle.SelectedItem.Value); }
                if (combobox.Items[combobox.SelectedIndex].Text != "")
                { filtros.Add("Palavra_x0020_Chave"); valores.Add(combobox.Items[combobox.SelectedIndex].Text); }
                if (dtInicio.Text != "")
                { 
                    filtros.Add("DataInicioVigencia"); valores.Add(Convert.ToDateTime(dtInicio.Text).ToString("s"));
                    filtros.Add("DataInicioVigenciaF"); valores.Add(Convert.ToDateTime(dtInicioFim.Text).ToString("s"));
                }
                if (txtlivre.Text != "")
                { filtros.Add("Objetivo"); valores.Add(txtlivre.Text); } //filtros.Add("DescricaoMTC"); valores.Add(txtlivre.Text); }

                SPQuery query = new SPQuery();
                query = Filtros.MontarQueryFiltros(filtros, valores);
                items = list.GetItems(query).GetDataTable();
            }
            catch (Exception exs)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('" + exs.Message.Replace("'", "") + "');", true);
            }

            DataTable dtNew = new DataTable();

            //SPList orgaos = SPContext.Current.Web.Lists["Gerência Responsável MTC"];
            SPList documentos = SPContext.Current.Web.Lists["Documentos Manual Técnico de Campo"];

            string perfil = Filtros.Responsavel(SPContext.Current.Web);

            if (items != null)
            {
                if (perfil == "Admin")
                    dtNew = MontaDataTableAll(items, documentos);
                else if (perfil == "Publico")
                    dtNew = MontaDataTablePublic(items, documentos);
                else
                    dtNew = MontaDataTableResp(items, documentos, perfil);
            }

            try
            {
                if (dtNew.Rows.Count > 0)
                {
                    gridTeste.DataSource = dtNew;
                    gridTeste.DataBind();
                    gridTeste.HeaderRow.TableSection = TableRowSection.TableHeader;
                    gridTeste.Visible = true;
                    lblresultadoVazio.Visible = false;
                }
                else
                {
                    gridTeste.Visible = false;
                    lblresultadoVazio.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('" + ex.Message.Replace("'", "") + "');", true);
            }
        }

        public DataTable MontaDataTableAll(DataTable items, SPList documentos)
        {
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("Vencido");
            dtNew.Columns.Add("Cód. MTC");
            dtNew.Columns.Add("Descrição");
            dtNew.Columns.Add("Órgão Resp.");
            dtNew.Columns.Add("Início Vigência");
            dtNew.Columns.Add("Fim Vigência");
            dtNew.Columns.Add("Status");
            dtNew.Columns.Add("Link para o Documento");
            foreach (DataRow row in items.Rows)
            {
                //só verficada no codigo data Fim de Vigencia
                DateTime dtInic = new DateTime(); bool temDtInicio = true; bool temDtFim= true;
                if (row["DataInicioVigencia"].ToString() != "")
                    dtInic = Convert.ToDateTime(row["DataInicioVigencia"].ToString());
                else
                    temDtInicio = false;

                DateTime dtfim = new DateTime();
                if (row["PrazoVigencia"].ToString() != "")
                {
                    int prazo = Convert.ToInt32(row["PrazoVigencia"].ToString());
                    if(temDtInicio)
                        dtfim = dtInic.AddYears(prazo).Date;
                    else
                        temDtFim = false;
                }
                else
                    temDtFim = false;

                bool temData = false;
                if (dtFim.Text != "")
                {
                    if (temDtFim)
                    {

                        if (dtfim.Date >= Convert.ToDateTime(dtFim.Text).Date && dtfim.Date <= Convert.ToDateTime(dtFimFim.Text).Date)
                            temData = false;
                        else
                            temData = true;
                    }
                    else
                        temData = true;
                }

                if (!temData)
                {
                    DataRow rowNew = dtNew.NewRow();

                    if (temDtFim)
                    {
                        if (dtfim < DateTime.Today.Date)
                            rowNew[0] = "../SiteAssets/JSManual/cancelado.gif";
                    }
                    else
                        rowNew[0] = "-";

                    if (row["CodigoMTC"].ToString() != "")
                        rowNew[1] = row["CodigoMTC"].ToString() + "|" + row["ID"].ToString();
                    else
                        rowNew[1] = "Sem Código|" + row["ID"].ToString();

                    rowNew[2] = row["DescricaoMTC"].ToString();

                    if (row["OrgaoResponsavelMTC"].ToString() != "")
                    {
                        rowNew[3] = row["OrgaoResponsavelMTC"].ToString().Split('-')[1];
                    }

                    if (temDtInicio)
                        rowNew[4] = dtInic.Date.ToString("dd/MM/yyyy");
                    else
                        rowNew[4] = "-";

                    if(temDtFim)
                        rowNew[5] = dtfim.Date.ToString("dd/MM/yyyy");
                    else
                        rowNew[5] = "-";

                    rowNew[6] = row["ControleDeStatusMTC"].ToString();

                    try
                    {
                        SPQuery queryDoc = new SPQuery();
                        queryDoc.Query = "<Where><Eq><FieldRef Name='ManualTecnico' LookupId='True' />" +
                                "<Value Type='Lookup'>" + row["ID"].ToString() + "</Value></Eq></Where>";

                        SPListItemCollection itensDoc = documentos.GetItems(queryDoc);
                        StringBuilder linkdocs = new StringBuilder();
                        foreach (SPListItem itemDoc in itensDoc)
                        {
                            if (itensDoc.Count > 1)
                            {
                                if(Path.GetExtension(itemDoc["Nome"].ToString()).ToLower() == ".pdf")
                                    linkdocs.AppendLine(itemDoc.ID.ToString() + "#" +SPContext.Current.Web.Url + "/Lists/DocumentosManualTecnico/" + itemDoc["Nome"].ToString() + "|");
                                else
                                    linkdocs.AppendLine(SPContext.Current.Web.Url + "/Lists/DocumentosManualTecnico/" + itemDoc["Nome"].ToString() + "|");
                            }
                            else
                            {
                                if (Path.GetExtension(itemDoc["Nome"].ToString()).ToLower() == ".pdf")
                                {
                                    linkdocs.AppendLine(itemDoc.ID.ToString() + "#" + SPContext.Current.Web.Url + "/Lists/DocumentosManualTecnico/" + itemDoc["Nome"].ToString());
                                }
                                else
                                    linkdocs.AppendLine(SPContext.Current.Web.Url + "/Lists/DocumentosManualTecnico/" + itemDoc["Nome"].ToString());
                            }
                        }

                        rowNew[7] = linkdocs;
                    }
                    catch(Exception ed){
                        WriteLog(SPContext.Current.Web, "Links Documentos", ed.Message);
                    }

                    dtNew.Rows.Add(rowNew);
                }
            }

            return dtNew;
        }

        public static void WriteLog(SPWeb web, string titulo, string erro)
        {
            try
            {
                SPList list = web.Lists["Logs"];
                if (list != null)
                {
                    SPListItem item = list.AddItem();
                    item[SPBuiltInFieldId.Title] = titulo;
                    item["Descricao"] = erro;
                    item.Update();
                }
            }
            catch { }
        }

        public DataTable MontaDataTableResp(DataTable items, SPList documentos, string orgao)
        {
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("Vencido");
            dtNew.Columns.Add("Cód. MTC");
            dtNew.Columns.Add("Descrição");
            dtNew.Columns.Add("Órgão Resp.");
            dtNew.Columns.Add("Início Vigência");
            dtNew.Columns.Add("Fim Vigência");
            dtNew.Columns.Add("Status");
            dtNew.Columns.Add("Link para o Documento");
            foreach (DataRow row in items.Rows)
            {
                string orgaoLinha = string.Empty; 
                if (row["OrgaoResponsavelMTC"].ToString() != "")
                    orgaoLinha = row["OrgaoResponsavelMTC"].ToString();

                bool status = true;

                if (row["ControleDeStatusMTC"].ToString() == "Aguardando Aprovação")
                {
                    if ((row["EmRevisao"].ToString() == "Não" || row["EmRevisao"].ToString() == "") && orgao != orgaoLinha)
                    {
                        status = false;
                    }
                }
                else if (row["ControleDeStatusMTC"].ToString() == "Cancelado")
                    status = false;
                
                if(status)
                {
                    DateTime dtInic = new DateTime();bool temDtInicio = true; bool temDtFim = true;
                    if (row["DataInicioVigencia"].ToString() != "")
                        dtInic = Convert.ToDateTime(row["DataInicioVigencia"].ToString());
                    else
                        temDtInicio = false;

                    DateTime dtfim = new DateTime();
                    if (row["PrazoVigencia"].ToString() != "")
                    {
                        int prazo = Convert.ToInt32(row["PrazoVigencia"].ToString());
                        if (temDtInicio)
                            dtfim = dtInic.AddYears(prazo).Date;
                        else
                            temDtFim = false;
                    }
                    else
                        temDtFim = false;

                    bool temData = false;
                    if (dtInicio.Text != "")
                    {
                        if (temDtFim)
                        {
                            if (dtfim.Date <= Convert.ToDateTime(dtFim.Text).Date)
                                temData = true;
                        }
                    }

                    if (!temData)
                    {

                        DataRow rowNew = dtNew.NewRow();

                        if (temDtFim)
                        {
                            if (dtfim < DateTime.Today.Date)
                                rowNew[0] = "../SiteAssets/JSManual/cancelado.gif";
                        }
                        else
                            rowNew[0] = "-";

                        if (row["CodigoMTC"].ToString() != "")
                            rowNew[1] = row["CodigoMTC"].ToString() + "|" + row["ID"].ToString();
                        else
                            rowNew[1] = "Sem Código|" + row["ID"].ToString();

                        rowNew[2] = row["DescricaoMTC"].ToString();

                        if (row["OrgaoResponsavelMTC"].ToString() != "")
                        {
                            rowNew[3] = row["OrgaoResponsavelMTC"].ToString().Split('-')[1];
                        }

                        if (temDtInicio)
                            rowNew[4] = dtInic.Date.ToString("dd/MM/yyyy");
                        else
                            rowNew[4] = "-";

                        if (temDtFim)
                            rowNew[5] = dtfim.Date.ToString("dd/MM/yyyy");
                        else
                            rowNew[5] = "-";

                        rowNew[6] = row["ControleDeStatusMTC"].ToString();

                        try
                        {
                            SPQuery queryDoc = new SPQuery();
                            queryDoc.Query = "<Where><Eq><FieldRef Name='ManualTecnico' LookupId='True' />" +
                                    "<Value Type='Lookup'>" + row["ID"].ToString() + "</Value></Eq></Where>";

                            SPListItemCollection itensDoc = documentos.GetItems(queryDoc);
                            StringBuilder linkdocs = new StringBuilder();

                            bool docGerencia = true;
                            if ((row["ControleDeStatusMTC"].ToString() == "Em revisão" || row["ControleDeStatusMTC"].ToString() == "Para comentários" || row["ControleDeStatusMTC"].ToString() == "Em consolidação") && orgao != orgaoLinha)
                                docGerencia = false;
                            else if (orgao != orgaoLinha)
                                docGerencia = false;

                            foreach (SPListItem itemDoc in itensDoc)
                            {
                                if (itensDoc.Count > 1 && docGerencia)
                                {
                                    if ((Path.GetExtension(itemDoc["Nome"].ToString()).ToLower() == ".doc" || Path.GetExtension(itemDoc["Nome"].ToString()).ToLower() == ".docx") && docGerencia)
                                        linkdocs.AppendLine(SPContext.Current.Web.Url + "/Lists/DocumentosManualTecnico/" + itemDoc["Nome"].ToString() + "|");
                                    else
                                        linkdocs.AppendLine(itemDoc.ID.ToString() + "#" + SPContext.Current.Web.Url + "/Lists/DocumentosManualTecnico/" + itemDoc["Nome"].ToString() + "|");
                                }
                                else
                                {
                                    if ((Path.GetExtension(itemDoc["Nome"].ToString()).ToLower() == ".doc" || Path.GetExtension(itemDoc["Nome"].ToString()).ToLower() == ".docx") && docGerencia)
                                    {
                                        linkdocs.AppendLine(SPContext.Current.Web.Url + "/Lists/DocumentosManualTecnico/" + itemDoc["Nome"].ToString());
                                        break;
                                    }
                                    else
                                    {
                                        if (Path.GetExtension(itemDoc["Nome"].ToString()).ToLower() == ".pdf")
                                        {
                                            linkdocs.AppendLine(itemDoc.ID.ToString() + "#" + SPContext.Current.Web.Url + "/Lists/DocumentosManualTecnico/" + itemDoc["Nome"].ToString());
                                            break;
                                        }
                                    }
                                }
                            }

                            rowNew[7] = linkdocs;
                        }
                        catch { }

                        dtNew.Rows.Add(rowNew);
                    }
                }
            }
            return dtNew;
        }

        public DataTable MontaDataTablePublic(DataTable items, SPList documentos)
        {
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("Vencido");
            dtNew.Columns.Add("Cód. MTC");
            dtNew.Columns.Add("Descrição");
            dtNew.Columns.Add("Órgão Resp.");
            dtNew.Columns.Add("Início Vigência");
            dtNew.Columns.Add("Fim Vigência");
            dtNew.Columns.Add("Status");
            dtNew.Columns.Add("Link para o Documento");
            foreach (DataRow row in items.Rows)
            {
                if (row["ControleDeStatusMTC"].ToString() != "Aguardando Aprovação" || row["ControleDeStatusMTC"].ToString() == "Cancelado")
                {
                    DateTime dtInic = new DateTime(); bool temDtInicio = true; bool temDtFim = true;
                    if (row["DataInicioVigencia"].ToString() != "")
                        dtInic = Convert.ToDateTime(row["DataInicioVigencia"].ToString());
                    else
                        temDtInicio = false;

                    DateTime dtfim = new DateTime();
                    if (row["PrazoVigencia"].ToString() != "")
                    {
                        int prazo = Convert.ToInt32(row["PrazoVigencia"].ToString());
                        if (temDtInicio)
                            dtfim = dtInic.AddYears(prazo).Date;
                        else
                            temDtFim = false;
                    }
                    else
                        temDtFim = false;

                    bool temData = false;
                    if (dtInicio.Text != "")
                    {
                        if (temDtFim)
                        {
                            if (dtfim.Date <= Convert.ToDateTime(dtFim.Text).Date)
                                temData = true;
                        }
                    }

                    if (!temData)
                    {

                        DataRow rowNew = dtNew.NewRow();
                        if (temDtFim)
                        {
                            if (dtfim < DateTime.Today.Date)
                                rowNew[0] = "../SiteAssets/JSManual/cancelado.gif";
                        }
                        else
                            rowNew[0] = "-";

                        if (row["CodigoMTC"].ToString() != "")
                            rowNew[1] = row["CodigoMTC"].ToString() + "|" + row["ID"].ToString();
                        else
                            rowNew[1] = "Sem Código|" + row["ID"].ToString();

                        rowNew[2] = row["DescricaoMTC"].ToString();

                        if (row["OrgaoResponsavelMTC"].ToString() != "")
                        {
                            rowNew[3] = row["OrgaoResponsavelMTC"].ToString().Split('-')[1];
                        }

                        if (temDtInicio)
                            rowNew[4] = dtInic.Date.ToString("dd/MM/yyyy");
                        else
                            rowNew[4] = "-";

                        if (temDtFim)
                            rowNew[5] = dtfim.Date.ToString("dd/MM/yyyy");
                        else
                            rowNew[5] = "-";

                        rowNew[6] = row["ControleDeStatusMTC"].ToString();

                        try
                        {
                            SPQuery queryDoc = new SPQuery();
                            queryDoc.Query = "<Where><Eq><FieldRef Name='ManualTecnico' LookupId='True' />" +
                                    "<Value Type='Lookup'>" + row["ID"].ToString() + "</Value></Eq></Where>";

                            SPListItemCollection itensDoc = documentos.GetItems(queryDoc);
                            StringBuilder linkdocs = new StringBuilder();
                            foreach (SPListItem itemDoc in itensDoc)
                            {
                                if (Path.GetExtension(itemDoc["Nome"].ToString()).ToLower() == ".pdf")
                                    linkdocs.AppendLine(itemDoc.ID.ToString() + "#" + SPContext.Current.Web.Url + "/Lists/DocumentosManualTecnico/" + itemDoc["Nome"].ToString());
                            }

                            rowNew[7] = linkdocs;
                        }
                        catch { }

                        dtNew.Rows.Add(rowNew);
                    }
                }
            }

            return dtNew;
        }

        protected void btLimpar_Click(object sender, EventArgs e)
        {
            drpTipo.SelectedIndex = 0;
            drpGrupo.SelectedIndex = 0;
            drpSubgrupo.Items.Clear();
            drpSubgrupo.Items.Insert(0, new ListItem("-- Selecione um Subgrupo --"));
            drpFabricante.SelectedIndex = 0;
            drpOrgao.SelectedIndex = 0;
            dtInicio.Text = "";
            dtInicioFim.Text = "";
            dtFim.Text = "";
            dtFimFim.Text = "";
            dprControle.SelectedIndex = 0;
            combobox.SelectedIndex = 0;
            txtlivre.Text = "";

            //gridTeste.DataBind();
            gridTeste.Visible = false;
            lblresultadoVazio.Visible = false;
            //ConsultaTodos();


        }

        public void ConsultaTodos()
        {
            SPList list = SPContext.Current.Web.Lists["Manual Técnico de Campo"];
            SPQuery query = new SPQuery();
            query.Query = " <Where><IsNotNull><FieldRef Name='ID' /></IsNotNull></Where>";
            DataTable items = null;

            items = list.GetItems(query).GetDataTable();

            DataTable dtNew = new DataTable();
            SPList documentos = SPContext.Current.Web.Lists["Documentos Manual Técnico de Campo"];

            string perfil = Filtros.Responsavel(SPContext.Current.Web);

            if (items != null)
            {
                if (perfil == "Admin")
                    dtNew = MontaDataTableAll(items, documentos);
                else if (perfil == "Publico")
                    dtNew = MontaDataTablePublic(items, documentos);
                else
                    dtNew = MontaDataTableResp(items, documentos, perfil);
            }

            try
            {
                if (dtNew.Rows.Count > 0)
                {
                    gridTeste.DataSource = dtNew;
                    gridTeste.DataBind();
                    gridTeste.HeaderRow.TableSection = TableRowSection.TableHeader;
                    gridTeste.Visible = true;
                    lblresultadoVazio.Visible = false;
                }
                else
                {
                    gridTeste.Visible = false;
                    lblresultadoVazio.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('" + ex.Message.Replace("'", "") + "');", true);
            }

            //query.ViewFields = "<FieldRef Name='ID'/><FieldRef Name='CodigoMTC' /><FieldRef Name='DescricaoMTC' /><FieldRef Name='OrgaoResponsavelMTC' />" +
            //    "<FieldRef Name='DataInicioVigencia' /><FieldRef Name='ControleDeStatusMTC' />";
            // query.ViewFieldsOnly = true;
            //query.RowLimit = 10000;
            //items = list.GetItems(query).GetDataTable();

            //gridTeste.DataSource = items;
            //gridTeste.DataBind();
            //gridTeste.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
}
