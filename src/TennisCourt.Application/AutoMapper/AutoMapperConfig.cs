using AutoMapper;
using AutoMapper.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TennisCourt.Application.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AllowNullDestinationValues = true;
                cfg.AllowNullCollections = true;

                cfg.DisableConstructorMapping();

                cfg.Internal().ForAllMaps
                (
                    (mapType, mapperExpression) =>
                    {
                        mapperExpression.MaxDepth(1);
                    }
                );

                cfg.AddProfile(new DtoToDomainMappingProfile());
                cfg.AddProfile(new ObjectToDtoMappingProfile());
            });
        }

        public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> map, Expression<Func<TDestination, object>> selector)
        {
            //https://stackoverflow.com/questions/4987872/ignore-mapping-one-property-with-automapper
            map.ForMember(selector, config => config.Ignore());
            return map;
        }
    }
}
