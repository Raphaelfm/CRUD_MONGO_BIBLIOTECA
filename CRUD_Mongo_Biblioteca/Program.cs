using CRUD_Mongo_Biblioteca.Conexao;
using CRUD_Mongo_Biblioteca.Model;
using CRUD_Mongo_Biblioteca.Controller;
using CRUD_Mongo_Biblioteca.Utilitarios;

//Chamada das classes
SplashScreen splashScreen = new SplashScreen();
Menu menu = new Menu();

splashScreen.Splash();

Thread.Sleep(5000);
Console.Clear();

menu.MenuPrincipal();

