using System.Collections.Specialized;
using System.Reflection.Metadata;

namespace MauiApp2
{
    public class SeatingUnit
    {
        public string Name { get; set; }
        public bool Reserved { get; set; }

        public SeatingUnit(string name, bool reserved = false)
        {
            Name = name;
            Reserved = reserved;
        }

    }

    public partial class MainPage : ContentPage
    {
        SeatingUnit[,] seatingChart = new SeatingUnit[5, 10];

        public MainPage()
        {
            InitializeComponent();
            GenerateSeatingNames();
            RefreshSeating();
        }

        private async void ButtonReserveSeat(object sender, EventArgs e)
        {
            var seat = await DisplayPromptAsync("Enter Seat Number", "Enter seat number: ");

            if (seat != null)
            {
                for (int i = 0; i < seatingChart.GetLength(0); i++)
                {
                    for (int j = 0; j < seatingChart.GetLength(1); j++)
                    {
                        if (seatingChart[i, j].Name == seat)
                        {
                            seatingChart[i, j].Reserved = true;
                            await DisplayAlert("Successfully Reserverd", "Your seat was reserverd successfully!", "Ok");
                            RefreshSeating();
                            return;
                        }
                    }
                }

                await DisplayAlert("Error", "Seat was not found.", "Ok");
            }
        }

        private void GenerateSeatingNames()
        {
            List<string> letters = new List<string>();
            for (char c = 'A'; c <= 'Z'; c++)
            {
                letters.Add(c.ToString());
            }

            int letterIndex = 0;

            for (int row = 0; row < seatingChart.GetLength(0); row++)
            {
                for (int column = 0; column < seatingChart.GetLength(1); column++)
                {
                    seatingChart[row, column] = new SeatingUnit(letters[letterIndex] + (column + 1).ToString());
                }

                letterIndex++;
            }
        }

        private void RefreshSeating()
        {
            grdSeatingView.RowDefinitions.Clear();
            grdSeatingView.ColumnDefinitions.Clear();
            grdSeatingView.Children.Clear();

            for (int row = 0; row < seatingChart.GetLength(0); row++)
            {
                var grdRow = new RowDefinition();
                grdRow.Height = 50;

                grdSeatingView.RowDefinitions.Add(grdRow);

                for (int column = 0; column < seatingChart.GetLength(1); column++)
                {
                    var grdColumn = new ColumnDefinition();
                    grdColumn.Width = 50;

                    grdSeatingView.ColumnDefinitions.Add(grdColumn);

                    var text = seatingChart[row, column].Name;

                    var seatLabel = new Label();
                    seatLabel.Text = text;
                    seatLabel.HorizontalOptions = LayoutOptions.Center;
                    seatLabel.VerticalOptions = LayoutOptions.Center;
                    seatLabel.BackgroundColor = Color.Parse("#333388");
                    seatLabel.Padding = 10;

                    if (seatingChart[row, column].Reserved == true)
                    {
                        //Change the color of this seat to represent its reserved.
                        seatLabel.BackgroundColor = Color.Parse("#883333");

                    }

                    Grid.SetRow(seatLabel, row);
                    Grid.SetColumn(seatLabel, column);
                    grdSeatingView.Children.Add(seatLabel);

                }
            }
        }
        //****************************************************************************************************************
        //****************************************************************************************************************
        //                          Completed by Muhammad Asjad Rehman Hashmi
        //****************************************************************************************************************
        //****************************************************************************************************************
        private async void ButtonReserveRange(object sender, EventArgs e)
        {
            var startSeat = await DisplayPromptAsync("Enter Start Seat Number", "Enter the starting seat number:");
            var endSeat = await DisplayPromptAsync("Enter End Seat Number", "Enter the ending seat number:");

            if (startSeat != null && endSeat != null)
            {
                bool foundStart = false;
                bool foundEnd = false;

                for (int i = 0; i < seatingChart.GetLength(0); i++)
                {
                    for (int j = 0; j < seatingChart.GetLength(1); j++)
                    {
                        if (seatingChart[i, j].Name == startSeat)
                        {
                            foundStart = true;

                            while (seatingChart[i, j].Name != endSeat && j < seatingChart.GetLength(1))
                            {
                                seatingChart[i, j].Reserved = true;
                                j++;
                            }

                            if (seatingChart[i, j].Name == endSeat)
                            {
                                seatingChart[i, j].Reserved = true;
                                foundEnd = true;
                                break;
                            }
                        }
                    }

                    if (foundStart && foundEnd)
                    {
                        await DisplayAlert("Success", "Seats reserved successfully!", "Ok");
                        RefreshSeating();
                        return;
                    }
                }

                await DisplayAlert("Error", "Invalid range of seats.", "Ok");
            }
        }

        //****************************************************************************************************************
        //****************************************************************************************************************

        //Assign to Team 2 Member
        private void ButtonCancelReservation(object sender, EventArgs e)
        {

        }

        //Assign to Team 3 Member
        private void ButtonCancelReservationRange(object sender, EventArgs e)
        {

        }

        //Assign to Team 4 Member
        private void ButtonResetSeatingChart(object sender, EventArgs e)
        {

        }
    }

}
