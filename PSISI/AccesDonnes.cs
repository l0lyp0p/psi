using MySql.Data.MySqlClient;

namespace PSISI
{
    public class AccesDonnes
    {
        private MySqlConnection connection;

        public AccesDonnes(string connectionString)
        {
            connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                Console.WriteLine("Connexion à la base de données réussie.");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Erreur de connexion : " + ex.Message);
            }
        }

        public MySqlConnection GetConnection()
        {
            return connection;
        }

        public void Terminer()
        {
            if (connection != null)
            {
                connection.Close();
                Console.WriteLine("Connexion fermée.");
            }
        }
    }
}
