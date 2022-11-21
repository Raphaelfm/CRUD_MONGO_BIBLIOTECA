using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD_Mongo_Biblioteca.Controller;

namespace CRUD_Mongo_Biblioteca.Utilitarios
{
    public class MenuInserir
    {
        private LivroController livro = new LivroController();
        private LeitorController leitor = new LeitorController();
        private AluguelController aluguel = new AluguelController();
        private LivroAluguelController itemAluguel = new LivroAluguelController();

        public void MenuInsere()
        {
            int opcao = 0;
            bool running = true;

            while(running)
            {
                Console.Clear();
                Console.WriteLine("Digite a opção desejada: (Somente o número da opção)");
                Console.WriteLine("1 - Cadastrar Livros");
                Console.WriteLine("2 - Cadastrar Leitor");
                Console.WriteLine("3 - Inserir Aluguel");
                Console.WriteLine("5 - Retornar ao menu principal");
                opcao = int.Parse(Console.ReadLine());

                switch(opcao)
                {
                    case 1:
                        livro.CadastrarLivro();
                        break;
                    case 2:
                        leitor.CadastrarLeitor();
                        break;
                    case 3:
                        aluguel.CadastrarAluguel();
                        itemAluguel.CadastrarLivroAluguel();

                        break;                    
                    case 5:
                        Console.WriteLine("Retornando ao menu principal...");
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Opção inválida, por favor digite a opção desejada conforme descrito no menu de opções.");
                        break;
                }
            }            
        }
    }
}
