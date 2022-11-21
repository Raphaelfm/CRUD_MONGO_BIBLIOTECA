using CRUD_Mongo_Biblioteca.Conexao;
using CRUD_Mongo_Biblioteca.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_Mongo_Biblioteca.Controller
{
    public class LeitorController
    {
        private Leitor leitor = new Leitor();
        private ConexaoBancoMongo conexao = new ConexaoBancoMongo();

        public void CadastrarLeitor()
        {
            leitor.Id = null;
            leitor.CodigoLeitor = null;
            leitor.Nome = null;
            leitor.Cpf = null;

            var codigo = GeraCodigoAsync();

            Console.WriteLine("Bem vindo ao cadastro de leitores, a seguir informe os dados conforme solicitado!");

            leitor.CodigoLeitor = codigo.Result;

            Console.Write("Nome do Leitor: ");
            leitor.Nome = Console.ReadLine();

            Console.Write("CPF: ");
            leitor.Cpf = Console.ReadLine();

            conexao.Leitor.InsertOneAsync(leitor);

            Console.WriteLine("Documento incluído com sucesso!");
            Console.Write("Pressione qualquer tecla para continuar: ");
            Console.ReadKey();
        }

        public async Task<int> GeraCodigoAsync()
        {
            int codigo = 1;
            var listaLeitor = await conexao.Leitor.Find(new BsonDocument()).ToListAsync();
            foreach (var doc in listaLeitor)
            {
                if (doc.CodigoLeitor.HasValue)
                {
                    codigo = doc.CodigoLeitor.Value + 1;
                }                
            }
            return codigo;
        }

        public int ContaEntidadeLeitor()
        {
            int quantidadeLeitor = 0;
            var leitores = conexao.Leitor.CountDocuments(new BsonDocument());
            quantidadeLeitor = (int)leitores;
            return quantidadeLeitor;
        }
    }
}
