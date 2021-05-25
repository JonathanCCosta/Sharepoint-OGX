using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using Furnas.OGX2.Chamados.ServiceChamados;

namespace Furnas.OGX2.Chamados.Features.Furnas.OGX2.ListaDeChamados
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("72a9edb7-a367-4630-9bc5-96110a4006a3")]
    public class FurnasOGX2EventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        private const string Admin = "Administradores GGA";
        private const string Colaboradores = "Colaboradores GGA";
        private const string ColaboradoresChamados = "Colaboradores GGA Chamados";
        private const string Visualizadores = "Visualizadores GGA";

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb webcurrent = properties.Feature.Parent as SPWeb;
            ConfiguraChamados.ChangeTitle(webcurrent);
            ConfiguraChamados.ChangeListasAuxiliares(webcurrent);
            ConfiguraChamados.ChangeModelo(webcurrent);
            ConfiguraChamados.ScriptChamados(webcurrent);
            ConfiguraChamados.Contador(webcurrent);
            ConfiguraChamados.ChangeListasTipoMTC(webcurrent);
            ConfiguraChamados.ChangeTitleAvalicaoChamados(webcurrent);
            ConfiguraChamados.ValueDefaultGrupoSubGrupoMTC(webcurrent);
            ConfiguraChamados.ValueDefaultFabricanteMTC(webcurrent);
            ConfiguraChamados.ChangeGerenciaResponsavel(webcurrent);
            //ConfiguraChamados.ChangeManualTecnico(webcurrent);
        }

        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWeb webcurrent = properties.Feature.Parent as SPWeb;
            webcurrent.Lists["Chamados"].Delete();
            webcurrent.Lists["Modelo de Chamado"].Delete();
            webcurrent.Lists["Tipo de Chamado"].Delete();
            webcurrent.Lists["Pesquisa de Satisfação"].Delete();
            webcurrent.Lists["Contador Chamado"].Delete();
            webcurrent.Lists["Manual Técnico"].Delete();
            webcurrent.Lists["Tipo MTC"].Delete();
            webcurrent.Lists["Gerência Responsável"].Delete();
            webcurrent.Lists["Grupo MTC"].Delete();
            webcurrent.Lists["Subgrupo MTC"].Delete();
            webcurrent.Lists["Fabricante"].Delete();
            webcurrent.Lists["Grupo MTC"].Delete();
            webcurrent.Lists["Gerência Responsável MTC"].Delete();
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
