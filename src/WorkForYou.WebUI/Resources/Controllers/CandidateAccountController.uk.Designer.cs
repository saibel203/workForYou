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
    internal class CandidateAccountController_uk {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CandidateAccountController_uk() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WorkForYou.WebUI.Resources.Controllers.CandidateAccountController.uk", typeof(CandidateAccountController_uk).Assembly);
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
        ///   Looks up a localized string similar to Вакансій за запитом не знайдено.
        /// </summary>
        internal static string NotFoundError {
            get {
                return ResourceManager.GetString("NotFoundError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Вакансій за запитом не знайдено або до списку не було додано жодної вакансії.
        /// </summary>
        internal static string NotFoundFavourite {
            get {
                return ResourceManager.GetString("NotFoundFavourite", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Помилка при спробі редагувати ваш акаунт.
        /// </summary>
        internal static string RefreshDetailsError {
            get {
                return ResourceManager.GetString("RefreshDetailsError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Деталі про ваш аккаунт успішно змінено.
        /// </summary>
        internal static string RefreshDetailsSuccess {
            get {
                return ResourceManager.GetString("RefreshDetailsSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Користувача не знайдено або виникла помилка.
        /// </summary>
        internal static string UserNotFoundError {
            get {
                return ResourceManager.GetString("UserNotFoundError", resourceCulture);
            }
        }
    }
}
