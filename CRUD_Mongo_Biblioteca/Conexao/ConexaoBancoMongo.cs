﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using CRUD_Mongo_Biblioteca.Model;

namespace CRUD_Mongo_Biblioteca.Conexao
{
    public class ConexaoBancoMongo
    {
        public const string STRING_DE_CONEXAO = "mongodb://localhost:27017";
        public const string NOME_DA_BASE = "SistemaBiblioteca";
        public const string NOME_DA_COLECAO = "Livros";

        private static readonly IMongoClient _cliente;
        private static readonly IMongoDatabase _BaseDados;

        static ConexaoBancoMongo()
        {
            _cliente = new MongoClient(STRING_DE_CONEXAO);
            _BaseDados = _cliente.GetDatabase(NOME_DA_BASE);
        }

        public IMongoClient Cliente { get { return _cliente; } }

        public IMongoCollection<Livro> Livro
        {
            get { return _BaseDados.GetCollection<Livro>(NOME_DA_COLECAO); }
        }
    }
}
