using AutoMapper;

using Coditech.DataAccessLayer.DataEntity;
using Coditech.Model;
using Coditech.Utilities.Filters;
using Coditech.ViewModel;

namespace Coditech
{
    public static class AutoMapperConfig
    {
        public static void Execute()
        {
            Mapper.CreateMap<FilterTuple, FilterDataTuple>();
            Mapper.CreateMap<UserMasterModel, UserLoginViewModel>().ReverseMap();
            Mapper.CreateMap<UserMasterModel, UserMaster>().ReverseMap();
            Mapper.CreateMap<UserMasterModel, UserMasterViewModel>().ReverseMap();
            Mapper.CreateMap<UserMasterViewModel, UserMasterModel>().ReverseMap();


            Mapper.CreateMap<ProductMasterModel, ProductMasterViewModel>().ReverseMap();
            Mapper.CreateMap<ProductMasterModel, ProductMaster>().ReverseMap();
        }
    }
}

