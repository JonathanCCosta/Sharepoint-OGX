using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using System.Xml;

namespace Furnas.OGX2.Service
{
    public class RelacaoWebPart
    {

        public static void GestaoInterna_X_Planos(SPWeb web)
        {

            SPList mainList = web.Lists["Planos"];
            SPList relatedList = web.Lists["Gestão Interna"];
            SPList relatedListMudanca = web.Lists["Solicitação de Mudanças"];
            // this is the Display Form web part page
            SPFile dispForm = web.GetFile(mainList.DefaultDisplayFormUrl);
            // get the web part manage which we will use to interact
            SPLimitedWebPartManager wpm = dispForm.GetLimitedWebPartManager(PersonalizationScope.Shared);
            // only execute this code if there is a single web part (the default for this list)
            // we don’t want to add the web parts over and over again ..
            if (wpm.WebParts.Count == 1)
            {
                // create our List View web part
                XsltListViewWebPart wpM = new XsltListViewWebPart();
                
                wpM.ListId = relatedListMudanca.ID;
                wpM.ListName = relatedListMudanca.ID.ToString();
                wpM.ViewGuid = relatedListMudanca.DefaultView.ID.ToString();
                wpM.XmlDefinition = relatedListMudanca.DefaultView.GetViewXml();
                wpM.AllowConnect = true;
                wpM.ChromeType = PartChromeType.TitleOnly;
                wpm.AddWebPart(wpM, "Main", 3);


                System.Web.UI.WebControls.WebParts.WebPart consumerM = wpm.WebParts[1];
                System.Web.UI.WebControls.WebParts.WebPart providerM = wpm.WebParts[0];
                ProviderConnectionPoint providerPointM = wpm.GetProviderConnectionPoints(providerM)["ListFormRowProvider_WPQ_"];
                ConsumerConnectionPoint consumerPointM = wpm.GetConsumerConnectionPoints(consumerM)["DFWP Filter Consumer ID"];

                SPRowToParametersTransformer optimusM = new SPRowToParametersTransformer();
                optimusM.ProviderFieldNames = new string[] { "Title" };
                optimusM.ConsumerFieldNames = new string[] { "NumeracaoIrma" };
                wpm.SPConnectWebParts(providerM, providerPointM, consumerM, consumerPointM, optimusM);
                dispForm.Update();

                XsltListViewWebPart wp = new XsltListViewWebPart();
                // Hook up the list and the view
                wp.ListId = relatedList.ID;
                wp.ListName = relatedList.ID.ToString();
                wp.ViewGuid = relatedList.DefaultView.ID.ToString();
                wp.XmlDefinition = relatedList.DefaultView.GetViewXml();
                // set basic properties along with the title
                wp.AllowConnect = true;
                wp.ChromeType = PartChromeType.TitleOnly;
                // add the web part to the Main zone in position 2
                //wp.AuthorizationFilter = ";;;;Administradores GGA";
                //wp.IsIncludedFilter = ";;;;Administradores GGA";
                wpm.AddWebPart(wp, "Main", 4);


                System.Web.UI.WebControls.WebParts.WebPart consumer = wpm.WebParts[1];
                System.Web.UI.WebControls.WebParts.WebPart provider = wpm.WebParts[0];
                ProviderConnectionPoint providerPoint = wpm.GetProviderConnectionPoints(provider)["ListFormRowProvider_WPQ_"];
                ConsumerConnectionPoint consumerPoint = wpm.GetConsumerConnectionPoints(consumer)["DFWP Filter Consumer ID"];

                SPRowToParametersTransformer optimus = new SPRowToParametersTransformer();
                optimus.ProviderFieldNames = new string[] { "ID" };
                optimus.ConsumerFieldNames = new string[] { "NumSGPMR" };
                wpm.SPConnectWebParts(provider, providerPoint, consumer, consumerPoint, optimus);

                // save the page
                dispForm.Update();

                //XsltListViewWebPart wpH = new XsltListViewWebPart();
                //// Hook up the list and the view
                //wpH.ListId = mainList.ID;
                //wpH.ListName = mainList.ID.ToString();
                //wpH.ViewGuid = mainList.DefaultView.ID.ToString();
                //wpH.XmlDefinition = mainList.DefaultView.GetViewXml();
                ////H set basic properties along with the title
                //wpH.AllowConnect = true;
                //wpH.ChromeType = PartChromeType.TitleOnly;
                //wpH.Title = "Gestao Interna - Histórico";
                ////wpH.Toolbar = "None";
                //wpm.AddWebPart(wp, "Main", 5);


                //System.Web.UI.WebControls.WebParts.WebPart consumerH = wpm.WebParts[1];
                //System.Web.UI.WebControls.WebParts.WebPart providerH = wpm.WebParts[0];
                //ProviderConnectionPoint providerPointH = wpm.GetProviderConnectionPoints(providerH)["ListFormRowProvider_WPQ_"];
                //ConsumerConnectionPoint consumerPointH = wpm.GetConsumerConnectionPoints(consumerH)["DFWP Filter Consumer ID"];

                //SPRowToParametersTransformer optimusH = new SPRowToParametersTransformer();
                //optimusH.ProviderFieldNames = new string[] { "ID" };
                //optimusH.ConsumerFieldNames = new string[] { "NumSGPMR" };
                //wpm.SPConnectWebParts(providerH, providerPointH, consumerH, consumerPointH, optimusH);

                //// save the page
                //dispForm.Update();
            }
        }

