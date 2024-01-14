using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Casino.Tests.Helpers
{
    // A custom object model validator for use in unit tests
    public class ObjectValidator : IObjectModelValidator
    {
        // Flag to determine whether to bypass validation
        private readonly bool _bypassValidation;

        // Constructor to set the bypass validation flag
        public ObjectValidator(bool bypassValidation)
        {
            _bypassValidation = bypassValidation;
        }

        // Method to validate an object
        public void Validate(ActionContext actionContext, ValidationStateDictionary validationState, string prefix,
            object model)
        {
            // Check if validation should not be bypassed
            if (!_bypassValidation)
            {
                // Create a validation context for the model
                var context = new ValidationContext(model, serviceProvider: null, items: null);
                var results = new List<ValidationResult>();

                // Attempt to validate the model
                bool isValid = Validator.TryValidateObject(
                    model, context, results,
                    validateAllProperties: true // Flag to validate all properties
                );

                // If the model is not valid, add each validation result to the action context's model state
                if (!isValid)
                {
                    results.ForEach((r) =>
                    {
                        foreach (var memberName in r.MemberNames)
                            actionContext.ModelState.AddModelError(memberName, r.ErrorMessage);
                    });
                }
            }
            // If validation is bypassed, no validation logic is performed
        }
    }
}