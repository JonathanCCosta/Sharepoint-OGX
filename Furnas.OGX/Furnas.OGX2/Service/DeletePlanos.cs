using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furnas.OGX2.Service
{
    public class DeletePlanos
    {
        public static int DeletePlanosEmCascata(SPWeb web, string plano, string nomeCiclo, int ciclo)
        {
            SPList listPlanos = web.Lists["Planos"];
            //bool retorno = listPlanos.Items.OfType<SPListItem>().Any(p => ciclo == new SPFieldLookupValue(Convert.ToString(p["Ciclo"])).LookupId && plano == p.ContentType.Name);

            try
            {
                return DeletePlanosEmMassa(ciclo, plano, listPlanos);
            }
            catch (Exception e)
            {
                return 0;
                throw new Exception(e.Message);
            }
        }

        public static int DeletePlanosEmMassa(int ciclo, string Plano, SPList list)
        {
            SPQuery query = new SPQuery();
            query.Query = string.Format("<Where><And><Eq><FieldRef Name='ContentType'/><Value Type='Computed'>{0}</Value></Eq><Eq><FieldRef Name='Ciclo' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq></And></Where>", Plano, ciclo);

            SPListItemCollection collItem = list.GetItems(query);

            if (collItem.Count == 0)
                return 0;
            else
            {
                int idDoc = Convert.ToInt32(collItem[0]["ID Documento"]);

                if (collItem != null)
                {
                    while (collItem.Count > 0)
                    {
                        DeletePlanosMudancaEmMassa(list.ParentWeb, Convert.ToString(collItem[0][SPBuiltInFieldId.Title]));
                        DeleteGestaoInternaEmMassa(list.ParentWeb, Convert.ToString(collItem[0][SPBuiltInFieldId.Title]));
                        collItem[0].Delete();
                    }
                }

                return idDoc;
            }
        }

        public static void DeletePlanosMudancaEmMassa(SPWeb web, string numeracao)
        {
            try
            {
                SPQuery query = new SPQuery();
                query.Query = string.Format("<Where><Eq><FieldRef Name='NumeracaoIrma' /><Value Type='Text'>{0}</Value></Eq></Where>", numeracao);

                SPListItemCollection collItem = web.Lists["Solicitação de Mudanças de planos importados"].GetItems(query);

                if (collItem.Count == 0)
                    return;
                else
                {
                    if (collItem != null)
                    {
                        while (collItem.Count > 0)
                        {
                            collItem[0].Delete();
                        }
                    }
                }
            }
            catch { }
        }

        public static void DeleteGestaoInternaEmMassa(SPWeb web, string numeracao)
        {
            try
            {
                SPQuery query = new SPQuery();
                //query.Query = string.Format("<Where><Eq><FieldRef Name='NumSGPMR' /><Value Type='Lookup'>{0}</Value></Eq></Where>", numeracao);
                query.Query = string.Format("<Where><Eq><FieldRef Name='NumeroSGPMR' /><Value Type='Text'>{0}</Value></Eq></Where>", numeracao);

                SPListItemCollection collItem = web.Lists["Gestão Interna"].GetItems(query);

                if (collItem.Count == 0)
                    return;
                else
                {
                    if (collItem != null)
                    {
                        while (collItem.Count > 0)
                        {
                            collItem[0].Delete();
                        }
                    }
                }
            }
            catch { }
        }
    }
}
