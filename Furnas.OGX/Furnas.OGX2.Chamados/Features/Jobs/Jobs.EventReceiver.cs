using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Furnas.OGX2.Chamados.Job;

namespace Furnas.OGX2.Chamados.Features.Jobs
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("ac340de0-f79c-4031-82df-7cb9a934bbb5")]
    public class JobsEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.
        public const string JobName = "Job - Controle Chamados GGA";

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
                /*foreach (SPJobDefinition current in sPSite.WebApplication.JobDefinitions)
                {
                    if (current.Name == JobName)
                    {
                        current.Delete();
                    }
                }*/

                new TimerJobChamados(JobName, parentWebApp) //sPSite.WebApplication)
                {
                    /*Schedule = new SPMinuteSchedule
                    {
                        BeginSecond=0,
                        Interval=20
                    }*/
                    Schedule = new SPDailySchedule
                    {
                        BeginHour =1
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
