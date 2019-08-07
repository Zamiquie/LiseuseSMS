using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LiseuseSMS
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        TwillioSMS sms = new TwillioSMS();
        string numTo; // Numero destinataire  
        private List<string> ListeSMS = new List<string>();
        Sqlite database = new Sqlite(); 

        public MainWindow()
        {
            InitializeComponent();
            //Rafraichissement des new messages en debut de lancement 
            Thread tr = new Thread (() =>database.SmsFromApi(ApiTwillio.CallApiTwilio()));
            tr.Start();
            

            //creation des ComboBox de tous les num de la db
            foreach (object name in database.AllNameContact())
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = name.ToString();
                tel.Items.Add(item);

                //Listage des numeros pour lecture sms
                ListBoxItem itembox= new ListBoxItem();
                itembox.Content = name.ToString();
                ReadSMS.Items.Add(itembox);
                
            }


        }

        private void click_sendSMS(object sender, RoutedEventArgs e)
        {
            if (message.Text != "")
            {
                sms.numTo = database.ReadContactName(numTo);
                sms.sendSMS(message.Text);
                SMSok.Visibility = Visibility.Visible;
                noMessage.Visibility = Visibility.Hidden;
                message.Text = "";
            }
            else
            {
                noMessage.Visibility = Visibility.Visible;
                SMSok.Visibility = Visibility.Hidden;
            }
        }

        private void DisplayDestinataire(object sender, SelectionChangedEventArgs args)
        {
            ComboBoxItem tel = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            numTo = tel.Content.ToString();
            corpus.Text = "Corps du SMS\nà destination de \n " + database.ReadContactName(numTo);

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem contactTel = ((sender as ListBox).SelectedItem as ListBoxItem);

            // tri par tel contact
            DockLecture.Children.Clear();

            foreach (string sms in database.ReadSMSByContact(database.ReadContactName(contactTel.Content.ToString())))
            {
                ListeSMS.Add(sms);
            }
            
            foreach (string oneSms in ListeSMS)
            {
                //border sms
                Border border = new Border();
                border.Background = Brushes.BlueViolet;
                border.BorderThickness = new Thickness(1);
                border.BorderBrush = Brushes.Black;
               
               

                //contenu sms
                TextBlock text = new TextBlock();
                text.Text = oneSms;
                text.TextWrapping = TextWrapping.Wrap;

                //ajout sms to border 
                border.Child = text;
               
                //ajout dock panel
                DockPanel.SetDock(border, Dock.Top);
                DockLecture.Children.Add(border);
                

            }
            ListeSMS.Clear();
            
        }

      
    }
}
