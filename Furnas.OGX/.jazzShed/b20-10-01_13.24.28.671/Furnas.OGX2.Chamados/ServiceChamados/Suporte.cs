using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furnas.OGX2.Chamados.ServiceChamados
{
    public class Suporte
    {
        public static string ValidaTextField(object fieldvalue)
        {
            return (fieldvalue != null) ? Convert.ToString(fieldvalue) : string.Empty;
        }

        public static string GeretenteImediato(SPUser user)
        {
            return "";
        }

        public static void Email_1(SPListItem item)
        {
            StringDictionary headers = new StringDictionary();

            headers.Add("from", "aplicacoesweb@furnas.com.br");
            SPFieldUserValue Author = new SPFieldUserValue(item.Web, item[SPBuiltInFieldId.Author].ToString());

            headers.Add("cc", Author.User.Email);
            headers.Add("subject", "Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString() + " foi recebida pela Gerência de Gestão e Monitoramento de Ativos - GGA.O.");
            headers.Add("content-type", "text/html");

            StringBuilder message = new StringBuilder();

            //message.Append("Assunto: Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString());
            //message.Append("</br>");
            message.Append("<a href='" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "&Source=" + item.ParentList.ParentWeb.Url + "/Lists/Chamados" + "'>" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "</a>");
            message.Append("</br>");
            message.Append("Chamado: " + new SPFieldLookupValue(item["Tipo de Chamado"].ToString()).LookupValue);
            message.Append("</br>");
            message.Append("Status:" + item["Controle de status"].ToString());
            message.Append("</br>");
            message.Append("Data da solicitação:" + item["Data da solicitação"].ToString().Substring(0,10));
            message.Append("</br>");
            message.Append("Descrição: " + item["DescricaoChamado"].ToString());

            Email mail = new Email();
            mail.CC = Author.User.Email + ";" + item["Imediato do Responsável"].ToString();
            mail.Assunto = "Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString() + " foi recebida pela Gerência de Gestão e Monitoramento de Ativos - GGA.O.";
            mail.Corpo = message.ToString();

            mail.EnviaEmail(item.Web);
        }

        public static void Email_9(SPListItem item)
        {
            StringDictionary headers = new StringDictionary();

            headers.Add("from", "aplicacoesweb@furnas.com.br");
            SPFieldUserValue Author = new SPFieldUserValue(item.Web, item[SPBuiltInFieldId.Author].ToString());

            StringBuilder message = new StringBuilder();

            message.Append("<a href='" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "&Source=" + item.ParentList.ParentWeb.Url + "/Lists/Chamados" + "'>" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "</a>");
            message.Append("</br>");
            message.Append("Chamado: " + new SPFieldLookupValue(item["Tipo de Chamado"].ToString()).LookupValue);
            message.Append("</br>");
            message.Append("Status: Encerrado");
            message.Append("</br>");
            message.Append("Data da solicitação:" + item["Data da solicitação"].ToString().Substring(0, 10));
            message.Append("</br>");
            message.Append("</br>");
            message.Append("Descrição:" + item["DescricaoChamado"].ToString());

            int version = item.Versions.Count - 1; int versionAux = 0;
            List<string> listEsclarecimento = new List<string>();
            List<string> listEsclarecimentoGerencia = new List<string>();
            while (version >= versionAux)
            {
                string versoesGerencia = ValidaTextField(item.Versions[versionAux]["EsclarecimentosGerencia"]);
                if (versoesGerencia != string.Empty)
                    listEsclarecimentoGerencia.Add(versoesGerencia);

                string versoes = ValidaTextField(item.Versions[versionAux]["Esclarecimentos"]);
                if (versoes != string.Empty)
                    listEsclarecimento.Add(versoes);

                versionAux++;
            }

            if (listEsclarecimento.Count > 0)
            {
                message.Append("</br>");
                message.Append("Esclarecimentos: ");
                foreach (string escl in listEsclarecimento)
                {
                    message.Append("</br>");
                    message.Append("- " + escl);
                }
            }

            if (listEsclarecimentoGerencia.Count > 0)
            {
                message.Append("</br>");
                message.Append("Esclarecimento Gerência: ");
                foreach (string esclGerencia in listEsclarecimentoGerencia)
                {
                    message.Append("</br>");
                    message.Append("- " + esclGerencia);
                }
            }

            message.Append("</br></br>");
            message.Append("Participe da Pesquisa de Satisfação:");
            message.Append("</br>");
            message.Append("<a href='" + item.ParentList.ParentWeb.Url + "/SitePages/Pesquisa.aspx" + "?IDChamado=" + item.ID + "'>" + item.ParentList.ParentWeb.Url + "/SitePages/Pesquisa.aspx" + "?IDChamado=" + item.ID + "</a>");

            Email mail = new Email();
            mail.CC = Author.User.Email + ";" + item["Imediato do Responsável"].ToString();
            mail.Assunto = "Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString() + " foi encerrada pela Gerência de Gestão e Monitoramento de Ativos - GGA.O.";
            mail.Corpo = message.ToString();

            mail.EnviaEmail(item.Web);
        }

        public static void Email_7(SPListItem item, string esclarecimento)
        {
            StringDictionary headers = new StringDictionary();

            headers.Add("from", "aplicacoesweb@furnas.com.br");
            //SPUser Resp = item.Web.EnsureUser(Suporte.ValidaTextField(item["Responsavel"]));//.Split('|')[1]);
            SPFieldUserValue Resp = null;
            if(ValidaTextField(item["Responsável"]) != string.Empty)
                Resp = new SPFieldUserValue(item.Web, item["Responsável"].ToString());

            //headers.Add("cc", Resp.User.Email);
            headers.Add("subject", "Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString() + " foi atualizada. Continuar o atendimento.");
            headers.Add("content-type", "text/html");

            StringBuilder message = new StringBuilder();

            //message.Append("Assunto: Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString());
            //message.Append("</br>");
            message.Append("<a href='" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "&Source=" + item.ParentList.ParentWeb.Url + "/Lists/Chamados" + "'>" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "</a>");
            message.Append("</br>");
            message.Append("Chamado: " + new SPFieldLookupValue(item["Tipo de Chamado"].ToString()).LookupValue);
            message.Append("</br>");
            message.Append("Status: Em andamento");// + item["Controle de status"].ToString());
            message.Append("</br>");
            message.Append("Data da solicitação:" + item["Data da solicitação"].ToString().Substring(0, 10));
            message.Append("</br>");
            message.Append("Descrição:" + item["DescricaoChamado"].ToString());
            if (item["ControleDeStatus"].ToString() == "Encaminhado à outra gerência")
            {
                //Esclarecimentos, se existir
                int version = item.Versions.Count - 1; int versionAux = 0;
                List<string> ListEsclarecimentos = new List<string>();
                while (version >= versionAux)
                {
                    string versoes = ValidaTextField(item.Versions[versionAux]["Esclarecimentos"]);
                    if (versoes != string.Empty)
                        ListEsclarecimentos.Add(versoes);

                    versionAux++;
                }

                if (ListEsclarecimentos.Count > 0)
                {
                    message.Append("</br>");
                    message.Append("Esclarecimentos: ");
                    foreach (string versoesEsclarecimentos in ListEsclarecimentos)
                    {
                        message.Append("</br>");
                        message.Append("- " + versoesEsclarecimentos);
                    }
                }

                //Esclarecimento gerencia
                message.Append("</br></br>");
                message.Append("Esclarecimento Gerência: ");
                message.Append("- " + esclarecimento);
                version = item.Versions.Count - 1;
                while (version >= 0)
                {
                    string versoes = ValidaTextField(item.Versions[version]["Esclarecimentos Gerência"]);
                    if (versoes != string.Empty)
                    {
                        message.Append("</br>");
                        message.Append("- " + versoes);
                    }
                    version--;
                }
            }

            if (item["ControleDeStatus"].ToString() == "Aguardando informações")
            {
                message.Append("</br>");
                message.Append("Esclarecimentos: ");
                message.Append("- " + esclarecimento);
                int version = item.Versions.Count - 1;
                while (version >= 0)
                {
                    string versoes = ValidaTextField(item.Versions[version]["Esclarecimentos"]);
                    if (versoes != string.Empty)
                    {
                        message.Append("</br>");
                        message.Append("- " + versoes);
                    }
                    version--;
                }

                //Esclarecimentos Gerencia, se existir
                version = item.Versions.Count - 1; int versionAux = 0;
                List<string> ListEsclarecimentos = new List<string>();
                while (version >= versionAux)
                {
                    string versoes = ValidaTextField(item.Versions[versionAux]["Esclarecimentos Gerência"]);
                    if (versoes != string.Empty)
                        ListEsclarecimentos.Add(versoes);

                    versionAux++;
                }

                if (ListEsclarecimentos.Count > 0)
                {
                    message.Append("</br>");
                    message.Append("Esclarecimentos Gerência: ");
                    foreach (string versoesEsclarecimentos in ListEsclarecimentos)
                    {
                        message.Append("</br>");
                        message.Append("- " + versoesEsclarecimentos);
                    }
                }
            }
            

            Email mail = new Email();
            if (Resp != null)
                mail.CC = Resp.User.Email + ";" + EmailsGrupoAdminsChamado(item.Web);
            else
                mail.CC = EmailsGrupoAdminsChamado(item.Web);
            mail.Assunto = "Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString() + " foi atualizada. Continuar o atendimento.";
            mail.Corpo = message.ToString();

            mail.EnviaEmail(item.Web);
        }

        public static void Email_4(SPListItem item)
        {
            StringDictionary headers = new StringDictionary();

            headers.Add("from", "aplicacoesweb@furnas.com.br");
            //SPUser Resp = item.Web.EnsureUser(Suporte.ValidaTextField(item["Responsavel"]));//.Split('|')[1]);
            SPFieldUserValue Resp = null;
            if(Suporte.ValidaTextField(item["Responsável"]) != string.Empty)
                Resp = new SPFieldUserValue(item.Web, item["Responsável"].ToString());

            StringBuilder message = new StringBuilder();

            message.Append("<a href='" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "&Source=" + item.ParentList.ParentWeb.Url + "/Lists/Chamados" + "'>" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "</a>");
            message.Append("</br>");
            message.Append("Chamado: " + new SPFieldLookupValue(item["Tipo de Chamado"].ToString()).LookupValue);
            message.Append("</br>");
            message.Append("Status:" + item["Controle de status"].ToString());
            message.Append("</br>");
            DateTime date = Convert.ToDateTime(item["Data da solicitação"]);
            message.Append("Data da solicitação:" + date.ToString("dd/MM/yyyy"));
            message.Append("</br>");
            message.Append("Descrição:" + item["DescricaoChamado"].ToString());
            Email mail = new Email();
            if(Resp != null)
                mail.CC = Resp.User.Email +";"+ EmailsGrupoAdminsChamado(item.Web);
            else
                mail.CC = EmailsGrupoAdminsChamado(item.Web);
            mail.Assunto = "Prazo previsto de atendimento da solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString() + " esgotou.";
            mail.Corpo = message.ToString();

            mail.EnviaEmail(item.Web);
        }

        public static void Email_2(SPListItem item)
        {
            StringDictionary headers = new StringDictionary();

            StringBuilder message = new StringBuilder();
            DateTime date = Convert.ToDateTime(item["Data da solicitação"]);
            message.Append("<a href='" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "&Source=" + item.ParentList.ParentWeb.Url + "/Lists/Chamados" + "'>" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "</a>");
            message.Append("</br>");
            message.Append("Chamado: " + new SPFieldLookupValue(item["Tipo de Chamado"].ToString()).LookupValue);
            message.Append("</br>");
            message.Append("Status:" + item["Controle de status"].ToString());
            message.Append("</br>");
            message.Append("Data da solicitação:" + date.ToString("dd/MM/yyyy"));
            message.Append("</br>");
            message.Append("Descrição:" + item["DescricaoChamado"].ToString());
            Email mail = new Email();
            mail.CC = EmailsGrupoAdminsChamado(item.Web);
            mail.Assunto = "Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString() + " com status Aberta desde " + date.ToString("dd/MM/yyyy");
            mail.Corpo = message.ToString();

            mail.EnviaEmail(item.Web);
        }

        public static void Email_5(SPListItem item, string status, string esclarecimentos)
        {
            StringDictionary headers = new StringDictionary();

            headers.Add("from", "aplicacoesweb@furnas.com.br");
            SPFieldUserValue Author = new SPFieldUserValue(item.Web, item[SPBuiltInFieldId.Author].ToString());

            headers.Add("cc", Author.User.Email);
            headers.Add("subject", "Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString() + " necessita de novas informações para ser executada.");
            headers.Add("content-type", "text/html");

            StringBuilder message = new StringBuilder();

            message.Append("<a href='" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "&Source=" + item.ParentList.ParentWeb.Url + "/Lists/Chamados" + "'>" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "</a>");
            message.Append("</br>");
            message.Append("Chamado: " + new SPFieldLookupValue(item["Tipo de Chamado"].ToString()).LookupValue);
            message.Append("</br>");
            message.Append("Status:" + status);
            message.Append("</br>");
            message.Append("Data da solicitação:" + item["Data da solicitação"].ToString().Substring(0, 10));
            message.Append("</br>");
            message.Append("Descrição:" + item["DescricaoChamado"].ToString());
            message.Append("</br>");
            message.Append("Esclarecimentos: ");
            message.Append("</br>");
            message.Append("- " + esclarecimentos);
            int version = item.Versions.Count - 1;
            while (version >= 0)
            {
                string versoes = ValidaTextField(item.Versions[version]["Esclarecimentos"]);
                if (versoes != string.Empty)
                {
                    message.Append("</br>");
                    message.Append("- " + versoes);
                }
                version--;
            }

            Email mail = new Email();
            mail.CC = Author.User.Email + ";" + item["Imediato do Responsável"].ToString();
            mail.Assunto = "Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString() + " necessita de novas informações para ser executada.";
            mail.Corpo = message.ToString();

            mail.EnviaEmail(item.Web);
        }

        public static void Email_8(SPListItem item, string status, string usersGerencia, string esclarecimento)
        {
            StringDictionary headers = new StringDictionary();

            headers.Add("from", "aplicacoesweb@furnas.com.br");
            SPFieldUserValue Author = new SPFieldUserValue(item.Web, item[SPBuiltInFieldId.Author].ToString());

            StringBuilder message = new StringBuilder();

            message.Append("<a href='" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "&Source=" + item.ParentList.ParentWeb.Url + "/Lists/Chamados" + "'>" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "</a>");
            message.Append("</br>");
            message.Append("Chamado: " + new SPFieldLookupValue(item["Tipo de Chamado"].ToString()).LookupValue);
            message.Append("</br>");
            message.Append("Status:" + status);
            message.Append("</br>");
            message.Append("Data da solicitação:" + item["Data da solicitação"].ToString().Substring(0, 10));
            message.Append("</br>");
            message.Append("Descrição:" + item["DescricaoChamado"].ToString());
            message.Append("</br>");

            int version = item.Versions.Count - 1; int versionAux = 0;
            List<string> ListEsclarecimentos = new List<string>();
            while (version >= versionAux)
            {
                string versoes = ValidaTextField(item.Versions[versionAux]["Esclarecimentos"]);
                if (versoes != string.Empty)
                    ListEsclarecimentos.Add(versoes);

                versionAux++;
            }

            if (ListEsclarecimentos.Count > 0)
            {
                message.Append("</br>");
                message.Append("Esclarecimentos: ");
                foreach (string versoesEsclarecimentos in ListEsclarecimentos)
                {
                    message.Append("</br>");
                    message.Append("- " + versoesEsclarecimentos);
                }
            }
            
            message.Append("</br></br>");
            message.Append("Esclarecimento Gerência: ");
            version = item.Versions.Count - 1; versionAux = 0;
            while (version >= versionAux)
            {
                string versoes = ValidaTextField(item.Versions[versionAux]["Esclarecimentos Gerência"]);
                if (versoes != string.Empty)
                {
                    message.Append("</br>");
                    message.Append("- " + versoes);
                }
                versionAux++;
            }

            Email mail = new Email();
            //mail.CC = Author.User.Email + ";" + item["Imediato do Responsável"].ToString();
            mail.CC = Author.User.Email + ";" + item["Imediato do Responsável"].ToString() + ";" + usersGerencia + ";" + EmailsGrupoAdminsChamado(item.Web);
            mail.Assunto = "Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString() + " foi direcionada à Gerência responsável.";
            mail.Corpo = message.ToString();

            mail.EnviaEmail(item.Web);
        }

        public static void Email_Job_8(SPListItem item, string status)
        {
            StringDictionary headers = new StringDictionary();

            headers.Add("from", "aplicacoesweb@furnas.com.br");
            SPFieldUserValue Author = new SPFieldUserValue(item.Web, item[SPBuiltInFieldId.Author].ToString());

            headers.Add("subject", "Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString() + " foi direcionada à Gerência responsável.");
            headers.Add("content-type", "text/html");

            StringBuilder message = new StringBuilder();

            message.Append("<a href='" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "&Source=" + item.ParentList.ParentWeb.Url + "/Lists/Chamados" + "'>" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "</a>");
            message.Append("</br>");
            message.Append("Chamado: " + new SPFieldLookupValue(item["Tipo de Chamado"].ToString()).LookupValue);
            message.Append("</br>");
            message.Append("Status:" + status);
            message.Append("</br>");
            DateTime date = Convert.ToDateTime(item["Data da solicitação"]);
            message.Append("Data da solicitação:" + date.ToString("dd/MM/yyyy"));
            message.Append("</br>");
            message.Append("Descrição:" + item["DescricaoChamado"].ToString());
            message.Append("</br>");
            message.Append("Esclarecimento Gerência: ");
            int version = item.Versions.Count-1;
            while (version >= 0)
            {
                string versoes = ValidaTextField(item.Versions[version]["Esclarecimentos Gerência"]);
                if (versoes != string.Empty)
                {
                    message.Append("</br>");
                    message.Append("- " + versoes);
                }
                version--;
            }

            string emailsUsers = string.Empty;
            try
            {
                SPListItem itemGerencia = item.Web.Lists["Gerência Responsável"].GetItemById(new SPFieldLookupValue(item["Gerência Responsável"].ToString()).LookupId);
                SPFieldUserValueCollection usersGerencia = new SPFieldUserValueCollection(item.Web, Convert.ToString(itemGerencia["Responsáveis"]));
                foreach (SPFieldUserValue userGerencia in usersGerencia)
                {
                    emailsUsers += userGerencia.User.Email + ";";
                }

                foreach (string email in Convert.ToString(itemGerencia["Gerente Imediato"]).Split(';'))
                {
                    if (email != "" && email != "null")
                    {
                        emailsUsers += email + ";";
                    }
                }
            }
            catch { }

            Email mail = new Email();
            mail.CC = Author.User.Email + ";" + item["Imediato do Responsável"].ToString() + ";" + emailsUsers + ";" + EmailsGrupoAdminsChamado(item.Web);
            mail.Assunto = "Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString() + " foi direcionada à Gerência responsável.";
            mail.Corpo = message.ToString();

            mail.EnviaEmail(item.Web);
        }

        public static string EmailsGrupoAdminsChamado(SPWeb web)
        {
            string emails = string.Empty;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                foreach (SPUser user in ImpersonatedWeb.SiteGroups["Administradores GGA Chamados"].Users)
                {
                    emails += user.Email + ";";
                }

                ImpersonatedWeb.Close(); ImpersonatedWeb.Dispose();
                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();

            });

            return emails;
        }

        public static void Email_10(SPListItem item, SPUser Resp) 
        {
            StringDictionary headers = new StringDictionary();

            headers.Add("from", "aplicacoesweb@furnas.com.br");
            //SPFieldUserValue Resp = new SPFieldUserValue(item.Web, item["Responsável"].ToString());
            
            headers.Add("cc", Resp.Email);
            headers.Add("subject", "Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString() + " foi redirecionada para " + Resp.Name);
            headers.Add("content-type", "text/html");

            StringBuilder message = new StringBuilder();

            message.Append("<a href='" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "&Source=" + item.ParentList.ParentWeb.Url + "/Lists/Chamados" + "'>" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "</a>");
            message.Append("</br>");
            message.Append("Chamado: " + new SPFieldLookupValue(item["Tipo de Chamado"].ToString()).LookupValue);
            message.Append("</br>");
            message.Append("Status:" + item["Controle de status"].ToString());
            message.Append("</br>");
            message.Append("Data da solicitação:" + item["Data da solicitação"].ToString().Substring(0, 10));
            message.Append("</br>");
            message.Append("Descrição:" + item["DescricaoChamado"].ToString());

            Email mail = new Email();
            mail.CC = Resp.Email;
            mail.Assunto = "Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString() + " foi redirecionada para " + Resp.Name;
            mail.Corpo = message.ToString();

            mail.EnviaEmail(item.Web);
        }

        public static void EmailResp_1(SPListItem item)
        {
            StringDictionary headers = new StringDictionary();

            headers.Add("from", "aplicacoesweb@furnas.com.br");
            SPFieldUserValue Author = new SPFieldUserValue(item.Web, item["Responsável"].ToString());

            headers.Add("cc", Author.User.Email);
            headers.Add("subject", "Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString());
            headers.Add("content-type", "text/html");

            StringBuilder message = new StringBuilder();

            message.Append("Assunto: Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString());
            message.Append("</br>");
            message.Append("Link: <a href='" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "'>" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "</a>");
            message.Append("</br>");
            message.Append("Chamado: " + item["Descrição do chamado"].ToString());
            message.Append("</br>");
            message.Append("Status:" + item["Controle de status"].ToString());
            message.Append("</br>");
            message.Append("Data da solicitação:" + item["Data da solicitação"].ToString().Substring(0, 10));

            Email mail = new Email();
            mail.CC = Author.User.Email;
            mail.Assunto = "Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString();
            mail.Corpo = message.ToString();

            mail.EnviaEmail(item.Web);
        }

        public static void EmailResp_1(SPItemEventProperties item)
        {
            StringDictionary headers = new StringDictionary();

            headers.Add("from", "aplicacoesweb@furnas.com.br");
            SPFieldUserValue Author = new SPFieldUserValue(item.Web, item.AfterProperties["Responsável"].ToString());

            headers.Add("cc", Author.User.Email);
            headers.Add("subject", "Solicitação de serviço " + item.ListItem[SPBuiltInFieldId.Title].ToString());
            headers.Add("content-type", "text/html");

            StringBuilder message = new StringBuilder();

            message.Append("Assunto: Solicitação de serviço " + item.ListItem[SPBuiltInFieldId.Title].ToString());
            message.Append("</br>");
            message.Append("Link: <a href='" + item.ListItem.ParentList.ParentWeb.Site.MakeFullUrl(item.ListItem.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ListItem.ID + "'>" + item.ListItem.ParentList.ParentWeb.Site.MakeFullUrl(item.ListItem.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ListItem.ID + "</a>");
            message.Append("</br>");
            message.Append("Chamado: " + item.AfterProperties["Descrição do chamado"].ToString());
            message.Append("</br>");
            message.Append("Status:" + item.AfterProperties["Controle de status"].ToString());
            message.Append("</br>");
            message.Append("Data da solicitação:" + item.AfterProperties["Data da solicitação"].ToString());

            Email mail = new Email();
            mail.CC = Author.User.Email;
            mail.Assunto = "Solicitação de serviço " + item.ListItem[SPBuiltInFieldId.Title].ToString();
            mail.Corpo = message.ToString();

            mail.EnviaEmail(item.Web);
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

        public static int NumeroDoChamado(SPWeb web)
        {
            int ID = 0;
             SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                SPList list = ImpersonatedWeb.Lists["Chamados"];
                SPQuery query = new SPQuery();
                query.RowLimit = 1;
                query.Query = "<OrderBy><FieldRef Name='ID' Ascending='FALSE' /></OrderBy>";

                SPListItem item =list.GetItems(query).Cast<SPListItem>().FirstOrDefault();

                if(item != null)
                    ID = item.ID;
                
                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();
            });

             return ID;
        }
    }
}
