using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using Furnas.OGX2.Service;
using System.IO;

namespace Furnas.OGX2.Webparts.WP_Importacao_Planos
{
    [ToolboxItemAttribute(false)]
    public partial class WP_Importacao_Planos : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public WP_Importacao_Planos()
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
                int IdDocumentoRepositorio = 0;
                SPWeb web = SPContext.Current.Web;
                try
                {
                    DataTable dtTemp = new DataTable();
                    
                    if (Path.GetExtension(flpArquivo.FileName) == ".xlsx")
                        dtTemp = Importacao.ReadExcel(flpArquivo.FileContent, web);
                    else
                        dtTemp = Importacao.ReadExceX(flpArquivo.FileContent, web);
                    
                    string Plano = ddlTipos.Items[ddlTipos.SelectedIndex].Text;
                    Service.Importacao.ValidaPlanilha(dtTemp, Plano, web);
                    int ciclo = Convert.ToInt32(ddlCiclo.SelectedValue);
                    IdDocumentoRepositorio = GravaDocumento(ciclo, Plano);
                    Service.Importacao.ImportDados(dtTemp, web, ciclo, Plano, ddlCiclo.Items[ddlCiclo.SelectedIndex].Text, IdDocumentoRepositorio);
                    
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Importação efetuada com sucesso!');", true);
                }
                catch (Exception e1)
                {
                    if (IdDocumentoRepositorio > 0)
                        web.Lists.TryGetList("Repositório").GetItemById(IdDocumentoRepositorio);

                    if(e1.Message.ToString() == "File contains corrupted data.")
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('O arquivo pode conter dados corrompidos.');", true);
                    else
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('" + e1.Message.Replace("'", "") + "');", true);

                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Selecione um arquivo Excel.');", true);
                return;
            }
        }

        public int GravaDocumento(int ciclo, string Plano)
        {
            SPDocumentLibrary library = SPContext.Current.Web.Lists["Repositório"] as SPDocumentLibrary;
            
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

            return item.ID;
        }

        internal static void LoadDropDown(System.Web.UI.WebControls.DropDownList ddlItems, SPWeb sPWeb, string listname, string lookupcolumn = null)
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

            //ddlItems.Items.Insert(0, new ListItem("-- Selecione um Ciclo --", "0"));
        }


    }
}
