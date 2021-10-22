using Microsoft.AspNetCore.Mvc;

namespace pdouelle.Blueprints.ControllerBase.ModelValidations
{
    public class ModelState
    {
        public  IActionResult Error { get; set; }

        public ModelState()
        {
            
        }
        
        public ModelState(IActionResult error)
        {
            Error = error;
        }
        
        public bool HasError()
        {
            if (Error is not null)
                return true;

            return false;
        }
    }
}