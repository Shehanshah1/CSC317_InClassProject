﻿using System.Collections.Specialized;
using System.Reflection.Metadata;
using System.Linq;

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

        // ✿ Completed by Aelin Med ᵔᴗᵔ ✿
        private async void ButtonCancelReservation(object sender, EventArgs e)
        {
            // This function was done using LINQ for practice instead of loops, also improving readability and conciseness

            // The await method waits for user imput
            var seat = await DisplayPromptAsync("Cancel Reservation", "Enter the seat number to cancel reservation: ");
            
            // If the user cancels nothing will be done and seat is null and returned
            if (seat == null)
                return;

            // Using LINQ to find the seat in the seating chart
            // Cast flattens the array into a single list, and FirstOrDefault finds the first seat match
            var seatToCancel = seatingChart.Cast<SeatingUnit>().FirstOrDefault(s => s.Name == seat);

            // If seatToCancel returns null, no seat was found and error message is displayed
            if (seatToCancel == null)
            {
                await DisplayAlert("Error", "Seat not found.", "Ok");
            }
            // If seat is found but not reserved, error message is displayed
            else if (!seatToCancel.Reserved)
            {
                await DisplayAlert("Error", "Seat not currently reserved.", "Ok");
            }
            // If seat is found and reserved, reserved property is set to false, cancelling the reservation
            else 
            {
                seatToCancel.Reserved = false;

                await DisplayAlert("Reservation Canceled", $"Reservation for seat {seat} was successfully canceled.", "Ok");

                // Call the refresh seating function to update the UI
                RefreshSeating();
            }
        }

        //Assign to Team 3 Member
        private async void ButtonCancelReservationRange(object sender, EventArgs e)
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

                            // Cancel reservations from start to end seat
                            while (j < seatingChart.GetLength(1) && seatingChart[i, j].Name != endSeat)
                            {
                                if (seatingChart[i, j].Reserved)
                                {
                                    seatingChart[i, j].Reserved = false; // Cancel reservation
                                }
                                j++;
                            }

                            // Check if the end seat is found and cancel its reservation
                            if (j < seatingChart.GetLength(1) && seatingChart[i, j].Name == endSeat)
                            {
                                if (seatingChart[i, j].Reserved)
                                {
                                    seatingChart[i, j].Reserved = false; // Cancel reservation
                                }
                                foundEnd = true;
                            }
                            break; // Break out of the inner loop
                        }
                    }

                    if (foundStart && foundEnd)
                    {
                        await DisplayAlert("Success", "Seats canceled successfully!", "Ok");
                        RefreshSeating();
                        return;
                    }
                }

                await DisplayAlert("Error", "Invalid range of seats.", "Ok");
            }
        }

// completed by reham afzal
      private async void ButtonResetSeatingChart(object sender, EventArgs e)
{
    bool confirm = await DisplayAlert("Confirm", "Are you sure you want to reset all seats?", "Yes", "No");

    if (confirm)
    {
        for (int i = 0; i < seatingChart.GetLength(0); i++)
        {
            for (int j = 0; j < seatingChart.GetLength(1); j++)
            {
                seatingChart[i, j].Reserved = false;
            }
        }

        await DisplayAlert("Success", "All seats have been reset.", "Ok");
        RefreshSeating();
    }
}
    }

}
