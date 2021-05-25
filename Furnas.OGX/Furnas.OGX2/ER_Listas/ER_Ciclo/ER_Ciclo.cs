using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace Furnas.OGX2.ER_Listas.ER_Ciclo
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class ER_Ciclo : SPItemEventReceiver
    {
        /// <summary>
        /// An item was updated.
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);
            try
            {
                Service.Ciclo.AlteraCiclo(properties.ListItem);
            }
            catch (Exception e)
            {
                Log(properties.ListItem.Web, e.Message.ToString(), "Alteração de Permissão Ciclo");
            }
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
    }
}