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
        private LivroController livro = new LivroController();
        private LeitorController leitor = new LeitorController();
        private AluguelController aluguel = new AluguelController();

        public void Splash()
        {
            Console.WriteLine("##################################################");
            Console.WriteLine("#");
            Console.WriteLine("# Sistema de Biblioteca com Mongo DB");

            Console.WriteLine("#");
            Console.WriteLine("##################################################");

            Console.WriteLine("# TOTAL DE REGISTROS EXISTENTES:");
            Console.WriteLine($"#  1 - LIVROS: {livro.ContaEntidadeLivro()}");
            Console.WriteLine($"#  2 - LEITOR: {leitor.ContaEntidadeLeitor()}");
            Console.WriteLine($"#  3 - ALUGUEL: {aluguel.ContaEntidadeAluguel()}");
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
