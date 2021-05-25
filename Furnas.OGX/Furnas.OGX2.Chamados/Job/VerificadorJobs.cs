using Furnas.OGX2.Chamados.ServiceChamados;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furnas.OGX2.Chamados.Job
{
    public class VerificadorJobs
    {
        public static void ChamadosStatus(SPWeb web)
        {
            foreach (SPListItem item in web.Lists["Chamados"].GetItems())
            {
                if (item["Controle de status"].ToString() == "Em andamento")
                {
                    try
                    {
                        DateTime hoje = DateTime.Today.Date;
                        DateTime itemData = Convert.ToDateTime(item["Data da solicitação"].ToString()).Date;

                        int prazo = Convert.ToInt32(item["Prazo de Atendimento"].ToString());
                        int result = DateTime.Compare(hoje, itemData.AddDays(prazo));

                        if (result == 0 || result > 0)
                        {
                            //if (Suporte.ValidaTextField(item["Responsável"]) != string.Empty)
                                Suporte.Email_4(item);
                        }
                    }
                    catch (Exception ex1) { Suporte.Log(item.Web, ex1.Message, "Job - Em andamento - " + item[SPBuiltInFieldId.Title].ToString()); }
                }
                else if (item["Controle de status"].ToString() == "Aguardando informações")
                {
                    try
                    {
                        DateTime hoje = DateTime.Today.Date;
                        //DateTime itemData = Convert.ToDateTime(item[SPBuiltInFieldId.Modified].ToString()).Date;

                        DateTime itemData = Convert.ToDateTime(item[SPBuiltInFieldId.Modified].ToString()).Date;
                        int prazo = Convert.ToInt32(item["Prazo de Atendimento"].ToString());
                        int result = DateTime.Compare(hoje, itemData.AddDays(prazo));

                        if (result == 0 || result > 0)
                        {
                            item["Controle de status"] = "Encerrado";
                            item.Update();
                        }
                    }
                    catch (Exception ex1) { Suporte.Log(item.Web, ex1.Message, "Job - Aguardando Informação - " + item[SPBuiltInFieldId.Title].ToString()); }
                }
                else if (item["Controle de status"].ToString() == "Encaminhado à outra gerência da DO")
                {
                    try
                    {
                        DateTime hoje = DateTime.Today.Date;
                        DateTime itemData = Convert.ToDateTime(item[SPBuiltInFieldId.Modified].ToString()).Date;
                        
                        
                        int prazo = Convert.ToInt32(item["Prazo de Atendimento"].ToString());
                        int result = DateTime.Compare(hoje, itemData.AddDays(prazo));

                        if (result == 0)
                        {
                            /*EventFiring eventFiring = new EventFiring();
                            eventFiring.DisableHandleEventFiring();*/

                            Suporte.Email_Job_8(item, "Encaminhado à outra gerência da DO");

                            /*eventFiring.EnableHandleEventFiring();*/
                        }
                    }
                    catch (Exception ex1) { Suporte.Log(item.Web, ex1.Message, "Job - Encaminhado à outra gerência - " + item[SPBuiltInFieldId.Title].ToString()); }
                }
                else if (item["Controle de status"].ToString() == "Aberto")
                {
                    try
                    {
                        DateTime hoje = DateTime.Today.Date;
                        DateTime itemData = Convert.ToDateTime(item["Data da solicitação"].ToString()).Date;

                        int prazo = Convert.ToInt32(item["Prazo de Atendimento"].ToString());
                        int result = DateTime.Compare(hoje, itemData.AddDays(3));

                        DateTime itemDataMod = Convert.ToDateTime(item[SPBuiltInFieldId.Modified].ToString()).Date;
                        int resultMod = DateTime.Compare(hoje, itemDataMod.AddDays(3));

                        //if (result == 0 || result > 0 || resultMod == 0 || resultMod > 0)
                        if (result == 0 || resultMod == 0)//a cada 3 dias
                        {
                            //if(Suporte.ValidaTextField(item["Responsável"]) != string.Empty)
                            Suporte.Email_2(item);
                        }
                        else if (result > 0 || resultMod > 0)
                        {
                            int multiploResult = Convert.ToInt32((hoje - itemData).TotalDays);
                            int multiploResultMod = Convert.ToInt32((hoje - itemDataMod).TotalDays);
                            if (((multiploResult % 3) == 0) || ((multiploResultMod % 3) == 0))
                            {
                                Suporte.Email_2(item);
                            }
                        }
                    }
                    catch (Exception ex1) { Suporte.Log(item.Web, ex1.Message, "Job - Aberto - " + item[SPBuiltInFieldId.Title].ToString()); }
                }
            }

        }
    }
}
