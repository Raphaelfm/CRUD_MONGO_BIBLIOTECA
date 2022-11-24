using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD_Mongo_Biblioteca.Conexao;
using CRUD_Mongo_Biblioteca.Model;
using MongoDB.Driver;

namespace CRUD_Mongo_Biblioteca.Controller
{
    public class LivroAluguelController
    {
        private ConexaoBancoMongo conexao = new ConexaoBancoMongo();
        private LivroAluguel itemAluguel = new LivroAluguel();
        //private LivroController livros = new LivroController();
        //private Aluguel aluguel = new Aluguel();
        //private AluguelController alugueis = new AluguelController();

        public void CadastrarLivroAluguel()
        {
            bool running = true;
            int opcao = 0;
            Console.WriteLine("Bem vindo ao cadastro de aluguel de livros, a seguir informe os dados conforme solicitado!");
            Console.WriteLine();
            while (running)
            {
                AplicaNulos();

                Console.WriteLine("Veja abaixo as informações necessárias, e cadastre os dados conforme codigos de livros: ");
                Console.WriteLine();
                Console.WriteLine("Listando Livros...");
                Console.WriteLine();
                ListarLivros();
                Thread.Sleep(1000);
                Console.WriteLine();

                Console.WriteLine("Insira as informações conforme os dados apresentados: ");
                itemAluguel.CodigoAluguel = PegaCodigoAluguel().Result;
                itemAluguel.CodigoLeitor = PegaCodigoLeitorl().Result;
                Thread.Sleep(1000);
                Console.Write("Codigo Livro: ");
                string codigo = Console.ReadLine();
                itemAluguel.CodigoLivro = int.Parse(codigo);
                itemAluguel.Titulo = PegaTituloLivro(int.Parse(codigo)).Result;
                Thread.Sleep(1000);

                Console.Write("Quantidade que está sendo alugada: ");
                itemAluguel.QuantidadeLivro = int.Parse(Console.ReadLine());

                itemAluguel.ValorUnitarioLivro = PegaValorLivro(int.Parse(codigo)).Result;
                Thread.Sleep(1000);
                itemAluguel.ValorTotalLivro = itemAluguel.QuantidadeLivro * itemAluguel.ValorUnitarioLivro;

                conexao.LivroAluguel.InsertOneAsync(itemAluguel);
                Console.WriteLine("Documento incluído com sucesso!");

                Console.WriteLine("Deseja incluir outro livro ao aluguel? 1 - Sim, 0 - Não (Digite apenas o número da opção)");
                opcao = int.Parse(Console.ReadLine());

                if (opcao == 0)
                {
                    Console.WriteLine("Encerrando pedido...");
                    running = false;
                }
            }



            Console.Write("Pressione qualquer tecla para continuar: ");
            Console.ReadKey();
        }

        public async void RelatorioLivrosAlugados()
        {
            Console.WriteLine("Listando Documentos");

            var listaLivros = await conexao.LivroAluguel.Find(new BsonDocument())
                                                           .ToListAsync();
            Console.WriteLine("{0, -12} {1, -12} {2, -32} {3, -4} {4, 9}\n", "Codigo Livro - ", "Codigo Aluguel - ", "Titulo", "Quantidade Alugada", "Valor");
            foreach (var doc in listaLivros)
            {
                Console.WriteLine("{0, 12} {1, 14} {2, 32} {3,12} {4, 22}", doc.CodigoLivro, doc.CodigoAluguel, doc.Titulo, doc.QuantidadeLivro, doc.ValorTotalLivro);
            }

            Console.WriteLine("Fim da lista...");
        }

        public void RemoveLivroAluguel()
        {
            AplicaNulos();
            int opcao = 0;
            int codigo = 0;
            int codigoAluguel = 0;
            Console.WriteLine("Verifique o código do livro alugado e o codigo do Aluguel que deseja remover na lista abaixo: ");
            RelatorioLivrosAlugados();
            Thread.Sleep(2000);
            Console.WriteLine();
            Console.Write("Informe o código do livro que deseja remover: ");
            codigo = int.Parse(Console.ReadLine());

            Console.WriteLine("Informe o código do aluguel que esse livro pertence: ");
            codigoAluguel = int.Parse(Console.ReadLine());

            Console.WriteLine("Tem certeza que deseja excluir esse registro? 1 - Sim, 0 - Não");
            opcao = int.Parse(Console.ReadLine());

            if (opcao == 1)
            {
                Console.WriteLine("Ao remover este livro, caso no aluguel não tenha outro registro, o Aluguel também será excluído.");
                Console.Write("Deseja realemente continuar? 1 - Sim, 0 - Não (Digite apenas o número da opção): ");
                opcao = int.Parse(Console.ReadLine());
                if (opcao == 1)
                {
                    VerificaRegistroAluguel(codigo);
                    Thread.Sleep(2000);                    
                    ExcluiLivro(codigo, codigoAluguel);
                    Thread.Sleep(2000);
                }
                else
                {
                    Console.WriteLine("Retornando ao menu de opções...");
                }
            }            
        }

