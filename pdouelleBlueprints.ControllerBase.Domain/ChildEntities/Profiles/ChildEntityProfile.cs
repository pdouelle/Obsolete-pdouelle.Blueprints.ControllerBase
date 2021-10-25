using AutoMapper;
using pdouelleBlueprints.ControllerBase.Domain.ChildEntities.Entities;
using pdouelleBlueprints.ControllerBase.Domain.ChildEntities.Models;
using pdouelleBlueprints.ControllerBase.Domain.ChildEntities.Models.Commands.CreateChildEntity;
using pdouelleBlueprints.ControllerBase.Domain.ChildEntities.Models.Commands.PatchChildEntity;

namespace pdouelleBlueprints.ControllerBase.Domain.ChildEntities.Profiles
{
    public class ChildEntityProfile : Profile
    {
        public ChildEntityProfile()
        {
            CreateMap<ChildEntity, ChildEntityDto>();
            CreateMap<CreateChildEntityCommandModel, ChildEntity>();
            CreateMap<ChildEntity, PatchChildEntityCommandModel>().ReverseMap();
        }
    }
}