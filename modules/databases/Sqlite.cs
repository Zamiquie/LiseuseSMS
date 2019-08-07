using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.SQLite.Linq;

namespace LiseuseSMS
{
    class Sqlite
    {
        //creation de la connection
        private SQLiteConnection connexion { get; set; }
        // creation de l'objet commande(envois request)
        private SQLiteCommand commande { get; set; }
        // creation de l'objet reader
       


        //ListeNumber
        private List<object> telNameContactAll { get; set; }

        //ListSMS
        private List<object> smsContact { get; set; }


        public Sqlite()
        {
            connexion = new SQLiteConnection("Data Source=telephone.sqlite3;version=3;");
            connexion.Open();
            //Create Objet commande
            commande = connexion.CreateCommand();

            //Var de recuperation des nom contact
            telNameContactAll = new List<object>();

            //var de recuperation des sms
            smsContact = new List<object>();

           
        }

        public void CreateTable(string nomTable)
        {
            //Creation Statment 
            commande.CommandText = "CREATE TABLE " + nomTable + "(" +
                "id integer primary key," +
                "text vachar(100));";
            //Execute Statment 
            Console.WriteLine(commande.ExecuteNonQuery());
        }

        // return Tous les contacts enregistrées
        public List<object> AllNameContact()
        {         
            commande.CommandText = "SELECT Contact FROM tb_telephone ORDER BY Contact asc";

           var reader = commande.ExecuteReader();

            while (reader.Read())
            {

                telNameContactAll.Add(reader.GetValue(0));
            }
            reader.Close();
            return telNameContactAll;
        }
        
        //return numero correspondant au nomContact
        public string ReadContactName(string nameContact)
        {
            commande.CommandText = "SELECT telephone FROM tb_telephone where Contact='" + nameContact + "';";
             var reader = commande.ExecuteReader();

            while (reader.Read())
            {
                object nameContacts = reader.GetValue(0);
                if (nameContact != "")
                {
                    reader.Close();
                    return nameContacts.ToString();
                }
                reader.Close();
                return "Numero Inconnu";

            }
            reader.Close();
            return "Numero Inconnu";


        }

        
        public List<object> ReadSMSByContact(string contact)
        {
            using (SQLiteCommand db = new SQLiteCommand(connexion))
            {
                db.CommandText = "SELECT contenu " +
                    "FROM tb_sms " +
                    "WHERE expediteur= @contact or destinataire=@contact  " +
                    "ORDER BY ID desc;";

                db.Parameters.AddWithValue("@contact",contact);

                var SMSReader = db.ExecuteReader();
                
                
                while (SMSReader.Read())
                {
                    smsContact.Add(Convert.ToString(SMSReader.GetValue(0)));

                }
               SMSReader.Close();

               return smsContact;
            }
            
        }

        // Enregistrement de tous les sms e,voyés et recus
        public void SmsFromApi (Message[] messages)
        {
            //creation d'une transaction
            using (SQLiteTransaction tr = connexion.BeginTransaction())
            {

                using (SQLiteCommand sql = new SQLiteCommand(connexion))
                {
                    sql.CommandText = "Delete from tb_sms";
                    sql.ExecuteNonQuery();
                    foreach (Message message in messages)
                    {
                        sql.CommandText = @"INSERT INTO tb_sms ('Contenu', 'expediteur', 'destinataire', 'send') VALUES (@body,@de,@a,@date);";
                        sql.Parameters.AddWithValue("@body", message.body);
                        sql.Parameters.AddWithValue("@de", message.from);
                        sql.Parameters.AddWithValue("@a", message.to);
                        sql.Parameters.AddWithValue("@date", message.date_sent);

                        Console.WriteLine();
                        sql.ExecuteNonQuery();
                    }
                }
                tr.Commit();
                Console.WriteLine("C'est fait");
            }
        }

    }
}
