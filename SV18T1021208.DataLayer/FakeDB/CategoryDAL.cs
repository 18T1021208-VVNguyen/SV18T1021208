using SV18T1021208.DomainModel;
using System;
using System.Collections.Generic;


namespace SV18T1021208.DataLayer.FakeDB
{
    public class CategoryDAL : ICommonDAL<Category>
    {
        public int Add(Category data)
        {
            throw new NotImplementedException();
        }

        public int Count(string seachValue)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int categoryID)
        {
            throw new NotImplementedException();
        }

        public Category Get(int categoryID)
        {
            throw new NotImplementedException();
        }

        public bool InUsed(int categoryID)
        {
            throw new NotImplementedException();
        }

        public IList<Category> List(int page, int pageSize, string seachValue)
        {
            List<Category> data = new List<Category>();
            data.Add(new Category()
            {
                CategoryID = 1,
                CategoryName = "Mỹ phẩm",
                Description = "hàng dành cho chị em"

            });
            data.Add(new Category()
            {
                CategoryID = 2,
                CategoryName = "Thực phẩm",
                Description = "Hàng cứu đói"
            });
            return data;
        }

        public bool Update(Category data)
        {
            throw new NotImplementedException();
        }
    }
}
