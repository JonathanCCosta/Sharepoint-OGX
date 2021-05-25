using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using System.IO;
using Furnas.OGX2.MTC.ServicesMTC;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace Furnas.OGX2.MTC.ER_MTC.ER_ListaControle
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class ER_ListaControle : SPItemEventReceiver
    {
        /// <summary>
        /// An item was added.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);

            base.EventFiringEnabled = false;

            if (properties.ListItem.ContentType.Name == "Legado")
            {
                string codigo = ServiceManual.GeraCodigoManualMTCAdd(properties);
                properties.ListItem["CodigoMTC"] = codigo;
            }
            else
                properties.ListItem["Revisão"] = 0;

            //Copia Anexo
            string fileName = properties.Web.GetFile(properties.ListItem.Attachments[0]).Name;
            SPFile files = properties.Web.GetFile(properties.ListItem.Attachments.UrlPrefix + fileName);
            SPDocumentLibrary library = properties.Web.Lists["Documentos Manual Técnico de Campo"] as SPDocumentLibrary;
            string nameFileNovo = Path.GetFileNameWithoutExtension(fileName) + "_" + properties.ListItem.ID + Path.GetExtension(fileName);
            string destURL = library.RootFolder.Url + "/" + nameFileNovo;
            byte[] b = files.OpenBinary();
            
            //atualiza Lookup do documento
            SPFile itemFile = library.RootFolder.Files.Add(destURL, b, true);
            SPListItem itemProp = itemFile.Item;
            itemProp["ManualTecnico"] = new SPFieldLookupValue(properties.ListItem.ID, properties.ListItem[SPBuiltInFieldId.Title].ToString());
            itemProp.Update();

            //Delete anexo
            properties.ListItem.Attachments.Delete(fileName);
            properties.ListItem.Update();

            base.EventFiringEnabled = true;

            if (properties.ListItem.ContentType.Name != "Legado")
            {
                ServicesMTC.EmailMTC.EmailAguardando(properties.ListItem);

                PermissaoMTC.PermissaoAguardando(properties.ListItem);
            }
            else
            {
                PermissaoMTC.PermissaoVigente(properties.ListItem);
            }
        }

        private static bool comenta = false;
        /// <summary>
        /// An item is being updated
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            base.ItemUpdating(properties);

            if (properties.ListItem.ContentType.Name != "Legado")
            {
                try
                {
                    if (new SPFieldLookupValue(ServiceManual.ValidaTextField(properties.AfterProperties["OrgaoResponsavelMTC"])).LookupId != 0 && new SPFieldLookupValue(ServiceManual.ValidaTextField(properties.ListItem["Órgão Responsável"])).LookupId != new SPFieldLookupValue(ServiceManual.ValidaTextField(properties.AfterProperties["OrgaoResponsavelMTC"])).LookupId
                        && (ServicesMTC.ServiceManual.ValidaTextField(properties.AfterProperties["ControleDeStatusMTC"]) != "Vigente" && ServicesMTC.ServiceManual.ValidaTextField(properties.AfterProperties["ControleDeStatusMTC"]) != "Cancelado"))
                    {
                        PermissaoMTC.PermissaoTrocaOrgao(properties.ListItem, properties);
                    }
                }
                catch { }

                try
                {
                    if (ServiceManual.ValidaTextField(properties.AfterProperties["ControleDeStatusMTC"]) == "Cancelado")
                    {
                        EmailMTC.EmailCancelado(properties.ListItem, ServiceManual.ValidaTextField(properties.AfterProperties["JustificativaCancelamento"]));

                        PermissaoMTC.PermissaoCancelado(properties.ListItem);
                    }
                    else if (ServiceManual.ValidaTextField(properties.ListItem["ControleDeStatusMTC"]) != "Para comentários" && ServiceManual.ValidaTextField(properties.AfterProperties["ControleDeStatusMTC"]) == "Para comentários")
                    {
                        EmailMTC.EmailParaComentarios(properties.ListItem, properties.AfterProperties["DescricaoMTC"].ToString(), properties.AfterProperties["DataLimiteComentario"].ToString());

                        PermissaoMTC.PermissaoParaComentarios(properties.ListItem);
                    }
                    else if (ServiceManual.ValidaTextField(properties.AfterProperties["ControleDeStatusMTC"]) == "Em consolidação")
                    {
                        EmailMTC.EmailEmConsolidacao(properties.ListItem);
                        PermissaoMTC.PermissaoEmConsolidacao(properties.ListItem, properties);
                    }
                    else if (ServiceManual.ValidaTextField(properties.ListItem["ControleDeStatusMTC"]) != "Em revisão" && ServiceManual.ValidaTextField(properties.AfterProperties["ControleDeStatusMTC"]) == "Em revisão")
                    {
                        EmailMTC.EmailRevisao(properties.ListItem, properties.AfterProperties["DescricaoMTC"].ToString());

                    }
                    else if (ServiceManual.ValidaTextField(properties.ListItem["ControleDeStatusMTC"]) == "Em consolidação" && ServiceManual.ValidaTextField(properties.AfterProperties["ControleDeStatusMTC"]) == "Aguardando Aprovação")
                    {
                        comenta = true;
                    }

                }
                catch { }
            }
        }

        /// <summary>
        /// An item was updated
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);

            if (properties.ListItem.ContentType.Name != "Legado")
            {
                if (ServiceManual.ValidaTextField(properties.ListItem["ControleDeStatusMTC"]) == "Vigente")
                {
                    if (properties.ListItem.Attachments.Count > 0)
                    {
                        base.EventFiringEnabled = false;

                        //Pega Documento existente
                        SPDocumentLibrary library = properties.Web.Lists["Documentos Manual Técnico de Campo"] as SPDocumentLibrary;
                        SPListItem itemlibrary = library.Items.OfType<SPListItem>().Where(p =>
                            new SPFieldLookupValue(Convert.ToString(p["Manual Técnico de Campo"])).LookupId == properties.ListItem.ID).First();

                        //Copia Anexo
                        string fileName = properties.Web.GetFile(properties.ListItem.Attachments[0]).Name;
                        SPFile files = properties.Web.GetFile(properties.ListItem.Attachments.UrlPrefix + fileName);
                        //SPDocumentLibrary library = properties.Web.Lists["Documentos Manual Técnico de Campo"] as SPDocumentLibrary;
                        string nameFileNovo = Path.GetFileNameWithoutExtension(fileName) + "_" + properties.ListItem.ID + Path.GetExtension(fileName);
                        string destURL = library.RootFolder.Url + "/" + nameFileNovo;
                        byte[] b = files.OpenBinary();

                        if (itemlibrary.Name != nameFileNovo)
                        {
                            try
                            {
                                SPFile itemFile = library.RootFolder.Files.Add(destURL, b, false);
                                SPListItem itemProp = itemFile.Item;
                                itemProp["ManualTecnico"] = new SPFieldLookupValue(properties.ListItem.ID, properties.ListItem[SPBuiltInFieldId.Title].ToString());
                                itemProp.Update();

                                itemlibrary.Delete();
                            }
                            catch { }
                        }
                        else
                        {
                            SPFile itemFile = library.RootFolder.Files.Add(destURL, b, true);
                            SPListItem itemProp = itemFile.Item;
                            itemProp["ManualTecnico"] = new SPFieldLookupValue(properties.ListItem.ID, properties.ListItem[SPBuiltInFieldId.Title].ToString());
                            itemProp.Update();
                        }

                        //Delete anexo
                        properties.ListItem.Attachments.Delete(fileName);
                        if (ServiceManual.ValidaTextField(properties.ListItem["EmRevisao"]) == "Não")
                        {
                            string codigo = ServiceManual.GeraCodigoManualMTCAdd(properties);
                            properties.ListItem["CodigoMTC"] = codigo;
                            properties.ListItem.Update();
                        }
                        else
                            properties.ListItem.Update();

                        base.EventFiringEnabled = true;

                        EmailMTC.EmailParaVigente(properties.ListItem);
                        PermissaoMTC.PermissaoVigente(properties.ListItem);
                    }
                    else
                    {
                        try
                        {
                            if (ServiceManual.ValidaTextField(properties.ListItem["EmRevisao"]) == "Não")
                            {
                                base.EventFiringEnabled = false;

                                string codigo = ServiceManual.GeraCodigoManualMTCAdd(properties);
                                properties.ListItem["CodigoMTC"] = codigo;
                                properties.ListItem.Update();

                                base.EventFiringEnabled = true;
                            }

                            EmailMTC.EmailParaVigente(properties.ListItem);
                            PermissaoMTC.PermissaoVigente(properties.ListItem);
                        }
                        catch { }
                    }

                    ServiceManual.VerificaHistorico(properties.ListItem);

                }
                else if (ServiceManual.ValidaTextField(properties.ListItem["ControleDeStatusMTC"]) == "Para comentários" && ServiceManual.ValidaTextField(properties.ListItem["EmRevisao"]) == "Sim")
                {
                    if (properties.ListItem.Attachments.Count > 0)
                    {
                        base.EventFiringEnabled = false;

                        //Pega Documento existente
                        SPDocumentLibrary library = properties.Web.Lists["Documentos Manual Técnico de Campo"] as SPDocumentLibrary;
                        SPListItem itemlibrary = library.Items.OfType<SPListItem>().Where(p =>
                            new SPFieldLookupValue(Convert.ToString(p["Manual Técnico de Campo"])).LookupId == properties.ListItem.ID).First();

                        //Copia Anexo
                        string fileName = properties.Web.GetFile(properties.ListItem.Attachments[0]).Name;
                        SPFile files = properties.Web.GetFile(properties.ListItem.Attachments.UrlPrefix + fileName);
                        //SPDocumentLibrary library = properties.Web.Lists["Documentos Manual Técnico de Campo"] as SPDocumentLibrary;
                        string nameFileNovo = Path.GetFileNameWithoutExtension(fileName) + "_" + properties.ListItem.ID + Path.GetExtension(fileName);
                        string destURL = library.RootFolder.Url + "/" + nameFileNovo;
                        byte[] b = files.OpenBinary();

                        if (itemlibrary.Name != nameFileNovo)
                        {
                            try
                            {
                                SPFile itemFile = library.RootFolder.Files.Add(destURL, b, false);
                                SPListItem itemProp = itemFile.Item;
                                itemProp["ManualTecnico"] = new SPFieldLookupValue(properties.ListItem.ID, properties.ListItem[SPBuiltInFieldId.Title].ToString());
                                itemProp.Update();

                                itemlibrary.Delete();
                            }
                            catch { }
                        }
                        else
                        {
                            SPFile itemFile = library.RootFolder.Files.Add(destURL, b, true);
                            SPListItem itemProp = itemFile.Item;
                            itemProp["ManualTecnico"] = new SPFieldLookupValue(properties.ListItem.ID, properties.ListItem[SPBuiltInFieldId.Title].ToString());
                            itemProp.Update();
                        }

                        //Delete anexo
                        properties.ListItem.Attachments.Delete(fileName);
                        properties.ListItem.Update();

                        base.EventFiringEnabled = true;
                    }
                }
                else if (ServiceManual.ValidaTextField(properties.ListItem["ControleDeStatusMTC"]) == "Em revisão")
                {
                    if (properties.ListItem.Attachments.Count > 0)
                    {
                        base.EventFiringEnabled = false;

                        //Pega Documento existente
                        SPDocumentLibrary library = properties.Web.Lists["Documentos Manual Técnico de Campo"] as SPDocumentLibrary;
                        SPListItem itemlibrary = library.Items.OfType<SPListItem>().Where(p =>
                            new SPFieldLookupValue(Convert.ToString(p["Manual Técnico de Campo"])).LookupId == properties.ListItem.ID).First();

                        //Copia Anexo
                        string fileName = properties.Web.GetFile(properties.ListItem.Attachments[0]).Name;
                        SPFile files = properties.Web.GetFile(properties.ListItem.Attachments.UrlPrefix + fileName);
                        //SPDocumentLibrary library = properties.Web.Lists["Documentos Manual Técnico de Campo"] as SPDocumentLibrary;
                        string nameFileNovo = Path.GetFileNameWithoutExtension(fileName) + "_" + properties.ListItem.ID + Path.GetExtension(fileName);
                        string destURL = library.RootFolder.Url + "/" + nameFileNovo;
                        byte[] b = files.OpenBinary();

                        if (itemlibrary.Name != nameFileNovo)
                        {
                            try
                            {
                                SPFile itemFile = library.RootFolder.Files.Add(destURL, b, false);
                                SPListItem itemProp = itemFile.Item;
                                itemProp["ManualTecnico"] = new SPFieldLookupValue(properties.ListItem.ID, properties.ListItem[SPBuiltInFieldId.Title].ToString());
                                itemProp.Update();

                                itemlibrary.Delete();
                            }
                            catch { }
                        }
                        else
                        {
                            SPFile itemFile = library.RootFolder.Files.Add(destURL, b, true);
                            SPListItem itemProp = itemFile.Item;
                            itemProp["ManualTecnico"] = new SPFieldLookupValue(properties.ListItem.ID, properties.ListItem[SPBuiltInFieldId.Title].ToString());
                            itemProp.Update();
                        }

                        //Delete anexo
                        properties.ListItem.Attachments.Delete(fileName);

                        properties.ListItem["Controle de Status"] = "Aguardando Aprovação";
                        properties.ListItem.Update();

                        base.EventFiringEnabled = true;

                        try
                        {
                            EmailMTC.EmailAguardando(properties.ListItem);
                            PermissaoMTC.PermissaoAguardandoUpdate(properties.ListItem);
                        }
                        catch { }
                    }
                }
                else if (ServiceManual.ValidaTextField(properties.ListItem["ControleDeStatusMTC"]) == "Aguardando Aprovação")
                {

                    if (properties.ListItem.Attachments.Count > 0)
                    {
                        base.EventFiringEnabled = false;

                        //Pega Documento existente
                        SPDocumentLibrary library = properties.Web.Lists["Documentos Manual Técnico de Campo"] as SPDocumentLibrary;
                        SPListItem itemlibrary = library.Items.OfType<SPListItem>().Where(p =>
                            new SPFieldLookupValue(Convert.ToString(p["Manual Técnico de Campo"])).LookupId == properties.ListItem.ID).First();

                        //Copia Anexo
                        string fileName = properties.Web.GetFile(properties.ListItem.Attachments[0]).Name;
                        SPFile files = properties.Web.GetFile(properties.ListItem.Attachments.UrlPrefix + fileName);
                        //SPDocumentLibrary library = properties.Web.Lists["Documentos Manual Técnico de Campo"] as SPDocumentLibrary;
                        string nameFileNovo = Path.GetFileNameWithoutExtension(fileName) + "_" + properties.ListItem.ID + Path.GetExtension(fileName);
                        string destURL = library.RootFolder.Url + "/" + nameFileNovo;
                        byte[] b = files.OpenBinary();

                        if (itemlibrary.Name != nameFileNovo)
                        {
                            try
                            {
                                SPFile itemFile = library.RootFolder.Files.Add(destURL, b, false);
                                SPListItem itemProp = itemFile.Item;
                                itemProp["ManualTecnico"] = new SPFieldLookupValue(properties.ListItem.ID, properties.ListItem[SPBuiltInFieldId.Title].ToString());
                                itemProp.Update();

                                itemlibrary.Delete();
                            }
                            catch { }
                        }
                        else
                        {
                            SPFile itemFile = library.RootFolder.Files.Add(destURL, b, true);
                            SPListItem itemProp = itemFile.Item;
                            itemProp["ManualTecnico"] = new SPFieldLookupValue(properties.ListItem.ID, properties.ListItem[SPBuiltInFieldId.Title].ToString());
                            itemProp.Update();
                        }

                        //Delete anexo
                        properties.ListItem.Attachments.Delete(fileName);
                        properties.ListItem.Update();

                        base.EventFiringEnabled = true;
                    }
                    ServicesMTC.EmailMTC.EmailAguardando(properties.ListItem);
                    PermissaoMTC.PermissaoAguardandoUpdate(properties.ListItem);
                }

                if (comenta)
                {
                    ServiceManual.ComentarioConsolidacao(properties.ListItem);
                    comenta = false;
                }
            }
            else
            {
                if (properties.ListItem.Attachments.Count > 0)
                {
                    base.EventFiringEnabled = false;

                    //Pega Documento existente
                    SPDocumentLibrary library = properties.Web.Lists["Documentos Manual Técnico de Campo"] as SPDocumentLibrary;
                    SPListItem itemlibrary = library.Items.OfType<SPListItem>().Where(p =>
                        new SPFieldLookupValue(Convert.ToString(p["Manual Técnico de Campo"])).LookupId == properties.ListItem.ID).First();

                    //Copia Anexo
                    string fileName = properties.Web.GetFile(properties.ListItem.Attachments[0]).Name;
                    SPFile files = properties.Web.GetFile(properties.ListItem.Attachments.UrlPrefix + fileName);
                    //SPDocumentLibrary library = properties.Web.Lists["Documentos Manual Técnico de Campo"] as SPDocumentLibrary;
                    string nameFileNovo = Path.GetFileNameWithoutExtension(fileName) + "_" + properties.ListItem.ID + Path.GetExtension(fileName);
                    string destURL = library.RootFolder.Url + "/" + nameFileNovo;
                    byte[] b = files.OpenBinary();

                    if (itemlibrary.Name != nameFileNovo)
                    {
                        try
                        {
                            SPFile itemFile = library.RootFolder.Files.Add(destURL, b, false);
                            SPListItem itemProp = itemFile.Item;
                            itemProp["ManualTecnico"] = new SPFieldLookupValue(properties.ListItem.ID, properties.ListItem[SPBuiltInFieldId.Title].ToString());
                            itemProp.Update();

                            itemlibrary.Delete();
                        }
                        catch { }
                    }
                    else
                    {
                        SPFile itemFile = library.RootFolder.Files.Add(destURL, b, true);
                        SPListItem itemProp = itemFile.Item;
                        itemProp["ManualTecnico"] = new SPFieldLookupValue(properties.ListItem.ID, properties.ListItem[SPBuiltInFieldId.Title].ToString());
                        itemProp.Update();
                    }

                    //Delete anexo
                    properties.ListItem.Attachments.Delete(fileName);
                    properties.ListItem.Update();

                    base.EventFiringEnabled = true;
                }
            }
        }

        /// <summary>
        /// An item is being deleted
        /// </summary>
        public override void ItemDeleting(SPItemEventProperties properties)
        {
            base.ItemDeleting(properties);

            SPDocumentLibrary library = properties.Web.Lists["Documentos Manual Técnico de Campo"] as SPDocumentLibrary;
            List<SPListItem> itemlibrary = library.Items.OfType<SPListItem>().Where(p =>
                new SPFieldLookupValue(Convert.ToString(p["Manual Técnico de Campo"])).LookupId == properties.ListItem.ID).ToList();

            int Items = itemlibrary.Count;
            for (int i = Items; i > 0; i--)
            {
                itemlibrary[0].Delete();
            }
            //itemlibrary.Delete();
        }

        /// <summary>
        /// An item is being added
        /// </summary>
        public override void ItemAdding(SPItemEventProperties properties)
        {
            base.ItemAdding(properties);
            try
            {
                if (properties.AfterProperties["ContentType"].ToString() == "Legado")
                {
                    string cod = ServiceManual.GeraCodigoManualMTCAddAfter(properties);

                    SPListItem item = properties.Web.Lists["Manual Técnico de Campo"].Items.OfType<SPListItem>().Where(p => Convert.ToString(p["CodigoMTC"]) == cod && Convert.ToString(p["ControleDeStatusMTC"]) != "Cancelado").FirstOrDefault();
                    if(item != null)
                        throw new Exception("Este valor já existe na lista.");
                }
            }
            catch (Exception err)
            {
                properties.ErrorMessage = err.Message;
                properties.Status = SPEventReceiverStatus.CancelWithError;
            }
        }

    }
}