using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_Mongo_Biblioteca.Model
{
    public class Aluguel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public int? CodigoAluguel { get; set; }
        public int? QuantidadeAluguel { get; set; }
        public int? CodigoLeitor { get; set; }
        public string? Cpf { get; set; }
        public int? CodigoLivro { get; set; }
        public double? ValorUnitario { get; set; }
        public double? ValorTotal { get; set; }
    }
}
