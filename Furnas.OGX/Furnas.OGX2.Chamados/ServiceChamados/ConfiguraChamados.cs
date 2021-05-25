using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using System.Xml;

namespace Furnas.OGX2.Chamados.ServiceChamados
{
    public class ConfiguraChamados
    {
        private const string Admin = "Administradores GGA";
        private const string AdminChamados = "Administradores GGA Chamados";
        private const string Colaboradores = "Colaboradores GGA";
        private const string ColaboradoresChamdos = "Colaboradores GGA Chamados";
        private const string ColaboradoresMTC = "Colaboradores Manual Técnico";
        private const string Visualizadores = "Visualizadores GGA";

        public static void ChangeTitle(SPWeb webcurrent)
        {
            SPList list = webcurrent.Lists.TryGetList("Chamados");
            list.DisableGridEditing = true;

            SPField field = list.Fields[SPBuiltInFieldId.Title];
            field.Title = "Número do chamado";
            field.Required = true;
            field.Update();

            SPField criado = list.Fields[SPBuiltInFieldId.Created];
            criado.Title = "Data da solicitação";
            criado.Update();

            SPField criadoPor = list.Fields[SPBuiltInFieldId.Author];
            criadoPor.Title = "Nome do solicitante";
            criadoPor.Update();

            list.EnableVersioning = true;
            list.Update();

            try
            {
                //    if (!GroupExistsInWebSite(webcurrent, AdminChamados))
                //    {
                //        SPUser userDefault = webcurrent.SiteAdministrators[0];
                //        webcurrent.SiteGroups.Add(AdminChamados, userDefault, null, AdminChamados);
                //        webcurrent.SiteGroups.Add(AdminChamados, userDefault, null, AdminChamados);
                //    }
                SPUser userDefault = webcurrent.SiteAdministrators[0];
                CreateGroup(webcurrent, AdminChamados, "Administradores da GGA Chamados", SPRoleType.Administrator, userDefault);
                CreateGroup(webcurrent, ColaboradoresChamdos, "Colaboradores da GGA Chamados", SPRoleType.Contributor, userDefault);
                //CreateGroup(webcurrent, AdminChamados, "Colaboradores da GGA Chamados", SPRoleType.Administrator, userDefault);
            }
            catch { }
        }

        public static void ChangeTitleAvalicaoChamados(SPWeb webcurrent)
        {
            SPList list = webcurrent.Lists.TryGetList("Pesquisa de Satisfação");
            list.DisableGridEditing = true;
            list.NavigateForFormsPages = false;

            SPField field = list.Fields[SPBuiltInFieldId.Title];
            field.Title = "Ver detalhes";
            field.Update();

            SPFile allForm = webcurrent.GetFile(list.DefaultViewUrl);
            SPLimitedWebPartManager wpmE = allForm.GetLimitedWebPartManager(PersonalizationScope.Shared);

            if (wpmE.WebParts.Count == 1)
            {
                // create our List View web part
                ScriptEditorWebPart wpE = new ScriptEditorWebPart();
                wpE.Content = "<script src='" + webcurrent.ServerRelativeUrl + "/SiteAssets/JSChamados/Pesquisa.js'></script>";
                wpmE.AddWebPart(wpE, "Main", 2);
                allForm.Update();
            }

            list.Update();
        }


