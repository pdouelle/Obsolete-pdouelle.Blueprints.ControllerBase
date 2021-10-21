using AutoMapper;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.ChildEntities.Entities;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.ChildEntities.Models;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.ChildEntities.Models.Commands.CreateChildEntity;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.ChildEntities.Models.Commands.PatchChildEntity;

namespace pdouelle.Blueprints.ControllerBase.Debug.Domain.ChildEntities.Profiles
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