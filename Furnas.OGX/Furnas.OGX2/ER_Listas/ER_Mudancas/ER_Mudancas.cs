using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using Furnas.OGX2.Service;

namespace Furnas.OGX2.ER_Listas.ER_Mudancas
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class ER_Mudancas : SPItemEventReceiver
    {
        /// <summary>
        /// An item was updated.
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);

            if (DuplicaItens.ValidaTextField(properties.ListItem["Alteração Solicitada"]) == "Exclusão" || DuplicaItens.ValidaTextField(properties.ListItem["Alteração Solicitada"]) == "Inclusão")
            {
                if (properties.ListItem.ContentType.Name == "Plano por Superação - Mudanças")
                    Service.DuplicaItens.CamposAlterados(properties.ListItem);
                else
                    Service.DuplicaItens.CamposAlteradosMelhoriaReforco(properties.ListItem);
            }

            if (DuplicaItens.ValidaTextField(properties.ListItem["Numeração"]) != string.Empty && DuplicaItens.ValidaTextField(properties.ListItem["Número de Gestão"]) != string.Empty && DuplicaItens.ValidaTextField(properties.ListItem["Alteração Solicitada"]) == "Inclusão")
                Service.DuplicaItens.NumeracaoParaGestao_Solicitacao(properties.ListItem);

        }

        /// <summary>
        /// An item was added
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);

            if (DuplicaItens.ValidaTextField(properties.ListItem["Alteração Solicitada"]) == "Exclusão" || DuplicaItens.ValidaTextField(properties.ListItem["Alteração Solicitada"]) == "Inclusão")
                Permissao.PermissaoMudancaExclusaoInclusao(properties.ListItem);
            else
                Permissao.PermissaoMudancaAlteracao(properties.ListItem);
        }

        /// <summary>
        /// An item is being updated
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            base.ItemUpdating(properties);

            if (DuplicaItens.ValidaTextField(properties.AfterProperties["StatusSolicitacao"]) != DuplicaItens.ValidaTextField(properties.ListItem["StatusSolicitacao"]))
            {
                ExportacaoSolicitacao.EmailMudancaStatus(properties.ListItem, DuplicaItens.ValidaTextField(properties.AfterProperties["StatusSolicitacao"]));
            }
        }


    }
}