        public static void ValueDefaultFabricanteMTC(SPWeb webcurrent)
        {
            SPList list = webcurrent.Lists.TryGetList("Grupo MTC");
            SPList listSubGrupo = webcurrent.Lists.TryGetList("SubGrupo MTC");
            list.DisableGridEditing = true;
            listSubGrupo.DisableGridEditing = true;
            //list.NavigateForFormsPages = false;

            //SPField field = list.Fields["Código"];
            //SPField fieldSubGrupo = list.Fields["Código"];
            //field.Title = "Ver detalhes";

            //field.Indexed = true;
            //field.EnforceUniqueValues = true;
            //field.Update();

            //fieldSubGrupo.Indexed = true;
            //fieldSubGrupo.EnforceUniqueValues = true;
            //fieldSubGrupo.Update();

            SPFile allForm = webcurrent.GetFile(list.DefaultNewFormUrl);
            SPFile allFormSubGrupo = webcurrent.GetFile(listSubGrupo.DefaultNewFormUrl);

            SPFile editForm = webcurrent.GetFile(list.DefaultEditFormUrl);
            SPFile editFormSubGrupo = webcurrent.GetFile(listSubGrupo.DefaultEditFormUrl);

            SPLimitedWebPartManager wpmE = allForm.GetLimitedWebPartManager(PersonalizationScope.Shared);
            SPLimitedWebPartManager wpmESubGrupo = allFormSubGrupo.GetLimitedWebPartManager(PersonalizationScope.Shared);

            SPLimitedWebPartManager wpmEdit = editForm.GetLimitedWebPartManager(PersonalizationScope.Shared);
            SPLimitedWebPartManager wpmEditSubGrupo = editForm.GetLimitedWebPartManager(PersonalizationScope.Shared);

            if (wpmE.WebParts.Count == 1 && wpmEdit.WebParts.Count == 1)
            {
                // create our List View web part
                ScriptEditorWebPart wpE = new ScriptEditorWebPart();
                wpE.Content = "<script src='" + webcurrent.ServerRelativeUrl + "/SiteAssets/JSChamados/CarregaGrupoMTC.js'></script>";
                wpmE.AddWebPart(wpE, "Main", 2);
               
                
                allForm.Update();
                editForm.Update();

            }

            list.Update();


            if (wpmESubGrupo.WebParts.Count == 1 && wpmEditSubGrupo.WebParts.Count == 1)
            {
                // create our List View web part
                ScriptEditorWebPart wpE = new ScriptEditorWebPart();
                wpE.Content = "<script src='" + webcurrent.ServerRelativeUrl + "/SiteAssets/JSChamados/CarregaGrupoMTC.js'></script>";
                wpmESubGrupo.AddWebPart(wpE, "Main", 2);

                wpmEditSubGrupo.AddWebPart(wpE, "Main", 2);

                allFormSubGrupo.Update();
                editFormSubGrupo.Update();

            }


            listSubGrupo.Update();
        }


        public static void ValueDefaultGrupoSubGrupoMTC(SPWeb webcurrent)
        {
            SPList list = webcurrent.Lists.TryGetList("Fabricante MTC");
           
            list.DisableGridEditing = true;
           
            //list.NavigateForFormsPages = false;

            SPField field = list.Fields["Código do Fabricante"];
           
            //field.Title = "Ver detalhes";

            field.Indexed = true;
            field.EnforceUniqueValues = true;
            field.Update();

            SPFile allForm = webcurrent.GetFile(list.DefaultNewFormUrl);
           

            SPFile editForm = webcurrent.GetFile(list.DefaultEditFormUrl);
            
            SPLimitedWebPartManager wpmE = allForm.GetLimitedWebPartManager(PersonalizationScope.Shared);
           
            SPLimitedWebPartManager wpmEdit = editForm.GetLimitedWebPartManager(PersonalizationScope.Shared);
            
            if (wpmE.WebParts.Count == 1)
            {
                // create our List View web part
                ScriptEditorWebPart wpE = new ScriptEditorWebPart();
                wpE.Content = "<script src='" + webcurrent.ServerRelativeUrl + "/SiteAssets/JSChamados/FabricanteMTC.js'></script>";
                wpmE.AddWebPart(wpE, "Main", 2);


                allForm.Update();
                editForm.Update();

            }

            list.Update();


        }




