using Newtonsoft.Json;

namespace pdouelle.Blueprints.ControllerBase.Errors
{
    public class ErrorObject
    {
        public string Title { get; set; }
        public string Detail { get; set; }
            
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}