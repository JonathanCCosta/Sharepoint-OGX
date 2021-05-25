using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Furnas.OGX2.Service
{
    public class DuplicaItens
    {
        public static int CopiarItemExclusao(SPListItem item, SPListItem plano)
        {
            if (plano.ContentType.Name == "Plano de Melhoria" || plano.ContentType.Name == "Plano de Reforço")
                return DeParaCamposMelhoriaReforcoExclusao(item, plano);
            else
                return DeParaCamposSuperacaoExclusao(item, plano);
        }

        protected static int DeParaCamposMelhoriaReforcoExclusao(SPListItem item, SPListItem plano)
        {
            try
            {
                //item["IDPlano"] = plano.ID.ToString();
                item["Numeração"] = ValidaTextField(plano[SPBuiltInFieldId.Title]); //- Caixa de Texto - Editável.
                item["Ciclo"] = new SPFieldLookupValue(ValidaTextField(plano["Ciclo"])).LookupValue; // - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Região"]) != string.Empty)
                    item["Região"] = ValidaTextField(plano["Região"]);// - Caixa de Seleção - Editável - *.(Default: Não se aplica). (Opções: Centro-Oeste, Nordeste, Norte, Sudeste, Sul).

                item["Agente"] = "FURNAS";// - Caixa de Texto - Editável - *.

                if (ValidaTextField(plano["Instalação"]) != string.Empty)
                {
                    SPListItem DI = item.Web.Lists["Domínio de Instalação"].Items.OfType<SPListItem>().Where(p=> p.Title == Convert.ToString(plano["Instalação"])).FirstOrDefault();
                    if (DI != null)//new SPFieldLookupValue(item.ID.ToString());
                        item["Instalação"] = new SPFieldLookupValue(DI.ID.ToString()); // - Caixa de Seleção - Editável - *.(Default: Não se aplica). (Exibir todos os domínios para instalação cadastrados no sistema).
                }

                if (ValidaTextField(plano["(KV)"]) != string.Empty)
                    item["Tensão Nominal (KV)"] = ValidaTextField(plano["(KV)"]);  // - Caixa de Seleção - Editável - *.(Default: Não se aplica). (Exibir todos os domínios para  KV cadastrados no sistema).

                if (ValidaTextField(plano["Tipo de Agente"]) != string.Empty)
                    item["Tipo de Agente"] = ValidaTextField(plano["Tipo de Agente"]); // - Caixa de Seleção - Editável - *.(Default: Não se aplica). (Opções: Consumidor Livre, Distribuição, Geração, Transmissão).

                if (ValidaTextField(plano["Função de Transmissão"]) != string.Empty)
                {
                    SPListItem FT = item.Web.Lists["Domínio de Função de Transmissão"].Items.OfType<SPListItem>().Where(p => p.Title == Convert.ToString(plano["Função de Transmissão"])).FirstOrDefault();
                    if (FT != null)
                        item["Função de Transmissão"] = new SPFieldLookupValue(FT.ID.ToString());  // - Caixa de Seleção- Editável - *.(Default: Não se aplica). (Exibir todos os domínios para "Função de Transmissão" condicionados a "Instalação" cadastrados no sistema).
                }

                if (ValidaTextField(plano["Origem da Indicação"]) != string.Empty)
                    item["Origem da Indicação"] = ValidaTextField(plano["Origem da Indicação"]);// - Caixa de Texto - Editável - *.

                if (ValidaTextField(plano["Tipo de Revitalização"]) != string.Empty)
                    item["Tipo de Revitalização"] = ValidaTextField(plano["Tipo de Revitalização"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Revitalização Necessária"]) != string.Empty)
                    item["Revitalização Necessária"] = ValidaTextField(plano["Revitalização Necessária"]);// - Caixa de Texto - Editável - *. (máx. 300 caracteres)

                if (ValidaTextField(plano["Justificativa"]) != string.Empty)
                    item["Justificativa"] = ValidaTextField(plano["Justificativa"]);// - Caixa de Texto - Editável - *. (máx. 300 caracteres)
                
                item["Colunas Alteradas"] = "Justificativa da Exclusão";
                
                /*if (ValidaTextField(plano["Justificativa da Exclusão"]) != string.Empty)
                    item["Justificativa da Exclusão"] = ValidaTextField(plano["Justificativa da Exclusão"]);// - Caixa de Texto - Editável.*/
                    

                if (ValidaTextField(plano["Observação"]) != string.Empty)
                    item["Observação"] = ValidaTextField(plano["Observação"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Classificação"]) != string.Empty)
                    item["Classificação"] = ValidaTextField(plano["Classificação"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Prazo após emissão Do PMI/Resolução Aneel (meses)"]) != string.Empty)
                    item["Prazo após emissão Do PMI/Resolução Aneel (meses)"] = ValidaTextField(plano["Prazo após emissão Do PMI/Resolução Aneel (meses)"]);//- Caixa de Texto - Editável - *.

                if (ValidaTextField(plano["Projeto que Originou a Indicação"]) != string.Empty)
                    item["Projeto que Originou a Indicação"] = ValidaTextField(plano["Projeto que Originou a Indicação"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["N° da recomendação no SGR"]) != string.Empty)
                    item["N° da recomendação no SGR"] = ValidaTextField(plano["N° da recomendação no SGR"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Custo Estimado (R$)"]) != string.Empty)
                    item["Custo Estimado (R$)"] = ValidaTextField(plano["Custo Estimado (R$)"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Descrição Do equipamento a ser substituído ou dos componentes da linha"]) != string.Empty)
                    item["Descrição Do equipamento a ser substituído ou dos componentes da linha"] = ValidaTextField(plano["Descrição Do equipamento a ser substituído ou dos componentes da linha"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Identificação Do equipamento a ser substituído (número operativo, Do unifilar, etc.)"]) != string.Empty)
                    item["Identificação Do equipamento a ser substituído (número operativo, Do unifilar, etc.)"] = ValidaTextField(plano["Identificação Do equipamento a ser substituído (número operativo, Do unifilar, etc.)"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Evidência que justifique a Melhoria ou Reforço"]) != string.Empty)
                    item["Evidência que justifique a Melhoria ou Reforço"] = ValidaTextField(plano["Evidência que justifique a Melhoria ou Reforço"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Fabricante"]) != string.Empty)
                    item["Fabricante"] = ValidaTextField(plano["Fabricante"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Modelo"]) != string.Empty)
                    item["Modelo"] = ValidaTextField(plano["Modelo"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["N° Série"]) != string.Empty)
                    item["N° Série"] = ValidaTextField(plano["N° Série"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Ano Início em Operação"]) != string.Empty)
                    item["Ano Início em Operação"] = ValidaTextField(plano["Ano Início em Operação"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Taxa de Depreciação (MCPSE – versão 9)"]) != string.Empty)
                    item["Taxa de Depreciação (MCPSE – versão 9)"] = ValidaTextField(plano["Taxa de Depreciação (MCPSE – versão 9)"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Ano Final de Vida Útil"]) != string.Empty)
                    item["Ano Final de Vida Útil"] = ValidaTextField(plano["Ano Final de Vida Útil"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Substituir nos próximos 4 anos (a partir de 1º de fevereiro Do ano corrente)?"]) != string.Empty)
                    item["Substituir nos próximos 4 anos (a partir de 1º de fevereiro Do ano corrente)?"] = ValidaTextField(plano["Substituir nos próximos 4 anos (a partir de 1º de fevereiro Do ano corrente)?"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Equipamento apto a permanecer por tempo adicional à Vida Útil"]) != string.Empty)
                    item["Equipamento apto a permanecer por tempo adicional à Vida Útil"] = ValidaTextField(plano["Equipamento apto a permanecer por tempo adicional à Vida Útil"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Ações propostas"]) != string.Empty)
                    item["Ações propostas"] = ValidaTextField(plano["Ações propostas"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Aumento de vida útil esperado, a partir da implantação Do reforço(anos)"]) != string.Empty)
                    item["Aumento de vida útil esperado, a partir da implantação Do reforço(anos)"] = ValidaTextField(plano["Aumento de vida útil esperado, a partir da implantação Do reforço(anos)"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Valor a ser investido(R$)"]) != string.Empty)
                    item["Valor a ser investido(R$)"] = ValidaTextField(plano["Valor a ser investido(R$)"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Situação da Análise"]) != string.Empty)
                    item["Situação da Análise"] = ValidaTextField(plano["Situação da Análise"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Indicações"]) != string.Empty)
                    item["Indicações"] = ValidaTextField(plano["Indicações"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Observação ONS"]) != string.Empty)
                    item["Observação ONS"] = ValidaTextField(plano["Observação ONS"]);// - Caixa de Texto - Editável.
                
                if (ValidaTextField(plano["Classificação ONS"]) != string.Empty)
                    item["Classificação ONS"] = ValidaTextField(plano["Classificação ONS"]);// - Caixa de Texto - Editável.
                
                if (ValidaTextField(plano["Responsável pela Análise"]) != string.Empty)
                    item["Responsável pela Análise"] = ValidaTextField(plano["Responsável pela Análise"]);// - Caixa de Texto - Editável.
                
                if (ValidaTextField(plano["Data de Necessidade Sistêmica"]) != string.Empty)
                    item["Data de Necessidade Sistêmica"] = TratarData(plano["Data de Necessidade Sistêmica"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Oficio/Resolução/Autorização ANEEL relativa a substituição Do equipamento/PMI"]) != string.Empty)
                    item["Oficio/Resolução/Autorização ANEEL relativa a substituição Do equipamento/PMI"] = ValidaTextField(plano["Oficio/Resolução/Autorização ANEEL relativa a substituição Do equipamento/PMI"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Data da Autorização/Emissão PMI"]) != string.Empty)
                    item["Data da Autorização/Emissão PMI"] = TratarData(plano["Data da Autorização/Emissão PMI"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Previsão da Implantação"]) != string.Empty)
                    item["Previsão da Implantação"] = TratarData(plano["Previsão da Implantação"]);// - Caixa de Texto - Editável.
                
                if (ValidaTextField(plano["Status da Revitalização"]) != string.Empty)
                    item["Status da Revitalização"] = ValidaTextField(plano["Status da Revitalização"]);// - Caixa de Seleção - Editável - *.(Default: Não se aplica). (Opções: Em Andamento, Executada, Não Iniciada).
                
                if (ValidaTextField(plano["Aquisição de Materiais - Início"]) != string.Empty)
                    item["Aquisição de Materiais - Início"] = TratarData(plano["Aquisição de Materiais - Início"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Aquisição de Materiais - Fim"]) != string.Empty)
                    item["Aquisição de Materiais - Fim"] = TratarData(plano["Aquisição de Materiais - Fim"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Obras Civis - Início"]) != string.Empty)
                    item["Obras Civis - Início"] = TratarData(plano["Obras Civis - Início"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Obras Civis - Fim"]) != string.Empty)
                    item["Obras Civis - Fim"] = TratarData(plano["Obras Civis - Fim"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Montagem Eletromecânica - Início"]) != string.Empty)
                    item["Montagem Eletromecânica - Início"] = TratarData(plano["Montagem Eletromecânica - Início"]);// - Caixa de Texto - Editável.
                
                if (ValidaTextField(plano["Montagem Eletromecânica - Fim"]) != string.Empty)
                    item["Montagem Eletromecânica - Fim"] = TratarData(plano["Montagem Eletromecânica - Fim"]);// - Caixa de Texto - Editável.
                
                if (ValidaTextField(plano["Comissionamento - Início"]) != string.Empty)
                    item["Comissionamento - Início"] = TratarData(plano["Comissionamento - Início"]);// - Caixa de Texto - Editável.
                
                if (ValidaTextField(plano["Comissionamento - Fim"]) != string.Empty)
                    item["Comissionamento - Fim"] = TratarData(plano["Comissionamento - Fim"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Operação Comercial"]) != string.Empty)
                    item["Operação Comercial"] = TratarData(plano["Operação Comercial"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Observações Gerais"]) != string.Empty)
                    item["Observações Gerais"] = ValidaTextField(plano["Observações Gerais"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Numeração PMI / PMIS"]) != string.Empty)
                    item["Numeração PMI / PMIS"] = ValidaTextField(plano["Numeração PMI / PMIS"]);// - Caixa de Seleção - Editável. (Default: Não se aplica). (Exibir todos os domínios para Numeração cadastrados no sistema).

                item.Update();

                return item.ID;
            }
            catch(Exception) {
                throw new Exception("Não foi possível criar uma Mudança de Exclusão."); 
            }
        }

        public static string ValidaTextField(object fieldvalue)
        {
            return (fieldvalue != null) ? Convert.ToString(fieldvalue) : string.Empty;
        }

        private static DateTime? TratarData(object item)
        {
            if (item == null || item.ToString() == "")
                return null;

            int dia = Convert.ToInt32(item.ToString().Split('/').First());
            int mes = Convert.ToInt32(item.ToString().Split('/')[1]);
            int ano = Convert.ToInt32(item.ToString().Split('/').Last().Substring(0, 4));//11/01/2016 00:00:00

            return new DateTime(ano, mes, dia);
        }

        protected static int DeParaCamposSuperacaoExclusao(SPListItem item, SPListItem plano)
        {
            try
            {
                //item["IDPlano"] = plano.ID.ToString();
                item["Numeração"] = ValidaTextField(plano[SPBuiltInFieldId.Title]);
                item["Ciclo"] = new SPFieldLookupValue(ValidaTextField(plano["Ciclo"])).LookupValue;

                if (ValidaTextField(plano["Região"]) != string.Empty)
                    item["Região"] = ValidaTextField(plano["Região"]);

                item["Agente"] = "FURNAS";

                if (ValidaTextField(plano["Tipo de Equipamento"]) != string.Empty)
                    item["Tipo de Equipamento"] = ValidaTextField(plano["Tipo de Equipamento"]);

                if (ValidaTextField(plano["Instalação"]) != string.Empty)
                {
                    try
                    {
                        SPListItem DI = item.Web.Lists["Domínio de Instalação"].Items.OfType<SPListItem>().Where(p => p.Title == Convert.ToString(plano["Instalação"])).FirstOrDefault();
                        if (DI != null)
                            item["Instalação"] = new SPFieldLookupValue(DI.ID.ToString()); // - Caixa de Seleção - Editável - *.(Default: Não se aplica). (Exibir todos os domínios para instalação cadastrados no sistema).
                    }
                    catch { }
                }

                if (ValidaTextField(plano["(KV)"]) != string.Empty)
                    item["Tensão Nominal (KV)"] = ValidaTextField(plano["(KV)"]);  // - Caixa de Seleção - Editável - *.(Default: Não se aplica). (Exibir todos os domínios para  KV cadastrados no sistema).

                if (ValidaTextField(plano["Tipo de Agente"]) != string.Empty)
                    item["Tipo de Agente"] = ValidaTextField(plano["Tipo de Agente"]); // - Caixa de Seleção - Editável - *.(Default: Não se aplica). (Opções: Consumidor Livre, Distribuição, Geração, Transmissão).

                if (ValidaTextField(plano["Razão principal para a superação deste equipamento"]) != string.Empty)
                    item["Razão principal para a superação deste equipamento"] = ValidaTextField(plano["Razão principal para a superação deste equipamento"]);

                if (ValidaTextField(plano["Nome Do Vão"]) != string.Empty)
                    item["Nome Do Vão"] = ValidaTextField(plano["Nome Do Vão"]);

                if (ValidaTextField(plano["Código de Identificação"]) != string.Empty)
                    item["Código de Identificação"] = ValidaTextField(plano["Código de Identificação"]);

                if (ValidaTextField(plano["Função de Transmissão"]) != string.Empty)
                {
                    try
                    {
                        SPListItem FT = item.Web.Lists["Domínio de Função de Transmissão"].Items.OfType<SPListItem>().Where(p => p.Title == Convert.ToString(plano["Função de Transmissão"])).FirstOrDefault();
                        if (FT != null)
                            item["Função de Transmissão"] = new SPFieldLookupValue(FT.ID.ToString());  // - Caixa de Seleção- Editável - *.(Default: Não se aplica). (Exibir todos os domínios para "Função de Transmissão" condicionados a "Instalação" cadastrados no sistema).
                    }
                    catch { }
                }

                if (ValidaTextField(plano["Número da Barra no Anafas"]) != string.Empty)
                    item["Número da Barra no Anafas"] = ValidaTextField(plano["Número da Barra no Anafas"]);

                if (ValidaTextField(plano["Nível de curto-circuito na barra monofásico (kA)"]) != string.Empty)
                    item["Nível de curto-circuito na barra monofásico (kA)"] = ValidaTextField(plano["Nível de curto-circuito na barra monofásico (kA)"]);

                if (ValidaTextField(plano["Nível de curto-circuito na barra trifásico (kA)"]) != string.Empty)
                    item["Nível de curto-circuito na barra trifásico (kA)"] = ValidaTextField(plano["Nível de curto-circuito na barra trifásico (kA)"]);

                if (ValidaTextField(plano["X/R na barra da subestação para curto-circuito monofásico"]) != string.Empty)
                    item["X/R na barra da subestação para curto-circuito monofásico"] = ValidaTextField(plano["X/R na barra da subestação para curto-circuito monofásico"]);

                if (ValidaTextField(plano["X/R na barra da subestação para curto-circuito trifásico"]) != string.Empty)
                    item["X/R na barra da subestação para curto-circuito trifásico"] = ValidaTextField(plano["X/R na barra da subestação para curto-circuito trifásico"]);

                if (ValidaTextField(plano["Capacidade interrupção simétrica nominal (kA)"]) != string.Empty)
                    item["Capacidade interrupção simétrica nominal (kA)"] = ValidaTextField(plano["Capacidade interrupção simétrica nominal (kA)"]);

                if (ValidaTextField(plano["Corrente de curto-circuito nominal (kA)"]) != string.Empty)
                    item["Corrente de curto-circuito nominal (kA)"] = ValidaTextField(plano["Corrente de curto-circuito nominal (kA)"]);

                if (ValidaTextField(plano["Valor nominal da crista da corrente de curto-circuito (kA)"]) != string.Empty)
                    item["Valor nominal da crista da corrente de curto-circuito (kA)"] = ValidaTextField(plano["Valor nominal da crista da corrente de curto-circuito (kA)"]);

                if (ValidaTextField(plano["Quantidade de equipamentos superados"]) != string.Empty)
                    item["Quantidade de equipamentos superados"] = ValidaTextField(plano["Quantidade de equipamentos superados"]);

                if (ValidaTextField(plano["Corrente de curto-circuito monofásica através Do disjuntor (calculada) (kA)"]) != string.Empty)
                    item["Corrente de curto-circuito monofásica através Do disjuntor (calculada) (kA)"] = ValidaTextField(plano["Corrente de curto-circuito monofásica através Do disjuntor (calculada) (kA)"]);

                if (ValidaTextField(plano["Corrente de curto-circuito trifásica através Do disjuntor (calculada) (kA)"]) != string.Empty)
                    item["Corrente de curto-circuito trifásica através Do disjuntor (calculada) (kA)"] = ValidaTextField(plano["Corrente de curto-circuito trifásica através Do disjuntor (calculada) (kA)"]);

                if (ValidaTextField(plano["X/R Do disjuntor (nominal)"]) != string.Empty)
                    item["X/R Do disjuntor (nominal)"] = ValidaTextField(plano["X/R Do disjuntor (nominal)"]);

                if (ValidaTextField(plano["Fator de primeiro polo nominal"]) != string.Empty)
                    item["Fator de primeiro polo nominal"] = ValidaTextField(plano["Fator de primeiro polo nominal"]);

                if (ValidaTextField(plano["Quantidade de secionadoras superadas no mesmo vão (bay) sem chave de terra"]) != string.Empty)
                    item["Quantidade de secionadoras superadas no mesmo vão (bay) sem chave de terra"] = ValidaTextField(plano["Quantidade de secionadoras superadas no mesmo vão (bay) sem chave de terra"]);

                if (ValidaTextField(plano["Quantidade de secionadoras superadas no mesmo vão (bay) com chave de terra"]) != string.Empty)
                    item["Quantidade de secionadoras superadas no mesmo vão (bay) com chave de terra"] = ValidaTextField(plano["Quantidade de secionadoras superadas no mesmo vão (bay) com chave de terra"]);

                if (ValidaTextField(plano["Corrente de curto-circuito monofásica através da secionadora (calculada) (kA)"]) != string.Empty)
                    item["Corrente de curto-circuito monofásica através da secionadora (calculada) (kA)"] = ValidaTextField(plano["Corrente de curto-circuito monofásica através da secionadora (calculada) (kA)"]);

                if (ValidaTextField(plano["Corrente de curto-circuito trifásica através da secionadora (calculada) (kA)"]) != string.Empty)
                    item["Corrente de curto-circuito trifásica através da secionadora (calculada) (kA)"] = ValidaTextField(plano["Corrente de curto-circuito trifásica através da secionadora (calculada) (kA)"]);

                if (ValidaTextField(plano["Saturação"]) != string.Empty)
                    item["Saturação"] = ValidaTextField(plano["Saturação"]);

                if (ValidaTextField(plano["Corrente de curto-circuito monofásica através da Bobina de bloqueio (calculada) (kA)"]) != string.Empty)
                    item["Corrente de curto-circuito monofásica através da Bobina de bloqueio (calculada) (kA)"] = ValidaTextField(plano["Corrente de curto-circuito monofásica através da Bobina de bloqueio (calculada) (kA)"]);

                if (ValidaTextField(plano["Corrente de curto-circuito trifásica através da Bobina de bloqueio (calculada) (kA)"]) != string.Empty)
                    item["Corrente de curto-circuito trifásica através da Bobina de bloqueio (calculada) (kA)"] = ValidaTextField(plano["Corrente de curto-circuito trifásica através da Bobina de bloqueio (calculada) (kA)"]);

                if (ValidaTextField(plano["Corrente de curto-circuito monofásica através Do TC (calculada) (kA)"]) != string.Empty)
                    item["Corrente de curto-circuito monofásica através Do TC (calculada) (kA)"] = ValidaTextField(plano["Corrente de curto-circuito monofásica através Do TC (calculada) (kA)"]);

                if (ValidaTextField(plano["Corrente de curto-circuito trifásica através Do TC (calculada) (kA)"]) != string.Empty)
                    item["Corrente de curto-circuito trifásica através Do TC (calculada) (kA)"] = ValidaTextField(plano["Corrente de curto-circuito trifásica através Do TC (calculada) (kA)"]);

                if (ValidaTextField(plano["Fator térmico"]) != string.Empty)
                    item["Fator térmico"] = ValidaTextField(plano["Fator térmico"]);

                if (ValidaTextField(plano["Corrente Nominal (A)"]) != string.Empty)
                    item["Corrente Nominal (A)"] = ValidaTextField(plano["Corrente Nominal (A)"]);

                if (ValidaTextField(plano["Corrente de carga calculada (A)"]) != string.Empty)
                    item["Corrente de carga calculada (A)"] = ValidaTextField(plano["Corrente de carga calculada (A)"]);

                if (ValidaTextField(plano["Origem da indicação (empresa solicitante/documento de referência)"]) != string.Empty)
                    item["Origem da indicação (empresa solicitante/documento de referência)"] = ValidaTextField(plano["Origem da indicação (empresa solicitante/documento de referência)"]);

                if (ValidaTextField(plano["Data de Necessidade"]) != string.Empty)
                    item["Data de Necessidade"] = ValidaTextField(plano["Data de Necessidade"]);

                if (ValidaTextField(plano["Observação"]) != string.Empty)
                    item["Observação"] = ValidaTextField(plano["Observação"]);

                if (ValidaTextField(plano["Projeto que Originou a Indicação"]) != string.Empty)
                    item["Projeto que Originou a Indicação"] = ValidaTextField(plano["Projeto que Originou a Indicação"]);

                if (ValidaTextField(plano["DeRating 1F"]) != string.Empty)
                    item["DeRating 1F"] = ValidaTextField(plano["DeRating 1F"]);

                if (ValidaTextField(plano["DeRating 3F"]) != string.Empty)
                    item["DeRating 3F"] = ValidaTextField(plano["DeRating 3F"]);

                if (ValidaTextField(plano["Superação por corrente nominal"]) != string.Empty)
                    item["Superação por corrente nominal"] = ValidaTextField(plano["Superação por corrente nominal"]);

                if (ValidaTextField(plano["Superação por corrente de curto-circuito simétrica"]) != string.Empty)
                    item["Superação por corrente de curto-circuito simétrica"] = ValidaTextField(plano["Superação por corrente de curto-circuito simétrica"]);

                if (ValidaTextField(plano["Superação pela crista da corrente de curto-circuito"]) != string.Empty)
                    item["Superação pela crista da corrente de curto-circuito"] = ValidaTextField(plano["Superação pela crista da corrente de curto-circuito"]);

                if (ValidaTextField(plano["Superação por X/R"]) != string.Empty)
                    item["Superação por X/R"] = ValidaTextField(plano["Superação por X/R"]);

                if (ValidaTextField(plano["Capacidade nominal de curto-circuito (kA)"]) != string.Empty)
                    item["Capacidade nominal de curto-circuito (kA)"] = ValidaTextField(plano["Capacidade nominal de curto-circuito (kA)"]);

                if (ValidaTextField(plano["Capacidade nominal de corrente (A)"]) != string.Empty)
                    item["Capacidade nominal de corrente (A)"] = ValidaTextField(plano["Capacidade nominal de corrente (A)"]);

                if (ValidaTextField(plano["Outras"]) != string.Empty)
                    item["Outras"] = ValidaTextField(plano["Outras"]);

                if (ValidaTextField(plano["Situação da Análise"]) != string.Empty)
                    item["Situação da Análise"] = ValidaTextField(plano["Situação da Análise"]);

                if (ValidaTextField(plano["Parecer final"]) != string.Empty)
                    item["Parecer final"] = ValidaTextField(plano["Parecer final"]);

                if (ValidaTextField(plano["Observação ONS"]) != string.Empty)
                    item["Observação ONS"] = ValidaTextField(plano["Observação ONS"]);

                if (ValidaTextField(plano["Classificação"]) != string.Empty)
                    item["Classificação"] = ValidaTextField(plano["Classificação"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Indicações"]) != string.Empty)
                    item["Indicações"] = ValidaTextField(plano["Indicações"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Oficio/Resolução/Autorização ANEEL relativa a substituição Do equipamento"]) != string.Empty)
                    item["Oficio/Resolução/Autorização ANEEL relativa a substituição Do equipamento"] = ValidaTextField(plano["Oficio/Resolução/Autorização ANEEL relativa a substituição Do equipamento"]);

                if (ValidaTextField(plano["Data da Autorização"]) != string.Empty)
                    item["Data da Autorização"] = TratarData(plano["Data da Autorização"]);               

                if (ValidaTextField(plano["Previsão da Implantação"]) != string.Empty)
                    item["Previsão da Implantação"] = TratarData(plano["Previsão da Implantação"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Data de Entrada em Operação"]) != string.Empty)
                    item["Data de Entrada em Operação"] = TratarData(plano["Data de Entrada em Operação"]);

                if (ValidaTextField(plano["Status da Revitalização"]) != string.Empty)
                    item["Status da Revitalização"] = ValidaTextField(plano["Status da Revitalização"]);// - Caixa de Seleção - Editável - *.(Default: Não se aplica). (Opções: Em Andamento, Executada, Não Iniciada).

                if (ValidaTextField(plano["Aquisição de Materiais - Início"]) != string.Empty)
                    item["Aquisição de Materiais - Início"] = TratarData(plano["Aquisição de Materiais - Início"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Aquisição de Materiais - Fim"]) != string.Empty)
                    item["Aquisição de Materiais - Fim"] = TratarData(plano["Aquisição de Materiais - Fim"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Obras Civis - Início"]) != string.Empty)
                    item["Obras Civis - Início"] = TratarData(plano["Obras Civis - Início"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Obras Civis - Fim"]) != string.Empty)
                    item["Obras Civis - Fim"] = TratarData(plano["Obras Civis - Fim"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Montagem Eletromecânica - Início"]) != string.Empty)
                    item["Montagem Eletromecânica - Início"] = TratarData(plano["Montagem Eletromecânica - Início"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Montagem Eletromecânica - Fim"]) != string.Empty)
                    item["Montagem Eletromecânica - Fim"] = TratarData(plano["Montagem Eletromecânica - Fim"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Comissionamento - Início"]) != string.Empty)
                    item["Comissionamento - Início"] = TratarData(plano["Comissionamento - Início"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Comissionamento - Fim"]) != string.Empty)
                    item["Comissionamento - Fim"] = TratarData(plano["Comissionamento - Fim"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Operação Comercial"]) != string.Empty)
                    item["Operação Comercial"] = TratarData(plano["Operação Comercial"]);// - Caixa de Texto - Editável.

                if (ValidaTextField(plano["Observações Gerais"]) != string.Empty)
                    item["Observações Gerais"] = ValidaTextField(plano["Observações Gerais"]);// - Caixa de Texto - Editável.
                
                item["Colunas Alteradas"] = "Justificativa da Exclusão";

                item.Update();

                return item.ID;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível criar uma Mudança de Exclusão.");
            }
        }

        public static int CamposAlterados(SPListItem item)
        {
            string cicloName =ValidaTextField(item["Ciclo"]);
            SPListItem plano = item.Web.Lists["Planos"].Items.OfType<SPListItem>().Where(p => Convert.ToString(item["Numeração"]) == Convert.ToString(p[SPBuiltInFieldId.Title]) && p.ContentType.Name == "Plano Por Superação" && cicloName == new SPFieldLookupValue(ValidaTextField(p["Ciclo"])).LookupValue).FirstOrDefault();
            string campos = string.Empty;

            if (plano != null)
            {
                try
                {
                    if (ValidaTextField(plano[SPBuiltInFieldId.Title]) != ValidaTextField(item["Numeração"]))
                        campos += "Numeração;";

                    if (ValidaTextField(item["Ciclo"]) != new SPFieldLookupValue(ValidaTextField(plano["Ciclo"])).LookupValue)
                        campos += "Ciclo;";

                    if (ValidaTextField(plano["Região"]) != ValidaTextField(item["Região"]))
                        campos += "Região;";

                    if (ValidaTextField(item["Agente"]) != ValidaTextField(plano["Agente"]))
                        campos += "Agente;";

                    if (ValidaTextField(plano["Tipo de Equipamento"]) != ValidaTextField(item["Tipo de Equipamento"]))
                        campos += "Tipo de Equipamento;";

                    if (ValidaTextField(plano["Instalação"]) != new SPFieldLookupValue(ValidaTextField(item["Instalação"])).LookupValue)
                        campos += "Instalação;";

                    if (ValidaTextField(item["Instalação - Outros"]) != string.Empty)// && ValidaTextField(plano["Instalação"]) != ValidaTextField(item["Instalação - Outros"]))
                        campos += "Instalação - Outros;";

                    if (ValidaTextField(plano["Tensão Nominal (kV)"]) != ValidaTextField(item["Tensão Nominal (KV)"]))
                        campos += "Tensão Nominal (KV);";

                    if (ValidaTextField(plano["Tipo de Agente"]) != ValidaTextField(item["Tipo de Agente"]))
                        campos += "Tipo de Agente;";

                    if (ValidaTextField(plano["Razão principal para a superação deste equipamento"]) != ValidaTextField(item["Razão principal para a superação deste equipamento"]))
                        campos += "Razão principal para a superação deste equipamento;";

                    if (ValidaTextField(plano["Nome Do Vão"]) != ValidaTextField(item["Nome Do Vão"]))
                        campos += "Nome Do Vão;";

                    if (ValidaTextField(plano["Código de Identificação"]) != ValidaTextField(item["Código de Identificação"]))
                        campos += "Código de Identificação;";

                    if (ValidaTextField(plano["Função de Transmissão"]) != new SPFieldLookupValue(ValidaTextField(item["Função de Transmissão"])).LookupValue)
                        campos += "Função de Transmissão;";

                    if (ValidaTextField(item["Função de Transmissão - Outros"]) != string.Empty)// && ValidaTextField(plano["Função de Transmissão"]) != ValidaTextField(item["Função de Transmissão - Outros"]))
                        campos += "Função de Transmissão - Outros;";

                    if (ValidaTextField(plano["Número da Barra no Anafas"]) != ValidaTextField(item["Número da Barra no Anafas"]))
                        campos += "Número da Barra no Anafas;";

                    if (ValidaTextField(plano["Nível de curto-circuito na barra monofásico (kA)"]) != ValidaTextField(item["Nível de curto-circuito na barra monofásico (kA)"]))
                        campos += "Nível de curto-circuito na barra monofásico (kA);";

                    if (ValidaTextField(plano["Nível de curto-circuito na barra trifásico (kA)"]) != ValidaTextField(item["Nível de curto-circuito na barra trifásico (kA)"]))
                        campos += "Nível de curto-circuito na barra trifásico (kA);";

                    if (ValidaTextField(plano["X/R na barra da subestação para curto-circuito monofásico"]) != ValidaTextField(item["X/R na barra da subestação para curto-circuito monofásico"]))
                        campos += "X/R na barra da subestação para curto-circuito monofásico;";

                    if (ValidaTextField(plano["X/R na barra da subestação para curto-circuito trifásico"]) != ValidaTextField(item["X/R na barra da subestação para curto-circuito trifásico"]))
                        campos += "X/R na barra da subestação para curto-circuito trifásico;";

                    if (ValidaTextField(plano["Capacidade interrupção simétrica nominal (kA)"]) != ValidaTextField(item["Capacidade interrupção simétrica nominal (kA)"]))
                        campos += "Capacidade interrupção simétrica nominal (kA);";

                    if (ValidaTextField(plano["Corrente de curto-circuito nominal (kA)"]) != ValidaTextField(item["Corrente de curto-circuito nominal (kA)"]))
                        campos += "Corrente de curto-circuito nominal (kA);";

                    if (ValidaTextField(plano["Valor nominal da crista da corrente de curto-circuito (kA)"]) != ValidaTextField(item["Valor nominal da crista da corrente de curto-circuito (kA)"]))
                        campos += "Valor nominal da crista da corrente de curto-circuito (kA);";

                    if (ValidaTextField(plano["Quantidade de equipamentos superados"]) != ValidaTextField(item["Quantidade de equipamentos superados"]))
                        campos += "Quantidade de equipamentos superados;";

                    if (ValidaTextField(plano["Corrente de curto-circuito monofásica através Do disjuntor (calculada) (kA)"]) != ValidaTextField(item["Corrente de curto-circuito monofásica através Do disjuntor (calculada) (kA)"]))
                        campos += "Corrente de curto-circuito monofásica através Do disjuntor (calculada) (kA);";

                    if (ValidaTextField(plano["Corrente de curto-circuito trifásica através Do disjuntor (calculada) (kA)"]) != ValidaTextField(item["Corrente de curto-circuito trifásica através Do disjuntor (calculada) (kA)"]))
                        campos += "Corrente de curto-circuito trifásica através Do disjuntor (calculada) (kA);";

                    if (ValidaTextField(plano["X/R Do disjuntor (nominal)"]) != ValidaTextField(item["X/R Do disjuntor (nominal)"]))
                        campos += "X/R Do disjuntor (nominal);";

                    if (ValidaTextField(plano["Fator de primeiro polo nominal"]) != ValidaTextField(item["Fator de primeiro polo nominal"]))
                        campos += "Fator de primeiro polo nominal;";

                    if (ValidaTextField(plano["Quantidade de secionadoras superadas no mesmo vão (bay) sem chave de terra"]) != ValidaTextField(item["Quantidade de secionadoras superadas no mesmo vão (bay) sem chave de terra"]))
                        campos += "Quantidade de secionadoras superadas no mesmo vão (bay) sem chave de terra;";

                    if (ValidaTextField(plano["Quantidade de secionadoras superadas no mesmo vão (bay) com chave de terra"]) != ValidaTextField(item["Quantidade de secionadoras superadas no mesmo vão (bay) com chave de terra"]))
                        campos += "Quantidade de secionadoras superadas no mesmo vão (bay) com chave de terra;";

                    if (ValidaTextField(plano["Corrente de curto-circuito monofásica através da secionadora (calculada) (kA)"]) != ValidaTextField(item["Corrente de curto-circuito monofásica através da secionadora (calculada) (kA)"]))
                        campos += "Corrente de curto-circuito monofásica através da secionadora (calculada) (kA);";

                    if (ValidaTextField(plano["Corrente de curto-circuito trifásica através da secionadora (calculada) (kA)"]) != ValidaTextField(item["Corrente de curto-circuito trifásica através da secionadora (calculada) (kA)"]))
                        campos += "Corrente de curto-circuito trifásica através da secionadora (calculada) (kA);";

                    if (ValidaTextField(plano["Saturação"]) != ValidaTextField(item["Saturação"]))
                        campos += "Saturação;";

                    if (ValidaTextField(plano["Corrente de curto-circuito monofásica através da Bobina de bloqueio (calculada) (kA)"]) != ValidaTextField(item["Corrente de curto-circuito monofásica através da Bobina de bloqueio (calculada) (kA)"]))
                        campos += "Corrente de curto-circuito monofásica através da Bobina de bloqueio (calculada) (kA);";

                    if (ValidaTextField(plano["Corrente de curto-circuito trifásica através da Bobina de bloqueio (calculada) (kA)"]) != ValidaTextField(item["Corrente de curto-circuito trifásica através da Bobina de bloqueio (calculada) (kA)"]))
                        campos += "Corrente de curto-circuito trifásica através da Bobina de bloqueio (calculada) (kA);";

                    if (ValidaTextField(plano["Corrente de curto-circuito monofásica através Do TC (calculada) (kA)"]) != ValidaTextField(item["Corrente de curto-circuito monofásica através Do TC (calculada) (kA)"]))
                        campos += "Corrente de curto-circuito monofásica através Do TC (calculada) (kA);";

                    if (ValidaTextField(plano["Corrente de curto-circuito trifásica através Do TC (calculada) (kA)"]) != ValidaTextField(item["Corrente de curto-circuito trifásica através Do TC (calculada) (kA)"]))
                        campos += "Corrente de curto-circuito trifásica através Do TC (calculada) (kA);";

                    if (ValidaTextField(plano["Fator térmico"]) != ValidaTextField(item["Fator térmico"]))
                        campos += "Fator térmico;";

                    if (ValidaTextField(plano["Corrente Nominal (A)"]) != ValidaTextField(item["Corrente Nominal (A)"]))
                        campos += "Corrente Nominal (A);";

                    if (ValidaTextField(plano["Corrente de carga calculada (A)"]) != ValidaTextField(item["Corrente de carga calculada (A)"]))
                        campos += "Corrente de carga calculada (A);";

                    if (ValidaTextField(plano["Origem da indicação (empresa solicitante/documento de referência)"]) != ValidaTextField(item["Origem da indicação (empresa solicitante/documento de referência)"]))
                        campos += "Origem da indicação (empresa solicitante/documento de referência);";

                    if (ValidaTextField(plano["Data de Necessidade"]) != ValidaTextField(item["Data de Necessidade"]))
                        campos += "Data de Necessidade;";

                    if (ValidaTextField(plano["Observação Furnas"]) != ValidaTextField(item["Observação"]))//alterei
                        campos += "Observação;";

                    if (ValidaTextField(plano["Projeto que Originou a Indicação"]) != ValidaTextField(item["Projeto que Originou a Indicação"]))
                        campos += "Projeto que Originou a Indicação;";

                    if (ValidaTextField(plano["DeRating 1F"]) != ValidaTextField(item["DeRating 1F"]))
                        campos += "DeRating 1F;";

                    if (ValidaTextField(plano["DeRating 3F"]) != ValidaTextField(item["DeRating 3F"]))
                        campos += "DeRating 3F;";

                    if (ValidaTextField(plano["Superação por corrente nominal"]) != ValidaTextField(item["Superação por corrente nominal"]))
                        campos += "Superação por corrente nominal;";

                    if (ValidaTextField(plano["Superação por corrente de curto-circuito simétrica"]) != ValidaTextField(item["Superação por corrente de curto-circuito simétrica"]))
                        campos += "Superação por corrente de curto-circuito simétrica;";

                    if (ValidaTextField(plano["Superação pela crista da corrente de curto-circuito"]) != ValidaTextField(item["Superação pela crista da corrente de curto-circuito"]))
                        campos += "Superação pela crista da corrente de curto-circuito;";

                    if (ValidaTextField(plano["Superação por X/R"]) != ValidaTextField(plano["Superação por X/R"]))
                        campos += "Superação por X/R;";

                    if (ValidaTextField(plano["Capacidade nominal de curto-circuito (kA)"]) != ValidaTextField(item["Capacidade nominal de curto-circuito (kA)"]))
                        campos += "Capacidade nominal de curto-circuito (kA);";

                    if (ValidaTextField(plano["Capacidade nominal de corrente (A)"]) != ValidaTextField(item["Capacidade nominal de corrente (A)"]))
                        campos += "Capacidade nominal de corrente (A);";

                    if (ValidaTextField(plano["Outras"]) != ValidaTextField(item["Outras"]))
                        campos += "Outras;";

                    if (ValidaTextField(plano["Situação da Análise"]) != ValidaTextField(item["Situação da Análise"]))
                        campos += "Situação da Análise;";

                    if (ValidaTextField(plano["Parecer final"]) != ValidaTextField(item["Parecer final"]))
                        campos += "Parecer final;";

                    if (ValidaTextField(plano["Observação ONS"]) != ValidaTextField(item["Observação ONS"]))
                        campos += "Observação ONS;";

                    if (ValidaTextField(plano["Classificação"]) != new SPFieldLookupValue(ValidaTextField(item["Classificação"])).LookupValue)
                        campos += "Classificação;";

                    if (ValidaTextField(plano["Indicações"]) != ValidaTextField(item["Indicações"]))
                        campos += "Indicações;";

                    if (ValidaTextField(plano["Oficio/Resolução/Autorização ANEEL relativa a substituição Do equipamento"]) != ValidaTextField(item["Oficio/Resolução/Autorização ANEEL relativa a substituição Do equipamento"]))
                        campos += "Oficio/Resolução/Autorização ANEEL relativa a substituição Do equipamento;";

                    if (ValidaTextField(plano["Data da Autorização"]) != ValidaTextField(item["Data da Autorização"]))
                        campos += "Data da Autorização;";

                    if (ValidaTextField(plano["Previsão da Implantação"]) != ValidaTextField(item["Previsão da Implantação"]))
                        campos += "Previsão da Implantação;";

                    if (ValidaTextField(plano["Data de Entrada em Operação"]) != ValidaTextField(item["Data de Entrada em Operação"]))
                        campos += "Data de Entrada em Operação;";

                    if (ValidaTextField(plano["Status da Revitalização"]) != ValidaTextField(item["Status da Revitalização"]))
                        campos += "Status da Revitalização;";

                    if (ValidaTextField(plano["Aquisição de Materiais - Início"]) != ValidaTextField(item["Aquisição de Materiais - Início"]))
                        campos += "Aquisição de Materiais - Início;";

                    if (ValidaTextField(plano["Aquisição de Materiais - Fim"]) != ValidaTextField(item["Aquisição de Materiais - Fim"]))
                        campos += "Aquisição de Materiais - Fim;";

                    if (ValidaTextField(plano["Obras Civis - Início"]) != ValidaTextField(item["Obras Civis - Início"]))
                        campos += "Obras Civis - Início;";

                    if (ValidaTextField(plano["Obras Civis - Fim"]) != ValidaTextField(item["Obras Civis - Fim"]))
                        campos += "Obras Civis - Fim;";

                    if (ValidaTextField(plano["Montagem Eletromecânica - Início"]) != ValidaTextField(item["Montagem Eletromecânica - Início"]))
                        campos += "Montagem Eletromecânica - Início;";

                    if (ValidaTextField(plano["Montagem Eletromecânica - Fim"]) != ValidaTextField(item["Montagem Eletromecânica - Fim"]))
                        campos += "Montagem Eletromecânica - Fim;";

                    if (ValidaTextField(plano["Comissionamento - Início"]) != ValidaTextField(item["Comissionamento - Início"]))
                        campos += "Comissionamento - Início;";

                    if (ValidaTextField(plano["Comissionamento - Fim"]) != ValidaTextField(item["Comissionamento - Fim"]))
                        campos += "Comissionamento - Fim;";

                    if (ValidaTextField(plano["Operação Comercial"]) != ValidaTextField(item["Operação Comercial"]))
                        campos += "Operação Comercial;";

                    if (ValidaTextField(plano["Observações Gerais"]) != ValidaTextField(item["Observações Gerais"]))
                        campos += "Observações Gerais;";

                    EventFiring eventFiring = new EventFiring();
                    eventFiring.DisableHandleEventFiring();

                    item["Colunas Alteradas"] = campos;
                    //item["IDPlano"] = plano.ID.ToString();
                    item.Update();

                    eventFiring.EnableHandleEventFiring();

                    return item.ID;
                }
                catch (Exception)
                {
                    throw new Exception("Não foi possível criar uma Mudança de Exclusão.");
                }
            }
            else
                return 0;
        }

        public static int CamposAlteradosMelhoriaReforco(SPListItem item)
        {
            string cicloName = ValidaTextField(item["Ciclo"]);
            SPListItem plano = null;
            if (item.ContentType.Name.Contains("Melhoria"))
                plano = item.Web.Lists["Planos"].Items.OfType<SPListItem>().Where(p => Convert.ToString(item["Numeração"]) == Convert.ToString(p[SPBuiltInFieldId.Title]) && p.ContentType.Name == "Plano de Melhoria" && cicloName == new SPFieldLookupValue(ValidaTextField(p["Ciclo"])).LookupValue).FirstOrDefault();
            else if (item.ContentType.Name.Contains("Reforço"))
                plano = item.Web.Lists["Planos"].Items.OfType<SPListItem>().Where(p => Convert.ToString(item["Numeração"]) == Convert.ToString(p[SPBuiltInFieldId.Title]) && p.ContentType.Name == "Plano de Reforço" && cicloName == new SPFieldLookupValue(ValidaTextField(p["Ciclo"])).LookupValue).FirstOrDefault();
            //SPListItem plano = item.Web.Lists["Planos"].Items.OfType<SPListItem>().Where(p => Convert.ToString(item["Numeração"]) == Convert.ToString(p[SPBuiltInFieldId.Title])).FirstOrDefault();
            string campos = string.Empty;
            if (plano != null)
            {
                try
                {
                    if (ValidaTextField(plano[SPBuiltInFieldId.Title]) != ValidaTextField(item["Numeração"]))
                        campos += "Numeração;";

                    if (ValidaTextField(item["Ciclo"]) != new SPFieldLookupValue(ValidaTextField(plano["Ciclo"])).LookupValue)
                        campos += "Ciclo;";

                    if (ValidaTextField(plano["Região"]) != ValidaTextField(item["Região"]))
                        campos += "Região;";

                    if (ValidaTextField(item["Agente"]) != ValidaTextField(plano["Agente"]))
                        campos += "Agente;";

                    if (ValidaTextField(plano["Instalação"]) != new SPFieldLookupValue(ValidaTextField(item["Instalação"])).LookupValue)
                        campos += "Instalação;";

                    if (ValidaTextField(item["Instalação - Outros"]) != string.Empty) //&& ValidaTextField(plano["Instalação"]) != ValidaTextField(item["Instalação - Outros"]))
                        campos += "Instalação - Outros;";

                    if (ValidaTextField(plano["(KV)"]) != ValidaTextField(item["Tensão Nominal (KV)"]))
                        campos += "Tensão Nominal (KV);";

                    if (ValidaTextField(plano["Tipo de Agente"]) != ValidaTextField(item["Tipo de Agente"]))
                        campos += "Tipo de Agente;";

                    if (ValidaTextField(plano["Função de Transmissão"]) != new SPFieldLookupValue(ValidaTextField(item["Função de Transmissão"])).LookupValue)
                        campos += "Função de Transmissão;";

                    if (ValidaTextField(item["Função de Transmissão - Outros"]) != string.Empty)//&& ValidaTextField(plano["Função de Transmissão"]) != ValidaTextField(item["Função de Transmissão - Outros"]))
                        campos += "Função de Transmissão - Outros;";

                    if (ValidaTextField(plano["Origem da Indicação"]) != ValidaTextField(item["Origem da Indicação"]))
                        campos += "Origem da Indicação;";

                    if (ValidaTextField(plano["Tipo de Revitalização"]) != ValidaTextField(item["Tipo de Revitalização"]))
                        campos += "Tipo de Revitalização;";

                    if (ValidaTextField(plano["Revitalização Necessária"]) != ValidaTextField(item["Revitalização Necessária"]))
                        campos += "Revitalização Necessária;";

                    if (ValidaTextField(plano["Justificativa"]) != ValidaTextField(item["Justificativa"]))
                        campos += "Justificativa;";

                    if (ValidaTextField(plano["Justificativa da Exclusão"]) != ValidaTextField(item["Justificativa da Exclusão"]))
                        campos += "Justificativa da Exclusão;";

                    if (ValidaTextField(plano["Observação"]) != ValidaTextField(item["Observação"]))
                        campos += "Observação;";

                    if (ValidaTextField(plano["Classificação Furnas"]) != new SPFieldLookupValue(ValidaTextField(item["Classificação"])).LookupValue)
                        campos += "Classificação;";

                    if (ValidaTextField(plano["Prazo após emissão Do PMI/Resolução Aneel (meses)"]) != ValidaTextField(item["Prazo após emissão Do PMI/Resolução Aneel (meses)"]))
                        campos += "Prazo após emissão Do PMI/Resolução Aneel (meses);";

                    if (ValidaTextField(plano["Projeto que Originou a Indicação"]) != ValidaTextField(item["Projeto que Originou a Indicação"]))
                        campos += "Projeto que Originou a Indicação;";

                    if (ValidaTextField(plano["N° da recomendação no SGR"]) != ValidaTextField(item["N° da recomendação no SGR"]))
                        campos += "N° da recomendação no SGR;";

                    if (ValidaTextField(plano["Custo Estimado (R$)"]) != ValidaTextField(item["Custo Estimado (R$)"]))
                        campos += "Custo Estimado (R$);";

                    if (ValidaTextField(plano["Descrição Do equipamento a ser substituído ou dos componentes da linha"]) != ValidaTextField(item["Descrição Do equipamento a ser substituído ou dos componentes da linha"]))
                        campos += "Descrição Do equipamento a ser substituído ou dos componentes da linha;";

                    if (ValidaTextField(plano["Identificação Do equipamento a ser substituído (número operativo, Do unifilar, etc.)"]) != ValidaTextField(item["Identificação Do equipamento a ser substituído (número operativo, Do unifilar, etc.)"]))
                        campos += "Identificação Do equipamento a ser substituído (número operativo, Do unifilar, etc.);";

                    if (ValidaTextField(plano["Evidência que justifique a Melhoria ou Reforço"]) != ValidaTextField(item["Evidência que justifique a Melhoria ou Reforço"]))
                        campos += "Evidência que justifique a Melhoria ou Reforço;";

                    if (ValidaTextField(plano["Fabricante"]) != ValidaTextField(item["Fabricante"]))
                        campos += "Fabricante;";

                    if (ValidaTextField(plano["Modelo"]) != ValidaTextField(item["Modelo"]))
                        campos += "Modelo;";

                    if (ValidaTextField(plano["N° Série"]) != ValidaTextField(item["N° Série"]))
                        campos += "N° Série;";

                    if (ValidaTextField(plano["Ano Início em Operação"]) != ValidaTextField(item["Ano Início em Operação"]))
                        campos += "Ano Início em Operação;";

                    if (ValidaTextField(plano["Taxa de Depreciação (MCPSE – versão 9)"]) != ValidaTextField(item["Taxa de Depreciação (MCPSE – versão 9)"]))
                        campos += "Taxa de Depreciação (MCPSE – versão 9);";

                    if (ValidaTextField(plano["Ano Final de Vida Útil"]) != ValidaTextField(item["Ano Final de Vida Útil"]))
                        campos += "Ano Final de Vida Útil;";

                    if (ValidaTextField(plano["Substituir nos próximos 4 anos (a partir de 1º de fevereiro Do ano corrente)?"]) != ValidaTextField(item["Substituir nos próximos 4 anos (a partir de 1º de fevereiro Do ano corrente)?"]))
                        campos += "Substituir nos próximos 4 anos (a partir de 1º de fevereiro Do ano corrente)?;";

                    if (ValidaTextField(plano["Equipamento apto a permanecer por tempo adicional à Vida Útil"]) != ValidaTextField(item["Equipamento apto a permanecer por tempo adicional à Vida Útil"]))
                        campos += "Equipamento apto a permanecer por tempo adicional à Vida Útil;";

                    if (ValidaTextField(plano["Ações propostas"]) != ValidaTextField(item["Ações propostas"]))
                        campos += "Ações propostas;";

                    if (ValidaTextField(plano["Aumento de vida útil esperado, a partir da implantação Do reforço(anos)"]) != ValidaTextField(item["Aumento de vida útil esperado, a partir da implantação Do reforço(anos)"]))
                        campos += "Aumento de vida útil esperado, a partir da implantação Do reforço(anos);";

                    if (ValidaTextField(plano["Valor a ser investido(R$)"]) != ValidaTextField(item["Valor a ser investido(R$)"]))
                        campos += "Valor a ser investido(R$);";

                    if (ValidaTextField(plano["Situação da Análise"]) != ValidaTextField(item["Situação da Análise"]))
                        campos += "Situação da Análise;";

                    if (ValidaTextField(plano["Indicações"]) != ValidaTextField(item["Indicações"]))
                        campos += "Indicações;";

                    if (ValidaTextField(plano["Observação ONS"]) != ValidaTextField(item["Observação ONS"]))
                        campos += "Observação ONS;";

                    if (ValidaTextField(plano["Classificação ONS"]) != ValidaTextField(item["Classificação ONS"]))
                        campos += "Classificação ONS;";

                    if (ValidaTextField(plano["Responsável pela Análise"]) != ValidaTextField(item["Responsável pela Análise"]))
                        campos += "Responsável pela Análise;";

                    if (ValidaTextField(plano["Data de Necessidade Sistêmica"]) != ValidaTextField(item["Data de Necessidade Sistêmica"]))
                        campos += "Data de Necessidade Sistêmica;";

                    if (ValidaTextField(plano["Oficio/Resolução/Autorização ANEEL relativa a substituição Do equipamento/PMI"]) != ValidaTextField(item["Oficio/Resolução/Autorização ANEEL relativa a substituição Do equipamento/PMI"]))
                        campos += "Oficio/Resolução/Autorização ANEEL relativa a substituição Do equipamento/PMI;";

                    if (ValidaTextField(plano["Data da Autorização/Emissão PMI"]) != ValidaTextField(item["Data da Autorização/Emissão PMI"]))
                        campos += "Data da Autorização/Emissão PMI;";

                    if (ValidaTextField(plano["Previsão da Implantação"]) != ValidaTextField(item["Previsão da Implantação"]))
                        campos += "Previsão da Implantação;";

                    if (ValidaTextField(plano["Status da Revitalização"]) != ValidaTextField(item["Status da Revitalização"]))
                        campos += "Status da Revitalização;";

                    if (ValidaTextField(plano["Aquisição de Materiais - Início"]) != ValidaTextField(item["Aquisição de Materiais - Início"]))
                        campos += "Aquisição de Materiais - Início;";

                    if (ValidaTextField(plano["Aquisição de Materiais - Fim"]) != ValidaTextField(item["Aquisição de Materiais - Fim"]))
                        campos += "Aquisição de Materiais - Fim;";

                    if (ValidaTextField(plano["Obras Civis - Início"]) != ValidaTextField(item["Obras Civis - Início"]))
                        campos += "Obras Civis - Início;";

                    if (ValidaTextField(plano["Obras Civis - Fim"]) != ValidaTextField(item["Obras Civis - Fim"]))
                        campos += "Obras Civis - Fim;";

                    if (ValidaTextField(plano["Montagem Eletromecânica - Início"]) != ValidaTextField(item["Montagem Eletromecânica - Início"]))
                        campos += "Montagem Eletromecânica - Início;";

                    if (ValidaTextField(plano["Montagem Eletromecânica - Fim"]) != ValidaTextField(item["Montagem Eletromecânica - Fim"]))
                        campos += "Montagem Eletromecânica - Fim;";

                    if (ValidaTextField(plano["Comissionamento - Início"]) != ValidaTextField(item["Comissionamento - Início"]))
                        campos += "Comissionamento - Início;";

                    if (ValidaTextField(plano["Comissionamento - Fim"]) != ValidaTextField(item["Comissionamento - Fim"]))
                        campos += "Comissionamento - Fim;";

                    if (ValidaTextField(plano["Operação Comercial"]) != ValidaTextField(item["Operação Comercial"]))
                        campos += "Operação Comercial;";

                    if (ValidaTextField(plano["Observações Gerais"]) != ValidaTextField(item["Observações Gerais"]))
                        campos += "Observações Gerais;";

                    if (ValidaTextField(plano["Numeração PMI / PMIS"]) != ValidaTextField(item["Numeração PMI / PMIS"]))
                        campos += "Numeração PMI / PMIS;";

                    EventFiring eventFiring = new EventFiring();
                    eventFiring.DisableHandleEventFiring();

                    item["Colunas Alteradas"] = campos;
                    //item["IDPlano"] = plano.ID.ToString();
                    item.Update();

                    eventFiring.EnableHandleEventFiring();

                    return item.ID;
                }
                catch (Exception)
                {
                    throw new Exception("Não foi possível criar uma Mudança de Exclusão.");
                }
            }
            else
                return 0;
        }

        public static void NumeracaoParaGestao_Solicitacao(SPListItem item)
        {
            try
            {
                SPListItem itemg = item.Web.Lists["Gestão Interna"].GetItemById(Convert.ToInt32(item["Número de Gestão"].ToString()));

                EventFiring eventFiring = new EventFiring();
                eventFiring.DisableHandleEventFiring();

                itemg["No. SGPMR"] = item["Numeração"].ToString();
                itemg.Update();

                eventFiring.EnableHandleEventFiring();
            }
            catch { }
        }
    }
}
