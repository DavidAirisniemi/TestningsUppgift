using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcSuperShop.Data;
using MvcSuperShop.Infrastructure.Context;
using MvcSuperShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcSuperShopTests.Services
{
    [TestClass]
    public class PricingServiceTest
    {
        public PricingService _sut { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            _sut = new PricingService();
        }

        [TestMethod]
        public void Check_If_Calculate_Prices_Works()
        {
            //arrange
            var products = new List<ProductServiceModel> { new ProductServiceModel { BasePrice = 100 } };
            var customer = new CurrentCustomerContext { Agreements = new List<Agreement>() }; 

            //act
            var result = _sut.CalculatePrices(products, customer);

            //assert
            Assert.AreEqual(100, result.First().Price);
        }

        [TestMethod]
        public void Check_If_Agreement_Changes_Price()
        {
            //arrange
            var products = new List<ProductServiceModel> { new ProductServiceModel { BasePrice = 100, Name = "Ford Hybrid" }  };
            var customer = new CurrentCustomerContext
            {
                Agreements = new List<Agreement> {
                new Agreement { AgreementRows = new List<AgreementRow>
                {
                    new AgreementRow
                    {
                        PercentageDiscount = 50,
                        ProductMatch = "hybrid"
                    }
                } } }
            };

            //act
            var result = _sut.CalculatePrices(products, customer);

            //assert
            Assert.AreEqual(50, result.First().Price);
        }

        [TestMethod]
        public void Check_If_Category_Changes_Price()
        {
            //arrange
            var products = new List<ProductServiceModel> { new ProductServiceModel { BasePrice = 100, Name = "", CategoryName = "Minivan" } };
            var customer = new CurrentCustomerContext
            {
                Agreements = new List<Agreement> {
                new Agreement { AgreementRows = new List<AgreementRow>
                {
                    new AgreementRow
                    {
                        PercentageDiscount = 50,
                        CategoryMatch = "van",

                    }
                } } }
            };

            //act
            var result = _sut.CalculatePrices(products, customer);

            //assert
            Assert.AreEqual(50, result.First().Price);
        }

        [TestMethod]
        public void Check_If_Manufacturer_Changes_Price()
        {
            //arrange
            var products = new List<ProductServiceModel> { new ProductServiceModel { BasePrice = 100, Name = "", ManufacturerName = "Audi" } };
            var customer = new CurrentCustomerContext
            {
                Agreements = new List<Agreement> {
                new Agreement { AgreementRows = new List<AgreementRow>
                {
                    new AgreementRow
                    {
                        PercentageDiscount = 50,
                        ManufacturerMatch = "Audi",

                    }
                } } }
            };

            //act
            var result = _sut.CalculatePrices(products, customer);

            //assert
            Assert.AreEqual(50, result.First().Price);
        }
    }
}