        public static void ChangeManualTecnico(SPWeb webcurrent)
        {
            SPList list = webcurrent.Lists.TryGetList("Manual Técnico");
            //list.DisableGridEditing = true;
            list.NavigateForFormsPages = false;

            SPField field = list.Fields["Código MTC"];
            //field.Title = "Código do MTC";
            field.Indexed = true;
            field.EnforceUniqueValues = true;
            field.Update();

            /*SPFieldLookup coll = (SPFieldLookup)list.Fields["Tipo MTC"];
            coll.Indexed = true;
            coll.RelationshipDeleteBehavior = SPRelationshipDeleteBehavior.Restrict;
            coll.Update();*/

            SPFile allForm = webcurrent.GetFile(list.DefaultEditFormUrl);
            SPLimitedWebPartManager wpmE = allForm.GetLimitedWebPartManager(PersonalizationScope.Shared);

            if (wpmE.WebParts.Count == 1)
            {
                // create our List View web part
                ScriptEditorWebPart wpE = new ScriptEditorWebPart();
                wpE.Content = "<script src='" + webcurrent.ServerRelativeUrl + "/SiteAssets/JSManual/ManualTecnico.js'></script>";
                wpmE.AddWebPart(wpE, "Main", 2);
                allForm.Update();
            }

            if (!GroupExistsInWebSite(webcurrent, ColaboradoresMTC))
            {
                SPUser userDefault = webcurrent.SiteAdministrators[0];
                webcurrent.SiteGroups.Add(ColaboradoresMTC, userDefault, userDefault, ColaboradoresMTC);
            }

            list.BreakRoleInheritance(false, false);

            SPGroup groupC = webcurrent.SiteGroups[Colaboradores];
            SPGroup groupD = webcurrent.SiteGroups[ColaboradoresMTC];
            SPGroup groupA = webcurrent.SiteGroups[Admin];
            SPGroup groupB = webcurrent.SiteGroups[Visualizadores];

            SPRoleAssignment roleAssignmentC = new SPRoleAssignment(groupC);
            SPRoleAssignment roleAssignmentD = new SPRoleAssignment(groupD);
            SPRoleAssignment roleAssignmentA = new SPRoleAssignment(groupA);
            SPRoleAssignment roleAssignmentB = new SPRoleAssignment(groupB);

            SPRoleDefinition roleDefinitionC = webcurrent.RoleDefinitions.GetByType(SPRoleType.Reader);
            SPRoleDefinition roleDefinitionA = webcurrent.RoleDefinitions.GetByType(SPRoleType.Administrator);
            SPRoleDefinition roleDefinitionB = webcurrent.RoleDefinitions.GetByType(SPRoleType.Contributor);

            roleAssignmentC.RoleDefinitionBindings.Add(roleDefinitionC);
            roleAssignmentD.RoleDefinitionBindings.Add(roleDefinitionB);
            roleAssignmentA.RoleDefinitionBindings.Add(roleDefinitionA);
            roleAssignmentB.RoleDefinitionBindings.Add(roleDefinitionC);

            list.RoleAssignments.Add(roleAssignmentC);
            list.RoleAssignments.Add(roleAssignmentD);
            list.RoleAssignments.Add(roleAssignmentA);
            list.RoleAssignments.Add(roleAssignmentB);

            list.Update();
        }

