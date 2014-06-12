using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using DM.DomainModel;
using StackExchange.Profiling;
using EmitMapper;

namespace DM.ViewModel.Mappings
{
    public static class ProductMapper
    {
        public static ProductViewModel ToViewModel(this Product product)
        {
            //AutoMapper,EmitMapper性能比拼，EmitMapper比AutoMapper快十倍
            /*
            using (MiniProfiler.Current.Step("AutoMapper()"))
            {
                for (int i = 0; i < 10000; i++)
                {
                    Mapper.DynamicMap<Product, ProductViewModel>(product);
                }
            }
            using (MiniProfiler.Current.Step("EmitMapper()"))
            {
                for (int i = 0; i < 10000; i++)
                {
                    ObjectsMapper<Product, ProductViewModel> mapper = ObjectMapperManager.DefaultInstance.GetMapper<Product, ProductViewModel>();
                    ProductViewModel dst = mapper.Map(product);
                }
            }
            */

            ObjectsMapper<Product, ProductViewModel> mapper = ObjectMapperManager.DefaultInstance.GetMapper<Product, ProductViewModel>();
            return mapper.Map(product);
        }

    }
}
