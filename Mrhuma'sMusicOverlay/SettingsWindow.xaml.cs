﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MrhumasMusicOverlay
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        MainWindow mainWindow;
        public SettingsWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            FillColorComboBoxes();
            this.mainWindow = mainWindow;
        }

        //Fills the foreground and background comboboxes with each color from the colors file
        private void FillColorComboBoxes()
        {
            foreach (HexColor hexColor in HexColor.Colors)
            {
                Color color = hexColor.ConvertToColor();
                if (color != new Color())
                {
                    ComboBoxItem foregroundItem = new ComboBoxItem
                    {
                        Content = hexColor.Name,
                        Background = new SolidColorBrush(color),
                        Foreground = new SolidColorBrush(Colors.White),
                    };

                    ComboBoxItem backgroundItem = new ComboBoxItem
                    {
                        Content = hexColor.Name,
                        Background = new SolidColorBrush(color),
                        Foreground = new SolidColorBrush(Colors.White),
                    };

                    if (color.G > 128 || color.R > 128 || color.B > 128)
                    {
                        foregroundItem.Foreground = new SolidColorBrush(Colors.Black);
                        backgroundItem.Foreground = new SolidColorBrush(Colors.Black);
                    }

                    ForegroundColorComboBox.Items.Add(foregroundItem);
                    BackgroundColorComboBox.Items.Add(backgroundItem);
                }
            };
        }

        //Update the selected options with the inputted settings
        public void UpdateSettingsDisplays(Settings settings)
        {
            //Set the color comboboxes
            foreach (ComboBoxItem item in BackgroundColorComboBox.Items)
            {
                if (item.Content.ToString() == settings.BackgroundColor)
                {
                    BackgroundColorComboBox.SelectedItem = item;
                }
            }

            foreach (ComboBoxItem item in ForegroundColorComboBox.Items)
            {
                if (item.Content.ToString() == settings.ForegroundColor)
                {
                    ForegroundColorComboBox.SelectedItem = item;
                }
            }

            //Set the music source combobox
            MusicSourceComboBox.SelectedIndex = (int)settings.MusicSource;

            //Set the IP source textbox
            IPSourceTextBox.Text = settings.IPSource;

            //Ensure no dropdown menus are open
            BackgroundColorComboBox.IsDropDownOpen = false;
            ForegroundColorComboBox.IsDropDownOpen = false;
            MusicSourceComboBox.IsDropDownOpen = false;
        }

        //Update the example text background whenever a new color is chosen
        private void BackgroundColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string color = ((ComboBoxItem)e.AddedItems[0]).Content.ToString(); //Gets the name of the color that was selected
            SolidColorBrush brush = new SolidColorBrush(HexColor.Colors.First(x => x.Name == color).ConvertToColor());
            songNameText.Background = brush;
            artistNameText.Background = brush;
        }

        //Update the example text foreground whenever a new color is chosen
        private void ForegroundColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string color = ((ComboBoxItem)e.AddedItems[0]).Content.ToString(); //Gets the name of the color that was selected
            SolidColorBrush brush = new SolidColorBrush(HexColor.Colors.First(x => x.Name == color).ConvertToColor());
            songNameText.Foreground = brush;
            artistNameText.Foreground = brush;
        }

        //Updates the settings with the currently selected foreground and background colors
        //and applies the new colors to the main window
        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.UpdateSettings(BackgroundColorComboBox.Text, ForegroundColorComboBox.Text, MusicSourceComboBox.SelectedIndex, IPSourceTextBox.Text);
            this.Close();
        }

        private void MusicSourceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Check if the selection was changed to YouTube
            if ((e.AddedItems[0] as ComboBoxItem).Content.ToString() == "YouTube")
            {
                //If the selection is YouTube, show the IPSource option
                IPSourceLabel.Visibility = Visibility.Visible;
                IPSourceTextBox.Visibility = Visibility.Visible;
                resetIPSourceButton.Visibility = Visibility.Visible;
            }
            else
            {
                //If the selection is not YouTube, hide the IPSource option
                IPSourceLabel.Visibility = Visibility.Hidden;
                IPSourceTextBox.Visibility = Visibility.Hidden;
                resetIPSourceButton.Visibility = Visibility.Hidden;
            }
        }

        private void resetIPSourceButton_Click(object sender, RoutedEventArgs e)
        {
            IPSourceTextBox.Text = "localhost";
        }
    }
}
