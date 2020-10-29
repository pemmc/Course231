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
            IEnumerable<object> r3 = products
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

            Print("Nomes começado com C", r3);

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

            Print("TIER 1 order by price e pelo nome", r4);

            //Do resultado do R5 ele vai pular 2 elementos e pegar somente 4
            IEnumerable<Product> r5 = r4.Skip(2).Take(4);

            Print("TIER 1 order by price e pelo nome - Skip(2).Take(4)", r5);

            Product r6 = products.First(); //r6 = products.Last();

            Console.WriteLine("FIRST TESTE1: " + r6);

            //Neste caso como não teremos nenhum resultado com preco MAIOR que 3000.0, o FIRST iria dar uma EXCEPTION.. teriamos que
            //tratá-la caso isso aconteça ou usar o FirstOrDefault, o que fizemos agora
            Product r7 = products
                        .Where(
                                p => p.Price > 3000.0

                                )
                        .FirstOrDefault();

            Console.WriteLine("FIRST TESTE1: <usando o first ou default " + r7);

            //aqui usando com o sigleordefault, como sei que é ID... só tem 1 elemento ou nao tem nenhum
            //se eu usar somente o single eu consigo pegar somente UM ELEMENTO e daí não irá retornar uma colecao
            //MAS UM SIMPLES ELEMENTO
            Product r8 = products
                        .Where(p => p.Id == 3)
                        .SingleOrDefault();

            Console.WriteLine("SINGLE: <usando o single ou default: " + r8);

            //nao tem ninguem com o ID 100
            r8 = products
                        .Where(p => p.Id == 100)
                        .SingleOrDefault();

            Console.WriteLine("SINGLE: <usando o single ou default: " + r8);

            //Vou usar O FIRST so para ter um elemento PRODUCT E NAO UMA COLECAO IEnumerable
            Product r9 = products
                      .Where(p => p.Price <= 100.0)
                      .First();

            Print2("FIRST: EXISTEM MAIS DO QUE 1 PRODUTO, mas usei o FIRST: ", r9);

            /*
            //UMA COLECAO IEnumerable
            IEnumerable<Product> r10 = products
                      .Where(
                                p => p.Price <= 100.0
                             );

            Print("SINGLE: EXISTEM MAIS DO QUE 1 PRODUTO, agora nao usei o single: ", r10);
            */

            //se eu deixar sem a expressão lambada eu teria que implementar o IComparable, senão dá uma exceção
            double r10 = products
                            .Max(
                                    p => p.Price

                                    );

            Print2("PRICE: Preço máximo: ", r10);

            double r11 = products
                            .Min(
                                    p => p.Price

                                    );

            Print2("PRICE: Preço mínimo: ", r11);

            double r12 = products
                             .Where(p => p.Category.Id == 1)
                             .Sum(p => p.Price);


            Print2("ID DA CATEGORIA 1: SOMA ", r12);

            double r13 = products
                             .Where(p => p.Category.Id == 1)
                             .Average(p => p.Price);


            Print2("ID DA CATEGORIA 1: média dos preços ", r13);

            //Aqui é um macete para não retornar uma colecao vazia que nao existe ID 5 nao tem
            //Usando antes o select com a expressao lambda do filtro que iria para o average
            //e depois usa-se o DEFAULTIFEMPTY! e daí somente chama-se o avarage!
            var r14 = products
                            .Where(p => p.Category.Id == 5)
                            .Select(p => p.Price)
                            .DefaultIfEmpty(0.0)
                            .Average()
                            ;

            Print2("ID DA CATEGORIA 15 NAO EXISTE: média dos preços - COLEÇÃO VAZIA ", r14.ToString("C", new CultureInfo("pt-BR")));


            //fazendo minha própria operação
            //usando uma funcao anonima!
            //É uma função que recebe x e y (x, y
            //e faz a operacao que quero agregar... somando o x e y ... 

            double r15 = products
                            .Where(p => p.Category.Id == 1)
                            .Select(p => p.Price)
                            .Aggregate(
                                        (x, y) => x + y
                            );

            Print2("CATEGORIA AGREGATE SUM ", r15.ToString("C", new CultureInfo("pt-BR")));

            //Com resultado vazio... passo um valor padrao inicial como 0.00
            double r16 = products
                            .Where(p => p.Category.Id == 5)
                            .Select(p => p.Price)
                            .Aggregate(
                                        0.0, (x, y) => x + y
                            );

            Print2("CATEGORIA AGREGATE SUM QUE RETORNARIA VAZIO PQ ID 5 NAO TEM", r16.ToString("C", new CultureInfo("pt-BR")));


            //POR CATEGORIA
            var r17 = products
                            .GroupBy(
                                p => p.Category
                            );

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
