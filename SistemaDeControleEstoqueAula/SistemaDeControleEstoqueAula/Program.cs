using System;
using System.Data;
using System.Data.SqlClient;

namespace SistemaDeContoleEstoqueAula
{
    class Program
    {
        static string connectionString = "Data Source=localhost\\sqlexpress;Initial Catalog=ControleDeEstoque;Integrated Security=True;";
        static void Main(string[] args)
        {
            int opcao;

            do
            {
                Console.Clear();
                Console.WriteLine("Sistema de controle de fluxo\n".ToUpper());
                Console.WriteLine("1 - Adicionar Produto");   //   C
                Console.WriteLine("2 - Listar Produtos");    //    R
                Console.WriteLine("3 - Atualizar Estoque"); //     U
                Console.WriteLine("4 - Remover Produtos"); //      D
                Console.WriteLine("5 - Relatorio Produtos em Baixa");
                Console.WriteLine("0 - Sair");
                Console.Write("\nEscolha uma opção: ");
                opcao = int.Parse(Console.ReadLine());

                switch(opcao)
                {
                    case 1: AdicionarProdutos(); break;
                    case 2: ListarProdutos(); break;
                    case 3: AtualizarProdutos(); break;
                    case 4: RemoverProdutos(); break;
                    case 5: RelatorioProdutosBaixa(); break;
                }
            }
            while (opcao != 0);
        }

        static void AdicionarProdutos()
        {
            Console.Write("Nome do Produto: ");
            string nomeProduto = Console.ReadLine();

            Console.Write("Preço do Produto: ");
            decimal precoProduto = decimal.Parse(Console.ReadLine());

            Console.Write("Quantidade do Produto: ");
            int quantidadeProduto = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Produtos (Nome, Preco, Quantidade) VALUES (@Nome, @Preco, @Quantidade)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nome", nomeProduto);
                    cmd.Parameters.AddWithValue("@Preco", precoProduto);
                    cmd.Parameters.AddWithValue("Quantidade", quantidadeProduto);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Produto adicionado com sucesso!");
                }
            }
            Console.ReadKey();
        }
        static void ListarProdutos()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Produtos";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("\n{0,-5} {1, -25} {2, -10} {3, -15} {4, -20}",
                                          "ID", "Nome", "Preço", "Quantidade", "DataCadastro");

                    Console.WriteLine(new string('-', 85));

                    while (reader.Read())
                    {
                        Console.WriteLine(
                            "{0,-5} {1, -25} {2, -10:C} {3, -15} {4, -20}",
                            reader["Id"],
                            reader["Nome"],
                            reader["Preco"],
                            reader["Quantidade"],
                            Convert.ToDateTime(reader["DataCadastro"]).ToString("dd/MM/yyyy HH:mm:ss")
                        );
                    }
                }
                Console.ReadKey();
            }
        }
        static void AtualizarProdutos()
        {
            Console.Write("ID do Produto: ");
            int idProduto = int.Parse(Console.ReadLine());
            Console.Write("Nova quantidade: ");
            int quantidadeProduto = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Produtos SET Quantidade = @Quantidade " +
                               "WHERE ID = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", idProduto);
                    cmd.Parameters.AddWithValue("@Quantidade", quantidadeProduto);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Estoque do produto atualizado com sucesso!");
                }
            }
            Console.ReadKey();
        }
        static void RemoverProdutos()
        {
            Console.Write("ID do Produto: ");
            int idProduto = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Produtos " +
                                "WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", idProduto);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Produto removido com sucesso!");
                }
            }
            Console.ReadKey();
        }
        static void RelatorioProdutosBaixa()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Produtos " +
                               "WHERE Quantidade < 10";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    Console.WriteLine("ID\tNome\t\t\t\tQuantidade");
                    Console.WriteLine(new string('-', 50)) ;
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Id"]}\t{reader["Nome"]}\t\t{reader["Quantidade"]}");
                    }
                }
            }
            Console.ReadKey();
        }
    }
}