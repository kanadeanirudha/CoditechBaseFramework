using AutoMapper;

using Coditech.Utilities.Filters;

namespace Coditech
{
    public static class AutoMapperConfig
    {
        public static void Execute()
        {
            Mapper.CreateMap<FilterTuple, FilterDataTuple>();
        }
    }
}

