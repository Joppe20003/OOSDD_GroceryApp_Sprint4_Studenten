using Grocery.Core.Helpers;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using Grocery.Core.Services;
using Moq;
using NUnit.Framework;
using System.Diagnostics;

namespace TestCore
{
    public class TestHelpers
    {
        private readonly Mock<IClientRepository> _clientRepository = new();
        private readonly Mock<IGroceryListItemsRepository> _grocyListItemsRepository = new();
        private readonly Mock<IGroceryListRepository> _grocyListRepository = new();
        private readonly Mock<IProductRepository> _productRepository = new();

        [SetUp]
        public void Setup()
        {
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
      
        [Test]
        public void TestOverviewProductInGrocyListReturnsTrue()
        {
            List<Product> products = new List<Product>
            {
                new Product(1, "Melk", 300),
                new Product(2, "Kaas", 100),
                new Product(3, "Brood", 400),
                new Product(4, "Cornflakes", 0),
                new Product(5, "Eieren", 200)
            };

            List<GroceryListItem> groceryListItems = new List<GroceryListItem>
            {
                new GroceryListItem(1, 1, 1, 3),
                new GroceryListItem(2, 1, 2, 1),
                new GroceryListItem(3, 1, 3, 4),
                new GroceryListItem(4, 2, 1, 2),
                new GroceryListItem(5, 2, 5, 5)
            };

            List<GroceryList> groceryLists = new List<GroceryList>
            {
                new GroceryList(1, "List1", DateOnly.Parse("2024-10-01"), "#FF0000", 1),
                new GroceryList(2, "List2", DateOnly.Parse("2024-10-02"), "#00FF00", 2)
            };

            List<Client> clients = new List<Client>
            {
                new Client(1, "M.J. Curie", "user1@mail.com", "hash1"),
                new Client(2, "H.H. Hermans", "user2@mail.com", "hash2")
            };

            _grocyListRepository.Setup(x => x.GetAll()).Returns(groceryLists);
            _grocyListItemsRepository.Setup(x => x.GetAllOnGroceryListId(It.IsAny<int>())).Returns<int>(id => groceryListItems.Where(i => i.GroceryListId == id).ToList());
            _clientRepository.Setup(x => x.Get(It.IsAny<int>())).Returns<int>(id => clients.FirstOrDefault(c => c.Id == id));
            _productRepository.Setup(x => x.Get(It.IsAny<int>())).Returns<int>(id => products.FirstOrDefault(p => p.Id == id));

            var service = new BoughtProductsService(
                _grocyListItemsRepository.Object,
                _grocyListRepository.Object,
                _clientRepository.Object,
                _productRepository.Object
            );

            var result = service.Get(1);

            Assert.IsTrue(result.Count > 0);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void TestOverviewNoProductInGrocyListReturnsTrue()
        {
            List<Product> products = new List<Product>
            {
                new Product(1, "Melk", 300),
                new Product(2, "Kaas", 100),
                new Product(3, "Brood", 400),
                new Product(4, "Cornflakes", 0),
                new Product(5, "Eieren", 200)
            };

            List<GroceryListItem> groceryListItems = new List<GroceryListItem>
            {
                new GroceryListItem(1, 1, 1, 3),
                new GroceryListItem(2, 1, 2, 1),
                new GroceryListItem(3, 1, 3, 4),
                new GroceryListItem(4, 2, 1, 2),
                new GroceryListItem(5, 2, 5, 5)
            };

            List<GroceryList> groceryLists = new List<GroceryList>
            {
                new GroceryList(1, "List1", DateOnly.Parse("2024-10-01"), "#FF0000", 1),
                new GroceryList(2, "List2", DateOnly.Parse("2024-10-02"), "#00FF00", 2)
            };

            List<Client> clients = new List<Client>
            {
                new Client(1, "M.J. Curie", "user1@mail.com", "hash1"),
                new Client(2, "H.H. Hermans", "user2@mail.com", "hash2")
            };

            _grocyListRepository.Setup(x => x.GetAll()).Returns(groceryLists);
            _grocyListItemsRepository.Setup(x => x.GetAllOnGroceryListId(It.IsAny<int>())).Returns<int>(id => groceryListItems.Where(i => i.GroceryListId == id).ToList());
            _clientRepository.Setup(x => x.Get(It.IsAny<int>())).Returns<int>(id => clients.FirstOrDefault(c => c.Id == id));
            _productRepository.Setup(x => x.Get(It.IsAny<int>())).Returns<int>(id => products.FirstOrDefault(p => p.Id == id));

            var service = new BoughtProductsService(
                _grocyListItemsRepository.Object,
                _grocyListRepository.Object,
                _clientRepository.Object,
                _productRepository.Object
            );

            var result = service.Get(4);

            Assert.IsTrue(result.Count == 0);
        }
    }
}