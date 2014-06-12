using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM.IService;
using DM.ViewModel;
using DM.IRepository;
using DM.Repository;
using DM.Infrastructure.UnitOfWork;
using DM.ViewModel.Mappings;

namespace DM.Service
{
    public class ProductService : IProductService
    {
        private IProductRepository productRepository;
        private UnitOfWork unitOfWork;

        public ProductService()
        {
            unitOfWork = new UnitOfWork();
            productRepository = new ProductRepository(unitOfWork);
        }

        public ProductViewModel GetProduct(string productID)
        {
            DomainModel.Product product= productRepository.FindById(productID);
            ProductViewModel productViewModel = ProductMapper.ToViewModel(product);
            return productViewModel;
        }
    }
}
