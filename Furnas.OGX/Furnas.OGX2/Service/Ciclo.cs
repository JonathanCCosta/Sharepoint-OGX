using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furnas.OGX2.Service
{
    public class Ciclo
    {
        public static void AlteraCiclo(SPListItem ciclo)
        {
            string status = ciclo["Fluxo de Controle"].ToString();

            if (status == "Fechado")
            {
                List<SPListItem> planos = ciclo.Web.Lists["Planos"].Items.OfType<SPListItem>().Where(p => new SPFieldLookupValue(Convert.ToString(p["Ciclo"])).LookupId == ciclo.ID).ToList();

                if (planos.Count > 0)
                {
                    foreach (SPListItem planoMudanca in planos)
                    {
                        List<SPListItem> mudancas = ciclo.Web.Lists["Solicitação de Mudanças"].Items.OfType<SPListItem>().Where(p => Convert.ToString(p["Numeração"]) == Convert.ToString(planoMudanca[SPBuiltInFieldId.Title]) && Convert.ToString(p["Alteração Solicitada"]) == "Alteração").ToList();

                        if (mudancas.Count > 0)
                        {
                            foreach (SPListItem solicitacoes in mudancas)
                            {
                                Service.Permissao.FechaCiclo(solicitacoes);
                            }
                        }
                    }
                }
            }
            else
            {
                List<SPListItem> planos = ciclo.Web.Lists["Planos"].Items.OfType<SPListItem>().Where(p => new SPFieldLookupValue(Convert.ToString(p["Ciclo"])).LookupId == ciclo.ID).ToList();

                if (planos.Count > 0)
                {
                    foreach (SPListItem planoMudanca in planos)
                    {
                        List<SPListItem> mudancas = ciclo.Web.Lists["Solicitação de Mudanças"].Items.OfType<SPListItem>().Where(p => Convert.ToString(p["Numeração"]) == Convert.ToString(planoMudanca[SPBuiltInFieldId.Title])).ToList();

                        if (mudancas.Count > 0)
                        {
                            foreach (SPListItem solicitacoes in mudancas)
                            {
                                Service.Permissao.AbreCiclo(solicitacoes);
                            }
                        }
                    }
                }
            }
        }
    }
}
