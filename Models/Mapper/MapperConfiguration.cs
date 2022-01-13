using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KSCIRC.Models.Mapper
{
    //helper class for startup mapping configuration
    public class MapperConfiguration
    {
        public IMapper ConfigureAutoMapper(System.IServiceProvider provider)
        {
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                AddMappingProfiles(cfg);
                cfg.ConstructServicesUsing(type => ActivatorUtilities.GetServiceOrCreateInstance(provider, type)); //set up DI so any services declared here can be injected into the mapper
            });

            var mapper = config.CreateMapper();

            //have automapper test the validation, to early detect any mapping issues. this should remove the need for a lot of testing, although makes the mapping a little more tedious http://automapper.readthedocs.io/en/latest/Configuration-validation.html
            config.AssertConfigurationIsValid();
            return mapper;
        }

        private void AddMappingProfiles(IMapperConfigurationExpression cfg)
        {
            //locate the default mapping profile and add any other profiles in the same project (TracDispatch.Maps)
            Assembly[] assemblies = { typeof(GeneMappingProfile).GetTypeInfo().Assembly };
            AddMapperProfiles(cfg, assemblies);
        }

        private void AddMapperProfiles(IMapperConfigurationExpression cfg, IEnumerable<Assembly> assembliesToScan)
        {
            assembliesToScan = assembliesToScan as Assembly[] ?? assembliesToScan.ToArray();

            var allTypes = assembliesToScan.SelectMany(a => a.ExportedTypes).ToArray();

            var profiles = allTypes
                .Where(t => typeof(Profile).GetTypeInfo().IsAssignableFrom(t.GetTypeInfo()))
                .Where(t => !t.GetTypeInfo().IsAbstract);

            foreach (var profile in profiles)
            {
                cfg.AddProfile(profile);
            }
        }
    }
}
