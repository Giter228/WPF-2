using System;
using System.Collections.Generic;
using System.Windows;

namespace Diary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string way = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        List<Note> notes = De_Serialize_.Deserialize<Note>(way + "\\Notes");
        private DateTime selectedDate = DateTime.Today;
        private bool flag = false;
        public MainWindow()
        {
            InitializeComponent();

            DatePick.Text = selectedDate.ToString();
            Fill_LBX();

            Delete_BTN.IsEnabled = false;
            Save_BTN.IsEnabled = false;
            
        }

        private void Create_BTN_Click(object sender, RoutedEventArgs e)
        {
            if (Input_name.Text == null || Input_Desc.Text == null || Input_name.Text.Trim() == "" || Input_Desc.Text.Trim() == "")
            {
                MessageBox.Show("Не все поля заполнены");
                return;
            }
            foreach (var note in notes)
            {
                if (note.Name == Input_name.Text && note.Description == Input_Desc.Text)
                {
                    MessageBox.Show("Не пытайтесь создать идентичную заметку!");
                    return;
                }
            }

            notes.Add(new Note(Input_name.Text, Input_Desc.Text, selectedDate));
            De_Serialize_.Serialize(notes, way + "\\Notes\\allNotes.json");

            LBX.Items.Clear();

            Fill_LBX();

            Input_name.Text = null;
            Input_Desc.Text = null;
        }

        private void Fill_LBX()
        {
            LBX.Items.Clear();
            foreach (var note in notes)
            {
                if (note.Date == selectedDate)
                {
                    LBX.Items.Add(note.Name);
                }
            }

            flag = true;
        }

        private void LBX_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Delete_BTN.IsEnabled = true;
            Save_BTN.IsEnabled = true;
            foreach (var note in notes)
            {
                if (note.Name == LBX.SelectedValue)
                {
                    Input_name.Text = note.Name;
                    Input_Desc.Text = note.Description;
                }
            }
        }

        private void DatePick_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            selectedDate = Convert.ToDateTime(DatePick.Text);
            
            Input_name.Text = null;
            Input_Desc.Text = null;
            Create_BTN.IsEnabled = true;

            if (flag) Fill_LBX();
        }

        private void Save_BTN_Click(object sender, RoutedEventArgs e)
        {
            foreach (var note in notes)
            {
                if (LBX.Items[LBX.SelectedIndex].ToString() == note.Name)
                {
                    note.Name = Input_name.Text;
                    note.Description = Input_Desc.Text;
                    note.Date = selectedDate;
                }
            }

            Fill_LBX();
            De_Serialize_.Serialize(notes, way + "\\Notes\\allNotes.json");

            Delete_BTN.IsEnabled = false;
            Save_BTN.IsEnabled = false;
            Create_BTN.IsEnabled = true;

            Input_name.Text = null;
            Input_Desc.Text = null;
        }

        private void Delete_BTN_Click(object sender, RoutedEventArgs e)
        {
            foreach (var note in notes)
            {
                if (LBX.Items[LBX.SelectedIndex].ToString() == note.Name)
                {
                    notes.Remove(note);
                    break;
                }
            }
            Fill_LBX();

            Input_name.Text = null;
            Input_Desc.Text = null;

            De_Serialize_.Serialize(notes, way + "\\Notes\\allNotes.json");
        }
    }
}
