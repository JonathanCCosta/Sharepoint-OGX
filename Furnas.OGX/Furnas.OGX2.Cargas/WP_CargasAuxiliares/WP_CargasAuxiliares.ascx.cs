using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Furnas.OGX2.Cargas.ResourcesCarga;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Taxonomy;

namespace Furnas.OGX2.Cargas.WP_CargasAuxiliares
{
    [ToolboxItemAttribute(false)]
    public partial class WP_CargasAuxiliares : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public WP_CargasAuxiliares()
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

        protected void carregar_Click(object sender, EventArgs e)
        {
            try
            {
                Stream s = ResourceCarga.LerDocumentNaBiblioteca(SPContext.Current.Web, "Carga MTC.xlsx");
                DataTable dt = ResourceCarga.ReadExcel(s);

                SPList list = SPContext.Current.Web.Lists["Tipo MTC"];
                List<string> distinct = new List<string>();

                foreach (DataRow row in dt.Rows)
                {
                    if (!distinct.Contains(row[1].ToString()))
                    {
                        try
                        {
                            SPListItem item = list.AddItem();
                            item["Codigo"] = row[1].ToString();
                            item["Descrição"] = row[2].ToString();
                            item["Descrição Tipo"] = row[1].ToString() + " - " + row[2].ToString();
                            item.Update();
                            distinct.Add(row[1].ToString());
                        }
                        catch (Exception ex)
                        {
                            ResourceCarga.WriteLog(SPContext.Current.Web, "Carregar Tipo MTC", ex.Message);
                        }
                    }
                }

                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Carga efetuada com sucesso!');", true);

            }
            catch (Exception ex)
            {
                ResourceCarga.WriteLog(SPContext.Current.Web, "Carregar Tipo MTC", ex.Message);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Erro ao processar Tipo MTC');", true);
            }
        }

        protected void carregarFabricante_Click(object sender, EventArgs e)
        {
            try
            {
                Stream s = ResourceCarga.LerDocumentNaBiblioteca(SPContext.Current.Web, "Fabricante.xlsx");
                DataTable dt = ResourceCarga.ReadExcel(s);

                SPList list = SPContext.Current.Web.Lists["Fabricante MTC"];
                List<string> distinct = new List<string>();

                foreach (DataRow row in dt.Rows)
                {
                    if (!distinct.Contains(row[0].ToString()))
                    {
                        try
                        {
                            SPListItem item = list.AddItem();
                            item["Código do Fabricante"] = row[0].ToString();
                            item["Descrição do Fabricante"] = row[1].ToString();
                            item["Descrição Fabricante"] = row[0].ToString() + " - " + row[1].ToString();
                            item.Update();
                            distinct.Add(row[0].ToString());
                        }
                        catch (Exception ex)
                        {
                            ResourceCarga.WriteLog(SPContext.Current.Web, "Carregar Tipo MTC", ex.Message);
                        }
                    }
                }

                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Carga efetuada com sucesso!');", true);

            }
            catch (Exception ex)
            {
                ResourceCarga.WriteLog(SPContext.Current.Web, "Carregar Fabricante MTC", ex.Message);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Erro ao processar Fabricante');", true);
            }
        }

        protected void carregarGrupo_Click(object sender, EventArgs e)
        {
            try
            {
                Stream s = ResourceCarga.LerDocumentNaBiblioteca(SPContext.Current.Web, "Grupo.xlsx");
                DataTable dt = ResourceCarga.ReadExcel(s);

                SPList list = SPContext.Current.Web.Lists["Grupo MTC"];
                List<string> distinct = new List<string>();

                foreach (DataRow row in dt.Rows)
                {
                    if (!distinct.Contains(row[0].ToString()))
                    {
                        SPListItem item = list.AddItem();
                        string codg = row[0].ToString().Length < 2 ? "0" + row[0].ToString() : row[0].ToString();
                        item["Código do Grupo"] = codg;
                        item["Descrição do Grupo"] = row[1].ToString();
                        item["Descrição Grupo"] = codg + " - " + row[1].ToString();
                        item.Update();
                        distinct.Add(row[0].ToString());
                    }
                }

                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Carga efetuada com sucesso!');", true);

            }
            catch (Exception ex)
            {
                ResourceCarga.WriteLog(SPContext.Current.Web, "Carregar Tipo MTC", ex.Message);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Erro ao processar Grupo');", true);
            }
        }

