﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmartGen.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.4.0.0")]
    internal sealed partial class Defaults : global::System.Configuration.ApplicationSettingsBase {
        
        private static Defaults defaultInstance = ((Defaults)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Defaults())));
        
        public static Defaults Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("TwoPoint")]
        public global::SmartGen.Types.CrossoverType Crossover {
            get {
                return ((global::SmartGen.Types.CrossoverType)(this["Crossover"]));
            }
            set {
                this["Crossover"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.65")]
        public double CrossoverProbability {
            get {
                return ((double)(this["CrossoverProbability"]));
            }
            set {
                this["CrossoverProbability"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("FlipBit")]
        public global::SmartGen.Types.MutationType Mutation {
            get {
                return ((global::SmartGen.Types.MutationType)(this["Mutation"]));
            }
            set {
                this["Mutation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.05")]
        public double MutationProbability {
            get {
                return ((double)(this["MutationProbability"]));
            }
            set {
                this["MutationProbability"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Tournament")]
        public global::SmartGen.Types.SelectionType Selection {
            get {
                return ((global::SmartGen.Types.SelectionType)(this["Selection"]));
            }
            set {
                this["Selection"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public int SelectionSize {
            get {
                return ((int)(this["SelectionSize"]));
            }
            set {
                this["SelectionSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("50")]
        public int MaxGenerationCount {
            get {
                return ((int)(this["MaxGenerationCount"]));
            }
            set {
                this["MaxGenerationCount"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("100")]
        public int PopulationSize {
            get {
                return ((int)(this["PopulationSize"]));
            }
            set {
                this["PopulationSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("15")]
        public double ErrorTolerance {
            get {
                return ((double)(this["ErrorTolerance"]));
            }
            set {
                this["ErrorTolerance"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("100")]
        public int InputLayerSize {
            get {
                return ((int)(this["InputLayerSize"]));
            }
            set {
                this["InputLayerSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public int OutputLayerSize {
            get {
                return ((int)(this["OutputLayerSize"]));
            }
            set {
                this["OutputLayerSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("-5")]
        public double MinWeight {
            get {
                return ((double)(this["MinWeight"]));
            }
            set {
                this["MinWeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public double MaxWeight {
            get {
                return ((double)(this["MaxWeight"]));
            }
            set {
                this["MaxWeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public double Bias {
            get {
                return ((double)(this["Bias"]));
            }
            set {
                this["Bias"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("TanH")]
        public global::SmartGen.Types.ActivationFunctionType ActivationFunction {
            get {
                return ((global::SmartGen.Types.ActivationFunctionType)(this["ActivationFunction"]));
            }
            set {
                this["ActivationFunction"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfInt xmlns:xsi=\"http://www.w3.org" +
            "/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <int>" +
            "10</int>\r\n</ArrayOfInt>")]
        public int[] HiddenLayers {
            get {
                return ((int[])(this["HiddenLayers"]));
            }
            set {
                this["HiddenLayers"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public double T {
            get {
                return ((double)(this["T"]));
            }
            set {
                this["T"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public int LinesToSkip {
            get {
                return ((int)(this["LinesToSkip"]));
            }
            set {
                this["LinesToSkip"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public int ClassesCount {
            get {
                return ((int)(this["ClassesCount"]));
            }
            set {
                this["ClassesCount"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(",")]
        public char ColumnSeparator {
            get {
                return ((char)(this["ColumnSeparator"]));
            }
            set {
                this["ColumnSeparator"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool DotAsDecimalSeparator {
            get {
                return ((bool)(this["DotAsDecimalSeparator"]));
            }
            set {
                this["DotAsDecimalSeparator"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public int TrainingRatio {
            get {
                return ((int)(this["TrainingRatio"]));
            }
            set {
                this["TrainingRatio"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public int TestRatio {
            get {
                return ((int)(this["TestRatio"]));
            }
            set {
                this["TestRatio"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int ValidationRatio {
            get {
                return ((int)(this["ValidationRatio"]));
            }
            set {
                this["ValidationRatio"] = value;
            }
        }
    }
}
