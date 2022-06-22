using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MvcSuperShop.Controllers;
using MvcSuperShop.Data;
using MvcSuperShop.Infrastructure.Context;
using MvcSuperShop.Services;
using MvcSuperShop.ViewModels;
using System.Security.Claims;

namespace MvcSuperShopTests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        private Mock<ICategoryService> _categoryServiceMock;
        private Mock<IProductService> _productServiceMock;
        private Mock<IMapper> _mapperMock;
        private ApplicationDbContext _context;
        private HomeController _sut;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestsDb")
                .Options;

            _categoryServiceMock = new Mock<ICategoryService>();
            _productServiceMock = new Mock<IProductService>();
            _mapperMock = new Mock<IMapper>();
            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();

            _sut = new HomeController(_categoryServiceMock.Object, _productServiceMock.Object, _mapperMock.Object, _context);
        }

        [TestMethod]
        public void Check_If_Index_Shows_Three_Categories()
        {
            //arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, "hej@hej.se"),
            }, "TestAuthentication"));

            _sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user,
                }
            };
            _categoryServiceMock.Setup(category => category.GetTrendingCategories(3)).Returns(new List<Category> { new Category(), new Category(), new Category() });
            _mapperMock.Setup(mapper => mapper.Map<List<CategoryViewModel>>(It.IsAny<List<Category>>())).Returns(new List<CategoryViewModel> { new CategoryViewModel(), new CategoryViewModel(), new CategoryViewModel() });
            //act
            var result = _sut.Index() as ViewResult;
            var model = result.Model as HomeIndexViewModel;

            //assert
            Assert.AreEqual(3, model.TrendingCategories.Count());
        }

        [TestMethod]
        public void Check_If_Index_Shows_Three_Products()
        {
            //arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, "hej@hej.se"),
            }, "TestAuthentication"));

            _sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user,
                }
            };
            var customer = new CurrentCustomerContext { Agreements = new List<Agreement>() };
            _productServiceMock.Setup(product => product.GetNewProducts(It.IsAny<int>(), customer)).Returns(new List<ProductServiceModel> {new ProductServiceModel(), new ProductServiceModel(), new ProductServiceModel() });
            _mapperMock.Setup(mapper => mapper.Map<List<ProductBoxViewModel>>(It.IsAny<IEnumerable<ProductServiceModel>>())).Returns(new List<ProductBoxViewModel> { new ProductBoxViewModel(), new ProductBoxViewModel(), new ProductBoxViewModel() });
            //act
            var result = _sut.Index() as ViewResult;
            var model = result.Model as HomeIndexViewModel;

            //assert
            Assert.AreEqual(3, model.NewProducts.Count());
        }
    }
}
