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