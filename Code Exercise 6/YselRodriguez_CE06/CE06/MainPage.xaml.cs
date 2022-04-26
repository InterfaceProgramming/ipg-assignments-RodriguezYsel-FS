using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CE06
{
    public partial class MainPage : ContentPage
    {
        Animal animal;
        Detail detail = new Detail();
        public MainPage()
        {
            InitializeComponent();

            //initialize animals list
            var animalList = new List<Animal>
            {
                new Animal(){Name = "Baboon", Image = "baboon.jpg", Description = "Baboons are primates comprising the genus Papio, one of the 23 genera of Old World monkeys. There are six species of baboon: the hamadryas baboon, the Guinea baboon, the olive baboon, the yellow baboon, the Kinda baboon and the chacma baboon." },
                new Animal(){Name = "Blue Monkey", Image = "bluemonkey.jpg", Description = "The blue monkey or diademed monkey is a species of Old World monkey native to Central and East Africa, ranging from the upper Congo River basin east to the East African Rift and south to northern Angola and Zambia. It sometimes includes Sykes', silver, and golden monkeys as subspecies" },
                new Animal(){Name = "Capuchin Monkey", Image = "capuchinmonkey.jpg", Description = "The capuchin monkeys are New World monkeys of the subfamily Cebinae. They are readily identified as the 'organ grinder' monkey, and have been used in many movies and television shows. The range of capuchin monkeys includes some tropical forests in Central America and South America as far south as northern Argentina." },
                new Animal(){Name = "Golden Lion Tamarin", Image = "goldenliontamarin.jpg", Description = "The golden lion tamarin, also known as the golden marmoset, is a small New World monkey of the family Callitrichidae. Native to the Atlantic coastal forests of Brazil, the golden lion tamarin is an endangered species." },
                new Animal(){Name = "Howler Monkey", Image = "howlermonkey.jpg", Description = "Howler monkeys are among the largest of the New World monkeys. They are famous for their loud howls, which can travel more than one mile through dense rain forest. These monkeys are native to South and Central American forests. Threats include human predation, habitat destruction, and capture for pets or zoo animals." },
                new Animal(){Name = "Squirrel Monkey", Image = "squirrelmonkey.jpg", Description = "Squirrel monkeys are New World monkeys of the genus Saimiri. Saimiri is the only genus in the subfamily Saimirinae. The name of the genus is of Tupi origin and was also used as an English name by early researchers. Squirrel monkeys live in the tropical forests of Central and South America in the canopy layer." },
                new Animal(){Name = "Baboon", Image = "baboon.jpg", Description = "Baboons are primates comprising the genus Papio, one of the 23 genera of Old World monkeys. There are six species of baboon: the hamadryas baboon, the Guinea baboon, the olive baboon, the yellow baboon, the Kinda baboon and the chacma baboon." },
                new Animal(){Name = "Blue Monkey", Image = "bluemonkey.jpg", Description = "The blue monkey or diademed monkey is a species of Old World monkey native to Central and East Africa, ranging from the upper Congo River basin east to the East African Rift and south to northern Angola and Zambia. It sometimes includes Sykes', silver, and golden monkeys as subspecies" },
                new Animal(){Name = "Capuchin Monkey", Image = "capuchinmonkey.jpg", Description = "The capuchin monkeys are New World monkeys of the subfamily Cebinae. They are readily identified as the 'organ grinder' monkey, and have been used in many movies and television shows. The range of capuchin monkeys includes some tropical forests in Central America and South America as far south as northern Argentina." },
                new Animal(){Name = "Golden Lion Tamarin", Image = "goldenliontamarin.jpg", Description = "The golden lion tamarin, also known as the golden marmoset, is a small New World monkey of the family Callitrichidae. Native to the Atlantic coastal forests of Brazil, the golden lion tamarin is an endangered species." },
            };

            carouselView.ItemsSource = animalList;
        }

        
        public void OnCurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            //current item in carousel has changed
            //set toolbar text to current animal
            try
            {
                animal = e.CurrentItem as Animal;
                toolbar.Text = animal.Name;
            }
            catch (Exception ex) { 
            }
           
        }

        public void toolbar_Clicked(object sender, EventArgs e)
        {
            //send the current animal to Detail Page
            Xamarin.Forms.MessagingCenter.Send<MainPage, Animal>(this, "display", animal);

            //redirect to Detail page
            Navigation.PushAsync(detail);
        }
    }
}
