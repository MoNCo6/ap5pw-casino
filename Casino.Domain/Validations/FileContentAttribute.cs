using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Casino.Domain.Implementation.Validations
{
    // Custom attribute for validating file content types
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class FileContentAttribute : ValidationAttribute, IClientModelValidator
    {
        // Field to store the required content type
        private readonly string contentType;

        // Constructor to set the required content type
        public FileContentAttribute(string contentType)
        {
            this.contentType = contentType;
        }

        // Override IsValid to validate the file content type
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // If the value is null, return success (use [Required] for mandatory fields)
            if (value == null)
            {
                return ValidationResult.Success;
            }
            // Check if the value is an IFormFile (file upload)
            else if (value is IFormFile formFile)
            {
                // Validate the file's content type
                if (formFile.ContentType.ToLower().Contains(contentType.ToLower()))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    // Return an error if the content type doesn't match
                    return new ValidationResult($"The {validationContext.MemberName} field is not {contentType}.");
                }
            }
            else
            {
                // Throw an exception if the attribute is used on a non-supported type
                throw new NotImplementedException(
                    $"The {nameof(FileContentAttribute)} is not implemented for the type: {value.GetType()}");
            }
        }

        // Add client-side validation rules
        public void AddValidation(ClientModelValidationContext context)
        {
            // Ensure 'data-val' attribute is set for client-side validation
            if (!context.Attributes.ContainsKey("data-val"))
                context.Attributes.Add("data-val", "true");

            // Add custom validation attributes for client-side validation
            context.Attributes.Add("data-val-filecontent",
                $"The {context.ModelMetadata.Name} field is not {contentType}.");
            context.Attributes.Add("data-val-filecontent-type", contentType);
        }
    }
}