using AutoMapper;

using Coditech.Utilities.Filters;

namespace RARIndia
{
    public static class AutoMapperConfig
    {
        public static void Execute()
        {
            Mapper.CreateMap<FilterTuple, FilterDataTuple>();
        }
    }
}

