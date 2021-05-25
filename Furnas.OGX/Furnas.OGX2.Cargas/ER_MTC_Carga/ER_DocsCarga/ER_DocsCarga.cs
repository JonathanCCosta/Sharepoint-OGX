using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using System.IO;
using System.Data;
using Furnas.OGX2.Cargas.ResourcesCarga;
using System.Linq;
using Microsoft.SharePoint.Taxonomy;

namespace Furnas.OGX2.Cargas.ER_MTC_Carga.ER_DocsCarga
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class ER_DocsCarga : SPItemEventReceiver
    {
        /// <summary>
        /// An item was added.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);
            try
            {
                Stream s = ResourceCarga.LerDocumentNaBiblioteca(properties.Web, "Carga MTC.xlsx");
                DataTable dt = ResourceCarga.ReadExcel(s);

                TermCollection all = null;
                TaxonomySession taxonomySession = new TaxonomySession(properties.Site);
                TermStore termStore = taxonomySession.TermStores[0];
                if (termStore != null)
                {
                    foreach (Group group in termStore.Groups)
                    {
                        //if (group.Name.Contains("donet"))//("portalativosfurnas"))
                        if (group.Name.Contains("donet"))
                        {
                            TermSetCollection termSetColl = group.TermSets;
                            foreach (TermSet termSet in termSetColl)
                            {
                                if (termSet.Name == "Palavra Chave")
                                {
                                    all = termSet.Terms;
                                }
                            }
                        }
                    }
                }

                SPList MTC = properties.Web.Lists["Manual Técnico de Campo"];
                SPList Tipo = properties.Web.Lists["Tipo MTC"];
                SPList Fabricante = properties.Web.Lists["Fabricante MTC"];
                SPList Grupo = properties.Web.Lists["Grupo MTC"];
                SPList Subgrupo = properties.Web.Lists["Subgrupo MTC"];
                SPList Resposanvel = properties.Web.Lists["Gerência Responsável MTC"];
                int linha = 0;

                TaxonomyField taxonomyField = MTC.Fields["Palavra Chave"] as TaxonomyField;
                //TaxonomyFieldValue taxonomyFieldValue = new TaxonomyFieldValue(taxonomyField);
                TaxonomyFieldValueCollection taxonomyFieldValues = new TaxonomyFieldValueCollection(taxonomyField);
                
                string dir = @"\\corp0606\APLICACOES\OGX2\Anexos\";
                string[] files = Directory.GetFiles(dir);

                string dirDoc = @"\\corp0606\APLICACOES\OGX2\Anexos_GGA\";
                string[] filesDoc = Directory.GetFiles(dirDoc);

                EventFiringEnabled = false;
                foreach (DataRow row in dt.Rows)
                {
                    //if (linha >= 5)
                    //    break;
                    //{
                    try
                    {
                        SPContentType ct = MTC.ContentTypes["Legado"];
                        SPListItem item = MTC.AddItem();

                        item["ContentType"] = ct.Name;
                        SPListItem tipo = Tipo.GetItems().OfType<SPListItem>().Where(p => p["Código"].ToString() == row[1].ToString()).FirstOrDefault();
                        item["Tipo MTC"] = new SPFieldLookupValue(tipo.ID, tipo["Descrição Tipo"].ToString());
                        item["Descrição do MTC"] = row[3].ToString();
                        item["Controle de Status"] = row[7].ToString();
                        item["Revisão"] = row[9].ToString().Length < 2 ? "0" + row[9].ToString() : row[9].ToString();
                        item["Data de Início de Vigência"] = Convert.ToDateTime(row[12].ToString()).Date;

                        SPListItem grupo = Grupo.GetItems().OfType<SPListItem>().Where(p => p["Código do Grupo"].ToString() == row[14].ToString()).FirstOrDefault();
                        item["Grupo"] = new SPFieldLookupValue(grupo.ID, grupo["Descrição Grupo"].ToString());

                        SPListItem subgrupo = Subgrupo.GetItems().OfType<SPListItem>().Where(p => p["Código do Subgrupo"].ToString() == row[15].ToString() && new SPFieldLookupValue(p["Código do Grupo"].ToString()).LookupId == grupo.ID).FirstOrDefault();
                        item["Subgrupo"] = new SPFieldLookupValue(subgrupo.ID, subgrupo["Descrição Subgrupo"].ToString());

                        SPListItem fabricante = Fabricante.GetItems().OfType<SPListItem>().Where(p => p["Código do Fabricante"].ToString() == row[17].ToString()).FirstOrDefault();
                        item["Fabricante"] = new SPFieldLookupValue(fabricante.ID, fabricante["Descrição Fabricante"].ToString());

                        string caracteristica = row[19].ToString().Length < 2 ? "0" + row[19].ToString() : row[19].ToString();

                        item["Consecutivo"] = row[20].ToString().Length < 2 ? "0" + row[20].ToString() : row[20].ToString();

                        SPListItem resp = Resposanvel.GetItems().OfType<SPListItem>().Where(p => p["Órgão"].ToString().Contains(row[22].ToString())).FirstOrDefault();
                        item["Órgão Responsável"] = new SPFieldLookupValue(resp.ID, resp["Órgão"].ToString());

                        if (row[23].ToString() != "")
                        {
                            string cel = row[23].ToString().Replace("_x000D_", ";");
                            foreach (string palavra in cel.Split(';'))
                            {
                                if (palavra != "" && palavra != " ")
                                {
                                    Term term = all.Where<Term>(p => p.Name == palavra.TrimEnd()).First();
                                    TaxonomyFieldValue taxonomyFieldValue = new TaxonomyFieldValue(taxonomyField);
                                    taxonomyFieldValue.TermGuid = term.Id.ToString();
                                    taxonomyFieldValue.Label = term.Name;
                                    taxonomyFieldValues.Add(taxonomyFieldValue);
                                }
                            }
                            taxonomyField.SetFieldValue(item, taxonomyFieldValues);
                        }
                        //palavra chave **********************8

                        if (row[25].ToString() != "")
                            item["Objetivo"] = row[25].ToString();

                        if (row[27].ToString() != "")
                            item["Elaborado em"] = row[27].ToString().PadLeft(10);

                        if (row[28].ToString() != "")
                            item["Autores do MTC"] = row[28].ToString().Replace(";", " ");

                        if (row[29].ToString() != "")
                            item["Criados Por"] = row[29].ToString();

                        if (row[30].ToString() != "")
                            item["Criados Em"] = row[30].ToString().PadLeft(10);

                        if (row[31].ToString() != "")
                            item["Padronizado por"] = row[31].ToString();

                        if (row[32].ToString() != "")
                            item["Padronizado em"] = row[32].ToString().PadLeft(10);

                        if (row[33].ToString() != "")
                            item["Enviado para Liberação para Publicação Por"] = row[33].ToString();

                        if (row[34].ToString() != "")
                            item["Enviado para Liberação para Publicação em"] = row[34].ToString().PadLeft(10);

                        if (row[35].ToString() != "")
                            item["Liberado por"] = row[35].ToString();

                        if (row[36].ToString() != "")
                            item["Liberado em"] = row[36].ToString().PadLeft(10);

                        DateTime dt1 = Convert.ToDateTime(row[12].ToString());
                        DateTime dt2 = Convert.ToDateTime(row[38].ToString());
                        TimeSpan diffResult = dt2.Subtract(dt1);
                        if ((diffResult.TotalDays / 360) < 1)
                            item["Prazo de Vigência (anos)"] = "1";
                        else
                            item["Prazo de Vigência (anos)"] = Convert.ToInt32(diffResult.TotalDays / 360);

                        if (row[39].ToString() != "")
                            item["Cancelado por"] = row[39].ToString();

                        if (row[40].ToString() != "")
                            item["Cancelado em"] = row[40].ToString().PadLeft(10);

                        if (row[41].ToString() != "")
                            item["Justificativa Cancelamento"] = row[41].ToString();
                        //}

                        item["Código MTC"] = row[1].ToString() + "." + (row[14].ToString().Length < 2 ? "0" + row[14].ToString() : row[14].ToString()) + (row[15].ToString().Length < 2 ? "0" + row[15].ToString() : row[15].ToString()) +
                            row[17].ToString() + "." + caracteristica + "/" + (row[20].ToString().Length < 2 ? "0" + row[20].ToString() : row[20].ToString()) +
                            "-R" + row[9].ToString();

                        item.Update();



                        foreach (string f in files)
                        {
                            if (f.Substring(dir.Length).Split('-')[0] == row[0].ToString())
                            {
                                ResourceCarga.PegaArquivo(properties.Web, f, item);
                                break;
                            }
                        }

                        foreach (string f in filesDoc)
                        {
                            if (f.Substring(dirDoc.Length) == (row[5].ToString() + Path.GetExtension(f)))
                            {
                                ResourceCarga.PegaArquivo(properties.Web, f, item);
                                break;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        ResourceCarga.WriteLog(properties.Web, "Erro: Carregar MTCs", linha+e.Message);
                    }

                    linha++;
                }

                ResourceCarga.WriteLog(properties.Web, "Sucesso: Carregar MTCs", "Carga realizada com sucesso!");
                EventFiringEnabled = true;
            }
            catch (Exception ex)
            {
                ResourceCarga.WriteLog(properties.Web, "Erro: Carregar MTCs", ex.Message);
            }
        }


    }
}