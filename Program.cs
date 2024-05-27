using Lab_10_lib;
using Lab_12_OOP;
using System.Collections;
using System.Collections.Generic;

namespace Lab_14_OOP
{
    public class Program
    {
        static void Main(string[] args)
        {
            var queue = new Queue<Dictionary<Goods, MilkProduct>>();

            FillQueue(queue, 3, 2);

            Console.WriteLine("Элементы очереди");
            foreach (var item in queue)
            {
                Console.WriteLine("Элементы словаря");
                foreach (var value in item.Values)
                {
                    value.MilkShow();
                }
            }
            Console.WriteLine();
            Console.WriteLine();

            QueryProbe(queue);
            Console.WriteLine();
            QueryCount(queue);
            Console.WriteLine();
            QueryUnion(queue);
            Console.WriteLine();
            QueryAgregate(queue);
            Console.WriteLine();
            QueryGroup(queue);

            Console.WriteLine();
            Console.WriteLine("Хеш-таблица:");

            HashTable<MilkProduct> products = new(7);
            FillHashTable(products, 7);
            products.Print();
            Console.WriteLine();

            
            MethodQueryCount(products);
            
            MethodQueryAvr(products);
            MethodSelect(products);
            MethodQuerySorting2(products);

        }

        public static void FillQueue(Queue<Dictionary<Goods, MilkProduct>> queue, int sizeQ, int sizeD)
        {
            for (int i = 0; i < sizeQ; i++)
            {
                var goodsMilkDict = new Dictionary<Goods, MilkProduct>();
                for (int j = 0; j < sizeD; j++)
                {
                    var milk = new MilkProduct();
                    milk.RandomInit();
                    var goods = milk.BaseGoods;
                    goodsMilkDict.Add(goods, milk);
                }
                queue.Enqueue(goodsMilkDict);
            }
        }

        public static void FillHashTable(HashTable<MilkProduct> products, int countElements)
        {
            Random rnd = new();
            for (int i = 0; i < countElements; i++)
            {
                MilkProduct milk = new();
                milk.RandomInit();
                if (rnd.Next(1, 4) == 2)
                    milk.Name = "Молоко";
                products.Add(milk);
            }
           
        }

        public static void QueryProbe(Queue<Dictionary<Goods, MilkProduct>> queue)
        {
            Console.WriteLine("Вывести проcроченные продукты:");
            Console.WriteLine("С помощью LINQ");
            var probeQueryLinq = from dict in queue 
                                 from product in dict.Values
                                 where product.ExpirationDate < new DateOnly(2024,5,27) select product;
            foreach (var value in probeQueryLinq)
            {
                Console.WriteLine(value);
            }

            Console.WriteLine("С помощью методов расширений:");
            var probeQueryMethod = queue.SelectMany(d => d.Values.Where(p => p.ExpirationDate < new DateOnly(2024, 5, 27)));
            foreach(var value in probeQueryMethod)
            {
                Console.WriteLine(value);
            }
        }

        public static void QueryCount(Queue<Dictionary<Goods, MilkProduct>> queue)
        {
            Console.WriteLine("Количество продуков с жирностью менее 25%: ");
            Console.Write("С помощью LINQ: ");
            var probeQueryLinq = (from dict in queue
                                  from product in dict.Values
                                  where product.FatContent < 25 select product).Count();
            Console.WriteLine(probeQueryLinq);

            Console.Write("С помощью методов расширений: ");
            var probeQueryMethod = queue.SelectMany(d => d.Values.Where(p => p.FatContent < 25)).Count();
            Console.WriteLine(probeQueryMethod);
        }

