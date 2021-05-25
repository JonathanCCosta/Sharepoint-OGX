using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            headers.Add("subject", "Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString() + " conforme está na solicitação foi recebida pela Gerência de Gestão e Monitoramento de Ativos - GGA.O.");
            headers.Add("content-type", "text/html");

            StringBuilder message = new StringBuilder();

            //message.Append("Assunto: Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString());
            //message.Append("</br>");
            message.Append("<a href='" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "&Source=" + item.ParentList.ParentWeb.Url + "/Lists/Chamados" + "'>" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "</a>");
            message.Append("</br>");
            message.Append("Chamado: " + new SPFieldLookupValue(item["Tipo de Chamado"].ToString()).LookupValue);
            message.Append("</br>");
            message.Append("Solicitante: " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Author.User.Name.ToLower()));
            message.Append("</br>");
            message.Append("Status:" + item["Controle de status"].ToString());
            message.Append("</br>");
            message.Append("Data da solicitação:" + item["Data da solicitação"].ToString().Substring(0, 10));
            message.Append("</br>");
            message.Append("Descrição do Chamado: " + RetiraHTML(item["DescricaoChamado"].ToString()));

            Email mail = new Email();

            mail.CC = Author.User.Email + ";" + item["Imediato do Responsável"].ToString();
            

            mail.CCO = EmailsGrupoAdminsChamado(item.Web);


            mail.Assunto = "Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString() + " conforme está na solicitação foi recebida pela Gerência de Gestão e Monitoramento de Ativos - GGA.O.";
            mail.Corpo = message.ToString() + MensagemEmailAutomatico();

            mail.EnviaEmail(item.Web);
        }

        public static void Email_9(SPListItem item, string comentario)
        {
            StringDictionary headers = new StringDictionary();

            headers.Add("from", "aplicacoesweb@furnas.com.br");
            SPFieldUserValue Author = new SPFieldUserValue(item.Web, item[SPBuiltInFieldId.Author].ToString());

            StringBuilder message = new StringBuilder();

            message.Append("<a href='" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "&Source=" + item.ParentList.ParentWeb.Url + "/Lists/Chamados" + "'>" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "</a>");
            message.Append("</br>");
            message.Append("Chamado: " + new SPFieldLookupValue(item["Tipo de Chamado"].ToString()).LookupValue);
            message.Append("</br>");

            /* SPRINT 7 - 28876 */
            /* Tarefa - Alterar corpo do email com novas informações */
            message.Append("Solicitante: " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Author.User.Name.ToLower()));
            message.Append("</br>");

            message.Append("Status: Encerrado");
            message.Append("</br>");
            message.Append("Data da solicitação:" + item["Data da solicitação"].ToString().Substring(0, 10));
            message.Append("</br>");
            message.Append("Descrição do Chamado:" + RetiraHTML(item["DescricaoChamado"].ToString()) + "</br>");
            message.Append("</br>");

            if (comentario != string.Empty)
            {
                message.Append("Comentário:" + comentario + "</br>");
                message.Append("</br>");
            }

            int version = item.Versions.Count - 1; int versionAux = 0;
            List<string> listEsclarecimento = new List<string>();
            List<string> listEsclarecimentoGerencia = new List<string>();

            bool contemTitulo = false;

            while (version >= versionAux)
            {
                string versoesGerencia = ValidaTextField(item.Versions[versionAux]["EsclarecimentosGerencia"]);


                if (versoesGerencia != string.Empty)
                {
                    if (contemTitulo == false)
                    {
                        message.Append("Esclarecimentos outra Gerência: ");
                        contemTitulo = true;

                    }

                    //listEsclarecimentoGerencia.Add(versoesGerencia);

                    //foreach (string esclGerencia in listEsclarecimentoGerencia)
                    //{
                    DateTime dataModificacao = item.Versions[version].Created;

                    message.Append("</br>");
                    message.Append("</br>");
                    message.Append("- " + versoesGerencia);
                    message.Append("</br>");
                    message.Append("Nome do Usuario: " + item.Versions[version].CreatedBy.User.Name);
                    message.Append("</br>");
                    message.Append("Data: " + dataModificacao.ToShortDateString() + " " + dataModificacao.Hour + ":" + dataModificacao.Minute);
                    //}
                    //listEsclarecimentoGerencia.Add(versoesGerencia);


                }
                versionAux++;
            }


            versionAux = 0;

            contemTitulo = false;


            while (version >= versionAux)
            {
                string versoes = ValidaTextField(item.Versions[versionAux]["Esclarecimentos"]);

                if (versoes != string.Empty)
                {
                    if (contemTitulo == false)
                    {
                        message.Append("</br>");
                        message.Append("</br>");
                        message.Append("Esclarecimentos Solicitante: ");
                        contemTitulo = true;

                    }

                    //listEsclarecimento.Add(versoes);

                    if (versoes != string.Empty)
                    {

                        //foreach (string escl in listEsclarecimento)
                        //{

                        DateTime dataModificacao = item.Versions[version].Created;

                        message.Append("</br>");
                        message.Append("</br>");
                        message.Append("- " + versoes);
                        message.Append("</br>");
                        message.Append("Nome do Usuario: " + item.Versions[version].CreatedBy.User.Name);
                        message.Append("</br>");
                        message.Append("Data do esclarecimento:" + dataModificacao.ToShortDateString() + " " + dataModificacao.Hour + ":" + dataModificacao.Minute);
                        //}
                        //listEsclarecimento.Add(versoes);
                        //}


                    }

                }

                versionAux++;
            }

            /*if (listEsclarecimento.Count > 0)
            {
                message.Append("</br>");
                message.Append("Esclarecimentos: ");
                foreach (string escl in listEsclarecimento)
                {
                    message.Append("</br>");
                    message.Append("- " + escl);
                }
            }*/

            /*if (listEsclarecimentoGerencia.Count > 0)
            {
                message.Append("</br>");
                message.Append("Esclarecimento Gerência: ");
                foreach (string esclGerencia in listEsclarecimentoGerencia)
                {
                    message.Append("</br>");
                    message.Append("- " + esclGerencia);
                }
            }*/

            message.Append("</br></br>");
            message.Append("Participe da Pesquisa de Satisfação:");
            message.Append("</br>");
            message.Append("<a href='" + item.ParentList.ParentWeb.Url + "/SitePages/Pesquisa.aspx" + "?IDChamado=" + item.ID + "'>" + item.ParentList.ParentWeb.Url + "/SitePages/Pesquisa.aspx" + "?IDChamado=" + item.ID + "</a>");

            Email mail = new Email();

            mail.CC = Author.User.Email + ";" + item["Imediato do Responsável"].ToString();


            mail.CCO = EmailsGrupoAdminsChamado(item.Web);
            mail.Assunto = "Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString() + " foi encerrada pela Gerência de Gestão e Monitoramento de Ativos - GGA.O.";
            mail.Corpo = message.ToString() + MensagemEmailAutomatico();

            mail.EnviaEmail(item.Web);
        }

        public static void Email_7(SPListItem item, string esclarecimento, string usuarioModificado)
        {
            StringDictionary headers = new StringDictionary();

            headers.Add("from", "aplicacoesweb@furnas.com.br");
            SPFieldUserValue Resp = null;
            if (ValidaTextField(item["Responsável"]) != string.Empty)
                Resp = new SPFieldUserValue(item.Web, item["Responsável"].ToString());

            //Verificar
            SPFieldUserValue Author = new SPFieldUserValue(item.Web, item[SPBuiltInFieldId.Author].ToString());

            headers.Add("subject", "Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString() + " foi atualizada. Continuar o atendimento.");
            headers.Add("content-type", "text/html");

            StringBuilder message = new StringBuilder();

            message.Append("<a href='" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "&Source=" + item.ParentList.ParentWeb.Url + "/Lists/Chamados" + "'>" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "</a>");
            message.Append("</br>");
            message.Append("Chamado: " + new SPFieldLookupValue(item["Tipo de Chamado"].ToString()).LookupValue);

            /* SPRINT 7 - 28876 */
            /* Tarefa - Alterar corpo do email com novas informações */
            message.Append("</br>");
            message.Append("Solicitante: " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Author.User.Name.ToLower()));
            message.Append("</br>");

            message.Append("Status: Em andamento");
            message.Append("</br>");
            message.Append("Data da solicitação:" + item["Data da solicitação"].ToString().Substring(0, 10));
            message.Append("</br>");
            message.Append("Descrição do Chamado:" + RetiraHTML(item["DescricaoChamado"].ToString()) + "</br>");


            if (item["ControleDeStatus"].ToString() == "Encaminhado à outra gerência da DO")
            {
                //Esclarecimentos, se existir
                int version = item.Versions.Count - 1; int versionAux = 0;
                List<string> ListEsclarecimentos = new List<string>();

                bool contemTitulo = false;

                while (version >= versionAux)
                {
                    string versoes = ValidaTextField(item.Versions[versionAux]["Esclarecimentos"]);

                    if (versoes != string.Empty)
                    {
                        if (contemTitulo == false)
                        {
                            message.Append("</br>");
                            message.Append("Esclarecimentos Solicitante: ");
                            contemTitulo = true;
                        }

                        if (versoes != string.Empty)
                        {
                            //DateTime dataModificacao = item.Versions[version].Created;

                            message.Append("</br>");
                            message.Append("</br>");
                            message.Append("- " + versoes);
                            message.Append("</br></br>");

                            DateTime dataModificacao = Convert.ToDateTime(item.Versions[versionAux]["Modificado"].ToString()).AddHours(-3);
                            string usuarioNome = new SPFieldUserValue(item.Web, item.Versions[versionAux]["Modificado por"].ToString()).User.Name;
                            message.Append("Nome do Usuario: " + usuarioNome);
                            message.Append("</br>");
                            message.Append("Data do esclarecimento:" + dataModificacao.ToShortDateString() + " " + (dataModificacao.Hour <= 9 ? "0" + dataModificacao.Hour.ToString() : dataModificacao.Hour.ToString()) + ":" + (dataModificacao.Minute <= 9 ? "0" + dataModificacao.Minute.ToString() : dataModificacao.Minute.ToString()));

                        }
                    }

                    versionAux++;
                }

                versionAux = 0;
                contemTitulo = false;
                version = item.Versions.Count - 1;

                while (version >= versionAux)
                {
                    string versoesGerencia = ValidaTextField(item.Versions[versionAux]["EsclarecimentosGerencia"]);

                    if (versoesGerencia != string.Empty)
                    {
                        if (contemTitulo == false)
                        {
                            message.Append("</br></br>");
                            message.Append("</br>");
                            message.Append("Esclarecimentos outra Gerência: ");
                            contemTitulo = true;
                            message.Append("</br>");
                            message.Append("</br>");
                            message.Append("- " + esclarecimento);
                            message.Append("</br></br>");
                            message.Append("Nome do Usuario: " + usuarioModificado);
                            message.Append("</br>");
                            message.Append("Data: " + DateTime.Today.ToShortDateString() + " " + (DateTime.Now.Hour <= 9 ? "0" + DateTime.Now.Hour.ToString() : DateTime.Now.Hour.ToString()) + ":" + (DateTime.Now.Minute <= 9 ? "0" + DateTime.Now.Minute.ToString() : DateTime.Now.Minute.ToString()));
                        }

                        message.Append("</br>");
                        message.Append("</br>");
                        message.Append("- " + versoesGerencia);
                        message.Append("</br></br>");

                        DateTime dataModificacao = Convert.ToDateTime(item.Versions[versionAux]["Modificado"].ToString()).AddHours(-3);
                        string usuarioNome = new SPFieldUserValue(item.Web, item.Versions[versionAux]["Modificado por"].ToString()).User.Name;
                        message.Append("Nome do Usuario: " + usuarioNome);
                        message.Append("</br>");
                        message.Append("Data: " + dataModificacao.ToShortDateString() + " " + (dataModificacao.Hour <= 9 ? "0" + dataModificacao.Hour.ToString() : dataModificacao.Hour.ToString()) + ":" + (dataModificacao.Minute <= 9 ? "0" + dataModificacao.Minute.ToString() : dataModificacao.Minute.ToString()));
                    }
                    versionAux++;
                }

                /*versionAux = 0;
                contemTitulo = false;

                while (version >= versionAux)
                {
                    string versoes = ValidaTextField(item.Versions[versionAux]["Esclarecimentos"]);

                    if (versoes != string.Empty)
                    {
                        if (contemTitulo == false)
                        {
                            message.Append("</br>");
                            message.Append("</br>");
                            message.Append("Esclarecimentos Solicitante: ");
                            contemTitulo = true;
                        }

                        if (versoes != string.Empty)
                        {
                            //DateTime dataModificacao = item.Versions[version].Created;

                            message.Append("</br>");
                            message.Append("</br>");
                            message.Append("- " + versoes);
                            message.Append("</br>");

                            DateTime dataModificacao = Convert.ToDateTime(item.Versions[versionAux]["Modificado"].ToString()).AddHours(-3);
                            string usuarioNome = new SPFieldUserValue(item.Web, item.Versions[versionAux]["Modificado por"].ToString()).User.Name;
                            message.Append("Nome do Usuario: " + usuarioNome);
                            message.Append("</br>");
                            message.Append("Data do esclarecimento:" + dataModificacao.ToShortDateString() + " " + (dataModificacao.Hour <= 9 ? "0" + dataModificacao.Hour.ToString() : dataModificacao.Hour.ToString()) + ":" + (dataModificacao.Minute <= 9 ? "0" + dataModificacao.Minute.ToString() : dataModificacao.Minute.ToString()));

                        }
                    }

                    versionAux++;
                }*/
            }

            if (item["ControleDeStatus"].ToString() == "Aguardando informações")
            {
                //Esclarecimentos, se existir
                int version = item.Versions.Count - 1; int versionAux = 0;
                List<string> ListEsclarecimentos = new List<string>();

                bool contemTitulo = false;

                while (version >= versionAux)
                {
                    string versoes = ValidaTextField(item.Versions[versionAux]["Esclarecimentos"]);

                    if (versoes != string.Empty)
                    {
                        if (contemTitulo == false)
                        {
                            message.Append("</br>");
                            message.Append("</br>");
                            message.Append("Esclarecimentos Solicitante: ");
                            contemTitulo = true;
                            message.Append("</br>");
                            message.Append("</br>");
                            message.Append("- " + esclarecimento);
                            message.Append("</br></br>");
                            message.Append("Nome do Usuario: " + usuarioModificado);
                            message.Append("</br>");
                            message.Append("Data: " + DateTime.Today.ToShortDateString() + " " + (DateTime.Now.Hour <= 9 ? "0" + DateTime.Now.Hour.ToString() : DateTime.Now.Hour.ToString()) + ":" + (DateTime.Now.Minute <= 9 ? "0" + DateTime.Now.Minute.ToString() : DateTime.Now.Minute.ToString()));
                        }

                        if (versoes != string.Empty)
                        {
                            //DateTime dataModificacao = item.Versions[version].Created;

                            message.Append("</br>");
                            message.Append("</br>");
                            message.Append("- " + versoes);
                            message.Append("</br></br>");

                            DateTime dataModificacao = Convert.ToDateTime(item.Versions[versionAux]["Modificado"].ToString()).AddHours(-3);
                            string usuarioNome = new SPFieldUserValue(item.Web, item.Versions[versionAux]["Modificado por"].ToString()).User.Name;
                            message.Append("Nome do Usuario: " + usuarioNome);
                            message.Append("</br>");
                            message.Append("Data do esclarecimento:" + dataModificacao.ToShortDateString() + " " + (dataModificacao.Hour <= 9 ? "0" + dataModificacao.Hour.ToString() : dataModificacao.Hour.ToString()) + ":" + (dataModificacao.Minute <= 9 ? "0" + dataModificacao.Minute.ToString() : dataModificacao.Minute.ToString()));

                        }
                    }
                    versionAux++;
                }

                versionAux = 0;
                contemTitulo = false;
                version = item.Versions.Count - 1;

                while (version >= versionAux)
                {
                    string versoesGerencia = ValidaTextField(item.Versions[versionAux]["EsclarecimentosGerencia"]);

                    if (versoesGerencia != string.Empty)
                    {
                        if (contemTitulo == false)
                        {
                            message.Append("</br></br>");
                            message.Append("</br>");
                            message.Append("Esclarecimentos outra Gerência: ");
                            contemTitulo = true;
                            //message.Append("</br>");
                            //message.Append("</br>");
                            //message.Append("- " + esclarecimento);
                            //message.Append("</br>");
                            //message.Append("Nome do Usuario: " + usuarioModificado);
                            //message.Append("</br>");
                            //message.Append("Data: " + DateTime.Today.ToShortDateString() + " " + (DateTime.Now.Hour <= 9 ? "0" + DateTime.Now.Hour.ToString() : DateTime.Now.Hour.ToString()) + ":" + (DateTime.Now.Minute <= 9 ? "0" + DateTime.Now.Minute.ToString() : DateTime.Now.Minute.ToString()));
                        }

                        message.Append("</br>");
                        message.Append("</br>");
                        message.Append("- " + versoesGerencia);
                        message.Append("</br></br>");

                        DateTime dataModificacao = Convert.ToDateTime(item.Versions[versionAux]["Modificado"].ToString()).AddHours(-3);
                        string usuarioNome = new SPFieldUserValue(item.Web, item.Versions[versionAux]["Modificado por"].ToString()).User.Name;
                        message.Append("Nome do Usuario: " + usuarioNome);
                        message.Append("</br>");
                        message.Append("Data: " + dataModificacao.ToShortDateString() + " " + (dataModificacao.Hour <= 9 ? "0" + dataModificacao.Hour.ToString() : dataModificacao.Hour.ToString()) + ":" + (dataModificacao.Minute <= 9 ? "0" + dataModificacao.Minute.ToString() : dataModificacao.Minute.ToString()));
                    }
                    versionAux++;
                }

                /*versionAux = 0;
                contemTitulo = false;

                while (version >= versionAux)
                {
                    string versoes = ValidaTextField(item.Versions[versionAux]["Esclarecimentos"]);

                    if (versoes != string.Empty)
                    {
                        if (contemTitulo == false)
                        {
                            message.Append("</br>");
                            message.Append("</br>");
                            message.Append("Esclarecimentos Solicitante: ");
                            contemTitulo = true;
                        }

                        if (versoes != string.Empty)
                        {
                            //DateTime dataModificacao = item.Versions[version].Created;

                            message.Append("</br>");
                            message.Append("</br>");
                            message.Append("- " + versoes);
                            message.Append("</br>");

                            DateTime dataModificacao = Convert.ToDateTime(item.Versions[versionAux]["Modificado"].ToString()).AddHours(-3);
                            string usuarioNome = new SPFieldUserValue(item.Web, item.Versions[versionAux]["Modificado por"].ToString()).User.Name;
                            message.Append("Nome do Usuario: " + usuarioNome);
                            message.Append("</br>");
                            message.Append("Data do esclarecimento:" + dataModificacao.ToShortDateString() + " " + (dataModificacao.Hour <= 9 ? "0" + dataModificacao.Hour.ToString() : dataModificacao.Hour.ToString()) + ":" + (dataModificacao.Minute <= 9 ? "0" + dataModificacao.Minute.ToString() : dataModificacao.Minute.ToString()));

                        }
                    }
                    versionAux++;
                }*/
            }

            Email mail = new Email();

            if (Resp != null)
                mail.CC = Resp.User.Email + ";" + EmailsGrupoAdminsChamado(item.Web);
            else
                mail.CC = EmailsGrupoAdminsChamado(item.Web);

            mail.Assunto = "Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString() + " foi atualizada. Continuar o atendimento.";
            mail.Corpo = message.ToString() + MensagemEmailAutomatico();

            mail.EnviaEmail(item.Web);
        }

        public static void Email_4(SPListItem item)
        {
            StringDictionary headers = new StringDictionary();

            headers.Add("from", "aplicacoesweb@furnas.com.br");
            //SPUser Resp = item.Web.EnsureUser(Suporte.ValidaTextField(item["Responsavel"]));//.Split('|')[1]);
            SPFieldUserValue Resp = null;
            if (Suporte.ValidaTextField(item["Responsável"]) != string.Empty)
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
            message.Append("Descrição do Chamado:" + RetiraHTML(item["DescricaoChamado"].ToString()) + "</br>");
            Email mail = new Email();
            if (Resp != null)
                mail.CC = Resp.User.Email + ";" + EmailsGrupoAdminsChamado(item.Web);
            else
                mail.CC = EmailsGrupoAdminsChamado(item.Web);
            mail.Assunto = "Prazo previsto de atendimento da solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString() + " esgotou.";
            mail.Corpo = message.ToString() + MensagemEmailAutomatico();

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
            message.Append("Descrição do Chamado:" + RetiraHTML(item["DescricaoChamado"].ToString()));
            Email mail = new Email();
            mail.CC = EmailsGrupoAdminsChamado(item.Web);

            mail.Assunto = "Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString() + " com status Aberta desde " + date.ToString("dd/MM/yyyy");
            mail.Corpo = message.ToString() + MensagemEmailAutomatico();

            mail.EnviaEmail(item.Web);
        }

        /* Alterado SPRINT 7 */

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
            /* SPRINT 7 - 28876 */
            /* Tarefa - Alterar corpo do email com novas informações */
            message.Append("Solicitante: " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Author.User.Name.ToLower()));
            message.Append("</br>");
            message.Append("Status:" + status);
            message.Append("</br>");
            message.Append("Data da solicitação:" + item["Data da solicitação"].ToString().Substring(0, 10));
            message.Append("</br>");
            message.Append("Descrição do Chamado:" + RetiraHTML(item["DescricaoChamado"].ToString()) + "</br>");
            message.Append("</br>");
            message.Append("Esclarecimentos Solicitante: ");
            message.Append("</br></br>");
            message.Append("- " + esclarecimentos);
            message.Append("</br></br>");

            int version = item.Versions.Count - 1;

            DateTime dataModificacao = DateTime.Parse(item[SPBuiltInFieldId.Modified].ToString());

            message.Append("Nome do Usuario: " + item.Web.CurrentUser.Name);
            message.Append("</br>");
            message.Append("Data do esclarecimento: " + dataModificacao.ToShortDateString() + " " + dataModificacao.Hour + ":" + dataModificacao.Minute);
            message.Append("</br>");


            while (version >= 0)
            {
                string versoes = ValidaTextField(item.Versions[version]["Esclarecimentos"]);
                if (versoes != string.Empty)
                {

                    /*message.Append("Nome do Usuario: " + item.Versions[version]["Author"].ToString());
                    message.Append("</br>");
                    message.Append("Data do esclarecimento: " + DateTime.Parse(item.Versions[version]["Esclarecimentos"].ToString()));
                    message.Append("- " + versoes);*/
                    message.Append("</br>");
                    message.Append("- " + versoes);
                    message.Append("</br></br>");
                    message.Append("Nome do Usuario: " + item.Versions[version].CreatedBy.User.Name);
                    message.Append("</br>");
                    message.Append("Data do esclarecimento solicitante: " + dataModificacao.ToShortDateString() + " " + dataModificacao.Hour + ":" + dataModificacao.Minute);
                    message.Append("</br>");
                    message.Append("</br>");
                }
                version--;
            }

            version = item.Versions.Count - 1; int versionAux = 0;
            bool contemTitulo = false;
            while (version >= versionAux)
            {
                string versoes = ValidaTextField(item.Versions[versionAux]["Esclarecimentos Gerência"]);
                if (versoes != string.Empty)
                {
                    if (!contemTitulo)
                    {
                        message.Append("</br>");
                        message.Append("</br></br>");
                        message.Append("Esclarecimentos outra Gerência: ");
                        contemTitulo = true;
                    }
                    //DateTime dataModificacao = item.Versions[version].Created;

                    message.Append("</br>");
                    message.Append("</br>");
                    message.Append("- " + versoes);
                    message.Append("</br></br>");
                    DateTime dataModificacaos = Convert.ToDateTime(item.Versions[versionAux]["Modificado"].ToString()).AddHours(-3);
                    string usuarioNome = new SPFieldUserValue(item.Web, item.Versions[versionAux]["Modificado por"].ToString()).User.Name;
                    message.Append("Nome do Usuario: " + usuarioNome);
                    message.Append("</br>");
                    message.Append("Data do esclarecimento outra gerência: " + dataModificacaos.ToShortDateString() + " " + (dataModificacaos.Hour <= 9 ? "0" + dataModificacaos.Hour.ToString() : dataModificacaos.Hour.ToString()) + ":" + (dataModificacaos.Minute <= 9 ? "0" + dataModificacaos.Minute.ToString() : dataModificacaos.Minute.ToString()));
                }
                versionAux++;
            }

            Email mail = new Email();

            mail.CCO = EmailsGrupoAdminsChamado(item.Web);

            mail.CC = Author.User.Email + ";" + item["Imediato do Responsável"].ToString();
            mail.Assunto = "Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString() + " conforme está na solicitação necessita de novas informações para ser executada.";
            mail.Corpo = message.ToString() + MensagemEmailAutomatico();

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

            /* SPRINT 7 - 28876 */
            /* Tarefa - Alterar corpo do email com novas informações */
            message.Append("Solicitante: " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Author.User.Name.ToLower()));
            message.Append("</br>");

            message.Append("Status:" + status);
            message.Append("</br>");
            message.Append("Data da solicitação:" + item["Data da solicitação"].ToString().Substring(0, 10));
            message.Append("</br>");
            message.Append("Descrição do Chamado:" + RetiraHTML(item["DescricaoChamado"].ToString()) + "</br>");

            bool contemTitulo = false;
            int version = item.Versions.Count - 1; int versionAux = 0;
            List<string> ListEsclarecimentos = new List<string>();
            while (version >= versionAux)
            {
                string versoes = ValidaTextField(item.Versions[versionAux]["Esclarecimentos"]);
                if (versoes != string.Empty)
                {
                    if (!contemTitulo)
                    {
                        message.Append("</br>");
                        message.Append("Esclarecimentos Solicitante: ");
                        contemTitulo = true;
                    }
                    //ListEsclarecimentos.Add(versoes);
                    //DateTime dataModificacao = item.Versions[version].Created;
                    message.Append("</br>");
                    message.Append("</br>");
                    message.Append("- " + versoes);
                    message.Append("</br></br>");
                    DateTime dataModificacao = Convert.ToDateTime(item.Versions[versionAux]["Modificado"].ToString()).AddHours(-3);
                    string usuarioNome = new SPFieldUserValue(item.Web, item.Versions[versionAux]["Modificado por"].ToString()).User.Name;
                    message.Append("Nome do Usuario: " + usuarioNome);
                    message.Append("</br>");
                    message.Append("Data do esclarecimento solicitante: " + dataModificacao.ToShortDateString() + " " + (dataModificacao.Hour <= 9 ? "0" + dataModificacao.Hour.ToString() : dataModificacao.Hour.ToString()) + ":" + (dataModificacao.Minute <= 9 ? "0" + dataModificacao.Minute.ToString() : dataModificacao.Minute.ToString()));

                }

                versionAux++;
            }

            /*if (ListEsclarecimentos.Count > 0)
            {
                message.Append("</br>");
                message.Append("Esclarecimentos: ");
                foreach (string versoesEsclarecimentos in ListEsclarecimentos)
                {
                    message.Append("</br>");
                    message.Append("- " + versoesEsclarecimentos);
                    message.Append("</br>");
                    message.Append("Data de esclarecimento gerência:" + DateTime.Parse(item.Versions[version]["Esclarecimentos Gerência"].ToString()).ToShortDateString());

                }
            }*/

            //message.Append("</br></br>");
            version = item.Versions.Count - 1; versionAux = 0;
            contemTitulo = false;
            while (version >= versionAux)
            {
                string versoes = ValidaTextField(item.Versions[versionAux]["Esclarecimentos Gerência"]);
                if (versoes != string.Empty)
                {
                    if (!contemTitulo)
                    {
                        message.Append("</br></br>");
                        message.Append("</br>");
                        message.Append("Esclarecimentos outra Gerência: ");
                        contemTitulo = true;
                    }
                    //DateTime dataModificacao = item.Versions[version].Created;

                    message.Append("</br>");
                    message.Append("</br>");
                    message.Append("- " + versoes);
                    message.Append("</br></br>");
                    DateTime dataModificacao = Convert.ToDateTime(item.Versions[versionAux]["Modificado"].ToString()).AddHours(-3);
                    string usuarioNome = new SPFieldUserValue(item.Web, item.Versions[versionAux]["Modificado por"].ToString()).User.Name;
                    message.Append("Nome do Usuario: " + usuarioNome);
                    message.Append("</br>");
                    message.Append("Data do esclarecimento outra gerência: " + dataModificacao.ToShortDateString() + " " + (dataModificacao.Hour <= 9 ? "0" + dataModificacao.Hour.ToString() : dataModificacao.Hour.ToString()) + ":" + (dataModificacao.Minute <= 9 ? "0" + dataModificacao.Minute.ToString() : dataModificacao.Minute.ToString()));
                }
                versionAux++;
            }

            Email mail = new Email();
            //mail.CC = Author.User.Email + ";" + item["Imediato do Responsável"].ToString();
            mail.CC = Author.User.Email + ";" + item["Imediato do Responsável"].ToString() + ";" + usersGerencia + ";" + EmailsGrupoAdminsChamado(item.Web);
            mail.CCO = EmailsGrupoAdminsChamado(item.Web);
            mail.Assunto = "Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString() + " foi direcionada à Gerência responsável.";
            mail.Corpo = message.ToString() + MensagemEmailAutomatico();

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
            message.Append("Descrição do Chamado:" + item["DescricaoChamado"].ToString());
            message.Append("</br>");
            message.Append("Esclarecimento Gerência: ");
            int version = item.Versions.Count - 1;
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
            mail.CCO = EmailsGrupoAdminsChamado(item.Web);
            mail.Assunto = "Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString() + " foi direcionada à Gerência responsável.";
            mail.Corpo = message.ToString() + MensagemEmailAutomatico();

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
                    if (user.Email != "")
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
            SPFieldUserValue Author = new SPFieldUserValue(item.Web, item[SPBuiltInFieldId.Author].ToString());

            headers.Add("cc", Resp.Email);
            headers.Add("subject", "Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString() + " foi redirecionada para " + Resp.Name);
            headers.Add("content-type", "text/html");

            StringBuilder message = new StringBuilder();

            message.Append("<a href='" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "&Source=" + item.ParentList.ParentWeb.Url + "/Lists/Chamados" + "'>" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "</a>");
            message.Append("</br>");
            message.Append("Chamado: " + new SPFieldLookupValue(item["Tipo de Chamado"].ToString()).LookupValue);
            message.Append("</br>");
            message.Append("Solicitante: " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Author.User.Name.ToLower()));
            message.Append("</br>");
            message.Append("Status:" + item["Controle de status"].ToString());
            message.Append("</br>");
            message.Append("Data da solicitação:" + item["Data da solicitação"].ToString().Substring(0, 10));
            message.Append("</br>");
            message.Append("Descrição do Chamado:" + RetiraHTML(item["DescricaoChamado"].ToString()));

            Email mail = new Email();
            mail.CC = Resp.Email;

            mail.CCO = EmailsGrupoAdminsChamado(item.Web);
            mail.Assunto = "Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString() + " foi redirecionada para " + Resp.Name;
            mail.Corpo = message.ToString() + MensagemEmailAutomatico();

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
            mail.CCO = EmailsGrupoAdminsChamado(item.Web);
            mail.Assunto = "Solicitação de serviço " + item[SPBuiltInFieldId.Title].ToString();
            mail.Corpo = message.ToString() + MensagemEmailAutomatico();

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
            mail.CCO = EmailsGrupoAdminsChamado(item.Web);
            mail.Assunto = "Solicitação de serviço " + item.ListItem[SPBuiltInFieldId.Title].ToString();
            mail.Corpo = message.ToString() + MensagemEmailAutomatico();

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

               SPListItem item = list.GetItems(query).Cast<SPListItem>().FirstOrDefault();

               if (item != null)
                   ID = item.ID;

               ImpersonatedSite.Close(); ImpersonatedSite.Dispose();
           });

            return ID;
        }

        /* SPRINT 7 - 28876 */
        /*Incluir mensagem de email automático*/
        public static string MensagemEmailAutomatico()
        {
            StringBuilder message = new StringBuilder();

            message.Append("<br /><br /><br /><br />");
            message.Append("Este é um e-mail automático, ");
            message.Append("favor não responder. Todas as tratativas devem ser feitas através do link ");
            message.Append("informado no corpo de e-mail, ");
            message.Append("ou seja, diretamente no Portal do GGA.O. ");

            return message.ToString();

        }

        public static string RetiraHTML(string texto)
        {
            Regex regex = new Regex("\\<[^\\>]*\\>");
            return regex.Replace(texto, String.Empty);
        }
    }
}
