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
    internal class AccountController_en {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal AccountController_en() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WorkForYou.WebUI.Resources.Controllers.AccountController.en", typeof(AccountController_en).Assembly);
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
        ///   Looks up a localized string similar to An error occurred while trying to change the password.
        /// </summary>
        internal static string ChangePasswordError {
            get {
                return ResourceManager.GetString("ChangePasswordError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Password changed successfully.
        /// </summary>
        internal static string ChangePasswordSuccess {
            get {
                return ResourceManager.GetString("ChangePasswordSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You have successfully exited.
        /// </summary>
        internal static string LogoutSuccess {
            get {
                return ResourceManager.GetString("LogoutSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred while trying to change the user&apos;s general information.
        /// </summary>
        internal static string RefreshProfileError {
            get {
                return ResourceManager.GetString("RefreshProfileError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to General user information successfully changed.
        /// </summary>
        internal static string RefreshProfileSuccess {
            get {
                return ResourceManager.GetString("RefreshProfileSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error trying to delete user.
        /// </summary>
        internal static string RemoveUserError {
            get {
                return ResourceManager.GetString("RemoveUserError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User deleted successfully.
        /// </summary>
        internal static string RemoveUserSuccess {
            get {
                return ResourceManager.GetString("RemoveUserSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error loading picture.
        /// </summary>
        internal static string UploadImageError {
            get {
                return ResourceManager.GetString("UploadImageError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The picture has been successfully changed.
        /// </summary>
        internal static string UploadImageSuccess {
            get {
                return ResourceManager.GetString("UploadImageSuccess", resourceCulture);
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