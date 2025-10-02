using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class GroceryListItemsService : IGroceryListItemsService
    {
        private readonly IGroceryListItemsRepository _groceriesRepository;
        private readonly IProductRepository _productRepository;

        public GroceryListItemsService(IGroceryListItemsRepository groceriesRepository, IProductRepository productRepository)
        {
            _groceriesRepository = groceriesRepository;
            _productRepository = productRepository;
        }

        public List<GroceryListItem> GetAll()
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public List<GroceryListItem> GetAllOnGroceryListId(int groceryListId)
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll().Where(g => g.GroceryListId == groceryListId).ToList();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public GroceryListItem Add(GroceryListItem item)
        {
            return _groceriesRepository.Add(item);
        }

        public GroceryListItem? Delete(GroceryListItem item)
        {
            throw new NotImplementedException();
        }

        public GroceryListItem? Get(int id)
        {
            return _groceriesRepository.Get(id);
        }

        public GroceryListItem? Update(GroceryListItem item)
        {
            return _groceriesRepository.Update(item);
        }

        public List<BestSellingProducts> GetBestSellingProducts(int topX = 5)
        {
            var producten = _groceriesRepository.GetAll();

            if (producten == null || !producten.Any())
                return new List<BestSellingProducts>();

            var bestSellingProducts = producten
                .GroupBy(p => p.ProductId)
                .Select(g =>
                {
                    Product product = _productRepository.Get(g.Key);
                    int numberOfSells = g.Sum(x => x.Amount);

                    return new BestSellingProducts(
                        product.Id,
                        product.Name,
                        product.Stock,
                        numberOfSells,
                        0
                    );
                })
                .Take(topX)
                .OrderByDescending(g => g.NrOfSells)
                .ToList();

            for (int i = 0; i < bestSellingProducts.Count; i++)
            {
                bestSellingProducts[i].ranking = i + 1;
            }

            //bestSellingProducts.OrderByDescending(x => x.NrOfSells);

            return bestSellingProducts;
        }

        private void FillService(List<GroceryListItem> groceryListItems)
        {
            foreach (GroceryListItem g in groceryListItems)
            {
                g.Product = _productRepository.Get(g.ProductId) ?? new(0, "", 0);
            }
        }
    }
}
