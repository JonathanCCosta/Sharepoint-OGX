﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Furnas.OGX2.MTC.WebPartsPaginas.WP_AcoesRevisao {
    using System.Web.UI.WebControls.Expressions;
    using System.Web.UI.HtmlControls;
    using System.Collections;
    using System.Text;
    using System.Web.UI;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.SharePoint.WebPartPages;
    using System.Web.SessionState;
    using System.Configuration;
    using Microsoft.SharePoint;
    using System.Web;
    using System.Web.DynamicData;
    using System.Web.Caching;
    using System.Web.Profile;
    using System.ComponentModel.DataAnnotations;
    using System.Web.UI.WebControls;
    using System.Web.Security;
    using System;
    using Microsoft.SharePoint.Utilities;
    using System.Text.RegularExpressions;
    using System.Collections.Specialized;
    using System.Web.UI.WebControls.WebParts;
    using Microsoft.SharePoint.WebControls;
    
    
    public partial class WP_AcoesRevisao {
        
        protected global::System.Web.UI.WebControls.Button btRevisao;
        
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl actionsRevisao;
        
        public static implicit operator global::System.Web.UI.TemplateControl(WP_AcoesRevisao target) 
        {
            return target == null ? null : target.TemplateControl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.Button @__BuildControlbtRevisao() {
            global::System.Web.UI.WebControls.Button @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Button();
            this.btRevisao = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "btRevisao";
            @__ctrl.Text = "Solicitar Revisão do MTC";
            @__ctrl.ClientIDMode = global::System.Web.UI.ClientIDMode.Static;
            @__ctrl.Width = new System.Web.UI.WebControls.Unit(180D, global::System.Web.UI.WebControls.UnitType.Pixel);
            @__ctrl.Click -= new System.EventHandler(this.btRevisao_Click);
            @__ctrl.Click += new System.EventHandler(this.btRevisao_Click);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.HtmlControls.HtmlGenericControl @__BuildControlactionsRevisao() {
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl;
            @__ctrl = new global::System.Web.UI.HtmlControls.HtmlGenericControl("fieldset");
            this.actionsRevisao = @__ctrl;
            @__ctrl.ID = "actionsRevisao";
            @__ctrl.Visible = false;
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n    <legend>Ações de Solicitação</legend>\r\n    <div>\r\n        <div class=\"espac" +
                        "o\">"));
            global::System.Web.UI.WebControls.Button @__ctrl1;
            @__ctrl1 = this.@__BuildControlbtRevisao();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("</div>\r\n        <br /><br />\r\n    </div>\r\n"));
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private void @__BuildControlTree(global::Furnas.OGX2.MTC.WebPartsPaginas.WP_AcoesRevisao.WP_AcoesRevisao @__ctrl) {
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"

<style type=""text/css"">
    fieldset {
        padding:20px;
        width:100%;
    }
    .espaco {
        float:left;
        margin-right:10px;
    }
    #alteracaoPlano {
        border-radius: 5px;
        padding: 5px 10px 5px 35px;
        background: url(../../SiteAssets/edit_24x24.png) left 8px top 7px no-repeat;
        height: 40px;
        width: 200px!important;
        cursor:pointer;
    }
</style>

"));
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl1;
            @__ctrl1 = this.@__BuildControlactionsRevisao();
            @__parser.AddParsedSubObject(@__ctrl1);
        }
        
        private void InitializeControl() {
            this.@__BuildControlTree(this);
            this.Load += new global::System.EventHandler(this.Page_Load);
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        protected virtual object Eval(string expression) {
            return global::System.Web.UI.DataBinder.Eval(this.Page.GetDataItem(), expression);
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        protected virtual string Eval(string expression, string format) {
            return global::System.Web.UI.DataBinder.Eval(this.Page.GetDataItem(), expression, format);
        }
    }
}