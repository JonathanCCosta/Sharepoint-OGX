using Furnas.OGX2.MTC.ServicesMTC;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furnas.OGX2.MTC.JobsMTC
{
    public class JobVerificaMTC
    {
        public static void VerificaMTCemComentarios(SPWeb web)
        {
            try
            {
                SPList list = web.Lists["Manual Técnico de Campo"];

                List<SPListItem> items = list.Items.OfType<SPListItem>().Where(p =>
                    Convert.ToString(p["Controle de Status"]) == "Para comentários").ToList();

                DateTime hoje = DateTime.Today.Date;

                foreach (SPListItem item in items)
                {
                    try
                    {
                        DateTime itemData = Convert.ToDateTime(item["DataLimiteComentario"].ToString()).Date;
                        int result = DateTime.Compare(hoje, itemData);

                        if (result > 0)
                        {
                            item["Controle de Status"] = "Em consolidação";
                            item["Comentários"] = "Período de comentários foi encerrado.";

                            EventFiring eventFiring = new EventFiring();
                            eventFiring.DisableHandleEventFiring();

                            item.Update();

                            eventFiring.EnableHandleEventFiring();

                            EmailMTC.EmailEmConsolidacaoJob(item);
                            PermissaoMTC.PermissaoEmConsolidacaoJob(item);
                        }
                    }
                    catch { }
                }
            }
            catch { }
        }

        public static void VerificaMTC12Meses(SPWeb web)
        {
            try
            {
                SPList list = web.Lists["Manual Técnico de Campo"];

                List<SPListItem> items = list.Items.OfType<SPListItem>().Where(p =>
                    Convert.ToString(p["Controle de Status"]) == "Vigente").ToList();

                DateTime hoje = DateTime.Today.Date;

                List<SPListItem> vencidos = new List<SPListItem>();

                foreach (SPListItem item in items)
                {
                    DateTime itemData = Convert.ToDateTime(item["DataInicioVigencia"].ToString()).Date;
                    int result = DateTime.Compare(hoje.AddMonths(12), itemData);//igual a 1
                    int result2 = DateTime.Compare(itemData, hoje);// igual -1

                    if ((result == 0 || result > 0) && (result2 > 0 || result2 == 0))
                    {
                        vencidos.Add(item);
                    }
                }

                StringBuilder message = new StringBuilder();
                message.Append("<table border='1' style='font-size:12px;border-collapse: collapse;'>");
                message.Append("<tr style='font-weight:bold;font-size:13px;text-align:center;'><td width='150px'>Código MTC</td><td width='200px'>Data de Início de Vigência</td><td width='200px'>Prazo de Vigência (anos)</td><td width='300px'>Descrição do MTC</td><tr/>");
                foreach (SPListItem vencido in vencidos)
                {
                    message.Append("<tr>");
                    message.Append("<td style='text-align:center;'>" + vencido["CodigoMTC"].ToString() + "</td>");
                    DateTime data = Convert.ToDateTime(vencido["DataInicioVigencia"]);
                    message.Append("<td style='text-align:center;'>" + data.ToString("dd/MM/yyyy") + "</td>");
                    message.Append("<td style='text-align:center;'>" + vencido["PrazoVigencia"].ToString() + "</td>");
                    message.Append("<td>" + vencido["DescricaoMTC"].ToString() + "</td>");
                    message.Append("</tr>");
                }
                message.Append("</table>");

                if(vencidos.Count > 0)
                    EmailMTC.EmailVencimentoMeses(web, message, "Término de vigência MTC próximos 12 meses");
            }
            catch { }
        }

        public static void VerificaMTCMes(SPWeb web)
        {
            try
            {
                SPList list = web.Lists["Manual Técnico de Campo"];

                List<SPListItem> items = list.Items.OfType<SPListItem>().Where(p =>
                    Convert.ToString(p["Controle de Status"]) == "Vigente").ToList();

                DateTime hoje = DateTime.Today.Date;

                List<SPListItem> vencidos = new List<SPListItem>();

                foreach (SPListItem item in items)
                {
                    DateTime itemData = Convert.ToDateTime(item["DataInicioVigencia"].ToString()).Date;
                    
                    if (itemData.Month == hoje.Month && itemData.Year == hoje.Year)
                    {
                        vencidos.Add(item);
                    }
                }

                StringBuilder message = new StringBuilder();
                message.Append("<table border='1' style='font-size:12px;border-collapse: collapse;'>");
                message.Append("<tr style='font-weight:bold;font-size:13px;text-align:center;'><td width='150px'>Código MTC</td><td width='200px'>Data de Início de Vigência</td><td width='200px'>Prazo de Vigência (anos)</td><td width='300px'>Descrição do MTC</td><tr/>");
                foreach (SPListItem vencido in vencidos)
                {
                    message.Append("<tr>");
                    message.Append("<td style='text-align:center;'>" + vencido["CodigoMTC"].ToString() + "</td>");
                    DateTime data = Convert.ToDateTime(vencido["DataInicioVigencia"]);
                    message.Append("<td style='text-align:center;'>" + data.ToString("dd/MM/yyyy") + "</td>");
                    message.Append("<td style='text-align:center;'>" + vencido["PrazoVigencia"].ToString() + "</td>");
                    message.Append("<td>" + vencido["DescricaoMTC"].ToString() + "</td>");
                    message.Append("</tr>");
                }
                message.Append("</table>");

                if(vencidos.Count > 0)
                    EmailMTC.EmailVencimentoMeses(web, message, "Término de vigência MTC mensal");
            }
            catch { }

            try
            {
                string path = @"C:\DocumentosMTC\";
                DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
            }
            catch { }
        }
    }
}
