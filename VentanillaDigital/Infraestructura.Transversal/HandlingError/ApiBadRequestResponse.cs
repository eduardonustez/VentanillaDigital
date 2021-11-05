using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infraestructura.Transversal.HandlingError
{
   public class ApiBadRequestResponse 
    {
        public IEnumerable<string> Errors { get; }

        public ApiBadRequestResponse(ModelStateDictionary modelState)
         
        {
            if (modelState.IsValid)
            {
                throw new ArgumentException("ModelState must be invalid", nameof(modelState));
            }

            Errors = modelState.SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage).ToArray();
        }
     
        public ApiBadRequestResponse(string message)
        {
            List<string> lsErrors = new List<string>();
            lsErrors.Add(message);
            Errors =lsErrors.ToArray();
        }
    }
}
