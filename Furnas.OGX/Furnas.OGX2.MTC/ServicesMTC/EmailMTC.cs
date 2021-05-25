using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furnas.OGX2.MTC.ServicesMTC
{
    public class EmailMTC
    {
        public static void EmailAguardando(SPListItem item)
        {
            try
            {
                string emailsUsers = string.Empty;
                SPFieldUserValueCollection usersGerencia = new SPFieldUserValueCollection();

                SPListItem orgao = item.Web.Lists["Gerência Responsável MTC"].GetItemById(new SPFieldLookupValue(item["Órgão Responsável"].ToString()).LookupId);
                usersGerencia = new SPFieldUserValueCollection(item.Web, Convert.ToString(orgao["Responsáveis"]));
                
                foreach (SPFieldUserValue userGerencia in usersGerencia)
                    emailsUsers += userGerencia.User.Email + ";";

                emailsUsers += Convert.ToString(orgao["Gerente Imediato"]).Replace("null", "");

                foreach (SPFieldUserValue userGerenciaAut in new SPFieldUserValueCollection(item.Web, Convert.ToString(item["Autores"])))
                    emailsUsers += userGerenciaAut.User.Email + ";";

                foreach (SPFieldUserValue userGerenciaColab in new SPFieldUserValueCollection(item.Web, Convert.ToString(item["Colaboradores"])))
                    emailsUsers += userGerenciaColab.User.Email + ";";

                foreach (SPFieldUserValue userGerenciaRev in new SPFieldUserValueCollection(item.Web, Convert.ToString(item["Revisores"])))
                    emailsUsers += userGerenciaRev.User.Email + ";";


                SPFieldUserValue Author = new SPFieldUserValue(item.Web, item[SPBuiltInFieldId.Author].ToString());                
                StringBuilder message = new StringBuilder();
                message.Append("<a href='" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "&Source=" + item.ParentList.ParentWeb.Url + "/Lists/ManualTecnico" + "'>" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "</a>");
                message.Append("</br>");
                message.Append("Descrição do MTC: " + item["Descrição do MTC"].ToString());
                message.Append("</br>");
                message.Append("Justificativa da Criação: " + item["Justificativa Solicitação"].ToString());
                message.Append("</br>");

                Email mail = new Email();
                mail.CCO = Author.User.Email + ";" + emailsUsers + ";" + EmailsGrupoAdminsChamado(item.Web);
                mail.Assunto = "Aguardando Aprovação MTC Número ID " + item.ID;
                mail.Corpo = message.ToString();

                mail.EnviaEmail(item.Web);
                
            }
            catch (Exception ex) { Log(item.Web, ex.Message, "Email Aguardando_" + item.ID); }
        }

        public static void EmailEmConsolidacao(SPListItem item)
        {
            try
            {
                string emailsUsers = string.Empty;
                SPFieldUserValueCollection usersGerencia = new SPFieldUserValueCollection();

                SPListItem orgao = item.Web.Lists["Gerência Responsável MTC"].GetItemById(new SPFieldLookupValue(item["Órgão Responsável"].ToString()).LookupId);
                usersGerencia = new SPFieldUserValueCollection(item.Web, Convert.ToString(orgao["Responsáveis"]));

                foreach (SPFieldUserValue userGerencia in usersGerencia)
                    emailsUsers += userGerencia.User.Email + ";";

                emailsUsers += Convert.ToString(orgao["Gerente Imediato"]).Replace("null", "");

                SPFieldUserValue Author = new SPFieldUserValue(item.Web, item[SPBuiltInFieldId.Author].ToString());
                StringBuilder message = new StringBuilder();
                message.Append("<a href='" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "&Source=" + item.ParentList.ParentWeb.Url + "/Lists/ManualTecnico" + "'>" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "</a>");
                message.Append("</br>");
                message.Append("Descrição do MTC: " + item["Descrição do MTC"].ToString());
                message.Append("</br>");
                message.Append("Justificativa da Criação: " + item["Justificativa Solicitação"].ToString());
                message.Append("</br>");

                Email mail = new Email();
                mail.CCO = Author.User.Email + ";" + emailsUsers + ";" + EmailsGrupoAdminsChamado(item.Web);
                mail.Assunto = "Em Consolidação MTC Número ID " + item.ID;
                mail.Corpo = message.ToString();

                mail.EnviaEmail(item.Web);

            }
            catch (Exception ex) { Log(item.Web, ex.Message, "Email Consolidacao_" + item.ID); }
        }

        public static void EmailEmConsolidacaoJob(SPListItem item)
        {
            try
            {
                string emailsUsers = string.Empty;
                SPFieldUserValueCollection usersGerencia = new SPFieldUserValueCollection();

                SPListItem orgao = item.Web.Lists["Gerência Responsável MTC"].GetItemById(new SPFieldLookupValue(item["Órgão Responsável"].ToString()).LookupId);
                usersGerencia = new SPFieldUserValueCollection(item.Web, Convert.ToString(orgao["Responsáveis"]));

                foreach (SPFieldUserValue userGerencia in usersGerencia)
                    emailsUsers += userGerencia.User.Email + ";";

                emailsUsers += Convert.ToString(orgao["Gerente Imediato"]).Replace("null", "");

                SPFieldUserValue Author = new SPFieldUserValue(item.Web, item[SPBuiltInFieldId.Author].ToString());
                StringBuilder message = new StringBuilder();
                message.Append("<a href='" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "&Source=" + item.ParentList.ParentWeb.Url + "/Lists/ManualTecnico" + "'>" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "</a>");
                message.Append("</br>");
                message.Append("Descrição do MTC: " + item["Descrição do MTC"].ToString());
                message.Append("</br>");
                message.Append("Justificativa da Criação: " + item["Justificativa Solicitação"].ToString());
                message.Append("</br>");

                Email mail = new Email();
                mail.CCO = Author.User.Email + ";" + emailsUsers + ";" + EmailsGrupoAdminsChamado(item.Web);
                mail.Assunto = "Em Consolidação MTC Número ID " + item.ID;
                mail.Corpo = message.ToString();

                mail.EnviaEmail(item.Web);

            }
            catch (Exception ex) { Log(item.Web, ex.Message, "Email Consolidacao_" + item.ID); }
        }

        public static void EmailCancelado(SPListItem item, string justificativa)
        {
            try
            {
                string emailsUsers = string.Empty;
                SPFieldUserValueCollection usersGerencia = new SPFieldUserValueCollection();

                SPListItem orgao = item.Web.Lists["Gerência Responsável MTC"].GetItemById(new SPFieldLookupValue(item["Órgão Responsável"].ToString()).LookupId);
                usersGerencia = new SPFieldUserValueCollection(item.Web, Convert.ToString(orgao["Responsáveis"]));

                foreach (SPFieldUserValue userGerencia in usersGerencia)
                    emailsUsers += userGerencia.User.Email + ";";

                emailsUsers += Convert.ToString(orgao["Gerente Imediato"]).Replace("null", "");


                SPFieldUserValue Author = new SPFieldUserValue(item.Web, item[SPBuiltInFieldId.Author].ToString());
                StringBuilder message = new StringBuilder();
                message.Append("<a href='" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "&Source=" + item.ParentList.ParentWeb.Url + "/Lists/ManualTecnico" + "'>" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "</a>");
                message.Append("</br>");
                message.Append("Descrição do MTC: " + item["Descrição do MTC"].ToString());
                message.Append("</br>");
                message.Append("Justificativa do Cancelamento: " + justificativa);
                message.Append("</br>");

                Email mail = new Email();
                mail.CCO = Author.User.Email + ";" + emailsUsers + ";" + EmailsGrupoAdminsChamado(item.Web);
                mail.Assunto = "Cancelado MTC Número ID " + item.ID;
                mail.Corpo = message.ToString();

                mail.EnviaEmail(item.Web);

            }
            catch (Exception ex) { Log(item.Web, ex.Message, "Email Cancelado_" + item.ID); }
        }

        public static void EmailParaComentarios(SPListItem item, string descricao, string data)
        {
            try
            {
                string emailsUsers = string.Empty;
                //SPFieldUserValueCollection usersGerencia = new SPFieldUserValueCollection();

                SPListItemCollection orgaos = item.Web.Lists["Gerência Responsável MTC"].GetItems();

                foreach (SPListItem orgao in orgaos)
                {
                    SPFieldUserValueCollection usersGerencia = new SPFieldUserValueCollection(item.Web, Convert.ToString(orgao["Responsáveis"]));

                    foreach (SPFieldUserValue userGerencia in usersGerencia)
                        emailsUsers += userGerencia.User.Email + ";";

                    emailsUsers += Convert.ToString(orgao["Gerente Imediato"]).Replace("null", "");
                }

                SPFieldUserValue Author = new SPFieldUserValue(item.Web, item[SPBuiltInFieldId.Author].ToString());
                StringBuilder message = new StringBuilder();
                message.Append("<a href='" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "&Source=" + item.ParentList.ParentWeb.Url + "/Lists/ManualTecnico" + "'>" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "</a>");
                message.Append("</br>");
                message.Append("Descrição do MTC: " + descricao);
                //message.Append("</br>");
                //message.Append(descricao); //item["Descrição do MTC"].ToString());
                message.Append("</br>");
                message.Append("Data limite para comentários: " + Convert.ToDateTime(data).ToShortDateString());//item["Data Limite Comentário"].ToString());
                message.Append("</br>");

                Email mail = new Email();
                mail.CCO = Author.User.Email + ";" + emailsUsers + ";" + EmailsGrupoAdminsChamado(item.Web);
                mail.Assunto = "Para Comentários MTC Número ID " + item.ID;
                mail.Corpo = message.ToString();

                mail.EnviaEmail(item.Web);

            }
            catch (Exception ex) { Log(item.Web, ex.Message, "Email ParaComentarios_" + item.ID); }
        }

        public static void EmailRevisao(SPListItem item, string descricao)
        {
            try
            {
                string emailsUsers = string.Empty;
                //SPFieldUserValueCollection usersGerencia = new SPFieldUserValueCollection();

                SPListItemCollection orgaos = item.Web.Lists["Gerência Responsável MTC"].GetItems();

                foreach (SPListItem orgao in orgaos)
                {
                    SPFieldUserValueCollection usersGerencia = new SPFieldUserValueCollection(item.Web, Convert.ToString(orgao["Responsáveis"]));

                    foreach (SPFieldUserValue userGerencia in usersGerencia)
                        emailsUsers += userGerencia.User.Email + ";";

                    emailsUsers += Convert.ToString(orgao["Gerente Imediato"]).Replace("null", "");

                }

                SPFieldUserValue Author = new SPFieldUserValue(item.Web, item[SPBuiltInFieldId.Author].ToString());
                StringBuilder message = new StringBuilder();
                message.Append("<a href='" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "&Source=" + item.ParentList.ParentWeb.Url + "/Lists/ManualTecnico" + "'>" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "</a>");
                message.Append("</br>");
                message.Append("Descrição do MTC: " + descricao);
                message.Append("</br>");

                Email mail = new Email();
                mail.CCO = Author.User.Email + ";" + emailsUsers + ";" + EmailsGrupoAdminsChamado(item.Web);
                mail.Assunto = "Em Revisão MTC Número ID " + item.ID;
                mail.Corpo = message.ToString();

                mail.EnviaEmail(item.Web);

            }
            catch (Exception ex) { Log(item.Web, ex.Message, "Email ParaComentarios_" + item.ID); }
        }

        public static void EmailParaVigente(SPListItem item)
        {
            try
            {
                string emailsUsers = string.Empty;

                SPListItemCollection orgaos = item.Web.Lists["Gerência Responsável MTC"].GetItems();

                foreach (SPListItem orgao in orgaos)
                {
                    SPFieldUserValueCollection usersGerencia = new SPFieldUserValueCollection(item.Web, Convert.ToString(orgao["Responsáveis"]));

                    foreach (SPFieldUserValue userGerencia in usersGerencia)
                        emailsUsers += userGerencia.User.Email + ";";

                    emailsUsers += Convert.ToString(orgao["Gerente Imediato"]).Replace("null", "");

                }

                SPFieldUserValue Author = new SPFieldUserValue(item.Web, item[SPBuiltInFieldId.Author].ToString());
                StringBuilder message = new StringBuilder();
                message.Append("<a href='" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "&Source=" + item.ParentList.ParentWeb.Url + "/Lists/ManualTecnico" + "'>" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "</a>");
                message.Append("</br>");
                message.Append("Descrição do MTC: " + item["Descrição do MTC"].ToString());
                message.Append("</br>");
                message.Append("Justificativa Criação: " + item["Justificativa Solicitação"].ToString());
                message.Append("</br>");

                Email mail = new Email();
                mail.CCO = Author.User.Email + ";" + emailsUsers + ";" + EmailsGrupoAdminsChamado(item.Web);
                mail.Assunto = " Vigente MTC Número ID " + item.ID;
                mail.Corpo = message.ToString();

                mail.EnviaEmail(item.Web);

            }
            catch (Exception ex) { Log(item.Web, ex.Message, "Email Vigente_" + item.ID); }
        }

        public static void EmailVencimentoMeses(SPWeb web, StringBuilder message, string assunto)
        {
            try
            {
                Email mail = new Email();
                mail.CCO = EmailsGrupoAdminsChamado(web);
                mail.Assunto = assunto;
                mail.Corpo = message.ToString();

                mail.EnviaEmail(web);

            }
            catch (Exception ex) { Log(web, ex.Message, "Email JobMes12_"); }
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

        public static string EmailsGrupoAdminsChamado(SPWeb web)
        {
            string emails = string.Empty;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                foreach (SPUser user in ImpersonatedWeb.SiteGroups["Administradores GGA MTC"].Users)
                {
                    if (user.Email != "")
                        emails += user.Email + ";";
                }

                ImpersonatedWeb.Close(); ImpersonatedWeb.Dispose();
                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();

            });

            return emails;
        }

    }
}
