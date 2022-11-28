using CRUD_Mongo_Biblioteca.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_Mongo_Biblioteca.Utilitarios
{
    public class MenuAtualizar
    {
        private LivroController livro = new LivroController();
        private LeitorController leitor = new LeitorController();
        private AluguelController aluguel = new AluguelController();
        private LivroAluguelController itemAluguel = new LivroAluguelController();

        public void MenuAtualizacoes()
        {
            int opcao = 0;
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("Digite a opção desejada: (Somente o número da opção)");
                Console.WriteLine("1 - Atualizar Livros cadastrados");
                Console.WriteLine("2 - Atualizar Leitores cadastrados");
                Console.WriteLine("3 - Atualizar Aluguel");
                Console.WriteLine("4 - Atualizar Livros Alugados");
                Console.WriteLine("5 - Retornar ao menu principal");
                opcao = int.Parse(Console.ReadLine());

                switch (opcao)
                {
                    case 1:
                        livro.AlteraLivro();
                        Thread.Sleep(2000);
                        Console.WriteLine("Pressione qualquer tecla para limpar a tela e continuar");
                        Console.ReadKey();
                        break;
                    case 2:
                        leitor.AlteraLeitor();
                        Thread.Sleep(2000);
                        Console.WriteLine("Pressione qualquer tecla para limpar a tela e continuar");
                        Console.ReadKey();
                        break;
                    case 3:
                        aluguel.AlteraAluguel();
                        Thread.Sleep(2000);
                        Console.WriteLine("Pressione qualquer tecla para limpar a tela e continuar");
                        Console.ReadKey();
                        break;
                    case 4:
                        itemAluguel.AlteraLivroAluguel();
                        Thread.Sleep(2000);
                        Console.WriteLine("Pressione qualquer tecla para limpar a tela e continuar");
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
