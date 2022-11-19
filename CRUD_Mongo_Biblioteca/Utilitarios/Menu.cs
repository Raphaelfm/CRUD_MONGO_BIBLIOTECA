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
            Console.WriteLine("Digite a opção desejada: (Somente o número da opção)");
            Console.WriteLine();
            Console.WriteLine("1 - Relatórios");
            Console.WriteLine("2 - Inserir documentos");
            Console.WriteLine("3 - Remover documentos");
            Console.WriteLine("4 - Atualizar documentos");
            Console.WriteLine("5 - Sair");
        }
        
    }
}
