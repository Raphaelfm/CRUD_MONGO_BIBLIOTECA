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
            itemAluguel.CodigoAluguel = null;
            itemAluguel.QuantidadeLivro = null;
            itemAluguel.CodigoLivro = null;
            itemAluguel.ValorUnitarioLivro = null;
            itemAluguel.ValorTotalLivro = null;

            Console.WriteLine("Bem vindo ao cadastro de aluguel de livros, a seguir informe os dados conforme solicitado!");
            Console.WriteLine();
            Console.WriteLine("Veja abaixo as informações necessárias, e cadastre os dados conforme codigos de livros: ");            
            Console.WriteLine();
            Console.WriteLine("Listando Livros...");
            Console.WriteLine();
            ListarLivros();
            Thread.Sleep(1000);
            Console.WriteLine();

            Console.WriteLine("Insira as informações conforme os dados apresentados: ");
            itemAluguel.CodigoAluguel = PegaCodigoAluguel().Result;
            Thread.Sleep(1000);
            Console.Write("Codigo Livro: ");
            string codigo = Console.ReadLine();
            itemAluguel.CodigoLivro = int.Parse(codigo);

            Console.Write("Quantidade que está sendo alugada: ");
            itemAluguel.QuantidadeLivro = int.Parse(Console.ReadLine());
            
            itemAluguel.ValorUnitarioLivro = PegaValorLivro(int.Parse(codigo)).Result;
            Thread.Sleep(1000);
            itemAluguel.ValorTotalLivro = itemAluguel.QuantidadeLivro * itemAluguel.ValorUnitarioLivro;

            conexao.LivroAluguel.InsertOneAsync(itemAluguel);

            Console.WriteLine("Documento incluído com sucesso!");
            Console.Write("Pressione qualquer tecla para continuar: ");
            Console.ReadKey();
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
    }
}
