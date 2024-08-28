using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Model
{
#if WPF
    public class ApplicationData
    {
        public class Current
        {

            [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.0.1.0")]
            internal sealed partial class RoamingSettings : global::System.Configuration.ApplicationSettingsBase
            {

                private static RoamingSettings defaultInstance = ((RoamingSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new RoamingSettings())));

                public static RoamingSettings Values
                {
                    get
                    {
                        return defaultInstance;
                    }
                }

                [global::System.Configuration.UserScopedSettingAttribute()]
                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.Configuration.DefaultSettingValueAttribute("0")]
                public int UID
                {
                    get
                    {
                        return ((int)(this["UID"]));
                    }
                    set
                    {
                        this["UID"] = value;
                    }
                }

                [global::System.Configuration.UserScopedSettingAttribute()]
                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.Configuration.DefaultSettingValueAttribute("0")]
                public int SessionID
                {
                    get
                    {
                        return ((int)(this["SessionID"]));
                    }
                    set
                    {
                        this["SessionID"] = value;
                    }
                }

                [global::System.Configuration.UserScopedSettingAttribute()]
                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.Configuration.DefaultSettingValueAttribute("0")]
                public int ProfileVersion
                {
                    get
                    {
                        return ((int)(this["ProfileVersion"]));
                    }
                    set
                    {
                        this["ProfileVersion"] = value;
                    }
                }

                [global::System.Configuration.UserScopedSettingAttribute()]
                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.Configuration.DefaultSettingValueAttribute("")]
                public string RegistrationAESKey
                {
                    get
                    {
                        return ((string)(this["RegistrationAESKey"]));
                    }
                    set
                    {
                        this["RegistrationAESKey"] = value;
                    }
                }

                [global::System.Configuration.UserScopedSettingAttribute()]
                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.Configuration.DefaultSettingValueAttribute("")]
                public string Email
                {
                    get
                    {
                        return ((string)(this["Email"]));
                    }
                    set
                    {
                        this["Email"] = value;
                    }
                }

                [global::System.Configuration.UserScopedSettingAttribute()]
                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.Configuration.DefaultSettingValueAttribute("")]
                public string SessionKey
                {
                    get
                    {
                        return ((string)(this["SessionKey"]));
                    }
                    set
                    {
                        this["SessionKey"] = value;
                    }
                }

                [global::System.Configuration.UserScopedSettingAttribute()]
                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.Configuration.DefaultSettingValueAttribute("")]
                public string ContainerName
                {
                    get
                    {
                        return ((string)(this["ContainerName"]));
                    }
                    set
                    {
                        this["ContainerName"] = value;
                    }
                }

                [global::System.Configuration.UserScopedSettingAttribute()]
                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.Configuration.DefaultSettingValueAttribute("")]
                public string ServerRSAKey
                {
                    get
                    {
                        return ((string)(this["ServerRSAKey"]));
                    }
                    set
                    {
                        this["ServerRSAKey"] = value;
                    }
                }

                [global::System.Configuration.UserScopedSettingAttribute()]
                [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                [global::System.Configuration.DefaultSettingValueAttribute("")]
                public string MasterAESKey
                {
                    get
                    {
                        return ((string)(this["MasterAESKey"]));
                    }
                    set
                    {
                        this["MasterAESKey"] = value;
                    }
                }


            }
        }
    }
#endif
}