        public static void ChangeGerenciaResponsavel(SPWeb webcurrent)
        {
            SPList list = webcurrent.Lists.TryGetList("Gerência Responsável");

            SPList listGerenciaResponsavel = webcurrent.Lists.TryGetList("Gerência Responsável MTC");

            list.DisableGridEditing = true;
            listGerenciaResponsavel.DisableGridEditing = true;
            //list.NavigateForFormsPages = false;

            SPField field = list.Fields[SPBuiltInFieldId.Title];
            SPField fieldGerenciaResponasvel = listGerenciaResponsavel.Fields[SPBuiltInFieldId.Title];

            field.Title = "Órgão";
            field.Indexed = true;
            field.EnforceUniqueValues = true;
            field.Update();

            fieldGerenciaResponasvel.Title = "Órgão";
            fieldGerenciaResponasvel.Indexed = true;
            fieldGerenciaResponasvel.EnforceUniqueValues = true;
            fieldGerenciaResponasvel.Update();

            SPFile allForm = webcurrent.GetFile(list.DefaultNewFormUrl);
            SPFile allFormGerenciaResponsavel = webcurrent.GetFile(listGerenciaResponsavel.DefaultNewFormUrl);

            SPLimitedWebPartManager wpmE = allForm.GetLimitedWebPartManager(PersonalizationScope.Shared);
            SPLimitedWebPartManager wpmEGerenciaResponsavel = allFormGerenciaResponsavel.GetLimitedWebPartManager(PersonalizationScope.Shared);

            if (wpmE.WebParts.Count == 1 || wpmEGerenciaResponsavel.WebParts.Count == 1)
            {
                // create our List View web part
                ScriptEditorWebPart wpE = new ScriptEditorWebPart();
                wpE.Content = "<script src='" + webcurrent.ServerRelativeUrl + "/SiteAssets/JSChamados/CarregaGerencia.js'></script>";
                wpmE.AddWebPart(wpE, "Main", 3);
                wpmEGerenciaResponsavel.AddWebPart(wpE, "Main", 3);

                ContentEditorWebPart wpAE = new ContentEditorWebPart();
                XmlDocument xmlDoc = new XmlDocument();
                XmlElement xmlElement = xmlDoc.CreateElement("Root");
                xmlElement.InnerText = //"<link rel='stylesheet' type='text/css' href='" + web.ServerRelativeUrl + "/SiteAssets/JSChamados/carregaModelo.css' />" +
                    //"<div id='containerModelos'></div>" +
                    "<script type='text/javascript' src='http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js'></script>" +
                    "<script>" +
                    "function ShowProgress() {" +
                    "        setTimeout(function () {" +
                    "            var modal = $('<div />');" +
                    "            modal.addClass('modal');" +
                    "            $('body').append(modal);" +
                    "            var loading = $('.loading');" +
                    "            loading.show();" +
                    "            var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);" +
                    "            var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);" +
                    "            loading.css({ top: top, left: left });" +
                    "        }, 200);" +
                    "}" +
                    "</script>" +
                    "<style>" +
                     " .modal{" +
                     "       position: fixed;" +
                     "       top: 0;" +
                     "       left: 0;" +
                     "       background-color: black;" +
                     "       z-index: 99;" +
                     "       opacity: 0.3;" +
                     "       filter: alpha(opacity=30);" +
                     "       -moz-opacity: 0.3;" +
                     "       min-height: 100%;" +
                     "       width: 100%;" +
                     "   }" +
                     "   .loading{" +
                     "       text-align: center;" +
                     "       font-family: Arial;" +
                     "       font-size: 10pt;" +
                     "       border: 2px solid #67CFF5;" +
                     "       width: 200px;" +
                     "       height: 110px;" +
                     "       display: none;" +
                     "       position: fixed;" +
                     "       background-color: White;" +
                     "       z-index: 999;" +
                     "       font-weight:bold;" +
                     "}" +
                "</style>" +
                "<div class='loading' style='display: none; top: 271.5px; left: 578px;'>" +
                "<br/>Carregando, por favor aguarde...<br/><br/>" +
                "<img src='../../SiteAssets/CSS/ajax-loader.gif' alt=''/>" +
                "<br/></div> ";
                wpAE.Content = xmlElement;
                wpAE.Content.InnerText = xmlElement.InnerText;
                wpmE.AddWebPart(wpAE, "Main", 2);
                wpmEGerenciaResponsavel.AddWebPart(wpAE, "Main", 2);

                allForm.Update();
                allFormGerenciaResponsavel.Update();
            }

            SPFile editForm = webcurrent.GetFile(list.DefaultEditFormUrl);
            SPFile editFormGerenciaResponsavel = webcurrent.GetFile(listGerenciaResponsavel.DefaultEditFormUrl);

            SPLimitedWebPartManager wpmEdit = editForm.GetLimitedWebPartManager(PersonalizationScope.Shared);
            SPLimitedWebPartManager wpmEditGerenciaResponsaveGerencia = editFormGerenciaResponsavel.GetLimitedWebPartManager(PersonalizationScope.Shared);

            if (wpmEdit.WebParts.Count == 1 || wpmEditGerenciaResponsaveGerencia.WebParts.Count == 1)
            {
                // create our List View web part
                ScriptEditorWebPart wpEdit = new ScriptEditorWebPart();
                wpEdit.Content = "<script src='" + webcurrent.ServerRelativeUrl + "/SiteAssets/JSChamados/CarregaGerenciaEdit.js'></script>";
                wpmEdit.AddWebPart(wpEdit, "Main", 3);
                wpmEditGerenciaResponsaveGerencia.AddWebPart(wpEdit, "Main", 3);

                ContentEditorWebPart wpAEd = new ContentEditorWebPart();
                XmlDocument xmlDoc = new XmlDocument();
                XmlElement xmlElement = xmlDoc.CreateElement("Root");
                xmlElement.InnerText =
                    "<script type='text/javascript' src='http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js'></script>" +
                    "<script>" +
                    "function ShowProgress() {" +
                    "        setTimeout(function () {" +
                    "            var modal = $('<div />');" +
                    "            modal.addClass('modal');" +
                    "            $('body').append(modal);" +
                    "            var loading = $('.loading');" +
                    "            loading.show();" +
                    "            var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);" +
                    "            var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);" +
                    "            loading.css({ top: top, left: left });" +
                    "        }, 200);" +
                    "}" +
                    "</script>" +
                    "<style>" +
                     " .modal{" +
                     "       position: fixed;" +
                     "       top: 0;" +
                     "       left: 0;" +
                     "       background-color: black;" +
                     "       z-index: 99;" +
                     "       opacity: 0.3;" +
                     "       filter: alpha(opacity=30);" +
                     "       -moz-opacity: 0.3;" +
                     "       min-height: 100%;" +
                     "       width: 100%;" +
                     "   }" +
                     "   .loading{" +
                     "       text-align: center;" +
                     "       font-family: Arial;" +
                     "       font-size: 10pt;" +
                     "       border: 2px solid #67CFF5;" +
                     "       width: 200px;" +
                     "       height: 110px;" +
                     "       display: none;" +
                     "       position: fixed;" +
                     "       background-color: White;" +
                     "       z-index: 999;" +
                     "       font-weight:bold;" +
                     "}" +
                "</style>" +
                "<div class='loading' style='display: none; top: 271.5px; left: 578px;'>" +
                "<br/>Carregando, por favor aguarde...<br/><br/>" +
                "<img src='../../SiteAssets/CSS/ajax-loader.gif' alt=''/>" +
                "<br/></div> ";
                wpAEd.Content = xmlElement;
                wpAEd.Content.InnerText = xmlElement.InnerText;
                wpmEdit.AddWebPart(wpAEd, "Main", 2);
                wpmEditGerenciaResponsaveGerencia.AddWebPart(wpAEd, "Main", 2);

                editForm.Update();
                editFormGerenciaResponsavel.Update();
            }

            list.BreakRoleInheritance(false, false);

            SPGroup groupC = webcurrent.SiteGroups[Colaboradores];
            SPGroup groupA = webcurrent.SiteGroups[Admin];
            SPGroup groupB = webcurrent.SiteGroups[Visualizadores];

            SPRoleAssignment roleAssignmentC = new SPRoleAssignment(groupC);
            SPRoleAssignment roleAssignmentA = new SPRoleAssignment(groupA);
            SPRoleAssignment roleAssignmentB = new SPRoleAssignment(groupB);

            SPRoleDefinition roleDefinitionC = webcurrent.RoleDefinitions.GetByType(SPRoleType.Reader);
            SPRoleDefinition roleDefinitionA = webcurrent.RoleDefinitions.GetByType(SPRoleType.Administrator);

            roleAssignmentC.RoleDefinitionBindings.Add(roleDefinitionC);
            roleAssignmentA.RoleDefinitionBindings.Add(roleDefinitionA);
            roleAssignmentB.RoleDefinitionBindings.Add(roleDefinitionC);

            list.RoleAssignments.Add(roleAssignmentC);
            list.RoleAssignments.Add(roleAssignmentA);
            list.RoleAssignments.Add(roleAssignmentB);

            list.Update();
            listGerenciaResponsavel.Update();
        }