        public static void ScriptGestao(SPWeb web)
        {
            SPList mainList = web.Lists["Gestão Interna"];

            SPFile newForm = web.GetFile(mainList.DefaultNewFormUrl);
            SPLimitedWebPartManager wpm = newForm.GetLimitedWebPartManager(PersonalizationScope.Shared);

            if (wpm.WebParts.Count == 1)
            {
                ScriptEditorWebPart wpN = new ScriptEditorWebPart();
                wpN.Content = "<script src='" + web.ServerRelativeUrl + "/SiteAssets/JS/ValidaGestaoInterna.js'></script><script src='" + web.ServerRelativeUrl + "/SiteAssets/JS/DateFormat.js'></script>";
                wpm.AddWebPart(wpN, "Main", 2);
                newForm.Update();
            }

            SPFile editForm = web.GetFile(mainList.DefaultEditFormUrl);
            SPLimitedWebPartManager wpmE = editForm.GetLimitedWebPartManager(PersonalizationScope.Shared);

            if (wpmE.WebParts.Count == 1)
            {
                // create our List View web part
                ScriptEditorWebPart wpE = new ScriptEditorWebPart();
                wpE.Content = "<script src='" + web.ServerRelativeUrl + "/SiteAssets/JS/ValidaGestaoInterna.js'></script>";
                wpmE.AddWebPart(wpE, "Main", 2);
                editForm.Update();
            }

            /*SPFile allItems = web.GetFile(mainList.DefaultViewUrl);
            SPLimitedWebPartManager wpmA = allItems.GetLimitedWebPartManager(PersonalizationScope.Shared);

            if (wpmA.WebParts.Count == 1)
            {
                
                ScriptEditorWebPart wpA = new ScriptEditorWebPart();
                wpA.Content = "<script src='" + web.ServerRelativeUrl + "/SiteAssets/JS/IncluirMudanca.js'></script>";
                wpmA.AddWebPart(wpA, "Main", 2);
                allItems.Update();
            }*/
        }

