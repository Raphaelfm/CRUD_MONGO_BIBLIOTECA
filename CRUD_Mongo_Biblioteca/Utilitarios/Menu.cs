using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_Mongo_Biblioteca.Utilitarios
{
    public class Menu
    {
        public void MenuPrincipal()
        {
            MenuInserir inserir = new MenuInserir();
            MenuRelatorios relatorios = new MenuRelatorios();
            MenuRemover remover = new MenuRemover();
            int opcao = 0;
            bool running = true;

            while (running)
            {
                Console.WriteLine("Digite a opção desejada: (Somente o número da opção)");
                Console.WriteLine();
                Console.WriteLine("1 - Relatórios");
                Console.WriteLine("2 - Inserir documentos");
                Console.WriteLine("3 - Remover documentos");
                Console.WriteLine("4 - Atualizar documentos");
                Console.WriteLine("5 - Sair");

                opcao = int.Parse(Console.ReadLine());

                switch (opcao)
                {
                    case 1:
                        Console.WriteLine("Listando opções de Relatórios...");
                        relatorios.MenuRelatorio();
                        break;
                    case 2:
                        Console.WriteLine("Bem vindo ao cadastro de documentos...");
                        inserir.MenuInsere();
                        break;
                    case 3:
                        Console.WriteLine("Informe o que deseja excluir...");
                        remover.MenuRemoverDados();
                        break;
                    case 4:
                        break;
                    case 5:
                        Console.WriteLine("Obrigado por utilizar nosso software...");
                        Console.WriteLine("Até a próxima...");
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