        public void ExcluiLivro(int codigo, int codigoAluguel)
        {
            var construtor = Builders<LivroAluguel>.Filter;
            var condicao = construtor.Eq(x => x.CodigoLivro, codigo) & construtor.Eq(x => x.CodigoAluguel, codigoAluguel);

            Console.WriteLine($"Removendo o livro selecionado do aluguel {codigoAluguel}...");
            conexao.LivroAluguel.DeleteOneAsync(condicao);
            Console.WriteLine("Registro excluido com sucesso!");
        }

        //Verifica se no aluguel existe algum livro associado além do livro que está sendo excluído para determinar se o aluguel será excluido
        public async void VerificaRegistroAluguel(int codigo)
        {
            int existe = 0;
            int codigoAluguel = 0;
            var construtor = Builders<LivroAluguel>.Filter;
            var condicao = construtor.Eq(x => x.CodigoLivro, codigo);

            var listaLivros = await conexao.LivroAluguel.Find(condicao).ToListAsync();

            foreach (var cod in listaLivros)
            {
                codigoAluguel = cod.CodigoAluguel.Value;
            }

            construtor = Builders<LivroAluguel>.Filter;
            condicao = construtor.Eq(x => x.CodigoAluguel, codigoAluguel);

            listaLivros = await conexao.LivroAluguel.Find(condicao).ToListAsync();

            foreach (var doc in listaLivros)
            {
                if (doc.CodigoLivro != codigo)
                {
                    existe = 1;
                }
            }

            if (existe != 1)
            {
                var construtor1 = Builders<Aluguel>.Filter;
                var condicao1 = construtor1.Eq(x => x.CodigoAluguel, codigoAluguel);

                Console.WriteLine("Excluindo Aluguel");
                await conexao.Aluguel.DeleteOneAsync(condicao1);
                Console.WriteLine("Aluguel Excluido com sucesso!");
            }
        }

        public int ContaEntidadeLivroAluguel()
        {
            int quantidadeAluguel = 0;
            var aluguel = conexao.LivroAluguel.CountDocuments(new BsonDocument());
            quantidadeAluguel = (int)aluguel;
            return quantidadeAluguel;
        }

        public async void ListarLivros()
        {
            var listaLivros = await conexao.Livro.Find(new BsonDocument())
                                                           .ToListAsync();
            //int quantidade = livros.ContaEntidadeLivro();
            foreach (var doc in listaLivros)
            {
                Console.WriteLine($"Codigo Livro: {doc.CodigoLivro} | Titulo: {doc.Titulo} | Estoque disponível: {doc.QuantidadeDisponivel}");
            }
        }

        public async Task<int> PegaCodigoAluguel()
        {
            int codigo = 1;
            var listaAlugel = await conexao.Aluguel.Find(new BsonDocument()).ToListAsync();
            foreach (var doc in listaAlugel)
            {
                if (doc.CodigoAluguel.HasValue)
                {
                    codigo = doc.CodigoAluguel.Value;
                }
            }
            return codigo;
        }

        public async Task<int> PegaCodigoLeitorl()
        {
            int codigo = 1;
            var listaAlugel = await conexao.Aluguel.Find(new BsonDocument()).ToListAsync();
            foreach (var doc in listaAlugel)
            {
                if (doc.CodigoLeitor.HasValue)
                {
                    codigo = doc.CodigoLeitor.Value;
                }
            }
            return codigo;
        }

        public async Task<double> PegaValorLivro(int codigo)
        {
            var construtor = Builders<Livro>.Filter;
            var condicao = construtor.Eq(x => x.CodigoLivro, codigo);
            double valor = 0;
            var listaLeitores = await conexao.Livro.Find(condicao)
                                                           .ToListAsync();

            foreach (var doc in listaLeitores)
            {
                valor = doc.ValorAluguel.Value;
            }

            return valor;
        }

        public void AlteraLivroAluguel()
        {

        }

        public async Task<string> PegaTituloLivro(int codigo)
        {
            var construtor = Builders<Livro>.Filter;
            var condicao = construtor.Eq(x => x.CodigoLivro, codigo);
            string titulo = "";
            var listaLeitores = await conexao.Livro.Find(condicao)
                                                           .ToListAsync();

            foreach (var doc in listaLeitores)
            {
                titulo = doc.Titulo;
            }

            return titulo;
        }

        public void AplicaNulos()
        {
            itemAluguel.Id = null;
            itemAluguel.CodigoAluguel = null;
            itemAluguel.CodigoLeitor = null;
            itemAluguel.QuantidadeLivro = null;
            itemAluguel.CodigoLivro = null;
            itemAluguel.Titulo = null;
            itemAluguel.ValorUnitarioLivro = null;
            itemAluguel.ValorTotalLivro = null;
        }
    }
}