        public static void ChangeModelo(SPWeb webcurrent)
        {
            SPList list = webcurrent.Lists.TryGetList("Modelo de Chamado");
            list.NavigateForFormsPages = false;
            //SPField field = list.Fields[SPBuiltInFieldId.Title];
            //field.Title = "Modelo de Chamado";
            //field.Update();
            list.Update();

            SPFile editForm = webcurrent.GetFile(list.DefaultEditFormUrl);
            SPLimitedWebPartManager wpmE = editForm.GetLimitedWebPartManager(PersonalizationScope.Shared);

            if (wpmE.WebParts.Count == 1)
            {
                // create our List View web part
                ScriptEditorWebPart wpE = new ScriptEditorWebPart();
                wpE.Content = "<script src='" + webcurrent.ServerRelativeUrl + "/SiteAssets/JSChamados/ModeloChamado.js'></script>";
                wpmE.AddWebPart(wpE, "Main", 2);
                editForm.Update();
            }

            list.BreakRoleInheritance(false, false);

            SPGroup groupC = webcurrent.SiteGroups[Colaboradores];
            SPGroup groupA = webcurrent.SiteGroups[Admin];
            SPGroup groupB = webcurrent.SiteGroups[Visualizadores];

            SPRoleAssignment roleAssignmentC = new SPRoleAssignment(groupC);
            SPRoleAssignment roleAssignmentA = new SPRoleAssignment(groupA);
            SPRoleAssignment roleAssignmentB = new SPRoleAssignment(groupB);

            SPRoleDefinition roleDefinitionC = webcurrent.RoleDefinitions.GetByType(SPRoleType.Reader);
            SPRoleDefinition roleDefinitionA = webcurrent.RoleDefinitions.GetByType(SPRoleType.Contributor);

            roleAssignmentC.RoleDefinitionBindings.Add(roleDefinitionC);
            roleAssignmentA.RoleDefinitionBindings.Add(roleDefinitionA);
            roleAssignmentB.RoleDefinitionBindings.Add(roleDefinitionC);

            list.RoleAssignments.Add(roleAssignmentC);
            list.RoleAssignments.Add(roleAssignmentA);
            list.RoleAssignments.Add(roleAssignmentB);

            list.Update();
        }

