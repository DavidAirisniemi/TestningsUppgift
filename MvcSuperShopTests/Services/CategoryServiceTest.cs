using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcSuperShop.Data;
using MvcSuperShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcSuperShopTests.Services
{
    [TestClass]
    public class CategoryServiceTest
    {
        private ApplicationDbContext _context;
        private CategoryService _sut;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestsDb")
                .Options;

            _context = new ApplicationDbContext(options);
            _sut = new CategoryService(_context);
            SeedData();
        }
        public void SeedData()
        {
            _context.Categories.AddRange(
                new List<Category>
                {
                new Category {Icon = "h", Name = "1"},
                new Category {Icon = "j", Name = "2"},
                new Category {Icon = "k", Name = "3"},
                new Category {Icon = "l", Name = "4"},
                });
            _context.SaveChanges();
        }

        [TestMethod]
        public void Check_If_Correct_Amount_Of_Categories_Are_Sent()
        {
            //act
            var result = _sut.GetTrendingCategories(3).ToList();

            //assert
            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public void Check_If_Incorrect_Amount_Of_Categories_Are_Sent()
        {
            //act
            var result = _sut.GetTrendingCategories(-3).ToList();

            //assert
            Assert.AreEqual(0, result.Count());
        }
    }
}
