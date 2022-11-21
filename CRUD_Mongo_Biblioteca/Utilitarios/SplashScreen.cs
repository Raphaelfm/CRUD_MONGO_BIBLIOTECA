using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD_Mongo_Biblioteca.Controller;

namespace CRUD_Mongo_Biblioteca.Utilitarios
{
    public class SplashScreen
    {
        LivroController livro = new LivroController();
        public void Splash()
        {
            Console.WriteLine("##################################################");
            Console.WriteLine("#");
            Console.WriteLine("# Sistema de Biblioteca com Mongo DB");

            Console.WriteLine("#");
            Console.WriteLine("##################################################");

            Console.WriteLine("# TOTAL DE REGISTROS EXISTENTES:");
            Console.WriteLine($"#  1 - LIVRO: {livro.ContaEntidadeLivro()}");
            Console.WriteLine($"#  2 - LEITOR: ");
            Console.WriteLine($"#  3 - ALUGUEL: ");
            Console.WriteLine("#");

            Console.WriteLine("# SISTEMA DESENVOLVIDO POR: \n" +
                "# BRUNO CORREIA BARBOSA  \n" +
                "# HYAGO ESIO CAMPOS ALMEIDA \n" +
                "# MATHEUS ALVES NEITZL \n" +
                "# PHELLIPE SANTOS SARMENTO \n" +
                "# RAPHAEL FERREIRA DE MORAES \n" +
                "# TIAGO DE LIMA SANTOS ABREU");
            Console.WriteLine("#");

            Console.WriteLine("# DISCIPLINA: BANCO DE DADOS - 2022/2 \n# PROFESSOR: HOWARD ROATTI");
            Console.WriteLine("##################################################\n");
        }
    }
}
