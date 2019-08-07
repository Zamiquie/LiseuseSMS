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
       private SQLiteDataReader reader { get; set;}


       //ListeNumber
       private List<object> telNameContactAll { get; set; }
    

        public Sqlite()
        {
             connexion = new SQLiteConnection("Data Source=telephone.sqlite3;version=3;");
             //open the connetion
             connexion.Open();
            
            //Create Objet commande
             commande = connexion.CreateCommand();
           

            telNameContactAll = new List<object>();
        }

       public void CreateTable(string nomTable)
        {
            //Creation Statment 
            commande.CommandText = "CREATE TABLE "+nomTable+"(" +
                "id integer primary key," +
                "text vachar(100));";
            //Execute Statment 
            Console.WriteLine(commande.ExecuteNonQuery());
        }

       public List<object> AllNameContact()
        {
            commande.CommandText = "SELECT Contact FROM tb_telephone";

            reader = commande.ExecuteReader();

            while (reader.Read())
            {
              
                telNameContactAll.Add(reader.GetValue(0)); 
            }
            reader.Close();
            return telNameContactAll; 
        }
       public string ReadContactName(string nameContact)
        {

            commande.CommandText = "SELECT telephone FROM tb_telephone where Contact='"+nameContact+"';";

            reader = commande.ExecuteReader();
           
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

       public void RegisterSMS(string sms,string tiers)
        {
            //
            using (SQLiteCommand sql = new SQLiteCommand("INSERT INTO tb_sms (Contenu,TelephoneTiers) VALUES(@bodysms,@tiers);", connexion)) {
                sql.Parameters.AddWithValue("@bodysms", sms);
                sql.Parameters.AddWithValue("@tiers", tiers);
                sql.CommandText = "INSERT INTO tb_sms (Contenu,TelephoneTiers) VALUES(@bodysms,@tiers);";
                sql.ExecuteNonQuery();

            }
            
          

        }
        
    }
}
