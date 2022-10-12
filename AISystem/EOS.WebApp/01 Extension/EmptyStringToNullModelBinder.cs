using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;

namespace EOS.WebApp
{
    public class EmptyStringToNullModelBinder : DefaultModelBinder
    {
        protected override void SetProperty(ControllerContext controllerContext,
            ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
        {
            if (value == null && propertyDescriptor.PropertyType == typeof(string))
            {
                value = string.Empty;
            }

            base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
        }
    }
}