using CRUD_Mongo_Biblioteca.Controller;
using CRUD_Mongo_Biblioteca.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_Mongo_Biblioteca.Utilitarios
{
    public class MenuRelatorios
    {
        private LivroController livro = new LivroController();
        private LeitorController leitor = new LeitorController();
        private AluguelController aluguel = new AluguelController();
        private LivroAluguelController itemAluguel = new LivroAluguelController();

        public void MenuRelatorio()
        {
            int opcao = 0;
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("Digite a opção desejada: (Somente o número da opção)");
                Console.WriteLine("1 - Relatório de Livros cadastrados");
                Console.WriteLine("2 - Relatório de Leitores cadastrados");
                Console.WriteLine("3 - Relatório de Aluguel");
                Console.WriteLine("4 - Relatório de Livros Alugados");
                Console.WriteLine("5 - Retornar ao menu principal");
                opcao = int.Parse(Console.ReadLine());

                switch (opcao)
                {
                    case 1:
                        livro.RelatorioLivros();
                        Thread.Sleep(2000);
                        Console.WriteLine("Pressione qualquer tecla para limpara a tela e continuar");
                        Console.ReadKey();
                        break;
                    case 2:
                        leitor.RelatorioLeitores();
                        Thread.Sleep(2000);
                        Console.WriteLine("Pressione qualquer tecla para limpara a tela e continuar");
                        Console.ReadKey();
                        break;
                    case 3:
                        aluguel.RelatorioAlugueis();
                        Thread.Sleep(2000);
                        Console.WriteLine("Pressione qualquer tecla para limpara a tela e continuar");
                        Console.ReadKey();
                        break;
                    case 4:
                        itemAluguel.RelatorioLivrosAlugados();
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
