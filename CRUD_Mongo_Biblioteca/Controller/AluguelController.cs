using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD_Mongo_Biblioteca.Conexao;
using CRUD_Mongo_Biblioteca.Model;
using MongoDB.Driver;
using DnsClient.Internal;

namespace CRUD_Mongo_Biblioteca.Controller
{    
    public class AluguelController
    {
        private Aluguel aluguel = new Aluguel();
        private ConexaoBancoMongo conexao = new ConexaoBancoMongo();
        private Leitor leitor = new Leitor();
        private LeitorController leitores = new LeitorController();
        private Livro livro = new Livro();
        private LivroController livros = new LivroController();
        private LivroAluguelController itemAluguel = new LivroAluguelController();


        public void CadastrarAluguel()
        {
            AplicaNulos();

            var codigo = GeraCodigoAsync();

            Console.WriteLine("Bem vindo ao cadastro de aluguel de livros, a seguir informe os dados conforme solicitado!");
            Console.WriteLine();
            Console.WriteLine("Veja abaixo as informações necessárias, e cadastre os dados conforme codigos de leitores e livros: ");
            Console.WriteLine();
            Console.WriteLine("Listanto Leitores...");
            Console.WriteLine();
            ListarLeitoresAsync();
            Thread.Sleep(1000);

            Console.WriteLine("Insira as informações conforme os dados apresentados: ");
            aluguel.CodigoAluguel = GeraCodigoAsync().Result;
            Console.WriteLine("Codigo do Leitor: ");
            string codigoLeitor = Console.ReadLine();
            aluguel.CodigoLeitor = int.Parse(codigoLeitor);
            int co = int.Parse(codigoLeitor);
            string cpf = PegaCpfLeitor(co).Result;
            Thread.Sleep(1000);
            aluguel.Cpf = cpf;
            aluguel.Nome = PegaNomeLeitor(co).Result;
            Thread.Sleep(1000);
            aluguel.ValorTotal = 0;

            conexao.Aluguel.InsertOneAsync(aluguel);
            itemAluguel.CadastrarLivroAluguel();

            Console.WriteLine("Documento incluído com sucesso!");
            Console.Write("Pressione qualquer tecla para continuar: ");
            Console.ReadKey();
        }

        public async void RelatorioAlugueis()
        {
            Console.WriteLine("Listando Documentos");

            var listaAluguel = await conexao.Aluguel.Find(new BsonDocument())
                                                           .ToListAsync();
            Console.WriteLine("{0, -7} {1, -12} {2, -32} {3, -12} {4, 12}\n", "Codigo", "C. Leitor", "Nome", "CPF", "Valor Total");
            foreach (var doc in listaAluguel)
            {
                Console.WriteLine("{0, -7} {1, -12} {2, -32} {3, -12} {4, 12}", doc.CodigoAluguel, doc.CodigoLeitor, doc.Nome, doc.Cpf, doc.ValorTotal);
            }

            Console.WriteLine("Fim da lista...");
        }

        public void RemoveAluguel()
        {
            AplicaNulos();
            int opcao = 0;
            int codigo = 0;
            Console.WriteLine("Verifique o código do aluguel que deseja remover na lista abaixo: ");
            RelatorioAlugueis();
            Thread.Sleep(2000);
            Console.WriteLine();
            Console.Write("Informe o código do aluguel que deseja remover: ");
            codigo = int.Parse(Console.ReadLine());

            Console.WriteLine("Tem certeza que deseja excluir esse registro? 1 - Sim, 0 - Não");
            opcao = int.Parse(Console.ReadLine());

            if (opcao == 1)
            {
                Console.WriteLine("Removendo este alguel, os itens associados ao mesmo serão removidos de LivroAluguel também.");
                Console.Write("Deseja realemente continuar? 1 - Sim, 0 - Não (Digite apenas o número da opção: )");
                opcao = int.Parse(Console.ReadLine());
                if (opcao == 1)
                {
                    ExcluiLivro(codigo);
                    Thread.Sleep(2000);
                    ExcluiAluguel(codigo);
                    Thread.Sleep(2000);
                }                
            }
            Console.WriteLine("Retornando ao menu de opções...");
            Console.WriteLine("Pressione qualquer tecla para continuar... ");
            Console.ReadKey();
        }

        public void ExcluiAluguel(int codigo)
        {
            var construtor = Builders<Aluguel>.Filter;
            var condicao = construtor.Eq(x => x.CodigoAluguel, codigo);

            Console.WriteLine("Excluindo Aluguel...");
            conexao.Aluguel.DeleteOneAsync(condicao);
            Console.WriteLine("Aluguel excluido com sucesso! \nPressione qualquer tecla para continuar... ");
            Console.ReadKey();
        }

        //Exclui o livro assosiado ao aluguel de LivroAlugel
        public void ExcluiLivro(int codigo)
        {
            var construtor = Builders<LivroAluguel>.Filter;
            var condicao = construtor.Eq(x => x.CodigoAluguel, codigo);

            Console.WriteLine("Limpando registros de livros referentes ao aluguel excluido...");
            conexao.LivroAluguel.DeleteManyAsync(condicao);
            Console.WriteLine("Registro excluido com sucesso! \nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }

        public async Task<int> GeraCodigoAsync()
        {
            int codigo = 1;
            var listaAlugel = await conexao.Aluguel.Find(new BsonDocument()).ToListAsync();
            foreach (var doc in listaAlugel)
            {
                if (doc.CodigoAluguel.HasValue)
                {
                    codigo = doc.CodigoAluguel.Value + 1;
                }
            }
            return codigo;
        }

        public int ContaEntidadeAluguel()
        {
            int quantidadeAluguel = 0;
            var aluguel = conexao.Aluguel.CountDocuments(new BsonDocument());
            quantidadeAluguel = (int)aluguel;
            return quantidadeAluguel;
        }

        public async void ListaLeitores()
        {
            var listaLeitores = await conexao.Leitor.Find(new BsonDocument())
                                                           .ToListAsync();
            int quantidade = leitores.ContaEntidadeLeitor();
            foreach (var doc in listaLeitores)
            {
                Console.WriteLine($"Codigo Leitor: {doc.CodigoLeitor} | Nome Leitor: {doc.Nome} | CPF Leitor: {doc.Cpf}");                  
            }
        }       

        public void ListarLeitoresAsync()
        {
            ListaLeitores();
        }        

        public async Task<string> PegaCpfLeitor(int codigo)
        {
            var construtor = Builders<Leitor>.Filter;
            var condicao = construtor.Eq(x => x.CodigoLeitor, codigo);
            string cpf = "";
            var listaLeitores = await conexao.Leitor.Find(condicao)
                                                           .ToListAsync();

            foreach(var doc in listaLeitores)
            {
                cpf = doc.Cpf;
            }

            return cpf;
        }

        public async Task<string> PegaNomeLeitor(int codigo)
        {
            var construtor = Builders<Leitor>.Filter;
            var condicao = construtor.Eq(x => x.CodigoLeitor, codigo);
            string nome = "";
            var listaLeitores = await conexao.Leitor.Find(condicao)
                                                           .ToListAsync();

            foreach (var doc in listaLeitores)
            {
                nome = doc.Nome;
            }

            return nome;
        }

        private void AplicaNulos()
        {
            aluguel.Id = null;
            aluguel.CodigoAluguel = null;
            aluguel.CodigoLeitor = null;
            aluguel.Nome = null;
            aluguel.Cpf = null;
            aluguel.ValorTotal = 0;
        }
    }
}
