﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dialy {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.7.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("17")]
        public int FontSize {
            get {
                return ((int)(this["FontSize"]));
            }
            set {
                this["FontSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string FolderPath {
            get {
                return ((string)(this["FolderPath"]));
            }
            set {
                this["FolderPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("14")]
        public int TaskFontSize {
            get {
                return ((int)(this["TaskFontSize"]));
            }
            set {
                this["TaskFontSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("14")]
        public int SearchFontSize {
            get {
                return ((int)(this["SearchFontSize"]));
            }
            set {
                this["SearchFontSize"] = value;
            }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public Dialy.WindowStat MainWindowStat
        {
            get
            {
                return (Dialy.WindowStat)(this["MainWindowStat"]);
            }
            set
            {
                this["MainWindowStat"] = value;
            }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public Dialy.WindowStat TaskWindowStat
        {
            get
            {
                return (Dialy.WindowStat)(this["TaskWindowStat"]);
            }
            set
            {
                this["TaskWindowStat"] = value;
            }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public Dialy.WindowStat SearchWindowStat
        {
            get
            {
                return (Dialy.WindowStat)(this["SearchWindowStat"]);
            }
            set
            {
                this["SearchWindowStat"] = value;
            }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public System.Collections.Generic.List<char> HeadSpaces
        {
            get
            {
                return ((System.Collections.Generic.List<char>)(this["HeadSpaces"]));
            }
            set
            {
                this["HeadSpaces"] = value;
            }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public System.Collections.Generic.List<char> HeadMarks
        {
            get
            {
                return ((System.Collections.Generic.List<char>)(this["HeadMarks"]));
            }
            set
            {
                this["HeadMarks"] = value;
            }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string Entropy
        {
            get
            {
                return ((string)(this["Entropy"]));
            }
            set
            {
                this["Entropy"] = value;
            }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("200")]
        public System.Windows.GridLength TaskAreaWidth
        {
            get
            {
                return ((System.Windows.GridLength)(this["TaskAreaWidth"]));
            }
            set
            {
                this["TaskAreaWidth"] = value;
            }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string TaskAreaValue
        {
            get
            {
                return ((string)(this["TaskAreaValue"]));
            }
            set
            {
                this["TaskAreaValue"] = value;
            }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("14")]
        public int SearchLogLimit
        {
            get
            {
                return ((int)(this["SearchLogLimit"]));
            }
            set
            {
                this["SearchLogLimit"] = value;
            }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public System.Collections.Generic.List<string> SearchLog
        {
            get
            {
                return ((System.Collections.Generic.List<string>)(this["SearchLog"]));
            }
            set
            {
                this["SearchLog"] = value;
            }
        }
    }
}
