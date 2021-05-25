using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Web.UI.WebControls.WebParts;
using Furnas.OGX2.Service;

namespace Furnas.OGX2.Webparts.WP_ImportaSolicitacoes
{
    [ToolboxItemAttribute(false)]
    public partial class WP_ImportaSolicitacoes : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public WP_ImportaSolicitacoes()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (Path.GetExtension(flpArquivo.FileName) == ".xlsx")
            {
                int lote = 0; erros.Text = ""; divValidacao.Visible = false;
                SPWeb web = SPContext.Current.Web;
                try
                {
                    DataTable dtTemp = new DataTable();

                    if (Path.GetExtension(flpArquivo.FileName) == ".xlsx")
                        dtTemp = ImportacaoSolicitacao.ReadExcel(flpArquivo.FileContent);

                    string Plano = ddlTipo.Items[ddlTipo.SelectedIndex].Text;
                    string errosRet = Service.ImportacaoSolicitacao.ValidaPlanilha(dtTemp, Plano, web);

                    if (errosRet == string.Empty)
                    {
                        lote = Log(web, "Importação de Solicitações em andamento", Plano, 0);
                        Service.ImportacaoSolicitacao.ImportDadosSolicitacao(dtTemp, web, Plano, lote);
                        Log(web, "Importação de Solicitações Concluída", Plano, lote);
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Importação efetuada com sucesso!');", true);
                    }
                    else
                    {
                        erros.Text = "<br/><b>Erros encontrados: </b></br></br>" + errosRet.ToString();
                        divValidacao.Visible = true;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Erros encontrados verifique os logs abaixo.');", true);
                    }
                }
                catch (Exception e1)
                {
                    if (e1.Message.ToString() == "File contains corrupted data.")
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('O arquivo pode conter dados corrompidos.');", true);
                    else
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('" + e1.Message.Replace("'", "") + "');", true);

                    Log(web, "Importação de Solicitações com erros.", ddlTipo.Items[ddlTipo.SelectedIndex].Text + "-" + e1.Message , lote);

                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Selecione um arquivo Excel.');", true);
                return;
            }
        }

        public static int Log(SPWeb web, string desc, string plano, int id)
        {
            try
            {
                SPList logs = web.Lists["Log Solicitação Novos Planos"];
                SPListItem item = null;
                if (id == 0)
                {
                    item = logs.AddItem();
                    item[SPBuiltInFieldId.Title] = plano;
                }
                else
                {
                    item = logs.GetItemById(id);
                }
                item["Descrição"] = desc;
                item.Update();

                return item.ID;
            }
            catch { }

            return 0;
        }
    }
}
