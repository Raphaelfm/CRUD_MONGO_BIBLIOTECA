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

        public void CadastrarAluguel()
        {
            aluguel.Id = null;
            aluguel.CodigoAluguel = null;            
            aluguel.CodigoLeitor = null;
            aluguel.Cpf = null;            
            aluguel.ValorTotal = null;

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
    }
}
