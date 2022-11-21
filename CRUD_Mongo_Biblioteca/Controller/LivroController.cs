using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD_Mongo_Biblioteca.Model;
using CRUD_Mongo_Biblioteca.Conexao;
using MongoDB.Driver;
using MongoDB.Bson;

namespace CRUD_Mongo_Biblioteca.Controller
{
    public class LivroController
    {
        Livro livro = new Livro();
        ConexaoBancoMongo conexao = new ConexaoBancoMongo();

        public void CadastrarLivro()
        {
            livro.Id = null;
            livro.CodigoLivro = null;
            livro.Titulo = null;
            livro.Autor = null;
            livro.Ano = null;
            livro.Paginas = null;
            livro.Assunto = null;

            var codigo = GeraCodigoAsync();

            Console.WriteLine("Bem vindo ao cadastro de livros, a seguir informe os dados conforme solicitado!");

            livro.CodigoLivro = codigo.Result;

            Console.Write("Titulo do Livro: ");
            livro.Titulo = Console.ReadLine();

            Console.Write("Autor: ");
            livro.Autor = Console.ReadLine();

            Console.Write("Ano do Livro: ");
            livro.Ano = int.Parse(Console.ReadLine());

            Console.Write("Quantidade de páginas: ");
            livro.Paginas = int.Parse(Console.ReadLine());

            Console.Write("Quantidade Disponível: ");
            livro.QuantidadeDisponivel= int.Parse(Console.ReadLine());

            Console.Write("Valor aluguel: ");
            livro.ValorAluguel = Double.Parse(Console.ReadLine());


            //Cria uma lista para adicionar vários assuntos em uma unica chamada de método
            Console.Write("Assunto: ");
            bool insereMais = true;
            List<string> assuntos = new List<string>();
            while (insereMais)
            {
                int opcao = int.Parse("1");

                Console.Write("Assunto do livro: ");
                string? assunto = Console.ReadLine();
                assuntos.Add(assunto);

                Console.WriteLine("Deseja incluir mais assuntos para este livro? 0 - Sim, 1 - Não");
                opcao = int.Parse(Console.ReadLine());

                if(opcao == 1) { insereMais= false; }
            }
            livro.Assunto = assuntos;

            conexao.Livro.InsertOneAsync(livro);
            Console.WriteLine("Documento incluído com sucesso!");
        }

        public async Task<int> GeraCodigoAsync()
        {
            int codigo = 0;
            var listaLivro = await conexao.Livro.Find(new BsonDocument()).ToListAsync();
            foreach (var doc in listaLivro)
            {
                if (doc.CodigoLivro.HasValue)
                {                    
                    codigo = doc.CodigoLivro.Value + 1;
                }
                else
                {
                    codigo = 1;
                    return codigo;
                }
            }
            return codigo;
        }

        public int ContaEntidadeLivro()
        {
            int quantidadeLivro = 0;
            var livros = conexao.Livro.CountDocuments(new BsonDocument());
            quantidadeLivro = (int)livros;
            return quantidadeLivro;
        }
    }
}
