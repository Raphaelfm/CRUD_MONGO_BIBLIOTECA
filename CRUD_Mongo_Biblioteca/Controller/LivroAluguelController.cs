using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD_Mongo_Biblioteca.Conexao;
using CRUD_Mongo_Biblioteca.Model;

namespace CRUD_Mongo_Biblioteca.Controller
{
    public class LivroAluguelController
    {
        private ConexaoBancoMongo conexao = new ConexaoBancoMongo();
        private LivroAluguel itemAluguel = new LivroAluguel(); 

        public void CadastrarLivroAluguel()
        {
            itemAluguel.QuantidadeLivro = null;
            itemAluguel.CodigoLivro = null;
            itemAluguel.ValorUnitarioLivro = null;
            itemAluguel.ValorTotalLivro = null;
        }


        public int ContaEntidadeLivroAluguel()
        {
            int quantidadeAluguel = 0;
            var aluguel = conexao.LivroAluguel.CountDocuments(new BsonDocument());
            quantidadeAluguel = (int)aluguel;
            return quantidadeAluguel;
        }
    }
}
