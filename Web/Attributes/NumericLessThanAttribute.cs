using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Web.Attributes
{
    public class NumericLessThanAttribute : ValidationAttribute, IClientValidatable
    {
        private const string LessThanErrorMessage = "{0} must be less than {1}.";
        private const string LessThanOrEqualToErrorMessage = "{0} має бути меньше ніж {1}.";

        public string OtherProperty { get; private set; }

        public string DisplayProperty { get; set; }

        private bool _allowEquality;

        public bool AllowEquality
        {
            get { return this._allowEquality; }
            set
            {
                this._allowEquality = value;

                // Set the error message based on whether or not
                // equality is allowed
                if(string.IsNullOrEmpty(ErrorMessage)) ErrorMessage = (value ? LessThanOrEqualToErrorMessage : LessThanErrorMessage);
            }
        }

        public NumericLessThanAttribute(string otherProperty, string errorMsg = "")
            : base(LessThanErrorMessage)
        {
            if (otherProperty == null) { throw new ArgumentNullException("otherProperty"); }
            
            this.OtherProperty = otherProperty;
            if (!string.IsNullOrEmpty(errorMsg)) ErrorMessage = errorMsg;

        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, this.OtherProperty);
        }

        decimal _decValue;
        decimal _decOtherPropertyValue;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);

            if (otherPropertyInfo == null)
            {
                return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "Could not find a property named {0}.", OtherProperty));
            }

            object otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);

         
            // Check to ensure the validating property is numeric
            if (!decimal.TryParse(value.ToString(), out _decValue))
            {
                return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "{0} is not a numeric value.", validationContext.DisplayName));
            }

            // Check to ensure the other property is numeric
            if (!decimal.TryParse(otherPropertyValue.ToString(), out _decOtherPropertyValue))
            {
                return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "{0} is not a numeric value.", OtherProperty));
            }

            // Check for equality
            if (AllowEquality && _decValue == _decOtherPropertyValue)
            {
                return null;
            }
            // Check to see if the value is greater than the other property value
            else if (_decValue > _decOtherPropertyValue)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return null;
        }

        public static string FormatPropertyForClientValidation(string property)
        {
            if (property == null)
            {
                throw new ArgumentException("Value cannot be null or empty.", "property");
            }
            return "*." + property;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationNumericLessThanRule(FormatErrorMessage(metadata.DisplayName), FormatPropertyForClientValidation(this.OtherProperty), this.AllowEquality);
        }
    }
}