        public static void ChangeListasAuxiliares(SPWeb webcurrent)
        {
            SPList list = webcurrent.Lists.TryGetList("Tipo de Chamado");
            list.DisableGridEditing = true;
            list.NavigateForFormsPages = false;

            SPField field = list.Fields[SPBuiltInFieldId.Title];
            field.Indexed = true;
            field.EnforceUniqueValues = true;
            field.Update();

            List<string> tipos = new List<string>() { "Chamado sem formulário", "Dispositivos Móveis", "Notas e Ordens", "Padronização do Módulo SAP-PM", "Planos de Manutenção", "Perfil", "Sistema de Acompanhamento de Manutenção", "Objetos Técnicos" };

            foreach (string tipo in tipos)
            {
                SPListItem item = list.AddItem();
                item[SPBuiltInFieldId.Title] = tipo;
                item.Update();
            }

            list.Update();
        }

        public static void ChangeListasTipoMTC(SPWeb webcurrent)
        {
            SPList list = webcurrent.Lists.TryGetList("Tipo MTC");
            list.DisableGridEditing = true;
            list.NavigateForFormsPages = false;

            SPField field = list.Fields[SPBuiltInFieldId.Title];
            field.Indexed = true;
            field.EnforceUniqueValues = true;
            field.Update();

            list.BreakRoleInheritance(false, false);

            SPGroup groupC = webcurrent.SiteGroups[Colaboradores];
            SPGroup groupA = webcurrent.SiteGroups[Admin];
            SPGroup groupB = webcurrent.SiteGroups[Visualizadores];

            SPRoleAssignment roleAssignmentC = new SPRoleAssignment(groupC);
            SPRoleAssignment roleAssignmentA = new SPRoleAssignment(groupA);
            SPRoleAssignment roleAssignmentB = new SPRoleAssignment(groupB);

            SPRoleDefinition roleDefinitionC = webcurrent.RoleDefinitions.GetByType(SPRoleType.Reader);
            SPRoleDefinition roleDefinitionA = webcurrent.RoleDefinitions.GetByType(SPRoleType.Administrator);

            roleAssignmentC.RoleDefinitionBindings.Add(roleDefinitionC);
            roleAssignmentA.RoleDefinitionBindings.Add(roleDefinitionA);
            roleAssignmentB.RoleDefinitionBindings.Add(roleDefinitionC);

            list.RoleAssignments.Add(roleAssignmentC);
            list.RoleAssignments.Add(roleAssignmentA);
            list.RoleAssignments.Add(roleAssignmentB);

            list.Update();
        }

