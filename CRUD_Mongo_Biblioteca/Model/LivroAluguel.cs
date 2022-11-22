﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_Mongo_Biblioteca.Model
{
    public class LivroAluguel
    {
        public int? CodigoAluguel { get; set; }
        public int? QuantidadeLivro { get; set; }
        public int? CodigoLivro { get; set; }
        public string? Titulo { get; set; }
        public double? ValorUnitarioLivro { get; set; }
        public double? ValorTotalLivro { get; set; }
    }
}
