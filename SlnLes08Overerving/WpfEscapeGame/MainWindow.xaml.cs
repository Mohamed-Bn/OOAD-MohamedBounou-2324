using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace WpfEscapeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Room currentRoom; // will become useful in later versions

        public enum MessageType
        {
            NotWorking,
            NotPickupable,
            Dropped
        }

        public MainWindow()
        {
            InitializeComponent();

            // define room
            Room room1 = new Room("bedroom", "I seem to be in a medium sized bedroom. There is a locker to the left, a green door in front of me, and a bed to the right. ", "/img/ss-bedroom.png");
            Room room2 = new Room("living room", "I seem to be in a living room. There is a door to the left, another door with a keylock in front of me, and a cabinet to the right. ", "/img/ss-living.png");
            Room room3 = new Room("computer room", "I seem to be in a computer room. There is a computer to the left and a door to the right. ", "/img/ss-computer.png");

            // define items
            Key key1 = new Key("small silver key", "A small silver key, makes me think of one I had at highschool. ");
            Key key2 = new Key("large key", "A large key. Could this be my way out? ");
            LockableItem locker = new LockableItem("locker", "A locker. I wonder what's inside. ", false);
            locker.HiddenItem = key2;
            locker.Key = key1;
            Item bed = new Item("bed", "Just a bed. I am not tired right now. ", false);
            bed.HiddenItem = key1;
            Item chair = new Item("chair", "Just a chair. I don't wanna sit right now. ", false);
            Item poster = new Item("poster", "Just your average poster. Nothing wrong with it. ");

            Item cabinet = new Item("cabinet", "Just an average looking cabinet. Maybe there is something in it. ", false);
            Item plant = new Item("plant", "Just a plant. It looks well grown. ", false);
            Item clock = new Item("clock", "Just a clock. I don't care about the time right now. ");

            Item computer = new Item("computer", "Just a computer. I hope it has minecraft installed on it. ", false);
            Item pot = new Item("pot", "Just a pot. It's an empty pot. ", false);
            Item airConditioner = new Item("air conditioner", "Just an air conditioner. It's nice and fresh next to it. ", false);

            // define doors
            Door door1 = new Door("green door", "It's a door too the living room", key2, room2);
            Door door2 = new Door("normal door", "It's a door too the computer room", false, room3);
            Door door3 = new Door("normal door", "It's a door too the living room", false, room2);
            Door door4 = new Door("door with keypad", "It's a door too an unknown room", null);

            // setup bedroom
            room1.Items.Add(new Item("floor mat", "A bit ragged floor mat, but still one of the most popular designs. "));
            room1.Items.Add(bed);
            room1.Items.Add(locker);
            room1.Items.Add(chair);
            room1.Items.Add(poster);
            room1.Doors.Add(door1);

            room2.Items.Add(new Item("floor mat", "A bit ragged floor mat, but still one of the most popular designs. "));
            room2.Items.Add(cabinet);
            room2.Items.Add(plant);
            room2.Items.Add(clock);
            room2.Doors.Add(door2);
            room2.Doors.Add(door4);

            room3.Items.Add(computer);
            room3.Items.Add(pot);
            room3.Items.Add(chair);
            room3.Items.Add(airConditioner);
            room3.Doors.Add(door3);

            // start game
            currentRoom = room1;
            lblMessage.Content = "I am awake, but cannot remember who I am!? Must have been a hell of a party last night... ";
            txtRoomDesc.Text = currentRoom.Description;
            UpdateUI();
        }

        /// <summary>
        /// Update de items in de ListBoxes
        /// </summary>
        private void UpdateUI()
        {
            lstRoomItems.Items.Clear();
            foreach (Item itm in currentRoom.Items)
            {
                lstRoomItems.Items.Add(itm);
            }
            lstRoomDoors.Items.Clear();
            foreach (Door door in currentRoom.Doors)
            {
                lstRoomDoors.Items.Add(door);
            }

            // bron: https://stackoverflow.com/questions/3787137/change-image-source-in-code-behind-wpf
            imgRoom.Source = new BitmapImage(new Uri(currentRoom.Image, UriKind.Relative));
        }

        private void LstItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnCheck.IsEnabled = lstRoomItems.SelectedValue != null; // room item selected
            btnPickUp.IsEnabled = lstRoomItems.SelectedValue != null; // room item selected
            btnUseOn.IsEnabled = lstRoomItems.SelectedValue != null && lstMyItems.SelectedValue != null; // room item and picked up item selected
            btnDrop.IsEnabled = lstMyItems.SelectedValue != null; // my item selected
            btnOpenWith.IsEnabled = lstRoomDoors.SelectedValue != null && lstMyItems.SelectedValue != null;
            btnEnter.IsEnabled = lstRoomDoors.SelectedValue != null;
        }

        private void BtnCheck_Click(object sender, RoutedEventArgs e)
        {
            // 1. find item to check
            Item roomItem = (Item)lstRoomItems.SelectedItem;
            if (roomItem is LockableItem)
            {
                LockableItem lRoomItem = (LockableItem)lstRoomItems.SelectedItem;

                // 2. is it locked?
                if (lRoomItem.IsLocked)
                {
                    lblMessage.Content = $"{roomItem.Description}It is firmly locked. ";
                    return;
                }
            }

            // 3. does it contain a hidden item?
            Item foundItem = roomItem.HiddenItem;
            if (foundItem != null)
            {
                lblMessage.Content = $"Oh, look, I found a {foundItem.Name} ";
                lstMyItems.Items.Add(foundItem);
                roomItem.HiddenItem = null;
                return;
            }

            // 4. just another item; show description
            lblMessage.Content = roomItem.Description;
        }

        private void BtnUseOn_Click(object sender, RoutedEventArgs e)
        {
            // 1. find both items
            Item myItem = (Item)lstMyItems.SelectedItem;
            Item roomItem = (Item)lstRoomItems.SelectedItem;
            if (roomItem is LockableItem)
            {
                LockableItem lRoomItem = (LockableItem)lstRoomItems.SelectedItem;

                // 2. item doesn't fit
                if (lRoomItem.Key != myItem)
                {
                    lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.NotWorking);
                    return;
                }

                // 3. item fits; other item unlocked
                lRoomItem.IsLocked = false;
                lRoomItem.Key = null;
                lstMyItems.Items.Remove(myItem);
                lblMessage.Content = $"I just unlocked the {roomItem.Name}!";
            }
            else
            {
                lblMessage.Content = "WTF am I trying to do";
            }
        }

        private void BtnPickUp_Click(object sender, RoutedEventArgs e)
        {
            // 1. find room selected item
            Item selItem = (Item)lstRoomItems.SelectedItem;

            // 2. add item to your items list
            if (selItem.IsPortable)
            {
                lblMessage.Content = $"I just picked up the {selItem.Name}. ";
                lstMyItems.Items.Add(selItem);
                lstRoomItems.Items.Remove(selItem);
                currentRoom.Items.Remove(selItem);
                return;
            }
            lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.NotPickupable);
        }

        private void BtnDrop_Click(object sender, RoutedEventArgs e)
        {
            // 1. find your selected item
            Item myItem = (Item)lstMyItems.SelectedItem;

            // 2. add item to room items list
            lblMessage.Content = $"{RandomMessageGenerator.GetRandomMessage(MessageType.Dropped)}{myItem.Name}. ";
            lstRoomItems.Items.Add(myItem);
            lstMyItems.Items.Remove(myItem);
            currentRoom.Items.Add(myItem);
        }

        private void BtnOpenWith_Click(object sender, RoutedEventArgs e)
        {
            // 1. find both items
            Item myItem = (Item)lstMyItems.SelectedItem;
            Door roomDoor = (Door)lstRoomDoors.SelectedItem;

            // 2. item doesn't fit
            if (roomDoor.Key != myItem)
            {
                lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.NotWorking);
                return;
            }

            // 3. item fits; other item unlocked
            roomDoor.IsLocked = false;
            roomDoor.Key = null;
            lstMyItems.Items.Remove(myItem);
            lblMessage.Content = $"I just unlocked the door too the {roomDoor.ToRoom}!";
        }

        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {
            // 1. find item to check
            Door roomDoor = (Door)lstRoomDoors.SelectedItem;

            // 2. is it locked?
            if (roomDoor.IsLocked)
            {
                lblMessage.Content = $"The door is firmly locked. ";
                return;
            }

            // 3. enter the next room
            currentRoom = roomDoor.ToRoom;
            txtRoomDesc.Text = currentRoom.Description;
            UpdateUI();
        }
    }
}