using Lab_12_OOP;
using Lab_10_lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Lab_14_OOP
{
    public static class ExtensionMethod
    {
        public static int CountProduct(this HashTable<Product> hashTable, Func<Product, bool> selectRule)
        {
            int count = hashTable.Where(selectRule).Count();
            return count;
        }

        public static List<MilkProduct> SortBy(this HashTable<MilkProduct> hashTable, Func<MilkProduct, double> selectRule)
        {
            var productList = new List<MilkProduct>();
            foreach (var item in hashTable)
                productList.Add(item);

            for (int i = 0; i < productList.Count - 1; i++)
            {
                for (int j = 0; j < productList.Count - 1 - i; j++)
                {
                    if (selectRule(productList[j]) > selectRule(productList[j + 1]))
                    {
                        var temp = productList[j];
                        productList[j] = productList[j + 1];
                        productList[j + 1] = temp;
                    }
                }
            }
            return productList;

        }

        public static IEnumerable<MilkProduct> SelectProduct(this HashTable<MilkProduct> hashTable, Func<MilkProduct, bool> selectRule)
        {
            var selection = hashTable.Where(selectRule);

            return selection;
        }

        public static double AvrageBy(this HashTable<MilkProduct> hashTable, Func<MilkProduct, double> selectRule)
        {
            if (hashTable == null)
            {
                throw new ArgumentNullException(nameof(hashTable));
            }

            double sum = 0;
            int count = 0;

            foreach (var product in hashTable)
            {
                sum += selectRule(product);
                count++;
            }

            if (count == 0)
            {
                throw new InvalidOperationException("The collection is empty.");
            }

            return sum / count;
        }
    }
}
