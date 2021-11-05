using System;
using System.ComponentModel.DataAnnotations;

namespace ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property|AttributeTargets.Field|AttributeTargets.Parameter,
        AllowMultiple =false)]
    public class PasswordAttribute : RegularExpressionAttribute
    {
        public PasswordAttribute() : base("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$")
        {
            ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)";
        }
    }
}
