﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Furnas.OGX2.Webparts.WP_ExclusaoMudancaJustificativa {
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
    
    
    public partial class WP_ExclusaoMudancaJustificativa {
        
        protected global::System.Web.UI.HtmlControls.HtmlTextArea justificativa;
        
        protected global::System.Web.UI.WebControls.Button btSalvar;
        
        protected global::System.Web.UI.WebControls.Button btCancel;
        
        protected global::System.Web.UI.WebControls.TextBox NumSolicitacao;
        
        public static implicit operator global::System.Web.UI.TemplateControl(WP_ExclusaoMudancaJustificativa target) 
        {
            return target == null ? null : target.TemplateControl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.HtmlControls.HtmlTextArea @__BuildControljustificativa() {
            global::System.Web.UI.HtmlControls.HtmlTextArea @__ctrl;
            @__ctrl = new global::System.Web.UI.HtmlControls.HtmlTextArea();
            this.justificativa = @__ctrl;
            @__ctrl.ID = "justificativa";
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("style", "width:400px;height:200px;");
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.Button @__BuildControlbtSalvar() {
            global::System.Web.UI.WebControls.Button @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Button();
            this.btSalvar = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "btSalvar";
            @__ctrl.Text = "Salvar";
            @__ctrl.ClientIDMode = global::System.Web.UI.ClientIDMode.Static;
            @__ctrl.CssClass = "btMod";
            @__ctrl.Click -= new System.EventHandler(this.btSalvar_Click);
            @__ctrl.Click += new System.EventHandler(this.btSalvar_Click);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.Button @__BuildControlbtCancel() {
            global::System.Web.UI.WebControls.Button @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Button();
            this.btCancel = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "btCancel";
            @__ctrl.Text = "Cancelar";
            @__ctrl.CssClass = "btMod";
            @__ctrl.OnClientClick = "CloseFormCancel()";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.TextBox @__BuildControlNumSolicitacao() {
            global::System.Web.UI.WebControls.TextBox @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.TextBox();
            this.NumSolicitacao = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "NumSolicitacao";
            @__ctrl.ClientIDMode = global::System.Web.UI.ClientIDMode.Static;
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private void @__BuildControlTree(global::Furnas.OGX2.Webparts.WP_ExclusaoMudancaJustificativa.WP_ExclusaoMudancaJustificativa @__ctrl) {
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n<style>\r\n    .btMod {width:70px;}\r\n    .ms-webpartPage-root {border-spacing:1" +
                        "0px!important;}\r\n    #DeltaPlaceHolderMain {width: auto!important;}\r\n    .ms-cui" +
                        "-ribbonTopBars {display:none!important;}\r\n    #pageTitle{display:none!important;" +
                        "}\r\n\r\n     .modal{\r\n        position: fixed;\r\n        top: 0;\r\n        left: 0;\r\n" +
                        "        background-color: black;\r\n        z-index: 99;\r\n        opacity: 0.3;\r\n " +
                        "       filter: alpha(opacity=30);\r\n        -moz-opacity: 0.3;\r\n        min-heigh" +
                        "t: 100%;\r\n        width: 100%;\r\n    }\r\n\r\n    .loading{\r\n        text-align: cent" +
                        "er;\r\n        font-family: Arial;\r\n        font-size: 10pt;\r\n        border: 2px " +
                        "solid #67CFF5;\r\n        width: 200px;\r\n        height: 110px;\r\n        display: " +
                        "none;\r\n        position: fixed;\r\n        background-color: White;\r\n        z-ind" +
                        "ex: 999;\r\n        font-weight:bold;\r\n    }\r\n</style>\r\n\r\n<script type=\"text/javas" +
                        "cript\" src=\"http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js\"></s" +
                        "cript>\r\n\r\n<script type=\"text/javascript\">\r\n\r\n    function ShowProgress() {\r\n    " +
                        "    setTimeout(function () {\r\n            var modal = $(\'<div />\');\r\n           " +
                        " modal.addClass(\"modal\");\r\n            $(\'body\').append(modal);\r\n            var" +
                        " loading = $(\".loading\");\r\n            loading.show();\r\n            var top = Ma" +
                        "th.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);\r\n            va" +
                        "r left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);\r\n     " +
                        "       loading.css({ top: top, left: left });\r\n        }, 200);\r\n    }\r\n\r\n</scri" +
                        "pt>\r\n\r\n<script type=\"text/javascript\" src=\"../SiteAssets/JS/justificativaExclusa" +
                        "oMudanca.js\"></script>\r\n<div style=\"width:400px\">\r\n    <div>\r\n        <span styl" +
                        "e=\"font-weight:bold;font-size:16px;\">Justificativa da Exclusão:</span>\r\n        " +
                        "<br /><br />\r\n        "));
            global::System.Web.UI.HtmlControls.HtmlTextArea @__ctrl1;
            @__ctrl1 = this.@__BuildControljustificativa();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n     </div>\r\n    <div style=\"margin-top:20px;text-align:center;\">\r\n        "));
            global::System.Web.UI.WebControls.Button @__ctrl2;
            @__ctrl2 = this.@__BuildControlbtSalvar();
            @__parser.AddParsedSubObject(@__ctrl2);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("&nbsp;&nbsp;\r\n        "));
            global::System.Web.UI.WebControls.Button @__ctrl3;
            @__ctrl3 = this.@__BuildControlbtCancel();
            @__parser.AddParsedSubObject(@__ctrl3);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n    </div>\r\n</div>\r\n"));
            global::System.Web.UI.WebControls.TextBox @__ctrl4;
            @__ctrl4 = this.@__BuildControlNumSolicitacao();
            @__parser.AddParsedSubObject(@__ctrl4);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n<div class=\"loading\">\r\n    <br />Carregando, por favor aguarde...<br /><br />\r\n" +
                        "    <img src=\"../SiteAssets/CSS/ajax-loader.gif\" alt=\"\" />\r\n</div>"));
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