using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace PSI
{
    public class Application
    {
        private AccesDonnes access;

        public static void Main(string[] args)
        {
            Application app = new Application();
            app.Demarrer();
        }

        private void Demarrer()
        {
            access = new AccesDonnes("SERVER=localhost;PORT=3306;DATABASE=liv_in_paris;UID=root;PASSWORD=root!");

            Affichage.InitialiserConsole();
            ExecuterMenuPrincipal();

            access.Terminer();
        }

        private void ExecuterMenuPrincipal()
        {
            bool continuer = true;
            while (continuer)
            {
                int choix = Affichage.AfficherMenuPrincipal();

                switch (choix)
                {
                    case 1:
                        AfficherClients();
                        break;
                    case 2:
                        AfficherClientsEntreprises();
                        break;
                    case 3:
                        AfficherCuisiniers();
                        break;
                    case 4:
                        AfficherPlats();
                        break;
                    case 5:
                        AjouterClientP();
                        break;
                    case 6:
                        AjouterCommande();
                        break;
                    case 7:
                        AjouterClientE();
                        break;
                    case 8:
                        AjouterCuisinier();
                        break;
                    case 9:
                        Affichage.AfficherCommandes();
                        break;
                    case 10:
                        Affichage.AfficherStatistiquesLivraisons();
                        break;
                    case 11:
                        Affichage.AfficherCommandesParPeriode();
                        break;
                    case 12:
                        Affichage.AfficherMoyennePrixCommandes();
                        break;
                    case 13:
                        Affichage.AfficherMoyenneCompteClient();
                        break;
                    case 14:
                        Affichage.AfficherCommandesParClient();
                        break;
                    case 15:
                        continuer = false;
                        Affichage.AfficherMessageConfirmation("Application fermée");
                        break;
                }
            }
        }

        private void AfficherClients()
        {
            try
            {
                var clients = new List<string>();
                using var cmd = new MySqlCommand("SELECT nom, prénom FROM Client_P", access.GetConnection());
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    clients.Add($"{reader["prénom"]} {reader["nom"]}");
                }

                Affichage.AfficherListe(clients, "LISTE DES CLIENTS PARTICULIERS");
            }
            catch (Exception ex)
            {
                Affichage.AfficherErreur(ex.Message);
            }
        }

        private void AfficherClientsEntreprises()
        {
            try
            {
                var clients = new List<string>();
                using var cmd = new MySqlCommand("SELECT nom_entreprise, adresse, referent FROM Client_E", access.GetConnection());
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    clients.Add($"{reader["nom_entreprise"]} - {reader["adresse"]} - Référent: {reader["referent"]}");
                }

                Affichage.AfficherListe(clients, "LISTE DES CLIENTS ENTREPRISES");
            }
            catch (Exception ex)
            {
                Affichage.AfficherErreur(ex.Message);
            }
        }

        private void AfficherCuisiniers()
        {
            try
            {
                var cuisiniers = new List<string>();
                using var cmd = new MySqlCommand("SELECT prénom, nom FROM Cuisinier", access.GetConnection());
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cuisiniers.Add($"{reader["prénom"]} {reader["nom"]}");
                }

                Affichage.AfficherListe(cuisiniers, "LISTE DES CUISINIERS");
            }
            catch (Exception ex)
            {
                Affichage.AfficherErreur(ex.Message);
            }
        }

        private void AfficherPlats()
        {
            try
            {
                var plats = new List<string>();
                using var cmd = new MySqlCommand(
                    "SELECT nom_plat, prix_pp FROM Plat",
                    access.GetConnection());

                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    plats.Add($"{reader["nom_plat"]} - {reader["prix_pp"]}€");
                }

                Affichage.AfficherListe(plats, "PLATS DU JOUR");
            }
            catch (Exception ex)
            {
                Affichage.AfficherErreur(ex.Message);
            }
        }

        private void AjouterClientP()
        {
            try
            {
                var (nom, prenom, adresse, email, codePostal, ville, idUtilisateur) = Affichage.AfficherFormulaireAjoutClientP();

                if (!UtilisateurExiste(idUtilisateur))
                {
                    AjouterUtilisateur(idUtilisateur);
                }

                using var cmd = new MySqlCommand(
                    "INSERT INTO Client_P (telephone_P, nom, prénom, adresse, email, code_postal, ville, id_utilisateur) VALUES (@telephone_P, @nom, @prenom, @adresse, @email, @codePostal, @ville, @idUtilisateur)",
                    access.GetConnection());
                cmd.Parameters.AddWithValue("@telephone_P", Guid.NewGuid().ToString());
                cmd.Parameters.AddWithValue("@nom", nom);
                cmd.Parameters.AddWithValue("@prenom", prenom);
                cmd.Parameters.AddWithValue("@adresse", adresse);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@codePostal", codePostal);
                cmd.Parameters.AddWithValue("@ville", ville);
                cmd.Parameters.AddWithValue("@idUtilisateur", idUtilisateur);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Affichage.AfficherMessageConfirmation("Client particulier ajouté avec succès.");
                }
                else
                {
                    Affichage.AfficherErreur("Erreur lors de l'ajout du client particulier.");
                }
            }
            catch (Exception ex)
            {
                Affichage.AfficherErreur(ex.Message);
            }
        }

        private bool UtilisateurExiste(string idUtilisateur)
        {
            try
            {
                using var cmd = new MySqlCommand(
                    "SELECT COUNT(*) FROM Utilisateur WHERE id_utilisateur = @idUtilisateur",
                    access.GetConnection());
                cmd.Parameters.AddWithValue("@idUtilisateur", idUtilisateur);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
            catch (Exception ex)
            {
                Affichage.AfficherErreur(ex.Message);
                return false;
            }
        }

        private void AjouterUtilisateur(string idUtilisateur)
        {
            try
            {
                Console.Write("Mot de passe pour le nouvel utilisateur : ");
                string motDePasse = Console.ReadLine();

                using var cmd = new MySqlCommand(
                    "INSERT INTO Utilisateur (id_utilisateur, MDP) VALUES (@idUtilisateur, @MDP)",
                    access.GetConnection());
                cmd.Parameters.AddWithValue("@idUtilisateur", idUtilisateur);
                cmd.Parameters.AddWithValue("@MDP", motDePasse);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Affichage.AfficherMessageConfirmation("Utilisateur ajouté avec succès.");
                }
                else
                {
                    Affichage.AfficherErreur("Erreur lors de l'ajout de l'utilisateur.");
                }
            }
            catch (Exception ex)
            {
                Affichage.AfficherErreur(ex.Message);
            }
        }

        private void AjouterCommande()
        {
            try
            {
                var (idClient, nomPlat, quantite, prix) = Affichage.AfficherFormulaireAjoutCommande();

                string idPlat = ObtenirIdPlat(nomPlat);
                if (string.IsNullOrEmpty(idPlat))
                {
                    idPlat = AjouterPlat(nomPlat, prix);
                }

                string idCommande = CreerCommande();

                using var cmd = new MySqlCommand(
                    "INSERT INTO Ligne_Commande (id_livraison, date_livraison, idplat) VALUES (@idLivraison, @dateLivraison, @idPlat)",
                    access.GetConnection());
                cmd.Parameters.AddWithValue("@idLivraison", Guid.NewGuid().ToString());
                cmd.Parameters.AddWithValue("@dateLivraison", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@idPlat", idPlat);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Affichage.AfficherMessageConfirmation("Commande ajoutée avec succès.");
                }
                else
                {
                    Affichage.AfficherErreur("Erreur lors de l'ajout de la commande.");
                }
            }
            catch (Exception ex)
            {
                Affichage.AfficherErreur(ex.Message);
            }
        }

        private string ObtenirIdPlat(string nomPlat)
        {
            try
            {
                using var cmd = new MySqlCommand(
                    "SELECT idPlat FROM Plat WHERE nom_plat = @nomPlat",
                    access.GetConnection());
                cmd.Parameters.AddWithValue("@nomPlat", nomPlat);

                var result = cmd.ExecuteScalar();
                return result?.ToString() ?? string.Empty;
            }
            catch (Exception ex)
            {
                Affichage.AfficherErreur(ex.Message);
                return string.Empty;
            }
        }

        private string AjouterPlat(string nomPlat, float prix)
        {
            try
            {
                string idPlat = Guid.NewGuid().ToString();

                using var cmd = new MySqlCommand(
                    "INSERT INTO Plat (idPlat, nom_plat, prix_pp) VALUES (@idPlat, @nomPlat, @prix)",
                    access.GetConnection());
                cmd.Parameters.AddWithValue("@idPlat", idPlat);
                cmd.Parameters.AddWithValue("@nomPlat", nomPlat);
                cmd.Parameters.AddWithValue("@prix", prix);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return idPlat;
                }
                else
                {
                    Affichage.AfficherErreur("Erreur lors de l'ajout du plat.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Affichage.AfficherErreur(ex.Message);
                return null;
            }
        }

        private string CreerCommande()
        {
            try
            {
                using var cmd = new MySqlCommand(
                    "INSERT INTO Commande (id_commande, date_commande) VALUES (@idCommande, @dateCommande)",
                    access.GetConnection());
                string idCommande = Guid.NewGuid().ToString();
                cmd.Parameters.AddWithValue("@idCommande", idCommande);
                cmd.Parameters.AddWithValue("@dateCommande", DateTime.Now.ToString("yyyy-MM-dd"));

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return idCommande;
                }
                else
                {
                    Affichage.AfficherErreur("Erreur lors de la création de la commande.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Affichage.AfficherErreur(ex.Message);
                return null;
            }
        }

        private void AjouterClientE()
        {
            try
            {
                var (idEntreprise, nom, adresse, referent, idUtilisateur) = Affichage.AfficherFormulaireAjoutClientE();

                if (!UtilisateurExiste(idUtilisateur))
                {
                    AjouterUtilisateur(idUtilisateur);
                }

                using var cmd = new MySqlCommand(
                    "INSERT INTO Client_E (id_entreprise, nom_entreprise, adresse, referent, id_utilisateur) VALUES (@idEntreprise, @nom, @adresse, @referent, @idUtilisateur)",
                    access.GetConnection());
                cmd.Parameters.AddWithValue("@idEntreprise", idEntreprise);
                cmd.Parameters.AddWithValue("@nom", nom);
                cmd.Parameters.AddWithValue("@adresse", adresse);
                cmd.Parameters.AddWithValue("@referent", referent);
                cmd.Parameters.AddWithValue("@idUtilisateur", idUtilisateur);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Affichage.AfficherMessageConfirmation("Client entreprise ajouté avec succès.");
                }
                else
                {
                    Affichage.AfficherErreur("Erreur lors de l'ajout du client entreprise.");
                }
            }
            catch (Exception ex)
            {
                Affichage.AfficherErreur(ex.Message);
            }
        }

        private void AjouterCuisinier()
        {
            try
            {
                var (telephone, nom, prenom, adresse, email, idUtilisateur) = Affichage.AfficherFormulaireAjoutCuisinier();

                if (!UtilisateurExiste(idUtilisateur))
                {
                    AjouterUtilisateur(idUtilisateur);
                }

                using var cmd = new MySqlCommand(
                    "INSERT INTO Cuisinier (telephone_C, nom, prénom, adresse, email, id_utilisateur) VALUES (@telephone, @nom, @prenom, @adresse, @email, @idUtilisateur)",
                    access.GetConnection());
                cmd.Parameters.AddWithValue("@telephone", telephone);
                cmd.Parameters.AddWithValue("@nom", nom);
                cmd.Parameters.AddWithValue("@prenom", prenom);
                cmd.Parameters.AddWithValue("@adresse", adresse);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@idUtilisateur", idUtilisateur);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Affichage.AfficherMessageConfirmation("Cuisinier ajouté avec succès.");
                }
                else
                {
                    Affichage.AfficherErreur("Erreur lors de l'ajout du cuisinier.");
                }
            }
            catch (Exception ex)
            {
                Affichage.AfficherErreur(ex.Message);
            }
        }
    }
}
