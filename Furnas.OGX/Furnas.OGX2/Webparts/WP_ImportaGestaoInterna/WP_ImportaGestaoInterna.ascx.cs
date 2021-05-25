using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using System.IO;
using System.Data;
using Furnas.OGX2.Service;

namespace Furnas.OGX2.Webparts.WP_ImportaGestaoInterna
{
    [ToolboxItemAttribute(false)]
    public partial class WP_ImportaGestaoInterna : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public WP_ImportaGestaoInterna()
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
                LoadDropDown(ddlCiclo, SPContext.Current.Web, "Ciclos");
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (Path.GetExtension(flpArquivo.FileName) == ".xlsx" || Path.GetExtension(flpArquivo.FileName) == ".xls")
            {
                string Plano = ddlTipos.Items[ddlTipos.SelectedIndex].Text;
                int ciclo = Convert.ToInt32(ddlCiclo.SelectedValue);
                SPWeb web = SPContext.Current.Web;
                try
                {
                    DataTable dtTemp = new DataTable();

                    if (Path.GetExtension(flpArquivo.FileName) == ".xlsx")
                        dtTemp = ImportacaoGestao.ReadExcel(flpArquivo.FileContent, web);
                    else
                        dtTemp = ImportacaoGestao.ReadExceX(flpArquivo.FileContent, web);

                    if (dtTemp.Rows.Count > 0)
                    {
                        //Plano = ddlTipos.Items[ddlTipos.SelectedIndex].Text;
                        Service.ImportacaoGestao.ValidaPlanilhaGestao(dtTemp, Plano, web);
                        //int ciclo = Convert.ToInt32(ddlCiclo.SelectedValue);
                        Service.ImportacaoGestao.ImportDadosGestao(dtTemp, web, ciclo, Plano, ddlCiclo.Items[ddlCiclo.SelectedIndex].Text);
                        GravaDocumentoGestao(ciclo, Plano);

                        Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Importação efetuada com sucesso!');", true);
                    }
                    else
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Não existem dados a serem importados.');", true);
                }
                catch (Exception e1)
                {
                    Log(web, "Erro: " + e1.Message.ToString(), Plano + " / " + ciclo);
                    if (e1.Message.ToString() == "File contains corrupted data.")
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('O arquivo pode conter dados corrompidos.');", true);
                    else if (e1.Message == "A coluna No. SGPRM deve ser prenchida!")
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('A coluna No. SGPRM deve ser prenchida!');", true);
                    else if (e1.Message == "A coluna Previsão de implantação FURNAS deve conter somente datas.")
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('A coluna Previsão de implantação FURNAS deve conter somente datas.');", true);
                    else if (e1.Message == "A coluna Data da última atualização de informações deve conter somente datas.")
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('A planilha não esta no Modelo esperado.');", true);
                    else if (e1.Message == "A planilha não esta no Modelo esperado.")
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('A planilha não esta no Modelo esperado.');", true);
                    else
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Ocorreu um erro na Importação. Tente verificar o modelo de importação.');", true);
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('" + e1.Message.Replace("'", "") + "');", true);
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Selecione um arquivo Excel.');", true);
                return;
            }
        }

        public void GravaDocumentoGestao(int ciclo, string Plano)
        {
            SPDocumentLibrary library = SPContext.Current.Web.Lists["Repositório Gestão Interna"] as SPDocumentLibrary;

            string cicloName = ddlCiclo.Items[ddlCiclo.SelectedIndex].Text;
            string name = Path.GetFileNameWithoutExtension(flpArquivo.FileName) + "_" + cicloName + "_" + Plano + "_" + DateTime.Now.ToString("dd-MM-yyyy") + Path.GetExtension(flpArquivo.FileName);

            string destURL = library.RootFolder.Url + "/" + name;
            byte[] filebytes = flpArquivo.FileBytes;
            SPFile file = library.RootFolder.Files.Add(destURL, filebytes, true);

            SPListItem item = file.Item;
            item[SPBuiltInFieldId.Title] = name;
            item["Plano de Gerenciamento"] = Plano;
            item["Ciclo"] = new SPFieldLookupValue(ciclo, cicloName);
            item.Update();
        }

        internal static void LoadDropDown(System.Web.UI.WebControls.DropDownList ddlItems, SPWeb sPWeb, string listname)
        {
            SPList list = sPWeb.Lists[listname];

            foreach (SPListItem item in list.Items)
            {
                ListItem lt = new ListItem();
                lt.Value = item.ID.ToString();
                lt.Text = item.Title;

                ddlItems.Items.Add(lt);
            }

            List<ListItem> itemsDDL = ddlItems.Items.Cast<ListItem>().OrderBy(lt => lt.Text).ToList();
            ddlItems.Items.Clear();
            ddlItems.Items.AddRange(itemsDDL.ToArray());
        }

        public static void Log(SPWeb web, string erro, string plano)
        {
            try
            {
                SPList GerenciaPlanos = web.Lists["Logs"];
                SPListItem item = GerenciaPlanos.AddItem();

                item[SPBuiltInFieldId.Title] = plano;
                item["Descrição"] = erro;
                item.Update();
            }
            catch { }
        }
    }
}
