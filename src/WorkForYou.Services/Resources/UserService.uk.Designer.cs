﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WorkForYou.Services.Resources {
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
    internal class UserService_uk {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal UserService_uk() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WorkForYou.Services.Resources.UserService.uk", typeof(UserService_uk).Assembly);
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
        ///   Looks up a localized string similar to Кандидат у списку.
        /// </summary>
        internal static string CandidateInList {
            get {
                return ResourceManager.GetString("CandidateInList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Кандидата успішно додано до списку.
        /// </summary>
        internal static string CandidateSuccessAddToList {
            get {
                return ResourceManager.GetString("CandidateSuccessAddToList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Кандидата успішно видалено зі списку.
        /// </summary>
        internal static string CandidateSuccessRemoveFromList {
            get {
                return ResourceManager.GetString("CandidateSuccessRemoveFromList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Під час отримання завдання або користувача сталася помилка.
        /// </summary>
        internal static string ErrorCandidateInList {
            get {
                return ResourceManager.GetString("ErrorCandidateInList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Помилка отримання користувача.
        /// </summary>
        internal static string ErrorGettingUser {
            get {
                return ResourceManager.GetString("ErrorGettingUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Помилка отримання користувача або користувач не є кандидатом.
        /// </summary>
        internal static string ErrorGettingUserOrUserNotCandidate {
            get {
                return ResourceManager.GetString("ErrorGettingUserOrUserNotCandidate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Помилка отримання користувача або користувач не є роботодавцем.
        /// </summary>
        internal static string ErrorGettingUserOrUserNotEmployer {
            get {
                return ResourceManager.GetString("ErrorGettingUserOrUserNotEmployer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to У списку немає жодного кандидата.
        /// </summary>
        internal static string NoCandidateInList {
            get {
                return ResourceManager.GetString("NoCandidateInList", resourceCulture);
            }
        }
    }
}