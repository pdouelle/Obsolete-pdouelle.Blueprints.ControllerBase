namespace pdouelle.Blueprints.ControllerBase.Errors
{
    public class ErrorObject
    {
        protected string Title { get; set; }
        protected string Detail { get; set; }

        public override string ToString() => $"{Title} {Detail}";
    }
}