        protected void carregarSubgrupo_Click(object sender, EventArgs e)
        {
            try
            {
                Stream s = ResourceCarga.LerDocumentNaBiblioteca(SPContext.Current.Web, "Subgrupo.xlsx");
                DataTable dt = ResourceCarga.ReadExcel(s);

                SPList list = SPContext.Current.Web.Lists["Subgrupo MTC"];
                string cod = string.Empty;
                SPListItemCollection items = SPContext.Current.Web.Lists["Grupo MTC"].GetItems();

                foreach (DataRow row in dt.Rows)
                {
                    if (row[2].ToString() != "")
                    {
                        SPListItem item = list.AddItem();
                        if (row[2].ToString().Length < 2)
                        {
                            cod = "0" + row[2].ToString();
                            item["Código do Subgrupo"] = cod;
                        }
                        else
                        {
                            cod = row[2].ToString();
                            item["Código do Subgrupo"] = cod;
                        }

                        item["Descrição do Subgrupo"] = row[3].ToString();
                        item["Descrição Subgrupo"] = cod + " - " + row[3].ToString();

                        string codg = row[0].ToString().Length < 2 ? "0" + row[0].ToString() : row[0].ToString();

                        SPListItem proc = items.OfType<SPListItem>().Where(p => p["Código do Grupo"].ToString() == codg).FirstOrDefault();
                        if (proc != null)
                            item["Código do Grupo"] = new SPFieldLookupValue(proc.ID, proc.Title);
                        item.Update();
                    }
                }

                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Carga efetuada com sucesso!');", true);

            }
            catch (Exception ex)
            {
                ResourceCarga.WriteLog(SPContext.Current.Web, "Carregar Tipo MTC", ex.Message);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Erro ao processar SubGrupo');", true);
            }
        }

        protected void carregarMTC_Click(object sender, EventArgs e)
        {
            try
            {
                Stream s = ResourceCarga.LerDocumentNaBiblioteca(SPContext.Current.Web, "Carga MTC.xlsx");
                DataTable dt = ResourceCarga.ReadExcel(s);

                SPList list = SPContext.Current.Web.Lists["Gerência Responsável"];
                List<string> orgaos = new List<string>();

                foreach (DataRow row in dt.Rows)
                {
                    if (row[22].ToString() != "")
                    {
                        if (!orgaos.Contains(row[22].ToString()))
                        {
                            SPListItem item = list.AddItem();
                            item[SPBuiltInFieldId.Title] = row[22].ToString();
                            item.Update();

                            orgaos.Add(row[22].ToString());
                        }
                    }
                }

                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Carga efetuada com sucesso!');", true);

            }
            catch (Exception ex)
            {
                ResourceCarga.WriteLog(SPContext.Current.Web, "Carregar Tipo MTC", ex.Message);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Erro ao processar SubGrupo');", true);
            }
        }

        protected void carregaPalavraChave_Click(object sender, EventArgs e)
        {
            try
            {
                // Get a TaxonomySession from the site
                TaxonomySession session = new TaxonomySession(SPContext.Current.Site);
                TermStore termStore = session.TermStores[0];
                TermSet palavraChave = null;
                if (termStore != null)
                {
                    foreach (Group group in termStore.Groups)
                    {
                        if (palavraChave == null)
                        {
                            if (group.Name.Contains(SPContext.Current.Site.Url.Split('/')[3]))
                            {
                                TermSetCollection termSetColl = group.TermSets;
                                foreach (TermSet termSet in termSetColl)
                                {
                                    if (termSet.Name == "Palavra Chave")
                                    {
                                        //termSet.CreateTerm("Teste1", 1033);
                                        palavraChave = termSet;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                            break;
                    }
                    int linha = 0;
                    if (palavraChave != null)
                    {
                        List<string> distinct = new List<string>();
                        try
                        {
                            Stream s = ResourceCarga.LerDocumentNaBiblioteca(SPContext.Current.Web, "Carga MTC.xlsx");
                            DataTable dt = ResourceCarga.ReadExcel(s);

                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[23].ToString() != "")
                                {
                                    string cel = row[23].ToString().Replace("_x000D_", ";");
                                    foreach (string palavra in cel.Split(';'))
                                    {
                                        if (palavra != "")
                                        {
                                            if (!distinct.Contains(palavra))
                                            {
                                                palavraChave.CreateTerm(palavra, 1033);
                                                termStore.CommitAll();
                                                distinct.Add(palavra);
                                            }
                                        }
                                    }

                                }
                                linha++;
                            }
                        }
                        catch (Exception ex)
                        {
                            ResourceCarga.WriteLog(SPContext.Current.Web, "Carregar Palavra Chave", ex.Message+linha);
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Erro ao processar Palavra Chave');", true);
                        }

                        //termStore.CommitAll();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Carga efetuada com sucesso!');", true);
                    }
                }
            }
            catch(Exception ex) {
                ResourceCarga.WriteLog(SPContext.Current.Web, "Carregar Palavra Chave", ex.Message);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Erro ao processar Palavra Chave');", true);
            }
        }
    }
}