        public static void ScriptCiclos(SPWeb web)
        {
            SPList mainList = web.Lists["Ciclos"];

            SPFile newForm = web.GetFile(mainList.DefaultNewFormUrl);
            SPLimitedWebPartManager wpm = newForm.GetLimitedWebPartManager(PersonalizationScope.Shared);

            if (wpm.WebParts.Count == 1)
            {
                ScriptEditorWebPart wpN = new ScriptEditorWebPart();
                wpN.Content = "<script src='" + web.ServerRelativeUrl + "/SiteAssets/JS/ValidaCiclo.js'></script>";
                wpm.AddWebPart(wpN, "Main", 2);
                newForm.Update();
            }

            SPFile editForm = web.GetFile(mainList.DefaultEditFormUrl);
            SPLimitedWebPartManager wpmE = editForm.GetLimitedWebPartManager(PersonalizationScope.Shared);

            if (wpmE.WebParts.Count == 1)
            {
                // create our List View web part
                ScriptEditorWebPart wpE = new ScriptEditorWebPart();
                wpE.Content = "<script src='" + web.ServerRelativeUrl + "/SiteAssets/JS/ValidaCiclo.js'></script>";
                wpmE.AddWebPart(wpE, "Main", 2);
                editForm.Update();
            }
        }

        public static void ScriptPlanoMudanca(SPWeb web)
        {
            SPList mainList = web.Lists["Solicitação de Mudanças"];

            SPFile newForm = web.GetFile(mainList.DefaultNewFormUrl);
            SPLimitedWebPartManager wpm = newForm.GetLimitedWebPartManager(PersonalizationScope.Shared);

            if (wpm.WebParts.Count == 1)
            {
                ScriptEditorWebPart wpN = new ScriptEditorWebPart();
                wpN.Content = "<script src='" + web.ServerRelativeUrl + "/SiteAssets/JS/ValidaPlanoMudanca.js'></script><script src='" + web.ServerRelativeUrl + "/SiteAssets/JS/carregaPlano.js'></script><script src='" + web.ServerRelativeUrl + "/SiteAssets/JS/DateFormat.js'></script>";
                wpm.AddWebPart(wpN, "Main", 2);                

                newForm.Update();
            }

            SPFile editForm = web.GetFile(mainList.DefaultEditFormUrl);
            SPLimitedWebPartManager wpmE = editForm.GetLimitedWebPartManager(PersonalizationScope.Shared);

            if (wpmE.WebParts.Count == 1)
            {
                // create our List View web part
                ScriptEditorWebPart wpE = new ScriptEditorWebPart();
                wpE.Content = "<script src='" + web.ServerRelativeUrl + "/SiteAssets/JS/ValidaPlanoMudanca.js'></script><script src='" + web.ServerRelativeUrl + "/SiteAssets/JS/carregaPlano.js'></script><script src='" + web.ServerRelativeUrl + "/SiteAssets/JS/DateFormat.js'></script>";
                wpmE.AddWebPart(wpE, "Main", 2);
                editForm.Update();
            }

            SPFile displayForm = web.GetFile(mainList.DefaultDisplayFormUrl);
            SPLimitedWebPartManager wpmD = displayForm.GetLimitedWebPartManager(PersonalizationScope.Shared);


            System.Web.UI.WebControls.WebParts.WebPart displayExclusao = GetWebPart(web, "Furnas.OGX2 - WP_ExclusaoMudanca");
            if (displayExclusao != null)
            {
                displayExclusao.ChromeType = PartChromeType.None;
                wpmD.AddWebPart(displayExclusao, "Main", 1);
                displayForm.Update();
            }

            SPFile allItems = web.GetFile(mainList.DefaultViewUrl);
            SPLimitedWebPartManager wpmA = allItems.GetLimitedWebPartManager(PersonalizationScope.Shared);

            if (wpmA.WebParts.Count == 1)
            {
                // create our List View web part
                ScriptEditorWebPart wpA = new ScriptEditorWebPart();
                wpA.Content = "<script src='" + web.ServerRelativeUrl + "/SiteAssets/JS/IncluirMudanca.js'></script>";
                wpmA.AddWebPart(wpA, "Main", 1);
                allItems.Update();
            }

            /*SPFile displayForm2 = web.GetFile(mainList.DefaultDisplayFormUrl);
            SPLimitedWebPartManager wpmD2 = displayForm2.GetLimitedWebPartManager(PersonalizationScope.Shared);

            if (wpmD.WebParts.Count == 1)
            {
                 //create our List View web part
                ScriptEditorWebPart wpD2 = new ScriptEditorWebPart();
                wpD2.Content = "<script src='" + web.ServerRelativeUrl + "/SiteAssets/JS/DestacaMudancas.js'></script>";
                wpmD.AddWebPart(wpD2, "Main", 3);
                displayForm.Update();
            }*/


            SPFile displayForm2 = web.GetFile(mainList.DefaultDisplayFormUrl);
            SPLimitedWebPartManager wpmD2 = displayForm2.GetLimitedWebPartManager(PersonalizationScope.Shared);

            if (wpmD2.WebParts.Count == 1)
            {
                SPList relatedList = web.Lists["Gestão Interna"];
                XsltListViewWebPart wpD2 = new XsltListViewWebPart();
                // Hook up the list and the view
                wpD2.ListId = relatedList.ID;
                wpD2.ListName = relatedList.ID.ToString();
                wpD2.ViewGuid = relatedList.DefaultView.ID.ToString();
                wpD2.XmlDefinition = relatedList.DefaultView.GetViewXml();
                //D2 set basic properties along with the title
                wpD2.AllowConnect = true;
                wpD2.ChromeType = PartChromeType.TitleOnly;
                // add the web part to the Main zone in position 2
                //wp.AuthorizationFilter = ";;;;Administradores GGA";
                //wp.IsIncludedFilter = ";;;;Administradores GGA";
                wpmD2.AddWebPart(wpD2, "Main", 3);


                System.Web.UI.WebControls.WebParts.WebPart consumer = wpmD2.WebParts[1];
                System.Web.UI.WebControls.WebParts.WebPart provider = wpmD2.WebParts[0];
                ProviderConnectionPoint providerPoint = wpmD2.GetProviderConnectionPoints(provider)["ListFormRowProvider_WPQ_"];
                ConsumerConnectionPoint consumerPoint = wpmD2.GetConsumerConnectionPoints(consumer)["DFWP Filter Consumer ID"];

                SPRowToParametersTransformer optimus = new SPRowToParametersTransformer();
                optimus.ProviderFieldNames = new string[] { "NumGestao" };
                optimus.ConsumerFieldNames = new string[] { "ID" };
                wpmD2.SPConnectWebParts(provider, providerPoint, consumer, consumerPoint, optimus);

                // save the page
                displayForm2.Update();
            }
        }

