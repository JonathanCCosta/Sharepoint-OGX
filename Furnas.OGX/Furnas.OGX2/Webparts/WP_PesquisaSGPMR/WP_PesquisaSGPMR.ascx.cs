using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Furnas.OGX2.Service;
using Microsoft.SharePoint;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Globalization;
using ClosedXML.Excel;
using System.IO;
using System.Linq;

namespace Furnas.OGX2.Webparts.WP_PesquisaSGPMR
{
    [ToolboxItemAttribute(false)]
    public partial class WP_PesquisaSGPMR : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public WP_PesquisaSGPMR()
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
                //CamposTipo();
                //divOutraFuncaoTransmissaoErro.Visible = false;
                divOutraFuncaoTransmissao.Attributes.Add("style", "display:none;");
                divOutraInstalacao.Attributes.Add("style", "display:none;");
            }
        }

        public void CamposTipo()
        {
            if (drpTipo.Value == "0")
            {
                divTipoEquipamento.Visible = false;
                divCodIdentificacao.Visible = false;
                drpTipoEquipamento.SelectedIndex = 0;
                txtCodIdentificacao.Text = "";
            }
            else
            {
                divTipoEquipamento.Visible = true;
                divCodIdentificacao.Visible = true;
            }
        }

        public void CarregaFiltros()
        {
            SPWeb web = SPContext.Current.Web;
            FiltrosSGPMR.LoadDropDown(drpCiclo, web, "Ciclos", "");
            //FiltrosSGPMR.LoadDropDown(drpInstalacao, web, "Domínio de Instalação", "");
            FiltrosSGPMR.LoadDropDownH(drpInstalacao, web, "Domínio de Instalação", "");
            FiltrosSGPMR.LoadDropDownH(drpFuncaoTransmissao, web, "Domínio de Função de Transmissão", "");
            //drpFuncaoTransmissao.Items.Add("Outra Função de transmissão"); Já existe na tabela
            //Tipo equipamento/ indicações -- Existentes
            FiltrosSGPMR.LoadRegistrosExistentes(drpTipoEquipamento, web, "Planos", "TipoEquipamento");
            FiltrosSGPMR.LoadRegistrosExistentes(drpIndicacoes, web, "Planos", "Indicacao");
            //Gestao Interna - Existentes
            FiltrosSGPMR.LoadRegistrosExistentes(drpOrgaoSolicitante, web, "Gestão Interna", "OrgaoSolicitante");
            FiltrosSGPMR.LoadRegistrosExistentes(drpOrgaoGestor, web, "Gestão Interna", "OrgaoGestorObra");
            FiltrosSGPMR.LoadRegistrosExistentes(drpOrgaoGestorResponsavel, web, "Gestão Interna", "ResponsavelOrgaoGestorObra");        
        }

        //protected void drpInstalacao_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    FiltrosSGPMR.LoadDropDownLookup(drpFuncaoTransmissao, SPContext.Current.Web, "Domínio de Função de Transmissão", drpInstalacao.SelectedItem.Value, "DominioInstalacao");
        //}

        //protected void drpFuncaoTransmissao_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (drpFuncaoTransmissao.SelectedItem.Text == "Outra Função de transmissão")
        //        divOutraFuncaoTransmissao.Visible = true;
        //    else
        //        divOutraFuncaoTransmissao.Visible = false;
        //}

        //protected void drpTipo_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    CamposTipo();
        //}

        protected void btPesquisar_Click(object sender, EventArgs e)
        {
            ErroDt.Visible = false; ErrosDT.Visible = false; ErroDtFim.Visible=false;
            //divOutraFuncaoTransmissaoErro.Visible = false;

            bool flag = true;

            if (drpFuncaoTransmissao.Items[drpFuncaoTransmissao.SelectedIndex].Text == "Outra Função de transmissão")
            {
                if (txtFuncaoTransmissao.Text == string.Empty)
                {
                    divOutraFuncaoTransmissaoErro.Visible = true;
                    flag = false;
                    divOutraFuncaoTransmissao.Attributes.Add("style", "display:block;");
                }
            }

            if (drpInstalacao.Items[drpInstalacao.SelectedIndex].Text == "Outra Instalação")
            {
                if (txtOutraInstalacao.Text == string.Empty)
                {
                    divErroInstalacao.Visible = true;
                    flag = false;
                    divOutraInstalacao.Attributes.Add("style", "display:block;");
                }
            }
            
            if ((dtInicio.Text != "" && dtInicioFim.Text == "") || (dtInicio.Text == "" && dtInicioFim.Text != ""))
            {
                ErrosDT.Visible = true;
                ErroDt.Visible = true;
                return;
            }
            else if (dtInicio.Text != "")
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

            if(flag)
                ConsultaFiltros();

        }

        protected void btLimpar_Click(object sender, EventArgs e)
        {
            drpTipo.SelectedIndex = 0;
            drpCiclo.SelectedIndex = 0;
            txtNumeracao.Text = "";
            drpInstalacao.SelectedIndex = 0;
            drpTipoEquipamento.SelectedIndex = 0;
            txtCodIdentificacao.Text = "";
            drpFuncaoTransmissao.SelectedIndex = 0;
            divOutraFuncaoTransmissao.Visible = false;
            divOutraFuncaoTransmissaoErro.Visible = false;
            divErroInstalacao.Visible = false;
            drpRevitalizacao.SelectedIndex = 0;
            drpIndicacoes.SelectedIndex = 0;
            dtInicio.Text = "";
            dtInicioFim.Text = "";
            ErrosDT.Visible = false;
            ErroDt.Visible = false;
            ErroDtFim.Visible = false;
            drpOrgaoGestor.SelectedIndex = 0;
            drpOrgaoGestorResponsavel.SelectedIndex = 0;
            drpOrgaoSolicitante.SelectedIndex = 0;
            //CamposTipo();
            grid.Visible = false;
            lblresultadoVazio.Visible = false;

            divOutraFuncaoTransmissao.Attributes.Add("style", "display:none;");
            divOutraInstalacao.Attributes.Add("style", "display:none;");

            BtExportar.Visible = false;
        }

        public static string sbExport = string.Empty;

        public void ConsultaFiltros()
        {
            SPList list = SPContext.Current.Web.Lists["Planos"];
            DataTable items = null;

            List<string> filtros = new List<string>(); List<string> valores = new List<string>();

            try
            {
                if (drpCiclo.SelectedItem.Value != "0")
                { filtros.Add("Ciclo"); valores.Add(drpCiclo.SelectedItem.Value); }
                if (txtNumeracao.Text != "")
                { filtros.Add("Title"); valores.Add(txtNumeracao.Text); }
                if (drpInstalacao.Value != "0")
                {
                    if (txtOutraInstalacao.Text != "")
                    {
                        filtros.Add("InstalacaoO"); valores.Add(txtOutraInstalacao.Text);
                    }
                    else
                    {
                        filtros.Add("Instalacao"); valores.Add(drpInstalacao.Items[drpInstalacao.SelectedIndex].Text);
                    }
                }
                if (drpFuncaoTransmissao.Value != "0")
                {
                    if (txtFuncaoTransmissao.Text != "")
                    {
                        filtros.Add("FuncaoTransmissaoO"); valores.Add(txtFuncaoTransmissao.Text);
                    }
                    else
                    {
                        filtros.Add("FuncaoTransmissao"); valores.Add(drpFuncaoTransmissao.Items[drpFuncaoTransmissao.SelectedIndex].Text);
                    }
                }
                if (drpRevitalizacao.SelectedItem.Value != "0")
                { filtros.Add("StatusRevitalizacao"); valores.Add(drpRevitalizacao.SelectedItem.Text); }
                if (drpIndicacoes.SelectedItem.Value != "0")
                { filtros.Add("Indicacao"); valores.Add(drpIndicacoes.SelectedItem.Text); }
                
                if (drpTipo.Value != "0")
                {
                    if (drpTipoEquipamento.SelectedItem.Value != "0")
                    { filtros.Add("TipoEquipamento"); valores.Add(drpTipoEquipamento.SelectedItem.Text); }
                    if (txtCodIdentificacao.Text != "")
                    { filtros.Add("CodigoIdentificacao"); valores.Add(txtCodIdentificacao.Text); }
                }
               
                if (dtInicio.Text != "")
                {
                    filtros.Add("PrevisaoImplantacao"); valores.Add(Convert.ToDateTime(dtInicio.Text).ToString("s"));
                    filtros.Add("PrevisaoImplantacaoF"); valores.Add(Convert.ToDateTime(dtInicioFim.Text).ToString("s"));
                }

                filtros.Add("ContentType"); valores.Add(DeParaSolicitacao(drpTipo.Items[drpTipo.SelectedIndex].Text));

                SPQuery query = new SPQuery();
                query = FiltrosSGPMR.MontarQueryFiltros(filtros, valores);
                items = list.GetItems(query).GetDataTable();

                sbExport = query.Query;
            }
            catch (Exception exs)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('" + exs.Message.Replace("'", "") + "');", true);
            }


            DataTable itemsG = GestaoFiltros();
            if (itemsG != null && items != null)
                items = PlanosXGestao(items, itemsG);
            else if (items == null && itemsG != null)
            {
                if(filtros.Count <= 1)
                    items = BuscaPlanosPorGestao(itemsG);
            }
                
            DataTable dtNew = new DataTable();
            
            if (items != null)
            {
                if (drpTipo.Items[drpTipo.SelectedIndex].Text != "Plano de Reforços por Superação")
                    dtNew = MontaDataTableMelhoriaReforco(items);
                else
                    dtNew = MontaDataTableSuperacao(items);
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
                    BtExportar.Visible = true;
                }
                else
                {
                    grid.Visible = false;
                    lblresultadoVazio.Visible = true;
                    BtExportar.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('" + ex.Message.Replace("'", "") + "');", true);
            }
        }

        public static string gestaoExport= string.Empty;

        public DataTable GestaoFiltros()
        {
            DataTable itemsG = null;

            if (drpOrgaoSolicitante.SelectedItem.Value != "0" || drpOrgaoGestor.SelectedItem.Value != "0" || drpOrgaoGestorResponsavel.SelectedItem.Value != "0")
            {
                SPList listG = SPContext.Current.Web.Lists["Gestão Interna"];

                List<string> filtrosG = new List<string>(); List<string> valoresG = new List<string>();
                if (drpOrgaoSolicitante.SelectedItem.Value != "0")
                { filtrosG.Add("OrgaoSolicitante"); valoresG.Add(drpOrgaoSolicitante.SelectedItem.Text); }
                if (drpOrgaoGestor.SelectedItem.Value != "0")
                { filtrosG.Add("OrgaoGestorObra"); valoresG.Add(drpOrgaoGestor.SelectedItem.Text); }
                if (drpOrgaoGestorResponsavel.SelectedItem.Value != "0")
                { filtrosG.Add("ResponsavelOrgaoGestorObra"); valoresG.Add(drpOrgaoGestorResponsavel.SelectedItem.Text); }

                SPQuery query = new SPQuery();
                query = FiltrosSGPMR.MontarQueryFiltros(filtrosG, valoresG);
                itemsG = listG.GetItems(query).GetDataTable();

                gestaoExport = query.Query;
            }

            return itemsG;
        }

        public DataTable PlanosXGestao(DataTable p, DataTable g)
        {
            DataTable itemsG = new DataTable();

            foreach (DataRow row in p.Rows)
            {
                foreach (DataRow rowg in g.Rows)
                {
                    if (rowg["NumeroSGPRM"].ToString() == row["Title"].ToString())
                    {
                        if (itemsG.Rows.Count == 0)
                        {
                            foreach (DataColumn coll in row.Table.Columns)
                            {
                                itemsG.Columns.Add(coll.ColumnName);
                            }
                            itemsG.Rows.Add(row.ItemArray);
                            break;
                        }
                        else
                        {
                            itemsG.Rows.Add(row.ItemArray);
                            break;
                        }
                    }
                }
            }

            return itemsG;
        }

        public DataTable BuscaPlanosPorGestao(DataTable g)
        {
            DataTable itemsP = new DataTable();

            SPWeb web = SPContext.Current.Web;
            string tipo = drpTipo.Items[drpTipo.SelectedIndex].Text; //drpTipo.SelectedItem.Text;

            foreach (DataRow row in g.Rows)
            {
                DataTable its = FiltrosSGPMR.PesquisaPlano(web, row["NumeroSGPRM"].ToString(), tipo);
                if (its != null)
                {
                    if (itemsP.Rows.Count == 0)
                    {
                        foreach (DataColumn coll in its.Columns)
                        {
                            itemsP.Columns.Add(coll.ColumnName);
                        }
                    }

                    if (its.Rows.Count > 1)
                    {
                        foreach (DataRow it in its.Rows)
                        {
                            itemsP.Rows.Add(it.ItemArray);
                        }
                    }
                    else
                        itemsP.Rows.Add(its.Rows[0].ItemArray);
                }
            }

            return itemsP;
        }

        public static string DeParaSolicitacao(string plano)
        {
            if (plano.Contains("Melhoria"))
                return "Plano de Melhoria - Mudanças";
            else
                return "Plano por Superação - Mudanças";
        }

        public static string DeParaPlanos(string plano)
        {
            if (plano.Contains("Melhoria") || plano.Contains("Melhorias"))
                return "Plano de Melhoria";
            else
                return "Plano Por Superação";
        }

        public DataTable MontaDataTableMelhoriaReforco(DataTable items)
        {
            DataTable dtNew = new DataTable();

            dtNew.Columns.Add("Ciclo");
            dtNew.Columns.Add("Numeração");
            dtNew.Columns.Add("Instalação");
            dtNew.Columns.Add("Função de Transmissão");
            dtNew.Columns.Add("Status da Revitalização");
            dtNew.Columns.Add("Indicações");
            dtNew.Columns.Add("Previsão da Instalação");

            foreach (DataRow row in items.Rows)
            {
                try
                {
                    DataRow rowNew = dtNew.NewRow();

                    if (row["Ciclo"].ToString() != "")
                        rowNew[0] = row["Ciclo"].ToString();
                    else
                        rowNew[0] = "-";

                    if (row["Title"].ToString() != "")
                        rowNew[1] = row["Title"].ToString() + "|" + row["ID"].ToString();
                    else
                        rowNew[1] = "Sem Numeração|" + row["ID"].ToString();

                    if (row["Instalacao"].ToString() != "")
                        rowNew[2] = row["Instalacao"].ToString();
                    else
                        rowNew[2] = "-";

                    if (row["FuncaoTransmissao"].ToString() != "")
                        rowNew[3] = row["FuncaoTransmissao"].ToString();
                    else
                        rowNew[3] = "-";

                    if (row["StatusRevitalizacao"].ToString() != "")
                        rowNew[4] = row["StatusRevitalizacao"].ToString();
                    else
                        rowNew[4] = "-";

                    if (row["Indicacao"].ToString() != "")
                        rowNew[5] = row["Indicacao"].ToString();
                    else
                        rowNew[5] = "-";

                    if (row["PrevisaoImplantacao"].ToString() != "")
                    {
                        DateTime dtInic = Convert.ToDateTime(row["PrevisaoImplantacao"].ToString());
                        rowNew[6] = dtInic.Date.ToString("dd/MM/yyyy");
                    }
                    else
                        rowNew[6] = "-";

                    dtNew.Rows.Add(rowNew);
                }
                catch (Exception ed)
                {
                    WriteLog(SPContext.Current.Web, "Pesquisa", ed.Message);
                }
            }
            return dtNew;
        }

        public DataTable MontaDataTableSuperacao(DataTable items)
        {
            DataTable dtNew = new DataTable();

            dtNew.Columns.Add("Ciclo");
            dtNew.Columns.Add("Numeração");
            dtNew.Columns.Add("Instalação");
            dtNew.Columns.Add("Tipo de Equipamento");
            dtNew.Columns.Add("Código de Identificação");

            foreach (DataRow row in items.Rows)
            {
                try
                {
                    DataRow rowNew = dtNew.NewRow();

                    if (row["Ciclo"].ToString() != "")
                        rowNew[0] = row["Ciclo"].ToString();
                    else
                        rowNew[0] = "-";

                    if (row["Title"].ToString() != "")
                        rowNew[1] = row["Title"].ToString() + "|" + row["ID"].ToString();
                    else
                        rowNew[1] = "Sem Numeração|" + row["ID"].ToString();

                    if (row["Instalacao"].ToString() != "")
                        rowNew[2] = row["Instalacao"].ToString();
                    else
                        rowNew[2] = "-";

                    if (row["TipoEquipamento"].ToString() != "")
                        rowNew[3] = row["TipoEquipamento"].ToString();
                    else
                        rowNew[3] = "-";

                    if (row["CodigoIdentificacao"].ToString() != "")
                        rowNew[4] = row["CodigoIdentificacao"].ToString();
                    else
                        rowNew[4] = "-";

                    dtNew.Rows.Add(rowNew);

                }
                catch (Exception ed)
                {
                    WriteLog(SPContext.Current.Web, "Pesquisa", ed.Message);
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

        protected void BtExportar_Click(object sender, EventArgs e)
        {
            ConsultaFiltrosExport();
        }

        public void ConsultaFiltrosExport()
        {
            SPWeb web = SPContext.Current.Web;
            SPList list = web.Lists["Planos"];
            DataTable items = null;
            
            try
            {
                SPQuery query = new SPQuery();
                query.Query = sbExport;
                items = list.GetItems(query).GetDataTable();
            }
            catch (Exception exs)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('" + exs.Message.Replace("'", "") + "');", true);
            }

            DataTable itemsG = null;
            SPList listG = web.Lists["Gestão Interna"];
            if (gestaoExport != string.Empty)
            {
                SPQuery queryG = new SPQuery();
                queryG.Query = gestaoExport;
                itemsG = listG.GetItems(queryG).GetDataTable();
            }

            if (itemsG != null && items != null)
                items = PlanosXGestao(items, itemsG);
            else if (items == null && itemsG != null)
                items = BuscaPlanosPorGestao(itemsG);

            DataTable dtNew = new DataTable();

            if (items != null)
            {
                dtNew = ReturnTableMontada(items, web, list, listG, drpTipo.Items[drpTipo.SelectedIndex].Text);
            }

            try
            {
                if (dtNew.Rows.Count > 0)
                {
                    string name = BuildFileName(DateTime.Now, drpTipo.Items[drpTipo.SelectedIndex].Text); 
                    string path = "C:\\ExportacoesPlanosGGA\\";
                    DirectoryInfo directoryInfo = new DirectoryInfo(path); 
                    directoryInfo.Create();
                    bool Exportou = false;

                    var wb = new XLWorkbook();
                    var ws = wb.Worksheets.Add(dtNew, drpTipo.Items[drpTipo.SelectedIndex].Text);

                    //ws.Range(ws.FirstCellUsed(), ws.LastCellUsed()).CreateTable().Theme = XLTableTheme.TableStyleLight9;
                    ws.Columns().AdjustToContents();
                    wb.SaveAs(path + name);
                    wb.Dispose();
                    Exportou = true;

                    if (Exportou)
                    {
                        if (ExportacaoSolicitacao.UploadFileDataBackup(web, path + name))
                        {
                            try { File.Delete(path + name); }
                            catch { }
                            string pagina = "<script>window.open('" + web.Url.ToString() + "/Documentos Compartilhados/" + name + "','_blank');</script>";
                            Page.Response.Write(pagina);
                        }

                        WriteLog(web, "Exportar Planos - Pesquisa", "Concluída com sucesso.");
                    }
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Exportação efetuada com sucesso!');", true);
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('" + ex.Message.Replace("'", "") + "');", true);
            }
        }

        private static string BuildFileName(DateTime currentdate, string CicloName)
        {
            return string.Format("Exportação Planos {0} - {1}.xlsx", new object[]
			{
				CicloName,
				currentdate.ToString("dd-MM-yyyy_hh-mm-ss")
			});
        }

        private static DataTable ReturnTableMontada(DataTable dt, SPWeb web, SPList list, SPList listG, string tipo)
        {
            if (dt != null)
            {
                SPFieldCollection ct = list.ContentTypes[DeParaPlanos(tipo)].Fields;
                Dictionary<string, string> cd = new Dictionary<string, string>();
                foreach (SPField f in ct)
                {
                    cd.Add(f.Title, f.InternalName);
                }

                SPFieldCollection fieldG = listG.ContentTypes[0].Fields;
                Dictionary<string, string> cg = new Dictionary<string, string>();
                foreach (SPField fg in fieldG)
                {
                    try
                    {
                        cg.Add(fg.Title, fg.InternalName);
                        if(fg.InternalName == "Title")
                            dt.Columns.Add(fg.Title);
                        else
                            dt.Columns.Add(fg.InternalName);
                    }
                    catch { }
                }

                DataTable dt2 = Vigentes(listG);

                foreach (DataRow row in dt.Rows)
                {
                    DataRow rowGestao = RetornaGI(Convert.ToString(row["Title"]), dt2);

                    if (rowGestao != null)
                    {
                        foreach (DataColumn col in row.Table.Columns)
                        {
                            if (col.ColumnName != "Title")
                            {
                                if (cg.ContainsValue(col.ColumnName))
                                    row[col.ColumnName] = Convert.ToString(rowGestao[col.ColumnName]);
                                //else
                                    //row["NumeroSGPRM"] = Convert.ToString(rowGestao[col.ColumnName]);
                            }
                        }
                    }

                }

                foreach (DataColumn column in dt.Columns.Cast<DataColumn>().ToList())
                {
                    if (cd.ContainsValue(column.ColumnName))
                        column.ColumnName = list.Fields.GetFieldByInternalName(column.ColumnName).Title;
                    else if (cg.ContainsValue(column.ColumnName))
                        column.ColumnName = listG.Fields.GetFieldByInternalName(column.ColumnName).Title;
                    else
                        dt.Columns.Remove(column.ColumnName);
                }
            }

            dt.Columns.Remove("ID Documento");
            dt.Columns.Remove("Nome do Ciclo");
            dt.Columns.Remove("Aprovação da solicitação");
            dt.Columns.Remove("Histórico");
            dt.Columns.Remove("Tipo de Conteúdo");

            return dt;
        }

        public static DataRow RetornaGI(string numeracao, DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                if (row["NumeroSGPRM"].ToString() == numeracao)
                    return row;
            }

            return null;
        }

        public static DataTable Vigentes(SPList list)
        {
            SPQuery query = new SPQuery();
            query.Query = "<Where><Eq><FieldRef Name='AprovacaoSolicitacao' /><Value Type='Choice'>Aprovado</Value></Eq></Where>";
            return list.GetItems(query).GetDataTable();
        }
    }
}
