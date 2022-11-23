using CRUD_Mongo_Biblioteca.Controller;
using CRUD_Mongo_Biblioteca.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_Mongo_Biblioteca.Utilitarios
{
    public class MenuRemover
    {
        private LivroController livro = new LivroController();
        private LeitorController leitor = new LeitorController();
        private AluguelController aluguel = new AluguelController();
        private LivroAluguelController itemAluguel = new LivroAluguelController();

        public void MenuRemoverDados()
        {
            int opcao = 0;
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("Digite a opção desejada: (Somente o número da opção)");
                Console.WriteLine("1 - Remover Livros cadastrados");
                Console.WriteLine("2 - Remover Leitores cadastrados");
                Console.WriteLine("3 - Remover Aluguel");
                Console.WriteLine("4 - Remover um livro de Aluguel");
                Console.WriteLine("5 - Retornar ao menu principal");
                opcao = int.Parse(Console.ReadLine());

                switch (opcao)
                {
                    case 1:
                        livro.RemoveLivro();
                        Thread.Sleep(2000);
                        Console.WriteLine("Pressione qualquer tecla para limpara a tela e continuar");
                        Console.ReadKey();
                        break;
                    case 2:
                        leitor.RemoverLeitores();
                        Thread.Sleep(2000);
                        Console.WriteLine("Pressione qualquer tecla para limpara a tela e continuar");
                        Console.ReadKey();
                        break;
                    case 3:
                        aluguel.RemoveAluguel();
                        Thread.Sleep(2000);
                        Console.WriteLine("Pressione qualquer tecla para limpara a tela e continuar");
                        Console.ReadKey();
                        break;
                    case 4:
                        Thread.Sleep(2000);
                        Console.WriteLine("Pressione qualquer tecla para limpara a tela e continuar");
                        Console.ReadKey();
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