        public static void ScriptChamados(SPWeb web)
        {
            SPList mainList = web.Lists["Chamados"];

            SPFile newForm = web.GetFile(mainList.DefaultNewFormUrl);
            SPLimitedWebPartManager wpm = newForm.GetLimitedWebPartManager(PersonalizationScope.Shared);

            if (wpm.WebParts.Count == 1)
            {
                ContentEditorWebPart wpA = new ContentEditorWebPart();
                XmlDocument xmlDoc = new XmlDocument();
                XmlElement xmlElement = xmlDoc.CreateElement("Root");
                xmlElement.InnerText = "<link rel='stylesheet' type='text/css' href='" + web.ServerRelativeUrl + "/SiteAssets/JSChamados/carregaModelo.css' />" +
                    "<div id='containerModelos'><div class='modelos'><span><h3>Modelos de Chamado para Download</h3></span>" +
                    "<div id='modelosGradeAntes'><span>Nenhum Modelo carregado</span></div><div id='modelosGrade'></div>" +
                    "</div></div>";
                wpA.Content = xmlElement;
                wpA.Content.InnerText = xmlElement.InnerText;
                wpm.AddWebPart(wpA, "Main", 1);
                newForm.Update();

                ScriptEditorWebPart wpN = new ScriptEditorWebPart();
                wpN.Content = "<script src='" + web.ServerRelativeUrl + "/SiteAssets/JSChamados/IncluirChamado.js'></script>";
                wpm.AddWebPart(wpN, "Main", 3);
                newForm.Update();
            }

            SPFile editForm = web.GetFile(mainList.DefaultEditFormUrl);
            SPLimitedWebPartManager wpmE = editForm.GetLimitedWebPartManager(PersonalizationScope.Shared);

            if (wpmE.WebParts.Count == 1)
            {
                ContentEditorWebPart wpAE = new ContentEditorWebPart();
                XmlDocument xmlDoc = new XmlDocument();
                XmlElement xmlElement = xmlDoc.CreateElement("Root");
                xmlElement.InnerText = //"<link rel='stylesheet' type='text/css' href='" + web.ServerRelativeUrl + "/SiteAssets/JSChamados/carregaModelo.css' />" +
                    //"<div id='containerModelos'></div>" +
                    "<script type='text/javascript' src='http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js'></script>" +
                    "<script>" +
                    "function ShowProgress() {" +
                    "        setTimeout(function () {" +
                    "            var modal = $('<div />');" +
                    "            modal.addClass('modal');" +
                    "            $('body').append(modal);" +
                    "            var loading = $('.loading');" +
                    "            loading.show();" +
                    "            var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);" +
                    "            var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);" +
                    "            loading.css({ top: top, left: left });" +
                    "        }, 200);" +
                    "}" +
                    "</script>" +
                    "<style>" +
                     " .modal{" +
                     "       position: fixed;" +
                     "       top: 0;" +
                     "       left: 0;" +
                     "       background-color: black;" +
                     "       z-index: 99;" +
                     "       opacity: 0.3;" +
                     "       filter: alpha(opacity=30);" +
                     "       -moz-opacity: 0.3;" +
                     "       min-height: 100%;" +
                     "       width: 100%;" +
                     "   }" +
                     "   .loading{" +
                     "       text-align: center;" +
                     "       font-family: Arial;" +
                     "       font-size: 10pt;" +
                     "       border: 2px solid #67CFF5;" +
                     "       width: 200px;" +
                     "       height: 110px;" +
                     "       display: none;" +
                     "       position: fixed;" +
                     "       background-color: White;" +
                     "       z-index: 999;" +
                     "       font-weight:bold;" +
                     "}" +
                "</style>" +
                "<div class='loading' style='display: none; top: 271.5px; left: 578px;'>" +
                "<br/>Carregando, por favor aguarde...<br/><br/>" +
                "<img src='../../SiteAssets/CSS/ajax-loader.gif' alt=''/>" +
                "<br/></div> ";
                wpAE.Content = xmlElement;
                wpAE.Content.InnerText = xmlElement.InnerText;
                wpmE.AddWebPart(wpAE, "Main", 1);
                editForm.Update();

                // create our List View web part
                ScriptEditorWebPart wpE = new ScriptEditorWebPart();
                wpE.Content = "<script src='" + web.ServerRelativeUrl + "/SiteAssets/JSChamados/EditChamado.js'></script>";
                wpmE.AddWebPart(wpE, "Main", 3);
                editForm.Update();
            }

            SPFile dispForm = web.GetFile(mainList.DefaultDisplayFormUrl);
            SPLimitedWebPartManager wpmD = dispForm.GetLimitedWebPartManager(PersonalizationScope.Shared);

            if (wpmD.WebParts.Count == 1)
            {
                ScriptEditorWebPart wpD = new ScriptEditorWebPart();
                wpD.Content = "<script src='" + web.ServerRelativeUrl + "/SiteAssets/JSChamados/DispChamado.js'></script>";
                wpmD.AddWebPart(wpD, "Main", 2);
                dispForm.Update();
            }

            /*SPFile allItems = web.GetFile(mainList.DefaultViewUrl);
            SPLimitedWebPartManager wpmA = allItems.GetLimitedWebPartManager(PersonalizationScope.Shared);

            if (wpmA.WebParts.Count == 1)
            {
                // create our List View web part
                ScriptEditorWebPart wpA = new ScriptEditorWebPart();
                wpA.Content = "<script src='" + web.ServerRelativeUrl + "/SiteAssets/JS/EncaminhaChamado.js'></script>";
                wpmA.AddWebPart(wpA, "Main", 2);
                allItems.Update();
            }*/
        }

        public static void Contador(SPWeb web)
        {
            try
            {
                SPList contador = web.Lists["Contador Chamado"];
                SPListItem item = contador.AddItem();

                item[SPBuiltInFieldId.Title] = "0";
                item.Update();
            }
            catch { }
        }

        private static void CreateGroup(SPWeb webcurrent, string groupname, string description, SPRoleType sPRoleType, params SPUser[] predefinedusers)
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

        private static bool GroupExistsInWebSite(SPWeb web, string name)
        {
            return web.Groups.OfType<SPGroup>().Count(g => g.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)) > 0;
        }

    }
}
