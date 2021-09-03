using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace zModelLayer.Attributes
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        //private readonly string ErrorMessage;
        private readonly string[] _extensions;
        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            List<IFormFile> files = value as List<IFormFile>;
            if(files.Count() == 0 || files == null)
            {
                return new ValidationResult($"upload file please");
            }
            bool isExtensionOK = files.Select(g => Path.GetExtension(g.FileName).ToLower()).Any(x => _extensions.Contains(x));
            if (isExtensionOK)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult($"{files.FirstOrDefault(g => !_extensions.Contains(Path.GetExtension(g.FileName).ToLower())).FileName} extension is not allowed!");

            }

        }

    }
}
