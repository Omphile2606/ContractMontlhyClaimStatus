using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using ContractMonthlyClaimSystem.Models;
using ContractMontlhyClaimStatus.Repositories;
using Microsoft.Win32;

namespace ContractMontlhyClaimStatus
{
    public partial class MainWindow : Window
    {
        private string chosenFileFullPath;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnUpload_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Documents (.pdf;.docx;.xlsx)|.pdf;.docx;.xlsx",
                Title = "Select supporting document"
            };

            if (dlg.ShowDialog() == true)
            {
                chosenFileFullPath = dlg.FileName;
                TxtFileName.Text = System.IO.Path.GetFileName(chosenFileFullPath);
            }
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            TxtStatus.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(TxtLecturerName.Text))
            {
                MessageBox.Show("Enter Lecturer name.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(TxtHoursWorked.Text.Trim(), out int hours) || hours < 0)
            {
                MessageBox.Show("Enter a valid number of hours.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(TxtHourlyRate.Text.Trim(), out decimal rate) || rate < 0)
            {
                MessageBox.Show("Enter a valid hourly rate.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string storedFileName = null;
            string storedFullPath = null;
            if (!string.IsNullOrEmpty(chosenFileFullPath) && File.Exists(chosenFileFullPath))
            {
                try
                {
                    string uploadFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UploadedFiles");
                    Directory.CreateDirectory(uploadFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(chosenFileFullPath);
                    storedFullPath = System.IO.Path.Combine(uploadFolder, uniqueFileName);
                    File.Copy(chosenFileFullPath, storedFullPath, true);
                    storedFileName = System.IO.Path.GetFileName(storedFullPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to store uploaded file: " + ex.Message, "Upload Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            var newClaim = new Claim
            {
                LecturerName = TxtLecturerName.Text.Trim(),
                HoursWorked = hours,
                HourlyRate = rate,
                Notes = TxtNotes.Text.Trim(),
                DocumentFileName = storedFileName,
                DocumentFullPath = storedFullPath
            };

            TxtLecturerName.Text = string.Empty;
            TxtHoursWorked.Text = string.Empty;
            TxtHourlyRate.Text = string.Empty;
            TxtNotes.Text = string.Empty;
            chosenFileFullPath = null;
            TxtFileName.Text = string.Empty;

            TxtStatus.Text = string.Format("Claim submitted (ID {0}) — Total: R{1:F2}", newClaim.Id, newClaim.TotalAmount);
        }

        private void BtnViewClaims_Click(object sender, RoutedEventArgs e)
        {
            var listWindow = new ClaimsListWindow();
            listWindow.Owner = this;
            listWindow.Show();
        }
    }
}