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
        private Livro livro = new Livro();
        private ConexaoBancoMongo conexao = new ConexaoBancoMongo();

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
            livro.QuantidadeDisponivel = int.Parse(Console.ReadLine());

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

                if (opcao == 1) { insereMais = false; }
            }
            livro.Assunto = assuntos;

            conexao.Livro.InsertOneAsync(livro);
            Console.WriteLine("Documento incluído com sucesso!");
            Console.Write("Pressione qualquer tecla para continuar: ");
            Console.ReadKey();
        }

        public async void RelatorioLivros()
        {
            Console.WriteLine("Listando Documentos");

            var listaLivros = await conexao.Livro.Find(new BsonDocument())
                                                           .ToListAsync();
            Console.WriteLine("{0, -5} {1, -32} {2, -4} {3, -4} {4, 9}\n", "Codigo", "Titulo", "Quantidade de Páginas", "Quantidade Disponível", "Valor");
            foreach (var doc in listaLivros)
            {
                Console.WriteLine("{0, -5} {1, -32} {2, 20} {3, 20} {4, 9}", doc.CodigoLivro, doc.Titulo, doc.Paginas, doc.QuantidadeDisponivel, doc.ValorAluguel);
            }

            Console.WriteLine("Fim da lista...");
        }

        public void RemoveLivro()
        {
            int opcao = 0;
            int codigo = 0;
            Console.WriteLine("Verifique o código do livro que deseja remover na lista abaixo: ");
            RelatorioLivros();
            Thread.Sleep(2000);
            Console.WriteLine();
            Console.Write("Informe o código do livro que deseja remover: ");
            codigo = int.Parse(Console.ReadLine());

            Console.Write("Tem certeza que deseja excluir esse registro? 1 - Sim, 0 - Não");
            opcao = int.Parse(Console.ReadLine());
            if (opcao == 1)
            {
                ExcluiLivro(codigo);
                Thread.Sleep(2000);
            }
            else
            {
                Console.WriteLine("Retornando ao menu de opções...");
                Console.WriteLine("Pressione qualquer tecla para continuar... ");
                Console.ReadKey();
            }

        }

        public async void ExcluiLivro(int codigo)
        {
            var construtor = Builders<Livro>.Filter;
            var condicao = construtor.Eq(x => x.CodigoLivro, codigo);

            Console.WriteLine("Excluindo livros");
            await conexao.Livro.DeleteOneAsync(condicao);
        }

        public async Task<int> VerificaRegistro(int codigo)
        {
            int existe = 0;
            var construtor = Builders<LivroAluguel>.Filter;
            var condicao = construtor.Eq(x => x.CodigoLivro, codigo);

            var listaLivros = await conexao.Livro.Find(new BsonDocument()).ToListAsync();

            if (listaLivros.Any())
            {
                existe = 1;
            }

            return existe;
        }

        public async Task<int> GeraCodigoAsync()
        {
            int codigo = 1;
            var listaLivro = await conexao.Livro.Find(new BsonDocument()).ToListAsync();
            foreach (var doc in listaLivro)
            {
                if (doc.CodigoLivro.HasValue)
                {
                    codigo = doc.CodigoLivro.Value + 1;
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
