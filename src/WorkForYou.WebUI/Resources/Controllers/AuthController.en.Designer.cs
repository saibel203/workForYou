﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WorkForYou.WebUI.Resources.Controllers {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class AuthController_en {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal AuthController_en() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WorkForYou.WebUI.Resources.Controllers.AuthController.en", typeof(AuthController_en).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error while trying to confirm Email.
        /// </summary>
        internal static string ConfirmEmailError {
            get {
                return ResourceManager.GetString("ConfirmEmailError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Email has been successfully verified.
        /// </summary>
        internal static string ConfirmEmailSuccess {
            get {
                return ResourceManager.GetString("ConfirmEmailSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred while trying to log in.
        /// </summary>
        internal static string LoginError {
            get {
                return ResourceManager.GetString("LoginError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You have successfully logged in.
        /// </summary>
        internal static string LoginSuccess {
            get {
                return ResourceManager.GetString("LoginSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred while trying to register.
        /// </summary>
        internal static string RegisterError {
            get {
                return ResourceManager.GetString("RegisterError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You have successfully registered.
        /// </summary>
        internal static string RegisterSuccess {
            get {
                return ResourceManager.GetString("RegisterSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The operation was successful. Check your email.
        /// </summary>
        internal static string RemindPasswordSuccess {
            get {
                return ResourceManager.GetString("RemindPasswordSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred while trying to reset the password.
        /// </summary>
        internal static string ResetPasswordError {
            get {
                return ResourceManager.GetString("ResetPasswordError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Password changed successfully.
        /// </summary>
        internal static string ResetPasswordSuccess {
            get {
                return ResourceManager.GetString("ResetPasswordSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred.
        /// </summary>
        internal static string SomeError {
            get {
                return ResourceManager.GetString("SomeError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User not found or an error occurred.
        /// </summary>
        internal static string UserNotFound {
            get {
                return ResourceManager.GetString("UserNotFound", resourceCulture);
            }
        }
    }
}
