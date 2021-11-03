using System.Collections.Generic;
using pdouelle.LinqExtensions.Interfaces;
using pdouelle.QueryStringParameters;

namespace pdouelleBlueprints.ControllerBase.Domain.ChildEntities.Models.Queries.GetChildEntityList
{
    public class GetChildEntityListQueryModel : QueryStringPaginationSort, IInclude
    {
        public List<string> Include { get; set; }
    }
}