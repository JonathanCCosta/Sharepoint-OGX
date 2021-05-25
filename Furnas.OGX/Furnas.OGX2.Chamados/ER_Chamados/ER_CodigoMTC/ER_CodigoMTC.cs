using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using System.Text;

namespace Furnas.OGX2.Chamados.ER_Chamados.ER_CodigoMTC
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class ER_CodigoMTC : SPItemEventReceiver
    {

        public override void ItemCheckedIn(SPItemEventProperties properties)
        {
            base.ItemCheckedIn(properties);

            //base.EventFiringEnabled = false;

            //string codigo = GeraCodigoManualMTC(properties);

            //properties.ListItem["CodigoMTC"] = codigo;

            //properties.ListItem.Update();

            //base.EventFiringEnabled = true;
        }

       

        /// <summary>
        /// An item is being added.
        /// </summary>
        public override void ItemAdding(SPItemEventProperties properties)
        {
            base.ItemAdding(properties);

            base.EventFiringEnabled = false;

         

        }

        /// <summary>
        /// An item is being updated.
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            base.ItemUpdating(properties);
        }

        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);


            base.EventFiringEnabled = false;

            //string codigo = GeraCodigoManualMTC(properties);

            //properties.ListItem["CodigoMTC"] = codigo;

            //properties.ListItem.SystemUpdate();

            //base.EventFiringEnabled = true;
        }


        public string GeraCodigoManualMTC(SPItemEventProperties properties)
        {
           
            StringBuilder codigo = new StringBuilder();

            if (properties.ListItem["TipoMTC"] != null)
            {
                //tipoMTC = properties.ListItem["TipoMTC"].ToString();
                codigo.Append(new SPFieldLookupValue(properties.ListItem["TipoMTC"].ToString()).LookupValue + ".");

            }

            if (properties.ListItem["Grupo"] != null)
            {
                //grupo = properties.ListItem["GrupoMTC"].ToString();
                codigo.Append(new SPFieldLookupValue(properties.ListItem["Grupo"].ToString()).LookupValue);
            }

            if (properties.ListItem["Subgrupo"] != null)
            {
                //subGrupo = properties.ListItem["Subgrupo"].ToString();
                codigo.Append(new SPFieldLookupValue(properties.ListItem["Subgrupo"].ToString()).LookupValue + ".");

            }

            if (properties.ListItem["Fabricante"] != null)
            {
                //fabricante = properties.ListItem["Fabricante"].ToString();
                codigo.Append(new SPFieldLookupValue(properties.ListItem["Fabricante"].ToString()).LookupValue + ".00/");
            }

            if (properties.ListItem["Consecutivo"] != null)
            {
                //consecutivo = properties.ListItem["Consecutivo"].ToString();
                codigo.Append(properties.ListItem["Consecutivo"].ToString() + "-");
            }

            if (properties.ListItem["Revisao"] != null)
            {
                //revisao = "R" + properties.ListItem["Revisao"].ToString();
                codigo.Append("R" + properties.ListItem["Revisao"].ToString());
            }

        return codigo.ToString();
        }

    }
}