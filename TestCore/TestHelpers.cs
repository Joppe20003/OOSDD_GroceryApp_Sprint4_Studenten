using Grocery.Core.Helpers;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using Grocery.Core.Services;
using Moq;
using NUnit.Framework;
using System.Diagnostics;

namespace TestCore
{
    public class TestHelpers
    {
        private readonly Mock<IGroceryListItemsRepository> _grocyListItemsRepository = new();
        private readonly Mock<IProductRepository> _productsRepository = new();


        [SetUp]
        public void Setup()
        {
        }


        //Happy flow
        [Test]
        public void TestPasswordHelperReturnsTrue()
        {
            string password = "user3";
            string passwordHash = "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=";
            Assert.IsTrue(PasswordHelper.VerifyPassword(password, passwordHash));
        }

        [TestCase("user1", "IunRhDKa+fWo8+4/Qfj7Pg==.kDxZnUQHCZun6gLIE6d9oeULLRIuRmxmH2QKJv2IM08=")]
        [TestCase("user3", "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=")]
        public void TestPasswordHelperReturnsTrue(string password, string passwordHash)
        {
            Assert.IsTrue(PasswordHelper.VerifyPassword(password, passwordHash));
        }


        //Unhappy flow
        [Test]
        public void TestPasswordHelperReturnsFalse()
        {
            string password = "wrongpassword";
            string passwordHash = "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=";



            Assert.IsFalse(PasswordHelper.VerifyPassword(password, passwordHash)); //Zelf uitwerken
        }

        [TestCase("user1", "IunRhDKa+fWo8+4/Qfj7Pg==.kDxZnUQHCZun6gLIE6d9oeULLRIuRmxmH2QKJv2IM08")]
        [TestCase("user3", "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA")]
        public void TestPasswordHelperReturnsFalse(string password, string passwordHash)
        {
            Assert.IsFalse(PasswordHelper.VerifyPassword(password, password)); //Zelf uitwerken zodat de test slaagt!
        }

        [Test]
        public void TestGetFiveBestProductsSellsReturnsTrue()
        {
            List<Product> products = new List<Product>
            {
                new Product(1, "Melk", 300),
                new Product(2, "Kaas", 100),
                new Product(3, "Brood", 400),
                new Product(4, "Cornflakes", 0),
                new Product(5, "Eieren", 200),
                new Product(6, "Boter", 50),
                new Product(7, "Appels", 150),
                new Product(8, "Sinaasappelsap", 75)
            };

            List<GroceryListItem> groceryListItems = new List<GroceryListItem>
            {
                new GroceryListItem(1, 1, 1, 3),  
                new GroceryListItem(2, 1, 2, 1),  
                new GroceryListItem(3, 1, 3, 4),  
                new GroceryListItem(4, 2, 1, 2),  
                new GroceryListItem(5, 2, 2, 5),  
                new GroceryListItem(6, 2, 4, 2),  
                new GroceryListItem(7, 3, 5, 7),  
                new GroceryListItem(8, 3, 6, 1),  
                new GroceryListItem(9, 4, 7, 3),  
                new GroceryListItem(10, 4, 8, 4)  
            };

            _productsRepository.Setup(x => x.GetAll()).Returns(products);
            _grocyListItemsRepository.Setup(x => x.GetAll()).Returns(groceryListItems);

            foreach (var product in products)
            {
                _productsRepository.Setup(x => x.Get(product.Id)).Returns(product);
            }

            var uc = new GroceryListItemsService(_grocyListItemsRepository.Object, _productsRepository.Object);
            var result = uc.GetBestSellingProducts();

            Assert.IsTrue(result.Count == 5);

        }

        [Test]
        public void TestGetDetailFiveBestProductSellsReturnsTrue()
        {
            List<Product> products = new List<Product>
            {
                new Product(1, "Melk", 300),
                new Product(2, "Kaas", 100),
                new Product(3, "Brood", 400),
                new Product(4, "Cornflakes", 0)
            };

            List<GroceryListItem> groceryListItems = new List<GroceryListItem>
            {
                new GroceryListItem(1, 1, 1, 3),
                new GroceryListItem(2, 1, 2, 1),
                new GroceryListItem(3, 1, 3, 4),
                new GroceryListItem(4, 2, 1, 2),
                new GroceryListItem(5, 2, 2, 5),
            };

            _productsRepository.Setup(x => x.GetAll()).Returns(products);
            _grocyListItemsRepository.Setup(x => x.GetAll()).Returns(groceryListItems);

            foreach (var product in products)
            {
                _productsRepository.Setup(x => x.Get(product.Id)).Returns(product);
            }

            var uc = new GroceryListItemsService(_grocyListItemsRepository.Object, _productsRepository.Object);
            var result = uc.GetBestSellingProducts();

            Assert.AreEqual(result[0].Ranking, 1);
        }

        [Test]
        public void TestBestSellingProductsReturnsCorrectTopProducts()
        {
            List<Product> products = new List<Product>
            {
                new Product(1, "Melk", 300),
                new Product(2, "Kaas", 100),
                new Product(3, "Brood", 400)
            };

            List<GroceryListItem> groceryListItems = new List<GroceryListItem>
            {
                new GroceryListItem(1, 1, 1, 3),
                new GroceryListItem(2, 1, 2, 1), 
                new GroceryListItem(3, 1, 3, 4), 
                new GroceryListItem(4, 2, 1, 2), 
                new GroceryListItem(5, 2, 2, 5)  
            };

            _productsRepository.Setup(x => x.GetAll()).Returns(products);
            _grocyListItemsRepository.Setup(x => x.GetAll()).Returns(groceryListItems);

            foreach (var product in products)
            {
                _productsRepository.Setup(x => x.Get(product.Id)).Returns(product);
            }

            var service = new GroceryListItemsService(_grocyListItemsRepository.Object, _productsRepository.Object);
            var result = service.GetBestSellingProducts();

            Assert.AreEqual("Kaas", result[0].Name);
            Assert.AreEqual(6, result[0].NrOfSells);

            Assert.AreEqual("Melk", result[1].Name);
            Assert.AreEqual(5, result[1].NrOfSells);

            Assert.AreEqual("Brood", result[2].Name);
            Assert.AreEqual(4, result[2].NrOfSells);
        }
    }
}
