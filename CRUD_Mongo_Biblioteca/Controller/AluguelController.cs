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
            aluguel.Id = null;
            aluguel.CodigoAluguel = null;            
            aluguel.CodigoLeitor = null;
            aluguel.Cpf = null;            
            aluguel.ValorTotal = 0;

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
    }
}
