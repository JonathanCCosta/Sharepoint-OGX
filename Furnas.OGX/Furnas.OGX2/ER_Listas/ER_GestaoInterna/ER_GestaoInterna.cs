using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using Furnas.OGX2.Service;

namespace Furnas.OGX2.ER_Listas.ER_GestaoInterna
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class ER_GestaoInterna : SPItemEventReceiver
    {
        /// <summary>
        /// An item was added.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);

            GestaoInterna.HistoricoGestao(properties.ListItem);
            //Função de Permissionamento de Leitura igual de Solicitação de Mudança
            Permissao.PermissaoMudancaExclusaoInclusao(properties.ListItem);
        }

        /// <summary>
        /// An item is being updated
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            base.ItemUpdating(properties);

            if (DuplicaItens.ValidaTextField(properties.ListItem["Aprovação da solicitação"]) != DuplicaItens.ValidaTextField(properties.AfterProperties["AprovacaoSolicitacao"]))
            {
                if (DuplicaItens.ValidaTextField(properties.AfterProperties["AprovacaoSolicitacao"]) == "Aprovado" && DuplicaItens.ValidaTextField(properties.ListItem["Histórico"]) == "Não")
                    GestaoInterna.HistoricoGestaoUpdate(properties.ListItem);
                //else if (DuplicaItens.ValidaTextField(properties.AfterProperties["AprovacaoSolicitacao"]) == "Rejeitado" && DuplicaItens.ValidaTextField(properties.ListItem["Histórico"]) == "Não")
                    //GestaoInterna.HistoricoGestaoRejeita(properties.ListItem);//Permissao.PermissaoMudancaExclusaoInclusao(properties.ListItem);//Função de Permissionamento de Leitura igual de Solicitação de Mudança
            }

            if (DuplicaItens.ValidaTextField(properties.AfterProperties["AprovacaoSolicitacao"]) != DuplicaItens.ValidaTextField(properties.ListItem["AprovacaoSolicitacao"]))
            {
                GestaoInterna.EmailMudancaStatus(properties.ListItem, DuplicaItens.ValidaTextField(properties.AfterProperties["AprovacaoSolicitacao"]));
            }
        }

        /// <summary>
        /// An item is being deleted
        /// </summary>
        public override void ItemDeleting(SPItemEventProperties properties)
        {
            base.ItemDeleting(properties);
            try
            {
                GestaoInterna.DeleteGestaoInterna(properties.ListItem);
            }
            catch { }
        }

        /// <summary>
        /// An item was updated
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);
            if (DuplicaItens.ValidaTextField(properties.ListItem["Aprovação da solicitação"]) == "Rejeitado" && DuplicaItens.ValidaTextField(properties.ListItem["Histórico"]) == "Não")
                GestaoInterna.HistoricoGestaoRejeita(properties.ListItem);
        }
    }
}