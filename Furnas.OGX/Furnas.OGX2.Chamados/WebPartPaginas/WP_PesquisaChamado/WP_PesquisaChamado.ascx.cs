using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Furnas.OGX2.Chamados.ServiceChamados;

namespace Furnas.OGX2.Chamados.WebPartPaginas.WP_PesquisaChamado
{
    [ToolboxItemAttribute(false)]
    public partial class WP_PesquisaChamado : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public WP_PesquisaChamado()
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
                SPWeb web = SPContext.Current.Web;
                FiltrosPesquisa.LoadDropDown(drpTipoChamado, web, "Tipo de Chamado", "Título");
                FiltrosPesquisa.LoadDropDown(drpGerencia, web, "Gerência Responsável", "Órgão");

                if (FiltrosPesquisa.E_admin(web, "Administradores GGA Chamados", web.CurrentUser.Name))
                    onlyAdmin.Visible = true;

            }
        }

        protected void btPesquisar_Click(object sender, EventArgs e)
        {
            ErroDtInicio.Visible = false; ErrosDT.Visible = false; ErroDtFim.Visible = false;
            //divSolicitante.Visible = false; erroSolicitante.Visible = false;

            bool flag = true;

            if (dtIncio.Text == "" && dtFim.Text != "")
            {
                ErrosDT.Visible = true;
                ErroDtInicio.Visible = true;
                return;
            }
            else if (dtIncio.Text != "" && dtFim.Text == "")
            {
                ErrosDT.Visible = true;
                ErroDtInicio.Visible = true;
                return;
            }
            else if (dtIncio.Text != "")
            {
                try
                {
                    DateTime ini = Convert.ToDateTime(dtIncio.Text);
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
                            return;
                        }
                    }
                }
                catch
                {
                    ErrosDT.Visible = true;
                    return;
                }
            }
            else if (solicitante.AllEntities.Count > 1)
            {
                //divSolicitante.Visible = true;
                //erroSolicitante.Visible = true;
                return;
            }

            if (flag)
            {
                if (txtNumChamado.Text == "" && drpStatus.SelectedItem.Value == "0" && drpTipoChamado.SelectedItem.Value == "0" &&
                    dtIncio.Text == "" && dtFim.Text == "" && txtBusca.Text == "" && drpGerencia.SelectedItem.Value == "0" &&
                    solicitante.AllEntities.Count == 0 && drpComplexidade.SelectedItem.Value == "0" && drpImportancia.SelectedItem.Value == "0")
                {
                    //ConsultaTodos();
                    ConsultaFiltrosConstains();
                }
                else if (txtNumChamado.Text == "" && drpStatus.SelectedItem.Value == "0" && drpTipoChamado.SelectedItem.Value == "0" &&
                    dtIncio.Text == "" && dtFim.Text == "" && txtBusca.Text != "" && drpGerencia.SelectedItem.Value == "0" &&
                    solicitante.AllEntities.Count == 0 && drpComplexidade.SelectedItem.Value == "0" && drpImportancia.SelectedItem.Value == "0")
                {
                    ConsultaFiltrosConstains();
                }
                else
                    ConsultaFiltros();
            }
        }

        public void ConsultaFiltrosConstains()
        {
            SPList list = SPContext.Current.Web.Lists["Chamados"];
            DataTable items = null;

            try{
                SPQuery query = new SPQuery();
                items = list.GetItems().GetDataTable();
            }
            catch (Exception exs)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('" + exs.Message.Replace("'", "") + "');", true);
            }

            DataTable dtNew = new DataTable();
            
            string perfil = FiltrosPesquisa.Responsavel(SPContext.Current.Web);

            if (items != null)
            {
                if (perfil == "Admin")
                    dtNew = MontaDataTableAll(items);
                else if (perfil == "Publico")
                    dtNew = MontaDataTablePublico(items);
                else
                    dtNew = MontaDataTableResp(items, perfil);
            }

            try
            {
                if (dtNew.Rows.Count > 0)
                {
                    grid.DataSource = dtNew;
                    grid.DataBind();
                    grid.HeaderRow.TableSection = TableRowSection.TableHeader;
                    grid.Visible = true;
                    lblresultadoVazio.Visible = false;
                }
                else
                {
                    grid.Visible = false;
                    lblresultadoVazio.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('" + ex.Message.Replace("'", "") + "');", true);
            }
        }

        public DataTable MontaDataTableResp(DataTable items, string orgao)
        {
            if (txtBusca.Text != "")
            {
                items = RevisaComentatiosEsclarecimentos(items);
            }
            
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("Número do Chamado");
            dtNew.Columns.Add("Controle de Status");
            dtNew.Columns.Add("Tipo de Chamado");
            dtNew.Columns.Add("Nome do Solicitante");
            dtNew.Columns.Add("Assunto");
            dtNew.Columns.Add("Data de Solicitação");
            dtNew.Columns.Add("Data de Encerramento");

            foreach (DataRow row in items.Rows)
            {
                //string orgaoLinha = string.Empty;
                //if (row["OrgaoResponsavelMTC"].ToString() != "")
                //    orgaoLinha = row["OrgaoResponsavelMTC"].ToString();
                try
                {
                    DataRow rowNew = dtNew.NewRow();

                    if (row["Title"].ToString() != "")
                        rowNew[0] = row["Title"].ToString() + "|" + row["ID"].ToString();
                    else
                        rowNew[0] = "Sem Numeração|" + row["ID"].ToString();

                    if (row["ControleDeStatus"].ToString() != "")
                        rowNew[1] = row["ControleDeStatus"].ToString();
                    else
                        rowNew[1] = "-";

                    if (row["TipoDeChamado"].ToString() != "")
                        rowNew[2] = row["TipoDeChamado"].ToString();
                    else
                        rowNew[2] = "-";

                    if (row["Author"].ToString() != "")
                        rowNew[3] = row["Author"].ToString();
                    else
                        rowNew[3] = "-";

                    if (row["Assunto"].ToString() != "")
                        rowNew[4] = row["Assunto"].ToString();
                    else
                        rowNew[4] = "-";

                    if (row["Created"].ToString() != "")
                    {
                        DateTime dtInic = Convert.ToDateTime(row["Created"].ToString());
                        rowNew[5] = dtInic.Date.ToString("dd/MM/yyyy");
                    }
                    else
                        rowNew[5] = "-";

                    if (row["DataEncerramento"].ToString() != "")
                    {
                        DateTime dtInic = Convert.ToDateTime(row["DataEncerramento"].ToString());
                        rowNew[6] = dtInic.Date.ToString("dd/MM/yyyy");
                    }
                    else
                        rowNew[6] = "-";

                    dtNew.Rows.Add(rowNew);
                }
                catch (Exception ed)
                {
                    WriteLog(SPContext.Current.Web, "Pesquisa - Chamados", ed.Message);
                }
            }

            return dtNew;
        }

        public void ConsultaFiltros()
        {
            SPList list = SPContext.Current.Web.Lists["Chamados"];
            DataTable items = null;

            List<string> filtros = new List<string>(); List<string> valores = new List<string>();

            try
            {
                if (txtNumChamado.Text != "")
                { filtros.Add("Title"); valores.Add(txtNumChamado.Text); }
                if (drpStatus.SelectedItem.Value != "0")
                { filtros.Add("ControleDeStatus"); valores.Add(drpStatus.SelectedItem.Value); }
                if (drpTipoChamado.SelectedItem.Value != "0")
                { filtros.Add("TipoDeChamado"); valores.Add(drpTipoChamado.SelectedItem.Value); }
                if (dtIncio.Text != "")
                {
                    filtros.Add("Created"); valores.Add(Convert.ToDateTime(dtIncio.Text).ToString("s"));
                    filtros.Add("CreatedF"); valores.Add(Convert.ToDateTime(dtFim.Text).ToString("s"));
                }
                if (drpGerencia.SelectedItem.Value != "0")
                { filtros.Add("GerenciaResponsavel"); valores.Add(drpGerencia.SelectedItem.Value); }
                if (solicitante.AllEntities.Count > 0)
                { 
                    filtros.Add("Author"); 
                    valores.Add(solicitante.AllEntities[0].DisplayText); 
                }
                if (drpImportancia.SelectedItem.Value != "0")
                { filtros.Add("Importancia"); valores.Add(drpImportancia.SelectedItem.Value); }
                if (drpComplexidade.SelectedItem.Value != "0")
                { filtros.Add("Complexidade"); valores.Add(drpComplexidade.SelectedItem.Value); }
                //if (txtBusca.Text != "")
                //{ filtros.Add("DescricaoChamado"); valores.Add(txtBusca.Text); }

                SPQuery query = new SPQuery();
                query = FiltrosPesquisa.MontarQueryFiltros(filtros, valores);
                items = list.GetItems(query).GetDataTable();
            }
            catch (Exception exs)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('" + exs.Message.Replace("'", "") + "');", true);
            }

            DataTable dtNew = new DataTable();

            string perfil = FiltrosPesquisa.Responsavel(SPContext.Current.Web);

            if (items != null)
            {
                if (perfil == "Admin")
                    dtNew = MontaDataTableAll(items);
                else if( perfil == "Publico")
                    dtNew = MontaDataTablePublico(items);
                else
                    dtNew = MontaDataTableResp(items, perfil);
            }

            try
            {
                if (dtNew.Rows.Count > 0)
                {
                    grid.DataSource = dtNew;
                    grid.DataBind();
                    grid.HeaderRow.TableSection = TableRowSection.TableHeader;
                    grid.Visible = true;
                    lblresultadoVazio.Visible = false;
                }
                else
                {
                    grid.Visible = false;
                    lblresultadoVazio.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('" + ex.Message.Replace("'", "") + "');", true);
            }
        }

        protected void btLimpar_Click(object sender, EventArgs e)
        {
            //ErroDtInicio.Visible = false;
            //ErroDtFim.Visible = false;
            //ErrosDT.Visible = false;
            //erroSolicitante.Visible = false;
            //divSolicitante.Visible = false;
            //drpStatus.SelectedIndex = 0;
            //drpTipoChamado.SelectedIndex = 0;
            //drpGerencia.SelectedIndex = 0;
            //drpComplexidade.SelectedIndex = 0;
            //drpImportancia.SelectedIndex = 0;

            //dtIncio.Text = "";
            //dtFim.Text = "";
            //txtNumChamado.Text = "";
            //txtBusca.Text = "";

            //solicitante.AllEntities.Clear();
            //grid.Visible = false;
            //lblresultadoVazio.Visible = false;

            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        public DataTable RevisaComentatiosEsclarecimentos(DataTable items)
        {
            SPList list = SPContext.Current.Web.Lists["Chamados"];
            string busca = txtBusca.Text;
            bool flag = true;

            DataTable dtNew = items.Clone(); //new DataTable();

            foreach (DataRow row in items.Rows)
            {
                flag = true;
                if (!row["DescricaoChamado"].ToString().Contains(busca) || !row["Comentario"].ToString().Contains(busca))
                {
                    SPListItem item = list.GetItemById(Convert.ToInt32(row["ID"].ToString()));

                    int version = item.Versions.Count - 1; int versionAux = 0;

                    while (version >= versionAux)
                    {
                        string versoes = Suporte.ValidaTextField(item.Versions[versionAux]["Esclarecimentos"]);

                        if (versoes != string.Empty)
                        {
                            if (versoes.Contains(busca))
                            {
                                flag = true;
                                break;
                            }
                            else
                                flag = false;
                        }
                        versionAux++;
                    }

                    if (!flag)
                    {
                        versionAux = 0;version = item.Versions.Count - 1;

                        while (version >= versionAux)
                        {
                            string versoesGerencia = Suporte.ValidaTextField(item.Versions[versionAux]["EsclarecimentosGerencia"]);

                            if (versoesGerencia != string.Empty)
                            {
                                if (versoesGerencia.Contains(busca))
                                {
                                    flag = true;
                                    break;
                                }
                                else
                                    flag = false;
                            }

                            versionAux++;
                        }
                    }
                }

                if (flag)
                {
                    dtNew.ImportRow(row);
                    //dtNew.Rows.Add(row.ItemArray);
                }

            }
            return dtNew;
        }

        public DataTable MontaDataTableAll(DataTable items)
        {
            if (txtBusca.Text != "")
            {
                items = RevisaComentatiosEsclarecimentos(items);
            }

            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("Número do Chamado");
            dtNew.Columns.Add("Controle de Status");
            dtNew.Columns.Add("Tipo de Chamado");
            dtNew.Columns.Add("Nome do Solicitante");
            dtNew.Columns.Add("Assunto");
            dtNew.Columns.Add("Data de Solicitação");
            dtNew.Columns.Add("Data de Encerramento");

            foreach (DataRow row in items.Rows)
            {
                try
                {
                    DataRow rowNew = dtNew.NewRow();

                    if (row["Title"].ToString() != "")
                        rowNew[0] = row["Title"].ToString() + "|" + row["ID"].ToString();
                    else
                        rowNew[0] = "Sem Numeração|" + row["ID"].ToString();

                    if (row["ControleDeStatus"].ToString() != "")
                        rowNew[1] = row["ControleDeStatus"].ToString();
                    else
                        rowNew[1] = "-";

                    if (row["TipoDeChamado"].ToString() != "")
                        rowNew[2] = row["TipoDeChamado"].ToString();
                    else
                        rowNew[2] = "-";

                    if (row["Author"].ToString() != "")
                        rowNew[3] = row["Author"].ToString();
                    else
                        rowNew[3] = "-";

                    if (row["Assunto"].ToString() != "")
                        rowNew[4] = row["Assunto"].ToString();
                    else
                        rowNew[4] = "-";

                    if (row["Created"].ToString() != "")
                    {
                        DateTime dtInic = Convert.ToDateTime(row["Created"].ToString());
                        rowNew[5] = dtInic.Date.ToString("dd/MM/yyyy");
                    }
                    else
                        rowNew[5] = "-";

                    if (row["DataEncerramento"].ToString() != "")
                    {
                        DateTime dtInic = Convert.ToDateTime(row["DataEncerramento"].ToString());
                        rowNew[6] = dtInic.Date.ToString("dd/MM/yyyy");
                    }
                    else
                        rowNew[6] = "-";

                    dtNew.Rows.Add(rowNew);
                }
                catch (Exception ed)
                {
                    WriteLog(SPContext.Current.Web, "Pesquisa - Chamados", ed.Message);
                }
            }

            return dtNew;
        }

        public DataTable MontaDataTablePublico(DataTable items)
        {
            if (txtBusca.Text != "")
            {
                items = RevisaComentatiosEsclarecimentos(items);
            }

            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("Número do Chamado");
            dtNew.Columns.Add("Controle de Status");
            dtNew.Columns.Add("Tipo de Chamado");
            dtNew.Columns.Add("Nome do Solicitante");
            dtNew.Columns.Add("Assunto");
            dtNew.Columns.Add("Data de Solicitação");
            dtNew.Columns.Add("Data de Encerramento");

            string idUser = SPContext.Current.Web.CurrentUser.Name;

            foreach (DataRow row in items.Rows)
            {
                try
                {
                    if (row["Author"].ToString() == idUser.ToString())
                    {
                        DataRow rowNew = dtNew.NewRow();

                        if (row["Title"].ToString() != "")
                            rowNew[0] = row["Title"].ToString() + "|" + row["ID"].ToString();
                        else
                            rowNew[0] = "Sem Numeração|" + row["ID"].ToString();

                        if (row["ControleDeStatus"].ToString() != "")
                            rowNew[1] = row["ControleDeStatus"].ToString();
                        else
                            rowNew[1] = "-";

                        if (row["TipoDeChamado"].ToString() != "")
                            rowNew[2] = row["TipoDeChamado"].ToString();
                        else
                            rowNew[2] = "-";

                        if (row["Author"].ToString() != "")
                            rowNew[3] = row["Author"].ToString();
                        else
                            rowNew[3] = "-";

                        if (row["Assunto"].ToString() != "")
                            rowNew[4] = row["Assunto"].ToString();
                        else
                            rowNew[4] = "-";

                        if (row["Created"].ToString() != "")
                        {
                            DateTime dtInic = Convert.ToDateTime(row["Created"].ToString());
                            rowNew[5] = dtInic.Date.ToString("dd/MM/yyyy");
                        }
                        else
                            rowNew[5] = "-";

                        if (row["DataEncerramento"].ToString() != "")
                        {
                            DateTime dtInic = Convert.ToDateTime(row["DataEncerramento"].ToString());
                            rowNew[6] = dtInic.Date.ToString("dd/MM/yyyy");
                        }
                        else
                            rowNew[6] = "-";

                        dtNew.Rows.Add(rowNew);
                    }
                }
                catch (Exception ed)
                {
                    WriteLog(SPContext.Current.Web, "Pesquisa - Chamados", ed.Message);
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

    }
}
