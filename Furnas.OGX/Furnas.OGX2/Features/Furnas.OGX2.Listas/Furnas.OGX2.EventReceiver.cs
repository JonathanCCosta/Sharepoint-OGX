using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;

namespace Furnas.OGX2.Features.Furnas.OGX2.Listas
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("6649ba19-6b42-4baa-8b8a-ca54b86b8b52")]
    public class FurnasOGX2EventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        private const string Admin = "Administradores GGA";
        private const string Colaboradores = "Colaboradores GGA";
        private const string Visualizadores = "Visualizadores GGA";

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb webcurrent = properties.Feature.Parent as SPWeb;
            SPUser userDefault = webcurrent.SiteAdministrators[0];

            //BreakWebRoleInheritance(webcurrent);

            //DESCOMENTAR, POIS É SÓ PRA MELHORIA
            /*CreateGroup(webcurrent, Admin, "Administradores da GGA", SPRoleType.Administrator, userDefault);
            CreateGroup(webcurrent, Colaboradores, "Colaboradores da GGA", SPRoleType.Contributor, userDefault);
            CreateGroup(webcurrent, Visualizadores, "Visualizadores da GGA", SPRoleType.Reader, userDefault);

            ChangeTitle(webcurrent);

            Service.RelacaoWebPart.GestaoInterna_X_Planos(webcurrent);
            Service.RelacaoWebPart.ScriptGestao(webcurrent);
            Service.RelacaoWebPart.ScriptCiclos(webcurrent);
            Service.RelacaoWebPart.ScriptPlanos(webcurrent);
            Service.RelacaoWebPart.ScriptPlanoMudanca(webcurrent);*/
        }

        private void BreakWebRoleLists(SPWeb webcurrent)
        {
            SPList list = webcurrent.Lists.TryGetList("Ciclos");
            list.BreakRoleInheritance(true,false);

            SPGroup group = webcurrent.SiteGroups[Colaboradores];
            SPRoleAssignment roleAssignments = new SPRoleAssignment(group);
            //SPRoleDefinition roleDefinitiona = webcurrent.RoleDefinitions.GetByType(SPRoleType.Contributor);
            //roleAssignments.RoleDefinitionBindings.Remove(roleDefinitiona);
            list.RoleAssignments.Remove(group);
            //roleAssignments.Update();
            //list.Update();

            /*SPRoleAssignment roleAssignment = new SPRoleAssignment(group);
            SPRoleDefinition roleDefinition = webcurrent.RoleDefinitions.GetByType(SPRoleType.Reader);
            roleAssignment.RoleDefinitionBindings.Add(roleDefinition);
            list.RoleAssignments.Add(roleAssignment);*/

            list.Update();
            
        }

        private void CreateGroup(SPWeb webcurrent, string groupname, string description, SPRoleType sPRoleType, params SPUser[] predefinedusers)
        {
            SPGroup group = null;

            try
            {
                group = webcurrent.SiteGroups[groupname];
            }
            catch { }

            if (group == null)
            {
                webcurrent.SiteGroups.Add(groupname, webcurrent.Site.Owner as SPMember, webcurrent.Site.Owner, description);
                group = webcurrent.SiteGroups[groupname];

                SPRoleDefinition roleDefinition = webcurrent.RoleDefinitions.GetByType(sPRoleType);
                SPRoleAssignment roleAssignment = new SPRoleAssignment(group);
                roleAssignment.RoleDefinitionBindings.Add(roleDefinition);
                webcurrent.RoleAssignments.Add(roleAssignment);
                webcurrent.Update();

                foreach (SPUser user in predefinedusers)
                    group.AddUser(user);
                group.Update();
            }
        }

        public void BreakWebRoleInheritance(SPWeb webcurrent)
        {
            bool allowunsafe = webcurrent.AllowUnsafeUpdates;
            webcurrent.AllowUnsafeUpdates = true;
            webcurrent.BreakRoleInheritance(false);
            webcurrent.Update();
            webcurrent.AllowUnsafeUpdates = allowunsafe;
        }

        private void ChangeTitle(SPWeb webcurrent)
        {
            SPList list = webcurrent.Lists.TryGetList("Ciclos");
            list.DisableGridEditing = true;
            SPField field = list.Fields[SPBuiltInFieldId.Title];
            field.Title = "Nome do Ciclo";
            field.Update();            

            list.BreakRoleInheritance(false, false);

            SPGroup groupC = webcurrent.SiteGroups[Colaboradores];
            SPGroup groupA = webcurrent.SiteGroups[Admin];
            
            SPRoleAssignment roleAssignmentC = new SPRoleAssignment(groupC);
            SPRoleAssignment roleAssignmentA = new SPRoleAssignment(groupA);
            
            SPRoleDefinition roleDefinitionC = webcurrent.RoleDefinitions.GetByType(SPRoleType.Reader);
            SPRoleDefinition roleDefinitionA = webcurrent.RoleDefinitions.GetByType(SPRoleType.Contributor);

            roleAssignmentC.RoleDefinitionBindings.Add(roleDefinitionC);
            roleAssignmentA.RoleDefinitionBindings.Add(roleDefinitionA);

            list.RoleAssignments.Add(roleAssignmentC);
            list.RoleAssignments.Add(roleAssignmentA);

            list.Update();

            SPList listP = webcurrent.Lists.TryGetList("Planos");
            listP.DisableGridEditing = true;
            SPField fieldP = listP.Fields[SPBuiltInFieldId.Title];
            fieldP.Title = "Numeração";
            fieldP.Update();
            SPField fieldModificado = listP.Fields[SPBuiltInFieldId.Modified];
            fieldModificado.Title = "Modificado em";
            fieldModificado.Update();
            
            listP.BreakRoleInheritance(false, false);

            listP.RoleAssignments.Add(roleAssignmentC);
            listP.RoleAssignments.Add(roleAssignmentA);

            listP.Update();

            SPList listGestaoInterna = webcurrent.Lists.TryGetList("Gestão Interna");
            listGestaoInterna.DisableGridEditing = true;
            //listGestaoInterna.NavigateForFormsPages = false;
            listGestaoInterna.Update();

            SPField fieldTitle = listGestaoInterna.Fields[SPBuiltInFieldId.Title];
            fieldTitle.DefaultValue = "Ver detalhes";
            fieldTitle.Update();
            /*SPField fieldModificadoGI = listGestaoInterna.Fields[SPBuiltInFieldId.Modified];
            fieldModificadoGI.Title = "Data da operação";
            fieldModificadoGI.Update();
            SPField fieldModificadoPorGI = listGestaoInterna.Fields[SPBuiltInFieldId.Editor];
            fieldModificadoPorGI.Title = "Identificação do Usuário";
            fieldModificadoPorGI.Update();*/

            SPList listIrma = webcurrent.Lists.TryGetList("Solicitação de Mudanças");
            listIrma.DisableGridEditing = true;
            listIrma.Update();

            SPField fieldTitleIrma = listIrma.Fields[SPBuiltInFieldId.Title];
            fieldTitleIrma.Title = "Ver detalhes";
            fieldTitleIrma.DefaultValue = "Ver detalhes";
            fieldTitleIrma.Required = false;
            fieldTitleIrma.ShowInDisplayForm = false;
            fieldTitleIrma.ShowInEditForm = false;
            fieldTitleIrma.ShowInNewForm = false;
            fieldTitleIrma.Update();

        }

        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            /*SPWeb webcurrent = properties.Feature.Parent as SPWeb;
            RemoveList(webcurrent, "Ciclos");*/
        }

        public void RemoveList(SPWeb currentweb, params string[] listnames)
        {
            foreach (string listname in listnames)
                currentweb.Lists[listname].Delete();
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
