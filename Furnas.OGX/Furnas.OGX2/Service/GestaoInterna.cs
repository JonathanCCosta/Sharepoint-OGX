using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Furnas.OGX2.Service;

namespace Furnas.OGX2.Service
{
    public class GestaoInterna
    {
        public static void HistoricoGestao(SPListItem item)
        {
            try
            {
                if (Convert.ToString(item["Aprovação da solicitação"]) == "Aprovado")
                {
                    //int ID = new SPFieldLookupValue(Convert.ToString(item["No. SGPMR"])).LookupId;
                    string ID = Convert.ToString(item["No. SGPMR"]);
                    //List<SPListItem> items = item.Web.Lists["Gestão Interna"].Items.OfType<SPListItem>().Where(p => new SPFieldLookupValue(Convert.ToString(p["No. SGPMR"])).LookupId == ID && p.ID != item.ID && Convert.ToString(p["Histórico"]) == "Não").ToList();
                    List<SPListItem> items = item.Web.Lists["Gestão Interna"].Items.OfType<SPListItem>().Where(p => Convert.ToString(p["No. SGPMR"]) == ID && p.ID != item.ID && Convert.ToString(p["Histórico"]) == "Não").ToList();

                    foreach (SPListItem itemH in items)
                    {
                        EventFiring eventFiring = new EventFiring();
                        eventFiring.DisableHandleEventFiring();

                        itemH["Histórico"] = "Sim";
                        itemH.Update();

                        eventFiring.EnableHandleEventFiring();
                    }
                }
                else if (Convert.ToString(item["Aprovação da solicitação"]) == "Rejeitado")
                {
                    EventFiring eventFiring = new EventFiring();
                    eventFiring.DisableHandleEventFiring();

                    item["Histórico"] = "Sim";
                    item.Update();

                    eventFiring.EnableHandleEventFiring();
                }
            }
            catch { }
        }

        public static void HistoricoGestaoUpdate(SPListItem item)
        {
            try
            {
                //int ID = new SPFieldLookupValue(Convert.ToString(item["No. SGPMR"])).LookupId;
                string ID = Convert.ToString(item["No. SGPMR"]);
                //List<SPListItem> items = item.Web.Lists["Gestão Interna"].Items.OfType<SPListItem>().Where(p => new SPFieldLookupValue(Convert.ToString(p["No. SGPMR"])).LookupId == ID && p.ID != item.ID && Convert.ToString(p["Histórico"]) == "Não").ToList();
                List<SPListItem> items = item.Web.Lists["Gestão Interna"].Items.OfType<SPListItem>().Where(p => Convert.ToString(p["No. SGPMR"]) == ID && p.ID != item.ID && Convert.ToString(p["Histórico"]) == "Não").ToList();

                foreach (SPListItem itemH in items)
                {
                    EventFiring eventFiring = new EventFiring();
                    eventFiring.DisableHandleEventFiring();

                    itemH["Histórico"] = "Sim";
                    itemH.Update();

                    eventFiring.EnableHandleEventFiring();
                }
            }
            catch { }
        }

        public static void HistoricoGestaoRejeita(SPListItem item)
        {
            try
            {
                EventFiring eventFiring = new EventFiring();
                eventFiring.DisableHandleEventFiring();
                item.Web.AllowUnsafeUpdates = true;
                
                item["Histórico"] = "Sim";
                item.Update();
                
                item.Web.AllowUnsafeUpdates = false;
                eventFiring.EnableHandleEventFiring();
            }
            catch { }
        }

        public static void DeleteGestaoInterna(SPListItem item)
        {
            if (Convert.ToString(item["Histórico"]) == "Não")
            {
                //int ID = new SPFieldLookupValue(Convert.ToString(item["No. SGPMR"])).LookupId;
                //SPListItem itemH = item.Web.Lists["Gestão Interna"].Items.OfType<SPListItem>().Where(p => p.ID != item.ID && Convert.ToString(p["Aprovação da solicitação"]) == "Aprovada").FirstOrDefault();
                //SPListItem itemH  = item.Web.Lists["Gestão Interna"].Items.OfType<SPListItem>().Where(p => new SPFieldLookupValue(Convert.ToString(p["No. SGPMR"])).LookupId == ID && p.ID != item.ID && Convert.ToString(p["Aprovação da solicitação"]) == "Aprovado").Last();
                string ID = Convert.ToString(item["No. SGPMR"]);
                SPListItem itemH = item.Web.Lists["Gestão Interna"].Items.OfType<SPListItem>().Where(p => Convert.ToString(p["No. SGPMR"]) == ID && p.ID != item.ID && Convert.ToString(p["Aprovação da solicitação"]) == "Aprovado").Last();
                if (itemH != null)
                {
                    EventFiring eventFiring = new EventFiring();
                    eventFiring.DisableHandleEventFiring();

                    itemH["Histórico"] = "Não";
                    itemH.Update();

                    eventFiring.EnableHandleEventFiring();
                }
            }
        }

        public static void EmailMudancaStatus(SPListItem item, string status)
        {
            StringBuilder message = new StringBuilder();

            message.Append("O status da Gestão Interna do Plano <a href='" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "'>" + DuplicaItens.ValidaTextField(item["No. SGPMR"]) + "</a> foi alterado para Status " + status + ".");
            message.Append("</br>");
            //O status da solicitação <Nome do Plano> foi alterado para Status <Status>.

            EmailSGPMR.Email mail = new EmailSGPMR.Email();
            SPFieldUserValue Author = new SPFieldUserValue(item.Web, item[SPBuiltInFieldId.Author].ToString());
            mail.CC = Author.User.Email;

            mail.Assunto = "Atualização de Status da Gestão Interna do SGPMR";
            mail.Corpo = message.ToString();

            mail.EnviaEmail(item.Web);
        }
    }
}