        public static void ScriptPlanos(SPWeb web)
        {
            SPList mainList = web.Lists["Planos"];

            SPFile newForm = web.GetFile(mainList.DefaultNewFormUrl);
            SPLimitedWebPartManager wpm = newForm.GetLimitedWebPartManager(PersonalizationScope.Shared);

            if (wpm.WebParts.Count == 1)
            {
                ScriptEditorWebPart wpN = new ScriptEditorWebPart();
                wpN.Content = "<script src='" + web.ServerRelativeUrl + "/SiteAssets/JS/NewPlanos.js'></script>";
                wpm.AddWebPart(wpN, "Main", 2);
                newForm.Update();
            }

            SPFile editForm = web.GetFile(mainList.DefaultEditFormUrl);
            SPLimitedWebPartManager wpmE = editForm.GetLimitedWebPartManager(PersonalizationScope.Shared);

            if (wpmE.WebParts.Count == 1)
            {
                // create our List View web part
                ScriptEditorWebPart wpE = new ScriptEditorWebPart();
                wpE.Content = "<script src='" + web.ServerRelativeUrl + "/SiteAssets/JS/EditPanos.js'></script>";
                wpmE.AddWebPart(wpE, "Main", 2);
                editForm.Update();
            }

            SPFile displayForm = web.GetFile(mainList.DefaultDisplayFormUrl);
            SPLimitedWebPartManager wpmD = displayForm.GetLimitedWebPartManager(PersonalizationScope.Shared);

            System.Web.UI.WebControls.WebParts.WebPart displayAlterarcao = GetWebPart(web, "Furnas.OGX2 - WP_RequisitaMudanca");
            if (displayAlterarcao != null)
            {
                displayAlterarcao.ChromeType = PartChromeType.None;
                displayAlterarcao.AuthorizationFilter = ";;;;Administradores GGA,Colaboradores GGA";
                wpmD.AddWebPart(displayAlterarcao, "Main", 2);
                displayForm.Update();

                ScriptEditorWebPart wpD = new ScriptEditorWebPart();
                wpD.Content = "<script src='" + web.ServerRelativeUrl + "/SiteAssets/JS/ManipulaGestao.js'></script>";
                wpmD.AddWebPart(wpD, "Main", 6);
            }

            SPFile allItems = web.GetFile(mainList.DefaultViewUrl);
            SPLimitedWebPartManager wpmA = allItems.GetLimitedWebPartManager(PersonalizationScope.Shared);

            if (wpmA.WebParts.Count == 1)
            {
                // create our List View web part
                //ScriptEditorWebPart wpA = new ScriptEditorWebPart();
                ContentEditorWebPart wpA = new ContentEditorWebPart();
                XmlDocument xmlDoc = new XmlDocument();
                XmlElement xmlElement = xmlDoc.CreateElement("Root");
                xmlElement.InnerText = "<script src='" + web.ServerRelativeUrl + "/SiteAssets/JS/IncluirMudanca.js'></script><table id='Hero-WPQ2N' " +
                "cellpadding='0' cellspacing='0' border='0'><tbody><tr><td class='ms-list-addnew ms-textXLarge ms-list-addnew-aligntop ms-soften'>" +
                "<a id='idHomePageNewItem' class='ms-heroCommandLink' href='" + web.ServerRelativeUrl + "/_layouts/15/listform.aspx?PageType=8&amp;ListId=%7B6212C42A%2D0686%2D4B94%2DBF65%2DD05046BF14AE%7D&amp;RootFolder=" + 
                "data-viewctr='0' onclick='OpenDialog('" + web.ServerRelativeUrl +"/SitePages/SolicitacaoInclusao.aspx','Solicitação de Inclusão');return false;' "+ 
                "target='_self' title='Adicionar um novo item a esta lista ou biblioteca.'><span class='ms-list-addnew-imgSpan20'><img id='idHomePageNewItem-img' src='/_layouts/15/images/spcommon.png?rev=23' " + 
                "class='ms-list-addnew-img20' alt=''/></span><span>Solicitar inclusão de um novo Plano</span></a><br/></td></tr></tbody></table>";
                wpA.Content = xmlElement;
                wpA.Content.InnerText = xmlElement.InnerText;
                
                //wpA.Content = "<script src='" + web.ServerRelativeUrl + "/SiteAssets/JS/IncluirMudanca.js'></script>";
                wpmA.AddWebPart(wpA, "Main", 1);
                allItems.Update();
            }
        }


        protected static System.Web.UI.WebControls.WebParts.WebPart GetWebPart(SPWeb web, string webPartName)
        {
            SPList webPartGallery;
            if (web.IsRootWeb)
            {
                webPartGallery = web.GetCatalog(
                   SPListTemplateType.WebPartCatalog);
            }
            else
            {
                webPartGallery = web.ParentWeb.GetCatalog(
                   SPListTemplateType.WebPartCatalog);
            }
            var webParts = webPartGallery.GetItems();
            int i = 0;
            foreach (SPListItem w in webParts)
            {
                if (w.Title == webPartName) //"Furnas.OGX2 - WP_RequisitaMudanca")
                {
                    var typeName = webParts[i].GetFormattedValue("WebPartTypeName");
                    var assemblyName = webParts[i].GetFormattedValue("WebPartAssembly");
                    var webPartHandle = Activator.CreateInstance(
                        assemblyName, typeName);

                    System.Web.UI.WebControls.WebParts.WebPart webPart =
                        (System.Web.UI.WebControls.WebParts.WebPart)webPartHandle.Unwrap();

                    return webPart;
                } 
                i++;
            }

            return null;
        }
    }
}
