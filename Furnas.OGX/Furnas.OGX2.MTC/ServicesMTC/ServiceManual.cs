using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furnas.OGX2.MTC.ServicesMTC
{
    public class ServiceManual
    {
        public static string ValidaTextField(object fieldvalue)
        {
            return (fieldvalue != null) ? Convert.ToString(fieldvalue) : string.Empty;
        }

        public static void ComentarioConsolidacao(SPListItem item)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(item.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                SPListItem itemN = ImpersonatedWeb.Lists["Manual Técnico de Campo"].GetItemById(item.ID);

                EventFiring eventFiring = new EventFiring();
                eventFiring.DisableHandleEventFiring();

                itemN["Comentários"] = "Novo ciclo de aprovação";
                itemN["Existe Comentários"] = "N";
                itemN.Update();

                eventFiring.EnableHandleEventFiring();

                ImpersonatedWeb.Close(); ImpersonatedWeb.Dispose();
                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();
            });
        }

        public static string GeraCodigoManualMTCAdd(SPItemEventProperties properties)
        {

            StringBuilder codigo = new StringBuilder();

            if (properties.ListItem["TipoMTC"] != null)
            {
                //codigo.Append(new SPFieldLookupValue(properties.ListItem["TipoMTC"].ToString()).LookupValue + ".");
                codigo.Append(RetornaCod("Tipo MTC", "Codigo", properties.ListItem, "TipoMTC") + ".");

            }

            if (properties.ListItem["Grupo"] != null)
            {
                //codigo.Append(new SPFieldLookupValue(properties.ListItem["Grupo"].ToString()).LookupValue);
                codigo.Append(RetornaCod("Grupo MTC", "CodigoGrupoMTC", properties.ListItem, "Grupo"));

            }

            if (properties.ListItem["Subgrupo"] != null)
            {
                //codigo.Append(new SPFieldLookupValue(properties.ListItem["Subgrupo"].ToString()).LookupValue + ".");
                codigo.Append(RetornaCod("Subgrupo MTC", "Codigo", properties.ListItem, "Subgrupo"));
            }

            if (properties.ListItem["Fabricante"] != null)
            {
                codigo.Append(RetornaCod("Fabricante MTC", "Codigo", properties.ListItem, "Fabricante") + ".00/");
                //codigo.Append(new SPFieldLookupValue(properties.ListItem["Fabricante"].ToString()).LookupValue + ".00/");
            }

            if (properties.ListItem["Consecutivo"] != null)
            {
                //consecutivo = properties.ListItem["Consecutivo"].ToString();
                codigo.Append(properties.ListItem["Consecutivo"].ToString() + "-");
            }

            if (properties.ListItem["Revisao"] != null)
            {
                //revisao = "R" + properties.ListItem["Revisao"].ToString();
                codigo.Append("R" + properties.ListItem["Revisao"].ToString());
            }
            else
                codigo.Append("R0");

            return codigo.ToString();
        }

        public static string RetornaCod(string list, string campoRetorno, SPListItem item, string campoConsulta)
        {
            SPListItem it = item.Web.Lists[list].GetItemById(new SPFieldLookupValue(item[campoConsulta].ToString()).LookupId);
            return it[campoRetorno].ToString();
        }

        public static void VerificaHistorico(SPListItem item)
        {
            try
            {
                if (ValidaTextField(item["Revisão"]) != "0")
                {
                    string codigoAtual = ValidaTextField(item["CodigoMTC"]);
                    int revisaoAnterior = Convert.ToInt32(item["Revisão"].ToString());
                    string codigo = codigoAtual.Remove(codigoAtual.Length - 1) + (revisaoAnterior - 1).ToString();

                    SPListItem itens = item.Web.Lists["Manual Técnico de Campo"].Items.OfType<SPListItem>().Where(p => Convert.ToString(p["CodigoMTC"]) == codigo).FirstOrDefault();

                    if (itens != null)
                    {
                        MudaStatus(itens);
                    }

                }
            }
            catch { }
        }

        public static void MudaStatus(SPListItem item)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(item.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                SPListItem itemN = ImpersonatedWeb.Lists["Manual Técnico de Campo"].GetItemById(item.ID);

                EventFiring eventFiring = new EventFiring();
                eventFiring.DisableHandleEventFiring();

                itemN["Controle de Status"] = "Histórico";
                itemN.Update();

                eventFiring.EnableHandleEventFiring();

                ImpersonatedWeb.Close(); ImpersonatedWeb.Dispose();
                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();
            });
        }

        public static bool TemRevisaoAtiva(SPWeb web, string id)
        {
            SPListItem MTC = web.Lists.TryGetList("Manual Técnico de Campo").GetItemById(Convert.ToInt32(id));
            
            string codigoAtual = ValidaTextField(MTC["CodigoMTC"]);
            int revisaoAnterior = Convert.ToInt32(MTC["Revisão"].ToString());
            string codigo = codigoAtual.Remove(codigoAtual.Length - 1) + (revisaoAnterior + 1).ToString();

            try
            {
                SPListItem itens = web.Lists["Manual Técnico de Campo"].Items.OfType<SPListItem>().Where(p => ValidaTextField(p["CodigoMTC"]) == codigo && ValidaTextField(p["Controle de Status"]) == "Em revisão").FirstOrDefault();
                if (itens != null)
                    return true;
                else
                    return false;
            }
            catch { return false; }
        }

        public static SPListItem CopyItemRevisao(SPWeb web, string id)
        {
            SPList sourceList = web.Lists.TryGetList("Manual Técnico de Campo");

            SPListItem MTC = sourceList.GetItemById(Convert.ToInt32(id));

            if (sourceList != null)
            {
                SPContentType ct = sourceList.ContentTypes["Fluxo de solicitação do MTC"];
                SPListItem destItem = sourceList.Items.Add();
                foreach (SPField field in MTC.Fields)
                {
                    if (!field.ReadOnlyField && !field.Hidden && field.InternalName != "Attachments")
                    {
                        if (destItem.Fields.ContainsField(field.InternalName))
                        {
                            if (field.InternalName != "Comentarios" || field.InternalName != "DataLimiteComentario")
                                destItem[field.InternalName] = MTC[field.InternalName];
                        }
                    }
                }
                EventFiring eventFiring = new EventFiring();
                eventFiring.DisableHandleEventFiring();
                
                destItem["ControleDeStatusMTC"] = "Em revisão";
                destItem["EmRevisao"] = "Sim";
                destItem["Revisao"] = Convert.ToInt32(MTC["Revisão"].ToString()) +1;
                destItem["ExisteComentarios"] = "N";
                destItem["ContentType"] = ct.Name;
                string codigoAtual = ValidaTextField(MTC["CodigoMTC"]);
                int revisaoAnterior = Convert.ToInt32(MTC["Revisão"].ToString());
                string codigo = codigoAtual.Remove(codigoAtual.Length - 1) + (revisaoAnterior + 1).ToString();
                destItem["CodigoMTC"] = codigo;
                
                destItem.Update();

                eventFiring.EnableHandleEventFiring();

                return destItem;
            }

            return null;
        }

        public static void DocumentoCopy(SPListItem item, string id)
        {
            //Pega Documento existente
            SPDocumentLibrary library = item.Web.Lists["Documentos Manual Técnico de Campo"] as SPDocumentLibrary;
            SPListItem itemlibrary = library.Items.OfType<SPListItem>().Where(p =>
                new SPFieldLookupValue(Convert.ToString(p["Manual Técnico de Campo"])).LookupId == Convert.ToInt32(id)).First();

                        
            string nameFileNovo = Path.GetFileNameWithoutExtension(itemlibrary.Name.Replace(id,"")) + "_" + item.ID + Path.GetExtension(itemlibrary.Name);
            SPFile docNovo = library.RootFolder.Files.Add(library.RootFolder.Url + "/" + nameFileNovo, itemlibrary.File.OpenBinary(), false);
            docNovo.Item["ManualTecnico"] = new SPFieldLookupValue(item.ID, item[SPBuiltInFieldId.Title].ToString());
            docNovo.Item.Update();
        }

        public static string GeraCodigoManualMTCAddAfter(SPItemEventProperties properties)
        {

            StringBuilder codigo = new StringBuilder();
            if (properties.AfterProperties["TipoMTC"] != null)
            {
                codigo.Append(RetornaCodAfter("Tipo MTC", "Codigo", properties, "TipoMTC") + ".");
            }

            if (properties.AfterProperties["Grupo"] != null)
            {
                //codigo.Append(new SPFieldLookupValue(properties.ListItem["Grupo"].ToString()).LookupValue);
                codigo.Append(RetornaCodAfter("Grupo MTC", "CodigoGrupoMTC", properties, "Grupo"));

            }

            if (properties.AfterProperties["Subgrupo"] != null)
            {
                //codigo.Append(new SPFieldLookupValue(properties.ListItem["Subgrupo"].ToString()).LookupValue + ".");
                codigo.Append(RetornaCodAfter("Subgrupo MTC", "Codigo", properties, "Subgrupo"));
            }

            if (properties.AfterProperties["FabricanteMTC"] != null)
            {
                codigo.Append(RetornaCodAfter("Fabricante MTC", "Codigo", properties, "FabricanteMTC") + ".00/");
            }

            if (properties.AfterProperties["Consecutivo"] != null)
            {
                codigo.Append(properties.AfterProperties["Consecutivo"].ToString() + "-");
            }

            if (properties.AfterProperties["Revisao"] != null)
            {
                codigo.Append("R" + properties.AfterProperties["Revisao"].ToString());
            }
            else
                codigo.Append("R0");

            return codigo.ToString();
        }

        public static string RetornaCodAfter(string list, string campoRetorno, SPItemEventProperties item, string campoConsulta)
        {
            SPListItem it = item.Web.Lists[list].GetItemById(new SPFieldLookupValue(item.AfterProperties[campoConsulta].ToString()).LookupId);
            return it[campoRetorno].ToString();
        }
    }
}
