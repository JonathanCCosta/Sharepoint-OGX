using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using Furnas.OGX2.Chamados.ServiceChamados;

namespace Furnas.OGX2.Chamados.ER_Chamados.ER_Chamados
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class ER_Chamados : SPItemEventReceiver
    {
        /// <summary>
        /// An item was added.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);

            Suporte.Email_1(properties.ListItem);

            try
            {
                PermissaoChamado.PermissaoAbreChamado(properties.ListItem);
            }
            catch (Exception ex) { Suporte.Log(properties.ListItem.Web, ex.Message, "Permissão Chamado - Abertura"); }

            try
            {
                SPListItem item = properties.Web.Lists["Contador Chamado"].GetItemById(1);
                int cont = Convert.ToInt32(item[SPBuiltInFieldId.Title].ToString()) +1;
                item[SPBuiltInFieldId.Title] = cont.ToString();
                item.Update();
            }
            catch (Exception ex) { Suporte.Log(properties.ListItem.Web, ex.Message, "Contador Chamado"); }
            #region Antiga Logica
            /*if (Suporte.ValidaTextField(properties.ListItem["Responsável"]) != string.Empty)
            {
                PermissaoChamado.PermissaoResponsavelAdded(properties.ListItem);
                if (Suporte.ValidaTextField(properties.ListItem["Enviar email ao responsável"]) == "Sim")
                    Suporte.EmailResp_1(properties.ListItem);
            }*/
            #endregion

        }

        /// <summary>
        /// An item is being updated
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            base.ItemUpdating(properties);

            /*if (Suporte.ValidaTextField(properties.ListItem["Controle de status"]) != Suporte.ValidaTextField(properties.AfterProperties["ControleDeStatus"]))
            {
                if (Suporte.ValidaTextField(properties.AfterProperties["ControleDeStatus"]) == "Em andamento")
                {
                    PermissaoChamado.PermissaoChamadoEmAndamento(properties.ListItem);
                    //Suporte.Email_1(properties.ListItem);
                }
            }*/

            properties.Web.AllowUnsafeUpdates = true;

            base.EventFiringEnabled = false;

            SPFieldUserValue resp = null;
            SPUser NovoResponsavel = null;string respID="";
            try
            {
                if (Suporte.ValidaTextField(properties.ListItem["Responsavel"]) != string.Empty)
                {
                    //resp = properties.Web.EnsureUser(Suporte.ValidaTextField(properties.ListItem["Responsavel"]));
                    resp = new SPFieldUserValue(properties.ListItem.Web, properties.ListItem["Responsavel"].ToString());
                }

                if (Suporte.ValidaTextField(properties.AfterProperties["Responsavel"]) != string.Empty)
                {
                    //NovoResponsavel = properties.Web.EnsureUser(Suporte.ValidaTextField(properties.AfterProperties["Responsavel"]));
                    if (Suporte.ValidaTextField(properties.AfterProperties["Responsavel"]).Length > 2)
                    {
                        string respCorp = properties.AfterProperties["Responsavel"].ToString().Split('|')[1];
                        NovoResponsavel = properties.Web.EnsureUser(respCorp);
                    }
                    else if (Suporte.ValidaTextField(properties.AfterProperties["Responsavel"]).Length > 0)
                        respID = Suporte.ValidaTextField(properties.AfterProperties["Responsavel"]);

                }
            }
            catch { }

            if (resp != null && NovoResponsavel != null)
            {
                if (resp.User.ID != NovoResponsavel.ID)
                {
                    //if (Suporte.ValidaTextField(properties.AfterProperties["Responsavel"]) != string.Empty)
                    //{
                        try
                        {   
                            Suporte.Email_10(properties.ListItem, NovoResponsavel);
                           
                            PermissaoChamado.PermissaoTrocaResponsavelUpdated(properties, NovoResponsavel);   
                        }
                        catch (Exception ex) { Suporte.Log(properties.ListItem.Web, ex.Message, "Permissão Chamado - Troca de Responsavel"); }
                    //}
                }
            }
            else if (NovoResponsavel != null)
            {
                Suporte.Email_10(properties.ListItem, NovoResponsavel);

              
                PermissaoChamado.PermissaoTrocaResponsavelUpdated(properties, NovoResponsavel);
            }
            else if (resp != null)
            {
                
                if(resp.User.ID.ToString() != respID) //if(NovoResponsavel == null)
                    
                    PermissaoChamado.PermissaoTrocaResponsavelUpdated(properties, NovoResponsavel);
            }
                


            if (Suporte.ValidaTextField(properties.ListItem["ControleDeStatus"]) != Suporte.ValidaTextField(properties.AfterProperties["ControleDeStatus"]))
            {
                if (Suporte.ValidaTextField(properties.AfterProperties["ControleDeStatus"]) == "Encerrado")
                {
                    //email de encerramento
                    Suporte.Email_9(properties.ListItem, Suporte.ValidaTextField(properties.AfterProperties["Comentario"]));
                    //Retira permissão de editor de geral
                    try
                    {
                        PermissaoChamado.PermissaoEncerra(properties);
                    }
                    catch (Exception ex) { Suporte.Log(properties.ListItem.Web, ex.Message, "Permissão Chamado - Encerramento"); }
                }
                else if (Suporte.ValidaTextField(properties.AfterProperties["ControleDeStatus"]) == "Aguardando informações")
                {
                    try
                    {
                        Suporte.Email_5(properties.ListItem, "Aguardando informações", Suporte.ValidaTextField(properties.AfterProperties["Esclarecimentos"]));
                        
                        
                        PermissaoChamado.PermissaoChamadoInformacao(properties.ListItem);
                    }
                    catch (Exception ex) { Suporte.Log(properties.ListItem.Web, ex.Message, "Permissão Chamado - Email_5"); }

                }
                /*else if (Suporte.ValidaTextField(properties.AfterProperties["ControleDeStatus"]) == "Encaminhado à outra gerência")
                {
                    try
                    {
                        string emailsUsers = string.Empty;
                        SPFieldUserValueCollection usersGerencia = new SPFieldUserValueCollection();

                        SPListItem itemGerencia = properties.Web.Lists["Gerência Responsável"].GetItemById(new SPFieldLookupValue(properties.AfterProperties["Gerência Responsável"].ToString()).LookupId);
                        usersGerencia = new SPFieldUserValueCollection(properties.Web, Convert.ToString(itemGerencia["Responsáveis"]));
                        int total = 1;
                        foreach (SPFieldUserValue userGerencia in usersGerencia)
                        {
                            if (usersGerencia.Count == total)
                                emailsUsers += userGerencia.User.Email;
                            else
                                emailsUsers += userGerencia.User.Email + ";";

                            total++;
                        }

                        foreach (string email in Convert.ToString(itemGerencia["Gerente Imediato"]).Split(';'))
                        {
                            SPUser user = properties.Web.SiteUsers.GetByEmail(email);
                            usersGerencia.Add(new SPFieldUserValue(properties.Web, user.ID, user.LoginName));
                        }

                        Suporte.Email_8(properties.ListItem, "Encaminhado à outra gerência", usersGerencia, properties.AfterProperties["Esclarecimentos Gerência"].ToString());
                        PermissaoChamado.PermissaoChamadoOutraGerencia(properties.ListItem, usersGerencia);
                    }
                    catch (Exception ex) { Suporte.Log(properties.ListItem.Web, ex.Message, "Permissão Chamado - Email_8"); }

                }*/
                else if (Suporte.ValidaTextField(properties.AfterProperties["ControleDeStatus"]) == "Em andamento" && Suporte.ValidaTextField(properties.ListItem["ControleDeStatus"]) == "Aguardando informações")
                {
                    try
                    {
                        Suporte.Email_7(properties.ListItem, Suporte.ValidaTextField(properties.AfterProperties["Esclarecimentos"]), properties.UserDisplayName);
                        PermissaoChamado.PermissaoChamadoEmAndamento(properties.ListItem);
                    }
                    catch (Exception ex) { Suporte.Log(properties.ListItem.Web, ex.Message, "Permissão Chamado - Email_7"); }
                }
                else if (Suporte.ValidaTextField(properties.AfterProperties["ControleDeStatus"]) == "Em andamento" && Suporte.ValidaTextField(properties.ListItem["ControleDeStatus"]) == "Encaminhado à outra gerência da DO")
                {
                    try
                    {
                        Suporte.Email_7(properties.ListItem, properties.AfterProperties["EsclarecimentosGerencia"].ToString(), properties.UserDisplayName);
                        PermissaoChamado.PermissaoChamadoEmAndamentoPosGerencia(properties.ListItem);
                    }
                    catch (Exception ex) { Suporte.Log(properties.ListItem.Web, ex.Message, "Permissão Chamado - Email_7"); }
                }
                else if (Suporte.ValidaTextField(properties.AfterProperties["ControleDeStatus"]) == "Em andamento")
                {
                    try
                    {
                        PermissaoChamado.PermissaoChamadoEmAndamento(properties.ListItem);
                    }
                    catch (Exception ex) { Suporte.Log(properties.ListItem.Web, ex.Message, "Permissão Chamado - Email_5"); }

                }
            }

            if (Suporte.ValidaTextField(properties.AfterProperties["ControleDeStatus"]) == "Encaminhado à outra gerência da DO")//Troca de gerencia
            {
                try
                {
                    if (Suporte.ValidaTextField(properties.ListItem["GerenciaResponsavel"]) != Suporte.ValidaTextField(properties.AfterProperties["GerenciaResponsavel"]))
                    {
                        //Item antiga gerencia
                        SPListItem itemGerencia = properties.Web.Lists["Gerência Responsável"].GetItemById(new SPFieldLookupValue(Suporte.ValidaTextField(properties.ListItem["GerenciaResponsavel"])).LookupId);
                        SPFieldUserValueCollection usersGerencia = new SPFieldUserValueCollection(properties.Web, Suporte.ValidaTextField(itemGerencia["Responsáveis"]));

                        foreach (string email in Suporte.ValidaTextField(itemGerencia["Gerente Imediato"]).Split(';'))
                        {
                            if (email != "" && email != "null")
                            {
                                //string u = SPUtility.GetLoginNameFromEmail(properties.Web.Site, email);
                                //SPPrincipalInfo u = SPUtility.ResolvePrincipal(properties.Web, email, SPPrincipalType.All, SPPrincipalSource.All, null, true);

                                SPUser user = properties.Web.SiteUsers.GetByEmail(email);
                                //SPUser user = properties.Web.EnsureUser(u); //properties.Web.SiteUsers.GetByEmail(email);
                                usersGerencia.Add(new SPFieldUserValue(properties.Web, user.ID, user.LoginName));
                            }
                        }

                        if (usersGerencia.Count > 0)
                        {
                            PermissaoChamado.TrocaGerenciaResponsavel(usersGerencia, properties.ListItem);
                        }
                    }
                }
                catch { }
            }
            properties.Web.AllowUnsafeUpdates = false;

            base.EventFiringEnabled = true;
        }

        /// <summary>
        /// An item was updated
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);

            if (Suporte.ValidaTextField(properties.ListItem["ControleDeStatus"]) == "Encaminhado à outra gerência da DO")
            {
                try
                {
                    string emailsUsers = string.Empty;
                    SPFieldUserValueCollection usersGerencia = new SPFieldUserValueCollection();

                    SPListItem itemGerencia = properties.Web.Lists["Gerência Responsável"].GetItemById(new SPFieldLookupValue(properties.ListItem["Gerência Responsável"].ToString()).LookupId);
                    usersGerencia = new SPFieldUserValueCollection(properties.Web, Convert.ToString(itemGerencia["Responsáveis"]));
                    //int total = 1;
                    foreach (SPFieldUserValue userGerencia in usersGerencia)
                    {
                       
                            /*if (usersGerencia.Count == total)
                                emailsUsers += userGerencia.User.Email;
                            else*/
                            emailsUsers += userGerencia.User.Email + ";";

                        

                       // total++;
                    }

                    foreach (string email in Convert.ToString(itemGerencia["Gerente Imediato"]).Split(';'))
                    {
                        if (email != "" && email != "null")
                        {
                            emailsUsers += email + ";";
                            //string u = SPUtility.GetLoginNameFromEmail(properties.Web.Site, email);

                            SPUser user = properties.Web.SiteUsers.GetByEmail(email);
                            
                            //Erro no null SPRINT 7
                            //SPUser user = properties.Web.EnsureUser(u); //properties.Web.SiteUsers.GetByEmail(email);
                            usersGerencia.Add(new SPFieldUserValue(properties.Web, user.ID, user.LoginName));
                        }
                    }
                    //Suporte.Log(properties.ListItem.Web, emailsUsers, "Permissão Chamado - Email_8");
                    Suporte.Email_8(properties.ListItem, "Encaminhado à outra gerência da DO", emailsUsers, properties.ListItem["Esclarecimentos Gerência"].ToString());
                    PermissaoChamado.PermissaoChamadoOutraGerencia(properties.ListItem, usersGerencia);
                }
                catch (Exception ex) { Suporte.Log(properties.ListItem.Web, ex.Message, "Permissão Chamado - Email_8"); }
            }

            /*if (Suporte.ValidaTextField(properties.AfterProperties["ControleDeStatus"]) == "Encerrado")
            {
                PermissaoChamado.PermissaoEncerra(properties);
            }
            else
            {
                if (Suporte.ValidaTextField(properties.ListItem["Responsavel"]) != string.Empty)
                {
                    PermissaoChamado.PermissaoTrocaResponsavelUpdating(properties);
                    if (Suporte.ValidaTextField(properties.ListItem["Enviar email ao responsável"]) == "Sim") { }
                        //Suporte.EmailResp_1(properties.ListItem);
                }*
            }*/
        }
    }
}