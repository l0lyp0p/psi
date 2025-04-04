using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace PSI
{
    public static class Affichage
    {
        static ConsoleColor CouleurTitre = ConsoleColor.Cyan;
        static ConsoleColor CouleurMenu = ConsoleColor.White;
        static ConsoleColor CouleurSelection = ConsoleColor.Green;
        static ConsoleColor CouleurErreur = ConsoleColor.Red;

        public static (int choix, string id, string mdp) AfficherEcranConnexion()
        {
            Console.Clear();
            AfficherEntete("LIV_IN PARIS - CONNEXION");

            Console.WriteLine("1. Se connecter");
            Console.WriteLine("2. Mode test");
            Console.WriteLine("3. Quitter");
            Console.Write("Choix : ");

            string choix = Console.ReadLine();

            if (choix == "1")
            {
                Console.Write("\nIdentifiant : ");
                string id = Console.ReadLine();
                Console.Write("Mot de passe : ");
                string mdp = Console.ReadLine();
                return (1, id, mdp);
            }

            return (choix == "2" ? 2 : 3, null, null);
        }

        public static int AfficherMenuAdmin()
        {
            return AfficherMenuInteractif("MENU ADMIN", new List<string>
            {
                "Gérer les utilisateurs",
                "Voir toutes les commandes",
                "Retour"
            });
        }

        public static void InitialiserConsole()
        {
            Console.Title = "LIV_IN Paris - Gestion Culinaire";
            Console.CursorVisible = false;
        }

        public static int AfficherMenuPrincipal()
        {
            var options = new List<string>
            {
                "Liste des clients particuliers",
                "Liste des clients entreprises",
                "Liste des cuisiniers",
                "Plats du jour",
                "Ajouter un client particulier",
                "Ajouter une commande",
                "Ajouter un client entreprise",
                "Ajouter un cuisinier",
                "Simuler une commande",
                "Statistiques des livraisons",
                "Commandes par période",
                "Moyenne des prix des commandes",
                "Moyenne des comptes clients",
                "Commandes par client",
                "Quitter l'application"
            };

            return AfficherMenuInteractif("MENU PRINCIPAL", options);
        }

        public static void AfficherEntete(string titre)
        {
            Console.Clear();
            Console.ForegroundColor = CouleurTitre;
            DessinerLigne(50);
            CentrerTexte(titre.ToUpper(), 50);
            DessinerLigne(50);
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void AfficherListe<T>(List<T> elements, string titre, bool avecNumerotation = true)
        {
            AfficherEntete(titre);

            Console.ForegroundColor = CouleurMenu;
            for (int i = 0; i < elements.Count; i++)
            {
                Console.WriteLine($"{(avecNumerotation ? $"{i + 1}. " : "")}{elements[i]}");
            }
            Console.ResetColor();

            AttendreInteraction();
        }

        public static void AfficherMessageConfirmation(string message)
        {
            Console.ForegroundColor = CouleurSelection;
            Console.WriteLine($"\n✓ {message}");
            Console.ResetColor();
            AttendreInteraction();
        }

        public static void AfficherErreur(string message)
        {
            Console.ForegroundColor = CouleurErreur;
            Console.WriteLine($"\n⚠ ERREUR : {message}");
            Console.ResetColor();
            AttendreInteraction();
        }

        private static int AfficherMenuInteractif(string titre, List<string> options)
        {
            int selection = 0;
            ConsoleKey key;

            do
            {
                AfficherEntete(titre);

                for (int i = 0; i < options.Count; i++)
                {
                    Console.ForegroundColor = i == selection ? CouleurSelection : CouleurMenu;
                    Console.WriteLine($"{(i == selection ? "→ " : "  ")}{options[i]}");
                }

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.DownArrow:
                        selection = (selection + 1) % options.Count;
                        break;
                    case ConsoleKey.UpArrow:
                        selection = (selection - 1 + options.Count) % options.Count;
                        break;
                }

            } while (key != ConsoleKey.Enter);

            Console.ResetColor();
            return selection + 1;
        }

        private static void CentrerTexte(string texte, int largeur)
        {
            int espaces = (largeur - texte.Length) / 2;
            Console.WriteLine(new string(' ', Math.Max(0, espaces)) + texte);
        }

        private static void DessinerLigne(int longueur)
        {
            Console.WriteLine(new string('═', longueur));
        }

        public static void AttendreInteraction()
        {
            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey(true);
        }

        public static (string nom, string prenom, string adresse, string email, string codePostal, string ville, string idUtilisateur) AfficherFormulaireAjoutClientP()
        {
            Console.Clear();
            AfficherEntete("AJOUTER UN CLIENT PARTICULIER");

            Console.Write("Nom : ");
            string nom = Console.ReadLine();
            Console.Write("Prénom : ");
            string prenom = Console.ReadLine();
            Console.Write("Adresse : ");
            string adresse = Console.ReadLine();
            Console.Write("Email : ");
            string email = Console.ReadLine();
            Console.Write("Code Postal : ");
            string codePostal = Console.ReadLine();
            Console.Write("Ville : ");
            string ville = Console.ReadLine();
            Console.Write("ID Utilisateur : ");
            string idUtilisateur = Console.ReadLine();

            return (nom, prenom, adresse, email, codePostal, ville, idUtilisateur);
        }

        public static (string idClient, string nomPlat, string quantite, float prix) AfficherFormulaireAjoutCommande()
        {
            Console.Clear();
            AfficherEntete("AJOUTER UNE COMMANDE");

            Console.Write("ID du Client : ");
            string idClient = Console.ReadLine();

            Console.Write("Nom du Plat : ");
            string nomPlat = Console.ReadLine();

            Console.Write("Quantité (Petite, Moyenne, Grande) : ");
            string quantite = Console.ReadLine();

            // Vérifiez que la quantité est l'une des valeurs acceptées
            while (!new[] { "Petite", "Moyenne", "Grande" }.Contains(quantite))
            {
                Console.WriteLine("Quantité invalide. Veuillez entrer 'Petite', 'Moyenne', ou 'Grande'.");
                Console.Write("Quantité : ");
                quantite = Console.ReadLine();
            }

            // Demandez le prix uniquement si le plat n'existe pas déjà
            Console.Write("Prix du plat : ");
            string prixInput = Console.ReadLine();
            float prix;
            if (!float.TryParse(prixInput, out prix))
            {
                Console.WriteLine("Prix invalide. Veuillez entrer un nombre valide.");
                prix = 0; // ou toute autre valeur par défaut
            }

            return (idClient, nomPlat, quantite, prix);
        }

        public static (string idEntreprise, string nom, string adresse, string referent, string idUtilisateur) AfficherFormulaireAjoutClientE()
        {
            Console.Clear();
            AfficherEntete("AJOUTER UN CLIENT ENTREPRISE");

            Console.Write("ID de l'entreprise : ");
            string idEntreprise = Console.ReadLine();
            Console.Write("Nom de l'entreprise : ");
            string nom = Console.ReadLine();
            Console.Write("Adresse : ");
            string adresse = Console.ReadLine();
            Console.Write("Référent : ");
            string referent = Console.ReadLine();
            Console.Write("ID Utilisateur : ");
            string idUtilisateur = Console.ReadLine();

            return (idEntreprise, nom, adresse, referent, idUtilisateur);
        }

        public static (string telephone, string nom, string prenom, string adresse, string email, string idUtilisateur) AfficherFormulaireAjoutCuisinier()
        {
            Console.Clear();
            AfficherEntete("AJOUTER UN CUISINIER");

            Console.Write("Téléphone : ");
            string telephone = Console.ReadLine();
            Console.Write("Nom : ");
            string nom = Console.ReadLine();
            Console.Write("Prénom : ");
            string prenom = Console.ReadLine();
            Console.Write("Adresse : ");
            string adresse = Console.ReadLine();
            Console.Write("Email : ");
            string email = Console.ReadLine();
            Console.Write("ID Utilisateur : ");
            string idUtilisateur = Console.ReadLine();

            return (telephone, nom, prenom, adresse, email, idUtilisateur);
        }

        public static void AfficherCommandes()
        {
            Console.Clear();
            AfficherEntete("SIMULATION DE COMMANDE");

            // Simuler la création d'une commande
            Console.WriteLine("Simulation de la création d'une commande...");

            // Calcul du prix de la commande
            float prixCommande = CalculerPrixCommande();
            Console.WriteLine($"Prix de la commande : {prixCommande}€");

            // Calcul du chemin le plus court
            string chemin = CalculerCheminPlusCourt();
            Console.WriteLine($"Chemin le plus court pour la livraison : {chemin}");

            Affichage.AttendreInteraction();
        }

        private static float CalculerPrixCommande()
        {
            // Logique de calcul du prix de la commande
            // Ici, nous simulons un prix fixe
            return 25.50f;
        }

        private static string CalculerCheminPlusCourt()
        {
            // Logique de calcul du chemin le plus court
            // Ici, nous simulons un chemin fixe
            return "Chemin A -> B -> C";
        }

        public static void AfficherStatistiquesLivraisons()
        {
            Console.Clear();
            AfficherEntete("STATISTIQUES DES LIVRAISONS");

            // Récupérer et afficher les livraisons par cuisinier
            var livraisonsParCuisinier = ObtenirLivraisonsParCuisinier();
            foreach (var livraison in livraisonsParCuisinier)
            {
                Console.WriteLine($"{livraison.Key} : {livraison.Value} livraisons");
            }

            Affichage.AttendreInteraction();
        }

        private static Dictionary<string, int> ObtenirLivraisonsParCuisinier()
        {
            var livraisonsParCuisinier = new Dictionary<string, int>();

            try
            {
                using var connection = new MySqlConnection("SERVER=localhost;PORT=3306;DATABASE=liv_in_paris;UID=root;PASSWORD=root");
                connection.Open();

                using var cmd = new MySqlCommand(
                    @"SELECT CONCAT(c.nom, ' ', c.prénom) AS nom_cuisinier, COUNT(*) AS nombre_livraisons
                      FROM Ligne_Commande lc
                      JOIN Plat p ON lc.idplat = p.idPlat
                      JOIN Cuisinier c ON p.telephone_C = c.telephone_C
                      GROUP BY c.nom, c.prénom",
                    connection);

                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string nomCuisinier = reader["nom_cuisinier"].ToString();
                    int nombreLivraisons = reader.GetInt32("nombre_livraisons");
                    livraisonsParCuisinier[nomCuisinier] = nombreLivraisons;
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                Affichage.AfficherErreur(ex.Message);
            }

            return livraisonsParCuisinier;
        }

        public static void AfficherCommandesParPeriode()
        {
            Console.Clear();
            AfficherEntete("COMMANDES PAR PÉRIODE");

            Console.Write("Entrez la date de début (AAAA-MM-JJ): ");
            string dateDebut = Console.ReadLine();
            Console.Write("Entrez la date de fin (AAAA-MM-JJ): ");
            string dateFin = Console.ReadLine();

            // Logique pour afficher les commandes par période
            var commandes = ObtenirCommandesParPeriode(dateDebut, dateFin);
            AfficherListe(commandes, "Commandes par période");
        }

        private static List<string> ObtenirCommandesParPeriode(string dateDebut, string dateFin)
        {
            var commandes = new List<string>();

            try
            {
                using var connection = new MySqlConnection("SERVER=localhost;PORT=3306;DATABASE=liv_in_paris;UID=root;PASSWORD=root");
                connection.Open();

                using var cmd = new MySqlCommand(
                    "SELECT id_commande, date_commande FROM Commande WHERE date_commande BETWEEN @dateDebut AND @dateFin",
                    connection);
                cmd.Parameters.AddWithValue("@dateDebut", dateDebut);
                cmd.Parameters.AddWithValue("@dateFin", dateFin);

                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string idCommande = reader["id_commande"].ToString();
                    string dateCommande = reader["date_commande"].ToString();
                    commandes.Add($"Commande ID: {idCommande}, Date: {dateCommande}");
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                Affichage.AfficherErreur(ex.Message);
            }

            return commandes;
        }

        public static void AfficherMoyennePrixCommandes()
        {
            Console.Clear();
            AfficherEntete("MOYENNE DES PRIX DES COMMANDES");

            // Logique pour calculer et afficher la moyenne des prix des commandes
            double moyennePrix = CalculerMoyennePrixCommandes();
            Console.WriteLine($"Moyenne des prix des commandes : {moyennePrix}€");

            AttendreInteraction();
        }

        private static double CalculerMoyennePrixCommandes()
        {
            double totalPrix = 0;
            int nombreCommandes = 0;

            try
            {
                using var connection = new MySqlConnection("SERVER=localhost;PORT=3306;DATABASE=liv_in_paris;UID=root;PASSWORD=root");
                connection.Open();

                using var cmd = new MySqlCommand(
                    "SELECT AVG(prix_pp) AS moyenne_prix FROM Plat WHERE idPlat IN (SELECT idPlat FROM Ligne_Commande)",
                    connection);

                var result = cmd.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    return Convert.ToDouble(result);
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                Affichage.AfficherErreur(ex.Message);
            }

            return 0;
        }

        public static void AfficherMoyenneCompteClient()
        {
            Console.Clear();
            AfficherEntete("MOYENNE DES COMPTES CLIENTS");

            // Logique pour calculer et afficher la moyenne des comptes clients
            double moyenneCompte = CalculerMoyenneCompteClient();
            Console.WriteLine($"Moyenne des comptes clients : {moyenneCompte}€");

            AttendreInteraction();
        }

        private static double CalculerMoyenneCompteClient()
        {
            double totalCompte = 0;
            int nombreClients = 0;

            try
            {
                using var connection = new MySqlConnection("SERVER=localhost;PORT=3306;DATABASE=liv_in_paris;UID=root;PASSWORD=root");
                connection.Open();

                using var cmd = new MySqlCommand(
                    "SELECT AVG(prix_pp) AS moyenne_compte FROM Commande",
                    connection);

                var result = cmd.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    return Convert.ToDouble(result);
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                Affichage.AfficherErreur(ex.Message);
            }

            return 0;
        }

        public static void AfficherCommandesParClient()
        {
            Console.Clear();
            AfficherEntete("COMMANDES PAR CLIENT");

            Console.Write("Entrez l'ID du client : ");
            string idClient = Console.ReadLine();

            // Logique pour afficher les commandes par client
            var commandes = ObtenirCommandesParClient(idClient);
            AfficherListe(commandes, "Commandes par client");
        }

        private static List<string> ObtenirCommandesParClient(string idClient)
        {
            var commandes = new List<string>();

            try
            {
                using var connection = new MySqlConnection("SERVER=localhost;PORT=3306;DATABASE=liv_in_paris;UID=root;PASSWORD=root");
                connection.Open();

                using var cmd = new MySqlCommand(
                    "SELECT id_commande, date_commande FROM Commande WHERE id_commande IN (SELECT id_commande FROM Est_Composee WHERE id_livraison IN (SELECT id_livraison FROM Livraison_P WHERE telephone_P = @idClient))",
                    connection);
                cmd.Parameters.AddWithValue("@idClient", idClient);

                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string idCommande = reader["id_commande"].ToString();
                    string dateCommande = reader["date_commande"].ToString();
                    commandes.Add($"Commande ID: {idCommande}, Date: {dateCommande}");
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                Affichage.AfficherErreur(ex.Message);
            }

            return commandes;
        }
    }
}
