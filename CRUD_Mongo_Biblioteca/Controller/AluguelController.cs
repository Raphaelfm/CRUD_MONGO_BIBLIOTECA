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
    public class AluguelController
    {
        private Aluguel aluguel = new Aluguel();
        private ConexaoBancoMongo conexao = new ConexaoBancoMongo();
        private Leitor leitor = new Leitor();
        private LeitorController leitores = new LeitorController();
        private Livro livro = new Livro();

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
            ListaLeitores();

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
            var listaLivros = await conexao.Leitor.Find(new BsonDocument())
                                                           .ToListAsync();
            int quantidade = leitores.ContaEntidadeLeitor();
            string[] livros = new string[quantidade];
            foreach (var doc in listaLivros)
            {
                Console.WriteLine($"Codigo Leitor: {doc.CodigoLeitor} | Nome Leitor: {doc.Nome} | CPF Leitor: {doc.Cpf}");                  
            }
        }
    }
}
