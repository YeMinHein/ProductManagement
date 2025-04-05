using Dapper;
using ProductManagement.Models;
using System.Data;

namespace ProductManagement.Data
{
    public class ProductRepository
    {
        private readonly DapperContext _context;

        public ProductRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProducts(int page, int pageSize)
        {
            var query = @"SELECT * FROM Products 
                 ORDER BY CreatedDate DESC 
                 OFFSET @Offset ROWS 
                 FETCH NEXT @PageSize ROWS ONLY";

            var parameters = new
            {
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            };

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Product>(query, parameters);
            }
        }

        public async Task<int> GetTotalCount()
        {
            var query = "SELECT COUNT(*) FROM Products";

            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteScalarAsync<int>(query);
            }
        }

        public async Task<Product> GetProductById(int id)
        {
            var query = "SELECT * FROM Products WHERE Id = @Id";

            using (var connection = _context.CreateConnection())
            {
                var product = await connection.QuerySingleOrDefaultAsync<Product>(query, new { id });
                return product;
            }
        }

        public async Task<int> CreateProduct(Product product)
        {
            var query = @"INSERT INTO Products (Name,Description, Price) 
                           VALUES (@Name, @Description, @Price);
                           SELECT CAST(SCOPE_IDENTITY() as int)";

            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, product);
                return id;
            }
        }

        public async Task UpdateProduct(Product product)
        {
            var query = @"UPDATE Products 
                          SET Name = @Name, Description=@Description, Price = @Price
                          WHERE Id = @Id";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, product);
            }
        }

        public async Task DeleteProduct(int id)
        {
            var query = "DELETE FROM Products WHERE Id = @Id";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { id });
            }
        }
    }
}