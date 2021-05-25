using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Furnas.OGX2.MTC.JobsMTC;

namespace Furnas.OGX2.MTC.Features.Furnas.OGX.MTC.Job
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("d8c435a7-137e-49f7-b35d-e0f2c34f558a")]
    public class FurnasOGXMTCEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.
        public const string JobName = "Job - Controle MTC";

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                SPWebService.ContentService.RemoteAdministratorAccessDenied = false;
                SPWebService.ContentService.Update(true);
                //SPSite sPSite = properties.Feature.Parent as SPSite;
                SPWebApplication parentWebApp = (SPWebApplication)properties.Feature.Parent;
                foreach (SPJobDefinition job in parentWebApp.JobDefinitions)
                {
                    if (job.Name == JobName)
                    {
                        job.Delete();
                    }
                }

                new TimerJobMTC(JobName, parentWebApp) //sPSite.WebApplication)
                {
                    Schedule = new SPDailySchedule
                    {
                        BeginHour = 1
                    }
                }.Update();
                SPWebService.ContentService.RemoteAdministratorAccessDenied = true;
                SPWebService.ContentService.Update(true);
            });
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


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
