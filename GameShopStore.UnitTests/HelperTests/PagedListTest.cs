using FluentAssertions;
using GameShopStore.Application.Helpers;
using GameShopStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GameShopStore.UnitTests.HelperTests
{
    //It's not tested as of this moment
    public class PagedListTest : UnitTestBase, IDisposable
    {
        public Task<PagedList<T>> PagedListHelperTest<T>(IQueryable<T> source, int pageNumber, int pageSize)
        {
            return PagedList<T>.CreateAsync(source, pageNumber, pageSize);
        }


        // TO DO: change source of expected from dbContext to concrete Data
        [Theory]
        [InlineData(1, 1)]
        public void Given_IQueryableDataPageNumberPageSize_When_CreateAsync_ThenReturn_PagedList(int pageNumber, int pageSize)
        {
            //Arrange
            var products = _context.Products.Include(l => l.Languages)
                                    .ThenInclude(productLanguage => productLanguage.Language)
                                .Include(p => p.Pictures)
                                .Include(p => p.Category)
                                .Include(p => p.SubCategories)
                                    .ThenInclude(productSubCategory => productSubCategory.SubCategory)
                                .Include(p => p.Requirements);

            var expected = products.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            //Act
            var result = this.PagedListHelperTest<Product>(products, pageNumber, pageSize).Result;

            //Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