        public static void QueryUnion(Queue<Dictionary<Goods, MilkProduct>> queue)
        {
            Console.WriteLine("Вывести продукты с весом более 80 и менее 30:");
            Console.WriteLine("С помощью LINQ");
            var unionQueryLinq = (from dict in queue
                                  from product in dict.Values
                                  where product.Weight > 80 select product)
                                .Union((from dict in queue
                                        from product in dict.Values
                                        where product.Weight < 30 select product));
            foreach (var value in unionQueryLinq)
            {
                Console.Write(value);
                Console.WriteLine(" Вес: " + value.Weight);
            }

            Console.WriteLine("С помощью методов расширений:");
            var cheapProducts = queue.SelectMany(d => d.Values.Where(p => p.Weight > 80));
            var expensiveProoducts = queue.SelectMany(d => d.Values.Where(p => p.Weight < 30));
            var unionQueryMethod = cheapProducts.Union(expensiveProoducts);
            foreach (var value in unionQueryMethod)
            {
                Console.Write(value);
                Console.WriteLine(" Вес: "+ value.Weight);
            }
        }

        public static void QueryAgregate(Queue<Dictionary<Goods, MilkProduct>> queue)
        {
            Console.WriteLine("Вывести cамый дорогой и самый дешевый продукт:");
            Console.WriteLine("С помощью LINQ");
            var cheapQueryLinq = (from dict in queue
                                  from product in dict.Values
                                  select product).Min();
            var expensiveQueryLinq = (from dict in queue
                                      from product in dict.Values
                                      select product).Max();
            Console.Write(cheapQueryLinq);
            Console.WriteLine(" Цена: " + cheapQueryLinq.Price);
            Console.Write(expensiveQueryLinq);
            Console.WriteLine(" Цена: " + expensiveQueryLinq.Price);

            Console.WriteLine("С помощью методов расширений:");
            var cheapQueryMethod = queue.SelectMany(d => d.Values).Min();
            var expensiveQueryMethod = queue.SelectMany(d => d.Values).Max();
            Console.Write(cheapQueryLinq);
            Console.WriteLine(" Цена: " + cheapQueryMethod.Price);
            Console.Write(expensiveQueryLinq);
            Console.WriteLine(" Цена: " + expensiveQueryLinq.Price);
        }

        public static void QueryGroup(Queue<Dictionary<Goods, MilkProduct>> queue)
        {
            Console.WriteLine("Группировака продуктов по названию:");
            Console.WriteLine("С помощью LINQ");
            var groupLinq = (from dict in queue
                             from product in dict.Values
                             group product by product.Name
                             into g select new { Name = g.Key, Count = g.Count() }).ToList();

            foreach(var item in groupLinq)
            {
                Console.WriteLine(item);
            }

            var groupMethod = queue.SelectMany(d => d.Values.GroupBy(p => p.Name).Select(g => new { Name = g.Key, Count = g.Count() }));
            Console.WriteLine("С помощью методов расширений");
            foreach (var item in groupMethod)
            {
                Console.WriteLine(item);
            }
        }

        public static void MethodQueryCount(HashTable<MilkProduct> products)
        {
            Console.WriteLine($"Количество товаров с названием Молоко: {products.Count(p => p.Name == "Молоко")}");
        }

        public static void MethodQuerySorting(HashTable<MilkProduct> products)
        {
            Product[] sortedByPriceAscending = products.SortByPrice((p1, p2) => p1.Price > p2.Price);
            foreach (var item in sortedByPriceAscending)
            {
                Console.WriteLine(item);
            }
        }

        public static void MethodQuerySorting2(HashTable<MilkProduct> products)
        {
            Console.WriteLine("Сортировка по цене");
            var sorting = products.SortBy(p => p.Price);

            foreach(var item in sorting)
            {
                if (item != null)
                {
                    Console.Write(item);
                    Console.WriteLine($" Цена: {item.Price}");

                }

            }
        }

        public static void MethodQueryAvr(HashTable<MilkProduct> products)
        {
            Console.WriteLine($"Средняя цена товара: {products.AvrageBy(p => p.Price):F2}");
        }

        public static void MethodSelect(HashTable<MilkProduct> products)
        {
            Console.WriteLine("Вывести все товары с 2 в названии");

            var select = products.SelectProduct(p => p.Name.Contains('2'));

            foreach(var item in select)
            {
                Console.WriteLine(item);
            }
        }
    }
}