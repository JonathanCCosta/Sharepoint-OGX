using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Furnas.OGX2.MTC.ServicesMTC;

namespace Furnas.OGX2.MTC.Features.Furnas.OGX.MTC.Recursos
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("31df81e4-bcd3-437e-96e6-8932ccf53c26")]
    public class FurnasOGXMTCEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.
        private const string Admin = "Administradores GGA MTC";
        private const string Colab = "Colaboradores GGA MTC";
        //private const string AdminSite = "Administradores Site";

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb webcurrent = properties.Feature.Parent as SPWeb;

            try
            {
                SPUser userDefault = webcurrent.SiteAdministrators[0];
                ConfiguraMTC.CreateGroup(webcurrent, Admin, "Administradores da GGA MTC", SPRoleType.Administrator, userDefault);
                //ConfiguraMTC.CreateGroup(webcurrent, Admin, "Administradores da GGA Site", SPRoleType.Administrator, userDefault);
                ConfiguraMTC.CreateGroup(webcurrent, Colab, "Colaboradores da GGA MTC", SPRoleType.Contributor, userDefault);
            }
            catch { }

            ConfiguraMTC.ChangeManualTecnico(webcurrent);
            ConfiguraMTC.InseriWebParts(webcurrent);
            ConfiguraMTC.Manual_x_Docs(webcurrent);
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWeb webcurrent = properties.Feature.Parent as SPWeb;
            webcurrent.Lists["Manual Técnico de Campo"].Delete();
            webcurrent.Lists["Documentos Manual Técnico de Campo"].Delete();
        }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
