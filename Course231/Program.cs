using System;
using System.Collections.Generic;
using System.Globalization;

using System.Linq;

using Course231.Entities;

namespace Course231
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
                //1. Criar um data source(coleção, array, recurso de E/ S, etc.)
                int[] numbers = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

                //2. Definir a query / consulta ... expressão de consulta
                // O LINK É COMPOSTO POR VÁRIOS EXTENSION MÉTODOS
                // MEU PREDICADO
                // este result e IEnumerable, contempla todos, mais que a LIST ou simplesmente usar VAR
                IEnumerable<int> result = numbers
                                .Where( x => x%2 == 0 )
                                .Select( x => x * 10);

                //3. Executar a query(foreach ou alguma operação terminal)
                foreach(int x in result)
                {
                    Console.WriteLine(x);

                }
            */

            //LINQ com LAMBDA

            //Estanciando minha categoria
            Category c1 = new Category() { Id = 1, Name = "Tools", Tier = 2 };
            Category c2 = new Category() { Id = 2, Name = "Computers", Tier = 1 };
            Category c3 = new Category() { Id = 3, Name = "Eletronics", Tier = 1 };

            //Estanciando meus produtos - datasource
            List<Product> products = new List<Product>()
                                        {
                                            new Product() { Id = 1,     Name = "Computer",    Price = 1100.0, Category = c2 },
                                            new Product() { Id = 2,     Name = "Hammer",      Price = 90.0,   Category = c1 },
                                            new Product() { Id = 3,     Name = "TV",          Price = 1700.0, Category = c3 },
                                            new Product() { Id = 4,     Name = "Notebook",    Price = 1300.0, Category = c2 },
                                            new Product() { Id = 5,     Name = "Saw",         Price = 80.0,   Category = c1 },
                                            new Product() { Id = 6,     Name = "Tablet",      Price = 700.0,  Category = c2 },
                                            new Product() { Id = 7,     Name = "Camera",      Price = 700.0,  Category = c3 },
                                            new Product() { Id = 8,     Name = "Printer",     Price = 350.0,  Category = c3 },
                                            new Product() { Id = 9,     Name = "MacBook",     Price = 1800.0, Category = c2 },
                                            new Product() { Id = 10,    Name = "Sound Bar",   Price = 700.0,  Category = c3 },
                                            new Product() { Id = 11,    Name = "Level",       Price = 70.0,   Category = c1 },
                                            new Product() { Id = 12,    Name = "MEU PRODUTO", Price = 10.0,   Category = new Category() { Id = 4, Name = "MINHACATEGORIA", Tier = 1 } }
                                         };

            //====> 231. Demo - LINQ com notação similar à SQL <======

            //var ou IEnumerable
            //IEnumerable<Product> r1 = products.Where(p => p.Category.Tier == 1 && p.Price < 900.0);
            //
            // from --- PRODUTO CARTESIANO --- TODO objeto P na fonte de dados PRODUCTS
            // where --- RESTRICAO
            // select --- PROJETCAO

            //Select *
            //FROM PRODUCT
            //WHERE PRODUCT.CATEGORY_ID = CATEGORY.ID

            IEnumerable<Product> r1 = 
                        from p in products
                        where p.Category.Tier == 1 && p.Price < 900.0
                        select p;

            Print("--- TIER: 1 AND PRICE: ATÉ 900 ---", r1);

            //IEnumerable<string> r2 = products
            //                            .Where(p => p.Category.Name == "Tools")
            //                            .Select(p => p.Name);

            IEnumerable<string> r2 =
                    from p in products
                    where p.Category.Name == "Tools"
                    select p.Name;

            Print("Names of products from tools", r2);

            //vou criar um objeto anônimo, com alias para categoria
            //COLOCO UM ALIAS OU APELIDO PARA TIRAR A AMBIGUIDADE DO name da categoria com o dos products
            /*IEnumerable<object> r3 = products
                        .Where(
                                p => p.Name[0] == 'C'

                                )
                        .Select(
                                 p => new {
                                     p.Name,
                                     p.Price,
                                     CategoriaName = p.Category.Name

                                 }
                               );
            */

            IEnumerable<object> r3 =
                    from p in products
                    where p.Name[0] == 'C'
                    select new
                    {
                        p.Name,
                        p.Price,
                        CategoriaName = p.Category.Name

                    };

            Print("Nomes começado com C", r3);

            /*
            IEnumerable<Product> r4 = products
                                .Where(
                                    p => p.Category.Tier == 1

                                    )
                                .OrderBy(
                                        p => p.Price

                                        )

                                .ThenBy(
                                        p => p.Name

                                     );
            */

            IEnumerable<Product> r4 =
                    from p in products
                    where p.Category.Tier == 1
                    orderby p.Name
                    orderby p.Price
                    select p;

            Print("TIER 1 order by price e pelo nome", r4);

            //Do resultado do R5 ele vai pular 2 elementos e pegar somente 4
            //IEnumerable<Product> r5 = r4.Skip(2).Take(4);

            IEnumerable<Product> r5 =
                                   ( from p in r4
                                    select p )
                                        .Skip(2).Take(4);
           
            Print("TIER 1 order by price e pelo nome - Skip(2).Take(4)", r5);

            //Product r6 = products.First(); //r6 = products.Last();
            Product r6 = (
                            from p in products
                            select p
                            ).First();
             
            Console.WriteLine("FIRST TESTE1: " + r6);



            //POR CATEGORIA
            /*
            var r17 = products
                            .GroupBy(
                                p => p.Category
                            );
            */

            var r17 =
                        from p in products
                        group p by p.Category;

            foreach (IGrouping<Category, Product> group in r17)
            {
                Console.WriteLine("Categoria: " + group.Key.Name);

                foreach (Product p in group)
                {
                    Console.WriteLine(p);
                    //Console.WriteLine("Produto: " + p.Name );

                }

                Console.WriteLine();
            }

        }

        static void Print<T>(string message, IEnumerable<T> collection)
        {
            Console.WriteLine(message);

            foreach (T obj in collection)
            {
                Console.WriteLine(obj);

            }

            Console.WriteLine();

        }

        //Era do tipo Products, mas agora estou passando tanto products quanto um tipo double
        static void Print2<T>(string message, T product)
        {
            Console.WriteLine();


            Console.WriteLine(
                    message
                    + " "
                    + product);

            Console.WriteLine();


        }


    }
}
