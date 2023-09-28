using ProductsBusiness.Dto;
using ProductsBusiness.Filters;
using ProductsDao.Models;

namespace ProductsBusiness.Profiles;

public static class FilterMapper
{
    public static ProductFilterChain AsProductFilterChain(this FiltersDto filtersDto)
    {
        var filters = new List<ProductFilter>();

        if (filtersDto.ProductWarehouseIdFilter is not null)
        {
            var warehouseId = new WarehouseId(filtersDto.ProductWarehouseIdFilter.Value);
            filters.Add(new ProductWarehouseIdFilter(warehouseId));
        }
        if (filtersDto.ProductCategoryFilter is not null)
        {
            var productCategory = Enum.Parse<ProductCategory>(filtersDto.ProductCategoryFilter);
            filters.Add(new ProductCategoryFilter(productCategory));
        }
        if (filtersDto.ProductManufactureDateFilter is not null)
        {
            filters.Add(new ProductManufactureDateFilter(filtersDto.ProductManufactureDateFilter.Value));
        }

        return new ProductFilterChain(filters);
    }
}