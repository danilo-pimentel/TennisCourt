using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace TennisCourt.Api.Swagger
{
    public class ActionHidingConvention : IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            if (action.Controller.ControllerName.ToLower().Equals("metadata"))
            {
                action.ApiExplorer.IsVisible = false;
            }
        }
    }
}
